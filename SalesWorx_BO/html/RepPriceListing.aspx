<%@ Page Title="Price List" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepPriceListing.aspx.vb" Inherits="SalesWorx_BO.RepPriceListing" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

 div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   } 
    </style>
     <script>


         var prm = Sys.WebForms.PageRequestManager.getInstance();

         prm.add_initializeRequest(InitializeRequest);
         prm.add_endRequest(EndRequest);
         var postBackElement;

         function InitializeRequest(sender, args) {

             $("#MainContent_SearchBtn").attr("disabled", "disabled");
           }


       
         function EndRequest(sender, args) {
             $("#MainContent_SearchBtn").removeAttr("disabled");
         }


         function alertCallBackFn(arg) {

         }

         function clickSearch() {
             $("#MainContent_SearchBtn").click()
             return false;
         }

         function clickExportExcel() {
             $("#MainContent_BtnExportExcel").click()
             return false

         }
         function clickExportPDF() {
             $("#MainContent_BtnExportPDF").click()
             return false
         }
</script>
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Price Listing</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	  <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
          <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
    </telerik:RadAjaxLoadingPanel>

 <telerik:RadAjaxPanel ID="l" runat="server" LoadingPanelID ="RadAjaxLoadingPanel1">
                <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"   >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
<div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Organization<em><span>&nbsp;</span>*</em></label>
              <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"  Width="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </telerik:RadComboBox>
               </div>
                                            </div>
              <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Agency</label>
           
                 <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlAgency" Width="100%" 
                    runat="server" AutoPostBack="true"  >
                 </telerik:RadComboBox>
            </div>
                                            </div>
              <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>UOM</label>
             
                <telerik:RadComboBox Skin="Simple" Filter="Contains"  CssClass="inputSM" Width="100%"
                    runat="server"  ID="ddlUOM" >
               </telerik:RadComboBox>
                               </div>
                                            </div>
              <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Price List Type </label>
            
                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlType" Width="100%" 
                    runat="server" >
                </telerik:RadComboBox>
             </div>
                                            </div>
              <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Product </label>
                
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
      </div>
      </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
               <div id="RepDiv" runat="server" visible="false">
  <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
                <p><strong>Agency: </strong> <asp:Label ID="lbl_agency" runat="server" Text=""></asp:Label></p>
                <p><strong>UOM: </strong> <asp:Label ID="lbl_UOM" runat="server" Text=""></asp:Label></p>
                <p><strong>Price List: </strong> <asp:Label ID="lbl_PriceList" runat="server" Text=""></asp:Label></p>
                <p><strong>Product: </strong> <asp:Label ID="lbl_Product" runat="server" Text=""></asp:Label></p>
                
            </span>
            </i>      
        </div>
    </div>
            <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
                   <div class="overflowx">
             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray" Width="100%"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="True"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description"  >
                                                                <ItemStyle Wrap="True"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Cost_Price" HeaderText="Purchase Price"
                                                                  SortExpression ="Cost_Price" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Price_List_Type" HeaderText="Price List"
                                                                  SortExpression ="Price_List_Type"  >
                                                                <ItemStyle Wrap="True"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_UOM" HeaderText="Item UOM"
                                                                  SortExpression ="Item_UOM" >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="IsCashItem" HeaderText="Is Cash Item"
                                                                  SortExpression ="IsCashItem"  >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"   />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Unit_Selling_Price" HeaderText="Unit Selling Price"
                                                                  SortExpression ="Unit_Selling_Price" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                       </div>
                    <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
            </div>

        </telerik:RadAjaxPanel>
        <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
       
   </div>
</asp:Content>