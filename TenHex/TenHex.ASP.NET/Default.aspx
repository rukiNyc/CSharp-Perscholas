<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>A Hex-Dec-Oct-Bin Converter in ASP.NET</title>
	<style>
		html {
			background-color: navy;
			font-family: 'Comic Sans MS';
			font-weight: bold;
		}
		body {
			margin: 40px;
			padding: 20px;
			height: 80%;
			min-height: 260px;
			background-color: white;
		}
		.sep label {
			margin: 0 20px 0 0;
		}
		.ioTable {
			width:100%;
			margin-top: 20px;
		}
		.tdV {
			text-align:right;
			margin: 0 8px 0 0;
			width:25%;
			height:40px;
		}
		.tdO {
			color: green;
			width:75%;
		}
		.err {
			color: red;
			text-align:center;
		}
	</style>
	<script type="text/javascript">
		function setCursor() {
			var tb = document.getElementById('tbInput');
			tb.focus();
			tb.value = tb.value;
		}
		function submitForm() { document.forms[0].submit(); }
		function applyInput(e) {
			if (e) {
				// textbox input:
				if (e.keyCode >= 48 && e.keyCode <= 70) submitForm();
				if (e.keyCode == 13) submitForm();
				// radiobutton onclick:
				if (e.srcElement && e.srcElement.checked == true) {
					document.forms[0].submit();
				}
			}
		}
	</script>
</head>
<body onload="setCursor()">
	<form id="form1" runat="server">
		<div>
			<asp:Panel runat="server" ID="Panel1" GroupingText="Input Mode">
				<table>
					<tr>
						<td><asp:RadioButton ID="rbHex" runat="server" Text="HexaDecimal" GroupName="it" CssClass="sep" onclick="applyInput(event)" /></td>
						<td><asp:RadioButton ID="rbDec" runat="server" Text="Decimal" GroupName="it" Checked="true" CssClass="sep" onclick="applyInput(event)"/></td>
						<td><asp:RadioButton ID="rbOct" runat="server" Text="Octal" GroupName="it" CssClass="sep" onclick="applyInput(event)"/></td>
						<td><asp:RadioButton ID="rbBin" runat="server" Text="Binary" GroupName="it" CssClass="sep" onclick="applyInput(event)"/></td>
						<td><asp:CheckBox ID="cbLong" runat="server" Text="Use Long Integers" CssClass="sep" onclick="applyInput(event)"/></td>
					</tr>
				</table>
			</asp:Panel>
				<table class="ioTable">
					<tr>
						<td class="tdV"><asp:Label ID="lblInput" runat="server" /></td>
						<td class="tdO"><asp:TextBox ID="tbInput" runat="server" Text="1" onkeyup="applyInput(event)" AutoCompleteType="None" style="width:90%" /></td>
					</tr>
					<tr>
						<td class="tdV">Hexadecimal Value:</td>
						<td class="tdO"><asp:Label ID="lblHex" runat="server" /></td>
					</tr>
					<tr>
						<td class="tdV">Decimal Value:</td>
						<td class="tdO"><asp:Label ID="lblDec" runat="server" /></td>
					</tr>
					<tr>
						<td class="tdV">Octal Value:</td>
						<td class="tdO"><asp:Label ID="lblOct" runat="server" /></td>
					</tr>
					<tr>
						<td class="tdV">Binary Value:</td>
						<td class="tdO"><asp:Label ID="lblBin" runat="server" /></td>
					</tr>
					<tr>
						<td colspan="2" class="err"><asp:label ID="errMsg" runat="server"/></td>
					</tr>
				</table>
		</div>
	</form>
</body>
</html>
