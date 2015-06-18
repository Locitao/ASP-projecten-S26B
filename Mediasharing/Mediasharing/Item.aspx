<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="Mediasharing.Item" %>

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
            <asp:Label ID="lblItemMessages" runat="server" Text="Comments:"></asp:Label>
            <br/>
            <asp:ListBox ID="lbItemMessages" runat="server" CssClass="listbox2" OnSelectedIndexChanged="lbItemMessages_SelectedIndexChanged"></asp:ListBox>
            <br/>

            <asp:Label ID="lblReplies" runat="server" Text="Reactions:"></asp:Label>
            <br/>
            <asp:ListBox ID="lbReactions" runat="server" CssClass="listbox2"></asp:ListBox>
            <br/>
        </div>

    </div>
    </form>
</body>
</html>
