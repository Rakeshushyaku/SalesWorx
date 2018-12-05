<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanEOTDetailed.aspx.vb" Inherits="SalesWorx_BO.Rep_VanEOTDetailed" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">      
div.RadGrid .rgPager .rgAdvPart     
{     
    display:none;        
}     
.widgetblkinsmall .row .col-xs-6:last-child {
    text-align: right;
}
.rgDataDiv
{
    max-height:240px;
    height:auto !important;
}
@media (max-width: 768px)
{
    .visit-statistics td span
    {
        font-size: 14px !important;
    }
    .visit-statistics td span span
    {
        font-size: 18px !important;
    }
}
</style>
    <script>

        function alertCallBackFn(arg) {

        }

        function OpenViewWindowOrder(cid, Refno) {
            
            var URL
           // URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + document.getElementById('<%= horg.ClientID%>').value
            URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + document.getElementById('<%= horg.ClientID%>').value + '&Type=O'
            var oWnd = radopen(URL, null);
            oWnd.SetSize(800, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        function OpenViewWindowReturn(cid, Refno) {
             
            var URL
            //URL = 'RepDetails.aspx?Type=Return&ReportName=ReturnDetailsNew&ID=' + cid + '&OrgID=' + document.getElementById('<%= horg.ClientID%>').value
            URL = 'Rep_ReturnDetails.aspx?ID=' + cid + '&OrgID=' + document.getElementById('<%= horg.ClientID%>').value + '&Type=R'
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        function OpenCollectionViewWindow(cid) {
            var URL
            // URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(850, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }


        document.onkeydown = function (evt) {

            evt = evt || window.event;
            if (evt.keyCode == 27) {

                HideRadWindow();
            }
        };

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }


        }

        function clickExportExcel() {
           
            $("#MainContent_BtnExportExcel").click()
            
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <h4><a href="Rep_EOTSummary.aspx" title="Back"><i class="fa fa-arrow-circle-o-left"></i></a> Van EOT</h4>
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:HiddenField runat="server" id="horg"></asp:HiddenField>
            <asp:HiddenField runat="server" id="hvan"></asp:HiddenField>
            <asp:HiddenField runat="server" id="hDate"></asp:HiddenField>
           
             
    <div id="summary" runat="server">
        <div class="row">
            <div class="col-sm-4">
               <label> <h5>Sales Rep Name</h5></label><p><strong class="text-blue"><h5><asp:Label ID="lbl_Sp" runat="server"></asp:Label></h5></strong></p>
            </div>
            <div class="col-sm-4">
                <label><h5>Date</h5></label><p><strong class="text-blue"><h5><asp:Label ID="lbl_Date" runat="server"></asp:Label></h5></strong></p>
            </div>
            <div class="col-sm-4 text-right">
                <p><a href="javascript:clickExportExcel()" class="btn btn-sm btn-success"><i class="fa fa-file-excel-o"></i> Export Excel</a>
                <a href="javascript:clickExportPDF()" class="btn btn-sm btn-danger"><i class="fa fa-file-pdf-o"></i> Export PDF</a></p>
            </div>
        </div>
        <hr />
        <!--<p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;"> Visit Details</p>-->
        <div class="row">
            <div class="col-sm-5 col-md-5 col-lg-5">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                        Visit Details
                    </div>
                    <div class="dashseccontent">
                        <div class="chart-wrapper" style="padding-top:0;padding-bottom:0;">
                            <table class="visit-statistics" width="100%" cellspacing="0" cellpadding="0" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="td-col-4 text-left" style="padding:4px 0;" colspan="3">Customers</td>
                                            </tr>
                                            <tr>
                                            <td class="td-col-3" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Planned
                                                    <asp:Label ID="lbl_Schcalls" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label></span>
                                                
                                            </td>
                                            <td class="td-col-3" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Coverage
                                                    <asp:Label ID="lbl_totoutvisited" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="td-col-4" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Coverage %
                                                     <asp:Label ID="lbl_adherence" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td-col-2">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Out of route
                                                   <asp:Label ID="lbl_UnplannedOutlets" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="td-col-2">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Billed 
                                                    <asp:Label ID="lbl_billed" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="td-col-1">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Productivty %
                                                    <asp:Label ID="lbl_productivity" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                        </tr>
                                    </tbody></table>
                        </div>
                    </div>
                    
                    <div class="dashseccontent">
                        <div class="chart-wrapper" style="padding-top:0px;padding-bottom:0;">
                            <table class="visit-statistics" width="100%" cellspacing="0" cellpadding="0" border="0" style="border-top: 1px solid #ccc;">
                                <tbody>
                                    <tr>
                                        <td class="td-col-4 text-left" style="padding:4px 0;">Calls</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="chart-wrapper" style="padding-top:0px;padding-bottom:0;">
                            <table class="visit-statistics" width="100%" cellspacing="0" cellpadding="0" border="0" style="height:137px;">
                                        <tbody>
                                            <tr>
                                            <td class="td-col-2" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Actual Calls
                                                    <asp:Label ID="lbl_visited" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                                </span>
                                            </td>
                                            <td class="td-col-2" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Out of Route
                                                    <span style="color:#248AAF;font-size:24px;font-weight:bold;"><asp:Label ID="lbl_visitOut" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label></span>
                                                </span>
                                            </td>
                                            <td class="td-col-1" width="33.3%">
                                                <span style="font-size: 14px; font: 'Segoe UI'; text-align: center; padding-left: 2px;">Success Calls
                                                    <span style="color:#248AAF;font-size:24px;font-weight:bold;"><asp:Label ID="lbl_Success" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label></span>
                                                </span>
                                            </td>
                                        </tr></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-7 col-md-7 col-lg-7">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                        <div class="row">
                            <div class="col-sm-6">
                                Transaction Details
                            </div>
                            <div class="col-sm-6">
                                <div id="divCurrency" runat="server"  >
                                    <h5 class='text-right' style="margin: 0; line-height: 21px;">Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="dashseccontent">
                        <div class="chart-wrapper" style="padding-top:0;padding-bottom: 0;">
                            <table width="100%" class="visit-statistics" cellspacing="0" cellpadding="0" border="0">
                                  <tr>
                                      <td class="text-center td-col-3" width="33.3%" style="padding:4px 0;">Sales</td>
                                      <td class="text-center td-col-3" width="33.3%" style="padding:4px 0;">Returns</td>
                                      <td class="text-center td-col-4" width="33.3%" style="padding:4px 0;">Collection</td>
                                  </tr>
                                  <tr>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Cash Sales
                                            <asp:Label ID="lblCashSales" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                          
                                     </td>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Resellable
                                            <asp:Label ID="lbl_Resellable" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                              
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 2px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                         
	                                        <div style="float: right;">
                                                <asp:Label ID="lbl_retunsCount" runat="server" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                     </td>
                                      <td class="td-col-4">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Cash Collection
                                            <asp:Label ID="lbl_cash" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 2px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                        <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lbl_CashCollCount" runat="server" Text="0" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                     </td>
                                  </tr>
                                  <tr>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Credit Sales
                                            <asp:Label ID="lblCreditSales" runat="server" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 0px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                        <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lblCreditSalesCount" runat="server" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                     </td>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Non Resellable
                                            <asp:Label ID="lbl_nonResell" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                     </td>
                                      <td class="td-col-4">
                                        <span style="font-size: 14px; font: 'Segoe UI';">CDC Collection
                                            <asp:Label ID="lbl_CDC" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 2px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                       <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lbl_CDCCount" runat="server"  Text="0" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                     </td>
                                  </tr>
                                  <tr>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Total Sales
                                            <asp:Label ID="lblTotalSales" runat="server" Font-Bold="true" ForeColor="#26a65b" Font-Size="24px"></asp:Label>
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 0px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                        <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lblTotalSalescount" runat="server" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>

	                                        </div>
                                        </div>--%>
                                     </td>
                                      <td class="td-col-3">
                                        <span style="font-size: 14px; font: 'Segoe UI';">Total Returns
                                            <asp:Label ID="lbl_retuns" runat="server" Font-Bold="true" ForeColor="#26a65b" Font-Size="24px"></asp:Label> 
                                            </span>
                                     </td>
                                      <td class="td-col-4">
                                        <span style="font-size: 14px; font: 'Segoe UI';">PDC Collection
                                            <asp:Label ID="lbl_PDc" runat="server" Text="0.00" Font-Bold="true" ForeColor="#248aaf" Font-Size="24px"></asp:Label>
                                        </span>
                                        <%--<div style="border: #e1e1e1 solid 1px; padding: 2px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                       <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lbl_PDCCount" runat="server"  ForeColor="#444444" Font-Size="12px" Text="0" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                     </td>
                                  </tr>
                                  <tr>
                                      <td class="td-col-2" colspan="2">
                                          <span style="font-size: 16px; font: 'Segoe UI'; line-height:24px;">Net Sales</span>
                                          <asp:Label ID="lblnet" runat="server" Font-Bold="true" ForeColor="#ef4836" Font-Size="24px"></asp:Label>
                                     </td>
                                      <td class="td-col-1">
                                          <span style="font-size: 16px; font: 'Segoe UI'; line-height:24px;">Total Collections </span>
                                          <asp:Label ID="lbl_Collection" runat="server" Font-Bold="true" ForeColor="#26a65b" Font-Size="24px"></asp:Label>
                                          <%--<div style="border: #e1e1e1 solid 1px; padding: 0px 4px; font-weight: 600; overflow:hidden; margin:0 15px;">
	                                        <div style="float: left;">
		                                        <span style="font-size: 12px; color:#444;"></span>
	                                        </div>
	                                        <div style="float: right;">
                                                <asp:Label ID="lbl_CollectionCount" runat="server" ForeColor="#444444" Font-Size="12px" Visible="false"></asp:Label>
	                                        </div>
                                        </div>--%>
                                      </td>
                                  </tr>
                            </table>
                            </div>
                    </div>
                </div>
            </div>
        </div>

        <!--<div class="row">
            
            <div class="col-sm-4 col-md-3 col-lg-2-1">
                <div class="widgetblk widgetblkinsmall">Actual Visits<div class="text-primary">
            </div>
                </div>
                </div>
             <div class="col-sm-4 col-md-3 col-lg-2-1">
                <div class="widgetblk widgetblkinsmall">Actual Planned Visits<div class="text-primary"><asp:Label ID="lbl_plannedvisited" runat="server"></asp:Label>
            </div>
                </div>
                </div>
             <div class="col-sm-4 col-md-3 col-lg-2-1">
                <div class="widgetblk widgetblkinsmall">Unplanned Visits<div class="text-primary"><asp:Label ID="lbl_unplannedvisited" runat="server"></asp:Label>
            </div>
                </div>
                </div>

            <div class="col-sm-4 col-md-3 col-lg-2-1">
                <div class="widgetblk widgetblkinsmall">Planned Customers<div class="text-primary"><asp:Label ID="lbl_plannedOutlets" runat="server"></asp:Label>
            </div>
                    </div>
                </div>
            </div>-->
        
     
</div>
            <p><br /></p>

            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Calls</p>
             <div class="table-responsive">
         <telerik:RadGrid id="gvRepVisits" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                   <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="0"   />
                </ClientSettings>
             
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer"  >
                                                                    <ItemStyle  Wrap="true" Width="225px" />
                                                                    <ItemTemplate >
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>'  ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>                                              
                                                             
                                                       <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visit_Start_Date" HeaderText="Start Time" SortExpression ="Visit_Start_Date"
                                                               DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" >
                                                                <ItemStyle Wrap="False" Width="150px"/>
                                                            </telerik:GridBoundColumn>
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visit_End_Date" HeaderText="End Time"
                                                                  SortExpression ="Visit_End_Date" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" >
                                                                <ItemStyle Wrap="False"  Width="150px" />
                                                            </telerik:GridBoundColumn>
                                                           

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="InvCnt" HeaderText="Invoice </br>Count"
                                                                  SortExpression ="InvCnt" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="OrderAmt" HeaderText="Invoice Amount"
                                                                  SortExpression ="OrderAmt" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" Width="90px"   />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="RMACnt" HeaderText="Returns Count"
                                                                  SortExpression ="RMACnt" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"   />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="RMA" HeaderText="Returns Amount"
                                                                  SortExpression ="RMA" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  Width="90px"   />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="CollectionCnt" HeaderText="Collection Count"
                                                                  SortExpression ="CollectionCnt" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                     <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Payment" HeaderText="Collected Amount"
                                                                  SortExpression ="Payment" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"   Width="90px"   />
                                                           </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                    </div>     
            <p><br /><br /></p>
            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Sales Invoices</p>
                              <div   class="table-responsive">
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                      <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="0"  />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression ="Orig_Sys_Document_Ref"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowOrder(""{0}"", ""{1}"");", Eval("Row_ID"), Eval("Orig_Sys_Document_Ref"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="true" />
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Start_Time" HeaderText="Order Date" SortExpression ="Start_Time"
                                                                DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                     <ItemStyle Wrap="true" Width="250px"/>
                                                                </telerik:GridTemplateColumn>
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Order_amt" HeaderText="Amount"
                                                                  SortExpression ="Order_amt" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Mode" HeaderText="Mode"
                                                                  SortExpression ="Mode" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"/>
                                                            </telerik:GridBoundColumn>
                                                               
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
 </div> 
            <p><br /><br /></p>
             <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Returns</p>  

            
                                 
                     <div   class="table-responsive">
                         
                              <telerik:RadGrid id="gvRepReturn" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                     <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="0" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression ="Orig_Sys_Document_Ref"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowReturn(""{0}"", ""{1}"");", Eval("Row_ID"), Eval("Orig_Sys_Document_Ref"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                              <ItemStyle Wrap="true" Width="100px"/>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Start_Time" HeaderText="Order Date" SortExpression ="Start_Time"
                                                                DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="false" Width="150px"/>
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                 <ItemStyle Wrap="true" Width="130px"/>
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                    <ItemStyle Wrap="true" Width="250px"/>
                                                                </telerik:GridTemplateColumn>
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Resellable" HeaderText="Resellable"
                                                                  SortExpression ="Resellable" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="true"  HorizontalAlign="Right" Width="80px" />
                                                           </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="NonResellable" HeaderText="Non <br/>Resellable"
                                                                  SortExpression ="NonResellable" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="true"  HorizontalAlign="Right" Width="80px" />
                                                           </telerik:GridBoundColumn>

                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Total" HeaderText="Total"
                                                                  SortExpression ="Total" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="true"  HorizontalAlign="Right" Width="90px" />
                                                           </telerik:GridBoundColumn>
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                          </div>                  
                 
            <p><br /><br /></p>
            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Collections</p>
             <div  class="table-responsive">
               <telerik:RadGrid id="gvRepcollection" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="0" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Collection_Ref_No"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Collection_Ref_No" SortExpression ="Collection_Ref_No"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Collection_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Collection_Ref_No")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenCollectionViewWindow(""{0}"");" , Eval("Collection_ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                             <ItemStyle Wrap="true" Width="100px"/>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collected_On" HeaderText="Collected On" SortExpression ="Collected_On"
                                                                DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="true" Width="100px" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collected_By" HeaderText="Collected By"
                                                                  SortExpression ="Collected_By" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                     <ItemStyle Wrap="true" Width="250px" />
                                                                </telerik:GridTemplateColumn>
                                                          

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collection_Type" HeaderText="Pay.Type"
                                                                  SortExpression ="Collection_Type" >
                                                               <ItemStyle Wrap="true" Width="100px" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Amount" HeaderText="Amount"
                                                                  SortExpression ="Amount" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="false"  HorizontalAlign="Right"  Width="100px" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Discount" HeaderText="Discount"
                                                                  SortExpression ="Discount" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="false"  HorizontalAlign="Right"   Width="100px"/>
                                                            </telerik:GridBoundColumn>
                                                           
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                 
                 
                    <p><br /><br /></p>
            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Stock</p>

                 <div class="overflowx">
            <telerik:RadGrid ID="gvrep_stock" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="6" AllowPaging="True" runat="server"
                GridLines="None" GroupingSettings-GroupContinuesFormatString="">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                 
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="6" CommandItemDisplay="Top">
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>

                   
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                     

                     <telerik:GridTemplateColumn uniqueName="Product"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Item" SortExpression ="Product"
                                                                HeaderText="Product" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:Label CssClass="label-width" ID="Product" runat="server" Text='<%# Bind("Item")%>'></asp:Label>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="left" Width="200" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                        
                          <telerik:GridTemplateColumn uniqueName="UOM"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="UOM" SortExpression ="UOM"
                                                                HeaderText="UOM" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:Label CssClass="label-width" ID="lbl_UOM" runat="server" Text='<%# Bind("UOM")%>'></asp:Label>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="left" Width="200" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>

                          <telerik:GridTemplateColumn uniqueName="Opening_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Opening_S" SortExpression ="Opening_S"
                                                                HeaderText="Opening<br/>Stock<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Opening_S" runat="server" Text='<%# Bind("Opening_S", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowOpening_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                     
                             <telerik:GridTemplateColumn uniqueName="Opening_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Opening_N" SortExpression ="Opening_N"
                                                                HeaderText="Opening Stock<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50"  ID="Lnk_Opening_N" runat="server" Text='<%# Bind("Opening_N", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowOpening_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Right" Width="100" />
                                  <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                        
                           <telerik:GridTemplateColumn uniqueName="Loaded"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Loaded" SortExpression ="Loaded"
                                                                HeaderText="Loaded" >
                                                            <ItemTemplate>
                                                                
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Loaded" runat="server" Text='<%# Bind("Loaded", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowLoad(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                                <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>        

                         <telerik:GridTemplateColumn uniqueName="Sold"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Sold" SortExpression ="Sold"
                                                                HeaderText="Sold" >
                                                            <ItemTemplate>
                                                              
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Sold" runat="server" Text='<%# Bind("Sold", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowSold(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>     
                            <telerik:GridTemplateColumn uniqueName="Returned_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Returned_S" SortExpression ="Returned_S"
                                                                HeaderText="Returned<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                 
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Returned_S" runat="server" Text='<%# Bind("Returned_S", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowReturned_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                                 <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>                        
                        <telerik:GridTemplateColumn uniqueName="Returned_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Returned_N" SortExpression ="Returned_N"
                                                                HeaderText="Returned<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Returned_N" runat="server" Text='<%# Bind("Returned_N", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowReturned_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>  
                         <telerik:GridTemplateColumn uniqueName="Unload_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Unload_S" SortExpression ="Unload_S"
                                                                HeaderText="Unloaded<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                               
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Unload_S" runat="server" Text='<%# Bind("Unload_S", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowUnload_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="100" />
                              <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn uniqueName="Unload_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Unload_N" SortExpression ="Unload_N"
                                                                HeaderText="Unloaded<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                                 
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Unload_N" runat="server" Text='<%# Bind("Unload_N", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowUnload_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>   
                        <telerik:GridTemplateColumn uniqueName="Closing_S"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Closing_S" SortExpression ="Closing_S"
                                                                HeaderText="Closing<br/>Stock<br/>(Sellable)" >
                                                            <ItemTemplate>
                                                                
                                                                 <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Closing_S" runat="server" Text='<%# Bind("Closing_S", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowClosing_S(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="100" />
                             <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn>
                     
                             <telerik:GridTemplateColumn uniqueName="Closing_N"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Closing_N" SortExpression ="Closing_N"
                                                                HeaderText="Closing Stock<br/>(Non Sellable)" >
                                                            <ItemTemplate>
                                                              
                                                                <asp:LinkButton Enabled = "false" Width="50" ID="Lnk_Closing_N" runat="server" Text='<%# Bind("Closing_N", "{0:N0}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindowClosing_N(""{0}"", ""{1}"");", Eval("SalesRep_ID"), Eval("Inventory_Item_ID"))%>'  ></asp:LinkButton>
                                                            </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Right" Width="100" />
                                  <HeaderStyle VerticalAlign="Top" />
                           </telerik:GridTemplateColumn> 
                    </Columns>
                    <GroupByExpressions>

                                            <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                     
                                                                <telerik:GridGroupByField FieldAlias="Van" FieldName="Salesrep_name"   
                                                                   ></telerik:GridGroupByField>
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                             
                                                                     <telerik:GridGroupByField FieldName="Salesrep_name"  >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                      
                                                         
                                                    </GroupByExpressions>
                </MasterTableView>
            </telerik:RadGrid>
            </div>      
                           </div>
                                   <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

                                       </ContentTemplate>
        
        </asp:UpdatePanel> 
     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>

    <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
</asp:Content>
