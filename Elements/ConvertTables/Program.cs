using Base;
using Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertTables
{
	class Program : ConsoleAppBase
	{
		const string TBL = "tblTest.txt";
		static int Main(string[] args)
		{
			if (args.Length == 0)
			{
				WriteError("Please enter the path to the table.");
				return 1;
			}
			string tblSrc = args[0];
			if (!File.Exists(tblSrc))
			{
				WriteError("Source table '{0}' not found.", tblSrc);
				return 2;
			}
			try
			{
				ElementList els = new ElementList();
				els.ReadTable(tblSrc);
				els.CreateTable(Path.GetDirectoryName(tblSrc), TBL);
				WriteSuccess("Table {0} created.", TBL);
			}
			catch(Exception ex)
			{
				WriteError(ex);
				return 3;
			}
			return 0;
		}
	}
}
