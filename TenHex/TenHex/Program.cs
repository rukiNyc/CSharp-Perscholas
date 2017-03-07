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
using TenHex.Lib;

namespace TenHex
{
	class Program
	{
		static void ShowUsage()
		{
			Console.WriteLine("TenHex input modes are d (decimal), h (hexadecimal), o (octal), or b (binary)");
			Console.WriteLine("Enter one of these letters to change input mode.");
			Console.WriteLine("Enter any value in the current mode to see conversions.");
			Console.WriteLine("Enter 'q' to Quit.");
			Console.WriteLine();
		}

		static void Main(string[] args)
		{
			ShowUsage();

			InputType inputType = InputType.Decimal;
			Action<string> setInputType = (ss) =>
			{
				if (String.IsNullOrEmpty(ss)) return;
				switch (Char.ToLower(ss[0]))
				{
					case 'd': inputType = InputType.Decimal; break;
					case 'h': inputType = InputType.HexaDecimal; break;
					case 'o': inputType = InputType.Octal; break;
					case 'b':	inputType = InputType.Binary; break;
				}
			};
			if (args.Length > 0) setInputType(args[0]);
			while (true)
			{
				Console.Write($"Enter {inputType} value: ");
				string input = Console.ReadLine();
				if (input.Length == 1 && Char.IsLetter(input[0]))
				{
					if (Char.ToLower(input[0]) == 'q') break;
					setInputType(input);
					continue;
				}
				try
				{
					TenHexConverter th = TenHexConverter.Parse(input, inputType);
					Console.ForegroundColor = ConsoleColor.Green;	// indicate success
					if (inputType != InputType.HexaDecimal) Console.WriteLine($"Hex: {th.HexValue}");
					if (inputType != InputType.Decimal) Console.WriteLine($"Dec: {th.DecimalValue}");
					if (inputType != InputType.Octal) Console.WriteLine($"Oct: {th.OctValue}");
					if (inputType != InputType.Binary) Console.WriteLine($"Bin: {th.BinValue}");
					Console.ResetColor();
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;	// indicate failure
					Console.WriteLine($"Conversion failed: {ex.Message}");
					Console.ResetColor();
				}
			}
		}
	}
}
