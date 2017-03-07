using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTable.Models
{
	public class GroupLabelModel : TableItemModelBase
	{
		private int _group, _row, _column;

		public GroupLabelModel(int group):
			base (TableEntryType.FamilyLabel)
		{
			_group = group;
			_row = 0;
			_column = _group;
		}

		public override int Column => _column;

		public override int Row => _row;

		public int Group => _group;
	}
}
