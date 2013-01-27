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

#ifndef ENTRY_SEARCH_H
#define ENTRY_SEARCH_H

#include "wcontext.h"
#include "directoryIndex.h"
#include "debug.h"

/*
** definition for NTFS_ENTRY_INFO structure
*/
typedef struct __NTFS_ENTRY_INFO {
	int     referenceNumber;
	int     level;
	wchar_t folder[MAX_PATH + 10];
} NTFS_ENTRY_INFO;


// ****************************************************************************
// *                                                                          *
// * searchObjectEntries - searches the file records on all drives            *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::searchObjectEntries() {
	/*
	** for all used volumes
	*/
	for(int i = 0;i < 27;i++) {
		if( searchVolumes[i] == 1 ) {
			/*
			** get volume type
			*/
			if( volInfo.getVolumeType(i) == FSTYPE_NTFS ) {
				if( !searchFileRecords((wchar_t)('A' + i)) ) {
					log.addToLog(SEVERITY_HIGH,L"Failed to search file record on NTFS drive %c",(wchar_t)i);
					return 0;
				}
			}
		}
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * appendString - append a string ( representing a path ) to destination    *
// * Optimized in assembly for speed                                          *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::appendString(wchar_t *dst, wchar_t *src, int len) {
	_asm {
		mov ecx,len       // len
		test ecx,ecx      // test if len == 0
		jz noLength
		mov esi,dst       // dst
		mov edx,src       // src
		
		/*
		** find NULL character
		*/
	nullLoopStart:
		mov eax,[esi]
		cmp ax,0          // NULL ?
		je charNull       // NULL char found, exit loop
		
		add esi,2         // increment dst
		jmp nullLoopStart // continue the loop
		
	charNull:
		/*
		** should we append '\' ?
		*/
		mov eax,[esi - 2] 
		cmp al,92
		je endSlashFound
		
		mov al,92         // append '\'
		mov [esi],ax
		add esi,2         // increment dst
		
	endSlashFound:
		/*
		** copy 4 bytes each step
		*/
		cmp ecx,2
		jl noFourMultiple
		
		mov eax,[edx]     // copy *src
		mov [esi],eax     // to *dst
		sub ecx,2         // 2 characters were copied
		add esi,4         // increment dst by 2 characters
		add edx,4         // increment src by 2 characters
		
		jmp endSlashFound
		
	noFourMultiple:
		/*
		** copy 2 bytes each step
		*/
		cmp ecx,0
		je finish
		
		mov ax,[edx]
		mov [esi],ax
		sub ecx,1
		add esi,2
		add edx,2
		
		jmp noFourMultiple
		
	finish:
		xor eax,eax
		mov [esi],ax      // NULL
		
	noLength:
	}
}


// ****************************************************************************
// *                                                                          *
// * searchFileRecords - searches the file records of all required files      *
// * ( that should be wiped ) on the volume                                   *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::searchFileRecords(wchar_t volume) {
	int                     volumeNumber = volume - 'A';
	Stack<NTFS_ENTRY_INFO>  stack;
	NTFS_ENTRY_INFO         entryInfo,auxEntryInfo;
	NTFS_VOLUME            *ntfsVolume;
	ULARGE_INTEGER          offset;
	FILE_RECORD             fileRecord;
	FILE_RECORD             auxRecord;
	DIRECTORY_INDEX         dirIndex;
	ATTRIBUTE_HEADER       *attrHeader;
	RUNLIST                 indexRunlist;
	int                     indexAllocationSize;
	INDEX_ENTRY            *file;
	FILE_INFO              *fileInfo;
	
	if( volume < 'A' || volume > 'Z' ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for searchFileRecords %c",volume);
		return 0;
	}
	
	if( ntfsVolumes[volumeNumber] == NULL ) {
		ntfsVolumes[volumeNumber] = new NTFS_VOLUME;
		if( !ntfsVolumes[volumeNumber]->initialize(volume) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize ntfsVolume %c",volume);
			return 0;
		}
	}
	ntfsVolume = ntfsVolumes[volumeNumber];
	
	/*
	** insert root directory (.) in stack
	*/
	entryInfo.level = 0;
	entryInfo.folder[0] = volume; entryInfo.folder[1] = ':';
	entryInfo.folder[2] = '\\';   entryInfo.folder[3] = NULL;
	entryInfo.referenceNumber = 5; // root directory .
	stack.insert(entryInfo);
	
	/*
	** set the options for the file record and directory index
	*/
	fileRecord.volume = ntfsVolume->volume;
	fileRecord.recordSize = ntfsVolume->fileRecordSize;
	fileRecord.sectorSize = ntfsVolume->sectorSize;
	fileRecord.clusterSize = ntfsVolume->clusterSize;
	fileRecord.MFTRunlist = &ntfsVolume->mftRunlist;
	
	auxRecord.volume = ntfsVolume->volume;
	auxRecord.recordSize = ntfsVolume->fileRecordSize;
	auxRecord.sectorSize = ntfsVolume->sectorSize;
	auxRecord.clusterSize = ntfsVolume->clusterSize;
	auxRecord.MFTRunlist = &ntfsVolume->mftRunlist;
	
	dirIndex.volume = ntfsVolume->volume;
	dirIndex.sectorSize = ntfsVolume->sectorSize;
	dirIndex.clusterSize = ntfsVolume->clusterSize;
	
	/*
	** start searching...
	*/
	while( !stack.isEmpty() ) {
		/*
		** extract folder from the stack
		*/
		stack.extract(&entryInfo);
		
		/*
		** load file record
		*/
		fileRecord.resetObject();
		offset.QuadPart = ntfsVolume->getFileRecordOffset(entryInfo.referenceNumber & 0xffffffffffff);
        
		if( offset.QuadPart == -1 ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: File record not found %s",entryInfo.folder);
			return 0;
		}
        
		fileRecord.referenceNumber = entryInfo.referenceNumber;
		
		if( !fileRecord.readRecord(offset) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: File record %d couldn't be read %s",entryInfo.folder);
			return 0;
		}
		
		/*
		** reset directory index
		*/
		dirIndex.resetObject();
		
		/*
		** load INDEX_ROOT
		*/
		attrHeader = fileRecord.getNextAttributeHeader(AttributeIndexRoot);
        
		if( attrHeader == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load INDEX_ROOT for %s",entryInfo.folder);
			return 0;
		}
		
		/*
		** set INDEX_ROOT
		*/
		if( !dirIndex.setIndexRoot((INDEX_ROOT *)fileRecord.getAttributeBody()) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set INDEX_ROOT for %s",entryInfo.folder);
			return 0;
		}
		
		/*
		** large folder ?
		*/
		if( dirIndex.getIndexType() == INDEXTYPE_LARGE ) {
			attrHeader = fileRecord.getNextAttributeHeader(AttributeIndexAllocation);
            
			if( attrHeader == NULL ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load INDEX_ALLOCATION for %s",entryInfo.folder);
				return 0;
			}
			
			indexAllocationSize = attrHeader->attributes.nonResident.allocatedSize;
			
			/*
			** load runlist
			*/
			indexRunlist.resetObject();

			if( !fileRecord.getAttributeRunlist(indexRunlist) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load index allocation runlist for %s",entryInfo.folder);
				return 0;
			}
			
			attrHeader = fileRecord.getNextAttributeHeader(AttributeBitmap);
			
            if( attrHeader == NULL ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load BITMAP for %s",entryInfo.folder);
				return 0;
			}
			
			if( !dirIndex.setIndexAllocationAndBitmap(&indexRunlist,(unsigned char *)fileRecord.getAttributeBody(),
													  indexAllocationSize) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize LARGE directory for %s",entryInfo.folder);				  
				return 0;
			}
		}
		
		/*
		** get the files !
		*/
 		while( (file = dirIndex.getNextFilename()) != NULL ) {			
			// file
			auxEntryInfo = entryInfo;
			auxEntryInfo.level++;
			auxEntryInfo.referenceNumber = file->MFTReference;

			/*
			** append folder name
			*/
			appendString(auxEntryInfo.folder,file->fileName,file->filenameLength);
			fileInfo = fileHash.findFile(auxEntryInfo.folder,wcslen(auxEntryInfo.folder));
            
			if( fileInfo != NULL ) {
				fileInfo->aux = file->MFTReference;
				
				if( file->fileFlags & 0x10000000 ) {
					/*
					** things are getting complicated... :(
					** the index allocation must be saved, because it will, probably,
					** be destroyed by NTFS when it deletes the files in the folder
					*/
					offset.QuadPart = ntfsVolume->getFileRecordOffset(fileInfo->aux & 0xffffffffffff);
                    
					if( offset.QuadPart == -1 ) {
						dbgPrint(__WFILE__,__LINE__,L"ERROR: File record not found %s",entryInfo.folder);
						return 0;
					}
					
					auxRecord.referenceNumber = fileInfo->aux & 0xffffffffffff;
                    
					if( !auxRecord.readRecord(offset) ) {
						dbgPrint(__WFILE__,__LINE__,L"ERROR: File record couldn't be read %s",entryInfo.folder);
						return 0;
					}
					
					/*
					** get index allocation ( only for large directory )
					*/
					attrHeader = auxRecord.getNextAttributeHeader(AttributeIndexAllocation);
                    
					if( attrHeader != NULL ) {
						/*
						** extract runlist
						*/
						fileInfo->indexAllocationSize = attrHeader->attributes.nonResident.allocatedSize;
						fileInfo->runlistLength = attrHeader->length - 
                                                  attrHeader->attributes.nonResident.datarunOffset;

						/*
						** allocate memory for runlist
						*/
						fileInfo->indexAllocation = new unsigned char[fileInfo->runlistLength];

						/*
						** copy runlist
						*/
						memcpy(fileInfo->indexAllocation,
                               auxRecord.getAttributeRunlistAsByte(),
                               fileInfo->runlistLength);
					}
					else {
						fileInfo->indexAllocation = NULL;
						fileInfo->runlistLength = 0;
						fileInfo->indexAllocationSize = 0;
					}
					
					auxRecord.resetObject();
				}
			}
			
			if( file->fileFlags & 0x10000000 ) { // folder
				if( dirHash[volumeNumber].findFolder(entryInfo.level,file->fileName,file->filenameLength) ) {
					stack.insert(auxEntryInfo);
				}
			}
		}
	}
	
	return 1;
}

#endif
