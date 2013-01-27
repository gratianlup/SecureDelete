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

#ifndef LOGGER_H
#define LOGGER_H

#include <stdio.h>
#include <windows.h>
#include "linkedList.h"

/*
** definitions for the severity of a message
*/
#define SEVERITY_HIGH   3
#define SEVERITY_MEDIUM 2
#define SEVERITY_LOW    1


class WIPE_ERROR {
public:
	SYSTEMTIME  time;
	wchar_t    *message;
	int         severity;
	
	WIPE_ERROR() {
		message = NULL;
	}
	~WIPE_ERROR() {
		if( message != NULL ) {
			delete[] message;
			message = NULL;
		}
	}
	
	WIPE_ERROR & operator =(WIPE_ERROR &error) {
		if( message != NULL ) {
			delete[] message;
			message = NULL;
		}
		
		message = new wchar_t[wcslen(error.message) + 1];
		wcscpy(message,error.message);
		time = error.time;
		severity = error.severity;
		
		return *this;
	}
};


class LOGGER {
private:
	wchar_t  logFilename[MAX_PATH];
	FILE    *logFile;
	int      logOpen;

	void     clearErrorList();
	void     writeLogStart();
	void     writeLogEnd();
	wchar_t *getLocalTime(int miliseconds);
	wchar_t *getLocalDate();
	
public:
	LLIST<WIPE_ERROR>  errorList;
	
	int minSeverity; // for message filtering
	int useLogfile;
	int append;      // write new messages at the end of the file
	int sizeLimit;
	
	int   openLog(wchar_t *fileName);
	void  closeLog();
	void  addToLog(int severity,wchar_t *format,...);
};


// ****************************************************************************
// *                                                                          *
// * openLog - open the file where the log is written                         *
// *                                                                          *
// ****************************************************************************
inline int LOGGER::openLog(wchar_t *fileName) {
	if( logOpen == 1 ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Log-ul era deja deschis");
	}
	
	wcsncpy(logFilename,fileName, MAX_PATH);
	int appendValue = append;

	// limit file size
	if(sizeLimit > 0) {
		// get file size
		HANDLE fileHandle = CreateFile(logFilename,GENERIC_READ,0,NULL,
                                       OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);

		if(fileHandle != INVALID_HANDLE_VALUE) {
			ULARGE_INTEGER size;
			size.LowPart = GetFileSize(fileHandle,&size.HighPart);

			if(size.LowPart != INVALID_FILE_SIZE && GetLastError() == NO_ERROR) {
				if(size.QuadPart >= sizeLimit) {
					appendValue = false;
				}
			}

			// close the file
			CloseHandle(fileHandle);
		}
	}

	logFile = _wfopen(logFilename,appendValue == 1 ? L"a" : L"w");
    
	if( logFile == NULL ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Nu am putut deschide log-ul");
		return 0;
	}
	
	logOpen = 1;
	writeLogStart();
	return 1;
}


// ****************************************************************************
// *                                                                          *
// * closeLog - closes the log file                                           *
// *                                                                          *
// ****************************************************************************
inline void LOGGER::closeLog() {
	if( logOpen == 1 ) {
		/*
		** scriu partea de sfasit al log-ului ( footer )
		*/
		writeLogEnd();
		fclose(logFile);
		logOpen = 0;
	}
}


// ****************************************************************************
// *                                                                          *
// * clearErrorList - clears the list with errors                             *
// *                                                                          *
// ****************************************************************************
inline void LOGGER::clearErrorList() {
	errorList.clear();
}


// ****************************************************************************
// *                                                                          *
// * getLocalTime - gets the current time as a string                         *
// *                                                                          *
// ****************************************************************************
wchar_t *LOGGER::getLocalTime(int miliseconds) {
	SYSTEMTIME time;
	static wchar_t localTime[MAX_PATH];
	localTime[0] = NULL;
	wchar_t aux[10] = L"";
	
	GetLocalTime(&time);
	
	if( !GetTimeFormat(LOCALE_USER_DEFAULT,TIME_FORCE24HOURFORMAT,&time,L"HH':'mm':'ss",localTime,MAX_PATH) ) {
		dbgPrint(__WFILE__,__LINE__,L"ERROR: Nu am putut formata ora");
		return NULL;
	}
	
	if( miliseconds == 1 ) {
		/*
		** add milliseconds too
		*/
		_itow(time.wMilliseconds,aux,10);
		wcscat(localTime,L":");
		wcscat(localTime,aux);
		
		if( time.wMilliseconds < 100 ) {
			wcscat(localTime,L" ");
		}
	}
	
	return localTime;
}


// ****************************************************************************
// *                                                                          *
// * getLocalDate - gets the current date as a string
// *                                                                          *
// ****************************************************************************
wchar_t * LOGGER::getLocalDate() {
	SYSTEMTIME time;
	static wchar_t localDate[MAX_PATH];
	localDate[0] = NULL;
	wchar_t aux[MAX_PATH] = L"";
	
	GetLocalTime(&time);
	_itow(time.wDay,aux,10);
	wcscat(localDate,aux);
	wcscat(localDate,L"/");
	_itow(time.wMonth,aux,10);
	wcscat(localDate,aux);
	wcscat(localDate,L"/");
	_itow(time.wYear,aux,10);
	wcscat(localDate,aux);
	return localDate;
}


// ****************************************************************************
// *                                                                          *
// * writeLogStart - write the header of a log file                           *
// *                                                                          *
// ****************************************************************************
inline void LOGGER::writeLogStart() {
	if( !logOpen ) {
		/*
		** fisierul nu a fost deschis ( BUG ? )
		*/
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Log-ul nu este deschis. Nu pot scrie header-ul");
		return;
	}
	
	fwprintf(logFile,L"********************************************************************************\n");
	fwprintf(logFile,L"* SECUREDELETE LOG FILE                                                        *\n");
	
	wchar_t *date = getLocalDate();
	wchar_t *time = getLocalTime(0);
	int space = 59 - wcslen(date) - wcslen(time) - 1;
	
	fwprintf(logFile,L"* Wipe started on: %s %s",date,time);
	for(int i = 0;i < space;i++) {
		fwprintf(logFile,L" ");
	}
	fwprintf(logFile,L" *\n");
	fwprintf(logFile,L"********************************************************************************");
	fwprintf(logFile,L"\n\n");
	fflush(logFile);
}


// ****************************************************************************
// *                                                                          *
// * writeLogEnd - writes the footer of a log file                            *
// *                                                                          *
// ****************************************************************************
inline void LOGGER::writeLogEnd() {
	if( !logOpen ) {
		dbgPrint(__WFILE__,__LINE__,L"WARNING: Log-ul nu este deschis. Nu pot scrie header-ul");
		return;
	}
	
	fwprintf(logFile,L"\n");
	fwprintf(logFile,L"********************************************************************************\n");
	
	wchar_t *date = getLocalDate();
	wchar_t *time = getLocalTime(0);
	int space = 61 - wcslen(date) - wcslen(time) - 1;
	fwprintf(logFile,L"* Wipe ended on: %s %s",date,time);
    
	for(int i = 0;i < space;i++) {
		fwprintf(logFile,L" ");
	}
    
	fwprintf(logFile,L" *\n");
	fwprintf(logFile,L"********************************************************************************\n\n");
	fflush(logFile);
}


// ****************************************************************************
// *                                                                          *
// * addToLog - writes an error to the log file                               *
// *                                                                          *
// ****************************************************************************
inline void LOGGER::addToLog(int severity,wchar_t *format,...) {
	wchar_t message[MAX_PATH * 2];
	va_list arg;
	
	/*
	** filter based on severity
	*/
	if( severity <= minSeverity ) {
		dbgPrint(__WFILE__,__LINE__,L"Mesajul nu a trecut de filtrul de severitate");
	}
	
	/*
	** format the message and replace parameters
	*/
	va_start(arg,format);
	vswprintf(message,format,arg);
	va_end(arg);
	
	/*
	** write the message to the log file
	*/
	if( useLogfile == 1 && logOpen != 0 ) {
		fwprintf(logFile,L"[%s] %s\n",getLocalTime(1),message);
		fflush(logFile);
	}
	
	WIPE_ERROR error;
	
	error.message = new wchar_t[wcslen(message) + 1];
	wcscpy(error.message,message);
	GetLocalTime(&error.time);
	error.severity = severity;
	errorList.insert(&error);
}

#endif
