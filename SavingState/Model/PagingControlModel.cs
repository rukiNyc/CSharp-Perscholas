/*	Copyright (c) 2017  Kenneth Brady
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and asssociated documentation files (the "Software"), to deal
 *	in the Sortware without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *	
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *	
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
*/

using CommonTools.Lib.Extensions;
using CommonTools.Lib.MVVM;
using NOAA.Lib.DataModel;

namespace SavingState.Model
{
	public class PagingControlModel : ModelBase
	{
		private int _offset, _count, _limit;

		public PagingControlModel()
		{
			Reset();
		}

		public PagingControlModel(int offset, int limit, int count = 0)
		{
			_count = count;
			_offset = offset;
			_limit = limit;
		}

		public PagingControlModel(ResultSet resultSet): this()
		{
			ApplyResultSet(resultSet);
		}

		public int Offset
		{
			get { return _offset; }
			set
			{
				_offset = value;
				RaisePropertyChanged(nameof(Offset));
			}
		}

		public int Count => _count;

		public void SetCount(int count)
		{
			// Count is readonly from XAML, settable from code
			_count = count;
			RaisePropertyChanged(nameof(Count));
		}

		public int Limit
		{
			get { return _limit; }
			set
			{
				_limit = value;
				RaisePropertyChanged(nameof(Limit));
			}
		}

		public void Reset()
		{
			Offset = 1;
			_count = 0;
			RaisePropertyChanged(nameof(Count));
			Limit = 25;  // Default for CDOService
		}

		public TextInputFilterType FilterType => TextInputFilterType.Custom;

		public void ApplyResultSet(ResultSet resultSet)
		{
			if (resultSet != null)
			{
				_count = resultSet.Count;
				RaisePropertyChanged(nameof(Count));
				Offset = resultSet.Offset;
				Limit = resultSet.Limit;
			}
		}

		public TextInputFilter IsValidOffset => IsValidOffsetImpl;

		public TextInputFilter IsValidLimit => IsValidLimitImpl;

		public bool IsValidOffsetImpl(string newInput, string fullText)
		{
			int v;
			return int.TryParse(fullText, out v) && v <= _count;
		}

		public bool IsValidLimitImpl(string newInput, string fullText)
		{
			int v;
			return (int.TryParse(fullText, out v) && v <= 1000);
		}
	}
}
