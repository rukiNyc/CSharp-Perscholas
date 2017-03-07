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
