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

#ifndef ATTRIBUTES_H
#define ATTRIBUTES_H

/*
** definition for ATTRIBUTE_HEADER structure
*/
typedef struct __ATTRIBUTE_HEADER {
	unsigned long	   attributeType;
	short unsigned int length;
	short unsigned int reserverd;
	unsigned char	   nonResidentFlag;
	unsigned char	   nameLength;
	short unsigned int nameOffset;
	short unsigned int flags;
	short unsigned int attributeID;

	union ATTR {
		struct RESIDENT {
			unsigned long      length;
			short unsigned int attributeOffset;
			unsigned char      indexedFlag;
			unsigned char      padding;
		} resident;

		struct NONRESIDENT {
			__int64	           startVCN;
			__int64	           endVCN;
			short unsigned int datarunOffset;
			short unsigned int compressionSize;
			unsigned char      padding[4];
			__int64	           allocatedSize;
			__int64	           realSize;
			__int64	           initializedSize;
		} nonResident;
	} attributes;
} ATTRIBUTE_HEADER;


/*
** definition for ATTRIBUTE_LIST structure
*/
typedef struct __ATTRIBUTE_LIST {
	unsigned long      attributeType;
	short unsigned int entryLength;
	unsigned char      nameLength;
	unsigned char      nameOffset;
	__int64            startVCN;
	__int64            baseReference;
	short unsigned int attributeId;
} ATTRIBUTE_LIST;


/*
** definition for FILENAME structure
*/
typedef struct __FILENAME {
	__int64       parentMFTReference;
	FILETIME      fileCreationTime;
	FILETIME      fileModifiedTime;
	FILETIME      fileMFTChanged;
	FILETIME      fileAccesTime;
	__int64       allocatedSize;
	__int64       realSize;
	unsigned long fileAttributes;
	unsigned long eaReparseTag;
	unsigned char filenameLength;
	unsigned char filenameType;
	wchar_t       filename[0]; 
} FILENAME; 


/*
** definition for STANDARD_INFORMATION structure
*/
typedef struct __STANDARD_INFORMATION {
	FILETIME      fileCreationTime;
	FILETIME      fileModifiedTime;
	FILETIME      fileMFTChanget;
	FILETIME      fileAccesTime;
	unsigned long fileAttributes;
	unsigned long maximumVersions;
	unsigned long versionNumber;
	unsigned long classId;
} STANDARD_INFORMATION;


/*
** definition for INDEX_ROOT ( includes INDEX_NODE ) structure
*/
typedef struct __INDEX_ROOT {
	unsigned long attributeType;
	unsigned long collationRule;
	unsigned long indexAllocationEntrySize;
	unsigned char clustersPerIndexRecord;
	unsigned char aux[3];
	unsigned long indexEntryOffset;
	unsigned long realSize;
	unsigned long allocatedSize;
	unsigned char flags;
	unsigned char padding[3];
} INDEX_ROOT;


/*
** definition for INDEX_HEADER ( includes INDEX_NODE ) structure
*/
typedef struct __INDEX_HEADER {
	unsigned char      magic[4]; // INDX
	short unsigned int fixupArrayOffset;
	short unsigned int fixupArraySize;
	__int64            logFileNumber;
	__int64            indxVCN;
	unsigned long      indexEntryOffset;
	unsigned long      realSize;
	unsigned long      allocatedSize;
	unsigned char      leafNode;
	unsigned char      padding[3];
} INDEX_HEADER;


/*
** definition for INDEX_ENTRY ( for directory index ) structure
*/
typedef struct __INDEX_ENTRY {
	__int64            MFTReference;
	short unsigned int indexEntrySize;
	short unsigned int fileNameOffset;
	short unsigned int indexFlags;
	unsigned char      padding[2];
	__int64            parentMFTReference;
	FILETIME           fileCreation;
	FILETIME           lastModification;
	FILETIME           recordLastModification;
	FILETIME           lastAcces;
	__int64            fileAllocatedSize;
	__int64            fileRealSize;
	__int64            fileFlags;
	unsigned char      filenameLength;
	unsigned char      filenameType;
	wchar_t            fileName[0];
} INDEX_ENTRY;


/*
** definition for VOLUME_INFORMATION structure
*/
typedef struct __VOLUME_INFORMATION {
	__int64            unused;
	char               majorVersion;
	char               minorVersion;
	short unsigned int flags;
	unsigned long      unused2;
} VOLUME_INFORMATION;


/*
** definition for attribute tipe
*/
typedef enum {
	AttributeStandardInformation = 0x10,
	AttributeAttributeList       = 0x20,
	AttributeFileName            = 0x30,
	AttributeObjectId            = 0x40,
	AttributeSecurityDescriptor  = 0x50,
	AttributeVolumeName          = 0x60,
	AttributeVolumeInformation   = 0x70,
	AttributeData                = 0x80,
	AttributeIndexRoot           = 0x90,
	AttributeIndexAllocation     = 0xA0,
	AttributeBitmap              = 0xB0,
	AttributeReparsePoint        = 0xC0,
	AttributeEAInformation       = 0xD0,
	AttributeEA                  = 0xE0,
	AttributePropertySet         = 0xF0,
	AttributeLoggedUtilityStream = 0x100
} ATTRIBUTE_TYPE;

#endif
