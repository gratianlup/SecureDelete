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
using System.IO;
using DebugUtils.Debugger;
using System.Threading;

namespace SecureDelete.FileSearch
{
	#region Size Filter

	[Serializable]
	public sealed class SizeFilter : FilterBase
	{
		#region Constants

		// SizeValue is specified in KB, so we need to multiply it with 1024 to obtain bytes
		private const int SizeValueShift = 10;

		#endregion

		#region Fields

		private long shiftedSizeValue;

		#endregion

		#region Constructor

		public SizeFilter()
		{
			_enabled = true;
		}

		public SizeFilter(SizeImplication sizeImplication, long sizeValue)
		{
			_enabled = true;

			_sizeImplication = sizeImplication;
			_sizeValue = sizeValue;
			shiftedSizeValue = _sizeValue << SizeValueShift;
		}

		#endregion

		#region Properties

		private SizeImplication _sizeImplication;
		public SizeImplication SizeImplication
		{
			get { return _sizeImplication; }
			set { _sizeImplication = value; }
		}

		private long _sizeValue;
		public long SizeValue
		{
			get { return _sizeValue; }
			set { _sizeValue = value; shiftedSizeValue = _sizeValue << SizeValueShift; }
		}

		#endregion

		#region Public methods

		public override bool Allow(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			bool condition = _condition == FilterCondition.IS ? true : false;

			if (File.Exists(file))
			{
				FileInfo info = new FileInfo(file);

				if (_sizeImplication == SizeImplication.Equals)
				{
					if (info.Length == shiftedSizeValue)
					{
						return condition;
					}
				}
				else if (_sizeImplication == SizeImplication.LessThan)
				{
					if (info.Length <= shiftedSizeValue)
					{
						return condition;
					}
				}
				else
				{
					if (info.Length >= shiftedSizeValue)
					{
						return condition;
					}
				}
			}
			else
			{
				return !condition;
			}

			return !condition;
		}


		public override object Clone()
		{
			SizeFilter temp = new SizeFilter();

			temp._name = _name;
			temp._condition = _condition;
			temp._enabled = _enabled;
			temp._sizeImplication = _sizeImplication;
			temp._sizeValue = _sizeValue;
			temp.shiftedSizeValue = shiftedSizeValue;

			return temp;
		}

		#endregion
	}

	#endregion

	#region Date Filter

	public enum FileDateType
	{
		CreationDate, AccessDate, WriteDate
	}

	[Serializable]
	public sealed class DateFilter : FilterBase
	{
		#region Properties

		private DateImplication _dateImplication;
		public DateImplication DateImplication
		{
			get { return _dateImplication; }
			set { _dateImplication = value; }
		}

		private FileDateType _dateType;
		public FileDateType DateType
		{
			get { return _dateType; }
			set { _dateType = value; }
		}

		private DateTime _dateValue;
		public DateTime DateValue
		{
			get { return _dateValue; }
			set { _dateValue = value; }
		}

		#endregion

		#region Constructor

		public DateFilter()
		{
			_enabled = true;
		}

		public DateFilter(FileDateType type, DateImplication dateImplication, DateTime dateValue)
		{
			_enabled = true;

			_dateType = type;
			_dateImplication = dateImplication;
			_dateValue = dateValue;
		}

		#endregion

		#region Public methods

		public override bool Allow(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			bool condition = _condition == FilterCondition.IS ? true : false;

			if (File.Exists(file))
			{
				FileInfo info = new FileInfo(file);
				DateTime date;

				if (_dateType == FileDateType.CreationDate)
				{
					date = info.CreationTime;
				}
				else if (_dateType == FileDateType.AccessDate)
				{
					date = info.LastAccessTime;
				}
				else
				{
					date = info.LastWriteTime;
				}

				// Filter the date
				if (_dateImplication == DateImplication.From)
				{
					if (date.Day == _dateValue.Day && date.Month == _dateValue.Month &&
						date.Year == _dateValue.Year)
					{
						return condition;
					}
				}
				else if (_dateImplication == DateImplication.OlderOrFrom)
				{
					if (date <= _dateValue)
					{
						return condition;
					}
				}
				else
				{
					if (date >= _dateValue)
					{
						return condition;
					}
				}
			}
			else
			{
				return !condition;
			}

			return !condition;
		}


		public override object Clone()
		{
			DateFilter temp = new DateFilter();

			temp._name = _name;
			temp._condition = _condition;
			temp._dateImplication = _dateImplication;
			temp._dateType = _dateType;
			temp._dateValue = _dateValue;
			temp._enabled = _enabled;

			return temp;
		}

		#endregion
	}

	#endregion

	#region Attribute Filter

	[Serializable]
	public sealed class AttributeFilter : FilterBase
	{
		#region Properties

		private FileAttributes _attributeValue;
		public FileAttributes AttributeValue
		{
			get { return _attributeValue; }
			set { _attributeValue = value; }
		}

		#endregion

		#region Constructor

		public AttributeFilter()
		{
			_enabled = true;
		}

		public AttributeFilter(FileAttributes attributeValue)
		{
			_enabled = true;
			_attributeValue = attributeValue;
		}

		#endregion

		#region Public methods

		public override bool Allow(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			bool condition = _condition == FilterCondition.IS ? true : false;

			if (File.Exists(file))
			{
				FileAttributes attributes = File.GetAttributes(file);

				if ((attributes & _attributeValue) == _attributeValue)
				{
					return condition;
				}
			}
			else
			{
				return !condition;
			}

			return !condition;
		}


		public override object Clone()
		{
			AttributeFilter temp = new AttributeFilter();

			temp._name = _name;
			temp._attributeValue = _attributeValue;
			temp._condition = _condition;
			temp._enabled = _enabled;

			return temp;
		}

		#endregion
	}

	#endregion
}