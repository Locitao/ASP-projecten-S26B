<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReservation.aspx.cs" Inherits="ReservationSystem.NewReservation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reservering</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
    <form id="reservering" runat="server">
    <div id="container">
        <h1>Create a reservation.</h1>
        <div class="myForm">
            <p class="tekstinput">Location ID you want to reserve:</p><br/>
            <p class="tekstinput">How many people:</p><br/>
        </div>
        <div class="myForm">
            <asp:TextBox ID="tbLocation" CssClass="myForm" runat="server" TextMode="Number" MaxLength="3"></asp:TextBox><br/>
            <asp:TextBox ID="tbPeople" CssClass="myForm" runat="server" TextMode="Number"></asp:TextBox><br/>
        </div>
        <asp:Button runat="server" ID="btnReserve" Text="Create reservation." CssClass="button" OnClick="btnReserve_Click"/><br/>
        <p>List of locations that are free; their IDs correspond to the ones on the map.</p><br />
        <asp:ListBox ID="lbLocations" runat="server" Height="116px" Width="179px"></asp:ListBox><br/>
        <a href="http://imgur.com/H5y3VBq"><img src="http://i.imgur.com/H5y3VBq.jpg" title="source: imgur.com" id="plattegrond"/></a>
        <div id="reservation">
            <div class="myForm">
                <p class="tekstinput"></p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
