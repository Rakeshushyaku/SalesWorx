<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepClientWiseSalesReturns.aspx.vb" Inherits="SalesWorx_BO.RepClientWiseSalesReturns" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
      <script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />
     <script type="text/javascript">

         function createChart3() {
             var res
            

             var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
          var param2 = document.getElementById('<%= HVan.ClientID%>').value;
          var param3 = document.getElementById('<%= HUID.ClientID %>').value;
          
          var param4 = document.getElementById('<%= HDate.ClientID%>').value;
          var param5 = document.getElementById('<%= HToDate.ClientID%>').value;

          var param6 = "G"
          var param7 = document.getElementById('<%= Hcustomer.ClientID %>').value;
          var c = document.getElementById('<%= HCount.ClientID%>').value;
          var Currency = document.getElementById('<%= HCurrency.ClientID%>').value;
             

          var dataSource = {
              type: "json",
              transport: {
                  read: {
                      url: "Chart.asmx/GetRepSalesbyClient", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                      contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                      type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                      dataType: "json"
                  }
                    ,
                  parameterMap: function (data, operation) {

                      var output = null;
                      switch (operation) {

                          case "read":
                             output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + ',param6: ' + JSON.stringify(param6) + ',param7: ' + JSON.stringify(param7) + ' }';
                           
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
                  text: "Sales Vs Returns " + Currency ,
                  color: "black",
                  font: "bold 13px Segoe UI"

              },

              legend: {
                  position: "top"
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
                        text: " ",
                        font: "bold 12px Segoe UI", color: "black"
                    },
                    labels: { font: "12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                },
              seriesColors: ["#4d5ec1", "#FF7663", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
                  name: "Sales",
                  field: "Amount",
                  categoryField: "Description",


              },

               {
                   name: "Return",
                   field: "ReturnAmount",
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






      }



      



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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h4>    Client Wise  Sales/Return </h4>
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
                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >

                                                  

                                            </div>
                                          </div>
                                                  
                                                    </div>
                                                 <div class="row">  
                                                <div class="col-sm-3">
                                            <div class="form-group">
                                                <label><asp:Label runat="server" ID="lbl_from" Text="From Date"></asp:Label></label>
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
                                                <label><asp:Label runat="server" ID="lbl_to" Text="To Date"></asp:Label></label>
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
                                                 <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Customer" EnableCheckAllItemsCheckBox="true" ID="ddlCustomer" Width ="100%" runat="server"   Filter="Contains">
                                        </telerik:RadComboBox >
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
                <p><strong>From Date</strong> <asp:Label ID="lbl_Fromdt" runat="server" Text=""></asp:Label></p>
                <p><strong>To Date</strong> <asp:Label ID="lbl_Todt" runat="server" Text=""></asp:Label></p>
               <p><strong>Customer: </strong> <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
<div>
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
<div id="summary" runat="server" class="row"></div> 
               <div class="overflowx" >
                                            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper1" runat="server" >
                                                <div id="chart3" style="height:100%" class="margin-14"> </div>
                                                </div>
                                                </div>
               <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>

              <div class="overflowx">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Location" HeaderText="Client"
                                                                  SortExpression ="Location" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sqty" HeaderText="Sales Qty"
                                                                  SortExpression ="Sqty" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SValue" HeaderText="Sales Value"
                                                                  SortExpression ="SValue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Rqty" HeaderText="Returns Qty"  
                                                                  SortExpression ="Rqty" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="RValue" HeaderText="Returns Value"  
                                                                  SortExpression ="RValue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ReturnPerc" HeaderText="Return %"  
                                                                  SortExpression ="ReturnPerc" DataType="System.Double" DataFormatString="{0:N2}">
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
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                  </div>

              <asp:HiddenField ID="HUID" runat="server" />
    <asp:HiddenField ID="HCurrency" runat="server" Value="(AED)" />
    <asp:HiddenField ID="HDate" runat="server" />
<asp:HiddenField ID="HToDate" runat="server" />    
                           <asp:HiddenField ID="Hcustomer" runat="server" />
         
         <asp:HiddenField ID="HVan" runat="server" />
         <asp:HiddenField ID="HorgID" runat="server" />
         <asp:HiddenField ID="HCount" runat="server" />

            </telerik:RadAjaxPanel>
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