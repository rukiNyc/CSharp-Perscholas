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
