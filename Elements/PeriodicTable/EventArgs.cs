using Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PeriodicTable
{
	public enum ElementSelectionType
	{
		Hover,
		Click,
		DoubleClick
	}

	public class ElementSelectedEventArgs: RoutedEventArgs
	{
		public ElementSelectedEventArgs(object source, Element element, ElementSelectionType type):
			base(PTable.ElementSelectedEvent, source)
		{
			if (element == null) throw new ArgumentNullException(nameof(element));
			Element = element;
			SelectionType = type;
		}

		public Element Element { get; private set; }
		public ElementSelectionType SelectionType { get; private set; }
	}

	public delegate void ElementSelectionRoutedEventHandler(object sender, ElementSelectedEventArgs e);

	public class ElementDeselectedEventArgs: RoutedEventArgs
	{
		public ElementDeselectedEventArgs(object source, Element deselectedElement, ElementSelectionType type):
			base(PTable.ElementDeselectedEvent, source)
		{
			DeselectedElement = deselectedElement;
			DeselectionType = type;
		}

		public Element DeselectedElement { get; private set; }
		public Element NewElement { get; set; }
		public ElementSelectionType DeselectionType { get; private set; }
	}

	public delegate void ElementDeselectionRoutedEventHandler(object sender, ElementDeselectedEventArgs e);
}
