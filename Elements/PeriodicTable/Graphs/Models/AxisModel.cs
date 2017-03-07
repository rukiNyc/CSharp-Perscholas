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
