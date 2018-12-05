<%@ Page Title="Journey Plan Adherence" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_JournPlanAd.aspx.vb" Inherits="SalesWorx_BO.Rep_JournPlanAd" %>
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

 #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:70px !important;
        }
    </style>
     <script>
         function onDataBound(e) {
             $("#MainContent_Panel1").hide()

         }

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
         function clickExportBiffExcel() {

             $("#MainContent_BtnExportBiffExcel").click()
             return false

         }

         function pageLoad(sender, args) {

             $('[data-toggle="tooltip"]').tooltip();
         }

         function createChart3() {

             var res
             var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;
          var param2 = document.getElementById('<%= hfVan.ClientID%>').value;
             var param3 = document.getElementById('<%= UId.ClientID%>').value;
             var param4 = document.getElementById('<%= HFrom.ClientID%>').value;


             var M = document.getElementById('<%= M.ClientID%>').value;
             var M1 = document.getElementById('<%= M1.ClientID%>').value;
             var M2 = document.getElementById('<%= M2.ClientID%>').value;
             $("#MainContent_Panel1").show()

             var dataSource = {
                 type: "json",
                 transport: {
                     read: {
                         url: "Chart.asmx/JPAdherence", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                         contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                         type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                         dataType: "json"
                     }
                       ,
                     parameterMap: function (data, operation) {

                         var output = null;
                         switch (operation) {

                             case "read":
                                 output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ' }';
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
                             Amount: { type: "number" },
                             ReturnAmount: { type: "number" },
                             Amount1: { type: "number" }
                         }
                     }
                 },

             }

             $("#chart3").kendoChart({
                 theme: "flat",
                 dataSource: dataSource,
                 dataBound: onDataBound,
                 title: {
                     text: "JP Adherence ",
                     color: "black",
                     font: "bold 12px Segoe UI"

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
                       max: 100,
                       title: {
                           text: "JP Adherence %",
                           font: "bold 12px Segoe UI", color: "black"
                       },
                       labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                   },
                 seriesColors: ["#FF7663", "#10C4B2", "#ef9933", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                 series: [
                  {
                      name: M2,

                      field: "Amount1",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} :  #=kendo.format('{0:N2}',dataItem.Amount1)# ",
                          font: "12px Segoe UI",
                          format: "{0:N2}",
                          color: "white",
                          background: "black"
                      },


                  },

                  {
                      name: M1,

                      field: "ReturnAmount",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} :  #=kendo.format('{0:N2}',dataItem.ReturnAmount)# ",
                          font: "12px Segoe UI",
                          format: "{0:N2}",
                          color: "white",
                          background: "black"
                      },

                  },

                  {
                      name: M,

                      field: "Amount",
                      categoryField: "Description",
                      tooltip: {
                          visible: true,
                          template: "Van: <b>${category}</b></br>${series.name} :  #=kendo.format('{0:N2}',dataItem.Amount)# ",
                          font: "12px Segoe UI",
                          format: "{0:N2}",
                          color: "white",
                          background: "black"
                      },


                  }


                 ]


             });

             var dist
             dist = document.getElementById('<%= HUseDistributionIncall.ClientID%>').value

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").html("Adherence % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Adherence to route plan'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").html("Adherence % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Adherence to route plan'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").html("Adherence % <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Adherence to route plan'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-color", "#ff7663")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_12").css("background-image", "none")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-color", "#10c4b2")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_15").css("background-image", "none")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-color", "#ef9933")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-image", "none")

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-color", "#ef9933")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_18").css("background-image", "none")

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").html("Planned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits planned'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-color", "#0090d9")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_10").css("background-image", "none")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").html("Planned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits planned'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-color", "#0090d9")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_13").css("background-image", "none")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").html("Planned <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits planned'></i>")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("background-color", "#0090d9")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_16").css("background-image", "none")

          if (dist == 'Y')
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan in which Distribution check was performed'></i>")
          else
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan'></i>")

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-color", "#97c95d")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_11").css("background-image", "none")

          if (dist == 'Y')
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan in which Distribution check was performed'></i>")
          else
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan'></i>")

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-color", "#97c95d")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_14").css("background-image", "none")

          if (dist == 'Y')
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan in which Distribution check was performed'></i>")
          else
              $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").html("Calls <i class='fa fa-info-circle' data-container='body' data-toggle='tooltip' title='Visits made according to route plan'></i>")

          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("background-color", "#97c95d")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("color", "#FFFFFF")
          $("#ctl00_MainContent_gvRep_ctl00_DataZone_ctl02_17").css("background-image", "none")



      }



</script>
  <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
    
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Journey Plan Adherence </h4>
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
           
      <asp:HiddenField runat="server" ID="hfOrg" />
         <asp:HiddenField runat="server" ID="hfVan" />
         <asp:HiddenField runat="server" ID="UId" />
         <asp:HiddenField runat="server" ID="HFrom" />
       <asp:HiddenField ID="M1" runat="server" />
       <asp:HiddenField ID="M2" runat="server" />
       <asp:HiddenField ID="M" runat="server" />
              <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
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
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                           </div>
                                        </div>
                                        <div class="row">
                                                           <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>Month</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" Width ="100%"  ID="txtFromDate" runat="server">
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
       <div id="RepDiv" runat="server" visible="false" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Month: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Month: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
               
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div>
              


 

 
              <div class="overflowx" style="position:relative;">
                  <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel2">

                       <div style="position:absolute;padding:5px;top:0;left:0;">
                                     <asp:ImageButton ID="img_export" runat="server" ImageUrl ="../assets/img/export-excel.png"  Visible="false"  OnClientClick="clickExportBiffExcel()"></asp:ImageButton>
                            </div>
                      </telerik:RadAjaxPanel>
                  <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">

                  <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="true"  PageSize="10"
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" cssClass="no-wrap" 
                                                     >
          <TotalsSettings GrandTotalsVisibility="None"  />
                                                    <Fields>
                                                          <telerik:PivotGridRowField DataField="salesRepName" ZoneIndex="0" Caption="Salesman " >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                         <telerik:PivotGridAggregateField DataField="Planned"   >
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Calls"  >
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Adherence" Caption="Adherence %" >
                                                        </telerik:PivotGridAggregateField>
                                                    </Fields>
         
                                                </telerik:RadPivotGrid>
                     </telerik:RadAjaxPanel> 
                  </div>
           <p><br /></p>
                  <div class="overflowx" >
                               <div class="chart-wrapper padding0" id="Chartid" runat="server">
        <div id="chart3" class="margin-14"  style ="width:100%;height:365px;">
        </div>
        </div>
                  </div>            
                         
             <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                      
                             
                       </div>

                           </telerik:RadAjaxPanel>

    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
        <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>
     <asp:HiddenField ID="HUseDistributionIncall" runat="server" Value="N" />
   <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>

    <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>
</asp:Content>