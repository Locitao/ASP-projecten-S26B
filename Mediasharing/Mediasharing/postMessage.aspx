<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostMessage.aspx.cs" Inherits="Mediasharing.PostMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Post a message</title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css"></style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!--Container DIV-->
        <div id="Container">
            
            <table class="custom">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Content"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbContent" runat="server" CssClass="tbcontent" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnPost" runat="server" CssClass="button" OnClick="btnPost_Click" Text="Post" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            
        </div>
    
    </div>
    </form>
</body>
</html>
