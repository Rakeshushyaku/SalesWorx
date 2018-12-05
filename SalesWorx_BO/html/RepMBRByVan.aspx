<%@ Page Title="Monthly Business Report - By Van" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepMBRByVan.aspx.vb" Inherits="SalesWorx_BO.RepMBRByVan" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script src="../scripts/kendo.all.min.js"></script>f
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
                var param3 = document.getElementById('<%= hfSMonth.ClientID%>').value;
                var param4 = document.getElementById('<%= hfEMonth.ClientID%>').value;
                var param5 = document.getElementById('<%= HTargetType.ClientID%>').value;
                var currency = document.getElementById('<%= lblC.ClientID%>').innerHTML;
                var TargetType = document.getElementById('<%= HTargetType.ClientID%>').value;
                var title

                if (TargetType == "Q") {
                    title = "Qty"
                }
                else {
                    title = "Value (" + currency + ")"
                }

                $("#MainContent_Panel1").show()

                $("#chart1").kendoChart({
                    theme: $(document).data("kendoSkin") || "flat",
                    dataBound: onDataBound,
                    dataSource: {
                        type: "json",
                        transport: {
                            read: {
                                url: "Chart.asmx/getTargetvsSalesMBR", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                dataType: "json"
                            }
,
                            parameterMap: function (data, operation) {

                                var output = null;
                                switch (operation) {

                                    case "read":
                                        output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + '}';
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
                                    MonthYear: { type: "string" }



                                }
                            }
                        },

                        group: { field: "Description" },
                        sort: [{ field: "DisplayOrder", dir: "asc" }, { field: "MonthYear", dir: "asc" }]


                    },






                    chartArea: {
                        background: "#fff"
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
                    seriesColors: ["#ff7663", "#10c4b2"],

                    seriesDefaults: {
                        type: "column",

                        labels: {
                            visible: true,
                            format: "{0:N0}",
                            font: "12px Segoe UI", color: "#333"


                        },




                        width: 30,
                        gap: 1

                    },

                    valueAxis: {
                        //axisCrossingValue: 0,
                        title: { text: title, font: "bold 12px Segoe UI", color: "#1c9ec4" },
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

    title: { text: "Van", font: "bold 12px  Segoe UI", color: "#4d5ec1" },
    labels: {
        font: "12px Segoe UI", color: "#333"

    },

    majorGridLines: {
        visible: true
    }
},


                    series: [{
                        field: "Unit",
                        categoryField: "AgencyName",

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
        .RadGrid_Simple .rgFooter > td, .RadGrid_Simple .rgFooterWrapper
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            background-color: lightyellow !important;
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

        .widgetblk .text-primary
        {
            font-size: 26px;
            font-weight: bold;
            color: #fff;
            text-align: right;
        }

        #chart1.k-chart svg
        {
            margin: 0 -14px;
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

        #MainContent_summary .col-sm-4:nth-child(1) .widgetblk, #MainContent_summary .col-sm-6:nth-child(1) .widgetblk
        {
            background-color: #ff7663;
        }

        #MainContent_summary .col-sm-4:nth-child(2) .widgetblk, #MainContent_summary .col-sm-6:nth-child(2) .widgetblk
        {
            background-color: #10c4b2;
        }

        #MainContent_summary .col-sm-4:nth-child(3) .widgetblk, #MainContent_summary .col-sm-6:nth-child(3) .widgetblk
        {
            background-color: #14b4fc;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Monthly Business Report - By Van/FSR</h4>
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
                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" EmptyMessage="Select Sales Org" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
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
                                            <label>Month</label>

                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="StartTime" runat="server" Visible="true" Skin="Simple" Width="100%">
                                                <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                </DateInput>

                                            </telerik:RadMonthYearPicker>
                                        </div>
                                    </div>
                                    <div class="col-sm-4" style ="visibility:hidden;">
                                        <div class="form-group">
                                            <label>Agency<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                EmptyMessage="Select Agency" ID="ddlAgency" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>


                                    <%--   <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Month To</label>
                                             
                                            <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="EndTime" runat="server" Visible="true"  Skin="Simple" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                        </div>
                                    </div>--%>
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
        <asp:HiddenField ID="hfUID" runat="server" />
        <asp:HiddenField ID="hfVans" runat="server" />
        <asp:HiddenField ID="hfSelVan" runat="server" />
        <asp:HiddenField ID="hfAgency" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="HTargetType" runat="server" />
        <asp:HiddenField ID="hfItem" runat="server" />
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
                            <strong>Month: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>
                        <p style ="visibility:hidden;">
                            <strong>Agency: </strong>
                            <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label>
                        </p>
                        
                        <%-- <p>
                            <strong>Month To: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>--%>
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
                        Total Vans/FSR
                        <div class="text-primary">
                            <asp:Label ID="lblTeamSize" runat="server"></asp:Label></div>

                    </div>
                </div>
            </div>

            <h5 class="text-right">Currency <span class="text-blue"><strong>
                <asp:Label runat="server" ID="lblC"></asp:Label></strong></span></h5>
            <div class="table-responsive">
                <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Target vs Sales By Van" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="Target vs Sales By Agency" Visible="false" runat="server">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Target vs Sales By Van/SKU" runat="server">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Summary" runat="server">
                        </telerik:RadTab>
                        <%--   <telerik:RadTab Text="Table" runat="server">
                               
                        </telerik:RadTab>--%>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">

                    <telerik:RadPageView ID="RadPageView1" runat="server">

                        <div class="chart-wrapper overflowx">
                            <div id="chart1">
                            </div>
                        </div>


                    </telerik:RadPageView>


                    <telerik:RadPageView ID="RadPageView3" runat="server">

                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                            <div class="table-responsive">
                                <telerik:RadGrid ID="gvAgency3" AllowSorting="True" Skin="Simple" BorderColor="LightGray"
                                    PageSize="9" AllowPaging="false" runat="server" ShowFooter="true"
                                    GridLines="None">

                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true" AllowGroupExpandCollapse="true">
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" HierarchyDefaultExpanded="true" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                                        PageSize="9">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                        <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>


                                        <Columns>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                                                SortExpression="Agency" AllowFiltering="false" FooterText="Total : ">
                                                <ItemStyle Wrap="False" HorizontalAlign="left" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="left" />
                                                <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="TargetValue" HeaderText="Target Value"
                                                SortExpression="TargetValue" Aggregate="Sum" AllowFiltering="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="SalesValue" HeaderText="Sales Value"
                                                SortExpression="SalesValue" Aggregate="Sum" AllowFiltering="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </telerik:RadAjaxPanel>

                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server">

                        <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                            <div class="table-responsive">
                                <telerik:RadGrid ID="gvAgency1" AllowSorting="True" Skin="Simple" BorderColor="LightGray" 
                                    PageSize="9" AllowPaging="false" runat="server" ShowFooter="true" AllowFilteringByColumn ="true" 
                                    GridLines="None">

                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true" AllowGroupExpandCollapse="true">
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" 
                                        AllowFilteringByColumn ="true" 
                                         HierarchyDefaultExpanded="true" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                                        PageSize="9">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                        <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                        <GroupByExpressions>

                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldName="Agy" SortOrder="Ascending"></telerik:GridGroupByField>

                                                    <%--  <telerik:GridGroupByField FieldName="TargetValue" FieldAlias="Target"  HeaderValueSeparator =" &nbsp;" 
                       Aggregate="Sum" FormatString="{0:N2}" />
                                              <telerik:GridGroupByField FieldName="SalesValue"  FieldAlias="Sales"   HeaderValueSeparator =" &nbsp;" 
                         Aggregate="Sum"  FormatString="{0:N2}" />--%>
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="Agy" SortOrder="Ascending"></telerik:GridGroupByField>

                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>

                                        </GroupByExpressions>
                                        <Columns>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="SKU"
                                                SortExpression="Agency" AllowFiltering="false" FilterControlToolTip ="Type SKU or description and press enter" FooterText="Total : " ShowFilterIcon ="false" CurrentFilterFunction ="Contains" AutoPostBackOnFilter ="true" >
                                                <ItemStyle Wrap="False" HorizontalAlign="left" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="left" />
                                                <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" UniqueName ="Target" DataField="TargetValue"  ShowFilterIcon ="false" 
                                                SortExpression="TargetValue" Aggregate="Sum" AllowFiltering="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" UniqueName ="Sales" DataField="SalesValue"
                                                SortExpression="SalesValue" Aggregate="Sum" AllowFiltering="false"  ShowFilterIcon ="false" >
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="SalesQty" HeaderText ="Sales Qty"
                                                SortExpression="SalesQty" Aggregate="Sum" AllowFiltering="false"  ShowFilterIcon ="false" >
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Right" Font-Bold="true" BackColor="LightGoldenrodYellow" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridBoundColumn>
                                          <%--  <telerik:GridTemplateColumn UniqueName="SalesQty" AllowFiltering="false" DataField="SalesQty"
                                                InitializeTemplatesFirst="false" HeaderText="Sales Qty" SortExpression="SalesQty" Aggregate="Sum" FooterText=" ">


                                                <ItemTemplate>


                                                    <asp:LinkButton runat="server" ID="lblBilled" Font-Underline="true" ToolTip="View sales" Font-Bold="true"
                                                        Text='<%# Bind("SalesQty")%>' OnClick="lblBilled_Click"></asp:LinkButton>
                                                    <asp:Label runat="server" ID="lblVanID" Visible="false" Text='<%# Bind("VanID")%>'></asp:Label>

                                                </ItemTemplate>


                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                            </telerik:GridTemplateColumn>--%>



                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>

                              
                            </div>


                        </telerik:RadAjaxPanel>
                           <asp:SqlDataSource ID="SqlSummary" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                        SelectCommand="Rep_MonthlyBusinessSummaryCopy" SelectCommandType="StoredProcedure">

                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                         <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                               <asp:ControlParameter ControlID="hfAgency" Name="Agency" DefaultValue="0" />
                            <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-jan-1900" />
                            <asp:ControlParameter ControlID="hfEMonth" Name="TMonth" DefaultValue="01-jan-1900" />
                               
                                                                                 
                                 <asp:Parameter  Name="Mode" DefaultValue="Agency Details" />
                        </SelectParameters>

                    </asp:SqlDataSource>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RView1" runat="server">



                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                            <div class="overflowx">
                                <telerik:RadGrid ID="gvSummary" AllowSorting="True" AutoGenerateColumns="true"
                                    Skin="Simple" BorderColor="LightGray" Width="100%"
                                    PageSize="15" AllowPaging="false" runat="server"
                                    GridLines="None">

                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Scrolling UseStaticHeaders="true" FrozenColumnsCount="2" />
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="true" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="false"
                                        PageSize="15">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                        <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>


                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>

                        </telerik:RadAjaxPanel>

                    </telerik:RadPageView>

                    <%--  <telerik:RadPageView ID="RadPageView3" runat="server">
                                 <div id="summary" runat="server"></div>
                               </telerik:RadPageView> --%>
                </telerik:RadMultiPage>

                <%--  </div> --%>
            </div>
           <%-- <telerik:RadWindow ID="ZeroBilledWindow" Title="Sales By Product" runat="server" Modal="true" Behaviors="Move,Close"
                ReloadOnShow="true" VisibleStatusbar="false" Height="515px" BorderColor="Black" Width="700px" Skin="Windows7">
                <ContentTemplate>
                    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel4">

                        <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                            PageSize="12" AllowPaging="True" runat="server" DataSourceID="SqlZero"
                            GridLines="None">

                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                            <ClientSettings EnableRowHoverStyle="true">
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" DataSourceID="SqlZero" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                PageSize="12">
                                <Columns>

                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Product"
                                        SortExpression="Product">
                                        <ItemStyle Wrap="true" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Salesqty" HeaderText="Total Qty Sold"
                                        SortExpression="Salesqty" DataType="System.Double" DataFormatString="{0:N2}">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales" HeaderText="Total Sales"
                                        SortExpression="Sales" DataType="System.Double" DataFormatString="{0:N2}">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Returnsqty" HeaderText="Total Qty Returned"
                                        SortExpression="Returnsqty" DataType="System.Double" DataFormatString="{0:N2}">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Returns" HeaderText="Total Returns"
                                        SortExpression="Returns" DataType="System.Double" DataFormatString="{0:N2}">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />

                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Net" HeaderText="Net Sales"
                                        SortExpression="Net" DataType="System.Double" DataFormatString="{0:N2}">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />

                                    </telerik:GridBoundColumn>
                                </Columns>

                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                    <asp:SqlDataSource ID="SqlZero" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                        SelectCommand="Rep_MBRSalesBySKU" SelectCommandType="StoredProcedure">

                        <SelectParameters>
                            <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                         
                            <asp:ControlParameter ControlID="hfSMonth" Name="FromDate" DefaultValue="01-jan-1900" />
                            <asp:ControlParameter ControlID="hfEMonth" Name="ToDate" DefaultValue="01-jan-1900" />
                               <asp:ControlParameter ControlID="hfSelVan" Name="SID" DefaultValue="0" />
                            <asp:ControlParameter ControlID="hfUID" Name="UID" DefaultValue="0" />
                                                        <asp:ControlParameter ControlID="hfAgency" Name="Agency" DefaultValue="0" />
                                 <asp:ControlParameter ControlID="hfItem" Name="Item" DefaultValue="0" />
                        </SelectParameters>

                    </asp:SqlDataSource>



                </ContentTemplate>
            </telerik:RadWindow>--%>
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
