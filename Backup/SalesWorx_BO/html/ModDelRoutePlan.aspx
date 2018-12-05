<%@ Page Title="Edit/Delete Route Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="ModDelRoutePlan.aspx.vb" Inherits="SalesWorx_BO.ModDelRoutePlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../js/RoutePlanner.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'block';
                postBackElement.disabled = true;
            }

        }

        function EndRequest(sender, args) {
            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'none';
                postBackElement.disabled = false;
            }
        } 

    </script>

    <input type="hidden" name="Action_Mode" value="" />
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Edit/Delete Route Plan</span></div>
                <div id="pagenote">
                    This screen may be used to modify or delete route plans prepared for vans.</div>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpPanel" runat="server">
                    <ProgressTemplate>
                        <div style="z-index: 9999; position: absolute; top: 48%; left: 48%;">
                            <img src="../images/Progress.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: 700px; color: #3399ff;">Processing...
                            </span>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel runat="server" ID="UpPanel">
                    <ContentTemplate>
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <asp:Panel ID="VanPanel" runat="server">
                                <tr>
                                    <td>
                                        <label>
                                            Van:
                                        </label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Route_FSR_ID" runat="server" AutoPostBack="true" DataTextField="SalesRep_Name"
                                            CssClass="input" TabIndex ="1" Width ="250px" DataValueField="SalesRep_ID" AppendDataBoundItems="true">
                                            <asp:ListItem>-- Select a Van --</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="BtnPanel" Visible="false">
                                <tr>
                                    <td>
                                        <label>
                                            Route Plans:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Route_ID"   TabIndex ="2" Width ="250px" runat="server" CssClass="input" DataTextField="Details"
                                            DataValueField="FSR_Plan_ID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left" style="padding-left: 20px; text-align: left height:50px">
                                        <asp:Button ID="ModBtn"  TabIndex ="3" runat="server" Text="View/Modify" CssClass="btnInputBlue" />
                                        &nbsp;
                                        <asp:Button ID="DelBtn" TabIndex ="4" runat="server" Text="Delete" CssClass="btnInputRed" />
                                    </td>
                                </tr>
                            </asp:Panel>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
