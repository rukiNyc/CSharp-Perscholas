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

using SavingState.Model.Plots;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace SavingState
{
	/// <summary>
	/// Interaction logic for TemperaturePlotView.xaml
	/// </summary>
	public partial class TemperaturePlotView : UserControl
	{
		public TemperaturePlotView()
		{
			InitializeComponent();
			SizeChanged += TemperaturePlotView_SizeChanged;
		}

		private void TemperaturePlotView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetPlotExtent();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property.Name == "DataContext") SetPlotExtent();
		}

		private void SetPlotExtent()
		{
			TemperaturePlotViewModel m = DataContext as TemperaturePlotViewModel;
			if (m != null) m.SetPlotExtent(dataPlot.ActualWidth, dataPlot.ActualHeight);
		}
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
