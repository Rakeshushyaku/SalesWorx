<%@ Page Title="Sync Exception Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="Rep_LastDeviceSyncLog.aspx.vb" Inherits="SalesWorx_BO.Rep_LastDeviceSyncLog" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Sync Exception Report</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >

  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	
<table  border="0" cellspacing="2" cellpadding="2" >
          <tr>
            <td  class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization"   Width ="150px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>
              </td>
                 <td  class="txtSMBold">Van :</td>
            <td>  <telerik:RadComboBox ID="ddl_Van" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AutoPostBack="true"
                CssClass="inputSM"      >
                       </telerik:RadComboBox>
                     
                     </td>
          </tr>
     
          <tr> 
            <td  class="txtSMBold">Last Sync.before :</td>
            <td>
              <asp:DropDownList ID="ddlHours" runat ="server"  CssClass="inputSM">
 <asp:ListItem Text ="Today" value="0">Today</asp:ListItem>
              <asp:ListItem Text ="24 Hours" value="24">24 Hours</asp:ListItem>
               <asp:ListItem Text ="48 Hours" value="48">48 Hours</asp:ListItem>
              <asp:ListItem Text ="72 Hours" Value ="72">72 Hours</asp:ListItem>
                 <asp:ListItem Text ="96 Hours" Value ="96">96 Hours</asp:ListItem>
              <asp:ListItem Text ="120 Hours" Value ="120">120 Hours</asp:ListItem>
              <asp:ListItem Text ="168 Hours" Value ="168">168 Hours</asp:ListItem>
              </asp:DropDownList>
             
            </td>
            <td  class="txtSMBold">
            Sync Type
            </td>
            <td>
                <asp:DropDownList ID="ddlSyncType" runat ="server"  CssClass="inputSM">
                </asp:DropDownList>
 <%--<asp:CompareValidator ID="CompareValidatorRadComboxBoxTables" runat="server" ValidationGroup="valsum"
 ValueToCompare="0" Operator="NotEqual" ControlToValidate="ddlSyncType" ForeColor ="Red"  
 errorMessage="*"  Font-Bold="true" />--%>
                 <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search"  />
            </td>       
          </tr>   
      
        </table>
 
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
	<tr>
       <td style="background:#ffffff">
           <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>      
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
