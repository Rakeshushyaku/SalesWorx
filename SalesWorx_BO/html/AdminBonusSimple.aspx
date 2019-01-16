<%@ Page Title="Admin Bonus Simple" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminBonusSimple.aspx.vb" Inherits="SalesWorx_BO.AdminBonusSimple" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


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
    <h4>Bonus Plan Listing</h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <div class="form-group">
        <label>
            <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label></label>
    </div>


   <%-- <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
        <contenttemplate>--%>
    <telerik:RadAjaxPanel runat ="server" ID="pl">
                                   <asp:Panel ID="pnlBonusheader" Visible="false" runat ="server" >
                                         <div class="row">
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                    Organization </label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddl_org"  TabIndex ="1" AutoPostBack="true" runat="server" Width ="100%" CssClass="inputSM">
                                    </telerik:RadComboBox></div>
                                             </div>
                                 
                              <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                                 Description</label>
                                                <telerik:RadTextBox runat ="server" ID="txtDesc"  Width  ="100%" TabIndex ="2" ></telerik:RadTextBox>
                                                   <%-- <asp:TextBox ID="txtDescription" runat="server" Width  ="100%"  Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" ></asp:TextBox>--%>
                                                     <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
   <asp:Label ID="lblPlanName" Visible ="false" runat ="server" ></asp:Label>   
                                                       
                                                </div>
                                  </div>
                                            
                                         
                                 <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Scheme Type </label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddlType"  TabIndex ="3"  runat="server" Width ="100%" CssClass="inputSM">
                                         
                                    </telerik:RadComboBox>
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
                                          <div class="col-sm-2">
                                            <div class="form-group">
                                                <label class="hidden-xs">&nbsp;</label>
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server"  CssClass ="btn btn-success"  Text="Add" TabIndex ="4" />
                                                        <asp:Button ID="btnClear" runat="server"  CssClass ="btn btn-default"  Text="Clear" TabIndex ="6" />
                                                   
                                                   
                                                    </div>
                                             
                                        
                                          
                                           
                                        </div>
                                              
                                 </div>
                                    </asp:Panel>
                             <%--   </contenttemplate>
    </asp:UpdatePanel>--%>


    <div class="table-responsive">
       
                <asp:GridView Width="100%" ID="dgv" DataKeyNames="Bns_Plan_ID" runat="server" EmptyDataText="No data to display"
                    AutoGenerateColumns="False" RowStyle-Wrap="false"
                    AllowPaging="true" AllowSorting="true" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">

                    <Columns>

                        <asp:BoundField DataField="Bns_Plan_ID" HeaderText="Plan ID" SortExpression="Bns_Plan_ID">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%-- <asp:TemplateField HeaderText="Description" SortExpression ="Description">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' ToolTip="Edit Plan"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="OrgName" HeaderText="Organization" SortExpression="OrgName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Plan_Type" HeaderText="Plan Type" SortExpression="Plan_Type">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="TotItems" HeaderText="Tot.Items" SortExpression="TotItems">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotCustomers" HeaderText="Tot.Customers Assigned"
                            SortExpression="TotCustomers">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="AddItems" ID="LBAddItems" runat="server"
                                    Text="Add/Edit Items" ToolTip="Add/Edit Item"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="AssignPlan" ID="LBAssignPlan" runat="server"
                                    Text="Assign Customers" ToolTip="Assign Bonus Plan"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="DeleteGroup" ID="Delete" runat="server"
                                    Text="Delete" ToolTip="Delete Plan" OnClientClick="javascript:return confirm('Would you like to delete this plan ,items and customers associated?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblPlanType" runat="server" Text='<%# Bind("PlanType")%>' Visible="False"></asp:Label>
                                <asp:Label ID="lblOrgId" runat="server" Text='<%# Bind("OrgId") %>' Visible="False"></asp:Label>
                                <asp:Label ID="lblTransType" runat="server" Text='<%# Bind("Transaction_Type") %>' Visible="False"></asp:Label>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                    Text='<%# Bind("Description") %>' Visible="false" ToolTip="Edit Plan"></asp:LinkButton>
                                <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LinkButton1" runat="server"
                                    Text="Edit" Visible="true" ToolTip="Edit Plan"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagerstyle" />
                    <HeaderStyle />
                    <RowStyle CssClass="tdstyle" />
                    <AlternatingRowStyle CssClass="alttdstyle" />
                </asp:GridView>





                 
                    <telerik:RadWindow ID="MPEAlloc" Title= "Confirmation                   " runat="server" Skin="Windows7" Behaviors="Close"
                                                Width="400px" Height="220px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                    <ContentTemplate>

                        
                                <p>
                                    <asp:Label ID="lblinfo1" runat="server" Font-Size="13px" Font-Bold="true" Visible="false"></asp:Label>
                                </p>



                            

                                <div class="form-group">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                    <asp:Label ID="lblmessage1" runat="server" Font-Size="13px"></asp:Label>
                                </div>



                            
                                <div class="form-group text-center">
                                    <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btn btn-primary" />
                                    <asp:Button ID="btnhide" Visible="false" CssClass="btn btn-default" TabIndex="26" runat="server"
                                        Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                    <asp:Button ID="btnClose1" runat="server" Text="No" CssClass="btn btn-danger" />

                                </div>
                            














                    </ContentTemplate>


                </telerik:RadWindow>

         <telerik:RadWindow ID="MPError" Title= "Information" runat="server" Skin="Windows7" Behaviors="Close"
                                                Width="400px" Height="220px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
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
        </div>
        </telerik:RadAjaxPanel>
         
    
    <asp:Label ID="MsgLbl" runat="server" CssClass='txtSM'></asp:Label>





    <asp:UpdateProgress ID="UpdateProgress1"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
