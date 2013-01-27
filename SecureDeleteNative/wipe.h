// Copyright (c) 2005 Gratian Lup. All rights reserved.
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

#ifndef WIPE_H
#define WIPE_H

#include "wcontext.h"

/*
** definitions for error messages
*/
#define ERRORMESSAGE_STOPPED           L"Wipe process stopped by user. File %s not wiped completely."
#define ERRORMESSAGE_CRC               L"CRC check error while wiping file %s. Check for hardware failure."
#define ERRORMESSAGE_WRITE             L"Unknown error while writing data."
#define ERRORMESSAGE_WMETHOD           L"Wipe method needed to wipe file %s not found. Check settings."
#define ERRORMESSAGE_NOTFOUND          L"File %s not found or is being used by another application."
#define ERRORMESSAGE_SIZE              L"Failed to get size of file %s."
#define ERRORMESSAGE_ADS               L"Failed to wipe alternate data streams (ADS) for file %s."
#define ERRORMESSAGE_FOLDER            L"Failed to delete folder %s."
#define ERRORMESSAGE_FILE_RECORD       L"Failed to delete file record for %s."

#define ERRORMESSAGE_FREE_SPACE_STOPPED L"Wipe process stopped by user. Free space on volume %c not wiped completely."
#define ERRORMESSAGE_FREE_SPACE_CRC     L"CRC check error while wiping free space on volume %c. Check for hardware failure."
#define ERRORMESSAGE_FREE_SPACE_WRITE   L"Unknown error while writing free space on volume %c."
#define ERRORMESSAGE_FREE_SPACE_WMETHOD L"Wipe method needed to wipe free space on volume %c not found. Check settings."
#define ERRORMESSAGE_FREE_SPACE_FOLDER  L"Failed to create temporary folder used to wipe free space on volume %c."


// ****************************************************************************
// *                                                                          *
// * startWipe - wipes each object from the wipe list                         *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::startWipe() {
	int        result;
	FILE_INFO *fileInfo;

	stopped = 0;
	ResetEvent(stoppedEvent);
	wstatus.stopped = 0;

	/*
	** search file records
	*/
	if( woptions.totalDelete == 1 ) {
		updateMessage(WSTATUS_MESSAGE_TOTAL_DELETE);
		updateAuxMessage(WSTATUS_SUBMESSAGE_TOTAL_DELETE);
		searchObjectEntries();
	}

	for(__int64 i = 0;i < wobjects.number;i++) {
		wobject = wobjects[i];
		wstatus.stopped = 0;
		
		switch( wobject->type ) {
			case WOTYPE_FILE: {
				setObjectIndex();
				wstatus.type = WOTYPE_FILE;
				result = wipeFile(wobject->path,1);
				handleFileError(wobject->path,result);
				break;
			}
			case WOTYPE_FOLDER: {
				setObjectIndex();
				wstatus.type = WOTYPE_FOLDER;
				wipeFolder(wobject->path);
				
				/*
				** we must only now delete the file records, else
				** NTFS writes the metadata back to disk !
				** For example, when a folder is opened, NTFS loads all metadata
				** (index records,file record) in cache (helped by the Cache Manager)
				** and modifies the metadata in memory. When the folder is closed or
				** a certain amount of time passes, NTFS instructs the Cache Manager
				** to flush the metadata on disk. If we would wipe the metadata before
				** closing the folder (FindClose), NTFS would write it back !!!
				** 
				** For more informations see Microsoft Windows Internals, Fourth Edition
				** (By Mark E. Russinovich, David A. Solomon)
				*/
				if( woptions.totalDelete ) {
					FlushFileBuffers(ntfsVolumes[wobject->path[0] - 'A']->volume);
					FlushFileBuffers(ntfsVolumes[wobject->path[0] - 'A']->volume);
					
					/*
					** change status message (it can last some time, depending
					** on the folder size)
					*/
					updateMessage(WSTATUS_MESSAGE_FOLDER_FILE_RECORDS);
					updateAuxMessage(WSTATUS_SUBMESSAGE_FOLDER_FILE_RECORDS,wobject->path);
					updateWobjectTotal(folderFileInfo.number);
					wstatus.type = WOTYPE_MFT;
					
					/*
					** wipe file records
					*/
					for(int i = 0;i < folderFileInfo.number;i++) {
						if( i % 15 == 0 ) {
							setWobjectWiped(i);
						}
						fileInfo = folderFileInfo[i];

						if( wipeFileInfo(fileInfo,wobject->path) != ERRORCODE_SUCCESS ) {
							dbgPrint(__WFILE__,__LINE__,L"ERROR ERROR ERROR !!!");
						}
					}
					setWobjectWiped(folderFileInfo.number);
					
					folderFileInfo.clear();
				}
				
				break;
			}
			case WOTYPE_DRIVE: {
				setObjectIndex();
				wstatus.type = WOTYPE_DRIVE;
				result = wipeMultipleFreeSpace(wobject->path);
				wstatus.stopped = 1;

				// wait for children and destroy them
				waitChildren();
				getChildrenErrors();
				destroyChildren();

				break;
			}
		}
	}
	
	/*
	** finished, set event so that parent thread knows
	*/
	stopped = 1;
	wstatus.stopped = 1;
	log.closeLog();
	SetEvent(stoppedEvent);
}


// ****************************************************************************
// *                                                                          *
// * handleFileError - creates an error message for the error that occurred   *
// * while wiping a file                                                      *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::handleFileError(wchar_t *file,int errorCode) {
	if(errorCode == ERRORCODE_SUCCESS) {
		return;
	}

	// allocate the failed object structure
	WSMALL_OBJECT *failedObject = new WSMALL_OBJECT;

	failedObject->type = WOTYPE_FILE;
	wcscpy(failedObject->path,file);
	
	switch( errorCode ) {
		case ERRORCODE_STOPPED: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_STOPPED,file);
			break;
		}
		case ERRORCODE_CRC: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_CRC,file);
			break;
		}
		case ERRORCODE_WRITE: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_WRITE,file);
			break;
		 }
		case ERRORCODE_WMETHOD: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_WMETHOD,file);
			break;
		}
		case ERRORCODE_NOTFOUND: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_NOTFOUND,file);
			break;
		}
		case ERRORCODE_SIZE: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_SIZE,file);
			break;
		}
		case ERRORCODE_ADS: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_ADS,file);
			break;
		}
		case ERRORCODE_FILE_RECORD: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FILE_RECORD,file);
			break;
		}
	}
	
	/*
	** insert in error list
	*/
	failedObject->log = log.errorList.number - 1;
	failed.insert(&failedObject);
}


// ****************************************************************************
// *                                                                          *
// * handleFileError - creates an error message for the error that occurred   *
// * while wiping a folder                                                    *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::handleFolderError(wchar_t *folder,int errorCode) {
	if(errorCode == ERRORCODE_SUCCESS) {
		return;
	}

	// allocate the failed object structure
	WSMALL_OBJECT *failedObject = new WSMALL_OBJECT;

	failedObject->type = WOTYPE_FOLDER;
	wcscpy(failedObject->path,folder);
	
	switch( errorCode ) {
		case ERRORCODE_FOLDER: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FOLDER,folder);
			break;
		}
		case ERRORCODE_FILE_RECORD: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FILE_RECORD,folder);
			break;
		}
	}
	
	/*
	** insert in error list
	*/
	failedObject->log = log.errorList.number - 1;
	failed.insert(&failedObject);				
}


// ****************************************************************************
// *                                                                          *
// * handleFileError - creates an error message for the error that occurred   *
// * while wiping the free space                                              *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::handleFreeSpaceError(wchar_t volume,int errorCode) {
	WSMALL_OBJECT *failedObject = new WSMALL_OBJECT;

	failedObject->type = WOTYPE_DRIVE;
	failedObject->path[0] = volume;
	failedObject->path[1] = NULL;

	switch( errorCode ) {
		case ERRORCODE_STOPPED: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FREE_SPACE_STOPPED,volume);
			break;
		}
		case ERRORCODE_CRC: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FREE_SPACE_CRC,volume);
			break;
		}
		case ERRORCODE_WRITE: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FREE_SPACE_WRITE,volume);
			break;
		}
		case ERRORCODE_WMETHOD: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FREE_SPACE_WMETHOD,volume);
			break;
		}
		case ERRORCODE_FREE_SPACE_FOLDER: {
			log.addToLog(SEVERITY_HIGH,ERRORMESSAGE_FREE_SPACE_FOLDER,volume);
			break;
		}
	}

	/*
	** insert in error list
	*/
	failedObject->log = log.errorList.number - 1;
	failed.insert(&failedObject);
}

#endif
