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

#ifndef MFTRECORD_H
#define MFTRECORD_H

#include <windows.h>
#include "debug.h"
#include "runList.h"
#include "attributes.h"


/*
** definition for MFT_RECORD structure
*/
typedef struct __MFT_RECORD {
	unsigned char      magic[4]; // FILE
	short unsigned int fixupArrayOffset;
	short unsigned int fixupArraySize;
	__int64            lsn;
	short unsigned int referenceNumber;
	short unsigned int hardlinkCount;
	short unsigned int attributeOffset;
	short unsigned int flags;
	unsigned long      recordRealSize;
	unsigned long      recordAllocatedSize;
	__int64            baseRecordReference;
	short unsigned int nextAttributeID;
	char               align[2];
	unsigned long      mftRecordNuber;
} MFT_RECORD;


class FILE_RECORD {
private:
	unsigned char *data;
	int            position;      // position file record
	int            realSize;      // space used by the file record
	int            initialized;   // 1 if the class was initialized successful
	unsigned char *attributeBody;
	
	ATTRIBUTE_HEADER *lastAttribute;
	
	/*
	** variables used for external attributes ( ATTRIBUTE_LIST ) 
	*/
	LLIST<__int64>  externalAttributes;
	int             eaPosition;
	FILE_RECORD    *childRecord; // pointer to the child's file record
	
	int               inList(__int64 childReferenceNumber);
	__int64           getAttributeSize();
	int               checkFixupArray(unsigned char *fixupArray,int length); 
	ATTRIBUTE_HEADER *getExternalAttrHeader();
	
public:
	FILE_RECORD() {
		data = NULL;
		lastAttribute = NULL;
		attributeBody = NULL;
		initialized = 0;
		position = 0;
		childRecord = NULL;
		eaPosition = 0;
	}
	~FILE_RECORD() {
		if( data != NULL ) {
			delete[] data;
		}
		if( attributeBody != NULL ) {
			delete[] attributeBody;
		}
	}
	
	__int64        referenceNumber;
	int            sectorSize;
	int            clusterSize;
	int            recordSize;
	HANDLE         volume;
	RUNLIST       *MFTRunlist;
	unsigned char  signature[2];
	
	int setRecord(unsigned char *record);                            // reads the record from memory
	int readRecord(ULARGE_INTEGER &offset);                          // reads the record from disk
	
	MFT_RECORD       *getMftRecord();                                // gets an file record structure
	ATTRIBUTE_HEADER *getNextAttributeHeader();                      // gets the next attribute ( only the header )
	ATTRIBUTE_HEADER *getNextAttributeHeader(ATTRIBUTE_TYPE filter); // same as above, but with filter
	void             *getAttributeBody();                            // gets the attribute body
	int               getAttributeRunlist(RUNLIST &aux);             // gets the attribute runlist
	unsigned char    *getAttributeRunlistAsByte();
	LLIST<__int64>   *getChildRecords();                             // gets a list with the reference numbers of all child records
	unsigned char    *getRecordBuffer();
	
	int resetPosition(); // start reading of attributes from beginning
	int resetObject();
};


// ****************************************************************************
// *                                                                          *
// * inList - checks if a reference number is in the child records list       *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::inList(__int64 childReferenceNumber) {
	for(int i = 0;i < externalAttributes.number;i++) {
		if( externalAttributes[i] == childReferenceNumber ) {
			/*
			** its already in the list...
			*/
			return 1;
		}
	}

	return 0;
}


// ****************************************************************************
// *                                                                          *
// * getAttributeSize - gets the size of the body                             *
// *                                                                          *
// ****************************************************************************
inline __int64 FILE_RECORD::getAttributeSize() {
	if( !initialized || data == NULL || position >= realSize || lastAttribute == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}

	if( eaPosition < externalAttributes.number && childRecord != NULL ) {
		return childRecord->getAttributeSize();
	}
	else {
		return lastAttribute->nonResidentFlag == 1 ? 
               lastAttribute->attributes.nonResident.realSize :
               lastAttribute->attributes.resident.length;
	}
}


// ****************************************************************************
// *                                                                          *
// * checkFixupArray - performs sanity check                                  *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::checkFixupArray(unsigned char *fixupArray,int length) {	
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
			** the file record is corrupt
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
// * getExternalAttrHeader - gets an external attribute                       *
// * The function fails only if the readed file record was corrupt or no      *
// * other child records exists.                                              *
// *                                                                          *
// ****************************************************************************
inline ATTRIBUTE_HEADER *FILE_RECORD::getExternalAttrHeader() {
	ULARGE_INTEGER    offset;
	__int64           childReference;
	ATTRIBUTE_HEADER *attributeHeader = NULL;

	if( childRecord == NULL ) {
		/*
		** load the first child record
		*/
		childReference = externalAttributes[eaPosition];
		offset.QuadPart = MFTRunlist->getLCN(double(childReference * recordSize) /
                                             double(clusterSize)) * clusterSize + 
                                             (childReference * recordSize) % clusterSize;
		/*
		** create a new file record object
		*/
		childRecord = new FILE_RECORD;

		/*
		** set the options
		*/
		childRecord->recordSize = recordSize;
		childRecord->sectorSize = sectorSize;
		childRecord->clusterSize = clusterSize;
		childRecord->volume = volume;
		childRecord->referenceNumber = childReference;
		childRecord->MFTRunlist = MFTRunlist;

		/*
		** now read the file record
		*/
		if( !childRecord->readRecord(offset) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Failed to read file record");
			return NULL;
		}
	}

	/*
	** get the next external attribute
	*/
	if( childRecord != NULL ) {
		attributeHeader = childRecord->getNextAttributeHeader();

		if( attributeHeader == NULL && eaPosition < (externalAttributes.number - 1) ) {
			/*
			** deallocate the current child record
			*/
			eaPosition++;

			if( childRecord != NULL ) {
				delete childRecord;
			}

			/*
			** get the location of the child record
			*/
			childReference = externalAttributes[eaPosition];
			offset.QuadPart = MFTRunlist->getLCN((childReference * recordSize) / clusterSize) * 
                                                 clusterSize + (childReference * recordSize) % clusterSize;

			/*
			** create a new file record object
			*/
			childRecord = new FILE_RECORD;

			/*
			** set the options
			*/
			childRecord->recordSize = recordSize;
			childRecord->sectorSize = sectorSize;
			childRecord->clusterSize = clusterSize;
			childRecord->volume = volume;
			childRecord->referenceNumber = childReference;
			childRecord->MFTRunlist = MFTRunlist;

			/*
			** now read the record
			*/
			if( !childRecord->readRecord(offset) ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Failed to read file record");
				return NULL;
			}			

			attributeHeader = childRecord->getNextAttributeHeader();
		}
		else if( attributeHeader == NULL && (eaPosition + 1) == externalAttributes.number ) {
			/*
			** there are no other child records
			*/
			eaPosition = externalAttributes.number + 1;
		}
	}

	return attributeHeader;
}


// ****************************************************************************
// *                                                                          *
// * setRecord - reads a file record from memory                              *
// * Settings should be set before.                                           *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD ::setRecord(unsigned char *newRecord) {
	if( volume == INVALID_HANDLE_VALUE || recordSize <= 0 || clusterSize <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for setRecord");
		return 0;
	}
	
	/*
	** allocate memory
	*/
	resetObject();
	data = new unsigned char[recordSize];
	
	/*
	** copy the file record
	*/
	memcpy(data,newRecord,sizeof(unsigned char) * recordSize);
	
	/*
	** now perform sanity check
	*/
	initialized = 1;	
	MFT_RECORD *record = getMftRecord();
    
	if( record == NULL ) {
		return 0;
	}
	
	/*
	** check the fixupArray
	*/
	if( record->fixupArraySize > 0 ) {
		if( !checkFixupArray(&data[record->fixupArrayOffset],record->fixupArraySize) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: File record %d is corrupt. Fixup array check failed",
                                        referenceNumber);
			return 0;
		}
	}
	
	realSize = record->recordRealSize;	
	lastAttribute = NULL;
	position = 0;
	childRecord = NULL;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * readRecord - reads the file record from disk                             *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::readRecord(ULARGE_INTEGER &offset) {
	unsigned long dataRead;

	if( volume == INVALID_HANDLE_VALUE || recordSize <= 0 || clusterSize <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for readRecord");
		return 0;
	}
	
	/*
	** allocate memory
	*/
	resetObject();
	data = new unsigned char[recordSize];
	
	/*
	** read the file record
	*/
	if( SetFilePointer(volume,offset.LowPart,(long *)&offset.HighPart,FILE_BEGIN) == 
                       INVALID_SET_FILE_POINTER &&
		GetLastError() != NO_ERROR ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set file pointer to position %d",offset.QuadPart);
		return 0;	
	}

	if( !ReadFile(volume,data,recordSize,&dataRead,NULL) && 
        (dataRead != recordSize) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read index record ( readed only %d bytes )",dataRead);
		return 0;
	}

	/*
	** now perform sanity check
	*/
	initialized = 1;	

	MFT_RECORD *record = getMftRecord();
	if( record == NULL ) {
		return 0;
	}
	
	/*
	** check the fixupArray
	*/
	if( record->fixupArraySize > 0 ) {
		if( !checkFixupArray(&data[record->fixupArrayOffset],record->fixupArraySize) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: The file record is corrupt. Fixup array check failed");
			return 0;
		}
	}
	
	realSize = record->recordRealSize;	
	lastAttribute = NULL;
	position = 0;

	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getMftRecord - gets a file record structure                              *
// *                                                                          *
// ****************************************************************************
MFT_RECORD * FILE_RECORD::getMftRecord() {
	if( !initialized || data == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return NULL;
	}

	/*
	** perform sanity check
	*/
	if( data[0] != 'F' || data[1] != 'I' || data[2] != 'L' || data[3] != 'E' ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Corrupt file record %d",referenceNumber);
		return NULL;
	}
	
	return (MFT_RECORD *)&data[0];
}


// ****************************************************************************
// *                                                                          *
// * getNextAttributeHeader - gets the next attribute ( only the header ).    *
// * It doesn't return the ATTRIBUTE_LIST attribute, instead it returns the   *
// * attributes located in the child file record.                             *
// *                                                                          *
// ****************************************************************************
ATTRIBUTE_HEADER *FILE_RECORD::getNextAttributeHeader() {
	ATTRIBUTE_HEADER *attributeHeader;
	
	if( !initialized || data == NULL || position >= realSize ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}
	
	/*
	** should check the child record first ?
	*/
	if( eaPosition < externalAttributes.number && childRecord != NULL ) {
		attributeHeader = getExternalAttrHeader();
		
		if( attributeHeader != NULL ) {
			/*
			** attribute extracted successfully
			*/
			return attributeHeader;
		}
	}
	
	if( position == 0 ) {
		/*
		** reading the first attribute, get position from file record
		*/
		MFT_RECORD *record = getMftRecord();
		position += record->attributeOffset;
	}
	else {
		/*
		** an attribute was already read, increment position with its size
		*/
		position += lastAttribute->length;
	}
	
	/*
	** check if the position is corrupt
	*/
	if( position >= realSize || position <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: MFT Record este corupt ( position = %d )",position);
		return NULL;
	}
	
	lastAttribute = (ATTRIBUTE_HEADER *)&data[position];
    
	if( lastAttribute->attributeType == (unsigned long) - 1 ) {
		/*
		** reached the end marker attribute ( attributeType = 2^32 - 1 )
		*/
		lastAttribute = NULL;
		position = realSize + 1;
	}
	else if( lastAttribute->attributeType == AttributeAttributeList ) {
		/*
		** now attributes are extracted from other file records
		*/
		ATTRIBUTE_LIST *attributeList = (ATTRIBUTE_LIST *)getAttributeBody();
		ATTRIBUTE_LIST *aux = attributeList;
		int attrListPosition = 0;
		__int64 attributeSize = getAttributeSize();
		__int64 childReferenceNumber;
		
		/*
		** ensure the list is empty
		*/
		externalAttributes.clear();
		
		while( attrListPosition < attributeSize ) {
			attributeList = (ATTRIBUTE_LIST *)((unsigned char *)aux + attrListPosition);
			childReferenceNumber = attributeList->baseReference & 0xffffffffffff;
			
			if( childReferenceNumber != referenceNumber && !inList(childReferenceNumber) ) {
				/*
				** add the reference number in the list
				*/
				externalAttributes.insert(&childReferenceNumber);
			}
			
			attrListPosition += attributeList->entryLength;
		}
		
		/*
		** now obtain the first external attribute
		*/
        eaPosition = 0;
		childRecord = NULL;
		return getExternalAttrHeader();
	}
	
	return lastAttribute;
}


// ****************************************************************************
// *                                                                          *
// * getNextAttributeHeader - gets the next attribute, with filter            *
// *                                                                          *
// ****************************************************************************
ATTRIBUTE_HEADER *FILE_RECORD::getNextAttributeHeader(ATTRIBUTE_TYPE filter) {
	ATTRIBUTE_HEADER *indexAttribute = NULL;
	
	if( !initialized || data == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}
	
	/*
	** reset position
	*/
	resetPosition();
	indexAttribute = getNextAttributeHeader();
	
	/*
	** loop through the attributes until the attribute is found
	*/
	while( indexAttribute != NULL && indexAttribute->attributeType != filter ) {
		indexAttribute = getNextAttributeHeader();
	}
	
	return indexAttribute;
}


// ****************************************************************************
// *                                                                          *
// * getAttributeBody - gets the body of the attribute ( including non-       *
// * resident body )                                                          *
// *                                                                          *
// ****************************************************************************
void *FILE_RECORD::getAttributeBody() {
	RUNLIST        bodyRunlist;
	RUN            run;
	ULARGE_INTEGER offset;
	unsigned long  dataRead;
	
	if( !initialized || data == NULL || position >= realSize || lastAttribute == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}
	
	if( lastAttribute->nonResidentFlag == 0 ) {
		/*
		** the body is resident
		*/
		return (void *)&data[position + lastAttribute->attributes.resident.attributeOffset];
	}
	else {
		/*
		** the body is nonresident, first check the child file record
		*/
		if( eaPosition < externalAttributes.number && childRecord != NULL ) {
			return childRecord->getAttributeBody();
		}
		else {
			/*
			** exclude some attributes
			*/
			if( lastAttribute->attributeType != AttributeIndexRoot &&
				lastAttribute->attributeType != AttributeBitmap && 
                lastAttribute->attributeType != AttributeStandardInformation &&
				lastAttribute->attributeType != AttributeFileName && 
                lastAttribute->attributeType != AttributeAttributeList ) {
				dbgPrint(__WFILE__,__LINE__,L"WARNING: Trying to get body of an excluded attribute (%d)",
                                            lastAttribute->attributeType);
				return NULL;
			}
			
			/*
			** alloccate memory
			*/
			if( attributeBody != NULL ) {
				delete[] attributeBody;
				attributeBody = NULL;
			}
			
			/*
			** check for errors
			*/
			if( lastAttribute->attributes.nonResident.allocatedSize >= 10485760 ) { // over 10 MB !!!
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Attribute body exceeds 10 MB! (corrupt record ?)");
			}
			
			attributeBody = new unsigned char[lastAttribute->attributes.nonResident.allocatedSize];
			
			/*
			** get the runlist
			*/
			bodyRunlist.setRunlist(&data[position + lastAttribute->attributes.nonResident.datarunOffset],
								   lastAttribute->length - lastAttribute->attributes.nonResident.datarunOffset);
			
			while( bodyRunlist.getNextRun(run) ) {
				/*
				** read the run and read the data
				*/
				offset.QuadPart = run.startLCN * clusterSize;
							   
				if( SetFilePointer(volume,offset.LowPart,(long *)&offset.HighPart,FILE_BEGIN) == 
                    INVALID_SET_FILE_POINTER && GetLastError() != NO_ERROR ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set pointer to position %d",offset.QuadPart);
					return 0;	
				}
				
				if( !ReadFile(volume,&attributeBody[run.startVCN * clusterSize],
                              run.length * clusterSize,&dataRead,NULL) && 
                    (dataRead != run.length * clusterSize) ) {
					dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read attribute body. Could read only %d bytes",dataRead);
					return 0;
				}
			}
			
			return (void *)attributeBody;
		}
	}

	return NULL;
}


// ****************************************************************************
// *                                                                          *
// * getAttributeRunlist - gets the runlist of the attribute ( useful for     *
// * DATA and INDEX_ALLOCATION attributes )                                   *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::getAttributeRunlist(RUNLIST &aux) {
	RUNLIST runlist;
	
	if( !initialized || data == NULL || 
        position >= realSize || lastAttribute == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return 0;
	}
	
	/*
	** get the runlist, first check child file record ( if one exists )
	*/
	if( eaPosition < externalAttributes.number && childRecord != NULL ) {
		childRecord->getAttributeRunlist(aux);
	}
	else if( lastAttribute->nonResidentFlag == 1 ) { // only for nonresident attributes
		runlist.setRunlist(&data[position + lastAttribute->attributes.nonResident.datarunOffset],
			lastAttribute->length - lastAttribute->attributes.nonResident.datarunOffset);
			
		aux = runlist;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getAttributeRunlistAsByte - get the raw data of the runlist              *
// *                                                                          *
// ****************************************************************************
inline unsigned char *FILE_RECORD::getAttributeRunlistAsByte() {
	if( !initialized || data == NULL || position >= realSize || lastAttribute == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized or other error");
		return NULL;
	}

	/*
	** get the runlist, first check child file record ( if one exists )
	*/
	if( eaPosition < externalAttributes.number && childRecord != NULL ) {
		return childRecord->getAttributeRunlistAsByte();
	}
	else if( lastAttribute->nonResidentFlag == 1 ) { // only for nonresident attributes
		return &data[position + lastAttribute->attributes.nonResident.datarunOffset];
	}
}


// ****************************************************************************
// *                                                                          *
// * getChildRecords - gets the list with child records                       *
// *                                                                          *
// ****************************************************************************
LLIST<__int64> *FILE_RECORD::getChildRecords() {
	ATTRIBUTE_LIST *attributeList;
	ATTRIBUTE_LIST *aux;
	int attrListPosition = 0;
	__int64 attributeSize;
	__int64 childReferenceNumber;
	
	/*
	** find ATTRIBUTE_LIST attribute
	*/
	ATTRIBUTE_HEADER *attrHeader = getNextAttributeHeader(AttributeAttributeList);
	
	if( attrHeader == NULL ) {
		return NULL;
	}

	attributeList = (ATTRIBUTE_LIST *)getAttributeBody();
	attributeSize = getAttributeSize();
	aux = attributeList;

	/*
	** get the extended attributes
	*/
	externalAttributes.clear();

	while( attrListPosition < attributeSize ) {
		attributeList = (ATTRIBUTE_LIST *)((unsigned char *)aux + attrListPosition);
		childReferenceNumber = attributeList->baseReference & 0xffffffffffff;

		if( childReferenceNumber != referenceNumber && !inList(childReferenceNumber) ) {
			/*
			** add the reference number in the list
			*/
			externalAttributes.insert(&childReferenceNumber);
		}

		attrListPosition += attributeList->entryLength;
	}

	if( externalAttributes.number <= 0 ) {
		return NULL;
	}
	
	return &externalAttributes;
}


// ****************************************************************************
// *                                                                          *
// * getRecordBuffer - gets a pointer to the file record buffer               *
// *                                                                          *
// ****************************************************************************
inline unsigned char *FILE_RECORD::getRecordBuffer() {
	return data;
}


// ****************************************************************************
// *                                                                          *
// * resetPosition - start the reading of attributes from beginning           *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::resetPosition() {
	if( !initialized || data == NULL ) {
		return 0;
	}
	
	position = 0;
	lastAttribute = NULL;
	externalAttributes.clear();
	eaPosition = 0;
	
	/*
	** deallocate memory
	*/
	if( childRecord != NULL ) {
		delete childRecord;
		childRecord = NULL;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
inline int FILE_RECORD::resetObject() {
	if( !initialized || data == NULL ) {
		return 0;
	}
	
	resetPosition();
	initialized = 0;
	
	/*
	** deallocate memory
	*/
	if( data != NULL ) {
		delete[] data;
		data = NULL;
	}
	
	if( attributeBody != NULL ) {
		delete[] attributeBody;
		attributeBody = NULL;
	}
	
	return 1;
}

#endif
