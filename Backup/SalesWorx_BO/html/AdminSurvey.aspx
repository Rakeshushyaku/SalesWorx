<%@ Page Title="Manage Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminSurvey.aspx.vb" Inherits="SalesWorx_BO.AdminSurvey" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
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
                    <span class="pgtitile3">Manage Survey</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblSurvey" runat="server" Text="Survey :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSurvey" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblTitle" runat="server" Text="Title :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTitle" runat="server" CssClass="inputSM" TabIndex="2" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <td>
                                                    <br />
                                                </td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="Label1" runat="server" Text="Start Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="inputSM" TabIndex="3"></asp:TextBox>
                                                <asp:ImageButton ID="ibSD" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/btnCal.gif" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <td>
                                                    <br />
                                                </td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="Label4" runat="server" Text="Expiry Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtExpDate" runat="server" CssClass="inputSM" TabIndex="4"></asp:TextBox>
                                                <asp:ImageButton ID="IbED" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/btnCal.gif" />
                                            </td>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    <asp:Label ID="Label2" runat="server" Text="Type Code :"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTypeCode" runat="server" AutoPostBack="true" TabIndex="5"
                                                        CssClass="inputSM">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="txtSMBold">
                                                    <asp:Label ID="Label5" runat="server" Text="Assign To:"></asp:Label>
                                                </td>
                                                <td>
                                                  
                                                                <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto" BorderStyle="Groove"
                                                                    BorderWidth="1px" Width ="400px">
                                                                    <asp:CheckBoxList ID="chkSalRep"  runat="server" RepeatColumns="2" >
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                            </td>
                                                    
                                            </tr>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnAdd" runat="server" TabIndex="7" CssClass="btnInputBlue" Text="Add"
                                                        OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnSave" runat="server" TabIndex="8" CssClass="btnInputGreen" Text="Save"
                                                        OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnModify" runat="server" TabIndex="9" CssClass="btnInputOrange" Text="Modify"
                                                        OnClick="btnModify_Click" />
                                                    <asp:Button ID="btnDelete" runat="server" CssClass="btnInputRed" TabIndex="10" Text="Delete"
                                                        OnClick="btnDelete_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnInputRed" Text="Cancel" TabIndex="11"
                                                        OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                    </table>
                                    <ajaxToolkit:CalendarExtender ID="CEtxtStartDate" runat="server" Format="MM/dd/yyyy"
                                        PopupButtonID="ibSD" TargetControlID="txtStartDate">
                                    </ajaxToolkit:CalendarExtender>
                                    <ajaxToolkit:CalendarExtender ID="CEtxtEndDate" runat="server" Format="MM/dd/yyyy"
                                        PopupButtonID="ibED" TargetControlID="txtExpDate">
                                    </ajaxToolkit:CalendarExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UPModal" runat="server">
                                <ContentTemplate>
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
                                                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                    <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="text-align: center;">
                                                    <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    </td> </tr> </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UPModal" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
