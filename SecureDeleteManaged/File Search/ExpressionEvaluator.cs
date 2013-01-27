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

namespace SecureDelete.FileSearch
{
	public class ExpressionEvaluator
	{
		#region Nested types

		private enum ElementType
		{
			Tree, Filter, Implication
		}

		private struct ExpressionElement
		{
			public ElementType Type;
			public object Element;
		}

		#endregion

		#region Constants

		public const char ExpressionStartChar = '(';
		public const char ExpressionEndChar = ')';
		public const string AndImplication = "AND";
		public const string OrImplication = "OR";
		private readonly char[] ExpressionSeparators = new char[] { ' ', ExpressionStartChar, ExpressionEndChar, '\t', '\n' };

		#endregion

		#region Constructor

		public ExpressionEvaluator()
		{
		}

		#endregion

		#region Properties

		private FileFilter _fileFilter;
		public FileFilter Filters
		{
			get { return _fileFilter; }
			set { _fileFilter = value; }
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Verifies the parantesis.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		private bool VerifyParantesis(string expression)
		{
			int open = 0;
			int close = 0;

			int length = expression.Length;
			for (int i = 0; i < length; i++)
			{
				if (expression[i] == ExpressionStartChar)
				{
					open++;
				}
				else if (expression[i] == ExpressionEndChar)
				{
					close++;
				}
			}

			return open == close;
		}


		/// <summary>
		/// Gets the type of the implication.
		/// </summary>
		/// <param name="s">The string.</param>
		/// <param name="implication">The implication.</param>
		/// <returns></returns>
		private bool GetImplicationType(string s, out FilterImplication implication)
		{
			implication = FilterImplication.AND;

			if (s == null || s.Length == 0)
			{
				return false;
			}

			if (s == AndImplication)
			{
				implication = FilterImplication.AND;
				return true;
			}
			else if (s == OrImplication)
			{
				implication = FilterImplication.OR;
				return true;
			}

			// no valid Implication
			return false;
		}


		/// <summary>
		/// Gets the filter with the specified name.
		/// </summary>
		/// <param name="s">The string.</param>
		/// <returns></returns>
		private FilterBase GetFilter(string s)
		{
			if (_fileFilter == null)
			{
				throw new Exception("FileFilter not set");
			}

			int count = _fileFilter.FilterCount;
			for (int i = 0; i < count; i++)
			{
				FilterBase filter = _fileFilter.GetFilter(i);

				if (filter.Name != null && filter.Name == s)
				{
					return filter;
				}
			}

			return null;
		}


		/// <summary>
		/// Associates the given tree elements.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="elementCount">The element count.</param>
		/// <returns></returns>
		private ExpressionTree AssociateElements(ExpressionElement[] elements,int elementCount)
		{
			if (elementCount == 2)
			{
				throw new Exception("Invalid expression element count");
			}

			// handle special case
			if (elementCount == 1)
			{
				if (elements[0].Type == ElementType.Implication)
				{
					return null;
				}
				else if (elements[0].Type == ElementType.Tree)
				{
					return (ExpressionTree)elements[0].Element;
				}
				else
				{
					ExpressionTree tree = new ExpressionTree();
					tree.Root = new FilterNode((FilterBase)elements[0].Element);

					return tree;
				}
			}

			// normal case
			// middle element should be an Implication
			if (elements[1].Type != ElementType.Implication)
			{
				return null;
			}

			ExpressionTree parent = new ExpressionTree();

			ExpressionElement left = elements[0];
			ExpressionElement right = elements[2];
			FilterImplication implication = (FilterImplication)elements[1].Element;

			// build the tree
			if (left.Type == ElementType.Tree && right.Type == ElementType.Filter)
			{
				parent = ExpressionTreeBuilder.AssociateTreeWithFilter((ExpressionTree)left.Element,
																	   (FilterBase)right.Element,
																		implication,TreeDirection.Left);
			}
			else if (left.Type == ElementType.Filter && right.Type == ElementType.Tree)
			{
				parent = ExpressionTreeBuilder.AssociateTreeWithFilter((ExpressionTree)right.Element,
																	   (FilterBase)left.Element,
																		implication,TreeDirection.Right);
			}
			else if (left.Type == ElementType.Tree && right.Type == ElementType.Tree)
			{
				parent = ExpressionTreeBuilder.AssociateTrees((ExpressionTree)left.Element,
															  (ExpressionTree)right.Element,
															   implication);
			}
			else if (left.Type == ElementType.Filter && right.Type == ElementType.Filter)
			{
				parent = ExpressionTreeBuilder.AssociateFilters((FilterBase)left.Element,
																(FilterBase)right.Element,
																implication);
			}

			return parent;
		}


		/// <summary>
		/// Parses the expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="start">The start position.</param>
		/// <param name="tree">The tree.</param>
		/// <param name="expressionLength">Length of the expression.</param>
		/// <returns></returns>
		private bool ParseExpression(string expression, int start, out ExpressionTree tree, out int expressionLength)
		{
			tree = null;
			expressionLength = 0;

			if (start < 0 || start >= expression.Length)
			{
				return false;
			}

			// get the end position
			int position = start;

			ExpressionElement[] elements = new ExpressionElement[3];
			int elementPosition = 0;
			int length = expression.Length;

			while (position < length)
			{
				// jump over spaces
				if (expression[position] == ' ' || expression[position] == ExpressionEndChar)
				{
					position++;
					continue;
				}

				bool positionChanged = false;

				if (expression[position] == ExpressionStartChar)
				{
					ExpressionTree child;
					int subexpressionLength;

					// invalid expression
					if (ParseExpression(expression, position + 1, out child, out subexpressionLength) == false)
					{
						return false;
					}

					// invalid expression
					if (elementPosition == 1)
					{
						return false;
					}
					
					elements[elementPosition].Type = ElementType.Tree;
					elements[elementPosition].Element = child;
					elementPosition++;
					position += subexpressionLength + 1;
					positionChanged = true;
				}
				else
				{
					// get the first substring
					string[] components = expression.Substring(position).Split(ExpressionSeparators, StringSplitOptions.RemoveEmptyEntries);

					// invalid expression
					if (components.Length == 0)
					{
						return false;
					}

					FilterImplication implication;
					if (GetImplicationType(components[0], out implication))
					{
						// valid Implication
						if (elementPosition != 1)
						{
							// invalid expression
							return false;
						}

						elements[elementPosition].Type = ElementType.Implication;
						elements[elementPosition].Element = implication;
						elementPosition++;
					}
					else
					{
						// Filter name
						if (elementPosition == 1)
						{
							// invalid expression
							return false;
						}

						// get the Filter
						FilterBase filter = GetFilter(components[0]);

						// invalid expression
						if (filter == null)
						{
							return false;
						}

						elements[elementPosition].Type = ElementType.Filter;
						elements[elementPosition].Element = filter;
						elementPosition++;
					}

					position += components[0].Length + 1;
					positionChanged = true;
				}

				if (positionChanged == false)
				{
					position++;
				}

				if (position >= length || elementPosition >= 3)
				{
					// end of expression, build evaluation tree
					if (elementPosition == 2)
					{
						// invalid expression
						return false;
					}

					// build the tree
					tree = AssociateElements(elements,elementPosition);
					expressionLength = position - start;
					return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Converts the tree to string representation.
		/// </summary>
		/// <param name="node">The base node.</param>
		/// <returns></returns>
		private string TreeToString(ExpressionNode node)
		{
			if (node == null)
			{
				return string.Empty;
			}

			StringBuilder builder = new StringBuilder();

			if (node.Left != null)
			{
				if (node.Left.Type == ExpressionType.Implication)
				{
					builder.Append(ExpressionStartChar);
				}

				builder.Append(TreeToString(node.Left));
				
				if (node.Left.Type == ExpressionType.Implication)
				{
					builder.Append(ExpressionEndChar);
				}
			}

			if (node.Type == ExpressionType.Implication)
			{
				builder.Append(" ");
				ImplicationNode implication = (ImplicationNode)node;

				if (implication.Implication == FilterImplication.AND)
				{
					builder.Append(AndImplication);
				}
				else if (implication.Implication == FilterImplication.OR)
				{
					builder.Append(OrImplication);
				}

				builder.Append(" ");
			}
			else
			{
				FilterNode filter = (FilterNode)node;

				if (filter.Filter.Name != null)
				{
					builder.Append(filter.Filter.Name);
				}
			}

			if (node.Right != null)
			{
				if (node.Right.Type == ExpressionType.Implication)
				{
					builder.Append(ExpressionStartChar);
				}

				builder.Append(TreeToString(node.Right));

				if (node.Right.Type == ExpressionType.Implication)
				{
					builder.Append(ExpressionEndChar);
				}
			}

			return builder.ToString();
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Evaluates the expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public ExpressionTree EvaluateExpression(string expression)
		{
			// validate parameters
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			if (VerifyParantesis(expression) == false)
			{
				return null;
			}

			ExpressionTree tree;
			int length;

			// return the resulted tree from parsing
			if (ParseExpression(expression, 0, out tree, out length))
			{
				return tree;
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// Gets the expression tree string.
		/// </summary>
		/// <param name="tree">The tree.</param>
		/// <returns></returns>
		public string GetExpressionTreeString(ExpressionTree tree)
		{
			if (tree == null)
			{
				throw new ArgumentNullException("tree");
			}

			return TreeToString(tree.Root).Trim();
		}

		#endregion
	}
}
