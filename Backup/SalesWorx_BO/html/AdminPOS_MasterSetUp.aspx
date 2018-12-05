<%@ Page Title="POSM Question Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminPOS_MasterSetUp.aspx.vb" Inherits="SalesWorx_BO.AdminPOS_MasterSetUp" %>
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
                  return confirm('Would you like to delete the Questions?');
                  return true;
               }
               alert('Select at least one Question!');
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
                    <span class="pgtitile3">POSM Question Setup</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                          
                                            
                                            
                                            <td width="75" class="txtSMBold">
                                                Filter By :
                                            </td>
                                            <td>
                                                
                                                <asp:DropDownList ID="ddQGroup" runat="server" AutoPostBack ="true" CssClass="inputSM" TabIndex="2">
                                                </asp:DropDownList>
                                                  &nbsp;
                                                  <asp:DropDownList ID="ddQuestion" runat="server"  CssClass="inputSM" TabIndex="2">
                                                </asp:DropDownList>  
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFilterVal" runat="server" ToolTip ="Enter Filter Value" 
                                                    autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3" Width="126px"></asp:TextBox>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    
                                                    <asp:Button ID="Btn_reset" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Clear Filter" />
                                                    
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
                                                <asp:GridView Width="100%" ID="gvPOSM" runat="server" EmptyDataText="No Data to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="return TestCheckBox()"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete POSM Question" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected Questions?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit POSM Question" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QGroup" HeaderText="Question Group" SortExpression="QGroup">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="QuestionValues" HeaderText="Question Value"
                                                            SortExpression="QuestionValues">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Code" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                <asp:Label ID="lbl_CodeType" runat="server" Text='<%# Bind("Code_type") %>'></asp:Label>
                                                                <asp:Label ID="lbl_GroupCode" runat="server" Text='<%# Bind("QGroupCode") %>'></asp:Label>
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
                                            POSM Question </asp:Panel>
                                                                        
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                         <table width="450px" border="0" cellspacing="2" cellpadding="2" >
                                            <tr>
                                                <td   class="txtSMBold">
                                                    Question Group :
                                                </td>
                                                <td>
                                                   <asp:DropDownList ID="ddl_QGroup" runat="server" AutoPostBack ="true" CssClass="inputSM" TabIndex="2">
                                                </asp:DropDownList>
                                               
                                                  
                                                </td>
                                                </tr>
                                                
                                                <tr>
                                                <td   class="txtSMBold">
                                                    Question:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_Question" runat="server"  CssClass="inputSM" TabIndex="2">
                                                </asp:DropDownList> 
                                                </td>
                                                </tr>
                                            <tr>
                                                <td   class="txtSMBold">
                                                    Code :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCode" TabIndex="2" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                                <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%></tr>
                                            <tr>
                                                <td  class="txtSMBold">
                                                    Description :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" TabIndex="3" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </td>
                                               <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtRate" ID="ReqRate"
                                                    runat="server" ErrorMessage="Conversion Rate Required"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ValidationExpression="[0-9]+" ControlToValidate="txtRate" ID="RegExpression" Display="None" runat="server" ErrorMessage="Conversion Rate should be in numbers."></asp:RegularExpressionValidator>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FTBtxtConvertRate" FilterType="Numbers,Custom"
                                                    ValidChars="." runat="server" TargetControlID="txtRate">
                                                </ajaxToolkit:FilteredTextBoxExtender>--%>
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
                                                     <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="5" 
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" OnClientClick="return DisableValidation()"
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
