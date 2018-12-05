<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DelayedTrendByDiv.aspx.vb" Inherits="SalesWorx_BO.Rep_DelayedTrendByDiv" %>
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
  

       
div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

    .k-chart svg{
	margin:0 -14px;
}
    </style>
    <script>
        function RefreshChart() {
            createChart3();
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
     <script>

         var internetUsers1 = [];
         var chartseries = [];


         function createlineChart() {

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
                 title: { text: "Delayed % by Division", align: "center", font: "bold 12px Segoe UI", color: "black" },
                 seriesDefaults: {
                     style: "smooth",
                     width: "1px", type: "line", missingValues: "interpolate", format: "{0}%",
                 },
                 seriesColors: ["#607d8b", "#eb6357", "#14b4fc", "#97c95d", "#ef9933", "#07B312", "#F9CE1D", "#5D3E49", "#003300", "#B2007E", "#F48B94", "#F94718", "#376525", "#D1B67F", "#8A0059"],
                 legend: { position: "bottom" },
                 tooltip: {
                     visible: true, template: "<b>${series.name}</b></br>${category} -- ${value}", font: "12px Segoe UI",
                     color: "white",
                     background: "black"
                 },
                 series: chartseries,
                 valueAxis: {
                     title: {
                         text: "Delayed % ",
                         font: "bold 12px Segoe UI",
                         color: "black"

                     },
                     max: 100,
                     labels: {
                         color: "black", format: "{0}%", font: " 12px Segoe UI",
                     },

                 },
                 categoryAxis: {
                     categories: internetUsers1[0].Mdate,
                     majorGridLines: {
                         visible: false
                     },

                     labels: {
                         rotation: -45,
                         color: "black", font: "bold 12px Segoe UI",
                         padding: { top: 1, left: -1 }

                     }
                 }
             });


         }

         function createChart3() {

             var response = []

             var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;



             var param2 = document.getElementById('<%= HFromDate.ClientID%>').value
             var param3 = document.getElementById('<%= HTodate.ClientID%>').value
              
             var param5 = document.getElementById('<%= hfLoc.ClientID%>').value;

              

             var param4 = document.getElementById('<%= UId.ClientID%>').value;




             if (param1 != "" && param2 != "" & param3 != "") {

                 $.ajax({
                     type: "POST", contentType: "application/json;charset=utf-8", url: "Chart.asmx/CollectionTrendByDivision", data: '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '"}', dataType: "json", success: function (response) {

                         internetUsers1 = response.d
                         createlineChart();

                     }
                           , error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown) }

                 });
             }
         }



    </script> 
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Delayed Trend By Division</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

       <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                       </UpdatedControls>
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>

	
 
     <telerik:RadAjaxPanel ID="l" runat ="server"  >

     <asp:HiddenField ID="UId" runat="server" />
     <asp:HiddenField ID="HSearched" runat="server" Value="0" />
            <asp:HiddenField ID="HFromDate" runat="server" />
          <asp:HiddenField ID="HTodate" runat="server" />
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
                                                    <div class="col-sm-6" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>

                                                 <div class="col-sm-6">
                                                     
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select Organisation" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>

                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Location</label>
                                                <telerik:RadComboBox  Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"  CheckBoxes="true" EmptyMessage="Select Location" ID="ddlLocation" Width ="100%" runat="server" DataTextField="Description" DataValueField="Sales_District_ID" >
                                                 </telerik:RadComboBox>
                                            </div>
                                          </div>

                                                
                                                     
                                           <div class="col-sm-6">
                                               <div class="row">
                                                   <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>From Month</label>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                            </div>
                                          </div>
                                                   <div class="col-sm-6">
                                                   <div class="form-group">
                                                <label>To Month</label>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtToDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                                       </div>
                                            </div>
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
                                             
             
  </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 

             <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Location: </strong><asp:Label ID="lbl_Loc" runat="server" Text=""></asp:Label></p>
              <p><strong>From Month: </strong> <asp:Label ID="lbl_Month" runat="server" Text=""></asp:Label></p>
              <p><strong>To Month: </strong> <asp:Label ID="lbl_ToMonth" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>

            <asp:HiddenField ID="hfOrg" runat="server" />
            <asp:HiddenField ID="hfLoc" runat="server" />
            <asp:HiddenField ID="hFrom" runat="server" />
            <asp:HiddenField ID="Hto" runat="server" />
                     <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
    <div id="ErrorMsg" ></div>
    <p><br /></p>
           <div class="overflowx">
     <div class="chart-wrapper" id="Chartid" runat="server" style="padding:25px 0 0 0;" >
        <div id="chart3"   style ="width:100%;height:365px;"></div>
     </div>
             </div>   
       <hr />
     <div id="summary" runat="server"></div>
            

       </telerik:RadAjaxPanel>
 
       <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
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
	
</asp:Content>
