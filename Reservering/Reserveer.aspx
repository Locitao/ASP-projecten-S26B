<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reserveer.aspx.cs" Inherits="Reserveer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reserveren</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
    <form id="reserveForm" runat="server">
        <div id="container">
            <h1>Social Media Event</h1>
            <h3>Reserveren</h3>
            <div class="myForm">
                <p class="tekstinput">Voornaam:</p><br/>
                <p class="tekstinput">Tussenvoegsel:</p><br/>
                <p class="tekstinput">Achternaam:</p><br />
                <p class="tekstinput">Straat:</p><br />
                <p class="tekstinput">Huisnummer:</p><br/>
                <p class="tekstinput">Woonplaats:</p><br/>
                <p class="tekstinput">Bankrekening nummer:</p><br/>
            </div>
            <div class="myForm">
                <asp:TextBox ID="tbVoornaam" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbTussenvoegsel" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbAchternaam" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbStraat" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbHuisnummer" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbWoonplaats" CssClass="myForm" runat="server"></asp:TextBox><br/>
                <asp:TextBox ID="tbBankrekening" CssClass="myForm" runat="server" MaxLength="10"></asp:TextBox>
            </div>
            <br/><asp:Button runat="server" ID="btnSubmitReserve" Text="Create Reservation" CssClass="button" OnClick="btnSubmitReserve_Click"/>
        </div>
    </form>
</body>
</html>
