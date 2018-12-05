<%@ Page Title="Currency Management " Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="CurrencyCode.aspx.vb" Inherits="SalesWorx_BO.CurrencyCode" %>

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
    </script>
   </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <h4>Currency Management</h4>

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
                                                        <telerik:RadComboBoxItem Value="Currency Code" Text="Currency Code"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
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
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                            </div>
                                        </div>
                                 </div> 
                                </ContentTemplate>
                            </asp:UpdatePanel>
                  <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvCurrency" runat="server" EmptyDataText="No Currency to Display"
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
                                                                 OnClientClick="return ConfirmDelete('Would you like to delete the selected currency ?',event);"
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete Currency Code" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected currency?',event);" CssClass="checkboximgvalign" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Currency Code" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Currency_Code" HeaderText="Currency Code" SortExpression="Currency_Code">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Conversion_Rate" DataFormatString="{0:F2}" HeaderText="Conversion Rate"
                                                            SortExpression="Conversion_Rate">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Decimal_Digits" HeaderText="Decimal Digits" SortExpression="Decimal_Digits">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Currency_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                                           
                                    <telerik:RadWindow ID="MPEDetails" Title= "Currency Code Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Currency Code</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtCurrencyCode" runat="server" Width ="100%" TabIndex="1" MaxLength="3"></asp:TextBox>
                                                    <asp:RegularExpressionValidator
                                                        ValidationExpression="[a-zA-Z]+" ControlToValidate="txtCurrencyCode" ID="RFV"  ValidationGroup ="valsum"
                                                        Display="None" runat="server" ErrorMessage="Currency Code should be in characters."></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                               <label> Description</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtDescription" TabIndex="2" Width ="100%"  runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Conversion Rate</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtRate" TabIndex="3" Width ="100%"  runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ValidationExpression="[0-9]+" ControlToValidate="txtRate" ID="RegExpression" Display="None" runat="server" ErrorMessage="Conversion Rate should be in numbers."></asp:RegularExpressionValidator>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTBtxtConvertRate" FilterType="Numbers,Custom"  
                                                        ValidChars="." runat="server" TargetControlID="txtRate">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Decimal Digits</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox ID="ddlDigits" Width ="100%" runat="server" TabIndex="4" Skin="Simple" Filter="Contains">
                                                        <Items>
                                                           <telerik:RadComboBoxItem Text="--Select--" Value="--Select--" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="0" Value="0" />
                                                            <telerik:RadComboBoxItem Text="2" Value="2" />
                                                            <telerik:RadComboBoxItem Text="3" Value="3" />
                                                            <telerik:RadComboBoxItem Text="4" Value="4" />

                                                        </Items>
                                                         
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-5">
                                                <label>Country</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox ID="ddl_Country" Width ="100%" runat="server" TabIndex="4" Skin="Simple" Filter="Contains">
                                                      
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
