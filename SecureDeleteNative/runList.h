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

#ifndef RUNLIST_H
#define RUNLIST_H

#include <windows.h>
#include "linkedList.h"
#include "debug.h"

/*
** definition for RUN structure
*/
typedef struct __RUN {
	__int64 startLCN;
	__int64 startVCN;
	__int64 length;
} RUN;


class RUNLIST {
private:
	unsigned char *runlist;
	int            runlistLength;
	LLIST<RUN>     runs;
	
	int     position; // position in runlist
	__int64 lastLCN;
	__int64 vcn;
	
	int  getRunSize(int &lengthSize,int &offsetSize);
	void getRunLength(RUN &run,int lengthSize);
	void getRunOffset(RUN &run,int offsetSize);
public:
	RUNLIST() {
		runlist = NULL;
		position = 0;
		lastLCN = 0;
		vcn = 0;
	}
	~RUNLIST() {
		resetObject();
	}
	
	void     setRunlist(unsigned char *newRunList,int length);
	int      getNextRun(RUN &run);
	int      getRuns();
	__int64  getLCN(__int64 VCN);
	int      getRunNumber();
	RUNLIST& operator= (RUNLIST &newRunList);
	void     resetPosition();
	void     resetObject();
};


// ****************************************************************************
// *                                                                          *
// * setRunList - initializes the object with a new runlist                   *
// *                                                                          *
// ****************************************************************************
inline void RUNLIST::setRunlist(unsigned char *newRunList,int length) {
	/*
	** aloccate memory
	*/
	resetObject();
	runlist = new unsigned char[length + 1];
	memcpy(runlist,newRunList,sizeof(unsigned char) * length);
	
	runlistLength = length;
	position = 0;
	lastLCN = 0;
	vcn = 0;
}


// ****************************************************************************
// *                                                                          *
// * getRunSize - gets the size of the offset and length fields               *
// *                                                                          *
// ****************************************************************************
inline int RUNLIST::getRunSize(int &lengthSize, int &offsetSize) {
	/*
	** the size of the length field is in the upper 4 bits. Use the mask
	** 0xf (00001111)
	*/
	lengthSize = (int)(runlist[position] & 0xf);
	
	/*
	** the size of the offset field is in the lower 4 bits. Use the mask
	** 0xf0 (11110000) 
	*/
	offsetSize = (int)(runlist[position] & 0xf0) >> 4;
	
	/*
	** check for errors
	*/
	if( lengthSize > 8 || offsetSize > 8 ) {
		return 0;
	}
	
	if( (lengthSize + offsetSize) > (runlistLength - position -1) || (lengthSize == 0 && offsetSize == 0) ) {
		return 0;
	}
	
	position += 1;
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getRunLength - gets the length ( in clusters ) of the current run        *
// * The length can be 0 if the run is the end run ( 0x00 )                   *
// *                                                                          *
// ****************************************************************************
inline void RUNLIST::getRunLength(RUN &run,int lengthSize) {	
	/*
	** copy lengthSize bytes
	*/
	run.length = 0;
	memcpy(&run.length,&runlist[position],lengthSize);
	
	/*
	** increment position with lengthSize bytes
	*/
	position += lengthSize;
}


// ****************************************************************************
// *                                                                          *
// * getRunOffset - gets the offset ( in clusters ) of the current run        *
// * The offset can be 0 if the run is sparse/compressed or is the end run    *
// *                                                                          *
// ****************************************************************************
inline void RUNLIST::getRunOffset(RUN &run,int offsetSize) {	
	/*
	** copy offsetSize bytes
	*/
	run.startLCN = 0;
	memcpy(&run.startLCN,&runlist[position],offsetSize);
	
	/*
	** check if the offset is a negative number
	** apply mask 1 << (offsetSize * 8 - 1) , ex: 10000000 for offsetSize = 1 byte (128 decimal)
	*/
	if( run.startLCN & ((__int64)1 << (__int64)(offsetSize * 8 - 1)) ) {
		run.startLCN = ((__int64)1 << (__int64)(offsetSize * 8)) - run.startLCN;
		run.startLCN = -run.startLCN;
	}

	/*
	** compute real offset ( the current offset is relative to the previous )
	*/
	run.startLCN += lastLCN;
	lastLCN = run.startLCN;

	/*
	** if offsetSize = 0, the run is sparse and dosen't has an offset
	*/
	if( offsetSize == 0 ) {
		run.startLCN = -1;
	}
	
	position += offsetSize;
}


// ****************************************************************************
// *                                                                          *
// * getNextRun - gets the next run from the runlist                          *
// *                                                                          *
// ****************************************************************************
inline int RUNLIST::getNextRun(RUN &run) {
	int lengthSize;
	int offsetSize;

	if( position >= runlistLength || runlist == NULL ) {
		return 0;
	}
	
	/*
	** get the size of the length and offset fields
	*/
	if( !getRunSize(lengthSize,offsetSize) ) {
		return 0;
	}
	
	/*
	** get the length and the offset
	*/
	getRunLength(run,lengthSize);
	getRunOffset(run,offsetSize);
	
	run.startVCN = vcn;
	vcn += run.length; // increment the VCN
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getRuns - creates a list with all runs. Used by getLCN()                 *
// *                                                                          *
// ****************************************************************************
inline int RUNLIST::getRuns() {
	RUN run;

	if( position >= runlistLength || runlist == NULL ) {
		return 0;
	}
	
	/*
	** ensure the list is empty
	*/
	runs.clear();
	
	while( getNextRun(run) ) {
		runs.insert(&run);
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * getLCN - gets the LCN for the given VCN                                  *
// *                                                                          *
// ****************************************************************************
inline __int64 RUNLIST::getLCN(__int64 VCN) {
	RUN run;
	RUN run2;
	
	if( runs.number == 0 ) {
		if( !getRuns() ) {
			return -1;
		}
	}
		
	for(int i = 0;i < runs.number;i++) {
		run = runs[i];
		run2 = runs[i + 1];
		
		if( run.startVCN <= VCN && VCN < (run.startVCN + run.length) ) {
			/*
			** run found
			*/
			if( run.startLCN == -1 ) {
				return -1;
			}
			
			return run.startLCN + (VCN - run.startVCN);
		}
	}
	
	/*
	** VCN not found
	*/
	return -1;
}


// ****************************************************************************
// *                                                                          *
// * getRunNumber - gets the number of data runs in the current runlist       *
// *                                                                          *
// ****************************************************************************
int RUNLIST::getRunNumber() {
	if( runs.number == 0 ) {
		if( !getRuns() ) {
			return 0;
		}
	}
	
	return runs.number;
}

// ****************************************************************************
// *                                                                          *
// * operator= - copy  RUNLIST objects                                        *
// *                                                                          *
// ****************************************************************************
inline RUNLIST& RUNLIST::operator =(RUNLIST &newRunList) {
	/*
	** allocate memory
	*/
	if( runlist != NULL ) {
		delete[] runlist;
	}
	
	/*
	** copy the runlist
	*/
	runlist = new unsigned char[newRunList.runlistLength + 1];
	memcpy(runlist,newRunList.runlist,sizeof(unsigned char) * newRunList.runlistLength);
	
	/*
	** copy remaining values
	*/
	runlistLength = newRunList.runlistLength;
	position = newRunList.position;
	lastLCN = newRunList.lastLCN;
	runs = newRunList.runs;
	return *this;
}


// ****************************************************************************
// *                                                                          *
// * resetPosition - start the reading of runs from beginning                 *
// *                                                                          *
// ****************************************************************************
inline void RUNLIST::resetPosition() {
	position = 0;
	vcn = 0;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - sets the object in a clean state                           *
// *                                                                          *
// ****************************************************************************
inline void RUNLIST::resetObject() {
	resetPosition();
	
	if( runlist != NULL ) {
		delete[] runlist;
		runlist = NULL;
	}
	
	runs.clear();
}

#endif
