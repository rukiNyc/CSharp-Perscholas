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

using PeriodicTable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Graphs.Models
{
	public enum AxisDimension { X, Y }

	public class AxisModel : ModelBase
	{
		private List<TickModel> _ticks;
		private double _minValue, _maxValue;
		private string _label;
		public AxisModel(AxisDimension dimension, string label, double minValue, double maxValue, int count=10)
		{
			Dimension = dimension;
			_label = label;
			_minValue = minValue;
			_maxValue = maxValue;
			_ticks = new List<TickModel>();
			double vRange = maxValue - minValue, deltaV = vRange / count;
			for(int i=0;i<=count;++i)
			{
				double v = minValue + i * deltaV;
				double relV = (double)i / count;
				TickModel tm;
				switch(Dimension)
				{
					case 
						AxisDimension.X:	tm = new TickModel(relV, 0, v.ToString("F2"));
						tm.SetValueExtent(minValue, vRange, 0, 1);
						break;
					default:
						tm = new TickModel(0, (1-relV), v.ToString("F2"));
						tm.SetValueExtent(0, 1, minValue, vRange);
						break;
				}
				_ticks.Add(tm);
			}
		}

		public AxisDimension Dimension { get; private set; }
		public string Label => _label;
		public double MinValue => _minValue;
		public double MaxValue => _maxValue;
		public IEnumerable<TickModel> Ticks => _ticks;

		public void SetScreenExtent(double width, double height)
		{
			_ticks.ForEach(t => t.SetScreenExtent(width, height));
			if (Dimension == AxisDimension.X) _ticks.ForEach(t =>
			{
				System.Diagnostics.Debug.WriteLine($"{t.Position.X}");
			});
		}

	}
}
