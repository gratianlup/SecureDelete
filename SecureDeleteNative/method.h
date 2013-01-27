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

#ifndef METHOD_H
#define METHOD_H

#include <windows.h>
#include <stdio.h>
#include "debug.h"

/*
** definitions for the data type used by a step
*/
#define WSTEP_PATTERN    0
#define WSTEP_RANDOM     1
#define WSTEP_RANDBYTE   2
#define WSTEP_COMPLEMENT 3


class WSTEP {
public:
	unsigned char pattern[256];
	int           patternLength;
	int           patternType;
	
	void setPattern(unsigned char *newPattern,int length);
	WSTEP() {
		patternType = 0;
		patternLength = 0;
	}
};


inline void WSTEP::setPattern(unsigned char *newPattern, int length) {
	memcpy(pattern,newPattern,length);
	patternLength = length;
	patternType = WSTEP_PATTERN;
}


class WMETHOD {
public:
	unsigned long  id;
	wchar_t        name[MAX_PATH];
	wchar_t        path[MAX_PATH];
	WSTEP         *steps;
	int            nSteps;
	
	int            shuffle;
	int            shuffleFirst;
	int            shuffleLast;
	unsigned char *random;
	
	int checkWipe;
	
	WMETHOD() {
		steps = NULL;
		nSteps = 0;
		shuffle = shuffleFirst = shuffleLast = 0;
		random = NULL;
		checkWipe = 0;
	}
	~WMETHOD() {
		if( steps != NULL && nSteps > 0 ) {
			/*
			** free the memory used for the steps
			*/
			delete[] steps;
			nSteps = 0;
			
			if( shuffle ) {
				delete[] random;
				random = NULL;
			}
		}
	}
	
	int  setNSteps(int newNSteps);
	int  setRandom(unsigned char *buffer,int length);
	void rotateRandom();
	int  performShuffle();
	int  saveMethod();
	int  loadMethod(wchar_t *file);
};


// ****************************************************************************
// *                                                                          *
// * setNSteps - aloca memorie pentru pasii de stergere ( nu foloseste lista  *
// * inlantuita pentru viteza )                                               *
// *                                                                          *
// ****************************************************************************
inline int WMETHOD::setNSteps(int newNSteps) {
	if( newNSteps <= 0 || newNSteps > 65365 ) {
		/*
        ** too many steps, probably the data file is corupt
		*/
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Too many wipe steps ( corrupt file? )");
		return 0;
	}
	
	steps = new WSTEP[newNSteps];
	nSteps = newNSteps;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * setRandom - allocate memory and copy the random numbers                  *
// *                                                                          *
// ****************************************************************************
inline int WMETHOD::setRandom(unsigned char *buffer, int length) {
	if( shuffle == 0 || shuffleLast == 0 || 
        length == 0 || length > (shuffleLast - shuffleFirst) ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Invalid parameters (%d) or shuffle not enabled",length);
		return 0;
	}
	
	random = new unsigned char[length];
	memcpy(random,buffer,length);
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * rotateRandom - permutes all bytes from the random buffer to left by 1    *
// * position                                                                 *
// * Ex: 1 2 3 4 5 becomes 2 3 4 5 1                                          *
// *                                                                          *
// ****************************************************************************
inline void WMETHOD::rotateRandom() {
	unsigned char aux = random[0];
	
	for(int i = 0;i < nSteps - 1;i++) {
		random[i] = random[i + 1];
	}
	
	random[nSteps] = aux;
}


// ****************************************************************************
// *                                                                          *
// * performShuffle - shuffles the wipe step order                            *
// *                                                                          *
// ****************************************************************************
inline int WMETHOD::performShuffle() {
	WSTEP aux;
	int   newLocation;

	if( shuffle == 0 || (shuffleLast - shuffleFirst) <= 0 || random == NULL ) {
		/*
		** shuffle is not activated
		*/
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Shuffle is not activated");
		return 0;
	}
	
	for(int i = shuffleFirst;i < shuffleLast;i++) {
		/*
		** compute the new location
		*/
		newLocation = shuffleFirst + (random[i - shuffleFirst] % (shuffleLast - shuffleFirst));
		
		/*
		** swap the current step with the one in its new location 
		*/
		if( newLocation > i ) {
			aux = steps[newLocation];
			steps[newLocation] = steps[i];
			steps[i] = aux;
		}
	}
	
	rotateRandom();
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * saveMethod - save the wipe method in the associated file                 *
// *                                                                          *
// ****************************************************************************
inline int WMETHOD::saveMethod() {
	FILE *f;
	
	/*
	** try to open the file
	*/
	f = _wfopen(path,L"w");
	
	if( f == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not open the destination file");
		return 0;
	}
	
	/*
	** write signature
	*/
	fwprintf(f,L"SDM\n");
	
	/*
	** write general information
	*/
	fwprintf(f,L"id %d\n",id);
	fwprintf(f,L"name %d \"%s\"\n",wcslen(name),name);
	fwprintf(f,L"steps %d\n",nSteps);
	fwprintf(f,L"shuffle %d\n",shuffle);
    
	if( shuffle == 1 ) {
		fwprintf(f,L"shuffleFirst %d\n",shuffleFirst);
		fwprintf(f,L"shuffleLast %d\n",shuffleLast);
	}
    
	fwprintf(f,L"check %d\n",checkWipe);
	
	/*
	** write information about each wipe step
	*/
	for(int i = 0;i < nSteps;i++) {
		fwprintf(f,L"step %d %d",i,steps[i].patternType);
		
		if( steps[i].patternType == WSTEP_PATTERN ) {
			/*
			** write wipe pattern using the decimal values like 123-124-125-...
			*/
			fwprintf(f,L" %d \"",steps[i].patternLength);
			
			for(int j = 0;j < steps[i].patternLength;j++) {
				fwprintf(f,j == steps[i].patternLength - 1 ? L"%d\"" : L"%d ",
                         (int)steps[i].pattern[j]);
			}
		}
		
		fwprintf(f,L"\n");
	}
	
	fclose(f);
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * loadMethod - loads a wipe method from the specified file                 *
// *                                                                          *
// ****************************************************************************
inline int WMETHOD::loadMethod(wchar_t *file) {
	FILE          *f;
	wchar_t        buffer[MAX_PATH];
	unsigned char  pattern[MAX_PATH];
	int            aux,aux2,i;
	
	/*
	** try to open the file
	*/
	wcscpy(path,file);
	f = _wfopen(path,L"r");

	if( f == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not open source file");
		return 0;
	}
	
    /*
	** check if the file contains a wipe method
	*/
	fwscanf(f,L"%s",buffer);
    
	if( wcscmp(buffer,L"SDM") ) {
		fclose(f);
		return 0; // not a sdm file !
	}
	
	while( fwscanf(f,L"%s",buffer) != EOF ) {
		if( wcscmp(buffer,L"id") == 0 ) {
			fwscanf(f,L"%d",&id);
		}
		else if( wcscmp(buffer,L"name") == 0 ) {
			/*
			** read the wipe method name
			*/
			fwscanf(f,L"%d",&aux);
			fwscanf(f,L"%2c",buffer);
			
			if( aux >= MAX_PATH ) {
				fclose(f);
				return 0;
			}
			
			for(i = 0;i <= aux;i++) {
				fwscanf(f,L"%c",&buffer[i]);
			}
			
			if( buffer[i - 1] != L'"' ) { // invalid name
				fclose(f);
				return 0;
			}
			
			buffer[i] = NULL;
			wcscpy(name,buffer);
		}
		else if( wcscmp(buffer,L"steps") == 0 ) {
			/*
			** read the number of steps
			*/
			fwscanf(f,L"%d",&aux);
            
			if( !setNSteps(aux) ) {
				return 0; // number of steps is invalid
			}
		}
		else if( wcscmp(buffer,L"step") == 0 ) {
			/*
			** read information about the wipe step
			*/
			fwscanf(f,L"%d %d",&aux,&aux2);
            
			if( steps == NULL ) {
				dbgPrint(__WFILE__,__LINE__,L"ERROR: Memory for steps not allocated yet");
			}
			
			steps[aux].patternType = aux2;
			
			if( aux2 == WSTEP_PATTERN ) {
				/*
				** read the pattern
				*/
				fwscanf(f,L"%d",&aux2);
				fwscanf(f,L"%2c",buffer);
				
				if( aux2 >= 256 ) {
					fclose(f);
					return 0;
				}
				
				for(i = 0;i < aux2;i++) {
					fwscanf(f,L"%d",&pattern[i]);
				}
                
				pattern[i] = NULL;
				steps[aux].setPattern((unsigned char *)pattern,aux2);
			}
		}
		else if( wcscmp(buffer,L"shuffle") == 0 ) {
			/*
			** read if shuffle is active
			*/
			fwscanf(f,L"%d",&shuffle);
		}
		else if( wcscmp(buffer,L"shuffleFirst") == 0 ) {
			/*
			** read the first step included in shuffle
			*/
			fwscanf(f,L"%d",&shuffleFirst);
		}
		else if( wcscmp(buffer,L"shuffleLast") == 0 ) {
			/*
			** read the last step included in shuffle
			*/
			fwscanf(f,L"%d",&shuffleLast);
		}
		else if( wcscmp(buffer,L"check") == 0 ) {
			/*
			** read if writing on disk should be checked
			*/
			fwscanf(f,L"%d",&checkWipe);
		}
	}
	
	if( steps <= 0 || name[0] == NULL ) {
		fclose(f);
		return 0;
	}
	
	fclose(f);
	return 1;
}

#endif
