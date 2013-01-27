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

// disable deprecation warnings
#define _CRT_SECURE_NO_DEPRECATE

#include "wcontextManager.h"

WCONTEXT_MANAGER manager;

extern "C" int CreateWipeContext(int *context) {
	if( IsBadWritePtr(context, 1) ) {
		return ERRORCODE_UNKNOWN;
	}
	
	return manager.createContext(context);
}

extern "C" int DestroyWipeContext(int context) {
	return manager.destroyContext(context);
}


extern "C" int InitializeWipeContext(int context) {
	return manager.initializeContext(context);
}


extern "C" int StartWipeContext(int context) {
	return manager.startContext(context);
}


extern "C" int StopWipeContext(int context) {
	return manager.stopContext(context);
}


extern "C"  int PauseWipeContext(int context) {
	return manager.pauseContext(context);
}


extern "C" int ResumeWipeContext(int context) {
	return manager.resumeContext(context);
}


extern "C" int SetWipeOptions(int context,WOPTIONS *wipeOptions) {
	if( IsBadReadPtr(wipeOptions, 1) ) {
		return ERRORCODE_UNKNOWN;
	}
	
	return manager.setWipeOptions(context,wipeOptions);
}


extern "C" int InsertWipeObject(int context,WEXTENDED_OBJECT *wipeObject) {
	if( IsBadReadPtr(wipeObject, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.insertWipeObject(context,wipeObject);
}


extern "C" int GetContextStatus(int context) {
	return manager.getContextStatus(context);
}


extern "C" int GetWipeStatus(int context,WSTATUS *wipeStatus) {
	if( IsBadWritePtr(wipeStatus, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getWipeStatus(context,wipeStatus);
}


extern "C" int GetFailedObjectNumber(int context,int *failedNumber) {
	if( IsBadWritePtr(failedNumber, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getFailedObjectNumber(context,failedNumber);
}


extern "C" int GetFailedObject(int context,int position,WSMALL_OBJECT *failedObject) {
	if( IsBadWritePtr(failedObject, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getFailedObject(context,position,failedObject);
}


extern "C" int GetErrorNumber(int context,int *errorNumber) {
	if( IsBadWritePtr(errorNumber, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getErrorNumber(context,errorNumber);
}


extern "C" int GetError(int context,int position,WIPE_ERROR2 *wipeError) {
	if( IsBadWritePtr(wipeError, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getError(context,position,wipeError);
}

extern "C" int GetChildrenNumber(int context,int *childrenNumber) {
	if( IsBadWritePtr(childrenNumber, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getChildNumber(context,childrenNumber);
}

extern "C" int GetChildWipeStatus(int context,int child,WSTATUS *wipeStatus) {
	if( IsBadWritePtr(wipeStatus, 1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getChildWipeStatus(context,child,wipeStatus);
}


/*
** DLL entry point
*/
BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD dwReason, LPVOID lpvReserved) {
	if( dwReason == DLL_PROCESS_DETACH ) {
		/*
		** do cleanup
		*/
		manager.resetObject();
	}

	return 1;
}