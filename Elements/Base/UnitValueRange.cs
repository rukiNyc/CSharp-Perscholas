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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	public class UnitValueRange : UnitValue
	{
		public static new UnitValueRange Parse(string v, string u)
		{
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			string[] parts = v.Split('-');
			double d1 = Double.NaN, d2 = 0;
			switch (parts.Length)
			{
				case 0:
					d1 = d2 = double.Parse(v);
					break;
				case 1:
					d1 = d2 = double.Parse(parts[0].Trim());
					break;
				case 2:
					d1 = double.Parse(parts[0].Trim());
					d2 = double.Parse(parts[1].Trim());
					break;
			}
			if (!double.IsNaN(d1)) return new UnitValueRange(d1, d2, u, vq);
			throw new Exception(String.Format("UnitValueRange.Parse failed: {0}", v));
		}

		public UnitValueRange(double d, double d2, string unit, ValueQualifier vq = ValueQualifier.None):
			base(d, unit, vq)
		{
			Value2 = d2;
		}

		public double Value2 { get; private set; }

		public override string Print(PrintOptions ops)
		{
			StringBuilder s = new StringBuilder();
			if (Value == Value2) s.AppendFormat(Value.ToString()); else
				s.AppendFormat("{0} - {1}", Value, Value2);
			if (ops.ShowUnits()) s.Append(" ").Append(Units);
			if (Qualifier != ValueQualifier.None &&  ops.ShowQualifier()) s.Append(" ").Append(Qualifier);
			return s.ToString();
		}

		public override double AsDouble()
		{
			return (Value + Value2) / 2;
		}
	}
}
