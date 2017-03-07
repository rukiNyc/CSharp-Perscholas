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

namespace TenHex.Forms
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbUseLong = new System.Windows.Forms.CheckBox();
			this.rbBin = new System.Windows.Forms.RadioButton();
			this.rbOct = new System.Windows.Forms.RadioButton();
			this.rbDec = new System.Windows.Forms.RadioButton();
			this.rbHex = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblErr = new System.Windows.Forms.Label();
			this.lblBin = new System.Windows.Forms.Label();
			this.lblOct = new System.Windows.Forms.Label();
			this.lblDec = new System.Windows.Forms.Label();
			this.lblHex = new System.Windows.Forms.Label();
			this.tbInput = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.groupBox1.Controls.Add(this.cbUseLong);
			this.groupBox1.Controls.Add(this.rbBin);
			this.groupBox1.Controls.Add(this.rbOct);
			this.groupBox1.Controls.Add(this.rbDec);
			this.groupBox1.Controls.Add(this.rbHex);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.groupBox1.Size = new System.Drawing.Size(1141, 104);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Input Mode";
			// 
			// cbUseLong
			// 
			this.cbUseLong.AutoSize = true;
			this.cbUseLong.Location = new System.Drawing.Point(779, 49);
			this.cbUseLong.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.cbUseLong.Name = "cbUseLong";
			this.cbUseLong.Size = new System.Drawing.Size(165, 25);
			this.cbUseLong.TabIndex = 4;
			this.cbUseLong.Text = "Use Long Integers";
			this.cbUseLong.UseVisualStyleBackColor = true;
			// 
			// rbBin
			// 
			this.rbBin.AutoSize = true;
			this.rbBin.Location = new System.Drawing.Point(572, 49);
			this.rbBin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.rbBin.Name = "rbBin";
			this.rbBin.Size = new System.Drawing.Size(75, 25);
			this.rbBin.TabIndex = 3;
			this.rbBin.Tag = "2";
			this.rbBin.Text = "Binary";
			this.rbBin.UseVisualStyleBackColor = true;
			// 
			// rbOct
			// 
			this.rbOct.AutoSize = true;
			this.rbOct.Location = new System.Drawing.Point(392, 49);
			this.rbOct.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.rbOct.Name = "rbOct";
			this.rbOct.Size = new System.Drawing.Size(70, 25);
			this.rbOct.TabIndex = 2;
			this.rbOct.Tag = "8";
			this.rbOct.Text = "Octal";
			this.rbOct.UseVisualStyleBackColor = true;
			// 
			// rbDec
			// 
			this.rbDec.AutoSize = true;
			this.rbDec.Checked = true;
			this.rbDec.Location = new System.Drawing.Point(212, 49);
			this.rbDec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.rbDec.Name = "rbDec";
			this.rbDec.Size = new System.Drawing.Size(86, 25);
			this.rbDec.TabIndex = 1;
			this.rbDec.TabStop = true;
			this.rbDec.Tag = "10";
			this.rbDec.Text = "Decimal";
			this.rbDec.UseVisualStyleBackColor = true;
			// 
			// rbHex
			// 
			this.rbHex.AutoSize = true;
			this.rbHex.Location = new System.Drawing.Point(32, 49);
			this.rbHex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.rbHex.Name = "rbHex";
			this.rbHex.Size = new System.Drawing.Size(123, 25);
			this.rbHex.TabIndex = 0;
			this.rbHex.Tag = "16";
			this.rbHex.Text = "HexaDecimal";
			this.rbHex.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Blue;
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Location = new System.Drawing.Point(0, 103);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(62, 502);
			this.panel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Location = new System.Drawing.Point(61, 421);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(945, 82);
			this.panel2.TabIndex = 0;
			// 
			// lblErr
			// 
			this.lblErr.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lblErr.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblErr, 2);
			this.lblErr.ForeColor = System.Drawing.Color.Red;
			this.lblErr.Location = new System.Drawing.Point(477, 451);
			this.lblErr.Name = "lblErr";
			this.lblErr.Size = new System.Drawing.Size(52, 21);
			this.lblErr.TabIndex = 10;
			this.lblErr.Text = "label6";
			// 
			// lblBin
			// 
			this.lblBin.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBin.AutoSize = true;
			this.lblBin.ForeColor = System.Drawing.Color.Green;
			this.lblBin.Location = new System.Drawing.Point(254, 367);
			this.lblBin.Name = "lblBin";
			this.lblBin.Size = new System.Drawing.Size(52, 21);
			this.lblBin.TabIndex = 9;
			this.lblBin.Text = "label9";
			// 
			// lblOct
			// 
			this.lblOct.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblOct.AutoSize = true;
			this.lblOct.ForeColor = System.Drawing.Color.Green;
			this.lblOct.Location = new System.Drawing.Point(254, 283);
			this.lblOct.Name = "lblOct";
			this.lblOct.Size = new System.Drawing.Size(52, 21);
			this.lblOct.TabIndex = 8;
			this.lblOct.Text = "label8";
			// 
			// lblDec
			// 
			this.lblDec.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDec.AutoSize = true;
			this.lblDec.ForeColor = System.Drawing.Color.Green;
			this.lblDec.Location = new System.Drawing.Point(254, 199);
			this.lblDec.Name = "lblDec";
			this.lblDec.Size = new System.Drawing.Size(52, 21);
			this.lblDec.TabIndex = 7;
			this.lblDec.Text = "label7";
			// 
			// lblHex
			// 
			this.lblHex.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblHex.AutoSize = true;
			this.lblHex.ForeColor = System.Drawing.Color.Green;
			this.lblHex.Location = new System.Drawing.Point(254, 115);
			this.lblHex.Name = "lblHex";
			this.lblHex.Size = new System.Drawing.Size(52, 21);
			this.lblHex.TabIndex = 6;
			this.lblHex.Text = "label6";
			// 
			// tbInput
			// 
			this.tbInput.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbInput.HideSelection = false;
			this.tbInput.Location = new System.Drawing.Point(263, 28);
			this.tbInput.Name = "tbInput";
			this.tbInput.Size = new System.Drawing.Size(731, 28);
			this.tbInput.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(142, 367);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(106, 21);
			this.label5.TabIndex = 4;
			this.label5.Text = "Binary Value:";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(147, 283);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 21);
			this.label4.TabIndex = 3;
			this.label4.Text = "Octal Value:";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(131, 199);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(117, 21);
			this.label3.TabIndex = 2;
			this.label3.Text = "Decimal Value:";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(96, 115);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 21);
			this.label2.TabIndex = 1;
			this.label2.Text = "Hexadecimal Value:";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(192, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 21);
			this.label1.TabIndex = 0;
			this.label1.Text = "Input:";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.tbInput, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblHex, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblDec, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblOct, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblBin, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblErr, 0, 5);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 103);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1007, 504);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1005, 605);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Comic Sans MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "Form1";
			this.Text = "Numeric Converter";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbUseLong;
		private System.Windows.Forms.RadioButton rbBin;
		private System.Windows.Forms.RadioButton rbOct;
		private System.Windows.Forms.RadioButton rbDec;
		private System.Windows.Forms.RadioButton rbHex;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblErr;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbInput;
		private System.Windows.Forms.Label lblHex;
		private System.Windows.Forms.Label lblDec;
		private System.Windows.Forms.Label lblOct;
		private System.Windows.Forms.Label lblBin;
	}
}

