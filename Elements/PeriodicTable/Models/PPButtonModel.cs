using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Models
{
	public class PPButtonModel : TableItemModelBase
	{
		public PPButtonModel() : base(TableEntryType.PPButton) { }

		public override int Column => 1;

		public override int Row => 8;

		public override int ColumnSpan => 3;
	}
}
