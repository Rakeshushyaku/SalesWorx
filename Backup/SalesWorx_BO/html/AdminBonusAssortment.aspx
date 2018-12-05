<%@ Page Title="Admin Bonus Assortment" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminBonusAssortment.aspx.vb" Inherits="SalesWorx_BO.AdminBonusAssortment" %>
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


 </style> 
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Bonus Assortment Plan Listing</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="PnlOrderDetails"  Font-Bold ="true"  ForeColor="#0090d9"   GroupingText="" runat="server">
                                        <table border="0" cellspacing="2" cellpadding="2">
                                         <tr>
                                <td class="txtSMBold" width="125">
                                    Organization :
                                  
                                  </td>
                                    <td class="txtSMBold">
                                    <asp:DropDownList ID="ddl_org"  TabIndex ="1" AutoPostBack="true" runat="server" Width ="200px" CssClass="inputSM">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                <tr>
                              <td width="75" class="txtSMBold">
                                                 Description
                                                </td>
                                                <td >
                                                    <asp:TextBox ID="txtDescription" runat="server" Width ="200px"  Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" ></asp:TextBox>
                                                     <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
   <asp:Label ID="lblPlanName" Visible ="false" runat ="server" ></asp:Label>   
                                                       
                                                </td>
                                </tr> 
                                           
                                            
                                           <tr>
                 <td  class="txtSMBold" style ="width:25%;" >
                  Valid From</td>
                 <td  style ="width:25%;">
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"   Width="100px" TabIndex ="3"    runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                <td  class="txtSMBold" style ="width:25%;" >         
                     Valid To</td><td  style ="width:25%;">
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100px" TabIndex ="4"   runat="server" 
                                    >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false"  ID="Requiredfieldvalidator2" Width ="3px" 
                                  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                           <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date">
                    </asp:CompareValidator>
                     
                        </td> 
                        
                        </tr>
                               <tr>
                                 <td></td>   
                                    <td >
                                      <asp:CheckBox ID="chActive"  Visible ="false" CssClass ="inputSM" runat ="server" Text ="Is Active"  />
                                    </td> 
                                    </tr>  
                                 <tr>
                                 <td colspan ="2"><br /></td>
                                 </tr>
                                          <tr>
                                           <td></td>
                                                 <td  >
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass="btnInputBlue" Text="Add" TabIndex ="5" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass="btnInputGrey" Text="Clear" TabIndex ="6" />
                                                   
                                                   
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
                                    <asp:GridView Width="100%" ID="dgv" DataKeyNames="Assortment_Plan_ID" runat="server" EmptyDataText="No data to display"
                                        AutoGenerateColumns="False" RowStyle-Wrap="false" 
                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"
                                         >
                                     
                                        <Columns>
                                          
                                            <asp:BoundField DataField="Assortment_Plan_ID" HeaderText="Plan ID" SortExpression="Assortment_Plan_ID" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Description" SortExpression ="Description">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' ToolTip="Edit Plan"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                          
                                            <asp:BoundField  DataField="OrgName" HeaderText="Organization" SortExpression="OrgName">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_To"
                                                                HeaderText="Valid To"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="IsActive" Visible="false"  HeaderText="Is Active" SortExpression="IsActive">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" Visible="false" SortExpression="UpdatedBy">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UpdatedAt" Visible ="false" HeaderText="Updated At" DataFormatString="{0:dd-MM-yyyy}"
                                                SortExpression="UpdatedAt">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="AddItems" ID="LBAddItems" runat="server"
                                                        Text="Add/Edit Items" ToolTip="Add/Edit Item" Visible ='<%# Bind("IsAddItems") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                               <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="DeleteGroup" ID="Delete" runat="server"
                                                        Text="Delete" ToolTip="Delete Plan"  OnClientClick="javascript:return confirm('Would you like to deactivate this plan and items associated?');"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrgId" runat="server" Text='<%# Bind("OrgId") %>'></asp:Label>
                                                                  <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("Valid_From") %>'></asp:Label>
                                                                       <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("Valid_To") %>'></asp:Label>
                                                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Bind("IsActive") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                        </Columns>
                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />  
                                    </asp:GridView>
                                              <table>
                                            <tr>
                                                <td>
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
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel> 
                                                    
                                                    
                                                    
                                                    
                                               <table>
                                        <tr>
                                            <td>
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
                                                                 <asp:Label ID="lblinfo1" runat="server" Font-Size ="13px" Font-Bold ="true"   ></asp:Label>
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
                
                
                                
                              
                                          <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="clsPnl"
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
