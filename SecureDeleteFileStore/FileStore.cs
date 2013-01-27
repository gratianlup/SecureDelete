// Copyright (c) 2007 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote 
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor 
// may "SecureDelete" appear in their names without prior written 
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using DebugUtils.Debugger;

namespace FileStore {
    [Serializable]
    public class StoreData {
        public bool Encrypted;
        public bool UseDPAPI;

        public byte[] RootFolder;
        public Dictionary<Guid, StoreFile> Files;
    }

    [System.Diagnostics.DebuggerVisualizer(typeof(StoreVisualizer))]
    [Serializable]
    public class FileStore {
        #region Constants

        public readonly char[] Separators = new char[] { '\\' };

        #endregion

        #region Fields

        public Dictionary<Guid, StoreFile> files;
        private byte[] EncryptionIV;

        #endregion

        #region Constructor

        public FileStore() {
            _root = new StoreFolder("root");
            files = new Dictionary<Guid, StoreFile>();
        }

        #endregion

        #region Properties

        private StoreFolder _root;
        public StoreFolder Root {
            get { return _root; }
            set { _root = value; }
        }

        private bool _encrypt;
        public bool Encrypt {
            get { return _encrypt; }
            set { _encrypt = value; }
        }

        private byte[] _encryptionKey;
        public byte[] EncryptionKey {
            get { return _encryptionKey; }
            set { _encryptionKey = value; }
        }

        private bool _useDPAPI;
        public bool UseDPAPI {
            get { return _useDPAPI; }
            set { _useDPAPI = value; }
        }

        #endregion

        #region Private methods

        private StoreFolder GetFolder(string path, out StoreFolder parent) {
            string[] components = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if(components.Length == 0) {
                parent = null;
                return _root;
            }

            parent = _root;
            StoreFolder folder = _root;

            // try to locate the folder
            int length = components.Length;
            for(int i = 0; i < length; i++) {
                parent = folder;

                if(folder.Subfolders.TryGetValue(components[i], out folder) == false) {
                    // not found
                    return null;
                }
            }

            return folder;
        }


        private StoreFolder GetFolder(string path) {
            StoreFolder parent;
            return GetFolder(path, out parent);
        }


        private StoreFolder CreateFolderImpl(string path) {
            string[] components = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if(components.Length == 0) {
                return null;
            }

            StoreFolder parent = _root;
            StoreFolder folder = null;

            // locate the parent
            int position = 0;
            int length = components.Length;

            while(position < (length - 1)) {
                if(parent.Subfolders.TryGetValue(components[position], out folder) == false) {
                    break;
                }

                parent = folder;
                position++;
            }

            // create the required folders
            while(position < length) {
                string name = components[position];
                folder = new StoreFolder(name);

                parent.Subfolders.Add(name, folder);
                parent = folder;

                position++;
            }

            return folder;
        }


        private Guid? GetFileId(string path) {
            string[] components = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            if(components.Length == 0) {
                return null;
            }

            string fileName = components[components.Length - 1];
            StoreFolder folder = GetFolder(path.Substring(0, path.Length - fileName.Length));

            if(folder == null) {
                return null;
            }

            // check if the folder contains the file
            if(folder.Files.ContainsKey(fileName)) {
                return folder.Files[fileName];
            }

            return null;
        }


        private byte[] CompressData(byte[] data) {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
            ms.Position = 0;

            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            return compressed;
        }


        private byte[] DecompressData(byte[] data) {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);

            byte[] buffer = new byte[data.Length];

            ms.Position = 0;
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
            zip.Read(buffer, 0, buffer.Length);

            return buffer;
        }


        private void InitEncryptionAlgorithm() {
            if(EncryptionIV == null || EncryptionIV.Length == 0) {
                // create the IV
                EncryptionIV = new byte[16]; // 128 bytes
                Random rand = new Random(31);

                rand.NextBytes(EncryptionIV);
            }
        }


        private byte[] EncryptData(byte[] data) {
            if(_useDPAPI) {
                try {
                    return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to encrypt data using DPAPI. Exception: {0}", e.Message);
                    return new byte[0];
                }
            }
            else {
                InitEncryptionAlgorithm();

                RijndaelManaged aes = null;
                ICryptoTransform encryptor = null;
                CryptoStream encStream = null;
                MemoryStream inputStream = null;

                try {
                    inputStream = new MemoryStream();
                    aes = new RijndaelManaged();
                    aes.IV = EncryptionIV;
                    aes.Key = _encryptionKey;
                    encryptor = aes.CreateEncryptor(_encryptionKey, EncryptionIV);

                    // encrypt
                    encStream = new CryptoStream(inputStream, encryptor, CryptoStreamMode.Write);

                    encStream.Write(data, 0, data.Length);
                    encStream.FlushFinalBlock();
                    encStream.Close();

                    return inputStream.ToArray();
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to encrypt data using AES. Exception: {0}", e.Message);
                    return new byte[0];
                }
                finally {
                    if(aes != null) {
                        aes.Clear();
                    }
                    if(encStream != null) {
                        encStream.Close();
                    }
                    if(inputStream != null) {
                        inputStream.Close();
                    }
                }
            }
        }


        private byte[] DecryptData(byte[] data) {
            if(_useDPAPI) {
                try {
                    return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to decrypt data using DPAPI. Exception: {0}", e.Message);
                    return new byte[0];
                }
            }
            else {
                RijndaelManaged aes = null;
                MemoryStream inputStream = null;
                CryptoStream decStream = null;

                try {
                    InitEncryptionAlgorithm();

                    inputStream = new MemoryStream(data);
                    aes = new RijndaelManaged();
                    aes.IV = EncryptionIV;
                    aes.Key = _encryptionKey;
                    ICryptoTransform decryptor = aes.CreateDecryptor(_encryptionKey, EncryptionIV);

                    // decrypt
                    decStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);

                    byte[] buffer = new byte[data.Length];
                    int length = decStream.Read(buffer, 0, data.Length);

                    if(length == 0) {
                        return new byte[0];
                    }
                    else {
                        byte[] decryptedData = new byte[data.Length];
                        buffer.CopyTo(decryptedData, 0);

                        return buffer;
                    }
                }
                catch(Exception e) {
                    Debug.ReportError("Failed to decrypt data using AES. Exception: {0}", e.Message);
                    return null;
                }
                finally {
                    if(aes != null) {
                        aes.Clear();
                    }
                    if(decStream != null) {
                        decStream.Close();
                    }
                    if(inputStream != null) {
                        inputStream.Close();
                    }
                }
            }
        }


        private void SetFileData(StoreFile file, byte[] data, StoreMode storeMode) {
            if(data == null) {
                file.ResetData();
            }

            byte[] buffer = data;

            // compress
            if((storeMode & StoreMode.Compressed) == StoreMode.Compressed) {
                buffer = CompressData(buffer);
            }

            // encrypt
            if((storeMode & StoreMode.Encrypted) == StoreMode.Encrypted) {
                buffer = EncryptData(buffer);
            }

            file.StoreMode = storeMode;
            file.SetData(buffer, data.Length);
        }


        private MemoryStream SerializeFolders() {
            MemoryStream stream = new MemoryStream();

            try {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, _root);

                return stream;
            }
            catch(Exception e) {
                return null;
            }
        }


        private bool DeserializeFolders(byte[] data) {
            MemoryStream stream = null;

            if(_encrypt) {
                // decrypt the folder data
                data = DecryptData(data);
            }

            try {
                stream = new MemoryStream(data);
                BinaryFormatter formatter = new BinaryFormatter();

                _root = (StoreFolder)formatter.Deserialize(stream);
                return true;
            }
            catch(Exception e) {
                return false;
            }
            finally {
                if(stream != null) {
                    stream.Close();
                }
            }
        }

        #endregion

        #region Public methods

        #region Folder management

        /// <summary>
        /// Creates a folder.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CreateFolder(string path) {
            if(path == null) {
                throw new ArgumentNullException(path);
            }

            if(GetFolder(path) == null) {
                CreateFolderImpl(path);
            }
        }


        /// <summary>
        /// Checks if the given folder exists.
        /// </summary>
        /// <param name="path">The folder path.</param>
        public bool FolderExists(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            return GetFolder(path) != null;
        }


        /// <summary>
        /// Deletes a folder.
        /// </summary>
        /// <param name="path">The folder path.</param>
        public void DeleteFolder(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            StoreFolder parent;
            StoreFolder folder = GetFolder(path, out parent);

            // don't delete root folder
            if(parent == folder) {
                return;
            }

            // delete all the subfolders
            while(folder.Subfolders.Keys.Count > 0) {
                DeleteFolder(path + "\\" + folder.Subfolders.Keys[0]);
            }

            // delete all files
            while(folder.Files.Count > 0) {
                DeleteFile(path + "\\" + folder.Files.Keys[0]);
            }

            // delete the folder from the parent
            parent.Subfolders.Remove(folder.Name);
        }


        /// <summary>
        /// Moves a folder.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        public void MoveFolder(string sourcePath, string destinationPath) {
            if(sourcePath == null || destinationPath == null) {
                throw new ArgumentNullException("sourcePath | destionationPath");
            }

            string[] sourceComponents = sourcePath.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            string[] destinationComponents = destinationPath.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if(sourceComponents.Length == 0 || destinationComponents.Length == 0) {
                return;
            }


            StoreFolder sourceFolder, sourceFolderParent;
            StoreFolder destinationFolder, destinationFolderParent;

            sourceFolder = GetFolder(sourcePath, out sourceFolderParent);
            destinationFolder = GetFolder(destinationPath, out destinationFolderParent);

            if(sourceFolder == null) {
                // source folder not found
                return;
            }

            if(destinationFolder == null) {
                // create the destination folder
                CreateFolderImpl(destinationPath);
                destinationFolder = GetFolder(destinationPath, out destinationFolderParent);
            }

            // copy all data
            destinationFolder.Files = sourceFolder.Files;
            destinationFolder.Subfolders = sourceFolder.Subfolders;

            // remove source folder
            sourceFolderParent.Subfolders.Remove(sourceFolder.Name);
        }


        /// <summary>
        /// Gets a string array of the subfolders in the given folder.
        /// </summary>
        /// <param name="path">The folder path.</param>
        public string[] GetSubfolders(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            StoreFolder folder = GetFolder(path);

            if(folder != null) {
                string[] folders = new string[folder.Subfolders.Count];

                for(int i = 0; i < folder.Subfolders.Count; i++) {
                    folders[i] = folder.Subfolders.Keys[i];
                }

                return folders;
            }

            return null;
        }


        /// <summary>
        /// Gets a string array of the files in the given folder.
        /// </summary>
        /// <param name="path">The folder path.</param>
        public string[] GetFolderFiles(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            StoreFolder folder = GetFolder(path);

            if(folder != null) {
                string[] files = new string[folder.Files.Count];

                for(int i = 0; i < folder.Files.Count; i++) {
                    files[i] = folder.Files.Keys[i];
                }

                return files;
            }

            return null;
        }


        /// <summary>
        /// Gets a StoreFile array of the files in the given folder.
        /// </summary>
        /// <param name="path">The folder path.</param>
        public StoreFile[] GetFolderFilesEx(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            StoreFolder folder = GetFolder(path);

            if(folder != null) {
                StoreFile[] files = new StoreFile[folder.Files.Count];

                for(int i = 0; i < folder.Files.Count; i++) {
                    files[i] = this.files[folder.Files.Values[i]];
                }

                return files;
            }

            return null;
        }


        /// <summary>
        /// Removes all files and folders from the store.
        /// </summary>
        private void Format() {
            _root = new StoreFolder("root");
            files.Clear();
        }

        #endregion

        #region File management

        /// <summary>
        /// Creates a file.
        /// </summary>
        /// <param name="path">The file path.</param>
        public StoreFile CreateFile(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            string[] components = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            if(components.Length == 0) {
                return null;
            }

            string fileName = components[components.Length - 1];
            StoreFolder folder = GetFolder(path.Substring(0, path.Length - fileName.Length));

            // check if the folder contains the file
            if(folder.Files.ContainsKey(fileName)) {
                return files[folder.Files[fileName]];
            }

            // add the file
            StoreFile file = new StoreFile(fileName);
            Guid fileId = Guid.NewGuid();
            folder.Files.Add(fileName, fileId);
            files.Add(fileId, file);

            return file;
        }


        /// <summary>
        /// Checks if the given file exists.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns></returns>
        public bool FileExists(string path) {
            if(path == null) {
                throw new ArgumentNullException(path);
            }

            return GetFileId(path).HasValue;
        }


        /// <summary>
        /// Gets the file extended information.
        /// </summary>
        /// <param name="path">The file path.</param>
        public StoreFile GetFile(string path) {
            if(path == null) {
                throw new ArgumentNullException(path);
            }

            Guid? id = GetFileId(path);

            if(id.HasValue && files.ContainsKey(id.Value)) {
                return files[id.Value];
            }

            // not found
            return null;
        }


        /// <summary>
        /// Stores the given data into the file.
        /// </summary>
        /// <param name="file">The file in which to store the data.</param>
        /// <param name="data">The data to store.</param>
        public void WriteFile(StoreFile file, byte[] data, StoreMode storeMode) {
            if(file == null) {
                throw new ArgumentNullException("file");
            }

            SetFileData(file, data, storeMode);
        }


        /// <summary>
        /// Stores the given data into the file.
        /// </summary>
        /// <param name="file">The file in which to store the data.</param>
        /// <param name="data">The Stream to store.</param>
        public void WriteFile(StoreFile file, Stream stream, StoreMode storeMode) {
            if(file == null || stream == null) {
                throw new ArgumentNullException("file | stream");
            }

            if(stream.Length > 0) {
                // allocate buffer
                byte[] buffer = new byte[(int)stream.Length];

                // read from stream
                if(stream.Read(buffer, 0, (int)stream.Length) != 0) {
                    SetFileData(file, buffer, storeMode);
                }
            }
            else {
                file.ResetData();
            }
        }


        /// <summary>
        /// Stores the data contained in the given file into the file.
        /// </summary>
        /// <param name="file">The file in which to store the data.</param>
        /// <param name="data">The path of the data file.</param>
        public bool WriteFile(StoreFile file, string dataPath, StoreMode storeMode) {
            if(file == null || dataPath == null) {
                throw new ArgumentNullException("file");
            }

            if(File.Exists(dataPath)) {
                try {
                    byte[] buffer = File.ReadAllBytes(dataPath);
                    SetFileData(file, buffer, storeMode);
                }
                catch {
                    file.ResetData();
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Get the contents of the given file.
        /// </summary>
        /// <param name="file">The file from which to get the data.</param>
        public byte[] ReadFile(StoreFile file) {
            if(file == null) {
                throw new ArgumentNullException("file");
            }

            // check if there is anything to read
            if(file.Data == null) {
                return null;
            }

            byte[] data = file.Data;

            if((file.StoreMode & StoreMode.Encrypted) == StoreMode.Encrypted) {
                data = DecryptData(data);
            }

            if((file.StoreMode & StoreMode.Compressed) == StoreMode.Compressed) {
                data = DecompressData(data);
            }

            if(data.Length > file.RealSize) {
                byte[] buffer = new byte[file.RealSize];
                Buffer.BlockCopy(data, 0, buffer, 0, (int)file.RealSize);

                return buffer;
            }

            return data;
        }


        /// <summary>
        /// Get the contents of the given file.
        /// </summary>
        /// <param name="file">The path of the file from which to get the data.</param>
        public byte[] ReadFile(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            // try to get the file
            StoreFile file = GetFile(path);

            if(file != null) {
                return ReadFile(file);
            }

            return null;
        }


        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">The file path.</param>
        public void DeleteFile(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            string[] components = path.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            if(components.Length == 0) {
                return;
            }

            string fileName = components[components.Length - 1];
            StoreFolder folder = GetFolder(path.Substring(0, path.Length - fileName.Length));

            // check if the folder contains the file
            if(folder.Files.ContainsKey(fileName)) {
                Guid fileId = folder.Files[fileName];

                // remove
                folder.Files.Remove(fileName);
                files.Remove(fileId);
            }
        }


        /// <summary>
        /// Moves a file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        public void MoveFile(string sourcePath, string destinationPath) {
            if(sourcePath == null || destinationPath == null) {
                throw new ArgumentNullException("sourcePath | destionationPath");
            }

            string[] sourceComponents = sourcePath.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
            string[] destinationComponents = destinationPath.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if(sourceComponents.Length == 0 || destinationComponents.Length == 0) {
                return;
            }

            // extract the file names
            string sourceFileName = sourceComponents[sourceComponents.Length - 1];
            string destinationFileName = destinationComponents[destinationComponents.Length - 1];

            StoreFolder sourceFolder = GetFolder(sourcePath.Substring(0, sourcePath.Length - sourceFileName.Length));
            StoreFolder destinationFolder = CreateFolderImpl(destinationPath.Substring(0, destinationPath.Length - destinationFileName.Length));

            // copy to destination
            if(destinationFolder.Files.ContainsKey(destinationFileName)) {
                destinationFolder.Files[destinationFileName] = sourceFolder.Files[sourceFileName];
            }
            else {
                destinationFolder.Files.Add(destinationFileName, sourceFolder.Files[sourceFileName]);
            }

            // remove from source
            sourceFolder.Files.Remove(sourceFileName);
        }


        /// <summary>
        /// Tries to locate the given file.
        /// </summary>
        /// <param name="path">The folder from where to start searching.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="subfolders">Search in subfolders.</param>
        public StoreFile FindFile(string path, string fileName, bool subfolders) {
            StoreFolder folder = GetFolder(path);

            if(folder != null) {
                // check the files
                int length = folder.Files.Keys.Count;
                IList<string> keys = folder.Files.Keys;

                for(int i = 0; i < length; i++) {
                    if(keys[i] == fileName) {
                        return files[folder.Files[fileName]];
                    }
                }

                // check in subfolders
                if(subfolders) {
                    length = folder.Subfolders.Keys.Count;
                    keys = folder.Subfolders.Keys;

                    for(int i = 0; i < length; i++) {
                        StoreFile file = FindFile(path + "\\" + folder.Name, fileName, subfolders);

                        if(file != null) {
                            // file found, stop searching
                            return file;
                        }
                    }
                }
            }

            // nothing found
            return null;
        }

        #endregion

        #region Save / load

        /// <summary>
        /// Saves the store to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool Save(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            FileStream writer = null;

            try {
                writer = new FileStream(path, FileMode.OpenOrCreate);

                if(writer.CanWrite) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    StoreData data = new StoreData();

                    data.Files = files;
                    data.Encrypted = _encrypt;
                    data.UseDPAPI = _useDPAPI;
                    MemoryStream folderStream = SerializeFolders();

                    if(_encrypt) {
                        data.RootFolder = EncryptData(folderStream.ToArray());
                    }
                    else {
                        data.RootFolder = folderStream.ToArray();
                    }


                    formatter.Serialize(writer, data);
                    return true;
                }
            }
            catch(Exception e) {
                return false;
            }
            finally {
                if(writer != null) {
                    writer.Close();
                }
            }

            return true;
        }


        /// <summary>
        /// Loads the store from the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public bool Load(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            // check if the file exists
            if(File.Exists(path) == false) {
                return false;
            }

            FileStream reader = null;

            try {
                reader = new FileStream(path, FileMode.Open);

                if(reader.CanRead) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    StoreData data = (StoreData)formatter.Deserialize(reader);

                    files = data.Files;
                    _encrypt = data.Encrypted;
                    _useDPAPI = data.UseDPAPI;
                    DeserializeFolders(data.RootFolder);

                    return true;
                }
            }
            catch(Exception e) {
                Debug.ReportError("Failed to deserialize store. Path: {0}, Exception: {1}", path, e.Message);
                return false;
            }
            finally {
                if(reader != null) {
                    reader.Close();
                }
            }

            return true;
        }

        #endregion

        #endregion
    }
}
