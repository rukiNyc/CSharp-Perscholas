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
using System.IO;
using System.Collections.Generic;

namespace Scrape
{
	class Arguments
	{
		static readonly string dfltOut = "..\\..\\Output";
		public Arguments(string[] args)
		{
			string outDir = dfltOut;
			foreach (string a in args)
			{
				if (a.StartsWith("/o:"))
				{
					outDir = a.Substring(3).ToLower();
					if (outDir.IndexOf(":\\") != 1)
					{
						// relative path:
						outDir = Path.Combine(Environment.CurrentDirectory, outDir);
					}
				}
				if (a.StartsWith("/i:"))
				{
					string inDir = a.Substring(3).ToLower();
					if (inDir.IndexOf(":\\") != 1) inDir = Path.Combine(Environment.CurrentDirectory, inDir);
					InputDirectory = inDir;
				}
			}
			OutputDirectory = outDir;
			if (String.IsNullOrEmpty(InputDirectory)) throw new ArgumentException("Input directory must be provided.");
		}

		public string OutputDirectory { get; private set; }
		public string InputDirectory { get; private set; }

		public List<FileInfo> InputFiles
		{
			get
			{
				List<FileInfo> r = new List<FileInfo>();
				foreach (String name in Directory.GetFiles(InputDirectory, "*.txt")) r.Add(new FileInfo(name));
				return r;
			}
		}
	}
}
