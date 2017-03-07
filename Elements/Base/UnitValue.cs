using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	public class UnitValue : DoubleValue
	{
		public static UnitValue Parse(string v, string unit)
		{
			if (String.IsNullOrEmpty(v)) throw new ArgumentException("v cannot be null or empty.");
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			double d;
			//"2.267 moststableallotrope"
			if (double.TryParse(v, out d) || DoubleValue.TryParseScientific(v, out d)) return new UnitValue(d, unit, vq);
			throw new ArgumentException("Failed to parse UnitValue: " + v);
		}

		public static int IndexOfWhiteSpace(string s)
		{
			if (String.IsNullOrEmpty(s)) return -1;
			for(int i = 0; i < s.Length; ++i)
			{
				if (Char.IsWhiteSpace(s[i])) return i;
			}
			return -1;
		}
		public static bool TryParse(string s, out int i)
		{
			i = 0;
			if (String.IsNullOrEmpty(s)) return false;
			int ii = 0;
			bool dF = true;
			while (ii < s.Length && !IsDigit(s[ii], ref dF)) ii++;
			StringBuilder sb = new StringBuilder();
			while (ii < s.Length && IsDigit(s[ii], ref dF)) sb.Append(s[ii++]);
			int r;
			if (Int32.TryParse(sb.ToString(), out r))
			{
				i = r;
				return true;
			}
			return false;
		}
		public static bool TryParse(string s, out double d)
		{
			d = Double.NaN;
			if (String.IsNullOrEmpty(s)) return false;
			int i = 0;
			bool dF = false;
			while (i < s.Length && !IsDigit(s[i], ref dF)) i++;
			StringBuilder sb = new StringBuilder();
			dF = false;
			while (i < s.Length && IsDigit(s[i], ref dF)) sb.Append(s[i++]);
			double r;
			if (Double.TryParse(sb.ToString(), out r))
			{
				d = r;
				return true;
			}
			return false;
		}
		public static bool IsDigit(char c, ref bool dotFound)
		{
			if (Char.IsNumber(c)) return true;
			if (c == '.')
			{
				if (dotFound) return false;
				dotFound = true;
				return true;
			}
			return false;
		}
		public static bool TryParse(string s, out double d, out string u)
		{
			// expect syntax nn.nn uuuu
			d = Double.NaN; u = String.Empty;
			if (String.IsNullOrEmpty(s)) return false;
			int ndx = IndexOfWhiteSpace(s);
			if (ndx > 0)
			{
				string sd = s.Substring(0, ndx).Trim(), su = s.Substring(ndx).Trim();
				if (Double.TryParse(sd, out d))
				{
					u = su;
					return true;
				}
			}
			return false;
		}
		public static bool TryParse(string s, string unitName, out double d)
		{
			d = Double.NaN;
			if (String.IsNullOrEmpty(s)) return false;
			int ndx = s.IndexOf(unitName);
			if (ndx > 0)
			{
				ndx--;
				while (ndx >= 0 && Char.IsWhiteSpace(s[ndx])) ndx--;
				int n = ndx;
				while (n > 0 && (Char.IsDigit(s[n]) || s[n] == '.')) n--;
				s = s.Substring(n, ndx - n + 1);
				return Double.TryParse(s, out d);
			}
			return false;
		}
		public static string RemoveBrackets(string s)
		{
			if (String.IsNullOrEmpty(s)) return s;
			int n = s.IndexOf('[');
			while (n >= 0)
			{
				int n2 = s.IndexOf(']', n + 1);
				if (n2 > n)
				{
					s = s.Substring(0, n) + s.Substring(n2 + 1);
					n = s.IndexOf('[');
				}
			}
			return s;
		}

		private static readonly string[] _falseStarts = new string[] { "est.", "α form: est.", "(r.t.) (α, poly", "at r.t. α, poly:" };

		public UnitValue(double v, string units, ValueQualifier qualifier): base(v, qualifier)
		{
			Units = units;
		}

		public UnitValue(double v, string units):
			base(v)
		{
			Value = v;
			Units = units;
		}

		public UnitValue(string sVal)
		{
			if (String.IsNullOrWhiteSpace(sVal)) throw new ArgumentException();
			ValueQualifier vq = DoubleValue.FindQualifier(ref sVal);
			foreach (String fs in _falseStarts)
			{
				if (sVal.StartsWith(fs))
				{
					sVal = sVal.Substring(fs.Length + 1).Trim();
					break;
				}
			}
			sVal = RemoveBrackets(sVal);
			int i1 = 0;
			bool dF = false;
			while (i1 < sVal.Length && !IsDigit(sVal[i1], ref dF)) i1++;
			StringBuilder sb = new StringBuilder();
			dF = false;
			while (i1 < sVal.Length && IsDigit(sVal[i1], ref dF)) sb.Append(sVal[i1++]);
			if (sb.Length == 0) throw new ArgumentException(String.Format("Unable to parse double from string '{0}'.", sVal));
			Value = double.Parse(sb.ToString());
			sVal = sVal.Substring(i1).Trim();
			Units = sVal;
			Qualifier = vq;
		}

		public string Units { get; private set; }

		public override string ToString()
		{
			switch (Qualifier)
			{
				case ValueQualifier.None: return String.Format("{0} {1}", Value, Units);
				default:
					return String.Format("{0}\t({1})\t({2})", Value, Units, Qualifier);
			}
			
		}

		public override string Print(PrintOptions ops)
		{
			StringBuilder s = new StringBuilder();
			s.Append(Value);
			if (ops.ShowUnits())
			{
				s.Append(SPACE).Append(Units);
			}
			if (ops.ShowQualifier() && Qualifier != ValueQualifier.None)
			{
				s.Append(" ").Append(Qualifier);
			}
			return s.ToString();
		}
	}

	public class UnitValueList : List<UnitValue>, IPrintable
	{
		public static UnitValueList Parse(string v, string unit)
		{
			// "2372.3;5250.5"
			string[] p = v.Split(';');
			UnitValueList r = new Base.UnitValueList();
			foreach(string s in p)
			{
				r.Add(new Base.UnitValue(double.Parse(s), unit));
			}
			return r;
		}

		public string Print(PrintOptions ops)
		{
			StringBuilder s = new StringBuilder();
			foreach(UnitValue uv in this)
			{
				if (s.Length > 0) s.Append(";");
				s.Append(uv.Print(ops));
			}
			return s.ToString();
		}
	}
}
