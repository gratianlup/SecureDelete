// Copyright (c) 2006 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
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

#ifndef DEBUG_H
#define DEBUG_H

#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <fstream>


/*
** definition for Unicode __WFILE__ macro
*/
#define WIDEN2(x) L ## x
#define WIDEN(x) WIDEN2(x)
#define __WFILE__ WIDEN(__FILE__)

// ****************************************************************************
// *                                                                          *
// * getFile - returns a pointer to the file name of a path                   *
// *                                                                          *
// ****************************************************************************
inline wchar_t *getFile(wchar_t *s) {
	s += wcslen(s);
	while( *s != '\\' ) s--;
	return s + 1;
}

#ifdef _DEBUG
/*
** definitions for the path of the output and configuration files
*/
#define DEBUG_CONFIG L"debug.config"
#define DEBUG_FILE L"debug.txt"     

typedef struct __DBGOPTIONS {
	int     writeToConsole;
	int     writeToFile;
	int     writeToStudio;
	int     studioBreak;
	int     showMessage;
	int     playSound;
	wchar_t soundFile[MAX_PATH];
} DBGOPTIONS;

/*
** the number of messages displayed since the program started
*/
int count = 0;


// ****************************************************************************
// *                                                                          *
// * readSettings - reads the settings of the debugger                        *
// *                                                                          *
// ****************************************************************************
void readSettings(DBGOPTIONS &options) {
	FILE *f = _wfopen(DEBUG_CONFIG,L"rt");
	wchar_t line[MAX_PATH * 2];

	/*
	** reset settings (no option is activated by default)
	*/
	options.playSound = options.showMessage = options.studioBreak = 
    options.writeToConsole = options.writeToFile = options.writeToStudio = 0;

	if(f == NULL) {
		return;
	}
	
	/*
	** read the options from the data file (one on each line)
	*/
	while( fwscanf(f,L"%s",line) != EOF ) {
		if( wcsncmp(line,L"console",wcslen(L"console")) == 0 ) {
			options.writeToConsole = 1;
		}
		else if( wcsncmp(line,L"file",wcslen(L"file")) == 0 ) {
			options.writeToFile = 1;
		}
		else if( wcsncmp(line,L"studio",wcslen(L"studio")) == 0 ) {
			options.writeToStudio = 1;
		}
		else if( wcsncmp(line,L"break",wcslen(L"break")) == 0 ) {
			options.studioBreak = 1;
		}
		else if( wcsncmp(line,L"message",wcslen(L"message")) == 0 ) {
			options.showMessage = 1;
		}
		else if( wcsncmp(line,L"sound",wcslen(L"sound")) == 0 ) {
			options.playSound = 1;
			
			/*
			** extract the sound file name
			*/
			char ch;
			int ct = 0;
			fwscanf(f,L"%c",&ch);
            
			while( fwscanf(f,L"%c",&ch) && ch != '\n' ) {
				options.soundFile[ct++] = ch;
			}
            
			options.soundFile[ct] = NULL;
		}
	}
	
	fclose(f);
}

// ****************************************************************************
// *                                                                          *
// * getTime - obains the current time in the hh::mm::ss::mili format.        *
// *                                                                          *
// ****************************************************************************
inline wchar_t *getTime() {
	static wchar_t t[100];	
	
	SYSTEMTIME time;
	GetLocalTime(&time);
	swprintf(t,L"%02d:%02d:%02d:%03d",time.wHour,time.wMinute,time.wSecond,time.wMilliseconds);
	return t;
}

inline unsigned long getType(wchar_t *s) {
	wchar_t p[MAX_PATH * 2];
	wcscpy(p,s);
	
	_wcslwr(p);
	
	if( wcsstr(p,L"error")) {
		return MB_ICONERROR;
	}
	else if( wcsstr(p,L"warning")) {
		return MB_ICONWARNING;
	}
	
	return MB_ICONINFORMATION;
}

// ****************************************************************************
// *                                                                          *
// * DBG_PRINT - write the message on the console or to a file                *
// *                                                                          *
// ****************************************************************************
inline void dbgPrint(wchar_t *sourceFile,int lineNumber,wchar_t *format,...) {
	DBGOPTIONS options;               
	wchar_t    message[MAX_PATH * 2];
	wchar_t    cf[MAX_PATH * 3]; // message for console/file
	wchar_t    mb[MAX_PATH * 3]; // message for message box
	va_list    arg;
	
	readSettings(options);
	
	/*
	** add error number
	*/
	swprintf(cf,L"MESSAGE #%d\n",count + 1);
	swprintf(mb,L"MESSAGE #%d\n",count + 1);
	
	/*
	** add time information
	*/
	wcscat(cf,getTime());
	wcscat(cf,L" | ");
	wcscat(mb,L"TIME: ");
	wcscat(mb,getTime());
	wcscat(mb,L"\n");
	
	/*
	** add source file information
	*/
	wcscat(cf,getFile(sourceFile));
	wcscat(mb,L"FILE: ");
	wcscat(mb,getFile(sourceFile));
	wcscat(mb,L"\n");
	
	/*
	** add source line information
	*/
	swprintf(&cf[wcslen(cf)],L"_% 4d",lineNumber);
	wcscat(mb,L"LINE: ");
	swprintf(&mb[wcslen(mb)],L"%d\n",lineNumber);
	
	/*
	** add the message
	*/
	va_start(arg,format);
	vswprintf(message,format,arg);
	va_end(arg);
	
	swprintf(&cf[wcslen(cf)],L" | %s\n",message);
	swprintf(&mb[wcslen(mb)],L"\n%s\n\nStop program execution ?",message);
	
	/*
	** write to the console or to a file
	*/
	if( options.writeToConsole ) {
		wprintf(L"%s\n",cf);
	}
	
	if( options.writeToFile ) {
		FILE *f = _wfopen(DEBUG_FILE,count > 0 ? L"a+" : L"w");
		
		if( f != NULL ) {
			if( count == 0 ) {
				fwprintf(f,L"*********************************************************************\n");
				fwprintf(f,L"* SecureDelete debug file                                           *\n");
				fwprintf(f,L"*********************************************************************\n\n");
			}

			fwprintf(f,L"%s",cf);
			
			fclose(f);
		}
	}
	
    /*
	** write to the Visual Studio output window
	*/
	if( options.writeToStudio ) {
		OutputDebugString(cf);
		
		if( options.studioBreak ) {
			DebugBreak();
		}
	}
	
	/*
	** show a MessageBox
	*/
	if( options.showMessage ) {
		int result = MessageBox(NULL,mb,L"SecureDelete Debugger",MB_YESNO | getType(mb));
		
		if( result == IDYES ) {
			exit(1); // opresc programul
		}
	}
	
	count++;
}
#else
	wchar_t  ringBuffer[64][MAX_PATH * 3];
	int      ringPosition = 0;
	int      ringWasFull = 0;
	int      dump = 0;
	
	inline void dbgPrint(wchar_t *sourceFile,int lineNumber,wchar_t *format,...) {
		wchar_t    message[MAX_PATH * 2];
		wchar_t    cf[MAX_PATH * 3]; // message for console/file
		va_list    arg;

		if( !dump ) {
			return;
		}

		/*
		** add time information
		*/
		wcscpy(cf,getFile(sourceFile));

		/*
		** add source line information
		*/
		swprintf(&cf[wcslen(cf)],L"_% 4d",lineNumber);

		/*
		** add message
		*/
		va_start(arg,format);
		vswprintf(message,format,arg);
		va_end(arg);

		swprintf(&cf[wcslen(cf)],L" | %s\n",message);
		wcscpy(&ringBuffer[ringPosition][0],cf);
		
		ringPosition = (ringPosition + 1) % 512;
		
		if( ringPosition == 0 ) {
			ringWasFull = 1;
		}
	}
	
	inline void dumpRingBuffer() {
		FILE *f = _wfopen(L"dump.txt",L"w");
		printf("\n\ndump started\n");
		
		if( ringWasFull == 1 ) {
			for(int i = ringPosition;i < 512;i++) {
				fwprintf(f,ringBuffer[i]);
				fflush(f);
			}
		}
		
		for(int i = 0;i < ringPosition;i++) {
			fwprintf(f,ringBuffer[i]);
			fflush(f);
		}
		
		fclose(f);
	}
#endif

#endif
