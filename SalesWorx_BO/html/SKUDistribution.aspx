<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="SKUDistribution.aspx.vb" Inherits="SalesWorx_BO.SKUDistribution" %>
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
         function createChart3() {
             var response = []

             var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
             var param2 = document.getElementById('<%= hVan.ClientID%>').value;
             var param3 = document.getElementById('<%= UID.ClientID%>').value;
             var param4 = document.getElementById('<%= hfrom.ClientID%>').value;
             var param5 = document.getElementById('<%= Hto.ClientID%>').value;
             var param6 = document.getElementById('<%= hType.ClientID%>').value;
             var param7 = document.getElementById('<%= Hby.ClientID%>').value;
             var param8 = document.getElementById('<%= hID.ClientID%>').value;
             var param9 = document.getElementById('<%= HSite.ClientID%>').value;

             var dataSource = {
                 type: "json",
                 transport: {
                     read: {
                         url: "Chart.asmx/SKUDistribution", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                         contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                         type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                         dataType: "json"
                     }
                       ,
                     parameterMap: function (data, operation) {

                         var output = null;
                         switch (operation) {

                             case "read":
                                 output = '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '", param6:"' + param6 + '", param7:"' + param7 + '", param8:"' + param8 + '", param9:"' + param9 + '"}';
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
                 }
                 
             }

             $("#chart3").kendoChart({
                 theme: "flat",
                 dataSource: dataSource,
                 title: {
                     text: "SKU Distribution",
                     color: "black",
                     font: "bold 12px Segoe UI"

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
                       title: {
                           text: "Price Buckets",
                           font: "bold 12px Segoe UI", color: "black"
                       },
                       labels: { font: " 12px Segoe UI", color: "black" },

                       minorGridLines: { visible: false },
                       majorGridLines: {
                           visible: false
                       }
                   },


                 valueAxis:
                   {
                       title: {
                           text: "Avg. Sales %"  ,
                           font: "bold 12px Segoe UI", color: "black"
                       },
                       max: 100,
                       labels: { font: " 12px Segoe UI", color: "black", format: "{0}%" }, majorGridLines: { visible: false }
                   },
                 seriesColors: ["#EB6357 ", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                 series: [{
                     field: "Amount",
                     categoryField: "Description",


                 }],
                 tooltip: {
                     visible: true,
                     format: "{0:N2}",
                     font: "12px Segoe UI",
                     color: "white",
                     background: "black"
                 },


             });


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
       <h4>SKU Distribution</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	
     <asp:HiddenField ID="UId" runat="server" />
    <asp:HiddenField ID="HSearched" runat="server" Value="0" />
  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
                                                    
                                                 <div class="col-sm-4">
                                                     
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddVan"  EmptyMessage="Select Van/FSR" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>

                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Type</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_Type"    Width ="100%" runat="server"  Filter="Contains">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="Q" Text="Sales Units" Selected="true"  />
                                                        <telerik:RadComboBoxItem Value="V" Text="Sales Value" />
                                                    </Items>
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>

                                                     <div class="col-sm-2" style="display:none ">
                                            <div class="form-group">
                                                <label>Distribution By</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_by"    Width ="100%" runat="server"  Filter="Contains" AutoPostBack="true" >
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="C" Text="Customer" />
                                                        <telerik:RadComboBoxItem Value="P" Text="Price List"  Selected="true"  />
                                                    </Items>
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>

                                                    <div class="col-sm-5" style="display:none ">
                                            <div class="form-group">
                                                <label><asp:Label runat="server" ID="lbl_by" Text="Customer"></asp:Label></label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_ID"    Width ="100%" runat="server"  Filter="Contains">
                                                    
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>

                                               
                                                   <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>From Date</label>
                                                   <telerik:RadDatePicker ID="txtFromDate"   Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                            </div>
                                          </div>
                                                 <div class="col-sm-4">
                                                   <div class="form-group">
                                                <label>To Date</label>
                                                  <telerik:RadDatePicker ID="txtTodate"    Width ="100%" runat="server">
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
              <p><strong>Van: </strong><asp:Label ID="lblVan" runat="server" Text=""></asp:Label></p>
              <p><strong>Type: </strong><asp:Label ID="lblType" runat="server" Text=""></asp:Label></p>
              
              <p><strong><asp:Label ID="lbl_Argbytxt" runat="server" Text="" Visible="false"></asp:Label>  </strong><asp:Label ID="lbl_ArgbyID" runat="server" Text="" Visible="false"></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>
              <asp:Label ID="lbl_Argby" runat="server" Text="" Visible="false"></asp:Label> 
            </span>
            </i>      
        </div>
    </div>

            <asp:HiddenField ID="hfOrg" runat="server" />
            <asp:HiddenField ID="hVan" runat="server" />
            <asp:HiddenField ID="hType" runat="server" />
            <asp:HiddenField ID="Hby" runat="server" />
            <asp:HiddenField ID="hID" runat="server" />
            <asp:HiddenField ID="HSite" runat="server" />
            <asp:HiddenField ID="hfrom" runat="server" />
            <asp:HiddenField ID="Hto" runat="server" />
                   
 
  <p>&nbsp;</p>
       <div id="ReptDiv" runat="server" visible="true">
<div class="overflowx">
     <div class="chart-wrapper padding0" id="Chartid" runat="server" >
        <div id="chart3" class="margin-14"  style ="width:100%;height:320px;">
        </div>
        </div>
       </div>

       </div>
            </ContentTemplate> 

  </asp:UpdatePanel> 
       <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>

	<asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel21" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
          
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
	
</asp:Content>

