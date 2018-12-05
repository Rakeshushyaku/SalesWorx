<%@ Page Title="PDC Receivables" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_PDCReceivables.aspx.vb" Inherits="SalesWorx_BO.Rep_PDCReceivables" %>


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
        .RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

    .RadPanelBar_Simple .rpExpandable span.rpExpandHandle{
    background-position: 100% -5px !important;
}
    
    </style>
    <script>
        function clickSearch() {
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

        function OpenViewWindow(cid) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            URL = 'RepDetails.aspx?OrgID=' + combo.get_selectedItem().get_value() + '&Type=Cust&ReportName=CustomerDetails&ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        document.onkeydown = function (evt) {

            evt = evt || window.event;
            if (evt.keyCode == 27) {

                HideRadWindow();
            }
        };

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }


        }

    </script>
     <script>
         function createChart3() {
             
             var combo = $find('<%= ddlOrganization.ClientID%>');
                        var param1 = combo.get_selectedItem().get_value()

                        var comboVan = $find('<%= ddlVan.ClientID%>');
                    var param2 = comboVan.get_selectedItem().get_value()

                    var param3 = document.getElementById('<%= UID.ClientID%>').value;

                    if (param1 != "0" && param2 != "0" && param3 != "0") {
                        $("#chart3").kendoChart({

                            dataSource: {
                                type: "json",
                                transport: {
                                    read: {
                                        url: "Chart.asmx/GetPDCReceivables", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                                        contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                                        type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                                        dataType: "json"
                                    }
                                      ,
                                    parameterMap: function (data, operation) {

                                        var output = null;
                                        switch (operation) {

                                            case "read":
                                                output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + '}';
                                                
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
                                text: "",
                                color: "white",
                                font: "14px Calibri"

                            },


                            seriesDefaults: {
                                type: "column", labels: {
                                    visible: false,
                                    format: "{0:N0}",

                                    font: "12px Calibri"
                                }

                            },
                            chartArea: {
                                background: "White"

                            },
                            categoryAxis:
                              {
                                  labels: { font: "12px Calibri", color: "#428bca" },

                                  minorGridLines: { visible: false },
                                  majorGridLines: {
                                      visible: false
                                  }
                              },


                            valueAxis:
                              {

                                  labels: { font: "12px Calibri", color: "#428bca" }, majorGridLines: { visible: false }
                              },
                            seriesColors: ["#6699FF", "#CCCC66", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                            series: [{
                                field: "Amount",
                                categoryField: "Description",


                            }],
                            tooltip: {
                                visible: true,
                                format: "{0:N0}",
                                font: "12px Calibri",
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
       <h4>PDC Receivables</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px">
     <asp:HiddenField ID="UId" runat="server" />
  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                            </div>
                                             <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-primary"  ID="SearchBtn" runat="server" Text="Search" OnClientClick="clickSearch()" />
                                                    <div class="form-group fontbig"> <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                       </div>
                                                    </div>
                                                 
                                            </div>
             
  </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
                              
                              <div id="summary" runat="server" class="row"></div> 
           
            
 </ContentTemplate> 

  </asp:UpdatePanel> 
  
     <div class="chart-wrapper">
        <div id="chart3"   style ="width:100%;height:215px;">
        </div>
        </div>
       
</td>
</tr>
	 
    </table>

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
