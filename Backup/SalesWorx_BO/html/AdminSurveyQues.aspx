﻿<%@ Page Title="Survey Question" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminSurveyQues.aspx.vb" Inherits="SalesWorx_BO.AdminSurveyQues" %>

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
                    <span class="pgtitile3">Survey Questions</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblSurvey" runat="server" Text="Survey :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSurvey" Width ="250px" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblQuestion" runat="server" Text="Question :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuestion" runat="server" Width ="250px" CssClass="inputSM" TabIndex="2" Rows="3"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblQuestion1" runat="server" Text="Question :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlQuestions" Width ="250px" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    AppendDataBoundItems="true" TabIndex="3">
                                                    <asp:ListItem Selected="True" Value="">--Select a Question--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                     
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="lblResponse" runat="server" Text="Default Response :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlResponses" Width ="250px" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    AppendDataBoundItems="true" TabIndex="4">
                                                    <asp:ListItem Selected="True" Value="">--Select a Response--</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <tr>
                                                <td>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnAdd" runat="server" TabIndex="5" CssClass="btnInputBlue" Text="Add"
                                                        OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnSave" runat="server" TabIndex="6" CssClass="btnInputGreen" Text="Save"
                                                        OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnModify" runat="server" TabIndex="7" CssClass="btnInputOrange" Text="Modify"
                                                        OnClick="btnModify_Click" />
                                                    <asp:Button ID="btnDelete" runat="server" CssClass="btnInputRed" TabIndex="8" Text="Delete"
                                                        OnClick="btnDelete_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnInputRed" Text="Cancel" TabIndex="9"
                                                        OnClick="btnCancel_Click" />
                                                </td>
                                            </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UPModal" runat="server" >
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
                                                    <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
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