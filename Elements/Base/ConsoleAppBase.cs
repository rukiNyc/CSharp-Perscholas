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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{

	public class ConsoleAppBase
	{
		public static void WriteError(object error)
		{
			string msg = (error == null) ? String.Empty : error.ToString();
			using (ConsoleWriter w = new ConsoleWriter(ConsoleColor.Red, msg))	{ }
		}

		public static void WriteError(string fmt, params object[] args)
		{
			if (args == null || args.Length == 0) WriteError((object)fmt); else WriteError(String.Format(fmt, args));
		}

		public static void WriteSuccess(string msg)
		{
			using(ConsoleWriter w = new ConsoleWriter(ConsoleColor.Green, msg))	{ }
		}

		public static void WriteSuccess(string fmt, params object[] args)
		{
			WriteSuccess(String.Format(fmt, args));
		}
	}
}
