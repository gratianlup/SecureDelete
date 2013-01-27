// Copyright (c) 2005 Gratian Lup. All rights reserved.
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

#ifndef COMPRESSED_H
#define COMPRESSED_H

#include "wcontext.h"

/*
** definition for MAPPING_PAIR structure
*/
typedef struct __MAPPING_PAIR {
	unsigned __int64 vcn;
	unsigned __int64 lcn;
} MAPPING_PAIR;

/*
** definition for GET_RETRIEVAL_DESCRIPTOR structure
*/
typedef struct __GET_RETRIEVAL_DESCRIPTOR {
	unsigned long pairs;
	__int64       startVcn;
	MAPPING_PAIR  pair[1];
} GET_RETRIEVAL_DESCRIPTOR;


#define FILEMAP_LENGTH               16386
#define FSCTL_GET_RETRIEVAL_POINTERS 0x90073


/*
** definition for FILE_STREAM_INFORMATION structure
*/
#pragma pack(4)
struct FILE_STREAM_INFORMATION {
    unsigned long  nextEntry;
    unsigned long  nameLength;
    LARGE_INTEGER  size;
    LARGE_INTEGER  allocationSize;
    wchar_t        name[1];
};
#pragma pack()


/*
** definition for ADS_INFO structure
*/
typedef struct __ADS_INFO {
	wchar_t        path[MAX_PATH];
	LARGE_INTEGER size;
} ADS_INFO;


// ****************************************************************************
// *                                                                          *
// * wipeCompressedFile - wipe a compressed file ( see NTFS Documentation )   *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeCompressedFile(wchar_t *path) {
	int                       volumeNumber = path[0] - 'A';
	NTFS_VOLUME              *ntfsVolume;
	ULARGE_INTEGER            compressedSize;
	int                       status = STATUS_INVALID_PARAMETER;
	IO_STATUS_BLOCK           ioStatus;
	GET_RETRIEVAL_DESCRIPTOR *mappings;
	__int64                   map[FILEMAP_LENGTH];
	__int64                   startVcn = 0;
	unsigned long             result;

	/*
	** open volume
	*/
	if(ntfsVolumes[volumeNumber] == NULL) {
		ntfsVolumes[volumeNumber] = new NTFS_VOLUME;
	}

	ntfsVolume = ntfsVolumes[volumeNumber];
    
	if(ntfsVolume->volumeOpen == 0) {
		if(!ntfsVolumes[volumeNumber]->openVolume(path[0])) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize ntfsVolume %c",path[0]);
			return 0;
		}
	}
	
	if(stopped) {
		return ERRORCODE_STOPPED;
	}
	
	/*
	** update compressed file size
	*/
	compressedSize.LowPart = GetCompressedFileSize(wobject->path,&compressedSize.HighPart);
    
	if( compressedSize.LowPart == INVALID_FILE_SIZE && GetLastError() != NO_ERROR ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get file size for %s", wobject->path);
		return ERRORCODE_SIZE;
	}

	updateWobjectTotal(compressedSize.QuadPart * wowmethod->nSteps * 
                       (wowmethod->checkWipe ? 2 : 1));
	resetWobjectWiped();
	mappings = (GET_RETRIEVAL_DESCRIPTOR *)map;
	
	/*
	** get mappings pairs ( a sort of runlist )
	*/
	status = ntc.NtFsControlFile(woHandle,NULL,NULL,0,
								 &ioStatus,FSCTL_GET_RETRIEVAL_POINTERS,
								 &startVcn,sizeof(startVcn),mappings,
								 FILEMAP_LENGTH * sizeof(__int64));
	 
	while((status == STATUS_SUCCESS)         || 
          (status == STATUS_BUFFER_OVERFLOW) ||
		  (status == STATUS_PENDING)) {
		if(status == STATUS_PENDING) {
			WaitForSingleObject(ntfsVolume->volume,INFINITE);
		}
		
		if((ioStatus.status != STATUS_SUCCESS) && 
           (ioStatus.status != STATUS_BUFFER_OVERFLOW)) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't retrieve mapping pairs for file %s", wobject->path);
			return ERRORCODE_UNKNOWN;
		}
		
		/*
		** wipe each mapping pair
		*/
		startVcn = mappings->startVcn;
		for(int i = 0;i < mappings->pairs;i++) {
			if(stopped) {
				return ERRORCODE_STOPPED;
			}
			
			/*
			** check if mapping pair is valid
			*/
			if(mappings->pair[i].lcn == ((__int64) - 1)) { // -1
                // not valid (see NTFS Documentation - Runlist for more details)
				continue;
			}
			
			woPosition.QuadPart = mappings->pair[i].lcn * 
                                  volInfo.getVolumeClusterSize(volInfo.cv);
			woSize.QuadPart = (mappings->pair[i].vcn - startVcn) * 
                               volInfo.getVolumeClusterSize(volInfo.cv);
			startVcn = mappings->pair[i].vcn;
			
			/*
			** mapping pair
			*/
			result = wipeStream(ntfsVolume->volume);
            
			if(result != ERRORCODE_SUCCESS) {
				return result;
			}
		}
		
		if(status >= 0) {
			break; // no more mapping pairs
		}
		
		status = ntc.NtFsControlFile(woHandle, NULL, NULL, 0, &ioStatus,
									 FSCTL_GET_RETRIEVAL_POINTERS,&startVcn,
									 sizeof(__int64), mappings, 
                                     FILEMAP_LENGTH * sizeof(__int64));
	}
	
	/*
	** wipe successful ?
	*/
	if((status != STATUS_SUCCESS) &&
       (status != STATUS_INVALID_PARAMETER)) {
		return ERRORCODE_UNKNOWN;
	}

	return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * wipeAds - wipes all data streams of a file ( see NTFS Documentation )    *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeAds() {
	IO_STATUS_BLOCK          ioStatus;
	FILE_STREAM_INFORMATION *streamInfo;
	LLIST<ADS_INFO>          adsList;
	ADS_INFO                 ads;
	HANDLE                   adsHandle;
	unsigned long            result;
	int                      status;
	
	/*
	** get stream information
	*/
	status = ntc.NtQueryInformationFile(woHandle,&ioStatus,(FILE_STREAM_INFORMATION *)buffer,
										BUFFER_LENGTH,FileStreamInformation);
										
	if(status < 0) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get file streams");
		return ERRORCODE_UNKNOWN;
	}
	
	streamInfo = (FILE_STREAM_INFORMATION *)buffer;

	do {
		if(stopped) {
			return ERRORCODE_STOPPED;
		}

		/*
		** ensure the stream name ends with NULL
		** prevents some weired bug under Vista
		*/
		streamInfo->name[streamInfo->nameLength / sizeof(wchar_t)] = NULL;
		
		/*
		** check if this is the default stream
		** if not, add it to the ADS list
		*/
		if(wcscmp(streamInfo->name,L"::$DATA")) {
			ads.size = streamInfo->size;
			wcscpy(ads.path,wobject->path);
			wcscat(ads.path,streamInfo->name);
			
			/*
			** insert in ads list
			*/
			adsList.insert(&ads);
		}
		
		/*
		** more streams ?
		*/
		if(streamInfo->nextEntry) {
            unsigned char* nextEntry = (unsigned char *)streamInfo + streamInfo->nextEntry;
			streamInfo = (FILE_STREAM_INFORMATION *)(nextEntry);
		} 
		else {
		 streamInfo = NULL;
		}
	} while(streamInfo != NULL);
	
	/*
	** wipe founded streams
	*/
	for(int i = 0;i < adsList.number;i++) {
		if(stopped) {
			return ERRORCODE_STOPPED;
		}
		
		/*
		** open stream
		*/
        ads = adsList[i];
		adsHandle = CreateFile(ads.path,GENERIC_READ | GENERIC_WRITE, 0, NULL,
							   OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_NO_BUFFERING |
							   FILE_FLAG_SEQUENTIAL_SCAN, NULL);
							   
		if(adsHandle == INVALID_HANDLE_VALUE) {
			dbgPrint(__WFILE__,__LINE__,L"Couldn't open stream %s",ads.path);
			return ERRORCODE_ADS;
		}
		
		woPosition.QuadPart = 0;
		woSize.QuadPart = ads.size.QuadPart;
		
		/*
		** wipe stream
		*/
		result = wipeStream(adsHandle);
		CloseHandle(adsHandle);
		
		if(result != ERRORCODE_SUCCESS) { // error !
			return result;
		}
	}
	
	return ERRORCODE_SUCCESS;
}

#endif
