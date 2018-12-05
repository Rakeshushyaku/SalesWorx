<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="Rep_DistributionDetails.aspx.vb" Inherits="SalesWorx_BO.Rep_DistributionDetails" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            height: 29px;
        }
        .style2
        {
            height: 29px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Distribution Check Details</span></div>
	
	<table width="100%" border="1" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="table1">
	<tr>
<td style="padding:6px 12px">
<table  border="0" cellspacing="2" cellpadding="2" >
   <tr>
              <td class="txtSMBold">
                Organization: </td>
              <td>
                  <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="inputSM" 
                      DataTextField="Description" DataValueField="MAS_Org_ID" Width="200px" 
                      AutoPostBack="True">
                  </asp:DropDownList>
              </td>
              <td class="txtSMBold">
                     Van.:</td>
              <td>
                  <asp:DropDownList ID="ddlVan" runat="server" CssClass="inputSM" 
                      DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Width="200px">
                  </asp:DropDownList>
                  
                  
                  
                  
              </td>
              <td class="txtSMBold">
                  Outlet:</td>
              <td>
                   <asp:DropDownList CssClass="inputSM" ID="ddlCustomer"   Width ="250px"
                runat="server" DataTextField="Outlet" DataValueField="CustomerID">
                </asp:DropDownList> </td>
     </tr> 
         
          <tr> 
             <td class="style1">Year:</td><td class="style2">
                 <asp:DropDownList ID="ddlyear" runat="server" 
                     CssClass="inputSM" 
                     Height="17px" Width="100px">
                 </asp:DropDownList>
                </td><td class="style1">Month:</td><td class="style2">
                 <asp:DropDownList ID="ddlMonth" runat="server" 
                     CssClass="inputSM"  
                     Width="100px">
                 </asp:DropDownList>
                 &nbsp;
                
                 <asp:Button ID="btnSearch1" runat="server" CssClass="btnInput" Text="Search" />
                
          </tr>  
        
        <td class="style2">
            &nbsp;</td>
        
        <td class="style2">
            &nbsp;</td>
        
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
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
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