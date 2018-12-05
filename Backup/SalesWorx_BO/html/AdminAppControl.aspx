<%@ Page Title="Application Control" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminAppControl.aspx.vb" Inherits="SalesWorx_BO.AdminAppControl" %>
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
                                            <td class="txtSM">
                                                <asp:Label ID="ControlParamsTree" Visible="false" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="50%">
                                                <asp:Panel ID="Panel1"    Font-Size="12px" Font-Names="Calibri" Font-Bold ="true" runat="server" BorderStyle="Groove" BorderWidth="1px" 
                                                    ScrollBars="Auto">
                                                    <%-- <asp:CheckBoxList ID="chkParams" runat="server" RepeatColumns="3" Font-Bold="False"
                                                        CellPadding="2" CellSpacing="4">
                                                    </asp:CheckBoxList>--%>
                                                    <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="0" ShowCheckBoxes="All">
                                                        <RootNodeStyle    Font-Size="13px" Font-Names="Calibri" Font-Bold ="true" ForeColor="#993333" />
                                                    </asp:TreeView>
                                                </asp:Panel>
                                            </td>
                                            <td valign="top" width="50%">
                                           
                                                <asp:Panel ID="pnlOth" runat="server"  BorderStyle="Groove" BorderWidth="1px"
                                                       Font-Size="12px" Font-Names="Calibri" Font-Bold ="true" GroupingText="Others" ScrollBars="Auto">
                                                    <table cellpadding="4" cellspacing="1" width="100%">
                                                    <tr>
                                                            <td class="txtSM" width="350px">
                                                             Discount Mode:
                                                            </td>
                                                            <td class="txtSM" width="350px">
                                                                <asp:DropDownList ID="ddlDiscountMode"  Width ="170px" TabIndex ="1"  CssClass="txtSM" runat="server" >
                                                                    <asp:ListItem Text="Percentage" Value="P"></asp:ListItem>
                                                                    <asp:ListItem Text="Value" Value="V"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                <td class="txtSMBold">
                                                    Discount Minimum Limit:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDisMinLimit" Width="170px" TabIndex ="2" CssClass="txtSM" runat="server"></asp:TextBox>
                                                                                                      <ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtDisMinLimit"   ValidChars="." FilterType="Numbers,Custom">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Discount Maximum Limit:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDisMaxLimit" Width="170px" TabIndex ="3" CssClass="txtSM" runat="server"></asp:TextBox>
                                                                                                      <ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtDisMaxLimit"  ValidChars="." FilterType="Numbers,Custom">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                                        <tr>
                                                            <td class="txtSM" width="350px">
                                                             Credit Note Limit Mode:
                                                            </td>
                                                            <td class="txtSM" width="350px">
                                                                <asp:DropDownList ID="ddlCNLimitMode"  Width ="170px" TabIndex ="4"  CssClass="txtSM" runat="server" >
                                                                    <asp:ListItem Text="Transaction" Value="T"></asp:ListItem>
                                                                    <asp:ListItem Text="Daily" Value="D"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="txtSM" width="350px">
                                                             Collection Mode:
                                                            </td>
                                                            <td class="txtSM" width="350px">
                                                                <asp:DropDownList ID="ddlCollectMode"  Width ="170px" TabIndex ="5"  CssClass="txtSM" runat="server" >
                                                                    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                                                    <asp:ListItem Text="Cash Customer" Value="CASH_CUST"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="txtSM" width="350px">
                                                                Return Stock Merge Mode:
                                                            </td>
                                                            <td class="txtSM" width="350px">
                                                                <asp:DropDownList ID="ddlCRMergeMode"  Width ="170px"  TabIndex ="6" CssClass="txtSM" runat="server" >
                                                                    <asp:ListItem Text="Expiry" Value="E"></asp:ListItem>
                                                                    <asp:ListItem Text="Damage" Value="D"></asp:ListItem>
                                                                    <asp:ListItem Text="Resellable" Value="R"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="txtSM" width="350px">
                                                                   Van Load Type:
                                                            </td>
                                                            <td class="txtSM" width="350px">
                                                               <asp:DropDownList ID="ddlVanLoad" Width ="170px" TabIndex ="7" CssClass="txtSM" runat="server" >
                                                                    <asp:ListItem Text="AGENCY" Value="AGENCY"></asp:ListItem>
                                                                    <asp:ListItem Text="ERP" Value="ERP"></asp:ListItem>
                                                                    
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                          <tr>
                                                <td class="txtSM" width="350px">
                                                     Van UnLoad Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUnLoad" CssClass="txtSM"  Width ="170px" TabIndex ="8"  runat="server" >
                                                                    <asp:ListItem Text="AGENCY" Value="AGENCY"></asp:ListItem>
                                                                    <asp:ListItem Text="ERP" Value="ERP"></asp:ListItem>
                                                                    
                                                                    
                                                                </asp:DropDownList>
                                                </td>
                                            </tr>
                                                <tr>
                                                <td class="txtSM" width="450px" valign ="top" >
                                                    Unload Quantity Change Mode:
                                                </td>
                                                <td>
                                            <telerik:RadComboBox ID="chkUnloadChange" TabIndex="9" runat="server"  Height="200"
                                                width="170px" CheckBoxes="true" CheckedItemsTexts="DisplayAllInInput"
                                                EnableCheckAllItemsCheckBox="true" ForeColor="Black" Localization-CheckAllString="All"
                                                EmptyMessage="Please Select">
                                                 <items>
                                                    <telerik:RadComboBoxItem Text ="Unsold" Value ="U" />
                                                       <telerik:RadComboBoxItem Text ="Expiry" Value ="E" />
                                             <telerik:RadComboBoxItem Text ="Damage" Value ="D" />
                                                 
                                                    <telerik:RadComboBoxItem Text ="Resellable" Value ="R" />
                                                    </items>
                                            </telerik:RadComboBox>
                                            <%--     <asp:CheckBoxList ID="chkUnloadChange" CssClass="txtSM" runat="server" RepeatColumns="1" 
                                            Font-Bold="False" CellPadding="2" CellSpacing="4">
                                              <asp:ListItem Text="Unload" Value="U"></asp:ListItem>
                                                                    <asp:ListItem Text="Expiry" Value="E"></asp:ListItem>
                                                                    <asp:ListItem Text="Damage" Value="D"></asp:ListItem>
                                                                    <asp:ListItem Text="Resellable" Value="R"></asp:ListItem>
                                                                    
                                        </asp:CheckBoxList>  --%>
                                                </td>
                                            </tr>
                                                    </table>
                                                </asp:Panel>
                                     
                                             
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <br />
                                        </tr>
                                        <%--<tr>
			<td class="txtSM" width="10px">&nbsp;</td> 
            <td class="txtSM" width="140px"><br><b>Plan Due Date: </b></td>
            <td class="txtSM" align="left" width="600px"><BR> <asp:TextBox ID="RoutePlan_Due_Date" runat="server" CssClass="txtSM"></asp:TextBox>&nbsp;(<i>day of the month [1-15]</i>)<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="RoutePlan_Due_Date" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>
                </td>
          </tr>   --%>
                                        <tr>
                                            <td class="txtSM" width="140px">
                                                &nbsp;
                                            </td>
                                            <td align="left" class="txtSM" width="600px">
                                                <asp:Button ID="Button1" runat="server" Text="Update" CssClass="btnInputBlue" />
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
                                        <asp:Label ID="lblinfo" runat="server" Font-Size ="14px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                       <asp:Label ID="lblMessage" runat="server"  Font-Size ="13px"    ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
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
