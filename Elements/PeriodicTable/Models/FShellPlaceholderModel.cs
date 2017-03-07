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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PeriodicTable.Models
{
	public class FShellPlaceholderModel : TableItemModelBase
	{
		private int _row;

		public FShellPlaceholderModel(ElementCategory ec) :
			base(TableEntryType.FShellPlaceholder)
		{
			ElementCategory = ec;
			switch(ec)
			{
				case ElementCategory.Lanthanide:
					_row = 6;
					Range = "57 - 71";
					break;
				case ElementCategory.Actinide:
					_row = 7;
					Range = "89 - 103";
					break;
				default:
					throw new Exception($"Unexpected category: {ec}");
			}
		}
		
		public ElementCategory ElementCategory { get; private set; }

		public string Name => ElementCategory.ToString() + "s";

		public override int Column => 3;

		public override int Row => _row;

		public string Range { get; private set; }

		public Brush Background => ElementModel.ColorForCategory(ElementCategory);
	}
}
