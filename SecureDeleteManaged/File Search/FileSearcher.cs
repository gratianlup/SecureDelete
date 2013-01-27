// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2007 Lup Gratian - All Rights Reserved
//   
// ***************************************************************         

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DebugUtils.Debugger;
using System.Threading;
using System.Text.RegularExpressions;

namespace SecureDelete.FileSearch
{
	/// <summary>
	/// Used to notify the user that a series of files was founded.
	/// </summary>
	public class FileSearchArgs : EventArgs
	{
		#region Fields

		public string[] files;
		public bool stop;
		public bool lastSet;

		#endregion

		#region Constructor

		public FileSearchArgs() { }

		public FileSearchArgs(string[] files, bool lastSet)
		{
			this.files = files;
			this.lastSet = lastSet;
		}

		#endregion
	}


	/// <summary>
	/// Used to notify the user that a series of files was founded.
	/// </summary>
	public delegate void FileSearchDelegate(object sender, FileSearchArgs e);


	/// <summary>
	/// Provides support for searching files in directories and their subdirectories.
	/// Search can be performed asynchronously.
	/// </summary>
	public class FileSearcher
	{
		#region Nested types

		private struct ThreadParams
		{
			public string folder;
			public string pattern;
			public bool regexPattern;
			public bool includeSubfolders;
		}

		#endregion

		#region Constants

		private const int DefaultResultChunk = 16;

		#endregion

		#region Fields

		private object searchLock;
		private object stopLock;
		private Queue<string> unsentFiles;
		private bool stop;
		private DateTime lastSendTime;
		private Regex regex;

		#endregion

		#region Constructor

		public FileSearcher()
		{
			searchLock = new object();
			stopLock = new object();
			unsentFiles = new Queue<string>();
			_resultChunkLength = DefaultResultChunk;
		}

		#endregion

		#region Properties

		private FileFilter _fileFilter;
		public FileFilter FileFilter
		{
			get { return _fileFilter; }
			set { _fileFilter = value; }
		}

		private int _resultChunkLength;
		public int ResultChunkLength
		{
			get { return _resultChunkLength; }
			set { _resultChunkLength = value; }
		}

		private TimeSpan _maximumWaitTime;
		public TimeSpan MaximumWaitTime
		{
			get { return _maximumWaitTime; }
			set { _maximumWaitTime = value; }
		}

		public bool Stopped
		{
			get
			{
				lock (stopLock)
				{
					return stop;
				}
			}
		}

		#endregion

		#region Events

		public event FileSearchDelegate OnFilesFound;

		#endregion

		#region Private methods

		private bool GetStop()
		{
			lock (stopLock)
			{
				return stop;
			}
		}


		private void SetStop(bool value)
		{
			lock (stopLock)
			{
				stop = value;
			}
		}


		private string[] ProcessFiles(string[] files)
		{
			// check the parameters
			Debug.AssertNotNull(files, "Files is null");

			if (_fileFilter == null || files.Length == 0)
			{
				return files;
			}

			List<string> allowedFiles = new List<string>();

			int count = files.Length;
			for (int i = 0; i < count; i++)
			{
				if (_fileFilter.AllowFile(files[i])) 
				{
					allowedFiles.Add(files[i]);
				}
			}

			return allowedFiles.ToArray();
		}


		private void SendResultChunk(string[] files, bool lastSet)
		{
			FileSearchArgs e = new FileSearchArgs(files, lastSet);

			if (OnFilesFound != null)
			{
				OnFilesFound(this, e);

				if (e.stop == true)
				{
					SetStop(true);
				}
			}
		}


		private void SendFiles(string[] files, bool forced)
		{
			int position = 0;

			// special case
			if (forced)
			{
				SendResultChunk(files, false);
				lastSendTime = DateTime.Now;
				return;
			}

			if (unsentFiles.Count > 0)
			{
				if (unsentFiles.Count + files.Length >= _resultChunkLength)
				{
					// send a chunk
					string[] chunk = new string[_resultChunkLength];

					unsentFiles.CopyTo(chunk, 0);

					for (int i = unsentFiles.Count; i < _resultChunkLength; i++)
					{
						chunk[i] = files[i - unsentFiles.Count];
					}

					position = _resultChunkLength - unsentFiles.Count;
					unsentFiles.Clear();

					SendResultChunk(chunk, false);
					if (GetStop() == true)
					{
						return;
					}
				}
			}

			// send full chuncks
			int chunkCount = (files.Length - position) / _resultChunkLength;
			for (int i = 0; i < chunkCount; i++)
			{
				string[] chunk = new string[_resultChunkLength];

				int lastIndex = position + _resultChunkLength;
				for (int j = position; j < lastIndex; j++)
				{
					chunk[j - position] = files[j];
				}

				position += _resultChunkLength;

				// send the chunk
				SendResultChunk(chunk, false);
				if (GetStop() == true)
				{
					return;
				}
			}

			// send or store last files
			if (position != files.Length)
			{
				for (int i = position; i < files.Length; i++)
				{
					unsentFiles.Enqueue(files[i]);
				}
			}

			stop = false;
			lastSendTime = DateTime.Now;
		}


		private void ResetSearcher()
		{
			stop = false;
			lastSendTime = DateTime.Now;
			regex = null;
		}


		private void InitializeRegex(string pattern, bool compiled)
		{
			try
			{
				if (compiled)
				{
					regex = new Regex(pattern, RegexOptions.Compiled);
				}
				else
				{
					regex = new Regex(pattern);
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Failed to initialize Regex. Error: {0}", e.Message);
			}
		}


		private bool MaximumWaitTimeExceeded()
		{
			if (_maximumWaitTime.TotalMilliseconds == 0)
			{
				return false;
			}

			TimeSpan diff = DateTime.Now - lastSendTime;
			return diff > _maximumWaitTime;
		}


		private string[] SearchFilesImpl(string folder, string pattern, bool includeSubfolders, bool async)
		{
			// check the parameters
			Debug.AssertNotNull(folder, "Folder is null");

			if (GetStop() == true)
			{
				return null;
			}

			// fields
			List<string> files = null;

			try
			{
				string[] tempFiles = null;

				// get the files
				if (pattern == null || pattern.Length == 0)
				{
					tempFiles = Directory.GetFiles(folder);
				}
				else
				{
					if (regex == null)
					{
						tempFiles = Directory.GetFiles(folder, pattern);
					}
					else
					{
						// apply regex match
						tempFiles = Directory.GetFiles(folder, "*.*"); // get all files in this case
						List<string> matchingFiles = new List<string>();

						int count = tempFiles.Length;
						for (int i = 0; i < count; i++)
						{
							FileInfo info = new FileInfo(tempFiles[i]);

							if (regex.IsMatch(info.Name))
							{
								matchingFiles.Add(tempFiles[i]);
							}

							if(i % 32 == 0 && GetStop() == true)
							{
								return null;
							}
						}

						tempFiles = matchingFiles.ToArray();
					}
				}

				// notify the user about the new files
				if (tempFiles.Length > 0)
				{
					if (async)
					{
						if (_fileFilter != null)
						{
							// SPEED TESTING - START
							//Debug.PerformanceManager.AddEvent("photo_filter", true);

							List<string> validFiles = new List<string>(_resultChunkLength);

							int count = tempFiles.Length;
							for (int i = 0; i < count; i++)
							{
								if (_fileFilter.AllowFile(tempFiles[i]))
								{
									validFiles.Add(tempFiles[i]);
								}

								if ((i > 0 && (i % _resultChunkLength) == 0) ||
									MaximumWaitTimeExceeded() == true)
								{
									SendFiles(validFiles.ToArray(), true);
									validFiles.Clear();
								}
							}

							// SPEED TESTING - END
							//Debug.PerformanceManager.StopEvent("photo_filter");							
							//Debug.PerformanceManager.GenerateHtmlSummary();

							// send last chunk
							if (validFiles.Count > 0)
							{
								SendFiles(validFiles.ToArray(), false);
							}

							// stop execution
							if (GetStop() == true)
							{
								return null;
							}
						}
						else
						{
							SendFiles(tempFiles, false);
						}

						// stop execution
						if (GetStop() == true)
						{
							return null;
						}
					}
					else
					{
						// process the files
						if (_fileFilter != null)
						{
							tempFiles = ProcessFiles(tempFiles);
						}

						// append the files to the list
						files = new List<string>();
						files.AddRange(tempFiles);
					}
				}

				// ****************************************************************************************************
				// Subfolders
				// ****************************************************************************************************

				if (includeSubfolders)
				{
					string[] tempDirs = Directory.GetDirectories(folder);

					if (tempDirs != null && tempDirs.Length > 0)
					{
						for (int i = 0; i < tempDirs.Length; i++)
						{
							tempFiles = SearchFilesImpl(tempDirs[i], pattern, includeSubfolders, async);

							if (tempFiles != null && tempFiles.Length > 0 && async == false)
							{
								if (files == null)
								{
									files = new List<string>();
								}

								files.AddRange(tempFiles);
							}

							if (GetStop() == true)
							{
								return null;
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while searching files. Exception: {0}", e.Message);
			}

			if (files != null)
			{
				return files.ToArray();
			}
			else
			{
				return null;
			}
		}


		private void SendLastSet()
		{
			string[] chunk = null;

			if (unsentFiles.Count > 0)
			{
				chunk = new string[unsentFiles.Count];
				unsentFiles.CopyTo(chunk, 0);
				unsentFiles.Clear();
			}

			// send the chunk
			SendResultChunk(chunk, true);
		}


		private void SearchAsync(object param)
		{
			if ((param is ThreadParams) == false)
			{
				return;
			}

			ThreadParams p = (ThreadParams)param;

			lock (searchLock)
			{
				try
				{
					ResetSearcher();

					// initialize regex pattern
					if (p.regexPattern)
					{
						// compile for faster execution
						InitializeRegex(p.pattern, true);
					}

					// start searching
					SearchFilesImpl(p.folder, p.pattern, p.includeSubfolders, true);
					SendLastSet();
				}
				catch (Exception e)
				{
					Debug.ReportError("Error while searching asynchronously. Exception: {0}", e.Message);
				}
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Search files
		/// </summary>
		/// <param name="folder">The folder where to begin searching.</param>
		/// <param name="pattern">The pattern the files need to match (ex. *.txt).</param>
		/// <param name="includeSubfolders">Specifies whether or not to search the subfolders.</param>
		/// <returns>An array of FileInfo structures describing the founded files.</returns>
		public string[] SearchFiles(string folder, string pattern,bool regexPattern, bool includeSubfolders)
		{
			lock (searchLock)
			{
				ResetSearcher();
				
				// initialize regex pattern
				if (regexPattern)
				{
					// compile for faster execution
					InitializeRegex(pattern, true);
				}

				// start searching
				return SearchFilesImpl(folder, pattern, includeSubfolders, false);
			}
		}


		/// <summary>
		/// Search files asynchronously
		/// </summary>
		/// <param name="folder">The folder where to begin searching.</param>
		/// <param name="pattern">The pattern the files need to match (ex. *.txt).</param>
		/// <param name="includeSubfolders">Specifies whether or not to search the subfolders.</param>
		public void SearchFilesAsync(string folder, string pattern,bool regexPattern, bool includeSubfolders)
		{
			Thread t = new Thread(SearchAsync);
			t.Name = "SearchThread";

			ThreadParams param = new ThreadParams();
			param.folder = folder;
			param.pattern = pattern;
			param.regexPattern = regexPattern;
			param.includeSubfolders = includeSubfolders;

			t.Priority = ThreadPriority.BelowNormal;
			t.Start(param);
		}

		public void Stop()
		{
			SetStop(true);
		}

		#endregion
	}
}