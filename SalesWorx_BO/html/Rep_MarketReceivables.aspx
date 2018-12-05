<%@ Page Title="Market Receivables" Language="vb" AutoEventWireup="false"   CodeBehind="Rep_MarketReceivables.aspx.vb" Inherits="SalesWorx_BO.Rep_MarketReceivables" MasterPageFile="~/html/Site.Master"%>
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
.k-chart svg{
	margin:0 -14px;
}

    </style>
     <script>

         function OnClientDropDownClosing(sender, eventArgs) {

             $("#MainContent_btn_LoadVan").click()

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

</script>

       <script>
           var internetUsers1 = [];
           var chartseries = [];
           

           function createlineChart() {
              
               var currency = document.getElementById('<%= HCurrency.ClientID%>').value;

               chartseries = [];
               for (var groupIx = 0; groupIx < internetUsers1.length; groupIx++) {
                   chartseries.push({
                       data: internetUsers1[groupIx].Percentage,
                       name: internetUsers1[groupIx].OrgName
                   });

               }
               $("#chart3").kendoChart({
                   theme: $(document).data("kendoSkin") || "flat",
                   dataSource: { data: internetUsers1 },
                   title: { text: "Market Receivables " + currency, align: "center", font: " bold 12px Segoe UI", color: "black" },
                   seriesDefaults: {
                       style: "smooth",
                       width: "1px", type: "line", missingValues: "interpolate", format: "{0}%",
                   },
                   seriesColors: ["#0090DF", "#79F604", "#FF00FF", "#F10909", "#1C09D5", "#3F3844", "#09CBEE", " #FFC300 ", "#B6F679", "#8C001A", "#6510E8", "#FBE5C9", "#7D83A5", "#F660AB", "#581845 ", "#F9CE1D", "#F40A78", "#5D3E49", " #04fac2", "#EC2C3B", "#003300", "#B2007E", "#F48B94", "#F94718", "#376525", "#D1B67F", "#8A0059", "#66C907", "#0090DF", "#CD6155", "#2980B9", "#6698FF", "#FFCBA4", "#F75D59"],
                   legend: { position: "bottom" },
                   tooltip: { visible: true, template: "<b>${series.name}</b></br>${category} -- ${value}", color: "#FFFFFF" },
                   series: chartseries,
                   valueAxis: {
                       title: {
                           text: "  " ,
                           font: "bold 12px Segoe UI",
                           color: "black"

                       },

                       labels: {
                           color: "black"
                       },

                   },
                   categoryAxis: {
                       categories: internetUsers1[0].Mdate,
                       majorGridLines: {
                           visible: false
                       },

                       labels: {
                           rotation: -45,
                           color: "black",
                           padding: { top: 1, left: -1 }

                       }
                   }
               });


           }

           function createChart3() {
               
               var response = []

               var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;

               var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
               var param3 = document.getElementById('<%= Hto.ClientID%>').value;

               var param5 = document.getElementById('<%= hfVan.ClientID%>').value;

               var param4 = document.getElementById('<%= UId.ClientID%>').value;

               var param6 = document.getElementById('<%= Hcount.ClientID%>').value;

               
              
               if (param1 != "" && param2 != "" & param3 != "" & param4 != "" & param5 != "") {
                 
                   $.ajax({
                       type: "POST", contentType: "application/json;charset=utf-8", url: "Chart.asmx/ReceivablesbyVan", data: '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '", param6:"' + param6 + '"}', dataType: "json", success: function (response) {

                           internetUsers1 = response.d
                           createlineChart();

                       }
                             , error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown) }

                   });
               }
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
     <h4>Market Receivables</h4>
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

	
 
     <telerik:RadAjaxPanel ID="l" runat ="server"  >
         <asp:HiddenField runat="server" ID="hfOrg" />
         <asp:HiddenField runat="server" ID="hfVan" />
         <asp:HiddenField runat="server" ID="UId" />
         <asp:HiddenField runat="server" ID="HFrom" />
         <asp:HiddenField runat="server" ID="Hto" />
         <asp:HiddenField runat="server" ID="Hcount" />
         <asp:HiddenField runat="server" ID="HCurrency" />
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
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  Filter="Contains" ID="ddlOrganization"  Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"  >
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
                                           
                                        
                                         
                                                 <div class="col-sm-6">
                                                       <div class="row">
                                                           <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>From Date</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromDate" runat="server" Width="100%">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker> 
                                                                                                              
                                                    </div>
                                                 </div>
                                                 <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>To Date</label>
                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtToDate" runat="server" Width="100%" >
                                                             <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                        </telerik:RadMonthYearPicker> 
                                                           
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Month: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Month: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
               
            </span>
            </i>      
        </div>
    </div>
  <br />
            <div class="row">
                <div class="col-sm-7"></div>
                <div class="col-sm-5">
                    <div class="form-group">
                        <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Type"  Width ="100%" runat="server" AutoPostBack="True" Visible="false"  >
                        <Items>
                            <telerik:RadComboBoxItem Value="10" Text="Top 10 Vans/FSR" Selected="true"  />
                            <telerik:RadComboBoxItem Value="0" Text="All Vans/FSR" />
                        </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
                 
  <div class="overflowx">
         <div class="chart-wrapper padding0" id="Chartid" runat="server" >
        <div id="chart3"   style ="width:100%;height:365px;">
        </div>
             </div>
        </div>

<hr />
<div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>

  <div id="summary" runat="server" class="row"></div>
              <div class="overflowx">
                  <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server"  >
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
                              
                         
             <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                      
                             <div style="display:none">
          <asp:Button  CssClass ="btn btn-primary"  ID="btn_LoadVan" runat="server" Text="Export"  /> </div>
                       

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