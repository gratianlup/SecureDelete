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
	/// <summary>
	/// Method used to wipe data.
	/// </summary>
	public class WipeMethod : ICloneable
	{
		#region Fields
		
		private char[] splitChars;
		private List<WipeStepBase> wipeSteps;
		private int stepNumber;

		#endregion

		#region Constructor

		public WipeMethod()
		{
			splitChars = new char[] { ' ', '"', '\n', '\t' };
			wipeSteps = new List<WipeStepBase>();
			_name = string.Empty;
		}

		#endregion

		#region Properties

		public List<WipeStepBase> Steps
		{
			get { return wipeSteps; }
		}

		private int _id;
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private string _author;
		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		private bool _checkWipe;
		public bool CheckWipe
		{
			get { return _checkWipe; }
			set { _checkWipe = value; }
		}

		private bool _shuffle;
		public bool Shuffle
		{
			get { return _shuffle; }
			set { _shuffle = value; }
		}

		private int _shuffleFirst;
		public int ShuffleFirst
		{
			get { return _shuffleFirst; }
			set { _shuffleFirst = value; }
		}

		private int _shuffleLast;
		public int ShuffleLast
		{
			get { return _shuffleLast; }
			set { _shuffleLast = value; }
		}

		#endregion

		#region Private methods

		private bool ParseLine(string line)
		{
			if (line == null || line.Length == 0)
			{
				return true;
			}

			string[] components = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

			// skip empty lines
			if (components.Length < 1)
			{
				return true;
			}

			// handle the command
			switch (components[0])
			{
				case "SDM":
					{
						return true;
					}
				case "id":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("Id field not present");
							return false;
						}

						if (int.TryParse(components[1], out _id) == false)
						{
							Debug.ReportWarning("Invalid id. Id field: {0}", components[1]);
							return false;
						}

						break;
					}
				case "name":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("Name length field not present");
							return false;
						}

						int nameLength = 0;
						if (int.TryParse(components[1], out nameLength) == false)
						{
							Debug.ReportWarning("Invalid name length. Name length field: {0}", components[1]);
							return false;
						}

						if (nameLength > 0)
						{
							if (components.Length < 3)
							{
								Debug.ReportWarning("Name field not present");
								return false;
							}

							// append remaining components to form the name
							string name = components[2];

							for (int i = 3; i < components.Length; i++)
							{
								name += " " + components[i];
							}

							if (name.Length != nameLength)
							{
								Debug.ReportWarning("The length of the name field is invalid. Should be: {0}, But is: {1}", nameLength, name.Length );
								return false;
							}

							_name = name;
						}
						else
						{
							_name = string.Empty;
						}

						break;
					}
				case "steps":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("Step number field not present");
							return false;
						}

						if (int.TryParse(components[1], out stepNumber) == false)
						{
							Debug.ReportWarning("Invalid step number. Step number field: {0}", components[1]);
							return false;
						}

						if (stepNumber < 0 || stepNumber > 0x10000)
						{
							Debug.ReportWarning("Invalid step number. Step number: {0}", stepNumber);
							return false;
						}

						break;
					}
				case "shuffle":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("Shuffle state field not present");
							return false;
						}

						int state = 0;
						if (int.TryParse(components[1], out state) == false)
						{
							Debug.ReportWarning("Invalid shuffle state. Shuffle state field: {0}", components[1]);
							return false;
						}

						_shuffle = Convert.ToBoolean(state);
						break;
					}
				case "shuffleFirst":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("ShuffleFirst number field not present");
							return false;
						}

						if (int.TryParse(components[1], out _shuffleFirst) == false)
						{
							Debug.ReportWarning("Invalid ShuffleFirst number. ShuffleFirst number field: {0}", components[1]);
							return false;
						}

						break;
					}
				case "shuffleLast":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("ShuffleLast number field not present");
							return false;
						}

						if (int.TryParse(components[1], out _shuffleLast) == false)
						{
							Debug.ReportWarning("Invalid ShuffleLast number. ShuffleLast number field: {0}", components[1]);
							return false;
						}

						break;
					}
				case "check":
					{
						if (components.Length < 2)
						{
							Debug.ReportWarning("Check state field not present");
							return false;
						}

						int state = 0;
						if (int.TryParse(components[1], out state) == false)
						{
							Debug.ReportWarning("Invalid check state. Check state field: {0}", components[1]);
							return false;
						}

						_checkWipe = Convert.ToBoolean(state);
						break;
					}
				case "step":
					{
						return HandleStep(line);
					}
			}

			return true;
		}

		private bool HandleStep(string line)
		{
			string[] components = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

			if (components.Length < 3)
			{
				Debug.ReportWarning("Invalid step. Step data: {0}", line);
				return false;
			}

			// get the number
			int number;
			if (int.TryParse(components[1], out number) == false)
			{
				Debug.ReportWarning("Invalid step number. Number field: {0}", components[1]);
				return false;
			}

			// validate the number
			if (number < 0 || number >= stepNumber)
			{
				Debug.ReportWarning("Invalid step number. Number: {0}, Max. step: {1}", number, stepNumber);
				return false;
			}

			int type;
			if (int.TryParse(components[2], out type) == false)
			{
				Debug.ReportWarning("Invalid step Type. Type field: {0}", components[2]);
				return false;
			}

			switch (type)
			{
				case 0:
					{
						PatternWipeStep step = new PatternWipeStep(number);

						if (step.FromNative(line, splitChars) == true)
						{
							wipeSteps.Add(step);
						}

						break;
					}
				case 1:
					{
						wipeSteps.Add(new RandomWipeStep(number));
						break;
					}
				case 2:
					{
						wipeSteps.Add(new RandomByteStep(number));
						break;
					}
				case 3:
					{
						wipeSteps.Add(new ComplementStep(number));
						break;
					}
				default:
					{
						Debug.ReportWarning("Invalid step Type. Type: {0}", type);
						return false;
					}
			}

			return true;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Set the object in a clean state
		/// </summary>
		public void Reset()
		{
			wipeSteps.Clear();
			stepNumber = 0;
			_name = string.Empty;
			_id = 0;
			_shuffle = false;
			_shuffleFirst = _shuffleLast = 0;
			_checkWipe = false;
		}


		/// <summary>
		/// Validate the method
		/// </summary>
		public bool ValidateMethod()
		{
			// id should not be 0
			if (_id == 0)
			{
				return false;
			}

			for (int i = 1; i < wipeSteps.Count; i++)
			{
				if (wipeSteps[i].Number <= wipeSteps[i - 1].Number)
				{
					return false;
				}
			}

			// shuffle order
			if (_shuffle == true && _shuffleFirst >= _shuffleLast)
			{
				return false;
			}

			// no complement step on first position
			if (wipeSteps.Count > 0 && wipeSteps[0].Type == WipeStepType.Complement)
			{
				return false;
			}

			// valid
			return true;
		}


		/// <summary>
		/// Read the method from the native format
		/// </summary>
		public bool ReadNative(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			StreamReader reader = null;

			try
			{
				reader = new StreamReader(file);

				if (reader.BaseStream.CanRead == false)
				{
					Debug.ReportWarning("Failed to open file in read mode. File: {0}", file);
					return false;
				}

				// reset the method
				Reset();

				// read the file line by line
				while (reader.EndOfStream == false)
				{
					string line = reader.ReadLine();

					if (ParseLine(line) == false)
					{
						return false;
					}
				}

				// validate the read method
				if (ValidateMethod() == false)
				{
					Reset();
					Debug.ReportWarning("Invalid wipe method. File: {0}", file);
					return false;
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while reading native method. File: {0}, Exception: {1}", file, e.Message);
				return false;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}

			return true;
		}


		/// <summary>
		/// Save the method to native format
		/// </summary>
		public bool SaveNative(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			StreamWriter writer = null;
			StringBuilder builder = new StringBuilder();

			try
			{
				writer = new StreamWriter(file);

				if (writer.BaseStream.CanWrite)
				{
					// header
					builder.AppendLine("SDM");

					// id
					builder.Append("id ");
					builder.Append(_id);
					builder.AppendLine();

					// name
					builder.Append("name ");
					builder.Append(_name.Length);
					builder.Append(" \"");
					builder.Append(_name);
					builder.AppendLine("\"");

					// step number
					builder.Append("steps ");
					builder.Append(wipeSteps.Count);
					builder.AppendLine();

					// CRC check
					builder.Append("check ");
					builder.Append(Convert.ToInt32(_checkWipe));
					builder.AppendLine();

					// shuffle
					builder.Append("shuffle ");
					builder.Append(Convert.ToInt32(_shuffle));
					builder.AppendLine();

					// shuffleFirst and shuffleLast
					if (_shuffle)
					{
						builder.Append("shuffleFirst ");
						builder.Append(_shuffleFirst);
						builder.AppendLine();
						builder.Append("shuffleLast ");
						builder.Append(_shuffleLast);
						builder.AppendLine();
					}

					// wipe steps
					if (wipeSteps.Count > 0)
					{
						foreach (WipeStepBase step in wipeSteps)
						{
							builder.Append("step ");
							builder.Append(step.ToNative());
							builder.AppendLine();
						}
					}

					writer.Write(builder.ToString());
				}
			}
			catch (Exception e)
			{
				Debug.ReportError("Error while writing native file. File: {0}, Exception {1}", file, e.Message);
				return false;
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
			}

			return true;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			WipeMethod temp = new WipeMethod();

			if (splitChars != null)
			{
				temp.splitChars = new char[splitChars.Length];
				splitChars.CopyTo(temp.splitChars,0);
			}

			if (wipeSteps != null)
			{
				temp.wipeSteps = new List<WipeStepBase>(wipeSteps.Count);

				int count = wipeSteps.Count;
				for (int i = 0; i < count; i++)
				{
					temp.wipeSteps.Add((WipeStepBase)wipeSteps[i].Clone());
				}

				temp.stepNumber = count;
			}
			else
			{
				temp.stepNumber = 0;
				temp.wipeSteps = new List<WipeStepBase>();
			}

			temp._id = _id;
			temp._name = _name;
			temp._description = _description;
			temp._author = _author;
			temp._checkWipe = _checkWipe;
			temp._shuffle = _shuffle;
			temp._shuffleFirst = _shuffleFirst;
			temp._shuffleLast = _shuffleLast;

			return temp;
		}

		#endregion
	}
}