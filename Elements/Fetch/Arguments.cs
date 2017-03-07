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

using Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetch
{
	class Arguments
	{
		private List<Element> _inElems = new List<Element>();
		private static string _elemError = "Argument list includes both /a and one or more individual elements.";
		public Arguments(string[] args, List<Element> elements)
		{
			bool all = false, any = false;
			foreach(string arg in args)
			{
				if (arg.StartsWith("/"))
				{
					if (arg == "/a")
					{
						if (any) throw new ArgumentException(_elemError);
						_inElems.AddRange(elements);
						all = true;
					}
					else
					{
						if (arg.StartsWith("/o:"))
						{
							if (!String.IsNullOrEmpty(OutputFolder)) throw new ArgumentException("Output folder is specified more than once.");
							string outF = arg.Substring(3).ToLower();
							int ndx = outF.IndexOf(":\\");
							if (outF.IndexOf(":\\") != 1)
							{
								// relative path
								OutputFolder = Path.Combine(Environment.CurrentDirectory, outF);
							}
							else OutputFolder = outF;
						} else
						{
							throw new ArgumentException("Unrecognized argument:\t{0}", arg);
						}
					}
				} else
				{
					// name, number, or symbol:
					Element e = null;
					int n;
					if (Int32.TryParse(arg, out n) && n < elements.Count)
					{
						e = elements.Find(ee => ee.Number == n);
					} else
					{
						e = elements.Find(ee => (String.Equals(arg, ee.Name, StringComparison.OrdinalIgnoreCase) || String.Equals(arg, ee.Symbol)));
					}
					if (e == null)
					{
						throw new ArgumentException("Element '{0}' not found.", arg);
					}
					if (all) throw new ArgumentException(_elemError);
					_inElems.Add(e);
					any = true;
				}
			}
		}

		public IEnumerable<Element> Elements { get { return _inElems; } }
		public string OutputFolder { get; private set; }
		public string CreateOutputPath(string fileName)
		{
			return Path.Combine(OutputFolder, fileName + ".txt");
		}

	}
}
