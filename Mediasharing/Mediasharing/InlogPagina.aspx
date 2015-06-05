<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InlogPagina.aspx.cs" Inherits="Mediasharing.InlogPagina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing applicatie</title>
    <link rel="stylesheet" href="StyleSheet.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:RadioButtonList ID="rbGebruiker" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
            <asp:ListItem Selected="True">Gebruiker</asp:ListItem>
            <asp:ListItem>Administrator</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <asp:Label ID="lblGegevens" runat="server" Text="Vul hieronder uw gegevens in."></asp:Label>
        <br />
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lblGebruikersnaamOrRfid" runat="server" Text="RFIDcode:"></asp:Label>
                </td>
                <td id="tblInlog">
                    <asp:TextBox ID="tbRfidCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lblWachtwoord" runat="server" Text="Wachtwoord:" Visible="False"></asp:Label>
                </td>
                <td id="tblInlog">
                    <asp:TextBox ID="tbWachtwoord" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnInloggen" runat="server" Text="Inloggen" />
        <br />
    
    </div>
    </form>
</body>
</html>
