﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_TransactionStatus.aspx.vb" Inherits="SalesWorx_BO.Rep_TransactionStatus" %>
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
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
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

        function OpenViewWindow(cid,Type) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            if(Type=='Invoice')
                
                URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=O';
              // URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            else if (Type == 'Return')
                //URL = 'RepDetails.aspx?Type=Return&ReportName=ReturnDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
                URL = 'Rep_ReturnDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=R';
            else if (Type == 'Pre Sales Order')
                URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=SO';
               // URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            else if (Type == 'Sales return')
                URL = 'Rep_SRDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=SR';
              //  URL = 'RepDetails.aspx?Type=SR&ReportName=SRDetailed&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            else
                      URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
                //URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
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
       <h4>Transaction Status Report</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	
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
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%"   runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label><telerik:RadComboBox Skin="Simple"  Filter="Contains"  EmptyMessage="Select Van/FSR"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddVan" Width ="100%" 
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                 </telerik:RadComboBox>
                      
                    </div>
                                          </div>

               
           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer</label><telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                      
                    </div>
                                          </div>

                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Status</label><telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Status" Filter="Contains"   CheckBoxes="true" EnableCheckAllItemsCheckBox="true" ID="ddl_Status" Width ="100%" 
                    runat="server" >
                 </telerik:RadComboBox>
                      
                    </div>
                                          </div>

          <div class="col-sm-4">
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
              <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>To Date</label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>  
              <p><strong>Status: </strong><asp:Label ID="lbl_status" runat="server" Text=""></asp:Label></p>  
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
               
            </span>
            </i>      
        </div>
    </div>
                              <br />

                              <div id="summary" runat="server" class="row"></div> 
                              <div class="table-responsive">
                                   <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                       <Columns>
                                                          <telerik:GridTemplateColumn uniqueName="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression ="Orig_Sys_Document_Ref"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"");", Eval("Row_ID"), Eval("DocType"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="DocType" HeaderText="Document Type" SortExpression ="DocType"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>                                       
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Start_time" HeaderText="Creation Date" SortExpression ="Start_time"
                                                              DataFormatString="{0:dd-MMM-yyyy}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                          
 
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Order_Amt" HeaderText="Amount"
                                                                  SortExpression ="Order_Amt" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Order_Status" HeaderText="Status"
                                                                  SortExpression ="Order_Status" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="POSTEDTOERP" HeaderText="Posted to ERP At<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="POSTEDTOERP" DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" HeaderTooltip="If posted to ERP, date and time of posting" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                                  </div>
                              <asp:HiddenField ID="HCurrency" runat="server" />          
 <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
                                   <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>     
 </ContentTemplate> </asp:UpdatePanel> 
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
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
