<%@ Page Title="Invoice Print Headers" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminPrintHeaders.aspx.vb" Inherits="SalesWorx_BO.AdminPrintHeaders" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .tablecellalign th,
        .tablecellalign th a {
            color: #0d638f;
            text-decoration: none;
        }

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
            vertical-align: middle;
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
            float: left;
            display: inline-block;
        }

        #ctl00_ContentPlaceHolder1_grdCustomerSegment td {
            white-space: normal !important;
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

        function alertCallBackFn(arg) {
            HideRadWindow()
        }

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                //    $('a[class=rwCloseButton')[0].click();
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Invoice Print Headers</h4>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <asp:HiddenField ID="HCommonHdrforVan" runat="server" />
    <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                                     <div class="row">
                                    <div class="col-md-3 col-lg-4">
                                    <div class="form-group">  <label>
                                               Type
                                           </label>
                                           
                                                <telerik:RadComboBox Skin="Simple" ID="ddl_Type" Width="100%" runat="server" CssClass="inputSM" AutoPostBack="true" >
                                              
                                                </telerik:RadComboBox>

                                           
                                           </div>
                                        </div>
                                            <div class="col-md-3 col-lg-4">
                                                <div class="form-group">
                                             <label>
                                                Filter value
                                            </label>
                                           
                                            
                                                <asp:TextBox ID="txtFilterVal"  runat="server" Width="100%" ToolTip ="Enter Filter Value" autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3" ></asp:TextBox>
                                                    
                                               
                                                 </div>    
                                            </div>
                                         <div class="col-md-6 col-lg-4">
                                                <div class="form-group">
                                                     <label><br /></label>
                                                <div>
                                          
                                                     <asp:Button ID="btnFilter" runat="server" CausesValidation="False"  CssClass ="btn btn-primary"
                                                    OnClick="btnFilter_Click" TabIndex="4" Text="Filter" />
                                                    <asp:Button ID="btnclearFilter" runat="server" CausesValidation="false" CssClass ="btn btn-default" 
                                                     OnClick="btnclearFilter_Click" TabIndex="1" Text="Clear Filter" />
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="false"  CssClass="btn btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                   </div>   
                                                    </div>
                                             </div>
                                        </div>
                                </contenttemplate>
    </asp:UpdatePanel>

    <div class="table-responsive">
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <asp:Panel ID="pnl_client" runat="server">

                    <asp:GridView Width="100%" ID="grdCustomerSegment" runat="server" EmptyDataText="No Data to show"
                        EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                        AllowPaging="True" AllowSorting="True" PageSize="25" CellPadding="0"
                        CssClass="tablecellalign">

                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                        <EmptyDataRowStyle Font-Bold="True" />
                        <Columns>
                            <asp:TemplateField>

                                <HeaderStyle HorizontalAlign="Left" Width="40px" />

                                <ItemTemplate>

                                    <asp:ImageButton ToolTip="Delete" ID="btnDelete" OnClick="btnDelete_Click"
                                        runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete this print header?');" />
                                    <asp:ImageButton ID="btnEdit" ToolTip="Edit" runat="server" CausesValidation="false"
                                        ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" />
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Info_Key" HeaderText="" SortExpression="Info_Key">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Value_5" HeaderText="Header Line 1" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Value_4" HeaderText="Header Line 2" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Value_3" HeaderText="Header Line 3" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="200px" />
                            </asp:BoundField>


                            <asp:BoundField DataField="Value_7" HeaderText="Header Line 4" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Value_8" HeaderText="Header Line 5" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Value_9" HeaderText="Header Line 6" HtmlEncode="false">
                                <ItemStyle Wrap="true" />
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
                                    <asp:Label ID="lblVal2" runat="server" Text='<%# Bind("Value_2")%>'></asp:Label>


                                    <asp:Label ID="lblVal7" runat="server" Text='<%# Bind("Value_7")%>'></asp:Label>
                                    <asp:Label ID="lblVal8" runat="server" Text='<%# Bind("Value_8")%>'></asp:Label>
                                    <asp:Label ID="lblVal9" runat="server" Text='<%# Bind("Value_9")%>'></asp:Label>


                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Logo">
                                <ItemTemplate>


                                    <asp:ImageButton ID="imgLogo" runat="server" ImageUrl='<%#Eval("Logo") %>' Width="250px" Height="90px" />
                                    <asp:ImageButton ID="imgLogo2" runat="server" ImageUrl='<%#Eval("logo4") %>' Width="250px" Height="90px" />

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagerstyle" />
                        <HeaderStyle />
                        <RowStyle CssClass="tdstyle" />
                        <AlternatingRowStyle CssClass="alttdstyle" />
                    </asp:GridView>

                </asp:Panel>

                <telerik:RadWindow ID="MPEDetails" Title="Print Header" runat="server" Skin="Windows7" Behaviors="Move,Close"
                    AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                    <ContentTemplate>
                        <div class="popupcontentblk">
                            <p>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                            </p>
                            <div class="row">
                                <div class="col-sm-5">
                                    <label>
                                        <asp:Label runat="server" ID="lbl_Info_Key"></asp:Label></label>
                                </div>
                                <div class="col-sm-7">
                                    <div class="form-group">
                                        <telerik:RadComboBox Skin="Simple" ID="ddl_Client" runat="server" TabIndex="1" CssClass="inputSM" Width="100%"></telerik:RadComboBox>
                                        <telerik:RadComboBox Skin="Simple" ID="ddl_vans" runat="server" TabIndex="1" CssClass="inputSM" Width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="txt" runat="server">
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line1</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line1" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="50" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line2</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line2" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line3</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line3" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line4</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line7" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line5</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line8" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5">
                                        <label>Line6</label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:TextBox ID="txt_Line9" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="img" runat="server">
                                <div class="row" id="a4" runat="server">
                                    <div class="col-sm-5">
                                        <label>Logo (A4 Printer)<br />
                                            <small>Image size: 1960px X 277px</small></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label1" Text="Keep the same file"></asp:Label>

                                            <asp:RadioButtonList ID="rdo_keepSamefile" runat="server" Visible="false" RepeatDirection="Horizontal" CssClass="inputSM">
                                                <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:FileUpload class="file" ID="fUpload" runat="server" />
                                            <br />
                                            <br />
                                            <asp:Image ID="img_Logo" runat="server" Visible="false" Width="400px" Height="80px" />
                                            <asp:Label runat="server" ID="lbl_logo2"></asp:Label>
                                            <asp:HiddenField runat="Server" ID="hImgFile"></asp:HiddenField>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="inc4" runat="server">
                                    <div class="col-sm-5">
                                        <label>Logo (4Inch Printer)<br />
                                            <small>Image size: 825px X 285px</small></label>
                                    </div>
                                    <div class="col-sm-7">
                                        <div class="form-group">
                                            <asp:FileUpload class="file" ID="fUpload4inc" runat="server" />
                                            <br />
                                            <br />
                                            <asp:Image ID="img_Logo4inc" runat="server" Visible="false" Width="400px" Height="80px" />
                                            <asp:Label runat="server" ID="lbl_logo24inc"></asp:Label>
                                            <asp:HiddenField runat="Server" ID="hImgFile4inc"></asp:HiddenField>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="row">
                                <div class="col-sm-5">
                                    <label></label>
                                </div>
                                <div class="col-sm-7">
                                    <div class="form-group">


                                        <asp:Button ID="btnSave" CssClass="btn btn-success" TabIndex="5"
                                            runat="server" Text="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnUpdate" CssClass="btn btn-success" OnClick="btnUpdate_Click"
                                            runat="server" Text="Update" />
                                        <asp:Button ID="btnCancel" CssClass="btn btn-default" TabIndex="6" OnClientClick="return DisableValidation()"
                                            runat="server" CausesValidation="false" Text="Cancel" />
                                        <asp:HiddenField ID="hCommonVanOrgID" runat="server" />
                                    </div>
                                </div>
                            </div>


                            <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                <img alt="Processing..." src="../images/Progress.gif" />
                                <span>Processing... </span>
                            </asp:Panel>



                        </div>
                    </ContentTemplate>
                </telerik:RadWindow>


            </ContentTemplate>

        </asp:UpdatePanel>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>

