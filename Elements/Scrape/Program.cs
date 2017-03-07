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
using Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Scrape
{
	class Program : ConsoleAppBase
	{
		static readonly ElementList _elements = new ElementList();
		static readonly Terms _terms = new Terms();

		static int Main(string[] args)
		{
			Arguments A;
			try {
				A = new Arguments(args);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 1;
			}
			try
			{
				Scrape(A);
				//_elements.CompileUniqueTerms(Path.Combine(A.InputDirectory, "Terms"), _terms);
				_elements.CreateTable(Path.Combine(A.InputDirectory, "Terms"));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return -1;
			}
			return 0;
		}

		static void Scrape(Arguments args)
		{
			foreach(FileInfo f in args.InputFiles)
			{
				try
				{
					LoadQualities(f);
				}
				catch(Exception ex)
				{
					WriteError("File {0} failed: {1}", f.Name, ex.Message);
					throw;
				}
			}
		}

		static void LoadQualities(FileInfo fInfo)
		{
			Element element = _elements.Find(Path.GetFileNameWithoutExtension(fInfo.FullName));
			if (element.Name == "Phosphorus")
			{
				//System.Diagnostics.Debugger.Break();
			}
			XElement xHtml = XElement.Load(fInfo.FullName);
			IEnumerable<XElement> tbls =
				from xe in xHtml.Descendants("table") select xe;
			XElement infoBox = tbls.First(xe => xe.Attributes("class").Any() && xe.Attributes("class").First().Value == "infobox");
			if (infoBox != null)
			{
				if (LoadQualities(element, infoBox))
				{
					WriteSuccess("{0} scraped", element.Name);
				} else
				{
					WriteError("{0} failed", element.Name);
				}
			} else
			{
				WriteError("infoBox not found:\t{0}", element.Name);
			}
		}

		static bool LoadQualities(Element element, XElement infoBox)
		{
			Qualities q = new Qualities(element);
			int nQ = 0;
			foreach(XNode xn in infoBox.Nodes())
			{
				XElement xe = xn as XElement;
				if (xe != null)
				{
					List<XNode> nodes = new List<XNode>(xe.Nodes());
					List<XElement> elems = new List<XElement>();
					foreach (XNode xn2 in nodes) if (xn2 is XElement) elems.Add((XElement)xn2);
					if (elems.Count == 2)
					{
						XElement e0 = elems[0], e1 = elems[1];
						string valName = e0.Value.Replace((char)160, (char)32);
						if (valName.StartsWith("Ionisation"))
						{
							valName = valName.Replace("Ionisation", "Ionization");
						}
						if (valName.IndexOf("vaporisation") > 0) valName = valName.Replace("isa", "iza");
						foreach(Term term in _terms)
						{
							try
							{
								if (valName.IndexOf(term.Name) >= 0)
								{
									if (term.ParseTerm(e1.Value, q)) nQ++;
									break;
								}
							}
							catch(Exception)
							{
								WriteError("Term {0} failed", term.Name);
								throw;
							}
						}
					}
				}
			}
			return nQ >= 8;
		}

		static bool LoadQuality(Term term, Qualities q, XElement infoBox)
		{
			IEnumerable<XElement> trms =
				from xe in infoBox.Descendants() where xe.Value.Contains(term.Name) select xe;
			XElement trm = trms.FirstOrDefault();
			if (trm == null) return false;
			try
			{
				return term.ParseTerm(((XElement)trm.LastNode).Value, q);
			}
			catch(Exception ex)
			{
				throw new Exception(String.Format("Term: {0}", term.Name), ex);
			}
		}

		static void ShowHelp()
		{
			Console.WriteLine("Arguments:");
			Console.WriteLine("/i: input directory  (defaults to current/Output");
			Console.WriteLine("/o: output directory  (defaults to input directory)");
		}
	}
}
