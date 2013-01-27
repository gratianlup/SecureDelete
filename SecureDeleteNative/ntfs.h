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

#ifndef NTFS_H
#define NTFS_H

#include "wcontext.h"

/*
** definition for FILE_RECORD_CACHE structure
*/
typedef struct __FILE_RECORD_CACHE {
	__int64       referenceNumber;
	LARGE_INTEGER offset;
} FILE_RECORD_CACHE;


#define WIPE_UNUSED_FILE_RECORD  1
#define WIPE_USED_FILE_RECORD    2
#define WIPE_NO_FILE_RECORD      0


// ****************************************************************************
// *                                                                          *
// * wipeNTFSFileInfo - wipes the file record of the given files, including   *
// * the child records                                                        *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeNTFSFileInfo(FILE_INFO *fileInfo,wchar_t *path) {
	MFT_RECORD     *mftRecord;
	LLIST<__int64> *childRecords = NULL;
	int             hadChilds = 0;
    
	currentVolume = ntfsVolumes[path[0] - 'A'];
	fileRecord.resetObject();
    
	if( !readFileRecord(fileInfo->aux,path[0] - 'A') ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read file record for %s",path);
		return ERRORCODE_FILE_RECORD;
	}
	
	if( stopped ) {
		return ERRORCODE_STOPPED;
	}
	
	/*
	** check for child records ( small chance to find )
	*/
	childRecords = fileRecord.getChildRecords();
	if( childRecords != NULL ) {
		hadChilds = 1;
		dbgPrint(__WFILE__,__LINE__,L"WARNING: File record with child records !");

		for(int i = 0;i < childRecords->number;i++) {
			fileRecord.resetObject();
			
			if( readFileRecord((*childRecords)[i],path[0] - 'A') ) {
				mftRecord = fileRecord.getMftRecord();
				
				if( mftRecord->flags & 0x01 ) {
					wipeUsedFileRecord(&fileRecord,NULL);
				}
				else {
					wipeUnusedFileRecord(&fileRecord,NULL);
				}
			}
		}
	}
	
	if( stopped ) {
		return ERRORCODE_STOPPED;
	}
	
	if( hadChilds == 1 ) {
		/*
		** the file record must be reread
		*/
		fileRecord.resetObject();
		readFileRecord(fileInfo->aux,path[0] - 'A');
	}
	
	mftRecord = fileRecord.getMftRecord();
    
	if( mftRecord->flags & 0x01 ) {
		/*
		** file record for non-deleted file
		*/
		return wipeUsedFileRecord(&fileRecord,fileInfo);
	}
	else {
		/*
		** file record for deleted file
		*/
		return wipeUnusedFileRecord(&fileRecord,fileInfo);
	}
	
	return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * readFileRecord - reads a file record from disk                           *
// *                                                                          *
// ****************************************************************************
int WCONTEXT::readFileRecord(__int64 referenceNumber,int volume) {	
	fileRecord.volume = currentVolume->volume;
	fileRecord.recordSize = currentVolume->fileRecordSize;
	fileRecord.sectorSize = currentVolume->sectorSize;
	fileRecord.clusterSize = currentVolume->clusterSize;
	fileRecord.referenceNumber = referenceNumber & 0xffffffffffff;
	fileRecord.MFTRunlist = &currentVolume->mftRunlist;
	
	/*
	** read file record
	*/
	fileRecordOffset.QuadPart = currentVolume->getFileRecordOffset(referenceNumber & 0xffffffffffff);
	if(fileRecordOffset.QuadPart == -1 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't find file record %d",referenceNumber & 0xffffffffffff);
		return 0;
	}
	
	if( !fileRecord.readRecord(fileRecordOffset) ) {
		return 0;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * roundUpToSector - computes the nearest multiple of the sector greater    *
// * than dataSize                                                            *
// * CURRENTLY NOT USED !                                                     *
// *                                                                          *
// ****************************************************************************
inline __int64 WCONTEXT::roundUpToSector(__int64 dataSize,int sectorSize) {
	return (dataSize + sectorSize) - ((dataSize + sectorSize) % sectorSize);
}


// ****************************************************************************
// *                                                                          *
// * roundDownToSector - computes the nearest multiple of the sector smaller  *
// * than dataSize                                                            *
// * CURRENTLY NOT USED !                                                     *
// *                                                                          *
// ****************************************************************************
inline __int64 WCONTEXT::roundDownToSector(__int64 dataSize,int sectorSize) {
	return dataSize - (dataSize % sectorSize);
}


// ****************************************************************************
// *                                                                          *
// * fillSignature - fills each sector of the record buffer with the fixup    * 
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::fillSignature(unsigned char *signature,int position,int bufferLength) {
	int completeSectors;
	int length = bufferLength - position;

	completeSectors = bufferLength / currentVolume->sectorSize;
	memset(&buffer[position],0,sizeof(unsigned char) * length);

	for(int i = 1;i <= completeSectors;i++) {
		buffer[i * currentVolume->sectorSize - 2] = signature[0];
		buffer[i * currentVolume->sectorSize - 1] = signature[1];
	}
}


// ****************************************************************************
// *                                                                          *
// * clearFixups - sets the fixups for unused sectors to 0                    *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::clearFixups(unsigned char *fixups,int position,int bufferLength) {
	if( currentVolume->sectorSize == 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Trying to divide by zero ( sectorSize )");
		return;
	}
	
	int firstSector = position / currentVolume->sectorSize;
	int sectors = bufferLength / currentVolume->sectorSize;
	
	/*
	** set to 0 the fixups for unused sectors
	*/
	for(int i = firstSector;i < sectors;i++) {
		*((short unsigned int *)(fixups + 0x02 + (0x02 * i))) = 0x00;
	}
}


// ****************************************************************************
// *                                                                          *
// * wipeUnusedFileRecord - wipes a deleted file record, including the        *
// * directory index                                                          *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeUnusedFileRecord(FILE_RECORD *fileRecord,FILE_INFO *fileInfo) {
	MFT_RECORD        mftRecord;
	DIRECTORY_INDEX   dirIndex;
	ATTRIBUTE_HEADER *attrHeader;
	INDEX_HEADER     *indexRecord;
	RUNLIST           indexRunlist;
	int               result;
	unsigned long     bytesWrited;
	int               indexAllocationSize;
	void             *pMftRecord = NULL;
	unsigned char     signature[2] = {1,0};
	
	/*
	** get MFT_RECORD structure ( contains basic information )
	*/
	pMftRecord = (unsigned char *)fileRecord->getMftRecord();
	if( pMftRecord != NULL ) {
		memcpy(&mftRecord,fileRecord->getMftRecord(),sizeof(MFT_RECORD));
		
		if( mftRecord.flags & 0x01 ) {
			/*
			** File record is actually used !
			*/
			return wipeUsedFileRecord(fileRecord,fileInfo);
		}
	}
	
	/*
	** file is a directory ?
	*/
	if( pMftRecord != NULL && (mftRecord.flags & 0x02) && woptions.wipeUnusedIndexRecord ) {
		dirIndex.resetObject();
		dirIndex.volume = currentVolume->volume;
		dirIndex.sectorSize = currentVolume->sectorSize;
		dirIndex.clusterSize = currentVolume->clusterSize;
		
		/*
		** load INDEX_ROOT
		*/
		attrHeader = fileRecord->getNextAttributeHeader(AttributeIndexRoot);
        
		if( attrHeader == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load INDEX_ROOT");
			goto wipeUsedFileRecord;
		}

		/*
		** set INDEX_ROOT
		*/
		if( !dirIndex.setIndexRoot((INDEX_ROOT *)fileRecord->getAttributeBody()) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set INDEX_ROOT");
			goto wipeUsedFileRecord;
		}

		/*
		** load runlist
		*/
		indexRunlist.resetObject();
		
		if( fileInfo != NULL && fileInfo->indexAllocation != NULL ) {
			/*
			** load saved runlist
			*/
			indexRunlist.setRunlist(fileInfo->indexAllocation,fileInfo->runlistLength);
			indexAllocationSize = fileInfo->indexAllocationSize;
		}
		else {
			attrHeader = fileRecord->getNextAttributeHeader(AttributeIndexAllocation);
			if( attrHeader == NULL ) {
				goto wipeUsedFileRecord;
			}

			indexAllocationSize = attrHeader->attributes.nonResident.allocatedSize;
			
			/*
			** load runlist from disk
			*/
			if( !fileRecord->getAttributeRunlist(indexRunlist) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load index allocation runlist");
				goto wipeUsedFileRecord;
			}
		}

		attrHeader = fileRecord->getNextAttributeHeader(AttributeBitmap);
        
		if( attrHeader == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load BITMAP");
			goto wipeUsedFileRecord;
		}

		if( !dirIndex.setIndexAllocationAndBitmap(&indexRunlist,(unsigned char *)fileRecord->getAttributeBody(),
												  indexAllocationSize) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize LARGE directory");			  
				goto wipeUsedFileRecord;
		}
		
		/*
		** wipe all valid index records
		*/
		while( (indexRecord = dirIndex.getNextIndexRecord(0)) != NULL ) {
			woPosition.QuadPart = dirIndex.getRecordOffset(-1);
			woSize.QuadPart = dirIndex.indexRecordSize;
			
			fastWrite = 0;
			result = wipeStream(currentVolume->volume);
			
			if( result != ERRORCODE_SUCCESS ) {
				dbgPrint(__WFILE__,__LINE__,L"WARNING: Failed to wipe index record ( %d )",result);
			}
			
			if( stopped ) {
				return ERRORCODE_STOPPED;
			}
		}
	}

wipeUsedFileRecord:
	/*
	** now wipe file record
	*/
	woPosition = fileRecordOffset;
	woSize.QuadPart = currentVolume->fileRecordSize; // always 1024
	fastWrite = 0;
	result = wipeStream(currentVolume->volume);
	
	/*
	** build file record from scratch ( even if errors occurred )
	** Layout of a "blank" File Record
	**
	** | MFT_RECORD   |
	** | UPDATE ARRAY |
	** | END MARKER   | -> 4 bytes
	*/
	memset(buffer,0,currentVolume->fileRecordSize);
	
	/*
	** common part
	*/
	*((unsigned long *)(buffer + 0x00)) = 0x454c4946;    // FILE
	*((short unsigned int *)(buffer + 0x06)) = 0x03;     // fixup array size
	*((short unsigned int *)(buffer + 0x10)) = 0x00;     // sequence number
	*((unsigned long *)(buffer + 0x1C)) = currentVolume->fileRecordSize;
	*((short unsigned int *)(buffer + 0x28)) = 0x01;
	*((short unsigned int *)(buffer + 510)) = 0x01;      // fixup value, sector 1
	*((short unsigned int *)(buffer + 1022)) = 0x01;     // fixup value, sector 2
	
	if( currentVolume->volumeMajorVersion >= 3 && currentVolume->volumeMinorVersion >= 1 ) {
		/*
		** under XP/2003/Vista ( see NTFS Documentation )
		*/
		*((short unsigned int *)(buffer + 0x04)) = 0x30;  // fixup array offset
		*((short unsigned int *)(buffer + 0x14)) = 0x38;  // attribute offset
		*((unsigned long *)(buffer + 0x18)) = 0x40;       // file usage
		*((unsigned long *)(buffer + 0x38)) = 0xFFFFFFFF;
		*((short unsigned int *)(buffer + 0x30)) = 0x01;  // fixup value
	}
	else {
		/*
		** under NT/2000
		*/
		*((short unsigned int *)(buffer + 0x04)) = 0x2a;
		*((short unsigned int *)(buffer + 0x14)) = 0x30;
		*((unsigned long *)(buffer + 0x18)) = 0x38;
		*((unsigned long *)(buffer + 0x30)) = 0xFFFFFFFF;
		*((short unsigned int *)(buffer + 0x2a)) = 0x01;  // fixup value
	}
	
	/*
	** write "blank" file record
	*/
	if( SetFilePointer(currentVolume->volume,fileRecordOffset.LowPart,
					   (long *)&fileRecordOffset.HighPart,FILE_BEGIN) == 
                       INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set file pointer to position ");
			return ERRORCODE_FILE_RECORD;	
	}
	
	if( !WriteFile(currentVolume->volume,buffer,currentVolume->fileRecordSize, &bytesWrited,NULL) || 
         bytesWrited != currentVolume->fileRecordSize ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't write blank record !!!");
		return ERRORCODE_FILE_RECORD;			   
	}

	if( result != ERRORCODE_SUCCESS ) {
		return ERRORCODE_FILE_RECORD;
	}
	else {
		return ERRORCODE_SUCCESS;
	}
}


// ****************************************************************************
// *                                                                          *
// * wipeUsedFileRecord - wipes the slack of a file record ( not deleted )    *
// * and of a directory index                                                 * 
// *                                                                          *
// ****************************************************************************
int WCONTEXT::wipeUsedFileRecord(FILE_RECORD *fileRecord,FILE_INFO *fileInfo) {
	MFT_RECORD       *mftRecord;
	unsigned char    *recordData;
	DIRECTORY_INDEX   dirIndex;
	ATTRIBUTE_HEADER *attrHeader;
	INDEX_HEADER     *indexRecord;
	RUNLIST           indexRunlist;
	__int64           defaultPosition;
	LARGE_INTEGER     indexRecordOffset;
	int               indexAllocationSize;
	int               result;
	unsigned long     bytesWrited;

	/*
	** get MFT_RECORD structure ( contains basic information )
	*/
	if( (mftRecord = fileRecord->getMftRecord()) == NULL ) {
		return ERRORCODE_CORRUPTED_FS;
	}
	
	if( (mftRecord->flags & 0x01) == 0 ) {
		/*
		** oops ! File record is actually unused !!!
		*/
		return wipeUnusedFileRecord(fileRecord,fileInfo);
	}
	
	/*
	** file is a directory ?
	*/
	if( mftRecord->flags & 0x02 && (woptions.wipeUsedIndexRecord == 1 || 
                                    woptions.wipeUnusedIndexRecord == 1) ) {
		dirIndex.resetObject();
		dirIndex.volume = currentVolume->volume;
		dirIndex.sectorSize = currentVolume->sectorSize;
		dirIndex.clusterSize = currentVolume->clusterSize;

		/*
		** load INDEX_ROOT
		*/
		attrHeader = fileRecord->getNextAttributeHeader(AttributeIndexRoot);
        
		if( attrHeader == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load INDEX_ROOT");
			return ERRORCODE_FILE_RECORD;	
		}

		/*
		** set INDEX_ROOT
		*/
		if( !dirIndex.setIndexRoot((INDEX_ROOT *)fileRecord->getAttributeBody()) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set INDEX_ROOT");
			return ERRORCODE_FILE_RECORD;	
		}
		
		if( dirIndex.getIndexType() == INDEXTYPE_LARGE ) {
			/*
			** load runlist
			*/
			indexRunlist.resetObject();

			if( fileInfo != NULL && fileInfo->indexAllocation != NULL ) {
				/*
				** load saved runlist
				*/
				indexRunlist.setRunlist(fileInfo->indexAllocation,fileInfo->runlistLength);
				indexAllocationSize = fileInfo->indexAllocationSize;
			}
			else {
				attrHeader = fileRecord->getNextAttributeHeader(AttributeIndexAllocation);
                
				if( attrHeader == NULL ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load INDEX_ALLOCATION");
					return ERRORCODE_DIRECTORY_INDEX;
				}

				indexAllocationSize = attrHeader->attributes.nonResident.allocatedSize;
				
				/*
				** load runlist from disk
				*/
				if( !fileRecord->getAttributeRunlist(indexRunlist) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load index allocation runlist");
					return ERRORCODE_DIRECTORY_INDEX;
				}
			}

			attrHeader = fileRecord->getNextAttributeHeader(AttributeBitmap);
            
			if( attrHeader == NULL ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load BITMAP");
				return ERRORCODE_DIRECTORY_INDEX;
			}

			if( !dirIndex.setIndexAllocationAndBitmap(&indexRunlist,(unsigned char *)fileRecord->getAttributeBody(),
				indexAllocationSize) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize LARGE directory");			  
				return ERRORCODE_DIRECTORY_INDEX;
			}

			/*
			** wipe all valid index records
			*/
			while( (indexRecord = dirIndex.getNextIndexRecord(0)) != NULL ) {
				if( dirIndex.getRecordState(-1) == 0 ) {
					/*
					** index record unused, wipe it all !
					*/
					woPosition.QuadPart = dirIndex.getRecordOffset(-1);
					woSize.QuadPart = dirIndex.indexRecordSize;
					fastWrite = 0;
					result = wipeStream(currentVolume->volume);

					if( result != ERRORCODE_SUCCESS ) {
						dbgPrint(__WFILE__,__LINE__,L"WARNING: Failed to wipe index record ( %d )",result);
						return ERRORCODE_DIRECTORY_INDEX;	
					}
					
					if( stopped ) {
						return ERRORCODE_STOPPED;
					}
				}
				else if( woptions.wipeUsedIndexRecord ) {
					/*
					** index record used, wipe only slack space
					*/
					recordData = dirIndex.getRecordBuffer();
					indexRecordOffset.QuadPart = dirIndex.getRecordOffset(-1);
					
					defaultPosition = indexRecordOffset.QuadPart + indexRecord->realSize + 0x18;
					woPosition.QuadPart = indexRecordOffset.QuadPart;
					woSize.QuadPart = dirIndex.indexRecordSize;
					fastWrite = 0;
					result = wipeStream(currentVolume->volume);
					
					/*
					** write signature
					*/
					memcpy(buffer,recordData,dirIndex.indexRecordSize);
					fillSignature(dirIndex.signature,indexRecord->realSize + 0x18,
								  dirIndex.indexRecordSize);
					clearFixups(&buffer[indexRecord->fixupArrayOffset],
                                indexRecord->realSize + 0x18,
								dirIndex.indexRecordSize);
					
					/*
					** write record with clean slack space
					** if it cannot be written, all files in the folder are lost !!!
					** the only hope is that chkdsk or NTFS log can recover it !
					*/
					if( SetFilePointer(currentVolume->volume,woPosition.LowPart,
                                       (long *)&woPosition.HighPart,FILE_BEGIN) == 
                                       INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
						dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set stream pointer to position %d",woPosition.QuadPart);
						return ERRORCODE_DIRECTORY_INDEX;
					}

					if( !WriteFile(currentVolume->volume,buffer,dirIndex.indexRecordSize,
						&bytesWrited,NULL) || bytesWrited != dirIndex.indexRecordSize ) {
						dbgPrint(__WFILE__,__LINE__,L"ERROR: Write error %d",GetLastError());
						return ERRORCODE_DIRECTORY_INDEX;	
					}
					
					if( stopped ) {
						return ERRORCODE_STOPPED;
					}
				}
			}
		}
	}
	
	/*
	** wipe file record slack space
	*/
	recordData = fileRecord->getRecordBuffer();
	defaultPosition = fileRecordOffset.QuadPart + mftRecord->recordRealSize - 0x04;
	woPosition.QuadPart = fileRecordOffset.QuadPart;
	woSize.QuadPart = currentVolume->fileRecordSize;
	
	fastWrite = 0;
	result = wipeStream(currentVolume->volume);
	
	/*
	** write signature
	*/
	memcpy(buffer,recordData,mftRecord->recordAllocatedSize);
	fillSignature(fileRecord->signature,mftRecord->recordRealSize - 0x04,
				  currentVolume->fileRecordSize);
	clearFixups(&buffer[mftRecord->fixupArrayOffset],mftRecord->recordRealSize - 0x04,
				currentVolume->fileRecordSize);
	
	/*
	** write record with clean slack space
	** if it fails, the file is lost !!!
	** the only hope is that chkdsk or NTFS log can recover it !
	*/
	if( SetFilePointer(currentVolume->volume,woPosition.LowPart,
                       (long *)&woPosition.HighPart,FILE_BEGIN) == 
                       INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set stream pointer to position %d",
                                    fileRecordOffset.QuadPart);
		return ERRORCODE_FILE_RECORD;
	}
	
	if( !WriteFile(currentVolume->volume,buffer,currentVolume->fileRecordSize,
				   &bytesWrited,NULL) || bytesWrited != currentVolume->fileRecordSize ) {
	   dbgPrint(__WFILE__,__LINE__,L"ERROR: Write error %d",GetLastError());
	   return ERRORCODE_FILE_RECORD;	
	}
	
	if( result != ERRORCODE_SUCCESS ) {
		return ERRORCODE_FILE_RECORD;
	}
	else {
		return ERRORCODE_SUCCESS;
	}
}


// ****************************************************************************
// *                                                                          *
// * wipeMFT - wipes all used and/or unused file records on the volume,       *
// * including directory indexes ( only if the user wants... )                *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::wipeMFT(wchar_t volume) {
	__int64            fileRecords;
	unsigned char     *mftBitmap;
	unsigned char      cacheBuffer[262144];
	FILE_RECORD_CACHE  recordCache[16];
	int                inCache = 0;
	int                wipeRecord = 0;
	unsigned long      bytesRead;
	int                notSuccessive = 0;
	int                result;
	
	/*
	** prepare volume...
	*/
	if( ntfsVolumes[volume] == NULL ) {
		ntfsVolumes[volume] = new NTFS_VOLUME;
		if( !ntfsVolumes[volume]->initialize(volume) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't initialize ntfsVolume %c",volume);
			return ERRORCODE_CORRUPTED_FS;
		}
	}
	currentVolume = ntfsVolumes[volume];
	volInfo.cv = volume - 'A';
	
	/*
	** compute number of file records
	*/
	fileRecords = currentVolume->getMFTSize() / currentVolume->fileRecordSize;
	mftBitmap = currentVolume->getMFTBitmap();
	
	/*
	** progress information
	*/
	updateAuxMessage(WSTATUS_SUBMESSAGE_MFT);
	updateWobjectTotal(fileRecords);
	resetWobjectWiped();
	wstatus.type = WOTYPE_MFT;
	
	if( fileRecords < 25 || mftBitmap == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid file records number or bitmap");
		return ERRORCODE_CORRUPTED_FS;
	}
	
	for(__int64 i = 5;i < fileRecords;i++) {
		if( stopped ) {
			return ERRORCODE_STOPPED;
		}
		
		setWobjectWiped(i);
		
		if( !woptions.wipeUsedFileRecord && bitSet(mftBitmap,i) && inCache == 0 ) {
			continue;
		}
		else if( !woptions.wipeUnusedFileRecord && !bitSet(mftBitmap,i) && inCache == 0 ) {
			continue;
		}
		
		recordCache[inCache].referenceNumber = i;
		recordCache[inCache].offset.QuadPart = currentVolume->getFileRecordOffset(i);
		if( inCache > 0 && ((recordCache[inCache].offset.QuadPart - 
                             recordCache[inCache - 1].offset.QuadPart) != 
                            currentVolume->fileRecordSize) ) {
			notSuccessive = 1;
			i--;
		}
		else {
			inCache++;
		}
		
		if( inCache == 16 || notSuccessive == 1 || i == fileRecords - 1 ) {
			/*
			** read up to 16 file records into cache
			*/
			woPosition.QuadPart = currentVolume->getFileRecordOffset(recordCache[0].referenceNumber);
			woSize.QuadPart = inCache * currentVolume->fileRecordSize;
			
			if( SetFilePointer(currentVolume->volume,woPosition.LowPart,
                               (long *)&woPosition.HighPart,FILE_BEGIN) == 
                               INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set stream pointer to position %d",woPosition.QuadPart);
					return ERRORCODE_UNKNOWN;	
			}

			if( !ReadFile(currentVolume->volume,cacheBuffer,woSize.LowPart,
				&bytesRead,NULL) || bytesRead != woSize.LowPart ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Write error %d",GetLastError());
					return ERRORCODE_UNKNOWN;
			}
			
			for(int j = 0;j < inCache;j++) {
				if( stopped ) {
					return ERRORCODE_STOPPED;
				}
			
				/*
				** don't wipe records between 6 and 25
				*/
				if( recordCache[j].referenceNumber < 25 && recordCache[j].referenceNumber != 5 ) {
					continue;
				}
			
				wipeRecord = WIPE_NO_FILE_RECORD;
				if( bitSet(mftBitmap,recordCache[j].referenceNumber) ) {
					if( woptions.wipeUsedFileRecord ) {
						wipeRecord = WIPE_USED_FILE_RECORD;
					}
				}
				else if( woptions.wipeUsedFileRecord ) {
					wipeRecord = WIPE_UNUSED_FILE_RECORD;
				}
				
				if( wipeRecord  != WIPE_NO_FILE_RECORD ) {
					fileRecord.volume = currentVolume->volume;
					fileRecord.recordSize = currentVolume->fileRecordSize;
					fileRecord.sectorSize = currentVolume->sectorSize;
					fileRecord.clusterSize = currentVolume->clusterSize;
					fileRecord.referenceNumber = recordCache[j].referenceNumber;
					fileRecord.MFTRunlist = &currentVolume->mftRunlist;
					
					fileRecordOffset.QuadPart = recordCache[j].offset.QuadPart;
					fileRecord.resetObject();
					fileRecord.setRecord(&cacheBuffer[j * currentVolume->fileRecordSize]);
					
					if( wipeRecord == WIPE_UNUSED_FILE_RECORD ) {
						/*
						** wipe unused file record
						*/
						result = wipeUnusedFileRecord(&fileRecord,NULL);
					}
					else {
						/*
						** wipe used file record
						*/
						result = wipeUsedFileRecord(&fileRecord,NULL);
					}
					
					if( result != ERRORCODE_SUCCESS && result != ERRORCODE_CORRUPTED_FS ) {
						/*
						** add error in log
						*/
						if( result == ERRORCODE_DIRECTORY_INDEX ) {
							log.addToLog(SEVERITY_HIGH,L"ERROR: Failed to wipe directory index for file record %d.",
                                         recordCache[j].referenceNumber);
						}
						else {
							log.addToLog(SEVERITY_HIGH,L"ERROR: Failed to wipe file record %d.",
                                         recordCache[j].referenceNumber);
						}
					}
				}
			}
			
			/*
			** no file record in cache
			*/
			inCache = 0;
			notSuccessive = 0;
		}
	}
	
	return ERRORCODE_SUCCESS;
}

#endif
