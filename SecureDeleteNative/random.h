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

#ifndef RANDOM_H
#define RANDOM_H

#include <windows.h>
#include <process.h>
#include <winperf.h>
#include <wincrypt.h>
#include <TlHelp32.h>
#include "md5.h"
#include "isaac32.h"
#include "mersenne_twister.h"

/*
** definitions for the type of the random generator
*/
#define RNG_ISAAC    0
#define RNG_MERSENNE 1


/*
** definitions for the type of pool value mixing
*/
#define QUALITY_REQUIRED 100
#define QUALITY_FAST      25
#define QUALITY_SLOW      75
#define POOL_MIX           4


/*
** definitions for the wait periods
*/
#define TOUCHPOOL_INTERVAL    5000 // 5 secunde 
#define MOVEPOOL_INTERVAL   180000 // 3 minute   
#define POOLUPDATE_COUNT         5

/*
** other definitions
*/
#define MAXIMUM_TOOL_HELP_DATA 16


/*
** definitions for functions from ntdll.dll
*/
typedef DWORD (WINAPI *NTQUERYSYSTEMINFORMATION)(DWORD dwType,DWORD dwData,DWORD dwMaxSize,DWORD dwDataSize);
#define MODULENAME_NTDLL                      L"NTDLL.DLL"
#define FUNCTIONNAME_NTQUERYSYSTEMINFORMATION "NtQuerySystemInformation"

#define PERFORMANCE_BUFFER_SIZE 65536   // porneste la 64K
#define PERFORMANCE_BUFFER_STEP 16384   // pas de 16K


/*
** definitions for NetAPI32
*/
#define MODULENAME_NETAPI L"NETAPI32.DLL"

typedef DWORD (WINAPI *NETSTATISTICSGET)(LPWSTR szServer,LPWSTR szService,
                                          DWORD dwLevel,DWORD dwOptions,LPBYTE *lpBuffer);
typedef DWORD (WINAPI *NETAPIBUFFERSIZE )(LPVOID lpBuffer,LPDWORD cbBuffer);
typedef DWORD (WINAPI *NETAPIBUFFERFREE )(LPVOID lpBuffer);

#define FUNCTIONNAME_NETSTATISTICSGET "NetStatisticsGet"
#define FUNCTIONNAME_NETAPIBUFFERSIZE "NetApiBufferSize"
#define FUNCTIONNAME_NETAPIBUFFERFREE "NetApiBufferFree"

#define RANDOM_KEY_PRODUCTOPTIONS  L"SYSTEM\\CurrentControlSet\\Control\\ProductOptions"
#define RANDOM_KEY_PRODUCTTYPE     L"ProductType"
#define RANDOM_NTWORKSTATION_TOKEN L"WinNT"

#undef SERVICE_WORKSTATION
#undef SERVICE_SERVER
#define SERVICE_WORKSTATION (L"LanmanWorkstation")
#define SERVICE_SERVER      (L"LanmanServer")


/*
** definitions for CryptoAPI
*/
#define MODULENAME_ADVAPI32 L"ADVAPI32.DLL"

typedef BOOL (WINAPI *CRYPTACQUIRECONTEXT)(HCRYPTPROV *,LPCTSTR,LPCTSTR,DWORD,DWORD);
typedef BOOL (WINAPI *CRYPTGENRANDOM)(HCRYPTPROV,DWORD,BYTE *);
typedef BOOL (WINAPI *CRYPTRELEASECONTEXT)(HCRYPTPROV,DWORD);

#define FUNCTIONNAME_CRYPTACQUIRECONTEXT "CryptAcquireContextA"
#define FUNCTIONNAME_CRYPTGENRANDOM      "CryptGenRandom"
#define FUNCTIONNAME_CRYPTRELEASECONTEXT "CryptReleaseContext"

#ifndef INTEL_DEF_PROV
    #define INTEL_DEF_PROV L"Intel Hardware Cryptographic Service Provider"
#endif


/*
** definition of the thread that thouches the random generator data
** periodically to prevent the OS to swap it
*/
unsigned long WINAPI swapThread(void *lpParam);


/*
** describes the random number generator options
*/
typedef struct __RNG_OPTIONS {
    int randomProvider;
    int useSlowPool;
    int preventWriteToSwap;
    int reseed;
    int reseedInterval;
    int poolUpdateInterval;
} RNG_OPTIONS;


class RNG {
private:
    int                initialized;
    unsigned char      pool[512];
    int                position;
    randctx_isaac32    isaac32;
    randctx_mersenne32 mersenne32;
    CRITICAL_SECTION   cs;
    HANDLE             swapT;
    
    int fixedItemsAdded;
    int quality;

    void addBytes(void *data,int length);
    void mixPool();
    void destroyData(void *data,int length);
    void fastPool();
    void slowPool();
    void slowPoolPerf();
    void slowPoolToolHelp();
    void slowPoolNetwork();
    void slowPoolCrypto();
public:
    RNG_OPTIONS options;
    int         stopExecution;
    HANDLE      swapEvent;
    
    RNG() {
        initialized = 0;
    }
    ~RNG() {
        disableRNG();
    }
    
    int           initRNG();
    void          disableRNG();
    void          setOptions(RNG_OPTIONS &newOptions);
    int           getRandom(unsigned char *buffer,int length);
    unsigned long getULong();
    void          createSeed();
    void          initISAAC_MERSENNE();
    void          touchPool();
    void          movePool();
};


// ****************************************************************************
// *                                                                          *
// * initRNG - initializes the random number generator                        *
// *                                                                          *
// ****************************************************************************
inline int RNG::initRNG() {
    /*
    ** resetez pool
    */
    memset(pool,0,sizeof(unsigned char) * 512);
    position = 0;
    swapT = NULL;
    swapEvent = NULL;
    
    InitializeCriticalSection(&cs);

    /*
    ** allocate memory
    */
    if( options.randomProvider == RNG_ISAAC ) {
        isaac32.randmem = (unsigned long *)VirtualAlloc(NULL,RANDSIZ * sizeof(unsigned long),MEM_COMMIT,PAGE_READWRITE);

        if( isaac32.randmem == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for ISAAC.randmem");
            return 0;
        }

        VirtualLock(isaac32.randmem,RANDSIZ * sizeof(unsigned long));
        isaac32.randrsl = (unsigned long *)VirtualAlloc(NULL,RANDSIZ * sizeof(unsigned long),MEM_COMMIT,PAGE_READWRITE);

        if( isaac32.randrsl == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for ISAAC.randmem");
            return 0;
        }
        
        VirtualLock(isaac32.randrsl,RANDSIZ * sizeof(unsigned long));
    }
    else {
        /*
        ** MERSENNE TWISTER used instead
        */
        mersenne32.mt = (unsigned long *)VirtualAlloc(NULL,Nr * sizeof(unsigned long),MEM_COMMIT,PAGE_READWRITE);
        if( mersenne32.mt == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for mersenne32.mt");
            return 0;
        }
        VirtualLock(mersenne32.mt,Nr * sizeof(unsigned long));
    }
    
    /*
    ** create seed
    */
    fixedItemsAdded = 0;
    quality = 0;
    createSeed();

    /*
    ** initialize ISAAC or MERSENNE TWISTER
    */
    initISAAC_MERSENNE();
    
    /*
    ** start swapThread
    */
    if( options.preventWriteToSwap || options.reseed ) {
        unsigned long id;
        swapT = CreateThread(NULL,0,swapThread,this,0,&id);
        swapEvent = CreateEvent(NULL,0,0,NULL);
    }
    
    initialized = 1;
    return 1;
}


// ****************************************************************************
// *                                                                          *
// * disableRNG - destroys the random number generator                        *
// *                                                                          *
// ****************************************************************************
inline void RNG::disableRNG() {
    if( !initialized ) {
        return;
    }

    /*
    ** free the used data buffers
    */
    destroyData(pool,512);
    position = 0;
    
    /*
    ** stop the swap thread
    */
    if( swapT != NULL ) {
        /*
        ** wait until the thread terminates and frees its data
        */
        stopExecution = 1;
        WaitForSingleObject(swapEvent,5000);
        CloseHandle(swapT);
        CloseHandle(swapEvent);
        
    }

    DeleteCriticalSection(&cs);
    
    /*
    ** free the memory used by ISAAC or MERSENNE TWISTER
    */
    if( options.randomProvider == RNG_ISAAC ) {
        destroyData(isaac32.randmem,RANDSIZ * sizeof(unsigned long));
        destroyData(isaac32.randrsl,RANDSIZ * sizeof(unsigned long));
        
        VirtualUnlock(isaac32.randmem,RANDSIZ * sizeof(unsigned long));
        VirtualFree(isaac32.randmem,0,MEM_RELEASE);
        VirtualUnlock(isaac32.randrsl,RANDSIZ * sizeof(unsigned long));
        VirtualFree(isaac32.randrsl,0,MEM_RELEASE);
        destroyData(&isaac32,sizeof(randctx_isaac32));
    }
    else {
        destroyData(mersenne32.mt,Nr * sizeof(unsigned long));
        VirtualUnlock(mersenne32.mt,Nr * sizeof(unsigned long));
        VirtualFree(mersenne32.mt,0,MEM_RELEASE);
        destroyData(&mersenne32,sizeof(randctx_mersenne32));
    }
    
    initialized = 0;
}


// ****************************************************************************
// *                                                                          *
// * setOptions - sets the options to be used by the random number generator  *
// *                                                                          *
// ****************************************************************************
inline void RNG::setOptions(RNG_OPTIONS &newOptions) {
    options = newOptions;

    if( options.reseed ) {
        options.poolUpdateInterval = options.reseedInterval / POOLUPDATE_COUNT;
    }
}


// ****************************************************************************
// *                                                                          *
// * destroyData - modifies the data buffers so that the original values      *
// * can no longer be recorwered                                              *
// *                                                                          *
// ****************************************************************************
inline void RNG::destroyData(void *data, int length) {
    unsigned char *aux = (unsigned char *)data;

    for(int i = 0;i < length;i++) {
        aux[i] ^= ~(aux[i] | 21);
    }
    
    memset(aux, 0,sizeof(unsigned char) * length);
}


// ****************************************************************************
// *                                                                          *
// * addBytes - adds new data to the pool and combines with the existing data *
// *                                                                          *
// ****************************************************************************
inline void RNG::addBytes(void *data, int length) {
    unsigned char *aux = (unsigned char *)data;

    for(int i = 0;i < length;i++) {
        pool[position] ^= aux[i];
        position = (position + 1) % 512;
        
        /*
        ** mix the pool if it is full
        */
        if( position == 0 ) {
            mixPool();		
        }
    }
}


// ****************************************************************************
// *                                                                          *
// * mixPool - mixes the pool using the MD5 algorithm                         *
// * |--- 64b ---|----------- 448b-----------|                                *
// * |...........|...........................|                                *
// * \   | ^ |   /                                                            *
// *  \  | | |  /   ==> MD5 applied with 16b step                             *
// *   \ |   | /                                                              *
// *    \|16b|/                                                               *
// *                                                                          *
// ****************************************************************************
inline void RNG::mixPool() {
    unsigned char temp[65];
    md5Context    md5;
    
    /*
    ** step 1 - copy the last 24 bytes
    */
    memcpy(temp,&pool[487],24);
    
    /*
    ** copy the first 36 bytes
    */
    memcpy(&temp[24],pool,36);
    md5Start(&md5);
    md5Update(&md5,temp,64);
    md5Finish(&md5,pool);
    
    /*
    ** step 2 - copty the last 8 bytes
    */
    memcpy(temp,&pool[503],8);
    
    /*
    ** copy the first 56 bytes
    */
    memcpy(&temp[8],pool,56);
    md5Start(&md5);
    md5Update(&md5,temp,64);
    md5Finish(&md5,&pool[16]);
    
    /*
    ** the rest of the steps don't reach the margins
    */
    for(unsigned int i = 2;i < 30;i++) {
        md5Start(&md5);
        md5Update(&md5,&pool[i * 16 - 24],64);
        md5Finish(&md5,&pool[i * 16]);
    }
    
    /*
    ** step 31 - copy 56 bytes from the end of the array
    */
    memcpy(temp,&pool[455],56);
    
    /*
    ** copy 8 bytes from the start of the array
    */
    memcpy(&temp[56],pool,8);
    md5Start(&md5);
    md5Update(&md5,temp,64);
    md5Finish(&md5,&pool[488]);

    /*
    ** step 32 - copy 40 bytes from the end of the array
    */
    memcpy(temp,&pool[471],40);
    
    /*
    ** copy 24 bytes from the start of the array
    */
    memcpy(&temp[40],pool,24);
    md5Start(&md5);
    md5Update(&md5,temp,64);
    md5Finish(&md5,&pool[495]);

    destroyData(temp,65);
    destroyData((unsigned char *)&md5,sizeof(md5Context));
}


// ****************************************************************************
// *                                                                          *
// * fastPool - add to the pool info that is fast to access and collect       *
// *                                                                          *
// ****************************************************************************
inline void RNG::fastPool() {
    ULARGE_INTEGER freeBytesAvailableToCaller;
    ULARGE_INTEGER totalNumberOfBytes;
    ULARGE_INTEGER totalNumberOfFreeBytes;
    wchar_t        drive[10];
    int            driveType;
    
    for(int i = 'C';i <= 'M';i++) {
        swprintf(drive,L"%c:\\",i);
        
        /*
        ** interogate only hdds (other units might have a large delay or
        ** might not even be inserted, like Cds or floppies.
        */
        driveType = GetDriveType(drive);
        
        if( driveType != DRIVE_UNKNOWN && driveType != DRIVE_NO_ROOT_DIR &&
            driveType != DRIVE_CDROM && driveType != DRIVE_REMOTE &&
            driveType != DRIVE_REMOVABLE ) {
            if( !GetDiskFreeSpaceEx(drive,&freeBytesAvailableToCaller,&totalNumberOfBytes,&totalNumberOfFreeBytes) ) {
                dbgPrint(__WFILE__,__LINE__,L"WARNING: Nu am putut obtine spatiul liber pentru unitatea %c",i);
            }
            else {
                addBytes(&freeBytesAvailableToCaller,sizeof(ULARGE_INTEGER));
                addBytes(&freeBytesAvailableToCaller,sizeof(ULARGE_INTEGER));
                addBytes(&freeBytesAvailableToCaller,sizeof(ULARGE_INTEGER));
            }
        }
    }
    
    /*
    ** add keyboard cursor position
    */
    POINT caretPos;
    if( !GetCaretPos(&caretPos) ) {
        dbgPrint(__WFILE__,__LINE__,L"WARNING: Nu am putut obtine pozitia carret-ului");
    }
    else {
        addBytes(&caretPos.x,sizeof(long));
        addBytes(&caretPos.y,sizeof(long));
    }
    
    /*
    ** add mouse cursor position
    */
    POINT cursorPos;
    if( !GetCursorPos(&cursorPos) ) {
        dbgPrint(__WFILE__,__LINE__,L"WARNING: Nu am putut obtine pozitia cursoruui (mouse)");
    }
    else {
        addBytes(&cursorPos.x,sizeof(long));
        addBytes(&cursorPos.y,sizeof(long));
    } 
    
    /*
    ** add info about the current process and the open windows
    */
    unsigned long aux;
    
    aux = (unsigned long)GetActiveWindow();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetCapture();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetClipboardOwner();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetClipboardViewer();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetCurrentProcess();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetCurrentProcessId();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetCurrentThread();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetCurrentThreadId();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetDesktopWindow();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetFocus();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetForegroundWindow();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetInputState();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetMessagePos();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetMessageTime();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetOpenClipboardWindow();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetProcessHeap();
    addBytes(&aux,sizeof(unsigned long));
    
    aux = (unsigned long)GetProcessWindowStation();
    addBytes(&aux,sizeof(unsigned long));
    
    /*
    ** adaug informatii despre cantitatea de memorie libera
    */
    MEMORYSTATUS ms;
    ms.dwLength = sizeof(MEMORYSTATUS);
    GlobalMemoryStatus(&ms);
    addBytes(&ms,sizeof(MEMORYSTATUS));
    
    /*
    ** add info about the current thread
    */
    FILETIME       ftCreationTime;
    FILETIME       ftExitTime;
    FILETIME       ftKernelTime;
    FILETIME       ftUserTime;
    ULARGE_INTEGER setSize;
    
    if( !GetThreadTimes(GetCurrentThread(),&ftCreationTime,&ftExitTime,&ftKernelTime,&ftUserTime) ) {
        dbgPrint(__WFILE__,__LINE__,L"WARNING: could not obtain info about the current thread");
    }
    else {
        addBytes(&ftCreationTime,sizeof(FILETIME));
        addBytes(&ftExitTime,sizeof(FILETIME));
        addBytes(&ftKernelTime,sizeof(FILETIME));
        addBytes(&ftUserTime,sizeof(FILETIME));
    }
    
    if( GetThreadTimes(GetCurrentProcess(),&ftCreationTime,&ftExitTime,&ftKernelTime,&ftUserTime) ) {
        addBytes(&ftCreationTime,sizeof(FILETIME));
        addBytes(&ftExitTime,sizeof(FILETIME));
        addBytes(&ftKernelTime,sizeof(FILETIME));
        addBytes(&ftUserTime,sizeof(FILETIME));
    }
    
    if( !GetProcessWorkingSetSize(GetCurrentProcess(),&setSize.LowPart,&setSize.HighPart)) {
        dbgPrint(__WFILE__,__LINE__,L"WARNING: could not obtain info about the application size");
    }
    else {
        addBytes(&setSize,sizeof(ULARGE_INTEGER));
    }
    
    /*
    ** add current time information
    */
    LARGE_INTEGER perf;
    aux = GetTickCount();
    addBytes(&aux,sizeof(unsigned long));
    
    QueryPerformanceCounter(&perf);
    addBytes(&perf,sizeof(LARGE_INTEGER));
    
    
    /*
    ** add information which doesn't changes until the OS restarts a single time
    */
    if( !fixedItemsAdded ) {
        TIME_ZONE_INFORMATION tzi;
        OSVERSIONINFO         osv;
        SYSTEM_INFO           si;
        STARTUPINFO           sa;
        
        aux = (unsigned long)GetSystemMetrics(SM_CXSCREEN);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemMetrics(SM_CYSCREEN);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemMetrics(SM_CXHSCROLL);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemMetrics(SM_CYHSCROLL);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemMetrics(SM_CXMAXIMIZED);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemMetrics(SM_CYMAXIMIZED);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSysColor(COLOR_3DFACE);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSysColor(COLOR_DESKTOP);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSysColor(COLOR_INFOBK);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSysColor(COLOR_WINDOW);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetDialogBaseUnits();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemDefaultLangID();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetSystemDefaultLCID();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetOEMCP();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetACP();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetOEMCP();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetKeyboardLayout(0);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetKeyboardType(0);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetKeyboardType(2);
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetDoubleClickTime();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetCaretBlinkTime();
        addBytes(&aux,sizeof(unsigned long));
        
        aux = (unsigned long)GetLogicalDrives();
        addBytes(&aux,sizeof(unsigned long));
        
        if( GetTimeZoneInformation(&tzi) != TIME_ZONE_ID_INVALID ) {
            addBytes(&tzi,sizeof(TIME_ZONE_INFORMATION));
        }
        
        /*
        **	info about the used OS
        */
        osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);

        if( !GetVersionEx(&osv) ) {
            dbgPrint(__WFILE__,__LINE__,L"WARNING: Nu am putut obtine versiunea Windows");
        }
        else {
            addBytes(&osv,sizeof(OSVERSIONINFO));
        }
        
        GetSystemInfo(&si);
        addBytes(&si,sizeof(SYSTEM_INFO));
        sa.cb = sizeof(STARTUPINFO);
        GetStartupInfo(&sa);
        addBytes(&sa,sizeof(STARTUPINFO));
        
        fixedItemsAdded = 1;
        destroyData(&tzi,sizeof(TIME_ZONE_INFORMATION));
        destroyData(&osv,sizeof(OSVERSIONINFO));
        destroyData(&si,sizeof(SYSTEM_INFO));
        destroyData(&sa,sizeof(STARTUPINFO));	
    }
    
    /*
    **	increase pool information "quality"
    */
    quality += QUALITY_FAST;
    
    /*
    **	destroy the temporary variables
    */
    destroyData(&freeBytesAvailableToCaller,sizeof(ULARGE_INTEGER));
    destroyData(&totalNumberOfBytes,sizeof(ULARGE_INTEGER));
    destroyData(&totalNumberOfFreeBytes,sizeof(ULARGE_INTEGER));
    destroyData(drive,sizeof(char) * 10);
    destroyData(&driveType,sizeof(int));
    destroyData(&caretPos,sizeof(POINT));
    destroyData(&cursorPos,sizeof(POINT));
    destroyData(&aux,sizeof(unsigned long));
    destroyData(&ms,sizeof(MEMORYSTATUS));
    destroyData(&ftCreationTime,sizeof(FILETIME));
    destroyData(&ftExitTime,sizeof(FILETIME));
    destroyData(&ftKernelTime,sizeof(FILETIME));
    destroyData(&ftUserTime,sizeof(FILETIME));
    destroyData(&setSize,sizeof(ULARGE_INTEGER));
    destroyData(&perf,sizeof(LARGE_INTEGER));
}


// ****************************************************************************
// *                                                                          *
// * createSeed - creates a 4096 bit number used to initialize                *
// * the associated random number generated using the pool data               *
// *                                                                          *
// ****************************************************************************
inline void RNG::createSeed() {
    EnterCriticalSection(&cs);
    
    while( quality < QUALITY_REQUIRED ) {
        fastPool();
        
        /*
        **	use more information if desired
        */
        if( options.useSlowPool == 1 ) {
            slowPool();
        }
    }
    
    /*
    **	mix the pool again after adding all the info
    */
    for(int i = 0;i < POOL_MIX;i++) {
        mixPool();
    }
    
    LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * initISAAC_MERSENNE - initializes the random generator with the seed      *
// *                                                                          *
// ****************************************************************************
void RNG::initISAAC_MERSENNE() {
    EnterCriticalSection(&cs);

    if( options.randomProvider == RNG_ISAAC ) {
        /*
        ** initialize ISAAC
        */
        memcpy((unsigned char *)isaac32.randrsl,pool,RANDSIZ * 2);
        memcpy((unsigned char *)isaac32.randrsl + RANDSIZ / 2,pool,RANDSIZ * 2);
        randinit32(&isaac32,1);
    }
    else {
        /*
        ** initialize MERSENNE TWISTER
        */
        init_by_array((DWORD *)pool,512,&mersenne32);
    }
    
    LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * slowPoolToolHelp - adds information about the active processes           *
// *                                                                          *
// ****************************************************************************
inline void RNG::slowPoolToolHelp() {
    PROCESSENTRY32 pe32;
    THREADENTRY32  te32;
    MODULEENTRY32  me32;
    HEAPLIST32     hl32;
    HEAPENTRY32    he32;
    HANDLE         snapshot = NULL;
    int            ctOuter;
    int            ctInner;
    
    /*
    ** get the list of active processes
    */
    snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPALL,0);
    
    if( snapshot == NULL ) {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: could not obtain ToolHelp32 handle");
        return;
    }
    
    hl32.dwSize = sizeof(HEAPLIST32);
    he32.dwSize = sizeof(HEAPENTRY32);
    
    if( Heap32ListFirst(snapshot,&hl32) ) {
        /*
        ** add the locations of all objects found in the heap
        */
        ctOuter = 0;

        do {
            addBytes(&hl32,sizeof(HEAPLIST32));
            ctInner = 0;
            
            if( Heap32First(&he32,hl32.th32ProcessID,hl32.th32HeapID) ) {
                do {
                    addBytes(&he32,sizeof(HEAPENTRY32));
                    ctInner++;
                } while( Heap32Next(&he32) && ctInner < MAXIMUM_TOOL_HELP_DATA );
            }

            ctOuter++;
        } while( Heap32ListNext(snapshot,&hl32) && ctOuter < MAXIMUM_TOOL_HELP_DATA );
    }

    /*
    ** iterate the process list
    */
    pe32.dwSize = sizeof(PROCESSENTRY32);
    
    if( Process32First(snapshot,&pe32) ) {
        do {
            addBytes(&pe32,sizeof(PROCESSENTRY32));
        } while( Process32Next(snapshot,&pe32) );
    }
    
    /*
    ** iterate the thread list
    */
    te32.dwSize = sizeof(THREADENTRY32);
    
    if( Thread32First(snapshot,&te32) ) {
        ctOuter = 0;

        do {
            addBytes(&te32,sizeof(THREADENTRY32));
            ctOuter++;
        } while( Thread32Next(snapshot,&te32) && ctOuter < MAXIMUM_TOOL_HELP_DATA );    
    }
    
    /*
    ** iterate the list with the loaded modules
    */
    if( Module32First(snapshot,&me32) ) {
        ctOuter = 0;
        do {
            addBytes(&me32,sizeof(MODULEENTRY32));
            ctOuter++;
        } while( Module32Next(snapshot,&me32) && ctOuter < MAXIMUM_TOOL_HELP_DATA );
    }
    
    /*
    ** ToolHelp handle no longer required
    */
    CloseHandle(snapshot);
    
    /*
    ** destroy the temporary variables
    */
    destroyData(&pe32,sizeof(PROCESSENTRY32));
    destroyData(&te32,sizeof(THREADENTRY32));
    destroyData(&me32,sizeof(MODULEENTRY32));
    destroyData(&hl32,sizeof(HEAPLIST32));
    destroyData(&he32,sizeof(HEAPENTRY32));
}


// ****************************************************************************
// *                                                                          *
// * slowPoolPerf - adauga statistice despre performanta unitatilor HDD si    *
// * informatii de sistem obtinute prin functia nedocumentata din ntdll.dll,  *
// * NtQuerySystemInfo ( este incarcata dinamic )                             *
// *                                                                          *
// ****************************************************************************
inline void RNG::slowPoolPerf() {
    HINSTANCE                ntdll;
    NTQUERYSYSTEMINFORMATION NtQuerySystemInformation;
    PPERF_DATA_BLOCK         perfData = NULL;
    unsigned long            size;
    int                      status;
    
    perfData = (PPERF_DATA_BLOCK)new unsigned char[PERFORMANCE_BUFFER_SIZE];
    ntdll = LoadLibrary(MODULENAME_NTDLL);

    if( ntdll == NULL ) {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load NTDLL.DLL");
    }
    else {
        NtQuerySystemInformation = (NTQUERYSYSTEMINFORMATION)GetProcAddress(ntdll,FUNCTIONNAME_NTQUERYSYSTEMINFORMATION);

        if( NtQuerySystemInformation == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load NtQuerySystemInformation");
            FreeLibrary(ntdll);
        }
    }
    
    if( NtQuerySystemInformation != NULL ) {
        /*
        ** obtain performance statistics
        */
        for(int i = 0;i < 64;i++) { // scanez doar primele 64 de tipuri de informatii
            size = PERFORMANCE_BUFFER_SIZE;
            status = NtQuerySystemInformation(i,(unsigned long)perfData,32768,(unsigned long)&size);

            if( status == ERROR_SUCCESS && size > 0 ) {
                addBytes(perfData,size);
            }
        }
    }
    
    FreeLibrary(ntdll);
    
    /*
    ** get statistics about I/O performance for each drive
    */
    DISK_PERFORMANCE diskPerf;
    wchar_t          deviceName[MAX_PATH];
    HANDLE           device;
    
    for(int i = 0; ;i++) {
        swprintf(deviceName,L"\\\\.\\PhysicalDrive%d",i);
        
        device = CreateFile(deviceName,0,FILE_SHARE_READ | FILE_SHARE_WRITE,
                            NULL,OPEN_EXISTING,0,NULL);
                            
        if( device == INVALID_HANDLE_VALUE ) {
            continue;
        }
        
        if( DeviceIoControl(device,IOCTL_DISK_PERFORMANCE,NULL,0,&diskPerf,
                            sizeof(DISK_PERFORMANCE),&size,NULL)) {
            /*
            ** add the statistics to the pool
            */
            addBytes(&diskPerf,size);
        }
        else {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Failed to get performance statistics for drive %s",deviceName);
        }
        
        CloseHandle(device);
    }
    
    /*
    ** destroy the temporary variables
    */
    destroyData(perfData,sizeof(unsigned char) * PERFORMANCE_BUFFER_SIZE);
    delete[] perfData;
    destroyData(&diskPerf,sizeof(DISK_PERFORMANCE));
}


// ****************************************************************************
// *                                                                          *
// * slowPoolNetwork - add netword statistics                                 *
// *                                                                          *
// ****************************************************************************
void RNG::slowPoolNetwork() {
    HINSTANCE        netapi;
    NETSTATISTICSGET NetStatisticsGet;
    NETAPIBUFFERSIZE NetApiBufferSize;
    NETAPIBUFFERFREE NetApiBufferFree;
    
    /*
    ** load NetAPI32 functions
    */
    netapi = LoadLibrary(MODULENAME_NETAPI);
    if( netapi == NULL ) {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load NetAPI32");
        return;
    }
    else {
        NetStatisticsGet = (NETSTATISTICSGET)GetProcAddress(LoadLibrary(MODULENAME_NETAPI),FUNCTIONNAME_NETSTATISTICSGET);
        NetApiBufferSize = (NETAPIBUFFERSIZE)GetProcAddress(LoadLibrary(MODULENAME_NETAPI),FUNCTIONNAME_NETAPIBUFFERSIZE);
        NetApiBufferFree = (NETAPIBUFFERFREE)GetProcAddress(LoadLibrary(MODULENAME_NETAPI),FUNCTIONNAME_NETAPIBUFFERFREE);
        
        /*
        ** close NETAPI32 if one of the functions could not be found
        */
        if( NetStatisticsGet == NULL || NetApiBufferSize == NULL ||
            NetApiBufferFree == NULL ) {
            FreeLibrary(netapi);
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load NetAPI32 functions");
            return;	
        }
    }
    
    /*
    ** check if the OS is a Server version or a standard (Workstation) version.
    */
    int  server = 0;
    HKEY key;
    
    if( RegOpenKeyEx(HKEY_LOCAL_MACHINE,RANDOM_KEY_PRODUCTOPTIONS,0,KEY_READ,&key) == ERROR_SUCCESS) {
        unsigned char value[32];
        unsigned long status;
        unsigned long size = sizeof(value);

        status  = RegQueryValueEx(key,RANDOM_KEY_PRODUCTTYPE,0,NULL,value,&size);
        if( status == ERROR_SUCCESS && _wcsicmp((const wchar_t *)value,RANDOM_NTWORKSTATION_TOKEN) ) {
            server = 1;
        }

        RegCloseKey(key);
    }
    else {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load registry key %s",RANDOM_KEY_PRODUCTOPTIONS);
    }
    
    /*
    ** add network statistics to the pool
    */
    unsigned char *buffer = NULL;
    unsigned long  size;
    
    if( NetStatisticsGet(NULL,server ? SERVICE_SERVER : SERVICE_WORKSTATION,0,0,&buffer) == 0 ) {
        NetApiBufferSize(buffer,&size);
        addBytes(buffer,size);
        NetApiBufferFree(buffer);
    }
    
    FreeLibrary(netapi);
}


// ****************************************************************************
// *                                                                          *
// * slowPoolCrypto - add data provided by CryptoAPI                          *
// *                                                                          *
// ****************************************************************************
void RNG::slowPoolCrypto() {
    HINSTANCE           advapi;
    HCRYPTPROV          cryptoContext;
    CRYPTACQUIRECONTEXT CryptAcquireContext;
    CRYPTGENRANDOM      CryptGenRandom;
    CRYPTRELEASECONTEXT CryptReleaseContext;
    int                 providerFound = 0;
    unsigned char       aux[129];
    
    /*
    ** load ADVAPI32 and the CryptoAPI functions
    */
    advapi = LoadLibrary(MODULENAME_ADVAPI32);
    if( advapi == NULL ) {
        dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load ADVAPI32");
        return;
    }
    else {
        CryptAcquireContext = (CRYPTACQUIRECONTEXT)GetProcAddress(advapi,FUNCTIONNAME_CRYPTACQUIRECONTEXT);
        CryptGenRandom = (CRYPTGENRANDOM)GetProcAddress(advapi,FUNCTIONNAME_CRYPTGENRANDOM);
        CryptReleaseContext = (CRYPTRELEASECONTEXT)GetProcAddress(advapi,FUNCTIONNAME_CRYPTRELEASECONTEXT);
        
        if( CryptAcquireContext == NULL || CryptGenRandom == NULL ||
            CryptReleaseContext == NULL ) {
            /*
            ** one of the functions could not be loaded, close ADVAPI32
            */
            FreeLibrary(advapi);
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not load CryptoAPI functions");
            return;	
        }
    }
    
    /*
    ** try to get a CryptoAPI context with Intel generator ( very low chance to find one though )
    */
    if( CryptAcquireContext(&cryptoContext,NULL,INTEL_DEF_PROV,PROV_INTEL_SEC,0) ) {
        providerFound = 1;
    }
    else {
        if( CryptAcquireContext(&cryptoContext,NULL,NULL,PROV_RSA_FULL,0) ) {
            providerFound = 1;
        }
        else if( GetLastError() == NTE_BAD_KEYSET ) {
            /*
            ** no chey is defined, create one now
            */
            dbgPrint(__WFILE__,__LINE__,L"Creating CryptoAPI key");

            if( CryptAcquireContext(&cryptoContext,NULL,NULL,PROV_RSA_FULL,CRYPT_NEWKEYSET) ) {
                providerFound = 1;
            }
        }
    }
    
    if( providerFound ) {
        /*
        ** get some random numbers from CryptoAPI
        */
        if( !CryptGenRandom(cryptoContext,128,aux) ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not get random numbers from CryptoAPI");
        }
        else {
            /*
            ** add the obtained random numbers to the pool
            */
            addBytes(aux,sizeof(unsigned char) * 128);
            destroyData(aux,sizeof(unsigned char) * 128);
        }
    }
    
    CryptReleaseContext(cryptoContext,0);
    FreeLibrary(advapi);
}


// ****************************************************************************
// *                                                                          *
// * slowPool - adds much more information to the pool, but it is slow        *
// *                                                                          *
// ****************************************************************************
inline void RNG::slowPool() {
    slowPoolToolHelp();
    slowPoolPerf();
    slowPoolNetwork();
    slowPoolCrypto();

    /*
    ** increase the pool "quality"
    */
    quality += QUALITY_SLOW;
}


// ****************************************************************************
// *                                                                          *
// * getRandom - fill the buffer with random numbers                          *
// *                                                                          *
// ****************************************************************************
inline int RNG::getRandom(unsigned char *buffer,int length) {
    unsigned long aux;
    int           count = 0;
    
    EnterCriticalSection(&cs);
    
    /*
    ** fill the buffer with random numbers.
    */
    while( length >= 7 ) {
        if(options.randomProvider == RNG_ISAAC) {
            aux = isaacRand32(&isaac32);
        }
        else {
            aux = genrand_int32(&mersenne32);
        }
                
        /*
        ** split the numbers in 4 bytes
        */
        *(buffer + 0) = (unsigned char)(aux     );
        *(buffer + 1) = (unsigned char)(aux >>  8);
        *(buffer + 2) = (unsigned char)(aux >> 16);
        *(buffer + 3) = (unsigned char)(aux >> 24);
        buffer += 4;
                
        /*
        ** repeat
        */
        if(options.randomProvider == RNG_ISAAC) {
            aux = isaacRand32(&isaac32);
        }
        else {
            aux = genrand_int32(&mersenne32);
        }

        *(buffer + 0) = (unsigned char)(aux     );
        *(buffer + 1) = (unsigned char)(aux >>  8);
        *(buffer + 2) = (unsigned char)(aux >> 16);
        *(buffer + 3) = (unsigned char)(aux >> 24);
        buffer += 4;
                
        length -= 8;
        count += 8;
    }

    // Fill the remaining bytes.
    for(int i = 0;i < length;i++) {
        if(options.randomProvider == RNG_ISAAC) {
            aux = isaacRand32(&isaac32);
        }
        else {
            aux = genrand_int32(&mersenne32);
        }

        *buffer = (unsigned char)aux;
        buffer++;
    }
  
    LeaveCriticalSection(&cs);
    return 1;
}


// ****************************************************************************
// *                                                                          *
// * getULong - get a 32 bit random number                                    *
// *                                                                          *
// ****************************************************************************
inline unsigned long RNG::getULong() {
    unsigned long aux = 0;
    
    EnterCriticalSection(&cs);

    if( options.randomProvider == RNG_ISAAC ) {
        aux = isaacRand32(&isaac32);
    }
    else {
        aux = genrand_int32(&mersenne32);
    }
    
    LeaveCriticalSection(&cs);
    return aux;
}


// ****************************************************************************
// *                                                                          *
// * touchPool - touch the memory locations used to keep the pool             *
// * and the random generator internal data                                   * 
// *                                                                          *
// ****************************************************************************
inline void RNG::touchPool() {
    unsigned long aux;
    
    EnterCriticalSection(&cs);
    
    /*
    ** access the first, last and middle elelments
    */
    if( options.randomProvider == RNG_ISAAC ) {
        aux = isaac32.randmem[0];
        isaac32.randmem[0] = 0xFFFFFFFF;
        isaac32.randmem[0] = aux;
            
        aux = isaac32.randmem[RANDSIZ / 2 - 1];
        isaac32.randmem[RANDSIZ / 2 - 1] = 0xFFFFFFFF;
        isaac32.randmem[RANDSIZ / 2 - 1] = aux;
            
        aux = isaac32.randmem[RANDSIZ - 1];
        isaac32.randmem[RANDSIZ - 1] = 0xFFFFFFFF;
        isaac32.randmem[RANDSIZ - 1] = aux;
            
        aux = isaac32.randrsl[0];
        isaac32.randrsl[0] = 0xFFFFFFFF;
        isaac32.randrsl[0] = aux;

        aux = isaac32.randrsl[RANDSIZ / 2 - 1];
        isaac32.randrsl[RANDSIZ / 2 - 1] = 0xFFFFFFFF;
        isaac32.randrsl[RANDSIZ / 2 - 1] = aux;

        aux = isaac32.randrsl[RANDSIZ - 1];
        isaac32.randrsl[RANDSIZ - 1] = 0xFFFFFFFF;;
        isaac32.randrsl[RANDSIZ - 1] = aux;
    }
    else {
        aux = mersenne32.mt[0];
        mersenne32.mt[0] = 0xFFFFFFFF;
        mersenne32.mt[0] = aux;

        aux = mersenne32.mt[Nr / 2 - 1];
        mersenne32.mt[Nr / 2 - 1] = 0xFFFFFFFF;
        mersenne32.mt[Nr / 2 - 1] = aux;

        aux = mersenne32.mt[Nr - 1];
        mersenne32.mt[Nr - 1] = 0xFFFFFFFF;
        mersenne32.mt[Nr - 1] = aux;
    }

    LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * movePool - move the pool and random generator internal data              *
// * to another memory location.                                              *
// *                                                                          *
// ****************************************************************************
inline void RNG::movePool() {
    unsigned long auxisaac[RANDSIZ];
    unsigned long auxmersenne[Nr];
    
    EnterCriticalSection(&cs);
    
    if( options.randomProvider == RNG_ISAAC ) {
        memcpy(auxisaac,isaac32.randmem,sizeof(unsigned long) * RANDSIZ);
        destroyData(isaac32.randmem,sizeof(unsigned long) * RANDSIZ);
        VirtualUnlock(isaac32.randmem,sizeof(unsigned long) * RANDSIZ);
        VirtualFree(isaac32.randmem,0,MEM_RELEASE);
        
        isaac32.randmem = (unsigned long *)VirtualAlloc(NULL,sizeof(unsigned long) * RANDSIZ,MEM_COMMIT,PAGE_READWRITE);

        if( isaac32.randmem == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for ISAAC.randmem");
        }

        VirtualLock(isaac32.randmem,sizeof(unsigned long) * RANDSIZ);
        memcpy(isaac32.randmem,auxisaac,sizeof(unsigned long) * RANDSIZ);
        
        // --------------------------------------------------------------------
        
        memcpy(auxisaac,isaac32.randrsl,sizeof(unsigned long) * RANDSIZ);
        destroyData(isaac32.randrsl,sizeof(unsigned long) * RANDSIZ);
        VirtualUnlock(isaac32.randrsl,sizeof(unsigned long) * RANDSIZ);
        VirtualFree(isaac32.randrsl,0,MEM_RELEASE);

        isaac32.randrsl = (unsigned long *)VirtualAlloc(NULL,sizeof(unsigned long) * RANDSIZ,MEM_COMMIT,PAGE_READWRITE);
        if( isaac32.randrsl == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for ISAAC.randrsl");
        }

        VirtualLock(isaac32.randrsl,sizeof(unsigned long) * RANDSIZ);
        memcpy(isaac32.randrsl,auxisaac,sizeof(unsigned long) * RANDSIZ);
        destroyData(auxisaac,sizeof(unsigned long) * RANDSIZ);
    }
    else {
        memcpy(auxmersenne,mersenne32.mt,sizeof(unsigned long) * Nr);
        VirtualUnlock(mersenne32.mt,sizeof(unsigned long) * Nr);
        VirtualFree(mersenne32.mt,0,MEM_RELEASE);
        
        mersenne32.mt = (unsigned long *)VirtualAlloc(NULL,sizeof(unsigned long) * Nr,MEM_COMMIT,PAGE_READWRITE);

        if( mersenne32.mt == NULL ) {
            dbgPrint(__WFILE__,__LINE__,L"ERROR: Could not allocate memory for MERSENNE.mt");
        }
        
        VirtualLock(mersenne32.mt,Nr * sizeof(unsigned long));
        memcpy(mersenne32.mt,auxmersenne,sizeof(unsigned long) * Nr);
        destroyData(auxmersenne,sizeof(unsigned long) * Nr);
    }
    
    LeaveCriticalSection(&cs);
}


// ****************************************************************************
// *                                                                          *
// * swapThread                                                               *
// *                                                                          *
// ****************************************************************************
unsigned long WINAPI swapThread(void *lpParam) {
    RNG           *rng = (RNG *)lpParam;
    unsigned long  touchTime = GetTickCount();
    unsigned long  moveTime = touchTime;
    unsigned long  poolUpdateTime = touchTime;
    unsigned long  reseedTime = touchTime;
    unsigned long  time;
    
    while( 1 ) {
        time = GetTickCount();
        if( time - touchTime >= TOUCHPOOL_INTERVAL ) {
            if( rng->stopExecution == 1 ) {
                SetEvent(rng->swapEvent);
                return 0;
            }
            rng->touchPool();			
            touchTime = GetTickCount();
        }
        
        time = GetTickCount();
        if( time - moveTime >= MOVEPOOL_INTERVAL ) {
            if( rng->stopExecution == 1 ) {
                SetEvent(rng->swapEvent);
                return 0;
            }
            rng->movePool();
            moveTime = GetTickCount();
        }
        
        time = GetTickCount();
        if( time - poolUpdateTime >= rng->options.poolUpdateInterval ) {
            if( rng->stopExecution == 1 ) {
                SetEvent(rng->swapEvent);
                return 0;
            }
            rng->createSeed();
            poolUpdateTime = GetTickCount();
        }
        
        time = GetTickCount();
        if( rng->options.reseed == 1 && 
            (time - reseedTime >= rng->options.reseedInterval) ) {
            if( rng->stopExecution == 1 ) {
                SetEvent(rng->swapEvent);
                return 0;
            }
            rng->initISAAC_MERSENNE();
            reseedTime = GetTickCount();
        }
        
        if( rng->stopExecution == 1 ) {
            SetEvent(rng->swapEvent);
            return 0;
        }
        
        /*
        ** wait before touching again...
        */
        Sleep(250);
    }
    
    return 0;
}

#endif
