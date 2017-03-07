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
