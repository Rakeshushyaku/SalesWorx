<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanStockSummary.aspx.vb" Inherits="SalesWorx_BO.Rep_VanStockSummary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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

        function RefreshChart() {
            createChart3();
        }
    </script>
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

   <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Van/FSR Stock Summary</h4>
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
            <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
              
                 </div>
                                          </div>
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
            <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlVan" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  EmptyMessage="Select Van/FSR" 
                    runat="server" DataTextField="SalesRep_Name"  Width ="100%" DataValueField="SalesRep_ID">
                </telerik:RadComboBox>
 </div>
                                          </div>

                                                  <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Agency<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                EmptyMessage="Select Agency" ID="ddlAgency" Width="100%" runat="server" AutoPostBack="true">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                                    
                                                    </div>
                                                <div class="row">

                                                    
                                                     <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Brand<em><span>&nbsp;</span>*</em></label>
                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true" CheckBoxes="true"
                                                EmptyMessage="Select Brand" ID="ddlBrand" Width="100%" runat="server">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                                    <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Item</label>
                                                
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
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
                <p><strong>Agency: </strong> <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label></p>
                <p><strong>Brand: </strong> <asp:Label ID="lbl_brand" runat="server" Text=""></asp:Label></p>
                <p><strong>Product: </strong> <asp:Label ID="lbl_SkU" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
           <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
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
                                                                     
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR" SortExpression ="SalesRep_Name"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                                                                                             
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code" SortExpression ="Item_Code"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="true" />
                                                                   </telerik:GridBoundColumn>
                                                                   <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="StockUOM" HeaderText="UOM"
                                                                  SortExpression ="StockUOM" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Type" HeaderText="Type"
                                                                  SortExpression ="Type" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Lot_Qty" HeaderText="Quantity"
                                                                  SortExpression ="Lot_Qty" DataType="System.Double" DataFormatString="{0:#,###.0000}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                              
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
         </telerik:RadAjaxPanel>

       <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="Btn_LoadItem" runat="server" Text="Export" />
   </div>
	

 </asp:Content>