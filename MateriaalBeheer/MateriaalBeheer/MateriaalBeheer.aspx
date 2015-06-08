<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MateriaalBeheer.aspx.cs" Inherits="MateriaalBeheer.MateriaalBeheer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Materiaal Beheer</title>
    <link rel="stylesheet" href="Huisstijl.css" />
    <link rel="stylesheet" href="MateriaalBeheer.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        
    <asp:Panel runat="server" ID="pnlMain">
    <div>
        <asp:ListBox ID="listBox" runat="server" Height="149px" style="margin-top: 0px" Width="212px"></asp:ListBox><br/>
        <input type="checkbox" name="option1" value="Uitgeleend" checked="checked"/> Uitgeleend <br/>
        <input type="checkbox" name="option2" value="Gereserveerd" checked="checked"/> Gereserveerd<br/>
        <input type="checkbox" name="option3" value="Vrij" checked="checked"/> Vrij 
    </div>
        
     
    <div>
        <asp:Button ID="btnLeenUit" CssClass="button" runat="server" Text="Leen item uit" OnClick="btnLeenUit_Click"/>
        <asp:Button ID="BtnReserveer" CssClass="button" runat="server" Text="Reserveer item"/>
        <asp:Button ID="BtRetourneer" CssClass="button" runat="server" Text="Item retourneren"/>
        <asp:Button ID="BtnVeranderCategorie" CssClass="button" runat="server" Text="Verander categorie"/>
        <asp:Button ID="BtnNieuwItem" CssClass="button" runat="server" Text="Voeg Item toe" />
        <asp:Button ID="BtnNieuweCategorie" CssClass="button" runat="server" Text="Voeg categorie toe"/>
    </div>
    </asp:Panel>
    <div>
        <asp:Panel ID="pnlPopUpLeenItem" runat="server">
            <p id="LendItemPar">Name: haha <br />price: 25<br/>status: lend</p>
            <br />RFID Code: 
        <asp:TextBox ID="tbRFID" runat="server" >hahaha</asp:TextBox>
            <br/> return date:
        <asp:TextBox ID="tbDateReturn" runat="server" >21/09/2016</asp:TextBox>
        <asp:Button ID="Button1" CssClass="button" runat="server" Text="Leen item uit" OnClick="btnLeenUitPopUp_Click"/>
        </asp:Panel>
    </div>

    </form>

</body>
</html>
