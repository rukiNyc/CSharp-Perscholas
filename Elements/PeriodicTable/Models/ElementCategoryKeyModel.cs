using Elements;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PeriodicTable.Models
{
	public class ElementCategoryKeyModel : TableItemModelBase
	{
		public static List<ECKey> CreateKeys()
		{
			List<ECKey> r = new List<ElementCategoryKeyModel.ECKey>();
			foreach(ElementCategory ec in Enum.GetValues(typeof(ElementCategory)))
			{
				if (ec == ElementCategory.Unknown) continue;
				r.Add(new ECKey(ec));
			}
			return r;
		}

		public class ECKey
		{
			public ElementCategory Category { get; set; }
			public Brush Color => ElementModel.ColorForCategory(Category);

			public ECKey(ElementCategory ec)
			{
				Category = ec;
			}
		}

		private List<ECKey> _keys = CreateKeys();
		public ElementCategoryKeyModel(): base(TableEntryType.ElementCategoryKey) { }

		public override int Column => 3;

		public override int Row => 1;

		public override int RowSpan => 3;

		public override int ColumnSpan => 4;

		public IEnumerable<ECKey> Keys => _keys;
	}
}
