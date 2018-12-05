<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ProductiveCalls.aspx.vb" Inherits="SalesWorx_BO.Rep_ProductiveCalls" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

    <style type="text/css">
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
        #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:70px !important;
        }
.rpgTable th.rpgColumnHeader a.rpgIcon {
    display:none;
}
    </style>
    <script>
        function pageLoad(sender, args) {

            $('[data-toggle="tooltip"]').tooltip();
        }
        function createChart3() {
            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
            var param2 = document.getElementById('<%= HSID.ClientID%>').value;
            var param3 = document.getElementById('<%= HUID.ClientID%>').value;
            

            var M = document.getElementById('<%= HM.ClientID%>').value;
            var M1 = document.getElementById('<%= HM1.ClientID%>').value;
            var M2 = document.getElementById('<%= HM2.ClientID%>').value;

             var dataSource = {
                 type: "json",
                 transport: {
                     read: {
                         url: "Chart.asmx/ProdcutiveCallsCustomer", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                         contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                         type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                         dataType: "json"
                     }
                       ,
                     parameterMap: function (data, operation) {

                         var output = null;
                         switch (operation) {

                             case "read":
                                 output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3)   + ' }';
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
                     text: "Productive Customers",
                     color: "black",
                     font: "bold 14px Segoe UI"

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
                       labels: { font: " 12px Segoe UI", color: "bold", rotation: -90 },

                       minorGridLines: { visible: false },
                       majorGridLines: {
                           visible: false
                       }
                   },


                 valueAxis:
                   {
                       title: {
                           text: "No. of Customers"  ,
                           font: "bold 12px Segoe UI", color: "bold"
                       },
                       labels: { font: " 12px Segoe UI", color: "bold" }, majorGridLines: { visible: false }
                   },
                 seriesColors: ["Tomato", "#10c4b2", "#ef9933", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                 series: [{
                     name: M2,
                     field: "Amount",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>${series.name} : Customers - ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     }
                 },

                  {
                      name: M1,
                      field: "ReturnAmount",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} :  Customers -  ${value}",
                          font: "12px Segoe UI",
                          color: "white",
                          background: "black"
                      },


                  },

                  {
                      name: M,
                      field: "Amount1",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} :  Customers -  ${value}",
                          font: "12px Segoe UI",
                          color: "white",
                          background: "black"
                      },


                  }
                 ],
                  


             });






         }

        function createChart4() {
            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
             var param2 = document.getElementById('<%= HSID.ClientID%>').value;
             var param3 = document.getElementById('<%= HUID.ClientID%>').value;

            var M = document.getElementById('<%= HM.ClientID%>').value;
            var M1 = document.getElementById('<%= HM1.ClientID%>').value;
            var M2 = document.getElementById('<%= HM2.ClientID%>').value;

             var dataSource = {
                 type: "json",
                 transport: {
                     read: {
                         url: "Chart.asmx/ProdcutiveCallsVisits", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                         contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                         type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                         dataType: "json"
                     }
                       ,
                     parameterMap: function (data, operation) {

                         var output = null;
                         switch (operation) {

                             case "read":
                                 output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ' }';
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
                     text: "Productive Calls",
                     color: "black",
                     font: "bold 14px Segoe UI"

                 },

                 legend: {
                     position: "top",
                     
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
                           text: "No. of Calls",
                           font: "bold 12px Segoe UI", color: "black"
                       },
                       labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                   },
                 seriesColors: ["Tomato", "#10c4b2", "#ef9933", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                 series: [{
                     name: M2,
                     field: "Amount",
                     categoryField: "Description",
                     tooltip: {
                         visible: true,
                         template: "Van: <b>${category}</b></br>${series.name} :  Calls -  ${value}",
                         font: "12px Segoe UI",
                         color: "white",
                         background: "black"
                     }
                 },

                  {
                      name: M1,
                      field: "ReturnAmount",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} : Calls - ${value}",
                          font: "12px Segoe UI",
                          color: "white",
                          background: "black"
                      },


                  },

                  {
                      name: M,
                      field: "Amount1",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} : Calls - ${value}",
                          font: "12px Segoe UI",
                          color: "white",
                          background: "black"
                      },


                  }
                 ],



             });






         }

         


        function alertCallBackFn(arg) {

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

        function UpdateHeader() {

            createChart3();
            createChart4()
            
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-color", "Tomato")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-color", "#10c4b2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-image", "none")

            
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-color", "#ef9933")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-color", "Tomato")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-color", "#10c4b2")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-image", "none")

            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-color", "#ef9933")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("color", "#FFFFFF")
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-image", "none")

            
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl01_ctl00_ColumnExpandCollapseButton_0_0").hide()
            $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl01_ctl02_ColumnExpandCollapseButton_4_0").hide()
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <h4>Productive Calls</h4>

<telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
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

   <telerik:RadAjaxPanel runat="server" ID="g">
           
        <asp:HiddenField ID="HSID" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />
       <asp:HiddenField ID="HUID" runat="server" />

        <asp:HiddenField ID="HM" runat="server" />
        <asp:HiddenField ID="HM1" runat="server" />
       <asp:HiddenField ID="HM2" runat="server" />


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
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
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
                                              </div>
                                                    
                                                    <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
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
       <div id="RepDiv" runat="server" visible="false" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
               
               
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div>
             
                              <div class="table-responsive">
                                 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  AllowSorting="false" 
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"   ColumnsSubTotalsPosition="None"   />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" ZoneIndex="0"  UniqueName="Description" >
                                                                 
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Type" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None"  >
                 
                                                        </telerik:PivotGridColumnField>

                                                         <telerik:PivotGridAggregateField DataField="C1" SortOrder="None" >  
                                                                     
                                                        </telerik:PivotGridAggregateField>
                                                        
                                                         
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>

                              </div>
           <p><br /><br /></p>
            <h3>Customers</h3>
            <div class="overflowx">
            <div class="chart-wrapper padding0" style="height:400px;" id="Chartwrapper" runat="server" >
                                                <div id="chart3" style="height:100%" class="margin-14"> </div>
                                                </div>
                 </div>
            <p><br /></p>
           <hr />
            <p><br /></p>
           <h3>Calls</h3>
            <div class="overflowx">
            <div class="chart-wrapper padding0" style="height:400px;" id="Chartwrapper1" runat="server" >
                                                <div id="chart4" style="height:100%" class="margin-14"> </div>
                                                </div>
                 </div>
        </div>
              

    </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
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



