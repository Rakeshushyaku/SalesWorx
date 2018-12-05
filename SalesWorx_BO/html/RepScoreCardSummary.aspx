<%@ Page Title="Score Card Summary By Van" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepScoreCardSummary.aspx.vb" Inherits="SalesWorx_BO.RepScoreCardSummary" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script src="../scripts/kendo.all.min.js"></script>
    <script src="../scripts/kendo.dataviz.min.js"></script>
    <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />


    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock19">

        <script type="text/javascript">

            function pageLoad(sender, args) {
                $('.rgMasterTable').find('th > a').attr("data-container", "body");
                $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
                $('[data-toggle="tooltip"]').tooltip();
            }

            function onDataBound(e) {
                $("#MainContent_Panel1").hide()

            }
          


        </script>

    </telerik:RadScriptBlock>


    <style>
        #MainContent_summary .col-sm-4:nth-child(1) .widgetblk, #MainContent_summary .col-sm-6:nth-child(1) .widgetblk
        {
            background-color: #ff7663;
        }
        .col-lg-2-1 {
    width: 23% !important;
}
        #MainContent_summary .col-sm-4:nth-child(2) .widgetblk, #MainContent_summary .col-sm-6:nth-child(2) .widgetblk
        {
            background-color: #10c4b2;
        }

        #MainContent_summary .col-sm-4:nth-child(3) .widgetblk, #MainContent_summary .col-sm-6:nth-child(3) .widgetblk
        {
            background-color: #14b4fc;
        }

        .rgGroupHeader td p
        {
            display: inline;
            margin: 0;
            padding: 0 10px;
            color: #000 !Important;
            font-weight: bold !Important;
        }

        .RadGrid_Simple .rgCommandRow
        {
            background: whitesmoke;
            color: #000;
            /* height: 15px !important; */
        }

        .rgGroupHeader td
        {
            padding-left: 8px;
            padding-bottom: 2px;
            background-color: #eff9ff !Important;
            color: #000 !Important;
        }

        div[id*="ReportDiv"]
        {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }

        .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            font-size: 13px;
            font-weight: bold;
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #eff9ff !Important;
            font-weight: bold !Important;
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
    </style>
    <script type="text/javascript">
        function RefreshChart() {

            //createChart1();


        }

        function clickSearch() {
        
            $("#ctl00_MainContent_rpbFilter_i0_SearchBtn").click()
            return false;
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
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>


     <style type="text/css">
    div.RadGrid .rgHeader {
       white-space:nowrap;
    }
    .k-chart svg{
	margin:0 -7px;
}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4> Score Card Summary By Van/FSR </h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
          <%--    <telerik:AjaxSetting AjaxControlID="SearchBtn">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" LoadingPanelID ="lp" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>

        </AjaxSettings>
    </telerik:RadAjaxManager>

   <telerik:RadAjaxLoadingPanel ID="lp" runat="server"  InitialDelayTime ="2" MinDisplayTime ="2" >
          <asp:Panel ID="Panel2" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>
     

   </telerik:RadAjaxLoadingPanel>
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                          <ContentTemplate >
                              </ContentTemplate> 
                              </asp:UpdatePanel> --%>
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

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Organization<em><span>&nbsp;</span>*</em></label>

                                            <telerik:RadComboBox Filter="Contains" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" Skin="Simple"  ID="ddlOrganization" Width="100%" runat="server">
                                            </telerik:RadComboBox>
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
        

        <div id="Args" runat="server" visible="false">
            <div id="popoverblkouter">
                Hover on icon to view search criteria <i class="fa fa-info-circle">
                    <span class="popoverblk">

                        <p>
                            <strong>Organisation: </strong>
                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Van: </strong>
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


        <%--  <div id="summary" runat="server" class="row">--%>
        <div id="rpt" runat="server" visible="false" style="padding-top: 5px">


            <div class="row" id="MainContent_summary">
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                        Overall Target (<asp:Label ID="lblTargetCurr" runat="server"></asp:Label>)<div class="text-primary">
                            <asp:Label ID="lblTarget" runat="server"></asp:Label></div>

                    </div>
                </div>
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                        Overall Sales (<asp:Label ID="lblSalesCurr" runat="server"></asp:Label>)<div class="text-primary">
                            <asp:Label ID="lblSales" runat="server"></asp:Label></div>

                    </div>
                </div>

                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                        Overall Achievement
                        <div class="text-primary">
                            <asp:Label ID="lblAchPercent" runat="server"></asp:Label>
                            %</div>

                    </div>
                </div>

                <div class="col-sm-4 col-md-3 col-lg-2-1" style="display: none;">
                    <div class="widgetblk widgetblkinsmall">
                        Total Vans/FSR Selected
                        <div class="text-primary">
                            <asp:Label ID="lblTeamSize" runat="server"></asp:Label></div>

                    </div>
                </div>
               <h5>Time Gone <strong><span class="text-blue"><asp:Label runat="server" ID="lblTime"></asp:Label> %</span></strong></h5>
                 <h5>No.Of Working Days <strong><span class="text-blue"><asp:Label runat="server" ID="lblTotWorking"></asp:Label></span></strong></h5>
                       <h5>Days Over Working <strong><span class="text-blue"><asp:Label runat="server" ID="lblDaysOver"></asp:Label></span></strong></h5>
            </div>

            <p><br /></p>
            <div class="row">
                <div class="col-sm-4">
                    <h5>Month <strong><span class="text-blue"><asp:Label runat="server" ID="lblSelMonth"></asp:Label></span></strong></h5>
                    
                      
                </div>
             
               
                <div class="col-sm-6 text-right" id="divCurrency" runat ="server">
                    <h5>Currency <strong><span class="text-blue"><asp:Label runat="server" ID="lblC"></asp:Label></span></strong></h5>
                </div>
            </div>

            <div class="table-responsive">

                   <%-- <div style="position:relative;">
            <div style="position:absolute;padding:5px;top:0;left:0;" >
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false" 
                                         ></asp:ImageButton>
                            </div>
                            <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"    
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" cssClass="no-wrap"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="RowsAndColumns"  ColumnsSubTotalsPosition="None" />
                                                                                                        
                                                    <Fields>
                                                       
                                                       
                                                       
                                                          <telerik:PivotGridRowField DataField="Outlet" CellStyle-Width ="250px" ZoneIndex="0"  Caption="Customer" UniqueName="Outlet" >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Category" SortOrder="None" Caption ="Category">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                        
                                                         <telerik:PivotGridAggregateField DataField="LYMTD" Caption="LY MTD" Aggregate="Sum"  SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="MTD" Caption ="MTD" Aggregate="Sum" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="Percentage" Aggregate="Sum" Caption ="%Age" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                       
                                                         
                                                       
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>
           </div> --%>

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

                    <telerik:RadPageView ID="RadPageView1" runat="server">
                        <div class="overflowx" >
                            
                            <div id="chart1" class="margin-14">
                                <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Target vs Sales vs LYMTD"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"   ID="TVSChart" Height="500" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="Target" Name="Target" GroupName="g1" >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor="#ff7663"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Target :   #=dataItem.Target#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:ColumnSeries DataFieldY="Sales" Name="Sales"  GroupName="g2">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor= "#10c4b2" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Sales :   #=dataItem.Sales#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>

                    <telerik:ColumnSeries DataFieldY="PercentCP" Name="LYMTD"  GroupName="g3">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor= "#6DE4FF" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                LYMTD : #=dataItem.PercentCP#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                 
                </Series>
                <XAxis DataLabelsField="Van/FSR" >
                    <LabelsAppearance RotationAngle="60"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="Value" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>
               <%-- <AdditionalYAxes   >
                                        
                                       <telerik:AxisY  Name="Acheivement" Color="Crimson" MinValue="0" MaxValue="101"  >
                                          
                                           <TitleAppearance Text="Acheivement%"    >
                                                <TextStyle Color="Crimson" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Margin="10" />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>   --%>
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
                            

                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        
   
                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">
                            <telerik:RadGrid ID="gvVans" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="8" AllowPaging="false" runat="server" DataSourceID="SqlDataSource1" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="true" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1"
                                    PageSize="8">
                                    <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" Visible="false"  ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                       <%-- <telerik:GridTemplateColumn UniqueName="Van" DataField="Description" Aggregate="Count" 
                                            FooterText="Total : " AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" FilterControlToolTip="Type the van name and press enter"
                                            InitializeTemplatesFirst="false" HeaderText="Van" SortExpression="Description">

                                            <HeaderTemplate>
                                                <div style ="float:left;">
                                                Van
                                                    </div>
                                   <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                               
                                                <asp:Label ID="label1" runat="server" Text='<%# Eval("Description")%>'></asp:Label>
                                            </ItemTemplate>


                                            <HeaderStyle HorizontalAlign="Left" Font-Bold="true" ForeColor="#0090d9" Font-Size="13px" />
                                            <ItemStyle HorizontalAlign="left" />
                                            <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridTemplateColumn>--%>

                                        <%-- <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Van" AutoPostBackOnFilter ="true" 
                            SortExpression="Agency"  Aggregate ="Count" FooterText ="Total : "  AllowFiltering ="true" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the van name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>--%>

                                         <telerik:GridTemplateColumn uniqueName="Agency"  HeaderStyle-HorizontalAlign="Left"   ShowFilterIcon ="false"  AllowFiltering ="false"
                                               HeaderStyle-Wrap="false" DataField="Agency" SortExpression ="Agency"
                                                                HeaderText="Van/FSR"  Aggregate ="Count" FooterText ="Total : " >
                                                            <ItemTemplate>
                                                            
                                                                <asp:HiddenField runat="server" ID="HSID" Value='<%# Bind("SRID")%>' />
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Agency")%>' ForeColor="SteelBlue" Font-Underline="true"  OnClick="ViewDetails_Click" Width="100%"  ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>



                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="TargetValue" HeaderText="Target<i class='fa fa-info-circle'></i>"
                                            SortExpression="TargetValue" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Target value defined for the month">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="SalesValue" HeaderText="Sales<i class='fa fa-info-circle'></i>"
                                            SortExpression="SalesValue" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Target achieved for the month">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Achievement" HeaderText="Achievement %<i class='fa fa-info-circle'></i>" AllowFiltering="false"
                                            SortExpression="Achievement" UniqueName="Achievement" DataFormatString="{0:f0}" FooterAggregateFormatString="{0:N0}" HeaderTooltip="Percentage of Target achieved">
                                          <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="DailyTGT" HeaderText="Daily Tgt<i class='fa fa-info-circle'></i>"
                                            SortExpression="DailyTGT" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Daily target">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="LYMTD" HeaderText="LYMTD sales<i class='fa fa-info-circle'></i>"
                                            SortExpression="LYMTD" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Last year month to date sales">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="YTDTarget" HeaderText="YTD Tgt<i class='fa fa-info-circle'></i>"
                                            SortExpression="YTDTarget" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Current year to date target">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="YTDSales" HeaderText="YTD Ach<i class='fa fa-info-circle'></i>"
                                            SortExpression="YTDSales" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Current year to date sales">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="YTDAch" HeaderText="YTD Ach %<i class='fa fa-info-circle'></i>"
                                            SortExpression="YTDAch" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Percentage of Target achieved YTD">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Variance" HeaderText="Variance<i class='fa fa-info-circle'></i>"
                                            SortExpression="Variance" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="YTD Variance">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_ScoreCardTargetvsSales" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                                <asp:Parameter Name="Mode" DefaultValue="Grid" />

                            </SelectParameters>

                        </asp:SqlDataSource>

                    </telerik:RadPageView>
                </telerik:RadMultiPage>





            </div>
        </div>
    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
        <asp:Label runat="server" ForeColor="White" Font-Size ="1px" ID="lblC1"></asp:Label>
    </div>


    <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10" 
        runat="server"  >
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
