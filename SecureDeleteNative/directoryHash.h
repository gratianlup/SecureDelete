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

#ifndef DIRECTORY_HASH_H
#define DIRECTORY_HASH_H

#include "linkedList.h"
#include "debug.h"
#include "crc32.h"

/*
** definitions for hash function
*/
#define DIRHASH_LENGTH 9937
#define MAX_LEVELS 260


/*
** definition for DIRLEVEL structure
*/
class DIRLEVEL {
public:
	SMALL_LLIST<unsigned long> *list[DIRHASH_LENGTH];
	
	DIRLEVEL() {
		for(int i = 0;i < DIRHASH_LENGTH;i++) {
			list[i] = NULL;
		}
	}
	~DIRLEVEL() {
		for(int i = 0;i < DIRHASH_LENGTH;i++) {
			/*
			** deallocate memory
			*/
			if( list[i] != NULL ) {		
				delete list[i];
				list[i] = NULL;
			}
		}
	}
};


class DIRECTORY_HASH {
private:
	DIRLEVEL *levels[MAX_LEVELS];
public:
	DIRECTORY_HASH() {
		for(int i = 0;i < MAX_LEVELS;i++) {
			levels[i] = NULL;
		}
	}
	~DIRECTORY_HASH() {
		resetObject();
	}
	
	unsigned long elfHash(unsigned char *key,int len);
	void          insertFile(wchar_t *file,int length);
	int           findFolder(int level,wchar_t *file,int length);
	void          resetObject();
};


// ****************************************************************************
// *                                                                          *
// * elfHash - hashes a given key with the string hash algorithm ELF          *
// *                                                                          *
// ****************************************************************************
inline unsigned long DIRECTORY_HASH::elfHash(unsigned char *key, int len) {
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
		add eax,eax      // faster on P4 than shl
		add eax,eax
		add eax,eax
		add eax,eax

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
// * insertFile - inserts a file in the directory hash                        *
// *                                                                          *
// ****************************************************************************
void DIRECTORY_HASH::insertFile(wchar_t *file,int length) {
	wchar_t                     fileCopy[MAX_PATH + 1];
	wchar_t                    *start;
	wchar_t                    *end;
	unsigned long               h;
	unsigned long               folderCrc;
	SMALL_LLIST<unsigned long> *folderInfoList;
	int                         found = 0;

	if( file == NULL || length <= 0 ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Invalid parameters for insertFile");
		return;
	}
	
	// create a copy
	wcscpy(fileCopy,file);

	/*
	** break the file/folder in subfolders
	*/
	start = wcstok(fileCopy + 3,L"\\");
	
	for(int i = 0;;i++) {
 		end = wcstok(NULL,L"\\");
		if( end == NULL ) break;
		 
		/*
		** compute hash
		*/
		folderCrc = 0;
		computeCrc(&folderCrc,(unsigned char *)start,sizeof(wchar_t) * wcslen(start));
		h = elfHash((unsigned char *)start,sizeof(wchar_t) * wcslen(start));
		h %= DIRHASH_LENGTH;
		
		if( levels[i] == NULL ) {
			/*
			** allocate new level
			*/
			levels[i] = new DIRLEVEL;
		}
		
		if( levels[i]->list[h] == NULL ) {
			levels[i]->list[h] = new SMALL_LLIST<unsigned long>;
		}
		
		/*
		** check if the folder is already in the list
		*/
		folderInfoList = levels[i]->list[h];
		
		while( folderInfoList != NULL ) {
			if( folderInfoList->data == folderCrc ) {
				found = 1;
				break;
			}
			
			folderInfoList = folderInfoList->next;
		}
		
		if( !found ) {
			folderInfoList = levels[i]->list[h];
			if( folderInfoList->data != NULL ) {
				while( folderInfoList->next != NULL ) {
					folderInfoList = folderInfoList->next;
				}

				folderInfoList->next = new SMALL_LLIST<unsigned long>;
				folderInfoList = folderInfoList->next;
				folderInfoList->next = NULL;
			}
			
			folderInfoList->data = folderCrc;
		}
		
		start = end;
	}
}


// ****************************************************************************
// *                                                                          *
// * findFolder - searches a folder in the directory hash at the given level  *
// *                                                                          *
// ****************************************************************************
int DIRECTORY_HASH::findFolder(int level,wchar_t *file,int length) {
	unsigned long h;
	unsigned long crc;
	SMALL_LLIST<unsigned long> *folderInfoList;
	
	/*
	** first check: level
	*/
	if( levels[level] == NULL ) {
		return 0;
	}
	
	/*
	** compute hash
	*/
	h = elfHash((unsigned char *)file,sizeof(wchar_t) * length);
	h %= DIRHASH_LENGTH;
	
	/*
	** second check: list[h]
	*/
	if( levels[level]->list[h] == NULL ) {
		return 0;
	}
	
	/*
	** third check: find the file in list[h]
	*/
	folderInfoList = levels[level]->list[h];
	
	if( folderInfoList->next == NULL ) {
		return 1;
	}
	else {
		/*
		** search list[h]
		*/
		crc = 0;
		computeCrc(&crc,(unsigned char *)file,sizeof(wchar_t) * length);
		
		while( folderInfoList != NULL ) {
			if( folderInfoList->data == crc ) {
				return 1; // found !
			}
			
			folderInfoList = folderInfoList->next;
		}
	}
	
	/*
	** folder not found at given level
	*/
	return 0;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
void DIRECTORY_HASH::resetObject() {
	for(int i = 0;i < MAX_LEVELS;i++) {
		if( levels[i] != NULL ) {
			delete levels[i];
			levels[i] = NULL;
		}
	}
}

#endif
