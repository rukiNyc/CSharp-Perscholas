using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
	public enum MagneticOrdering
	{
		Unknown,
		Diamagnetic,
		Paramagnetic,
		Ferromagnetic,
		AntiFerromagnetic
	}

	public static class MagneticOrderingEx
	{
		public static MagneticOrdering Parse(string s)
		{
			if (s.IndexOf("diamagnetic") >= 0) return MagneticOrdering.Diamagnetic;
			if (s.IndexOf("paramagnetic") >= 0) return MagneticOrdering.Paramagnetic;
			if (s.IndexOf("antiferromagnetic") >= 0) return MagneticOrdering.AntiFerromagnetic;
			if (s.IndexOf("ferromagnetic") >= 0) return MagneticOrdering.Ferromagnetic;
			return MagneticOrdering.Unknown;
		}
	}
}
