<%@ Page Title="Collection Listing" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="CollectionListing.aspx.vb" Inherits="SalesWorx_BO.CollectionListing" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
        var TargetBaseControl = null;

        window.onload = function() {
            try {
                TargetBaseControl =
           document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }
        function TestCheckBox() {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkRelease";
             var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to release the selected Collection ?');
                  return true;
               }
        alert('Select at least one Collection !');
                return false;

          }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkRelease";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Held PDC</span></div>
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="105" class="txtSMBold">Organization :</td>
            <td><asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="170px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList>
              </td>
                 <td width="105" class="txtSMBold">Collection Ref. No:</td>
            <td colspan="3"><asp:TextBox ID="txtCollectionRefNo"   Width ="170px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                     <asp:Button CssClass="btnInputGrey" ID="SearchBtn" runat="server" Text="Search" />
                     </td>
          </tr>
          <tr> 
            <td width="105" class="txtSMBold">From Date :</td>
            <td>
                <asp:TextBox  ID="txtFromDate"   Width ="130px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                
            </td>
            <td width="105" class="txtSMBold">To Date:</td>
            <td colspan="3">
                <asp:TextBox  ID="txtToDate"   Width ="130px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd/MM/yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
            </td>       
          </tr>
        </table>
</td>
</tr>
	<tr>
       <td>
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server">
        <ContentTemplate>
        <table  border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td>
              <asp:GridView  width="100%" ID="GVCollectionList" runat="server" DataKeyNames="Collection_ID"
                  EmptyDataText="No Collections to Display" EmptyDataRowStyle-Font-Bold="true" 
                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" 
                   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                  
                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                    <EmptyDataRowStyle Font-Bold="True" />
                  <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All" runat="server" />
                            <asp:ImageButton ToolTip="Release Selected Items " ID="btnReleaseAll" runat="server" CausesValidation="false"   ImageUrl="~/images/edit-13.png"   OnClientClick="return TestCheckBox()"
                                OnClick="btnReleaseAll_Click" />
                        </HeaderTemplate>
                        <HeaderStyle HorizontalAlign="Left"  Wrap="false" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRelease" runat="server" />
                            <asp:ImageButton ToolTip="Release Collection" ID="btnRelease" OnClick="btnRelease_Click" runat="server" CausesValidation="false"   ImageUrl="~/images/edit-13.png"   OnClientClick="javascript:return confirm('Would you like to release the selected Collection?');" />                            
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField SortExpression="Collection_ID">                         
                          <ItemTemplate>
                              <asp:HiddenField ID="hdnCollection_ID" runat="server" Value='<%# Bind("Collection_ID") %>'/>                          
                          </ItemTemplate>                          
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Collection Ref No" SortExpression="Collection_Ref_No">                         
                          <ItemTemplate>
                              <asp:LinkButton ID="lnkbtnCollection_Ref_No" runat="server" Text='<%# Bind("Collection_Ref_No") %>' OnClick="btnDetail_Click" Font-Bold="true"/>
                          </ItemTemplate>
                          <ItemStyle Wrap="False" />
                      </asp:TemplateField>
                    <asp:BoundField DataField="Collected_On" HeaderText="Collected On"  
                          SortExpression="Collected_On" NullDisplayText="N/A" 
                          DataFormatString="{0:dd/MM/yyyy}" >
                      <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Collected_By" HeaderText="Collected By"  
                          SortExpression="Collected_By">
                        <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                     <asp:BoundField DataField="Customer_Name" ItemStyle-HorizontalAlign="center" 
                          HeaderText="Customer Name" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                    <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="center" 
                          HeaderText="Amount" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>
                     <asp:BoundField DataField="Cheque_No" ItemStyle-HorizontalAlign="center" HeaderText="Cheque No" >
                        <ItemStyle HorizontalAlign="Center" />
                     </asp:BoundField>                      
                     <asp:BoundField DataField="Cheque_Date" DataFormatString = "{0:dd/MM/yyyy}" 
                          HeaderText="Cheque Date" SortExpression="Cheque_Date" >
                         <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Bank_Name" HeaderText="Bank_Name" SortExpression="Bank_Name" >
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                          DataField="Emp_Code" HeaderText="Emp Code">
                          <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                      </asp:BoundField>
                  </Columns>
                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
              </asp:GridView>
            <asp:HiddenField ID="hdnCollectionID" runat="server" value=""/>
          </td>
          </tr>          
        </table>
        <table>
        <tr>
            <td>
            <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPECollection"
                    runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                    CancelControlID="btnCancel">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width ="420" CssClass="modalPopup">
                    <asp:Panel ID="DragPnl" runat="server" Width ="415" Style="cursor: move; background-color: #3399ff;
                        text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                        Collection Details</asp:Panel>
                    <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                    <table>
                    <tr>
                        <td>
                             <asp:GridView  width="100%" ID="GVCollDtlList" runat="server" EmptyDataText="No details to Display" EmptyDataRowStyle-Font-Bold="true" 
                                  AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true" 
                                  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                 
                                  <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                  <Columns>
                                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Collection_Line_Ref" SortExpression="Collection_Line_Ref" HeaderText="Collection Line Ref"/>                    
                                     <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No"  SortExpression="Invoice_No" NullDisplayText="N/A" >
                                      <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount"  SortExpression="Amount" NullDisplayText="N/A"  >
                                      <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ERP_Status" HeaderText="ERP Status"  SortExpression="ERP_Status"/>                    
                                  </Columns>
                                  <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                              </asp:GridView>
                        </td>
                    </tr>
                     <tr>
                        <td align ="center">
                        <br /><br />
                         <asp:Button ID="btnReleaseDtl" CssClass="btnInput" runat="server" CausesValidation="false" Text="Release" OnClientClick="javascript:return confirm('Would you like to release this Collection?');" />
                         <asp:Button ID="btnCancel" CssClass="btnInput" runat="server" CausesValidation="false" Text="Close" />
                        </td>
                    </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        </table>
        <table>
            <tr>
                 <td>
                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                        TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                        Drag="true" />
                      <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                    padding: 3px; display:none;">
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
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="SearchBtn" EventName="Click" />
          </Triggers>
        </asp:UpdatePanel>                    
       </td>              
	</tr>
  
    </table>
	<asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                <span style="font-size: 12px; color: #3399ff;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    <br />
	</td> 
	</tr>
	</table>
</asp:Content>
