<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="Rep_Summary.aspx.vb" Inherits="SalesWorx_BO.Rep_Summary" %>
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
	<span class="pgtitile3">Purchase Vs Sales Vs Returns Report</span></div>
	
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
                  Organization :
              </td>
              <td>
                  <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="inputSM" 
                      DataTextField="Description" DataValueField="MAS_Org_ID" Width="200px" 
                      AutoPostBack="True">
                  </asp:DropDownList>
              </td>
              <td class="txtSMBold">
                  Agency:</td>
              <td>
                  <asp:DropDownList ID="ddlAgency" runat="server" CssClass="inputSM" 
                      DataTextField="Agency" DataValueField="Agency" Width="200px">
                  </asp:DropDownList>
                  
                  
                  
              </td>
     </tr> 
     <tr> 
          <td class="txtSMBold">
              Type:</td>  
            <td> 
                <asp:DropDownList ID="ddlType" runat="server" CssClass="inputSM" Width="200px" 
                    AutoPostBack="True">
                    <asp:ListItem Value="D">Daily</asp:ListItem>
                    <asp:ListItem Value="W">Weekly</asp:ListItem>
                    <asp:ListItem Value="M">Monthly</asp:ListItem>
                    <asp:ListItem Value="Y">Yearly</asp:ListItem>
                </asp:DropDownList>
          </td>
            <td  class="txtSMBold" >
                Van:</td>
               <td  class="txtSMBold" >
                   <asp:DropDownList ID="ddlVan" runat="server" CssClass="inputSM" 
                       DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Width="200px">
                   </asp:DropDownList>
            </td>
          </tr>  
         
          <tr> 
            <td  class="txtSMBold">Period :</td>
            <td>
                   <asp:DropDownList ID="ddlFromYear" runat="server" CssClass="inputSM" AutoPostBack="true"
                    >
                </asp:DropDownList>
                           <asp:DropDownList ID="ddlFromMonth" runat="server" CssClass="inputSM" AutoPostBack="true"
                    >
                </asp:DropDownList>
                <asp:DropDownList ID="ddlWeek" runat="server" CssClass="inputSM" Visible="False" 
                    >
                </asp:DropDownList>
                <asp:Panel runat="server" ID="pnl_daily"><table cellpadding='0' cellspacing='0'><tr><td class="txtSMBold">From:</td><td><asp:TextBox  ID="txtfromDate"  Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox> 
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtfromDate" PopupButtonID="txtfromDate"  />                
                </td><td class="txtSMBold">To:</td><td><asp:TextBox  ID="txtToDate"  Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender2" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
         </td></tr></table>
                     </asp:Panel>
            </td>
            <td  class="txtSMBold">Report Type:</td>
            <td>
                  <asp:DropDownList ID="ddlRType" runat="server" CssClass="inputSM">
                      <asp:ListItem Text="Table" Value="T"></asp:ListItem>
                      <asp:ListItem Text="Graph" Value="G"></asp:ListItem>
                  </asp:DropDownList>
                  <asp:Button ID="btnSearch0" runat="server" CssClass="btnInput" Text="Search" />
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
