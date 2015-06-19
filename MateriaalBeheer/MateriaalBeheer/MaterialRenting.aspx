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


        <div>
            <asp:Button ID="btnLendProduct" CssClass="button" runat="server" Text="Lend Item" OnClick="btnLendProduct_Click"/>
            <asp:Button ID="btnReserveProduct" CssClass="button" runat="server" Text="Reserve Item" OnClick="btnReserveProduct_OnClick"/>
            <asp:Button ID="btnReturnProduct" CssClass="button" runat="server" Text="Return Item" OnClick="btnReturnProduct_OnClick"/>
            <asp:Button ID="btChangeCategory" CssClass="button" runat="server" Text="Change Category"/>
            <asp:Button ID="btnNewItem" CssClass="button" runat="server" Text="Add Item"/>
            <asp:Button ID="btNewCategory" CssClass="button" runat="server" Text="Add Category"/>
        </div>
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
                        <asp:TextBox ID="tbLendReturnDate" runat="server">01-01-2016</asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnCheckStatus" CssClass="button" runat="server" Text="Check Status" OnClick="btnCheckStatus_OnClick"/>
            <asp:Button ID="btnLendPopUp" CssClass="button" runat="server" Text="Lend Item" OnClick="btnLendPopUp_OnClick"/>


        </asp:Panel>
    </div>

    <div>
        <asp:Panel ID="pnlPopUpReserveItem" runat="server">
            <asp:Label id="lblReserveItem" runat="server" text="Name: Name <br />price: 25<br/>status: reserved"/>
            <table style="border: hidden;">
                <tr style="border: hidden;">
                    <td>BarCode:</td>
                    <td>
                        <asp:TextBox ID="tbReserveBarcode" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr style="border: hidden;">
                    <td>Lend Date:</td>
                    <td>
                        <asp:TextBox ID="tbReserveLendDate" runat="server">21-09-2016</asp:TextBox>
                    </td>
                </tr>
                <tr style="border: hidden;">
                    <td>Return Date:</td>
                    <td>
                        <asp:TextBox ID="tbReserveReturnDate" runat="server">22-09-2016</asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnReservePopUp" CssClass="button" runat="server" Text="Reserve Item" OnClick="btnReservePopUp_OnClick"/>
        </asp:Panel>
    </div>

</form>

</body>
</html>