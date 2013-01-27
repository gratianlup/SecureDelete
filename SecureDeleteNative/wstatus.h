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

#ifndef WSTATUS_H
#define WSTATUS_H

#include "wcontext.h"

/*
** definitions for status messages
*/
#define WSTATUS_MESSAGE_FILE                   L"Wiping file"
#define WSTATUS_MESSAGE_FREE_SPACE             L"Wiping free space for volume %c:\\"
#define WSTATUS_SUBMESSAGE_FREE_SPACE          L"Wiping free space"
#define WSTATUS_SUBMESSAGE_CLUSTER_TIPS        L"Wiping cluster tip for file %s"
#define WSTATUS_SUBMESSAGE_MFT                 L"Wiping MFT file entries"
#define WSTATUS_MESSAGE_FOLDER_FILE_RECORDS    L"Wiping file records for folder"
#define WSTATUS_SUBMESSAGE_FOLDER_FILE_RECORDS L"%s"
#define WSTATUS_MESSAGE_TOTAL_DELETE           L"Total Delete"
#define WSTATUS_SUBMESSAGE_TOTAL_DELETE        L"Searching file records..."


// ****************************************************************************
// *                                                                          *
// * updateMessage - updates the message ( ex: Wiping file... )               *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateMessage(wchar_t *format,...) {
	va_list arg;
	
	if( format == NULL ) return;
	
	EnterCriticalSection(&cs);
	va_start(arg,format);
	vswprintf_s(wstatus.message,512,format,arg);
	va_end(arg);
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateAuxMessage - updates the second message                            *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateAuxMessage(wchar_t *format,...) {
	va_list arg;

	if( format == NULL ) return;

	EnterCriticalSection(&cs);
	va_start(arg,format);
	vswprintf_s(wstatus.auxMessage,512,format,arg);
	va_end(arg);
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateWipedBytes - updates the number of bytes wiped                     *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateWipedBytes(__int64 n) {
	EnterCriticalSection(&cs);
	wstatus.wipedBytes += n;

	/*
	** announce parent context
	*/
	if(parent != NULL) 
	{
		parent->updateWipedBytes(n);
	}

	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateWobjectTotal - sets the size of the current wipe object            *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateWobjectTotal(__int64 n) {
	if( wstatus.type == WOTYPE_MFT ) {
		return;
	}
	
	EnterCriticalSection(&cs);
	wstatus.objectBytes = n;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateWobjectWiped - updates the number of bytes wiped from the current  *
// * wipe object                                                              *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateWobjectWiped(__int64 n) {
	if( wstatus.type == WOTYPE_MFT || wstatus.type == WOTYPE_CLUSTER_TIPS ) {
		return;
	}
	
	EnterCriticalSection(&cs);
	wstatus.objectWipedBytes += n;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateWobjectWiped2 - updates the number of objects wiped from the       *
// * wipe object                                                              *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateWobjectWiped2(__int64 n) {
	EnterCriticalSection(&cs);
	wstatus.objectWipedBytes += n;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * setWobjectWiped2 - sets the number of bytes wiped from the wipe object   *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::setWobjectWiped(__int64 n) {
	EnterCriticalSection(&cs);
	wstatus.objectWipedBytes = n;
	LeaveCriticalSection(&cs);
}

// ****************************************************************************
// *                                                                          *
// * updateWobjectWiped - resets to 0 the number of bytes wiped from the      *
// * current wipe object                                                      *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::resetWobjectWiped() {
	EnterCriticalSection(&cs);
	wstatus.objectWipedBytes = 0;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateClusterTips - updates the number of bytes in cluster tips          *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateClusterTips(int n) {
	EnterCriticalSection(&cs);
	wstatus.clusterTipsBytes += n;
	wstatus.wipedBytes += n;
	wstatus.totalBytes += n;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * updateStepInfo - updates the info about current step and total steps     *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::updateStepInfo(int step, int totalSteps) {
	EnterCriticalSection(&cs);
	wstatus.step = step;
	wstatus.steps = totalSteps;
	LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * setObjectIndex - sets the index of the object that is currently wiped    *
// *                                                                          *
// ****************************************************************************
inline void WCONTEXT::setObjectIndex() {
	EnterCriticalSection(&cs);
	wstatus.objectIndex++;
	LeaveCriticalSection(&cs);
}

#endif
