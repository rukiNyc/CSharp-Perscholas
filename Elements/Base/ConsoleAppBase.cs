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
