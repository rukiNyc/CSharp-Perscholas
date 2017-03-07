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

using Base;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Elements
{
	public class Discovery : IPrintable, IAsDouble
	{
		static readonly Regex _rxYr = new Regex(@"\((\d{1,4})\)"), _rxBc = new Regex(@"(\d{1,4}) BC");
		public static Discovery Parse(string v)
		{
			
			// "Egyptians and Sumerians (3750 BC)"
			if (String.IsNullOrEmpty(v)) return null;
			int yr = int.MinValue;
			string disco = null;
			Match m = _rxBc.Match(v);
			if (m.Success)
			{
				yr = -int.Parse(m.Groups[1].Value);
				disco = v.Substring(0, m.Groups[0].Index - 1).Trim();
			}
			else
			{
				m = _rxYr.Match(v);
				if (m.Success)
				{
					yr = int.Parse(m.Groups[1].Value);
					disco = v.Substring(0, m.Groups[0].Index).Trim();
				}
			}
			return new Discovery(disco, yr);
		}

		public Discovery(string name, int year)
		{
			Name = name;
			Year = year;
		}

		public string Name { get; private set; }
		public int Year { get; private set; }

		public string Print(PrintOptions ops)
		{
			return ToString();
		}

		public override string ToString()
		{
			string yr = (Year > 0) ? Year.ToString() : string.Format("{0} BC", -Year);
			return string.IsNullOrEmpty(Name) ? String.Format("({0})", yr) : string.Format("{0} ({1})", Name, yr);
		}

		public double AsDouble()
		{
			return Year;
		}
	}
}
