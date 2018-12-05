<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_SalesbyVan.aspx.vb" Inherits="SalesWorx_BO.Rep_SalesbyVan" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style scoped>
        #MainContent_divsummary .col-sm-4:nth-child(1) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(1) .widgetblk{
	background-color: #4d5ec1;
}

#MainContent_divsummary .col-sm-4:nth-child(2) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(2) .widgetblk{
	background-color: #eb6357;
}

#MainContent_divsummary .col-sm-4:nth-child(3) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(3) .widgetblk{
	background-color: #14b4fc;
}

#MainContent_divsummary .col-sm-4:nth-child(4) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(4) .widgetblk{
	background-color: #97c95d;
}

#MainContent_divsummary .col-sm-4:nth-child(5) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(5) .widgetblk{
	background-color: #ef9933;
}

#MainContent_divsummary .col-sm-4:nth-child(6) .widgetblk,
#MainContent_divsummary .col-sm-6:nth-child(6) .widgetblk{
	background-color: #607d8b;
}
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


   #MainContent_divSummaryNet .col-sm-4:nth-child(1) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(1) .widgetblk{
	background-color: #4d5ec1;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(2) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(2) .widgetblk{
	background-color: #eb6357;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(3) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(3) .widgetblk{
	background-color: #14b4fc;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(4) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(4) .widgetblk{
	background-color: #97c95d;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(5) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(5) .widgetblk{
	background-color: #ef9933;
}

#MainContent_divSummaryNet .col-sm-4:nth-child(6) .widgetblk,
#MainContent_divSummaryNet .col-sm-6:nth-child(6) .widgetblk{
	background-color: #607d8b;
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
          var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
          var param2 = document.getElementById('<%= HVan.ClientID%>').value;
          var param3 = document.getElementById('<%= HUID.ClientID %>').value;
          var param4 = document.getElementById('<%= HType.ClientID%>').value;
          var param5 = document.getElementById('<%= HDate.ClientID%>').value;
          var param6 = document.getElementById('<%= HToDate.ClientID%>').value;
          var param7 = document.getElementById('<%= HAgency.ClientID%>').value;
          var param8 = document.getElementById('<%= HItem.ClientID%>').value;
          var param9 = document.getElementById('<%= Hcustomer.ClientID %>').value;
         
          var Currency = document.getElementById('<%= HCurrency.ClientID%>').value;

          var dataSource = {
              type: "json",
              transport: {
                  read: {
                      url: "Chart.asmx/GetRepSalesbyVan", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                      contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                      type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                      dataType: "json"
                  }
                    ,
                  parameterMap: function (data, operation) {

                      var output = null;
                      switch (operation) {

                          case "read":
                              output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ',param6: ' + JSON.stringify(param6) + ',param7: ' + JSON.stringify(param7) + ',param8: ' + JSON.stringify(param8) + ',param9: ' + JSON.stringify(param9) + ' }';
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
                  text: "Sales Vs Returns" + Currency,
                  color: "black",
                  font: "bold 12px Segoe UI"

              },

              legend: {
                  position: "top",
                  font: "12px Segoe UI"
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
                    labels: { font: " 12px Segoe UI", color: "black", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },


              valueAxis:
                {
                    title: {
                        text: "   " ,
                        font: "bold 12px Segoe UI", color: "black"
                    },
                    labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                },
              seriesColors: ["#4d5ec1", "#FF7663", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
                  name: "Sales",
                  field: "Amount",
                  categoryField: "Description",
                  tooltip: {
                      visible: true,
                      format: "{0:N2}",
                      font: "12px Segoe UI",
                      template: "Van : ${category}  <br>Sales: #=dataItem.Amount# ",
                      color: "white",
                      background: "black"
                  },

              },

               {
                   name: "Returns",
                   field: "ReturnAmount",
                   categoryField: "Description",
                   
                   tooltip: {
                       visible: true,
                       format: "{0:N2}",
                       font: "12px Segoe UI",
                       template: "Van : ${category}  <br>Returns: #=dataItem.ReturnAmount# ",
                       color: "white",
                       background: "black"
                   },
               }],
             


          });






      }




      function createChart4() {
         
          var res
          var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
          var param2 = document.getElementById('<%= HVan.ClientID%>').value;
          var param3 = document.getElementById('<%= HUID.ClientID %>').value;
          var param4 = document.getElementById('<%= HType.ClientID%>').value;
          var param5 = document.getElementById('<%= HDate.ClientID%>').value;
          var param6 = document.getElementById('<%= HToDate.ClientID%>').value;
          var param7 = document.getElementById('<%= HAgency.ClientID%>').value;
          var param8 = document.getElementById('<%= HItem.ClientID%>').value;
          var param9 = document.getElementById('<%= Hcustomer.ClientID %>').value;

          var Currency = document.getElementById('<%= HCurrency.ClientID%>').value;

          var dataSource = {
              type: "json",
              transport: {
                  read: {
                      url: "Chart.asmx/GetRepNetSalesbyVan", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                      contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                      type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                      dataType: "json"
                  }
                    ,
                  parameterMap: function (data, operation) {

                      var output = null;
                      switch (operation) {

                          case "read":
                              output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ',param6: ' + JSON.stringify(param6) + ',param7: ' + JSON.stringify(param7) + ',param8: ' + JSON.stringify(param8) + ',param9: ' + JSON.stringify(param9) + ' }';
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

          $("#chart4").kendoChart({
              theme: "flat",
              dataSource: dataSource,
              title: {
                  text: "Net Sales" + Currency,
                  color: "black",
                  font: "bold 12px Segoe UI"

              },

              legend: {
                  position: "top",
                  visible:false
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
                    labels: { font: " 12px Segoe UI", color: "black", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },


              valueAxis:
                {
                    title: {
                        text: "  " ,
                        font: "bold 12px Segoe UI", color: "black"
                    },
                    labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                },
              seriesColors: ["#14b4fc", "#FF7663", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
                  name: "Net Sales",
                  field: "Amount",
                  categoryField: "Description",


              }],
              tooltip: {
                  visible: true,
                  template: "Van : ${category}  <br> Net  Sales : #=dataItem.Amount# ",
                  format: "{0:N2}",
                  font: "12px Segoe UI",
                  color: "white",
                  background: "black"
              },


          });






      }

     

      function RefreshChart() {
          createChart3();
      }

      function RefreshNetChart() {
           
          createChart4();
      }
      function clickExportBiffExcel() {

          $("#MainContent_BtnExportBiffExcel").click()
          return false

      }
      function LoadItems() {

          $("#MainContent_Btn_LoadItem").click()
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

      function pageLoad(sender, args) {
          $('.rgMasterTable').find('th > a').attr("data-container", "body");
          $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
          $('[data-toggle="tooltip"]').tooltip();
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

        div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }   
    </Style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Sales By Van/FSR</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                           <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                       </UpdatedControls>
                    
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>


     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
     
 
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-6">
                                                     
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" AutoPostBack="true"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >

                                                  

                                            </div>
                                          </div>
                                                 <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>Type</label>
                                                  <telerik:RadComboBox Skin="Simple"    ID="ddl_Type" Width ="100%" runat="server"  Filter="Contains" AutoPostBack="true" >
                                                      <Items>
                                                          <telerik:RadComboBoxItem Text="Over All" Selected="true" Value="0" />
                                                          <telerik:RadComboBoxItem Text="Monthly"   Value="1" />
                                                          <telerik:RadComboBoxItem Text="Weekly"  Value="2" />
                                                      </Items>
                                        </telerik:RadComboBox >

                                                  

                                            </div>
                                          </div>
                                                    </div>
                                                 <div class="row">  
                                                <div class="col-sm-3">
                                                    <label><asp:Label runat="server" ID="lbl_from" Text="From Date"></asp:Label></label>
                                            <div class="form-group">
                                                
                                                <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromMonth" runat="server" Visible="false"  Skin="Simple" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                            </div>
                                          </div>
                                                    <div class="col-sm-3">
                                                        <label><asp:Label runat="server" ID="lbl_to" Text="To Date"></asp:Label></label>
                                            <div class="form-group">
                                                
                                                <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtToMonth" runat="server" Visible="false" Skin="Simple" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                            </div>
                                          </div>
                                              <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Agency</label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true"   EmptyMessage="Select Agency" EnableCheckAllItemsCheckBox="true"  Filter="Contains" ID="ddl_Agency" Width ="100%" runat="server">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                        </div>
                                                 <div class="row"> 
                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Item</label>
                                                
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
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
           <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat ="server" >
         <div id="rptsect" runat="server" visible="false"  >
                              <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                <p><strong>Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
                <p><strong> <asp:Label ID="lbl_fromtxt" runat="server" Text=""></asp:Label></strong> <asp:Label ID="lbl_Fromdt" runat="server" Text=""></asp:Label></p>
                <p><strong> <asp:Label ID="lbl_Totxt" runat="server" Text=""></asp:Label></strong> <asp:Label ID="lbl_Todt" runat="server" Text=""></asp:Label></p>
                <p><strong>Agency: </strong> <asp:Label ID="lbl_agency" runat="server" Text=""></asp:Label></p>
                <p><strong>Product: </strong> <asp:Label ID="lbl_SkU" runat="server" Text=""></asp:Label></p>
                <p><strong>Customer: </strong> <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
         
                              
            <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
             
             <div class="table-responsive" id="Detailed"  runat="server">

                 <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Overall" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Sales Vs Returns" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="" runat="server" Visible="false" >
                        </telerik:RadTab>
                        

                    </Tabs>
                </telerik:RadTabStrip>
                  <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       <telerik:RadPageView ID="RadPageView1" runat="server">
                                            <p style="padding-top:15px;"><asp:Label ID="lblDesc1" Text="" runat="server" ></asp:Label></p>

                                           <div id="divSummaryNet" runat="server" class="row"></div> 
                           <p><br /></p>
                           <div class="overflowx" >
                                            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper1" runat="server" >
                                                <div id="chart4" style="height:100%" class="margin-14"> </div>
                                                </div>
                               </div>
                           <hr />
                                             <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
 <div class="overflowx">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" width="100%"
                                                                    GridLines="None"  OnItemCommand="gvRep_ItemCommand" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12"  CommandItemDisplay="Top" >

                                                        <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                                                        <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_name" HeaderText="Van"
                                                                  SortExpression ="SalesRep_name" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales" HeaderText="Total Sales<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Sales" DataType="System.Double" DataFormatString="{0:N2}" HeaderTooltip="Total sales value">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Returns" HeaderText="Total Returns<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Returns" DataType="System.Double" DataFormatString="{0:N2}"  HeaderTooltip="Total returns value">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Net" HeaderText="Net Sales<i class='fa fa-info-circle'></i>"  
                                                                  SortExpression ="Net" DataType="System.Double" DataFormatString="{0:N2}" HeaderTooltip="Net sales value" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
                                         </div>
                                             </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView2" runat="server">
                                            <p style="padding-top:15px;"><asp:Label ID="lblDesc" Text="" runat="server" ></asp:Label></p>

                                            <div id="summary" runat="server" class="row"></div> 
                                            <p><br /></p>
                                            <div class="overflowx" >
                                            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper" runat="server" >
                                                <div id="chart3" style="height:100%" class="margin-14"> </div>
                                                </div>
                                                </div>
                                
                                             </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView3" runat="server">
                                            
                                           
     
      <div id="divsummary" runat="server" class="row"></div> 
    
                                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
                                              <div id="divCurrency1" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency1" runat="server" Text=""></asp:Label></strong></span></h5>
</div>

                                                <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="true"  PageSize="10"
                                                    ID="RadPivotGrid1" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" 
                                                     >
          
                                                    <Fields>
                                                          <telerik:PivotGridRowField DataField="salesRepName" ZoneIndex="0" Caption="Salesman " >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>

                                                        <telerik:PivotGridAggregateField DataField="Sales"     >
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Returns" >
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Net" >
                                                        </telerik:PivotGridAggregateField>

                                                       
                                                    </Fields>
         
                                                </telerik:RadPivotGrid>

                         
                           
                                                             
                                            </telerik:RadPageView>
                      </telerik:RadMultiPage>
</div>
              
   
        </div>
     
    <asp:HiddenField ID="HUID" runat="server" />
    <asp:HiddenField ID="HCurrency" runat="server" Value="(AED)" />
    <asp:HiddenField ID="HDate" runat="server" />
<asp:HiddenField ID="HToDate" runat="server" />    
              <asp:HiddenField ID="HType" runat="server" />
         <asp:HiddenField ID="HAgency" runat="server" />
         <asp:HiddenField ID="Hcustomer" runat="server" />
         <asp:HiddenField ID="HItem" runat="server" />
         <asp:HiddenField ID="HVan" runat="server" />
         <asp:HiddenField ID="HorgID" runat="server" />
          <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
            </telerik:RadAjaxPanel> 
       </telerik:RadAjaxPanel>  


    

	       
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="Btn_LoadItem" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
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