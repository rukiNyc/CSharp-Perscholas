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
using System.ComponentModel;
using TenHex.Lib;

namespace TenHexWPF
{
	public class TenHexModel : INotifyPropertyChanged
	{
		private TenHexConverter _tenHex;
		private InputType _inputType = InputType.Decimal;
		private string _inputValue = String.Empty, _errMsg = String.Empty;
		private bool _useLong;

		/// <summary>
		/// Default constructor
		/// </summary>
		public TenHexModel()
		{
			InputValue = "0";
		}

		public InputType InputType
		{
			get { return _inputType; }
			set
			{
				_inputType = value;
				InputValue = _inputValue;
			}
		}

		public string InputValue	// This property is get/set by the UI via the binding to TextBox.Text
		{
			get { return _inputValue; }
			set
			{
				_inputValue = value;
				try
				{
					if (_useLong) _tenHex = TenHexConverter.ParseLong(_inputValue, _inputType); else _tenHex = TenHexConverter.Parse(_inputValue, _inputType);
					// Success, so clear any error:
					ErrorMsg = String.Empty;
				}
				catch (Exception ex)
				{
					ErrorMsg = ex.Message;
					_tenHex = null;
				}
				RaisePropertyChanged("Hex", "Dec", "Oct", "Bin");
			}
		}

		public bool UseLongInt
		{
			get { return _useLong; }
			set
			{
				_useLong = value;
				InputValue = InputValue;	// force refresh
			}
		}

		public string ErrorMsg
		{
			get { return _errMsg; }
			private set
			{
				_errMsg = value;
				RaisePropertyChanged("ErrorMsg");
			}
		}

		public string Hex => _tenHex?.HexValue;

		public string Dec => _tenHex?.DecimalValue.ToString();

		public string Oct => _tenHex?.OctValue;

		public string Bin => _tenHex?.BinValue;

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(params string[] propertyNames)
		{
			if (PropertyChanged != null)
			{
				foreach (string n in propertyNames)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(n));
				}
			}
		}
	}
}
