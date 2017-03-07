namespace Elements
{
	public enum ElementCategory
	{
		NonMetal,
		NobleGas,
		AlkaliMetal,
		AlkalineEarthMetal,
		Metalloid,
		TransitionMetal,
		PostTransitionMetal,
		Lanthanide,
		Actinide,
		Unknown
	}

	public static class ElementCategoryEx
	{
		private static readonly string[] _terms = { "nonmetal", "noble gas", "alkali metal", "alkaline earth metal", "metalloid", "transition metal", "post-transition metal",
			"lanthanide", "actinide" };
		public static ElementCategory Parse(string inp)
		{
			int nsome = inp.IndexOf("sometimes");
			if (nsome < 0) nsome = inp.IndexOf("alternatively");
			if (nsome > 0) inp = inp.Substring(0, nsome - 1);
			if (inp.IndexOf(_terms[_terms.Length - 3]) > 0) return ElementCategory.PostTransitionMetal;
			for(int i = 0; i < _terms.Length; ++i)
			{
				int n = inp.IndexOf(_terms[i]);
				if (n > 0 && (nsome < 0 || n < nsome)) return (ElementCategory)i;
			}
			return ElementCategory.Unknown;
		}
	}
}
