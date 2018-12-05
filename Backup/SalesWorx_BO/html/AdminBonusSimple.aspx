<%@ Page Title="Admin Bonus Simple" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminBonusSimple.aspx.vb" Inherits="SalesWorx_BO.AdminBonusSimple" %>
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
                    <span class="pgtitile3">Bonus Plan Listing</span></div>
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
                                   <asp:Panel ID="pnlBonusheader" Visible="false" runat ="server" >
                                        <table border="0" cellspacing="2" cellpadding="2" >
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
                                    <asp:GridView Width="100%" ID="dgv" DataKeyNames="Bns_Plan_ID" runat="server" EmptyDataText="No data to display"
                                        AutoGenerateColumns="False" RowStyle-Wrap="false" 
                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"
                                         >
                                     
                                        <Columns>
                                          
                                            <asp:BoundField DataField="Bns_Plan_ID" HeaderText="Plan ID" SortExpression="Bns_Plan_ID" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <%-- <asp:TemplateField HeaderText="Description" SortExpression ="Description">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' ToolTip="Edit Plan"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                            <asp:BoundField  DataField="Description" HeaderText="Description" SortExpression="Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField  DataField="OrgName" HeaderText="Organization" SortExpression="OrgName">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                          
                                                           
                                            <asp:BoundField DataField="TotItems" HeaderText="Tot.Items" SortExpression="TotItems">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign ="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TotCustomers"  HeaderText="Tot.Customers Assigned" 
                                                SortExpression="TotCustomers">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign ="Center" />
                                            </asp:BoundField>
                                           
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="AddItems" ID="LBAddItems" runat="server"
                                                        Text="Add/Edit Items" ToolTip="Add/Edit Item" ></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="AssignPlan" ID="LBAssignPlan" runat="server"
                                                        Text="Assign Customers" ToolTip="Assign Bonus Plan" ></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="" Visible ="false" >
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="DeleteGroup" ID="Delete" runat="server"
                                                        Text="Delete" ToolTip="Delete Plan"  OnClientClick="javascript:return confirm('Would you like to delete this plan ,items and customers associated?');"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrgId" runat="server" Text='<%# Bind("OrgId") %>'></asp:Label>
                                                                <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' Visible ="false"  ToolTip="Edit Plan"></asp:LinkButton>
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
