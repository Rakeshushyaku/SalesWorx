<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_CollectionDiscount.aspx.vb" Inherits="SalesWorx_BO.Rep_CollectionDiscount" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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

     
    
    </style>
    <script>
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
            //URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
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
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Collection Discount</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	

  <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
        <ContentTemplate>
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
                                                  <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van/FSR" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" >
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                                     
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                <telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>
                                        
                                         
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>  
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>    
            </span>
            </i>      
        </div>
    </div>
             
                              <div id="summary" runat="server" class="row"></div>
            <p><br /></p>
            <div class="table-responsive">
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" ExportSettings-ExportOnlyData="true" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                                  
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                    
                   
                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Collection_Ref_No"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Collection_Ref_No" SortExpression ="Collection_Ref_No"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Collection_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Collection_Ref_No")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenViewWindow(""{0}"");" , Eval("Collection_ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                   <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Invoice_No" HeaderText="Invoice No"
                                                                  SortExpression ="Invoice_No" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                            
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collected_On" HeaderText="Collected On" SortExpression ="Collected_On"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                       
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesREp_Name" HeaderText="Collected By"
                                                                  SortExpression ="SalesREp_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Collection_Type" HeaderText="Payment <br/>Type"
                                                                  SortExpression ="Collection_Type" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Amount" HeaderText="Amount"
                                                                  SortExpression ="Amount" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Discount" HeaderText="Discount"
                                                                  SortExpression ="Discount" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Net" HeaderText="Net Amount"
                                                                  SortExpression ="Net" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status"
                                                                  SortExpression ="Status" >
                                                                <ItemStyle Wrap="True"  />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                </div>
                              <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                                    
                              <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>  
                       

                           </ContentTemplate>
           

 

                                     </asp:UpdatePanel> 

     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
           <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
   </div>

   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img  src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       


</asp:Content>