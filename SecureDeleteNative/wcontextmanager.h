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

#ifndef WCONTEXT_MANAGER_H
#define WCONTEXT_MANAGER_H

#include "wcontext.h"
#include "memory.h"
#include "stream.h"
#include "ntfsVolume.h"
#include "entrySearch.h"
#include "file.h"
#include "wstatus.h"
#include "compressed.h"
#include "ntfs.h"
#include "disk.h"
#include "fileInfo.h"
#include "freeSpace.h"
#include "wipe.h"

#define MAXIMUM_WCONTEXT     128
#define WCONTEXT_ID_INTERVAL  28

/*
** error codes
*/
#define ERRORCODE_NO_MORE_CONTEXTS 1
#define ERRORCODE_INVALID_CONTEXT  2
#define ERRORCODE_INVALID_REQUEST  3
#define ERRORCODE_INITIALIZATION   4

#define STATUS_WIPE    1
#define STATUS_PAUSED  2
#define STATUS_STOPPED 3


/*
** definition for COMPLETE_WCONTEXT structure
*/
typedef struct __COMPLETE_WCONTEXT {
    WCONTEXT *wcontext;
    HANDLE    thread;
    int       id;
    int       used;
    int       status;
} COMPLETE_WCONTEXT;


/*
** definition for WIPE_ERROR2 structure ( same as WIPE_ERROR, but without
** dynamic memory allocation )
*/
typedef struct __WIPE_ERROR2 {
    SYSTEMTIME time;
    int        severity;
    wchar_t    message[MAX_PATH * 2];
} WIPE_ERROR2;


// ****************************************************************************
// *                                                                          *
// * wcontextThread - the thread in which a wipe context runs                 *
// *                                                                          *
// ****************************************************************************
unsigned long WINAPI wcontextThread(void *param) {
    COMPLETE_WCONTEXT *completeWContext = (COMPLETE_WCONTEXT *)param;

    completeWContext->wcontext->startWipe();
    completeWContext->status = STATUS_STOPPED; // finished

    return 0;
}


class WCONTEXT_MANAGER {
private:
    COMPLETE_WCONTEXT wcontext[MAXIMUM_WCONTEXT];	
    COMPLETE_WCONTEXT *lastWContext;
    int contextNumber;
    
public:
    WCONTEXT_MANAGER() {
        for(int i = 0;i < MAXIMUM_WCONTEXT;i++) {
            wcontext[i].wcontext = NULL;
            wcontext[i].id = 0;
            wcontext[i].used = 0;
            wcontext[i].status = STATUS_STOPPED;
        }
        
        lastWContext = NULL;
        contextNumber = 0;
    }
    WOPTIONS w;
    int  createContext(int *id);
    int  findContext(int id);
    int  destroyContext(int id);
    int  initializeContext(int id);
    int  startContext(int id);
    int  stopContext(int id);
    int  pauseContext(int id);
    int  resumeContext(int id);
    int  setWipeOptions(int id,WOPTIONS *woptions);
    int  insertWipeObject(int id,WEXTENDED_OBJECT *object);
    int  getContextStatus(int id);
    int  getWipeStatus(int id,WSTATUS *wstatus);
    int  getFailedObjectNumber(int id,int *number);
    int  getFailedObject(int id,int position,WSMALL_OBJECT *failedObject);
    int  getErrorNumber(int id,int *number);
    int  getError(int id,int position,WIPE_ERROR2 *wipeError);
    int  getChildNumber(int id,int *number);
    int  getChildWipeStatus(int id,int child,WSTATUS *wstatus);
    void resetObject();
};


// ****************************************************************************
// *                                                                          *
// * createContext - creates a context object ( not initialized ! )           *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::createContext(int *id) {
    int i;
    
    /*
    ** search first empty slot
    */
    for(i = 0;i < MAXIMUM_WCONTEXT;i++) {
        if( !wcontext[i].used ) {
            break; // found !
        }
    }
    
    if( i == MAXIMUM_WCONTEXT ) {
        dbgPrint(__WFILE__,__LINE__,L"WARNING: All context slots used");
        return ERRORCODE_NO_MORE_CONTEXTS;	
    }
    
    /*
    ** create new context and context id
    */
    wcontext[i].wcontext = new WCONTEXT;
    wcontext[i].id = i * WCONTEXT_ID_INTERVAL;
    wcontext[i].used = 1;
    wcontext[i].status = STATUS_STOPPED;
    wcontext[i].wcontext->parent = NULL; // no parent
    lastWContext = &wcontext[i];
    contextNumber++;
    
    *id = wcontext[i].id;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * findContext - searches a context in the context slots                    *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::findContext(int id) {
    int i;
    
    /*
    ** search context
    */
    if( lastWContext != NULL && contextNumber > 0 ) {
        if( lastWContext->id == id ) {
            return ERRORCODE_SUCCESS;
        }
    }
    
    for(i = 0;i < MAXIMUM_WCONTEXT;i++) {
        if( wcontext[i].used == 1 && wcontext[i].id == id ) {
            break; // found
        }
    }
    
    if( i == MAXIMUM_WCONTEXT ) {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: No more free wipe contexts.");
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        lastWContext = &wcontext[i];
        return ERRORCODE_SUCCESS;
    }
    
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * destroyContext - destroy a context object. Wipe should be stopped before *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::destroyContext(int id) {
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    
    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** destroy context
    */
    lastWContext->wcontext->resetObject();
    delete lastWContext->wcontext;
    
    lastWContext->used = 0;
    contextNumber--;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * initializeContext - initializes a context ( options must be set before ) *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::initializeContext(int id) {
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    /*
    ** initialize context
    */
    if( !lastWContext->wcontext->initialize() ) {
        return ERRORCODE_INITIALIZATION;
    }
    
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * pauseContext - pause wipe on the given context                          *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::pauseContext(int id) {
    if( id != -1 ) {
        /*
        ** find context
        */
        if( findContext(id) != ERRORCODE_SUCCESS ) {
            return ERRORCODE_INVALID_CONTEXT;
        }

        /*
        ** check if valid context
        */
        if( lastWContext->wcontext == NULL ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
        else {
            if( lastWContext->status == STATUS_STOPPED || lastWContext->status == STATUS_PAUSED ) {
                return ERRORCODE_INVALID_CONTEXT;
            }
        }
    }
    
    SuspendThread(lastWContext->thread);
    lastWContext->status = STATUS_PAUSED;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * resumeContext - resumes wipe on the given context (should be called only *
// * if paused)                                                               *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::resumeContext(int id) {
    if( id != -1 ) {
        /*
        ** find context
        */
        if( findContext(id) != ERRORCODE_SUCCESS ) {
            return ERRORCODE_INVALID_CONTEXT;
        }

        /*
        ** check if valid context
        */
        if( lastWContext->wcontext == NULL ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
        else {
            if( lastWContext->status != STATUS_PAUSED ) {
                return ERRORCODE_INVALID_CONTEXT;
            }
        }
    }
    
    ResumeThread(lastWContext->thread);
    lastWContext->status = STATUS_WIPE;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * startWipe - starts wipe on the given context. Now is the thread created  *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::startContext(int id) {
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    /*
    ** create thread
    */
    lastWContext->thread = CreateThread(NULL,0,wcontextThread,
                                        (void *)lastWContext,0,NULL);
    lastWContext->status = STATUS_WIPE;										
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * stopContext - stops wipe on the given context. Sends a stop message to   *
// * the context and waits until it finishes                                  *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::stopContext(int id) { 
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status == STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    /*
    ** stop context
    */
    lastWContext->wcontext->stop();
    if( lastWContext->status == STATUS_PAUSED ) {
        resumeContext(-1); // resume paused thread
    }

    if(lastWContext->wcontext->childrenNumber > 0) {
        lastWContext->wcontext->stopChildren();
        lastWContext->wcontext->waitChildren();
    }
    
    /*
    ** wait until context finishes
    */
    WaitForSingleObject(lastWContext->wcontext->stoppedEvent,INFINITE);
    CloseHandle(lastWContext->thread);
    lastWContext->status = STATUS_STOPPED;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * setWipeOptions - sets the context options. Should be called only when    *
// * stopped                                                                  *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::setWipeOptions(int id,WOPTIONS *woptions) {
    if( woptions == NULL ) {
        return ERRORCODE_UNKNOWN;
    }
    
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    __try {
        lastWContext->wcontext->setOptions(woptions);
    }
    __except( GetExceptionCode() == EXCEPTION_ACCESS_VIOLATION ) {
        return ERRORCODE_INVALID_REQUEST;
    }
    
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * insertWipeObject - inserts an object in the context wipe list. Should be *
// * called only when stopped                                                 *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::insertWipeObject(int id,WEXTENDED_OBJECT *object) {
    if( object == NULL ) {
        return ERRORCODE_UNKNOWN;
    }
    
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_REQUEST;
        }
    }
    
    lastWContext->wcontext->insertObject(object);	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getContextStatus - gets the context status (wiping/paused/stopped)       *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getContextStatus(int id) {
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    
    return lastWContext->status;
}


// ****************************************************************************
// *                                                                          *
// * getWIpeStatus - gets a wipe status (WSTATUS) structure containing        *
// * details about the wipe progress                                          *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getWipeStatus(int id, WSTATUS *wstatus) {
    if( wstatus == NULL ) {
        return ERRORCODE_UNKNOWN;
    }
    
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    
    EnterCriticalSection(&lastWContext->wcontext->cs);
    memcpy(wstatus,&lastWContext->wcontext->wstatus,sizeof(WSTATUS));
    wstatus->context = id;
    LeaveCriticalSection(&lastWContext->wcontext->cs);
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getFailedObjectNumber - gets the number of failed objects. Should be     *
// * called only if stopped                                                   *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getFailedObjectNumber(int id, int *number) {
    if( number == NULL ) {
        return ERRORCODE_UNKNOWN;
    }
    
    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    *number = lastWContext->wcontext->failed.number;	
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getFailedObject - gets a WSMALL_OBJECT structure containing the failed   *
// * object located in the given position                                     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getFailedObject(int id, int position, WSMALL_OBJECT *failedObject) {
    if( failedObject == NULL ) {
        return ERRORCODE_UNKNOWN;
    }

    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }
    
    if( position < 0 || position >= lastWContext->wcontext->failed.number ) {
        return ERRORCODE_INVALID_REQUEST;
    }
    
    /*
    ** get failed object
    */
    memcpy(failedObject,lastWContext->wcontext->failed[position],sizeof(WSMALL_OBJECT));
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getErrorNumber - gets the number of failed objects. Should be called     *
// * only if stopped                                                          *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getErrorNumber(int id, int *number) {
    if( number == NULL ) {
        return ERRORCODE_UNKNOWN;
    }

    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }

    *number = lastWContext->wcontext->log.errorList.number;
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * getFailedObject - gets a WIPE_ERROR structure containing the error       *
// * object located in the given position                                     *
// *                                                                          *
// ****************************************************************************
inline int WCONTEXT_MANAGER::getError(int id, int position, WIPE_ERROR2 *wipeError) {
    WIPE_ERROR aux;
    
    if( wipeError == NULL ) {
        return ERRORCODE_UNKNOWN;
    }

    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }
    else {
        if( lastWContext->status != STATUS_STOPPED ) {
            return ERRORCODE_INVALID_CONTEXT;
        }
    }

    if( position < 0 || position >= lastWContext->wcontext->log.errorList.number ) {
        return ERRORCODE_INVALID_REQUEST;
    }

    /*
    ** get failed object
    */
    aux = lastWContext->wcontext->log.errorList[position];
    wipeError->time = aux.time;
    wipeError->severity = aux.severity;
    wcscpy(wipeError->message,aux.message);
    return ERRORCODE_SUCCESS;
}


inline int WCONTEXT_MANAGER::getChildNumber(int id,int *number) {
    if( number == NULL ) {
        return ERRORCODE_UNKNOWN;
    }

    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    EnterCriticalSection(&lastWContext->wcontext->cs);
    *number = lastWContext->wcontext->childrenNumber;
    LeaveCriticalSection(&lastWContext->wcontext->cs);
    return ERRORCODE_SUCCESS;
}


inline int WCONTEXT_MANAGER::getChildWipeStatus(int id,int child,WSTATUS *wstatus) {
    if( wstatus == NULL ) {
        return ERRORCODE_UNKNOWN;
    }

    /*
    ** find context
    */
    if( findContext(id) != ERRORCODE_SUCCESS ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    /*
    ** check if valid context
    */
    if( lastWContext->wcontext == NULL ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    if( child < 0 || child >= lastWContext->wcontext->childrenNumber ) {
        return ERRORCODE_INVALID_CONTEXT;
    }

    EnterCriticalSection(&lastWContext->wcontext->children[child]->cs);
    memcpy(wstatus,&lastWContext->wcontext->children[child]->wstatus,sizeof(WSTATUS));
    wstatus->context = lastWContext->wcontext->children[child]->id;
    LeaveCriticalSection(&lastWContext->wcontext->children[child]->cs);
    return ERRORCODE_SUCCESS;
}


// ****************************************************************************
// *                                                                          *
// * resetObject - destroy all context objects                                *
// *                                                                          *
// ****************************************************************************
void WCONTEXT_MANAGER::resetObject() {
    for(int i = 0;i < MAXIMUM_WCONTEXT;i++) {
        if( wcontext[i].used ) {
            stopContext(wcontext[i].id);
        }
    }
}

#endif
