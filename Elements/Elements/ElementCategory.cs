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
