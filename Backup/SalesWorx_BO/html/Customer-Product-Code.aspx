<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Customer-Product-Code.aspx.vb" Inherits="SalesWorx_BO.Customer_Product_Code" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
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
                    <span class="pgtitile3">Customer Product Codes</span></div>
                    
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
             
    
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                           
                                            <td  class="txtSMBold">
                                                &nbsp;Customer: &nbsp;</td>
                                            <td class="txtSMBold" width="135">
                                                <telerik:RadComboBox ID="ddl_FilterCustomer" runat="server" width="250" AllowCustomText="false" MarkFirstMatch="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                            </td><td class="txtSMBold" width="75">
                                                &nbsp;Product:</td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddl_FilterProduct" runat="server" AllowCustomText="false" MarkFirstMatch="true" width="250">
                                                    <Items></Items>
                                                    </telerik:RadComboBox>
                                                </td>
                                                <td><asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    TabIndex="4" Text="Search" />
                                                     <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                     <asp:Button ID="BTn_Import" runat="server" CausesValidation="false" CssClass="btnInputGreen"
                                                    OnClick="btnImport_Click" TabIndex="1" Text="Import" />
                                                    </td>
                                                </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" Width="90%"  style="padding:10px">
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No customer product code."
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
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
                                                                <asp:HiddenField ID="HCustomer" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Customer_ID_1") %>'/>
                                                                <asp:HiddenField ID="HItemCode" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Item_ID_1") %>'/>
                                                                                                                              
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Configuration" ID="btnDelete" 
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/_del.gif"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected configuration?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                   
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_ID_1" HeaderText="Customer" SortExpression="Customer_ID_1">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="description" HeaderText="Product" SortExpression="description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Cust_Item_Code" HeaderText="Customer Product   Code"
                                                            SortExpression="Cust_Item_Code" HtmlEncode="false">
                                                            <ItemStyle Wrap="False" />
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
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="500" CssClass="modalPopup" Style="display: none" >
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                        <asp:Panel ID="DragPnl" runat="server" Width="491px" Font-Size ="13px" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px">
                                            Customer Product Code</asp:Panel><asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        
                                        <table width="100%" cellpadding ="2" cellspacing ="2">
                                            <tr>
                                                <td class="txtSMBold">
                                                    Customer :
                                                </td>
                                                <td>
                                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <telerik:RadComboBox ID="ddl_Customer" width="300" runat="server" AllowCustomText="false" MarkFirstMatch="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold" >
                                                    Products:
                                                </td>
                                                <td >
                                                   <telerik:RadComboBox ID="ddl_Product"  runat="server" AllowCustomText="false" MarkFirstMatch="true" width="300">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    Code:
                                                </td>
                                                <td align="left" >
                                                    <asp:TextBox ID="txt_code" width="150" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnl_File" Visible="false">
                                             <tr>
                                                <td class="txtSMBold">
                                                    File:
                                                </td>
                                                <td >
                                                    <asp:FileUpload ID="file_import" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="file_import" Display="Dynamic"  ErrorMessage="*Please upload only .xls Files" ValidationExpression="^(.)+(.xls|.XLS)$"></asp:RegularExpressionValidator>
                                                  
                                                </td>
                                            </tr>     
                                            </asp:Panel>       
                                                     </ContentTemplate>
                                                    </asp:UpdatePanel>                   
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                                <td >
                                                    <asp:Button ID="btnSave" CssClass="btnInputGreen" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                    <asp:Button ID="btnUpdate" CssClass="btnInputGreen" Text="Update" OnClick="btnUpdate_Click"
                                                        runat="server" />
                                                    <asp:Button ID="btnCancel" CssClass="btnInputRed" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                </td>
                                            </tr>
                                        </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSave" />
                                            <asp:PostBackTrigger ControlID="btnUpdate" />
                                            </Triggers>
                                                    </asp:UpdatePanel> 
                                    </asp:Panel>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
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
                                    <asp:PostBackTrigger ControlID="btnSave" />
       
     
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                
                 </ContentTemplate>
                 </asp:UpdatePanel>
                
                
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
