<%@ Page Title="Org-Level Discount Limits" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminOrgLvlDiscountRule.aspx.vb" Inherits="SalesWorx_BO.AdminOrgLvlDiscountRule" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
  <script>
      function NumericOnly(e) {

          var keycode;

          if (window.event) {
              keycode = window.event.keyCode;
          } else if (e) {
              keycode = e.which;
          }
          if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
              return true;

          return false;
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
                return confirm('Would you like to delete?');
                return true;
            }
            alert('Select at least one row!');
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
                 $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }
    </script>

   </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <h4>Org-Level Discount Limits</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />   
                 <span>Processing... </span>   
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group text-right">
                                             
                                                    <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass ="btn btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                    
                                       </div>     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                <asp:Panel ID="pnl_client" runat="server">
                                  
                                                <asp:GridView Width="845px" ID="grdCustomerSegment" runat="server" EmptyDataText="No Data to show"
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
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Description" HeaderText="Organisation" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                             <ItemStyle Width="300px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_1" HeaderText="Min. Order Value" HtmlEncode="false" >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_5" HeaderText="Min. Discount (%)" HtmlEncode="false" >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Value_6" HeaderText="Max. Discount (%)" >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                       
                                                       
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                 <asp:Label ID="lbl_Info_Key" runat="server" Text='<%# Bind("Info_Key") %>'></asp:Label>
                                                                <asp:Label ID="lblRow_ID" runat="server" Text='<%# Bind("Row_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            
                                 </asp:Panel>

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
                               
                                   <telerik:RadWindow ID="MPEDetails" Title= "Org-Level Discount Limits" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <div class="popupcontentblk">
                                                               <p><asp:Label ID="lblMessage"   runat ="server" ForeColor ="Red" ></asp:Label></p>         
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                     <div class="row">
                                            <div class="col-sm-5">
                                                <label><asp:Label runat="server" ID="lbl_Info_Key" Text="Organisation"></asp:Label></label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                     <telerik:RadComboBox Skin="Simple" ID="ddl_Client" runat="server" TabIndex="1" CssClass="inputSM" Width="200"
                                                        ></telerik:RadComboBox>
                                                           </div>
                                            </div>
                                         <div class="col-sm-5">
                                                <label>Min Order Value</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                     <asp:TextBox ID="txt_OrderValue" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="50"  Width="200"
                                                       onKeypress='return NumericOnly(event)' ></asp:TextBox>
                                                           </div>
                                            </div>
                                          <div class="col-sm-5">
                                                <label>Min. Discount (%)</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                     <asp:TextBox ID="txt_min" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="50"  Width="200"
                                                       onKeypress='return NumericOnly(event)' ></asp:TextBox>
                                                           </div>
                                            </div>
                                         <div class="col-sm-5">
                                                <label>Max. Discount (%)</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                     <asp:TextBox ID="txt_max" runat="server" TabIndex="1" CssClass="inputSM" MaxLength="150"  Width="200"
                                                      onKeypress='return NumericOnly(event)'  ></asp:TextBox>
                                                           </div>
                                            </div>

                                         <div class="col-sm-5">
                                                
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="">
                                                    <asp:Button ID="btnSave" CssClass ="btn btn-success"  TabIndex="5" 
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnUpdate" CssClass ="btn btn-success"  OnClick="btnUpdate_Click"
                                                        runat="server" Text="Update"  />
                                                    <asp:Button ID="btnCancel" CssClass ="btn btn-default"  TabIndex="6" OnClientClick="return DisableValidation()"
                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                </td>
                                                </div>
                                            </div>
                                         </div>
                                       </div>   
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </ContentTemplate>
                                   </telerik:RadWindow>
                                
                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                       
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

