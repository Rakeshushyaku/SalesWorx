<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="DashBoardDetailedSalesbyAgency.aspx.vb" Inherits="SalesWorx_BO.DashBoardDetailedSalesbyAgency" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style scoped>
        .chart-wrapper, .chart-wrapper .k-chart {
       

        }
             .rgExpand, .rgCollapse
  {
    display: none;
  }
  .RadGrid .rgPagerLabel, .RadGrid .rgPager .RadComboBox, .RadGrid .rgPager .RadInput {
margin: 0 4px 0 0;
vertical-align: middle;

visibility: hidden;
}


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
   
<script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

  <script type="text/javascript">

      function createChart3() {
          var res
          var param = document.getElementById('<%= HUID.ClientID %>').value;
          var param2 = document.getElementById('<%= HDate.ClientID %>').value;
          var Currency = document.getElementById('<%= HCurrency.ClientID%>').value;

          var dataSource = {
              type: "json",
              transport: {
                  read: {
                      url: "Chart.asmx/GetSalesbyAgencyForMonth", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                      contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                      type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                      dataType: "json"
                  }
                    ,
                  parameterMap: function (data, operation) {

                      var output = null;
                      switch (operation) {

                          case "read":
                              output = '{ param1: ' + JSON.stringify(param) + ',param2: ' + JSON.stringify(param2) + ' }';
                              break;

                      }

                      return output;
                      return kendo.stringify(data);
                      // stringify the data send back to the server
                  }
              },
              schema: {
                  data: "d", // ASMX services return JSON in the following format { "d": <result> }. Specify how to get the result.
                  model: { // define the model of the data source.
                      fields: {

                          Description: { type: "string" },
                          Amount: { type: "number" }

                      }
                  }
              },
              sort: {
                  field: "Description",
                  dir: "asc"
              }
          }

          $("#chart3").kendoChart({
              theme: "flat",
              dataSource: dataSource,
              title: {
                  text: "",
                  color: "white",
                  font: "14px Calibri"

              },


              seriesDefaults: {
                  type: "column",
                  gap: 1.5,
                  labels: {
                      visible: false,
                      format: "{0:N0}",

                      font: "12px Segoe UI"
                  }

              },
              chartArea: {
                  background: "White"
              },
              categoryAxis:
                {
                    labels: { font: "12px Segoe UI", color: "#ef9933", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },


              valueAxis:
                {
                    title: {
                        text: "Sales " + Currency,
                        font: "12px Segoe UI", color: "#33CCFF"
                    },
                    labels: { font: "12px Segoe UI", color: "#ef9933" }, majorGridLines: { visible: false }
                },
              seriesColors: ["#14b4fc", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
                  field: "Amount",
                  categoryField: "Description",


              }],
              tooltip: {
                  visible: true,
                  format: "{0:N0}",
                  font: "12px Segoe UI",
                  color: "white",
                  background: "black"
              },


          });


          console.debug(dataSource.data)



      }

      $(document).ready(function () {

          createChart3();

      });

      function RefreshChart() {
          createChart3();
      }

      function alertCallBackFn(arg) {

      }

    </script>   
    <Style>
        input[type="text"].rdfd_
{
    margin:0 !important;
    padding:0 !important;
    height:0 !important;
    width:0 !important;
}

    </Style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
       <h4><a href="ASRDashboard.aspx" title="Back"><i class="fa fa-arrow-circle-o-left"></i></a> Sales By Agency</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                       </UpdatedControls>
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>
     

     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
     <asp:HiddenField ID="UId" runat="server" />
    <asp:HiddenField ID="hfOrgID" runat ="server" />
    <asp:HiddenField ID="HFrom" runat ="server" />
 
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"   AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>

                                                 <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Organization</label>
                                                <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>
                                                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month</label>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromDate" runat="server" >
                                                            <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                            </div>
                                          </div>
                                                     
                                           
                                             </div>
                                                </div>
                                                <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button   CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                   
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
              <p><strong>Month: </strong> <asp:Label ID="lbl_Month" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
         
                              <div id="summary" runat="server" class="row"></div> 
            <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
             
             <div class="table-responsive" id="Detailed"  runat="server">
 
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
                           
                  
</div>
             <br />
   
  <div class="overflowx" >
     <div class="chart-wrapper" style="height:400px" id="Chartwrapper" runat="server" >
<div id="chart3" style="height:100%"> </div>
</div>
      </div>
    <asp:HiddenField ID="HUID" runat="server" />
    <asp:HiddenField ID="HCurrency" runat="server" Value="(AED)" />
    <asp:HiddenField ID="HDate" runat="server" />
       </telerik:RadAjaxPanel>  


    

	       
 
	
</asp:Content>
