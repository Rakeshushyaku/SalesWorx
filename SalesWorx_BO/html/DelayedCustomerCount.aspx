<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DelayedCustomerCount.aspx.vb" Inherits="SalesWorx_BO.DelayedCustomerCount" MasterPageFile="~/html/Site.Master"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
 


 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../js/kendo.all.min.js"></script>
    <script src="../js/kendo.dataviz.min.js"></script>
    <link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

    <style>

       
         

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

.k-chart svg{
	margin:0 -14px;
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

         function RefreshChart() {
             createChart2()
         }

         function onDataBound(e) {
             $("#MainContent_Panel1").hide()

         }
         function createChart2() {
             var param = document.getElementById('<%= hfOrgID.ClientID%>').value;
             var param1 = document.getElementById('<%= hfSMonth.ClientID%>').value;
             var param2 = document.getElementById('<%= hfEMonth.ClientID%>').value;
             var param3 = document.getElementById('<%= UId.ClientID%>').value;

             $("#MainContent_Panel1").show()

             $("#chart2").kendoChart({
                 theme: $(document).data("kendoSkin") || "flat",
                 dataBound: onDataBound,
                 dataSource: {
                     type: "json",
                     transport: {
                         read: {
                             url: "Chart.asmx/DelayedCustomerCount", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                             contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                             type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                             dataType: "json"
                         }
,
                         parameterMap: function (data, operation) {

                             var output = null;
                             switch (operation) {

                                 case "read":
                                     output = '{ param: ' + JSON.stringify(param) + ',param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + '}';
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
                     sort: [ { field: "MonthYear", dir: "asc" }]


                 },






                 chartArea: {
                     background: "#fff"
                 },
                 title: {
                     text: "Delayed Customer Count",
                     font: " bold 12px Segoe UI",
                     color: "black"
                 },
                 legend: {
                     position: "top",

                     labels: {
                         font: "12px Segoe UI", color: "black",
                         background: "black"

                     }

                 },


                 seriesDefaults: {
                     type: "line",
                     style: "smooth",
                     width: "1px",
                     missingValues: "interpolate", labels: {
                         visible: false,
                         format: "{0:N0}",
                         font: "12px Segoe UI", color: "black",
                         background: "black"

                     }

                 },

                 valueAxis: {
                     title: {
                         text: "Delayed Customer Count",
                         font: "bold 12px Segoe UI", color: "black"
                     },
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
                         font: "12px Segoe UI", color: "black "
                     }
                 },
                 categoryAxis:
{
    
    labels: {
        font: " 12px Segoe UI", color: "black", step: 1,
        format: "MMM-yyyy"

    },

    majorGridLines: {
        visible: true
    }
},

                 seriesColors: ["DarkoliveGreen", "Dodgerblue", "Darkmagenta", "Crimson", "#00FFFF", "#00FF00", "#0000FF", "#2F4F4F", "#4682B4", "#800080", "#C71585", "#00CED1", "#66CDAA", "#9ACD32", "#CD853F", "#FFA07A", "#A52A2A", "#696969", "#000000", "#708090", "#191970", "#DDA0DD", "#DB7093", "#20B2AA", "#00FA9A", "#BDB76B", "#FFD700", "#FFDEAD", "#FF6347", "#FF0000", "#006400", "#808000", "#FFFF00"],
                 series: [{
                     field: "Unit",
                     categoryField: "AgencyName",

                     labels: {
                         font: "12px Segoe UI", color: "#333", step: 1
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
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }    
   
    input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
  
</style>
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Delayed Customer Count</h4>
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
  <telerik:RadAjaxPanel ID="l" runat ="server" >
                <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">
                                      <ContentTemplate>
                                         <div class="row">
                                              <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                      
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select Organization" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"  >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           
                                                 <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Month</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>                                                       
                                                    </div>
                                                 </div>
                                                 <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>To Month</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtToDate" runat="server" >
                                                             <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                        </telerik:RadMonthYearPicker> 
                                                           
                                                    </div>
                                                  </div>
                                             
                                           
                                            
                                             
                                     
                                                </div>
                                                </div>
                                             <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button   CssClass ="btn btn-sm btn-block btn-primary"   ID="SearchBtn" runat="server" Text="Search"  />
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

            <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>From: </strong> <asp:Label ID="lbl_FromMonth" runat="server" Text=""></asp:Label></p>
              <p><strong>To: </strong> <asp:Label ID="lbl_ToMonth" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>                             
 <br />
         <asp:HiddenField ID="hfOrgID" runat ="server" />
                              
                              <asp:HiddenField ID="hfSMonth" runat ="server" />
                              <asp:HiddenField ID="hfEMonth" runat ="server" />
  <asp:HiddenField ID="UId" runat ="server" />
  <div id="summary" runat="server" class="row"></div>
      <div class ="overflowx">
            <div class ="chart-wrapper padding0">
                                            <div id="chart2">
                                                </div>     
                                         </div> 
          </div>
     <p><br /><br /></p>
             <div class="table-responsive">
                  <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="true" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="true" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">
                    <Columns></Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                    <ItemStyle HorizontalAlign="Right" />   
                    <AlternatingItemStyle      HorizontalAlign="Right" />             
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                      </telerik:RadAjaxPanel>
                           
                  </div>
                              
                                   
                              <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>  
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

       <asp:Panel ID="Panel1" CssClass="overlay" runat="server"  style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
       </asp:Panel>

</asp:Content>