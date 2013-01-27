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

#ifndef MEMORY_H
#define MEMORY_H

#include "wcontext.h"

// ****************************************************************************
// *                                                                          *
// * fillBuffer - fills the write buffer with the given pattern               *
// * Written in assembly language ( ~25% speed increase )                     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::fillBuffer(unsigned char *pattern,long len,unsigned char patternLength) {
	unsigned char *aux = buffer;

    __asm {
        push esi                        // save ESI and EDI
        push edi

        mov esi,dword ptr[aux]          // load buffer address
        mov edx,len                     // load buffer length to be filled
        lea edx,[esi + edx]             // compute the address of the last element
        mov ch,byte ptr [patternLength] // load pattern length in CH ( maximum 255 )
        neg ch                          // negate ch ( implements a very fast loop )

    start:  
        mov cl,ch                       // copy the pattern length counter
        mov edi,dword ptr [pattern]     // load pattern address

    fillLoop:
        movzx al,byte ptr [edi]         // copy from pattern to buffer
        mov [esi],al                    

        add esi,1                       // advance in buffer
        add edi,1                       // advance in pattern
        add cl,1                        // increment pattern position
        jnz fillLoop                    // there are bytes in pattern left

        cmp edx,esi
        jl done                         // reached the buffer end. exit !

        jmp start                       // reached the pattern end, now fill the buffer from pattern beginning
    done:
        pop edi                         // restore ESI and EDI
        pop esi
    }
    
    return 1;
}


// ****************************************************************************
// *                                                                          *
// * setComplement - negate all the bytes in the buffer                       *
// * Written in assembly language for speed                                   *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::setComplement(long len) {
	unsigned char *aux = buffer;
	
    __asm {
        push esi                   // save ESI and EDI
        push edi

        mov esi,dword ptr[aux]     // load buffer address
        mov edx,len                // load buffer length
        lea edx,[esi + edx]        // compute the address of the last element

complementLoop:
        movzx eax,[esi]            // load element from buffer in AL
        neg al                     // negate AL
        mov [esi],al               // move back in memory ALa
        add esi,1                  // increment buffer position

        movzx eax,[esi]            // load element from buffer in AL
        neg al                     // negate AL
        mov [esi],al               // move back in memory AL
        add esi,1                  // increment buffer position

        cmp edx,esi
        jg complementLoop          // stop execution ?

        pop edi                    // restore ESI and EDI
        pop esi
    }

    return 1;
}

// ****************************************************************************
// *                                                                          *
// * fillStepData - fills the write buffer with the data of the given step    *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT::fillStepData(long length, int step) {
	unsigned char ch;
	
	if( wowmethod == NULL || step < 0 || 
        step > wowmethod->nSteps || length == 0 ) {
		return 0;
	}
	
	/*
	** pattern
	*/
	if( wowmethod->steps[step].patternType == WSTEP_PATTERN ) {
		return fillBuffer(wowmethod->steps[step].pattern,length,wowmethod->steps[step].patternLength);
	}
	/*
	** random
	*/
	else if( wowmethod->steps[step].patternType == WSTEP_RANDOM ) {
		return rng.getRandom(buffer,length);
	}
	/*
	** complement
	*/
	else if( wowmethod->steps[step].patternType == WSTEP_COMPLEMENT && step > 0 ) {
		return setComplement(length);
	}
	/*
	** random character
	*/
	else {
		if( !rng.getRandom(&ch,1) ) {
			dbgPrint(__WFILE__,__LINE__,L"Couldn't get random character");
			return 0;
		}
		
		return fillBuffer(&ch,length,1);
	}
	
	return 0;
}

#endif
