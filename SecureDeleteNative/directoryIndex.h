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

#ifndef DIRECTORY_INDEX_H
#define DIRECTORY_INDEX_H

#include <windows.h>
#include "debug.h"
#include "runList.h"
#include "attributes.h"
#include <math.h>

/*
** definitions for directory index size
*/
#define INDEXTYPE_SMALL 0
#define INDEXTYPE_LARGE 1

#define INDEXRECORD_CACHESIZE 16384 // 16KB, max read speed

/*
** check if bit in INDEX_ALLOCATION is set
*/
inline int bitSet(unsigned char *bitmap,__int64 position) {
	return (bitmap[position >> 3] & (1 << (position & 7))) != 0;
}


class DIRECTORY_INDEX {
private:
	INDEX_ROOT    *indexRoot;
	RUNLIST       *indexAllocation;
	unsigned char *indexBitmap;
	unsigned char  indexRecordCache[INDEXRECORD_CACHESIZE];
	
	int    initialized;
	int    largeIndex;          // 1 if its a large directory
	double indexRecordClusters; // size in clusters ( it can be < 1 )
	
	INDEX_HEADER *lastIndexRecord;
	INDEX_ENTRY  *lastIndexEntry;
	int           irCacheSize;     // number of records in cache
	int           irCacheMaxSize;  // maxinum number of records that can be cached
	int           irCacheStart;
	int           irCacheEnd;
	int           iRootPosition;   // position in the index root
	
	int indexRecords;    
	int iRecordPosition; // position in the index records list
	int lastIRPosition;  // position in the current index record
	
	int checkFixupArray(unsigned char *fixupArray,int length,unsigned char *data);
public:
	DIRECTORY_INDEX() {
		initialized = 0;
		largeIndex = 0;
		indexRecordSize = 0;
		indexRoot = NULL;
		lastIndexRecord = NULL;
		indexRecords = 0;
		iRecordPosition = 0;
		lastIRPosition = 0;
		irCacheSize = 0;
		irCacheStart = 0;
		irCacheEnd = 0;
	}
	
	int           sectorSize;
	int           clusterSize;
	HANDLE        volume;
	int           indexRecordSize;     // size ( in bytes ) of an index record ( usually 4096 )
	unsigned char signature[2];
	
	int setIndexRoot(INDEX_ROOT *newIndexRoot);
	int getIndexType(); // gets the size of the directory ( small/large )
	int setIndexAllocationAndBitmap(RUNLIST *newIndexAllocation,
                                    unsigned char *newIndexBitmap,
                                    int newIndexAllocationSize);
	
	INDEX_ENTRY   *getNextFilename();
	INDEX_HEADER  *getNextIndexRecord(int onlyUsed);
	int            getRecordNumber();
	int            getRecordState(int record);
	__int64        getRecordOffset(int record);
	unsigned char *getRecordBuffer();
	
	int           resetPosition();
	int           resetObject();
};


// ****************************************************************************
// *                                                                          *
// * checkFixupArray - performs sanity check                                  *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::checkFixupArray(unsigned char *fixupArray,int length,unsigned char *data) {	
	if( fixupArray == NULL || length <= 0 ) {
		return 0;
	}
	
	/*
	** copy the signature
	*/
	signature[0] = fixupArray[0];
	signature[1] = fixupArray[1];
	
	/*
	** check the signature in each sector and replace it with the original value
	*/
	for(int i = 1;i < length;i++) {
		if( data[i * sectorSize - 2] != signature[0] ||
			data[i * sectorSize - 1] != signature[1] ) {
			/*
			** the index record is corrupt
			*/
			return 0;	
		}
		
		/*
		** replace the signature with the original value
		*/
		data[i * sectorSize - 2] = fixupArray[i * 2];
		data[i * sectorSize - 1] = fixupArray[i * 2 + 1];
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * setIndexRoot - initializes the object with the given index root          *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::setIndexRoot(INDEX_ROOT *newIndexRoot) {
	if( newIndexRoot == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters in setIndexRoot");
		return 0;	
	}
	
	/*
	** allocate memory
	*/
	resetObject();	
	indexRoot = (INDEX_ROOT *)(new unsigned char[newIndexRoot->realSize]);
	
	/*
	** copy the index root structure
	*/
	memcpy(indexRoot,newIndexRoot,newIndexRoot->realSize);
	indexRoot->realSize = indexRoot->realSize;
	
	/*
	** set options
	*/
	initialized = 1;
	largeIndex = indexRoot->flags & INDEXTYPE_LARGE;
	indexRecordSize = indexRoot->indexAllocationEntrySize;
	
	if( indexRecordSize < clusterSize ) {
		indexRecordSize = clusterSize; // not 100% sure :)
		indexRecordClusters = 1;
	}
	else {
		indexRecordClusters = double((double)indexRecordSize / (double)clusterSize);	
	}
	
	irCacheMaxSize = INDEXRECORD_CACHESIZE / ((int)indexRecordClusters * clusterSize);
	iRootPosition = 0;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getIndexType - gets the type of the directory index ( small/large )      *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::getIndexType() {
	if( !initialized ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return -1;
	}
	
	return largeIndex;
}


// ****************************************************************************
// *                                                                          *
// * setIndexAllocationAndBitmap - enables large directory                    *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::setIndexAllocationAndBitmap(RUNLIST *newIndexAllocation,
                                                        unsigned char *newIndexBitmap,
														int newIndexAllocationSize) {
	if( !initialized || newIndexAllocation == NULL || newIndexBitmap == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return 0;
	}
	
	indexAllocation = newIndexAllocation;
	indexBitmap = newIndexBitmap;
	
	/*
	** get the number of index records
	*/
	indexAllocation->getRuns();
	indexRecords = newIndexAllocationSize / indexRecordSize;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getNextIndexRecord - gets the next index record                          *
// *                                                                          *
// ****************************************************************************
inline INDEX_HEADER *DIRECTORY_INDEX::getNextIndexRecord(int onlyUsed) {
	ULARGE_INTEGER offset;
	unsigned long  dataRead;
	static __int64 cacheLCN[32];

	if( !initialized || indexRecordSize <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Incerc sa citesc IndexHeader si nu am initializat clasa");
		return NULL;
	}
	
	/*
	** search the first index record that meets the condition
	*/
	for(;iRecordPosition < indexRecords;iRecordPosition++) {
		if( onlyUsed && bitSet(indexBitmap,iRecordPosition) ) {
			break;
		}
		else if( !onlyUsed ) {
			break;
		}
	}
	
	/*
	** there are no more index records ?
	*/
	if( iRecordPosition == indexRecords ) {
		return NULL;
	}
	
	/*
	** check if the record is in cache
	*/
	readCache:
	if( iRecordPosition >= irCacheStart && iRecordPosition <= irCacheEnd && irCacheSize > 0 ) {
		lastIndexRecord = (INDEX_HEADER *)&indexRecordCache[(iRecordPosition - irCacheStart) * 
                                                             indexRecordSize];	
		/*
		** perform sanity check
		*/
		if( lastIndexRecord->magic[0] != 'I' || lastIndexRecord->magic[1] != 'N' ||
			lastIndexRecord->magic[2] != 'D' || lastIndexRecord->magic[3] != 'X' ) {
			dbgPrint(__WFILE__,__LINE__,L"WARNING: Corrupt index record at LCN %d",
                     cacheLCN[iRecordPosition]);
			return NULL;
		}
	
		/*
		** check the fixup array
		*/
		if( lastIndexRecord->fixupArraySize > 0 ) {
			if( !checkFixupArray((unsigned char *)lastIndexRecord + lastIndexRecord->fixupArrayOffset,
                                  lastIndexRecord->fixupArraySize,(unsigned char *)lastIndexRecord) ) {
				/*
				** the index record is corrupt
				*/
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Corrupt index record at LCN %d. "
                                            L"Fixup array check failed", 
                                             cacheLCN[iRecordPosition]);
				return NULL;
			}
		}
		
		iRecordPosition++;
	}
	else {
		/*
		** read the record, obtain offset
		*/
		cacheLCN[0] = indexAllocation->getLCN(iRecordPosition * indexRecordClusters);
		irCacheStart = iRecordPosition;
		
		for(irCacheSize = 1; (irCacheSize < irCacheMaxSize) && 
                             (iRecordPosition + irCacheSize) < indexRecords - 1; irCacheSize++) {
			cacheLCN[irCacheSize] = indexAllocation->getLCN((iRecordPosition + irCacheSize) * 
                                                             indexRecordClusters);
			
			/*
			** check if the difference ( in clusters ) is equal to the clusters used by a index record
			*/
			if( cacheLCN[irCacheSize] - cacheLCN[irCacheSize - 1] != (int)indexRecordClusters ) {
				break;
			}
		}
		
		irCacheEnd = irCacheStart + irCacheSize - 1;
		offset.QuadPart = cacheLCN[0] * clusterSize;
		
		if( SetFilePointer(volume,offset.LowPart,(long *)&offset.HighPart,FILE_BEGIN) == 
            INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set pointer at position %d",offset.QuadPart);
			return NULL;
		}
		
		if( !ReadFile(volume,indexRecordCache,indexRecordSize * irCacheSize,&dataRead,NULL) && 
             (dataRead != indexRecordSize) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read index record ( readed only %d bytes )",dataRead);
			return NULL;
		}
		
		goto readCache; // damn, we need to use goto (but results ~35% speed increase)
	}
	
	return lastIndexRecord;	
}


// ****************************************************************************
// *                                                                          *
// * getNextFilename - gets the next filename in the directory ( not sorted   *
// * alphabetically - it dosen't uses a B+ tree )                             *
// *                                                                          *
// ****************************************************************************
inline INDEX_ENTRY *DIRECTORY_INDEX::getNextFilename() {
	int ok;
	
	if( !initialized || indexRecordSize <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}
	
	/*
	** try to get filenames from index root first
	*/
indexRootEntry:
	if( iRootPosition < (indexRoot->realSize) ) {
		if( iRootPosition == 0 ) {
			iRootPosition = 0x10 + indexRoot->indexEntryOffset;
		}
		else {
			iRootPosition += lastIndexEntry->indexEntrySize;
		}
		
		if( iRootPosition < indexRoot->realSize ) {
			lastIndexEntry = (INDEX_ENTRY *)((unsigned char *)indexRoot + iRootPosition);
		}
		else lastIndexEntry = NULL;
		
		/*
		** reached last entry ?
		*/
		if( lastIndexEntry != NULL && lastIndexEntry->indexFlags & 0x02 ) {
			lastIndexEntry = NULL;
			iRootPosition = 0x10 + indexRoot->realSize;
		}
		
		/*
		** exclude DOS names ( in 8.3 format )
		*/
		if( lastIndexEntry != NULL && !(lastIndexEntry->filenameType & 0x01) ) {
			goto indexRootEntry; // get other filename
			                     // damn, we need to use goto
		}
		
		if( lastIndexEntry != NULL ) {
			return lastIndexEntry;
		}
		else if( largeIndex == 0 ) {
			return NULL; // it's the last filename
		}
	}
	
	if( iRecordPosition == 0 ) {
		/*
		** reading the first index record
		*/
		if( getNextIndexRecord(1) == NULL ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read index header");
			return NULL;
		}
		
		lastIRPosition = 0;
	}
	
	/*
	** get a file name from the index record
	*/
getNextRecord:
	if( lastIRPosition < lastIndexRecord->realSize ) {
		if( lastIRPosition == 0 ) {
			lastIRPosition = 0x18 + lastIndexRecord->indexEntryOffset;
		}
		else {
			lastIRPosition += lastIndexEntry->indexEntrySize;
		}
		
		ok = 1;
	}
	else {
		/*
		** need to load next index record ?
		*/
		if( getNextIndexRecord(1) == NULL ) {
			/*
			** the entire folder is read, nothing more to do
			*/
			return NULL;
		}
		
		lastIRPosition = 0x18 + lastIndexRecord->indexEntryOffset;
		ok = 1;
	}
	
	if( ok == 1 ) {
		lastIndexEntry = (INDEX_ENTRY *)((unsigned char *)lastIndexRecord + lastIRPosition);
		
		/*
		** reached last entry ?
		*/
		if( lastIndexEntry->indexFlags & 0x02 ) {
			lastIndexEntry = NULL;
			iRootPosition = 0x10 + indexRoot->realSize;
			goto getNextRecord; // we need to use goto
		}
		
		/*
		** exclude DOS names ( in  8.3 format )
		*/
		if( lastIndexEntry != NULL && (lastIndexEntry->filenameType == 2) ) {
			goto getNextRecord; // we need to use goto
		}
		
		return lastIndexEntry;
	}
	else {
		/*
		** there are no more files in the directory !
		*/
		return NULL;
	}
}


// ****************************************************************************
// *                                                                          *
// * getRecordNumber - gets the number of records the directory index has     *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::getRecordNumber() {
	return indexRecords;
}


// ****************************************************************************
// *                                                                          *
// * getRecordStatus - gets the status ( used/unused ) of the record          *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::getRecordState(int record) {
	if( record == -1 && lastIndexRecord != NULL ) {
		return bitSet(indexBitmap,iRecordPosition - 1);
	}
	else if( record >= 0 && record < indexRecords ) {
		return bitSet(indexBitmap,record);
	}
	
	return 0;
}


// ****************************************************************************
// *                                                                          *
// * getRecordOffset - gets the offset of the record                          *
// *                                                                          *
// ****************************************************************************
inline __int64 DIRECTORY_INDEX::getRecordOffset(int record) {
	if( record == -1 && lastIndexRecord != NULL ) {
		return indexAllocation->getLCN((iRecordPosition - 1) * indexRecordClusters) * clusterSize;
	}
	else if( record >= 0 && record < indexRecords ) {
		return indexAllocation->getLCN(record * indexRecordClusters) * clusterSize;
	}
	
	return 0;
}


// ****************************************************************************
// *                                                                          *
// * getRecordBuffer - gets a pointer to the index record buffer              *
// *                                                                          *
// ****************************************************************************
inline unsigned char *DIRECTORY_INDEX::getRecordBuffer() {
	return (unsigned char *)((void *)lastIndexRecord);
}


// ****************************************************************************
// *                                                                          *
// * resetPosition - start the reading of next index record or filename from  *
// * the beginning                                                            *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::resetPosition() {
	if( !initialized ) {
		return 0;
	}

	largeIndex = 0;
	indexRecordSize = 0;
	iRootPosition = 0;	
	lastIndexEntry = NULL;
	indexRecords = 0;
	iRecordPosition = 0;
	lastIRPosition = 0;
	irCacheSize = 0;
	irCacheMaxSize = 0;
	irCacheStart = 0;
	irCacheEnd = 0;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
inline int DIRECTORY_INDEX::resetObject() {
	if( !initialized ) {
		return 0;
	}
	
	resetPosition();
	initialized = 0;

	if( indexRoot != NULL ) {
		delete indexRoot;
		indexRoot = NULL;
	}
    
	return 1;
}

#endif
