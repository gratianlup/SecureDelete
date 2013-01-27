using System;
using System.Collections.Generic;
using System.Text;

namespace TextParser {
    /// <summary>
    /// Interface used to define a collection of filter words
    /// </summary>
    public interface IParserFilters {
        string[] Keywords { get; }
        string[] Fields { get; }
        string[] Operators { get; }
        char[] Separators { get; }
        TextSeparator[] StringSeparators { get; }
        TextSeparator[] CommentSeparators { get; }
    }
}
