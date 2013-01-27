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

namespace SecureDelete
{
	public enum WipeStepType
	{
		Pattern, Random, RandomByte, Complement
	}

	/// <summary>
	/// The abstract class from where all wipe steps must derive.
	/// </summary>
	public abstract class WipeStepBase : ICloneable
	{
		#region Properties

		protected int _number;
		public int Number
		{
			get { return _number; }
			set { _number = value; }
		}

		protected WipeStepType _type;
		public WipeStepType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		#endregion

		#region Protected methods

		protected int MapTypeToNativeType()
		{
			switch (_type)
			{
				case WipeStepType.Pattern:
					{
						return 0;
					}
				case WipeStepType.Random:
					{
						return 1;
					}
				case WipeStepType.RandomByte:
					{
						return 2;
					}
				case WipeStepType.Complement:
					{
						return 3;
					}
			}

			return 1;
		}

		#endregion

		#region Abstract methods

		public abstract string ToNative();
		public abstract object Clone();

		#endregion
	}


	/// <summary>
	/// Wipe step where the data is a pattern.
	/// </summary>
	public sealed class PatternWipeStep : WipeStepBase
	{
		#region Constructor

		public PatternWipeStep()
		{
		}

		public PatternWipeStep(int number)
		{
			_type = WipeStepType.Pattern;
			_number = number;
		}

		public PatternWipeStep(int number, byte[] pattern)
		{
			_type = WipeStepType.Pattern;
			_number = number;
			_pattern = pattern;
		}

		public PatternWipeStep(string nativeForm, char[] stringSeparators)
		{
			_type = WipeStepType.Pattern;
			FromNative(nativeForm, stringSeparators);
		}

		#endregion

		#region Properties

		private byte[] _pattern;
		public byte[] Pattern
		{
			get { return _pattern; }
			set { _pattern = value; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Get the step from the native form
		/// </summary>
		public bool FromNative(string pattern, char[] stringSeparators)
		{
			Debug.AssertNotNull(pattern, "Pattern string is null");

			if (pattern.Length == 0)
			{
				return false;
			}

			string[] components = pattern.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			// there should be at least four components
			if (components.Length < 4)
			{
				return false;
			}
			else
			{
				int patternLength;

				if (int.TryParse(components[3], out patternLength) == false)
				{
					Debug.ReportWarning("Couldn't parse pattern length. Length field: {0}", components[0]);
					return false;
				}

				// perform some other validation on the pattern length
				if (patternLength < 0 || patternLength > 0x10000)
				{
					Debug.ReportWarning("Invalid pattern length. Length: {0}", patternLength);
					return false;
				}

				if (patternLength > (components.Length - 1))
				{
					Debug.ReportWarning("Not all pattern components present. Length: {0}, Present: {1}", patternLength, components.Length);
					return false;
				}

				if (patternLength > 0)
				{
					// allocate the pattern
					_pattern = new byte[patternLength];

					for (int i = 0; i < patternLength; i++)
					{
						if (byte.TryParse(components[4 + i], out _pattern[i]) == false)
						{
							Debug.ReportWarning("Failed to parse pattern component. Component field: {0}", components[4 + i]);

							// reset the pattern
							_pattern = null;

							return false;
						}
					}
				}

				return true;
			}
		}


		/// <summary>
		/// Get the native form of the step
		/// </summary>
		public override string ToNative()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(_number);
			builder.Append(' ');
			builder.Append(MapTypeToNativeType());

			if (_pattern != null)
			{
				builder.Append(' ');
				builder.Append(_pattern.Length);

				if (_pattern.Length > 0)
				{
					builder.Append(" \"");

					for (int i = 0; i < _pattern.Length; i++)
					{
						builder.Append(_pattern[i]);

						// append a space if not on the last position
						if (i != _pattern.Length - 1)
						{
							builder.Append(' ');
						}
					}

					builder.Append('"');
				}
			}

			return builder.ToString();
		}

		public override object Clone()
		{
			PatternWipeStep temp = new PatternWipeStep();

			temp._number = _number;
			temp._type = _type;
			if (_pattern != null)
			{
				temp._pattern = new byte[_pattern.Length];
				_pattern.CopyTo(temp._pattern, 0);
			}

			return temp;
		}

		#endregion
	}


	/// <summary>
	/// Wipe step where the data is random.
	/// </summary>
	public sealed class RandomWipeStep : WipeStepBase
	{
		#region Constructor

		public RandomWipeStep()
		{
		}

		public RandomWipeStep(int number)
		{
			_type = WipeStepType.Random;
			_number = number;
		}

		#endregion

		/// <summary>
		/// Get the native form of the step
		/// </summary>
		public override string ToNative()
		{
			return _number.ToString() + " " + MapTypeToNativeType().ToString();
		}

		public override object Clone()
		{
			RandomWipeStep temp = new RandomWipeStep();

			temp._number = _number;
			temp._type = _type;

			return temp;
		}
	}


	/// <summary>
	/// Wipe step where the data is a random byte.
	/// </summary>
	public sealed class RandomByteStep : WipeStepBase
	{
		#region Constructor

		public RandomByteStep()
		{
		}

		public RandomByteStep(int number)
		{
			_type = WipeStepType.RandomByte;
			_number = number;
		}

		#endregion

		/// <summary>
		/// Get the native form of the step
		/// </summary>
		public override string ToNative()
		{
			return _number.ToString() + " " + MapTypeToNativeType().ToString();
		}

		public override object Clone()
		{
			RandomByteStep temp = new RandomByteStep();

			temp._number = _number;
			temp._type = _type;

			return temp;
		}
	}


	/// <summary>
	/// Wipe step where the data is the complement of the previous data.
	/// </summary>
	public sealed class ComplementStep : WipeStepBase
	{
		#region Constructor

		public ComplementStep()
		{
		}

		public ComplementStep(int number)
		{
			_type = WipeStepType.Complement;
			_number = number;
		}

		#endregion

		/// <summary>
		/// Get the native form of the step
		/// </summary>
		public override string ToNative()
		{
			return _number.ToString() + " " + MapTypeToNativeType().ToString();
		}

		public override object Clone()
		{
			ComplementStep temp = new ComplementStep();

			temp._number = _number;
			temp._type = _type;

			return temp;
		}
	}
}