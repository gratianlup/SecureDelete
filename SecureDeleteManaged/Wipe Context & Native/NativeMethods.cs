// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2007 Lup Gratian - All Rights Reserved
//   
// ***************************************************************         

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SecureDelete
{
	/// <summary>
	/// Wrapper for the SecureDelete.dll library
	/// </summary>
	public class NativeMethods
	{
		#region Context error codes

		public const int ERRORCODE_NO_MORE_CONTEXTS = 1;
		public const int ERRORCODE_INVALID_CONTEXT  = 2;
		public const int ERRORCODE_INVALID_REQUEST  = 3;
		public const int ERRORCODE_INITIALIZATION   = 4;
		public const int ERRORCODE_SUCCESS          = 254;

				/*
				 * #define ERRORCODE_STOPPED             0
		#define ERRORCODE_CRC                 1
		#define ERRORCODE_WRITE               2
		#define ERRORCODE_WMETHOD             3
		#define ERRORCODE_NOTFOUND            4
		#define ERRORCODE_ABORTED             5 // NOT USED ( replaced by STOPPED ) !!!
		#define ERRORCODE_ACCESS              6
		#define ERRORCODE_SIZE                7
		#define ERRORCODE_ADS                 8
		#define ERRORCODE_FILE_RECORD         9
		#define ERRORCODE_CORRUPTED_FS       10
		#define ERRORCODE_DIRECTORY_INDEX    11
		#define ERRORCODE_FREE_SPACE_FOLDER  12
		#define ERRORCODE_FOLDER             13
		#define ERRORCODE_SUCCESS           254
		#define ERRORCODE_UNKNOWN           255
				 */

		#endregion

		#region Context status codes

		public const int STATUS_WIPE    = 1;
		public const int STATUS_PAUSED  = 2;
		public const int STATUS_STOPPED = 3;

		#endregion

		#region Object types

		public const byte TYPE_FILE = 0;
		public const byte TYPE_FOLDER = 1;
		public const byte TYPE_DRIVE = 2;
		public const byte TYPE_MFT = 4;
		public const byte TYPE_CLUSTER_TIPS = 5;

		#endregion

		#region File options

		public const ushort WIPE_ADS = 0x01;
		public const ushort WIPE_FILENAME = 0x02;

		#endregion

		#region Folder options

		public const ushort WIPE_SUBFOLDERS = 0x100;
		public const ushort DELETE_FOLDERS = 0x200;
		public const ushort USE_MASK = 0x400;

		#endregion

		#region Drive options

		public const ushort WIPE_FREE_SPACE = 0x01;
		public const ushort WIPE_CLUSTER_TIPS = 0x02;
		public const ushort WIPE_MFT = 0x04;

		#endregion

		#region RNG options

		public const int RNG_ISAAC = 0;
		public const int RNG_MERSENNE = 1;

		#endregion

		#region Message severity

		public const int SEVERITY_HIGH = 3;
		public const int SEVERITY_MEDIUM = 2;
		public const int SEVERITY_LOW = 1;

		#endregion

		#region Structures

		[StructLayout(LayoutKind.Sequential, Pack = 4, Size = 24)]
		public struct RngOptions
		{
			public int randomProvider;
			public bool useSlowPool;
			public bool preventWriteToSwap;
			public bool reseed;
			public int reseedInterval;
			public int poolUpdateInterval;
		}

		[StructLayout(LayoutKind.Sequential,Pack = 4,CharSet = CharSet.Unicode)]
		public struct WOptions
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string methodPath;

			public int useLogFile;
			public int appendLog;
			public int logSizeLimit;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string logFile;

			public int totalDelete;
			public int WipeUsedFileRecord;
			public int wipeUnusedFileRecord;
			public int wipeUnusedIndexRecord;
			public int wipeUsedIndexRecord;
			public int destroyFreeSpaceFiles;
			public RngOptions rngOptions;
			public int warnSystemFile;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
		public struct WObject
		{
			public byte type;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string path;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string aux;

			public int wipeMethod;
			public ushort wipeOptions;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WStatus
		{
			public int context;
			public long objectIndex;
			public byte stopped;
			public byte type;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
			public string message;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
			public string auxMessage;

			public long totalBytes;
			public long wipedBytes;
			public long clusterTipsBytes;
			public long objectBytes;
			public long objectWipedBytes;
			public ushort steps;
			public ushort step;
		}

		[StructLayout(LayoutKind.Sequential,Pack = 4, CharSet = CharSet.Unicode)]
		public struct WSmallObject
		{
			public byte type;
			public int log; // the associated error (the index)

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string path;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class SYSTEMTIME
		{
			public short wYear;
			public short wMonth;
			public short wDayOfWeek;
			public short wDay;
			public short wHour;
			public short wMinute;
			public short wSecond;
			public short wMilliseconds;

			public override string ToString()
			{
				return ("[SYSTEMTIME: " + this.wDay.ToString(CultureInfo.CurrentCulture) + "/" + this.wMonth.ToString(CultureInfo.CurrentCulture) + "/" + this.wYear.ToString(CultureInfo.CurrentCulture) + " " + this.wHour.ToString(CultureInfo.CurrentCulture) + ":" + this.wMinute.ToString(CultureInfo.CurrentCulture) + ":" + this.wSecond.ToString(CultureInfo.CurrentCulture) + "]");
			}
		}

		[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Unicode)]
		public struct WError
		{
			public SYSTEMTIME time;
			public int severity;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 520)]
			public string message;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create a context
		/// </summary>
		/// <param name="context">The Id of the created context. </param>
		/// <returns>The error code of the operation.</returns>
        [DllImport("SecureDeleteNative.dll", EntryPoint = "#1")]
		public static extern int CreateWipeContext(out int context);


		/// <summary>
		/// Destroys an existing context
		/// </summary>
		/// <param name="context">The id of the context to destroy.</param>
		/// <returns>The error code of the operation.</returns>
		[DllImport("SecureDeleteNative.dll", EntryPoint = "#2")]
		public static extern int DestroyWipeContext(int context);


        [DllImport("SecureDeleteNative.dll", EntryPoint = "#3")]
		public static extern int InitializeWipeContext(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#4")]
        public static extern int StartWipeContext(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#5")]
        public static extern int StopWipeContext(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#6")]
        public static extern int PauseWipeContext(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#7")]
        public static extern int ResumeWipeContext(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#8")]
		public static extern int SetWipeOptions(int context, ref WOptions wipeOptions);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#9")]
		public static extern int InsertWipeObject(int context, ref WObject wipeObject);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#10")]
		public static extern int GetContextStatus(int context);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#11")]
		public static extern int GetWipeStatus(int context,out WStatus wipeStatus);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#12")]
		public static extern int GetFailedObjectNumber(int context, out int failedNumber);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#13")]
		public static extern int GetFailedObject(int context,int position, out WSmallObject failedObject);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#14")]
		public static extern int GetErrorNumber(int context, out int errorNumber);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#15")]
		public static extern int GetError(int context, int position, out WError wipeError);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#16")]
		public static extern int GetChildrenNumber(int context, out int childrenNumber);

        [DllImport("SecureDeleteNative.dll", EntryPoint = "#17")]
		public static extern int GetChildWipeStatus(int context,int child, out WStatus wipeStatus);

		#endregion

		#region C++

		/*
		 * 
		 * SYSTEMTIME time;
	wchar_t    message[MAX_PATH * 2];
		 * 
		 * typedef struct __WSMALL_OBJECT {
	char    Type;
	wchar_t path[MAX_PATH];
	int     log;
} WSMALL_OBJECT;
		 * 
		 * typedef struct __WSTATUS {
	char               stopped;          // stopped ?
	char               Type;             // Type of current object
	wchar_t            message[512];     // primary message
	wchar_t            auxMessage[512];  // secondary message
	__int64            totalBytes;       // total bytes to be wiped
	__int64            wipedBytes;       // bytes written so far
	__int64            clusterTipsBytes; // size of cluster tips wiped
	__int64            objectBytes;      // total bytes of current object
	__int64            objectWipedBytes; // bytes written for the current object
	short unsigned int steps;            // current wipe method steps
	short unsigned int step;             // current step
	char padding[12];                    // align to 16 bytes
} WSTATUS;
		 * 
		 * 
		 * typedef struct __WEXTENDED_OBJECT {
	char               Type;
	wchar_t            path[MAX_PATH];
	wchar_t            aux[MAX_PATH];
	short unsigned int wmethod;
	short unsigned int woptions;
} WEXTENDED_OBJECT;
		 */


		/*
		 *
		 * 
		 * typedef struct __RNG_OPTIONS {
	int randomProvider;
	int useSlowPool;
	int _preventWriteToSwap;
	int reseed;
	int reseedInterval;
	int poolUpdateInterval;
} RNG_OPTIONS;
		 * 
		 * typedef struct __WOPTIONS {
	wchar_t     methodPath[MAX_PATH];
	int         useLogfile;
	int         appendLog;
	int         logSizeLimit;
	wchar_t     logFile[MAX_PATH];
	int         totalDelete;
	int         wipeUsedFileRecord;
	int         wipeUnusedFileRecord;
	int         wipeUnusedIndexRecord;
	int         wipeUsedIndexRecord;
	int         destroyFreeSpaceFiles;
	RNG_OPTIONS rngOptions;
} WOPTIONS;
		 * 
__declspec(dllexport) int CreateContext(int *context) {
	if( IsBadWritePtr(context,1) ) {
		return ERRORCODE_UNKNOWN;
	}
	
	return manager.createContext(context);
}


__declspec(dllexport) int DestroyContext(int context) {
	return manager.destroyContext(context);
}


__declspec(dllexport) int InitializeContext(int context) {
	return manager.initializeContext(context);
}


__declspec(dllexport) int StartContext(int context) {
	return manager.startContext(context);
}


__declspec(dllexport) int StopContext(int context) {
	return manager.stopContext(context);
}


__declspec(dllexport) int PauseContext(int context) {
	return manager.pauseContext(context);
}


__declspec(dllexport) int ResumeContext(int context) {
	return manager.resumeContext(context);
}


__declspec(dllexport) int SetWipeOptions(int context,WOPTIONS *wipeOptions) {
	if( IsBadReadPtr(wipeOptions,1) ) {
		return ERRORCODE_UNKNOWN;
	}
	
	return manager.setWipeOptions(context,wipeOptions);
}


__declspec(dllexport) int InsertWipeObject(int context,WEXTENDED_OBJECT *wipeObject) {
	if( IsBadReadPtr(wipeObject,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.insertWipeObject(context,wipeObject);
}


__declspec(dllexport) int GetContextStatus(int context) {
	return manager.getContextStatus(context);
}


__declspec(dllexport) int GetWipeStatus(int context,WSTATUS *wipeStatus) {
	if( IsBadWritePtr(wipeStatus,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getWipeStatus(context,wipeStatus);
}


__declspec(dllexport) int GetFailedObjectNumber(int context,int *failedNumber) {
	if( IsBadWritePtr(failedNumber,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getFailedObjectNumber(context,failedNumber);
}


__declspec(dllexport) int GetFailedObject(int context,int position,WSMALL_OBJECT *failedObject) {
	if( IsBadWritePtr(failedObject,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getFailedObject(context,position,failedObject);
}


__declspec(dllexport) int GetErrorNumber(int context,int *errorNumber) {
	if( IsBadWritePtr(errorNumber,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getErrorNumber(context,errorNumber);
}


__declspec(dllexport) int GetError(int context,int position,WIPE_ERROR2 *wipeError) {
	if( IsBadWritePtr(wipeError,1) ) {
		return ERRORCODE_UNKNOWN;
	}

	return manager.getError(context,position,wipeError);
}
		 */

		#endregion
	}
}
