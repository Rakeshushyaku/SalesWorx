<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="SalesTargetDefinition.aspx.vb" Inherits="SalesWorx_BO.SalesTargetDefinition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" >
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
    function DisableValidation() {
        //            Page_ValidationActive = false;
        //            return true;

    }
    function NumericOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }
        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
            return true;

        return false;
    }
    function IntegerOnly(e) {

        var keycode;

        if (window.event) {
            keycode = window.event.keyCode;
        } else if (e) {
            keycode = e.which;
        }

        if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
            return true;

        return false;
    }
    </script>

   <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
                <tr>
                    <td id="contentofpage" width="100%" height="76%" class="topshadow">
                        <div class="pgtitileposition">
                            <span class="pgtitile3">FSR Target Definition</span></div>  
                   
                            <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" SelectedIndex="0"
            >
            <Tabs>
                <telerik:RadTab runat="server" Text="Target Definition" PageViewID="PageView2" Selected="true" SelectedIndex="0">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Import Target Definition" PageViewID="PageView1" >
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Export Target Definition" PageViewID="PageView3" >
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server"    Width="850" SelectedIndex="0">
            <telerik:RadPageView ID="PageView2" runat="server" BorderStyle="Groove" BorderColor="Silver" BorderWidth="1"  Selected="true" ><table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml"><tr><td style="padding: 6px 12px"><asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional"><ContentTemplate><table  border="0" cellspacing="2" cellpadding="2"><tr><td width="105" class="txtSMBold">Organization :</td><td colspan="3" ><asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold">Van :</td><td colspan="3"><asp:DropDownList ID="ddlVan" AutoPostBack="true" Width ="200px" runat="server"  CssClass="inputSM"></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold">Target Year :</td><td class="style1"><asp:DropDownList ID="ddlYear" AutoPostBack="true" Width ="200px" runat="server"  CssClass="inputSM"></asp:DropDownList></td><td width="105" class="txtSMBold">Target Month:</td><td ><asp:DropDownList ID="ddlMonth" AutoPostBack="true" Width ="200px" runat="server"  CssClass="inputSM"></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold"></td><td class="style1"><asp:DropDownList ID="ddl_ValueType" AutoPostBack="True" Width ="200px"  CssClass="inputSM" Visible="false"
                    runat="server" ><asp:ListItem Value="Q">By Quantity</asp:ListItem><asp:ListItem Value="V">By Value</asp:ListItem><asp:ListItem Value="B" Selected="True">Both</asp:ListItem></asp:DropDownList></td><td width="105" class="txtSMBold"></td><td ><asp:Button CssClass="btnInputBlue" ID="Btn_Save" runat="server" Text="Save" /><asp:HiddenField ID="HSaveClick" runat="server" /></td></tr><%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%></table></ContentTemplate></asp:UpdatePanel></td></tr><tr><td style="width:100%;padding: 6px 12px" ><asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional"><ContentTemplate><table border="0" cellspacing="0" cellpadding="0"><tr><td align="right" style="padding:0 0 10px 0;"><asp:Button CssClass="btnInputGrey" ID="Btn_SetValues" Visible="false" runat="server" Text="Load Target Value from Price List based on Qty" /></td></tr><tr><td><telerik:RadGrid ID="Grd_Products" runat="server" Skin="Vista"  allowmultirowselection="True" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="True" PageSize="25" DataKeyNames="Item_Code"  ClientDataKeyNames="Item_Code"><GroupingSettings CaseSensitive="false" /><mastertableview width="100%" summary="RadGrid table" EditMode="InPlace" AllowFilteringByColumn="true" DataKeyNames="Item_Code"  ClientDataKeyNames="Item_Code"><NoRecordsTemplate><div>There are no records to display</div></NoRecordsTemplate><Columns><telerik:GridBoundColumn  UniqueName="Item_Code" DataField="Item_Code" HeaderText ="Item Code"   AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true" ><HeaderStyle Width="150px" /></telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Description"  DataField="Description" HeaderText ="Item Name"  AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true"><HeaderStyle Width="200px" /></telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Agency"  DataField="Agency" HeaderText ="Agency"  AllowFiltering="true" ShowFilterIcon="false" AutoPostBackOnFilter="true"><HeaderStyle Width="150px" /></telerik:GridBoundColumn><telerik:GridTemplateColumn UniqueName="TgtQty" AllowFiltering="false" 
                    InitializeTemplatesFirst="false" HeaderText="Qty"><HeaderStyle Width="40px"></HeaderStyle><ItemTemplate><asp:TextBox ID="txt_qty" runat="server" Text='<%# Bind("Target_Value_1") %>' onKeypress='return IntegerOnly(event)'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle></telerik:GridTemplateColumn><telerik:GridTemplateColumn UniqueName="TgtValue"  AllowFiltering="false" 
                    InitializeTemplatesFirst="false" HeaderText="Value"><HeaderStyle Width="40px"></HeaderStyle><ItemTemplate><asp:TextBox ID="txt_value" runat="server" Text='<%# Bind("Target_Value_2") %>' onKeypress='return NumericOnly(event)'></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center"></ItemStyle></telerik:GridTemplateColumn></Columns></mastertableview><ClientSettings><Selecting AllowRowSelect="True"></Selecting></ClientSettings><pagerstyle mode="NextPrevAndNumeric"></pagerstyle></telerik:RadGrid></td></tr></table></ContentTemplate></asp:UpdatePanel></td></tr></table></telerik:RadPageView>
                                  <telerik:RadPageView ID="PageView1" runat="server" BorderStyle="Groove" BorderColor="Silver" BorderWidth="1"><div ></div><table width="auto" border="0" cellspacing="0" cellpadding="0" class="tableformNrml"><tr><td style="padding: 6px 12px"><table  border="0" cellspacing="2" cellpadding="2"><tr><td width="105" class="txtSMBold">Organization :</td><td colspan="4" ><asp:DropDownList CssClass="inputSM" ID="ddl_importOrg"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" ></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold">Target Year :</td><td class="style1"><asp:DropDownList ID="ddl_ImportYear" AutoPostBack="true" Width ="200px" runat="server" CssClass="inputSM"></asp:DropDownList></td><td width="105" class="txtSMBold">Target Month:</td><td class="txtSMBold" width="105" ><asp:DropDownList ID="ddl_ImportMonth" runat="server" CssClass="inputSM"
                        ></asp:DropDownList></td><td><asp:Button ID="Btn_Import" runat="server" CssClass="btnInputBlue" 
                       Text="Import" /></td></tr><%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%></table></td></tr></table><div></div></telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView3" runat="server" BorderStyle="Groove" BorderColor="Silver" BorderWidth="1"><div ></div><table width="auto" border="0" cellspacing="0" cellpadding="0" class="tableformNrml"><tr><td style="padding: 6px 12px"><table  border="0" cellspacing="2" cellpadding="2"><tr><td width="105" class="txtSMBold">Organization :</td><td colspan="4" ><asp:DropDownList CssClass="inputSM" ID="ddl_ExportOrg"  Width ="200px"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold">Van :</td><td colspan="4"><asp:DropDownList ID="ddl_ExportVan"  CssClass="inputSM" runat="server" 
                    Width="200px" ></asp:DropDownList></td></tr><tr><td width="105" class="txtSMBold">Target Year :</td><td class="style1"><asp:DropDownList ID="ddl_ExportYear" AutoPostBack="true" Width ="200px" runat="server" CssClass="inputSM"></asp:DropDownList></td><td width="105" class="txtSMBold">Target Month:</td><td class="txtSMBold" width="105" ><asp:DropDownList ID="ddl_ExportMonth" runat="server" CssClass="inputSM"
                        ></asp:DropDownList></td><td><asp:Button ID="Btn_Export" runat="server" CssClass="btnInputBlue" 
                       Text="Export" /></td></tr><%-- <tr> 
            <td width="105" class="txtSMBold">&nbsp;</td>
            <td>
                &nbsp;</td>
            <td width="105" class="txtSMBold">Type :</td>
            <td colspan="3">
                <asp:DropDownList CssClass="inputSM" ID="ddlType" 
                    runat="server" DataTextField="Customer_Type" DataValueField="Customer_Type">
                </asp:DropDownList>
            </td>       
          </tr> --%></table></td></tr></table><div></div></telerik:RadPageView>
                            </telerik:RadMultiPage>
         
           <div>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm"  runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;">
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
                                                             
                                           </div>
             
            <div>
                                                <asp:Button ID="BtnPricelist" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpPricelist" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnPricelist" PopupControlID="Pnl_priceList" CancelControlID="btnClosethis"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_priceList" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    
                                                    <table id="table1"   cellpadding="10" style="background-color: White;">
                                                     <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px" colspan="2">
                                                                <asp:Label ID="lbl_Title" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td colspan="2" align="center" style="color:Green;padding: 3px" >
                                                                <asp:Label ID="LabelMsg" runat="server" Font-Size ="13px" Text="The Target Value will be loaded based on the Target qunatity you entred and the price list selection."></asp:Label>
                                                                
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td align="center" style="color:Black;padding: 3px">
                                                                <asp:Label ID="Label2" runat="server" Font-Size ="13px" Text="Price List"></asp:Label>
                                                            </td>
                                                            <td align="left" style="text-align: center">
                                                                <asp:DropDownList ID="ddl_priceList" runat="server" CssClass="inputSM">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;" colspan="2">sssasa
                                                                            <asp:Button ID="Btn_applyValue" runat="server" Text="OK" CssClass="btnInputGreen" />
                                                                            <asp:Button ID="btnClosethis" runat="server" Text="Cancel" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                             
                                            </div>
               
              
                                        
                                         
                                                   <div>
                                                <asp:Button ID="BtnImportHiiden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEImport" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="BtnImportHiiden" PopupControlID="Pnl_import" CancelControlID="btnCancelImport"
                                                    Drag="true" />
                                                <asp:Panel ID="Pnl_import" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    
                                                   <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move;font-family:Verdana,Tahoma; font-weight:bold; font-size:13px;
                          background-color:  #337AB7  ;
                        text-align: center; border: solid 1px  #337AB7  ; color: White; padding: 3px"  >
                        Import FSR Target</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text ="Note: Uploading a FSR target data removes any existing target data for the month specified in excel file."></asp:Label>
                  
                  </td>
                  </tr>      
                       
		 <tr>
    <td class ="txtSMBold">Select a File :</td>
    <td> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
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
               Text ="View Log" Visible="false"></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        
                    </table>
                                                </asp:Panel>
                                             </div>
                                </ContentTemplate>
                                <Triggers>
            <asp:PostBackTrigger ControlID="Btn_Export" /> 
            <asp:PostBackTrigger ControlID="btnImport" /> 
            <asp:PostBackTrigger ControlID="lbLog" /> 
	
        </Triggers>
                            </asp:UpdatePanel>
                            
                    
             
                       </td>
                       </tr>
                       </table>
    
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="padding-left: 400px" />
                <span style="font-size: 12px; color: #666;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
 
</asp:Content>
