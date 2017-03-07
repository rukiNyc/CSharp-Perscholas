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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TenHex.Lib;

public partial class _Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ApplyInput();
		tbInput.Focus();
	}

	private InputType GetInputType()
	{
		InputType it = InputType.Decimal;
		if (rbBin.Checked) it = InputType.Binary;
		else
			if (rbOct.Checked) it = InputType.Octal;
		else
			if (rbDec.Checked) it = InputType.Decimal;
		else it = InputType.HexaDecimal;
		return it;
	}

	private void ApplyInput()
	{
		InputType it = GetInputType();
		lblInput.Text = String.Format("Enter {0} Value:", it);
		string input = tbInput.Text;
		bool useLong = cbLong.Checked;
		try
		{
			TenHexConverter tc = useLong ? TenHexConverter.ParseLong(input, it) : TenHexConverter.Parse(input, it);
			errMsg.Text = String.Empty;
			lblHex.Text = tc.HexValue;
			lblDec.Text = tc.DecimalValue;
			lblOct.Text = tc.OctValue;
			lblBin.Text = tc.BinValue;
		}
		catch(Exception ex)
		{
			lblHex.Text = lblDec.Text = lblOct.Text = lblBin.Text = String.Empty;
			errMsg.Text = ex.Message;
		}
	}
}