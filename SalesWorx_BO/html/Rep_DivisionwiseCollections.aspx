<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DivisionwiseCollections.aspx.vb" Inherits="SalesWorx_BO.Rep_DivisionwiseCollections" %>

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
.k-chart svg{
	margin:0 -14px;
}
     
    
    </style>
    <script>
         
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

        
        function RefreshChart() {
            
            createChart3();
        }
        

    </script>
     <script>
         function createChart3() {

             var param1 = document.getElementById('<%= hfOrgID.ClientID%>').value;
             var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
            var param4 = document.getElementById('<%= UID.ClientID%>').value;

            
             if (param1 == "")
                 param1="0"

                    if (param1 != "" && param2 != "") {
                        $("#chart3").kendoChart({
                            theme: "flat",
                            dataSource: {
                                type: "json",
                                transport: {
                                    read: {
                                        url: "Chart.asmx/DivisionWiseCollection", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                        contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                        type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                        dataType: "json"
                                    }
                                      ,
                                    parameterMap: function (data, operation) {

                                        var output = null;
                                        switch (operation) {

                                            case "read":
                                                output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param4: ' + JSON.stringify(param4) +  '}';

                                                break;

                                        }
                                        console.debug(data)
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
                            },
                            title: {
                                text: "Delayed % by Division",
                                color: "black",
                                font: "bold 12px Segoe UI"

                            },


                            seriesDefaults: {
                                type: "column", labels: {
                                    visible: false,
                                    format: "{0}%",

                                    font: "12px Segoe UI"
                                }

                            },
                            chartArea: {
                                background: "White"

                            },
                            categoryAxis:
                              {
                                  labels: { font: " 12px  Segoe UI", color: "black" },
                                  
                                  minorGridLines: { visible: false },
                                  majorGridLines: {
                                      visible: false
                                  }
                              },


                            valueAxis:
                              {
                                  title: {
                                      text: " ",
                                      font: "bold 12px Segoe UI",
                                      color: "#428bca"

                                  },
                                  max: 100,
                                  labels: { font: " 12px Segoe UI", color: "black", format: "{0}%" }, majorGridLines: { visible: false }
                              },
                            seriesColors: ["#14b4fc"],
                            series: [{
                                field: "Amount",
                                categoryField: "Description",


                            }],
                            tooltip: {
                                visible: true,
                                format: "{0}%",
                                font: "12px Segoe UI",
                                color: "white",
                                background: "black"
                            }
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
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Division Wise Collection</h4>
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
     <asp:HiddenField ID="UId" runat="server" />
    <asp:HiddenField ID="hfOrgID" runat ="server" />
    <asp:HiddenField ID="HFrom" runat ="server" />
 <asp:HiddenField ID="HSearched" runat="server" Value="0" />

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
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select Organization"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                            </div>
                                                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month</label>
                                                  <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtFromDate" runat="server">
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
         <br />
                              <div id="summary" runat="server" class="row"></div> 

            <asp:HiddenField ID="hfCurrency" runat="server" />
                                 <asp:HiddenField ID="hfDecimal" runat="server" />
             
             <div class="table-responsive" id="Detailed"  runat="server">

       <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="true" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server"  Width="100%"
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
             <p><br /><br /></p>
   
  <div class="overflowx" >
     <div class="chart-wrapper padding0" id="Chartid" runat="server" >
        <div id="chart3"   style ="width:100%;height:245px;">
        </div>
        </div>
       </div>
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
