<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_OutofStock.aspx.vb" Inherits="SalesWorx_BO.Rep_OutofStock" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}
 
    .RadPanelBar_Simple .rpExpandable span.rpExpandHandle{
    background-position: 100% -5px !important;
}

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 


    </style>
     <script>


         function OnClientItemExpand(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "350px"

             }
         }

         function OnClientItemCollapse(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "450px"
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
    <h4>Out of Stock</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px;width:100%" > 
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems" onclientitemexpand="OnClientItemExpand"  onclientitemcollapse="OnClientItemCollapse"   >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
<table  border="0" cellspacing="2" cellpadding="2" width="100%">
          <tr>
            
                 <td  class="txtSMBold">Organization :</td>
            <td><telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"   Width ="250px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                                        AutoPostBack="True">
                </telerik:RadComboBox>
              </td>
              <td class="txtSMBold">Outlet :</td>
             <td><telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOutlet" 
                    runat="server"  Width ="250px" 
                     DataTextField="Outlet" DataValueField="CustomerID">
                </telerik:RadComboBox>&nbsp;
                 <asp:Button CssClass ="btn btn-primary" ID="DummySearchBtn" runat="server" Text="Search"  OnClientClick="return clickSearch()" /> 
                 </td>
          </tr> 
              
          <tr>
              <td class="txtSMBold">
                  Date:</td>
              <td>
                  <telerik:RadDatePicker ID="txtDate"   runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                  
              </td>
              <td class="txtSMBold">
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
          </tr>
              
        </table>

                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
     
 </ContentTemplate>
 </asp:UpdatePanel> 
  
   <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="SearchBtn"  runat="server" Text="Search" />
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
</td>
        </tr>
    </table>
    <div style="overflow:scroll; height:445px; border:groove;background-color:white;border-color:LightGrey;border-width:1px" id="RepSec" runat="server" visible="false">
  <rsweb:ReportViewer ID="RVMain" runat="server"     ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                    DocumentMapWidth="100%"  AsyncRendering="false" SizeToReportContent="true" > 
              </rsweb:ReportViewer>    
	 </div> 
</asp:Content>