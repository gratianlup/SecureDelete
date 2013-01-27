using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DebugUtils.Debugger;

namespace TextParser {
    #region Definitions

    public class Parser {
        #region Fields

        private string text;

        #region Filters

        List<string> keywords;
        List<string> operators;
        List<string> fields;
        List<char> separators;
        List<TextSeparator> stringSeparators;
        List<TextSeparator> commentSeparators;

        // optimization
        List<int> keywordsPriority;
        List<int> operatorsPriority;
        List<int> fieldsPriority;

        #endregion

        #region Parsing

        private bool[] keywordInvalid;

        private bool inString;
        private int stringStartIndex;
        private bool inComment;
        private int commentStartIndex;
        private TextSeparator usedSeparator;

        private int textIndex;
        private int lastIndex;
        private bool newWord;
        private bool newWordBySeparator;
        private bool inKeywordInvalid;
        private bool wasInKeywordInvalid;
        private int keywordInvalidStartIndex;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the structures used by the parser.
        /// </summary>
        /// <remarks>Called by the constructor.</remarks>
        private void InitParser() {
            keywords = new List<string>();
            fields = new List<string>();
            operators = new List<string>();
            separators = new List<char>();
            stringSeparators = new List<TextSeparator>();
            commentSeparators = new List<TextSeparator>();

            keywordsPriority = new List<int>();
            fieldsPriority = new List<int>();
            operatorsPriority = new List<int>();

            keywordInvalid = new bool[256];
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Parser() {
            InitParser();
        }

        /// <summary>
        /// Constructor with the possibility to load the word filters.
        /// </summary>
        /// <param name="filters"></param>
        public Parser(IParserFilters filters) {
            // default initialization
            InitParser();

            LoadFilters(filters);
        }

        #endregion

        #region Properties

        #region Filters

        public List<string> KeywordFiltes {
            get { return keywords; }
        }

        public List<string> FieldFilters {
            get { return fields; }
        }

        public List<string> OperatorFiltes {
            get { return operators; }
        }

        public List<char> SeparatorFiltes {
            get { return separators; }
        }

        public List<TextSeparator> StringSeparatorFilters {
            get { return stringSeparators; }
        }

        public List<TextSeparator> CommentSeparatorFilters {
            get { return commentSeparators; }
        }

        #endregion

        /// <summary>
        /// The text to be parsed.
        /// </summary>
        public string Text {
            get {
                return text;
            }
            set {
                text = value;
            }
        }

        #endregion

        #region Private methods

        #region Parsing

        /// <summary>
        /// Checks if the given string is a keyword.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string is a keyword.
        /// FALSE if the string is not a keyword.
        /// </returns>
        /// <remarks>Optimized</remarks>
        private bool IsKeyword(string s) {
            for(int i = 0; i < keywordsPriority.Count; i++) {
                if(keywords[keywordsPriority[i]].Equals(s)) {
                    if(i != 0) {
                        // bring in front of the list
                        int temp = keywordsPriority[i];
                        keywordsPriority.RemoveAt(i);
                        keywordsPriority.Insert(0, temp);
                    }

                    return true;
                }
            }

            // not found
            return false;
        }


        /// <summary>
        /// Checks if the given string is a field.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string is a field.
        /// FALSE if the string is not a field.
        /// </returns>
        /// <remarks>Optimized</remarks>
        private bool IsField(string s) {
            for(int i = 0; i < fieldsPriority.Count; i++) {
                if(fields[fieldsPriority[i]].Equals(s)) {
                    if(i != 0) {
                        // bring in front of the list
                        int temp = fieldsPriority[i];
                        fieldsPriority.RemoveAt(i);
                        fieldsPriority.Insert(0, temp);
                    }

                    return true;
                }
            }

            // not found
            return false;
        }


        /// <summary>
        /// Checks if the given string is an operator.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string is an operator.
        /// FALSE if the string is not a operator.
        /// </returns>
        /// <remarks>Optimized</remarks>
        private bool IsOperator(string s) {
            for(int i = 0; i < operatorsPriority.Count; i++) {
                if(operators[operatorsPriority[i]].Equals(s)) {
                    // bring in front of the list
                    if(i != 0) {
                        int temp = operatorsPriority[i];
                        operatorsPriority.RemoveAt(i);
                        operatorsPriority.Insert(0, temp);
                    }

                    return true;
                }
            }

            // not found
            return false;
        }


        /// <summary>
        /// Checks if the given string is a separator.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string is a separator.
        /// FALSE if the string is not a separator.
        /// </returns>
        private bool IsSeparator(char c) {
            for(int i = 0; i < separators.Count; i++) {
                if(separators[i] == c) {
                    return true;
                }
            }

            // not found
            return false;
        }


        /// <summary>
        /// Checks if the given string is a string separator.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// The string separator if the string is a string separator.
        /// NULL if the string is not a string separator.
        /// </returns>
        private TextSeparator? IsStringSeparator(string s) {
            for(int i = 0; i < stringSeparators.Count; i++) {
                if(stringSeparators[i].start.Equals(s) ||
                   (stringSeparators[i].hasEnd && stringSeparators[i].end.Equals(s))) {
                    return stringSeparators[i];
                }
            }

            return null;
        }


        /// <summary>
        /// Checks if the given string is a comment separator.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// The comment separator if the string is a comment separator.
        /// NULL if the string is not a comment separator.
        /// </returns>
        private TextSeparator? IsCommentSeparator(string s) {
            for(int i = 0; i < commentSeparators.Count; i++) {
                if(commentSeparators[i].start.Equals(s) ||
                   (commentSeparators[i].hasEnd && commentSeparators[i].end.Equals(s))) {
                    return commentSeparators[i];
                }
            }

            return null;
        }


        /// <summary>
        /// Checks if the given character is part of a string or comment separator.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the character is a part of a separator string/comment separator.
        /// FALSE if the character is not a part of a separator string/comment separator.
        /// </returns>
        private bool IsPartOfKeywordInvalid(char c) {
            int index = Convert.ToInt32(c);

            if(index > 0 && index < 256) {
                return keywordInvalid[index];
            }

            return false;
        }


        /// <summary>
        /// Checks if a string contains a specific character.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string contains the character.
        /// FALSE if the string does not contain the character.
        /// </returns>
        private bool StringContainsChar(string s, char c) {
            if(s == null) {
                return false;
            }

            for(int i = 0; i < s.Length; i++) {
                if(s[i] == c) {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Checks if a string represents a number;
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        /// TRUE if the string represents a number.
        /// FALSE if the string does not represent a number.
        /// </returns>
        private bool IsNumber(string s) {
            if(s == null) {
                return false;
            }

            if(s.Length > 0) {
                return char.IsNumber(s, 0);
            }
            else {
                return false;
            }
        }

        #endregion

        #endregion

        #region Public methods

        #region Filters

        /// <summary>
        /// Load the filters from the given IParserFilters object.
        /// </summary>
        /// <param name="filters">The IParserFilters derived class from where to load the filters.</param>
        public void LoadFilters(IParserFilters filters) {
            Debug.AssertNotNull(filters, "Parser filters parameter is null");

            // load each type of filter
            if(filters.Keywords != null) {
                foreach(string s in filters.Keywords) {
                    keywords.Add(s);
                }
            }

            if(filters.Fields != null) {
                foreach(string s in filters.Fields) {
                    fields.Add(s);
                }
            }

            if(filters.Operators != null) {
                foreach(string s in filters.Operators) {
                    operators.Add(s);
                }
            }

            if(filters.Separators != null) {
                foreach(char c in filters.Separators) {
                    separators.Add(c);
                }
            }

            if(filters.StringSeparators != null) {
                foreach(TextSeparator ts in filters.StringSeparators) {
                    stringSeparators.Add(ts);
                }
            }

            if(filters.CommentSeparators != null) {
                foreach(TextSeparator ts in filters.CommentSeparators) {
                    commentSeparators.Add(ts);
                }
            }
        }

        /// <summary>
        /// Removes all loaded filters.
        /// </summary>
        public void RemoveAllFilters() {
            keywords.Clear();
            operators.Clear();
            separators.Clear();
            stringSeparators.Clear();
            commentSeparators.Clear();
        }

        #endregion

        #region Text loading

        /// <summary>
        /// Loads the text from the given stream.
        /// </summary>
        /// <param name="stream">The stream from where to load the filters.</param>
        /// <returns>
        /// TRUE if the text could be loaded.
        /// FALSE if the text could not be loaded.
        /// </returns>
        public bool LoadText(StreamReader stream) {
            if(stream == null || stream.BaseStream.CanRead == false) {
                Debug.ReportError("Cannot read from stream");
                return false;
            }

            try {
                StringBuilder builder = new StringBuilder();

                int ct = 0;

                while(stream.EndOfStream == false) {
                    if(ct != 0) {
                        builder.Append('\n');
                    }

                    builder.Append(stream.ReadLine());
                    ct++;
                }

                text = builder.ToString();
            }
            catch {
                Debug.ReportError("Exception thrown when trying to read from stream");
            }

            return true;
        }

        /// <summary>
        /// Add the field from the given assembly as field filters.
        /// </summary>
        /// <param name="name">The name of the assembly or the exact path to it.</param>
        /// <param name="addReferences">Add the fields from the assemblies referenced by the given one.</param>
        /// <param name="fromNamespace">Add fields only from the given namespaces.</param>
        /// <returns>
        /// TRUE if the fields from the assembly could be loaded.
        /// FALSE if the fields from the assembly could not be loaded.
        /// </returns>
        public bool AddFieldsFromAssembly(string name, bool addReferences, string[] fromNamespace) {
            if(name == null) {
                return false;
            }

            try {
                Assembly a;

                if(File.Exists(name)) {
                    a = Assembly.LoadFile(name);
                }
                else {
                    a = Assembly.Load(name);
                }

                // add referenced assemblies
                if(addReferences) {
                    foreach(AssemblyName an in a.GetReferencedAssemblies()) {
                        AddFieldsFromAssembly(an.FullName, false, fromNamespace);
                    }
                }

                // add the fields
                Type[] t = a.GetTypes();

                string[] fromNamespaceLower = null;
                if(fromNamespace != null && fromNamespace.Length > 0) {
                    fromNamespaceLower = new string[fromNamespace.Length];
                    for(int i = 0; i < fromNamespace.Length; i++) {
                        fromNamespaceLower[i] = fromNamespace[i].ToLower();
                    }
                }

                for(int i = 0; i < t.Length; i++) {
                    if(fields.Contains(t[i].Name) == false) {
                        // add only if it has the specified namespace
                        if(fromNamespaceLower != null) {
                            for(int j = 0; j < fromNamespaceLower.Length; j++) {
                                if(t[i].Namespace.ToLower().Contains(fromNamespaceLower[j])) {
                                    fields.Add(t[i].Name);
                                    break;
                                }
                            }
                        }
                        else {
                            fields.Add(t[i].Name);
                        }
                    }
                }
            }
            catch(Exception e) {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Add the field from the given assembly as field filters.
        /// </summary>
        /// <param name="name">The name of the assembly or the exact path to it.</param>
        /// <returns>
        /// TRUE if the fields from the assembly could be loaded.
        /// FALSE if the fields from the assembly could not be loaded.
        /// </returns>
        public bool AddFieldsFromAssembly(string name) {
            return AddFieldsFromAssembly(name, false, null);
        }


        /// <summary>
        /// Add the field from the given assembly as field filters.
        /// </summary>
        /// <param name="name">The name of the assembly or the exact path to it.</param>
        /// <param name="addReferences">Add the fields from the assemblies referenced by the given one.</param>
        /// <returns>
        /// TRUE if the fields from the assembly could be loaded.
        /// FALSE if the fields from the assembly could not be loaded.
        /// </returns>
        public bool AddFieldsFromAssembly(string name, bool addReferences) {
            return AddFieldsFromAssembly(name, addReferences, null);
        }

        #endregion

        #region Parsing

        /// <summary>
        /// Initialize the parsing engine.
        /// </summary>
        /// <param name="startIndex">The index in the text from where to start parsing.</param>
        /// <returns>
        /// TRUE if the parsing engine could be initialized.
        /// FALSE if the parsing engine could not be initialized.
        /// </returns>
        public bool InitParsing(int startIndex) {
            if(text == null || text.Length == 0) {
                return false;
            }

            // generate list of used operators characters
            for(int i = 0; i < operators.Count; i++) {
                for(int j = 0; j < operators[i].Length; j++) {
                    int index = Convert.ToInt32(operators[i][j]);

                    if(index > 0 && index < 256) {
                        // mark as used
                        keywordInvalid[index] = true;
                    }
                }
            }

            // generate list of used string separators characters
            for(int i = 0; i < stringSeparators.Count; i++) {
                // add start
                for(int j = 0; j < stringSeparators[i].start.Length; j++) {
                    int index = Convert.ToInt32(stringSeparators[i].start[j]);

                    if(index > 0 && index < 256) {
                        keywordInvalid[index] = true;
                    }
                }

                // add end
                if(stringSeparators[i].hasEnd) {
                    for(int j = 0; j < stringSeparators[i].start.Length; j++) {
                        int index = Convert.ToInt32(stringSeparators[i].start[j]);

                        if(index > 0 && index < 256) {
                            keywordInvalid[index] = true;
                        }
                    }
                }
            }

            // generate list of used operators characters
            for(int i = 0; i < commentSeparators.Count; i++) {
                // add start
                for(int j = 0; j < commentSeparators[i].start.Length; j++) {
                    int index = Convert.ToInt32(commentSeparators[i].start[j]);

                    if(index > 0 && index < 256) {
                        keywordInvalid[index] = true;
                    }
                }

                // add end
                if(commentSeparators[i].hasEnd) {
                    for(int j = 0; j < commentSeparators[i].start.Length; j++) {
                        int index = Convert.ToInt32(commentSeparators[i].start[j]);

                        if(index > 0 && index < 256) {
                            keywordInvalid[index] = true;
                        }
                    }
                }
            }

            // generate the priority lists
            keywordsPriority.Clear();

            for(int i = 0; i < keywords.Count; i++) {
                keywordsPriority.Add(i);
            }

            // generate the priority lists
            fieldsPriority.Clear();

            for(int i = 0; i < fields.Count; i++) {
                fieldsPriority.Add(i);
            }

            // ---
            operatorsPriority.Clear();

            for(int i = 0; i < operators.Count; i++) {
                operatorsPriority.Add(i);
            }

            // other things
            textIndex = startIndex;
            lastIndex = textIndex;
            inComment = false;
            inString = false;
            inKeywordInvalid = false;

            // initialization completed
            return true;
        }


        /// <summary>
        /// Initialize the parsing engine.
        /// </summary>
        /// <returns>
        /// TRUE if the parsing engine could be initialized.
        /// FALSE if the parsing engine could not be initialized.
        /// </returns>
        public bool InitParsing() {
            return InitParsing(0);
        }


        /// <summary>
        /// Get the next block of text.
        /// </summary>
        /// <returns>
        /// A TextBlock object if an text block was found.
        /// NULL if a text block was not found.
        /// </returns>
        public TextBlock? GetNextBlock() {
            newWord = false;

            if(textIndex >= text.Length) {
                return null;
            }

            for(int i = textIndex; i < text.Length; i++) {
                if(IsPartOfKeywordInvalid(text[i]) && (inString == false && inComment == false)) {
                    if(inKeywordInvalid == false) {
                        inKeywordInvalid = true;
                        keywordInvalidStartIndex = i;
                        newWord = true;
                    }
                    else {
                        newWord = false;
                    }
                }
                else if(inString || inComment) {
                    if((inString == true && usedSeparator.hasEnd && StringContainsChar(usedSeparator.end, text[i])) ||
                        (inComment == true && usedSeparator.hasEnd && StringContainsChar(usedSeparator.end, text[i]))) {
                        if(inKeywordInvalid == false) {
                            inKeywordInvalid = true;
                            keywordInvalidStartIndex = i;
                            newWord = true;
                        }
                        else {
                            newWord = false;
                        }
                    }
                    else if(IsSeparator(text[i])) {
                        inKeywordInvalid = false;
                        wasInKeywordInvalid = true;
                        newWord = true;
                        newWordBySeparator = true;
                    }
                }
                else {
                    if(IsSeparator(text[i])) {
                        if(text[i] == '\n' || (inString == false && inComment == false)) {
                            newWord = true;
                            newWordBySeparator = true;

                            if(inKeywordInvalid) {
                                inKeywordInvalid = false;
                                wasInKeywordInvalid = true;
                            }
                        }
                        else if(inKeywordInvalid) {
                            inKeywordInvalid = false;
                            wasInKeywordInvalid = true;
                            newWord = true;
                            newWordBySeparator = true;
                        }
                    }
                    else if(inKeywordInvalid) {
                        inKeywordInvalid = false;
                        wasInKeywordInvalid = true;
                        newWord = true;
                    }
                }

                // handle the new word
                if(newWord || i == text.Length - 1) {
                    string word;
                    int startIndex;
                    TextSeparator? separator;

                    if(wasInKeywordInvalid) {
                        word = text.Substring(keywordInvalidStartIndex, i - keywordInvalidStartIndex + (i == text.Length - 1 ? 1 : 0)).Trim();
                        startIndex = keywordInvalidStartIndex;
                        wasInKeywordInvalid = false;
                    }
                    else {
                        word = text.Substring(lastIndex, i - lastIndex + (i == text.Length - 1 ? 1 : 0)).Trim();
                        startIndex = lastIndex;
                    }


                    if(i > startIndex || i == text.Length - 1) {
                        bool returnValid = false;
                        TextBlockType type = TextBlockType.Keyword;

                        // special cases first
                        if(inString) {
                            if(usedSeparator.hasEnd) {
                                if(usedSeparator.end.Equals(word)) {
                                    if(usedSeparator.hasNotPrior == false || (usedSeparator.hasNotPrior && i > 0 && text[i - 1] != usedSeparator.notPrior)) {
                                        inString = false;
                                        word = text.Substring(stringStartIndex, i - stringStartIndex + (i == text.Length - 1 ? 1 : 0));
                                        type = TextBlockType.String;
                                        startIndex = stringStartIndex;
                                        returnValid = true;
                                    }
                                }
                            }
                            else if(text[i] == '\n') {

                            }
                        }
                        else {
                            if((separator = IsStringSeparator(word)).HasValue) {
                                inString = true;
                                stringStartIndex = keywordInvalidStartIndex;
                                usedSeparator = separator.Value;
                                returnValid = false;
                            }
                        }


                        if(inComment) {
                            if(usedSeparator.hasEnd) {
                                if(usedSeparator.end.Equals(word)) {
                                    if(usedSeparator.hasNotPrior == false || (usedSeparator.hasNotPrior && i > usedSeparator.end.Length && text[i - usedSeparator.end.Length - 1] != usedSeparator.notPrior)) {
                                        inComment = false;
                                        word = text.Substring(commentStartIndex, i - commentStartIndex + (i == text.Length - 1 ? 1 : 0));
                                        type = TextBlockType.Comment;
                                        startIndex = commentStartIndex;
                                        returnValid = true;
                                    }
                                }
                            }
                            else if(text[i] == '\n') {
                                inComment = false;
                                word = text.Substring(commentStartIndex, i - commentStartIndex + (i == text.Length - 1 ? 1 : 0));
                                type = TextBlockType.Comment;
                                startIndex = commentStartIndex;
                                returnValid = true;
                            }
                        }
                        else {
                            if((separator = IsCommentSeparator(word)).HasValue) {
                                inComment = true;
                                commentStartIndex = keywordInvalidStartIndex;
                                usedSeparator = separator.Value;
                                returnValid = false;
                            }
                        }

                        if(IsOperator(word)) {
                            type = TextBlockType.Operator;
                            returnValid = inString == false && inComment == false;
                        }
                        else if(IsKeyword(word)) {
                            type = TextBlockType.Keyword;
                            returnValid = returnValid = inString == false && inComment == false;
                        }
                        else if(IsField(word)) {
                            type = TextBlockType.Field;
                            returnValid = returnValid = inString == false && inComment == false;
                        }
                        else if(IsNumber(word)) {
                            type = TextBlockType.Number;
                            returnValid = returnValid = inString == false && inComment == false;
                        }
                        else {
                            type = TextBlockType.Unknown;
                            returnValid = true;
                        }

                        // return block
                        if(returnValid) {
                            TextBlock block = new TextBlock();

                            block.StartIndex = startIndex;
                            block.EndIndex = i;
                            block.Text = word;
                            block.Type = type;

                            lastIndex = i;
                            if(newWordBySeparator == true) {
                                lastIndex++;
                            }

                            textIndex = lastIndex;
                            newWord = false;
                            newWordBySeparator = false;

                            return block;
                        }
                    }

                    lastIndex = i;
                    if(newWordBySeparator == true) {
                        lastIndex++;
                    }

                    textIndex = lastIndex;
                    newWord = false;
                    newWordBySeparator = false;
                }
            }

            return null;
        }


        /// <summary>
        /// Resets the parser.
        /// </summary>
        /// <param name="startIndex">The index in the text from where to start parsing.</param>
        public void ResetParser(int startIndex) {
            textIndex = startIndex;
            lastIndex = textIndex;
        }


        /// <summary>
        /// Resets the parser.
        /// </summary>
        public void ResetParser() {
            ResetParser(0);
        }


        /// <summary>
        /// Gets the number of lines the text has.
        /// </summary>
        /// <returns>
        /// 0 if the text was not set or its length is 0.
        /// </returns>
        public int GetLineNumber() {
            if(text == null || text.Length == 0) {
                return 0;
            }

            int ct = 1;
            bool wasNewLine = false;

            for(int i = 0; i < text.Length; i++) {
                if(wasNewLine) {
                    ct++;
                    wasNewLine = false;
                }

                if(text[i] == '\n') {
                    wasNewLine = true;
                }
            }

            return ct;
        }

        #endregion

        #endregion
    }

    #endregion
}
