#region Copyright © 2008 Rickard Nilsson [rickard@rickardnilsson.net]
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

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeFormatter 
{
    /// <summary>
    /// A pattern matcher 
    /// </summary>
    public class Parser {
        private IList<ParsingStrategy> Strategies = new List<ParsingStrategy>();

        public void DefineAs(params ParsingStrategy[] strategies) {
            Strategies = new List<ParsingStrategy>(strategies);
        }

        public string Evaluate(string text) {
            return Regex.Replace(text, EvaluateElements);
        }

        private Regex Regex {
            get {
                StringBuilder sb = new StringBuilder();

                foreach (ParsingStrategy strategy in Strategies) {
                    if (sb.Length > 0) 
                        sb.Append("|");

                    sb.AppendFormat("(?<{0}>{1})", strategy.GroupName, strategy.Regex);
                }
                return new Regex(sb.ToString(), RegexOptions.Compiled | 
                                                RegexOptions.Singleline);
            }
        }

        private string EvaluateElements(Match match) {
            foreach (ParsingStrategy strategy in Strategies) {
                if (strategy.IsMatch(match)) {
                    return strategy.Evaluate(match);
                }
            }
            return match.Value;
        }
    }
}