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
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.IO;

namespace SecureDelete.Actions
{
	#region Event definitions

	public delegate void ScriptStateChangedDelegate(object sender, PipelineStateInfo state);
	public delegate void ScriptNewOutputDelegate(object sender, string output);

	#endregion

	public class ScriptExecutor
	{
		#region Fields

		private Runspace runspace;
		private Pipeline pipeline;

		#endregion

		#region Properties

		private List<IBridge> _bridgeList;
		public List<IBridge> BridgeList
		{
			get { return _bridgeList; }
			set { _bridgeList = value; }
		}

		#endregion

		#region Events

		public event ScriptStateChangedDelegate OnStateChanged;
		public event ScriptNewOutputDelegate OnNewOutput;

		#endregion

		#region Constructor

		public ScriptExecutor()
		{
			_bridgeList = new List<IBridge>();
		}

		#endregion

		#region Private methods

		private void ExportBridgeObjects()
		{
			if (_bridgeList == null || _bridgeList.Count == 0)
			{
				return;
			}

			foreach (IBridge bridge in _bridgeList)
			{
				runspace.SessionStateProxy.SetVariable(bridge.Name, bridge);	
			}
		}

		private void PipelineStateChanged(object sender, PipelineStateEventArgs e)
		{
			// report to parent
			if (OnStateChanged != null)
			{
				OnStateChanged(this, e.PipelineStateInfo);
			}
		}

		private void PipelineNewOutput(object sender, EventArgs e)
		{
			Collection<PSObject> data = pipeline.Output.NonBlockingRead();

			if (data.Count > 0 && OnNewOutput != null)
			{
				// build the string
				StringBuilder builder = new StringBuilder();

				foreach (PSObject obj in data)
				{
					builder.Append(obj);
				}

				// report to parent
				if (pipeline.PipelineStateInfo.State != PipelineState.Stopped &&
				   pipeline.PipelineStateInfo.State != PipelineState.Stopping)
				{
					OnNewOutput(this, builder.ToString());
				}
			}
		}

		#endregion

		#region Public methods

		public bool CreateRunspace()
		{
			if (runspace != null && runspace.RunspaceStateInfo.State != RunspaceState.Closed)
			{
				return true;
			}

			try
			{
				runspace = RunspaceFactory.CreateRunspace();
				runspace.Open();

				return true;
			}
			catch (Exception e)
			{
				Debug.ReportError("Failed to open runspace. Exception {0}", e.Message);
				return false;
			}
		}


		public void DestroyRunspace()
		{
			if (runspace == null)
			{
				return;
			}

			try
			{
				runspace.Close();
			}
			catch (Exception e)
			{
				Debug.ReportError("Failed to open runspace. Exception {0}", e.Message);
			}
		}


		public bool RunScriptAsync(string script)
		{
			if (runspace == null || runspace.RunspaceStateInfo.State != RunspaceState.Opened)
			{
				throw new Exception("Invalid runspace");
			}

			try
			{
				// create the pipeline
				pipeline = runspace.CreatePipeline();
				pipeline.Commands.AddScript(script);
				pipeline.Input.Close();

				// export the objects
				ExportBridgeObjects();

				// set events
				pipeline.StateChanged += PipelineStateChanged;
				pipeline.Output.DataReady += PipelineNewOutput;

				// start!
				pipeline.InvokeAsync();

				return true;
			}
			catch(Exception e)
			{
				Debug.ReportError("Failed to start script. Exception {0}", e.Message);
				return false;
			}
		}


		public void Stop()
		{
			if (pipeline != null && pipeline.PipelineStateInfo.State != PipelineState.Stopped &&
			   pipeline.PipelineStateInfo.State != PipelineState.Stopping)
			{
				pipeline.Stop();
			}
			else
			{
				throw new Exception("Pipeline not allocated or already stopped");
			}
		}

		#endregion
	}
}