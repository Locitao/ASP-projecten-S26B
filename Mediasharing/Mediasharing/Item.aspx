﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="Mediasharing.Item" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Media Item</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css"></style>
</head>
<body>
    <form id="form1" runat="server">
        <!--Container DIV-->
    <div id ="Container">
        <!--ItemView DIV-->
        <div id ="ItemView">
            <asp:Image ID="uploadedImage" runat="server" />
            <asp:Repeater ID="RepeaterItemView" runat="server">
                <ItemTemplate>
                        <table class = "custom">
                            <tr>
                                <td><asp:Label runat="server" Text="Posted by: "> </asp:Label>
                                </td>
                                <td>
                                <asp:Label runat="server" Text='<%# Eval("GEBRUIKERSNAAM") %>'> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Label runat="server" Text="Title: "> </asp:Label>
                                </td>
                                <td>
                                <asp:Label runat="server" Text='<%# Eval("TITEL") %>'> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:Label runat="server" Text="Content: "> </asp:Label>
                                </td>
                                <td>
                                <asp:Label runat="server" Text='<%# Eval("INHOUD") %>'> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
            </asp:Repeater>
        </div>    
        
        <!--MessageView DIV-->
        <div id="MessageView">
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
            <table class="auto-style1">
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

    </div>
    </form>
</body>
</html>
