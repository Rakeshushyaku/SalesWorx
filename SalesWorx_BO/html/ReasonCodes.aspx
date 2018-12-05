<%@ Page Title="Reason Code Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ReasonCodes.aspx.vb" Inherits="SalesWorx_BO.ReasonCodes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
<script language="javascript" type="text/javascript">


    var TargetBaseControl = null;

    window.onload = function () {
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
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
                if (AddString != -1 || EditString != -1) {

                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
                if (cancelString != -1) {
                    HideRadWindow();
                }
            }
        }



        function ConfirmDelete(msg, event) {

            var ev = event ? event : window.event;
            var callerObj = ev.srcElement ? ev.srcElement : ev.target;
            var callbackFunctionConfirmDelete = function (arg, ev) {
                if (arg) {
                    callerObj["onclick"] = "";
                    if (callerObj.click) callerObj.click();
                    else if (callerObj.tagName == "A") {
                        try {
                            eval(callerObj.href)
                        }
                        catch (e) { }
                    }
                }
            }
            radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
            return false;
        }

        function RadConfirm(sender, args) {
            var callBackFunction = Function.createDelegate(sender, function (shouldSubmit) {
                if (shouldSubmit) {
                    this.click();
                }
            });

            var text = "Would you like to release this Collection?";
            radconfirm(text, callBackFunction, 350, 150, null, "Confirmation");
            args.set_cancel(true);
        }


        function alertCallBackFn(arg) {
            HideRadWindow()
        }

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
               // $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }
    </script>
    <script type="text/javascript">
        $(window).resize(function () {
            var win = $find('<%= MPEDetails.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
      </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <h4>Reason Code Management</h4>
                <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
   
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label> Filter By </label>
                                    <div class="row">
                                        <div class="col-sm-4">
                                    <div class="form-group">  
                                              <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Reason Code" Text="Reason Code"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                       
                    </Items>
                </telerik:RadComboBox>
                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group"> 
                                                 <telerik:RadTextBox runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%"></telerik:RadTextBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-4">
                                                 <div class="form-group">   
                                              
                                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />

                                            <asp:Button ID="btnImport" runat="server" Visible ="false"  CausesValidation="false" CssClass="btnInputGreen"
                                                    TabIndex="2" Text="Import" OnClick="btnImport_Click" />
                                                     </div>
                                                </div>
                                 
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                   
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
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  />
                                                                <asp:ImageButton ToolTip="Delete Reason Code" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected reason code?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Reason Code" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Reason_Code") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
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
                                           
                                    <telerik:RadWindow ID="MPEDetails" Title= "Reason Code Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true"  ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                                   <p><asp:Label ID="lblPop" runat ="server" ForeColor ="Red" ></asp:Label></p>

                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Code</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtReasonCode" runat="server" TabIndex="1" Width="100%"></asp:TextBox>
                                                
                                                <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtReasonCode"
                                                    ID="ReqReasonCode" runat="server" ErrorMessage="Reason Code Required"> </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Description</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtDescription" runat="server" TabIndex="2" Width="100%"></asp:TextBox>
                                                
                                                <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Purpose</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox id="ddl_purpose" runat="server" TabIndex="3" Width="100%" Skin="Simple"  ></telerik:RadComboBox>
                                                    <%--<asp:CompareValidator ID="CVddlPurpose" runat="server" ErrorMessage="Select Purpose"
                                                        Display="None" Text=" " Operator="NotEqual" ValueToCompare="--Select--" ControlToValidate="ddlPurpose"></asp:CompareValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-5"></div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadButton ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click" Skin="Simple" TabIndex="6" Text="Save">
                                                    </telerik:RadButton>
                                                    <telerik:RadButton ID="btnUpdate" runat="server" CssClass="btn btn-success" OnClick="btnUpdate_Click" Skin="Simple" TabIndex="6" Text="Update">
                                                    </telerik:RadButton>
                                                      <telerik:RadButton ID="btnCancel" Skin="Simple"  CssClass ="btn btn-default" 
                                                        OnClick="btnCancel_Click"
                                                            runat="server" Text="Cancel" TabIndex ="6" >
                                      </telerik:RadButton>
                                                </div>
                                            </div>
                                        </div>
                                        
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif"  />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                               
                                        </div>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                              </ContentTemplate>
                                                  </telerik:RadWindow> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>   
                     
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span >Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
           
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
    
</asp:Content>
