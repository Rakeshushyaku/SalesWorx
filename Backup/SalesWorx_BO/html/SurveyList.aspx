<%@ Page Title="Survey List" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="SurveyList.aspx.vb" Inherits="SalesWorx_BO.SurveyList" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Customer Survey</span><asp:ImageButton ID="imgPrint" 
                ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" 
                BorderWidth="0" BorderStyle="None" AlternateText="Print" 
                style="height: 14px"   /></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td >Survey :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlSurvey" runat="server" AutoPostBack="true" DataTextField="Survey_title" DataValueField="Survey_Id"/>
            </td>
                 <td ><asp:Label ID="lblCustVan" runat="server" Text="Customer :"></asp:Label></td>
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
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
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
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
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