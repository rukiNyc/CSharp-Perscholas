using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	public enum ValueQualifier
	{
		None,
		Estimated,
		Predicted,
		Empirical,
		Calculated,
		Extrapolated,
		Circa,
		MostStableAllotrope
	}

	public class DoubleValue : IPrintable, IAsDouble
	{
		static DoubleValue()
		{
			List<string> qf = new List<string>(
				Enum.GetNames(typeof(ValueQualifier)).Select<string, string>((vq) => vq.ToLower()));
			qf.RemoveAt(0);
			_qualifiers = qf.ToArray();
		}

		private readonly static string[] _qualifiers; // = { "estimated", "predicted", "empirical", "calculated", "extrapolated" };
		private const string CA = "ca.";
		protected const char TAB = '\t';
		protected const char SPACE = ' ';

		public static ValueQualifier FindQualifier(ref string s)
		{
			ValueQualifier r = ValueQualifier.None;
			if (String.IsNullOrEmpty(s)) return r;
			string sret = s.ToLower();
			if (s.StartsWith(CA))
			{
				s = s.Substring(CA.Length).TrimStart();
				return ValueQualifier.Circa;
			}
			Func<int, Func<string, string>> findQ3 = (nn) =>
			 {
				 string vq = _qualifiers[nn];
				 int ndx = sret.IndexOf(vq);
				 Func<string, string> ret = null;
				 if (ndx > 0)
				 {
					 if (ndx > 0 && sret[ndx - 1] == '(')
					 {
						 ret = (ss) => ss.Remove(ndx - 1, vq.Length + 2);
					 }
					 else
					 {
						 ret = (ss) => ss.Remove(ndx, vq.Length);
					 }
					 r = (ValueQualifier)(nn + 1);
				 }
				 return ret;
			 };
			Func<string, string> trimmer = null;
			for (int i=0;i<_qualifiers.Length;++i)
			{
				trimmer = findQ3(i);
				if (trimmer != null)
				{
					s = trimmer(s).Trim();
					break;
				}
			}
			return r;
		}

		public static bool TryParseScientific(string s, out double d)
		{
			const char DOT = (char)183;
			d = double.NaN;
			if (String.IsNullOrEmpty(s)) return false;
			int n = 0;
			while (Char.IsLetter(s[n]) || Char.IsPunctuation(s[n]) || Char.IsWhiteSpace(s[n])) n++;
			s = s.Substring(n);
			// +16.5·10−6 cm3/mol
			bool positive = true;
			if (s[0] == '+') s = s.Substring(1); else
			if (s[0]=='-' || s[0] == '\u2212')
			{
				positive = false;
				s = s.Substring(1);
			}
			n = s.IndexOf(DOT);
			if (n > 0)
			{
				try
				{
					string sv = s.Substring(0, n), se = s.Substring(n + 1);
					double v;
					if (Double.TryParse(sv, out v))
					{
						if (se.Length > 3 && se[0] == '1' && se[1] == '0')
						{
							bool? epos = null;
							switch (se[2])
							{
								case '+':	epos = true; break;
								case '-':
								case '\u2212': epos = false; break;
							}
							n = 3;
							while (Char.IsDigit(se[n])) n++;
							se = se.Substring(3, n - 3);
							int e;
							if (int.TryParse(se, out e))
							{
								double s1 = positive ? 1 : -1, s2 = epos.GetValueOrDefault(true) ? 1 : -1;
								d = s1 * v * Math.Pow(10, s2 * e);
								return true;
							}
						}
					}
				}
				catch(Exception ex)
				{
					ConsoleWriter.WriteError(ex.Message);
				}

			}
			return false;
		}

		public static bool TryParseRange(string s, out double d1, out double d2)
		{
			d1 = Double.NaN; d2 = Double.NaN;
			if (String.IsNullOrEmpty(s)) return false;
			int n = 0;
			while (!Char.IsDigit(s[n]) && !(s[n] == '.')) n++;
			s = s.Substring(n);
			string[] parts = s.Split('–');
			if (parts.Length == 2)
			{
				string s2 = parts[1];
				n = s2.Length - 1;
				while (!Char.IsDigit(s2[n])) n--;
				s2 = s2.Substring(0, n + 1);
				double da, db;
				if (double.TryParse(parts[0], out da) && double.TryParse(s2, out db))
				{
					d1 = da;
					d2 = db;
					return true;
				}
			}
			return false;
		}

		public DoubleValue(double value, ValueQualifier vq = ValueQualifier.None)
		{
			Value = value;
			Qualifier = vq;
		}

		protected DoubleValue() { }

		public Double Value { get; protected set; }

		public ValueQualifier Qualifier { get; protected set; }

		public override string ToString()
		{
			switch (Qualifier)
			{
				case ValueQualifier.None: return Value.ToString();
				default: return String.Format("{0}\t({1})", Value, Qualifier);
			}
		}

		public virtual string Print(PrintOptions ops)
		{
			StringBuilder s = new StringBuilder();
			s.Append(Value);
			if (ops.ShowQualifier())
			{
				s.Append(TAB).Append(Qualifier);
			}
			return s.ToString();
		}

		public virtual double AsDouble() { return Value; }
	}
}
