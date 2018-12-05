<%@ Page Title="Audit Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AuditSurvey.aspx.vb" Inherits="SalesWorx_BO.AuditSurvey" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Audit Survey</span></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td >Survey :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlSurvey" runat="server" AutoPostBack="true" DataTextField="Survey_title" DataValueField="Survey_Id"/>
            </td>
                 <td ><asp:Label ID="lblCustVan" runat="server" Text="Van :"></asp:Label></td>
                 <td width="105" class="txtSMBold">
                <asp:DropDownList CssClass="inputSM" ID="ddlCustomer" runat="server" AutoPostBack="true" DataTextField="Name" DataValueField="ID"/>                        
              </td>
            <td>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
            <tr>
                <td class="txtSMBold">
                <asp:Label ID="lblStartDatetxt" runat="server" Text="Start Date :" Visible="false"></asp:Label>
                    </td>
                <td class="txtSMBold">
                <asp:Label ID="lblStartDateval" runat="server" CssClass="inputSM"></asp:Label>
                </td>
                <td  align="left" class="txtSMBold"><asp:Label ID="lblEndDatetxt" runat="server" Text="End Date :" Visible="false"></asp:Label></td>
                <td width="105" class="txtSMBold"><asp:Label ID="lblEndDateval" runat="server" 
                        CssClass="inputSM"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>      
        </table>
</td>
</tr>
<tr><td></td></tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVCustSurvey" runat="server" EmptyDataText="No survey to display" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                 CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                   
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                  <Columns>
                     <asp:BoundField DataField="Survey_Timestamp" HeaderText="Surveyed On"  SortExpression="Survey_Timestamp" DataFormatString = "{0:dd/MM/yyyy}" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    
                     <asp:TemplateField HeaderText="Details" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                        <ItemTemplate>
                        <asp:HyperLink  runat="server" ID="hyp" Text="Click here to view survey details" navigateurl='<%# String.Format("SurveyListDetail.aspx?customerID={0}&siteID={1}&surcustresdate={2}&cust={3}&surid={4}", Eval("Customer_ID"), Eval("Site_Use_ID"), FormatDateTime(Eval("Survey_Timestamp"), DateFormat.ShortDate),Eval("Name"),Eval("Survey_ID")) %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField> 
                  </Columns>
                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
          </td>
          </tr>
          <tr>
          <td>
             
          </td>
          </tr>
          
        </table>
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>