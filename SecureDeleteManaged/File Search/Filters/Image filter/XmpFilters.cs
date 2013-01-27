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
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Xml;

namespace SecureDelete.FileSearch
{
	#region Image rating filter

	[Serializable]
	public sealed class ImageRatingFilter : ImageNumberFilter
	{
		public override ImageFilter.ImageProperty PropertyType
		{
			get { return ImageProperty.Rating; }
		}

		public ImageRatingFilter()
		{
			_enabled = true;
		}

		protected override bool AllowProperty(bool condition, ImageData data)
		{
			int? rating = XmpReader.GetRating(data);

			// presume the rating is 0 if it couldn't be retrieved
			if (rating.HasValue == false && _value > 0)
			{
				return !condition;
			}
			else
			{
				rating = 0;
			}
			
			// filter
			if (_sizeImplication == SizeImplication.Equals)
			{
				if (rating.Value == _value)
				{
					return condition;
				}
			}
			else if (_sizeImplication == SizeImplication.LessThan)
			{
				if (rating.Value <= _value)
				{
					return condition;
				}
			}
			else
			{
				if (rating.Value >= _value)
				{
					return condition;
				}
			}

			return !condition;
		}

		public override object Clone()
		{
			ImageRatingFilter temp = new ImageRatingFilter();

			temp._name = _name;
			temp._condition = _condition;
			temp._enabled = _enabled;
			temp._value = _value;
			temp._sizeImplication = _sizeImplication;

			return temp;
		}
	}

	#endregion

	#region Image tags filters

	public enum TagImplication
	{
		All, Some
	}


	public class ImageTag
	{
		#region Fields

		[NonSerialized]
		private Regex regex;

		public string Value;
		public bool   Enabled;
		public bool   MatchCase;
		public bool   RegularExpression;

		#endregion

		#region Constructor

		public ImageTag()
		{
			Enabled = true;
		}

		public ImageTag(string value) : this()
		{
			Value = value;
		}

		public ImageTag(string value, bool matchCase, bool regularExpression) : this()
		{
			Value = value;
			MatchCase = matchCase;
			RegularExpression = regularExpression;
		}

		#endregion

		#region Private methods

		private bool? IsRegexMatch(string text)
		{
			// validate data
			if (Value == null || text == null)
			{
				return null;
			}

			try
			{
				if (regex == null)
				{
					// compile the expression so it executes faster
					regex = new Regex(Value, RegexOptions.Compiled);
				}

				return regex.IsMatch(text);
			}
			catch (Exception e)
			{
				Debug.ReportError("Error in regex match. Exception: {0}", e.Message);
				return null;
			}
		}

		#endregion

		#region Public methods

		public bool MatchTag(string text)
		{
			if (RegularExpression)
			{
				bool? result = IsRegexMatch(text);

				if (result.HasValue)
				{
					return result.Value;
				}
				else
				{
					// probably invalid regex
					return false;
				}
			}
			else
			{
				return text.IndexOf(Value, MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase) >= 0;
			}
		}

		#endregion
	}


	public class ImageTagsFilter : ImageFilter
	{
		public override ImageFilter.ImageProperty PropertyType
		{
			get { return ImageProperty.Tags; }
		}

		private List<ImageTag> _tags;
		public List<ImageTag> Tags
		{
			get { return _tags; }
			set { _tags = value; }
		}

		private TagImplication _tagImplication;
		public SecureDelete.FileSearch.TagImplication TagImplication
		{
			get { return _tagImplication; }
			set { _tagImplication = value; }
		}

		public ImageTagsFilter()
		{
			_enabled = true;
			_tags = new List<ImageTag>();
		}


		private bool MatchTags(List<string> tagList, ImageTag tag)
		{
			if (tagList == null || tagList.Count == 0)
			{
				return false;
			}

			int count = tagList.Count;
			for (int i = 0; i < count; i++)
			{
				if (tag.MatchTag(tagList[i]))
				{
					// found a match
					return true;
				}
			}

			// no match found
			return false;
		}


		protected override bool AllowProperty(bool condition, ImageData data)
		{
			// get the list of tags
			List<string> tagList = XmpReader.GetTags(data);

			if (tagList != null && tagList.Count > 0)
			{
				int count  = _tags.Count;
				bool result = true;

				for (int i = 0; i < count; i++)
				{
					// skip disabled tags
					if (_tags[i].Enabled == false)
					{
						continue;
					}

					result &= MatchTags(tagList, _tags[i]);

					if (_tagImplication == TagImplication.All && result == false)
					{
						// not all tags match
						return !condition;
					}
					else if (_tagImplication == TagImplication.Some && result == true)
					{
						return condition;
					}
				}

				if (result == false)
				{
					return !condition;
				}
				else
				{
					return condition;
				}
			}

			return !condition;
		}

		public override object Clone()
		{
			ImageTagsFilter temp = new ImageTagsFilter();

			temp._name = _name;
			temp._condition = _condition;
			temp._enabled = _enabled;

			return temp;
		}
	}

	#endregion
}