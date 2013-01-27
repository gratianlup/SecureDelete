// Copyright (c) Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// Copyright (c) Gratian Lup. All rights reserved.
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

#ifndef FREE_SPACE_H
#define FREE_SPACE_H

#include "wcontext.h"

#define FREE_SPACE_FOLDER     L"__sd_folder__"
#define FREE_SPACE_FILE_SIZE  1073741824       // 1 GB
#define FREE_SPACE_FILE_SIZE2  104857600

// ****************************************************************************
// *                                                                          *
// * deleteFolder - deletes a folder, including its subfolders.               *
// * INSECURE VERSION ! ( but very fast )                                     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::deleteFolder(wchar_t *folder) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	int              folderStrLength;
	wchar_t         *aux;

	folderStrLength = wcslen(folder);
	wcscpy(path,folder);
    
	if( path[folderStrLength - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}

	wcscat(path,L"*.*");
	
	/*
	** find the firs file
	*/
	find = FindFirstFile(path,&ffData);

	if( find == INVALID_HANDLE_VALUE ) {
		return 1; // folder dosen't exists
	}

	do {
		if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // file
			wcscpy(filePath,folder);
			
			if( filePath[folderStrLength - 1] != L'\\' ) {
				/*
				** append \
				*/
				wcscat(filePath,L"\\");
			}
			
			wcscat(filePath,ffData.cFileName);
			
			/*
			** delete file
			*/
			if( !DeleteFile(filePath) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't delete file %s",filePath);
			}
		}
		else if(!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
				!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                  ffData.cFileName[2] == 0) ) {
			wcscpy(filePath,folder);
			
			if( filePath[folderStrLength - 1] != L'\\' ) {
				/*
				** append \
				*/
				wcscat(filePath,L"\\");
			}
			
			wcscat(filePath,ffData.cFileName);
							
			/*
			** delete folder
			*/
			deleteFolder(filePath);
		}
	} while( FindNextFile(find,&ffData) );
	
	FindClose(find);
	
	if( !RemoveDirectory(folder) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't remove directory %s ( maybe it isn't empty )",path);
		return 0;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * insertFolderInFileHash - inserts all files in file hash, including those *
// * found in subfolders                                                      *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::insertFolderInFileHash(wchar_t *folder,LLIST<wchar_t *> *fileList) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	wchar_t         *aux;
	int              folderStrLength;

	folderStrLength = wcslen(folder);
	wcscpy(path,folder);
    
	if( path[wcslen(path) - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}

	wcscat(path,L"*.*");

	/*
	** find the firs file
	*/
	find = FindFirstFile(path,&ffData);

	if( find == INVALID_HANDLE_VALUE ) {
		return;
	}

	do {
		if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // file
			wcscpy(filePath,folder);

			if( filePath[folderStrLength - 1] != L'\\' ) {
				/*
				** append \
				*/
				wcscat(filePath,L"\\");
			}

			wcscat(filePath,ffData.cFileName);

			/*
			** insert file in file hash ?
			*/
			if( fileList != NULL ) {
				aux = new wchar_t[wcslen(filePath) + 1];
				wcscpy(aux,filePath);
				fileHash.insertFile(aux,wcslen(aux));
				dirHash[aux[0] - 'A'].insertFile(aux,wcslen(aux));
				fileList->insert(&aux);
			}
		}
		else if(!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
                !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                  ffData.cFileName[2] == 0) ) {
            wcscpy(filePath,folder);

            if( filePath[folderStrLength - 1] != L'\\' ) {
                /*
                ** append \
                */
                wcscat(filePath,L"\\");
            }

            wcscat(filePath,ffData.cFileName);

            /*
            ** delete folder
            */
            insertFolderInFileHash(filePath,fileList);
		}
	} while( FindNextFile(find,&ffData) );

	FindClose(find);

	/*
	** insert folder in file hash ?
	*/
	if( fileList != NULL ) {
		aux = new wchar_t[folderStrLength + 1];
		wcscpy(aux,folder);
		fileHash.insertFile(aux,wcslen(aux));
		dirHash[aux[0] - 'A'].insertFile(aux,wcslen(aux));
		fileList->insert(&aux);
	}
}


// ****************************************************************************
// *                                                                          *
// * destroyFreeSpaceFolder - ( useful if FBI hunts you :D )                  *
// *                                                                          *
// ****************************************************************************
int WCONTEXT::destroyFreeSpaceFolder(wchar_t *folder) {
	LLIST<wchar_t *>  fileList;
	FILE_INFO        *fileInfo;
	wchar_t          *aux;

	if( folder == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Invalid parameters ( folder )");
		return 0;
	}

	/*
	** search file records
	*/
	if( woptions.destroyFreeSpaceFiles == 1 ) {
		dirHash[folder[0] - 'A'].resetObject();
		insertFolderInFileHash(folder,&fileList);
		searchFileRecords(folder[0]);
	}
	
	/*
	** delete folder
	*/
	if( !deleteFolder(folder) ) {
		return 0;
	}

	if( woptions.destroyFreeSpaceFiles == 1 ) {
		/*
		** destroy file records and directory index
		*/
		FlushFileBuffers(ntfsVolumes[folder[0] - 'A']->volume); // we must flush all metadata on disk,
																// else NTFS can write it back just
																// after we wiped it :(
		
		for(int i = 0;i < fileList.number;i++) {
			aux = fileList[i];
			fileInfo = fileHash.findFile(aux,wcslen(aux));
			
			if( fileInfo != NULL ) {
				if( wipeFileInfo(fileInfo,aux) != ERRORCODE_SUCCESS ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR ! ERROR ! ERROR ! WHY ???");
				}
			}
		}
	}
	
	/*
	** clear file list
	*/
	for(int i = 0;i < fileList.number;i++) {
		aux = fileList[i];
		delete[] aux;
	}
    
	fileList.clear();
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * generateRandomFilename -                                                 *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::genereateRandomFilename(wchar_t *fileName,int length) {
	static wchar_t  alphabet[] = L"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; // 60
	
	for(int i = 0;i < length;i++) {
		fileName[i] = alphabet[(unsigned long)rng.getULong() % 59];
	}
	fileName[length] = NULL;
}


// ****************************************************************************
// *                                                                          *
// * decompressFolder - decompresses a folder on an NTFS volume               *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::decompressFolder(wchar_t *folder) {
	HANDLE        folderHandle;
	unsigned long returned;
	
	if( volInfo.getVolumeType(folder[0] - 'A') != FSTYPE_NTFS ) {
		return; // only under NTFS files/folders can be compressed
	}
	
	/*
	** open folder
	*/
	folderHandle = CreateFile(folder,GENERIC_READ | GENERIC_WRITE,0,NULL,
							  OPEN_EXISTING,FILE_FLAG_BACKUP_SEMANTICS,NULL);
							  
	if( folderHandle == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't open folder %s",folder);
		return;
	}
	
	/*
	** disable compression
	*/
	DeviceIoControl(folderHandle,FSCTL_SET_COMPRESSION,NULL,0,
                    COMPRESSION_FORMAT_NONE, sizeof(int),&returned,NULL);
	
}


// ****************************************************************************
// *                                                                          *
// * wipeFreeSpace - wipes the free space on a single volume, including       *
// * cluster tips and MFT. It creates files in a temporary folder until the   *
// * the volume is full. The method is more reliable than wiping each cluster *
// * from the volume bitmap. The only drawback is that file informations and  *
// * the temporary directory remains on disk and an investigator could        *
// * recognize that the free space was wipe. But I found a solution ( only    *
// * under NTFS ) - see destroyFreeSpaceFolder function.                      *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeFreeSpace(wchar_t volume) {
	wchar_t          folderPath[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	wchar_t          randomFilename[MAX_PATH];
	__int64          freeSpace;
	int              result;
	
	if( volume < 'A' || volume > 'Z' ) {
		dbgPrint(__WFILE__,__LINE__,L"Invalid parameters");
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** load wipe method
	*/
	if( !setWOMethod(wobject->wmethod) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method for %s",wobject->path);
		return ERRORCODE_WMETHOD;
	}
	
	/*
	** set current work volume
	*/
	volInfo.cv = volume - 'A';
	folderPath[0] = volume; folderPath[1] = ':';
	folderPath[2] = '\\';   folderPath[3] = NULL;
	
	/*
	** set information
	*/
	updateMessage(WSTATUS_MESSAGE_FREE_SPACE,volume);
	updateAuxMessage(WSTATUS_SUBMESSAGE_FREE_SPACE);
	wcscat(folderPath,FREE_SPACE_FOLDER);
	
	/*
	** wipe free space
	*/
	if( wobject->woptions & WOOPTION_WIPE_FREE_SPACE ) {
		/*
		** destroy free space folder ( if it exists )
		*/
		if( !destroyFreeSpaceFolder(folderPath) ) {
			return ERRORCODE_FREE_SPACE_FOLDER;
		}
		
		/*
		** create new folder
		*/
		if( !CreateDirectory(folderPath,NULL) && GetLastError() != ERROR_ALREADY_EXISTS ) {
			return ERRORCODE_FREE_SPACE_FOLDER;
		}
		
		/*
		** finally, wipe the free space
		*/
		if( !getDriveFreeSpace(volume,freeSpace) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get free space for volume %c",volume);
			return ERRORCODE_UNKNOWN;
		}
		
		if( stopped ) {
			destroyFreeSpaceFolder(folderPath);
			return ERRORCODE_STOPPED;
		}
		
		/*
		** update progress statistics
		*/
		updateWobjectTotal(freeSpace * wowmethod->nSteps * (wowmethod->checkWipe ? 2 : 1));
		resetWobjectWiped();
		
		while( freeSpace > 0 ) {          
			/*
			** generate temporary file name
			*/
			genereateRandomFilename(randomFilename,64);
			wsprintf(filePath,L"%s\\%s",folderPath,randomFilename);
			
			/*
			** how much to write ?
			*/
			if( freeSpace >= FREE_SPACE_FILE_SIZE ) {
				woSize.QuadPart = FREE_SPACE_FILE_SIZE; // write 1 GB
				fastWrite = 1;
			}
			else if( freeSpace >= FREE_SPACE_FILE_SIZE2 ) { 
				woSize.QuadPart = FREE_SPACE_FILE_SIZE2; // write 100 MB
				fastWrite = 1;
			}
			else {
				woSize.QuadPart = freeSpace;
				fastWrite = 0;
			}
			
			/*
			** create file
			*/
			woHandle = CreateFile(filePath,GENERIC_WRITE | GENERIC_READ,
								  FILE_SHARE_WRITE | FILE_SHARE_READ,NULL,
								  OPEN_ALWAYS,FILE_ATTRIBUTE_NORMAL |
								  (fastWrite == 1 ? FILE_FLAG_NO_BUFFERING : 0) | 
								  FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH,NULL);
								  
			if( woHandle == INVALID_HANDLE_VALUE ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't create file %s",filePath);
				return ERRORCODE_UNKNOWN;
			}
								  
			woPosition.QuadPart = 0;
			result = wipeStream(woHandle);
			
			if( result != ERRORCODE_SUCCESS ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't wipe file %s",filePath);
				CloseHandle(woHandle);
				destroyFreeSpaceFolder(folderPath);
				return result;
			}
			
			/*
			** close file
			*/
			CloseHandle(woHandle);
			
			/*
			** update free space quantity
			*/
			if( !getDriveFreeSpace(volume,freeSpace) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get free space for volume %c",volume);
				destroyFreeSpaceFolder(folderPath);
				return ERRORCODE_UNKNOWN;
			}
			
			if( stopped ) {
				CloseHandle(woHandle);
				destroyFreeSpaceFolder(folderPath);
				return ERRORCODE_STOPPED;
			}
		}
		
		/*
		** destroy free space folder
		*/
		if( !destroyFreeSpaceFolder(folderPath) ) {
			return ERRORCODE_FREE_SPACE_FOLDER;
		}
	}
	
	if( wobject->woptions & WOOPTION_WIPE_CLUSTER_TIPS ) {
		/*
		** wipe cluster tips
		*/
		folderPath[0] = volume; folderPath[1] = ':';
		folderPath[2] = '\\';   folderPath[3] = NULL;
		
		wstatus.type = WOTYPE_CLUSTER_TIPS;
		wstatus.objectBytes = countFilesInFolder(folderPath);
		wstatus.objectWipedBytes = 0;
		
		wipeClusterTipsRecursive(folderPath);
		wstatus.objectWipedBytes = wstatus.objectBytes;
	}
	
	if( wobject->woptions & WOOPTION_WIPE_MFT ) {
		/*
		** wipe MFT
		*/
		setObjectIndex();
		wipeMFT(volume);
	}
	
	return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getNextVolumes - gets a run of volumes located on the same disk          *
// *                                                                          *
// ****************************************************************************
inline wchar_t *WCONTEXT::getNextVolumes(wchar_t *volumes, int &length,int *disks) {
	int strLength;
	int i;
	
	if( volumes == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters");
		return NULL;
	}

	strLength = wcslen(volumes);
	if( strLength == 0 ) {
		return NULL;
	}
	
	/*
	** search all volumes that resides on the same disk
	*/
	for(i = 1;i < strLength;i++) {
		if( disks[i] != disks[0] ) {
			break;
		}
	}
	
	length = i;
	return volumes;
}


// ****************************************************************************
// *                                                                          *
// * freeSpaceThread - very simple thread used for wiping volumes located on  *
// * different disks the same time                                            *
// *                                                                          *
// ****************************************************************************
unsigned long WINAPI freeSpaceThread(void *param) {
	WCONTEXT *child = (WCONTEXT *)param;
	child->startWipe();
	return 0;
}


// ****************************************************************************
// *                                                                          *
// * wipeMultipleFreeSpace - wipes volumes located on different disks the     *
// * same time. For each run with volumes located on the same disk, a child   *
// * context is created that runs in a separate thread. First run is handled  *
// * by the parent context.                                                   *
// *                          First Thread       Second Thread                *
// * EXAMPLE:                +-----------+        +----------+                *
// * VOLUME: C D E F         |PARENT: C,D| -----> |CHILD: E,F|                *
// * DISK  : 0 0 1 1         +-----------+        +----------+                *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeMultipleFreeSpace(wchar_t *volumes) {
	int               volumesNumber;
	int               volumeDisk[27];
	int               auxDisk;
	wchar_t           auxVolume;
	wchar_t          *volumeRun;
	int               firstRunLength;
	int               volumeRunLength;
	int               firstRun = 1;
	WCONTEXT         *child;
	int               result = ERRORCODE_UNKNOWN;
	WEXTENDED_OBJECT  wexobject;
	
	if( volumes == NULL || wcslen(volumes) == 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters");
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** first step: get disk where the volume resides
	*/
	volumesNumber = wcslen(volumes);
    
	for(int i = 0;i < volumesNumber;i++) {
		volInfo.getVolumeDisk(volumes[i] - 'A',volumeDisk[i]);
	}
	
	/*
	** second step: sort volumes
	*/
	for(int i = 0;i < volumesNumber;i++) {
		for(int j = i + 1;j < volumesNumber;j++) {
			if( volumeDisk[i] > volumeDisk[j] ) {
				auxDisk = volumeDisk[i];
				volumeDisk[i] = volumeDisk[j];
				volumeDisk[j] = auxDisk;
				
				auxVolume = volumes[i];
				volumes[i] = volumes[j];
				volumes[j] = auxVolume;
			}
		}
	}
	
	/*
	** third step: create a context for each drive
	*/
	childrenNumber = 0;
	volumeRun = volumes;
    
	while( (volumeRun == getNextVolumes(volumeRun,volumeRunLength,volumeDisk)) != NULL ) {
		if( !firstRun ) {
			children[childrenNumber] = new WCONTEXT;
			child = children[childrenNumber];
			child->parent = this;

			/*
			** set options and initialize context
			*/
			child->setOptions(&woptions);
			child->id = id + childrenNumber;
			
			if( !child->initialize() ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize child context");
				stopChildren();
				destroyChildren();
				return ERRORCODE_UNKNOWN;
			}
			
			/*
			** insert wipe object
			*/
			wexobject.type = wobject->type;
			wexobject.wmethod = wobject->wmethod;
			wexobject.woptions = wobject->woptions;
			memcpy(wexobject.path,volumeRun,sizeof(wchar_t) * volumeRunLength);
			wexobject.path[volumeRunLength] = NULL;
			
			if( !child->insertObject(&wexobject) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't insert free space object into child context");
				stopChildren();
				destroyChildren();
				return ERRORCODE_UNKNOWN;
			}
			
			/*
			** create thread
			*/
			childrenThreads[childrenNumber] = CreateThread(NULL,0,freeSpaceThread,
														   (void *)child,0,NULL);
			childrenNumber++;
		}
		else {
			firstRunLength = volumeRunLength;
			firstRun = 0;
		}

		volumeRun += volumeRunLength;
	}
	
	/*
	** parent context wipes volumes located on first drive
	*/
	for(int i = 0;i < firstRunLength;i++) {
		result = wipeFreeSpace(volumes[i]);
		
		if( result != ERRORCODE_SUCCESS ) {
			handleFreeSpaceError(volumes[i],result);
		}
		
		if( stopped ) {
			break;
		}
	}
}


// ****************************************************************************
// *                                                                          *
// * wipeClusterTip - wipes the cluster tip ( also known as slack space )     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeClusterTip(wchar_t *file) {
	int            result;
	unsigned long  attributes;
	FILETIME       ftCreation,ftLastAccess,ftLastWrite;
	ULARGE_INTEGER size;
	
	if( file == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameter");
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	* get file attributes
	*/
	attributes = GetFileAttributes(file);
	if( attributes == INVALID_FILE_ATTRIBUTES ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get attributes for file %s",file);
		return ERRORCODE_NOTFOUND;
	}
	
	/*
	** don't wipe cluster tip for compressed/encrypted/sparse files
	*/
	if( (attributes & FILE_ATTRIBUTE_SYSTEM)  || 
        (attributes & FILE_ATTRIBUTE_HIDDEN ) ||
		(attributes & FILE_ATTRIBUTE_READONLY) ) {
		return ERRORCODE_SUCCESS;	
	}
	
	/*
	** update status message
	*/
	updateAuxMessage(WSTATUS_SUBMESSAGE_CLUSTER_TIPS,file);
	
	/*
	** open file
	*/
	woHandle = CreateFile(file,GENERIC_WRITE | GENERIC_READ,FILE_SHARE_WRITE | FILE_SHARE_READ,NULL,
						  OPEN_EXISTING,FILE_FLAG_WRITE_THROUGH | FILE_FLAG_RANDOM_ACCESS,NULL);
						  
	if( woHandle == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't open file %s",file);
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** save current file times
	*/
	if( !GetFileTime(woHandle,&ftCreation,&ftLastAccess,&ftLastWrite) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get times for file %s",file);
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** get file size
	*/
	size.LowPart = GetFileSize(woHandle,&size.HighPart);
	if( size.LowPart == INVALID_FILE_SIZE && GetLastError() != NO_ERROR ) {
		return ERRORCODE_UNKNOWN;
	}
	
	/*
	** wipe cluster tip
	*/
	volInfo.cv = file[0] - 'A';
	woSize.QuadPart = volInfo.getVolumeClusterSize(volInfo.cv) - 
                      (size.QuadPart % volInfo.getVolumeClusterSize(volInfo.cv));
	woPosition.QuadPart = size.QuadPart;
    
	if( woSize.QuadPart <= 0 ) {
		return ERRORCODE_SUCCESS; // file has size = 0 or size is multiple of sector size
	}
	
	fastWrite = 0;
	result = wipeStream(woHandle);
	
	/*
	** restore file size and file times
	*/
	SetFileTime(woHandle,&ftCreation,&ftLastAccess,&ftLastWrite);
	SetFilePointer(woHandle,size.LowPart,(long *)size.HighPart,FILE_BEGIN);
	SetEndOfFile(woHandle);
	
	/*
	** close file
	*/
	CloseHandle(woHandle);	
	return result;
}


// ****************************************************************************
// *                                                                          *
// * wipeClusterTipsRecursive - wipes the cluster tips in all files in the    *
// * given folder and all his subfolders                                      *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeClusterTipsRecursive(wchar_t *folder) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	wchar_t         *aux;
	int              folderStrLength;

	folderStrLength = wcslen(folder);
	wcscpy(path,folder);
    
	if( path[folderStrLength - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}

	wcscat(path,L"*.*");

	/*
	** find the firs file
	*/
	find = FindFirstFile(path,&ffData);

	if( find == INVALID_HANDLE_VALUE ) {
		return 1; // folder dosen't exists
	}

	do {
		if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // file
			wcscpy(filePath,folder);

			if( filePath[folderStrLength - 1] != L'\\' ) {
				/*
				** append \
				*/
				wcscat(filePath,L"\\");
			}

			wcscat(filePath,ffData.cFileName);

			/*
			** wipe cluster tip
			*/
			if( wipeClusterTip(filePath) != ERRORCODE_SUCCESS ) {
				log.addToLog(SEVERITY_MEDIUM,L"Failed to wipe cluster tip for file %s. File is used by another program",filePath);
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't wipe cluster tip for file %s",filePath);
			}
			
			/*
			** update wipe status
			*/
			updateWobjectWiped2(1);
		}
		else if(!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
                !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                  ffData.cFileName[2] == 0) ) {
            wcscpy(filePath,folder);

            if( filePath[folderStrLength - 1] != L'\\' ) {
                /*
                ** append \
                */
                wcscat(filePath,L"\\");
            }

            wcscat(filePath,ffData.cFileName);

            /*
            ** delete folder
            */
            if(!stopped) {
                wipeClusterTipsRecursive(filePath);
            }
		}
	} while( FindNextFile(find,&ffData) && !stopped );

	FindClose(find);
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * countFilesInFolder - counts all files in the given folder and in all its *
// * subfolders                                                               *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::countFilesInFolder(wchar_t *folder) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	wchar_t         *aux;
	int              ct = 0;
	int              folderStrLength;
	
	folderStrLength = wcslen(folder);
	wcscpy(path,folder);
    
	if( path[folderStrLength - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}

	wcscat(path,L"*.*");

	/*
	** find the firs file
	*/
	find = FindFirstFile(path,&ffData);

	if( find == INVALID_HANDLE_VALUE ) {
		return 1; // folder dosen't exists
	}

	do {
		if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // file
			/*
			** count file
			*/
			ct++;
		}
		else if(!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
                !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                  ffData.cFileName[2] == 0) ) {
            wcscpy(filePath,folder);

            if( filePath[folderStrLength - 1] != L'\\' ) {
                /*
                ** append \
                */
                wcscat(filePath,L"\\");
            }

            wcscat(filePath,ffData.cFileName);

            /*
            ** count files in subfolder
            */
            ct += countFilesInFolder(filePath);
		}
	} while( FindNextFile(find,&ffData) && !stopped );

	FindClose(find);

	return ct;
}

#endif
