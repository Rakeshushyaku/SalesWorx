<%@ Page Title="Assign Bonus Plan To Customer" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AssignBonusPlanToCustomer.aspx.vb" Inherits="SalesWorx_BO.AssignBonusPlanToCustomer" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            height: 43px;
        }
    </style>
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
                            <span class="pgtitile3">Assign Bonus Plan To Customer</span></div>
                        <div id="pagenote">
                            This screen may be used to assign bonus plan to customers</div>
                            <asp:UpdatePanel runat="server" ID="TopPanel" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                             <asp:PostBackTrigger  ControlID="lbLog"   />
                              
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>    
        <ContentTemplate>
        <div style="float:right;">
          <asp:Button  ID="btnBack" runat="server" Text="Go Back" CssClass="btnInputGrey"
                                                          
                                                                 />
                                                                 
                                                                   <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass="btnInputOrange" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btnInputGreen" Text="Import" TabIndex ="12" />                                   
        </div> 
            
                        <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
                            <tr>
                                <th>
                                </th>
                            </tr>
                             <tr>
                             <td class="txtSMBold">
                             Organization * :
                             </td>
                                <td >
                                    
                                    <asp:DropDownList ID="ddOraganisation" AutoPostBack="true"  Width ="180px" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                </td>
                                </tr> 
                               
                            <tr valign ="top" >
                               <td class="txtSMBold">
                              Bonus Plan * :
                             </td>
                                <td class="txtSMBold" >
                                   
                                  
                                    <asp:DropDownList ID="ddlBonusPlan" AutoPostBack="true" Width ="180px" runat="server" CssClass="inputSM">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblnote" runat ="server" Text ="Note: Assigning a bonus plan to the customer removes any existing plan assigned to the same customer." Font-Bold ="true" ForeColor ="#0090d9"></asp:Label>
                                </td>
                                </tr> 
                                 <tr>
                                  <td class="txtSMBold" >Filter By Cust.No</td>
                              <td >
                                  
                                 <asp:TextBox  runat ="server" ID="txtFilter"  Width ="180px"  CssClass="inputSM"></asp:TextBox>
                                    <asp:Button ID="btnFilter" runat="server" Text="Filter"
                                                               CssClass="btnInputGreen"  />
                                                                   <asp:Button  ID="btnReset" runat="server" Text="Reset" CssClass="btnInputRed"
                                                          
                                                                 />
                                </td>
                            
                             </tr>
                            <tr>
                                <td colspan ="2">
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
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="350" ID="lstDefault" runat="server">
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
                                                          
                                                                OnClick="btnRemove_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button Width="100" ID="btnRemoveAll" runat="server" Text=" <- Remove All"  Enabled ="false" 
                                                            OnClientClick="javascript:return confirm('Would you like to remove all the assigned customers?');" 
                                                                CssClass="btnInputRed" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                         
                                                        </td>
                                                    </tr>
                                                    <tr>
                                              <td>
                                   
                                </td>
                                </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:ListBox Rows="20" SelectionMode="Multiple" Width="350" ID="lstSelected" runat="server">
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
                                                 <asp:Label ID="lblinfo" runat="server" Font-Size ="14px" Font-Bold ="true" Font-Names ="verdana"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center">
                                                                                               <asp:Label ID="lblMessage" runat="server"  Font-Size ="13px" Font-Names ="verdana" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInput" />
                                            </td>
                                        </tr>
                                    </table>
                                     </asp:Panel>
        </td>
        </tr>                        
                          
                    </table>
                    
                                               <div>
                                                <asp:Button ID="BtnImportHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEImport" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnImportHidden" PopupControlID="Pnl_import" CancelControlID="btnCancelImport"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_import" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    
                                                   <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move;font-family:Verdana,Tahoma; font-weight:bold; font-size:13px;
                          background-color:  #337AB7  ;
                        text-align: center; border: solid 1px  #337AB7  ; color: white; padding: 3px"  >
                        Import Customer Bonus Plan</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2"  width="100%">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold" >Select a File :</td>
    <td style ="color:Black;"> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
          <td>
           
             
          </td>
          <td>
           <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btnInputGrey" /> <asp:Button ID="btnCancelImport" CssClass="btnInputRed" TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass="btnInputBlue" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	   <asp:PostBackTrigger ControlID="btnImport" /> 
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                       
                         <asp:GridView Width="100%" ID="dgvErros"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" Visible="false" 
                                                        AllowPaging="true" AllowSorting="false"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                        
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                        
                         
                        </td>
                        </tr>
                    </table>
                                                </asp:Panel>
                                             </div>
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
