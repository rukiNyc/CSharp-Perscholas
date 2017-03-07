using Elements;
using PeriodicTable.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PeriodicTable
{

	public class ElementPane : Control
	{
		static ElementPane()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementPane), new FrameworkPropertyMetadata(typeof(ElementPane)));
		}

		public static DependencyProperty ElementProperty = DependencyProperty.Register("Element", typeof(Element), typeof(ElementPane));

		public Element Element
		{
			get { return (Element)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		private void RaiseElementSelectedEvent(ElementSelectionType type)
		{
			IElementSource esrc = DataContext as IElementSource;
			if (esrc != null)
			{
				RaiseEvent(new ElementSelectedEventArgs(this, esrc.Element, type));
			}
		}

		private void RaiseElementDeselectedEvent(ElementSelectionType type)
		{
			IElementSource esrc = DataContext as IElementSource;
			if (esrc != null)
			{
				RaiseEvent(new ElementDeselectedEventArgs(this, esrc.Element, type));
			}
		}

		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			RaiseElementSelectedEvent(ElementSelectionType.Hover);
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			RaiseElementDeselectedEvent(ElementSelectionType.Hover);
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			RaiseElementSelectedEvent(ElementSelectionType.Click);
		}

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			RaiseElementSelectedEvent(ElementSelectionType.DoubleClick);
		}
	}
}
