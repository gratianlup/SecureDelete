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

#ifndef VOLUME_INFO_H
#define VOLUME_INFO_H

#include <windows.h>
#include "debug.h"

/*
** definition for VOLUME_DATA structure
*/
typedef struct __VOLUME_DATA {
	int            fileSystem;
	int            clusterSize;
	int            sectorSize;
	int            disk;
} VOLUME_DATA;


/*
** definitions for filesystem type
*/
#define FSTYPE_NTFS    0
#define FSTYPE_FAT32   1
#define FSTYPE_FAT     2
#define FSTYPE_UNKNOWN 3


class VOLUME_INFO {
private:
	VOLUME_DATA volumes[27]; // maximum number of volumes ( A...Z )
	
	void getVolumes(int *buffer);
	
public:
	int underWindows2000;
	int cv;               // current volume

	VOLUME_INFO() {
		for(int i = 0;i < 27;i++) {
			volumes[i].fileSystem = FSTYPE_UNKNOWN;
			volumes[i].clusterSize = 0;
			volumes[i].sectorSize = 0;
			volumes[i].disk = -1;
		}
		
		cv = 0;
	}
	
	int createVolumeList();
	int getVolumeDisk(wchar_t volume,int &disk,LARGE_INTEGER &volumeOffset);
	int getVolumeType(int volume);
	int getVolumeClusterSize(int volume);
	int getVolumeSectorSize(int volume);
	int getVolumeDisk(int volume,int &disk);
};


// ****************************************************************************
// *                                                                          *
// * getParttitions - gets all disc volumes                                   *
// *                                                                          *
// ****************************************************************************
inline void VOLUME_INFO::getVolumes(int *buffer) {
	int          ct = 1;
	wchar_t      drive[4];
	unsigned int type;
	
	for(int i = 'A';i <= 'Z';i++) {
		drive[0] = i; drive[1] = ':';
		drive[2] = '\\'; drive[3] = NULL;
		
		type = GetDriveType(drive);
		
		/*
		** add only hdd volumes
		*/
		if ( type != DRIVE_UNKNOWN && 
             type != DRIVE_NO_ROOT_DIR && 
             type != DRIVE_CDROM &&
			 type != DRIVE_REMOTE) {
			buffer[ct++] = i;
		}
	}
	
	buffer[0] = ct;
}


// ****************************************************************************
// *                                                                          *
// * getVolumeSectorSize - gets the sector size of the given volume           *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::getVolumeDisk(wchar_t volume,int &disk,LARGE_INTEGER &volumeOffset) {
	HANDLE               volumeHandle;
	wchar_t              volumeName[] = L"\\\\.\\ :";
	unsigned char        aux[16384];
	VOLUME_DISK_EXTENTS *diskExtents;
	unsigned long        bytesReturned;

	volumeName[4] = volume;

	volumeHandle = CreateFile(volumeName,GENERIC_WRITE | GENERIC_READ,
						      FILE_SHARE_WRITE | FILE_SHARE_READ,NULL,
							  OPEN_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);

	if( volumeHandle == INVALID_HANDLE_VALUE ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't open volume %c",volume);
		return 0;
	}

	/*
	** get the disk where the volume resides
	*/
	diskExtents = (VOLUME_DISK_EXTENTS *)aux;
	if( !DeviceIoControl(volumeHandle,IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS,
		NULL,0,diskExtents,16384,&bytesReturned,NULL) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get volune disk extents for %c",volume);
			CloseHandle(volumeHandle);
			return 0;	 
	}

	CloseHandle(volumeHandle);
	disk = diskExtents->Extents[0].DiskNumber;

	return 1;
}


// ****************************************************************************
// *                                                                          *
// * createVolumeList - creates a list with information about each disc       *
// * volume in the system                                                     *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::createVolumeList() {
	int           vol[27];
	wchar_t       fileSystem[MAX_PATH];
	wchar_t       root[4];
	unsigned long sectorsPerCluster;
    unsigned long bytesPerSector;
    unsigned long numberOfFreeClusters;
    unsigned long totalNumberOfClusters;
    LARGE_INTEGER volumeOffset;
	
	/*
	** get the volumes list
	*/
	getVolumes(vol);
	
	for(int i = 1;i < vol[0];i++) {
		root[0] = vol[i]; root[1] = ':';
		root[2] = '\\';    root[3] = NULL;

		if( !GetVolumeInformation(root,NULL,0,NULL,NULL,NULL,fileSystem,MAX_PATH) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get volume info ( volume %d )",(wchar_t)i);
			continue;
		}
		
		/*
		** set filesystem type
		*/
		if( wcscmp(fileSystem,L"NTFS") == 0 ) {
			volumes[vol[i] - 'A'].fileSystem = FSTYPE_NTFS;
		}
		else if( wcscmp(fileSystem,L"FAT32") == 0 ) {
			volumes[vol[i] - 'A'].fileSystem = FSTYPE_FAT32;
		}
		else if( wcscmp(fileSystem,L"FAT") == 0 ) {
			volumes[vol[i] - 'A'].fileSystem = FSTYPE_FAT;
		}
		else {
			volumes[vol[i] - 'A'].fileSystem = FSTYPE_UNKNOWN;
			
			dbgPrint(__WFILE__,__LINE__,L"WARNING: Unknown file system");
		}
		
		/*
		** set cluster and sector size
		*/
		if( !GetDiskFreeSpace(root,&sectorsPerCluster,&bytesPerSector,
							  &numberOfFreeClusters,&totalNumberOfClusters) ) {
			dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't get cluster and sector size ( volume %d )",(wchar_t)i);
			return 0;
		}
		
		volumes[vol[i] - 'A'].sectorSize  = bytesPerSector;
		volumes[vol[i] - 'A'].clusterSize = bytesPerSector * sectorsPerCluster;
		
		/*
		** get the disk where the volume resides ( only for Win2000+ )
		*/
		if( underWindows2000 ) {
			getVolumeDisk((wchar_t)vol[i],volumes[vol[i] - 'A'].disk,volumeOffset);
		}
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getVolumeType - gets the file system type of the given volume            *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::getVolumeType(int volume) {
	if( volume < 0 || volume >= 27 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid volume info request");
		return FSTYPE_UNKNOWN;
	}
	
	return volumes[volume].fileSystem;
}


// ****************************************************************************
// *                                                                          *
// * getVolumeClusterSize - gets the cluster size of the given volume         *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::getVolumeClusterSize(int volume) {
	if( volume < 0 || volume >= 27 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid volume info request");
		return 0;
	}
	
	return volumes[volume].clusterSize;
}


// ****************************************************************************
// *                                                                          *
// * getVolumeSectorSize - gets the sector size of the given volume           *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::getVolumeSectorSize(int volume) {
	if( volume < 0 || volume >= 27 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid volume info request");
		return 0;
	}
	
	return volumes[volume].sectorSize;
}


// ****************************************************************************
// *                                                                          *
// * getVolumeDiskAndOffset - gets the disk where the volume resides and the  *
// * offset of the first sector of the volume                                 *
// *                                                                          *
// ****************************************************************************
inline int VOLUME_INFO::getVolumeDisk(int volume,int &disk) {
	if( volume < 0 || volume >= 27 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid volume info request");
		return 0;
	}
	
	disk = volumes[volume].disk;
	
	return 1;
}

#endif
