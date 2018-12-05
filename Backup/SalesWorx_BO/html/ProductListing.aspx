<%@ Page Title="Product List" Language="vb" AutoEventWireup="false" EnableEventValidation="false"  MasterPageFile="~/html/ReportMasterForASPX.Master" CodeBehind="ProductListing.aspx.vb" Inherits="SalesWorx_BO.ProductListing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script language="javascript" type="text/javascript">
      var TargetBaseControl = null;

      window.onload = function() {
          try {
              TargetBaseControl =
           document.getElementById('<%= Me.Panel.ClientID %>');
          }
          catch (err) {
              TargetBaseControl = null;
          }
      }
      function pageLoad(sender, args) {
          if (!args.get_isPartialLoad()) {
              //  add our handler to the document's
              //  keydown event
              $addHandler(document, "keydown", onKeyDown);
          }
      }

      function onKeyDown(e) {
          if (e && e.keyCode == Sys.UI.Key.esc) {
              // if the key pressed is the escape key, dismiss the dialog
              $find('<%= Me.MpInfoError.ClientID %>').hide();
             
          }
      }
      
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Product&nbsp; List</span><asp:ImageButton ID="imgPrint" ImageUrl="~/images/iconPrinter.gif" runat="server" ImageAlign="Right" BorderWidth="0" BorderStyle="None" AlternateText="Print"   /></div>
	
	<table width="95%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="75" class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList>
            </td>
            <td width="75" class="txtSMBold">Type :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlProductType" runat="server" >
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">MSL</asp:ListItem>
                    <asp:ListItem Value="2">Non-MSL</asp:ListItem>
                </asp:DropDownList>
                &nbsp;<asp:Button CssClass="btnInput" ID="SearchBtn" runat="server" Text="Search" />
            </td>
          </tr>
          <tr> 
            <td width="75" class="txtSMBold">Item Code :</td>
            <td><asp:TextBox ID="txtItemCode" CssClass="inputSM" runat="server"></asp:TextBox>
             </td>
                <td width="75" class="txtSMBold">Description :</td>
            <td><asp:TextBox ID="txtDescription" CssClass="inputSM" runat="server"></asp:TextBox> 
                     </td>      
          </tr>
        </table>
 </ContentTemplate> </asp:UpdatePanel> 
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0" width="100%" >
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVProductList" runat="server" 
                  EmptyDataText="No Products to Display" EmptyDataRowStyle-Font-Bold="true" 
                  CssClass="txtSM" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                  PageSize="25" CellPadding="6">
                    <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" 
                        CssClass="tdstyle" Height="12px" Wrap="True" />
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                   <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="Item_No" HeaderText="Item No"  SortExpression="Item_No" NullDisplayText="N/A" Visible="false"  >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                     <asp:BoundField HeaderStyle-HorizontalAlign="Left"  DataField="Inventory_Item_ID" HeaderText="Inv. Item ID"  SortExpression="Inventory_Item_ID" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left"  DataField="Item_Code" HeaderText="Item Code"  SortExpression="Item_Code" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                   <asp:BoundField ItemStyle-HorizontalAlign="center" DataField="Description" HeaderText="Description" SortExpression="Description">
                       <HeaderStyle HorizontalAlign="Left" />
                       <ItemStyle HorizontalAlign="Left" />
                   </asp:BoundField> 
                <%--
                   <asp:BoundField DataField="Brand_Code" HeaderText="Brand Code"  SortExpression="Brand_Code" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                   <asp:BoundField DataField="Item_Size" HeaderText="Item Size"  SortExpression="Item_Size" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                   <asp:BoundField DataField="EANNO" HeaderText="EANNO"  SortExpression="EANNO" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>--%>
                   <asp:BoundField DataField="Primary_UOM_Code" ItemStyle-HorizontalAlign="center" HeaderText="Primary UOM Code" >
                       <ItemStyle HorizontalAlign="Center" />
                   </asp:BoundField>
                 <%--  <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Promo_Item" HeaderText="Is-Promo Item" SortExpression="Promo_Item" />--%>
                   <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="IsMSL" HeaderText="Is-MSL"  SortExpression="IsMSL">
                       <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                 </Columns>
                   <PagerStyle CssClass="pagernumberlink" />
                       <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" 
                           BorderWidth="1px" CssClass="headerstyle" />
              </asp:GridView>
          </td>
          </tr>
          
        </table>
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
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
