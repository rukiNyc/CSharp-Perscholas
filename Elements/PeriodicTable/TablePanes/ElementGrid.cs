using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PeriodicTable
{
	class ElementGrid : Grid
	{
		public ElementGrid()
		{
			IsItemsHost = true;
			for (int i = 0; i < 19; ++i)
			{
				ColumnDefinition cd = new ColumnDefinition();
				// Note: default cd.Width = 1, and GridUnitType = Star
				if (i == 0)
				{
					cd.Width = new GridLength(30, GridUnitType.Pixel);					
				}
				ColumnDefinitions.Add(cd);
			}
			for (int i = 0; i < 11; ++i)
			{
				RowDefinition rd = new RowDefinition();
				if (i == 0) rd.Height = new GridLength(30, GridUnitType.Pixel);
				RowDefinitions.Add(rd);
			}
		}
	}
}
