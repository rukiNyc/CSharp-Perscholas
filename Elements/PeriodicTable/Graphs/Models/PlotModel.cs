using Base;
using Elements;
using PeriodicTable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECKey = PeriodicTable.Models.ElementCategoryKeyModel.ECKey;

namespace PeriodicTable.Graphs.Models
{
	public class PlotModel : ModelBase
	{
		private static ElementList Elements = ElementsModel.Elements;

		private AxisModel _xAxis, _yAxis;
		private List<PointModel> _points;
		private List<ECKey> _ecKeys = ElementCategoryKeyModel.CreateKeys();

		public PlotModel(string xQuality, string yQuality)
		{
			_points = new List<Models.PointModel>();
			List<double?> xValues = Elements.GetValuesAsDouble(xQuality), yValues = Elements.GetValuesAsDouble(yQuality);

			List<ValuePair> values = Elements.GetValuesAsDouble(xQuality, yQuality, true);
			ValueCounter vcx = new ValueCounter(), vcy = new ValueCounter();
			values.ForEach(vp =>
			{
				vcx.CountValue(vp.V1);
				vcy.CountValue(vp.V2);
			});
			foreach(ValuePair vp in values)
			{
				StringBuilder s = new StringBuilder();
				s.AppendLine($"{vp.Element.Name}:");
				s.AppendLine($"{xQuality}: {vp.V1}");
				s.Append($"{yQuality}: {vp.V2}");
				PointModel pm = new PointModel(vcx.RelativeValueOf(vp.V1), vcy.RelativeValueOf(vp.V2), s.ToString(),
					ElementModel.ColorForCategory(vp.Element.Qualities.ElementCategory));
				_points.Add(pm);
			}
			if (vcx.HasRange && vcy.HasRange) // do we have plottable data?
			{
				_points.ForEach(p => p.SetValueExtent(vcx.Minimum, vcx.Range, vcy.Minimum, vcy.Range));
			}
			else _points.Clear();
			if (_points.Count == 0) return; // TODO: display "No Data" message?
			Func<string, string> withUnits = (string q) =>
			{
				string u = Units.GetName(q);
				if (String.IsNullOrEmpty(u)) return q;
				return $"{q}  ({u})";
			};
			_xAxis = new AxisModel(AxisDimension.X, withUnits(xQuality), vcx.Minimum, vcx.Maximum);
			_yAxis = new AxisModel(AxisDimension.Y, withUnits(yQuality), vcy.Minimum, vcy.Maximum);
		}

		public AxisModel XAxis => _xAxis;
		public AxisModel YAxis => _yAxis;
		public IEnumerable<PointModel> Points => _points;
		public IEnumerable<ECKey> Keys => _ecKeys;

		public void SetScreenExtent(double width, double height)
		{
			_points.ForEach(p => p.SetScreenExtent(width, height));
			if (_xAxis != null)  _xAxis.SetScreenExtent(width, height);
			if (_yAxis != null) _yAxis.SetScreenExtent(width, height);
		}
	}
}
