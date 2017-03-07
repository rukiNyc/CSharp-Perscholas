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
