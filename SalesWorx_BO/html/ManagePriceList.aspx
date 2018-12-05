<%@ Page Title="Manage Price List" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManagePriceList.aspx.vb" Inherits="SalesWorx_BO.ManagePriceList" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
 <style>
     .rcTimePopup
 {
   display:none ! important;
 }
     
 </style> 
    <script>
        function alertCallBackFn(arg) {
           
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
            radconfirm(msg, callbackFunctionConfirmDelete, 350, 120, null, 'Confirmation');
            return false;
        }
    </script> 
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Manage Price List</h4>
          <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />  
                 <span>Processing... </span>    
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
         <telerik:RadAjaxPanel runat ="server" ID="raf">
                <div class="form-group">  <label>
                            <asp:Label runat="server" ID="ConfirmationMsg"></asp:Label></label></div>

                         
                            <%--<asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                   <asp:Panel ID="pnlBonusheader" runat ="server" >
                                          <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                                    Organization <em><span>&nbsp;</span>*</em></label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddl_org"  TabIndex ="1" AutoPostBack="true" runat="server" Width ="100%" CssClass="inputSM">
                                    </telerik:RadComboBox></div>
                                             </div>
                                 
                              <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                                                 Description<em><span>&nbsp;</span>*</em></label>
                                                
                                                    <asp:TextBox ID="txtDescription" runat="server" Width  ="100%"  Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" ></asp:TextBox>
                                                     <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
   <asp:Label ID="lblPlanName" Visible ="false" runat ="server" ></asp:Label>   
                                                       
                                                </div>
                                  </div>
                                            
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                                                 Code<em><span>&nbsp;</span>*</em></label>
                                                
                                                    <asp:TextBox ID="txtcode" runat="server" Width  ="100%"  Font-Bold ="false"  TabIndex ="2"
                                                        CssClass ="inputSM" ></asp:TextBox>
                                                    
                                                       
                                                </div>
                                  </div>    
                               
                                
                                          <div class="col-sm-4">
                                            <div class="form-group">
                                                                                                              <asp:Button ID="btnAddItems" 
                                                                                                              runat="server"  CssClass ="btn btn-success"  Text="Add" TabIndex ="5" />
                                                        <asp:Button ID="btnClear" runat="server"  CssClass ="btn btn-default"  Text="Clear" TabIndex ="6" />
                                                   
                                                   
                                                    </div>
                                             
                                        
                                          
                                           
                                        </div>
                                              </div>
                                 
                                    </asp:Panel>
                           <%--     </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        
         
                            <div class="table-responsive">
                     <%--    <asp:UpdatePanel ID="clsPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <asp:GridView Width="100%" ID="dgv" DataKeyNames="Price_List_ID" runat="server" EmptyDataText="No data to display"
                                        AutoGenerateColumns="False" RowStyle-Wrap="false" 
                                        AllowPaging="true" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                     
                                        <Columns>
                                           <asp:TemplateField HeaderText="" >
                                                <ItemTemplate>
                                                   
   <asp:ImageButton ToolTip="Delete Price List" ID="btnDelete" runat="server" CommandName="DeleteGroup" 
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" 
         OnClientClick="return ConfirmDelete('Would you like to delete this price list ,items and customers associated?',event);"
        />


                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrgId" Visible="False" runat="server" Text='<%# Bind("OrgId") %>'></asp:Label>
                                                                <asp:LinkButton CssClass="txtLinkSM" CommandName="EditPlan" ID="LBEditPlan" runat="server"
                                                        Text='<%# Bind("Description") %>' Visible ="false"  ToolTip="Edit Price List"></asp:LinkButton>

   <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server"  CommandName="EditPlan"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                            <asp:BoundField DataField="Price_List_ID" HeaderText="Price List ID" SortExpression="Price_List_ID" >
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
                                            <asp:BoundField  DataField="Price_List_Code" HeaderText="Price List Code" SortExpression="Price_List_Code">
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
                                                <ItemStyle HorizontalAlign ="Center" />
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton CssClass="txtLinkSM" CommandName="AssignPlan" ID="LBAssignPlan" runat="server"
                                                        Text="Assign Customers" ToolTip="Assign Price List to customer" ></asp:LinkButton>
                                                </ItemTemplate>
                                                   <ItemStyle HorizontalAlign ="Center" />
                                            </asp:TemplateField>
                                              
                                        </Columns>
                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />  
                                    </asp:GridView>
                                            
                                                    
                                                    
                                                    
                                                 
                                     <telerik:RadWindow ID="MPEAlloc" Title= "Confirmation" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Height="210px" Width="400px"  ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                
                                                    <div class="rwDialogPopup">
                                                        <p><asp:Label ID="lblinfo1" runat="server" Font-Size ="13px" Visible="false"   ></asp:Label></p>
                                                        <p><asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                                               <asp:Label ID="lblmessage1" runat="server"  Font-Size ="13px"></asp:Label></p>
                                                         <div style="padding:15px 0;">
                                                                
                                                                <asp:Button ID="btnYes" runat="server" Text="Yes"  CssClass ="btn btn-primary"/>
                                                                <asp:Button ID="btnhide" Visible="false" CssClass="btnInput" TabIndex="26" runat="server"
                                                                    Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                                                <asp:Button ID="btnClose1" runat="server" Text="No"  CssClass ="btn btn-danger" />
                                                          </div>    
                                                    </div>      
                                                     
                                               </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                                
                            <%--    </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                            <asp:Label ID="MsgLbl" runat="server" CssClass='txtSM'></asp:Label>
                      
                
                </telerik:RadAjaxPanel>
                                
                              
                                          <asp:UpdateProgress ID="UpdateProgress1" 
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
          
</asp:Content>
