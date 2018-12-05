<%@ Page Title="MSL Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AdminMSL.aspx.vb" Inherits="SalesWorx_BO.AdminMSL" %>

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
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
        } 

    </script>
 <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">MSL Management</span></div>
                        <div id="pagenote">
                            This screen may be used to create a product list for all vans</div>
                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
        <ContentTemplate>
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <tr>
                                <th>
                                </th>
                            </tr>
                            <tr>
                                <td class="txtSMBold">
                                    <asp:Label ID="lblLable" runat="server" Font-Bold ="true"  Text="Please select organization :"></asp:Label>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddOraganisation" Width ="200px" AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                </td>
                                </tr> 
                                <tr>
                                <td  class="txtSMBold" colspan ="2"> 
                                    <asp:Label ID="Label1" runat="server" Font-Bold ="true"  Text="Please select a van :"></asp:Label>
                                
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                
                                    <asp:DropDownList ID="ddlSalesRep" Width ="200px" AutoPostBack="true" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                </td>
                               
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProdAvailed" Font-Bold="true" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label Font-Bold="true" ID="lblProdAssign" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="400" ID="lstDefault" runat="server">
                                                </asp:ListBox>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button align="center" Width="100" ID="btnAdd" runat="server" Text="Add -> "
                                                                CssClass="btnInputBlue" OnClick="btnAdd_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnAddAll" runat="server" Text="Add All -> " CssClass="btnInputBlue"
                                                                />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnRemove" runat="server" Text=" <- Remove" CssClass="btnInputRed"
                                                            en
                                                                OnClick="btnRemove_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnRemoveAll" runat="server" Text=" <- Remove All"  Enabled ="false" 
                                                            OnClientClick="javascript:return confirm('Would you like to remove all the assigned products?');" 
                                                                CssClass="btnInputRed" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                         
                                                        </td>
                                                    </tr>
                                                    <tr>
                                              <td>
                                    <asp:Button ID="btnImport" Visible ="false"  runat="server" CausesValidation="false" CssClass="btnInputOrange"
                                        TabIndex="2" Text="Import" Width="100" OnClick="btnImport_Click" />
                                </td>
                                </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="400" ID="lstSelected" runat="server">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                           
                        </table>
                         </ContentTemplate>
     </asp:UpdatePanel> 
                        </td> 
                        </tr> 
                        </table> 
                 <asp:UpdatePanel ID="UPModal" runat="server" >
                                <ContentTemplate>
                                         <table>
        <tr>
            <td>
                <asp:Button ID="btnImportHidden"  CssClass="btnInputBlue"  runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="ImportPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="ImportPnl" runat="server" Style="display: none" Width="350" CssClass="modalPopup">
                    <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move; background-color: #3399ff;
                        text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="345">
                        Import/ReImport Van/Item ID</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table width="340px" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td>
                                <br />
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td width="100px" class="txtSMBold">
                                Select File :
                            </td>
                            <td width="240px">
                                <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                <br />
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr valign ="top">
                            <td width="100px" class="txtSMBold">
                            </td>
                            <td width="240px" >
                                <asp:RadioButton ID="rbRebuild"  GroupName="rbg" Text="Rebuild All"  runat="server"/>
                                                                <asp:RadioButton ID="rbAppend" Text="Append"  GroupName="rbg" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td width="100px">
                            </td>
                            <td width="240px">
                                <asp:Button ID="btnImportSave"  CssClass="btnInputBlue"  TabIndex="4" CausesValidation="false"
                                    runat="server" Text="Import" />
                                <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false" />
                                <asp:Button ID="btnCancelImport" CssClass="btnInputRed" TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
   </table
    
   
    
             
           
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
                                                                                                <asp:Label ID="lblMessage"  Font-Size ="13px" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                <asp:Button ID="btnClose" runat="server" Text="Ok"  CssClass="btnInputBlue"  />
                                            </td>
                                        </tr>
                                    </table>
                                     </asp:Panel>
        </td>
        </tr>                        
                          
                    </table>
     </ContentTemplate>
     </asp:UpdatePanel> 
        
  
   
    
  
           <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpModal"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
         
</asp:Content>
