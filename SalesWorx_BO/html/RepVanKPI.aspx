<%@ Page Title="Van KPI Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepVanKPI.aspx.vb" Inherits="SalesWorx_BO.RepVanKPI" %>

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
    <style>
        .no-wrap.RadPivotGrid_Default .rpgContentZoneDiv th, .no-wrap.RadPivotGrid_Default .rpgRowHeader {
    white-space: normal !important;
    }
       
    </style>

    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock19">

        <script type="text/javascript">

            function PivotLoadShow() {
                $("#MainContent_rpt").show();
            }
            function PivotLoadHide() {
                $("#MainContent_rpt").hide();
            }
            function pageLoad(sender, args) {
                $('.rgMasterTable').find('th > a').attr("data-container", "body");
                $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
                $('[data-toggle="tooltip"]').tooltip();
                $('#ctl00_MainContent_gvRep_ctl00_RowZone1').text('No');
                $('#ctl00_MainContent_gvRep_ctl00_RowZone2').text('Van KPI');
                $('#ctl00_MainContent_gvRep_ctl00_RowZone3').text('Target');


            }

            function onDataBound(e) {
                $("#MainContent_Panel1").hide();
            }

            function onPivotGrid(sender, args) {
                //$telerik.$(".rpgCollapse", sender).attr("onclick", "");


            }

        </script>

    </telerik:RadScriptBlock>


    <style>
        #MainContent_summary .col-sm-4:nth-child(1) .widgetblk, #MainContent_summary .col-sm-6:nth-child(1) .widgetblk
        {
            background-color: #ff7663;
        }

        .RadPivotGrid_Default .rpgIcon
        {
            color: #333;
            visibility: hidden !important;
            padding: 0px !important;
            margin: -5px !important;
            /* text-align: left; */
        }

        .RadPivotGrid_Default th, .RadPivotGrid_Default td, .RadPivotGrid_Default .rpgOuterTableWrapper, .RadPivotGrid_Default .rpgContentZoneDiv td
        {
            border-color: #d9d9d9;
            font-weight: 500 !important;
        }


        html .RadPivotGrid .rpgRowsZone, html .RadPivotGrid .rpgRowHeaderZone, html .RadPivotGrid .rpgPagerCell, html .RadPivotGrid .rpgDataZone, html .RadPivotGrid .rpgFilterZone, html .RadPivotGrid .rpgHorizontalScroll {
    border-left: 0 none;  font-weight: 500 !important;
  color:white !important;
    background-color: #549F3B !important;
    border-right: #fff solid 1px;
text-align: center;
}
        .no-wrap.RadPivotGrid_Default .rpgContentZoneDiv th, .no-wrap.RadPivotGrid_Default .rpgRowHeader
        {
            white-space: nowrap;
            font-weight: 500 !important;
            background-color: aliceblue  !important;
            color: #0090d9 !important;
        }

        .col-lg-2-1
        {
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
        div.RadGrid .rgHeader
        {
            white-space: nowrap;
        }

        .k-chart svg
        {
            margin: 0 -7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Van/FSR KPI Report </h4>
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

    <telerik:RadAjaxLoadingPanel ID="lp" runat="server" InitialDelayTime="2" MinDisplayTime="2">
        <asp:Panel ID="Panel2" CssClass="overlay" runat="server" Style="display: none">
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

                                            <telerik:RadComboBox Filter="Contains" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" Skin="Simple" ID="ddlOrganization" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                CheckBoxes="true" EmptyMessage="Select a van" ID="ddlVan" Width="100%" runat="server">
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
        <div id="rpt" runat="server"  style="padding-top: 5px;display:none">



            <div class="row">
                <div class="col-sm-4">
                    <h5>Month <strong><span class="text-blue">
                        <asp:Label runat="server" ID="lblSelMonth"></asp:Label></span></strong></h5>


                </div>



            </div>
            

              <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="false" PageSize="10" EnableViewState="true"
                        ID="gvRep" runat="server"  
                        ShowFilterHeaderZone="false" AllowFiltering="false" RowGroupsDefaultExpanded="true" ShowColumnHeaderZone="false"
                         ShowRowHeaderZone="false" ShowDataHeaderZone="false" CssClass="no-wrap">
                        <TotalsSettings GrandTotalsVisibility="None"  RowsSubTotalsPosition="None" ColumnsSubTotalsPosition="None"  />
                        
                        <Fields>
                            
                            
                            <telerik:PivotGridRowField DataField="Sequence"
                                CellStyle-ForeColor="#0090d9" IsHidden="false" CellStyle-Width="40px" ZoneIndex="0" SortOrder="Ascending"
                                Caption="No" UniqueName="Sequence">
                            </telerik:PivotGridRowField>

                            <telerik:PivotGridRowField DataField="Parameter" CellStyle-ForeColor="#0090d9" IsHidden="false" CellStyle-Width="210px" ZoneIndex="0" Caption="Van/FSR KPI" UniqueName="Parameter">
                            </telerik:PivotGridRowField>
                            <telerik:PivotGridRowField DataField="DefaultValue" CellStyle-ForeColor="#0090d9" CellStyle-Width="180px" ZoneIndex="0" Caption="Target" UniqueName="DefaultValue">
                            </telerik:PivotGridRowField>

                            <telerik:PivotGridColumnField DataField="Van" CellStyle-BackColor="Red" SortOrder="none" Caption="Van/FSR">
                            </telerik:PivotGridColumnField>


                            <telerik:PivotGridAggregateField    DataField="ActualValue" CellStyle-ForeColor="DarkOliveGreen"  
                                DataFormatString="{0:N0}" Caption="ActualValue" SortOrder="None"   >
                                   
                            </telerik:PivotGridAggregateField>




                        </Fields>
                        <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                    </telerik:RadPivotGrid>



          
        </div>
       
    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
        <asp:Label runat="server" ForeColor="White" Font-Size="1px" ID="lblC1"></asp:Label>
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
