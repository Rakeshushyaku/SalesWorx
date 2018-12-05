<%@ Page Title="Orders" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMasterForASPX.Master" CodeBehind="Orders.aspx.vb" Inherits="SalesWorx_BO.Orders" EnableEventValidation="false"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	        <span class="pgtitile3">Orders</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="105" class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>
              </td>
                 <td width="105" class="txtSMBold">Van :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlVan" 
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                </asp:DropDownList>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
           
          <tr> 
            <td width="105" class="txtSMBold">From Date :</td>
            <td>
                <asp:TextBox  ID="txtFromDate" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                
            </td>
            <td width="105" class="txtSMBold">To Date :</td>
            <td>
                <asp:TextBox  ID="txtToDate" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd/MM/yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
            </td>       
          </tr>   
          <tr> 
            <td width="105" class="txtSMBold">Customer :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlCustomer" 
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </asp:DropDownList>            
            </td>            
              <td class="txtSMBold">
                  Order Type :</td>
            <td>
                             
                <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="inputSM">
                    <asp:ListItem Value="0">ALL</asp:ListItem>
                    <asp:ListItem Value="Invoice">Invoice</asp:ListItem>
                    <asp:ListItem Value="Order">Order</asp:ListItem>
                </asp:DropDownList>
                             
                        
            </td>       
          </tr>         
        </table>
  </ContentTemplate>
                  <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="ddlOrganization" 
                          EventName="SelectedIndexChanged" />
        </Triggers>
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
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVOrders" runat="server" EmptyDataText="No orders to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                     <asp:BoundField DataField="System_Order_Date" HeaderText="Order Date"  SortExpression="System_Order_Date" DataFormatString = "{0:dd/MM/yyyy}" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                      <asp:TemplateField HeaderText="Order Ref. No" SortExpression="Orig_Sys_Document_Ref" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                       <%--<asp:HyperLink  runat="server" ID="hyp" Text='<%# Bind("Orig_Sys_Document_Ref") %>' navigateurl='<%# GetUrl( Eval("Orig_Sys_Document_Ref"), Eval("Row_ID"), Eval("Customer_Name"),Eval("OrderType"))%>'></asp:HyperLink>--%>
                                                 <asp:LinkButton  runat="server" ID="hyp"  Text='<%# Bind("Orig_Sys_Document_Ref") %>'   CommandArgument='<%# GetUrl( Eval("Orig_Sys_Document_Ref"), Eval("Row_ID"), Eval("Customer_Name"),Eval("OrderType"))%>' OnClick="Cust_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField> 
                     <asp:BoundField DataField="OrderType" HeaderText="Order Type"  SortExpression="OrderType"  NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  SortExpression="SalesRep_Name" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="Customer_Name" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Customer Name" SortExpression="Customer_Name">
                      
                     </asp:BoundField>
                     <asp:BoundField DataField="Customer_No" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Customer No" >
                       
                     </asp:BoundField>
                   
                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Order_Status" HeaderText="Status" SortExpression="Order_Status"/>                                        
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>
              <br /><div style="padding-left:1px" align="center" >
               &nbsp;&nbsp; <asp:Button ID="btnBack" CssClass="btnInput" runat="server" Text="Back" 
                  Visible="False"/>  </div>    <br />   

          </td>
          </tr>
          <tr>
          <td>
               <asp:Label ID="lblCustomer" class="txtSMBold" runat="server" Text="" Visible="false"></asp:Label>  
              <asp:HiddenField ID="hdnVisitID" runat="server" Value=""/>
          </td>
          </tr>
          
        </table>
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
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
