<%@ Page Title="Held New Customer" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="HeldNewCustomer.aspx.vb" Inherits="SalesWorx_BO.HeldNewCustomer" EnableEventValidation="false"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    #ctl00_ContentPlaceHolder1_MapWindow_C
    {
    	overflow: hidden !important;
    }
    .RadWindow_Default a.rwIcon {
   background-image: none !important;
}

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
 </script>
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Held New Customers</span>
	</div> 
	<telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">   </telerik:RadWindowManager>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
  
                             
       
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                        <telerik:RadWindow ID="MapWindow" Title ="Order Details" runat="server"  Behaviors="Move,Close" 
         width="900px" height="650px"  ReloadOnShow="false"  Modal ="true"  VisibleStatusbar="false"  Overlay="true"  >
               <ContentTemplate>
               <table  cellpadding ="4" cellspacing ="4">
               <tr>
               <td class ="txtSMBold" >Order Ref. No</td>
               <td><asp:Label runat ="server" ID="lblOrdRef"  Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
                 <td class ="txtSMBold" >Creation Date</td>
               <td><asp:Label runat ="server" ID="lblOrderDate"  Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               </tr> 
                 <tr>
                   <td class ="txtSMBold" >Cust. Name</td>
               <td><asp:Label runat ="server" ID="lblCustomerName"  Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
                 <td class ="txtSMBold" >Start Time</td>
               <td><asp:Label runat ="server" ID="lblStartTime"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               
               
               </tr> 
                  <tr>
                 <td class ="txtSMBold" >Tel. No</td>
               <td><asp:Label runat ="server" ID="lblTelNO"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
           
                <td class ="txtSMBold" >End Time</td>
               <td><asp:Label runat ="server" ID="lblEndTime"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               </tr> 
                 <tr>
               <td class ="txtSMBold" >Order Amount</td>
               <td><asp:Label runat ="server" ID="lblOrdAmount"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
                <td class ="txtSMBold" >Customer PO</td>
               <td><asp:Label runat ="server" ID="lblCustPONo"  Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               </tr> 
               
                  <tr>
               <td class ="txtSMBold" >Latitude</td>
               <td><asp:Label runat ="server" ID="lblOrdLat"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
                <td class ="txtSMBold" >Longitude</td>
               <td><asp:Label runat ="server" ID="lblOrdLng"   Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               </tr> 
               
                <tr>
               <td class ="txtSMBold" >Approval Code</td>
               <td><asp:Label runat ="server" ID="lblAppCode"   Font-Bold ="true" ForeColor ="Green"></asp:Label></td>
                <td class ="txtSMBold" >Approved By</td>
               <td><asp:Label runat ="server" ID="lblAppby"   Font-Bold ="true" ForeColor ="Green"></asp:Label></td>
               </tr> 
               
               
                   <tr>
                    <td class ="txtSMBold" >Approval Reason</td>
               <td><asp:Label runat ="server" ID="lblReason"   Font-Bold ="true" ForeColor ="Green"></asp:Label></td>
               <td class ="txtSMBold" >Comments</td>
               <td ><asp:Label runat ="server" ID="lblComments"    Font-Bold ="true" ForeColor ="Blue"></asp:Label></td>
               
               </tr> 
               
               
                  <tr>
               <td class ="txtSMBold" valign ="top"  >ERP Reference</td>
              
               <td  valign ="top" colspan ="2"  ><asp:TextBox runat ="server" ID="txtRemarks" MaxLength ="100"
                width="350px" CssClass ="inputSM"  ></asp:TextBox>

              
            
                
                               </td>
                               <td> <asp:Button CssClass="btnInputGreen"  ID="btnConcile"  OnClick ="btnConcile_Click" runat="server" Text="Mark as Reconciled"  OnClientClick="javascript:return confirm('Would you like to reconcile this order?');" />       </td>
                               
               </tr> 
             
               </table>
              <table style="width:100%">
              <tr><td style="width:100%"> <asp:GridView  width="100%" ID="GvOrderDetails" runat="server" EmptyDataText="No records to display" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"  
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                  
                    <asp:BoundField DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code"
                        NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="True" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Description" HeaderText="Description"  SortExpression="Description" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                    <ItemStyle Wrap="True" />
                    </asp:BoundField>
                    
                     <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By"  SortExpression="ApprovedBy" 
                         HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                     <ItemStyle Wrap="True" />
                      <HeaderStyle Wrap ="false" />
                    </asp:BoundField>
                        <asp:BoundField DataField="ApprovalCode" HeaderText="Approval Code"  SortExpression="ApprovalCode"
                         HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                    </asp:BoundField>
                    
                      <asp:BoundField DataField="UsedFor" HeaderText="Approval Reason"  SortExpression="UsedFor"
                         HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                     <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                    </asp:BoundField>
                     
                    <asp:BoundField DataField="Ordered_Quantity" HeaderText="Qty"  SortExpression="Ordered_Quantity"  DataFormatString="{0:N0}"
                    NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right">
                      <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Display_UOM" HeaderStyle-Wrap="false"
                     HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="UOM" SortExpression="Display_UOM">
                       <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                     </asp:BoundField>
                     <asp:BoundField DataField="UPrice" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right" HeaderText="Price" DataFormatString="{0:N2}" >
                          <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                     </asp:BoundField>
                    <asp:BoundField DataField="Discount" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right" HeaderText="Discount" DataFormatString="{0:N2}" >
                         <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                     </asp:BoundField>
                <asp:BoundField DataField="ItemPrice" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right" HeaderText="Value" DataFormatString="{0:N2}" >
                         <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                     </asp:BoundField>
                   
                  </Columns>
                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView></td></tr>
              <tr><td> <asp:GridView  width="100%" ID="Gv_Receipts" runat="server" EmptyDataText="No collection received againt this order" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"  
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                  
                    <asp:BoundField DataField="Collection_Ref_No" HeaderText="Collection_Ref_No" 
                        NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="True" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Collected_On" HeaderText="Collected_On"  NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                    <ItemStyle Wrap="True" />
                    </asp:BoundField>
                    
                     <asp:BoundField DataField="Amount" HeaderText="Amount"   
                         HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                     <ItemStyle Wrap="True" />
                      <HeaderStyle Wrap ="false" />
                    </asp:BoundField>
                      
                      <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  
                         HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                     <ItemStyle Wrap="True" /><HeaderStyle Wrap ="false" />
                    </asp:BoundField>
                     
                    
                   
                  </Columns>
                  
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView></td></tr>
              </table>
                    
              
             
               </ContentTemplate>
          </telerik:RadWindow>
<table width="100%" border="0" cellspacing="2" cellpadding="2">
          <tr>
            <td  class="txtSMBold">Organization :</td>
            <td>
               <asp:DropDownList CssClass="inputSM" ID="ddlOrganization"   Width ="250px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>   
              </td>
                   <td></td>
          </tr>
          <tr>
             <td class="txtSMBold">Van :</td>
            <td> <asp:DropDownList CssClass="inputSM" ID="ddlVan"  Width ="250px"
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                </asp:DropDownList>  
                   
                     </td>
                     <td></td>
          </tr>
           
          <tr> 
            <td  class="txtSMBold">From Date :</td>
            <td>
                <asp:TextBox  ID="txtFromDate" CssClass="inputSM" Width ="100px" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                
           
                 
        
        <span class ="txtSMBold">To Date </span> 
                <asp:TextBox  ID="txtToDate"  Width ="100px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd/MM/yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
               
            </td>   <td></td>
          </tr>
          
          <tr>
          <td colspan ="2" >
          <div style="float:left;">
           <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />  
                   <asp:Button CssClass="btnInputRed" ID="btnReset" runat="server" Text="Reset" />  
                   </div> 
                   </td> 
                    <td>
          <div style="float:right;">
                  <asp:Button CssClass="btnInputBlue" ID="btnApprove" runat="server" Text="Release to ERP"  OnClientClick="javascript:return confirm('Would you like to release the selected orders?');" />  
                   <asp:Button CssClass="btnInputGreen" ID="btnCancel" runat="server" Text="Mark as Reconciled"  OnClientClick="javascript:return confirm('Would you like to reconcile the selected orders?');" />        
                   </div> 
                   </td> 
                   </tr> 
            <tr> 
            <td  class="txtSMBold"></td>
            <td>
                
                
                  <asp:DropDownList CssClass="inputSM" Visible ="false"  ID="ddlCustomer"   Width ="250px"
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </asp:DropDownList>           
                             
            </td>            
                   
              <td></td>
                             
                        
        
          </tr>   
        </table>
       
	
 
  </ContentTemplate>
                
                  </asp:UpdatePanel>
        
                  <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
                    
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
      
              <asp:GridView  width="100%" ID="GVOrders" runat="server" EmptyDataText="No records to display" EmptyDataRowStyle-Font-Bold="true" 
                 AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"  DataKeyNames ="Orig_Sys_Document_Ref"
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
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
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                     
                     
                    <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  SortExpression="SalesRep_Name" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Customer_Name" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Name" SortExpression="Customer_Name">
                      
                     </asp:BoundField>
                     <asp:BoundField DataField="Customer_No" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Tel.No" >
                       
                     </asp:BoundField>
                    <asp:BoundField DataField="OrderAmount" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" 
                     ItemStyle-HorizontalAlign="right" HeaderText="Order Amount" DataFormatString="{0:N2}" >
                       
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
            

         
           <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxtoolkit:modalpopupextender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
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
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
            
          </Triggers>
        </asp:UpdatePanel>      <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel19" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>                  
       </td>              
	</tr>
  
    </table>
	
	</td> 
	</tr>
	</table>
	
	
</asp:Content>
