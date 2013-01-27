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

#pragma once
#include "resource.h"       // main symbols

#include "SDShellNative.h"
#include <shlobj.h>
#include <comdef.h>
#include "linkedList.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

//
// operation type definitions
//
#define STANDARD_OPERATION    1
#define MOVE_OPERATION        2
#define RECYCLE_BIN_OPERATION 4

//
// object type definitions
//
#define OBJECT_TYPE_FILE   1
#define OBJECT_TYPE_FOLDER 2
#define OBJECT_TYPE_DRIVE  3

//
// data header definition
//
typedef struct _SDHeader {
	int size; // the size of this structure
	int operationType;
	int objectCount;
	wchar_t data1[MAX_PATH + 1];
	wchar_t data2[MAX_PATH + 2];
} SDHeader;


//
// wipe object definition
//
typedef struct _SDObject {
	int size; // the size of this structure
	int pathOffset;
	int objectType;
	int pathLength;
	wchar_t path[0];
} SDObject;


//
// executing context definitions
//
typedef struct _ShellContext {
	LLIST<wchar_t *> fileList;
	wchar_t dropFolder[MAX_PATH + 1];
	int dragMenu;
	HANDLE threadHandle;
} ShellContext;


//
// registry key definitions
//
#define SecureDeleteKey L"Software\\SecureDelete"
#define ShellNormalEnabled L"ShellNormalEnabled"
#define ShellMoveEnabled L"ShellMoveEnabled"
#define ShellRecycleEnabled L"ShellRecycleEnabled"

//
// CSDShellExt
//
class ATL_NO_VTABLE CSDShellExt :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CSDShellExt, &CLSID_SDShellExt>,
	public IDispatchImpl<ISDShellExt, &IID_ISDShellExt, &LIBID_SDShellNativeLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IShellExtInit,
	public IContextMenu {
public:
	//
	// constructor
	//
	CSDShellExt() {
	}

DECLARE_REGISTRY_RESOURCEID(IDR_SDSHELLEXT)

DECLARE_NOT_AGGREGATABLE(CSDShellExt)

BEGIN_COM_MAP(CSDShellExt)
	COM_INTERFACE_ENTRY(ISDShellExt)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IShellExtInit)	
	COM_INTERFACE_ENTRY(IContextMenu)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct() {
		return S_OK;
	}

	void FinalRelease() {
	}

protected:
	LLIST<wchar_t *> fileList;
	wchar_t dropFolder[MAX_PATH + 1];
	int dragMenu;
	ShellContext *context;
	DWORD normalEnabled;
	DWORD moveEnabled;
	DWORD recycleEnabled;
	SDHeader dataHeader;

public:
	//
	// methods
	//
	STDMETHODIMP Initialize(LPCITEMIDLIST, LPDATAOBJECT, HKEY);
	void ClearFileList();
	int  GetExtensionDirectory(wchar_t *path);

	//
	// context menu
	//
	STDMETHODIMP GetCommandString(UINT, UINT, UINT*, LPSTR, UINT);
	STDMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO);
	STDMETHODIMP QueryContextMenu(HMENU, UINT, UINT, UINT, UINT);
};

OBJECT_ENTRY_AUTO(__uuidof(SDShellExt), CSDShellExt)
