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
using NOAA.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SavingState.Model.Plots
{
	public class TemperaturePlotViewModel : ModelBase
	{
		private static List<AxisTickModel> CreateXTicks()
		{
			List<AxisTickModel> r = new List<AxisTickModel>();
			for(int i=0;i<12;++i)
			{
				string monthName = new DateTime(2017, (i+1), 1).ToString("MMM", CultureInfo.InvariantCulture);
				double relPos = (double)i / 11d;
				r.Add(new AxisTickModel(i, relPos, monthName));
			}
			return r;
		}

		private static List<AxisTickModel> CreateYTicks()
		{
			Func<double, string> labelCnvt = d => d.ToString("F1");
			List<AxisTickModel> r = new List<AxisTickModel>();
			for(int i=0;i<10;++i)
			{
				double relPos = 1 - (double)i / 9d;
				r.Add(new AxisTickModel(i, relPos) { LabelConverter = labelCnvt });
			}
			return r;
		}

		private PlotModel _plot;
		private XAxisModel _xAxis;
		private YAxisModel _yAxis;
		private double _lastWidth, _lastHeight;

		public TemperaturePlotViewModel()
		{
			_plot = new PlotModel();
			_xAxis = new XAxisModel("Year", CreateXTicks());
			_yAxis = new YAxisModel("Temperature (\u00b0 C)", CreateYTicks());
		}

		public XAxisModel XAxis => _xAxis;

		public YAxisModel YAxis => _yAxis;

		public PlotModel Plot => _plot;

		public void SetStationData(IEnumerable<StationValue> stationValues, int year)
		{
			_plot.SetStationValues(stationValues);
			_xAxis.SetLabel($"Year {year}");
			_yAxis.SetDataRange(_plot.MinTemp, _plot.MaxTemp);
			RaisePropertyChanged(nameof(Plot));
		}

		/// <summary>
		/// Update the model(s) to use new dimensions of the plot view
		/// </summary>
		/// <param name="width">width of the plot view</param>
		/// <param name="height">height of the plot view</param>
		public void SetPlotExtent(double width, double height)
		{
			_lastWidth = width;
			_lastHeight = height;
			_xAxis.SetExtent(width);
			_yAxis.SetExtent(height);
			_plot.SetExtent(width, height);
		}
	}
}
