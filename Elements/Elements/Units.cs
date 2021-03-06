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
using System.Reflection;

namespace Base
{
	public static class Units
	{
		public static string GetName(string qualityName)
		{
			if (String.IsNullOrEmpty(qualityName)) return String.Empty;
			PropertyInfo p = typeof(Units).GetProperty(qualityName, typeof(string));
			if (p == null) return String.Empty;
			return p.GetValue(null) as string;
		}

		public const string CCPerMol = "cm3/mol";
		public const string DegreesK = "\u00b0K";
		public const string KiloPascals = "kPa";
		public const string MegaPascals = "MPa";
		public const string GigaPascals = "GPa";
		public const string MetersPerSecond = "m/s";
		public const string KiloJoulesPerMole = "kJ/mol";
		public const string MicroOhmMeters = "µΩ·m";
		public const string MicronPerMeterDegreeK = "µm/(m·K)";
		public const string JoulesPerMoleDegreeK = "J/(mol·K)";
		public const string PicoMeter = "pm";
		public const string WattsPerMeterDegreeK = "W/(m·K)";
		public const string GramsPerCC = "g/cm3";

		public static string Element { get { return ""; } }
		public static string AtomicNumber { get { return ""; } }
		public static string GroupBlock { get { return "Group/Block"; } }
		public static string Period { get { return ""; } }
		public static string StandardAtomicWeight { get { return ""; } }
		public static string ElementCategory { get { return ""; } }
		public static string ElectronConfiguration { get { return ""; } }
		public static string Phase { get { return ""; } }
		public static string MeltingPoint { get { return DegreesK; } }
		public static string BoilingPoint { get { return DegreesK; } }
		public static string Density { get { return GramsPerCC; } }
		public static string TriplePoint { get { return String.Format("{0} @ {1}", DegreesK, KiloPascals); } }
		public static string CriticalPoint { get { return String.Format("{0} @ {1}", DegreesK, MegaPascals); } }
		public static string HeatOfFusion { get { return KiloJoulesPerMole; } }
		public static string HeatOfVaporization { get { return KiloJoulesPerMole; } }
		public static string MolarHeatCapacity { get { return JoulesPerMoleDegreeK; } }
		public static string OxidationStates { get { return ""; } }
		public static string Electronegativity { get { return ""; } }
		public static string IonizationEnergies { get { return KiloJoulesPerMole; } }
		public static string AtomicRadius { get { return PicoMeter; } }
		public static string CovalentRadius { get { return PicoMeter; } }
		public static string VanDerWaalsRadius { get { return PicoMeter; } }
		public static string CrystalStructure { get { return ""; } }
		public static string SpeedOfSound { get { return MetersPerSecond; } }
		public static string ThermalExpansion { get { return MicronPerMeterDegreeK; } }
		public static string ThermalConductivity { get { return WattsPerMeterDegreeK; } }
		public static string ElectricalResistivity { get { return MicroOhmMeters; } }
		public static string MagneticOrdering { get { return ""; } }
		public static string MagneticSusceptibility { get { return CCPerMol; } }
		public static string YoungsModulus { get { return GigaPascals; } }
		public static string ShearModulus { get { return GigaPascals; } }
		public static string BulkModulus { get { return GigaPascals; } }
		public static string PoissonRatio { get { return ""; } }
		public static string MohsHardness { get { return ""; } }
		public static string VickersHardness { get { return MegaPascals; } }
		public static string BrinellHardness { get { return MegaPascals; } }
		public static string CASNumber { get { return ""; } }
		public static string Discovery { get { return "Name (Year)"; } }
	}


}
