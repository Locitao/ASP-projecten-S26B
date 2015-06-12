<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Voorpagina.aspx.cs" Inherits="Voorpagina" %>

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
            
            <p>Het social media event brengt mensen samen m.b.v. verscheidene Social Media.</p><br/>

            <asp:Button ID="btnReserve" CssClass="button" runat="server" Text="Reserveer" OnClick="btnReserve_Click" />
            
        </div>
        
        <asp:Label ID="testLabel" runat="server" Text=""></asp:Label>
        
    </form>
</body>
</html>
