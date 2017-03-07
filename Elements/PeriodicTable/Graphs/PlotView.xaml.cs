using PeriodicTable.Graphs.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeriodicTable.Graphs
{
	/// <summary>
	/// Interaction logic for PlotView.xaml
	/// </summary>
	public partial class PlotView : UserControl
	{
		public PlotView()
		{
			InitializeComponent();
		}

		public double PlotWidth { get { return dataPlot.ActualWidth; } }
		public double PlotHeight { get { return dataPlot.ActualHeight; } }
	}

	public class CanvasExtentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int factor = Int32.Parse(parameter as string);
			double v = (double)value;
			return (v / factor) - 50;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
