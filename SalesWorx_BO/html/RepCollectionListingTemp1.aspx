<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepCollectionListingTemp1.aspx.vb" Inherits="SalesWorx_BO.RepCollectionListingTemp1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .RadPanelBar_Simple a.rpLink, .RadPanelBar_Simple div.rpLink, .RadPanelBar_Simple a.rpLink:hover,
        .RadPanelBar_Simple a.rpSelected, .RadPanelBar_Simple div.rpSelected, .RadPanelBar_Simple a.rpSelected:hover  {
    background-color: #999 !important;
    border-color: #999 !important;
    color:#fff !important;
}

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

    .RadPanelBar_Simple .rpExpandable span.rpExpandHandle{
    background-position: 100% -5px !important;
}
.abc
{
    border: #ccc solid 2px;
    background: #fff;
}
    </style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Collection Listing</h4>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                            <table border="0" cellspacing="2" cellpadding="2" style="padding:5px 0px 5px" >
                                <tr>
                                    <td  class="txtSMBold">
                                        Organization:
                                    </td>
                                    <td>
                                         <telerik:RadComboBox Skin="Simple" ID="ddlOrganization" Width ="200px" runat="server" DataTextField="Description"
                                            DataValueField="MAS_Org_ID" AutoPostBack="True">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td  class="txtSMBold">
                                       Van:
                                    </td>
                                    <td>
                                         <telerik:RadComboBox Skin="Simple" ID="ddVan" Width ="200px" runat="server" DataTextField="SalesRep_Name" 

DataValueField="SalesRep_ID">
                                        </telerik:RadComboBox> &nbsp;
                                        
                                    </td>
                                    <td class="txtSMBold">
                                                                         Collection Ref. No:</td>
                                    <td><asp:TextBox ID="txtCollectionRefNo" Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>

                                        <asp:Button  CssClass ="btn btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
                                       </td>
                                    
                                </tr>
                                <tr>
                                    <td class="txtSMBold">
                                        From Date :
                                    </td>
                                    <td>
                                       
                                        <telerik:RadDatePicker ID="txtFromDate"   runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                    </td>
                                    <td  class="txtSMBold">
                                        To Date:
                                    </td>
                                    <td>
                                       
                                        <telerik:RadDatePicker ID="txtToDate"   runat="server">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar1" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker>
                                    </td>
                                </tr>
                                 
                            </table>
                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
                                        
                           </ContentTemplate> </asp:UpdatePanel> 
  
   <asp:UpdatePanel ID="UPModal" runat="server">
                                <ContentTemplate>
                                    <table width="auto" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
 
           
        
  
    </table>
	<div style="height:100%;" id="RepSec" runat="server" visible="false">
  <rsweb:ReportViewer ID="RVMain" runat="server"  CssClass="abc"   ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>    
	 </div>
</asp:Content>
