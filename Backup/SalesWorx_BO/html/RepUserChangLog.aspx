﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="RepUserChangLog.aspx.vb" Inherits="SalesWorx_BO.RepUserChangLog" %>



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
	<span class="pgtitile3">User Log Report</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<table  border="0" cellspacing="2" cellpadding="2">
          <tr>
            <td  class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization" width="150px" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>
              </td>
                 <td  class="txtSMBold">Van :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlVan"  width="150px"
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                </asp:DropDownList>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
           
          <tr> 
            <td class="txtSMBold">From Date :</td>
            <td>
                <asp:TextBox  ID="txtFromDate"  width="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                
            </td>
            <td  class="txtSMBold">To Date :</td>
            <td>
                <asp:TextBox  width="150px" ID="txtToDate" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
            </td>       
          </tr>   
          <tr> 
            <td  class="txtSMBold">User :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlUser"  width="150px"
                runat="server" DataTextField="UserName" DataValueField="User_ID">
                </asp:DropDownList>            
            </td>            
              <td class="txtSMBold">
                  Trans Type :</td>
            <td>
                             
                <asp:DropDownList ID="ddlOrderType" width="150px" runat="server" CssClass="inputSM">
                    <asp:ListItem Value="0">ALL</asp:ListItem>
                    <asp:ListItem Value="Inserted">Inserted</asp:ListItem>
                    <asp:ListItem Value="Deleted">Deleted</asp:ListItem>
                    <asp:ListItem Value="Updated">Updated</asp:ListItem>
                    <asp:ListItem Value="Approved">Approved</asp:ListItem>
                    <asp:ListItem Value="Login">Login</asp:ListItem>
                    <asp:ListItem Value="Logout">Logout</asp:ListItem>
                </asp:DropDownList>
                             
                        
            </td>       
          </tr>         
          
           <tr> 
            <td  class="txtSMBold">Module :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlModule"  width="150px"
                runat="server" >
                </asp:DropDownList>            
            </td>            
              <td class="txtSMBold">
                  Sub Module :</td>
            <td>
                             
                  <asp:DropDownList CssClass="inputSM" ID="ddlSubModule"  width="150px"
                runat="server" >
                </asp:DropDownList>     
                             
                        
            </td>       
          </tr>         
          
           <tr> 
            <td  class="txtSMBold">Key Value :</td>
            <td>
                <asp:TextBox  ID="txtKeyValue" width="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                
            </td>
            <td colspan ="2">
            </td>       
          </tr>   
        </table>
  </ContentTemplate>
                  <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="ddlOrganization" 
                          EventName="SelectedIndexChanged" />
        </Triggers>
                  </asp:UpdatePanel>
  
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
