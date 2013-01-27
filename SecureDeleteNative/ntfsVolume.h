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

#ifndef NTFS_VOLUME_H
#define NTFS_VOLUME_H

#include <windows.h>
#include "debug.h"
#include "mftRecord.h"

/*
** definition for NTFS_BOOT_RECORD structure
*/
#pragma pack(1) // pack on 1-byte boundry
struct NTFS_BOOT_RECORD {
	unsigned char      jump[3];
	__int64            oemId;
	short unsigned int bytesPerSector;
	unsigned char      sectorPerCluster;
	unsigned char      unused1[7];
	unsigned char      mediaDescriptor;
	unsigned char      unused2[2];
	short unsigned int sectorsPerTrack;
	short unsigned int headNumber;
	unsigned char      unused3[8];
	unsigned char      unused4[4];
	__int64            sectorsNumber;
	__int64            MFTStart; 
	__int64            MFTMirrStart;
	char               clusterPerMFTRecord;
	unsigned char      unused5[3];
	char               clusterPerIndexRecord;
	unsigned char      unused6[3];
	__int64            volumeSerial;
};
#pragma pack()


class NTFS_VOLUME {
public:
	HANDLE            volume;
	wchar_t           volumeLetter;
	FILE_RECORD       mft;
	RUNLIST           mftRunlist;
	unsigned char    *mftBitmap;
	int               bitmapSize;
	__int64           mftRealSize;
	__int64           mftAllocatedSize;
	BYTE              aux[512];
	NTFS_BOOT_RECORD *bootRecord;
	
	int volumeOpen;
	int bootRecordReaded;
	int mftReaded;
	
	int           sectorSize;
	int           clusterSize;
	int           fileRecordSize;
	char          volumeMajorVersion;
	char          volumeMinorVersion;
	int           volumeLocked;
	
	NTFS_VOLUME() {
		volume = INVALID_HANDLE_VALUE;
		bootRecord = NULL;
		fileRecordSize = 0;
		volumeOpen = 0;
		bootRecordReaded = 0;
		mftReaded = 0;
		mftBitmap = NULL;
		volumeLocked = 0;
	}
	~NTFS_VOLUME() {
		resetObject();
	}
	
	int            isWindowsVista();
	int            openVolume(wchar_t vol);
	int            readBootRecord();
	int            getFileRecordSize();
	__int64        getMFTOffset();
	int            readMFT();
	int            readVolumeInfo();
	int            initialize(wchar_t vol);
	__int64        getFileRecordOffset(__int64 referenceNumber);
	__int64        getMFTSize();
	unsigned char *getMFTBitmap();
	void           resetObject();
};


// ****************************************************************************
// *                                                                          *
// * isWindowsVista - checks if the program is running under Windows Vista+   *
// *                                                                          *
// ****************************************************************************
inline int NTFS_VOLUME::isWindowsVista() {
	OSVERSIONINFO versionInfo;

	versionInfo.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&versionInfo);

	if( versionInfo.dwMajorVersion >= 6 ) {
		//return 1;
		return 0;
	}

	return 0;
}


// ****************************************************************************
// *                                                                          *
// * openVolume - opens a given volume for low level access                   *
// *                                                                          *
// ****************************************************************************
inline int NTFS_VOLUME::openVolume(wchar_t vol) {
	wchar_t volumeName[] = L"\\\\.\\ :";
	
	if( vol < 'A' || vol > 'Z' ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameter for openVolume %c",vol);
		return 0;
	}
	
	/*
	** volume already open ?
	*/
	if( volumeOpen == 1 ) {
		return 1;
	}

    volumeName[4] = vol;
	volume = CreateFile(volumeName,GENERIC_WRITE | GENERIC_READ,
							FILE_SHARE_WRITE | FILE_SHARE_READ,NULL,
							OPEN_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);
	return volume != INVALID_HANDLE_VALUE;
}


// ****************************************************************************
// *                                                                          *
// * readBootRecord - reads the boot record of the opened volume              *
// *                                                                          *
// ****************************************************************************
inline int NTFS_VOLUME::readBootRecord() {
	unsigned long bytesRead;
	
	if( volume == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: NTFS volume handle invalid");
		return 0;
	}
	
	if( SetFilePointer(volume,0,NULL,FILE_BEGIN) == INVALID_SET_FILE_POINTER ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't set file pointer");
		DWORD a = GetLastError();
		return 0;
	}
	
	if( !ReadFile(volume,aux,512,&bytesRead,NULL) || bytesRead != 512 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read boot record");
		return 0;
	} 
	
	bootRecord = (NTFS_BOOT_RECORD *)&aux[0];
	
	/*
	** sanity check
	*/
	if( memcmp((const void *)&bootRecord->oemId,(const void *)"NTFS",4) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid file system");
		bootRecord = NULL;
		return 0;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getFileRecordSize - gets the size of a mft file record                   *
// *                                                                          *
// ****************************************************************************
inline int NTFS_VOLUME::getFileRecordSize() {
	if( bootRecord == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return 0;
	}
	
	if( bootRecord->clusterPerMFTRecord < 0 ) {
		/*
		** val = -log2(clusterPerMFTRecord) => clusterPerMFTRecord = 2^(-val)
		*/
		return (1 << (-(bootRecord->clusterPerMFTRecord)));
	}
	else {
		/*
		** val = cluster number
		*/
		return bootRecord->clusterPerMFTRecord * 
               bootRecord->bytesPerSector * 
               bootRecord->sectorPerCluster;
	}
}


// ****************************************************************************
// *                                                                          *
// * getMFTOffset - gets the location where MFT starts                        *
// *                                                                          *
// ****************************************************************************
inline __int64 NTFS_VOLUME::getMFTOffset() {	
	if( bootRecord == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return 0;
	}
	
	return bootRecord->MFTStart * 
           __int64(bootRecord->bytesPerSector * 
                   bootRecord->sectorPerCluster);
}


// ****************************************************************************
// *                                                                          *
// * readMFT - reads $MFT                                                     *
// *                                                                          *
// ****************************************************************************
inline int NTFS_VOLUME::readMFT() {
	ULARGE_INTEGER    offset;
	ATTRIBUTE_HEADER *attrHeader;
	unsigned char    *bitmap;
	
	if( bootRecord == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return 0;
	}
	
	fileRecordSize = getFileRecordSize();
	sectorSize = bootRecord->bytesPerSector;
	clusterSize = bootRecord->bytesPerSector * bootRecord->sectorPerCluster;
	
	mft.volume = volume;
	mft.recordSize = fileRecordSize;
	mft.sectorSize = sectorSize;
	mft.clusterSize = clusterSize;
	mft.referenceNumber = 0;

	offset.QuadPart = getMFTOffset();
	
	/*
	** read $MFT's file record
	*/
	if( !mft.readRecord(offset) ) {
		return 0;
	}
	
	/*
	** search the DATA attribute and extract the runlist
	*/
	attrHeader = mft.getNextAttributeHeader(AttributeData);
	if( attrHeader == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: DATA attribute for file $MFT not found");
		return 0;
	}
	
	if( !mft.getAttributeRunlist(mftRunlist) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't obtain runlist for $MFT");
		return 0;
	}
	
	if( !mftRunlist.getRuns() ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't obtain runs from runlist");
		return 0;
	}
	
	mftRealSize = attrHeader->attributes.nonResident.realSize;
	mftAllocatedSize = attrHeader->attributes.nonResident.allocatedSize;
	
	/*
	** search the BITMAP attribute and extract the body
	*/
	mft.resetPosition();
	
	attrHeader = mft.getNextAttributeHeader(AttributeBitmap);
	if( attrHeader == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: BITMAP attribute for file $MFT not found");
		return 0;
	}
	
	bitmap = (unsigned char *)mft.getAttributeBody();
	if( bitmap == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Cooudln't retrieve body for BITMAP attribute");
		return 0;
	}
	
	/*
	** allocate memory for bitmap
	*/
	if( mftBitmap != NULL ) {
		delete[] mftBitmap;
	}
	
	if( attrHeader->nonResidentFlag == 1 ) {
		bitmapSize = attrHeader->attributes.nonResident.realSize;
	}
	else {
		bitmapSize = attrHeader->attributes.resident.length;
	}
	mftBitmap = new unsigned char[bitmapSize];
	
	/*
	** copy the bitmap
	*/
	memcpy(mftBitmap,bitmap,bitmapSize);
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * readVolumeInfo - reads the version of the NTFS used in the volume        *
// *                                                                          *
// ****************************************************************************
int NTFS_VOLUME::readVolumeInfo() {
	FILE_RECORD         volumeRecord;
	ULARGE_INTEGER      offset;
	ATTRIBUTE_HEADER   *attrHeader;
	VOLUME_INFORMATION *volumeInfo;
	
	/*
	** get offset for $Volume
	*/
	offset.QuadPart = getFileRecordOffset(3);
	if( offset.QuadPart == -1 ) {
		return 0;
	}
	
	/*
	** initialize and read file record
	*/
	volumeRecord.volume = volume;
	volumeRecord.recordSize = fileRecordSize;
	volumeRecord.sectorSize = sectorSize;
	volumeRecord.clusterSize = clusterSize;
	volumeRecord.referenceNumber = 3;
	volumeRecord.MFTRunlist = &mftRunlist;
	
	if( !volumeRecord.readRecord(offset) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't read $Volume");
		return 0;
	}
	
	/*
	** search VOLUME_INFORMATION attribute
	*/
	attrHeader = volumeRecord.getNextAttributeHeader(AttributeVolumeInformation);
	if( attrHeader == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get VOLUME_INFORMATION");
		return 0;
	}
	
	volumeInfo = (VOLUME_INFORMATION *)volumeRecord.getAttributeBody();
	if( volumeInfo == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get VOLUME_INFORMATION body");
		return 0;
	}
	
	/*
	** extract NTFS version ( 1.2 on NT, 3.0 on 2000 and 3.1 on XP/2003/Vista )
	*/
	volumeMajorVersion = volumeInfo->majorVersion;
	volumeMinorVersion = volumeInfo->minorVersion;
	
	mftReaded = 1;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * initialize - initializes a NTFS volume                                   *
// *                                                                          *
// ****************************************************************************
int NTFS_VOLUME::initialize(wchar_t vol) {
	resetObject();
	
	if( !openVolume(vol)) {
		return 0;
	}
	
	if( !readBootRecord() ) {
		resetObject();
		return 0;
	}
	
	if( !readMFT() ) {
		resetObject();
		return 0;
	}
	
	if( !readVolumeInfo() ) {
		resetObject();
		return 0;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getFileRecordOffset - gets the offset of a given file offset             *
// *                                                                          *
// ****************************************************************************
inline __int64 NTFS_VOLUME::getFileRecordOffset(__int64 referenceNumber) {
	if( mftRunlist.getRunNumber() <= 0 || mftBitmap == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return -1;
	}
	
	return mftRunlist.getLCN(double(referenceNumber * fileRecordSize) / double(clusterSize)) *
                             clusterSize + (referenceNumber * fileRecordSize) % clusterSize;
}


// ****************************************************************************
// *                                                                          *
// * getMFTSize - gets the allocated size of MFT                              *
// *                                                                          *
// ****************************************************************************
inline __int64 NTFS_VOLUME::getMFTSize() {
	ATTRIBUTE_HEADER *attrHeader;
	
	if( !mftReaded ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return 0;
	}
	
	/*
	** search data attribute
	*/
	attrHeader = mft.getNextAttributeHeader(AttributeData);
	if( attrHeader == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't find DATA atribute for $MFT");
		return 0;
	}
	
	return attrHeader->attributes.nonResident.allocatedSize;
}


// ****************************************************************************
// *                                                                          *
// * getMFTBitmap - gets a pointer to the bitmap of MFT                       *
// *                                                                          *
// ****************************************************************************
inline unsigned char *NTFS_VOLUME::getMFTBitmap() {
	ATTRIBUTE_HEADER *attrHeader;

	if( !mftReaded ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Object not initialized");
		return NULL;
	}

	/*
	** search data attribute
	*/
	attrHeader = mft.getNextAttributeHeader(AttributeBitmap);
	if( attrHeader == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't find BITMAP atribute for $MFT");
		return NULL;
	}
	
	return (unsigned char *)mft.getAttributeBody();
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
void NTFS_VOLUME::resetObject() {
	if( bootRecordReaded ) {
		bootRecord = NULL;
		bootRecordReaded = 0;
	}
	
	if( mftReaded ) {
		mft.resetObject();
		mftRunlist.resetObject();
		mftReaded = 0;
	}
	
	if( mftBitmap != NULL ) {
		delete[] mftBitmap;
		mftBitmap = NULL;
	}
}

#endif
