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
