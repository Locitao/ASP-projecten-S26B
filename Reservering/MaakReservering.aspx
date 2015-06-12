<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MaakReservering.aspx.cs" Inherits="MaakReservering" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reservering</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
    <form id="reservering" runat="server">
    <div id="container">
        <h1>Maak een account aan</h1>
        <div class="myForm">
            <p class="tekstinput">Email:</p><br/>
            <p class="tekstinput">Gebruikersnaam:</p><br/>
        </div>
        <div class="myForm">
            <asp:TextBox ID="tbEmail" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbGebruikersnaam" CssClass="myForm" runat="server"></asp:TextBox><br/>
        </div>
        <asp:Button runat="server" ID="btnCreateAccount" Text="Create account!" CssClass="button"/>
        <a href="http://imgur.com/H5y3VBq"><img src="http://i.imgur.com/H5y3VBq.jpg" title="source: imgur.com" id="plattegrond"/></a>
    </div>
    </form>
</body>
</html>
