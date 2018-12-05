<%@ Page Title="Asset type " Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AssetType.aspx.vb" Inherits="SalesWorx_BO.AssetType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
   
        function alertCallBackFn(arg) {

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

        function TestCheckBox(event) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                    return ConfirmDelete('Would you like to delete the selected asset type(s)?', event);
                    return true;
                }
              // alert('Select at least one survey!');
            radalert('Select at least one asset type!', 330, 180, 'Validation', alertCallBackFn);
            return false;

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
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
      <h4>Manage Asset Type</h4>
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
     

                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label>Filter By</label>
                                    <div class="row">
                                        <div class="col-sm-4">
                                   <div class="form-group">  
                                    <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  AutoPostBack ="true" OnSelectedIndexChanged ="ddFilterBy_SelectedIndexChanged"
                                        Width="100%" Height="100px" TabIndex="3"
                                        runat="server">
                                        <Items>

                                            <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="AssetType ID" Text="AssetType ID"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                       
                                        </Items>
                                       </telerik:RadComboBox>
                                </div>
                                            </div>
                                        <div class="col-sm-4">
                                   <div class="form-group"> 
                                       <telerik:RadTextBox TabIndex="4" autocomplete="off" runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%">
                                        </telerik:RadTextBox>
                                       </div>
                                            </div>
                                        <div class="col-sm-4">
                                   <div class="form-group"> 
                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"  CausesValidation="False" OnClick="btnFilter_Click" TabIndex="5"   
                                            runat="server" Text="Filter" CssClass ="btn btn-primary" />
                                   
                                   
                                         <telerik:RadButton ID="Clear" runat="server" CausesValidation="False" CssClass="btn btn-default" Skin ="Simple"
                                                     TabIndex="6" Text="Reset" />

                                      <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                           
                                         <telerik:RadButton ID="btnImport" Visible ="false"  runat="server" CausesValidation="false" CssClass="btn btn-warning" Skin ="Simple"
                                                    TabIndex="2" Text="Import" OnClick="btnImport_Click" />

                                       </div>
                                    </div>
                                </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
              <div class="table-responsive">
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
                                                                    OnClientClick="return TestCheckBox(event)"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete asset type" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected asset type?',event);"  />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit asset type" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
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
                                   
                                        <telerik:RadWindow ID="MPEDetails" Title= " Asset Type Details" runat="server" Skin="Windows7" Behaviors="Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                            
                                                  <asp:UpdatePanel ID="updatepnl" runat="server" >
                                                     <ContentTemplate>
                                                         <div class="popupcontentblk">
                                                             <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                                         <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                         <asp:Panel runat="server" ID="DetailPnl">
                                                         

                                                         <asp:TextBox ID="txtAssetTypeId" Visible ="false" Enabled ="false" runat="server" TabIndex="1" CssClass="inputSM"></asp:TextBox>
                                                         <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtCurrencyCode"
                                                    ID="ReqCurrencyCode" runat="server" ErrorMessage="Currency Code Required"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                        ValidationExpression="[a-zA-Z]+" ControlToValidate="txtCurrencyCode" ID="RFV"
                                                        Display="None" runat="server" ErrorMessage="Currency Code should be in characters."></asp:RegularExpressionValidator>--%>

                                                        <div class="row">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="Label2" Width ="150px" runat="server" Text="Description*"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtDescription" TabIndex="2" CssClass="inputSM" Width ="100%" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display:none;">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="lblA1"  runat="server" Text="Attribute 1"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtParam1" TextMode ="MultiLine" Width="100%" TabIndex="3" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display:none;">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="lblA2"   runat="server" Text="Attribute 2"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtParam2" TextMode ="MultiLine"   Width="100%"  TabIndex="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display:none;">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="lblA3"  runat="server" Text="Attribute 3"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtParam3" TextMode ="MultiLine"   Width="100%"  TabIndex="5" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display:none;">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="lblA4"  runat="server" Text="Attribute 4"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtParam4"   TabIndex="6"  CssClass="inputSM" runat="server" Width="100%"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTE" runat="server" FilterType="Numbers"
                                                                            TargetControlID="txtParam4">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                      <ajaxToolkit:TextBoxWatermarkExtender ID="TBWE2" runat="server" Enabled="True" 
                                                                                            TargetControlID="txtParam4" WatermarkText="Enter numbers only">
                                                                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display:none;">
                                                            <div class="col-sm-5">
                                                                <label><asp:Label ID="lblA5"  runat="server" Text="Attribute 5"></asp:Label></label>
                                                            </div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <asp:TextBox ID="txtParam5"  Width="100%"  TabIndex="7" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTE1" runat="server" FilterType="Numbers,Custom"
                                                                            TargetControlID="txtParam5" ValidChars =".">
                                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                                          <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" Enabled="True" 
                                                                                            TargetControlID="txtParam5" WatermarkText="Enter numbers only">
                                                                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-sm-5"></div>
                                                            <div class="col-sm-7">
                                                                <div class="form-group">
                                                                    <telerik:RadButton ID="btnSave"  OnClick="btnSave_Click" runat="server" 
                                                                          CssClass="btn btn-success" Skin ="Simple"
                                                                     TabIndex="8" Text="Save" />
                                                                     <telerik:RadButton ID="btnUpdate"  OnClick="btnUpdate_Click" runat="server" 
                                                                          CssClass="btn btn-success" Skin ="Simple"
                                                                     TabIndex="9" Text="Update" />
                                                                    <telerik:RadButton ID="btnCancel" runat="server"  OnClientClick="return DisableValidation()" CausesValidation="false" 
                                                                          CssClass="btn btn-default" Skin ="Simple"
                                                                     TabIndex="10" Text="Cancel" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                            <img alt="Processing..." src="../assets/img/ajax-loader.gif" style="z-index: 10010; vertical-align: middle;" />
                                                            <span>Processing... </span>
                                                        </asp:Panel>
                                                    </asp:Panel> 
                                                     </div>
                                                 
                                                     </ContentTemplate>                                
                                                 </asp:UpdatePanel>
                                                

                                              </ContentTemplate>
                                        </telerik:RadWindow>

                                     <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                
                                      
                                </ContentTemplate>
                            </asp:UpdatePanel>
          </div>
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
                            <td colspan="2">
                                 <span style ="padding-left:5px;padding-top:10px;"><asp:Label ID="lblerr"  CssClass ="txtSMBold" runat ="server" ForeColor ="Red" ></asp:Label>
                                                        </span>
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
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>

</asp:Content>
