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

using Elements;
using PeriodicTable.Models;
using System.Collections.Generic;

namespace PeriodicTable.Graphs.Models
{
	class GraphWindowModel : ModelBase
	{
		private static readonly List<string> _yValues = Qualities.GetDoublePropertyNames();
		private static readonly ElementList _elements = ElementsModel.Elements;

		static GraphWindowModel()
		{
			_yValues.Sort();
		}

		private string _xValue, _yValue;
		private PlotModel _plotModel;
		public double? _width, _height;
		public GraphWindowModel()
		{
			_xValue = _yValues[0];
			_yValue = _yValues[1];
			UpdatePlot();
		}

		public IEnumerable<string> XValues => _yValues;

		public string XValue
		{
			get { return _xValue; }
			set
			{
				_xValue = value;
				RaisePropertyChanged(nameof(XValue));
				UpdatePlot();
			}
		}

		public IEnumerable<string> YValues => _yValues;

		public string YValue
		{
			get { return _yValue; }
			set
			{
				_yValue = value;
				RaisePropertyChanged(nameof(YValue));
				UpdatePlot();
			}
		}

		public object PlotModel => _plotModel;

		public void SetPlotDimensions(double width, double height)
		{
			_width = width;
			_height = height;
			if (_plotModel != null) _plotModel.SetScreenExtent(width, height); else UpdatePlot();
		}

		private void UpdatePlot()
		{
			if (_width.HasValue && _height.HasValue)
			{
				_plotModel = new PlotModel(XValue, YValue);
				_plotModel.SetScreenExtent(_width.Value, _height.Value);
				RaisePropertyChanged(nameof(PlotModel));
			}
		}
	}
}
