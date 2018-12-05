<%@ Page Title="Revenue Dispersion" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_RevenueDispersion.aspx.vb" Inherits="SalesWorx_BO.Rep_RevenueDispersion" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
             var param3 = document.getElementById('<%= hID.ClientID%>').value;
             var param4 = document.getElementById('<%= hfrom.ClientID%>').value;


             var dataSource = {
                 type: "json",
                 transport: {
                     read: {
                         url: "Chart.asmx/RevenueDispersion", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                         contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                         type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                         dataType: "json"
                     }
                       ,
                     parameterMap: function (data, operation) {

                         var output = null;
                         switch (operation) {

                             case "read":
                                 output = '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '"}';
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
                     text: "Revenue Dispersion",
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
                           text: "Billing Amount",
                           font: "bold 12px Segoe UI", color: "bold"
                       },
                       labels: { font: " 12px Segoe UI", color: "bold" },

                       minorGridLines: { visible: false },
                       majorGridLines: {
                           visible: false
                       }
                   },


                 valueAxis:
                   {


                       labels: { font: " 12px Segoe UI", color: "bold", format: "{0}" }, majorGridLines: { visible: false }
                   },
                 seriesColors: ["#EB6357 ", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                 series: [{
                     name: "No of Invoices",
                     field: "Amount",
                     categoryField: "Description",
                 },
                 {
                     name: "No of Customers",
                     field: "ReturnAmount",
                     categoryField: "Description",


                 }
                 ],
                 tooltip: {
                     visible: true,
                     format: "{0:N0}",
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

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Revenue Dispersion</h4>
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px" >

   <telerik:RadAjaxPanel ID="l" runat ="server"  >
	<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"    >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
  <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                     <telerik:RadComboBox Skin="Simple" Filter="Contains"  ID="ddlOrganization"  Width ="100%" AutoPostBack="true"
                                            runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                                        </telerik:RadComboBox>
                                       </div></div> 
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                         <telerik:RadComboBox ID="ddl_Van" Skin="Simple" EmptyMessage="Select Van/FSR" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AutoPostBack="true"
                                            CssClass="inputSM"    Width ="100%"  >
                                                   </telerik:RadComboBox>
                                           </div></div>

                                                      

                                                       <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month</label>
                                                <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker> 
                                             </div></div>


                                                    </div>
                                                </div>
      <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
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
              <p><strong>Van: </strong><asp:Label ID="lblVan" runat="server" Text=""></asp:Label></p>
              <p><strong>Month: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
     <div class="overflowx">
         <div class="chart-wrapper padding0" id="Chartid" runat="server" >
        <div id="chart3"   style ="width:100%;height:320px;">
        </div>
        </div>  
         </div>     
         <p><br /><br /></p>

          <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" Width="75%" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Billing" HeaderText="Billing Amount"
                                                                  SortExpression ="Billing" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Invoice" HeaderText="No of Invoices"
                                                                  SortExpression ="Invoice" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Customer" HeaderText="No of Customers"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>
                                                             
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
        <asp:HiddenField ID="hfOrg" runat="server" />
            <asp:HiddenField ID="hVan" runat="server" />
            <asp:HiddenField ID="hID" runat="server" />
           <asp:HiddenField ID="hfrom" runat="server" />
            
              
   
    </telerik:RadAjaxPanel>
  
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
     
</td>
</tr>
	 
  
    </table>
	 
</asp:Content>