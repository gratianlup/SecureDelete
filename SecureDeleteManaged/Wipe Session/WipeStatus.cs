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
using DebugUtils.Debugger;

namespace SecureDelete
{
	[Serializable]
	public class WipeStatus
	{
		#region Field

		public WipeStatus[] Children;

		#endregion

		#region Properties

		private int _contextId;
		public int ContextId
		{
			get { return _contextId; }
			set { _contextId = value; }
		}

		SessionStatus _sessionStatus;
		public SessionStatus SessionStatus
		{
			get { return _sessionStatus; }
			set { _sessionStatus = value; }
		}

		ContextStatus _contextStatus;
		public ContextStatus ContextStatus
		{
			get { return _contextStatus; }
			set { _contextStatus = value; }
		}

		private long _objectIndex;
		public long ObjectIndex
		{
			get { return _objectIndex; }
			set { _objectIndex = value; }
		}

		WipeObjectType _objectType;
		public WipeObjectType ObjectType
		{
			get { return _objectType; }
			set { _objectType = value; }
		}

		private string _mainMessage;
		public string MainMessage
		{
			get { return _mainMessage; }
			set { _mainMessage = value; }
		}

		private string _auxMessage;
		public string AuxMessage
		{
			get { return _auxMessage; }
			set { _auxMessage = value; }
		}

		private long _bytesToWipe;
		public long BytesToWipe
		{
			get { return _bytesToWipe; }
			set { _bytesToWipe = value; }
		}

		private long _bytesWiped;
		public long BytesWiped
		{
			get { return _bytesWiped; }
			set { _bytesWiped = value; }
		}

		private long _bytesInClusterTipsWiped;
		public long BytesInClusterTipsWiped
		{
			get { return _bytesInClusterTipsWiped; }
			set { _bytesInClusterTipsWiped = value; }
		}

		private long _objectBytesToWipe;
		public long ObjectBytesToWipe
		{
			get { return _objectBytesToWipe; }
			set { _objectBytesToWipe = value; }
		}

		private long _objectBytesWiped;
		public long ObjectBytesWiped
		{
			get { return _objectBytesWiped; }
			set { _objectBytesWiped = value; }
		}

		private int _steps;
		public int Steps
		{
			get { return _steps; }
			set { _steps = value; }
		}

		private int _actualStep;
		public int ActualStep
		{
			get { return _actualStep; }
			set { _actualStep = value; }
		}

		private bool _wipeStopped;
		public bool WipeStopped
		{
			get { return _wipeStopped; }
			set { _wipeStopped = value; }
		}

		public bool HasChildren
		{
			get { return Children != null && Children.Length > 0; }
		}

		#endregion

		#region Public methods

		public void FromNative(NativeMethods.WStatus s)
		{
			_contextId = s.context;
			_objectIndex = s.objectIndex;
			_auxMessage = s.auxMessage;
			_mainMessage = s.message;

			switch (s.type)
			{
				case NativeMethods.TYPE_FILE:
					{
						_objectType = WipeObjectType.File;
						break;
					}
				case NativeMethods.TYPE_FOLDER:
					{
						_objectType = WipeObjectType.Folder;
						break;
					}
				case NativeMethods.TYPE_DRIVE:
					{
						_objectType = WipeObjectType.Drive;
						break;
					}
				case NativeMethods.TYPE_MFT:
					{
						_objectType = WipeObjectType.MFT;
						break;
					}
				case NativeMethods.TYPE_CLUSTER_TIPS:
					{
						_objectType = WipeObjectType.ClusterTips;
						break;
					}
			}

			_bytesToWipe = s.totalBytes;
			_bytesWiped = s.wipedBytes;
			_bytesInClusterTipsWiped = s.clusterTipsBytes;
			_objectBytesToWipe = s.objectBytes;
			_objectBytesWiped = s.objectWipedBytes;
			_steps = s.steps;
			_actualStep = s.step;
			_wipeStopped = s.stopped == 1;
		}


		public bool GetContextStatus(WipeContext context)
		{
			Debug.AssertNotNull(context,"Context is null");

			return context.GetContextStatus(out _contextStatus);
		}

		#endregion
	}
}