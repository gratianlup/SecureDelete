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

#ifndef NTFS_CONTEXT_H
#define NTFS_CONTEXT_H

#include "debug.h"

/*
** definition for IO_STATUS_BLOCK structure
*/
typedef struct __IO_STATUS_BLOCK {
	int           status;
	unsigned long information;
} IO_STATUS_BLOCK;


/*
** definition for APC routine
*/
typedef void (IO_APC_ROUTINE) (
	void            *ApcContext,
	IO_STATUS_BLOCK *IoStatusBlock,
	unsigned long    reserved
);


/*
** definition for NtFsControlFile function ( located in ntdll.dll )
*/
typedef int (__stdcall *NTFSCONTROLFILE)(
	HANDLE           fileHandle,
	HANDLE           event,             // optional
	IO_APC_ROUTINE  *apcRoutine,        // optional
	void            *ApcContext,        // optional
	IO_STATUS_BLOCK *ioStatusBlock,
	unsigned long    FsControlCode,
	void            *InputBuffer,       // optional
	unsigned long    InputBufferLength,
	void            *OutputBuffer,      // optional
	unsigned long    OutputBufferLength
);


/*
** definition for FILE_INFORMATION_CLASS enum
*/
typedef enum __FILE_INFORMATION_CLASS {
	FileDirectoryInformation       = 1,
	FileFullDirectoryInformation, // 2
	FileBothDirectoryInformation, // 3
	FileBasicInformation,         // 4
	FileStandardInformation,      // 5
	FileInternalInformation,      // 6
	FileEaInformation,            // 7
	FileAccessInformation,        // 8
	FileNameInformation,          // 9
	FileRenameInformation,        // 10
	FileLinkInformation,          // 11
	FileNamesInformation,         // 12
	FileDispositionInformation,   // 13
	FilePositionInformation,      // 14
	FileFullEaInformation,        // 15
	FileModeInformation,          // 16
	FileAlignmentInformation,     // 17
	FileAllInformation,           // 18
	FileAllocationInformation,    // 19
	FileEndOfFileInformation,     // 20
	FileAlternateNameInformation, // 21
	FileStreamInformation,        // 22
	FilePipeInformation,          // 23
	FilePipeLocalInformation,     // 24
	FilePipeRemoteInformation,    // 25
	FileMailslotQueryInformation, // 26
	FileMailslotSetInformation,   // 27
	FileCompressionInformation,   // 28
	FileObjectIdInformation,      // 29
	FileCompletionInformation,    // 30
	FileMoveClusterInformation,   // 31
	FileQuotaInformation,         // 32
	FileReparsePointInformation,  // 33
	FileNetworkOpenInformation,   // 34
	FileAttributeTagInformation,  // 35
	FileTrackingInformation,      // 36
	FileMaximumInformation
} FILE_INFORMATION_CLASS;


/*
** definitions for error codes returned by NtFsControlFile
*/
#define STATUS_SUCCESS                0
#define STATUS_BUFFER_OVERFLOW        0x80000005
#define STATUS_INVALID_PARAMETER      0xC000000D
#define STATUS_BUFFER_TOO_SMALL       0xC0000023
#define STATUS_ACCESS_DENIED          0xC0000011
#define STATUS_ALREADY_COMMITTED      0xC0000021
#define STATUS_INVALID_DEVICE_REQUEST 0xC0000010


/*
** definition for NtFsQueryInformationFile
*/
typedef int (__stdcall *NTQUERYINFORMATIONFILE)(
	IN  HANDLE                  fileHandle,
	OUT IO_STATUS_BLOCK        *ioStatusBlock,
	OUT void                   *fileInformation,
	IN  unsigned long           length,
	IN  FILE_INFORMATION_CLASS  fileInformationClass
);


/*
** definitions for function names
*/
#define FUNCTIONNAME_NTFSCONTROLFILE        "NtFsControlFile"
#define FUNCTIONNAME_NTQUERYINFORMATIONFILE "NtQueryInformationFile"

class NTFS_CONTEXT {
private:
	HINSTANCE ntdll;
	
public:
	NTFSCONTROLFILE        NtFsControlFile;
	NTQUERYINFORMATIONFILE NtQueryInformationFile;

	NTFS_CONTEXT() {
		ntdll = NULL;
	}
	~NTFS_CONTEXT() {
		destroy();
	}
	
	int  initialize();
	void destroy();
};


// ****************************************************************************
// *                                                                          *
// * initialize - loads the Nt Native functions from ntdll.dll                *
// *                                                                          *
// ****************************************************************************
inline int NTFS_CONTEXT::initialize() {
	if( ntdll != NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Try to initialize, but initialized already !");
		destroy();
	}
	
	/*
	** load ntdll.dll library
	*/
	ntdll = LoadLibrary(MODULENAME_NTDLL);
	
	if( ntdll == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load NTDLL.DLL");
		return 0;
	}
	
	NtFsControlFile = (NTFSCONTROLFILE)GetProcAddress(ntdll,FUNCTIONNAME_NTFSCONTROLFILE);
	NtQueryInformationFile = (NTQUERYINFORMATIONFILE)GetProcAddress(ntdll,FUNCTIONNAME_NTQUERYINFORMATIONFILE);
	
	if( NtFsControlFile == NULL || NtQueryInformationFile == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Couldn't load NT Native functions");
		return 0;
	}
	
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * destroy - unloads the functions                                          *
// *                                                                          *
// ****************************************************************************
inline void NTFS_CONTEXT::destroy() {
	if( ntdll != NULL ) {
		FreeLibrary(ntdll);
	}
}

#endif
