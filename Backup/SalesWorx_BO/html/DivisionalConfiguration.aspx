<%@ Page Title="Sales Org Configuration" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master"
    CodeBehind="DivisionalConfiguration.aspx.vb" Inherits="SalesWorx_BO.DivisionalConfiguration" %>

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
                return confirm('Would you like to delete the selected configurations?');
                return true;
            }
            alert('Select at least one configurations!');
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


        //     function Validate() {
        //         Page_ClientValidate();
        //         if (!Page_IsValid) {
        //             $find('<%=MpInfoError.ClientID%>').show();
        //             var Info = document.getElementById('<%=lblinfo.ClientID%>');
        //             Info.innerHTML = "Validation";
        //             document.getElementById('<%=lblMessage.ClientID%>').innerHTML = '';
        //             return false;
        //         }
        //     }

        //     function DisableValidation() {
        //         Page_ValidationActive = false;
        //         return true;

        //     }
    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
        <tr>
            <td id="contentofpage" width="100%" height="76%" class="topshadow">
                <div class="pgtitileposition">
                    <span class="pgtitile3">Sales Org Configuration</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                           
                                            <td class="txtSMBold" width="75">
                                              Organization:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFilterBy" runat="server" AutoPostBack="true" CssClass="inputSM"
                                                    TabIndex="2">
                                                </asp:DropDownList>
                                          
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Search" />
                                                      <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
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
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Configuration details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"   PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/_del.gif"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Configuration" ID="btnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/_del.gif"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected configuration?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Org_ID") %>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Description" HeaderText="Organization Name" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Allow_Manual_FOC" HeaderText="Allow Manual FOC" SortExpression="Allow_Manual_FOC">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Odo_Reading_At_Visit" HeaderText="Odometer Reading At Visit"
                                                            SortExpression="Odo_Reading_At_Visit">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Advance_PDC_Posting" HeaderText="Advance PDC Posting "
                                                            SortExpression="Advance_PDC_Posting">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Collection_OutputFolder" HeaderText="Collection Output Folder"
                                                            SortExpression="Collection_Output_Folder">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField DataField="CN_LIMIT" HeaderText="Credit Note Limit"
                                                            SortExpression="CN_LIMIT">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       <%-- <asp:BoundField DataField="Print_Format" HeaderText="Print Format"
                                                            SortExpression="Print_Format">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>--%>
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
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="500" CssClass="modalPopup" Style="display: none" >
                                        <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Width="491px" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                            Sales Org Details</asp:Panel>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        <table width="100%" cellpadding ="2" cellspacing ="2">
                                            <tr>
                                                <td class="txtSMBold">
                                                    Organization :
                                                </td>
                                                <td>
                                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <asp:DropDownList ID="drpOrganization" TabIndex ="1" Width="150px" runat="server" CssClass="inputSM" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold" cssclass="inputSM">
                                                    Allow Manual FOC:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkFOC" TabIndex ="2" runat="server" CssClass="inputSM" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Odometer Reading At Visit:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkOdo" TabIndex ="3" runat="server" CssClass="inputSM" TextAlign="Left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Advance PDC Posting:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPDCPosting" Width="150px" TabIndex ="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                    <asp:Label ID="Label1" runat="server" Text="(days)"></asp:Label>
                                                    <ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FT" runat="server" TargetControlID="txtPDCPosting" FilterType="Numbers">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Collection Output Folder:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCollectionOutputFolder" TabIndex ="5"  Width="150px" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Credit Note Limit:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCRLimit" Width="150px" TabIndex ="6" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                                                      <ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtCRLimit" FilterType="Numbers">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                           <%-- <tr>
                                            <td class="txtSMBold">
                                            Print Format
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbStandard" runat="server"  Text ="Standard" GroupName ="rb" CssClass="inputSM" />
                                                <asp:RadioButton ID="rbContinue" runat="server" Text ="Continuous" GroupName ="rb" CssClass="inputSM" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td class="txtSMBold">
                                                   Is DC Optional? :
                                                    <br />
                                                     <asp:Label ID="Label2" runat="server" Font-Bold ="false"  Text="(select to make distribution check optional for a van)"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upVan" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="Panel1" runat="server" Height="143px" ScrollBars="Auto" BorderStyle="Groove"
                                                                BorderWidth="1px" Width="300px" CssClass="inputSM">
                                                                <asp:CheckBoxList ID="chkSalesRep" runat="server" TabIndex ="7" RepeatColumns="2" Font-Bold="False"
                                                                    CellPadding="2" CellSpacing="4">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="drpOrganization" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
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
                                            <td></td>
                                                <td >
                                                    <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="7" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" Text="Update" OnClick="btnUpdate_Click"
                                                        runat="server" />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="8" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
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
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                </Triggers>
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
</asp:Content>
