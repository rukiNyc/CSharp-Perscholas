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
