<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="RepRouteMaster.aspx.vb" Inherits="SalesWorx_BO.RepRouteMaster" %>
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
	<span class="pgtitile3">Route Master</span></div>
	
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
            <td valign ="top"  class="txtSMBold">Organization :</td>
            <td style ="padding-left:10px;">
             <asp:Panel ID="Panel2" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px"  Width="300px">
            <asp:CheckBoxList ID="Chk_Organization" runat="server" RepeatColumns="2" 
                                             Font-Bold="False" CellPadding="2" CellSpacing="4" 
                     AutoPostBack="True">
                                        </asp:CheckBoxList>
                                        </asp:Panel>
              </td>
               <td  valign ="top"  class="txtSMBold">Van :</td>
            <td style ="padding-left:10px;">   <asp:Panel ID="Panel3" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                        BorderWidth="1px"  Width="300px">
                                        <asp:CheckBoxList ID="chkSalesRep" runat="server" RepeatColumns="2" 
                                             Font-Bold="False" CellPadding="2" CellSpacing="4">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
              <td>
                  &nbsp;</td>
          </tr>
          <%--           <asp:BoundField DataField="Scanned_Closing" ItemStyle-HorizontalAlign="center" HeaderText="Scanned" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>    --%>      
          
          <tr >
              <td class="txtSMBold" width="115">
                 Month:</td>
              <td>
                  <asp:DropDownList ID="ddl_month" runat="server" Width="150px" CssClass="inputSM" >
                  </asp:DropDownList>
              </td>
              <td class="txtSMBold">
                  Distributor Code:</td>
              <td class="txtSMBold">
                  <asp:TextBox ID="txt_distribCode" runat="server"  Width="150px" CssClass="inputSM" text="2102201"></asp:TextBox>
              </td>
              <td>
                  <asp:Button ID="SearchBtn" runat="server" CssClass="btnInputGrey" Text="Search" />
              </td>
          </tr>
        </table>
 
 </ContentTemplate> </asp:UpdatePanel> 
  
  
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
                  ProcessingMode="Remote" Width="58%" 
                 SizeToReportContent="true" AsyncRendering="false"  
               DocumentMapWidth="100%" ShowParameterPrompts="False" Visible="False" > 
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
