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
