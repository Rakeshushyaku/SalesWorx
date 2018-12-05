<%@ Page Title="Asset type " Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="AssetType.aspx.vb" Inherits="SalesWorx_BO.AssetType" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
          var TargetChildControl = "chkDelete";
             var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to delete the selected asset type?');
                  return true;
               }
        alert('Select at least one asset type!');
                return false;

          }

        function CheckAll(cbSelectAll) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                Inputs[n].checked = cbSelectAll.checked;
            }

        }


        function Validate() {
//            Page_ClientValidate();
//            if (!Page_IsValid) {
//                $find('<%=MpInfoError.ClientID%>').show();
//                var Info = document.getElementById('<%=lblinfo.ClientID%>');
//                Info.innerHTML = "Validation";
//                document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
//                return false;
            }
////        }

        function DisableValidation() {
//            Page_ValidationActive = false;
//            return true;

        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();

            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                if (AddString != -1 || EditString != -1) {
                    $get('<%= Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
                }
                postBackElement.disabled = true;
            }
        }


        function EndRequest(sender, args) {
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp = /_btnUpdate/;
                var myRegExp1 = /btnSave/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }
   
    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Manage Asset Type</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                            
                                            <td width="75" class="txtSMBold">
                                                Filter By :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="ddFilterBy_SelectedIndexChanged" CssClass="inputSM" TabIndex="3">
                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                    <asp:ListItem Value="AssetType ID">AssetType ID</asp:ListItem>
                                                    <asp:ListItem Value="Description">Description</asp:ListItem>
                                                </asp:DropDownList>  <asp:TextBox ID="txtFilterVal" runat="server" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="4"></asp:TextBox>
                                       
                                              
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="5" Text="Filter" />
                                                      <asp:Button ID="Clear" runat="server" CausesValidation="False" CssClass="btnInputRed"
                                                     TabIndex="6" Text="Reset" />
                                                          <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                     <asp:Button ID="btnImport" Visible ="false"  runat="server" CausesValidation="false" CssClass="btnInput"
                                                    TabIndex="2" Text="Import" OnClick="btnImport_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" width="99%" align="center" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvCurrency" runat="server" EmptyDataText="No asset types to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                  
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false"  ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="return TestCheckBox()"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete asset type" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected asset type?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit asset type" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Asset_type_id" HeaderText="AssetType ID" SortExpression="Asset_type_id">
                                                            <ItemStyle Wrap="False" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Last_Modified_At" DataFormatString ="{0:dd-MM-yyyy HH:mm}" HeaderText="Last Modified On" SortExpression="Last_Modified_At">
                                                            <ItemStyle Wrap="False" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                       <asp:BoundField DataField="UserName" HeaderText="Last Modified By" SortExpression="UserName">
                                                            <ItemStyle Wrap="False" />
                                                               <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Asset_type_id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Param1" runat="server" Text='<%# Bind("Custom_Attribute_1") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Param2" runat="server" Text='<%# Bind("Custom_Attribute_2") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Param3" runat="server" Text='<%# Bind("Custom_Attribute_3") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Param4" runat="server" Text='<%# Bind("Custom_Attribute_4") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Param5" runat="server" Text='<%# Bind("Custom_Attribute_5") %>'></asp:Label>
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
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPECurrency"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                   <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="350" CssClass="modalPopup">
                                      <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="345">
                                           Asset Type Details</asp:Panel>
                                                                        
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <table >
                                            <tr>
                                               
                                                <td colspan ="2">
                                                    <asp:TextBox ID="txtAssetTypeId" Visible ="false" Enabled ="false" runat="server" TabIndex="1" CssClass="inputSM"
                                                        ></asp:TextBox>
                                                </td>
                                                <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtCurrencyCode"
                                                    ID="ReqCurrencyCode" runat="server" ErrorMessage="Currency Code Required"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                        ValidationExpression="[a-zA-Z]+" ControlToValidate="txtCurrencyCode" ID="RFV"
                                                        Display="None" runat="server" ErrorMessage="Currency Code should be in characters."></asp:RegularExpressionValidator>--%></tr>
                                            <tr>
                                                <td   class="txtSMBold">
                                                   <asp:Label ID="Label2"  runat="server" Text="Description*:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" TabIndex="2" CssClass="inputSM" Width ="250px" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               
                                                <tr  style ="display:none;" >
                                                <td   class="txtSMBold">
                                                    <asp:Label ID="lblA1"  runat="server" Text="Attribute1:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParam1" TextMode ="MultiLine" Width="300px" TabIndex="3" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               <tr style ="display:none;" >
                                                <td   class="txtSMBold">
                                                    <asp:Label ID="lblA2"   runat="server" Text="Attribute2:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParam2" TextMode ="MultiLine"   Width="300px"  TabIndex="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               
                                               <tr style ="display:none;" >
                                                <td   class="txtSMBold"> 
                                                    <asp:Label ID="lblA3"  runat="server" Text="Attribute3:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParam3" TextMode ="MultiLine"   Width="300px"  TabIndex="5" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               </tr>
                                               
                                               
                                               <tr style ="display:none;">
                                                <td   class="txtSMBold">
                                                    <asp:Label ID="lblA4"  runat="server" Text="Attribute4:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParam4"   TabIndex="6"  CssClass="inputSM" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTE" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtParam4">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                  <ajaxToolkit:TextBoxWatermarkExtender ID="TBWE2" runat="server" Enabled="True" 
                                                                        TargetControlID="txtParam4" WatermarkText="Enter numbers only">
                                                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                                </td>
                                               </tr>
                                               
                                               
                                               <tr style ="display:none;" >
                                                <td   class="txtSMBold">
                                                    <asp:Label ID="lblA5"  runat="server" Text="Attribute5:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParam5"     TabIndex="7" CssClass="inputSM" runat="server"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Numbers,Custom"
                                                        TargetControlID="txtParam5" ValidChars =".">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                      <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="True" 
                                                                        TargetControlID="txtParam5" WatermarkText="Enter numbers only">
                                                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                          
                                                </td>
                                                
                                               </tr>
                                               
                                                
                                            <tr>
                                                <td colspan ="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #e10000;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                          <td></td>
                                                <td >
                                                     <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="8" 
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="9" OnClientClick="return DisableValidation()"
                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                
                                        <table>
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
                        </td>
                    </tr>
                </table>
                
                   <table>
        <tr>
            <td>
                <asp:Button ID="btnImportHidden" CssClass="btnInput" runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="ImportPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="ImportPnl" runat="server" Style="display: none" CssClass="modalPopup">
                    <asp:Panel ID="Dragpnl2" runat="server" CssClass="screen">
                        Import Asset Type Details</asp:Panel>
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table style="background:#ffffff" cellspacing="0" cellpadding="0">

                        <tr>
                            <td width="100px" class="txtSMBold">
                                Select File :
                            </td>
                            <td width="240px">
                                <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                            </td>
                            <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="ExcelFileUpload"
                                ID="RFV" runat="server" ErrorMessage="Select File"> </asp:RequiredFieldValidator>--%>
                        </tr>
                       

                      <%--  <tr>
                            <td width="100px" class="txtSMBold">
                            </td>
                            <td width="240px" valign="top">
                                <asp:RadioButton ID="rbRebuild" GroupName="rbg" Text="Rebuild All" runat="server" />
                                <asp:RadioButton ID="rbAppend" Text="Update" GroupName="rbg" runat="server" />
                            </td>
                        </tr>--%>

                        <tr>
                            <td width="100px">
                            </td>
                            <td width="240px">
                                <asp:Button ID="btnImportSave" CssClass="btnInput" TabIndex="1" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" />
                                <asp:Button ID="btnCancelImport" CssClass="btnInput" TabIndex="2" OnClientClick="return DisableValidation()"
                                    runat="server" CausesValidation="false" Text="Cancel" />
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
    </table>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
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
