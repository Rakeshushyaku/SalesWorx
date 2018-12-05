<%@ Page Title="POSM Question Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminPOS_MasterSetUp.aspx.vb" Inherits="SalesWorx_BO.AdminPOS_MasterSetUp" %>
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
        function TestCheckBox(event) {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkDelete";
             var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

           for (var n = 0; n < Inputs.length; ++n)
               if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                   //  return confirm('Would you like to delete the Questions?');
                   return ConfirmDelete('Would you like to delete the Questions?',event)
                  return true;
               }
           radalert('Select at least one Question!', 330, 180, 'Validation', alertCallBackFn);
         //  alert('Select at least one Question!');
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
//        
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
                    $get('<%= Me.UpdatePanel1.FindControl("Panel12").ClientID%>').style.display = 'block';
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
                var myRegExp2 = /btnCancel/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                var cancelString = postBackElement.id.search(myRegExp2);
                if (AddString != -1 || EditString != -1) {
                    $get('<%= Me.UpdatePanel1.FindControl("Panel12").ClientID%>').style.display = 'none';
                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;

                // Hiding the radwindow
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
            if (elem != null) {
                $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
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
  
        
                             <h4>POSM Question Setup</h4>
     <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel2" runat="server">      </asp:Panel> 
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
     
     
    
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label> Filter By</label>
                                    <div class="row">
                                     <div class="col-sm-4">
                                            <div class="form-group">  
                                         

                                         <telerik:RadComboBox ID="ddQGroup" Skin="Simple"  AutoPostBack ="true"
                                            Width="100%" Height="200px" TabIndex="2"
                                                runat="server">  </telerik:RadComboBox>  
                                                </div>
                                         </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">  
                                         <telerik:RadComboBox ID="ddQuestion" Skin="Simple"  
                                            Width="100%" Height="200px" TabIndex="3"
                                                runat="server">  </telerik:RadComboBox>

                                            </div>
                                         </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                
                                    

                                         <telerik:RadTextBox   ID="txtFilterVal" runat="server" ToolTip ="Enter Filter Value" 
                                                    autocomplete="off" CssClass="inputSM"
                                                    TabIndex="3" Width="100%"></telerik:RadTextBox>
                                                </div>
                                         </div>
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                         <telerik:RadButton ID="btnFilter" Skin ="Simple"  CausesValidation="False"   OnClick="btnFilter_Click" runat="server" Text="Filter"
                                               CssClass ="btn btn-primary" />

                                         <telerik:RadButton ID="Btn_reset" Skin ="Simple"  CausesValidation="False"   
                                             runat="server" Text="Clear Filter" CssClass ="btn btn-default" />

                                           <telerik:RadButton ID="btnAdd" Skin ="Simple"  CausesValidation="False"   OnClick="btnAdd_Click" 
                                             runat="server" Text="Add" CssClass ="btn btn-success" />
                                                    
                                                
                        

                                     </div>
                                    </div>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
               <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                        
                                                <asp:GridView Width="70%" ID="gvPOSM" runat="server" EmptyDataText="No Data to Display"
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
                                                                    OnClientClick="return TestCheckBox(event)"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete POSM Question" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png"  OnClientClick="return ConfirmDelete('Would you like to delete the selected Questions?',event);"  />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit POSM Question" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QGroup" HeaderText="Question Group" SortExpression="QGroup">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="QuestionValues" HeaderText="Question Value"
                                                            SortExpression="QuestionValues">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Code" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                <asp:Label ID="lbl_CodeType" runat="server" Text='<%# Bind("Code_type") %>'></asp:Label>
                                                                <asp:Label ID="lbl_GroupCode" runat="server" Text='<%# Bind("QGroupCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                




                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />

                                    <telerik:RadWindow ID="MPEDetails" Title= "POSM Question" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                        <ContentTemplate>
                                          
                                            
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                                         <ContentTemplate>
                                                  <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                             <div class="popupcontentblk">
                                             <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                               <div class="row">
		                                            <div class="col-sm-5">
			                                            <label>Question Group</label>
		                                            </div>
		                                            <div class="col-sm-7">
			                                            <div class="form-group">
                                                            <telerik:RadComboBox Width="100%" ID="ddl_QGroup" Skin="Simple"  AutoPostBack ="true" TabIndex="2" Height="200px" runat="server">  </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
		                                            <div class="col-sm-5">
			                                            <label>Question</label>
		                                            </div>
		                                            <div class="col-sm-7">
			                                            <div class="form-group">
                                                            <telerik:RadComboBox ID="ddl_Question" Skin="Simple"  TabIndex="2" Width="100%" Height="200px" runat="server">  </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
		                                            <div class="col-sm-5">
			                                            <label>Code</label>
		                                            </div>
		                                            <div class="col-sm-7">
			                                            <div class="form-group">
                                                            <asp:TextBox Width="100%" ID="txtCode" TabIndex="2" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtDescription"
                                                    ID="ReqDescription" runat="server" ErrorMessage="Description Required"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
		                                            <div class="col-sm-5">
			                                            <label>Description</label>
		                                            </div>
		                                            <div class="col-sm-7">
			                                            <div class="form-group">
                                                            <asp:TextBox Width="100%" ID="txtDescription" TabIndex="3" CssClass="inputSM" runat="server"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator Display="None" Text=" " ControlToValidate="txtRate" ID="ReqRate"
                                                    runat="server" ErrorMessage="Conversion Rate Required"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ValidationExpression="[0-9]+" ControlToValidate="txtRate" ID="RegExpression" Display="None" runat="server" ErrorMessage="Conversion Rate should be in numbers."></asp:RegularExpressionValidator>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FTBtxtConvertRate" FilterType="Numbers,Custom"
                                                    ValidChars="." runat="server" TargetControlID="txtRate">
                                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
		                                            <div class="col-sm-5"></div>
		                                            <div class="col-sm-7">
			                                            <div class="form-group">
                                                            <telerik:RadButton ID="btnSave" Skin ="Simple"    OnClick="btnSave_Click" runat="server" Text="Save"
                                                    CssClass ="btn btn-success" TabIndex="5" />
                                                   <telerik:RadButton ID="btnUpdate" Skin ="Simple"    OnClick="btnUpdate_Click" runat="server" Text="Update"
                                                    CssClass ="btn btn-success" TabIndex="5" />
                                                     <telerik:RadButton ID="btnCancel" Skin ="Simple" CausesValidation="false"   runat="server" Text="Cancel"
                                                    CssClass ="btn btn-default" TabIndex="5" OnClientClick="return DisableValidation()" />
                                        
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
                                            
            
                                
                                        <table>
                                            <tr>
                                                         <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                            
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
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
