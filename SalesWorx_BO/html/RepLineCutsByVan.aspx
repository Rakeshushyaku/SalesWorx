<%@ Page Title="Line Cuts By Van" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepLineCutsByVan.aspx.vb" Inherits="SalesWorx_BO.RepLineCutsByVan" %>

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
           
          
        </script>

    </telerik:RadScriptBlock>


    <style>
        #MainContent_summary .col-sm-4:nth-child(1) .widgetblk, #MainContent_summary .col-sm-6:nth-child(1) .widgetblk
        {
            background-color: #ef9933;
        }

        #MainContent_summary .col-sm-4:nth-child(2) .widgetblk, #MainContent_summary .col-sm-6:nth-child(2) .widgetblk
        {
            
            background-color: #97c95d;
        }

        #MainContent_summary .col-sm-4:nth-child(3) .widgetblk, #MainContent_summary .col-sm-6:nth-child(3) .widgetblk
        {
            background-color: #14b4fc;
        }
        .col-lg-2-1 {
    width: 22% !important;
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Line Cuts By Van/FSR</h4>
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


         















           <asp:HiddenField ID="hfSelVan" runat="server" />
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

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Organization<em><span>&nbsp;</span>*</em></label>

                                            <telerik:RadComboBox Filter="Contains" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" Skin="Simple"  ID="ddlOrganization" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                CheckBoxes="true" EmptyMessage="Select a Van/FSR" ID="ddlVan" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                    </div>
                                 <div class="row">

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>From Date</label>
                                            

                                            <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>


                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>To Date</label>
                                            

                                            <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>


                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
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
        <asp:HiddenField ID="hTSMonth" runat="server" />
        <asp:HiddenField ID="HCustomer_ID" runat="server" />
        <asp:HiddenField ID="HSiteID" runat="server" />
         <asp:HiddenField ID="HSelCustomer_ID" runat="server" />
        <asp:HiddenField ID="HSelSiteID" runat="server" />
            
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
                            <strong>From: </strong>
                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                        </p>

                        <p>
                            <strong>To: </strong>
                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                        </p>


                         <p>
                            <strong>Customer: </strong>
                            <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label>
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
                       Overall Unique SKU <div class="text-primary">
                            <asp:Label ID="lblTotOutlets" runat="server"></asp:Label></div>

                    </div>
                </div>
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Overall Billed SKU<div class="text-primary">
                            <asp:Label ID="lblBilled" runat="server"></asp:Label></div>

                    </div>
                </div>

                 <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                        Overall Line Cuts 
                        <div class="text-primary">
                            <asp:Label ID="lblAchPercent" runat="server"></asp:Label>
                            %</div>

                    </div>
                </div>

               
            </div>

            <p><br /></p>
            <div class="row">
                <div class="col-sm-4">
                    <h5>From date <strong><span class="text-blue"><asp:Label runat="server" ID="lblSelMonth"></asp:Label></span></strong></h5>
                </div>
                  <div class="col-sm-4">
                    <h5>To Date <strong><span class="text-blue"><asp:Label runat="server" ID="lblSelTodate"></asp:Label></span></strong></h5>
                </div>
                <div class="col-sm-4 text-right" style ="display:none;">
                    <h5>Currency <strong><span class="text-blue"><asp:Label runat="server" ID="lblC"></asp:Label></span></strong></h5>
                </div>
            </div>

            <div class="table-responsive">

                
              

                      <%--  <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">--%>
                                  <telerik:RadWindow ID="ZeroBilledWindow" Title ="Billed SKU" runat="server" modal="true"  Behaviors="Move,Close" 
           ReloadOnShow="true"  VisibleStatusbar="false" Height ="515px" BorderColor ="Black"  Width ="700px"  skin="Windows7"  >
               <ContentTemplate>
                           <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
                                    <telerik:RadGrid ID="rgZeroBilled" DataSourceID="SqlZero"
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None"
                                        PageSize="9" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="false" ShowFooter="true"
                                        GridLines="None">

                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="true">
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataSourceID="SqlZero" ShowFooter="true"  AllowFilteringByColumn ="false" 
                                            TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                            PageSize="9">
                                              
                                            <Columns>

                                                <telerik:GridBoundColumn DataField="Description" HeaderText="SKU" SortExpression="Description"
                                                     ShowFilterIcon="false" AllowFiltering="true"  FilterControlToolTip="Type SKU and press enter"
                                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" Aggregate="Count" FooterText="Total SKU : ">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>
                                              
                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" AllowFiltering ="false" ShowFilterIcon ="false" DataField="LastBilledOn" HeaderText="Last Billed On" SortExpression ="LastBilledOn"
                                                               DataFormatString="{0:dd-MMM-yyyy}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                            </Columns>
                                        </MasterTableView>


                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SqlZero" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                        SelectCommand="Rep_BilledLineDetails" SelectCommandType="StoredProcedure">

                                        <SelectParameters>
                                         <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FDate" DefaultValue="01-jan-1900" />
                                  <asp:ControlParameter ControlID="hTSMonth" Name="TDate" DefaultValue="01-jan-1900" />
                                 <asp:ControlParameter ControlID="HSelCustomer_ID" Name="Customer_ID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="HSelSiteID" Name="SiteID" DefaultValue="0" />            
                                        </SelectParameters>

                                    </asp:SqlDataSource>

                                </telerik:RadAjaxPanel>
                   
               </ContentTemplate>
          </telerik:RadWindow>

                            <telerik:RadGrid ID="gvVans" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="5" AllowPaging="true" runat="server" DataSourceID="SqlDataSource1" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="true" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1" CommandItemDisplay="Top"
                                    PageSize="5">
                                    <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                     

                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer" AutoPostBackOnFilter ="true" 
                            SortExpression="Customer"  Aggregate ="Count" FooterText ="Total Customers : "  AllowFiltering ="true" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the Customer name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>

                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="TotLineAssigned" HeaderText="Total SKU"
                                            SortExpression="TotLIneAssigned"  AllowFiltering="false">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                

                                         <telerik:GridTemplateColumn UniqueName="TotBilled" AllowFiltering="false" DataField ="TotBilled"
                                                    InitializeTemplatesFirst="false" HeaderText="SKU Billed" SortExpression ="TotBilled">


                                                    <ItemTemplate>

                                                    
                                                        <asp:LinkButton runat="server" ID="lblBilled" Font-Underline ="true" ToolTip ="View billed SKU" Font-Bold="true"
                                                           Text='<%# Bind("TotBilled")%>' OnClick ="lblBilled_Click"
                                                         ></asp:LinkButton>
                                                        <asp:Label runat ="server" ID="lblCustomerID" Visible ="false" Text ='<%# Bind("CustomerID")%>'></asp:Label>
                                                        <asp:Label runat ="server" ID="lblSiteID" Visible ="false" Text ='<%# Bind("SiteUse_ID")%>'></asp:Label>
                                                    </ItemTemplate>

                                                  
                                                   <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>


                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="BillPercentage" HeaderText="Line Cuts %"
                                            SortExpression="BillPercentage"  AllowFiltering="false">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                     
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        <%--</telerik:RadAjaxPanel>--%>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_LineCutByVan" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FDate" DefaultValue="01-01-1900" />
                                <asp:ControlParameter ControlID="hTSMonth" Name="TDate" DefaultValue="01-jan-1900" />
                                 <asp:ControlParameter ControlID="HCustomer_ID" Name="Customer_ID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="HSiteID" Name="SiteID" DefaultValue="0" />
                            </SelectParameters>

                        </asp:SqlDataSource>

             



            </div>
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
     <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>

</asp:Content>
