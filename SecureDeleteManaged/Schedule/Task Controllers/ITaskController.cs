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

namespace SecureDelete.Schedule
{
	public enum ControllerType
	{
		Power, HDD
	}


	public interface ITaskControllerSettings
	{
		ControllerType Type { get; }
	}


	public interface ITaskController
	{
		ControllerType Type { get; }
		TaskManager Parent { get;set; }
		ITaskControllerSettings Settings { get;set; }

		bool Enabled { get; }
		bool AllowTaskStart { get; }
		void TaskStarted(ScheduledTask task);
		void TaskStopped(ScheduledTask task, int remaining);
	}
}
