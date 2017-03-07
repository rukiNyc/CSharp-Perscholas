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
	public class QualifiedString : IPrintable
	{
		public static QualifiedString Parse(string v)
		{
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			return new QualifiedString(v.Trim(), vq);
		}

		public QualifiedString(string v, ValueQualifier vq = ValueQualifier.None)
		{
			Value = v;
			Qualifier = vq;
		}

		public String Value { get; private set; }
		public ValueQualifier Qualifier { get; private set; }

		public override string ToString()
		{
			switch (Qualifier)
			{
				case ValueQualifier.None: return Value;
				default:	return String.Format("{0} ({1})", Value, Qualifier);
			}
		}

		public virtual string Print(PrintOptions ops)
		{
			return (Qualifier != ValueQualifier.None) && ops.ShowQualifier() ? String.Concat(Value, ' ', Qualifier) : Value;
		}
	}
}
