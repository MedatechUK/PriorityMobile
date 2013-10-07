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

using System.Text.RegularExpressions;

namespace CodeFormatter 
{
    public class ParsingStrategy {
        private readonly string groupName;
        private readonly string regexString;
        private readonly MatchEvaluator Evaluator;

        public ParsingStrategy(string strategyName, string regex, MatchEvaluator evaluator) {
            groupName = strategyName;
            regexString = regex;
            Evaluator = evaluator;
        }

        public string Regex {
            get { return string.Format("(?<{0}>{1})", groupName, regexString); }
        }

        public string GroupName {
            get { return groupName; }
        }

        public bool IsMatch(Match match) {
            return match.Groups[groupName].Success;
        }

        public string Evaluate(Match match) {
            return Evaluator(match);
        }
    }
}