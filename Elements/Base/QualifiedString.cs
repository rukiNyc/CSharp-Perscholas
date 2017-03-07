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
