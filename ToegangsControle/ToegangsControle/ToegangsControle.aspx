﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToegangsControle.aspx.cs" Inherits="ToegangsControle.ToegangsControle" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="./stylesheet.css" type="text/css"/>
    <title></title>
</head>
<body>
<form id="Form" runat="server">
    <!-- Start content webpage -->
    <div class="content">
        <h1> Access Control</h1>

        <!-- start textbox -->
        <asp:TextBox class="textbox" ID="tbBarcode" runat="server" AutoPostBack="true" OnTextChanged="tbBarcode_TextChanged" Width="98%" MaxLength="13">Scan Code</asp:TextBox>
        <!-- end textbox -->
        <br/> <br/>

        <!-- start listbox -->
        <asp:ListBox class="listbox" ID="lbContent" runat="server" Width="100%" Height="70%" ViewStateMode="Enabled"></asp:ListBox>
        <!-- end listbox -->
        <br/> <br/>

        <!-- start buttons -->
        <asp:Button class="button" ID="bttnPaid" runat="server" Text="Toggle paid" OnClick="bttnPaid_Click"/>
        <asp:Button class="button" ID="bttnCancel" runat="server" Text="Cancel reservation" OnClick="bttnCancel_Click"/>
        <asp:Button class="button" ID="bttnPresent" runat="server" Text="List all attendees" OnClick="bttnPresent_Click"/>
        <asp:Button class="button1" ID="bttnRefresh" runat="server" Text="Refresh" OnClick="bttnRefresh_Click"/>
        <!-- end buttons -->
    </div>
    <!-- end content webpage -->
</form>
</body>
</html>