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
