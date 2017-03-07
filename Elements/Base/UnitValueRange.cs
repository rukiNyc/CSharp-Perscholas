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
