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
            <asp:Label ID="testLabel" runat="server" Text="Label"></asp:Label><br/>
            <asp:Button ID="TestButton" CssClass="button" runat="server" Text="Test Connection" OnClick="TestButton_Click" />
        </div>
        
    </form>
</body>
</html>
