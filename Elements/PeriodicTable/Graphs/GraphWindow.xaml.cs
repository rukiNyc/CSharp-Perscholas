using PeriodicTable.Graphs.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PeriodicTable.Graphs
{
	/// <summary>
	/// Interaction logic for GraphWindow.xaml
	/// </summary>
	public partial class GraphWindow : Window
	{
		private GraphWindowModel Model;
		public GraphWindow()
		{
			InitializeComponent();
			DataContext = Model = new GraphWindowModel();
			plot.SizeChanged += Plot_SizeChanged;
		}

		private void Plot_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Model.SetPlotDimensions(plot.PlotWidth, plot.PlotHeight);
		}
	}
}
