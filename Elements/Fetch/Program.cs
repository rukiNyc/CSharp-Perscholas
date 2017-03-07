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
using System.Threading.Tasks;
using System.Threading;
using Elements;

namespace Fetch
{
	class Program
	{
		static int nIn = 0;
		static int Main(string[] args)
		{
			if (args.Length == 0)
			{
				ShowHelp();
				return 1;
			}
			List<Element> elements = Element.LoadCore();
			Arguments A;
			try
			{
				A = new Arguments(args, elements);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 1;
			}
			foreach(Element e in A.Elements)
			{
				try
				{
					FetchElement(e, A);
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
					return 1;
				}
				Thread.Sleep(2000);
				int nTry = 0;
				while (nIn > 0 && 500 > nTry++) Thread.Sleep(500);		// Polling.  Evil lurks here.
			}
			return 0;
		}

		private static async void FetchElement(Element e, Arguments A)
		{
			Task<bool> fetch = Fetcher.FetchElement(e, A);
			bool success = false;
			string msg = String.Empty;
			Interlocked.Increment(ref nIn);
			try
			{
				success = await fetch;
				if (success) Console.WriteLine("Element {0} loaded.", e.Name);
			}
			catch(Exception ex)
			{
				msg = ex.Message;
			}
			if (!success)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Fetch failed for element {0}", e.Name);
				if (!String.IsNullOrEmpty(msg)) Console.WriteLine(msg);
				Console.ResetColor();
			}
			Interlocked.Decrement(ref nIn);
		}

		static void ShowHelp()
		{
			Console.WriteLine("Arguments:");
			Console.WriteLine("[none]:\tShow this message");
			Console.WriteLine("[element symbol or name or atomic #:\tfetch the element");
			Console.WriteLine("/a:\tfetch all elements");
			Console.WriteLine("/o:\t[directory path]   (default: currentdirectory/output");
		}
	}
}
