<%@ Page Title="Target vs Sales By Van" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepTargetvsSalesByVan.aspx.vb" Inherits="SalesWorx_BO.RepTargetvsSalesByVan" %>

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
            function createChart1() {
                var param = document.getElementById('<%= hfOrgID.ClientID%>').value;
                var param1 = document.getElementById('<%= hfVans.ClientID%>').value;
                var param2 = document.getElementById('<%= hfSMonth.ClientID%>').value;
                var currency = document.getElementById('<%= lblC1.ClientID%>').innerHTML;

                $("#MainContent_Panel1").show()

                $("#chart1").kendoChart({
                    theme: $(document).data("kendoSkin") || "flat",
                    dataBound: onDataBound,
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: "Chart.asmx/getTargetvsSalesByVan", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                dataType: "json"
                            }
,
                            parameterMap: function (data, operation) {

                                var output = null;
                                switch (operation) {

                                    case "read":
                                        output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + '}';
                                        break;

                                }
                                return output;
                                return kendo.stringify(data); // stringify the data send back to the server
                            }
                        },
                        schema: {
                            data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                            model: { // define the model of the data source.
                                fields: {


                                    Unit: { type: "number" },
                                    DispOrder: { type: "number" },
                                    AgencyName: { type: "number" },
                                    Count: { type: "number" },
                                    Description: { type: "string" }




                                }
                            }
                        },




                    },






                    chartArea: {
                        background: "#fff",

                    },
                    title: {
                        text: "",
                        font: "18px Segoe UI"

                    },
                    legend: {
                        position: "top",

                        labels: {
                            font: "12px Segoe UI", color: "#333",
                            background: "black"

                        }

                    },



                    valueAxis: [{
                        name: "Unit",
                        color: "#ff7663",


                        title: { text: "Target (" + currency + ")", font: "bold 12px Segoe UI", color: "#ff7663" },
                        labels: {
                            font: "bold 12px Segoe UI", color: "#ff7663"
                        }
                    },

                    {
                        name: "DispOrder",

                        color: "#10c4b2",

                        title: { text: "Sales (" + currency + ")", font: "bold 12px Segoe UI", color: "#10c4b2" },
                        labels: {
                            font: "bold 12px Segoe UI", color: "#10c4b2"
                        }
                    },


                    {
                        name: "AgencyName",
                        min: 0,
                        max: 100,
                        majorunit: 10,
                        color: "dodgerblue",

                        title: { text: "Achievement %", font: "bold 12px Segoe UI", color: "dodgerblue" },
                        labels: {
                            font: "bold 12px Segoe UI", color: "dodgerblue"
                        }
                    }
                    ]

                    ,
                    categoryAxis:
{
    field: "Description",
    title: { text: "Vans", font: "bold 12px  Segoe UI", color: "#4d5ec1" },
    color: "#4d5ec1",
    axisCrossingValues: [0, 100, 100, 0],
    labels: {
        font: "bold 12px  Segoe UI", color: "#4d5ec1"

    },

    majorGridLines: {
        visible: true
    }
},
                    seriesColors: ["#ff7663", "#10c4b2"],

                    series: [{
                        field: "Unit",
                        type: "column",

                        axis: "Unit",
                        tooltip: {
                            visible: true,

                            template: "Van : #= dataItem.Description#  <br> Target : " + currency + "  #=kendo.format('{0:N0}',dataItem.Unit)#",
                            font: "12px Segoe UI",
                            color: "white",
                            background: "black"
                        },
                        labels: {

                            font: "bold 12px  Segoe UI", color: "#ff7663"
                        }

                    },

                    {
                        field: "DispOrder",
                        type: "column",

                        axis: "DispOrder",
                        tooltip: {
                            visible: true,
                            template: "Van : #= dataItem.Description#  <br> Sales : " + currency + "  #=kendo.format('{0:N0}',dataItem.DispOrder)#",
                            font: "12px Segoe UI",
                            color: "white",
                            background: "black"
                        },
                        labels: {
                            font: "bold 12px  Segoe UI", color: "#10c4b2"
                        }

                    },
                     {
                         field: "AgencyName",
                         type: "line",
                         style: "smooth",
                         width: "1px",
                         axis: "AgencyName",
                         color: "dodgerblue",
                         tooltip: {
                             visible: true,
                             template: "Van : #= dataItem.Description#  <br> Achievement % : #=dataItem.AgencyName# ",
                             font: "12px Segoe UI",
                             color: "white",
                             background: "black"
                         },
                         labels: {
                             font: "bold 12px  Segoe UI", color: "dodgerblue"
                         }

                     }
                    ],


                    tooltip: {
                        visible: true,
                        format: "{0:N0}",
                        font: "12px Segoe UI",
                        color: "white",
                        background: "black"
                    }
                });
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
    <h4> <asp:Label runat ="server" ID="lblTitle" ></asp:Label> </h4>
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
                                                CheckBoxes="true" EmptyMessage="Select a van/FSR" ID="ddlVan" Width="100%" runat="server">
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
            </div>

            <p><br /></p>
            <div class="row">
                <div class="col-sm-6">
                    <h5>Month <strong><span class="text-blue"><asp:Label runat="server" ID="lblSelMonth"></asp:Label></span></strong></h5>
                </div>
                <div class="col-sm-6 text-right" id="divCurrency" runat ="server">
                    <h5>Currency <strong><span class="text-blue"><asp:Label runat="server" ID="lblC"></asp:Label></span></strong></h5>
                </div>
            </div>

            <div class="table-responsive">

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
                                <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Target vs Sales"
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
                    <telerik:ColumnSeries DataFieldY="Sales" Name="Sales"  GroupName="g1">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" FillStyle-BackgroundColor= "#10c4b2" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Sales :   #=dataItem.Sales#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:LineSeries Name="Acheivement %"  DataFieldY="PercentCP"  AxisName="Acheivement" >
                                                        <Appearance FillStyle-BackgroundColor="Crimson"></Appearance>
                                                        <TooltipsAppearance Color="White"  >
                                                            <ClientTemplate>
                                                              Acheivement % : #=dataItem.PercentCP#
                                                            </ClientTemplate>
                                                        </TooltipsAppearance>
                                                        <LabelsAppearance Visible="false"></LabelsAppearance>
                                                    </telerik:LineSeries>
                </Series>
                <XAxis DataLabelsField="Van" >
                    <LabelsAppearance RotationAngle="60"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="Value" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>
                <AdditionalYAxes   >
                                        
                                       <telerik:AxisY  Name="Acheivement" Color="Crimson" MinValue="0" MaxValue="101"  >
                                          
                                           <TitleAppearance Text="Acheivement%"    >
                                                <TextStyle Color="Crimson" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Margin="10" />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>   
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
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1" CommandItemDisplay="Top"
                                    PageSize="8">
                                    <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
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

                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Van/FSR" AutoPostBackOnFilter ="true" 
                            SortExpression="Description"  Aggregate ="Count" FooterText ="Total : "  AllowFiltering ="true" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the van/FSR name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>



                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="TotValue" HeaderText="Target<i class='fa fa-info-circle'></i>"
                                            SortExpression="TotValue" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Target value defined for the month">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="DispOrder" HeaderText="Sales<i class='fa fa-info-circle'></i>"
                                            SortExpression="DispOrder" Aggregate="Sum" AllowFiltering="false" HeaderTooltip="Target achieved for the month">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="MnOrder" HeaderText="Achievement %<i class='fa fa-info-circle'></i>" AllowFiltering="false"
                                            SortExpression="MnOrder" UniqueName="MnOrder" DataFormatString="{0:f0}" FooterAggregateFormatString="{0:N0}" HeaderTooltip="Percentage of Target achieved">
                                          <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_TargetvsSalesByVan" SelectCommandType="StoredProcedure">

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
