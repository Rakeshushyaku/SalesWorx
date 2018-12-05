<%@ Page Title="Held New Customer" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="HeldNewCustomer.aspx.vb" Inherits="SalesWorx_BO.HeldNewCustomer" EnableEventValidation="false"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style>
    #ctl00_ContentPlaceHolder1_MapWindow_C
    {
    	overflow: hidden !important;
    }
    .RadWindow_Default a.rwIcon {
   background-image: none !important;
}
input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
 <script type="text/javascript">
     var TargetBaseControl = null;

     window.onload = function() {
         try {
             TargetBaseControl =
           document.getElementById('<%= Me.Panel.ClientID %>');
         }
         catch (err) {
             TargetBaseControl = null;
         }
     }
        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.Panel.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }

     function alertCallBackFn(arg) {
         HideRadWindow()
     }
     function HideRadWindow() {

         var elem = $('a[class=rwCloseButton');

         if (elem != null && elem != undefined) {
             $('a[class=rwCloseButton')[0].click();
         }

         $("#frm").find("iframe").hide();
     }
    </script>
 
     </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server"><h4>Held New Customer</h4>
	<telerik:RadWindowManager ID="RadWindowManager2"  Skin="Simple" runat="server" EnableShadow="true"></telerik:RadWindowManager>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true"></telerik:RadWindowManager>
	
  
                             
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
               <telerik:RadWindow ID="MapWindow" Title ="Order Details"  Skin="Windows7"  runat="server"  Behaviors="Move,Close" width="600px" height="550px"  ReloadOnShow="false"  Modal="true"  VisibleStatusbar="false"  Overlay="true"  >
               <ContentTemplate>
                   <div class="popupcontentblk" style="width:auto;">
                        <p><asp:Label ID="lbl_Msg" runat="server" Text=""></asp:Label></p>
                        <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Order Ref. No</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblOrdRef" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Creation Date</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblOrderDate" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Cust. Name</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblCustomerName" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Start Time</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblStartTime" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>End Time</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblEndTime" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Tel. No</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblTelNO" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Order Amount</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblOrdAmount" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Customer PO</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblCustPONo" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Latitude</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblOrdLat" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Longitude</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblOrdLng" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Approval Code</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblAppCode" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Approved By</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblAppby" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                       <div class="row">
                            <div class="col-sm-6">
                                <label><strong>Approval Reason</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblReason" runat="server"></asp:Label></p>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label><strong>Comments</strong></label>
                                <div class="form-group">
                                    <p><asp:Label ID="lblComments" runat="server"></asp:Label></p>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label><strong>ERP Reference</strong></label>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="inputSM" MaxLength="100"></asp:TextBox>
                            <asp:Button ID="btnConcile" runat="server" CssClass="btn btn-primary" OnClick="btnConcile_Click" OnClientClick="javascript:return confirm('Would you like to reconcile this order?');" Text="Mark as Reconciled" />
                        </div>
                    
                    
              <div class="table-responsive overflowx">
             
                
                   <asp:GridView ID="GvOrderDetails" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" EmptyDataRowStyle-Font-Bold="true" EmptyDataText="No records to display" PageSize="25" width="100%">
                       <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle" Height="12px" Wrap="True" />
                       <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                       <Columns>
                           <asp:BoundField DataField="Item_Code" HeaderStyle-HorizontalAlign="Left" HeaderText="Item Code" ItemStyle-HorizontalAlign="Left" NullDisplayText="N/A" SortExpression="Item_Code">
                               <ItemStyle Wrap="True" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Description" HeaderStyle-HorizontalAlign="Left" HeaderText="Description" ItemStyle-HorizontalAlign="Left" NullDisplayText="N/A" SortExpression="Description">
                               <ItemStyle Wrap="True" />
                           </asp:BoundField>
                           <asp:BoundField DataField="ApprovedBy" HeaderStyle-HorizontalAlign="Left" HeaderText="Approved By" ItemStyle-HorizontalAlign="Left" SortExpression="ApprovedBy">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="ApprovalCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Approval Code" ItemStyle-HorizontalAlign="Left" SortExpression="ApprovalCode">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="UsedFor" HeaderStyle-HorizontalAlign="Left" HeaderText="Approval Reason" ItemStyle-HorizontalAlign="Left" SortExpression="UsedFor">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Ordered_Quantity" DataFormatString="{0:N0}" HeaderStyle-HorizontalAlign="Right" HeaderText="Qty" ItemStyle-HorizontalAlign="right" NullDisplayText="N/A" SortExpression="Ordered_Quantity">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Display_UOM" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="UOM" ItemStyle-HorizontalAlign="Left" SortExpression="Display_UOM">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="UPrice" DataFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Wrap="false" HeaderText="Price" ItemStyle-HorizontalAlign="right">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Discount" DataFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Wrap="false" HeaderText="Discount" ItemStyle-HorizontalAlign="right">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="ItemPrice" DataFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Wrap="false" HeaderText="Value" ItemStyle-HorizontalAlign="right">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                       </Columns>
                       <PagerStyle CssClass="pagerstyle" />
                       <HeaderStyle />
                       <RowStyle CssClass="tdstyle" />
                       <AlternatingRowStyle CssClass="alttdstyle" />
                   </asp:GridView>
                    </div>
                    
                 <div><br /></div>   
              <div class="table-responsive overflowx">    
                   <asp:GridView ID="Gv_Receipts" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" EmptyDataRowStyle-Font-Bold="true" EmptyDataText="No collection received againt this order" PageSize="25" width="100%">
                       <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle" Height="12px" Wrap="True" />
                       <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                       <Columns>
                           <asp:BoundField DataField="Collection_Ref_No" HeaderStyle-HorizontalAlign="Left" HeaderText="Collection_Ref_No" ItemStyle-HorizontalAlign="Left" NullDisplayText="N/A">
                               <ItemStyle Wrap="True" />  <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Collected_On" HeaderStyle-HorizontalAlign="Left" HeaderText="Collected_On" ItemStyle-HorizontalAlign="Left" NullDisplayText="N/A">
                               <ItemStyle Wrap="True" />  <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="Amount" HeaderStyle-HorizontalAlign="Left" HeaderText="Amount" ItemStyle-HorizontalAlign="Left">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                           <asp:BoundField DataField="SalesRep_Name" HeaderStyle-HorizontalAlign="Left" HeaderText="Van/FSR" ItemStyle-HorizontalAlign="Left">
                               <ItemStyle Wrap="True" />
                               <HeaderStyle Wrap="false" />
                           </asp:BoundField>
                       </Columns>
                       <HeaderStyle />
                       <RowStyle CssClass="tdstyle" />
                       <AlternatingRowStyle CssClass="alttdstyle" />
                   </asp:GridView>
                   
                    </div>
              </div>
             
               </ContentTemplate>
          </telerik:RadWindow>

            <div class="row">
          <div class="col-sm-6 col-md-4">
                             <div class="form-group">  <label>Organization </label>
            <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  Width ="100%" AutoPostBack ="true" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </telerik:RadComboBox>
            </div>
              </div>
                <div class="col-sm-6 col-md-4">
                             <div class="form-group">
                                 
                                 <label>Van/FSR</label>

                  <telerik:RadComboBox Skin="Simple" CssClass="inputSM" ID="ddlVan"   Width ="100%"
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                </telerik:RadComboBox>
                </div>
              </div>
              <div class="col-sm-6 col-md-4">
                  <div class="row">
                      <div class="col-sm-6">
                         <div class="form-group">                
                   <label>From Date </label>
                                 
              <telerik:RadDatePicker ID="txtFromDate"   runat="server" Width="100%">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd/MM/yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>

                  </div>

                  </div>
                <div class="col-sm-6">
                         <div class="form-group">                   
                                   
                                  <label> To Date</label>
                                  
                <telerik:RadDatePicker ID="txtToDate"   runat="server" Width="100%">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd/MM/yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar1" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                            </div>

                  </div>
                  </div>
              </div>  
            </div>  
              <div class="row">
                
                <div class="col-sm-6">
                         <div class="form-group">   
                             <asp:Button  CssClass ="btn btn-primary" ID="SearchBtn" runat="server" Text="Search" />  
                   <asp:Button  CssClass ="btn btn-default"  ID="btnReset" runat="server" Text="Reset" />
                
         
                                   

                                                  </div>
            </div>  
            <div class="col-sm-6 text-right">
                <div class="form-group">
<asp:Button  CssClass="btn btn-success" ID="btnApprove" runat="server" Text="Release to ERP"  OnClientClick="javascript:return confirm('Would you like to release the selected orders?');" />  
                   <asp:Button CssClass ="btn btn-info" ID="btnCancel" runat="server" Text="Mark as Reconciled"  OnClientClick="javascript:return confirm('Would you like to reconcile the selected orders?');" />        
               
                 <asp:DropDownList CssClass="inputSM" Visible ="false"  ID="ddlCustomer"   Width ="250px"
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </asp:DropDownList>  
                            </div>
                </div>
                
    </div>
                             

       
         
     
       
	
 
  </ContentTemplate>
                
</asp:UpdatePanel>
        
<asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
    <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
        <img  src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
        <span>Processing... </span>
     </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>  
                    

        <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
      
              <asp:GridView  width="100%" ID="GVOrders" runat="server" EmptyDataText="No records to display" EmptyDataRowStyle-Font-Bold="true" 
                 AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  DataKeyNames ="Orig_Sys_Document_Ref"
                  PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                  <Columns>
                  <asp:TemplateField>
                  <ItemTemplate>
                    <%--This is a placeholder for the details GridView--%>
                  </ItemTemplate> 
                </asp:TemplateField>
                  <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                            
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="Order Ref.No" SortExpression ="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                      HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                 <asp:LinkButton  runat="server" ID="lbOrdNo"  Text='<%# Bind("Orig_Sys_Document_Ref") %>'
                                              
                                                   OnClick ="lbOrdNo_Click"
                                                 
                                                    ></asp:LinkButton>
                                                      
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                     <asp:BoundField DataField="System_Order_Date" HeaderText="Order Date"  SortExpression="System_Order_Date" DataFormatString = "{0:dd-MM-yyyy}" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />  <HeaderStyle Wrap="false" />
                    </asp:BoundField>
                     
                     
                    <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  SortExpression="SalesRep_Name" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />  <HeaderStyle Wrap="false" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Customer_Name" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Name" SortExpression="Customer_Name">
                       <HeaderStyle Wrap="false" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Customer_No" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Tel.No" >
                         <ItemStyle Wrap="False" />  <HeaderStyle Wrap="false" />
                     </asp:BoundField>
                    <asp:BoundField DataField="OrderAmount" SortExpression ="OrderAmount" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right" HeaderText="Order Amount" DataFormatString="{0:N2}" >
                         <ItemStyle Wrap="False" />  <HeaderStyle Wrap="false" />
                     </asp:BoundField>
                   <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                 <asp:LinkButton  runat="server" ID="lbRelease"  Text="Release" 
                                                OnClientClick="javascript:return confirm('Would you like to release the selected orders?');"
                                                   OnClick ="lbRelease_Click"
                                                 
                                                    ></asp:LinkButton>
                                                      
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText=""  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                      
                                                 <asp:LinkButton  runat="server" ID="lbReconcile"  Text="Reconcile"
                                                 
                                                  OnClientClick="javascript:return confirm('Would you like to reconcile the selected orders?');"
                                                   OnClick ="lbReconcile_Click"
                                                    ></asp:LinkButton>
                                                    <asp:Label ID="lblOrdNo" runat ="server" Visible ="false"  Text='<%# Bind("Orig_Sys_Document_Ref") %>'></asp:Label>
                                                    <asp:Label ID="lbSTime" runat ="server" Visible ="false"  Text='<%# Bind("Start_Time") %>'></asp:Label>
                                                    <asp:Label ID="lbETime" runat ="server" Visible ="false"  Text='<%# Bind("End_Time") %>'></asp:Label>
                                                    <asp:Label ID="lbComment" runat ="server" Visible ="false"  Text='<%# Bind("Packing_Instructions") %>'></asp:Label>
                                                      <asp:Label ID="lbPONo" runat ="server" Visible ="false"  Text='<%# Bind("Customer_PO_Number") %>'></asp:Label>
                                                      <asp:Label ID="lblAppCode" runat ="server" Visible ="false"  Text='<%# Bind("ApprovalCode") %>'></asp:Label>
                                                      <asp:Label ID="lblAppBy" runat ="server" Visible ="false"  Text='<%# Bind("ApprovedBy") %>'></asp:Label>
                                                      <asp:Label ID="lblUsedFor" runat ="server" Visible ="false"  Text='<%# Bind("UsedFor") %>'></asp:Label>
                                                       <asp:Label ID="lblOrdLat" runat ="server" Visible ="false"  Text='<%# Bind("OrdLat") %>'></asp:Label>
                                                       <asp:Label ID="lblOrdLng" runat ="server" Visible ="false"  Text='<%# Bind("OrdLng") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                  
                 
                  </Columns>
                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
            

         
          
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
            
          </Triggers>
        </asp:UpdatePanel>      <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span>
       </asp:Panel>
       
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       
	
	 
	
	
</asp:Content>
