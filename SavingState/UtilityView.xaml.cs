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

using SavingState.Model;
using System.Windows;
using System.Windows.Controls;

namespace SavingState
{
	/// <summary>
	/// Interaction logic for UtilityView.xaml
	/// </summary>
	public partial class UtilityView : UserControl
	{
		public UtilityView()
		{
			InitializeComponent();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property.Name == "DataContext")
			{
				UtilityViewModel m = DataContext as UtilityViewModel;
				if (m != null) m.SetConfirmation(GetYesNo);
			}
		}

		private bool GetYesNo(string question)
		{
			MessageBoxResult r = MessageBox.Show(question, "Before proceeding...", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
			return (r == MessageBoxResult.Yes);
		}
	}
}
