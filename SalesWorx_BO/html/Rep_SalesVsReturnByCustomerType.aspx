<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_SalesVsReturnByCustomerType.aspx.vb" Inherits="SalesWorx_BO.Rep_SalesVsReturnByCustomerType" %>
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
     <script>

         function RefreshChart() {
             createChart3();
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
    display:none;        
   }      
</style>

    <script>
        function createChart3() {


            var param1 = document.getElementById('<%= hfOrgID.ClientID%>').value;
            var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
            var param3 = document.getElementById('<%= HTo.ClientID%>').value;
            var param4 = document.getElementById('<%= UID.ClientID%>').value;
            var param5 = document.getElementById('<%= HfVan.ClientID%>').value;

            $("#chart5").kendoChart(
                {

           


               dataSource: {
                   type: "json",
                   transport: {
                       read: {
                           url: "Chart.asmx/SalesVseReturnByCustType", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
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
                               Amount: { type: "number" }

                           }
                       }
                   },
                   sort: {
                       field: "Description",
                       dir: "asc"
                   }
               },
               title: {
                   text: "Sales Vs. Returns by Customer Type",
                   color: "black",
                   font: "bold 14px Segoe UI"

               },

               legend: {
                   position: "top"
               },

               seriesDefaults: {
                   type: "column", labels: {
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
                     title: { text: "  ", font: "12px Segoe UI", color: "black" },
                     labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                 },
               seriesColors: ["#4d5ec1", "#FF7663", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99"],
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
               }
           });
       }

    </script> 

    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Sales Vs. Returns by Customer Type</h4>
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
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van/FSR" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                         <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server" AutoPostBack="true" >
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
                                                          <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server" AutoPostBack="true" >
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
                                             <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button   CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
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
                                        
 
         
   <asp:HiddenField ID="hfOrgID" runat ="server" />
   <asp:HiddenField ID="UID" runat ="server" />
   <asp:HiddenField ID="HfVan" runat ="server" />
   <asp:HiddenField ID="HFrom" runat ="server" />
   <asp:HiddenField ID="HTo" runat ="server" />
   <asp:HiddenField ID="HCurrency" runat ="server" /> 

    <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>    
            </span>
            </i>      
        </div>
    </div>

       
           <div class="row">
               <div class="col-sm-8">
                   <div id="summary" runat="server" class="row"></div>
               </div>
                <div class="col-sm-12">
               
                 <p><br /></p>
                     
                    <div class="overflowx">
                                            <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper1" runat="server" >
                                                <div id="chart5" style="height:100%;" class="margin-14"> </div>
                                                </div>
                                               </div>

                    <p><br /></p>        
           </div>                                  
             </div>
        <div class="row">     
                <div class="col-sm-8">
                    <div style="height:4px;">&nbsp;</div>
              <div class="table-responsive">
                   <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server" >
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" OnItemCommand="gvRep_ItemCommand">
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10" CommandItemDisplay="Top" >
                    <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_type" HeaderText="Customer Type"
                                                                  SortExpression ="Customer_type" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales" HeaderText="Total Sales"
                                                                  SortExpression ="Sales" DataFormatString="{0:f2}" DataType="System.Decimal" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Returns" HeaderText="Total Returns"
                                                                  SortExpression ="Returns" DataFormatString="{0:f2}" DataType="System.Decimal"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Perc" HeaderText="Return Percentage"
                                                                  SortExpression ="Perc" DataFormatString="{0:f2}" DataType="System.Decimal"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>


                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                         
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                    </telerik:RadAjaxPanel>       
                 </div>
                    </div>
                </div>              
                        <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>            
                              <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>  
                       

                           </telerik:RadAjaxPanel>  

     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>

        


</asp:Content>