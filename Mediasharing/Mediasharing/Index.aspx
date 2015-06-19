<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mediasharing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing Applicatie</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css"></style>
</head>
<body>
    <form id="form1" runat="server">
        <!--Container DIV-->
    <div id="Container">
        
        <!--Banner DIV-->        
        <div id="Banner">
            <h1>Mediasharing application!</h1>
            <h2>ICT4EVENTS</h2>
        </div>
        
        <!--BerichtenBox DIV-->        
        <div id="MessageBox">
            <h3>Messages</h3>
            <asp:ListBox ID="lbMessages" runat="server" AutoPostBack="True" CssClass="listbox1" OnSelectedIndexChanged="lbMessages_SelectedIndexChanged"></asp:ListBox>
            <br />
            <asp:Label ID="lblMessageLikes" runat="server"></asp:Label>
            <br />
            <table class="custom">
                <tr>
                    <td>
                        <asp:Button ID="btnLikeMessage" runat="server" CssClass="button" Text="Like" OnClick="btnLikeMessage_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnReportMessage" runat="server" CssClass="button" Text="Report" OnClick="btnReportMessage_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <h3>Reactions</h3>
            <asp:ListBox ID="lbReactions" runat="server" CssClass="listbox1" AutoPostBack="True" OnSelectedIndexChanged="lbReactions_SelectedIndexChanged"></asp:ListBox>
            <br />
            <asp:Label ID="lblReactionLikes" runat="server"></asp:Label>
            <br />
            <table class="custom">
                <tr>
                    <td>
                        <asp:Button ID="btnLikeReaction" runat="server" CssClass="button" Text="Like" OnClick="btnLikeReaction_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnReportReaction" runat="server" CssClass="button" Text="Report" OnClick="btnReportReaction_Click" />
                    </td>
                </tr>
            </table>
        </div>

        <!--Categorie DIV-->

        <div id="Categories">
            <asp:Label ID="lblCategorie" runat="server" Text="Categorie"></asp:Label>
            <asp:Repeater ID="RepeaterCategories" runat="server">
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
        <div id="SubCategories">
            <asp:Label ID="lblSubCategorie" runat="server" Text="Sub Categorie"></asp:Label>
            <asp:Repeater ID="RepeaterSubCategories" runat="server">
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
            <asp:Repeater ID="RepeaterMediaItems" runat="server">
                <ItemTemplate>
                        <table class = "custom">
                            <tr>
                                <td>
                                    <a href="/Item/<%# Eval("ID") %>">
                                        <asp:Label runat="server" Text='<%# Eval("BESTANDSLOCATIE") %>'>
                                        </asp:Label>
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
            </asp:Repeater>
        </div>
        
        <!--ItemView DIV-->
        <div id="ItemView">
            <asp:Label ID="lblItemView" runat="server" Text="Item View"></asp:Label>
            <asp:Repeater ID="RepeaterItemView" runat="server">
            </asp:Repeater>
        </div>       
    </div>
    </form>
</body>
</html>
