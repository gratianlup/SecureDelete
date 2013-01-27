// ***************************************************************
//  SecureDelete   version:  1.0
//  -------------------------------------------------------------
//
//  Copyright (C) 2008 Lup Gratian - All Rights Reserved
//   
// ***************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SecureDelete
{
	public class Minidump
	{
		#region Win32 Interop
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll")]
		public static extern int GetCurrentProcessId();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
											   uint SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes,
											   int hTemplateFile);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("Dbghelp.dll")]
		public static extern bool MiniDumpWriteDump(IntPtr process, int processId, IntPtr file,
													MinidumpType type, IntPtr a, IntPtr b, IntPtr c);

		[Flags]
		public enum MinidumpType
		{
			MiniDumpNormal = 0x00,
			MiniDumpWithDataSegs = 0x01,
			MiniDumpWithFullMemory = 0x02,
			MiniDumpWithHandleData = 0x04,
			MiniDumpFilterMemory = 0x08,
			MiniDumpScanMemory  = 0x10,
			MiniDumpWithUnloadedModules = 0x20,
			MiniDumpWithIndirectlyReferencedMemory  = 0x40,
			MiniDumpFilterModulePaths = 0x80,
			MiniDumpWithProcessThreadData = 0x100
		}

		/*
		 *
		 * 
		 * BOOL MiniDumpWriteDump(
  HANDLE hProcess,
  DWORD ProcessId,
  HANDLE hFile,
  MINIDUMP_TYPE DumpType,
  PMINIDUMP_EXCEPTION_INFORMATION ExceptionParam,
  PMINIDUMP_USER_STREAM_INFORMATION UserStreamParam,
  PMINIDUMP_CALLBACK_INFORMATION CallbackParam
);

		 */

		#endregion

		#region Public methods

		public static bool WriteMinidump(IntPtr process, int processId, string path)
		{
			// open the file
			IntPtr fileHandle = CreateFile(path, 2, 0, 0, 2, 0x80, 0);

			// write the minidump
			bool result = MiniDumpWriteDump(process, processId, fileHandle, MinidumpType.MiniDumpNormal, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			CloseHandle(fileHandle);

			return result;
		}


		public static bool WriteMinidump(string path)
		{
			return WriteMinidump(GetCurrentProcess(), GetCurrentProcessId(), path);
		}

		#endregion
	}
}
