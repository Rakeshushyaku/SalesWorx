<%@ Page Title="Price Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/ReportMasterForASPX.Master" CodeBehind="PriceListing.aspx.vb" EnableEventValidation="false"  Inherits="SalesWorx_BO.PriceListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Price List</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="105" class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </asp:DropDownList>
              </td>
                 <td width="105" class="txtSMBold">Product:</td>
            <td>
                <asp:DropDownList ID="drpProduct" runat="server">
                </asp:DropDownList>
                &nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>                 
          <tr> 
            <td width="105" class="txtSMBold">Price List Type :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Price_List_Type" DataValueField="Price_List_ID">
                </asp:DropDownList>
            </td>
            <td width="105" class="txtSMBold">UOM:</td>
            <td>    
                <asp:DropDownList ID="ddlUOM" 
                    runat="server" DataTextField="Price_List_Type" 
                    DataValueField="Price_List_ID">
                </asp:DropDownList>
              </td>       
          </tr>          
          <tr> 
            <td width="105" class="txtSMBold">Agency:</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlAgency" 
                    runat="server" DataTextField="Price_List_Type" 
                    DataValueField="Price_List_ID">
                </asp:DropDownList>
            </td>
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>    
                &nbsp;</td>       
          </tr>          
        </table>
 </ContentTemplate> </asp:UpdatePanel> 
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVPriceList" runat="server" 
                  EmptyDataText="No Price list to Display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                  <%--  <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Price_List_Line_ID" SortExpression="Price_List_Line_ID" HeaderText="Price List Line ID"/>                    --%>
                  
                   <asp:BoundField DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code" NullDisplayText="N/A" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Description" HeaderText="Description"  SortExpression="Description" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                     <asp:BoundField DataField="Price_List_Type" HeaderText="Price List Type"  SortExpression="Price_List_Type" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Item_UOM" HeaderText="Item UOM"  SortExpression="Item_UOM" NullDisplayText="N/A" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                      <asp:TemplateField HeaderText="Unit Selling Price" 
                          SortExpression="Unit_Selling_Price" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  >
                          <ItemTemplate>
                              <asp:Label ID="Label1" runat="server" 
                                  Text='<%# GetSellingPrice(Eval("Unit_Selling_Price"),Eval("Decimal_digits"))%>'></asp:Label>
                          </ItemTemplate>
                          
                          <HeaderStyle  Wrap="False" />
                      </asp:TemplateField>
                  </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>
          </td>
          </tr>
          
        </table> <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxtoolkit:modalpopupextender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
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
	 
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>  
        <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress> 
             <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>                   
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
