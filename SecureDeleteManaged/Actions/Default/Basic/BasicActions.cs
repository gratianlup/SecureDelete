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
using System.Diagnostics;

namespace SecureDelete.Actions
{
	[Serializable]
	public sealed class ShutdownAction : CustomAction
	{
		public override string File
		{
			get { return "shutdown"; }
		}

		public override string Arguments
		{
			get { return "-s"; }
		}
	}


	[Serializable]
	public sealed class RestartAction : CustomAction
	{
		public override string File
		{
			get { return "shutdown"; }
		}

		public override string Arguments
		{
			get { return "-r"; }
		}
	}


	[Serializable]
	public sealed class LogoffAction : CustomAction
	{
		public override string File
		{
			get { return "shutdown"; }
		}

		public override string Arguments
		{
			get { return "-l"; }
		}
	}
}