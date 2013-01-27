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

#ifndef FILE_HASH_H
#define FILE_HASH_H

#include "linkedList.h"
#include "debug.h"
#include "crc32.h"

/*
** definitions for hash function
*/
#define FILEHASH_LENGTH 9937
#define MAX_VOLUMES       27
#define MAX_LEVELS       260


/*
** definition for FILE_INFO structure
*/
#pragma pack(1)
typedef struct __FILE_INFO {
	unsigned long  crc;
	__int64        aux;                 // reference number under NTFS
	unsigned char *indexAllocation;     // index allocation under NTFS
	unsigned long  runlistLength;
	int            indexAllocationSize;
} FILE_INFO;
#pragma  pack()


class FILELEVEL {
public: 
	SMALL_LLIST<FILE_INFO *> *list[FILEHASH_LENGTH];
	
	FILELEVEL() {
		for(int i = 0;i < FILEHASH_LENGTH;i++) {
			list[i] = NULL;
		}
	}
	~FILELEVEL() {
		SMALL_LLIST<FILE_INFO *> *fileInfo;
		
		for(int i = 0;i < FILEHASH_LENGTH;i++) {
			if( list[i] != NULL ) {
				fileInfo = list[i];

				while( fileInfo != NULL ) {
					if( fileInfo->data->indexAllocation != NULL ) {
						delete[] fileInfo->data->indexAllocation;
					}
					delete fileInfo->data;
					fileInfo = fileInfo->next;
				}

				delete list[i];
				list[i] = NULL;
			}
		}
	}
};


class FILE_HASH {
private:
	FILELEVEL *levels[MAX_LEVELS];
public:
	FILE_HASH() {
		for(int i = 0;i < MAX_LEVELS;i++) {
			levels[i] = NULL;
		}
	}
	~FILE_HASH() {
		resetObject();
	}
	
	unsigned long  elfHash(unsigned char *key,int length);
	int            getFilenameLevel(wchar_t *file,int len);
	void           insertFile(wchar_t *file,int length);
	FILE_INFO     *findFile(wchar_t *file,int length);
	void           resetObject();
};


// ****************************************************************************
// *                                                                          *
// * elfHash - hashes a given key with the string hash algorithm ELF          *
// *                                                                          *
// ****************************************************************************
inline unsigned long FILE_HASH::elfHash(unsigned char *key, int len) {
	unsigned long h = 0;
	
	if( len <= 0 ) {
		return 0;
	}
	
	_asm {
		xor eax,eax      // h
		xor ecx,ecx      // g
		mov esi,key      // key
		mov edi,len
		xor edx,edx      // *key
		
	loopStart:
		/*
		** h = (h << 4) + *key++ 
		*/
		add eax,eax      // |
		add eax,eax      // | => faster on P4 than shl
		add eax,eax      // |
		add eax,eax      // |
		
		mov dl,[esi]
		add dl,1
		add eax,edx
		add esi,1         // advance in key
		
		/*
		** g = h & 0xF0000000L
		*/
		mov ecx,eax
		and ecx,0xF0000000L
		
		/*
		** if( g ) h ^= g >> 24
		*/
		cmp ecx,0
		jne gNotZero
	
		shr ecx,24
		xor eax,ecx
	
	gNotZero:
		/*
		** h &= ~g
		*/
		not ecx
		and eax,ecx
	
		dec edi;
		jnz loopStart
		
		mov h,eax         // finally, copy eax in h
	}
	
	return h;
}


// ****************************************************************************
// *                                                                          *
// * getFilenameLevel - gets the level of the filename from the given path    *
// * Ex. C:\folder\subfolder\file.ext    => level 2                           *
// *          0        1        2
// *                                                                          *
// ****************************************************************************
inline int FILE_HASH::getFilenameLevel(wchar_t *file,int len) {
	int  level = 0;
	
	if( len <= 3 ) {
		return 0;
	}
	
	_asm {
		xor eax,eax           // eax contains number of levels
		mov ecx,len
		sub ecx,1             // don't read NULL
		mov esi,file          // file
		lea edi,[esi + 2*ecx]
		
		/*
		** eliminate \ from end of string
		*/
		mov dx,[edi]
		cmp dl,92             // '\' ?
		jne loopStart         // string dosen't ends with '\'
		sub ecx,1             
		mov [edi],0           // replace with NULL
		
	loopStart:
		mov dx,[esi]
		cmp dl,92             // '\' ?
		jne noSlash           // character not '\'
		add eax,1             // increment level number
		
	noSlash:
		add esi,2             // advance in file
		dec len
		jnz loopStart
		
		sub eax,1             // levels should start from 0
		mov level,eax         // copy in levels
	}
	
	return level;
}


// ****************************************************************************
// *                                                                          *
// * insertFile - inserts a file in the file hash                             *
// *                                                                          *
// ****************************************************************************
void FILE_HASH::insertFile(wchar_t *file,int length) {
	unsigned long             h;
	unsigned long             fileCrc;
	FILE_INFO                *fileInfo;
	wchar_t                  *lastSlash;
	int                       level = 0;
	SMALL_LLIST<FILE_INFO *> *fileInfoList;

	if( file[0] < 'A' || file[0] > 'Z' || file[1] != ':' ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for FILE_HASH::insertFile");
		return;
	}
	
	/*
	** find the last slash \
	*/
	if( file[length] == '\\' ) { // file shouldn't end with '\'
		file[length] = NULL;
		length--;
	}
	
	lastSlash = &file[length];
	
	while( *lastSlash != '\\' && lastSlash > file ) {
		lastSlash--;
	}
	
	level = getFilenameLevel(file,length);
		
	/*
	** compute hash and CRC
	*/
	computeCrc(&fileCrc,(unsigned char *)file,sizeof(wchar_t) * length);
	h = elfHash((unsigned char *)(lastSlash + 1),
                sizeof(wchar_t) * ((&file[length] - lastSlash) - 1));
	h %= FILEHASH_LENGTH;
	
	/*
	** create level ( if it dosen't exist )
	*/
	if( levels[level] == NULL ) {
		levels[level] = new FILELEVEL;
	}
	
	/*
	** create linked list for current key
	*/
	if( levels[level]->list[h] == NULL ) {
		levels[level]->list[h] = new SMALL_LLIST<FILE_INFO *>;
		levels[level]->list[h]->data = NULL;
		levels[level]->list[h]->next = NULL;
	}

	/*
	** allocate a new fileInfo structure and initialize it
	*/
	fileInfo = new FILE_INFO;
	fileInfo->crc = fileCrc;
	fileInfo->aux = 0;
	fileInfo->indexAllocation = NULL;
	fileInfo->indexAllocationSize = 0;
	
	/*
	** insert in the file hash
	*/
	fileInfoList = levels[level]->list[h];
	
	if( fileInfoList->data != NULL ) {
		while( fileInfoList->next != NULL ) {
			fileInfoList = fileInfoList->next;
		}
		
		fileInfoList->next = new SMALL_LLIST<FILE_INFO *>;
		fileInfoList = fileInfoList->next;
		fileInfoList->next = NULL;
	}
	
	fileInfoList->data = fileInfo;
}


// ****************************************************************************
// *                                                                          *
// * findFile - searches a file in the file hash                              *
// *                                                                          *
// ****************************************************************************
inline FILE_INFO *FILE_HASH::findFile(wchar_t *file, int length) {
	unsigned long             h;
	unsigned long             fileCrc;
	wchar_t                  *lastSlash;
	int                       level = 0;
	SMALL_LLIST<FILE_INFO *> *fileInfoList;
	FILE_INFO                *fileInfo;

	/*
	** first check: level
	*/
	level = getFilenameLevel(file,length);
		
	if( levels[level] == NULL ) {
		return NULL;
	}

	/*
	** find the last slash \
	*/
	if( file[length] == '\\' ) { // file shouldn't end with '\'
		file[length] = NULL;
		length--;
	}

	lastSlash = &file[length];

	while( *lastSlash != '\\' && lastSlash > file ) {
		lastSlash--;
	}

	/*
	** compute hash
	*/
	h = elfHash((unsigned char *)(lastSlash + 1),
                sizeof(wchar_t) * ((&file[length] - lastSlash) - 1));
	h %= FILEHASH_LENGTH;
	
	/*
	** second check: list[h]
	*/
	if( levels[level]->list[h] == NULL ) {
		return NULL;
	}
	
	/*
	** third check: find the file in list[h]
	*/
	fileInfoList = levels[level]->list[h];
	
	if( fileInfoList->next == NULL ) {
		return fileInfoList->data;
	}
	else {
		/*
		** search list[h]
		*/
		computeCrc(&fileCrc,(unsigned char *)file,sizeof(wchar_t) * length);
		
		while( fileInfoList != NULL ) {
			fileInfo = fileInfoList->data;
			
			if( fileInfo->crc == fileCrc ) {
				return fileInfo;
			}
			
			fileInfoList = fileInfoList->next;
		}
	}
	
	/*
	** file not found
	*/
	return NULL;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
void FILE_HASH::resetObject() {
	for(int i = 0;i < MAX_LEVELS;i++) {
		/*
		** deallocate memory
		*/
		delete levels[i];
		levels[i] = NULL;
	}
}

#endif
