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
	public class ValueCounter
	{
		private int _n;
		private double _min = double.MaxValue, _max = double.MinValue;
		private double _sum;

		public void CountValue(double value)
		{
			if (value < _min) _min = value;
			if (value > _max) _max = value;
			_sum += value;
			_n++;
		}

		public int N => _n;
		public double Minimum => _min;
		public double Maximum => _max;
		public double Mean => (_n > 0) ? _sum / _n : double.NaN;
		public bool HasRange => (_n > 0) && (_max - _min) > double.Epsilon;
		public double Range => HasRange ? (_max - _min) : double.NaN;

		public double RelativeValueOf(double value)
		{
			return HasRange ? (value - _min) / Range : double.NaN;
		}
	}
}
