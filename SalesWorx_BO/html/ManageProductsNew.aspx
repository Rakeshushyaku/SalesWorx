<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageProductsNew.aspx.vb" Inherits="SalesWorx_BO.ManageProductsNew" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
    <style type="text/css">
        .RadGrid .rgFilterRow{
           display:none;
    }
        .popupcontentblkwider {
            padding: 25px 15px 15px;
            position: relative;
            width: 750px;
            overflow-y: auto !important;
                height: calc(100vh - 145px);
        }

            .popupcontentblkwider > p,
            .popupcontenterror {
                display: block;
                font-size: 13px;
                left: 15px;
                margin: 0;
                position: absolute;
                top: 40px;
            }

            .popupcontentblkwider label {
                padding: 5px 0 0;
            }

            .popupcontentblkwider .form-group {
                margin-bottom: 10px;
            }

                .popupcontentblkwider .form-group .inputSM input[type="radio"],
                .popupcontentblkwider .form-group .inputSM input[type="checkbox"] {
                    margin-top: 8px;
                }

        .style1 {
            color: #000000;
            text-decoration: none;
            font-weight: bold;
            font-style: normal;
            font-variant: normal;
            font-size: 12px;
            line-height: normal;
            font-family: Calibri;
            height: 37px;
        }

        .style2 {
            height: 37px;
        }

        #ctl00_ContentPlaceHolder1_Panel {
            margin: 15px;
            padding: 10px;
            background: #fff;
        }

        .RadUpload .ruBrowse {
            width: auto;
            padding: 0 10px;
        }

        .ruFileInput {
            cursor: pointer;
        }
        div[id^='RadWindowWrapper_alert'].RadWindow_Simple.rwShadow,
        div[id^='RadWindowWrapper_confirm'].RadWindow_Simple.rwShadow {
            z-index:6006 !important;
        }
        #ctl00_MainContent_DocWindow_C {
            overflow:hidden !important;
        }
    </style>

    <script type="text/javascript">


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
                    return confirm('Would you like to delete the selected currency code?');
                    return true;
                }
            alert('Select at least one Currency Code!');
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
                if (AddString != -1 && EditString != -1) {                   
                }
                else {                 
                    $get('<%= Me.UpdateProgress1.ClientID%>').style.display = 'block';
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
                $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }


      
        function OnddlOrgClientTextChange(sender, eventArgs) {
            if (sender.get_text() != 'Select Organization')
            {           
                var txt = $find("<%= txtOrgID.ClientID%>");
                document.getElementById('ctl00_MainContent_DocWindow_C_txtOrgID').value = sender.get_text();    
             
            }
            else
                document.getElementById('ctl00_MainContent_DocWindow_C_txtOrgID').value = '';
                
       }
      


        function onPrimaryUOMChange(sender, args)
        {
            if (args.get_checked()) {
                var radNumRate = $find("<%= radNumRate.ClientID %>");
                radNumRate.set_value(1);
                $find('<%= radNumRate.ClientID%>').disable(); 
            }
            else {
                $find('<%= radNumRate.ClientID%>').enable(); 
            }

            ValidateConversionIsMandatory()
        }

        function ValidateConversionIsMandatory()
        {
            var primaryButton = $find("<%= chkIsPrimary.ClientID%>");
            var defaultButton = $find("<%= chkIsDefault.ClientID%>");
            var stockButton = $find("<%= chkIsStock.ClientID%>");

            if (primaryButton.get_checked() || defaultButton.get_checked() || stockButton.get_checked())
              $('#spnMand').show()            
            else
                $('#spnMand').hide()
        }

        function onDefaultUOMChange(sender, args) {
            ValidateConversionIsMandatory()
        }

        function onStockUOMChange(sender, args) {
            ValidateConversionIsMandatory()
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript">
            function OnClientFilesUploaded(sender, args) {
                $find('<%= RadAjaxManager2.ClientID%>').ajaxRequest();
            }
        </script>
    </telerik:RadScriptBlock>
    <h4>Manage Products</h4>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />

                </UpdatedControls>

            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>


    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>



    <%--<div class="mrgbtm">
        <div class="form-inline">
          <div class="form-group">
            <telerik:RadButton ID="btnAdd"   Skin ="Simple"  Text="Add New Product" runat ="server" CssClass ="btn btn-success" ></telerik:RadButton>
          </div>
        <div style="float:right !important;">

          <telerik:RadButton ID="btnClear" Skin ="Simple"  Visible ="false"   runat ="server" CssClass ="btn btn-default" Text ="Reset" >
                            </telerik:RadButton>
         
              <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server"    
                                             TemporaryFolder="C:\SalesWorx_CS\ExcelFolder" 
                                       
                                                           Skin ="Simple"         OnFileUploaded ="ExcelFileUpload_FileUploaded"
                 Localization-Select="Upload"    Height ="23px"     OnClientFilesUploaded="OnClientFilesUploaded" 
               MultipleFileSelection ="Disabled"   InitialFileInputsCount="1"  
              MaxFileInputsCount="1"   />
     <asp:LinkButton ID="lbLog" runat ="server" Text ="View Uploaded Log"   Font-Underline ="true" CssClass="btn btn-link"  
               ToolTip ="Click here to view the uploaded log" ForeColor ="Blue"
             OnClick ="lbLog_Click" ></asp:LinkButton>   
          </div>
          
        </div>
       </div>--%>

    <div class="mrgbtm">
        <div class="form-inline">
            <div class="form-group">
                <telerik:RadButton ID="btnAdd" AutoPostBack="true" Skin="Simple" Text="Add New Product" runat="server" CssClass="btn btn-success"></telerik:RadButton>
                <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Products" runat="server" CssClass="btn btn-danger"></telerik:RadButton>
                <telerik:RadButton ID="btndownloadTemplate" Skin="Simple" Text="Download Template" runat="server" CssClass="btn btn-primary"></telerik:RadButton>

            </div>
            <div class="form-group">
                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server"
                    Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"
                    Localization-Select="Import Products" OnClientFilesUploaded="OnClientFilesUploaded"
                    MultipleFileSelection="Disabled" InitialFileInputsCount="1"
                    MaxFileInputsCount="1" />
            </div>


            <div class="pull-right">
                <label>
                    <a id="link1" href="#">
                        <asp:Image alt="Upload Info" ToolTip="Upload Info" ImageUrl="~/images/info.png" ID="upl" runat="server" Width="18px" Height="18px" /></a>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" RelativeTo="Element" Width="300px" AutoCloseDelay="30000" BackColor="WhiteSmoke"
                        Height="360px" TargetControlID="link1" IsClientID="true" Animation="None" Position="TopCenter">
                        <h5>Upload Information</h5>
                        <p>New products will be uploaded. Existing products and invalid rows are ignored.</p>
                        <hr />
                        <h5>Validations</h5>
                        <ul style="padding: 0 0 0 15px; margin: 0; list-style-type: disc;">
                            <li>Item Code,Organization,Name and UOM columns are mandatory in Product.</li>
                            <li>UOM should be valid value.For any new UOM you have to provide the list to upload into the system.</li>
                            <li>Existing products are updated.New products are inserted.</li>
                            <li>Item Sheet name should be Product.</li>
                            <li>Item Code,UOM and  Conversion columns are mandatory in Item UOM Sheet.</li>
                            <li>Item UOM Sheet name should be Product_UOM.</li>
                        </ul>

                    </telerik:RadToolTip>

                    <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                        OnClick="lbLog_Click"></asp:LinkButton>
                    <telerik:RadButton ID="btnClear" Skin="Simple" Visible="false" runat="server" CssClass="btn btn-default" Text="Reset">
                    </telerik:RadButton>
                </label>
            </div>
        </div>


    </div>



    <telerik:RadWindowManager EnableViewState = "false" ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true" CssClass="RadWindow-Confirm">
    </telerik:RadWindowManager>













    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
        <contenttemplate>

                                 


                                    <p>
                    <asp:Label ID="lbl_msg_f" ForeColor ="Red" runat="server"></asp:Label>
                   
                </p>
                                    <div class="row">
                                             <div class="col-sm-2">
                                    <div class="form-group"> 
                                         <label>Organization</label> 
                                              <telerik:RadComboBox ID="ddlorg_F" Skin="Simple" Filter="Contains" Width="100%" TabIndex="7" EmptyMessage="Select Organization"  runat="server">
                  
                </telerik:RadComboBox>
                </div>
                                            </div>
                                             <div class="col-sm-2">
                                                <div class="form-group"> 
                                                     <label>Item code</label> 
                                                 <telerik:RadTextBox runat="server" ID="txtitem_code"  Skin="Simple" Width="100%"></telerik:RadTextBox>
                                                </div>
                                             </div>
                                             <div class="col-sm-2">
                                                <div class="form-group"> 
                                                     <label>Description   </label>
                                                 <telerik:RadTextBox runat="server" ID="txtDescription"  Skin="Simple" Width="100%"></telerik:RadTextBox>
                                                </div>
                                             </div>
                                        

                                            <div class="col-sm-2">
                                                <div class="form-group"> 
                                                     <label>Primary UOM </label>
                                         <telerik:RadComboBox ID="UOM" Skin="Simple" Filter="Contains" Width="100%" TabIndex="7" EmptyMessage="Select UOM"
                                            runat="server">
                                        </telerik:RadComboBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-2">
                                                <div class="form-group"> 
                                                 <label>Agency    </label>
                                      
                                         <telerik:RadComboBox ID="ddlAgency_F" Skin="Simple" Filter="Contains" Width="100%" TabIndex="7" EmptyMessage="Select Agency"
                                            runat="server">
                                        </telerik:RadComboBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-2">
                                                 <div class="form-group">   
                                               <label>&nbsp</label> 
                                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                                
                                                      <telerik:RadButton ID="btnclear_f" Skin ="Simple"    runat ="server" Text ="Clear" CssClass ="btn btn-default"  />
                       
                                          
                                                     </div>
                                                </div>

                                      </div> 

                                    
           
                                </contenttemplate>


    </asp:UpdatePanel>





    <div class="table-responsive">
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <telerik:RadGrid ID="rgProducts1" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                    PageSize="10" AllowPaging="True" runat="server" AllowFilteringByColumn="true"
                    GridLines="None">

                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                    <ExportSettings Excel-Format="ExcelML" ExportOnlyData="true" FileName="Products">
                    </ExportSettings>
                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                        DataKeyNames="Product_ID,Sales_Org_ID" ClientDataKeyNames="Product_ID,Sales_Org_ID" AllowFilteringByColumn="true"
                        PageSize="10">


                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true"></PagerStyle>

                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                                InitializeTemplatesFirst="false">


                                <ItemTemplate>

                                    <asp:ImageButton ID="btnEdit" ToolTip="Edit Product" runat="server" CausesValidation="false"
                                        OnClick="btnEdit_Click" ImageUrl="~/Images/edit-13.png" />

                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                                InitializeTemplatesFirst="false">


                                <ItemTemplate>
                                    <asp:Label ID="lblProductID" runat="server" Text='<%# Bind("Product_ID")%>' Visible="false"></asp:Label>
                                    <asp:ImageButton ID="btnDelete" ToolTip="Delete Product" runat="server" CausesValidation="false"
                                        CommandName="DeleteSelected"
                                        ImageUrl="~/Images/delete-13.png"
                                        Visible='<%# Bind("DeleteVisible")%>'
                                        OnClientClick="return ConfirmDelete('Are you sure to delete this item?',event);" />


                                    <asp:ImageButton ID="ImageButton1" ToolTip="Activate Product" runat="server" CausesValidation="false"
                                        CommandName="ActivateSelected"
                                        ImageUrl="~/Images/close.jpg"
                                        Visible='<%# Bind("ActivateVisible")%>'
                                        OnClientClick="return ConfirmDelete('Are you sure to active this item?',event);" />

                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="mUOMColumn" AllowFiltering="false"
                                InitializeTemplatesFirst="false" Visible="false">


                                <ItemTemplate>

                                    <asp:LinkButton ID="lnkbtn_UOM" CommandName="UOMSelected" Text="UOM" runat="server" />




                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </telerik:GridTemplateColumn>


                            <telerik:GridBoundColumn UniqueName="OrgName" AllowFiltering="false"
                                SortExpression="OrgName" HeaderText="Organization" DataField="OrgName"
                                ShowFilterIcon="false">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="false" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Item_code" AllowFiltering="false"
                                AutoPostBackOnFilter="true"
                                SortExpression="Item_code" HeaderText="Item Code" DataField="Item_code"
                                ShowFilterIcon="false">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="false" />
                            </telerik:GridBoundColumn>




                            <telerik:GridBoundColumn UniqueName="Description" AllowFiltering="false"
                                SortExpression="Description" HeaderText="Description" DataField="Description"
                                ShowFilterIcon="false">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="true" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Base_UOM" AllowFiltering="false"
                                SortExpression="Base_UOM" HeaderText="UOM" DataField="Base_UOM"
                                ShowFilterIcon="false">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="false" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Brand_Code" AllowFiltering="false"
                                SortExpression="Brand_Code" HeaderText="Brand" DataField="Brand_Code"
                                ShowFilterIcon="false">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="false" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn UniqueName="Agency" AllowFiltering="false"
                                SortExpression="Agency" HeaderText="Agency" DataField="Agency"
                                ShowFilterIcon="false">
                                <ItemStyle Wrap="false" />
                                <HeaderStyle Wrap="false" />
                            </telerik:GridBoundColumn>





                            <telerik:GridDateTimeColumn UniqueName="Created_At" SortExpression="Created_At" ShowFilterIcon="false" PickerType="DateTimePicker" HeaderText="Created On"
                                DataField="Created_At" DataFormatString="{0:dd-MM-yyyy HH:mm}" AllowFiltering="false" CurrentFilterFunction="StartsWith"
                                AutoPostBackOnFilter="true">
                                <HeaderStyle Wrap="false" />
                                <ItemStyle Wrap="false" />
                            </telerik:GridDateTimeColumn>
                            <telerik:GridBoundColumn UniqueName="Net_Price" Visible="false"
                                SortExpression="Net_Price" HeaderText="Net.Price" DataField="Net_Price"
                                ShowFilterIcon="false">
                                <ItemStyle Wrap="false" />
                            </telerik:GridBoundColumn>


                        </Columns>






                    </MasterTableView>

                </telerik:RadGrid>


                <script type="text/javascript">
                    $(window).resize(function () {
                        var win = $find('<%= DocWindow.ClientID%>');
                        if (win) {
                            if (!win.isClosed()) {
                                win.center();
                            }
                        }

                    });
                </script>
                <telerik:RadWindow ID="DocWindow" Title="Currency Code Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                    AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                    <ContentTemplate>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                            <ContentTemplate>

                                <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                 <asp:HiddenField ID="hidProdVal" runat="server" Value="-1" />

                                <div class="popupcontentblkwider">
                                    <p>
                                        <asp:Label ID="lblVMsg" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                    </p>

                                    <telerik:RadTabStrip ID="radProdTabStrip" runat="server" MultiPageID="radProdMultiPage" SelectedIndex="0">
                                        <Tabs>
                                            <telerik:RadTab runat="server" Text="Product Info" PageViewID="prodPageView" Selected="true" SelectedIndex="0">
                                            </telerik:RadTab>
                                            <telerik:RadTab runat="server" Text="UOM *" PageViewID="UOMPageView">
                                            </telerik:RadTab>

                                        </Tabs>
                                    </telerik:RadTabStrip>


                                    <telerik:RadMultiPage ID="radProdMultiPage" runat="server" Width="100%" SelectedIndex="0" BorderStyle="solid" BorderColor="Gray"
                                        BorderWidth="1px">

                                        <telerik:RadPageView ID="prodPageView" runat="server" Width="100%">
                                            <div style="padding:15px;">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Organization *
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlOrg" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                                            <telerik:RadComboBox ID="ddlOrg" Skin="Simple" Filter="Contains" Width="100%" TabIndex="1" EmptyMessage="Select Organization"
                                                              onclienttextchange="OnddlOrgClientTextChange"  onclientblur ="OnddlOrgClientTextChange"   runat="server">
                                                            </telerik:RadComboBox>

                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Item Code * 
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtProdCode"
                                                ErrorMessage="" ForeColor="Red" Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator></label>
                                                            <telerik:RadTextBox ID="txtProdCode" MaxLength="40" runat="server" Skin="Simple"
                                                                TabIndex="3" Width="100%">
                                                            </telerik:RadTextBox>
                                                            <asp:Label ID="lblProdID" Visible="false" runat="server"></asp:Label>
                                                            <asp:Label ID="lblOrgID" Visible="false" runat="server"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Description *    
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtProdName"
                                                ErrorMessage="" ForeColor="Red" Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator>
                                                            </label>
                                                            <telerik:RadTextBox ID="txtProdName" runat="server" MaxLength="240" Skin="Simple"
                                                                TabIndex="4" Width="100%">
                                                            </telerik:RadTextBox>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Brand 
                                                            </label>
                                                            <telerik:RadComboBox ID="ddlBrand" Skin="Simple" Filter="Contains" Width="100%" TabIndex="5" EmptyMessage="Select Brand"
                                                                runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                    <%--           <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                UOM *
                                            <asp:CompareValidator ID="CompareValidatorRadComboxBoxTables" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlUOM" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                                            <telerik:RadComboBox ID="ddlUOM" Skin="Simple" Filter="Contains" Width="100%" TabIndex="6" EmptyMessage="Select UOM"
                                                             OnSelectedIndexChanged="ddlUOM_SelectedIndexChanged"   runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>--%>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Agency    </label>
                                                            <%-- <telerik:RadTextBox ID="txtAgency" runat="server" Skin="Simple" MaxLength="50"
                                            TabIndex="6">
                                        </telerik:RadTextBox>--%>
                                                            <telerik:RadComboBox ID="ddlAgency" Skin="Simple" Filter="Contains" Width="100%" TabIndex="7" EmptyMessage="Select Agency"
                                                                runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                            <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Category    </label>
                                                            <telerik:RadComboBox ID="ddlCategory" Skin="Simple" Filter="Contains" Width="100%" TabIndex="8" EmptyMessage="Select Category"
                                                                runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">


                                            

                                                    <%--                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Default UOM 
                                            <asp:CompareValidator ID="CompareValidatorRadComboxBoxTablesdefaultUOM" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddldefaultUOM" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                                            <telerik:RadComboBox ID="ddldefaultUOM" Skin="Simple" Filter="Contains" Width="100%" TabIndex="9" EmptyMessage="Select Default UOM"
                                                                runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>--%>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Restrictive Return *
                                            <asp:CompareValidator ID="CompareValidatorRadComboxBoxTablesRestrictiveR" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlRestrictiveR" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                                            <telerik:RadComboBox ID="ddlRestrictiveR" Skin="Simple" Filter="Contains" Width="100%" TabIndex="10" EmptyMessage="Select Restrictive Return"
                                                                runat="server">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                   <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Cost Price</label>
                                                            <telerik:RadNumericTextBox runat="server" ID="txtCost" Skin="Simple"
                                                                TabIndex="11" MinValue="0"
                                                                autocomplete="off" NumberFormat-DecimalDigits="2"
                                                                IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false" Width="100%">
                                                            </telerik:RadNumericTextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Barcode</label>
                                                            <telerik:RadTextBox ID="txtBarcode" runat="server" Skin="Simple" MaxLength="50"
                                                                TabIndex="12" Width="100%">
                                                            </telerik:RadTextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="row">


                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Item Size</label>
                                                            <telerik:RadTextBox ID="txtItemSize" runat="server" Skin="Simple" MaxLength="40"
                                                                TabIndex="13" Width="100%">
                                                            </telerik:RadTextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Sub Category</label>
                                                            <telerik:RadTextBox ID="txtSubCategory" runat="server" Skin="Simple" MaxLength="50"
                                                                TabIndex="14" Width="100%">
                                                            </telerik:RadTextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>&nbsp;</label>
                                                            <telerik:RadButton ID="rsHasLots" runat="server" Text="Has Lots" TabIndex="15" AutoPostBack="false"
                                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple">
                                                            </telerik:RadButton>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">

                                              
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <telerik:RadButton ID="rsAllowpricechange" runat="server" Text="Allow Price Change" TabIndex="16" AutoPostBack="false"
                                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple">
                                                            </telerik:RadButton>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                        </telerik:RadPageView>

                                        <telerik:RadPageView ID="UOMPageView" runat="server" Width="100%" >

                                            <asp:UpdatePanel ID="upUOM" runat="server" UpdateMode="conditional">
                                                <ContentTemplate>
                                                    <div  style="padding:15px;">
                                                    <div class="row">

                                                        <asp:HiddenField ID="HOrgID" runat="server"></asp:HiddenField>


                                                        <div class="col-sm-4">
                                                            <label>Organization</label>
                                                            <div class="form-group">
                                                                <asp:TextBox ID="txtOrgID" Enabled="false" TabIndex="21" Width="100%" runat="server"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="col-sm-4">
                                                            <div class="form-group">
                                                                <label>
                                                                    UOM *       
                                                                </label>
                                                                <telerik:RadComboBox ID="ddlUOMNew" Skin="Simple" Filter="Contains" Width="100%" TabIndex="22" EmptyMessage="Select UOM"
                                                                    runat="server">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>

                                                       <div class="col-sm-4">
                                                             <label>&nbsp;</label>
                                                            <telerik:RadButton ID="chkIsPrimary" runat="server" Text="Is Primary UOM" TabIndex="23" AutoPostBack="false"
                                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple" OnClientCheckedChanged="onPrimaryUOMChange">
                                                            </telerik:RadButton>  
                                                          
                                                             <telerik:RadButton ID="chkIsDefault" runat="server" Text="Is Default UOM" TabIndex="24" AutoPostBack="false"
                                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple" OnClientCheckedChanged="onDefaultUOMChange" >
                                                            </telerik:RadButton>                                                           
                                                        </div>

                                      

                                                    </div>
                                                <div class="row">
                                                  <div class="col-sm-4">
                                                         
                                                                <label>Conversion Rate <span id="spnMand" style="display:none">*</span></label>
                                                                <div class="form-group">
                                                                    <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radNumRate" Width="190px"  
                                                                       autocomplete="off"  EmptyMessage="" MaxLength="10"   Type="Number" MinValue="1" 
                                                                        NumberFormat-DecimalDigits="0" NumberFormat-AllowRounding="false" TabIndex="25" ></telerik:RadNumericTextBox><br />


                                                                    <asp:TextBox ID="txtRate" Visible="false" TabIndex="3" Width="100%" runat="server" MaxLength="6" onKeypress='return IntegerOnly(event)'></asp:TextBox>
                                                                    <asp:RegularExpressionValidator Visible="false" ValidationExpression="[0-9]+" ControlToValidate="txtRate" ID="RegExpression" Display="None" runat="server" ErrorMessage="Conversion Rate should be in numbers."></asp:RegularExpressionValidator>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTBtxtConvertRate" FilterType="Numbers,Custom"
                                                                        ValidChars="." runat="server" TargetControlID="txtRate">
                                                                    </ajaxToolkit:FilteredTextBoxExtender>

                                                                </div>
                                                  


                                                   </div>


                                                    

                                                        <div class="col-sm-4">
                                                            <label>Sellable</label>
                                                            <div class="form-group">
                                                                <telerik:RadComboBox ID="ddl_Sellable" Skin="Simple" Filter="Contains" Width="100%" TabIndex="26"
                                                                    runat="server">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="Y" Text="Yes" Selected="true" />
                                                                        <telerik:RadComboBoxItem Value="N" Text="No" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                               

                                                        <div class="col-sm-4">
                                                            <label>&nbsp;</label>
                                                            <telerik:RadButton ID="chkIsStock" runat="server" Text="Is Stock UOM" TabIndex="27" AutoPostBack="false"
                                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple" OnClientCheckedChanged="onStockUOMChange">
                                                            </telerik:RadButton>  

                                                        </div>

                                                    </div>
                                                    <div class="row">

                                                        

                                                        <div class="col-sm-12">
                                                            <div class="form-group text-right">
                                                                <telerik:RadButton ID="btnAddToList" Skin="Simple" CssClass="btn btn-primary"  
                                                                    OnClick="btnAddToList_Click"
                                                                    runat="server" Text="Add to List" TabIndex="6">
                                                                </telerik:RadButton>

                                                                <telerik:RadButton ID="btnUpdateToList" Skin="Simple" CssClass="btn btn-primary" Visible="false"
                                                                    OnClick="btnUpdateToList_Click"
                                                                    runat="server" Text="Update List" TabIndex="6">
                                                                </telerik:RadButton>
                                                                <telerik:RadButton ID="btnUOMClear" Skin="Simple" CssClass="btn btn-default"
                                                                    OnClick="btnUOMClear_Click"
                                                                    runat="server" Text="Clear" TabIndex="6">
                                                                </telerik:RadButton>

                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>&nbsp;</div>
                                                    <asp:GridView Width="100%" ID="gvUOM" runat="server" EmptyDataText="No UOM to Display"
                                                        EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                                                        AllowPaging="false" AllowSorting="false" PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">

                                                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                        <Columns>
                                                            <asp:TemplateField>                                                 
                                                                <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" ToolTip="Edit Item Code" runat="server" CausesValidation="false"
                                                                        ImageUrl="~/images/edit-13.png" OnClick="btnUOMEdit_Click" CssClass="checkboximgvalign" />
                                                                    <asp:ImageButton ToolTip="Delete Item Code" ID="btnDelete" OnClick="btnUOMDelete_Click"
                                                                        runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected UOM from the list?',event);" CssClass="checkboximgvalign" />
                                                            
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block" />
                                                            </asp:TemplateField>                                                                                                

                                                            <asp:BoundField DataField="UOMText" HeaderText="Item UOM "
                                                                SortExpression="UOMText">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Conversion" DataFormatString="{0:##,###,##0}" HeaderText="Conversion"
                                                                SortExpression="Conversion">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("Item_UOM")%>'></asp:Label>
                                                                    <asp:Label ID="lblLocalID" runat="server" Text='<%# Bind("LocalID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SellableText" HeaderText="Is Sellable"
                                                                SortExpression="SellableText">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>

                                                           <asp:BoundField DataField="PrimaryUOMText" HeaderText="Is Primary"
                                                                SortExpression="PrimaryUOMText">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>

                                                           <asp:BoundField DataField="DefaultUOMText" HeaderText="Is Default"
                                                                SortExpression="DefaultUOMText">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>

                                                             <asp:BoundField DataField="StockUOMText" HeaderText="Is Stock"
                                                                SortExpression="StockUOMText">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                        <HeaderStyle />
                                                        <RowStyle CssClass="tdstyle" />
                                                        <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>





                                        </telerik:RadPageView>

                                    </telerik:RadMultiPage>



                                    <div><br /></div>
                                            <div class="text-center">
                                                <telerik:RadButton ID="btnSave" Skin="Simple" CssClass="btn btn-success"
                                                    OnClick="btnSave_Click" ValidationGroup="valsum"
                                                    runat="server" Text="Save" TabIndex="6">
                                                </telerik:RadButton>

                                                <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass="btn btn-success"
                                                    OnClick="btnUpdate_Click" ValidationGroup="valsum"
                                                    runat="server" Text="Update" TabIndex="6">
                                                </telerik:RadButton>

                                                <telerik:RadButton ID="btnCancel" Skin="Simple" CssClass="btn btn-default"
                                                    OnClick="btnCancel_Click" ValidationGroup="valsum"
                                                    runat="server" Text="Cancel" TabIndex="7">
                                                </telerik:RadButton>
                                            </div>
                                        
                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
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
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
