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

using CommonTools.Lib.MVVM;
using System;

namespace SavingState.Model.Plots
{
	public class AxisTickModel : ModelBase
	{
		public AxisTickModel(int index, double relativePosition, string label): this(index, relativePosition)
		{
			Label = label;
		}

		public AxisTickModel(int index, double relativePosition)
		{
			Index = index;
			RelativePosition = relativePosition;
		}

		public int Index { get; private set; }
		public double Position { get; private set; }
		public double RelativePosition { get; private set; }
		public string Label { get; private set; }
		public Func<double,string> LabelConverter { get; set; }

		/// <summary>
		/// Resize the axis with the containing view
		/// </summary>
		/// <param name="range">The pixel extent (width/height/depth etc.) of the axis</param>
		public void SetExtent(double range)
		{
			Position = RelativePosition * range;
			RaisePropertyChanged(nameof(Position));
		}

		/// <summary>
		/// Update labels to reflect changed range of data values.
		/// </summary>
		/// <param name="minVal">Minimum data value</param>
		/// <param name="maxVal">Maximum data value</param>
		/// <param name="invert">true if the axis is invereted (e.g., Y axis renders top to bottom, so is inverted relative to standard coordinate systems.</param>
		public void SetDataRange(double minVal, double maxVal, bool invert)
		{
			if (LabelConverter != null)
			{
				double relP = invert ? (1 - RelativePosition) : RelativePosition;
				double val = minVal + (maxVal - minVal) * relP;
				Label = LabelConverter(val);
				RaisePropertyChanged(nameof(Label));
			}
		}
	}
}
