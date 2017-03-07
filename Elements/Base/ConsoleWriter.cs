using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	internal sealed class ConsoleWriter : IDisposable
	{
		internal static void WriteError(string fmt, params object[] args)
		{
			using (new ConsoleWriter(ConsoleColor.Red))
			{
				Console.WriteLine(String.Format(fmt, args));
			}
		}

		private ConsoleColor _c1, _c2;
		internal ConsoleWriter(ConsoleColor color)
		{
			_c1 = Console.ForegroundColor;
			_c2 = color;
			Console.ForegroundColor = color;
		}

		internal ConsoleWriter(ConsoleColor color, string output): this(color)
		{
			Console.WriteLine(output);
		}

		void IDisposable.Dispose()
		{
			Console.ForegroundColor = _c1;
		}
	}
}
