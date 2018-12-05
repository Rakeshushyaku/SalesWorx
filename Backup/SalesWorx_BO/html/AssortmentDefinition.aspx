<%@ Page Title="Assortment Plan Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AssortmentDefinition.aspx.vb" Inherits="SalesWorx_BO.AssortmentDefinition" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <style>
     .rcTimePopup
 {
   display:none ! important;
 }
  .RadListBox_Default .rlbText, .RadListBox_Default .rlbItem, .RadListBox_Default .rlbButtonText, .RadListBox_Default .rlbEmptyMessage {
font: 13px "Calibri" ;
color: black;
}
.RadComboBox_Default .rcbInput, .RadComboBoxDropDown_Default {
font: 13px "Calibri";
color: black;
}
 </style> 
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Assortment Plan Details</span></div>
                 
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style ="float:right;">
                            <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label>
                              <asp:Button ID="btnGoBack"  CssClass="btnInputGrey" runat ="server" Text ="Go Back" />
                                           
                                          
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                  
                                  
                                                  
                                          
                           
                                
                                        <asp:Panel ID="Panel1"  Font-Bold ="true" runat="server" GroupingText ="Assortment Bonus Details">
                                         <table border="0" cellspacing="2" cellpadding="2">
                                         <tr>
                                         
                                         <td colspan ="6" style ="font-weight:bold;color:#0090d9;">Checkbox indicates  mandatory item</td>
                                         </tr>
                                         
                                        <tr>
                                        <td colspan ="2" style ="color:#0090d9; font-size:14px;font-weight:bold;">
                                        Ordered Items 
                                        </td>
                                        <td colspan ="2" style ="color:#0090d9; font-size:14px;font-weight:bold;">
                                   
                                        </td>
                                        <td colspan ="2" style ="color:#0090d9; font-size:14px;font-weight:bold;">
                                        Bonus Items
                                        </td>
                                        </tr>
                                        <tr>
                                        <td>Item Code:</td>
                                        <td>
                                         <telerik:RadComboBox ID="ddlOrdCode" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="1"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" CssClass ="inputSM"
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                        </td>
                                        <td colspan ="2">
                                           Organization : &nbsp;&nbsp; <asp:Label ID="lblOrgID" runat ="server"  Visible ="false" ></asp:Label>
                                   <asp:Label ID="lblOrgName" runat ="server"  Font-Bold ="true" ForeColor ="Green"  ></asp:Label>
                                        </td>
                                      
                                        <td>Item Code:</td>
                                        <td>
                                         <telerik:RadComboBox ID="ddlGetCode" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="3"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" CssClass ="inputSM"
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                        </td>
                                     
                                        </tr>
                                         <tr>
                                        <td>Description:</td>
                                        <td>
                                         <telerik:RadComboBox ID="ddlOrdDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending" 
                                                    MinimumFilterLength="1"  runat="server" CssClass ="inputSM" AutoPostBack ="true" 
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                        </td>
                                        <td  colspan ="2">
                                        Plan Name:
                                               &nbsp;&nbsp; <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
   <asp:Label ID="lblPlanName"  runat ="server"  Font-Bold ="true" ForeColor ="Green"  ></asp:Label>  
                                        </td>
                                       
                                        <td>Description:</td>
                                        <td>
                                         <telerik:RadComboBox ID="ddlgetDesc" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="4" Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" CssClass ="inputSM"
                                                    Height="200px" Width="300px">
                                                </telerik:RadComboBox>
                                        </td>
                                      
                                        </tr>
                                         <tr>
                                        <td></td>
                                        <td  >
                                      <%--    <asp:ImageButton runat ="server" ID="btnOrdAdd" ToolTip ="Add Item"  ImageUrl="~/images/add_24x24.png" />
                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:ImageButton runat ="server" ID="btnOrdRemove" ToolTip ="Remove Item"  ImageUrl="~/images/delete_24x24.png" />--%>
                                      
                                       <asp:Button ID="btnOrdAdd"  CssClass="btnInputBlue" runat ="server" Text ="Add" />
                                        
                                        <asp:Button ID="btnOrdRemove"  CssClass="btnInputRed" runat ="server" Text ="Remove" />
                                       
                                        </td>
                                        <td colspan ="2">
                                        </td>
                                     
                                       <td></td>
                                        <td style="padding-left: 23px;" >
                                     <asp:Button ID="btnGetAdd"  CssClass="btnInputBlue" runat ="server" Text ="Add" />
                                        <asp:Button ID="btnGetRemove"  CssClass="btnInputRed" runat ="server" Text ="Remove" />
                                      <%--   <asp:ImageButton runat ="server" ID="btnGetAdd" ToolTip ="Add Item"  ImageUrl="~/images/add_24x24.png" />
                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:ImageButton runat ="server" ID="btnGetRemove" ToolTip ="Remove Item"  ImageUrl="~/images/delete_24x24.png" />
                                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                          <asp:ImageButton runat ="server" ID="btnCopy" Width ="24px" Height ="24px" ToolTip ="Copy Ordered -> Bonus"  ImageUrl="~/images/iconCopy.gif" />--%>
                                         <asp:Button ID="btnCopy"  CssClass="btnInputOrange" runat ="server" Text ="Copy Ordered -> Bonus" />
                                        </td>
                                       
                                        </tr>
                                      
                                       
                                          <tr>
                                      
                                      <td colspan ="2" style ="font-weight:normal ! important;">
                                         <telerik:RadListBox ID="lstOrderd"  CssClass ="inputSM"    
                                         tooltip="Press CTRL key for multi select item"
                                           SelectionMode ="Multiple"    Width="350px" AutoPostBack ="true" 
                                          runat="server" CheckBoxes="true" ShowCheckAll="true" 
            
                Height="350px">
             
            </telerik:RadListBox>
                                        </td> 
                                        <td colspan ="2" valign ="top"    >
        
                            <asp:Panel ID="ZoneReg" Font-Bold ="true"  runat="server" GroupingText ="Bonus Rules"   >
                                            <table id="table2" width="100%" cellpadding="1" cellspacing ="0">
                                             <tr>
                                                    <td  class="txtSMBold" width="60px"  valign ="top" >
                                                        From Qty:
                                                        </td>
                                                          <td   valign ="top" >
                                                        <telerik:RadNumericTextBox CssClass ="inputSM"  runat="server" ID="txtFromQty"  Width="70px" TabIndex ="9" IncrementSettings-InterceptMouseWheel="false"
                                                                IncrementSettings-InterceptArrowKeys="false" NumberFormat-GroupSeparator=""
                                              MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>
                                            <asp:RequiredFieldValidator runat ="server" ID="rfv1" ControlToValidate ="txtFromQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                
                                                     <td class="txtSMBold"   width="60px"  valign ="top" >
                                                           To Qty:  </td>
                                                          <td    valign ="top" >
                                                        <telerik:RadNumericTextBox CssClass ="inputSM"  runat="server" ID="txtToQty" Width="70px"
                                                         TabIndex ="10" IncrementSettings-InterceptMouseWheel="false"
                                                        NumberFormat-GroupSeparator=""
                                                                IncrementSettings-InterceptArrowKeys="false"
                                                MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>
                                             <asp:RequiredFieldValidator runat ="server" ID="rfv2" ControlToValidate ="txtToQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                              <asp:CompareValidator ID="QtyCompareValidator" runat="server" ControlToValidate="txtToQty"
                        ControlToCompare="txtFromQty" Operator="GreaterThan"    Type="Double"
                        ErrorMessage="To qty > From qty">
                    </asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                  <tr>
                                                     <td class="txtSMBold"   valign ="top"  width="60px"  >
                                                          Type:  </td>
                                                          <td   valign ="top" >
                                                          
                                                       <asp:DropDownList CssClass ="inputSM" ID="ddlType"  Width="90px" runat="server" TabIndex ="11" >
                                                 
                                                    </asp:DropDownList>
                                                    
                                                    </td>
                                            
                                                     <td class="txtSMBold"  width="60px"   valign ="top">
                                                          Get Qty:  </td>
                                                          <td   valign ="top" >
                                                        <telerik:RadNumericTextBox runat="server" ID="txtGetQty" CssClass ="inputSM"  Width="70px" TabIndex ="12"
                                                         IncrementSettings-InterceptMouseWheel="false" NumberFormat-GroupSeparator=""
                                                                IncrementSettings-InterceptArrowKeys="false"
                                              MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>
                                             <asp:RequiredFieldValidator runat ="server" ID="rfv3" ControlToValidate ="txtGetQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                               
                                              
                                                <tr>
                                                    <td colspan ="4" align="center" style="text-align: center;">
                                                      <asp:HiddenField ID="hfRow" runat="server" />
                                                          <asp:HiddenField ID="hfDeletedSlabs" runat="server" />
                                                     <asp:Button ID="btnAddSlab" runat="server" TabIndex ="13" Text="Add" CssClass="btnInputGreen" ValidationGroup ="valslab" />
                                                   <asp:HiddenField ID="hfSlabID" runat ="server" />
                                                   <asp:HiddenField ID="hfOldFrom" runat ="server" />
                                                   <asp:HiddenField ID="hfOldTo" runat ="server" />
                                                   
                                                        <asp:Button ID="btnCanSlab" runat="server" TabIndex ="15" Text="Cancel" CssClass="btnInputRed" />
                                                    </td>
                                                </tr>
                                                <tr>
                                              
                                                <td colspan ="4" style ="font-weight:normal ! important;">
                                             
                          
                                                    <asp:GridView Width="100%" ID="dgvSlabs" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="SlabId"
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"  Visible ="false"  HeaderStyle-Wrap="false" DataField="SlabId"
                                                                HeaderText="SlabId">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="FromQty"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ToQty"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                               <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="TypeCode"
                                                                HeaderText="Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="GetQty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                             
                                                         
                                                             
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"  Visible ="false"  runat="server"  CommandName="EditSlab"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteSlab"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected rule?');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                          
                                                           
                                                         
                                                        
                                                        
                                                          
                                                        </Columns>
                                                         <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                                            
                         
                                                </td>
                                             
                                                </tr>
                                            </table>
                                        </asp:Panel>
     
                                 <br />
                                 <br />
                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;     
                                  <asp:Button ID="btnConfirm"  CssClass="btnInputBlue" runat ="server" Text ="Confirm" OnClientClick="javascript:return confirm('Would you like to confirm this?');" />
                                  
                                    
                                    
                                        </td>
                                      
                                      
                                         <td colspan ="2" style ="font-weight:normal ! important;">
                                           <telerik:RadListBox ID="lstGet"   CssClass ="inputSM"   SelectionMode ="Multiple"  
                                          runat="server"  Width="350px"   tooltip="Press CTRL key for multi select item"
            
                Height="350px">
             
            </telerik:RadListBox>
                                        </td>
                                       
                                        </tr>
                                        </table>
                                        </asp:Panel> 
                                          
                                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                    <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                        Drag="true" />
                                                      <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display:none;">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                 <asp:Label ID="lblinfo" runat="server" Font-Size ="13px" Font-Bold ="true"   ></asp:Label>
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
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel> 
                                                    
                                                    
                                                    
                                                    
                                             
                                                <asp:Button ID="Button2" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEAlloc" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="Button2" PopupControlID="pnlAlloc" Drag="true" />
                                                <asp:Panel ID="pnlAlloc" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display:none;">
                                                    <table id="table1" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                        <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                 <asp:Label ID="lblinfo1" runat="server" Font-Size ="14px" Font-Bold ="true"   ></asp:Label>
                                                            </td>
                                                        
                                                        
                                                           
                                                        </tr>
                                                        <tr>
                                                          
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                                               <asp:Label ID="lblmessage1" runat="server"  Font-Size ="13px"    ></asp:Label>
                                                            </td>
                                                            
                                                            
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center" style="text-align: center;">
                                                                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btnInputGreen" />
                                                                <asp:Button ID="btnhide" Visible="false" CssClass="btnInput" TabIndex="26" runat="server"
                                                                    Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                                                <asp:Button ID="btnClose1" runat="server" Text="No" CssClass="btnInputRed" />
                                                               
                                                            </td>
                                                        </tr>
                                                    </table>      
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                    
                                                </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="clsPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                
                                       
                                        <tr>
                                            <td>
                                               
                                                </td>
                                            </tr>
                                        </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="MsgLbl" runat="server" CssClass='txtSM'></asp:Label>
                        </td>
                    </tr>
                </table>
                
                
                                
                              
                                          <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPnl"
                    runat="server">
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
