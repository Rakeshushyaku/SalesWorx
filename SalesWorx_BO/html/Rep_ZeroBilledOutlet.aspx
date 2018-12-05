<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ZeroBilledOutlet.aspx.vb" Inherits="SalesWorx_BO.Rep_ZeroBilledOutlet" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


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
div[id*="ReportDiv"] {  
    overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

#MainContent_gvRep{
    margin:15px 0;
}
</style>
    <script>
        function onDataBound(e) {
            $("#MainContent_Panel1").hide()

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

         

        function OpenNotVisitedWindow(cid, name) {
            var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;

            var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
            var param3 = document.getElementById('<%= Hto.ClientID%>').value;

            var param4 = document.getElementById('<%= UId.ClientID%>').value;

            var URL
            URL = 'Rep_ZeroBilledOutlet_Detailed.aspx?OrgID=' + param1 + '&FromDt=' + param2 + '&ToDt=' + param3 + '&SID=' + cid + "&SPName=" + name;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(930, 630);
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

        function RefreshChart() {
            createChart3();
        }

        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
        }
    </script>
    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock19">
        
<script src="../scripts/kendo.all.min.js"></script>
<script src="../scripts/kendo.dataviz.min.js"></script>
<link href="../styles/kendo.dataviz.black.min.css" rel="stylesheet" />

  <script type="text/javascript">

      function createChart3() {
          var res
          var param1 = document.getElementById('<%= hfOrg.ClientID%>').value;

          var param2 = document.getElementById('<%= HFrom.ClientID%>').value;
          var param3 = document.getElementById('<%= Hto.ClientID%>').value;

          var param5 = document.getElementById('<%= hfVan.ClientID%>').value;

          var param4 = document.getElementById('<%= UId.ClientID%>').value;

          $("#MainContent_Panel1").show()

          var dataSource = {
              type: "json",
              transport: {
                  read: {
                      url: "Chart.asmx/ZerobilledOutlets", //specify the URL which should create new records. This is the Create method of the Products.asmx service.
                      contentType: "application/json; charset=utf-8", // tells the web service to serialize JSON
                      type: "POST", //use HTTP POST request as the default GET is not allowed for ASMX
                      dataType: "json"
                  }
                    ,
                  parameterMap: function (data, operation) {

                      var output = null;
                      switch (operation) {

                          case "read":
                              output = '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '"}';
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
              dataBound: onDataBound,
              title: {
                  text: "Zero billed customer % ",
                  color: "black",
                  font: "bold 14px Segoe UI"

              },


              seriesDefaults: {
                  type: "column",
                  gap: 1.5,
                  labels: {
                      visible: false,
                      format: "{0:N0}",

                      font: "bold 12px Segoe UI"
                  }

              },
              chartArea: {
                  background: "White"
              },
              categoryAxis:
                {
                    labels: { font: "12px Segoe UI", color: "black", rotation: -90 },

                    minorGridLines: { visible: false },
                    majorGridLines: {
                        visible: false
                    }
                },


              valueAxis:
                {
                    max:100,
                    title: {
                        text: "Zero billed customer % " ,
                        font: "bold 12px Segoe UI", color: "black"
                    },
                    labels: { font: "12px Segoe UI", color: "black" }, majorGridLines: { visible: false }
                },
              seriesColors: ["#0090d9", "#33CCFF", "#FF99CC", "#FF9933", "#FFFF99", "#FFCC99", "#9999FF"],
              series: [{
                  field: "Amount",
                  categoryField: "Description",


              }],
              tooltip: {
                  visible: true,
                  format: "{0:N2}",
                  font: "12px Segoe UI",
                  template: "Van: <b>${category}</b></br>Zero billed customer % :  #=kendo.format('{0:N2}',dataItem.Amount)#",
                  color: "white",
                  background: "black"
              },


          });


         



      }

      
     



    </script> 
        </telerik:RadScriptBlock>
    <style type="text/css">  
        .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            font-size: 13px;
            font-weight: bold;
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #eff9ff !Important;
            font-weight: bold !Important;
        }

    .k-chart svg{
	margin:0 -14px;
}
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Zero Billed Customers</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
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

     <telerik:RadAjaxPanel ID="l" runat="server">
          <asp:HiddenField runat="server" ID="hfOrg" />
         <asp:HiddenField runat="server" ID="hfVan" />
         <asp:HiddenField runat="server" ID="UId" />
         <asp:HiddenField runat="server" ID="HFrom" />
         <asp:HiddenField runat="server" ID="Hto" />
        
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
                                                  <telerik:RadComboBox Skin="Simple" ID="ddVan" EmptyMessage="Select Van/FSR"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                          
                                        </div>
                                        <div class="row">
                                        <div class="col-sm-6">
                                              <div class="row">
                                                  <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                 </div>
                                                 <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>To Date</label>
                                                          <telerik:RadDatePicker ID="txtToDate"   Width="100%" runat="server">
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
                             <div id="repdiv" runat="server" >
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
        
                              <div id="summary" runat="server" class="row"></div>
         <p><br /></p>
         <div class="overflowx" >           
                      <div class="chart-wrapper padding0" style="height:400px" id="Chartwrapper" runat="server" >
                        <div id="chart3" style="height:100%">
                        </div>
                       </div>
</div>
         <p><br /><br /></p>
                              <div class="table-responsive">
                                   <div class="row">
                                       <div class="col-lg-9">
                                 <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server"  >  
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             

                                                            <telerik:GridTemplateColumn uniqueName="SalesRep_Name"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="SalesRep_Name" SortExpression ="SalesRep_Name"
                                                                HeaderText="Van/FSR" >
                                                            <ItemTemplate>
                                                                 
                                                                <asp:LinkButton ID="Lnk_NotVisited" runat="server" Text='<%# Bind("SalesRep_Name")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenNotVisitedWindow(""{0}"",""{1}"");", Eval("SalesRep_ID"), Eval("SalesRep_Name"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                          <ItemStyle HorizontalAlign="Right" />
                                                          <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalCustomer" HeaderText="Customers Assigned<i class='fa fa-info-circle'></i>" SortExpression ="TotalCustomer"
                                                               HeaderTooltip="Unique customers assigned to the van/FSR"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="PlannedCust" HeaderText="Planned Customers<i class='fa fa-info-circle'></i>" SortExpression ="PlannedCust"
                                                               HeaderTooltip="Unique customers in route plan"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NotVisited" HeaderText="Not Visited Customers<i class='fa fa-info-circle'></i>" SortExpression ="NotVisited"
                                                               HeaderTooltip="Customers in route plan to which visits were not made"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                       
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NotBilled" HeaderText="Zero Billed Customers<i class='fa fa-info-circle'></i>" SortExpression ="NotBilled"
                                                                HeaderTooltip="Customers in route plan who have not billed" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                            
                                                            
                                                          
                                                               
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel> 
                                 </div>
                                  </div>
                                          
  
                           </div>
             </div>
                                
                              
                        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>    

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
         <asp:Panel ID="Panel1" CssClass="overlay" runat="server" style="display:none">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
     </asp:Panel>
   
</asp:Content>
