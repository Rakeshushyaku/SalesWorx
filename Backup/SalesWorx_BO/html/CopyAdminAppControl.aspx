<%@ Page Title="Application Control" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="CopyAdminAppControl.aspx.vb" Inherits="SalesWorx_BO.CopyAdminAppControl" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" src="../js/controlparamsHandler.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Application Control</span></div>
                <div id="pagenote">
                    The Application Control Screen may be used for controlling the additional modules
                    and features of SalesWorx.</div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <input type="hidden" name="SUB_POINT" id="SUB_POINT" value="">
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="4" cellspacing="1">
                                        <tr>
                                            <td class="txtSMBold">
                                                <asp:Label ID="ControlParamsTree" Visible="false" runat="server" Text="Label"></asp:Label>
                                                Parameter Type:
                                                <asp:DropDownList ID="ddlFilterBy" Width="170px" TabIndex="1" CssClass="txtSM" runat="server"
                                                    AutoPostBack="true" DataTextField ="Description" DataValueField ="Code">
                                                   <%-- <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Collection" Value="Collection"></asp:ListItem>
                                                    <asp:ListItem Text="Distribution Check" Value="Distribution Check"></asp:ListItem>
                                                    <asp:ListItem Text="Order" Value="Order"></asp:ListItem>
                                                    <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                                    <asp:ListItem Text="Return" Value="Return"></asp:ListItem>
                                                    <asp:ListItem Text="Survey" Value="Survey"></asp:ListItem>
                                                    <asp:ListItem Text="Van Load/UnLoad" Value="Van Load/UnLoad"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btnInputBlue" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="50%">
                                                <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView Width="100%" ID="gvParams" runat="server"  AutoGenerateColumns="false"
                                                            PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" DataKeyNames="Control_Key" OnRowDataBound="RowDataBound">
                                                           
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblParam"  runat="server" Text="Parameter" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblParamText"  CssClass ="txtSM" runat="server" Text='<%# Bind("ControlText") %>' />
                                                                        <asp:Label ID="lblControlType" Font-Bold="false" runat="server" Visible="false" Text='<%# Bind("ControlType") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblValue" runat="server" Text="Value" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="drpValue" Width="150px" runat="server" DataTextField="Description"
                                                                            DataValueField="Code">
                                                                        </asp:DropDownList>
                                                                        <telerik:RadComboBox ID="chkMulti" TabIndex="9" runat="server" Height="200" Width="150px"
                                                                            CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput" EnableCheckAllItemsCheckBox="true"
                                                                            ForeColor="Black" Localization-CheckAllString="All" EmptyMessage="Please Select"
                                                                            DataTextField="Description" DataValueField="Code">
                                                                        </telerik:RadComboBox>
                                                                        <asp:CheckBox ID="ChkValue" runat="server" Text="" TabIndex="1"  />
                                                                     <telerik:RadTimePicker ID="RTP"  runat="server"  MinDate="1900-01-01">
                                                                  <timeview cellspacing="-1"  TimeFormat = "HH:mm"    interval="00:30:00">
                                                </timeview>
                                                <timepopupbutton hoverimageurl="" imageurl="" />
                                                <datepopupbutton hoverimageurl="" visible="false" imageurl="" tabindex="0" />
                                               
                                               
                                                <dateinput readonly="true" dateformat="dd-MM-yyyy HH:mm" displaydateformat="HH:mm"/>
        </telerik:RadTimePicker>
                                                                        
                                                                        <asp:TextBox ID="txtValue" Width="50px" TabIndex="1" 
                                                                            CssClass="txtSM" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                            TargetControlID="txtValue" ValidChars="." FilterType="Numbers,Custom">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                           <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
                        <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                            <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                <tr align="center">
                                    <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                        border: solid 1px #3399ff; color: White; padding: 3px">
                                        <asp:Label ID="lblinfo" runat="server" Font-Size="14px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                        <asp:Label ID="lblMessage" runat="server" Font-Size="13px" Font-Names="Calibri"></asp:Label>
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
