<%@ Page Title="FSR Collection" Language="vb" AutoEventWireup="false" CodeBehind="Rep_FSRCollection.aspx.vb" Inherits="SalesWorx_BO.Rep_FSRCollection"  MasterPageFile="~/html/Site.Master"%>
 



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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


    </style>
     <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }  
   .k-chart svg{
	margin:0 -14px;
}    
</style>
   <script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

  <script type="text/javascript">

      function OnClientDropDownClosing(sender, eventArgs) {
          
          $("#MainContent_btn_LoadVan").click()
          
      }

      function createChart3() {
          var param1 = document.getElementById('<%= HOrgID.ClientID%>').value;
          var param2 = document.getElementById('<%= HUID.ClientID %>').value;
          var param3 = document.getElementById('<%= HFrom.ClientID%>').value;
          var param4 = document.getElementById('<%= HToDate.ClientID%>').value;
          var param5 = document.getElementById('<%= HVan.ClientID%>').value;
          var param6 = document.getElementById('<%= hfCC.ClientID%>').value;
          var Currency
          Currency = document.getElementById('<%= HCurrency.ClientID%>').value;
          if (param6 == "False") {
              $("#chart3").kendoChart({
                  theme: "flat",
                  dataSource: {
                      type: "json",
                      transport: {
                          read: {
                              url: "Chart.asmx/GetFSRCollection", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                              contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                              type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                              dataType: "json"
                          }
                            ,
                          parameterMap: function (data, operation) {

                              var output = null;
                              switch (operation) {

                                  case "read":
                                      output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ' }';
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

                                  Description: { type: "string" },
                                  Amount: { type: "number" },
                                  Amount1: { type: "number" },
                                  Amount2: { type: "number" }

                              }
                          }
                      },
                      sort: {
                          field: "Description",
                          dir: "asc"
                      }
                  },
                  title: {
                      text: "FSR Collection " + Currency,
                      color: "black",
                      font: "bold 12px Segoe UI"

                  },

                  legend: {
                      visible: true,
                      position: "top",
                      font: "12px Segoe UI"
                  },
                  seriesDefaults: {
                      type: "column",
                      stack: true,
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
                        labels: { font: "12px Segoe UI", color: "black", rotation: -90 },

                        minorGridLines: { visible: false },
                        majorGridLines: {
                            visible: false
                        }
                    },


                  valueAxis:
                    {
                        title: { text: " Collected Amount", font: "bold 12px Segoe UI", color: "black" },
                        labels: { font: "12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                    },
                  seriesColors: ["#4d5ec1", "#eb6357", "#14b4fc", "#FFFF99", "#FFCC99"],
                  series: [{
                      field: "Amount",
                      categoryField: "Description",
                      name: "Cash"

                  },

                  {
                      field: "Amount2",
                      categoryField: "Description",
                      name: "CDC"

                  }, {
                      field: "Amount1",
                      categoryField: "Description",
                      name: "PDC"

                  }],
                  tooltip: {
                      visible: true,
                      format: "{0:N0}",
                      font: "12px Segoe UI",
                      color: "white",
                      background: "black",
                      template: "#= series.name #: #= value #"
                  }
              });
          }

          else {
              $("#chart3").kendoChart({
                  theme: "flat",
                  dataSource: {
                      type: "json",
                      transport: {
                          read: {
                              url: "Chart.asmx/GetFSRCollection", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                              contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                              type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                              dataType: "json"
                          }
                            ,
                          parameterMap: function (data, operation) {

                              var output = null;
                              switch (operation) {

                                  case "read":
                                      output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ' }';
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

                                  Description: { type: "string" },
                                  Amount: { type: "number" },
                                  Amount1: { type: "number" },
                                  Amount2: { type: "number" },
                                  Amount3: { type: "number" }

                              }
                          }
                      },
                      sort: {
                          field: "Description",
                          dir: "asc"
                      }
                  },
                  title: {
                      text: "FSR Collection " + Currency,
                      color: "black",
                      font: "bold 12px Segoe UI"

                  },

                  legend: {
                      visible: true,
                      position: "top",
                      font: "12px Segoe UI"
                  },
                  seriesDefaults: {
                      type: "column",
                      stack: true,
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
                        labels: { font: "12px Segoe UI", color: "black", rotation: -90 },

                        minorGridLines: { visible: false },
                        majorGridLines: {
                            visible: false
                        }
                    },


                  valueAxis:
                    {
                        title: { text: " Collected Amount", font: "bold 12px Segoe UI", color: "black" },
                        labels: { font: "12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                    },
                  seriesColors: ["#4d5ec1", "#eb6357", "#14b4fc", "#97c95d", "#FFCC99"],
                  series: [{
                      field: "Amount",
                      categoryField: "Description",
                      name: "Cash"

                  },

                  {
                      field: "Amount2",
                      categoryField: "Description",
                      name: "CDC"

                  }, {
                      field: "Amount1",
                      categoryField: "Description",
                      name: "PDC"

                  },
                  
                  {
                      field: "Amount3",
                      categoryField: "Description",
                      name: "CC"

                  }
                  ],
                  tooltip: {
                      visible: true,
                      format: "{0:N0}",
                      font: "12px Segoe UI",
                      color: "white",
                      background: "black",
                      template: "#= series.name #: #= value #"
                  }
              });
          }


          }
      
      function RefreshChart() {
          createChart3();
      }
    </script>   

     <script>


          

         function alertCallBackFn(arg) {

         }

         function clickSearch() {
             $("#MainContent_SearchBtn").click()
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
</script>
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>FSR Collection</h4>
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
        <asp:HiddenField ID="hfCC" runat ="server" />
                <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"  >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">
                                      <ContentTemplate>
                                         <div class="row">
                                              <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                      <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>

                                         <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"  AutoPostBack="true" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Van/FSR" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                 <div class="col-sm-6">
                                                   <div class="row">
                                                       <div class="col-sm-6">
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

                                                        <div class="col-sm-6">
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
                                                     </div>
                                                     

                                                 </div>
                                                
                                             
                                           
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>  </label>
                                                 
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
                                        
   <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>Van: </strong><asp:Label ID="lbl_Van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_FromDate" runat="server" Text=""></asp:Label></p>
               <p><strong>To Date: </strong> <asp:Label ID="lbl_ToDate" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
         
                              <div id="summary" runat="server" class="row"></div> 
            <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
               <asp:HiddenField ID="HUID" runat="server" />
    
         <br />
         <div class="overflowx" >

                <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper" runat="server" >
<div id="chart3" style="height:100%"> </div>
</div>

                </div>

<p><br /><br /></p>
               
                   <div class="table-responsive" id="Detailed"  runat="server">

      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server" >
        <asp:HiddenField ID="HCurrency" runat="server" Value="(AED)" /><div id="dvCurrency" runat="server" visible="false" style="width:100%"  >
          <h5 class='text-right'  style="width:100%" >Currency <span class='text-blue'><strong><asp:label runat="server" ID="lblCurrency"></asp:label></strong></span></h5></div>
          
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_name" HeaderText="SalesRep"
                                                                  SortExpression ="SalesRep_name"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                          
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Cash" HeaderText="Cash"
                                                                  SortExpression ="CashAmount" DataType="System.Double" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                             <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Cheque" HeaderText="CDC"
                                                                  SortExpression ="Cheque" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                             <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="PDC" HeaderText="PDC"
                                                                  SortExpression ="PDC" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                             <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CC" HeaderText="CC"
                                                                  SortExpression ="CC" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                             <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="tot" HeaderText="Total"
                                                                  SortExpression ="tot" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                            <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
              
          </telerik:RadAjaxPanel> 
                           
                
</div>
                   
           

            
           
   
  
     
    
     <div style="display:none">
          <asp:Button  CssClass ="btn btn-primary"  ID="btn_LoadVan" runat="server" Text="Export"  /> </div>
        <asp:HiddenField ID="HToDate" runat="server" />
        <asp:HiddenField ID="HVan" runat="server" />
        <asp:HiddenField ID="HOrgID" runat="server" />

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