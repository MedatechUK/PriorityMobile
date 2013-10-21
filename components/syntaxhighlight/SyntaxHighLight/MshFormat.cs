#region Copyright � 2001-2003 Jean-Claude Manoli [jc@manoli.net]
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

namespace CodeFormatter
{
	/// <summary>
	/// Generates color-coded HTML 4.01 from MSH (code name Monad) source code.
	/// </summary>
	public class MshFormat : CodeFormat
	{
		/// <summary>
		/// Regular expression string to match single line comments (#).
		/// </summary>
		protected override string CommentRegEx
		{
			get { return @"#.*?(?=\r|\n)"; }
		}

		/// <summary>
		/// Regular expression string to match string and character literals. 
		/// </summary>
		protected override string StringRegEx
		{
			get { return @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'"; }
		}

		/// <summary>
		/// The list of MSH keywords.
		/// </summary>
		protected override string Keywords 
		{
			get 
			{ 
				return "function filter global script local private if else"
					+ " elseif for foreach in while switch continue break"
					+ " return default param begin process end throw trap";
			}
		}

		/// <summary>
		/// Use preprocessors property to hilight operators.
		/// </summary>
		protected override string Preprocessors
		{
			get
			{
				return "-band -bor -match -notmatch -like -notlike -eq -ne"
					+ " -gt -ge -lt -le -is -imatch -inotmatch -ilike"
					+ " -inotlike -ieq -ine -igt -ige -ilt -ile";
			}
		}

	}
}
