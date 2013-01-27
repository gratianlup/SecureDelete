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
	public class ImageData : IHelperObject
	{
		#region Fields

		public FileStream Stream;       // used by EXIF and XMP filters
		public Image Image;             // used by EXIF filters
		public ASCIIEncoding Ascii;     // used by EXIF filters
		public XmlDocument XmpDocument; // used by XMP filters

		#endregion

		#region Public methods

		public bool LoadImage(string file)
		{
			try
			{
				Stream = File.OpenRead(file);
				// load only the metadata
				Image = Image.FromStream(Stream, false, false);

				//Image = Image.FromFile(file);
				Ascii = new ASCIIEncoding();
			}
			catch (Exception e)
			{
				Debug.ReportError("Failed to open image {0}. Exception: {1}", file, e.Message);
				return false;
			}

			return true;
		}

		#endregion

		#region IHelperObject Members

		public void Dispose()
		{
			if (Stream != null)
			{
				Stream.Close();
			}

			if (Image != null)
			{
				Image.Dispose();
			}

			Image = null;
			Stream = null;
			XmpDocument = null;
		}

		#endregion
	}

	#region Abstract objects

	[Serializable]
	public abstract class ImageFilter : FilterBase
	{
		#region Constants

		private readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".tiff" };

		#endregion

		#region Nested types

		public enum ImageProperty
		{
			Title,  CameraMaker, CameraModel,  Orientation,  Software, 
			Author, Copyright,   ExposureTime, FNumber,      ExposureProgram, FocalLength,
			ISO,    DateTaken,   ExposureBias, MeteringMode, FlashFired, /* EXIF metadata */
			Rating, Tags /* XMP metadata */
		}

		#endregion

		#region Properties

		/// <summary>
		/// Abstract property that needs to be implemented in the derived objects
		/// </summary>
		public abstract ImageProperty PropertyType { get; }

		#endregion

		#region Private methods

		private bool IsImage(string file)
		{
			// check if it's an image
			FileInfo fileInfo = new FileInfo(file);
			string extension = fileInfo.Extension.ToLower();

			for (int i = 0; i < ImageExtensions.Length; i++)
			{
				if (extension == ImageExtensions[i])
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Abstract method that needs to be implemented in the derived objects
		/// </summary>
		protected abstract bool AllowProperty(bool condition,ImageData data);


		public override bool Allow(string file)
		{
			Debug.AssertNotNull(file, "File is null");

			bool condition = _condition == FilterCondition.IS ? true : false;
			ImageData data = null;

			if (_helperObject != null)
			{
				if (_helperObject is ImageData)
				{
					_helperObject = (ImageData)_helperObject;
				}
				else
				{
					// dispose the object
					_helperObject.Dispose();
				}
			}
			else
			{
				// create a new helper object
				_helperObject = new ImageData();
				data = (ImageData)_helperObject;

				// load the picture
				if (IsImage(file) == false || data.LoadImage(file) == false)
				{
					// failed
					return !condition;
				}
			}

			return AllowProperty(condition, data);
		}

		#endregion
	}


	[Serializable]
	public abstract class ImageNumberFilter : ImageFilter
	{
		#region Constants

		protected const double Epsilon = 0.00001;

		#endregion

		#region Properties

		protected SizeImplication _sizeImplication;
		public SizeImplication SizeImplication
		{
			get { return _sizeImplication; }
			set { _sizeImplication = value; }
		}

		protected double _value;
		public double Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion
	}

	[Serializable]
	public abstract class ImageTextFilter : ImageFilter
	{
		#region Fields

		[NonSerialized]
		private Regex regex;

		#endregion

		#region Properties

		protected bool _matchCase;
		public bool MatchCase
		{
			get { return _matchCase; }
			set { _matchCase = value; }
		}

		protected bool _regularExpression;
		public bool RegularExpression
		{
			get { return _regularExpression; }
			set { _regularExpression = value; }
		}

		protected string _value;
		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		#endregion

		#region Methods

		protected bool? IsRegexMatch(string text)
		{
			// validate data
			if (_value == null || text == null)
			{
				return null;
			}

			try
			{
				if (regex == null)
				{
					// compile the expression so it executes faster
					regex = new Regex(_value, RegexOptions.Compiled);
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
	}

	#endregion

	#region Helper objects

	public class ImageFilterProvider
	{
		public static ImageFilter GetImageFilter(ImageFilter.ImageProperty type)
		{
			switch (type)
			{
				case ImageFilter.ImageProperty.Author:
					{
						return new ImageAuthorFilter();
					}
				case ImageFilter.ImageProperty.CameraMaker:
					{
						return new ImageCameraMakerFilter();
					}
				case ImageFilter.ImageProperty.CameraModel:
					{
						return new ImageCameraModelFilter();
					}
				case ImageFilter.ImageProperty.Copyright:
					{
						return new ImageCopyrightFilter();
					}
				case ImageFilter.ImageProperty.DateTaken:
					{
						return new ImageDateTakenFilter();
					}
				case ImageFilter.ImageProperty.ExposureBias:
					{
						return new ImageExposureBiasFilter();
					}
				case ImageFilter.ImageProperty.ExposureProgram:
					{
						return new ImageExposureProgramFilter();
					}
				case ImageFilter.ImageProperty.ExposureTime:
					{
						return new ImageExposureTimeFilter();
					}
				case ImageFilter.ImageProperty.FlashFired:
					{
						return new ImageFlashFiredFilter();
					}
				case ImageFilter.ImageProperty.FNumber:
					{
						return new ImageFNumberFilter();
					}
				case ImageFilter.ImageProperty.FocalLength:
					{
						return new ImageFNumberFilter();
					}
				case ImageFilter.ImageProperty.ISO:
					{
						return new ImageIsoFilter();
					}
				case ImageFilter.ImageProperty.MeteringMode:
					{
						return new ImageMeteringModeFilter();
					}
				case ImageFilter.ImageProperty.Orientation:
					{
						return new ImageOrientationFilter();
					}
				case ImageFilter.ImageProperty.Software:
					{
						return new ImageSoftwareFilter();
					}
				case ImageFilter.ImageProperty.Title:
					{
						return new ImageTitleFilter();
					}
			}

			// not found
			return null;
		}
	}

	#endregion
}