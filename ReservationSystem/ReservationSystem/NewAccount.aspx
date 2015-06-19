<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewAccount.aspx.cs" Inherits="ReservationSystem.NewAccount" %>

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
        <h3>Create your account.</h3>
        <div class="myForm">
            <p class="tekstinput">Name:</p><br/>
            <p class="tekstinput">Addition:</p><br/>
            <p class="tekstinput">Surname:</p><br/>
            <p class="tekstinput">Street:</p><br/>
            <p class="tekstinput">House number:</p><br/>
            <p class="tekstinput">City:</p><br/>
            <p class="tekstinput">Bank account:</p><br/>
            <p class="tekstinput">Email:</p><br/>
            <p class="tekstinput">Username for event:</p><br/>
            <p class="tekstinput">Password for account:</p><br/>
        </div>
        <div class="myForm">
            <asp:TextBox ID="tbName" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbAddition" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbSurname" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbStreet" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbHouseNumber" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbCity" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbBankAccount" CssClass="myForm" runat="server" MaxLength="10"></asp:TextBox><br/>
            <asp:TextBox ID="tbEmail" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbUsername" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbPassword" CssClass="myForm" runat="server" TextMode="Password"></asp:TextBox><br/>
        </div>
        <br/><asp:Button runat="server" ID="btnSubmitReserve" Text="Create Account" CssClass="button" OnClick="btnSubmitReserve_Click"/>
    </div>
</form>
</body>
</html>