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
