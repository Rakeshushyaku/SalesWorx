<%@ Page Title="Monthly Business Report - Overall" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepMBRReport.aspx.vb" Inherits="SalesWorx_BO.RepMBRReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
     
    <style>

        .RadPivotGrid_Default .rpgContentZoneDiv th.targetcls
        {
            border-bottom-color: #ff7663 !important;
            border-bottom-width: 3px;
        }
        .RadPivotGrid_Default .rpgContentZoneDiv th.salescls
        {
            border-bottom-color: #10c4b2 !important;
            border-bottom-width: 3px;
        }
        input[type="text"].rdfd_
        {
            margin: 0 !important;
            padding: 0 !important;
            height: 0 !important;
            width: 0 !important;
        }
        /*.RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}*/
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
        .widgetblk .text-primary {
    font-size: 26px;
    font-weight: bold;
    color: #fff;
    text-align: right;
}
        #chart1.k-chart svg{
	margin:0 -14px;
}
    </style>
    <script type="text/javascript">
         
         
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

        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }

        function OnClientDropDownClosedHandler(sender, eventArgs) {
            $("#MainContent_rpbFilter_i0_dummyOrgBtn").click()
        }

        function RefreshChart() {

       


        }
    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }

           #MainContent_summary .col-sm-4:nth-child(1) .widgetblk, #MainContent_summary .col-sm-6:nth-child(1) .widgetblk {
    background-color:#ff7663 ;
}
        #MainContent_summary .col-sm-4:nth-child(2) .widgetblk, #MainContent_summary .col-sm-6:nth-child(2) .widgetblk {
    background-color:#10c4b2 ;
}
        #MainContent_summary .col-sm-4:nth-child(3) .widgetblk, #MainContent_summary .col-sm-6:nth-child(3) .widgetblk {
    background-color: #14b4fc;
}
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Monthly Business Report - Overall</h4>
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

    <%--   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                          <ContentTemplate >--%>
    <telerik:RadAjaxPanel ID="l" runat="server">
        <asp:HiddenField ID="hfCurrency" runat="server" />
        <asp:HiddenField ID="hfDecimal" runat="server" />
        <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
            ExpandMode="MultipleExpandedItems">
            <Items>

                <telerik:RadPanelItem Expanded="True" Text=" ">

                    <ContentTemplate>

                        <div class="row">
                            <%-- <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server" >--%>
                            <div class="col-sm-10 col-md-10 col-lg-10">
                                <div class="row">
                                    <div class="col-sm-4" runat="server" id="dvCountry">
                                        <div class="form-group">
                                            <label>Country</label>
                                            <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID" OnClientDropDownClosed="OnClientDropDownClosedHandler" AutoPostBack="true">
                                            </telerik:RadComboBox>

                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Organization<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Skin="Simple" Filter ="Contains"  EmptyMessage="Select Sales Org" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            <asp:Button ID="dummyOrgBtn" runat="server" Style="display: none" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                CheckBoxes="true" EmptyMessage="Select a Van/FSR" ID="ddlVan" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4" runat ="server" id="divAgency">
                                        <div class="form-group">
                                            <label>Agency<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                EmptyMessage="Select Agency" ID="ddlAgency" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Month From</label>
                                            
                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="StartTime" runat="server" Visible="true"  Skin="Simple" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Month To</label>
                                             
                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="EndTime" runat="server" Visible="true"  Skin="Simple" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                        </div>
                                    </div>

                                </div>
                                <%--  </telerik:RadAjaxPanel> --%>
                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                    <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                </div>
                                <div class="form-group fontbig text-center">
                                    <label>&nbsp;</label>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

                                </div>
                            </div>
                        </div>




                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>
        <asp:HiddenField ID="hfOrgID" runat="server" />
        <asp:HiddenField ID="hfVans" runat="server" />
        <asp:HiddenField ID="hfAgency" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
       <asp:HiddenField ID="HTargetType" runat="server" />

        <div id="Args" runat="server" visible="false">
            <div id="popoverblkouter">
                Hover on icon to view search criteria <i class="fa fa-info-circle">
                    <span class="popoverblk">
                        <p>
                            <strong>Country: </strong>
                            <asp:Label ID="lbl_Country" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Organisation: </strong>
                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Van: </strong>
                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Agency: </strong>
                            <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Month From: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Month To: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>
                    </span>
                </i>
            </div>
        </div>


        <%--  <div id="summary" runat="server" class="row">--%>
        <div id="rpt" runat="server" visible="false" style="padding-top: 5px">

             <div class="row" id="MainContent_summary">
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Target Cumulative (<asp:Label ID="lblTargetCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblTarget" runat="server"></asp:Label></div>
                    
                </div>
            </div>
            <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Sales Cumulative (<asp:Label ID="lblSalesCurr" runat="server"></asp:Label>)<div class="text-primary"><asp:Label ID="lblSales" runat="server"></asp:Label></div>
                
                </div>
            </div>
         <div class="col-sm-4">
                <div class="widgetblk widgetblkinsmall">Total Vans/FSR <div class="text-primary"><asp:Label ID="lblTeamSize" runat="server"></asp:Label></div>
                   
                </div>
            </div>
        </div>

            <h5 class="text-right">Currency <span class="text-blue"><strong>
                <asp:Label runat="server" ID="lblC"></asp:Label></strong></span></h5>
            <div class="table-responsive">
                <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>

                        <telerik:RadTab Text="Summary" runat="server">

                        </telerik:RadTab>
                        

                        <telerik:RadTab Text="Target vs Sales By Agency" runat="server">
                        </telerik:RadTab>
                        
                       <%--   <telerik:RadTab Text="Table" runat="server">
                               
                        </telerik:RadTab>--%>
                        <telerik:RadTab Text="Target vs Sales By Month" runat="server">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                 

                <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                    <telerik:RadPageView ID="RView1" runat="server">
                         <br />

                        <telerik:RadPanelBar runat="server" ID="RadPanelBar1" Skin="Default" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" " BorderStyle="None">

                                    <ContentTemplate>

                                        <div class="chart-wrapper overflowx">
                                            <div id="chart2">
                                                 <telerik:RadHtmlChart runat="server" ID="RadHtmlSummary"  Height="300px"  
                                                     ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  Legend-Appearance-Orientation="Vertical"  Legend-Appearance-Position="Top" 
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="true"  PlotArea-XAxis-MajorGridLines-Visible="false"   Transitions="true" Skin="Silk"
                                                       >
                                            <PlotArea   >
                                                <Series>
                                                    <telerik:LineSeries Name="Call Productivity %"  DataFieldY="PercentCP"  >
                                                        <Appearance FillStyle-BackgroundColor="#2DABC1"></Appearance>
                                                        <TooltipsAppearance Color="White"  >
                                                            <ClientTemplate>
                                                              Call Productivity % : #=dataItem.PercentCP#
                                                            </ClientTemplate>
                                                        </TooltipsAppearance>
                                                        <LabelsAppearance Visible="false"></LabelsAppearance>
                                                    </telerik:LineSeries>
                                                    <telerik:LineSeries Name="Growth Over Last Year %" DataFieldY="PercentGR">
                                                        <Appearance FillStyle-BackgroundColor="#FF7663"></Appearance>
                                                        <TooltipsAppearance Color="White" >
                                                            <ClientTemplate>
                                                              Growth Over Last Year % : #=dataItem.PercentGR#
                                                            </ClientTemplate>
                                                        </TooltipsAppearance>
                                                        <LabelsAppearance Visible="false"></LabelsAppearance>
                                                    </telerik:LineSeries>
                                                    <telerik:LineSeries Name="JP adherence %"  DataFieldY="PercentJP">
                                                         <Appearance FillStyle-BackgroundColor="#8AC24E"></Appearance>
                                                        <TooltipsAppearance Color="White">
                                                            <ClientTemplate>
                                                              JP adherence % : #=dataItem.PercentJP#
                                                            </ClientTemplate>
                                                        </TooltipsAppearance>
                                                        <LabelsAppearance Visible="false"></LabelsAppearance>
                                                    </telerik:LineSeries>
                                                    <telerik:LineSeries Name="Outlet Productivity %"  DataFieldY="PercentOP">
                                                           <Appearance FillStyle-BackgroundColor="Red"></Appearance>
                                                        <TooltipsAppearance Color="White" >
                                                            <ClientTemplate>
                                                              Outlet Productivity % : #=dataItem.PercentOP#
                                                            </ClientTemplate>
                                                        </TooltipsAppearance>
                                                        <LabelsAppearance Visible="false"></LabelsAppearance>
                                                    </telerik:LineSeries>
                                                </Series>
                                                <XAxis  DataLabelsField="Month">
                                                    <TitleAppearance Text="Month" >
                                                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" Bold="true"  />
                                                        </TitleAppearance>
                                                    
                                                </XAxis>
                                                <YAxis MaxValue="103" MinValue="0">
                                                    <LabelsAppearance DataFormatString="{0}%" />
                                                    <TitleAppearance  >
                                                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true"  />
                                                        </TitleAppearance>
                                                </YAxis>
                                                 
                                            </PlotArea>
                                                 
                                        </telerik:RadHtmlChart> 
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                        <telerik:RadPanelBar runat="server" ID="RadPanelBar2" Skin="Default" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" " BorderStyle="None">

                                    <ContentTemplate>
                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                                            <telerik:RadGrid ID="gvSummary" AllowSorting="True" AutoGenerateColumns="true"
                                                Skin="Simple" BorderColor="LightGray"
                                                PageSize="15" AllowPaging="false" runat="server"
                                                GridLines="None" OnItemCommand="gvRep_ItemCommand">

                                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                <ClientSettings EnableRowHoverStyle="true">
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="true" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="false"
                                                    PageSize="15" CommandItemDisplay="Top" >
                                                        <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                                                    <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                                    <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                                    <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>


                                                </MasterTableView>
                                            </telerik:RadGrid>
                                            <br />
                                            <div class="row">
                                                 <div class="col-sm-3"><b>*  Total</b></div>
                                                 <div class="col-sm-3"><b>**  Unique</b></div>
                                                <div class="col-sm-3"><b>***  Average </b></div>
                                                
                                                
                                            </div>
                                        </telerik:RadAjaxPanel>
                                    </ContentTemplate>
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                    </telerik:RadPageView>
                    

                    <telerik:RadPageView ID="RadPageView2" runat="server">

                        <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                             <br />
                            <div style="position:relative;">
                            <div style="position:absolute;padding:5px;top:0;left:0;">
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"    OnClientClick="clickExportBiffExcel()"></asp:ImageButton>
                            </div>

                              <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true"    
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"  />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Agency" CellStyle-Width ="100px" ZoneIndex="0"  Caption="Agency" UniqueName="Agency" >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Month" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField DataField="Target" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Sales" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>
                             </div>
                        </telerik:RadAjaxPanel>

                    </telerik:RadPageView>
                   

                    <telerik:RadPageView ID="RadPageView1" runat="server">
                        <br />
                       
                        <telerik:RadGrid id="gvtargetSales" AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="60%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="false" runat="server" 
                                GridLines="None" OnItemCommand="gvtargetSales_ItemCommand">
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="70%" GridLines="None" BorderColor="LightGray"
                    PageSize="10" CommandItemDisplay="Top" >
                                                        <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                                       
                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Month" HeaderText="Month"
                                                                  SortExpression ="Month" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                             
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="target" HeaderText="Target"
                                                                  SortExpression ="target" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales" HeaderText="Sales"
                                                                  SortExpression ="Sales" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                               
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                         <br />

                         <div class="chart-wrapper overflowx">
                            <div id="chart1">
                                
 <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Target vs Sales"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"   ID="Chart" Height="300" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="Target" Name="Target"  >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor="#ff7663"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Target :   #=dataItem.Target#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:ColumnSeries DataFieldY="Sales" Name="Sales">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor= "#10c4b2" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Sales :   #=dataItem.Sales#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                </Series>
                <XAxis DataLabelsField="Month" >
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="Value" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>
            </PlotArea>
            <Legend>
                <Appearance Visible="true" Position="Top"></Appearance>
            </Legend>
           <%-- <ChartTitle Text="Outlets Covered vs Billed">
            </ChartTitle>--%>
        </telerik:RadHtmlChart>

                            </div>
                        </div>
                       

                    </telerik:RadPageView>

                         <%--  <telerik:RadPageView ID="RadPageView3" runat="server">
                                 <div id="summary" runat="server"></div>
                               </telerik:RadPageView> --%>

                </telerik:RadMultiPage>

                <%--  </div> --%>
            </div>
        </div>
    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
          <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>


    <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>
</asp:Content>
