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
using System.Collections.Generic;

namespace SavingState.Model.Plots
{
	public enum AxisDimension { X, Y };

	public abstract class AxisModel : ModelBase
	{
		private List<AxisTickModel> _ticks;
		protected AxisModel(AxisDimension dimension, string label, IEnumerable<AxisTickModel> ticks)
		{
			Dimension = dimension;
			Label = label;
			_ticks = new List<AxisTickModel>(ticks);
		}

		public AxisDimension Dimension { get; private set; }	
		public string Label { get; private set; }
		public IEnumerable<AxisTickModel> Ticks => _ticks;

		public void SetExtent(double extent)
		{
			_ticks.ForEach(t => t.SetExtent(extent));
		}

		public void SetLabel(string label)
		{
			Label = label;
			RaisePropertyChanged(nameof(Label));
		}

		public void SetDataRange(double minVal, double maxVal)
		{
			foreach (AxisTickModel m in _ticks) m.SetDataRange(minVal, maxVal, Dimension == AxisDimension.Y);
		}
	}

	public class XAxisModel : AxisModel
	{
		public XAxisModel(string label, IEnumerable<AxisTickModel> ticks): base(AxisDimension.X, label, ticks) { }
	}

	public class YAxisModel : AxisModel
	{
		public YAxisModel(string label, IEnumerable<AxisTickModel> ticks): base(AxisDimension.Y, label, ticks) { }
	}
}
