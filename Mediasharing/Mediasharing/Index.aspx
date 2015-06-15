<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mediasharing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing Applicatie</title>
    <link href="StyleSheet.css" rel="stylesheet" />
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

        <!--Categorie DIV-->

        <div id="Categorie">
            <asp:Label ID="lblCategorie" runat="server" Text="Categorie"></asp:Label>
            <asp:Repeater ID="RepeaterCategorie" runat="server">
                 <ItemTemplate>
                        <table>
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
                        <table>
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
            <asp:Repeater ID="MediaItems" runat="server"></asp:Repeater>
        </div>
        
        <!--Item DIV-->
        <div id="Item">

        </div>
    </div>
    </form>
</body>
</html>
