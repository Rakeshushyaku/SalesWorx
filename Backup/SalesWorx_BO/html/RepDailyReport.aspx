<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="RepDailyReport.aspx.vb" Inherits="SalesWorx_BO.RepDailyReport" %>
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
	<span class="pgtitile3">Daily Report</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="table1">
	<tr>
<td style="padding:6px 12px">
<table border="0" cellspacing="2" cellpadding="2" >
          <tr>
            <td class="txtSMBold">Organization :</td>
            <td>
                <asp:DropDownList ID="ddl_org" with="150px"  runat="server" AutoPostBack="true">
                </asp:DropDownList>
              </td>
               <td class="txtSMBold">
                   </td>
              <td>
                 
              </td>
          </tr>
          <%--           <asp:BoundField DataField="Scanned_Closing" ItemStyle-HorizontalAlign="center" HeaderText="Scanned" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>    --%>      
          
          <tr>
              <td valign ="top" class="txtSMBold">
                  Van:</td>
              <td colspan="3">
                  <asp:Panel ID="Panel12" runat="server" BorderStyle="Groove" BorderWidth="1px" 
                          Height="143px" ScrollBars="Auto" Width="300px">
                          <asp:CheckBoxList ID="chkSalesRep" runat="server" CellPadding="2" 
                              CellSpacing="4" Font-Bold="False" RepeatColumns="2">
                          </asp:CheckBoxList>
                      </asp:Panel></td>
          </tr>
          
         
        
        <tr>
            <td class="txtSMBold">
                From Date:</td>
            <td>
                <asp:TextBox ID="txt_fromDate" with="150px"  runat="server" CssClass="inputSM"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                    CssClass="cal_Theme1" Format="dd-MMM-yyyy" PopupButtonID="txt_fromDate" 
                    TargetControlID="txt_fromDate" />
            </td>
            <td class="txtSMBold">
                To Date:</td>
            <td>
                <asp:TextBox ID="txt_ToDate" with="150px" runat="server" CssClass="inputSM"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                    CssClass="cal_Theme1" Format="dd-MMM-yyyy" PopupButtonID="txt_ToDate" 
                    TargetControlID="txt_ToDate" />
                <asp:Button ID="SearchBtn" runat="server" CssClass="btnInputGrey" Text="Search" />
            </td>
        </tr>
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

  
    </table>
    <table>	<tr>
       <td style="background:#ffffff">
           <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="58%" 
                 SizeToReportContent="true" AsyncRendering="false"  
               DocumentMapWidth="100%" ShowParameterPrompts="False" Visible="False" > 
              </rsweb:ReportViewer>      
       </td>              
	</tr></table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>