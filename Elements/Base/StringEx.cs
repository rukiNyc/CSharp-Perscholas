/*	Copyright (c) 2017  Kenneth Brady
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and asssociated documentation files (the "Software"), to deal
 *	in the Sortware without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *	
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *	
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base
{
	public static class StringEx
	{
		static readonly Regex _referenceRx = new Regex(@"\[\d+]");
		private const string _digits = "0123456789";

		static StringEx()
		{
			Digits = _digits.ToCharArray();
		}

		/// <summary>
		/// Remove references with the pattern \[\d+]
		/// </summary>
		/// <param name="s"></param>
		/// <returns>a string with pattern removed</returns>
		public static string RemoveReferences(string s)
		{
			List<Match> matches = new List<Match>(_referenceRx.Matches(s).Cast<Match>());
			List<Tuple<int, int>> ndxs = new List<Tuple<int, int>>(matches.Count);
			foreach(Match m in matches)
			{
				ndxs.Add(new Tuple<int, int>(m.Index, m.Length));
			}
			ndxs.Reverse();
			StringBuilder s2 = new StringBuilder(s);
			foreach(Tuple<int,int> t in ndxs)
			{
				s2 = s2.Remove(t.Item1, t.Item2);
			}
			return s2.ToString();
		}

		public static char[] Digits { get; private set; }
	}
}
