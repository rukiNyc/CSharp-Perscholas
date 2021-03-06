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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
	public class Element : IPrintable
	{
		public static List<Element> LoadCore()
		{
			List<Element> r = new List<Element>();
			string path = Path.Combine(Environment.CurrentDirectory, "ElementsCore.txt");
			string[] lines = File.ReadAllLines(path);
			for (int i = 0; i < lines.Length; ++i)
			{
				string[] parts = lines[i].Split('\t', ' ');
				switch (parts.Length)
				{
					case 0: continue;
					case 2:
					case 3:
						Element e = new Element();
						e.Number = i + 1;
						e.Symbol = parts[0];
						e.Name = parts[1];
						if (parts.Length == 3) e.Suffix = parts[2];
						r.Add(e);
						break;
					default:
						Console.WriteLine("Unexpected input at line {0}: \"{1}\"", i + 1, String.Join("\t", parts));
						continue;
				}
			}
			return r;
		}

		public int Number { get; private set; }
		public string Symbol { get; private set; }
		public string Name { get; private set; }
		public string WebName
		{
			get
			{
				return String.Concat(Name, Suffix);
			}
		}
		private string Suffix { get; set; }

		public Qualities Qualities { get; internal set; }

		public override string ToString()
		{
			return Name;
		}

		public string Print(PrintOptions ops)
		{
			return Name;
		}
	}
}
