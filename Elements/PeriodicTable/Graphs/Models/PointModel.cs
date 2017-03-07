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
using System.Windows.Media;

namespace PeriodicTable.Graphs.Models
{
	public class PointModel : ModelBase
	{
		private string _toolTip;
		private Brush _fill;

		public PointModel(double relX, double relY)
		{
			RelativeXValue = relX;
			RelativeYValue = relY;
		}

		public PointModel(double relX, double relY, string tooltip, Brush fill): this(relX, relY)
		{
			_toolTip = tooltip;
			_fill = fill;
		}

		private double RelativeXValue { get; set; }
		private double RelativeYValue { get; set; }
		public double XValue { get; private set; }
		public double YValue { get; private set; }
		public double X { get; private set; }
		public double Y { get; private set; }

		public string ToolTip
		{
			get
			{
				if (!string.IsNullOrEmpty(_toolTip)) return _toolTip;
				return $"{XValue},{YValue}";
			}
		}

		public Brush Fill => _fill;

		public void SetValueExtent(double minX, double xRange, double minY, double yRange)
		{
			XValue = minX + xRange * RelativeXValue;
			YValue = minY + yRange * RelativeYValue;
		}

		public void SetScreenExtent(double width, double height)
		{
			X = RelativeXValue * width;
			Y = RelativeYValue * height;
			RaisePropertyChanged(nameof(X), nameof(Y));
		}

		public override string ToString()
		{
			return $"{X.ToString("F2")}, {Y.ToString("F2")}";
		}
	}
}
