<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventManagement.aspx.cs" Inherits="EventManagement.EventManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event Management System</title>
    <link rel="stylesheet" href="Huisstijl.css"/>
</head>
<body>
    <form id="formEventManagement" runat="server" style="margin-left: auto; margin-right: auto;">
            <div style="width: 500px; margin-left: auto; margin-right: auto;">
                <asp:ListBox runat="server" ID="lbCampings" height="250px" width="400px"/><br/>
                <table style="border: hidden;">
                        <tr style="border: hidden;">
                            <td>Name:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbCampingName"/>
                            </td>
                        </tr>
                        <tr style="border: hidden;">
                            <td>Street:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbStreet"/>
                            </td>
                        </tr>
                    <tr style="border: hidden;">
                            <td>Number:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbStreetNumber" TextMode="Number"/>
                            </td>
                        </tr>
                    <tr style="border: hidden;">
                            <td>Postcode:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPostcode"/>
                            </td>
                        </tr>
                    <tr style="border: hidden;">
                            <td>City:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbCity"/>
                            </td>
                        </tr>
                </table>
                <asp:Button runat="server" ID="btnAddCamping" CssClass="button" text="Add Camping" OnClick="btnAddCamping_OnClick"/>
            </div>
        
            <div style="width: 500px; margin-left: auto; margin-right: auto;">
            <asp:ListBox runat="server" ID="lbCampingSpots" height="250px" width="400px"/><br/>
            <table style="border: hidden;">
                    <tr style="border: hidden;">
                        <td>Number:</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbSpotNumber" TextMode="Number"/>
                        </td>
                    </tr>
                    <tr style="border: hidden;">
                        <td>Capacity:</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbCapacity" Text="Number"/>
                        </td>
                    </tr>
                <tr style="border: hidden;">
                        <td>Specification:</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSpecification1"/>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbSpecificationValue1"/>
                        </td>
                    </tr>
                 <tr style="border: hidden;">
                        <td>Specification:</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSpecification2"/>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbSpecificationValue2"/>
                        </td>
                    </tr>
                 <tr style="border: hidden;">
                        <td>Specification:</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSpecification3"/>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbDdlSpecificationValue3"/>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnAddCampingSpot" CssClass="button" text="Add CampingSpot" OnClick="btnAddCampingSpot_OnClick"/>
            </div>
        
            <div style="width: 500px; margin-left: auto; margin-right: auto;">
                <asp:ListBox runat="server" ID="lbEvents" height="250px" width="400px"/><br/>
                <table style="border: hidden;">
                        <tr style="border: hidden;">
                            <td>Name:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbEventName"/>
                            </td>
                        </tr>
                        <tr style="border: hidden;">
                            <td>Location:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEvents"/>
                            </td>
                        </tr>
                    <tr style="border: hidden;">
                            <td>Starting Date:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbDateStart" TextMode="Date"/>
                            </td>
                        </tr>
                     <tr style="border: hidden;">
                            <td>Ending Date:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbDateEnd" TextMode="Date"/>
                            </td>
                        </tr>
                     <tr style="border: hidden;">
                            <td>Max visitors:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbEventCapacity" TextMode="Number"/>
                            </td>
                        </tr>
               </table>
               <asp:Button runat="server" ID="btnAddEvent" CssClass="button" text="Add Event" OnClick="btnAddEvent_OnClick"/>
            </div>
    </form>
</body>
</html>
