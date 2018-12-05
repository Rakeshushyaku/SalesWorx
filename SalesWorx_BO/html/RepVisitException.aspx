﻿<%@ Page Title="Visit Exception Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepVisitException.aspx.vb" Inherits="SalesWorx_BO.RepVisitException" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
       <script>



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

           function OpenViewWindow(sid, van,Mode) {
               var combo = $find('<%=ddlOrganization.ClientID%>');
               var MonthName = document.getElementById('<%= hfSMonth.ClientID %>').value;
               var URL
               URL = 'RepDetails.aspx?Type=VisitException&ReportName=VisitExceptionDetails&ID=' + sid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Van=' + van + '&OrgName=' + combo.get_selectedItem().get_text() + '&MonthName=' + MonthName + '&Mode=' + Mode
               var oWnd = radopen(URL, null);
               oWnd.SetSize(800, 580);
               oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
               oWnd.SetModal(true);
               oWnd.Center;
               oWnd.set_visibleStatusbar(false)

               return false

           }

        document.onkeydown = function (evt) {

            evt = evt || window.event;
            if (evt.keyCode == 27) {

                HideRadWindow();
            }
        };

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }


        }


    </script>
     <script type="text/javascript">
         $(window).resize(function () {
             var win = $find('winDetail');
             if (win) {
                 if (!win.isClosed()) {
                     win.center();
                 }
             }

         });
     </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Visit Exception Report</h4>
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


        

         
            <div class="row">
                <div class="col-sm-6">
                    <h5>Month <strong><span class="text-blue"><asp:Label runat="server" ID="lblSelMonth"></asp:Label></span></strong></h5>
                </div>
              
            </div>

            <div class="table-responsive">

                
              

        

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

                                     

                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="salesrep_name" HeaderText="Van" AutoPostBackOnFilter ="true" 
                            SortExpression="salesrep_name"  Aggregate ="Count" FooterText ="Total Vans : "  AllowFiltering ="true" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the van/FSR name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>

                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="TotalCustomersPDA" HeaderText="Customer Assigned"
                                            SortExpression="TotalCustomersPDA"  AllowFiltering="false">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                

                                         <telerik:GridTemplateColumn UniqueName="UnVisitedCustomer" AllowFiltering="false" DataField ="UnVisitedCustomer"
                                                    InitializeTemplatesFirst="false" HeaderText="UnVisited Customer" SortExpression ="UnVisitedCustomer">


                                                    <ItemTemplate>

                                                    
                                                        <asp:LinkButton runat="server" ID="lblUnVisitedCustomer" Font-Underline ="true" ToolTip ="View UnVisited Customer" Font-Bold="true"
                                                           Text='<%# Bind("UnVisitedCustomer")%>' 

                                                             OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"",""{2}"");", Eval("SID"), Eval("salesrep_name"), "C")%>'


                                                         ></asp:LinkButton>
                                                        <asp:Label runat ="server" ID="lblVanID" Visible ="false" Text ='<%# Bind("SID")%>'></asp:Label>
                                                          
                                                    </ItemTemplate>

                                                  
                                                   <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>

  <telerik:GridTemplateColumn UniqueName="MissedVisits" AllowFiltering="false" DataField ="UnVisitedCount" Aggregate ="Sum" FooterText ="Visits Missed : "
                                                    InitializeTemplatesFirst="false" HeaderText="Missed Visits"  SortExpression ="UnVisitedCount">


                                                    <ItemTemplate>

                                                    
                                                        <asp:LinkButton runat="server" ID="lblMissedVisits" Font-Underline ="true" ToolTip ="View Missed Visits Details" Font-Bold="true"
                                                           Text='<%# Bind("UnVisitedCount")%>' 

                                                             OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"",""{2}"");", Eval("SID"), Eval("salesrep_name"), "V")%>'


                                                         ></asp:LinkButton>
                                                        <asp:Label runat ="server" ID="lblVanID" Visible ="false" Text ='<%# Bind("SID")%>'></asp:Label>
                                                          
                                                    </ItemTemplate>

                                                  
                                                   <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>

                                     
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        <%--</telerik:RadAjaxPanel>--%>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_VisitExceptionFSR" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="hfOrgID" Name="OID" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfVans" Name="VanList" DefaultValue="0" />
                                <asp:ControlParameter ControlID="hfSMonth" Name="FMonth" DefaultValue="01-01-1900" />
                               

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
