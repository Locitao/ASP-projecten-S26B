<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadItem.aspx.cs" Inherits="Mediasharing.UploadItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload File</title>
    <style type="text/css"></style>
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="Container">
        <table class="custom">
            <tr>
                <td>
                    <asp:Label ID="lblBrowse" runat="server" Text="Select file: "></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="fuUpload" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTitle" runat="server" Text="Title: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblContent" runat="server" Text="Content:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbContent" runat="server" CssClass="tbcontent" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" OnClick="btnUpload_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr>
                <td>;</td>
                <td>
                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
