<%@ Page Title="Area Van Mapping" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AreaVanMapping.aspx.vb" Inherits="SalesWorx_BO.AreaVanMapping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            width: 120px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ConfirmOnDelete() {
            if (checkBoxselectedornot()) {
                if (confirm("The selected item(s) will be deleted. Continue?") == true)
                    return true;
                else
                    return false;
            } else return false;
        
    }
    function checkBoxselectedornot() {

        var frm = document.forms[0];
        var flag = false;
        for (var i = 0; i < document.forms[0].length; i++) {
            if (document.forms[0].elements[i].id.indexOf('chkSelect') != -1) {
                if (document.forms[0].elements[i].checked) {
                    flag = true
                }
            }
        }
        if (flag == true) {
            return true
        } else {
            alert('Please tick at least one checkbox to delete.')
            return false
        }

    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Van Territory Mapping</span></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >
        <ContentTemplate> <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label><asp:HiddenField
            ID="hfID" runat="server" Value="0" />
<table width="100%" border="0" cellpadding="3" cellspacing="10">
          <tr>
            <td class="txtSMBold">Van :</td>
            <td class="txtSMBold"><asp:DropDownList CssClass="inputSM" ID="drpVan"  Width ="250px"
                    runat="server" DataTextField="Description" DataValueField="ORG_HE_ID">
                </asp:DropDownList>
            </td>
          </tr>
          <tr>
            <td class="txtSMBold">Customer Segment :</td>
            <td class="txtSMBold">
                <asp:DropDownList CssClass="inputSM" Width ="250px" ID="drpCustomerSegment" runat="server" >
                  </asp:DropDownList>
            </td>
          </tr>
          <tr> 
            <td class="txtSMBold">Sales District:</td>
            <td class="txtSMBold"> <asp:DropDownList CssClass="inputSM" Width ="250px" ID="drpSalesDistrict" runat="server" >
                  </asp:DropDownList>
             </td>
          </tr>
          <tr> 
            <td class="txtSMBold">&nbsp;</td>
            <td class="txtSMBold"> <asp:Button CssClass="btnInputBlue" ID="btnAdd" runat="server" 
                    Text="Add" />
                <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" 
                    CausesValidation="False" />
                <asp:Button CssClass="btnInputRed" ID="btnDelete" runat="server" Text="Delete" 
                    CausesValidation="False" OnClientClick="return ConfirmOnDelete()" />
             </td>
          </tr>
        </table>
 </ContentTemplate> 
 </asp:UpdatePanel> 
</td>
</tr>
	<tr>
       <td align="center">
        <asp:UpdatePanel ID="Panel" runat="server">
        <ContentTemplate>
        <table  border="0" cellpadding="0" cellspacing="0" width ="100%">
        <tr>
        <td >
              <asp:GridView  width="100%" ID="grdAreaVanList" runat="server" 
                  EmptyDataText="No listing found" EmptyDataRowStyle-Font-Bold="true" 
                  AutoGenerateColumns="False" AllowPaging="True"
                   AllowSorting="True" 
                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                   
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                   <asp:TemplateField>
                                                         
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                 <asp:ImageButton ID="btnEdit" ToolTip="Edit Mapping" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AF_Map_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                   <asp:BoundField DataField="SalesRep_Name" HeaderText="Van"  SortExpression="SalesRep_Name" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                   <asp:BoundField ItemStyle-HorizontalAlign="center" DataField="CustomerSegment" SortExpression="CustomerSegment" HeaderText="Customer Segment">
                       <ItemStyle HorizontalAlign="Center" />
                   </asp:BoundField> 
                   <asp:BoundField DataField="SalesDistrict" HeaderText="Sales District"  SortExpression="SalesDistrict" NullDisplayText="N/A" >
                      <ItemStyle Wrap="False" />
                   </asp:BoundField>
                      <asp:TemplateField>
                          <ItemTemplate>
                              <asp:CheckBox ID="chkSelect" runat="server" />
                              <asp:HiddenField ID="hfMapID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"AF_Map_ID") %>' />
                               <asp:HiddenField ID="hfSalesrep_ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Salesrep_ID") %>' />
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
          
        </table>
         <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
              <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                        Drag="true" />
                                                    <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup"   style="display:none">
                                                    
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
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>
