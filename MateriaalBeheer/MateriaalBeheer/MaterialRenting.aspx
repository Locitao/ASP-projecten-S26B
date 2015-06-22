<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialRenting.aspx.cs" Inherits="MaterialRenting.MaterialRenting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Material Renting</title>
    <link rel="stylesheet" href="MaterialRenting.css"/>
    <link rel="stylesheet" href="Huisstijl.css"/>
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

</head>
<body>
<form id="form1" runat="server">

    <asp:Panel runat="server" ID="pnlMain">
        <div>
            From: <asp:TextBox ID="tbStartDate" runat="server" TextMode="Date"/><br/>
            To: <asp:TextBox ID="tbEndDate" runat="server" TextMode="Date"/><br/>
            <asp:Button runat="server" CssClass="button" ID="btnRefresh" Text="Refresh" OnClick="btnRefresh_OnClick"/><br/>
            <asp:ListBox ID="lbProducts" runat="server" Height="149px" Width="400px"></asp:ListBox><br/>
        </div>


            <asp:Button ID="btnLendProduct" CssClass="button" runat="server" Text="Lend Item" OnClick="btnLendProduct_Click"/>
            <asp:Button ID="btnReserveProduct" CssClass="button" runat="server" Text="Reserve Item" OnClick="btnReserveProduct_OnClick"/>
            <asp:Button ID="btnNewItem" CssClass="button" runat="server" Text="Add Item" OnClick="btnNewItem_OnClick"/>
    </asp:Panel>

    <div>
        <asp:Panel ID="pnlPopUpLendItem" runat="server">
            <asp:Label id="lblLendItem" runat="server" text="Name: Name <br />price: 25<br/>status: reserved"/>

            <table style="border: hidden;">
                <tr style="border: hidden;">
                    <td>BarCode:</td>
                    <td>
                        <asp:TextBox ID="tbLendBarcode" runat="server">0123456789</asp:TextBox>
                    </td>
                </tr>
                <tr style="border: hidden;">
                    <td>Return Date:</td>
                    <td>
                        <asp:TextBox ID="tbLendReturnDate" runat="server" TextMode="Date"/>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnCheckStatus" CssClass="button" runat="server" Text="Check Status" OnClick="btnCheckStatus_OnClick"/>
            <asp:Button ID="btnLendPopUpSave" CssClass="button" runat="server" Text="Lend Item" OnClick="btnLendPopUpSave_OnClick"/>
            <asp:Button ID="btnLendCancel" CssClass="button" runat="server" Text="Cancel" OnClick="btnLendCancel_OnClick"/>


        </asp:Panel>
    </div>

    <div>
        <asp:Panel ID="pnlPopUpReserveItem" runat="server">
            <asp:Label id="lblPopUpReserveItem" runat="server" text="Name: Name <br />price: 25<br/>status: reserved"/>

            <table style="border: hidden;">
                <tr style="border: hidden;">
                    <td>BarCode:</td>
                    <td>
                        <asp:TextBox ID="tbReserveBarcode" runat="server">0123456789</asp:TextBox>
                    </td>
                </tr>
                <tr style="border: hidden;">
                    <td>Return Date:</td>
                    <td>
                        <asp:TextBox ID="tbReserveLendDate" runat="server" TextMode="Date"/>
                    </td>
                </tr>
                <tr style="border: hidden;">
                    <td>Return Date:</td>
                    <td>
                        <asp:TextBox ID="tbReserveReturnDate" runat="server" TextMode="Date"/>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnReserveCheckStatus" CssClass="button" runat="server" Text="Check Status" OnClick="btnReserveCheckStatus_OnClick"/>
            <asp:Button ID="btnReserveSave" CssClass="button" runat="server" Text="Reserve Item" OnClick="btnReserveSave_OnClick"/>
            <asp:Button ID="btnReserveCancel" CssClass="button" runat="server" Text="Cancel" OnClick="btnReserveCancel_OnClick"/>
        </asp:Panel>
    </div>
    
    <div>
        <asp:Panel ID="pnlPopUpAddProduct" runat="server" >
            <asp:DropDownList ID="ddlProducts" runat="server" OnLoad="ddlProducts_OnLoad"/>
            <asp:Button ID="btnAddProduct" CssClass="button" runat="server" Text="Add Product" OnClick="btnAddProduct_OnClick"/>
        </asp:Panel>
    </div>

</form>

</body>
</html>