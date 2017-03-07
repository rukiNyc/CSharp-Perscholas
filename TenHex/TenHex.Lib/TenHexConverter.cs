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

namespace TenHex.Lib
{
	public enum InputType
	{
		HexaDecimal = 16,
		Decimal = 10,
		Octal = 8,
		Binary = 2
	}

	public class TenHexConverter
	{
		public static TenHexConverter Parse(string input, InputType mode)
		{
			int n = Convert.ToInt32(input, (int)mode);
			return new TenHexConverter(n);
		}

		public static TenHexConverter  ParseLong(string input, InputType mode)
		{
			long n = Convert.ToInt64(input, (int)mode);
			return new TenHexConverter(n);
		}

		public static bool TryParse(string input, InputType type, out TenHexConverter th)
		{
			th = null;
			try
			{
				th = Parse(input, type);
				return true;
			}
			catch { }
			return false;
		}

		public static bool TryParseLong(string input, InputType type, out TenHexConverter th)
		{
			th = null;
			try
			{
				th = ParseLong(input, type);
				return true;
			}
			catch { }		// snuff the exception
			return false;
		}

		private TenHexConverter(int decValue)
		{
			_v = decValue.ToString();
			_h = Convert.ToString(decValue, 16);
			_o = Convert.ToString(decValue, 8);
			_b = Convert.ToString(decValue, 2);
		}

		private TenHexConverter(long decValue)
		{
			_v = decValue.ToString();
			_h = Convert.ToString(decValue, 16);
			_o = Convert.ToString(decValue, 8);
			_b = Convert.ToString(decValue, 2);
		}

		string _v, _h, _o, _b;

		public string DecimalValue => _v;
		public string HexValue => _h;
		public string OctValue => _o;
		public string BinValue => _b;
	}

}
