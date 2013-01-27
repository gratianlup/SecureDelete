using System;
using System.Collections.Generic;
using System.Text;

namespace TextParser.IncludedFilters {
    /// <summary>
    /// Provides definitions for keywords, operators and separators used in C#
    /// </summary>
    public class CSharpFilters : IParserFilters {
        /// <summary>
        /// Provides a list of all keywords used in C#
        /// </summary>
        private static string[] CSharpKeywords = {"abstract", "event", "new", "struct","as",
                                                  "explicit", "null", "switch","base", "extern",
                                                  "this", "false", "operator",
                                                  "throw", "break", "finally", "out", "true",
                                                  "fixed", "override", "try", "case",
                                                  "params", "typeof", "catch", "for",
                                                  "private", "foreach", "protected",
                                                  "checked", "goto", "public", "unchecked",
                                                  "class", "if", "readonly", "unsafe", "const",
                                                  "implicit", "ref", "continue", "in",
                                                  "return", "using",
                                                  "virtual", "default", "interface", "sealed",
                                                  "volatile", "delegate", "internal",
                                                  "void", "do", "is", "sizeof", "while",
                                                  "lock", "stackalloc", "else", "static",
                                                  "enum", "namespace"
                                                 };
        // expose as property
        public string[] Keywords {
            get {
                return CSharpKeywords;
            }
        }

        /// <summary>
        /// provides a list of fields defined in C#
        /// </summary>
        private static string[] CSharpFields = {"event","byte","object","bool","float",
                                                "uint","char","ulong","ushort","decimal",
                                                "int","sbyte","short","double","long","string"
                                               };

        // expose as property
        public string[] Fields {
            get {
                return CSharpFields;
            }
        }

        /// <summary>
        /// Provides a list of all operators in C#
        /// </summary>
        private static string[] CSharpOperators = { "=", "==", "<", ">", "<=", ">=", "!=", "?", "??", ":","&","|","^",
                                                   "+=", "-=","*=","/=","&=","|=","^=","+","-","*","/","&&","||","<<",">>","<<=",">>="};

        // expose as property
        public string[] Operators {
            get {
                return CSharpOperators;
            }
        }

        /// <summary>
        /// Provides a list of default separators in C#
        /// </summary>
        public static char[] CSharpSeparators = { ' ', '(', ')', '[', ']', ':', ';', ',', '{', '}', '\t', '\n' };

        // expose as property
        public char[] Separators {
            get {
                return CSharpSeparators;
            }
        }

        /// <summary>
        /// Provides a list of string separators in C#
        /// </summary>
        public static TextSeparator[] CSharpStringSeparators = { new TextSeparator("\"", "\"", '\\'), new TextSeparator("\'", "\'", '\\') };

        // expose as property
        public TextSeparator[] StringSeparators {
            get {
                return CSharpStringSeparators;
            }
        }

        /// <summary>
        /// Provides a list of comment separators in C#
        /// </summary>
        public static TextSeparator[] CSharpCommentSeparators = { new TextSeparator("//"), new TextSeparator("///"),
                                                                  new TextSeparator("/*","*/") };

        // expose as property
        public TextSeparator[] CommentSeparators {
            get {
                return CSharpCommentSeparators;
            }
        }
    }
}
