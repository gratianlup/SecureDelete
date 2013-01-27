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

#include "stdafx.h"
#include "SDShellExt.h"
#include <atlconv.h>
#include <windows.h>
#include <Psapi.h>
#include <strsafe.h>
#include <Shlwapi.h>

// 
// initialization
//
STDMETHODIMP CSDShellExt::Initialize (LPCITEMIDLIST pidlFolder,	LPDATAOBJECT pDataObj, HKEY hProgID) {
    FORMATETC fmt = { CF_HDROP, NULL, DVASPECT_CONTENT,
                  -1, TYMED_HGLOBAL };
    STGMEDIUM stg = { TYMED_HGLOBAL };
    HDROP     hDrop;

    //
    // Look for CF_HDROP data in the data object. If there
    // is no such data, return an error back to Explorer.
    //
    if (FAILED(pDataObj->GetData(&fmt, &stg))) {
        return E_INVALIDARG;
    }

    //
    // check if extension is enabled
    //
    DWORD bufferSize = 4;
    DWORD keyType = REG_DWORD;
    SHGetValue(HKEY_CURRENT_USER, SecureDeleteKey, ShellNormalEnabled, &keyType, &normalEnabled, &bufferSize);
    SHGetValue(HKEY_CURRENT_USER, SecureDeleteKey, ShellMoveEnabled, &keyType, &moveEnabled, &bufferSize);
    SHGetValue(HKEY_CURRENT_USER, SecureDeleteKey, ShellRecycleEnabled, &keyType, &recycleEnabled, &bufferSize);

    if(!normalEnabled && !moveEnabled && !recycleEnabled) {
        // none of the features enabled
        return E_INVALIDARG;
    }

    //
    // reset data header
    //
    dataHeader.operationType = 0;
    dataHeader.objectCount = 0;

    //
    // get drop folder if drag-n-drop operation
    //
    if(pidlFolder != NULL && SHGetPathFromIDList(pidlFolder, dataHeader.data1)) {
        dataHeader.operationType = MOVE_OPERATION;
    }
 
    //
    // Get a pointer to the actual data.
    //
    hDrop = (HDROP)GlobalLock(stg.hGlobal);
    
    //
    // Make sure it worked.
    //
    if (hDrop == NULL) {
        return E_INVALIDARG;
    }

    //
    // get the files
    //
    unsigned int fileNumber = DragQueryFile (hDrop, 0xFFFFFFFF, NULL, 0);
    HRESULT hr = S_OK;

    if (fileNumber == 0) {
        GlobalUnlock (stg.hGlobal);
        ReleaseStgMedium (&stg);
        return E_INVALIDARG;
    }

    //
    // get the file names
    //
    wchar_t buffer[MAX_PATH];
    ClearFileList();

    for(unsigned int i = 0;i < fileNumber;i++) {
        if(DragQueryFile(hDrop, i, buffer, MAX_PATH) == 0) {
            //
            // error, abort
            //
            hr = E_INVALIDARG;
            break;
        }
            
        //
        // allocate string
        //
        int length = wcslen(buffer);
        wchar_t *name = new wchar_t[length + 1];
        wcscpy(name, buffer);

        //
        // insert into the list
        //
        fileList.insert(&name);
    }

    GlobalUnlock(stg.hGlobal);
    ReleaseStgMedium(&stg);
    return hr;
}


HRESULT CSDShellExt::QueryContextMenu(HMENU hmenu, UINT uMenuIndex, UINT uidFirstCmd, UINT uidLastCmd, UINT uFlags) {
    if (uFlags & CMF_DEFAULTONLY) {
        return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, 0);
    }

    //
    // insert the menu item
    //
    if(dataHeader.operationType == MOVE_OPERATION) {
        if(moveEnabled) {
            // drag-n-drop menu
            InsertMenu (hmenu, uMenuIndex, MF_BYPOSITION,
                        uidFirstCmd, _T("Move using SecureDelete"));
        }
    }
    else if(dataHeader.operationType == RECYCLE_BIN_OPERATION) {
        if(recycleEnabled) {
            // not implemented
        }
    }
    else if(normalEnabled) {
        // standard menu
        InsertMenu (hmenu, uMenuIndex, MF_BYPOSITION,
                    uidFirstCmd, _T("Wipe using SecureDelete"));
    }

    return MAKE_HRESULT(SEVERITY_SUCCESS, FACILITY_NULL, 1);
}


HRESULT CSDShellExt::GetCommandString(
    UINT idCmd, UINT uFlags, UINT* pwReserved,
    LPSTR pszName, UINT cchMax ) {
    //
    // not implemented yet...
    //
    return E_INVALIDARG;
}


int CSDShellExt::GetExtensionDirectory(wchar_t *path) {
    HMODULE moduleHandle = NULL;
    HANDLE processHandle =  NULL;

    processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, GetCurrentProcessId());

    if(processHandle == NULL) {
        //
        // failed to get process handle
        //
        return 0;
    }

    if(GetModuleHandleEx(0, L"SDShellNative.dll", &moduleHandle) == 0) {
        if(GetModuleHandleEx(0, L"SDShellNative32.dll", &moduleHandle) == 0) {
            //
            // try get for 64 bit version instead
            //
            if(GetModuleHandleEx(0, L"SDShellNative64.dll", &moduleHandle) == 0) {
                //
                // failed
                //
                CloseHandle(processHandle);
                return 0;
            }
        }
    }

    //
    // get the file path
    //
    if(GetModuleFileNameEx(processHandle, moduleHandle, path, MAX_PATH) == 0) {
        //
        // failed
        //
        CloseHandle(processHandle);
        return 0;
    }

    //
    // extract directory
    //
    int length = wcslen(path);

    for(int i = length - 1;i > 0;i--) {
        if(path[i] == '\\') {
            //
            // end the string here
            //
            path[i + 1] = 0;
            break;
        }
    }

    CloseHandle(processHandle);
    return 1;
}


void CSDShellExt::ClearFileList() {
    int count = fileList.number;

    for(int i = 0;i < count;i++) {
        wchar_t *fileName = fileList[i];

        //
        // release allocated memory
        //
        if(fileName != NULL) {
            delete[] fileName;
        }
    }

    //
    // clear list
    //
    fileList.clear();
}

HRESULT CSDShellExt::InvokeCommand(LPCMINVOKECOMMANDINFO pCmdInfo) {
    if (HIWORD(pCmdInfo->lpVerb) != 0) {
        return E_INVALIDARG;
    }

    switch (LOWORD(pCmdInfo->lpVerb)) {
        case 0: {
            HANDLE mappingHandle = 0;
            void *view = NULL;

            try {
                //
                // get the application path
                //
                wchar_t directory[MAX_PATH + 1];
                wchar_t appPath[MAX_PATH + 1];

                if(GetExtensionDirectory(directory) == 0) {
                    return E_INVALIDARG;
                }

                //
                // generate application name
                //
                wcscpy(appPath, directory);
                wcscat(appPath, L"\\SDShellManaged.exe");

                //
                // create mapped file
                //
                SECURITY_ATTRIBUTES sa;
                sa.nLength = sizeof(SECURITY_ATTRIBUTES);
                sa.lpSecurityDescriptor = NULL;
                sa.bInheritHandle = TRUE;

                DWORD requiredSize = sizeof(SDHeader) + fileList.number * (sizeof(SDObject) + MAX_PATH + 1) * sizeof(wchar_t);
                mappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, &sa, PAGE_READWRITE, 0, requiredSize, NULL);

                if(mappingHandle == 0) {
                    return E_INVALIDARG;
                }

                //
                // map view
                //
                view = MapViewOfFile(mappingHandle, FILE_MAP_WRITE, 0, 0, 0);

                if(view == NULL) {
                    CloseHandle(mappingHandle);
                    return E_INVALIDARG;
                }

                int position = 0;

                //
                // copy header
                //
                dataHeader.size = sizeof(SDHeader);
                dataHeader.objectCount = fileList.number;
                memcpy((unsigned char *)view + position, &dataHeader, sizeof(SDHeader));		
                position += sizeof(SDHeader);

                for(int i = 0;i < fileList.number;i++) {
                    SDObject *object = (SDObject *)((unsigned char *)view + position);
                    wchar_t *path = fileList[i];

                    //
                    // get file type (file / folder)
                    //
                    if((GetFileAttributes(path) & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY) {
                        //
                        // directory
                        //
                        object->objectType = OBJECT_TYPE_FOLDER;
                    }
                    else {
                        // file
                        object->objectType = OBJECT_TYPE_FILE;
                    }

                    object->pathLength = wcslen(path);
                    object->size = sizeof(SDObject) + (object->pathLength + 1) * sizeof(wchar_t);
                    object->pathOffset = (char *)object->path - (char *)object;
                    wcscpy(object->path, path);

                    //
                    // increment position
                    //
                    position += object->size;
                }	

                //
                // launch application
                //
                wchar_t command[MAX_PATH + 1];
                StringCchPrintf(command, MAX_PATH, L"\"%s\" %I64d", appPath, (INT64)(INT_PTR)mappingHandle);

                STARTUPINFO si;
                PROCESS_INFORMATION pi;
                ZeroMemory(&si, sizeof(si));
                ZeroMemory(&pi, sizeof(pi));
                si.cb = sizeof(si);
                CreateProcess(NULL, command, NULL, NULL, TRUE, 0, NULL, directory, &si, &pi);
            }
            catch(...) {
                //
                // cleanup
                //
                if(view != NULL) {
                    UnmapViewOfFile(view);
                    view = NULL;
                }

                if(mappingHandle != 0) {
                    CloseHandle(mappingHandle);
                }
            }

            return S_OK;
        }
        break;

    default:
        return E_INVALIDARG;
        break;
    }
}
