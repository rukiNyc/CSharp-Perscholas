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

namespace Elements
{
	public class TriplePoint : IPrintable
	{
		public static TriplePoint Parse(string sval, string units)
		{
			// "13.8033 @ 7.041"
			const char ast = '@';
			string[] vals = sval.Split(ast), us = units.Split(ast);
			UnitValue t = new UnitValue(double.Parse(vals[0]), us[0].Trim()),
				p = new UnitValue(double.Parse(vals[1]), us[1].Trim());
			return new TriplePoint(t, p);
		}

		public TriplePoint(UnitValue t, UnitValue p)
		{
			Temperature = t;
			Pressure = p;
		}

		public UnitValue Temperature { get; private set; }

		public UnitValue Pressure { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} @ {1}", Temperature, Pressure);
		}

		public string Print(PrintOptions ops)
		{
			return String.Format("{0} @ {1}", Temperature.Print(ops), Pressure.Print(ops));
		}
	}
}
