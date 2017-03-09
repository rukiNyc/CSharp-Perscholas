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
using System.Linq;

namespace SavingState.Model.Plots
{
	public class PlotModel : ModelBase
	{
		private List<TemperaturePoint> _tPoints;
		private double _lastWidth, _lastHeight;

		public PlotModel() { }

		public IEnumerable<TemperaturePoint> Points => _tPoints;
		public double MinTemp { get; private set; }
		public double MaxTemp { get; private set; }

		public void SetStationValues(IEnumerable<StationValue> values)
		{
			Dictionary<int, TemperaturePoint> data = new Dictionary<int, TemperaturePoint>();
			double tmax = Double.MinValue, tmin = Double.MaxValue;
			foreach (StationValue sv in values)
			{
				int day = sv.Date.DayOfYear;
				double dayMax = DateTime.IsLeapYear(sv.Date.Year) ? 366d : 365d;
				TemperaturePoint tp;
				double tmp = 0;
				if (data.ContainsKey(day)) tp = data[day];
				else
				{
					tp = new TemperaturePoint { Day = day, RelativeTime = day / dayMax, Date = sv.Date };
					data.Add(day, tp);
				}
				switch (sv.DataType)
				{
					case "TMIN":
						tmp = sv.Value / 10;
						if (tmp < tmin) tmin = tmp;
						tp.TMin = tmp;
						break;
					case "TMAX":
						tmp = sv.Value / 10;
						if (tmp > tmax) tmax = tmp;
						tp.TMax = tmp;
						break;
				}
			}
			MinTemp = tmin;
			MaxTemp = tmax;
			double deltaT = MaxTemp - MinTemp;
			_tPoints = data.Values.Where(tp => tp.HasMean).OrderBy(tp => tp.Day).ToList();
			if (_tPoints.Count > 1)
			{
				foreach (TemperaturePoint tp in _tPoints)
				{
					tp.RelativeTemperature = 1 - (tp.TMean - MinTemp) / deltaT;
					tp.SetExtent(_lastWidth, _lastHeight);
				}
			}
			else _tPoints.Clear();
			RaisePropertyChanged(nameof(Points));
		}

		public void SetExtent(double width, double height)
		{
			_lastWidth = width;  _lastHeight = height;
			if (_tPoints != null) _tPoints.ForEach(tp => tp.SetExtent(width, height));
		}

	}

	public class TemperaturePoint : ModelBase
	{
		public DateTime Date { get; set; }
		public int Day { get; set; }
		public double RelativeTime { get; set; }	// 0 - 1
		public double RelativeTemperature { get; set; }	// 0 - 1
		public double Time { get; private set; }	// pixels
		public double Temperature { get; private set; } // pixels
		public double? TMin { get; set; }
		public double? TMax { get; set; }
		public double TMean { get { return HasMean ? (TMin.Value + TMax.Value) / 2 : double.NaN; } }
		public bool HasMean { get { return TMin.HasValue && TMax.HasValue; } }
		public string Properties
		{
			get
			{
				return $"Date: {Date.ToString("MMM dd")}\rTMin: {TMin.Value}\rTMax: {TMax.Value}\rTAvg: {TMean}";
			}
		}

		public void SetExtent(double width, double height)
		{
			Time = RelativeTime * width;
			Temperature = RelativeTemperature * height;
			RaisePropertyChanged(nameof(Time), nameof(Temperature));
		}
	}
}
