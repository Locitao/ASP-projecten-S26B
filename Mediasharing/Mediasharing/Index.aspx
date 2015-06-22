<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Mediasharing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mediasharing Applicatie</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css">
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Container">
        
            <div id="Banner">
                <h1>Mediasharing application!</h1>
                <h1>ICT4EVENTS</h1>
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
                        <asp:Button ID="btnLikeMessage" runat="server" CssClass="buttondisabled" Text="Like" Enabled="False" OnClick="btnLikeMessage_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnReportMessage" runat="server" CssClass="buttondisabled" Text="Report" Enabled="False" OnClick="btnReportMessage_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <table class="custom">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Title: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Content: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnReaction" runat="server" CssClass="button" OnClick="btnReaction_Click" Text="Post Reaction" />
                    </td>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="error" Visible="False"></asp:Label>
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
                        <asp:Button ID="btnLikeReaction" runat="server" CssClass="buttondisabled" Text="Like" Enabled="False" OnClick="btnLikeReaction_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnReportReaction" runat="server" CssClass="buttondisabled" Text="Report" Enabled="False" OnClick="btnReportReaction_Click" />
                    </td>
                </tr>
            </table>
        </div>
        
        <!--SearchBox DIV-->
        <div id="SearchBox">
            <asp:Label ID="lblSearch" runat="server" Text="Search" CssClass="lblsearch"></asp:Label>
            <asp:TextBox ID="tbSearch" runat="server" CssClass="tbsearch"></asp:TextBox><asp:DropDownList ID="ddlSearch" runat="server" CssClass="ddlsearch">
                <asp:ListItem>Category</asp:ListItem>
                <asp:ListItem>Media Item</asp:ListItem>
            </asp:DropDownList><asp:Button ID="btnSearch" runat="server" Text="Go!" CssClass="buttonsearch" OnClick="btnSearch_Click" />
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
            <asp:Button ID="btnRoot" runat="server" CssClass="button" OnClick="btnRoot_Click" Text="Starting Category" />
            <asp:Button ID="btnReportCategory" runat="server" CssClass="button" Text="Report Category" OnClick="btnReportCategory_Click"/>
            <asp:Button ID="btnAddPost" runat="server" CssClass="addbutton" Text="Add Message" OnClick="btnAddPost_Click" />
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
            <asp:TextBox ID="tbCategoryName" runat="server" CssClass="addbox"></asp:TextBox>
            <br />
            <asp:Button ID="btnAddCategory" runat="server" CssClass="addbutton" Text="Create category" OnClick="btnAddCategory_Click" />
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
            <br />
            <asp:Button ID="btnUploadItem" runat="server" CssClass="addbutton" Text="Upload Item" OnClick="btnUploadItem_Click" />
        </div>
            
        <!--SearchView DIV-->
            <div id="SearchView">
                 <asp:Label ID="lblResults" runat="server" Text="Search Results:"></asp:Label>
                <asp:Repeater ID="RepeaterSearchCategories" runat="server">
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

                <asp:Repeater ID="RepeaterSearchMediaItems" runat="server">
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
    </div>
    </form>
</body>
</html>
