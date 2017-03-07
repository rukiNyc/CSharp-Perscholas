using Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Models
{
	public class ElementDetailModel : TableItemModelBase
	{
		public ElementDetailModel():
			base(TableEntryType.Detail)
		{

		}

		public override int Column => 7;

		public override int Row => 1;

		public override int ColumnSpan => 6;

		public override int RowSpan => 3;
	}
}
