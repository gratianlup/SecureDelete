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
