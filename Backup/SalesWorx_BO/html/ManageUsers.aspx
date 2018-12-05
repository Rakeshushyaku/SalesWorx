<%@ Page Title="Manage Users" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="ManageUsers.aspx.vb" Inherits="SalesWorx_BO.ManageUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
    <div class="pgtitileposition">
        <span class="pgtitile3">Manage Users</span></div>
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
                        <table  cellpadding="3" cellspacing="10">
                            <tr>
                                <td class="txtSMBold">
                                    User :
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpUser" Width ="250px"  CssClass="inputSM"  runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtUser" Width ="250px" runat="server" Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="txtSMBold">
                                    Password:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPwd" Width ="250px" runat="server" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                <td class="txtSMBold">
                                    Designation:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDesignation" Width ="250px" runat="server"  CssClass="inputSM"  AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td class="txtSMBold">
                                    User Type:</td>
                                <td>
                                    <asp:DropDownList ID="drpUserType" Width ="250px" runat="server"  CssClass="inputSM" >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 4px" valign="top" class="txtSMBold">
                                     <asp:Label ID="lbl_Van1" runat="server" Text="Van:"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="drpSalesRep" Width ="250px" runat="server"  CssClass="inputSM" >
                                    </asp:DropDownList>
                                    
                                </td>
                            </tr>
                            <tr>
                             <td class="txtSMBold">
                             <asp:Label runat="server" ID="lbl_Org" Text="Organization:" Visible="false"></asp:Label>
                             </td>
                             <td> <asp:DropDownList ID="ddl_org" Width ="250px" runat="server"  CssClass="inputSM"  AutoPostBack="true" Visible="false">
                                    </asp:DropDownList ></td>
                            </tr>
                            <tr>
                            <td class="txtSMBold" valign="top">
                                <asp:Label ID="lbl_van" runat="server" Text="Van:" Visible="false"></asp:Label>
                                </td>
                            <td><asp:Panel ID="Panel1" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px" Visible="False" Width="514px">
                                        <asp:CheckBoxList ID="chkSalesRep" runat="server" RepeatColumns="2" 
                                            Visible="False" Font-Bold="False" CellPadding="2" CellSpacing="4">
                                        </asp:CheckBoxList>
                                    </asp:Panel></td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="txtSMBold">
                                    <asp:Panel ID="pnlDefault" runat="server">
                                        <asp:Button ID="BtnAdd" runat="server" Text="Add"  CssClass="btnInputBlue"/>
                                        <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btnInputOrange" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnInputRed" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAdd" runat="server" Visible="false">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnInputGreen" />
                                        <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btnInputRed" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlModify" runat="server" Visible="false">
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnInputGreen" />
                                        <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="btnInputRed" />
                                    </asp:Panel>
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
