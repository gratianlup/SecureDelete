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

#ifndef WCONTEXT_H
#define WCONTEXT_H

#include <windows.h>
#include "logger.h"
#include "debug.h"
#include "random.h"
#include "linkedList.h"
#include "volumeInfo.h"
#include "fileHash.h"
#include "directoryHash.h"
#include "method.h"
#include "crc32.h"
#include "ntfsVolume.h"
#include "stack.h"
#include "ntfsContext.h"


/*
** definition for WOBJECT structure
** Stores an object that should be deleted
*/
typedef struct __WOBJECT {
	char                type;
	wchar_t            *path;
	wchar_t            *aux;
	unsigned int  wmethod;  // wipe method to use
	short unsigned int  woptions;
	
	__WOBJECT() {
		path = NULL;
	}
	~__WOBJECT() {
		if( path != NULL ) {
			delete[] path;
			path = NULL;
		}
	}
} WOBJECT;


/*
** definition for WEXTENDED_OBJECT structure
** Stores an object that should be deleted
*/
typedef struct __WEXTENDED_OBJECT {
	char               type;
	wchar_t            path[MAX_PATH];
	wchar_t            aux[MAX_PATH];
	unsigned int wmethod;
	short unsigned int woptions;
} WEXTENDED_OBJECT;

/*
** definition for WSMALL_OBJECT structure
** Used for failed objects
*/
#pragma pack(4)
typedef struct __WSMALL_OBJECT {
	char    type;
	int     log;
	wchar_t path[MAX_PATH];
} WSMALL_OBJECT;
#pragma  pack()


/*
** definition for WSTATUS structure
** Stores the status of the wipe context
*/
typedef struct __WSTATUS {
	int                context;
	__int64            objectIndex;
	char               stopped;          // stopped ?
	char               type;             // type of current object
	wchar_t            message[512];     // primary message
	wchar_t            auxMessage[512];  // secondary message
	__int64            totalBytes;       // total bytes to be wiped
	__int64            wipedBytes;       // bytes written so far
	__int64            clusterTipsBytes; // size of cluster tips wiped
	__int64            objectBytes;      // total bytes of current object
	__int64            objectWipedBytes; // bytes written for the current object
	short unsigned int steps;            // current wipe method steps
	short unsigned int step;             // current step
} WSTATUS;


/*
** definition for WOBPTIONS structure
** Stores the context options
*/
typedef struct __WOPTIONS {
	wchar_t     methodPath[MAX_PATH];
	int         useLogfile;
	int         appendLog;
	int         logSizeLimit;
	wchar_t     logFile[MAX_PATH];
	int         totalDelete;
	int         wipeUsedFileRecord;
	int         wipeUnusedFileRecord;
	int         wipeUnusedIndexRecord;
	int         wipeUsedIndexRecord;
	int         destroyFreeSpaceFiles;
	RNG_OPTIONS rngOptions;
} WOPTIONS;

/*
** write buffer maximum size
*/
#define BUFFER_LENGTH 1048576
#define MAXIMUM_CHILD_CONTEXTS 32

/*
** definition for types of WOBEJECTs
*/
#define WOTYPE_FILE         0
#define WOTYPE_FOLDER       1
#define WOTYPE_DRIVE        2
#define WOTYPE_PLUGIN       3
#define WOTYPE_MFT          4
#define WOTYPE_CLUSTER_TIPS 5


/*
** definitions for file options
*/
#define WOOPTION_WIPE_ADS      0x01
#define WOOPTION_WIPE_FILENAME 0x02


/*
** definitions for folder options
*/
#define WOOPTION_WIPE_SUBFOLDERS 0x100
#define WOOPTION_DELETE_FOLDERS  0x200
#define WOOPTION_MASK            0x400


/*
** definitions for free space options
*/
#define WOOPTION_WIPE_FREE_SPACE   0x01
#define WOOPTION_WIPE_CLUSTER_TIPS 0x02
#define WOOPTION_WIPE_MFT          0x04

/*
** definitions for error codes
*/
#define ERRORCODE_STOPPED             0
#define ERRORCODE_CRC                 1
#define ERRORCODE_WRITE               2
#define ERRORCODE_WMETHOD             3
#define ERRORCODE_NOTFOUND            4
#define ERRORCODE_ABORTED             5 // NOT USED ( replaced by STOPPED ) !!!
#define ERRORCODE_ACCESS              6
#define ERRORCODE_SIZE                7
#define ERRORCODE_ADS                 8
#define ERRORCODE_FILE_RECORD         9
#define ERRORCODE_CORRUPTED_FS       10
#define ERRORCODE_DIRECTORY_INDEX    11
#define ERRORCODE_FREE_SPACE_FOLDER  12
#define ERRORCODE_FOLDER             13
#define ERRORCODE_SUCCESS           254
#define ERRORCODE_UNKNOWN           255

/*
** MAIN CLASS
*/
class WCONTEXT {
public:
	unsigned long    id;
	CRITICAL_SECTION cs;
	LOGGER           log;
	RNG				 rng;     // random number generator
	int              stopped;
	HANDLE           stoppedEvent;
	WOPTIONS         woptions;
	
	/*
	** support for child contexts
	*/
	WCONTEXT *parent;
	HANDLE    childrenThreads[MAXIMUM_CHILD_CONTEXTS];
	WCONTEXT *children[MAXIMUM_CHILD_CONTEXTS];
	int       childrenNumber;
	
	/*
	** the write buffer
	*/
	unsigned char *buffer;
	
	/*
	** volume information
	*/
	VOLUME_INFO volInfo;
	
	/*
	** system information
	*/
	int underWindows2000;
	
	/*
	** wipe methods
	*/
	LLIST<WMETHOD *>  wmethods;    // all wipe methods
	WMETHOD          *wowmethod;   // wipe object wipe method
	int               wowmethodId; // ID of current wipe method
	
	/*
	** current object
	*/
	WOBJECT        *wobject;
	HANDLE          woHandle;
	ULARGE_INTEGER  woSize;
	ULARGE_INTEGER  woPosition;
	int             woMethod;
	int             fastWrite;
	
	LLIST<WOBJECT *>       wobjects; // objects to wipe !!!
	LLIST<WSMALL_OBJECT *> failed;   // list with objects that couldn't be wiped

	/*
	** for compressed files and ADS
	*/
	NTFS_CONTEXT ntc;

	/*
	** Total Delete
	*/
	FILE_HASH           fileHash;
	DIRECTORY_HASH      dirHash[27];
	NTFS_VOLUME        *ntfsVolumes[27];
	int                 searchVolumes[27];
	NTFS_VOLUME        *currentVolume;
	FILE_RECORD         fileRecord;
	ULARGE_INTEGER      fileRecordOffset;
	unsigned char       fileRecordBuffer[32768];
	LLIST<FILE_INFO *>  folderFileInfo;
	
	/*
	** test if data was correctly written ( CRC32 )
	*/
	unsigned long crc1; // crc computed form memory data
	unsigned long crc2; // crc computed from disc data, should be equal with crc1
	
	/*
	** wipe status
	*/
	WSTATUS wstatus;
	
	/*
	** ------------------------------------------------------------------------
	*/
	WCONTEXT() {
		buffer = NULL;
		wobject = NULL;
	}
	~WCONTEXT() {
		resetObject();
	}
	
	void setOptions(WOPTIONS *options);
	int  allocateBuffer();
	void deallocateBuffer();
	int  loadMethods();
	int  initialize();
	void startWipe();
	void stop();
	void stopChildren();
	void waitChildren();
	void destroyChildren();
	void getChildrenErrors();
	int  getFileSize(wchar_t *file,__int64 &fileSize);
	int  getDriveFreeSpace(wchar_t drive,__int64 &freeSpace);
	int  setWOMethod(unsigned long id);
	int  insertObject(WEXTENDED_OBJECT *object);
	void insertFailedObject(WOBJECT *object,int logPosition);
	void insertFailedFreeSpaceObject(wchar_t *volumes,int volume,int logPosition);
	int  insertFile(WEXTENDED_OBJECT *object);
	int  insertFolder(wchar_t *folder,WEXTENDED_OBJECT *object);
	void resetObject();
	
	/*
	** functions for error handling
	*/
	void handleFileError(wchar_t *file,int errorCode);
	void handleFolderError(wchar_t *folder,int errorCode);
	void handleFreeSpaceError(wchar_t volume,int errorCode);
	
	/*
	** functions for memory filling
	*/
	int fillBuffer(unsigned char *pattern,long len,unsigned char patternLength);
	int setComplement(long len);
	int fillStepData(long length,int step);
	
	/*
	** functions for obtaining Windows version
	*/
	int isWindows2000();
	
	/*
	** functions for wiping data streams
	*/
	__int64 roundToCluster(__int64 dataSize);
	int     computeStreamCrc(HANDLE handle,__int64 streamSize);
	int     wipeStream(HANDLE handle);
	
	/*
	** functions for total delete
	*/
	void appendString(wchar_t *dst,wchar_t *src,int len);
	int  searchObjectEntries();
	int  searchFileRecords(wchar_t volume);
	
	/*
	** functions for updating wipe status
	*/
	void updateMessage(wchar_t *format,...);
	void updateAuxMessage(wchar_t *format,...);
	void updateWobjectTotal(__int64 n);
	void updateWobjectWiped(__int64 n);
	void updateWobjectWiped2(__int64 n);
	void setWobjectWiped(__int64 n);
	void resetWobjectWiped();
	void updateWipedBytes(__int64 n);
	void updateClusterTips(int n);
	void updateStepInfo(int step,int totalSteps);
	void setObjectIndex();
	
	/*
	** functions for wiping files
	*/
	int  showMessageBox(wchar_t *file,unsigned long fileAttributes);
	void resetFileDateTime();
	void wipeFilename(wchar_t *path);
	int  wipeFile(wchar_t *file,int wipeFileRecord);
	
	/*
	** functions for wiping compressed files and ADS
	*/
	int wipeCompressedFile(wchar_t *path);
	int wipeAds();
	
	/*
	** function for wiping folders
	*/
	int wipeFolder(wchar_t *folder);
	
	/*
	** functions for low level disk access
	*/
	int  openDisk(int disk);
	int  openDiskReadOnly(int disk);
	void closeDisk(int disk);
	
	/*
	** functions for wiping NTFS file records
	*/
	int     wipeFileInfo(FILE_INFO *fileInfo,wchar_t *path);
	int     wipeNTFSFileInfo(FILE_INFO *fileInfo,wchar_t *path);
	int     wipeFATFileInfo(FILE_INFO *fileInfo,wchar_t *path);
	int     readFileRecord(__int64 referenceNumber,int volume);
	void    fillSignature(unsigned char *signature,int position,int bufferLength);
	void    clearFixups(unsigned char *fixups,int position,int bufferLength);
	__int64 roundUpToSector(__int64 dataSize,int sectorSize);   // NOT USED !!!
	__int64 roundDownToSector(__int64 dataSize,int sectorSize); // NOT USED !!!
	int     wipeUsedFileRecord(FILE_RECORD *fileRecord,FILE_INFO *fileInfo);
	int     wipeUnusedFileRecord(FILE_RECORD *fileRecord,FILE_INFO *fileInfo);
	int     wipeMFT(wchar_t volume);
	
	/*
	** functions for wiping free space
	*/
	int      deleteFolder(wchar_t  *folder);
	int      destroyFreeSpaceFolder(wchar_t *folder);
	void     decompressFolder(wchar_t *folder);
	void     insertFolderInFileHash(wchar_t *folder,LLIST<wchar_t *> *fileList);
	void     genereateRandomFilename(wchar_t *fileName,int length);
	wchar_t *getNextVolumes(wchar_t *volumes,int &length,int *disks);
	int      wipeMultipleFreeSpace(wchar_t *volumes);
	int      wipeFreeSpace(wchar_t volume);
	int      wipeClusterTip(wchar_t *file);
	int      wipeClusterTipsRecursive(wchar_t *folder);
	int      countFilesInFolder(wchar_t *folder);
};


// ****************************************************************************
// *                                                                          *
// * setOptions - sets the wipe options (should be called before initializing)*
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::setOptions(WOPTIONS *options) {
	memcpy(&woptions,options,sizeof(WOPTIONS));
}


// ****************************************************************************
// *                                                                          *
// * allocateBuffer - allocates memory for the write buffer                   *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::allocateBuffer() {
	if( buffer != NULL ) {
		memset(buffer,0,BUFFER_LENGTH);
		VirtualFree(buffer,0,MEM_RELEASE);
		buffer = NULL;
	}
	
	/*
	** allocate memory for buffer
	*/
	buffer = (unsigned char *)VirtualAlloc(NULL,BUFFER_LENGTH + 1,
                                           MEM_COMMIT,PAGE_READWRITE);	
	return buffer != NULL;
}


// ****************************************************************************
// *                                                                          *
// * isWindows2000 - checks if the program is running under Windows 2000 or   *
// * higher ( XP,2003,Vista )                                                 *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::isWindows2000() {
	OSVERSIONINFO versionInfo;
	
	versionInfo.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&versionInfo);
	
	if( versionInfo.dwMajorVersion >= 5 ) {
		return 1;
	}
	
	return 0;
}


// ****************************************************************************
// *                                                                          *
// * deallocateBuffer - deallocates memory used by write buffer               *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::deallocateBuffer() {
	if( buffer != NULL ) {
		memset(buffer,0,BUFFER_LENGTH);
		VirtualFree(buffer,0,MEM_RELEASE);
		buffer = NULL;
	}
}


// ****************************************************************************
// *                                                                          *
// * loadMethods - loads all wipe methods                                     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::loadMethods() {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	wchar_t          wmethodPath[MAX_PATH];
	WMETHOD         *wmethod;
	
	/*
	** load only *.sdm files
	*/
	wcscpy(path,woptions.methodPath);
	
	if( path[wcslen(path) - 1] != L'\\' ) {
		/*
		** append \
		*/
		wcscat(path,L"\\");
	}
	
	wcscat(path,L"*.sdm");
	
	/*
	** find first file
	*/
	find = FindFirstFile(path,&ffData);
	
	if( find == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't search methods directory");
		return 0;
	}
	
	/*
	** ensure the wmethods list is empty
	*/
	wmethods.clear();
	
	do {
		if( !(ffData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) ) { // no folders
			wmethod = new WMETHOD;
			
			/*
			** create method path
			*/
			wcscpy(wmethodPath,woptions.methodPath);
			
			if( wmethodPath[wcslen(wmethodPath) - 1] != L'\\' ) {
				/*
				** append \
				*/
				wcscat(wmethodPath,L"\\");
			}
			
			wcscat(wmethodPath,ffData.cFileName);
			
			if( !wmethod->loadMethod(wmethodPath) ) {
				log.addToLog(SEVERITY_HIGH,L"Failed to load wipe method %s. Wipe stopped.",wmethodPath);
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Failed to load wmethod %s",wmethodPath);
				return 0;
			}
			
			/*
			** random buffer if shuffle is set
			*/
			if( wmethod->shuffle ) {
				rng.getRandom(buffer,wmethod->nSteps);
				wmethod->setRandom(buffer,wmethod->nSteps);
			}
			
			/*
			** insert method in list
			*/
			wmethods.insert(&wmethod);
		}
	} while( FindNextFile(find,&ffData) );
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * initialize - initializes the context ( including log,random,             *
// * volume info, and write buffer )                                          *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::initialize() {
	/*
	** initialize LOGGER
	*/
	log.useLogfile = woptions.useLogfile;
	log.append = woptions.appendLog;
	log.sizeLimit = woptions.logSizeLimit;

	if( woptions.useLogfile ) {
		log.openLog(woptions.logFile);
	}

	/*
	** get Windows version
	*/
	underWindows2000 = isWindows2000();

	/*
	** allocate the write buffer
	*/
	if( !allocateBuffer() ) {
		log.addToLog(SEVERITY_HIGH,L"Buffer allocation failed.");
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Buffer allocation failed");
		return 0;
	}
	
	/*
	** initialize critical section and create stopped event
	*/
	InitializeCriticalSection(&cs);
	stoppedEvent = CreateEvent(NULL,1,0,NULL);
	
	/*
	** initialize random generator
	*/
	rng.setOptions(woptions.rngOptions);
	if( !rng.initRNG() ) {
		log.addToLog(SEVERITY_HIGH,L"Random number generator initialization failed.");
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Random number generator initialization failed.");
		return 0;
	}
		
	/*
	** initialize volume info
	*/
	volInfo.underWindows2000 = underWindows2000;
	if( !volInfo.createVolumeList() ) {
		log.addToLog(SEVERITY_HIGH,L"Couldn't retrieve volume information.");
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't retrieve volume information.");
		return 0;
	}
	
	for(int i = 0;i < 27;i++) {
		searchVolumes[i] = 0;
		ntfsVolumes[i] = NULL;
	}
	
	/*
	** load wipe methods
	*/
	if( !loadMethods() ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Wipe methods not loaded.");
		return 0;
	}
	
	/*
	** initialize NT native functions ( used when wiping compressed files )
	*/
	if( !ntc.initialize() ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: NT native functions not loaded.");
		return 0;
	}
	
	/*
	** other initializations
	*/
	crc1 = crc2 = 0;
	memset(&wstatus,0,sizeof(WSTATUS));
	childrenNumber = 0;
    return 1;
}


// ****************************************************************************
// *                                                                          *
// * stop - stops wipe                                                        *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::stop() {
	EnterCriticalSection(&cs);	
	stopped = 1;	
	LeaveCriticalSection(&cs);
}



// ****************************************************************************
// *                                                                          *
// * stopChildren - stops wipe on children (used for free space)              *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::stopChildren() {
	WCONTEXT *child;	
	
	for(int i = 0;i < childrenNumber;i++) {
		child = children[i];
		child->stop();
	}
}


// ****************************************************************************
// *                                                                          *
// * waitChildren - waits until all children finish wiping                    *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::waitChildren() {
	HANDLE childrenStoppedEvents[MAXIMUM_CHILD_CONTEXTS];
	WCONTEXT *child;

	for(int i = 0;i < childrenNumber;i++) {
		child = children[i];
		childrenStoppedEvents[i] = child->stoppedEvent;
	}

	/*
	** wait all children
	*/
	WaitForMultipleObjects(childrenNumber,childrenStoppedEvents,1,INFINITE);
}

inline void WCONTEXT::getChildrenErrors() {
	WCONTEXT      *child;
	WSMALL_OBJECT *failedChildObject;
	WIPE_ERROR     wipeError;

	for(int i = 0;i < childrenNumber;i++) {
		child = children[i];
				
		for(int j = 0;j < child->failed.number;j++) {
			if( child->failed[j]->log < 0 ) {
                continue;
            }
					
			/*
			** copy failed object
			*/
			failedChildObject = new WSMALL_OBJECT;
			memcpy(failedChildObject,child->failed[j],sizeof(WSMALL_OBJECT));

			wipeError = child->log.errorList[child->failed[j]->log];
			log.errorList.insert(&wipeError);
			failed.insert(&failedChildObject);
		}
	}
}


// ****************************************************************************
// *                                                                          *
// * destroyChildren - destroys all children (should be called after          *
// * stopChildren and waitChildren)                                           *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::destroyChildren() {
	WCONTEXT *child;	
	
	/*
	** destroy each child and free memory
	*/
	EnterCriticalSection(&cs);
	
	for(int i = 0;i < childrenNumber;i++) {
		child = children[i];
		
		// update wiped bytes
		updateWipedBytes(child->wstatus.totalBytes);

		child->resetObject();
		delete child;
		
		children[i] = NULL;
		childrenNumber = 0;
		
		CloseHandle(childrenThreads[i]);
	}
	
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * getFileSize - gets the file size ( compressed file size for compressed ) *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::getFileSize(wchar_t *file,__int64 &fileSize) {
	HANDLE         fileHandle;
	ULARGE_INTEGER size;
	int            compressed = 0;
	unsigned long  attributes;
	
	/*
	** check if file is compressed
	*/
	attributes = GetFileAttributes(file);

	if( attributes == INVALID_FILE_ATTRIBUTES ) {
		dbgPrint(__WFILE__,__LINE__,L"Couldn't get attributes for file %s",file);
		return 0;
	}
	
	if( (attributes & FILE_ATTRIBUTE_COMPRESSED) || 
        (attributes & FILE_ATTRIBUTE_ENCRYPTED)  ||
		(attributes & FILE_ATTRIBUTE_SPARSE_FILE) ) {
		compressed = 1;	
	}
	
	/*
	** open the file
	*/
	fileHandle = CreateFile(file,GENERIC_READ, 0,NULL,
							OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
							
	if( fileHandle == INVALID_HANDLE_VALUE ) {
		return 0;
	}

	/*
	** get the file size
	*/
	if( !compressed ) {
		size.LowPart = GetFileSize(fileHandle,&size.HighPart);
		
        if( size.LowPart == INVALID_FILE_SIZE && GetLastError() != NO_ERROR ) {
			return 0;
		}
	}
	else {
		size.LowPart = GetCompressedFileSize(file,&size.HighPart);

		if( size.LowPart == INVALID_FILE_SIZE && GetLastError() != NO_ERROR ) {
			return 0;
		}
	}
	
	fileSize = size.QuadPart;
	CloseHandle(fileHandle);	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * setWOMethod - sets current wipe method (only if not set)                 *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::setWOMethod(unsigned long id) {
	if( id == wowmethodId ) {
		return 1; // method already set
	}
	
	for(int i = 0;i < wmethods.number;i++) {
		if( wmethods[i]->id == id ) {
			wowmethod = wmethods[i];
			wowmethodId = wowmethod->id;
			return 1;
		}
	}
	
	return 0; // method not found
}


// ****************************************************************************
// *                                                                          *
// * insertObject - inserts a wipe object (file/folder/drive/*list)           *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::insertObject(WEXTENDED_OBJECT *object) {
	__int64 size;
	
	if( object == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for insertObject");
		return 0;
	}
	
	switch( object->type ) {
		case WOTYPE_FILE: {
			return insertFile(object);
			break;
		}
		case WOTYPE_FOLDER: {
			if( !insertFolder(object->path,object) ) {
				return 0;
			}
			
			return insertFile(object);
			
			break;
		}
		case WOTYPE_DRIVE: {
			WOBJECT *wobject;

			/*
			** allocate new WOBJECT
			*/
			wobject = new WOBJECT;
			wobject->path = new wchar_t[wcslen(object->path) + 1];

			wcscpy(wobject->path,object->path);
			wobject->type = object->type;
			wobject->wmethod = object->wmethod;
			wobject->woptions = object->woptions;
			
			__int64 totalFreeSpace = 0;
			int count = wcslen(wobject->path);

			for(int i = 0;i < count;i++) {
				if( !getDriveFreeSpace(wobject->path[i],size) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get free space for drive %c",wobject->path[0]);
					return 0;
				}

				totalFreeSpace += size;
			}
			
			/*
			** insert drive in the wobjects list
			*/
			wobjects.insert(&wobject);
			
			/*
			** update statistics
			*/
			if( !setWOMethod(wobject->wmethod) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method %d",wobject->wmethod);
				return 0;
			}

			wstatus.totalBytes += totalFreeSpace * wowmethod->nSteps;

			// double space to wipe if "check wipe" is enabled
			if(wowmethod->checkWipe) {
				wstatus.totalBytes *= 2;
			}
			
			return 1;
		}
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * insertFailedObject - insert an object in the failed objects list         *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::insertFailedObject(WOBJECT *object,int logPosition) {
	WSMALL_OBJECT *failedObject = new WSMALL_OBJECT;
	
	failedObject->type = object->type;
	wcscpy(failedObject->path,object->path);
	failedObject->log = logPosition;	
	failed.insert(&failedObject);
}


inline void WCONTEXT::insertFailedFreeSpaceObject(wchar_t *volumes,int volume,int logPosition) {
	WSMALL_OBJECT *failedObject = new WSMALL_OBJECT;

	failedObject->type = WOTYPE_DRIVE;
	failedObject->path[0] = volumes[volume];
	failedObject->path[1] = NULL;
	failedObject->log = logPosition;
	failed.insert(&failedObject);
}


// ****************************************************************************
// *                                                                          *
// * insertFile - inserts a file object                                       *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::insertFile(WEXTENDED_OBJECT *object) {
	WOBJECT *wobject;
	__int64 size;
	
	if( object == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"Invalid parameters for insertFile");
		return 0;
	}
	
	/*
	** allocate new WOBJECT
	*/
	wobject = new WOBJECT;
	wobject->path = new wchar_t[wcslen(object->path) + 1];
	wobject->aux = new wchar_t[wcslen(object->aux) + 1];
	
	wcscpy(wobject->path,object->path);
	wcscpy(wobject->aux,object->aux);
	wobject->type = object->type;
	wobject->wmethod = object->wmethod;
	wobject->woptions = object->woptions;
	
	/*
	** insert object in file hash ?
	*/
	if( woptions.wipeUsedFileRecord == 1 && woptions.totalDelete == 1 ) {
		fileHash.insertFile(wobject->path,wcslen(wobject->path));
		dirHash[wobject->path[0] - 'A'].insertFile(wobject->path,wcslen(wobject->path));
		searchVolumes[wobject->path[0] - 'A'] = 1;
	}

	/*
	** insert wobject in list
	*/
	wobjects.insert(&wobject);

	if( wobject->type != WOTYPE_FILE ) {
		return 1; // folders have no size
	}
	
	/*
	** get the file size
	*/
	if( !getFileSize(wobject->path,size) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get file size ( %s )",wobject->path);
		return 0;
	}
	
	/*
	** update statistics
	*/
	if( !setWOMethod(wobject->wmethod) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method %d",wobject->wmethod);
		return 0;
	}
    
	wstatus.totalBytes += size * wowmethod->nSteps;		
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * insertFolder - insert a folder object                                    *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::insertFolder(wchar_t *folder,WEXTENDED_OBJECT *object) {
	WIN32_FIND_DATA  ffData;
	HANDLE           find;
	wchar_t          path[MAX_PATH];
	int              folderStrLength;
	WEXTENDED_OBJECT wexobject;
	__int64          fileSize;
    
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
	if( object->woptions & WOOPTION_MASK ) {
		wcscat(path,object->aux);
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
				wcscpy(wexobject.path,folder);
				
				if( wexobject.path[folderStrLength - 1] != L'\\' ) {
					/*
					** append \
					*/
					wcscat(wexobject.path,L"\\");
				}
				
				wcscat(wexobject.path,ffData.cFileName);
				
				/*
				** get the file size
				*/
				if( !getFileSize(wexobject.path,fileSize) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get file size ( %s )",wexobject.path);
					continue;
				}

				/*
				** update statistics
				*/
				if( !setWOMethod(object->wmethod) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set wipe method %d",object->wmethod);
					continue;
				}
				wstatus.totalBytes += fileSize * wowmethod->nSteps;

				/*
				** insert file in file hash ?
				*/
				if( woptions.totalDelete == 1 ) {
					fileHash.insertFile(wexobject.path,wcslen(wexobject.path));
					dirHash[wexobject.path[0] - 'A'].insertFile(wexobject.path,wcslen(wexobject.path));
					searchVolumes[wexobject.path[0] - 'A'] = 1;
				}
			}
			else if( (object->woptions & WOOPTION_WIPE_SUBFOLDERS) && 
                     (object->woptions & WOOPTION_MASK) == 0 &&
					 !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == 0) && // not . and ..
					 !(ffData.cFileName[0] == '.' && ffData.cFileName[1] == '.' && 
                       ffData.cFileName[2] == 0) ) {
				wcscpy(wexobject.path,folder);
				
				if( wexobject.path[folderStrLength - 1] != L'\\' ) {
					/*
					** append \
					*/
					wcscat(wexobject.path,L"\\");
				}
				wcscat(wexobject.path,ffData.cFileName);
				
				insertFolder(wexobject.path,object);
				
				/*
				** insert folder in file hash ?
				*/
				if( woptions.totalDelete == 1 ) {
					fileHash.insertFile(wexobject.path,wcslen(wexobject.path));
					dirHash[wexobject.path[0] - 'A'].insertFile(wexobject.path,wcslen(wexobject.path));
					searchVolumes[wexobject.path[0] - 'A'] = 1;
				}
			}
		} while( FindNextFile(find,&ffData) );
	}
	
	FindClose(find);

	/*
	** if a mask is used, the folder needs to be rescanned in order to traverse
	** the subfolders
	*/
	if( (object->woptions & WOOPTION_WIPE_SUBFOLDERS) && (object->woptions & WOOPTION_MASK) ) {
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
					wcscpy(wexobject.path,folder);

					if( wexobject.path[folderStrLength - 1] != L'\\' ) {
						/*
						** append \
						*/
						wcscat(wexobject.path,L"\\");
					}

					wcscat(wexobject.path,ffData.cFileName);

					insertFolder(wexobject.path,object);
					
					/*
					** insert folder in file hash ?
					*/
					if( woptions.totalDelete == 1 ) {
						fileHash.insertFile(wexobject.path,wcslen(wexobject.path));
						dirHash[wexobject.path[0] - 'A'].insertFile(wexobject.path,wcslen(wexobject.path));
						searchVolumes[wexobject.path[0] - 'A'] = 1;
					}
				}
			}
		} while( FindNextFile(find,&ffData) );
		
		FindClose(find);
	}

	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getDriveFreeSpace                                                        *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::getDriveFreeSpace(wchar_t drive,__int64 &freeSpace) {
	ULARGE_INTEGER freeBytes;
	ULARGE_INTEGER totalBytes;
	ULARGE_INTEGER totalFreeBytes;
	wchar_t driveName[4];
	
	if( drive < 'A' || drive > 'Z' ) {
		return 0;
	}
	
	driveName[0] = drive; driveName[1] = ':';
	driveName[2] = '\\';  driveName[3] = NULL;
	
	if( !GetDiskFreeSpaceEx(driveName,&freeBytes,&totalBytes,&totalFreeBytes) ) {
		return 0;
	}
	
	freeSpace = totalFreeBytes.QuadPart;	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - clears memory and sets the object in a clean state         *
// *                                                                          *
// ****************************************************************************
void WCONTEXT::resetObject() {
	rng.disableRNG();	
	deallocateBuffer();
    DeleteCriticalSection(&cs);

	/*
	** delete wipe methods
	*/
	for(int i = 0;i < wmethods.number;i++) {
		delete wmethods[i];
	}

	wmethods.clear();
		
	/*
	** delete wipe objects
	*/
	for(int i = 0;i < wobjects.number;i++) {
		delete wobjects[i];
	}
	wobjects.clear();
		
	/*
	** delete failed objects
	*/
	for(int i = 0;i < failed.number;i++) {
		delete failed[i];
	}

    failed.clear();
}

#endif
