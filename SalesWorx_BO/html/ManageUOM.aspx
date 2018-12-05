<%@ Page Title="UOM Management " Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageUOM.aspx.vb" Inherits="SalesWorx_BO.ManageUOM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


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
                  return confirm('Would you like to delete the selected UOM?');
                  return true;
               }
           alert('Select at least one UOM!');
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
        function IntegerOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }

            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 0)
                return true;

            return false;
        }
    </script>
   </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <h4>Manage UOM</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
                 <span>Processing... </span>
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
      
        
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                       <label>
                                                Filter By
                                            </label>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Item UOM" Text="Item UOM"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Conversion" Text="Conversion"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox><asp:HiddenField ID="HOrgID" runat="server"></asp:HiddenField>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">   
                                            <div class="form-group">   
                                             <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value"  Width="100%"></telerik:RadTextBox>
                                          </div>
                                        </div>
                                        <div class="col-sm-4">   
                                            <div class="form-group">      
                                                 <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add UOM" CssClass="btn btn-success" />
                                                 <telerik:RadButton ID="btnBack" Skin="Simple" OnClick ="btnBack_Click" runat="server" Text="Go Back" CssClass="btn btn-default" />
                                            </div>
                                        </div>
                                 </div> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                  <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvUOM" runat="server" EmptyDataText="No UOM to Display"
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
                                                                 OnClientClick="return ConfirmDelete('Would you like to delete the selected UOM ?',event);"
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete Item Code" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected UOM?',event);" CssClass="checkboximgvalign" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Item Code" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Item_Code" HeaderText="Item Code" SortExpression="Item_Code">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Site" HeaderText="Organization" SortExpression="Site">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="Item_UOM"  HeaderText="Item UOM "
                                                            SortExpression="Item_UOM">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="Conversion" DataFormatString="{0:F2}" HeaderText="Conversion"
                                                            SortExpression="Conversion">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>

                                                       <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("Item_UOM")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Issellable"  HeaderText="Issellable"
                                                            SortExpression="Issellable">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>

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
                                           
                                    <telerik:RadWindow ID="MPEDetails" Title= "UOM Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Item Code</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtItemCode" runat="server" Width ="100%" TabIndex="1" MaxLength="3"></asp:TextBox>
                                                   <%-- <asp:RegularExpressionValidator
                                                        ValidationExpression="[a-zA-Z]+" ControlToValidate="txtItemCode" ID="RFV"  ValidationGroup ="valsum"
                                                        Display="None" runat="server" ErrorMessage="Item Code should be in characters."></asp:RegularExpressionValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                               <label> Organization</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtOrgID" TabIndex="2" Width ="100%"  runat="server"></asp:TextBox>
                                                </div>
                                            </div>


                                              <div class="col-sm-5">
                                                            <label>Primary UOM </label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                                <asp:TextBox ID="txtPrimaryUOM" TabIndex="2" Width ="100%"  runat="server" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                       <div class="col-sm-5">
                                                            <label>UOM *</label>
                                        

                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                                <telerik:RadComboBox ID="ddlUOM" Skin="Simple" Filter="Contains" Width="100%" TabIndex="6" EmptyMessage="Select UOM"
                                                             runat="server"  OnSelectedIndexChanged="ddlUOM_SelectedIndexChanged" >
                                        </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                            <div class="col-sm-5">
                                                <label>Conversion Rate *</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtRate" TabIndex="3" Width ="100%"  runat="server"  MaxLength="6" onKeypress='return IntegerOnly(event)'></asp:TextBox>
                                                    <asp:RegularExpressionValidator ValidationExpression="[0-9]+" ControlToValidate="txtRate" ID="RegExpression" Display="None" runat="server" ErrorMessage="Conversion Rate should be in numbers."></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTBtxtConvertRate" FilterType="Numbers,Custom"  
                                                        ValidChars="." runat="server" TargetControlID="txtRate">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                          
                                         
                                              <div class="col-sm-5">
                                                            <label>Sellable</label>
                                        

                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                                <telerik:RadComboBox ID="ddl_Sellable" Skin="Simple" Filter="Contains" Width="100%" TabIndex="7"  
                                                             runat="server" >
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="Y" Text="Yes" Selected="true" />
                                                                        <telerik:RadComboBoxItem Value="N" Text="No" />
                                                                    </Items>
                                        </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                         
                                            <div class="col-sm-5">
                                                
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="">
                                                    <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnSave_Click"  ValidationGroup ="valsum"
                                                            runat="server" Text="Save" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                                           OnClick="btnUpdate_Click"  ValidationGroup ="valsum"
                                                                            runat="server" Text="Update" TabIndex ="6" >
                                                      </telerik:RadButton>

                                                                     <telerik:RadButton ID="btnCancel" Skin="Simple"  CssClass ="btn btn-default" 
                                                                         OnClick="btnCancel_Click" ValidationGroup ="valsum"
                                                                            runat="server" Text="Cancel" TabIndex ="7" >
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
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
          
            
</asp:Content>
