<%@ Page Title="SKU Dispersion" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMaster.Master" CodeBehind="Rep_SKUDispersion.aspx.vb" Inherits="SalesWorx_BO.Rep_SKUDispersion" %>
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
	<span class="pgtitile3">SKU Dispersion</span></div>
	
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
                       </telerik:RadComboBox>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
     
          <tr> 
            <td  class="txtSMBold">Year :</td>
            <td>
               <asp:DropDownList CssClass="inputSM" ID="ddYear"   Width ="150px"
                    runat="server" DataTextField="Yr" DataValueField="Yr">
                </asp:DropDownList>
            </td>
            <td  class="txtSMBold">Month :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddMonth"   Width ="150px"
                    runat="server" >
                    <asp:ListItem Value="0" Text="-- Select a value --"></asp:ListItem>                    
                    <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                    <asp:ListItem Value="5" Text="May"></asp:ListItem>
                    <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                    <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                    <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                    <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                    <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                    <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                    <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                    
                </asp:DropDownList>
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