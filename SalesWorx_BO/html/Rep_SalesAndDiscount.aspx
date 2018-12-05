<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_SalesAndDiscount.aspx.vb" Inherits="SalesWorx_BO.Rep_SalesAndDiscount" %>
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

.widgetblkinsmall small span
{
    display: block;
    text-align: right;
}

    
    </style>
    <script>
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

        function OpenViewWindowOrder(cid, Refno) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
           // URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=O'
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        function OpenViewWindowReturn(cid, Refno) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            //URL = 'RepDetails.aspx?Type=Return&ReportName=ReturnDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            URL = 'Rep_ReturnDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=R'
          
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
       <h4>Sales and Discount Report</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	

 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
   <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
            <div class="row">
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                 <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van</label><telerik:RadComboBox Skin="Simple"  Filter="Contains"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddlVan" Width ="100%" 
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" EmptyMessage="Select Van">
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
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

              </div>
                                          </div>
                            
             <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>     
                </div>
                                                </div>
       <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="SearchBtn"  CssClass ="btn btn-primary btn-block"  runat="server" Text="Search" />
                                                    <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                     
                                                    </div>
                                                <div class="form-group text-center fontbig">
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
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p> 
                  <p><strong>Customer: </strong> <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label></p> 
            </span>
            </i>      
        </div>
    </div>   

            <div id="rep" runat="server" visible="false">
                <div id="summary" runat="server" class="row"></div>
                <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
           
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                          
                                                          
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales" HeaderText="Gross Sales"
                                                                  SortExpression ="Sales" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Discount" HeaderText="Discount"
                                                                  SortExpression ="Discount" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Net" HeaderText="Net Sales"
                                                                  SortExpression ="Net" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Returns" HeaderText="Returns"
                                                                  SortExpression ="Returns" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>
                                                               
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="RetPer" HeaderText="Returns %"
                                                                  SortExpression ="RetPer" DataType="System.Double" DataFormatString="{0:f2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                           </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                     
                

                                          
                     </div>       
            <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>

                                   </div>
              <asp:HiddenField runat="server" id="Hcurrency"></asp:HiddenField>
              

                           </ContentTemplate>
        
        </asp:UpdatePanel> 
                           
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
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       

	 
</asp:Content>
