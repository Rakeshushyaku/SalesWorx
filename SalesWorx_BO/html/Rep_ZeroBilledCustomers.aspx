<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ZeroBilledCustomers.aspx.vb" Inherits="SalesWorx_BO.Rep_ZeroBilledCustomers" %>
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
       <h4>Detailed Zero Billed Customers</h4>
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
                                                <label>Van</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddVan" EmptyMessage="Select Van"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
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
        
                  </div>            



                              <div class="table-responsive">

                                <telerik:RadTabStrip ID="ZeroBilledSlab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Planned Customers" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Not Visited Customers" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Zero Billed Customers" runat="server">
                        </telerik:RadTab>
                        

                    </Tabs>
                </telerik:RadTabStrip>
               <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       
                   <telerik:RadPageView ID="RadPageView1" runat="server">
                        <p style="padding-top:15px;"><asp:Label ID="lblDesc1" Text="" runat="server" ></asp:Label></p>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                           <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent" PageSize="10" AllowPaging="True" runat="server" GridLines="None" >
                               <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent" PageSize="10">
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             
                                                                                                                      
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Van" HeaderText="VAN"
                                                                  SortExpression ="Van" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vdate" HeaderText="Plan Date" SortExpression ="vdate"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer No"
                                                                  SortExpression ="Customer_No" >
                                                               <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer Name"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VistedYES" HeaderText="Visited<i class='fa fa-info-circle'></i>" SortExpression ="VistedYES"
                                                               HeaderTooltip="Visited or Not"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>


                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BilledYES" HeaderText="Billed<i class='fa fa-info-circle'></i>" SortExpression ="BilledYES"
                                                                HeaderTooltip="Billed or Not" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                           

                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visited" HeaderText="Visits<i class='fa fa-info-circle'></i>" SortExpression ="Visited"
                                                               HeaderTooltip="No of Visits"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Billed" HeaderText="Productive Visits<i class='fa fa-info-circle'></i>" SortExpression ="Billed"
                                                               HeaderTooltip="No of Productive Visits"  >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetSales" HeaderText="Net Sales"
                                                                  SortExpression ="NetSales" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>                                       
                                                                                                                          
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </telerik:RadAjaxPanel>
                                   </telerik:RadPageView>

                                 <telerik:RadPageView ID="RadPageView2" runat="server">
                                         <p style="padding-top:15px;"><asp:Label ID="Label1" Text="" runat="server" ></asp:Label></p>
                                 <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server"  >
                                 <telerik:RadGrid id="gvRep_visited" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent" PageSize="10" AllowPaging="True" runat="server" GridLines="None" > 
                                         <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="true">
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent" PageSize="10">
                                       <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             
                                                            
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Van" HeaderText="VAN"
                                                                  SortExpression ="Van" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vdate" HeaderText="Plan Date" SortExpression ="vdate"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer No"
                                                                  SortExpression ="Customer_No" >
                                                               <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer Name"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>


                                                             
                                                       
                                                                                                                       
                                                        </Columns>
                                                    </MasterTableView>
                                                    </telerik:RadGrid>
                                                     </telerik:RadAjaxPanel>
                                                                                                        
                                             </telerik:RadPageView>


                   <telerik:RadPageView ID="RadPageView3" runat="server">
                                            
                              <p style="padding-top:15px;"><asp:Label ID="Label2" Text="" runat="server" ></asp:Label></p>
                                       
                      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server"  >
                           <telerik:RadGrid id="gvRep_Billed" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"  PageSize="10" AllowPaging="True" runat="server" GridLines="None" >
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true">
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent" PageSize="10">
                                     <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             

                                                            
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Van" HeaderText="VAN"
                                                                  SortExpression ="Van" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vdate" HeaderText="Plan Date" SortExpression ="vdate"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer No"
                                                                  SortExpression ="Customer_No" >
                                                               <ItemStyle Wrap="False"  HorizontalAlign="Center"/>
                                                                   <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer Name"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>


                                                              
                                                                                                                 
                                                               
                                                        </Columns>
                                                     </MasterTableView>
                                                    </telerik:RadGrid>
                                                     </telerik:RadAjaxPanel>


                                         

                                
                                             </telerik:RadPageView>
                                        
                      </telerik:RadMultiPage>
                                           
  
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
