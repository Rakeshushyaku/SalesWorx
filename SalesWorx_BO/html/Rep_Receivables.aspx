<%@ Page Title="Receivables for a month" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_Receivables.aspx.vb" Inherits="SalesWorx_BO.Rep_Receivables" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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
    <h4>Receivables for a month</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px" >

  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	
<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems" onclientitemexpand="OnClientItemExpand"  onclientitemcollapse="OnClientItemCollapse"   >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
  <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization</label>
                                     <telerik:RadComboBox Skin="Simple" Filter="Contains"  ID="ddlOrganization"  Width ="100%" AutoPostBack="true"
                                            runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                                        </telerik:RadComboBox>
                                       </div></div> 

                  <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Year</label>
             <telerik:RadComboBox ID="ddYear" Skin="Simple" Filter="Contains"  runat="server" DataTextField="Yr" DataValueField="Yr"
                Width ="100%"  >
                       </telerik:RadComboBox>
               </div></div>
          
     
           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Month</label>
              <telerik:RadComboBox Skin="Simple" ID="ddMonth"   Width ="100%"
                    runat="server" >
                  <Items>
                    <telerik:RadComboBoxItem Value="0" Text="-- Select a value --"></telerik:RadComboBoxItem>                    
                    <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                    <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                    </Items>
              </telerik:RadComboBox>
               </div></div>
</div>
                                                </div>

       <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button ID="DummySearchBtn" runat="server" Text="Search"  OnClientClick="return clickSearch()"  CssClass ="btn btn-primary" />
                                                    </div>
                                                </div>
      </div>
                                        

      
 </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar>
 </ContentTemplate> </asp:UpdatePanel> 
  
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
   </div>
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
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
