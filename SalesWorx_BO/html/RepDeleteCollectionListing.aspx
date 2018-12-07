<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepDeleteCollectionListing.aspx.vb" Inherits="SalesWorx_BO.RepDeleteCollectionListing" %>



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


        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;

        function InitializeRequest(sender, args) {
            $(".rpTemplate .form-group").addClass("form-disabled");
        }

        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

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

        function OpenViewWindow(cid) {
            var URL
            //   URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid 
            var oWnd = radopen(URL, null);
            oWnd.SetSize(950, 600);
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

         
         
    </script>
    <telerik:RadScriptBlock runat="server" ID="RadScriptBlock19">
     <script>
         var internetUsers1 = [];
         var chartseries = [];


         function createlineChart() {

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
                 title: { text: "", align: "left", font: "18px Arial, Verdana, sans-serif", color: "#336699" },
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
                         text: "PDC Receivables ",
                         font: "bold 14px Segoe UI",
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
                       type: "POST", contentType: "application/json;charset=utf-8", url: "Chart.asmx/PDCReceivablesbyMonth", data: '{param1:"' + param1 + '",param2:"' + param2 + '",param3:"' + param3 + '", param4:"' + param4 + '", param5:"' + param5 + '", param6:"' + param6 + '"}', dataType: "json", success: function (response) {

                           internetUsers1 = response.d
                           createlineChart();

                       }
                             , error: function (jqXHR, textStatus, errorThrown) { alert(errorThrown) }

                   });
               }
         }
         function ConfirmDelete(msg, event) {

             var ev = event ? event : window.event;
             var callerObj = ev.srcElement ? ev.srcElement : ev.target;
             var callbackFunctionConfirmDelete = function (arg, ev) {
                 if (arg) {
                     callerObj["onclick"] = "";
                     if (callerObj.click) callerObj.click();
                     else if (callerObj.tagName == "A") {
                         try {
                             eval(callerObj.href)
                         }
                         catch (e) { }
                     }
                 }
             }
             radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
             return false;
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

    
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Collection Deletion Request</h4>
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
         <asp:HiddenField runat="server" ID="Hcount" />
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

                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                            
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"   AutoPostBack="true"   >
                                        </telerik:RadComboBox>
                                             
                              
                                            </div>
                                          </div>
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddVan" EmptyMessage="Select Van/FSR"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Collection Ref. No</label>
                                                <asp:TextBox ID="txtCollectionRefNo" Width ="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                            </div>
                                          </div>
                                        
                                         </div>
                                                <div class="row">
                                                 <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"   runat="server">
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
                                                          <telerik:RadDatePicker ID="txtToDate"   runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                  </div>
                                             
                                           
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Discount </label>
                                                  <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Discount" Width ="100%" runat="server" >
                                                      <Items>
                                                          <telerik:RadComboBoxItem Text="All" Selected="true" Value="0" />
                                                          <telerik:RadComboBoxItem Text="With Discount"   Value="1" />
                                                      </Items>
                                        </telerik:RadComboBox>
                                                </div>
                                            </div>
                                              <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Document Type </label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_heldpdc" Width ="100%" runat="server" >
                                                      <Items>
                                                          <telerik:RadComboBoxItem Text="All" Selected="true" Value="0" />
                                                          <telerik:RadComboBoxItem Text="Held PDC Only"   Value="1" />
                                                      </Items>
                                        </telerik:RadComboBox>
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
                                                <asp:HyperLink href="" CssClass=""  Visible="false" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""   Visible="false"  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                        </div>
                                       
                                    
                        
                                        
                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
                           <div id="repdiv">
                               <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>Collection Ref No.: </strong><asp:Label ID="lbl_RefNo" runat="server" Text=""></asp:Label></p>  
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><asp:Label ID="lbl_Discount" runat="server" Text=""></asp:Label></p> 
              <p><asp:Label ID="lbl_Held" runat="server" Text=""></asp:Label></p>    
            </span>
            </i>      
        </div>
    </div>

                              <div id="summary" runat="server" class="row"></div>
         <p><br /></p>
                              <div class="table-responsive">
                                  <telerik:RadTabStrip ID="Collectiontab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Collection Listing" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="PDC Receivables" runat="server">
                        </telerik:RadTab>
                        

                    </Tabs>
                </telerik:RadTabStrip>
                                   <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                                        <telerik:RadPageView ID="RadPageView1" runat="server">
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                            <asp:Label ID="lblSurveyDelID" runat="server" Text='<%# Bind("Collection_Ref_No")%>' Visible="false"></asp:Label>
                              <asp:Label ID="lblStatCode" runat="server" Text='<%# Bind("Collection_Ref_No")%>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="btnDelete" ToolTip="Delete Collection" runat="server" CausesValidation="false"
                                CommandName="DeleteSelected"
                                ImageUrl="~/Images/delete-13.png"
                                OnClientClick="return ConfirmDelete('Are you sure to delete this survey?',event);" />

                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>

                                                             <%--<telerik:GridTemplateColumn uniqueName="Collection_Ref_No"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Collection_Ref_No" SortExpression ="Collection_Ref_No"
                                                                HeaderText="Delete" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HReport_ID" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "Collection_ID") %>'/>
                                                                                                                                                                                              
                                                             
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete"  
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete?');" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                                                                                      
                                                            

                                                         <telerik:GridTemplateColumn uniqueName="Collection_Ref_No"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Collection_Ref_No" SortExpression ="Collection_Ref_No"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Collection_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Collection_Ref_No")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenViewWindow(""{0}"");" , Eval("Collection_ID") )%>'    ></asp:LinkButton>
                                                              <%--  <asp:Image ImageUrl="~/images/notes.png" Visible='<%# Bind("Chequeimageshow")%>' runat="server" ID="imgcheque" />--%>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collected_On" HeaderText="Collected On" SortExpression ="Collected_On"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collected_By" HeaderText="Collected By"
                                                                  SortExpression ="Collected_By" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                          

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collection_Type" HeaderText="Pay.Type"
                                                                  SortExpression ="Collection_Type" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Amount" HeaderText="Amount"
                                                                  SortExpression ="Amount" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Discount" HeaderText="Discount"
                                                                  SortExpression ="Discount" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status"
                                                                  SortExpression ="Status" >
                                                                <ItemStyle Wrap="True"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                            </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView2" runat="server">
                                            
                    <div class="row">
                                     <div class="col-sm-7"><h5>PDC Receivables of last one year </h5></div>
                                    <div class="col-sm-5">
                                        <div class="form-group" style="margin-top: 7px;">
                                            <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Type"  Width ="100%" runat="server" AutoPostBack="True" Visible="false"  >
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="10" Text="Top 10 Vans" Selected="true"  />
                                                    <telerik:RadComboBoxItem Value="0" Text="All Vans" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                        <div class="overflowx">
                       <div class="chart-wrapper" id="Chartid" runat="server" style="padding:25px 0 0;">
                        <div id="chart3"  style ="width:100%;height:365px; margin:0 -12px;">
                        </div>
                       </div>
                            </div>

<hr />
                                            <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
 <div class="overflowx">
                         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server"  >
                              <telerik:RadGrid id="gvPDC" AllowSorting="True" AutoGenerateColumns="true" Skin="Simple" BorderColor="LightGray"
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
                    </telerik:RadPageView>
                                       </telerik:RadMultiPage>
                                  
                           </div>
         </div>
                               <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
      
                         </telerik:RadAjaxPanel>
                           
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
       <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
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
