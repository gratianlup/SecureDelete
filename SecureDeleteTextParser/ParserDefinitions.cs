using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TextParser {
    #region Private definitions

    /// <summary>
    /// Structure for defining the start and end strings of a text separator
    /// </summary>
    [DebuggerDisplay("Start = {start,nq} {hasEnd == false ? \"\" : \"End = \" + end,nq}")]
    public struct TextSeparator {
        public string start;
        public bool hasEnd;
        public string end;
        public bool hasNotPrior;
        public char notPrior;

        #region Constructor

        public TextSeparator(string s) {
            start = s;
            end = "";
            hasEnd = false;
            hasNotPrior = false;
            notPrior = ' ';
        }

        public TextSeparator(string s, string e) {
            start = s;
            end = e;
            hasEnd = true;
            hasNotPrior = false;
            notPrior = ' ';
        }

        public TextSeparator(string s, string e, char np) {
            start = s;
            end = e;
            hasEnd = true;
            hasNotPrior = true;
            notPrior = np;
        }

        #endregion
    }

    #endregion

    #region Public definitions

    /// <summary>
    /// The categories a text could belong
    /// </summary>
    public enum TextBlockType {
        Keyword, Operator, Separator, String, Comment, Field, Number, Unknown
    }


    /// <summary>
    /// Structure for defining a text block by its start and end index an category
    /// </summary>
    [DebuggerDisplay("Text = {Text} Type = {Type}")]
    public struct TextBlock {
        #region Properties

        /// <summary>
        /// The text described by this textblock
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string text;
        public string Text {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// The index at which the this text starts
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int start;
        public int StartIndex {
            get { return start; }
            set { start = value; }
        }

        /// <summary>
        /// The index at which the this text ends
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int end;
        public int EndIndex {
            get { return end; }
            set { end = value; }
        }

        /// <summary>
        /// The type of text (keyword,operator,string,comment,etc)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextBlockType type;
        public TextBlockType Type {
            get { return type; }
            set { type = value; }
        }

        #endregion
    }

    #endregion
}
