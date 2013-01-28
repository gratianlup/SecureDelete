// Copyright (c) 2007 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor
// may "SecureDelete" appear in their names without prior written
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DebugUtils.Debugger;
using System.Threading;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SecureDelete.FileSearch {
    public enum ExpressionType {
        Implication,
        Filter
    }

    public enum FilterImplication {
        AND,
        OR
    }

    [Serializable]
    public abstract class ExpressionNode : ICloneable {
        public ExpressionType Type;

        public ExpressionNode Parent;
        public ExpressionNode Left;
        public ExpressionNode Right;

        #region ICloneable Members

        public abstract object Clone();

        #endregion
    }


    [Serializable]
    public sealed class ImplicationNode : ExpressionNode, ICloneable {
        public FilterImplication Implication;

        public ImplicationNode() {
            Type = ExpressionType.Implication;
        }

        public ImplicationNode(FilterImplication implication)
            : this() {
            this.Implication = implication;
        }

        #region ICloneable Members

        public override object Clone() {
            ImplicationNode temp = new ImplicationNode(Implication);
            temp.Left = (ExpressionNode)Left.Clone();
            temp.Right = (ExpressionNode)Right.Clone();
            temp.Left.Parent = temp;
            temp.Right.Parent = temp;
            return temp;
        }

        #endregion
    }


    [Serializable]
    [XmlInclude(typeof(AttributeFilter))]
    [XmlInclude(typeof(SizeFilter))]
    [XmlInclude(typeof(DateFilter))]
    [XmlInclude(typeof(ImageFilter))]
    public sealed class FilterNode : ExpressionNode, ICloneable {
        public FilterBase Filter;

        public FilterNode() {
            Type = ExpressionType.Filter;
        }

        public FilterNode(FilterBase filter)
            : this() {
            this.Filter = filter;
        }

        #region ICloneable Members

        public override object Clone() {
            FilterNode temp = new FilterNode((FilterBase)Filter.Clone());

            if(temp.Left != null) {
                temp.Left = (ExpressionNode)Left.Clone();
                temp.Left.Parent = temp;
            }

            if(temp.Right != null) {
                temp.Right = (ExpressionNode)Right.Clone();
                temp.Right.Parent = temp;
            }

            return temp;
        }

        #endregion
    }

    public enum TreeDirection {
        Left,
        Right
    }

    [Serializable]
    [XmlInclude(typeof(ImplicationNode))]
    [XmlInclude(typeof(FilterNode))]
    public class ExpressionTree : ICloneable {
        private ExpressionNode _root;
        public ExpressionNode Root {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// Creates a new node of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="item">The item to add to the node.</param>
        /// <returns></returns>
        private ExpressionNode NewNodeByType(ExpressionType type, object item) {
            if(type == ExpressionType.Filter) {
                return new FilterNode(item as FilterBase);
            }
            else {
                return new ImplicationNode((FilterImplication)item);
            }
        }


        /// <summary>
        /// Adds the root node of the tree.
        /// </summary>
        /// <param name="type">The node type.</param>
        /// <param name="item">The item to add to the node.</param>
        public void AddRoot(ExpressionType type, object item) {
            // check the parameters
            Debug.Assert(item != null, "Item is null");
            _root = NewNodeByType(type, item);
        }


        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        public void AddNode(ExpressionNode parent, TreeDirection direction, ExpressionType type, object item) {
            // check the parameters
            Debug.Assert(parent != null && item != null, "Parent or Item are null");

            if(direction == TreeDirection.Left) {
                parent.Left = NewNodeByType(type, item);
                parent.Left.Parent = parent;
            }
            else {
                parent.Right = NewNodeByType(type, item);
                parent.Right.Parent = parent;
            }
        }


        public void RemoveChildren(ExpressionNode parent, TreeDirection direction) {
            // check the parameters
            Debug.Assert(parent != null, "Parent is null");

            if(direction == TreeDirection.Left) {
                parent.Left = null;
            }
            else {
                parent.Right = null;
            }
        }


        public void RemoveChildren(ExpressionNode parent) {
            // check the parameters
            Debug.Assert(parent != null, "Parent is null");
            parent.Left = null;
            parent.Right = null;
        }


        public void RemoveUngrouped(ExpressionNode parent) {
            // check the parameters
            Debug.Assert(parent != null, "Parent is null");
            RemoveUngroupedRecursive(parent);
        }


        private void RemoveUngroupedRecursive(ExpressionNode parent) {
            if(parent == null) {
                return;
            }

            if(parent.Left == null && parent.Right == null) {
                return;
            }

            if(parent.Left != null && parent.Right != null) {
                RemoveUngroupedRecursive(parent.Left);
                RemoveUngroupedRecursive(parent.Right);
            }
            else {
                parent.Left = parent.Right = null;
            }
        }


        /// <summary>
        /// Method executed before serialization in order to destory the parent-child relationship
        /// </summary>
        public void DestroyParentChildRelationship(ExpressionNode parent) {
            if(parent == null) {
                return;
            }

            // remove link
            parent.Parent = null;

            // remove link for children
            if(parent.Left != null) {
                DestroyParentChildRelationship(parent.Left);
            }

            if(parent.Right != null) {
                DestroyParentChildRelationship(parent.Right);
            }
        }


        /// <summary>
        /// Method executed after deserialization in order to establish the parent-child relationship
        /// </summary>
        public void EstablishParentChildRelationship(ExpressionNode parent) {
            if(parent == null) {
                return;
            }

            if(parent.Left != null) {
                parent.Left.Parent = parent;
                EstablishParentChildRelationship(parent.Left);
            }

            if(parent.Right != null) {
                parent.Right.Parent = parent;
                EstablishParentChildRelationship(parent.Right);
            }
        }


        private List<ExpressionNode> GetNodesRecursive(ExpressionNode node, ExpressionType type) {
            List<ExpressionNode> list = new List<ExpressionNode>();

            if(node == null) {
                return list;
            }

            // add the node to the category if it represents a Filter
            if(node.Type == ExpressionType.Filter) {
                list.Add(node);
            }

            // add the filters from the children
            if(node.Left != null) {
                list.AddRange(GetNodesRecursive(node.Left, type));
            }
            if(node.Right != null) {
                list.AddRange(GetNodesRecursive(node.Right, type));
            }

            return list;
        }


        public List<ExpressionNode> GetNodes(ExpressionType type) {
            return GetNodesRecursive(_root, type);
        }

        #region ICloneable Members

        public object Clone() {
            ExpressionTree temp = new ExpressionTree();
            temp._root = (ExpressionNode)_root.Clone();
            return temp;
        }

        #endregion

        #region Serialization helpers

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context) {
            DestroyParentChildRelationship(_root);
        }


        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context) {
            EstablishParentChildRelationship(_root);
        }

        #endregion
    }


    public class ExpressionTreeBuilder {
        public static ExpressionTree AssociateFilters(FilterBase a, FilterBase b, 
                                                      FilterImplication implication) {
            // check the parameters
            Debug.Assert(a != null && b != null, "Null parameters");

            ExpressionTree result = new ExpressionTree();
            result.AddRoot(ExpressionType.Implication, implication);
            result.AddNode(result.Root, TreeDirection.Left, ExpressionType.Filter, a);
            result.AddNode(result.Root, TreeDirection.Right, ExpressionType.Filter, b);
            return result;
        }


        public static ExpressionTree AssociateTreeWithFilter(ExpressionTree tree, FilterBase filter, 
                                                             FilterImplication implication, TreeDirection treeDirection) {
            // check the parameters
            Debug.Assert(tree != null && filter != null, "Null parameters");
            ExpressionTree result = new ExpressionTree();

            if(treeDirection == TreeDirection.Left) {
                result.AddRoot(ExpressionType.Implication, implication);
                result.Root.Left = tree.Root;
                result.AddNode(result.Root, TreeDirection.Right, ExpressionType.Filter, filter);
            }
            else {
                result.AddRoot(ExpressionType.Implication, implication);
                result.Root.Right = tree.Root;
                result.AddNode(result.Root, TreeDirection.Left, ExpressionType.Filter, filter);
            }

            return result;
        }


        public static ExpressionTree AssociateTrees(ExpressionTree a, ExpressionTree b, FilterImplication implication) {
            // check the parameters
            Debug.Assert(a != null && b != null, "Null parameters");

            ExpressionTree result = new ExpressionTree();
            result.AddRoot(ExpressionType.Implication, implication);
            result.Root.Left = a.Root;
            result.Root.Right = b.Root;
            return result;
        }
    }
}
