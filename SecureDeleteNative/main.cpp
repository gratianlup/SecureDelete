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

// disable deprecation warnings
#define _CRT_SECURE_NO_DEPRECATE

#include "wcontextManager.h"

WCONTEXT_MANAGER manager;

extern "C" {
__declspec(dllexport) int CreateWipeContext(int *context) {
	return manager.createContext(context);
}

__declspec(dllexport) int DestroyWipeContext(int context) {
	return manager.destroyContext(context);
}


__declspec(dllexport) int InitializeWipeContext(int context) {
	return manager.initializeContext(context);
}


__declspec(dllexport) int StartWipeContext(int context) {
	return manager.startContext(context);
}


__declspec(dllexport) int StopWipeContext(int context) {
	return manager.stopContext(context);
}


__declspec(dllexport)  int PauseWipeContext(int context) {
	return manager.pauseContext(context);
}


__declspec(dllexport) int ResumeWipeContext(int context) {
	return manager.resumeContext(context);
}


__declspec(dllexport) int SetWipeOptions(int context,WOPTIONS *wipeOptions) {
	return manager.setWipeOptions(context,wipeOptions);
}


__declspec(dllexport) int InsertWipeObject(int context,WEXTENDED_OBJECT *wipeObject) {
	return manager.insertWipeObject(context,wipeObject);
}


__declspec(dllexport) int GetContextStatus(int context) {
	return manager.getContextStatus(context);
}


__declspec(dllexport) int GetWipeStatus(int context,WSTATUS *wipeStatus) {
	return manager.getWipeStatus(context,wipeStatus);
}


__declspec(dllexport) int GetFailedObjectNumber(int context,int *failedNumber) {
	return manager.getFailedObjectNumber(context,failedNumber);
}


__declspec(dllexport) int GetFailedObject(int context,int position,WSMALL_OBJECT *failedObject) {
	return manager.getFailedObject(context,position,failedObject);
}


__declspec(dllexport) int GetErrorNumber(int context,int *errorNumber) {
	return manager.getErrorNumber(context,errorNumber);
}


__declspec(dllexport) int GetError(int context,int position,WIPE_ERROR2 *wipeError) {
	return manager.getError(context,position,wipeError);
}

__declspec(dllexport) int GetChildrenNumber(int context,int *childrenNumber) {
	return manager.getChildNumber(context,childrenNumber);
}

__declspec(dllexport) int GetChildWipeStatus(int context,int child,WSTATUS *wipeStatus) {
	return manager.getChildWipeStatus(context,child,wipeStatus);
}
} // extern "C"


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
