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
	#region Filters base

	public enum FilterCondition
	{
		IS, IS_NOT
	}

	public interface IHelperObject
	{
		void Dispose();
	}

	[Serializable]
	public abstract class FilterBase : ICloneable
	{
		#region Properties

		protected FilterCondition _condition;
		public FilterCondition Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}

		protected bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		protected string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		protected IHelperObject _helperObject;
		public IHelperObject HelperObject
		{
			get { return _helperObject; }
			set { _helperObject = value; }
		}

		#endregion

		#region Virtual methods

		public abstract bool Allow(string file);
		public abstract object Clone();

		#endregion
	}

	#endregion

	#region Common types

	public enum SizeImplication
	{
		Equals, LessThan, GreaterThan
	}

	public enum DateImplication
	{
		From, NewerOrFrom, OlderOrFrom
	}

	#endregion


	[Serializable]
	public class FileFilter : ICloneable
	{
		#region Fields

		private List<FilterBase> filters;
		private bool treeValidated;
		[NonSerialized]
		private IHelperObject helperObject;

		#endregion

		#region Constructor

		public FileFilter()
		{
			filters = new List<FilterBase>();
		}

		#endregion

		#region Properties

		public int FilterCount
		{
			get { return filters.Count; }
		}

		public List<FilterBase> Filters
		{
			get { return filters; }
		}

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}


		private ExpressionTree _expressionTree;
		public ExpressionTree ExpressionTree
		{
			get { return _expressionTree; }
			set
			{
				_expressionTree = value;

				if (ValidateTree() == false)
				{
					_expressionTree = null;
				}
			}
		}

		#endregion

		#region Private methods

		private bool ValidateTree()
		{
			treeValidated = true;

			if (_expressionTree == null)
			{
				return false;
			}

			// get the category of filters found in the expression tree
			List<ExpressionNode> nodes = _expressionTree.GetNodes(ExpressionType.Filter);

			// check if the filters are in the category
			for (int i = 0; i < nodes.Count; i++)
			{
				if (filters.Contains(((FilterNode)nodes[i]).Filter) == false)
				{
					return false;
				}
			}

			return true;
		}


		private bool EvaluateTree(string file)
		{
			if (_expressionTree == null || _expressionTree.Root == null)
			{
				return true;
			}

			// validate the tree
			if (treeValidated == false)
			{
				if (ValidateTree() == false)
				{
					return true;
				}
			}

			return EvaluateTreeRecursive(_expressionTree.Root,file);
		}


		private bool EvaluateTreeRecursive(ExpressionNode node,string file)
		{
			if (node == null)
			{
				return true;
			}

			if (node.Type == ExpressionType.Filter)
			{
				return ((FilterNode)node).Filter.Allow(file);
			}
			else
			{
				if (node.Left == null || node.Right == null)
				{
					// invalid tree
					Debug.ReportWarning("Invalid expression tree");
					return true;
				}
				else
				{
					bool resultA = EvaluateTreeRecursive(node.Left, file);
					bool resultB = EvaluateTreeRecursive(node.Right, file);

					if (((ImplicationNode)node).Implication == FilterImplication.AND)
					{
						return (resultA && resultB);
					}
					else
					{
						return (resultA || resultB);
					}
				}
			}
		}


		private void DisposeHelperObject()
		{
			if (helperObject != null)
			{
				helperObject.Dispose();
				helperObject = null;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Add the Filter
		/// </summary>
		/// <param name="Filter">The Filter to add.</param>
		/// <remarks>Trows an exception if the Filter is already in added.</remarks>
		public void AddFilter(FilterBase filter)
		{
			// check the parameters
			Debug.AssertNotNull(filter, "Filters is null");
			Debug.Assert(filters.Contains(filter) == false, "Filters already in the category");

			filters.Add(filter);
			treeValidated = false;
		}


		/// <summary>
		/// Remove the Filter
		/// </summary>
		/// <param name="Filter">The Filter to remove.</param>
		public bool RemoveFilter(FilterBase filter)
		{
			// check the parameters
			Debug.AssertNotNull(filter, "Filters is null");

			if (filters.Contains(filter))
			{
				filters.Remove(filter);
				treeValidated = false;

				return true;
			}

			return false;
		}
		

		/// <summary>
		/// Remove all filters
		/// </summary>
		public void RemoveAllFilters()
		{
			filters.Clear();
			treeValidated = false;
		}


		/// <summary>
		/// Get the Filter
		/// </summary>
		/// <param name="index">The index where the Filter is located.</param>
		/// <returns></returns>
		public FilterBase GetFilter(int index)
		{
			// check the parameters
			Debug.Assert(index >= 0 && index < filters.Count, "Index out of range");

			return filters[index];
		}


		/// <summary>
		/// Get all filters matching the specified Type
		/// </summary>
		/// <param name="filterType">The Type of the Filter.</param>
		public FilterBase[] GetFilters(Type filterType)
		{
			// check the parameters
			Debug.AssertNotNull(filterType, "FilterType is null");

			List<FilterBase> f = new List<FilterBase>();

			for (int i = 0; i < filters.Count; i++)
			{
				if (filters[i].GetType().IsAssignableFrom(filterType))
				{
					f.Add(filters[i]);
				}
			}

			return f.ToArray();
		}


		/// <summary>
		/// Checks if the file matches the conditions imposed by the filters
		/// </summary>
		/// <param name="file">The file to check.</param>
		public bool AllowFile(string file)
		{
			// check the parameters
			Debug.AssertNotNull(file, "File is null");

			// allow all files if the Filter is disabled
			if (_enabled == false)
			{
				return true;
			}

			// use the expression tree to Filter the file
			if (_expressionTree != null)
			{
				return EvaluateTree(file);
			}

			// perform basic filtering
			int count = filters.Count;
			bool result = true;
			helperObject = null;

			for (int i = 0; i < count; i++)
			{
				FilterBase filter = filters[i];

				// skip disabled filters
				if (filter.Enabled == false)
				{
					continue;
				}

				filter.HelperObject = helperObject;
				result &= filter.Allow(file);
				helperObject = filter.HelperObject;
				filter.HelperObject = null;

				// return immediately if result is false
				if (result == false)
				{
					DisposeHelperObject();
					return false;
				}
			}
			
			// dispose the helper
			DisposeHelperObject();

			return result;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			FileFilter temp = new FileFilter();
			temp._enabled = _enabled;

			if (filters != null)
			{
				temp.filters = new List<FilterBase>(filters.Count);

				// clone the filters
				foreach (FilterBase filter in filters)
				{
					temp.filters.Add((FilterBase)filter.Clone());
				}
			}

			if (_expressionTree != null)
			{
				temp._expressionTree = (ExpressionTree)_expressionTree.Clone();
			}

			return temp;
		}

		#endregion
	}
}
