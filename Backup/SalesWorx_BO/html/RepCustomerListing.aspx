﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="RepCustomerListing.aspx.vb" Inherits="SalesWorx_BO.RepCustomerListing" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Customer Listing</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
<table  border="0" cellspacing="2" cellpadding="2">
          <tr>
            <td width="105" class="txtSMBold">Organization :</td>
            <td ><asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList>
              </td>
                 <td width="105" class="txtSMBold">Customer No:</td>
            <td ><asp:TextBox ID="txtCustomerNo" CssClass="inputSM" Width ="150px" runat="server"></asp:TextBox>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
          <tr> 
            <td width="105" class="txtSMBold">Segment :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlSegment" Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="Customer_Segment_ID">
                </asp:DropDownList>
            </td>
            <td width="105" class="txtSMBold">Sales District:</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlSalesDist"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="Sales_District_ID">
                </asp:DropDownList>
            </td>       
          </tr>         
          <tr> 
            <td width="105" class="txtSMBold">Name :</td>
            <td>
               <asp:TextBox ID="txtCustomerName" Width ="200px" CssClass="inputSM" runat="server"></asp:TextBox>
            </td>
            <td width="105" class="txtSMBold">Customer Status :</td>
            <td>    
                <asp:DropDownList ID="ddlCust_Stat" CssClass="inputSM" Width ="200px" runat="server">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Normal</asp:ListItem>
                    <asp:ListItem Value="2">Blocked</asp:ListItem>
                    <asp:ListItem Value="3">Cash Customer</asp:ListItem>
                </asp:DropDownList>
            </td>       
          </tr> 
         <%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%>
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
           <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove" 
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
