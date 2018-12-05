<%@ Page Title="Distribution Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepDistribution.aspx.vb" Inherits="SalesWorx_BO.RepDistribution" %>

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

            function onDataBound(e) {
                $("#MainContent_Panel1").hide()

            }

            function createChart1() {
                var param = document.getElementById('<%= hfOrgID.ClientID%>').value;
                var param1 = document.getElementById('<%= hfVans.ClientID%>').value;
                var param2 = document.getElementById('<%= hfAgency.ClientID%>').value;
                var param3 = document.getElementById('<%= hfBrand.ClientID%>').value;
                var param4 = document.getElementById('<%= hfSKU.ClientID%>').value;
                var param5 = document.getElementById('<%= hfSMonth.ClientID%>').value;
                var param6 = document.getElementById('<%= hfEMonth.ClientID%>').value;
                var param7 = document.getElementById('<%= hfGroupBy.ClientID%>').value;
                var param8 = document.getElementById('<%= hfShownBy.ClientID%>').value;

                $("#MainContent_Panel1").show()

                $("#chart1").kendoChart({
                    theme: $(document).data("kendoSkin") || "flat",
                    dataBound: onDataBound,
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: "Chart.asmx/getMonthlyDistribution", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                dataType: "json"
                            }
,
                            parameterMap: function (data, operation) {

                                var output = null;
                                switch (operation) {

                                    case "read":
                                        output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ',param6: ' + JSON.stringify(param6) + ',param7: ' + JSON.stringify(param7) + ',param8: ' + JSON.stringify(param8) + '}';
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

                                    AgencyName: { type: "string" },
                                    Unit: { type: "number" },
                                    DispOrder: { type: "string" },
                                    Description: { type: "string" },
                                    MonthYear: { type: "date" }



                                }
                            }
                        },

                        group: { field: "Description" },
                        sort: [{ field: "MonthYear", dir: "asc" }]



                    },






                    chartArea: {
                        background: "#fff"
                    },
                    title: {
                        text: "",
                        font: "18px Segoe UI"

                    },
                    legend: {
                        position: "right",
                        title: { text: "No.of Invoices", font: "bold 12px Segoe UI", color: "dodgerblue" },
                        labels: {

                            font: "12px Segoe UI", color: "#333",
                            background: "black"

                        }

                    },

                    seriesColors: ["DarkoliveGreen", "Dodgerblue", "Darkmagenta", "Crimson", "#00FFFF", "#00FF00", "#0000FF", "#2F4F4F", "#4682B4", "#800080", "#C71585", "#00CED1", "#66CDAA", "#9ACD32", "#CD853F", "#FFA07A", "#A52A2A", "#696969", "#000000", "#708090", "#191970", "#DDA0DD", "#DB7093", "#20B2AA", "#00FA9A", "#BDB76B", "#FFD700", "#FFDEAD", "#FF6347", "#FF0000", "#006400", "#808000", "#FFFF00"],
                    seriesDefaults: {
                        type: "line",
                        style: "smooth",
                        missingValues: "interpolate",
                        width: "1px",

                        labels: {
                            visible: false,
                            format: "{0:N0}",
                            font: "12px Segoe UI", color: "#333"


                        }

                    },

                    valueAxis: {
                        field: "Unit",
                        title: { text: "Count", font: "bold 12px Segoe UI", color: "#1c9ec4" },
                        line: {
                            visible: false
                        },
                        minorGridLines: {
                            visible: false
                        },
                        majorGridLines: {
                            visible: true
                        },
                        labels: {
                            font: "12px Segoe UI", color: "#333"
                        }
                    },
                    categoryAxis:
{
    field: "AgencyName",

    title: { text: "Month", font: "bold 12px  Segoe UI", color: "#4d5ec1" },
    labels: {
        font: "12px Segoe UI", color: "#333"

    },

    majorGridLines: {
        visible: true
    }
},


                    series: [{
                        field: "Unit",

                        tooltip: {
                            visible: true,

                            template: "#= dataItem.DispOrder# : #= dataItem.Description#  <br> Count : #=kendo.format('{0:N0}',dataItem.Unit)# ",
                            font: "12px Segoe UI",
                            color: "white",
                            background: "black"
                        },
                        labels: {
                            font: "12px Segoe UI", color: "#333"
                        }

                    }],

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

            createChart1();


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

        function OnClientDropDownClosedHandler(sender, eventArgs) {
            $("#MainContent_rpbFilter_i0_dummyOrgBtn").click()
        }


    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Distribution Report</h4>
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



    <telerik:RadAjaxPanel ID="l" runat="server">
        <asp:HiddenField ID="hfCurrency" runat="server" />
        <asp:HiddenField ID="hfDecimal" runat="server" />
        <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
            ExpandMode="MultipleExpandedItems">
            <Items>
                <telerik:RadPanelItem Expanded="True" Text=" ">

                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-10 col-md-10 col-lg-10">
                                <div class="row">
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
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
                                       <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month From</label>
                                                <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" Width="100%" ID="StartTime" runat="server">
                                                    <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                    </DateInput>

                                                </telerik:RadMonthYearPicker>



                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Agency<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                    EmptyMessage="Select Agency" ID="ddlAgency" Width="100%" runat="server" AutoPostBack="true">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Brand<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                    EmptyMessage="Select Brand" ID="ddlBrand" Width="100%" runat="server" AutoPostBack="true">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                      
                                        
                                        
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month To</label>

                                                <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" Width="100%" ID="EndTime" runat="server">
                                                    <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                    </DateInput>

                                                </telerik:RadMonthYearPicker>
                                            </div>
                                        </div>


                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Group By</label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlGroupBy" Width="100%"
                                                    runat="server">
                                                    <Items>

                                                        <telerik:RadComboBoxItem Text="Van/FSR" Value="Van"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Location" Value="Location"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Agency" Value="Agency"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Brand" Value="Brand"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="SKU" Value="SKU"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>


                                           <div class="col-sm-8">
                                            <div class="form-group">
                                                <label>SKU</label>
                                                <%--     <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                    EmptyMessage="Select SKU" ID="ddlSKU" Width="100%" runat="server">
                                                </telerik:RadComboBox>--%>

                                               <%-- <telerik:RadComboBox ID="ddlSKU" Skin="Simple" TabIndex="1" runat="server"
                                                    Filter="Contains" EmptyMessage="Please type a SKU"
                                                    EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                    EnableLoadOnDemand="True" ShowToggleImage="true" ShowDropDownOnTextboxClick="true" ShowMoreResultsBox="true"
                                                    Height="200" Width="100%" AutoPostBack="true" />--%>

                                                 <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="ddlSKU"  EmptyMessage="Please type Item No./Description"
                 InputType="Token" Width="100%"  MinFilterLength="2" DropDownWidth="200px"   >
            </telerik:RadAutoCompleteBox>

                                            </div>
                                        </div>















                                    </telerik:RadAjaxPanel>
                                </div>

                            </div>
                            <div class="col-sm-2 col-md-2 col-lg-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                    <asp:Button CssClass="btn btn-sm btn-block btn-default" ID="ClearBtn" runat="server" Text="Clear" />
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
        <asp:HiddenField ID="hfBrand" runat="server" />
        <asp:HiddenField ID="hfSKU" runat="server" />
        <asp:HiddenField ID="hfGroupBy" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="hfShownBy" runat="server" />


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
                            <strong>Agency: </strong>
                            <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Brand: </strong>
                            <asp:Label ID="lbl_Brand" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>SKU: </strong>
                            <asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Month From: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Month To: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>
                        <p>
                            <strong>Group By: </strong>
                            <asp:Label ID="lbl_View" runat="server" Text=""></asp:Label>
                        </p>
                    </span>
                </i>
            </div>
        </div>

        <p>&nbsp;</p>

        <div id="rpt" runat="server" visible="false" class="table-responsive">
            <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="Distribution By Month / Van" runat="server">
                    </telerik:RadTab>

                    <telerik:RadTab Text="Invoices & Outlets By Van" runat="server">
                    </telerik:RadTab>


                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">

                <telerik:RadPageView ID="RadPageView1" runat="server">
                    <%-- <telerik:RadAjaxPanel runat ="server" ID="pl">--%>
                    <div style="text-align: center; padding-top: 10px;">

                        <span style="font-weight: bold;">Show by </span>
                        <telerik:RadComboBox Skin="Simple" AutoPostBack="true" ID="ddlChartMode" Width="150px"
                            runat="server">
                            <Items>

                                <telerik:RadComboBoxItem Text="Invoices" Value="Invoices"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Outlets" Value="Outlets"></telerik:RadComboBoxItem>

                            </Items>
                        </telerik:RadComboBox>


                    </div>

                    <%-- </telerik:RadAjaxPanel>--%>

                    <div class="chart-wrapper padding0" style="width: 100%;">
                        <div id="chart1" class="margin-14">
                        </div>
                    </div>


                </telerik:RadPageView>

                <telerik:RadPageView ID="RadPageView2" runat="server">

                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">

                        <telerik:RadGrid ID="gvAgency1" AllowSorting="True" AutoGenerateColumns="true" Skin="Simple" BorderColor="LightGray"
                            PageSize="9" AllowPaging="false" runat="server"
                            GridLines="None">

                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                            <ClientSettings EnableRowHoverStyle="true" AllowGroupExpandCollapse="true">
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="true" TableLayout="Auto" HierarchyDefaultExpanded="true" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="false"
                                PageSize="9">
                                <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                <GroupByExpressions>

                                    <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldName="Agy" SortOrder="Ascending"></telerik:GridGroupByField>

                                            <%--   <telerik:GridGroupByField FieldName="TargetValue" FieldAlias="Target"  HeaderValueSeparator =" &nbsp;&nbsp;&nbsp;" 
                       Aggregate="Sum" FormatString="{0:G7}" />
                                              <telerik:GridGroupByField FieldName="SalesValue"  FieldAlias="Sales"   HeaderValueSeparator =" &nbsp;&nbsp;&nbsp;" 
                         Aggregate="Sum"  FormatString="{0:G7}" />--%>
                                        </SelectFields>
                                        <GroupByFields>
                                            <telerik:GridGroupByField FieldName="Agy" SortOrder="Ascending"></telerik:GridGroupByField>

                                        </GroupByFields>
                                    </telerik:GridGroupByExpression>

                                </GroupByExpressions>

                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>

                </telerik:RadPageView>




            </telerik:RadMultiPage>
        </div>
    </telerik:RadAjaxPanel>


    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
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

    <asp:Panel ID="Panel1" CssClass="overlay" runat="server" Style="display: none">
        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
    </asp:Panel>

</asp:Content>
