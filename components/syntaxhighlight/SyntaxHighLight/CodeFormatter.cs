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

using System;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Converts text to formatted syntax highlighted code.
/// </summary>
/// <remarks>
/// It is a work in progress.....
/// </remarks>

public class CodeFormatterExtension
{
    #region Constructors

    /// <summary>
    /// Maps custom events to the ServingContent event
    /// </summary>
    //public CodeFormatterExtension()
    //{
    //    Page.Serving += ServingContent;
    //    Post.Serving += ServingContent;
    //} 

    #endregion

    #region RegEx

    private Regex codeRegex = new Regex(@"\[code:(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;alt=(?<altlinenumbers>(?:on|off)))?(?:;(?<title>.*?))?\](?<code>.*?)\[/code\]",
        RegexOptions.Compiled
        | RegexOptions.CultureInvariant
        | RegexOptions.IgnoreCase
        | RegexOptions.Singleline);

    private Regex codeBeginTagRegex = new Regex(@"((\\r|\\n|<p>|</p>|<br />|\s)*\[code:.*?\](\\r|\\n|<p>|</p>|<br />|\s)*)",
        RegexOptions.Compiled
        | RegexOptions.CultureInvariant
        | RegexOptions.IgnoreCase
        | RegexOptions.Singleline);
    
    
    private Regex codeEndTagRegex = new Regex(@"((\\r|\\n|<p>|</p>|<br />|\s)*\[/code\])",
        RegexOptions.Compiled
        | RegexOptions.CultureInvariant
        | RegexOptions.IgnoreCase
        | RegexOptions.Singleline); 

    #endregion
    
    /// <summary>
    /// An event that handles ServingEventArgs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //void ServingContent(object sender, ServingEventArgs e)
    //{
    //    if (e.Body.Contains("[/code]"))
    //    {
    //        e.Body = codeRegex.Replace(e.Body, new MatchEvaluator(CodeEvaluator));
    //        e.Body = codeBeginTagRegex.Replace(e.Body, @"<p>");
    //        e.Body = codeEndTagRegex.Replace(e.Body, @"</p>");
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private string CodeEvaluator(Match match)
    {
        if (!match.Success)
            return match.Value;

        HighlightOptions options = new HighlightOptions();

        options.Language = match.Groups["lang"].Value;
        options.Code = match.Groups["code"].Value;
        options.DisplayLineNumbers = match.Groups["linenumbers"].Value == "on" ? true : false;
        options.Title = match.Groups["title"].Value;
        options.AlternateLineNumbers = match.Groups["altlinenumbers"].Value == "on" ? true : false;

        return Highlight(options, match.Value);
    }

    /// <summary>
    /// Returns the formatted text.
    /// </summary>
    /// <param name="options">Whatever options were set in the regex groups.</param>
    /// <param name="text">Send the e.body so it can get formatted.</param>
    /// <returns>The formatted string of the match.</returns>
    public string Highlight(HighlightOptions options, string text)
    {

        switch (options.Language)
        {
            case "c#":
                CodeFormatter.CSharpFormat csf = new CodeFormatter.CSharpFormat();
                csf.LineNumbers = options.DisplayLineNumbers;
                csf.Alternate = options.AlternateLineNumbers;                
                return csf.FormatCode(text);

            case "vb":
                CodeFormatter.VisualBasicFormat vbf = new CodeFormatter.VisualBasicFormat();
                vbf.LineNumbers = options.DisplayLineNumbers;
                vbf.Alternate = options.AlternateLineNumbers;
                //e.Body = codeRegex.Replace(text, new MatchEvaluator(CodeEvaluator));
                //e.Body = codeBeginTagRegex.Replace(codeRegex.Replace(text, new MatchEvaluator(CodeEvaluator)), @"<p>");
                //e.Body = codeEndTagRegex.Replace(codeBeginTagRegex.Replace(codeRegex.Replace(text, new MatchEvaluator(CodeEvaluator)), @"<p>"), @"</p>");
                return vbf.FormatCode(text);

            case "js":
                CodeFormatter.JavaScriptFormat jsf = new CodeFormatter.JavaScriptFormat();
                jsf.LineNumbers = options.DisplayLineNumbers;
                jsf.Alternate = options.AlternateLineNumbers;
                return jsf.FormatCode(text);

            case "html":
                CodeFormatter.HtmlFormat htmlf = new CodeFormatter.HtmlFormat();
                htmlf.LineNumbers = options.DisplayLineNumbers;
                htmlf.Alternate = options.AlternateLineNumbers;
                text = StripHtml(text).Trim();
                string code = htmlf.FormatCode(text).Trim();
                return code.Replace(Environment.NewLine, "<br />");

            case "xml":
                CodeFormatter.HtmlFormat xmlf = new CodeFormatter.HtmlFormat();
                xmlf.LineNumbers = options.DisplayLineNumbers;
                xmlf.Alternate = options.AlternateLineNumbers;
                text = StripHtml(text).Trim();
                string xml = xmlf.FormatCode(text).Trim();
                return xml.Replace(Environment.NewLine, "<br />");

            case "tsql":
                CodeFormatter.TsqlFormat tsqlf = new CodeFormatter.TsqlFormat();
                tsqlf.LineNumbers = options.DisplayLineNumbers;
                tsqlf.Alternate = options.AlternateLineNumbers;
                return tsqlf.FormatCode(text);

            case "msh":
                CodeFormatter.MshFormat mshf = new CodeFormatter.MshFormat();
                mshf.LineNumbers = options.DisplayLineNumbers;
                mshf.Alternate = options.AlternateLineNumbers;
                return mshf.FormatCode(text);
        }

        return string.Empty;
    }

    private static Regex _Regex = new Regex("<[^>]*>", RegexOptions.Compiled);

    private static string StripHtml(string html)
    {
      if (string.IsNullOrEmpty(html))
        return string.Empty;

      return _Regex.Replace(html, string.Empty);
    }

    /// <summary>
    /// Handles all of the options for changing the rendered code.
    /// </summary>
    public class HighlightOptions
    {
        private string language, title, code;
        private bool displayLineNumbers = false;
        private bool alternateLineNumbers = false;

        public HighlightOptions()
        {
        }

        public HighlightOptions(string language, string title, bool linenumbers, string code, bool alternateLineNumbers)
        {
            this.language = language;
            this.title = title;
            this.alternateLineNumbers = alternateLineNumbers;
            this.code = code;
            this.displayLineNumbers = linenumbers;
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public bool DisplayLineNumbers
        {
            get { return displayLineNumbers; }
            set { displayLineNumbers = value; }
        }
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool AlternateLineNumbers
        {
            get { return alternateLineNumbers; }
            set { alternateLineNumbers = value; }
        }
    }
}
