<%@ Page Title="Reason Code Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="ReasonCodes.aspx.vb" Inherits="SalesWorx_BO.ReasonCodes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">

        //        var _source;
        //        var _popup;
        //        function showConfirm(source) {
        //            this._source = source;
        //            this._popup = $find('<%=MPEReason.ClientID%>');
        //            this._popup.show();
        //            
        //        }

        //        function CancelClick() {
        //            this._popup.hide();
        //            this._popup = null;
        //            this._source = null;
        //        }


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
                return confirm('Would you like to delete the selected reason code?');
                return true;
            }
            alert('Select at least one Reason Code!');
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
//            }
        }

        function DisableValidation() {
//            Page_ValidationActive = false;
//            return true;

        }
        </script>
<script language="javascript" type="text/javascript">
      
    
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
                var myRegExp1 = /btnSaveAcc/
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
                var myRegExp1 = /btnSaveAcc/
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
                    <span class="pgtitile3">Reason Code Management</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                                        <tr>
                                          
                                            <td width="75" class="txtSMBold">
                                                Filter By :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddFilterBy_SelectedIndexChanged"
                                                    CssClass="inputSM" TabIndex="3">
                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                    <asp:ListItem Value="Reason Code">Reason Code</asp:ListItem>
                                                    <asp:ListItem Value="Description">Description</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterVal" runat="server" ToolTip="Enter Filter Value" autocomplete="off"
                                                    CssClass="inputSM" TabIndex="4"></asp:TextBox>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="5" Text="Filter" />
                                                     <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                     <asp:Button ID="btnImport" Visible ="false"  runat="server" CausesValidation="false" CssClass="btnInputOrange"
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
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvReasonCode" runat="server" EmptyDataText="No Items to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" TextAlign="Left" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" Text=" " />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="return TestCheckBox()"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  />
                                                                <asp:ImageButton ToolTip="Delete Reason Code" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected reason code?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Reason Code" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Reason_Code") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Reason_Code" HeaderText="Reason Code" SortExpression="Reason_Code">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReason" runat="server" Text='<%# Bind("Reason_Code") %>'></asp:Label>
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
                                    <asp:Button ID="btnHiddenReason" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEReason"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenReason" CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Style="display: none" Width="350" CssClass="modalPopup">
                                        <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="345">
                                            Reason Code Details</asp:Panel>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <table width="100%" border="0" cellspacing="2" cellpadding="2">
                                            <tr>
                                                <td width="75" class="txtSMBold">
                                                    Code :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReasonCode" Width="180px" runat="server" TabIndex="1" CssClass="inputSM"></asp:TextBox>
                                                </td>
                                                <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtReasonCode"
                                                    ID="ReqReasonCode" runat="server" ErrorMessage="Reason Code Required"> </asp:RequiredFieldValidator>--%>
                                            </tr>
                                            <tr>
                                                <td width="75" class="txtSMBold">
                                                    Description :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" TabIndex="2" Width="180px" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                                <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%></tr>
                                            <tr>
                                                <td width="75" class="txtSMBold">
                                                    Purpose :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPurpose" Width="180px" TabIndex="3" runat="server" CssClass="inputSM"
                                                        AppendDataBoundItems="true">
                                                        <asp:ListItem Selected="True" Value="">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--<asp:CompareValidator ID="CVddlPurpose" runat="server" ErrorMessage="Select Purpose"
                                                        Display="None" Text=" " Operator="NotEqual" ValueToCompare="--Select--" ControlToValidate="ddlPurpose"></asp:CompareValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan ="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                   <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="4"
                                                         runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" 
                                                        runat="server" Text="Update" OnClick="btnUpdate_Click" />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="5" 
                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                      
                                    </asp:Panel>
                                </ContentTemplate>
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
    <table>
        <tr>
            <td>
                <asp:Button ID="btnImportHidden" CssClass="btnInput" runat="Server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="ImportPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="ImportPnl" runat="server" Style="display: none" Width="350" CssClass="modalPopup">
                    <asp:Panel ID="Dragpnl2" runat="server" Style="cursor: move; background-color: #3399ff;
                        text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="345">
                        Import/ReImport Reason Code Details</asp:Panel>
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
                            <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="ExcelFileUpload"
                                ID="RFV" runat="server" ErrorMessage="Select File"> </asp:RequiredFieldValidator>--%>
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
                            <td width="100px" class="txtSMBold">
                                Purpose :
                            </td>
                            <td width="240px">
                                <asp:DropDownList ID="ddlIPurpose" TabIndex="3" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True" Value="">--Select--</asp:ListItem>
                                </asp:DropDownList>
                                <%--    <asp:CompareValidator ID="CV1" runat="server" ErrorMessage="Select Purpose" Display="None"
                                    Text=" " Operator="NotEqual" ValueToCompare="--Select--" ControlToValidate="ddlIPurpose"></asp:CompareValidator>--%>
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
                            <td width="100px" class="txtSMBold">
                            </td>
                            <td width="240px" valign="top">
                                <asp:RadioButton ID="rbRebuild" GroupName="rbg" Text="Rebuild All" runat="server" />
                                <asp:RadioButton ID="rbAppend" Text="Update" GroupName="rbg" runat="server" />
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
                                <asp:Button ID="btnImportSave" CssClass="btnInput" TabIndex="4" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                <asp:Button ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false"
                                    OnClientClick="return DisableValidation()" />
                                <asp:Button ID="btnCancelImport" CssClass="btnInput" TabIndex="5" OnClientClick="return DisableValidation()"
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
    <%--  <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpPanel"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
</asp:Content>
