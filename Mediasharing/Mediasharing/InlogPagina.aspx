<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InlogPagina.aspx.cs" Inherits="Mediasharing.InlogPagina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing applicatie</title>
    <link rel="stylesheet" href="StyleSheet.css" />
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Label ID="lblGegevens" runat="server" Text="Vul hieronder uw gegevens in."></asp:Label>
        <br />
        <table class="auto-style1">
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="lblId" runat="server" Text="Id:"></asp:Label>
                </td>
                <td id="tblInlog" class="auto-style1">
                    <asp:TextBox ID="tbId" runat="server" Height="17px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="lblWachtwoord" runat="server" Text="Wachtwoord:" Visible="False"></asp:Label>
                </td>
                <td id="tblInlog">
                    <asp:TextBox ID="tbWachtwoord" runat="server" Visible="False" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnInloggen" runat="server" Text="Inloggen" OnClick="btnInloggen_Click" />
        <br />
    
    </div>
    </form>
</body>
</html>
