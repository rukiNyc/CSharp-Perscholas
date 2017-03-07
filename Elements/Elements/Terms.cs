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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Elements
{
	public class Terms : List<Term>
	{
		private static readonly List<Term> _terms;
		static Terms()
		{
			List<Term> r = new List<Term>();
			foreach (string s in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "TermsCore.txt")))
			{
				r.Add(new Term(s));
			}
			_terms = r;
		}

		public Terms()
		{
			AddRange(_terms);
		}
	}

	public class Term
	{
		internal Term(string line)
		{
			string[] parts = line.Split('\t');
			Name = parts[0];
			Type = parts[1];
			CreateQualityName();
		}
		public string Name { get; private set; }
		public string Type { get; private set; }
		public string QualityName { get; private set; }

		public bool IsNumeric
		{
			get
			{
				return (Type == "i") || (Type == "d");
			}
		}

		public bool TryGetNumeric(Qualities q, out double v)
		{
			v = Double.NaN;
			object o = q.GetValue(QualityName);
			if (o == null) return false;
			v = (double)o;
			return true;
		}

		private void CreateQualityName()
		{
			string[] parts = Name.Split(' ');
			StringBuilder sb = new StringBuilder();
			foreach (string p in parts)
			{
				StringBuilder sbc = new StringBuilder(p);
				for (int i = 0; i < sbc.Length; ++i)
				{
					if (i == 0) sbc[i] = Char.ToUpper(sbc[i]);
					if (Char.IsPunctuation(sbc[i]))
					{
						for (int j = i + 1; j < sbc.Length; ++j)
						{
							sbc[j - 1] = sbc[j];
						}
						sbc.Length--;
					}
				}
				sb.Append(sbc.ToString());
			}
			QualityName = sb.ToString();

		}

		public bool ParseTerm(string value, Qualities qualities)
		{
			if (String.IsNullOrEmpty(value)) return false;
			switch (Name)
			{
				case "Density": return ParseDensity(value, qualities);
				case "Electrical resistivity": return ParseElectricalResistivity(value, qualities);
				case "Electron configuration": return ParseElectronConfiguration(value, qualities);
				case "Element category": qualities.ElementCategory = ElementCategoryEx.Parse(value); return true;
				case "Heat of fusion": return ParseHeatOfFusion(value, qualities);
				case "Heat of vaporization": return ParseDU(value, "kJ/mol", qualities);
				case "Magnetic ordering": qualities.MagneticOrdering = MagneticOrderingEx.Parse(value); return true;
				case "Magnetic susceptibility": return ParseMagneticSusceptibility(value, qualities);
				case "Molar heat capacity": return ParseHeatCapacity(value, qualities);
				case "Period": return ParsePeriod(value, qualities);
				case "Phase": return ParsePhase(value, qualities);
				case "Shear modulus": return ParseShearModulus(value, qualities);
				case "Speed of sound": return ParseSpeedOfSound(value, qualities);
				case "Thermal conductivity": return ParseThermalConductivity(value, qualities);
				case "Thermal expansion": return ParseThermalExpansion(value, qualities);
				case "Van der Waals radius": return ParseVanDerWaalsRadius(value, qualities);
				case "Vickers hardness": return ParseVickersHardness(value, qualities);
				case "Young's modulus": return ParseYoungsModulus(value, qualities);
				case "Boiling point": return ParseBoilingPoint(value, qualities);
				case "Melting point": return ParseMeltingPoint(value, qualities);
				case "Triple point": return ParseTriplePoint(value, qualities);
				case "Critical point": return ParseCriticalPoint(value, qualities);
				case "Covalent radius": return ParseCovalentRadius(value, qualities);
				case "Atomic radius": return ParseAtomicRadius(value, qualities);
				case "Bulk modulus": return ParseBulkModulus(value, qualities);
				case "Brinell hardness": return ParseBrinellHardness(value, qualities);
				case "Discovery": return ParseDiscovery(value, qualities);
				case "CAS Number": return ParseCAS(value, qualities);
			}
			switch (Type)
			{
				case "i": return ParseInteger(value, qualities);
				case "s":
					value = StringEx.RemoveReferences(value);
					SetValue(qualities, value); return true;
				case "xtal": return ParseCrystalStructure(value, qualities);
				case "d": return ParseDouble(value, qualities);
				case "ie": return ParseIE(value, qualities);
				case "du": return ParseDU(value, qualities);
				case "ii": return ParseIntegers(value, qualities);
				case "t":
				case "tp":
					SetValue(qualities, value);
					return true;
				case "grp": return ParseGroupBlock(value, qualities);
				default:
					throw new ArgumentException("Unrecognized Type: " + Type);
			}
		}

		private bool SetValue(Qualities q, object v)
		{
			Type t = q.GetType();
			PropertyInfo p = t.GetProperty(QualityName);
			if (p == null) throw new Exception(String.Format("Property Qualities.{0} not found", QualityName));
			p.SetValue(q, v);
			return true;
		}

		private bool ParseInteger(string v, Qualities q)
		{
			int i;
			if (UnitValue.TryParse(v, out i))
			{
				SetValue(q, i);
				return true;
			}
			return false;
		}

		private bool ParseDouble(string v, Qualities q)
		{
			double d;
			if (UnitValue.TryParse(v, out d))
			{
				SetValue(q, d);
				return true;
			}
			return false;
		}

		private bool ParseDU(string v, string unitName, Qualities q)
		{
			if (String.IsNullOrEmpty(v)) return false;
			v = StringEx.RemoveReferences(v);
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			double d;
			if (UnitValue.TryParse(v, unitName, out d))
			{
				UnitValue uv = new UnitValue(d, unitName, vq);
				return SetValue(q, uv);
			}
			return false;
		}

		private bool ParseDU(string v, Qualities q)
		{
			v = StringEx.RemoveReferences(v);
			SetValue(q, new UnitValue(v));
			return true;
		}

		private bool ParseIE(string value, Qualities q)
		{
			// Parse Ionization Energies
			// "1st: 499 kJ/mol\n2nd: 1170 kJ/mol\n3rd: 1900 kJ/mol\n(more)"
			string[] parts = value.Split(':', '\n');
			UnitValueList r = new UnitValueList();
			foreach (string s in parts)
			{
				if (s.IndexOf("kJ/mol") > 0) r.Add(new UnitValue(s));
			}
			if (r.Count > 0)
			{
				SetValue(q, r);
				return true;
			}
			return false;
		}

		private bool ParseCrystalStructure(string v, Qualities q)
		{
			CrystalStructure cs;
			if (CrystalStructureEx.TryParse(v, out cs))
			{
				q.CrystalStructure = cs;
				return true;
			}
			return false;
		}

		private bool ParseIntegers(string value, Qualities q)
		{
			// "+3, +2,[2] +1[3], −1, −2 ​(an amphoteric oxide)"
			value = RemoveBrackets(value);
			List<int> ri = new List<int>();
			foreach (String s in value.Split(',', ' ', ';'))
			{
				if (String.IsNullOrWhiteSpace(s)) continue;
				string ss = s;
				if (ss[0] == '−') ss = '-' + ss.Substring(1); // replace unicode $#8722
				int i;
				if (Int32.TryParse(ss, NumberStyles.AllowLeadingSign, null, out i)) ri.Add(i); else break;
			}
			if (ri.Count > 0)
			{
				SetValue(q, ri.ToArray());
				return true;
			}
			return false;
		}

		private bool ParseGroupBlock(string value, Qualities q)
		{
			const string GRP = "group", NA = "n/a";
			if (String.IsNullOrEmpty(value)) return false;
			value = value.Trim();
			if (!value.StartsWith(GRP)) return false;
			value = value.Substring(GRP.Length + 1);
			int gnum;
			if (value.StartsWith(NA)) gnum = 3; else 
				if (!UnitValue.TryParse(value, out gnum)) return false;
			int ndx = value.IndexOf(',');
			value = value.Substring(ndx + 2,1);
			Group grp = new Group(gnum, value, q.Element.Number == 1);
			q.GroupBlock = grp;
			return true;
		}

		private bool ParsePeriod(string v, Qualities q)
		{
			v = v.Replace("period", String.Empty).Trim();
			int p;
			if (Int32.TryParse(v, out p))
			{
				q.Period = p;
				return true;
			}
			return false;
		}

		private bool ParsePhase(string v, Qualities q)
		{
			v = StringEx.RemoveReferences(v);
			try
			{
				q.Phase = new QualifiedPhase(v);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool ParseDensity(string v, Qualities q)
		{
			const string GPL = "g/L", GCM3 = Units.GramsPerCC, GRPH = "graphite:", BLK = "black:";
			if (String.IsNullOrEmpty(v)) return false;
			int ndx, ndx2;  // General vars
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			v = StringEx.RemoveReferences(v);
			Action<string> getAllotrope = (string n) =>
			{
				ndx = v.IndexOf(n) + n.Length; ndx2 = v.IndexOf(GCM3, ndx);
				v = v.Substring(ndx);
				if (ndx2 > ndx)
				{
					ndx2 -= ndx;
					ndx2 += GCM3.Length;
					while (ndx2 > v.Length) ndx2--;
					v = v.Substring(0, ndx2);
				}
				v = v.Trim();
				vq = ValueQualifier.MostStableAllotrope;
			};
			switch (q.Element.Symbol)
			{ // Choose most stable allotropes:
				case "C": getAllotrope(GRPH); break;
				case "P": getAllotrope(BLK); break;
				case "S": getAllotrope("alpha:"); break;
				case "Se": getAllotrope("gray:"); break;
				case "Sn": getAllotrope("gray, α:"); break;
				case "Po": getAllotrope("alpha:"); break;
				// special case
				case "Br": v = v.Remove(0, "Br2, liquid:".Length).Trim(); break;
			}
			double d;
			string u;
			if (UnitValue.TryParse(v, out d, out u))
			{
				if (u == GPL)
				{
					d /= 1000;
					u = GCM3;
				}
				q.Density = new UnitValue(d, u, vq);
				return true;
			}
			return false;
		}

		private bool ParseElectricalResistivity(string v, Qualities q)
		{
			ValueQualifier vq = DoubleValue.FindQualifier(ref v);
			string[] UNITS = { "nΩ·m", "Ω·m", "µΩ·m" };
			double f = 1;
			for(int i=0;i<UNITS.Length;++i)
			{
				string s = UNITS[i];
				int ndx = v.IndexOf(s);
				if (ndx > 0)
				{
					v = v.Substring(0, ndx - 1).Trim();
					switch (i) {
						case 0:	f = 0.001; break;
						case 1:	f = 1000000; break;
					}
				}
				double d;
				if (UnitValue.TryParse(v, out d))
				{
					q.ElectricalResistivity = new UnitValue(d * f, UNITS[2], vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseElectronConfiguration(string s, Qualities q)
		{
			s = StringEx.RemoveReferences(s);
			const string LB = "or\n";
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			if (s.IndexOf(LB) > 0)
			{
				s = s.Replace(LB, ";");
			}
			q.ElectronConfiguration = new QualifiedString(s.Trim(), vq);
			return true;
		}

		private bool ParseMagneticSusceptibility(string s, Qualities q)
		{
			s = StringEx.RemoveReferences(s);
			double d;
			if (DoubleValue.TryParseScientific(s, out d))
			{
				q.MagneticSusceptibility = new UnitValue(d, "cm3/mol");
				return true;
			}
			return false;
		}

		private bool ParseHeatCapacity(string s, Qualities q)
		{
			const string UNITS = "J/(mol·K)";
			s = StringEx.RemoveReferences(s);
			double d;
			if (UnitValue.TryParse(s, UNITS, out d))
			{
				q.MolarHeatCapacity = new UnitValue(d, UNITS);
				return true;
			}
			return false;
		}

		private bool ParseHeatOfFusion(string s, Qualities q)
		{
			string UNIT = "kJ/mol";
			int n = 0;
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			if (s.StartsWith("("))
			{
				n = s.IndexOf(')');
				s = s.Substring(n + 1);
			}
			n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.HeatOfFusion = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseShearModulus(string s, Qualities q)
		{
			const string UNIT = "GPa";
			s = StringEx.RemoveReferences(s);
			double d;
			if (s.IndexOf("–") > 0)
			{
				double d1, d2;
				if (DoubleValue.TryParseRange(s, out d1, out d2))
				{
					d = (d1 + d2) / 2;
					q.ShearModulus = new UnitValue(d, UNIT);
				}
			}
			return ParseDU(s, UNIT, q);
		}

		private bool ParseSpeedOfSound(string s, Qualities q)
		{
			const string UNIT = "m/s";
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Replace(",", String.Empty);
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.SpeedOfSound = new UnitValue(d, UNIT);
					return true;
				}
			}
			return false;
		}

		private bool ParseBoilingPoint(string s, Qualities q)
		{
			const string UNIT = "K";
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			if (s.StartsWith("?"))
			{
				vq = ValueQualifier.Extrapolated;
				s = s.Substring(1).Trim();
			}
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.BoilingPoint = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseMeltingPoint(string s, Qualities q)
		{
			const string UNIT = "K";
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			if (s.StartsWith("?"))
			{
				vq = ValueQualifier.Extrapolated;
				s = s.Substring(1).Trim();
			}
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.MeltingPoint = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseThermalConductivity(string s, Qualities q)
		{
			const string UNIT = "W/(m·K)", EST = "est.";
			ValueQualifier vq = ValueQualifier.None;
			if (s.StartsWith(EST))
			{
				vq = ValueQualifier.Estimated;
				s = s.Substring(EST.Length).Trim();
			}
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d;
				if (DoubleValue.TryParseScientific(s, out d) || UnitValue.TryParse(s, out d))
				{
					q.ThermalConductivity = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseThermalExpansion(string s, Qualities q)
		{
			const string UNIT = "µm/(m·K)";
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				n = 0;
				while ((n < s.Length) && !Char.IsDigit(s[n])) n++;
				s = s.Substring(n);
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.ThermalExpansion = new UnitValue(d, UNIT);
					return true;
				}
			}
			return false;
		}

		private bool ParseVanDerWaalsRadius(string s, Qualities q)
		{
			const string UNIT = "pm";
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.VanDerWaalsRadius = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseVickersHardness(string s, Qualities q)
		{
			string UNIT = "MPa";
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d1, d2;
				if (DoubleValue.TryParseRange(s, out d1, out d2))
				{
					q.VickersHardness = new UnitValueRange(d1, d2, UNIT);
				}
				else
					if (UnitValue.TryParse(s, out d1))
				{
					q.VickersHardness = new UnitValueRange(d1, d1, UNIT);
				}
			}
			return q.VickersHardness != null;
		}

		private bool ParseYoungsModulus(string s, Qualities q)
		{
			const string UNIT = "GPa", EST = "est.";
			ValueQualifier vq = ValueQualifier.None;
			int n = s.IndexOf(EST);
			if (n > 0)
			{
				vq = ValueQualifier.Estimated;
				s = s.Substring(n + EST.Length).Trim();
			}
			n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				double d, d1;
				if (DoubleValue.TryParseRange(s, out d, out d1))
				{
					d = (d + d1) / 2;
				}
				else UnitValue.TryParse(s, out d);
				if (!Double.IsNaN(d))
				{
					q.YoungsModulus = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseTriple(string s, string UP, Action<TriplePoint> setter)
		{
			const string UT = "K";
			s = StringEx.RemoveReferences(s);
			int nt = s.IndexOf(UT), np = s.IndexOf(UP, nt);
			if (nt > 0 && np > 0)
			{
				string sp = s.Substring(nt + 2).Trim().Replace(",", ""), st = s.Substring(0, nt + 1).Trim();
				double dt, dp;
				if (UnitValue.TryParse(st, out dt) && UnitValue.TryParse(sp, out dp))
				{
					setter(new Elements.TriplePoint(new UnitValue(dt, Units.DegreesK), new UnitValue(dp, UP)));
					return true;
				}
			}
			return false;
		}

		private bool ParseTriplePoint(string s, Qualities q)
		{
			return ParseTriple(s,Units.KiloPascals, tp =>
			{
				q.TriplePoint = tp;
			});
		}

		private bool ParseCriticalPoint(string s, Qualities q)
		{
			return ParseTriple(s, Units.MegaPascals, tp =>
			{
				q.CriticalPoint = tp;
			});
		}

		private bool ParseAtomicRadius(string s, Qualities q)
		{
			const string UNIT = Units.PicoMeter;
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			int ndx = s.IndexOf(UNIT);
			if (ndx > 0)
			{
				s = s.Substring(0, ndx - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d)) {
					q.AtomicRadius = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseCovalentRadius(string s, Qualities q)
		{
			const string UNIT = Units.PicoMeter;
			ValueQualifier vq = DoubleValue.FindQualifier(ref s);
			int n = s.IndexOf(UNIT);
			if (n > 0)
			{
				s = s.Substring(0, n - 1).Trim();
				int n2 = s.IndexOf("–");
				if (n2 > 0)
				{
					string[] parts = s.Split('–');
					double d1, d2;
					if (UnitValue.TryParse(parts[0], out d1) && UnitValue.TryParse(parts[1], out d2))
					{
						q.CovalentRadius = new UnitValue((d1 + d2) / 2, UNIT, vq);
						return true;
					}
				}
				if (s.StartsWith("sp3")) s = s.Substring(5);
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.CovalentRadius = new UnitValue(d, UNIT, vq);
					return true;
				}
			}
			return false;
		}

		private bool ParseBulkModulus(string s, Qualities q)
		{
			const string UNIT = Units.GigaPascals;
			int ndx = s.IndexOf(UNIT);
			if (ndx > 0)
			{
				s = s.Substring(0, ndx - 1).Trim();
				double d;
				if (UnitValue.TryParse(s, out d))
				{
					q.BulkModulus = new UnitValue(d, UNIT);
					return true;
				}
			}
			return false;
		}

		private bool ParseBrinellHardness(string s, Qualities q)
		{
			const string UNIT = Units.MegaPascals;
			const char DASH = '–';
			int ndx = s.IndexOf(UNIT); 
			if (ndx > 0)
			{
				s = s.Substring(0, ndx - 1).Trim();
				ndx = s.IndexOf(DASH);
				if(ndx > 0)
				{
					string[] ss = s.Split(DASH);
					double d1, d2;
					if (UnitValue.TryParse(ss[0], out d1) && UnitValue.TryParse(ss[1], out d2))
					{
						q.BrinellHardness = new UnitValueRange(d1, d2, UNIT);
						return true;
					}
				} else
				{
					double d;
					if (UnitValue.TryParse(s, out d))
					{
						q.BrinellHardness = new UnitValueRange(d, d, UNIT);
						return true;
					}
				}
			}
			return false;
		}

		private bool ParseDiscovery(string s, Qualities q)
		{
			s = StringEx.RemoveReferences(s);
			int n1 = s.LastIndexOf("("), n2 = s.LastIndexOf(')');
			if (n1 > 0 && n2 > n1)
			{
				string disco = s.Substring(0, n1).Trim();
				string syr = s.Substring(n1 + 1);
				n1 = syr.IndexOf("June"); // Boron
				if (n1 > 0)
				{
					syr = syr.Substring(n1 + 5).Trim();
				}
				bool isBC = syr.IndexOf("BCE") > 0;
				double d;
				if (UnitValue.TryParse(syr, out d))
				{
					int yr = (int)d;
					if (isBC) yr *= -1;
					q.Discovery = new Discovery(disco, yr);
					return true;
				}
			}
			else // Sn, Sb have no parens
			{
				n1 = s.IndexOf("BC");
				if (n1 > 0)
				{
					string disco = String.Empty;
					s = s.Substring(0, n1 - 1).Trim();
					// before 5000
					int nn = s.IndexOfAny(StringEx.Digits);
					if (nn > 0)
					{
						disco = s.Substring(0, nn - 1).Trim();
						s = s.Substring(nn).Trim();
					}
					double d;
					if (UnitValue.TryParse(s, out d))
					{
						q.Discovery = new Discovery(disco, -(int)d);
						return true;
					}
				}
			}
			return false;
		}

		private bool ParseCAS(string s, Qualities q)
		{
			s = StringEx.RemoveReferences(s);
			if (s.IndexOf('\n') > 0) s = s.Replace("\n", "; ");
			q.CASNumber = s;
			return true;
		}

		private static string RemoveBrackets(string s)
		{
			int ndx = s.IndexOf('[');
			while (ndx >= 0)
			{
				int ndx2 = s.IndexOf(']', ndx);
				if (ndx2 > ndx)
				{
					s = s.Remove(ndx, 1 + ndx2 - ndx);
					ndx = s.IndexOf('[');
				}
				else break;
			}
			return s;
		}

		public override string ToString()
		{
			return QualityName;
		}
	}
}
