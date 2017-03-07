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
	public class Qualities
	{
		private static readonly List<string> _propNames;

		static Qualities()
		{
			_propNames = new List<string>();
			Type qt = typeof(Qualities);
			foreach(PropertyInfo pi in qt.GetProperties())
			{
				_propNames.Add(pi.Name);
			}
		}

		public static List<string> GetDoublePropertyNames()
		{
			Type asDoubleType = typeof(IAsDouble);
			List<string> r = new List<string>();
			foreach(string pn in _propNames)
			{
				PropertyInfo pi = typeof(Qualities).GetProperty(pn);
				if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(int) || asDoubleType.IsAssignableFrom(pi.PropertyType)) r.Add(pn);
			}
			return r;
		}

		public Qualities(Element element)
		{
			if (element == null) throw new ArgumentNullException("element");
			Element = element;
			Element.Qualities = this;
		}

		public Element Element { get; private set; }

		#region Properties

		public int AtomicNumber { get; set; }
		public Group GroupBlock { get; set; }
		public int Period { get; set; }
		public double StandardAtomicWeight { get; set; }
		public ElementCategory ElementCategory { get; set; }
		public QualifiedString ElectronConfiguration { get; set; }
		public QualifiedPhase Phase { get; set; }
		public UnitValue MeltingPoint { get; set; }
		public UnitValue BoilingPoint { get; set; }
		public UnitValue Density { get; set; }
		public TriplePoint TriplePoint { get; set; }
		public TriplePoint CriticalPoint { get; set; }
		public UnitValue HeatOfFusion { get; set; }
		public UnitValue HeatOfVaporization { get; set; }
		public UnitValue MolarHeatCapacity { get; set; }
		public int[] OxidationStates { get; set; }
		public double Electronegativity { get; set; }
		public UnitValueList IonizationEnergies { get; set; }
		public UnitValue AtomicRadius { get; set; }
		public UnitValue CovalentRadius { get; set; }
		public UnitValue VanDerWaalsRadius { get; set; }
		public CrystalStructure CrystalStructure { get; set; }
		public UnitValue SpeedOfSound { get; set; }
		public UnitValue ThermalExpansion { get; set; }
		public UnitValue ThermalConductivity { get; set; }
		public UnitValue ElectricalResistivity { get; set; }
		public MagneticOrdering MagneticOrdering { get; set; }
		public UnitValue MagneticSusceptibility { get; set; }
		public UnitValue YoungsModulus { get; set; }
		public UnitValue ShearModulus { get; set; }
		public UnitValue BulkModulus { get; set; }
		public double PoissonRatio { get; set; }
		public double MohsHardness { get; set; }
		public UnitValueRange VickersHardness { get; set; }
		public UnitValueRange BrinellHardness { get; set; }
		public string CASNumber { get; set; }
		public Discovery Discovery { get; set; }

		#endregion

		public string GetValue(string fieldName)
		{
			StringBuilder sb;
			switch (fieldName)
			{
				// fields convertible directly to string:
				case "AtomicNumber":
				case "GroupBlock":
				case "Period":
				case "Category":
				case "ElectronConfiguration":
				case "Phase":
				case "MeltingPoint":
				case "BoilingPoint":
				case "TriplePoint":
				case "CriticalPressure":
				case "CrystalStructure":
				case "MagneticOrdering":
				case "MagneticSusceptibility":
				case "CASNumber":
				case "Discovery":
				// int & double fields:
				case "Electronegativity":
				case "PoissonRatio":
				case "MohsHardness":
				case "AtomicWeight":
				case "StandardAtomicWeight":
					object v = ValueOf(fieldName);
					return (v == null) ? String.Empty : v.ToString();
				// int[]
				case "OxidationStates":
					int[] ii = ValueOf(fieldName) as int[];
					if (ii != null && ii.Length > 0)
					{
						sb = new StringBuilder();
						foreach(int i in ii)
						{
							if (sb.Length > 0) sb.Append(",");
							sb.Append(i);
						}
						return sb.ToString();
					}
					break;
				// DValue[]
				case "IonizationEnergies":
					UnitValue[] dvals = ValueOf(fieldName) as UnitValue[];
					if (dvals != null && dvals.Length > 0)
					{
						return dvals[0].Units;	// assume all units same
					}
					break;
				// DValue fields:
				default:
					UnitValue dv = ValueOf(fieldName) as UnitValue;
					return (dv == null) ? String.Empty : dv.ToString();
			}
			return String.Empty;
		}

		public object ValueOf(string fieldName)
		{
			if (fieldName == "AtomicWeight") fieldName = "StandardAtomicWeight";
			PropertyInfo p = this.GetType().GetProperty(fieldName);
			if (p == null) throw new ArgumentException("Unknown property: Qualities." + fieldName);
			return p.GetValue(this);
		}

		public double? GetValueAsDouble(string fieldName)
		{
			object v = ValueOf(fieldName);
			if (v == null) return null;
			if (v is double) return (double)v;
			if (v is int) return (double)(int)v;
			if (v is IAsDouble) return ((IAsDouble)v).AsDouble();
			return null;
		}

		public override string ToString()
		{
			return Element.ToString();
		}

		public static string QualityHeader()
		{
			return String.Join("\t", _propNames.ToArray());
		}

		public static string UnitsHeader()
		{
			StringBuilder s = new StringBuilder();
			int n = 0;
			_propNames.ForEach((ss) =>
			{
				if (0 < n++) s.Append("\t");
				s.Append(Units.GetName(ss));
			});
			return s.ToString();
		}

		public string Values()
		{
			StringBuilder s = new StringBuilder();
			Action<string> append = delegate (string v)
			{
				if (s.Length > 0) s.Append('\t');
				if (v != null) s.Append(v);
			};
			foreach(string n in _propNames)
			{
				object o = ValueOf(n);
				if (o == null)
				{
					append(String.Empty);
					continue;
				}
				IPrintable p = o as IPrintable;
				if (p != null) append(p.Print(PrintOptions.None)); else
				{
					switch (n)
					{
						case "OxidationStates":
							append(String.Join(",", (int[])o));
							break;
						default:
							if (o is string) append(o.ToString()); else
							// value types
							if (o is int)
							{
								if ((int)o == 0) append(String.Empty); else append(o.ToString());
							}
							else
								if (o is double)
							{
								if ((double)o == 0.0) append(String.Empty); else append(o.ToString());
							}
							else
							{
								int io = (int)o;
								if (io == 0 && o.ToString() == "Unknown") append(String.Empty); else append(o.ToString());
							}
							break;
					}
				}
			}
			return s.ToString();
		}

		public void ReadValues(string values)
		{
			Type qType = typeof(Qualities);
			string[] vals = values.Split('\t');
			int nVal = 0;
			foreach(string n in _propNames)
			{
				string sVal = vals[nVal++];
				if (String.IsNullOrEmpty(sVal)) continue;
				PropertyInfo pi = qType.GetProperty(n);
				MethodInfo[] mis = pi.PropertyType.GetMethods();
				MethodInfo mi = null;
				foreach(MethodInfo tmi in mis)
				{
					if (tmi.Name == "Parse" && tmi.IsStatic)
					{
						mi = tmi;
						break;
					}
				}
				if (mi != null)
				{
					List<object> args = new List<object>(2);
					args.Add(sVal);
					if (mi.GetParameters().Length == 2) args.Add(Units.GetName(n));
					pi.SetValue(this, mi.Invoke(null, args.ToArray()));
					continue;
				}
				if (pi.PropertyType.IsEnum)
				{
					pi.SetValue(this, Enum.Parse(pi.PropertyType, sVal));
					continue;
				}
				switch (pi.PropertyType.Name)
				{
					case "Int32[]":
						pi.SetValue(this, ParseInt32(sVal));
						break;
					case "Int32":
						pi.SetValue(this, int.Parse(sVal));
						break;
					case "Double":
						pi.SetValue(this, double.Parse(sVal));
						break;
					case "String":
						pi.SetValue(this, sVal);
						break;
				}
			}
		}

		private static int[] ParseInt32(string v)
		{
			if (string.IsNullOrEmpty(v)) return new int[0];
			string[] sVals = v.Split(',');
			int[] r = new int[sVals.Length];
			int n = 0;
			foreach(string s in sVals)
			{
				r[n++] = int.Parse(s);
			}
			return r;
		}



		//public static void WriteUnitsCode(string fpath)
		//{
		//	using (FileStream fs = File.OpenWrite(fpath))
		//	{
		//		using (StreamWriter w = new StreamWriter(fs))
		//		{
		//			foreach(string p in _propNames)
		//			{
		//				w.WriteLine(String.Format("public string {0} {{ get {{ return \"\"; }} }}", p));
		//			}
		//		}
		//	}
		//}
	}

}
