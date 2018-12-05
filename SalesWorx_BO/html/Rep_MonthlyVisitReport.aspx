<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_MonthlyVisitReport.aspx.vb" Inherits="SalesWorx_BO.Rep_MonthlyVisitReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        /*.RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}
 
    .RadPanelBar_Simple .rpExpandable span.rpExpandHandle{
    background-position: 100% -5px !important;
}*/

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


         function OnClientItemExpand(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "310px"

             }
         }

         function OnClientItemCollapse(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "380px"
             }

         }

         function alertCallBackFn(arg) {

         }

         function clickSearch() {
             $("#MainContent_SearchBtn").click()
             return false;
         }
</script>
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Monthly Customer Visits  </h4>	
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px;width:100%" >

  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
  <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                          <div class="row">
                                            <div class="col-sm-10">
<div class="row">
            <div class="col-sm-4">
                 <div class="form-group">
                     <label> Organization </label>
             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
             </div>
                </div>
                 <div class="col-sm-4">
                 <div class="form-group">
                     <label>Van </label>
             <telerik:RadComboBox ID="ddl_Van" Skin="Simple"  runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AutoPostBack="true"
                CssClass="inputSM"     Width ="100%" >
                       </telerik:RadComboBox>&nbsp;
                     
                 </div>
                </div> 
           <div class="col-sm-4">
                 <div class="form-group">
                     <label>Month </label>
            
                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_year" runat="server" 
                     CssClass="inputSM" Width ="100%" >
                </telerik:RadComboBox>
                     
            </div>
                </div> 
    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                      <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn"  runat="server" Text="Search" />
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
     
              <div id="summary" runat="server" class="row"></div> 

            <div class="overflowx">
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
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep" SortExpression ="SalesRep_Name"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridTemplateColumn uniqueName="customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="customer" SortExpression ="customer"
                                                                HeaderText="Customer" >
                                                            <ItemTemplate>
                                                               
                                                                <asp:Label ID="Site_Use_ID" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                            
                                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visit_Start_Date" HeaderText="Visit Start<br/> Date"
                                                                  SortExpression ="Visit_Start_Date" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visit_End_Date" HeaderText="Visit End <br/> Date"
                                                                  SortExpression ="Visit_End_Date" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="InvCnt " HeaderText="No Of <br/>Orders Taken"
                                                                  SortExpression ="InvCnt" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrderAmt" HeaderText="Total  Order <br/>Value"
                                                                  SortExpression ="OrderAmt" DataFormatString="{0:G17}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="RMACnt" HeaderText="No Of <br/>Returns Taken"
                                                                  SortExpression ="RMACnt" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                             </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="RMA" HeaderText="Total  Return<br/>Value"
                                                                  SortExpression ="RMA"  DataFormatString="{0:G17}" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
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
   </div>

   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
	 
    </table>
	 
</asp:Content>
