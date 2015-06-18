<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToegangsControle.aspx.cs" Inherits="ToegangsControle.ToegangsControle" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="./stylesheet.css" type="text/css" />
    <title></title>
</head>
<body>
    <form id="Form" runat="server">
        <!-- Start content webpage -->
        <div class="inhoud">
            <h1> ToegangsControle </h1>

            <!-- start textbox -->
            <asp:TextBox class="textbox" ID="tbBarcode" runat="server" AutoPostBack="true" OnTextChanged="tbBarcode_TextChanged" Width="98%" MaxLength="13">Scan Code</asp:TextBox>
            <!-- end textbox -->
            <br /> <br />

            <!-- start listbox -->
            <asp:ListBox class="listbox" ID="lbGegevens" runat="server" Width="100%" Height="70%" ViewStateMode="Enabled"></asp:ListBox>
            <!-- end listbox -->
            <br /> <br />

            <!-- start buttons -->
            <asp:Button class="button" ID="bttnBetaald" runat="server" Text="Betaald" OnClick="bttnBetaald_Click" />
            <asp:Button class="button" ID="bttnAnuleren" runat="server" Text="Anuleren reservering" OnClick="bttnAnuleren_Click" />
            <asp:Button class="button" ID="bttnAanwezig" runat="server" Text="Alle aanwezigen" OnClick="bttnAanwezig_Click" />
            <asp:Button class="button1" ID="bttnRefresh" runat="server" Text="Refresh" OnClick="bttnRefresh_Click" />
            <!-- end buttons -->
        </div>
        <!-- end content webpage -->
    </form>
</body>
</html>
