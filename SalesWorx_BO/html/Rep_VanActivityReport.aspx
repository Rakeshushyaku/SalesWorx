<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanActivityReport.aspx.vb" Inherits="SalesWorx_BO.Rep_VanActivityReport" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

     
    </style>
    <script>
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
        }
        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }

        function OpenLocWindow(Lat, Long, CustLat, CustLong) {

            var URL
            URL = 'ShowMap.aspx?Lat=' + Lat + '&Long=' + Long + '&Type=Visits&CustLat=' + CustLat + '&CustLong=' + CustLong;

            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)
            oWnd.moveTo(175, 10);
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

       

      
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      

    input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }

</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Vehicle Activity Report</h4>
	 <telerik:radwindowmanager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:radwindowmanager>
	
	
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
                              <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
            <div class="row">
                 <div class="col-sm-3" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                <div class="col-sm-4">
                                                          <div class="form-group">
                                                            <label>Van/FSR</label>
                                                           <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
                                                         </div>
                                                     </div>
                          <div class="col-sm-4">

                                            <div class="form-group">
                                                <label>Sync Type</label>
                                                <telerik:RadComboBox ID="ddlSyncType"  Skin="Simple" Width ="100%" runat="server" CssClass="inputSM">
                    <Items>
                    
                    </Items>
                </telerik:RadComboBox>
                                                 
              </div>
                                          </div>          
               
                                                 
                                                    
           
                     <div class="col-sm-3">

                                            <div class="form-group">
                                                <label>From Date</label>
                                                <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
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
                                                             <telerik:RadDatePicker ID="txtToDate"   Width ="100%" runat="server">
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
       <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="SearchBtn" CssClass ="btn btn-sm btn-block btn-primary"  runat="server" Text="Search" />
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
              <p><strong>Van: </strong><asp:Label ID="lbl_Van" runat="server" Text=""></asp:Label></p>
              <p><strong>Sync Type: </strong><asp:Label ID="lbl_SyncType" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
            </span>
            </i>      
        </div>
    </div>   
                      <br />
                               
                                 <div id="summary" runat="server" class="row"></div> 
                              <div class="table-responsive overflowx" >
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11" CommandItemDisplay="Top"  >
                    <CommandItemTemplate>
                        <div style="text-align:left;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                       <Columns>
                                                          
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR" SortExpression ="SalesRep_Name"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>  
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_No" HeaderText="Employee No" SortExpression ="Emp_Name"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>  
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Employee Name" SortExpression ="Emp_No"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>   
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="MobNo" HeaderText="Mobile" SortExpression ="MobNo"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>                                           
                                                         
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sync_Time" HeaderText="Last Sync </br> Time<i class='fa fa-info-circle'></i>"
                                                              DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  EmptyDataText="N/A"    SortExpression ="Sync_Time" HeaderTooltip="Time of last sync performed" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastVisitedCustomerNo" HeaderText="Last </br>Visited </br> Customer No" SortExpression ="LastVisitedCustomerNo"
                                                               EmptyDataText="N/A" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                           <telerik:GridTemplateColumn uniqueName="LastVisitedCustomer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LastVisitedCustomer" SortExpression ="LastVisitedCustomer"
                                                                        HeaderText="Last </br>Visited </br> Customer" >
                                                                    <ItemTemplate  >
                                                                         
                                                                        <asp:LinkButton ID="Lnk_RefID" runat="server"  Text='<%# Bind("LastVisitedCustomer")%>' ForeColor="SteelBlue" Font-Underline="true"  OnClientClick='<%# String.Format("OpenLocWindow(""{0}"",""{1}"",""{2}"",""{3}"");", Eval("lat"), Eval("long"), Eval("Cust_lat"), Eval("Cust_long"))%>'   Width="100%"  ></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                <ItemStyle Wrap="False" />
                                                                </telerik:GridTemplateColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastVisitedAt" HeaderText="Last </br>Visited On"
                                                                DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" EmptyDataText="N/A"   SortExpression ="LastVisitedAt" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Last_Inv_created_at" HeaderText="Last Invoice </br> Created at"
                                                               DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" EmptyDataText="N/A"    SortExpression ="Last_Inv_created_at" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastInvNo" HeaderText="Last </br> Created Inv No." SortExpression ="LastInvNo"
                                                               EmptyDataText="N/A" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastCollectedAt" HeaderText="Last </br> Collection </br> Created at"
                                                               DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  EmptyDataText="N/A"   SortExpression ="LastCollectedAt" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastCollection" HeaderText="Last </br> Created Collection</br> No." SortExpression ="LastCollection"
                                                               EmptyDataText="N/A" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           </div>

                              
                                   <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>     
 </ContentTemplate> </asp:UpdatePanel> 
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
      <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
   </div>
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       

	
</asp:Content>
