<%@ Page Title ="Assign Customers to Van for Survey" Language="vb" AutoEventWireup="false" CodeBehind="AdminSurveyFSRCustomers.aspx.vb"
    MasterPageFile="~/html/DefaultLayout.Master" Inherits="SalesWorx_BO.AdminSurveyFSRCustomers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
        } 

    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Assign Customers to Van for Survey</span></div>
                <%--  <div id="pagenote">
                           </div>--%>
                <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <tr>
                                <th>
                                </th>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="lblLable" runat="server" Font-Bold ="true" Text="Please select a van :"></asp:Label>
                                    <asp:DropDownList ID="ddlSalesRep"  Width ="250px" AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCustAvailed" Font-Bold="true" runat="server" Text="Customers Available:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label Font-Bold="true" ID="lblCustAssign" runat="server" Text="Customers Assigned :"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="400" ID="lstDefault" runat="server">
                                                </asp:ListBox>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button align="center" Width="100" ID="btnAdd" runat="server" Text="Add -> "
                                                                CssClass="btnInputBlue" OnClick="btnAdd_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnAddAll" runat="server" Text="Add All -> "
                                                                CssClass="btnInputBlue" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text=" <- Remove"
                                                                CssClass="btnInputRed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnRemoveAll" runat="server" Enabled ="false" 
                                                                Text=" <- Remove All" CssClass="btnInputRed" OnClientClick="javascript:return confirm('Would you like to remove all the assigned customers?');"  />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="400" ID="lstSelected" runat="server">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UPModal" runat="server">
        <ContentTemplate>
            <table width="auto" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                            TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                            Drag="true" />
                        <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                            background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                            padding: 3px; display: none;">
                            <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                <tr align="center">
                                    <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                        border: solid 1px #3399ff; color: White; padding: 3px">
                                        <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                <span style="font-size: 12px; color: #3399ff;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
