<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InlogPagina.aspx.cs" Inherits="Mediasharing.InlogPagina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing applicatie</title>
    <link rel="stylesheet" href="StyleSheet.css" />
    <style type="text/css">
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <asp:Label ID="lblGegevens" runat="server" Text="Vul hieronder uw gegevens in."></asp:Label>
        <br />
        <table class="custom">
            <tr>
                <td>
                    <asp:Label ID="lblId" runat="server" Text="Id:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbId" runat="server" Height="17px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblWachtwoord" runat="server" Text="Wachtwoord:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbWachtwoord" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnInloggen" runat="server" Text="Inloggen" OnClick="btnInloggen_Click" CssClass="button" Width="100px" />
        <br />
    
    </div>
    </form>
</body>
</html>
