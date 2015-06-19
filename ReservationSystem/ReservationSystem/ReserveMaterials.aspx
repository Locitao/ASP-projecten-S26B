<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReserveMaterials.aspx.cs" Inherits="ReservationSystem.ReserveMaterials" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reserve materials</title>
    <link rel="stylesheet" href="style.css"/>
</head>
<body>
    <form id="materialsform" runat="server">
    <div id="materials">
        <p>Enter the ID's of the items you wish to reserve. Not necessary to complete the reservation.</p><br/>
        <div class="myForm">
            <p class="tekstinput">ID of material:</p><br />
            <p class="tekstinput">ID of 2nd material:</p><br/>
            <p class="tekstinput">ID of 3d material:</p><br/>
            <p class="tekstinput">Start date of rent(format: dd/mm/yyyy):</p><br/>
            <p class="tekstinput">End date of rent(format: dd/mm/yyyy):</p>
        </div>
        <div class="myForm">
            <asp:TextBox ID="tbMatOne" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbMatTwo" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbMatThree" CssClass="myForm" runat="server"></asp:TextBox><br/>
            <asp:TextBox ID="tbStartDate" runat="server" TextMode="Date"></asp:TextBox><br/>
            <asp:TextBox ID="tbEndDate" runat="server" TextMode="Date"></asp:TextBox>
        </div>
        <asp:Button runat="server" CssClass="button" Text="Complete Reservation"/><br/>
        <asp:ListBox ID="lbMaterials" runat="server" Height="176px" Width="290px"></asp:ListBox><br/>
    </div>
    </form>
</body>
</html>
