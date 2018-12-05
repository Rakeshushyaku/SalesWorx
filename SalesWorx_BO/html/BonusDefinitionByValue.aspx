<%@ Page Title="Bonus Definition" Language="vb" AutoEventWireup="false" EnableEventValidation="false"
    MasterPageFile="~/html/Site.Master" CodeBehind="BonusDefinitionByValue.aspx.vb" Inherits="SalesWorx_BO.BonusDefinitionByValue" %>

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
     <script type="text/javascript">
         //<![CDATA[
         function onRowDropping(sender, args) {
             if (sender.get_id() == "<%=rgRules.ClientID%>") {
                var node = args.get_destinationHtmlElement();
                if (!isChildOf('<%=rgRules.ClientID%>', node) && !isChildOf('<%=rgRules.ClientID%>', node)) {
                    args.set_cancel(true);
                }
            }
            else {
                var node = args.get_destinationHtmlElement();
                if (!isChildOf('trashCan', node)) {
                    args.set_cancel(true);
                }
                else {
                    if (confirm("Are you sure you want to delete this order?"))
                        args.set_destinationHtmlElement($get('trashCan'));
                    else
                        args.set_cancel(true);
                }
            }
        }

        function isChildOf(parentId, element) {
            while (element) {
                if (element.id && element.id.indexOf(parentId) > -1) {
                    return true;
                }
                element = element.parentNode;
            }
            return false;
        }
        //]]>
    </script>
    <script language="javascript" type="text/javascript">

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }

        function alertCallBackFn(arg) {
            HideRadWindow()
        }

        function ConfirmDelete(msg, event) {

            var ev = event ? event : window.event;
            var callerObj = ev.srcElement ? ev.srcElement : ev.target;
            var callbackFunctionConfirmDelete = function (arg, ev) {
                if (arg) {
                    callerObj["onclick"] = "";
                    if (callerObj.click) callerObj.click();
                    else if (callerObj.tagName == "A") {
                        try {
                            eval(callerObj.href)
                        }
                        catch (e) { }
                    }
                }
            }
            radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Bonus Definition By Invoice Value</h4>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
            <span>Processing... </span>
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>





    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <triggers>
                            
                            </triggers>

        <contenttemplate>
                                
                                      
                                
                                
                                
                                
                        
                                   
                                    <asp:Panel ID="PnlOrderDetails"  GroupingText="" runat="server"  >
                                       
                                 <div class="row">
                                 <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Organization</label> 

                                  
                                  
                                        <telerik:RadComboBox Skin="Simple"  ID="ddl_org" Visible ="false" runat="server" Width ="200px"   AutoPostBack="true">
        </telerik:RadComboBox>
                                  <asp:Label runat ="server" Font-Bold ="true" ForeColor ="#248aaf" ID="lblOrg"></asp:Label>
                                    
                                </div>
                                     </div>
                               
                            <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Plan Name
                                  </label>
                                    
                                    <asp:Label ID="lblPlanName"  runat ="server" Font-Bold ="true"  ForeColor="#248aaf"  ></asp:Label>
                                    
                                </div>
                                </div>
                            </div>
                        
                           
                                             
                              
                            <div class="row">
                                             
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                  Minimum Invoice Value </label>
                                                 
                                                     <telerik:RadNumericTextBox runat="server" ID="txtInvAmount" 
                                                                    TabIndex="2"  MinValue="0" MaxLength ="7"
                                                                    autocomplete="off" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptMouseWheel="false"
                                                                    IncrementSettings-InterceptArrowKeys="false">
                                                                </telerik:RadNumericTextBox>
                                                </div>
                                               </div>
                                     
                                                
                                             
                                                 <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                   Minimum No.Of Items Order</label>
                                                 
                                                     <telerik:RadNumericTextBox runat="server" ID="txtMinItems" 
                                                                    TabIndex="3"  MinValue="0" MaxLength ="4"
                                                                    autocomplete="off" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptMouseWheel="false"
                                                                    IncrementSettings-InterceptArrowKeys="false">
                                                                </telerik:RadNumericTextBox>
                                                </div>
                                                     </div>
                                    <div class="row">
                                  <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                   FOC Item
                                                 </label>
                                                   <telerik:RadComboBox Skin="Simple" ID="ddlgetDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="4" Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Width="90%" Height="175px">
                                                </telerik:RadComboBox>
                                               </div>
                                                   </div>
                                               
                                        </div> 


                                </div>
                            <div class="row">
                                     
                <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                  Valid From</label>
                 
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="2000-01-01 00:00:00.000"  MaxDate ="2100-12-31 00:00:00.000"    Width="100%" TabIndex ="9"    runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator></div>
                    </div>
                                     <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                      
                     Valid To</label>
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="2000-01-01 00:00:00.000"  MaxDate ="2100-12-31 00:00:00.000"  Width="100%" TabIndex ="10"   runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false" ID="Requiredfieldvalidator2" Width ="3px"  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date">
                    </asp:CompareValidator>
                       
                     
                        </div>
                                         </div>
                                 
                                                <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                  FOC Qty </label>
                                              
                                                    <asp:TextBox ID="txtGetQty" runat="server" TabIndex ="8"
                                                            CssClass="inputSM" Width="40%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtGetQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                
                                                  <asp:Button ID="btnAddItems" runat="server" CssClass ="btn btn-success" Text="Add" TabIndex ="9" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-danger"  Text="Clear" TabIndex ="10" />
                                                                                           
                                           <asp:Button ID="btnGo"   runat="server" CssClass="btn btn-default" Text="Go Back" TabIndex ="10" />
                                                
                                               
                                               </div>
                                                    </div>
                                           
                                              <div class="col-sm-2">
                                            <div class="form-group">
                                     <label class="hidden-xs empty-label"><br /></label>
                                               
                                                
                                                       
                                            
                                                       
                                                             <asp:Label ID="lblLineId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                         <asp:HiddenField runat ="server" ID="hfOrgID" />
                                                     <asp:HiddenField runat ="server" ID="hfPlanID" />
                                                           <asp:Label ID="lblOrgId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                               <asp:Label ID="lblVF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblVT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                            
                                                     </div>
                                                  </div>
                                     </div>
                                    </asp:Panel>
                                    
                                  
                                         <div class="overflowx">
                                                   <telerik:RadGrid ID="rgRules" Skin="Simple"
                        AllowSorting="true" autogeneratedcolumns="false" AutoGenerateColumns="False" DataSourceID ="sqlDS" OnRowDrop="RadGrid1_RowDrop" 
                        PageSize="8" AllowPaging="false" runat="server" AllowFilteringByColumn="false" 
                        GridLines="None">
                        <GroupingSettings CaseSensitive="false" />
                        <clientsettings allowrowsdragdrop="True" allowcolumnsreorder="true" reordercolumnsonclient="true">
                    <Selecting AllowRowSelect="True" EnableDragToSelectRows="false"></Selecting>
                    <ClientEvents OnRowDropping="onRowDropping"></ClientEvents>  </clientsettings>
                        <MasterTableView Summary="RadGrid table" DataKeyNames ="Rule_ID"  DataSourceID ="sqlDS" AllowFilteringByColumn="false" AllowPaging="false" PageSize="8" TableLayout ="Auto"  >
                           
                            <NoRecordsTemplate>
                                <div>
                                    There are no records to display
                                </div>
                            </NoRecordsTemplate>

                            <Columns>
                             

                              <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                            InitializeTemplatesFirst="false">


                            <ItemTemplate>
                                 
                                 <asp:ImageButton ID="Edit" runat ="server" CommandName="EditRule"   ImageUrl="~/images/edit-13.png"   ToolTip="Edit Rule"  />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                            InitializeTemplatesFirst="false">


                            <ItemTemplate> <asp:Label ID="lblRule" Visible ="false"  runat="server" Text='<%# Bind("Rule_ID")%>'></asp:Label>
                                   <asp:Label ID="lblMinValue"  Visible ="false" runat="server" Text='<%# Bind("Min_Invoice_Value")%>'></asp:Label>
                                                                <asp:Label ID="lblMinItem" runat="server" Visible ="false" Text='<%# Bind("Min_Ordered_Items")%>'></asp:Label>
                                                                 <asp:Label ID="lblBCode" runat="server" Visible ="false" Text='<%# Bind("FOC_Item_Code")%>'></asp:Label>
                                                                 <asp:Label ID="lblBQty" runat="server" Visible ="false" Text='<%# Bind("FOC_Qty")%>'></asp:Label>
                                                                    <asp:Label ID="lblValidFrom" Visible ="false" runat="server" Text='<%# Bind("Valid_From") %>'></asp:Label>
                                                                       <asp:Label ID="lblValidTo" Visible ="false" runat="server" Text='<%# Bind("Valid_To") %>'></asp:Label>
                                                                 
                                                          
                                 <asp:ImageButton ID="DeleteRule" runat ="server" 
                                                                                                 CommandName="DeleteRule"  ImageUrl="~/images/delete-13.png"
                                              ToolTip="Delete Rule" 
                                               OnClientClick="return ConfirmDelete('Are you sure to delete this rule?',event);"  />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </telerik:GridTemplateColumn>

                                 <telerik:GridBoundColumn UniqueName="MinInvValue"   DataFormatString ="{0:N2}" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Min.Invoice Value" DataField="Min_Invoice_Value">
                                    <ItemStyle Wrap="false" HorizontalAlign ="Left"  />
                                    <HeaderStyle Wrap="false" HorizontalAlign ="Left"   />
                                </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn UniqueName="MinItems"   DataFormatString ="{0:N0}" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Min.Ordered Items" DataField="Min_Ordered_Items">
                                    <ItemStyle Wrap="false" HorizontalAlign ="Center"  />
                                    <HeaderStyle Wrap="false" HorizontalAlign ="Center"  />
                                </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn UniqueName="FOCItem"   AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="FOC Item" DataField="Description">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false"  />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="FOCQty"  DataFormatString ="{0:N0}"    AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="FOC Qty" DataField="FOC_Qty">
                                    <ItemStyle Wrap="false" HorizontalAlign ="Center" />
                                    <HeaderStyle Wrap="false" HorizontalAlign ="Center" />
                                </telerik:GridBoundColumn>
                                   <telerik:GridBoundColumn UniqueName="ApplySequence"     AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Apply Sequence" DataField="Apply_Sequence">
                                    <ItemStyle Wrap="false" HorizontalAlign ="Center" />
                                    <HeaderStyle Wrap="false" HorizontalAlign ="Center" />
                                </telerik:GridBoundColumn>

                                  <telerik:GridBoundColumn UniqueName="ValidFrom"  DataFormatString ="{0:dd-MMM-yyyy}"    AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Valid From" DataField="Valid_From">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" />
                                </telerik:GridBoundColumn>
                                 <telerik:GridBoundColumn UniqueName="ValidTo"  DataFormatString ="{0:dd-MMM-yyyy}"    AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Valid To" DataField="Valid_To">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" />
                                </telerik:GridBoundColumn>
                            </Columns>




                        </MasterTableView>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true"></PagerStyle>
                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                    </telerik:RadGrid>

                                               <asp:SqlDataSource ID="sqlDS" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                        SelectCommand="app_LoadRulesByInvoiceValue" SelectCommandType ="StoredProcedure"   >

                                        <SelectParameters >
                                            <asp:ControlParameter ControlID="hfPlanID" Name="PlanID" DefaultValue="0" />
                                         
                                        </SelectParameters>

                                    </asp:SqlDataSource>
                                             </div>   
                                    
                                </contenttemplate>
    </asp:UpdatePanel>











    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server"
        DisplayAfter="10">
        <progresstemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                                        <span style="font-size: 12px; font-weight:700px;color: #3399ff;">Processing...
                                        
                                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        
                                        </span>
                                    </asp:Panel>
                                  
                                </progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>

    <br />
    <br />

</asp:Content>
