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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingState.Model
{
	public class ProgressModel : ModelBase
	{
		private double _min, _max, _val;
		private string _countMessage = String.Empty;
		private bool _isIndeterminate = false;

		public ProgressModel(): this(0, 100, 0) { }

		public ProgressModel(double minimum, double maximum, double value)
		{
			_min = minimum;
			_max = maximum;
			_val = value;
		}

		public ProgressModel(bool isIndeterminate, string countMessage = ""): this()
		{
			_isIndeterminate = isIndeterminate;
			_countMessage = countMessage;
		}

		public double Minimum
		{
			get { return _min; }
			set
			{
				_min = value;
				RaisePropertyChanged(nameof(Minimum));
			}
		}

		public double Maximum
		{
			get { return _max; }
			set
			{
				_max = value;
				RaisePropertyChanged(nameof(Maximum), nameof(CountMessage));
			}
		}

		public double Value
		{
			get { return _val; }
			set
			{
				_val = value;
				RaisePropertyChanged(nameof(Value), nameof(CountMessage));
			}
		}

		public string CountMessage
		{
			get { return _countMessage; }
			private set
			{
				_countMessage = value;
				RaisePropertyChanged(nameof(CountMessage));
			}
		}

		public bool IsIndeterminate
		{
			get { return _isIndeterminate; }
			set
			{
				_isIndeterminate = value;
				RaisePropertyChanged(nameof(IsIndeterminate));
			}
		}

		public void InitializeCounting(int maxCount)
		{
			Value = 1;
			Maximum = maxCount;
			Minimum = 1;
			IsIndeterminate = false;
		}

		// Assumes that InitializeCounting has been called correctly
		public void SetCount(int count)
		{
			Value = count;
			CountMessage = $"{Value} of {Maximum}";
		}

		public void SetComplete()
		{
			SetCount((int)Maximum);
		}
	}
}
