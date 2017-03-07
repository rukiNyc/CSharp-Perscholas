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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
	public enum Phase
	{
		Gas,
		Liquid,
		Solid
	}

	public class QualifiedPhase : IPrintable
	{
		public static QualifiedPhase Parse(string value)
		{
			return new QualifiedPhase(value);
		}

		public QualifiedPhase(string p)
		{
			Qualifier = DoubleValue.FindQualifier(ref p);
			switch (p.Trim().ToLower())
			{
				case "solid": Phase = Phase.Solid; break;
				case "liquid": Phase = Phase.Liquid; break;
				case "gas": Phase = Phase.Gas; break;
			}
		}
		public QualifiedPhase(Phase p, ValueQualifier vq = ValueQualifier.None)
		{
			Phase = p;
			Qualifier = vq;
		}

		public ValueQualifier Qualifier { get; private set; }
		public Phase Phase { get; private set; }

		public override string ToString()
		{
			string r = Phase.ToString();
			if (Qualifier != ValueQualifier.None) r = String.Format("{0} ({1})", r, Qualifier);
			return r;
		}

		public string Print(PrintOptions ops)
		{
			string r = Phase.ToString();
			if (Qualifier != ValueQualifier.None && ops.ShowQualifier()) r = String.Format("{0} ({1})", r, Qualifier);
			return r;
		}
	}
}
