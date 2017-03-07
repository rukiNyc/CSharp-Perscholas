using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Models
{
	public enum TableEntryType
	{
		PeriodLabel,
		FamilyLabel,
		Element,
		PlaceHolder,
		ElementCategoryKey,
		FShellPlaceholder,
		Detail,
		PPButton
	}

	public abstract class TableItemModelBase : ModelBase
	{
		protected TableItemModelBase(TableEntryType type)
		{
			EntryType = type;
		}

		public TableEntryType EntryType { get; private set; }

		public abstract int Column { get; }

		public abstract int Row { get; }

		public virtual int ColumnSpan => 1;

		public virtual int RowSpan => 1;
	}
}
