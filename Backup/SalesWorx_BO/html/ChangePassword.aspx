<%@ Page Title="Change Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="SalesWorx_BO.ChangePassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Change Password</span></div>
        <div id="pagenote" >Use Change Password to change your password for access to the SalesWorx system.</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
    
        <ContentTemplate>
            <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                <tr>
                    <td>
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="3" cellpadding="10">
                            <tr>
                                <td class="txtSMBold">
                                    Old Password :
                                </td>
                                <td class="txtSMBold">
                                    <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="txtSMBold">
                                   New Password:
                                </td>
                                <td class="txtSMBold">
                                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="txtSMBold">
                                    Confirm Password
                                </td>
                                <td class="txtSMBold">
                                    <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                       
                            
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td class="txtSMBold">
                                    <asp:Button ID="Button1" runat="server" CssClass="btnInput" 
                                        Text="Change Password" />
                                </td>
                            </tr>
                       
                            
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                        Drag="true" />
                                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup"   style="display:none">
                                                    
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </td> 
    </tr> 
    </table> 
</asp:Content>
