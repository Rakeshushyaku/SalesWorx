<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepProductListing.aspx.vb" Inherits="SalesWorx_BO.RepProductListing" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
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
    <h4>Product Listing</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	 
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
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
             <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"    Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
            </div>
                                            </div>
                                                    
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Type</label>
           
                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlProductType" Width ="100%" runat="server" >
                    <Items>
                    <telerik:RadComboBoxItem Value="0" Text="All"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="1" Text="MSL"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="2" Text="Non-MSL"></telerik:RadComboBoxItem>
                        </Items>
                </telerik:RadComboBox>
                                                 </div>
                                            </div>
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                Item Code</label>
             <asp:TextBox ID="txtItemCode" CssClass="inputSM"  Width ="100%" runat="server"></asp:TextBox>
             </div>
                                                        </div>
                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Description  </label>
             <asp:TextBox ID="txtDescription" CssClass="inputSM"  Width ="100%" runat="server"></asp:TextBox> 
                     </div>
                    </div> 
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Agency</label>
             <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_Agency"    Width ="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="Select Agency"
                    runat="server" >
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
                <p><strong>Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
                <p><strong>Item code: </strong> <asp:Label ID="lbl_SkU" runat="server" Text=""></asp:Label></p>
                <p><strong>Description: </strong> <asp:Label ID="lbl_Desc" runat="server" Text=""></asp:Label></p>
                <p><strong>Agency: </strong> <asp:Label ID="lbl_agency" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
         
             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
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
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Brand_Code" HeaderText="Brand Code"
                                                                  SortExpression ="Brand_Code" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Primary_UOM_Code" HeaderText="Primary UOM"
                                                                  SortExpression ="Primary_UOM_Code"  >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                                                                  SortExpression ="Agency" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="IsMSL" HeaderText="Is MSL"
                                                                  SortExpression ="IsMSL" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Size" HeaderText="Item Size"
                                                                  SortExpression ="Item_Size" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="IsCashItem" HeaderText="Is Cash Item"
                                                                  SortExpression ="IsCashItem" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>


            
        </ContentTemplate>
 
  </asp:UpdatePanel> 
        <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
   </div>
</asp:Content>