using System;
using System.Text;

namespace Elements
{
	public enum CrystalStructure
	{
		Unknown,
		Hexagonal,
		HexagonalClosePacked,
		BodyCenteredCubic,
		Rhombohedral,
		FaceCenteredDiamondCubic,
		Cubic,
		FaceCenteredCubic,
		Orthorhombic,
		Tetragonal,
		Monoclinic
	}

	public static class CrystalStructureEx
	{
		//private const string HxCP = "​hexagonal close-packed";
		//private const string Hx = "hexagonal";
		//private const string RH = "rhombohedral";

		public static string Name(this CrystalStructure cs)
		{
			StringBuilder s = new StringBuilder();
			foreach(Char c in cs.ToString())
			{
				if (Char.IsUpper(c) && s.Length > 0)
				{
					s.Append(" ");
				}
				s.Append(Char.ToLower(c));
			}
			return s.ToString();
		}

		public static bool TryParse(string value, out CrystalStructure cs)
		{
			cs = CrystalStructure.Unknown;
			if (String.IsNullOrEmpty(value)) return false;
			try
			{
				cs = FromValue(value);
				return true;
			}
			catch (Exception) { }
			return false;
		}

		private static CrystalStructure FromValue(string value)
		{
			if (String.IsNullOrEmpty(value)) throw new ArgumentNullException("value");
			value = value.ToLower();
			//if (value.IndexOf("predicted") > 0 || value.IndexOf("extrapolated") >= 0) return CrystalStructure.Unknown;
			if (value.IndexOf("hexagonal") > 0 && value.IndexOf("close-packed") >= 8) return CrystalStructure.HexagonalClosePacked;
			if (value.IndexOf("hexagonal") >= 0) return CrystalStructure.Hexagonal;
			if (value.IndexOf("rhombohedral") >= 0) return CrystalStructure.Rhombohedral;
			if (value.IndexOf("face-centered diamond-cubic") >= 0) return CrystalStructure.FaceCenteredDiamondCubic;
			if (value.IndexOf("face-centered cubic") >= 0 || value.IndexOf("face-centred cubic") >= 0) return CrystalStructure.FaceCenteredCubic;
			if (value.IndexOf("​body-centred cubic") >= 0 || value.IndexOf("​body-centered cubic") >= 0) return CrystalStructure.BodyCenteredCubic;
			if (value.IndexOf("cubic") >= 0) return CrystalStructure.Cubic;
			if (value.IndexOf("orthorhombic") >= 0) return CrystalStructure.Orthorhombic;
			if (value.IndexOf("tetragonal") >= 0) return CrystalStructure.Tetragonal;
			if (value.IndexOf("monoclinic") >= 0) return CrystalStructure.Monoclinic;
			throw new ArgumentException("Unknown Crystal Structure: " + value);
		}
	}
}
