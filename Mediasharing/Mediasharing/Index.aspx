<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mediasharing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing Applicatie</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 66px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!--Container DIV-->
    <div id="Container">
        
        <!--Banner DIV-->        
        <div id="Banner">
            <h1>Mediasharing applicatie!</h1>
            <h2>ICT4EVENTS</h2>
        </div>
        
        <!--BerichtenBox DIV-->        
        <div id="BerichtenBox">
            <h3>Berichten</h3>
            <asp:ListBox ID="lbMessages" runat="server"></asp:ListBox>
            <br />
            <table class="custom">
                <tr>
                    <td>
                        <asp:Button ID="btnLike" runat="server" CssClass="button" Text="Like" />
                    </td>
                    <td>
                        <asp:Button ID="btnReport" runat="server" CssClass="button" Text="Report" />
                    </td>
                </tr>
            </table>
            <br />
            <h3>Reacties</h3>
            <asp:ListBox ID="lbReactions" runat="server"></asp:ListBox>
        </div>

        <!--Categorie DIV-->

        <div id="Categorie">
            <asp:Label ID="lblCategorie" runat="server" Text="Categorie"></asp:Label>
            <asp:Repeater ID="RepeaterCategorie" runat="server">
                 <ItemTemplate>
                        <table class ="custom">
                            <tr>
                                <td>
                                    <a href="/Index/<%# Eval("ID") %>">
                                        <asp:Label runat="server" Text='<%# Eval("NAAM") %>'>
                                        </asp:Label>
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
            </asp:Repeater>
        </div>
        
        <!--Subcategorie DIV-->
        <div id="SubCategorie">
            <asp:Label ID="lblSubCategorie" runat="server" Text="Sub Categorie"></asp:Label>
            <asp:Repeater ID="RepeaterSubCategorie" runat="server">
                 <ItemTemplate>
                        <table class = "custom">
                            <tr>
                                <td>
                                    <a href="/Index/<%# Eval("ID") %>">
                                        <asp:Label runat="server" Text='<%# Eval("NAAM") %>'>
                                        </asp:Label>
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
            </asp:Repeater>
        </div>
        
        <!--Mediaitem DIV-->
        <div id="MediaItems">
            <asp:Label ID="lblMediaItems" runat="server" Text="Media Items"></asp:Label>
            <asp:Repeater ID="MediaItems" runat="server"></asp:Repeater>
        </div>
        
    </div>
    </form>
</body>
</html>
