<%@ Page Title="Admin Bonus Assortment" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminBonusAssortment.aspx.vb" Inherits="SalesWorx_BO.AdminBonusAssortment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .rcTimePopup
        {
            display: none !important;
        }
    </style>
    <script>
        function alertCallBackFn(arg) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Bonus Assortment Plan Listing</h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                                    <asp:Panel ID="PnlOrderDetails"   GroupingText="" runat="server">
                                        <div class="row">
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization </label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddl_org"  TabIndex ="1" AutoPostBack="true" runat="server" Width ="100%" CssClass="inputSM">
                                    </telerik:RadComboBox>
                                 </div>
                                           </div>

                              <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                                 Description</label>
                                                
                                                    <asp:TextBox ID="txtDescription" runat="server" Width ="100%"   Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" ></asp:TextBox>
                                                     <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
   <asp:Label ID="lblPlanName" Visible ="false" runat ="server" ></asp:Label>   
                                                       
                                                </div>
                                  </div>
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Scheme Type </label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddlType"  TabIndex ="1"  runat="server" Width ="100%" CssClass="inputSM">
                                        <Items>
<telerik:RadComboBoxItem Value ="N"  Text ="Overall Quantity"/>
                                         
                                            <telerik:RadComboBoxItem Value ="I"  Text ="Minimum Quantity"/>
                                         
                                        </Items>
                                    </telerik:RadComboBox>
                                 </div>
                                           </div>
                                  </div>
                                             
                                         <div class="row">
                                         <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Valid From</label>
                  
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"   Width ="100%" TabIndex ="3"    runat="server" >
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator> </div>
                                             </div>
                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>      
                     Valid To</label>
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width ="100%" TabIndex ="4"   runat="server" 
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
                      <asp:CheckBox ID="chActive"  Visible ="false" CssClass ="inputSM" runat ="server" Text ="Is Active"  />
                      </div>
                            </div>
                                      <div class="col-sm-2" id="divtransactiontype" runat="server" visible="false"  >
                                            <div class="form-group">
                                                <label>Transaction Type </label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddl_transactionType"  TabIndex ="3"  runat="server" Width ="100%" CssClass="inputSM">
                                        <Items>
                                            
                                            <telerik:RadComboBoxItem Value ="CASH"  Text ="CASH"/>
                                         
                                            <telerik:RadComboBoxItem Value ="CREDIT"  Text ="CREDIT"/>
                                         
                                        </Items>
                                    </telerik:RadComboBox>
                                                 <asp:HiddenField runat="server" ID="H_TransType" Value="0" />
                                 </div>
                                           </div>
                                  <div class="col-sm-3" id="divCategorytype" runat="server" visible="false">
                                            <div class="form-group">
                                                <label>Category</label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple" ID="ddl_Category" EmptyMessage="Select Category" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                 </div>
                                           </div>
                                                   
                                 <div class="col-sm-3">
                                            <div class="form-group">
                                                <label class="hidden-xs empty-label"><br /></label>   
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server" CssClass ="btn btn-success" Text="Add" TabIndex ="5" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-default" Text="Clear" TabIndex ="6" />
                                                   
                                                   
                                                    </div>
                                     </div>
                                             </div>
                                    </asp:Panel>
                                    
                                </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="clsPnl" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                                    <div class="table-responsive">
                                    <asp:GridView Width="100%" ID="dgv" DataKeyNames="Assortment_Plan_ID" runat="server" EmptyDataText="No data to display"
                                        EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False" RowStyle-Wrap="false" 
                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                     
                                        <Columns>
                                          
                                            <asp:BoundField DataField="Assortment_Plan_ID" HeaderText="Plan ID" SortExpression="Assortment_Plan_ID" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Description" SortExpression ="Description">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' ToolTip="Edit Plan"></asp:LinkButton>
                                                    <asp:Label ID="lblTransType" runat="server" Text='<%# Bind("Transaction_Type") %>' Visible="False"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                          
                                            <asp:BoundField  DataField="OrgName" HeaderText="Organization" SortExpression="OrgName">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                              <asp:BoundField  DataField="Plan_Type" HeaderText="Plan Type" SortExpression="Plan_Type">
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
                                                <ItemStyle HorizontalAlign ="Center" />
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="AssignPlan" ID="LBAssignPlan" runat="server"
                                                        Text="Assign Customers" ToolTip="Assign Bonus Plan" ></asp:LinkButton>
                                                </ItemTemplate>
                                                   <ItemStyle HorizontalAlign ="Center" />
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="DeleteGroup" ID="Delete" runat="server"
                                                        Text="Delete" ToolTip="Delete Plan"  OnClientClick="javascript:return confirm('Would you like to deactivate this plan and items associated?');"></asp:LinkButton>
                                                </ItemTemplate>
                                                   <ItemStyle HorizontalAlign ="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrgId" runat="server" Text='<%# Bind("OrgId") %>'></asp:Label>
                                                                  <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("Valid_From") %>'></asp:Label>
                                                                       <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("Valid_To") %>'></asp:Label>
                                                                            <asp:Label ID="lblIsActive" runat="server" Text='<%# Bind("IsActive") %>'></asp:Label>
                                                                     <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("PlanType")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                        </Columns>
                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />  
                                    </asp:GridView>
                                               
                                   </div>                 
                                                    
                                                    
                                                    
                                               
                                                  <telerik:RadWindow ID="MPEAlloc" Title= "Confirmation" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Width="400px" Height="210px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
                                              <ContentTemplate>
                                                    <table id="table1" width="99%" cellpadding="10" style="background-color: White;">
                                                        <tr align="center" style="display:none">
                                                        <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                 <asp:Label ID="lblinfo1" runat="server" Font-Size ="13px" Font-Bold ="true"  Visible="false"   ></asp:Label>
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
                                                                <asp:Button ID="btnYes" runat="server" Text="Yes"  CssClass ="btn btn-primary"  />
                                                                <asp:Button ID="btnhide" Visible="false" CssClass="btnInput" TabIndex="26" runat="server"
                                                                    Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                                                <asp:Button ID="btnClose1" runat="server" Text="No"  CssClass ="btn btn-danger" />
                                                               
                                                            </td>
                                                        </tr>
                                                    </table>      
                                                   
                                                    
                                              </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 

             <telerik:RadWindow ID="MPError" Title= "Information" runat="server" Skin="Windows7" Behaviors="Close"
                                                Width="400px" Height="420px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                    <ContentTemplate>

                         <telerik:RadGrid id="GV_error" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="5" AllowPaging="false" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                   >
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                       
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Category" HeaderText="Category"
                                                                  SortExpression ="Category" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status"
                                                                  SortExpression ="Status" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                                
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Response" HeaderText="Response"
                                                                  SortExpression ="Response" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                        
                    
                            
                                <div class="form-group text-center">
                                     
                                    <asp:Button ID="Button2" Visible="false" CssClass="btn btn-default" TabIndex="26" runat="server"
                                        Text="OK" CausesValidation="false" OnClientClick="$find('MPError').Hide(); return false;" />
                                 

                                </div>
                            
                        
                    </ContentTemplate>


                </telerik:RadWindow>
                                                
                                </contenttemplate>
    </asp:UpdatePanel>

    <asp:Label ID="MsgLbl" runat="server" CssClass='txtSM'></asp:Label>





    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="clsPnl"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img  src="../assets/img/ajax-loader.gif"  alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>



</asp:Content>
