<%@ Page Title="Customer Visits" Language="vb" AutoEventWireup="false" EnableEventValidation="false"  MasterPageFile="~/html/ReportMasterForASPX.Master" CodeBehind="CustomerVisits.aspx.vb" Inherits="SalesWorx_BO.CustomerVisits" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Customer Visits</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	 <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
<table width="100%" border="0" cellspacing="0" cellpadding="0" >
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
          <%--           <asp:BoundField DataField="Scanned_Closing" ItemStyle-HorizontalAlign="center" HeaderText="Scanned" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>    --%>      
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
            <td colspan="2">
                <asp:DropDownList CssClass="inputSM" ID="ddlCustomer" 
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </asp:DropDownList>            
            </td>            
            <td>
               
            </td>       
          </tr>         
        </table>
 
</td>
</tr>
	<tr>
       <td>
      
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVCustomerVisits" runat="server" 
                  EmptyDataText="No customers to display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                     <asp:BoundField DataField="Visit_Start_Date" HeaderText="Visit Date"  SortExpression="Visit_Start_Date" DataFormatString = "{0:dd/MM/yyyy}" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  SortExpression="SalesRep_Name" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField> 
                    <asp:TemplateField HeaderText="Customer Name" SortExpression="Customer_Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                       <%-- <asp:HyperLink  runat="server" ID="hyp" Text='<%# Bind("Customer_Name") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Actual_Visit_ID","CustomerVisitDetails.aspx?visitid={0}")%>'></asp:HyperLink>--%>
                      <asp:LinkButton  runat="server" ID="hyp"  Text='<%# Bind("Customer_Name") %>'   CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Actual_Visit_ID","CustomerVisitDetails.aspx?visitid={0}")%>' OnClick="Cust_Click"></asp:LinkButton>

                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>                  
                   <asp:BoundField DataField="Customer_No" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="Customer No"  SortExpression="Customer_No">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                     <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Customer_Type" HeaderText="Customer Type"  
                          SortExpression="Customer_Type" Visible="False">
                         <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                     <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Customer_Class" HeaderText="Customer Class" Visible="False" >
                         <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                     <asp:BoundField DataField="Site_Use_ID" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="center" HeaderText="Site Use ID" Visible="false"  >
                         <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
          <%--           <asp:BoundField DataField="Scanned_Closing" ItemStyle-HorizontalAlign="center" HeaderText="Scanned" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>    --%>                  
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Visit_Start_Date" HeaderText="Start Time" 
                          SortExpression="Visit_Start_Date" DataFormatString = "{0:t}">
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Visit_End_Date" HeaderText="End Time"  
                          SortExpression="Visit_End_Date" DataFormatString = "{0:t}">                      
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                    <%--  <asp:BoundField DataField="OrderAmt" ItemStyle-HorizontalAlign="center" HeaderText="Invoice" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>  --%>
                      <asp:TemplateField HeaderText="DC">
                        
                          <ItemTemplate>
                              <asp:Image ID="ImgDC" runat="server" />
                            <%--  <asp:Label ID="Label3" runat="server" Text='<%# Bind("DC") %>'></asp:Label>--%>
                          </ItemTemplate>
                          <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                     <asp:TemplateField HeaderText="Invoice" ItemStyle-HorizontalAlign="Right">
                          <ItemTemplate>
                              <asp:Label ID="Label1" runat="server" 
                                  Text='<%# GetPrice(Eval("OrderAmt"),Eval("Decimal_digits"))%>'></asp:Label>
                          </ItemTemplate>
                          
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:TemplateField>
                       <asp:TemplateField HeaderText="Return" ItemStyle-HorizontalAlign="Right">
                          <ItemTemplate>
                              <asp:Label ID="LabelrETURN" runat="server" 
                                  Text='<%# GetPrice(Eval("RMA"),Eval("Decimal_digits"))%>'></asp:Label>
                          </ItemTemplate>
                          
                          <HeaderStyle HorizontalAlign="Left" Wrap="False"  />
                      </asp:TemplateField>
                       <asp:TemplateField HeaderText="Order" ItemStyle-HorizontalAlign="Right">
                          <ItemTemplate>
                              <asp:Label ID="LabelOrder" runat="server" 
                                  Text='<%# GetPrice(Eval("UnConfirmedOrderAmt"),Eval("Decimal_digits"))%>'></asp:Label>
                          </ItemTemplate>
                          
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:TemplateField>
                       <asp:TemplateField HeaderText="Payment" ItemStyle-HorizontalAlign="Right">
                          <ItemTemplate>
                              <asp:Label ID="lblPayment" runat="server" 
                                  Text='<%# GetPrice(Eval("Payment"),Eval("Decimal_digits"))%>'></asp:Label>
                          </ItemTemplate>
                          
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:TemplateField>
                      <%-- <asp:BoundField DataField="RMA" ItemStyle-HorizontalAlign="center" HeaderText="Return" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>--%> 
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>
          </td>
          </tr>
          
        </table>
        <table cellpadding="2">
        <tr><td style="padding:0px 3px">
           <td>	<table border="0" cellspacing="0" cellpadding="0" runat="server" id="tblTot">
        <tr><td height="25"></td><td>
            <asp:HiddenField ID="hfCurrency" runat="server" />
             <asp:HiddenField ID="hfDecimal" runat="server" />
        </td></tr>
	<tr><td  class="tdstyle">Total sales done for the date range :</td><td class="txtSMBold" > 
        <asp:Label CssClass="inputSM" ID="lblTotSalesDone" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">Total credit notes for the date range:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotCNote" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">Total payment for the date range:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotPayments" runat="server"></asp:Label></td></tr>
	
	<tr><td class="tdstyle"  >Total productive calls for the date range:</td>
        <td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotProductiveCalls" runat="server"></asp:Label>
        </td></tr>
	
	<tr><td  class="tdstyle">Total calls for the date range:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotCalls" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">&nbsp;</td><td class="txtSMBold" >
        &nbsp;</td></tr>
	
	<tr><td  class="tdstyle">Total sales for the month :</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotSalesCurrMonth" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">Total productive calls for the month:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotProdCallsCurrMonth" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">Total calls for the month:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblTotCallsCurrMonth" runat="server"></asp:Label></td></tr>
	
	<tr><td  class="tdstyle">Productivity percentage for the month:</td><td class="txtSMBold" >
        <asp:Label CssClass="inputSM" ID="lblProductivityPercentage" runat="server"></asp:Label></td></tr>
	
	</table>
	</td></tr></table></td></tr><tr><td>

	</td></tr>
  
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
        </asp:UpdatePanel>  <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
	

</asp:Content>
