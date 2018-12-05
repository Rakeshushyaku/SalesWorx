<%@ Page Title="Product Availability Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ProductAvailabilityReport.aspx.vb" Inherits="SalesWorx_BO.ProductAvailabilityReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../js/kendo.all.min.js"></script>
<script src="../js/kendo.dataviz.min.js"></script>
    <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />
<style>
input[type="text"].rdfd_
{
    margin:0 !important;
    padding:0 !important;
    height:0 !important;
    width:0 !important;
}
div[id*="ReportDiv"] {  
    overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

#MainContent_gvRep{
    margin:15px 0;
}
  #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:70px !important;
        }
.rpgTable th.rpgColumnHeader a.rpgIcon {
    display:none;
}
 .k-chart svg{
	        margin:0 -6px;
        }
</style>
    <script>
        function pageLoad(sender, args) {

            $('[data-toggle="tooltip"]').tooltip();
        }
        function onDataBound(e) {
            $("#MainContent_Panel1").hide()

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

        function OpenViewWindow(cid) {
            var URL
            //URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
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

        function RefreshChart() {
            createChart3();
        }
    
      
          var internetUsers1 = [];
          var chartseries = [];


          function createlineChart() {

              var Desc = document.getElementById('<%= Hdesc.ClientID%>').value;

               chartseries = [];
               for (var groupIx = 0; groupIx < internetUsers1.length; groupIx++) {
                   chartseries.push({
                       data: internetUsers1[groupIx].Percentage,
                       name: internetUsers1[groupIx].OrgName
                   });

               }
               $("#chart3").kendoChart({
                   theme: $(document).data("kendoSkin") || "flat",
                   dataSource: { data: internetUsers1 },
                   dataBound: onDataBound,
                   title: { text: Desc, align: "center", font: "bold 14px Segoe UI", color: "black" },
                   seriesDefaults: {
                       style: "smooth",
                       width: "1px", type: "line",  format: "{0}%",
                   },
                   seriesColors: ["#0090DF", "#79F604", "#FF00FF", "#F10909", "#1C09D5", "#3F3844", "#09CBEE", " #FFC300 ", "#a9354c", "#6510E8", "#7D83A5", "#F660AB", "#F40A78", "#5D3E49", " #04fac2", "#EC2C3B", "#003300", "#B2007E", "#F48B94", "#F94718", "#376525", "#D1B67F", "#8A0059", "#66C907", "#0090DF", "#CD6155", "#2980B9", "#6698FF", "#FFCBA4", "#F75D59"],
                   legend: { position: "bottom" },
                   tooltip: { visible: true, template: "<b>${series.name}</b></br>${category} - No of Customers - ${value}", color: "#FFFFFF" },
                   series: chartseries,
                   valueAxis: {
                       title: {
                           text: "No. of Customers "  ,
                           font: "bold 12px Segoe UI",
                           color: "black"

                       },

                       labels: {
                           font: " 12px Segoe UI",
                           color: "black"
                       },

                   },
                   categoryAxis: {
                       categories: internetUsers1[0].Mdate,
                       majorGridLines: {
                           visible: false
                       },

                       labels: {
                           rotation: -45,
                           font: " 12px Segoe UI",
                           color: "black",
                           padding: { top: 1, left: -1 }

                       }
                   }
               });


           }

           function createChart3() {
                
               var response = []

               var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;

               var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
               var param3 = document.getElementById('<%= Hto.ClientID%>').value;

               var param5 = document.getElementById('<%= hfVan.ClientID%>').value;

               var param4 = document.getElementById('<%= UId.ClientID%>').value;

               var param6 = document.getElementById('<%= HAgency.ClientID%>').value;

               var param7 = document.getElementById('<%= Hitem.ClientID%>').value;

               var param8 = document.getElementById('<%= HType.ClientID%>').value;
               $("#MainContent_Panel1").show()

             
              
                   $.ajax({
                       type: "POST", contentType: "application/json;charset=utf-8", url: "Chart.asmx/GetProductAvailability", data: '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '", param6:"' + param6 + '", param7:"' + param7 + '", param8:"' + param8 + '"}', dataType: "json", success: function (response) {

                           internetUsers1 = response.d
                           createlineChart();

                       }
                             , error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown) }

                   });
               }
            
        
       </script>
    <style type="text/css">  
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

    
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Product Availability Report</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
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
          <asp:HiddenField runat="server" ID="hfOrg" />
         <asp:HiddenField runat="server" ID="hfVan" />
         <asp:HiddenField runat="server" ID="UId" />
         <asp:HiddenField runat="server" ID="HFrom" />
         <asp:HiddenField runat="server" ID="Hto" />
         <asp:HiddenField runat="server" ID="HAgency" />
          <asp:HiddenField runat="server" ID="Hitem" />
          <asp:HiddenField runat="server" ID="HType" />
          <asp:HiddenField runat="server" ID="Hdesc" />
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                 </div>
                                          </div>
                                            <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_Van" EmptyMessage="Select Van/FSR"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
              
  
            
                       <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Agency </label>
             
                <telerik:RadComboBox   Skin="Simple" ID="ddlAgency"   Width ="100%"
                    runat="server" AutoPostBack="true"    >
                </telerik:RadComboBox >
                  <asp:HiddenField ID="hfVanValue" runat="server" Value="" />
           </div>
                           </div>               
</div>
<div class="row">
            <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Product</label>
                                                
                                                <telerik:RadComboBox ID="drpProduct"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>  
             <div class="col-sm-5">
                 <div class="row">
                     <div class="col-sm-6">
                         <div class="form-group">
                                                            <label>From Date </label>

                           <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                            <label>To Date </label>

                           <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                                    <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                    </DateInput>
                                                                    <Calendar ID="Calendar2" runat="server">
                                                                        <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                    </Calendar>
                                                                </telerik:RadDatePicker>
                           </div>
                             </div>
                 </div>
             </div>
            
                     <div class="col-sm-3">
             <div class="form-group"> <label>Availability </label>
           
                <telerik:RadComboBox   Skin="Simple"  ID="ddlAvail"   Width ="100%"
                    runat="server">
                    <Items>
                    <telerik:RadComboBoxItem Text="Entry" Value="Entry" Selected="true"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Text="Invoiced" Value="Invoiced" ></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Text="Exit" Value="Exit" ></telerik:RadComboBoxItem>
                    
                        </Items>
                </telerik:RadComboBox>
            </div>
                         </div>
                                                    </div>
  </div>
                                                 <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                      <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                                 </div>
                                             
             
  </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar>
      
          <div id="rptsect" runat="server" visible="false"  >
              <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                <p><strong>From Date: </strong> <asp:Label ID="lbl_fromDate" runat="server" Text=""></asp:Label></p>
                <p><strong>To Date: </strong> <asp:Label ID="lbl_Todate" runat="server" Text=""></asp:Label></p>
                 <p><strong>Agency: </strong> <asp:Label ID="lbl_agency" runat="server" Text=""></asp:Label></p>
                <p><strong>Product: </strong> <asp:Label ID="lbl_SkU" runat="server" Text=""></asp:Label></p>
                <p><strong>Availability: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>

               <div class="overflowx" >
                                            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper1" runat="server" >
                                                <div id="chart3" class="margin-14" style="height:100%">

                                                      
                                                </div>
                                                </div>
                               </div>
              <p><br /><br /></p>

               <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <%--<telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="true" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="true" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                         
                                                        <ItemStyle Width="50px" />
                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>--%>

                                                                <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"  />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Product" ZoneIndex="0"  UniqueName="Product"  SortOrder="none" CellStyle-CssClass="nowhitespace">
                                                                   
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Type" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None" CellStyle-CssClass="nowhitespace">
                 
                                                        </telerik:PivotGridColumnField>
                                                         <telerik:PivotGridAggregateField DataField="C1" SortOrder="none" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        

                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>
                                                               </telerik:RadAjaxPanel>

              </div>
   </telerik:RadAjaxPanel>
       <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="Btn_LoadItem" runat="server" Text="Export" />
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