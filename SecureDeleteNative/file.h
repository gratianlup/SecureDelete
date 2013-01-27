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

#ifndef FILE_H
#define FILE_H

#include "wcontext.h"
#include "wstatus.h"

#define FILENAME_OVERWRITE_STEPS 3

// ****************************************************************************
// *                                                                          *
// * resetFileDate - sets the file date to 1/1/1980 00:00                     *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::resetFileDateTime() {
	SYSTEMTIME  stTime;
	FILETIME    ftLocalTime;
	FILETIME    ftTime;

	stTime.wYear            = 1980;
	stTime.wMonth           = 1;
	stTime.wDayOfWeek       = 0;
	stTime.wDay             = 1;
	stTime.wHour            = 0;
	stTime.wMinute          = 0;
	stTime.wSecond          = 0;
	stTime.wMilliseconds    = 0;

	SystemTimeToFileTime(&stTime, &ftLocalTime);
	LocalFileTimeToFileTime(&ftLocalTime, &ftTime);
	SetFileTime(woHandle, &ftTime, &ftTime, &ftTime);
}


// ****************************************************************************
// *                                                                          *
// * wipeFileName - wipes the file name                                       *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::wipeFilename(wchar_t *path) {
	wchar_t         secondPath[MAX_PATH];
	unsigned char   random[MAX_PATH];
	static          wchar_t  alphabet[] = L"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; // 60
	int             pathLength;
	wchar_t        *lastSlash;
	int             lastSlashPos;
	int             filenameLength = 0;
	
	pathLength = wcslen(path);
	memcpy(secondPath,path,sizeof(wchar_t) * (pathLength + 1));
	
	/*
	** eliminate '\' from path end
	*/
	if( path[pathLength] == '\\' ) {
		path[pathLength] = NULL;
		pathLength--;
	}
	
	/*
	** find last slash
	*/
	lastSlash = &path[pathLength];
    
	while( *lastSlash != '\\' ) {
		lastSlash--;
		filenameLength++;
	}
	
	lastSlashPos = lastSlash - path;
	
	for(int i = 0;i < FILENAME_OVERWRITE_STEPS;i++) {
		rng.getRandom(random,filenameLength - 1);
	
		for(int j = lastSlashPos + 1;j < pathLength;j++) {
			secondPath[j] = alphabet[random[j] % 60];
		}
		
		if( !MoveFile(path,secondPath) ) {
			i--;
			continue;
		}
		
		memcpy(&path[lastSlashPos],&secondPath[lastSlashPos],
               sizeof(wchar_t) * filenameLength);
	}
}


// ****************************************************************************
// *                                                                          *
// * wipeFile - wipes a file                                                  *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeFile(wchar_t *file,int wipeFileRecord) {
	unsigned long  attributes;
	unsigned long  result;
	int            compressed = 0;
	FILE_INFO     *fileInfo;
	
	/*
	** set information
	*/
	setObjectIndex();
	updateMessage(WSTATUS_MESSAGE_FILE);
	updateAuxMessage(file);
	
	/*
	** load wipe method
	*/
	if( !setWOMethod(wobject->wmethod) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method for %s",file);
		return ERRORCODE_WMETHOD;
	}
	
	/*
	** get file attributes
	*/
	attributes = GetFileAttributes(file);
	if( attributes == INVALID_FILE_ATTRIBUTES ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get attributes for file %s. Error %d",
                                    file,GetLastError());
		return ERRORCODE_NOTFOUND;
	}
	
	/*
	** set current work volume
	*/
	volInfo.cv = file[0] - 'A';
	
	/*
	** ignore read-only attribute, wipe all files
	*/
	SetFileAttributes(file,FILE_ATTRIBUTE_NORMAL);
	
	/*
	** open file with read/write access (unbuffered)
	*/
	woHandle = CreateFile(file,GENERIC_READ | GENERIC_WRITE,0,NULL,
						  OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL | FILE_FLAG_NO_BUFFERING |
						  FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_WRITE_THROUGH,NULL);
						  
	if( woHandle == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't open file %s. Error %d",file,GetLastError());
		return ERRORCODE_ACCESS;
	}
	
	/*
	** check if the file is compressed/sparse of encrypted ( only under NTFS )
	*/
	if( volInfo.getVolumeType(volInfo.cv) == FSTYPE_NTFS ) {
		if( (attributes & FILE_ATTRIBUTE_COMPRESSED) || (attributes & FILE_ATTRIBUTE_ENCRYPTED) ||
			(attributes & FILE_ATTRIBUTE_SPARSE_FILE) ) {
			result = wipeCompressedFile(file);
			if( result != ERRORCODE_SUCCESS ) {
				return result;
			}
			
			compressed = 1;
		}
	}
	
	/*
	** should ADS be wiped ?
	*/
	if( wobject->woptions & WOOPTION_WIPE_ADS ) {
		result = wipeAds();
		if( result != ERRORCODE_SUCCESS ) {
			return result;
		}
	}
	
	/*
	** wipe default stream
	*/
	if( compressed == 0 ) { // only for uncompressed
		woSize.LowPart = GetFileSize(woHandle,&woSize.HighPart);
		if( woSize.LowPart == INVALID_FILE_SIZE && GetLastError() != NO_ERROR ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get file size for %s. Error %d",
                                        file,GetLastError());
			return ERRORCODE_SIZE;
		}
		
		/*
		** set data pointer and start wiping...
		*/
		if( woSize.QuadPart > 0 ) {
			updateWobjectTotal(woSize.QuadPart * wowmethod->nSteps * 
                               (wowmethod->checkWipe ? 2 : 1));
			resetWobjectWiped();
			woPosition.QuadPart = 0;
			fastWrite = 1;
			
			result = wipeStream(woHandle);
			
			setWobjectWiped(woSize.QuadPart * wowmethod->nSteps * 
                            (wowmethod->checkWipe ? 2 : 1));
			
			if( result != ERRORCODE_SUCCESS ) {
				CloseHandle(woHandle);
				return result;
			}
		}
	}
	
	if( woptions.totalDelete && wipeFileRecord ) {
		/*
		** TOTAL DELETE !
		*/
		CloseHandle(woHandle);
		
		DeleteFile(file);
		
		fileInfo = fileHash.findFile(file,wcslen(file));
 		if( fileInfo == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't find file record for %s",file);
			return ERRORCODE_FILE_RECORD;
		}
		
		FlushFileBuffers(ntfsVolumes[file[0] - 'A']->volume); // we must flush all metadata on disk,
												              // else NTFS can write it back just													  // after we wiped it :(
		result = wipeFileInfo(fileInfo,file);
		if( result!= ERRORCODE_SUCCESS ) {
			return result;
		}
	}
	else {
		/*
		** old method, not 100% reliable ( but faster )
		*/
		resetFileDateTime();
		CloseHandle(woHandle);
        
		if( wobject->woptions & WOOPTION_WIPE_FILENAME ) {
			wipeFilename(file);
		}
		
		DeleteFile(file);
	}
	
	return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * wipeFolder - wipes a folder, including its subfolders (if the user wants)*
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeFolder(wchar_t *folder) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          filePath[MAX_PATH];
	int              result;
	int              folderStrLength;
	FILE_INFO       *fileInfo;
    
	folderStrLength = wcslen(folder);
	wcscpy(path,folder);
    
	if( path[folderStrLength - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}

	/*
	** append mask ?
	*/
	if( wobject->woptions & WOOPTION_MASK ) {
		wcscat(path,wobject->aux);
	}
	else {
		// append *.*
		wcscat(path,L"*.*");
	}

	/*
	** find the firs file
	*/
	find = FindFirstFile(path,&ffData);

	if( find != INVALID_HANDLE_VALUE ) {		
		do {
			if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // file
				wcscpy(filePath,folder);

				if( folder[folderStrLength - 1] != L'\\' ) {
					/*
					** append \
					*/
					wcscat(filePath,L"\\");
				}
				wcscat(filePath,ffData.cFileName);

				/*
				** wipe file
				*/
				result = wipeFile(filePath,0);
				if( result != ERRORCODE_SUCCESS ) {
					handleFileError(filePath,result);
				}
				
				if( woptions.totalDelete ) {
					fileInfo = fileHash.findFile(filePath,wcslen(filePath));
					if( fileInfo == NULL ) {
						dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't find file record for %s",filePath);
					}
					else {
						/*
						** insert file info in list. We must wipe file records after
						** all files in the folder where wiped, else NTFS writes the
						** file records back when we call FindNextFile or FindClose :(
						*/
						dbgPrint(__WFILE__,__LINE__,L"Inserted %s",filePath);
						folderFileInfo.insert(&fileInfo);
					}
				}
			}
			else if( (wobject->woptions & WOOPTION_WIPE_SUBFOLDERS) && 
                     (wobject->woptions & WOOPTION_MASK) == 0 &&
                     !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
                     !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                       ffData.cFileName[2] == 0) ) {
				wcscpy(filePath,folder);

				if( path[folderStrLength - 1] != L'\\' ) {
					/*
					** append \
					*/
					wcscat(filePath,L"\\");
				}
				wcscat(filePath,ffData.cFileName);

				/*
				** wipe folder
				*/
				if(!stopped) {
					wipeFolder(filePath);
					
					if( woptions.totalDelete ) {
						fileInfo = fileHash.findFile(filePath,wcslen(filePath));
						if( fileInfo == NULL ) {
							dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't find file record for %s",filePath);
						}
						else {
							folderFileInfo.insert(&fileInfo); // see above !
						}
					}
				}
			}
		} while( FindNextFile(find,&ffData) && !stopped );
	}

	FindClose(find);

	/*
	** if a mask is used, the folder needs to be rescanned in order to traverse
	** the subfolders
	*/
	if( (wobject->woptions & WOOPTION_WIPE_SUBFOLDERS) && 
        (wobject->woptions & WOOPTION_MASK) ) {
		wcscpy(path,folder);
		if( path[folderStrLength - 1] != L'\\' ) {
			/*
			** append \
			*/
			wcscat(path,L"\\");
		}

		/*
		** append *.*
		*/
		wcscat(path,L"*.*");

		/*
		** find the firs file
		*/
		find = FindFirstFile(path,&ffData);

		if( find == INVALID_HANDLE_VALUE ) {
			return 0;
		}

		do {
			if( (ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // only folders
				if( !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
					!(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                      ffData.cFileName[2] == 0) ) {
					wcscpy(filePath,folder);

					if( filePath[wcslen(filePath) - 1] != L'\\' ) {
						/*
						** append \
						*/
						wcscat(filePath,L"\\");
					}
					wcscat(filePath,ffData.cFileName);
					
					/*
					** wipe folder
					*/
					wipeFolder(filePath);
					
					if( woptions.totalDelete ) {
						fileInfo = fileHash.findFile(filePath,wcslen(filePath));
						if( fileInfo == NULL ) {
							dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't find file record for %s",filePath);
						}
						else {
							folderFileInfo.insert(&fileInfo); // see above !
						}
					}
				}
			}
		} while( FindNextFile(find,&ffData) );

		FindClose(find);
	}
	
	/*
	** delete folder
	*/
	if( wobject->woptions & WOOPTION_DELETE_FOLDERS ) {
		SetFileAttributes(folder,FILE_ATTRIBUTE_NORMAL);
		if( !RemoveDirectory(folder) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't remove folder %s. Error %d",
                                        folder,GetLastError());
			handleFolderError(folder,ERRORCODE_FOLDER);
			return 0;
		}
			
		if( woptions.totalDelete ) {
			//FlushFileBuffers(ntfsVolumes[folder[0] - 'A']->volume);
			
			fileInfo = fileHash.findFile(folder,folderStrLength);
			if( fileInfo == NULL ) {
				dbgPrint(__WFILE__,__LINE__,L"WARNING: Couldn't find file record for %s",folder);
			}
			else {
				folderFileInfo.insert(&fileInfo); // see above !
			}
		}
	}

	return 1;
}

#endif
