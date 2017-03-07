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
