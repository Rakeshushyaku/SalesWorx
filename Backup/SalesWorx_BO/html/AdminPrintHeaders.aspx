<%@ Page Title="Invoice Print Headers" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminPrintHeaders.aspx.vb" Inherits="SalesWorx_BO.AdminPrintHeaders" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    .imageContainer {
    float: left; 
    margin: 5px; 
    padding: 2px; 
    position: relative; 
    background: #eeeeee;
}
 
    .imageContainer:hover {
        background-color: #a1da29 !important;
    }
 
.buttonsWrapper {
    display: inline-block;
    vertical-align:middle;
}
 
.image {
    cursor: pointer; 
    display: block;
}
 
.txt {
    border: 0px !important;
    background: #eeeeee !important;
    color: Black !important;
    margin-left: 25%;
    margin-right: auto;
    width: 100%;
    filter: alpha(opacity=50); /* IE's opacity*/
    opacity: 0.50;
    text-align: center;
}
 
#list {
    max-width: 900px;
}
 
.clearFix {
    clear: both;
}
 
.demo-container {
    max-width: 856px;
}
 
.sliderWrapper {
    float:left; 
    display:inline-block;
}
#ctl00_ContentPlaceHolder1_grdCustomerSegment td
{
	    white-space: normal !important;
}
</style>
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
                return confirm('Would you like to delete the print header?');
                return true;
            }
            
            alert('Select at least one print header!');
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
                    <span class="pgtitile3">Invoice Print Headers</span></div>
                <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
                    <tr>
                        <td style="padding: 6px 12px">
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                          
                                            <td width="75" class="txtSMBold">
                                               Type:
                                            </td>
                                           <td width="75" class="txtSMBold" >
                                                <asp:DropDownList ID="ddl_Type" runat="server" CssClass="inputSM" AutoPostBack="true" >
                                              
                                                </asp:DropDownList>

                                            </td>
                                           
                                            
                                            <td width="75" class="txtSMBold">
                                                Filter value :
                                            </td>
                                           
                                            <td>
                                                
                                                <asp:TextBox ID="txtFilterVal"  runat="server" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3" ></asp:TextBox>
                                                    
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btnInputGrey"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    <asp:Button ID="btnclearFilter" runat="server" CausesValidation="false" CssClass="btnInputBlue"
                                                     OnClick="btnclearFilter_Click" TabIndex="1" Text="Clear Filter" />
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btnInputGrey"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                     
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100%;padding: 6px 12px" >
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <asp:Panel ID="pnl_client" runat="server">
                                    <table border="0" cellspacing="0" cellpadding="0">
                                    
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="grdCustomerSegment" runat="server" EmptyDataText="No Data to show"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" 
                                                    CssClass="tablecellalign">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                           
                                                            <ItemTemplate>
                                                               
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete this print header?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                             <ItemStyle Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Info_Key" HeaderText="" SortExpression="Info_Key">
                                                            <ItemStyle Wrap="False" />
                                                             <ItemStyle Width="250px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_5" HeaderText="Header Line 1" HtmlEncode="false" >
                                                            <ItemStyle Wrap="False" />
                                                             <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_4" HeaderText="Header Line 2" HtmlEncode="false">
                                                            <ItemStyle Wrap="False" />
                                                             <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_3" HeaderText="Header Line 3" HtmlEncode="false">
                                                            <ItemStyle Wrap="False" />
                                                             <ItemStyle Width="200px" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRow_ID" runat="server" Text='<%# Bind("Row_ID") %>'></asp:Label>
                                                                <asp:Label ID="hlogo" runat="server" Text='<%# Bind("Value_1") %>'></asp:Label>
                                                                <asp:Label ID="vLogo" runat="server" Text='<%# Bind("Logo") %>'></asp:Label>
                                                                <asp:Label ID="lbl_key" runat="server" Text='<%# Bind("key") %>'></asp:Label>
                                                                <asp:Label ID="vLogo4inch" runat="server" Text='<%# Bind("logo4") %>'></asp:Label>
                                                                 <asp:Label ID="hlogo4inch" runat="server" Text='<%# Bind("Value_6") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Logo">
                                            <ItemTemplate>
                                                 
                                                            
                                                <asp:ImageButton ID="imgLogo" runat ="server"  ImageUrl='<%#Eval("Logo") %>'  Width ="250px" Height ="90px"
                                               />
                                              
                      
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
                                 </asp:Panel>
                               
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPECurrency"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Style="display: none" CssClass="modalPopup">
                                      <asp:Panel ID="DragPnl" Font-Size ="13px"  runat="server" Style="cursor: move; background-color: #3399ff;
                                            text-align: center; border: solid 1px #3399ff; color: White; padding: 3px" Width="620">
                                            <asp:Label runat="server" ID="lbl_Title" Width="620"></asp:Label> </asp:Panel>
                                                                        
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                         <table  border="0" cellspacing="2" cellpadding="2" width="100%" >
                                          <tr>
                                                <td   class="txtSMBold" >
                                                    <asp:Label runat="server" ID="lbl_Info_Key"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_Client" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150"
                                                        ></asp:DropDownList>
                                                </td>
                                              </tr>
                                              <asp:Panel  id="txt" runat="server">
                                            <tr>
                                                <td   class="txtSMBold">
                                                    Line1 :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Line1" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="50"
                                                        ></asp:TextBox>
                                                </td>
                                              </tr> 
                                               <tr>
                                                <td   class="txtSMBold">
                                                    Line2 :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Line2" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150"
                                                        ></asp:TextBox>
                                                </td>
                                              </tr>
                                               <tr>
                                                <td   class="txtSMBold">
                                                    Line3 :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Line3" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150"
                                                        ></asp:TextBox>
                                                </td>
                                              </tr>    
                                              </asp:Panel> 
                                               <asp:Panel  id="img" runat="server">
                                                 <tr>
                                                <td   class="txtSMBold" valign="top"> 
                                                    Logo (A4 Printer):(image of 1960px x 277px)
                                                </td>
                                                <td>
                                                 <asp:Label runat="server" ID="Label1" Text="Keep the same file"></asp:Label> 
                                                    
                                                    <asp:RadioButtonList ID="rdo_keepSamefile" runat="server"  Visible="false">
                                                    <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:FileUpload  class="file" id="fUpload"  runat="server"/>
                                                    <br /><br />
                                                    <asp:Image ID="img_Logo" runat="server" Visible="false"  Width ="400px" Height ="90px" />
                                                    <asp:Label runat="server" ID="lbl_logo2"></asp:Label> 
                                                    <asp:HiddenField runat="Server" ID="hImgFile"></asp:HiddenField> 
                                                </td>
                                              </tr> 
                                              <tr>
                                                <td   class="txtSMBold" valign="top"> 
                                                    Logo(4Inch Printer) :(image of 825px x 285px)
                                                </td>
                                                <td>
                                                                                                    
                                                   
                                                    <asp:FileUpload  class="file" id="fUpload4inc"  runat="server"/>
                                                    <br /><br />
                                                    <asp:Image ID="img_Logo4inc" runat="server" Visible="false"  Width ="400px" Height ="90px" />
                                                    <asp:Label runat="server" ID="lbl_logo24inc"></asp:Label> 
                                                    <asp:HiddenField runat="Server" ID="hImgFile4inc"></asp:HiddenField> 
                                                </td>
                                              </tr> 
                                               </asp:Panel>
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
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;width:100%"">
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
                                 
                                  <asp:PostBackTrigger ControlID="btnSave" />
                                     <asp:PostBackTrigger ControlID="btnUpdate" />
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

