<%@ Page Title="Asset Review Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepAssets.aspx.vb" Inherits="SalesWorx_BO.RepAssets" %>
  

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

         function OpenWindow( Count, RowID) {

            
                 var combo = $find('<%=ddlOrganization.ClientID%>');

                 var URL

                 URL = 'Rep_ViewAssetHistory.aspx?OrgID=' + combo.get_selectedItem().get_value() + '&Type=Asset&ReportName=AssetHistory&ID=' + RowID;

                 var oWnd = radopen(URL, null);
                 oWnd.SetSize(1075, 600);
                 oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                 oWnd.SetModal(true);
                 oWnd.Center;
                 oWnd.set_visibleStatusbar(false)
             
            return false

        }
</script>
    </asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
 <h4>Assets Review Report</h4>
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
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Organization<em><span>&nbsp;</span>*</em> </label>
           
             <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"  Width="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
               </telerik:RadComboBox>
              </div>
                                                     </div>

                <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Customer </label>
               
                 <telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
               </div>
                    </div>
          
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>
                  Asset Type </label>
               
                     <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlAssetType"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="AssetTypeID"  
                    >
                                    </telerik:RadComboBox>                                   
                                    
                
                 </div>
              </div>

               <div class="col-sm-4">
                                                     <label class="hidden-xs"><br /></label>
                                            <div class="form-group form-inline-blk">
                  <asp:CheckBox ID="chkActive" runat ="server" Text ="Show Active Assets Only"  />
                  
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
                <p><strong>Customer: </strong> <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>
                <p><strong>Asset Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
                <p><strong><asp:Label ID="lbl_Active" runat="server" Text=""></asp:Label></strong></p>
               
            </span>
            </i>      
        </div>
    </div>
         <asp:HiddenField id="HfDecimal" runat="server"></asp:HiddenField>
            <asp:HiddenField id="hfCurrency" runat="server"></asp:HiddenField>
             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" ShowFooter ="true"  >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="CustomerName"  FooterText ="Total : " >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="AssetType" HeaderText="Asset Type"
                                                                  SortExpression ="AssetType" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Asset_Code" HeaderText="Asset Code"
                                                                  SortExpression ="Asset_Code" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Is_Active" HeaderText="Is Active"
                                                                  SortExpression ="Is_Active" >
                                                                <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ChangeType" HeaderText="Change Type"
                                                                  SortExpression ="ChangeType" >
                                                                <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Condition" HeaderText="Condition"
                                                                  SortExpression ="Condition"  >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Presence" HeaderText="Presence"
                                                                  SortExpression ="Presence" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ActCount" HeaderText="Count"
                                                                  SortExpression ="ActCount" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="AssetValue" HeaderText="Value"
                                                                  SortExpression ="AssetValue"  Aggregate ="Sum"  >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                  <FooterStyle Wrap="False" HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridTemplateColumn  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" HeaderStyle-ForeColor="#0090d9"  
                                                                HeaderText="History" >
                                                            <ItemTemplate>
                                                              <%--  <asp:LinkButton ID="Lnk_RefID" runat="server"   Visible ='<%# Bind("LinkVisible")%>'   Text='View History' ForeColor="SteelBlue" Font-Underline="true"    OnClientClick='<%# String.Format("OpenWindow(""{0}"",""{1}"");", Eval("HistoryCount"), Eval("Asset_ID"))%>'    ></asp:LinkButton>--%>
                                                         
                                                                <asp:HiddenField runat="server" ID="HCount" Value='<%# Bind("HistoryCount")%>' />
                                                                <asp:HiddenField runat="server" ID="HAsset_ID" Value='<%# Bind("Asset_ID")%>' />
                                                                  <asp:LinkButton ID="Lnk_RefID" runat="server"   Visible ='<%# Bind("LinkVisible")%>'   Text='View History' ForeColor="SteelBlue" Font-Underline="true"    OnClick="Lnk_RefID_Click"    ></asp:LinkButton>

                                                                 </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
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