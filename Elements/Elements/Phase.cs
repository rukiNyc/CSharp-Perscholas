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
