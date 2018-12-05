<%@ Page Title="SKU wise Returns Periodwise" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepSKUwiseSalesReturn.aspx.vb" Inherits="SalesWorx_BO.RepSKUwiseSalesReturn" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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
     <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
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

        function OpenViewWindow(cid) {
           
            var URL
                     

            URL = 'RepDetails.aspx?Type=SKUReturns&ReportName=SKUwiseReturnsDetails&InID=' + cid + '&Org='+ document.getElementById('<%= ORGID.ClientID%>').value + '&From=' + document.getElementById('<%= HDate.ClientID%>').value + '&To=' + document.getElementById('<%= HToDate.ClientID%>').value
            
            var oWnd = radopen(URL, null);
            oWnd.SetSize(800, 580);
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
    </asp:Content><asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>SKU wise Returns Periodwise</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager> 
	 
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"  >
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
                                                <label>SKU</label>
                                                 <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                 <div class="col-sm-6">
                                                   <div class="row">
                                                       <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                        
   <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>SKU: </strong><asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_FromDate" runat="server" Text=""></asp:Label></p>
               <p><strong>To Date: </strong> <asp:Label ID="lbl_ToDate" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
            <div id="summary" runat="server" class="row"></div> 

              <div class="row">
             <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>

            <div id="divCurrency" runat="server" visible="false"  >
                 
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                <asp:HiddenField runat="server" ID="hfDigit" Value="N2" />
</div>
                  </div>

                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">
                    <Columns>

                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SKU" HeaderText="SKU (Item)"
                                                                  SortExpression ="SKU" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                       <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="StockUOM" HeaderText="UOM"
                                                                  SortExpression ="StockUOM" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                          
                         <telerik:GridTemplateColumn uniqueName="Returns"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Returns" SortExpression ="Returns"
                                                                HeaderText="Returns (Qty)" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Returns", "{0:##,#.00}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"");", Eval("Inventory_Item_ID"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                              <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>

                        

                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ReturnValue" HeaderText="Returns Value"
                                                                  SortExpression ="ReturnValue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
            <asp:HiddenField ID="HDate" runat="server" />
<asp:HiddenField ID="HToDate" runat="server" />    
<asp:HiddenField ID="ORGID" runat="server" />   

            <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

 </ContentTemplate>
 </asp:UpdatePanel> 
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
         
     
</asp:Content>

