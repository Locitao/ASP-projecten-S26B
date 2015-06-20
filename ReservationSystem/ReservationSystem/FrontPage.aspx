<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrontPage.aspx.cs" Inherits="ReservationSystem.Voorpagina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voorpagina</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
<form id="bigForm" runat="server">
    <div id="container">
        <h1>Social Media Sharing Event</h1>
        <h3>ICT4Events</h3>

        <p>This social media event brings people together through the use of Social Media.</p><br/>

        <asp:Button ID="btnReserve" CssClass="button" runat="server" Text="Create a reservation" OnClick="btnReserve_Click"/>
        <asp:Button runat="server" CssClass="button" ID="btnAcc" Text="Create an account" OnClick="btnAcc_Click"/>
        <p>If one of your friends has already reserved for you, you only need to create an account.</p>

    </div>

</form>
</body>
</html>