<%@ Page Title="POSM Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManagePOSMSurvey.aspx.vb" Inherits="SalesWorx_BO.ManagePOSMSurvey" %>

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
                   // return confirm('Would you like to delete the Questions?');
                   return ConfirmDelete('Would you like to delete the Questions?', event)
                   return true;
               }
            // alert('Select at least one Question!');
           radalert('Select at least one Question!', 330, 180, 'Validation', alertCallBackFn);
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
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
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
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
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
        function checkform() {
            var ddQGroup = document.getElementById('<%= Me.ddQGroup.ClientID %>');
            if (ddQGroup.value == '0') {
                alert("Please select the filter condition")
            return false
        }
            return true 
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
 
                                  <h4>POSM Survey Setup</h4>
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
                                    <label>Filter By</label>
                                      <div class="row">
                                     <div class="col-sm-4">
                                            <div class="form-group">  
                                              <telerik:RadComboBox ID="ddQGroup" Skin="Simple"  
                                            Width="100%" Height="200px" TabIndex="2"
                                            runat="server">    </telerik:RadComboBox>
                                                </div> 
                                         </div>
                                 <div class="col-sm-4">
                                            <div class="form-group">  
                                               <telerik:RadButton ID="btnFilter" Skin ="Simple" CausesValidation="False"   runat="server" Text="Filter"
                                                    CssClass ="btn btn-primary"  OnClick="btnFilter_Click" />
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add"   CausesValidation="false"
                                                     CssClass="btn btn-success" />  
                                                                              
                                 </div> 
                                         </div> 
                                          </div> 


                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                              
                                                <asp:HiddenField ID="HColCount" runat="server" Value="-1" />
                                                <asp:GridView Width="100%" ID="gvPOSM" runat="server" EmptyDataText="No Data to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  
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
                                                                <asp:ImageButton ToolTip="Delete Survey Question" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png"  OnClientClick="return ConfirmDelete('Would you like to delete the selected Survey Question?',event);"  />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Survey Question" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("QID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                                                                         
                                                       
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                               
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />

                                     <telerik:RadWindow ID="MPEDetails" Title= " POSM Survey" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" MinHeight="302" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
                                                  <ContentTemplate>

                                                       <asp:Panel runat="server" ID="DetailPnl"  >
                                                           <div class="popupcontentblk">
                                                          <p><asp:Label ID="lblPop" runat ="server" ForeColor ="Red" ></asp:Label></p>
                                                          <asp:HiddenField ID="HidVal" runat="server" Value="-1" />

                                                            <div class="row">
		                                                        <div class="col-sm-5">
			                                                        <label>Question Group</label>
		                                                        </div>
		                                                        <div class="col-sm-7">
			                                                        <div class="form-group">
                                                                        <telerik:RadComboBox ID="ddl_QGroup" Skin="Simple"  AutoPostBack ="true"   Height="250px" TabIndex="2"
                                                            runat="server" Width="100%"> </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <asp:Panel ID="pnl_details" runat="server" Visible="false">
                                                            <div class="row">
		                                                        <div class="col-sm-5">
			                                                        <label><asp:Label ID="Label2" runat="server" Text="Response Type"></asp:Label></label>
		                                                        </div>
		                                                        <div class="col-sm-7">
			                                                        <div class="form-group">
                                                                        <telerik:RadComboBox ID="ddl_Response" Skin="Simple"   Height="250px" TabIndex="2"
                                                            runat="server"  Width="100%"> </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
		                                                        <div class="col-sm-5">
			                                                        <label><asp:Label ID="lbl_Question1" runat="server" Text=""></asp:Label></label>
		                                                        </div>
		                                                        <div class="col-sm-7">
			                                                        <div class="form-group">
                                                                        <telerik:RadComboBox ID="ddl_Question1" Skin="Simple"  Height="250px" TabIndex="2"
                                                            runat="server" Width="100%"> </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
		                                                        <div class="col-sm-5">
			                                                        <label><asp:Label ID="lbl_Question2" runat="server" Text="" Visible="false"></asp:Label></label>
		                                                        </div>
		                                                        <div class="col-sm-7">
			                                                        <div class="form-group">
                                                                        <telerik:RadComboBox ID="ddl_Question2" Skin="Simple"   Height="250px" TabIndex="2"
                                                            runat="server" Width="100%"> </telerik:RadComboBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            </asp:Panel>
                                                            <div class="row">
		                                                        <div class="col-sm-5"></div>
		                                                        <div class="col-sm-7">
			                                                        <div class="form-group">
                                                                        <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                               OnClick="btnSave_Click" 
                                                                runat="server" Text="Save" TabIndex ="6" >
                                                         </telerik:RadButton>
                                                          <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                               OnClick="btnUpdate_Click" 
                                                                runat="server" Text="Update" TabIndex ="6" >
                                                         </telerik:RadButton>
                                                     <telerik:RadButton ID="btnCancel" Skin="Simple" CssClass ="btn btn-default" 
                                                               OnClientClick="return DisableValidation()" CausesValidation="false" Text="Cancel" 
                                                                runat="server" TabIndex ="6" >
                                                         </telerik:RadButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                             
                                                        <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                            <img alt="Processing..." src="../assets/img/ajax-loader.gif"  />
                                                            <span>Processing... </span>
                                                        </asp:Panel>
                                                    
                                                               </div>
                                                      </asp:Panel>
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

