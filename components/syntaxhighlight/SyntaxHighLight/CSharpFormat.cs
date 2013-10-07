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

using System.Text;
using System.Text.RegularExpressions;

namespace CodeFormatter
{
    /// <summary>
	/// Generates color-coded HTML 4.01 from C# source code.
	/// </summary>
	public class CSharpFormat : CLikeFormat
	{
	    public CSharpFormat() {
            Parser
                .DefineAs(
                    Comments,
                    PreprocessorDirectives,
                    StringLiterals,
                    Keywords_,
                    TypeDeclarations,
                    Declarations,
                    ConstructorInvocations,
                    Attributes,
                    GenericTypes,
                    StaticInvocations);
	    }

        /// <summary>
		/// The list of C# keywords.
		/// </summary>
		protected override string Keywords 
		{
			get 
			{ 
				return "abstract as base bool break byte case catch char "
				+ "checked class const continue decimal default delegate do double else "
                + "enum event explicit extern false finally fixed float for foreach get goto "
				+ "if implicit in int interface internal is lock long namespace new null "
				+ "object operator out override partial params private protected public readonly "
                + "ref return sbyte sealed set short sizeof stackalloc static string struct "
				+ "switch this throw true try typeof uint ulong unchecked unsafe ushort "
				+ "using value var virtual void volatile where while yield";
			}
		}

	    /// <summary>
		/// The list of C# preprocessors.
		/// </summary>
		protected override string Preprocessors
		{
			get 
			{ 
				return "#if #else #elif #endif #define #undef #warning "
					+ "#error #line #region #endregion #pragma";
			}
		}

        #region Parsing strategies
        /// <summary>
        /// Formatting the declaration of types
        /// </summary>
        protected virtual ParsingStrategy TypeDeclarations {
            get {
                return new ParsingStrategy(
                    "typeexpression", 
                    @"(?<=class +|interface +|enum +|struct +)(?<type>[A-Z]\w*)( *: *(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*)( *, *(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*))*)?",
                    delegate(Match match) {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("<span class=\"type\">{0}</span>", match.Groups["type"].Captures[0].Value);

                        if (match.Groups["type"].Captures.Count > 1) {
                            sb.Append(" : ");
                            for (int i = 1; i < match.Groups["type"].Captures.Count; i++) {
                                if (i > 1) {
                                    sb.Append(" , ");
                                }
                                Capture capture = match.Groups["type"].Captures[i];
                                sb.AppendFormat("{0}<span class=\"type\">{1}</span>", 
                                    match.Groups["namespace"].Captures[i-1].Value,
                                    capture.Value);
                            }
                        }
                        return sb.ToString();
                    });
            }
        }

        /// <summary>
        /// Formatting the declaration of type variables
        /// </summary>
	    protected virtual ParsingStrategy Declarations {
	        get {
	            return new ParsingStrategy(
                    "declaration",
                    @"(?<=public +|private +|protected +|internal +|static +|^|\W)(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*)(?= +\w|&lt;)",
                    delegate (Match match) {
                        return string.Format("{0}<span class=\"type\">{1}</span>", 
                            match.Groups["namespace"].Value,
                            match.Groups["type"].Value);
                    });
	        }
	    }

        /// <summary>
        /// Formatting of object initiation
        /// </summary>
	    protected virtual ParsingStrategy ConstructorInvocations {
	        get {
                return new ParsingStrategy(
                    "constrinv",
                    @"(?<=new( |&nbsp;)+)(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*)(?=((<|&lt;).*(>|&gt;))?\()",
                    delegate (Match match) {
                        return string.Format("{0}<span class=\"type\">{1}</span>", 
                            match.Groups["namespace"].Value,
                            match.Groups["type"].Value);
                    });
	        }
	    }

        /// <summary>
        /// Formatting invocation of static methods and properties
        /// </summary>
	    protected virtual ParsingStrategy StaticInvocations {
	        get {
	            return new ParsingStrategy(
                    "staticinv",
                    @"(?<=(?<nskwrd>namespace +)|^|\W)(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*)(?=\.)",
                    delegate (Match match) {
                        if (match.Groups["nskwrd"].Success)
                            return match.Value;

                        return string.Format("{0}<span class=\"type\">{1}</span>", 
                            match.Groups["namespace"].Value,
                            match.Groups["type"].Value);
                    });
	        }
	    }

        /// <summary>
        /// Formatting of type attributes
        /// </summary>
	    protected virtual ParsingStrategy Attributes {
	        get {
	            return new ParsingStrategy(
                    "attribute",
                    @"(?<=\[[^A-Z\]]*)(?<type>[A-Z]\w*)(?<params>(\([^\)]+\))?)((?<infix>[^,\]]*,[^A-Z\]]*)(?<type>[A-Z]\w*)(?<params>(\([^\)]+\))?))*(?=[^\]]*\])",
                    delegate (Match match) {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < match.Groups["type"].Captures.Count; i++) {
                            Capture capture = match.Groups["type"].Captures[i];
                            if (i > 0)
                                sb.Append(match.Groups["infix"].Value);

                            sb.AppendFormat("<span class=\"type\">{0}</span>", capture.Value);
                            string param = match.Groups["params"].Captures[i].Value;
                            Regex regex = new Regex(StringRegEx, RegexOptions.Compiled | RegexOptions.Singleline);
                            param = regex.Replace(param, StringLiterals.Evaluate);
                            sb.Append(param);
                        }

                        return sb.ToString();
                    });
	        }
	    }

        /// <summary>
        /// Formatting type parameters of generics
        /// </summary>
	    protected virtual ParsingStrategy GenericTypes {
	        get {
	            return new ParsingStrategy(
                    "generictype", 
                    @"(?<=&lt;(</?(br /|p)>| |&nbsp;|\\r|\\n|\\t|\w+,)*)(?<namespace>([A-Z]\w*\.)*)(?<type>[A-Z]\w*)(?=.*&gt;)",
	                delegate (Match match) {
	                    return string.Format("{0}<span class=\"type\">{1}</span>", 
                            match.Groups["namespace"].Value,
                            match.Groups["type"].Value);
	                });
	        }
        }
        #endregion
    }
}

