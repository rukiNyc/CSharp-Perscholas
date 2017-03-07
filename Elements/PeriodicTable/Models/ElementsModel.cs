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
using System.IO;

namespace PeriodicTable.Models
{
	public class ElementsModel : ModelBase
	{
		private static ElementList _elements;
		static ElementsModel()
		{
			_elements = new ElementList(Path.Combine(Environment.CurrentDirectory, "table.txt"));
		}

		public static ElementList Elements => _elements;

		public ElementsModel()
		{
			List<TableItemModelBase> tableItems = new List<Models.TableItemModelBase>(_elements.ConvertAll(e => new ElementModel(e)));
			List<int> periods = new List<int>(), groups = new List<int>();
			tableItems.ForEach(e =>
			{
				ElementModel em = (ElementModel)e;
				int n = periods.BinarySearch(em.Qualities.Period);
				if (n < 0) periods.Insert(~n, em.Qualities.Period);
				n = groups.BinarySearch(em.Qualities.GroupBlock.Number);
				if (n < 0) groups.Insert(~n, em.Qualities.GroupBlock.Number);
			});
			tableItems.AddRange(periods.ConvertAll(p => new PeriodLabelModel(p)));
			tableItems.AddRange(groups.ConvertAll(g => new GroupLabelModel(g)));
			tableItems.Add(new ElementCategoryKeyModel());
			tableItems.Add(new FShellPlaceholderModel(ElementCategory.Lanthanide));
			tableItems.Add(new FShellPlaceholderModel(ElementCategory.Actinide));
			tableItems.Add(new ElementDetailModel());
			tableItems.Add(new PPButtonModel());
			TableItems = tableItems;
		}

		public IEnumerable<TableItemModelBase> TableItems { get; private set; }

		public int Columns { get { return 18; } }

		public int Rows {  get { return 9; } }
	}
}
