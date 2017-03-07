using Elements;
using System;
using System.Windows.Media;

namespace PeriodicTable.Models
{
	public class ElementModel : TableItemModelBase, IElementSource
	{
		public static Brush ColorForCategory(ElementCategory ec)
		{
			switch(ec)
			{
				case ElementCategory.AlkaliMetal: return Brushes.Orange;
				case ElementCategory.AlkalineEarthMetal: return Brushes.Yellow;
				case ElementCategory.NonMetal: return Brushes.Chartreuse;
				case ElementCategory.Metalloid: return Brushes.Green;
				case ElementCategory.NobleGas: return Brushes.Turquoise;
				case ElementCategory.Lanthanide: return Brushes.LightSeaGreen;
				case ElementCategory.Actinide: return Brushes.Pink;
				case ElementCategory.TransitionMetal: return Brushes.Wheat;
				case ElementCategory.PostTransitionMetal: return Brushes.PaleGreen;
			}
			return Brushes.Transparent;
		}

		private int _row, _column;

		public ElementModel(Element e):
			base(TableEntryType.Element)
		{
			Element = e;
			var b = Qualities.GroupBlock;
			_column = b.Number - 1;
			_row = Qualities.Period - 1;
			switch (Qualities.ElementCategory)
			{
				case ElementCategory.Lanthanide:
					_row += 1;
					_column = 3 + Element.Number - 57;
					break;
				case ElementCategory.Actinide:
					_row += 1;
					_column = 3 + Element.Number - 89;
					break;
			}
			if (Qualities.ElementCategory == ElementCategory.Lanthanide || Qualities.ElementCategory == ElementCategory.Actinide) _row += 2;
			// Adjust for label row/col:
			_row++;
			_column++;
		}

		public Element Element { get; private set; }

		public Qualities Qualities => Element.Qualities;

		public override int Column { get { return _column; } }

		public override int Row { get { return _row; } }

		public string StandardAtomicWeight
		{
			get
			{
				double w = Qualities.StandardAtomicWeight;
				double iw = Math.Floor(w);
				double rem = w - iw;
				if (rem > Double.Epsilon) return Qualities.StandardAtomicWeight.ToString();
				return $"({iw})";
			}
		}

		public Brush Background => ColorForCategory(Qualities.ElementCategory);
	}
}
