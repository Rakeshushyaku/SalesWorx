<%@ Page Title="Incentive Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepIncentiveMonthDetails.aspx.vb" Inherits="SalesWorx_BO.RepIncentiveMonthDetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
        .hide
  {
    display: none !important;
  }
    </style>
    <script type="text/javascript">
        function OpenLocWindow(Org_ID, Incentive_Month, Incentive_Year,  Emp_Code) {


            var URL
            URL = 'RepIncentiveMonthDetails.aspx?Org_ID=' + Org_ID + '&Incentive_Month=' + Incentive_Month + '&Incentive_Year=' + Incentive_Year + '&Emp_Code=' + Emp_Code;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(1000, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            return false
        }
        function clickExportBiffExcel() {
            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
        function alertCallBackFn(arg) {

        }

        function clickSearch() {
            $("#MainContent_SearchBtn").click()
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="tooltip"]').on('click', function () { $(this).tooltip('hide'); });
        }

    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
        .k-chart svg{
	        margin:0 -6px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
      <h4><a href="RepIncentive.aspx?b=1"  title="Back"><i class="fa fa-arrow-circle-o-left"></i></a>Incentive</h4>
   
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional"  >
        <contenttemplate>
      

                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                    <div class="col-sm-6 col-md-4">
                                                        <div class="form-group">
                                                            <label >Transaction Ref. No</label>
                                                            <asp:TextBox ID="txtRefNo" Width="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6 col-md-4">
                                                        <div class="form-group">
                                                            <label>From Date</label>
                                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" Width ="100%">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar2" runat="server">
                                                                <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="OK" />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6 col-md-4">
                                                        <div class="form-group">
                                                            <label>To Date</label>
                                                            <telerik:RadDatePicker ID="txtToDate" runat="server" Width ="100%">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar1" runat="server">
                                                                <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="OK" />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                    </div>
                                                     <div class="col-sm-3">
                                                        <div class="form-group">&nbsp;</div>
                                                         </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                   
                                                </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                    <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                    <asp:HyperLink href="" CssClass ="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
             <div id="repdiv" runat="server" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
             
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div> 
                              <div class="table-responsive">
                             
                           

                              </div>

             
                
                 
                   <div class="table-responsive">
                                  <asp:Label ID="lblStatus" runat ="server" Visible ="false"></asp:Label>

                                <telerik:RadGrid ID="gvMonthDetails" AllowSorting="True" Skin="Simple" BorderColor="LightGray"
                                    PageSize="10" AllowPaging="True" runat="server" ShowFooter="true"
                                    GridLines="None">

                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true" AllowGroupExpandCollapse="true">
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" HierarchyDefaultExpanded="true" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                                      DataKeyNames="Trx_Ref_No"   PageSize="10">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                        <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>


                                        <Columns>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Trx_Ref_No" HeaderText="Transaction Ref.No"
                                                SortExpression="Trx_Ref_No" >
                                                <ItemStyle Wrap="False" HorizontalAlign="left" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="left" />
                                              
                                            </telerik:GridBoundColumn>


                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top" Visible="false" HeaderStyle-HorizontalAlign="Left" DataField="Trx_Type" HeaderText="Type"
                                                                  SortExpression ="Trx_Type" >
                                                                <ItemStyle Wrap="false" />
                                            </telerik:GridBoundColumn> 


                                             <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Trx_Date" HeaderText="Date"
                                                                  SortExpression ="Trx_Date" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" >
                                                                <ItemStyle Wrap="false"  HorizontalAlign="Left" />
                                            </telerik:GridBoundColumn> 
                                              


                                             <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Trx_Amt" HeaderText="Amount"
                                                                  SortExpression ="Trx_Amt" DataFormatString="{0:#,##0.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                              </telerik:GridBoundColumn>


                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Trx_Pending_Amt" HeaderText="Pending Amount"
                                                                  SortExpression ="Trx_Pending_Amt" DataFormatString="{0:#,##0.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                              </telerik:GridBoundColumn>

                                             <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Trx_Incentive" HeaderText="Incentive"
                                                                  SortExpression ="Trx_Incentive" DataFormatString="{0:#,##0.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                              </telerik:GridBoundColumn>

                                            
                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Pending_Incentive" HeaderText="Pending Incentive"
                                                                  SortExpression ="Pending_Incentive" DataFormatString="{0:#,##0.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                              </telerik:GridBoundColumn>
                                            

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                 <div class="row">
                                                                                                           
                                     <div class="col-sm-12">
                                          
                                     </div>
                                          
                                      </div>
                            </div>

<p><br /><br /></p>

               
           
</div>
       
     

    </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
                             <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export"  />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>



</asp:Content>
