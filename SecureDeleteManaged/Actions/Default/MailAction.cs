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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace SecureDelete.Actions
{
	[Serializable]
	public sealed class MailAction : IAction, ICloneable
	{
		#region Constants

		public const int DefaultMailPort = 25;
		public const string DefaultMailSubject = "SecureDelete";
		private const string DefaultBody = "Test message";
		private const string SendFailedMessage = "Failed to send mail. Exception: {0}";

		#endregion

		#region Fields

		[NonSerialized]
		private SmtpClient client;

		[NonSerialized]
		private ManualResetEvent sentEvent;

		#endregion

		#region Properties

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[NonSerialized]
		private WipeSession _session;
		public SecureDelete.WipeSession Session
		{
			get { return _session; }
			set { _session = value; }
		}

		[NonSerialized]
		private bool _afterWipe;
		public bool AfterWipe
		{
			get { return _afterWipe; }
			set { _afterWipe = value; }
		}

		[NonSerialized]
		private bool _blockingMode;
		public bool BlockingMode
		{
			get { return _blockingMode; }
			set { _blockingMode = value; }
		}

		private bool _hasMaximumExecutionTime;
		public bool HasMaximumExecutionTime
		{
			get { return _hasMaximumExecutionTime; }
			set { _hasMaximumExecutionTime = value; }
		}

		private TimeSpan _maximumExecutionTime;
		public TimeSpan MaximumExecutionTime
		{
			get { return _maximumExecutionTime; }
			set { _maximumExecutionTime = value; }
		}

		private string _toAdress;
		public string ToAdress
		{
			get { return _toAdress; }
			set { _toAdress = value; }
		}

		private string _fromAdress;
		public string FromAdress
		{
			get { return _fromAdress; }
			set { _fromAdress = value; }
		}

		private string _server;
		public string Server
		{
			get { return _server; }
			set { _server = value; }
		}

		private int _port;
		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

		private string _user;
		public string User
		{
			get { return _user; }
			set { _user = value; }
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private string _subject;
		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		private bool _sendFullReport;
		public bool SendFullReport
		{
			get { return _sendFullReport; }
			set { _sendFullReport = value; }
		}

		[NonSerialized]
		private SmtpStatusCode _resultCode;
		public SmtpStatusCode ResultCode
		{
			get { return _resultCode; }
			set { _resultCode = value; }
		}

		#endregion

		#region Constructor

		public MailAction()
		{
			_port = DefaultMailPort;
			_subject = DefaultMailSubject;
		}

		#endregion

		#region Public methods

		public static bool IsValidMailAdress(string adress)
		{
			if (adress == null || adress.Length == 0)
			{
				return false;
			}

			// validate using regex
			return Regex.IsMatch(adress, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$");
		}


		public bool Start()
		{
			// validate the properties
			// mail adress
			if (IsValidMailAdress(_toAdress) == false)
			{
				return false;
			}

			// user and password
			if (_user != null && _user.Length > 0)
			{
				if (_password == null || _password.Length == 0)
				{
					return false;
				}
			}

			// send the mail
			SmtpClient client = new SmtpClient();
			MailMessage message = new MailMessage();

			_fromAdress = "test@lg.com";
			message.To.Add(new MailAddress(_toAdress));
			message.From = new MailAddress(_fromAdress);
			message.Subject = _subject == null ? DefaultMailSubject : _subject;
			
			// get the wipe report and set it as body
			if (_session != null && _afterWipe)
			{
				message.Body = ReportExporter.GetHtmlString(_session.GenerateReport(), WebReportStyle.HTMLReportStyle);
			}
			else
			{
				message.Body = DefaultBody;
			}

			// initialize the Smtp client
			client = new SmtpClient(_server, _port);

			if (_user != null && _user.Length > 0)
			{
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(_user, _password);
			}

			// send it
			try
			{
				_resultCode = SmtpStatusCode.Ok;
				sentEvent.Reset();
				client.SendAsync(message, null);

				// wait until the mail is sent

			}
			catch (SmtpException e)
			{
				_resultCode = e.StatusCode;
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, SendFailedMessage, e.Message);
				return false;
			}
			catch (Exception e)
			{
				_resultCode = SmtpStatusCode.GeneralFailure;
				ActionErrorReporter.ReportError(_session, _afterWipe, ErrorSeverity.High, SendFailedMessage, e.Message);
				return false;
			}

			return true;
		}


		public void Stop()
		{
			if (client != null)
			{
				try
				{
					client.SendAsyncCancel();
				}
				catch (Exception e)
				{
					Debug.ReportError("Failed to stop mail operation. Exception: {0}", e.Message);
				}
			}
		}


		public bool EndStart()
		{
			return true;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			MailAction temp = new MailAction();

			return temp;
		}

		#endregion
	}
}