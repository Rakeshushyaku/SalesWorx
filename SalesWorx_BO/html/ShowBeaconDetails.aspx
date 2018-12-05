<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBeaconDetails.aspx.vb" Inherits="SalesWorx_BO.ShowBeaconDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="Maindiv" runat="server" >
            <div id="noinfo" runat="server" >No Details could be found</div>
            <div id="info" runat="server" >
            <div class="padding-20">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label>Customer</label>
                        <p><strong><asp:Label ID="lbl_Customer" runat="server" Text="Label"></asp:Label></strong></p>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <label>Visit Start Date</label> 
                        <p><strong><asp:Label ID="lbl_date" runat="server" Text="Label"></asp:Label></strong></p>
                    </div>
                </div>
            </div>
            <table class="table table-bordered">
                <tr>
                    <th>Beacon details at customer location</th>
                    <th>Beacon details detected during visit</th>
                </tr>
                <tr>
                    <td>
                        <p><asp:Label ID="lbl_CustBID" runat="server" Text="Label"></asp:Label></p>
                        <p><asp:Label ID="lbl_CustMajor" runat="server" Text="Label"></asp:Label></p>
                        <p><asp:Label ID="lbl_CustMinor" runat="server" Text="Label"></asp:Label></p>
                    </td>
                    <td>
                        <p><asp:Label ID="lbl_VBID" runat="server" Text="Label"></asp:Label></p>
                        <p><asp:Label ID="lbl_VMajor" runat="server" Text="Label"></asp:Label></p>
                        <p><asp:Label ID="lbl_VMinor" runat="server" Text="Label"></asp:Label></p>
                    </td>
                </tr>
            </table>
            </div>
            </div>
        </div>
    </form>
</body>
</html>
