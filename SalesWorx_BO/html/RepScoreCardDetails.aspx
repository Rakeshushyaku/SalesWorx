<%@ Page Title="Score Card Details By Van" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepScoreCardDetails.aspx.vb" Inherits="SalesWorx_BO.RepScoreCardDetails" %>

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
    <h4><a href="RepScoreCardSummary.aspx" title="Back"><i class="fa fa-arrow-circle-o-left"></i></a> Score Card Details </h4>
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
      

                        <div class="row">
                            <%-- <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server" >--%>
                            <div class="col-sm-10 col-md-10 col-lg-10">
                               <div class="row">
                                   <div class="col-sm-4">
               <label> <h5>Organization</h5></label><p><strong class="text-blue"><h5><asp:Label ID="lbl_org_Text" runat="server"></asp:Label></h5></strong></p>
            </div>
            <div class="col-sm-4">
               <label> <h5>Van</h5></label><p><strong class="text-blue"><h5><asp:Label ID="lbl_Sp" runat="server"></asp:Label></h5></strong></p>
            </div>
            <div class="col-sm-4">
                <label><h5>Month</h5></label><p><strong class="text-blue"><h5><asp:Label ID="lbl_Date" runat="server"></asp:Label></h5></strong></p>
            </div>
                                   
             
        </div>
                                <%--  </telerik:RadAjaxPanel> --%>
                            </div>
                                
<div class="col-sm-2 col-md-2 col-lg-2">
                   <div class="form-group fontbig text-center">
                   <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" ToolTip ="Export Excel" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF"  ToolTip ="Export PDF"  runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                       </div> 

            </div>
         
                        </div>


        <asp:HiddenField ID="hfOrgID" runat="server" />
        <asp:HiddenField ID="hfVans" runat="server" />
        <asp:HiddenField ID="hfAgency" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        

       


        <%--  <div id="summary" runat="server" class="row">--%>
        <div id="rpt" runat="server" style="padding-top: 5px">


            <div class="row" id="MainContent_summary">
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Target (<asp:Label ID="lblTargetCurr" runat="server"></asp:Label>)<div class="text-primary">
                            <asp:Label ID="lblTarget" runat="server"></asp:Label></div>

                    </div>
                </div>
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Sales (<asp:Label ID="lblSalesCurr" runat="server"></asp:Label>)<div class="text-primary">
                            <asp:Label ID="lblSales" runat="server"></asp:Label></div>

                    </div>
                </div>

                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Achievement
                        <div class="text-primary">
                            <asp:Label ID="lblAchPercent" runat="server"></asp:Label>
                            %</div>

                    </div>
                </div>

               
               <h5>Time Gone <strong><span class="text-blue"><asp:Label runat="server" ID="lblTime"></asp:Label> %</span></strong></h5>
                 <h5>No.Of Working Days <strong><span class="text-blue"><asp:Label runat="server" ID="lblTotWorking"></asp:Label></span></strong></h5>
                       <h5>Days Left <strong><span class="text-blue"><asp:Label runat="server" ID="lblDaysOver"></asp:Label></span></strong></h5>
            </div>

          
               
              
            </div>

            <div class="table-responsive">

           

                <telerik:RadTabStrip ID="AgencyTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                      
                        <telerik:RadTab Text="Target vs Achievement By Category" runat="server">
                        </telerik:RadTab>
                          <telerik:RadTab Text="Target vs Achievement By Chain" runat="server">
                        </telerik:RadTab>
                           <telerik:RadTab Text="Target vs Achievement By Outlet" runat="server">
                        </telerik:RadTab>
                          <telerik:RadTab Text="Outlet/Category LY MTD Vs CY MTD" runat="server">
                        </telerik:RadTab>

                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                      <telerik:RadPageView ID="RadPageView2" runat="server">
                        
   
                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">
                            <telerik:RadGrid ID="gvCategory" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="15" AllowPaging="false" runat="server" DataSourceID="SqlDataSource1" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="true" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1" 
                                    PageSize="15">
                                  
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                     

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Category" 
                            SortExpression="Agency"   FooterText ="Total : "  AllowFiltering ="false" ShowFilterIcon ="false"
                             >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>


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
                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Achievement" HeaderText="Ach.%<i class='fa fa-info-circle'></i>" AllowFiltering="false"
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
                                            SortExpression="YTDAch"  AllowFiltering="false" HeaderTooltip="Percentage of Target achieved YTD">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Merchandiser" HeaderText="Merchandiser<i class='fa fa-info-circle'></i>"
                                            SortExpression="Merchandiser"  AllowFiltering="false" HeaderTooltip="Merchandiser">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_ScoreCardCategoryBreakdown" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                                <asp:Parameter Name="Mode" DefaultValue="Grid" />

                            </SelectParameters>

                        </asp:SqlDataSource>

                    </telerik:RadPageView>

                      <telerik:RadPageView ID="RadPageView3" runat="server">
                        
   
                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                            <telerik:RadGrid ID="gvChain" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="15" AllowPaging="false" runat="server" DataSourceID="SqlDataSource2" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="true" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource2"
                                    PageSize="15">
                                   
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                     

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ChainCustomer" HeaderText="Chain Name" 
                            SortExpression="ChainCustomer"   FooterText ="Total : "  AllowFiltering ="false" ShowFilterIcon ="false"
                             >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>


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
                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Achievement" HeaderText="Ach.%<i class='fa fa-info-circle'></i>" AllowFiltering="false"
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
                                            SortExpression="YTDAch"  AllowFiltering="false" HeaderTooltip="Percentage of Target achieved YTD">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Merchandiser" HeaderText="Merchandiser<i class='fa fa-info-circle'></i>"
                                            SortExpression="Merchandiser"  AllowFiltering="false" HeaderTooltip="Merchandiser">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_ScoreCardChainBreakdown" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                                <asp:Parameter Name="Mode" DefaultValue="Grid" />

                            </SelectParameters>

                        </asp:SqlDataSource>

                    </telerik:RadPageView>


                              <telerik:RadPageView ID="RadPageView4" runat="server">
                        
   
                        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel3">
                            <telerik:RadGrid ID="gvOutlet" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="15" AllowPaging="false" runat="server" DataSourceID="SqlDataSource3" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="true" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource3"
                                    PageSize="15">
                                   
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                     

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Outlet" HeaderText="Outlet" 
                            SortExpression="Outlet"   FooterText ="Total : "  AllowFiltering ="false" ShowFilterIcon ="false"
                             >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>


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
                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Achievement" HeaderText="Ach.%<i class='fa fa-info-circle'></i>" AllowFiltering="false"
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
                                            SortExpression="YTDAch" AllowFiltering="false" HeaderTooltip="Percentage of Target achieved YTD">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Merchandiser" HeaderText="Merchandiser<i class='fa fa-info-circle'></i>"
                                            SortExpression="Merchandiser"  AllowFiltering="false" HeaderTooltip="Merchandiser">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_ScoreCardOutletBreakdown" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                                <asp:Parameter Name="Mode" DefaultValue="Grid" />

                            </SelectParameters>

                        </asp:SqlDataSource>

                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView1" runat="server">
                        <div class="overflowx" >
                            
                                 <div style="position:relative;">
            <div style="position:absolute;padding:5px;top:0;left:0;" >
                                   
                            </div>
                            <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"    
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" cssClass="no-wrap"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="RowsAndColumns"  ColumnsSubTotalsPosition="None" />
                                                                                                        
                                                    <Fields>
                                                       
                                                       
                                                       
                                                          <telerik:PivotGridRowField DataField="Outlet" CellStyle-Width ="370px" ZoneIndex="0"  Caption="Customer" UniqueName="Outlet" >
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
           </div> 
                        </div>
                       

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
