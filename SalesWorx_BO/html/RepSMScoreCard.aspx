<%@ Page Title="Sales Manager Scorecard" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="RepSMScoreCard.aspx.vb" Inherits="SalesWorx_BO.RepSMScoreCard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../js/kendo.all.min.js"></script>
    <script src="../js/kendo.dataviz.min.js"></script>


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">


        <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />




        <script type="text/javascript" language="javascript">
            function pageLoad(sender, args) {
                $('.rgMasterTable').find('th > a').attr("data-container", "body");
                $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
                $('[data-toggle="tooltip"]').tooltip();
            }

            function clickExportBiffExcel() {

                $("#MainContent_BtnExportBiffExcel").click()
                return false

            }

            function clickExportBiffExcel_TA() {

                $("#MainContent_BtnExportBiffExcel_TA").click()
                return false

            }

            function clickExportLog() {

                $("#MainContent_BtnExportLog").click()
                return false

            }
            function clickExportTop10() {

                $("#MainContent_BtnExportTop10").click()
                return false

            }
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            var postBackElement;
            function InitializeRequest(sender, args) {

                if (prm.get_isInAsyncPostBack())
                    args.set_cancel(true);
                postBackElement = args.get_postBackElement();
                $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                postBackElement.disabled = true;
            }

            function EndRequest(sender, args) {
                $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
                postBackElement.disabled = false;
            }


            function RefreshChart() {
               

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
            function alertCallBackFn(arg) {

            }


        </script>




    </telerik:RadCodeBlock>

    <style>
        #ctl00_MainContent_gvSalesGrowth_OT > tbody > tr:first-child
        {
            height: 50px !important;
        }
         .RadGrid_Simple .rgCommandRow
        {
            background: whitesmoke;
            color: #000;
            /* height: 15px !important; */
        }

         .rgGroupHeader td p
        {
            display: inline;
            margin: 0;
            padding: 0 10px;
            color: #000 !Important;
            font-weight: bold !Important;
        }

        .rgGroupHeader td
        {
            padding-left: 8px;
            padding-bottom: 2px;
            background-color: #eff9ff !Important;
            color: #000 !Important;
        }
        input[type="text"].rdfd_
        {
            margin: 0 !important;
            padding: 0 !important;
            height: 0 !important;
            width: 0 !important;
        }
          .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            font-size: 13px;
            font-weight: bold;
        }

        .visit-statistics td
        {
            padding: 0px 0 !important;
            text-align: center;
            font-size: 18px;
            color: #999;
        }

       
          .RadPanelBar .rpLink
        {
            cursor: pointer;
            text-decoration: none;
            overflow: hidden;
            background-color: transparent !important;
            border-color: transparent !important;
            zoom: 1;
            border-style: none !important;
        }

        .RadPanelBar_Default .rpRootGroup
        {
            border-color: lightgrey;
        }

        div[id*="ReportDiv"]
        {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }

        .RadPanelBar_Simple .rpExpandable span.rpExpandHandle
        {
            background-position: 100% -5px !important;
        }
      
    </style>
    <script>       if (typeof window.JSON === 'undefined') { document.write('<script src="../js/json2.js"><\/script>'); }</script>

    <style scoped>
        .chart-wrapper, .chart-wrapper .k-chart
        {
            margin: auto;
        }

        .rgExpand, .rgCollapse
        {
            display: none;
        }

        .RadGrid .rgPagerLabel, .RadGrid .rgPager .RadComboBox, .RadGrid .rgPager .RadInput
        {
            margin: 0 4px 0 0;
            vertical-align: middle;
            visibility: hidden;
        }
        .widgetblkinsmall .row .col-xs-6:last-child {
            border-left: 0px solid #fff;
            text-align:right;
        }
    </style>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">


    <h4>Sales Manager Scorecard</h4>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>



    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel10">
        <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>


        <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
            ExpandMode="MultipleExpandedItems">

            <Items>

                <telerik:RadPanelItem Expanded="True" Text=" ">


                    <ContentTemplate>

                        <div class="row">
                            <div class="col-sm-10">
                                <div class="row">


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>
                                                Organization<em><span>&nbsp;</span>*</em> 
                                                <asp:CompareValidator ID="CompareValidatorRadComboxBoxTables" runat="server" ValidationGroup="valsum"
                                                    ValueToCompare="--Select--" Operator="NotEqual" ControlToValidate="ddlOrganization" ForeColor="Red" ErrorMessage="*" Font-Bold="true" /></label>
                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                            </telerik:RadComboBox>

                                        </div>
                                    </div>


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>
                                                Sales Manager  <em><span>&nbsp;</span>*</em> 
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valsum"
                                                    ValueToCompare="--Select--" Operator="NotEqual" ControlToValidate="ddlMgr" ForeColor="Red" ErrorMessage="*" Font-Bold="true" /></label>
                                            <telerik:RadComboBox Skin="Simple" ID="ddlMgr" Width="100%" runat="server" Filter="Contains">
                                            </telerik:RadComboBox>

                                        </div>
                                    </div>




                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Month</label>
                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" Width="100%" ID="StartTime" runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                </DateInput>

                                            </telerik:RadMonthYearPicker>
                                        </div>
                                    </div>


                                </div>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button CssClass="btn btn-primary btn-block" ID="SearchBtn" runat="server" Text="Search" />
                                       <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                </div>
                                <div class="form-group text-center fontbig">
                                    <label>&nbsp;</label>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

                                </div>
                            </div>
                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>

        <asp:HiddenField ID="hfCurrency" runat="server" />
        <asp:HiddenField ID="hfDecimal" runat="server" />
        <asp:HiddenField ID="hfSE" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfRow" runat="server" />
        <asp:HiddenField ID="hfUserID" runat="server" />

        <div id="Args" runat="server" visible="false">
            <div id="popoverblkouter">
                Hover on icon to view search criteria <i class="fa fa-info-circle">
                    <span class="popoverblk">
                        <p>
                            <strong>Organisation: </strong>
                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Sales Manager: </strong>
                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label>
                        </p>

                        <p>
                            <strong>Month: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>

                    </span>
                </i>
            </div>
        </div>
       
       

        <div id="rpt" runat="server" visible="false" style="padding-top:5px">
        <div class="row">
            <div class="col-sm-4">
                <h5>Sales Manager <strong><span class="text-blue"><asp:Label runat="server" ID="lblMGR"></asp:Label></span></strong></h5>
            </div>
            <div class="col-sm-4">
                <h5><strong>Team Performance (<asp:Label runat ="server" ForeColor ="#0090d9" ID="lblTeamMonth"></asp:Label>)</strong></h5>
            </div>
            <div class="col-sm-4 text-right">
                <h5>Currency <strong><span class="text-blue"><asp:Label runat="server" ID="lblC"></asp:Label></span></strong></h5>
            </div>
        </div>

        <hr />
      
              
        <div class="row" id="MainContent_summary">
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Gross Sales (<asp:Label ID="lblOrdCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblSales" runat="server"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small>Average</small></div>
                        <div class="col-xs-6"><small><asp:Label ID="lblSalesAvg" runat="server"></asp:Label></small></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Returns (<asp:Label ID="lblRetCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblReturn" runat="server"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small>Average</small></div>
                        <div class="col-xs-6"><small><asp:Label ID="lblRetAvg" runat="server"></asp:Label></small></div>
                    </div>
                </div>
            </div>
             <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Net Sales(<asp:Label ID="lblNetCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblNet" runat="server"  Text="0"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small>Average</small></div>
                        <div class="col-xs-6"><small><asp:Label ID="lbl_NetAvg" runat="server" Text=""></asp:Label></small></div>
                    </div>
                </div>
            </div>
             <div class="col-sm-4 ">
                <div class="widgetblk widgetblkinsmall">Collections (<asp:Label ID="lblColCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblCollection" runat="server"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small>Average</small></div>
                        <div class="col-xs-6"><small><asp:Label ID="lblColAvg" runat="server"></asp:Label></small></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Team Size <!--<asp:Label ID="lblVCnt" runat="server"></asp:Label>--><div class="text-primary"><asp:Label ID="lblTeamSize" runat="server"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small style="visibility:hidden;">Average</small></div>
                        <div class="col-xs-6"><small style="visibility:hidden;">0</small></div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Total Calls <!--<asp:Label ID="lblTCount" runat="server"></asp:Label>--><div class="text-primary"><asp:Label ID="lblTotalCalls" runat="server"></asp:Label></div>
                    <div class="row">
                        <div class="col-xs-6"><small>Average</small></div>
                        <div class="col-xs-6"><small><asp:Label ID="lblAvgCalls" runat="server"></asp:Label></small></div>
                    </div>
                </div>
            </div>
           
        </div>

            


        <p>&nbsp;</p>




        <div class="row">
            <div class="col-sm-6">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                       Sales growth (Last 3 Months)
                    </div>   <br />
                    <div class="dashseccontent">

                         <telerik:RadTabStrip ID="GrowthTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage1" SelectedIndex="0">
                              <Tabs>

                                  <telerik:RadTab Text="Sales growth" runat="server"  id="Tab_Salesgrowth" >
                                    
                   
                                        <Tabs>
                                            <telerik:RadTab Text="Chart" runat="server">
                                            </telerik:RadTab>

                                            <telerik:RadTab Text="Table" runat="server">
                                            </telerik:RadTab>
                   
                                        </Tabs>
                                     
                                    </telerik:RadTab>

                                    <telerik:RadTab Text="Target Vs. Achievement " id="Tab_TA"  runat="server" >
                                         <Tabs>
                                            <telerik:RadTab Text="Chart" runat="server">
                                            </telerik:RadTab>

                                            <telerik:RadTab Text="Table" runat="server">
                                            </telerik:RadTab>
                   
                                        </Tabs>

                                    </telerik:RadTab>


                                </Tabs>
                             </telerik:RadTabStrip>
                        
                         <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0">

                    <telerik:RadPageView ID="RadPageView3" runat="server">
                         <div>
                               <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="true" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" PlotArea-XAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"   PlotArea-XAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="RadSalesGrowth" Height="330" Transitions="true" Skin="Silk">
                           <PlotArea>
                                    <Series >
                                        <telerik:LineSeries   DataFieldY="SalesQty" Name="Sales Qty"   >
                                            
                                            <LabelsAppearance Visible="false"></LabelsAppearance>
                                            <Appearance Overlay-Gradient="None" ></Appearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                <ClientTemplate >
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    Sales Qty :   #=dataItem.SalesQty#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="GrossSales" Name="Gross Sales">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    Gross Sales :   #=dataItem.GrossSales#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="NetSales" Name="Net Sales">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    Net Sales :   #=dataItem.NetSales#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="ReturnValue" Name="Return Value">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    Return Value :   #=dataItem.ReturnValue#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                    </Series>
                                  
                                    <XAxis DataLabelsField="MonthY" Color="LightGray"  >
                                       
                                        <MinorGridLines Visible="false"></MinorGridLines>
                                        <MajorGridLines Visible="false"></MajorGridLines>
                                    </XAxis>       
                                    <YAxis Color="LightGray" >
                                        <LabelsAppearance DataFormatString="#,#00.00"></LabelsAppearance>
                                         <TitleAppearance Position="Center" RotationAngle="0" Text="Total">
                                             <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" />
                                         </TitleAppearance>
                                    </YAxis>    
                                                   
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>
                           </div>
                        </telerik:RadPageView> 
                                 <telerik:RadPageView ID="RadPageView4" runat="server">

                           
                                                                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <div class="table-responsive" style="position:relative;height:330px;padding: 5px;  ">
                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel3">
                              
                                  <div style="position:absolute;padding:5px;top:0;left:0;">
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false"  OnClientClick="clickExportBiffExcel()"></asp:ImageButton>
                            </div>
                                 </telerik:RadAjaxPanel>
                                 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvSalesGrowth" runat="server"  AllowSorting="false" 
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"   ColumnsSubTotalsPosition="None"   />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  UniqueName="Description" SortOrder="None" >
                                                                 
                                                                </telerik:PivotGridRowField>
                                                      
                                                        
                                                        <telerik:PivotGridColumnField DataField="MonthYear" SortOrder="None"  >
                 
                                                        </telerik:PivotGridColumnField>

                                                         <telerik:PivotGridAggregateField DataField="TotValue" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        
                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>

                              </div>
        </telerik:RadAjaxPanel>
        

                           </telerik:RadPageView> 


                                                 <telerik:RadPageView ID="RadPageView5" runat="server">
                        
                     
                           <div>
                               <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="false" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" PlotArea-XAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"   PlotArea-XAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-TitleAppearance-TextStyle-FontSize="8" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="RadTAchart" Height="330" Transitions="true" Skin="Silk">
                           <PlotArea>
                                    <Series>
                                        
                                        <telerik:ColumnSeries   DataFieldY="TargetValue" Name="Target Value"   >
                                            <LabelsAppearance Visible="false" ></LabelsAppearance>
                                             
                                            <Appearance Overlay-Gradient="None">
                                                 <FillStyle BackgroundColor="#34B6FA" />
                                            </Appearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                <ClientTemplate >
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    Target Value :   #=dataItem.TargetValue#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                        <telerik:ColumnSeries DataFieldY="AchievementValue" Name="Achievement Value">
                                            <Appearance Overlay-Gradient="None">
                                                 <FillStyle BackgroundColor="#EB963C" />
                                            </Appearance>
                                             <LabelsAppearance Visible="false" >

                                             </LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                    AchievementValue :   #=dataItem.AchievementValue#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                 
                                    </Series>
                                    <Series>
                                      <telerik:LineSeries DataFieldY="TotalCalls" Name="Total Calls" AxisName="Total Calls" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Crimson" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                   Total Calls :   #=dataItem.TotalCalls#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="ProductiveCalls" Name="Total Productive Calls" AxisName="Total Productive Calls" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Green" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Month : #=dataItem.MonthY#
                                                    <br />
                                                   Total Productive Call :   #=dataItem.ProductiveCalls#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                    </Series>
                                    <XAxis DataLabelsField="MonthY" Color="LightGray"  >
                                      
                                        <MinorGridLines Visible="false"></MinorGridLines>
                                        <MajorGridLines Visible="false"></MajorGridLines>
                                    </XAxis>       
                                    <YAxis Color="LightGray"  >
                                        <LabelsAppearance DataFormatString="#,#00.00"></LabelsAppearance>
                                         <TitleAppearance Position="Center" RotationAngle="0" Text="Value">
                                             <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" />
                                         </TitleAppearance>
                                    </YAxis>    
                                    <AdditionalYAxes  >
                                        
                                       <telerik:AxisY  Name="Total Calls" Color="Crimson"  >
                                          <LabelsAppearance DataFormatString="#,#00"></LabelsAppearance>
                                           <TitleAppearance Text="Total Calls"   >
                                                <TextStyle Color="Crimson" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>

                                        <telerik:AxisY  Name="Total Productive Calls" Color="Green"  >
                                          <LabelsAppearance DataFormatString="#,#00"></LabelsAppearance>
                                           <TitleAppearance Text="Total Productive Calls"   >
                                                <TextStyle Color="Green" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>                     
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>
                           </div>
                   
                        </telerik:RadPageView> 


                             <telerik:RadPageView ID="RadPageView6" runat="server">

                           
                                                                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel4">
            <div class="table-responsive" style="position:relative;height:330px;padding: 5px;  ">
                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel5">
                              
                                  <div style="position:absolute;padding:5px;top:0;left:0;">
                                     <asp:ImageButton ID="img_export_TargetVsAchiev" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false"  OnClientClick="clickExportBiffExcel_TA()"></asp:ImageButton>
                            </div>
                                 </telerik:RadAjaxPanel>
                                 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvTargetVsAchiev" runat="server"  AllowSorting="false" 
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"   ColumnsSubTotalsPosition="None"   />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  UniqueName="Description" SortOrder="None" >
                                                                 
                                                                </telerik:PivotGridRowField>
                                                      
                                                        
                                                        <telerik:PivotGridColumnField DataField="MonthYear" SortOrder="None"  >
                 
                                                        </telerik:PivotGridColumnField>

                                                         <telerik:PivotGridAggregateField DataField="TotValue" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        
                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>

                              </div>
        </telerik:RadAjaxPanel>
        

                           </telerik:RadPageView> 


                               

                             </telerik:RadMultiPage> 
                    </div>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="dashsecblock">
                    <div class="dashsectitle">
                       Top 5 Customers Sales (<asp:Label runat ="server" ForeColor ="#0090d9" ID="lblTopCustMonth"></asp:Label>)
                    </div>   <br />
                    <div class="dashseccontent">
                     
                          <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Chart" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="Table" runat="server">
                        </telerik:RadTab>
                     

                    </Tabs>
                </telerik:RadTabStrip>
                                     <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">

                                <telerik:RadPageView ID="RadPageView1" runat="server" >
                                    <telerik:RadHtmlChart runat="server" ID="radTop10" Height="330">
                                     <PlotArea>
                                         <Series>
                                                 <telerik:DonutSeries Name="Sales" DataFieldY="TotValue" NameField="Description">
                                                   <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                   <ClientTemplate>
                                                    Customer : #=dataItem.Description#
                                                    <br />
                                                    Sales :   #=dataItem.TotValue.format(\"N2\")#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                                   <LabelsAppearance Visible="true" DataFormatString="#,##0.00"/>
                                                   </telerik:DonutSeries>
                                            </Series>
                                                                                        
                                        </PlotArea>
                                        <Legend>
                                            <Appearance Position="Bottom" Visible="true" />
                                        </Legend>
                                        <ChartTitle Text="">
                                        </ChartTitle>
                                    </telerik:RadHtmlChart>             
                        </telerik:RadPageView> 
                                     <telerik:RadPageView ID="RadPageView2" runat="server">
                                          <div class="table-responsive" style="position:relative;height:330px;padding: 5px; ">
                           
                                                                 <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">
            <telerik:RadGrid ID="gvCustomers" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
            
                PageSize="7" AllowPaging="true" runat="server" DataSourceID="SqlDataSource1"  ShowFooter ="false"  AllowFilteringByColumn ="true" 
                
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings> 
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto"  CommandItemDisplay="Top"  ShowFooter ="false"  AllowFilteringByColumn ="true" 
                     Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1"
                    PageSize="4">
                      <CommandItemTemplate>
                          <div style="float: left;">
                                            <asp:ImageButton ID="img_exp" runat="server" AlternateText="Export to excel" ToolTip="Export to excel"   ImageUrl="../assets/img/export-excel.png" OnClientClick="clickExportTop10()" />
                                          </div>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button1" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button1_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    
                    <Columns>



                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Customer"
                            SortExpression="Description" ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" FilterControlToolTip ="Type the customer name and press enter" >
                           <ItemStyle Wrap="False" HorizontalAlign="left"  />
                                                                <HeaderStyle Wrap ="false" HorizontalAlign="left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>
                       
                         

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="TotValue" HeaderText="Sales" UniqueName ="Sales"
                            SortExpression="TotValue" AllowFiltering ="false" >
                             <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                               <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                        </telerik:GridBoundColumn>

                  

                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
                                              </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="Rep_SMTop10CustomerSales" SelectCommandType="StoredProcedure">

            <SelectParameters>
                  <asp:ControlParameter ControlID="hfRow" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSE" Name="SMID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="MonthYEAR" />
                              <asp:Parameter Name="Mode" DefaultValue="All" />
                   
            </SelectParameters>

        </asp:SqlDataSource>

                           </telerik:RadPageView> 
                     </telerik:RadMultiPage> 
                    </div>
                </div>
            </div>
        
       
            </div>
        <p class="other-titles"><strong>Transactions By Van/FSR (<asp:Label runat ="server" ForeColor ="#0090d9" ID="lblTransMonth"></asp:Label>)</strong></p>
        
                    
        <div class="table-responsive">
                    <telerik:RadAjaxPanel runat="server" ID="g">
                        <telerik:RadGrid ID="gvLog" DataSourceID="sqlLog" 
                            AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None"
                            PageSize="10" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="true" ShowFooter="true"
                            GridLines="None" >

                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                            <ClientSettings EnableRowHoverStyle="true">
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" DataSourceID="sqlLog" CommandItemDisplay="Top" ShowFooter="true" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                PageSize="10"  >
                                                       
                                  <CommandItemTemplate>
                                      <div style="float: left;">
                                            <asp:ImageButton ID="img_exp" runat="server" AlternateText="Export to excel" ToolTip="Export to excel"   ImageUrl="../assets/img/export-excel.png" OnClientClick="clickExportLog()" />
                                          </div>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                             
                                   </div>
                                    </CommandItemTemplate>
                                <Columns>
                                  
                                    <telerik:GridBoundColumn DataField="SalesRep_Name" HeaderText="Van/FSR" SortExpression="SalesRep_Name" ShowFilterIcon="false" AllowFiltering="true"
                                        CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" FooterText="Total : " FilterControlToolTip="Type van/FSR name and press enter">
                                                <ItemStyle HorizontalAlign="Left" Wrap ="false"  />
                                        <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                        <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="TotCalls" SortExpression="TotCalls" HeaderTooltip="Total visits in which Distribution check was performed" Aggregate="Sum" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Total Calls<i class='fa fa-info-circle'></i>">
                                       
                                        <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="TSales"  UniqueName ="Sales" SortExpression="TSales" Aggregate="Sum" AllowFiltering="false" ShowFilterIcon="false" HeaderText="Total Sales">
                                          <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="TCreditNote"  UniqueName ="RMA" AllowFiltering="false" ShowFilterIcon="false" Aggregate="Sum" DataFormatString="{0:N2}" HeaderText="Total Returns" SortExpression="TCreditNote">
                                        <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="TNet"  UniqueName ="TNet" AllowFiltering="false" ShowFilterIcon="false" Aggregate="Sum" DataFormatString="{0:N2}" HeaderText="Net Sales" SortExpression="TNet">
                                        <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Payment"  UniqueName ="Colelction" HeaderText="Total Collections" SortExpression="Payment" FooterText ="" Aggregate="Sum" AllowFiltering="false" ShowFilterIcon="false">
                                          <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="RouteAdh" HeaderText="JP Adherence %<i class='fa fa-info-circle'></i>" AllowFiltering="false" ShowFilterIcon="false" SortExpression="RouteAdh" HeaderTooltip="Adherence to route plan">
                                         <HeaderStyle HorizontalAlign="Center"  Wrap ="true" />
                                        <ItemStyle HorizontalAlign="Right"  Wrap ="false" />
                                        <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                    </telerik:GridBoundColumn>
                                      <telerik:GridTemplateColumn HeaderText ="Van/FSR" Visible ="false"  >
                                        <HeaderTemplate >
                                            

                                        </HeaderTemplate>
                                        <ItemTemplate >
                                            
                                        </ItemTemplate>
                                       <FilterTemplate >
                          

                        <asp:ImageButton ID="btnShowAll" runat="server" ImageUrl="~/assets/img/close.jpg" AlternateText="Clear filter"
                            ToolTip="Clear filter" OnClick="btnShowAll_Click" Style="vertical-align: middle" />
                        </FilterTemplate>
                                    </telerik:GridTemplateColumn>


                                </Columns>
                            </MasterTableView>


                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="sqlLog" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_SMTop5VanSales" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfRow" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSE" Name="SMID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="MonthYEAR" />
                               
                            </SelectParameters>

                        </asp:SqlDataSource>

                    </telerik:RadAjaxPanel>
            </div>
                
        <%--</div> --%>
            </div>
    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportLog" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportTop10" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel_TA" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress1"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>

    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
        <asp:Label ID="Label2" runat="server" ForeColor="#923137">Loading... </asp:Label>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Progress.gif" ImageAlign="Middle" />
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
