<%@ Page Title="Customer Segment" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminCustomerSegment.aspx.vb" Inherits="SalesWorx_BO.AdminCustomerSegment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style type="text/css">
        #ctl00_MainContent_ExcelFileUpload {
            width: auto !important;
            display: inline-block;
            vertical-align: middle;
        }
        #ctl00_MainContent_ExcelFileUpload  ul li{
            padding-left:0 !important;
            font-size: 14px;
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
                    return confirm('Would you like to delete the selected customer segment?');
                return true;
            }
            alert('Select at least one customer segment!');
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
                 $('a[class=rwCloseButton')[0].click();
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

          <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript">
            function OnClientFilesUploaded(sender, args) {
                $find('<%= RadAjaxManager2.ClientID%>').ajaxRequest();
            }
        </script>
    </telerik:RadScriptBlock>
    <h4>Customer Segment Management</h4>
         <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />

                </UpdatedControls>

            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>
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
                                    <label>Filter By</label>
                                    <div class="row">
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Segment Code" Text="Segment Code"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
	                                    <div class="col-sm-4">
		                                    <div class="form-group">
                                                <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value"  Width="100%" TabIndex="1"></telerik:RadTextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
		                                    <div class="form-group">
                                                    <asp:Button ID="btnFilter" runat="server" CausesValidation="False"  CssClass ="btn btn-primary"
                                                    OnClick="btnFilter_Click" TabIndex="2" Text="Filter" />
                                                    <asp:Button ID="btnclearFilter" runat="server" CausesValidation="false"  CssClass ="btn btn-default" 
                                                     OnClick="btnclearFilter_Click" TabIndex="3" Text="Clear Filter" />
                                                    
                                                     <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btn btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="4" Text="Add" />
                                            </div>
                                        </div>
                                    </div>
                                   
                                </ContentTemplate>
                            </asp:UpdatePanel>



          <div class="mrgbtm">
                                            <div class="form-inline">
                                            <div class="form-group">

                                                                                       
                                                                                                        
                                                 <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Customer Segment" runat="server" CssClass="btn btn-primary"></telerik:RadButton>
                                                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server" 
                                                 Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"    Localization-Select="Upload" OnClientFilesUploaded="OnClientFilesUploaded" MultipleFileSelection="Disabled" InitialFileInputsCount="1"  MaxFileInputsCount="1"  />
                                      


                                                </div>

                                                      <div class="pull-right">
                <label>
                    <a id="link1" href="#">
                        <asp:Image alt="Upload Info" ToolTip="Upload Info" ImageUrl="~/images/info.png" ID="upl" runat="server" Width="18px" Height="18px" /></a>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" RelativeTo="Element" Width="300px" AutoCloseDelay="30000" BackColor="WhiteSmoke"
                        Height="200px" TargetControlID="link1" IsClientID="true" Animation="None" Position="TopCenter">
                        <p style="color: darkolivegreen; padding-left: 10px; font-size: 12px;">
                            <b style="color: orchid;"><u>Upload Information</u></b><br />
                            New Customer Segments will be uploaded. Existing Customer Segments and invalid rows are ignored.

           <br />
                            <br />
                            <b style="color: green;"><u>Validations</u></b><br />
                            <ul>
                                <li type="square">Description and Code are mandatory.</li>
                                <li type="square">Customer Segments Sheet contains only two columns named as Description,Code.</li>
                                <li type="square">Customer Segments Sheet name should be CustomerSegment.</li>
                            </ul>

                        </p>
                    </telerik:RadToolTip>

                    <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                         OnClick="lbLog_Click" ></asp:LinkButton>
                    <telerik:RadButton ID="btnClear" Skin="Simple" Visible="false" runat="server" CssClass="btn btn-default" Text="Reset">
                    </telerik:RadButton>
                </label>
            </div>
                                                </div>
                                   </div>   



                    <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                
                                 
                                                <asp:GridView Width="100%" ID="grdCustomerSegment" runat="server" EmptyDataText="No Customer Segment to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" 
                                                    CssClass="tablecellalign">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
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
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete customer segment" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected customer segment?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit customer segment" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Description" HeaderText="Customer Segment" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                         <asp:BoundField DataField="Customer_Segment_Code" HeaderText="Customer Segment Code" SortExpression="Customer_Segment_Code">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomer_Segment_ID" runat="server" Text='<%# Bind("Customer_Segment_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                         
                                   
                                         <telerik:RadWindow ID="MPEDetails" Title= "Customer Segment Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <div class="popupcontentblk">
                                     <p><asp:Label ID="lblMessage" runat ="server" ForeColor ="Red" ></asp:Label><asp:HiddenField ID="HidVal" runat="server" Value="-1" /></p>

                                           <div class="row">
		                                        <div class="col-sm-5">
			                                        <label>Customer Segment</label>
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:TextBox ID="txtDescription" runat="server" TabIndex="1" Width="100%" CssClass="inputSM" MaxLength="150"></asp:TextBox>
                                                                                                           </div>
                                                </div>

                                                   
                                            </div>

                                        <div class="row">
		                                        <div class="col-sm-5">
			                                        <label>Customer Segment Code</label>
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:TextBox ID="txtcode" runat="server" TabIndex="2" CssClass="inputSM" Width="100%"></asp:TextBox>
                                                     			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="row">
		                                        <div class="col-sm-5"></div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Button ID="btnSave"  CssClass ="btn btn-success"  TabIndex="2" 
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                        <asp:Button ID="btnUpdate"  CssClass ="btn btn-success" TabIndex="2"  OnClick="btnUpdate_Click"
                                                            runat="server" Text="Update"  />
                                                        <asp:Button ID="btnCancel"  TabIndex="3" CssClass ="btn btn-default"
                                                          OnClick="btnCancel_Click"   runat="server" CausesValidation="false" Text="Cancel" />
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                <img alt="Processing..."  src="../assets/img/ajax-loader.gif" />
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
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
         
           
</asp:Content>
