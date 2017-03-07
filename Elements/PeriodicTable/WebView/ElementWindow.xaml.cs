using Elements;
using PeriodicTable.Models;
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
using System.Globalization;
using FormsBrowser = System.Windows.Forms.WebBrowser;

namespace PeriodicTable.WebView
{
	/// <summary>
	/// Interaction logic for ElementWindow.xaml
	/// </summary>
	public partial class ElementWindow : Window
	{
		public static DependencyProperty ElementProperty = DependencyProperty.Register("Element", typeof(Element), typeof(ElementWindow),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, HandleElementChanged));

	private static void HandleElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((ElementWindow)sender).OnElementChanged(e);
		}

		public ElementWindow()
		{
			InitializeComponent();
			DataContext = new ElementWindowModel(_browser);
		}

		public Element Element
		{
			get { return (Element)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		private ElementWindowModel Model { get { return (ElementWindowModel)DataContext; } }

		private void OnElementChanged(DependencyPropertyChangedEventArgs e)
		{
			Model.Element = (Element)e.NewValue;
		}

		public class ElementWindowModel : ModelBase
		{
			private const string _wikiRoot = "https://en.wikipedia.org/w/index.php?title={0}&printable=yes";
			private const string _acsRoot = "http://kcvs.ca/isotopesmatter/iupacMaterials/javascript/Interactive%20Periodic%20Table%20of%20the%20Isotopes/HTML5/index.html";
			private const string _rscRoot = "http://www.rsc.org/periodic-table/element/{0}/{1}";

			private FormsBrowser _browser;
			private Element _element;
			private int _nSource;
			private Visibility _tableVisibility = Visibility.Collapsed;

			public ElementWindowModel(FormsBrowser browser)
			{
				_browser = browser;
			}

			public Element Element
			{
				get { return _element; }
				set
				{
					_element = value;
					RaisePropertyChanged("Element");
					ApplySource();
				}
			}

			public int NSource
			{
				get { return _nSource; }
				set
				{
					_nSource = value;
					RaisePropertyChanged("NSource");
					ApplySource();
				}
			}

			public Visibility TableVisibility
			{
				get { return _tableVisibility; }
				set
				{
					_tableVisibility = value;
					RaisePropertyChanged("TableVisibility");
				}
			}

			private void ApplySource()
			{
				string src = String.Empty;
				switch (_nSource)
				{
					case 0: src = string.Format(_wikiRoot, Element.WebName); break;
					case 1:	src = string.Format(_rscRoot, Element.Number, Element.Name.ToLower()); break;
					case 2:
						TableVisibility = Visibility.Visible;
						return;
				}
				TableVisibility = Visibility.Collapsed;
				if (_browser.Url?.OriginalString != src) _browser.Url = new Uri(src);
			}
		}
	}

	public class ElementWindowSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value.ToString() == parameter as string);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool b = (bool)value;
			if (b) return Int32.Parse(parameter as string);
			return 0;
		}
	}

	public class ReverseVisiblityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Visibility v = (Visibility)value;
			switch (v)
			{
				case Visibility.Visible:	return Visibility.Collapsed;
				default:	return Visibility.Visible;
				
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
