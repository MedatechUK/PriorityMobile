#region Copyright © 2008 Rickard Nilsson [rickard@rickardnilsson.net]
/*
 * This software is an altered version of the original and is provied 'as-is'.
 */
#endregion
#region Copyright © 2001-2003 Jean-Claude Manoli [jc@manoli.net]
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author(s) be held liable for any damages arising from
 * the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 *   1. The origin of this software must not be misrepresented; you must not
 *      claim that you wrote the original software. If you use this software
 *      in a product, an acknowledgment in the product documentation would be
 *      appreciated but is not required.
 * 
 *   2. Altered source versions must be plainly marked as such, and must not
 *      be misrepresented as being the original software.
 * 
 *   3. This notice may not be removed or altered from any source distribution.
 */ 
#endregion

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeFormatter
{
	/// <summary>
	/// Provides a base class for formatting languages similar to C.
	/// </summary>
	public abstract class CLikeFormat : CodeFormat
	{
        protected CLikeFormat() {
            Parser
                .DefineAs(
                    Comments, 
                    PreprocessorDirectives, 
                    StringLiterals, 
                    Keywords_);
        }
		/// <summary>
		/// Regular expression string to match single line and multi-line 
		/// comments (// and /* */). 
		/// </summary>
		protected override string CommentRegEx
		{
			get { return @"/\*.*?\*/|//.*?(?=<br />|\\r|\\n|$)"; }
		}

		/// <summary>
		/// Regular expression string to match string and character literals. 
		/// </summary>
		protected override string StringRegEx
		{
            get { return @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'|&#39;&#39;|&#39;.*&#39;|@?&quot;&quot;|@?&quot;.*?(?!\\).&quot;"; }
        }

        #region Parser elements
        protected virtual ParsingStrategy Comments {
	        get {
	            return new ParsingStrategy(
                    "comment", CommentRegEx,
                    delegate(Match match) {
                       StringReader reader = new StringReader(match.ToString());
                       string line;
                       StringBuilder sb = new StringBuilder();
                       while ((line = reader.ReadLine()) != null) {
                           if (sb.Length > 0) {
                               sb.Append("\n");
                           }
                           sb.Append("<span class=\"rem\">");
                           sb.Append(line);
                           sb.Append("</span>");
                       }
                       return sb.ToString();
                    });
	        }
	    }

	    protected virtual ParsingStrategy StringLiterals {
	        get {
	            return new ParsingStrategy(
                    "stringliteral", StringRegEx,
                    delegate(Match match) {
                       return "<span class=\"str\">" + match + "</span>";
                    });
	        }
	    }

	    protected virtual ParsingStrategy PreprocessorDirectives {
	        get {
	            return new ParsingStrategy(
	                "preprocessordirective", 
	                Preprocessors,
	                delegate(Match match) {
	                    return match.Value;
	                });
	        }
	    }

	    protected virtual ParsingStrategy Keywords_ {
	        get {
	            Regex r;
	            r = new Regex(@"\w+|-\w+|#\w+|@@\w+|#(?:\\(?:s|w)(?:\*|\+)?\w+)+|@\\w\*+");
	            string regKeyword = r.Replace(Keywords, @"(?<=^|\W)$0(?=\W)");
	            r = new Regex(@" +");
	            regKeyword = r.Replace(regKeyword, @"|");

	            return new ParsingStrategy(
                    "keyword", regKeyword,
                    delegate(Match match) {
                       return "<span class=\"kwrd\">" + match + "</span>";
                    });
	        }
        }
        #endregion
    }
}

