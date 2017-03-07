using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Models
{
	public class PeriodLabelModel : TableItemModelBase
	{
		private int _row, _column;
		public PeriodLabelModel(int p):
			base(TableEntryType.PeriodLabel)
		{
			Period = p;
			_column = 0;
			_row = p;
		}

		public int Period { get; set; }

		public override int Row => _row;

		public override int Column => _column;
	}
}
