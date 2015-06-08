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
        <asp:Button ID="btnLeenUit" CssClass="button" runat="server" Text="Leen item uit"/><br/>
        <asp:Button ID="BtnReserveer" CssClass="button" runat="server" Text="Reserveer item"/><br/>
        <asp:Button ID="BtRetourneer" CssClass="button" runat="server" Text="Item retourneren"/><br/>
        <asp:Button ID="BtnVeranderCategorie" CssClass="button" runat="server" Text="Verander categorie"/><br/>
        <asp:Button ID="BtnNieuwItem" CssClass="button" runat="server" Text="Voeg Item toe"/><br/>
        <asp:Button ID="BtnNieuweCategorie" CssClass="button" runat="server" Text="Voeg categorie toe"/><br/>
    </div>
    </form>
</body>
</html>
