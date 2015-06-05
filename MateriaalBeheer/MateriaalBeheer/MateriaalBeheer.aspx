<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MateriaalBeheer.aspx.cs" Inherits="MateriaalBeheer.MateriaalBeheer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Materiaal Beheer</title>
    <link rel="stylesheet" href="Huisstijl.css" />
    <link rel="stylesheet" href="MateriaalBeheer.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<asp:ListBox ID="ListBox1" runat="server" Height="149px" style="margin-top: 0px" Width="212px"></asp:ListBox><br/>
        <input type="checkbox" name="option1" value="Uitgeleend" checked/> Uitgeleend <br/>
        <input type="checkbox" name="option2" value="Gereserveerd" checked/> Gereserveerd<br/>
        <input type="checkbox" name="option3" value="Vrij" checked/> Vrij <br/>
    </div>
    <div>
        <asp:Button ID="btn1" CssClass="button" runat="server" Text="button1"/><br/>
        <asp:Button ID="Button1" CssClass="button" runat="server" Text="button1"/><br/>
        <asp:Button ID="Button2" CssClass="button" runat="server" Text="button1"/><br/>
        <asp:Button ID="Button3" CssClass="button" runat="server" Text="button1"/><br/>
        <asp:Button ID="Button4" CssClass="button" runat="server" Text="button1"/><br/>
        <asp:Button ID="Button5" CssClass="button" runat="server" Text="button1"/><br/>
    </div>
    </form>
</body>
</html>
