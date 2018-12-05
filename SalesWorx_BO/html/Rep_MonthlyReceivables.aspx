<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_MonthlyReceivables.aspx.vb" Inherits="SalesWorx_BO.Rep_MonthlyReceivables" %>
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
         .k-chart svg{
	margin:0 -14px;
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
        function OnClientDropDownClosing(sender, eventArgs) {

            $("#MainContent_btn_LoadVan").click()

        }
        function OpenViewWindow(cid) {
            var URL

            var param1 = document.getElementById('<%= hfOrgID.ClientID%>').value;
            var param2 = document.getElementById('<%= hYr.ClientID%>').value;
            var param3 = document.getElementById('<%= hMonth.ClientID%>').value;
            var param4 = document.getElementById('<%= UID.ClientID%>').value;

            URL = 'RepDetails.aspx?Type=MonthReceivable&ReportName=MonthlyReceicvablesbyCust&SID=' + cid + '&OrgID=' + param1 + '&Yr=' + param2 + '&M='+ param3 + '&Uid=' + param4;
            
            var oWnd = radopen(URL, null);
            oWnd.SetSize(650, 600);
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

            var param1 = document.getElementById('<%= hfOrgID.ClientID%>').value;
            var param2 = document.getElementById('<%= hYr.ClientID%>').value;
            var param3 = document.getElementById('<%= hMonth.ClientID%>').value;
            var param4 = document.getElementById('<%= UID.ClientID%>').value;
            var param5 = document.getElementById('<%= hfVan.ClientID%>').value;
            Currency = document.getElementById('<%= HCurrency.ClientID%>').value;
            $("#chart5").kendoChart({
                theme: "flat",
                dataSource: {
                    type: "json",
                    transport: {
                        read: {
                            url: "Chart.asmx/GetMonthyReceivables", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                            contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                            type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                            dataType: "json"
                        }
                          ,
                        parameterMap: function (data, operation) {

                            var output = null;
                            switch (operation) {

                                case "read":
                                    output = '{ param1: ' + JSON.stringify(param1) + ',param2: ' + JSON.stringify(param2) + ',param3: ' + JSON.stringify(param3) + ',param4: ' + JSON.stringify(param4) + ',param5: ' + JSON.stringify(param5) + '}';
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
                    text: "Receivables for a month " + Currency,
                    color: "black",
                    font: "bold 12px Segoe UI"

                },


                seriesDefaults: {
                    type: "column",
                    
                    labels: {
                        visible: false,
                        format: "{0:N0}",
                        color: "black",
                        font: "bold 12px Segoe UI"
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
                          text: "  " ,
                          font: "bold 12px Segoe UI", color: "black"
                      },
                      labels: { font: " 12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                  },
                seriesColors: ["#EB6357", "#CCCC66", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
                series: [{
                    
                    field: "Amount",
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
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Receivables for a month</h4>
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
                                       </div></div> 

                  

                  <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van/FSR"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                 </div>

          <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Month</label>
              <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromDate" Width ="100%" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>         
               </div></div>
     
           
</div>
                                                </div>

       <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"   ID="SearchBtn" runat="server" Text="Search"  />
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>


                           <asp:HiddenField ID="hfOrgID" runat ="server" />
                              <asp:HiddenField ID="hYr" runat ="server" />
                              <asp:HiddenField ID="hMonth" runat ="server" /> 
           
                              <div id="summary" runat="server" class="row"></div>
         <p><br /><br /></p>
       <div class="overflowx" >
              <div class="overflowx">   
                <div class="chart-wrapper padding0"  id="Chartwrapper" runat="server" style="height:400px" >
   <div id="chart5" style="width:100%"></div>
 
    </div>         
                  </div>   
           </div>                                  
                                           
         <p><br /><br /></p>
           <div class="row">
        <div class="col-sm-6 col-md-6">
               
                <div class="table-responsive">
                  <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
                        <asp:HiddenField ID="UId" runat="server" />
                         <asp:HiddenField ID="hfVan" runat="server" />
        <asp:HiddenField ID="HCurrency" runat="server" />
                      <div id="dvCurrency" runat="server" visible="false" style="width:100%"  >
          <h5 class='text-right' >Currency <span class='text-blue'><strong><asp:label runat="server" ID="lblCurrency"></asp:label></strong></span></h5></div>
                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                          
                                                                <telerik:GridTemplateColumn uniqueName="Van"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Van" SortExpression ="Van"
                                                                HeaderText="Sales Rep" >
                                                                     <ItemStyle Width="50%" />
                                                            <ItemTemplate>
                                                                
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Van")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"");", Eval("CreatedBy"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>                                              
                                                              
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Receivable" HeaderText="Receivable" 
                                                                  SortExpression ="Receivable" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" Width="50%" />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                      </telerik:RadAjaxPanel>
                    </div>
           
        </div>
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
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress> 

	

<%--	   <div style="overflow:scroll; height:100%; border:groove" id="RepSec" runat="server" visible="false">
  <rsweb:ReportViewer ID="RVMain" runat="server"  CssClass="abc"   ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>    
	 </div> --%>
</asp:Content>
