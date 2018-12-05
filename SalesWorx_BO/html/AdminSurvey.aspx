<%@ Page Title="Manage Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AdminSurvey.aspx.vb" Inherits="SalesWorx_BO.AdminSurvey" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

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

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
            postBackElement.disabled = true;
        }

        function EndRequest(sender, args) {
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
            postBackElement.disabled = false;
            var Filter = /ddFilterBy/
            var AddString = postBackElement.id.search(Filter);
            if (AddString == -1) {
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
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

        function TestCheckBox(event) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

          for (var n = 0; n < Inputs.length; ++n)
              if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return ConfirmDelete('Would you like to delete the selected survey(s)?', event);
                  return true;
              }
            // alert('Select at least one survey!');
          radalert('Select at least one survey!', 330, 180, 'Validation', alertCallBackFn);
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
   
                             <h4>Manage Survey</h4>
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
                                              <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="100px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Survey" Text="Survey"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Type" Text="Type"></telerik:RadComboBoxItem>
                       
                    </Items>
                                                  
                   </telerik:RadComboBox>
                                         </div>
                                            </div>
                                        <div class="col-sm-4">
                                     <div class="form-group"> 
                                             <telerik:RadTextBox runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%">

                                             </telerik:RadTextBox>
                                          
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
                                 
                                                <asp:GridView Width="100%" ID="gvSurvey" runat="server" EmptyDataText="No Survey to Display"
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
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete Survey" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected survey?',event);" CssClass="checkboximgvalign" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Survey" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                                 <asp:ImageButton ID="btnView" ToolTip="View Survey" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/view-13.png" OnClick="btnView_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Survey_Title" HeaderText="Survey" SortExpression="Survey_Title">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Survey_Type_Code" HeaderText="Type" SortExpression="Survey_Type_Code">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Start_Time" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Start Date"
                                                            SortExpression="Start_Time">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="End_Time" DataFormatString="{0:MM/dd/yyyy}" HeaderText="End Date"
                                                            SortExpression="Start_Time">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                        
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSurveyId" runat="server" Text='<%# Bind("Survey_id")%>'></asp:Label>
                                                                <asp:Label ID="TypeCode" runat="server" Text='<%# Bind("Survey_Type_Code")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            
                                       <telerik:RadWindow ID="MPEDetails" Title= "Survey Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                 <asp:UpdatePanel ID="Panel" runat="server">
                                                     <ContentTemplate>

                                                     <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                         <div class="popupcontentblk">
                                                    <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>

                                                    <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>Title</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <asp:TextBox ID="txtTitle" runat="server" Width ="100%" TabIndex="1" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>Start Date</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <telerik:RadDatePicker ID="txtStartDate"   runat="server" Skin="Simple"
                                                                  TabIndex="2">
                                                                    <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                                        viewselectortext="x">
                                                                </calendar>
                                                                    <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                                    <dateinput readonly="true" dateformat="MM/dd/yyyy" displaydateformat="MM/dd/yyyy">
                                                                </dateinput>
                                             
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>End Date</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <telerik:RadDatePicker ID="txtExpDate"  runat="server" Skin="Simple"
                                                                  TabIndex="2">
                                                                    <calendar Skin="Simple" usecolumnheadersasselectors="False" userowheadersasselectors="False"
                                                                        viewselectortext="x">
                                                                </calendar>
                                                                    <datepopupbutton hoverimageurl="" imageurl="" tabindex="0" />
                                                                    <dateinput readonly="true" dateformat="MM/dd/yyyy" displaydateformat="MM/dd/yyyy">
                                                                </dateinput>
                                             
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>Type</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <telerik:RadComboBox ID="ddlTypeCode" Skin="Simple"   Width="100%" Height="250px" TabIndex="2"
                                                        runat="server"> </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5">
			                                                <label>Assign To</label>
		                                                </div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <telerik:RadListBox ID="chkSalRep"  CssClass="multiColumn"  Visible="true"   TabIndex="4"  runat="server"  
                                                            CheckBoxes="true"  Skin ="Default" BackColor ="White"  BorderStyle ="None" BorderColor ="LightGrey" BorderWidth ="1px"
                                                                    DataTextField="UserName"  ForeColor ="Black" Height="100px"
                                                                     DataValueField="User_ID" style="top: 0px; left: 0px" Width="100%" >
                                                         </telerik:RadListBox>

                                         <%--        <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto" BorderStyle="Groove"
                                                                    BorderWidth="1px" Width ="400px">
                                                                    <asp:CheckBoxList ID="chkSalRep"  runat="server" RepeatColumns="2" >
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
		                                                <div class="col-sm-5"></div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-success" 
                                                                                       OnClick="btnSave_Click"  ValidationGroup ="valsum"
                                                                                        runat="server" Text="Save" TabIndex ="6" >
                                                                  </telerik:RadButton>

                                                                                 <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-success" 
                                                                                       OnClick="btnUpdate_Click"  ValidationGroup ="valsum"
                                                                                        runat="server" Text="Update" TabIndex ="6" >
                                                                  </telerik:RadButton>

                                                                                 <telerik:RadButton ID="btnCancel" Skin="Simple" CssClass ="btn btn-default" 
                                                                                     OnClientClick="return DisableValidation()"
                                                                                        runat="server" Text="Cancel" TabIndex ="6" >
                                                                  </telerik:RadButton>
                                                            </div>
                                                        </div>
                                                    </div>

                                                         
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color:  #e10000  ;">Processing... </span>
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
