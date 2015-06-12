<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mediasharing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <!--Container DIV-->
    <div id="Container">
        
        <!--Banner DIV-->        
        <div id="Banner">
            
        </div>

        <!--Categorie DIV-->

        <div id="Categorie">
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
            <asp:Repeater ID="SubCategorie" runat="server"></asp:Repeater>
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
