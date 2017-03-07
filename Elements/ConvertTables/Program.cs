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
