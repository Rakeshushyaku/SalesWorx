<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VanTerritory.aspx.vb" Inherits="SalesWorx_BO.VanTerritory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    Sales Rep.:
                </td>
                <td>
                    <asp:DropDownList ID="ddlSalesRep" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                        <asp:ListItem Selected="True" Text="-- Select Sales Rep. --" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td>
                    Customer Segment:
                </td>
                <td>
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="both" Width="250" Height="200"
                        Wrap="false" BorderStyle="Inset" Direction="LeftToRight" HorizontalAlign="Left">
                        <asp:CheckBoxList ID="lstCustomerSegment" runat="server" CellPadding="5" CellSpacing="5"
                            RepeatDirection="Vertical" RepeatLayout="Flow" TextAlign="Right">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
                <td>
                    Sales District:
                </td>
                <td>
                    <asp:Panel ID="Panel2" runat="server" ScrollBars="both" Width="250" Height="200"
                        Wrap="false" BorderStyle="Inset" Direction="LeftToRight" HorizontalAlign="Left">
                        <asp:CheckBoxList ID="lstSalesDistrict" runat="server" CellPadding="5" CellSpacing="5"
                            RepeatDirection="Vertical" RepeatLayout="Flow" TextAlign="Right">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" />
            </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
