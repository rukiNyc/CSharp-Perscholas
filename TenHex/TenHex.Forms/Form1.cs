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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenHex.Lib;

namespace TenHex.Forms
{
	public partial class Form1 : Form
	{
		InputType _inputType = InputType.Decimal;
		public Form1()
		{
			InitializeComponent();
			tbInput.Text = "1";
			tbInput.TextChanged += TbInput_TextChanged;
			rbBin.CheckedChanged += InputType_CheckedChanged;
			rbOct.CheckedChanged += InputType_CheckedChanged;
			rbDec.CheckedChanged += InputType_CheckedChanged;
			rbHex.CheckedChanged += InputType_CheckedChanged;
			cbUseLong.CheckedChanged += CbUseLong_CheckedChanged;
			ApplyInput();
			tbInput.SelectAll();
			tbInput.Focus();
		}

		private void CbUseLong_CheckedChanged(object sender, EventArgs e)
		{
			ApplyInput();
		}

		private void InputType_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			if (rb.Checked)
			{
				int tag = int.Parse(rb.Tag.ToString());
				_inputType = (InputType)tag;
				ApplyInput();
			}
		}

		private void TbInput_TextChanged(object sender, EventArgs e)
		{
			ApplyInput();
		}

		private void ApplyInput()
		{
			try
			{
				bool useLong = cbUseLong.Checked;
				TenHexConverter th = useLong ? TenHexConverter.ParseLong(tbInput.Text, _inputType) : TenHexConverter.Parse(tbInput.Text, _inputType);
				lblErr.Text = String.Empty;
				lblBin.Text = th.BinValue;
				lblOct.Text = th.OctValue;
				lblDec.Text = th.DecimalValue;
				lblHex.Text = th.HexValue;
			}
			catch(Exception ex)
			{
				lblBin.Text = lblOct.Text = lblDec.Text = lblHex.Text = String.Empty;
				lblErr.Text = ex.Message;
			}
		}
	}
}
