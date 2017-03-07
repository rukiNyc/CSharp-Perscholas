using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Graphs.Models
{
	public class TickModel
	{
		private PointModel _value;
		private string _label;

		public TickModel(double relX, double relY, string label)
		{
			_value = new PointModel(relX, relY);
			_label = label;
		}

		public PointModel Position => _value;
		public string Label => _label;

		public void SetValueExtent(double minX, double xRange, double minY, double yRange)
		{
			_value.SetValueExtent(minX, xRange, minY, yRange);
		}

		public void SetScreenExtent(double width, double height)
		{
			_value.SetScreenExtent(width, height);
		}

		public override string ToString()
		{
			return $"{_value.ToString()} : {_label}";
		}
	}
}
