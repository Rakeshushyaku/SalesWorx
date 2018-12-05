<%@ Page Title="Import Bonus Rules" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ImportBonusRules.aspx.vb" Inherits="SalesWorx_BO.ImportBonusRules" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
         <script language="javascript" type="text/javascript">
             var prm = Sys.WebForms.PageRequestManager.getInstance();

             prm.add_initializeRequest(InitializeRequest);
             prm.add_endRequest(EndRequest);
             var postBackElement;
             function InitializeRequest(sender, args) {

                 if (prm.get_isInAsyncPostBack())
                     args.set_cancel(true);
                 postBackElement = args.get_postBackElement();
     //            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                     $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                     postBackElement.disabled = true;
      //           }
            
             }

             function EndRequest(sender, args) {
         //        if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                     $get('<%= UpdateProgress1.ClientID %>').style.display = 'none';
                     postBackElement.disabled = false;
          //       }
            } 

</script>
         <input type="hidden" name="Action_Mode" value="" />
      <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Import Bonus Rules</span></div>
	<div id="pagenote" >This screen may be used to import bonus rules for products.</div>	
	   <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
          <ProgressTemplate>
          <div style="z-index:9999; position:absolute; top:48%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>

	    <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">

		 <tr>
    <td><label>Select a File </label> </td>
    <td><asp:FileUpload ID="ExcelFileUpload" runat="server" />
            <asp:Button ID="BtnImport" runat="server" Text="Import" CssClass="btnInputGreen" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
         </td>
       
          </tr>
        
<tr>
<td colspan ="4">
 
                                                <asp:GridView Width="100%" ID="dgvErros" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="txtSM" AutoGenerateColumns="False" 
                                                        AllowPaging="false" AllowSorting="false" PageSize="25" CellPadding="6" >
                                                     <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle"
                                                        Height="12px" Wrap="True" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColNo"
                                                                HeaderText="Col No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColName"
                                                                HeaderText="Colume Name">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>--%>
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ErrorText"
                                                                HeaderText="Error Text">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                        
                                                        
                                                          
                                                        </Columns>
                                                          <PagerStyle CssClass="pagernumberlink" />
                                                    <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                                        CssClass="headerstyle" />
                                                    </asp:GridView>
<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
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
                                                       <asp:Label ID="lblinfo" runat="server" Font-Size ="13px" Font-Bold ="true" ></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="text-align: center">
                                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                      <asp:Label ID="lblMessage" runat="server"  Font-Size ="13px"  ></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="text-align: center;">
                                                        <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                      
        <div id="divResult" runat="server" style="width:95%; height:365px; overflow:scroll; text-align:left; padding:10px; ">
         <asp:Label ID="lblResult" runat="server"></asp:Label>       
        </div>
    	</ContentTemplate>
	<Triggers>
	<asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	</Triggers>
	</asp:UpdatePanel>	
</td>

</tr>
  	
	  </table>
	
	</tr>
</table>
</asp:Content>
