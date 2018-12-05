<%@ Page Title="Manage Holidays" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminHolidays.aspx.vb" Inherits="SalesWorx_BO.AdminHolidays" %>


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
        function TestCheckBox() {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

          var Inputs = TargetBaseControl.getElementsByTagName("input");

          for (var n = 0; n < Inputs.length; ++n)
              if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                  return confirm('Would you like to delete the selected holiday?');
                  return true;
              }
          alert('Select at least one holiday!');
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
                var myRegExp2 = /btnCancel/
                var cancelString = postBackElement.id.search(myRegExp2);
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

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
    <Style>
         input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
    </Style>
   </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

          <h4>Holiday Management</h4>

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
                                    <div class="row">
                                            
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label> Description</label>
                      <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Width="100%" ></telerik:RadTextBox>
                                             </div>
                                         </div>
                        
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label> From Date </label><telerik:RadDatePicker ID="txt_fromDate"   runat="server"  Width="100%">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                </DateInput>
                                                <Calendar ID="Calendar2" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker> 
                                                </div>
                                             </div>

                         
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label> To Date </label>
                     <telerik:RadDatePicker ID="txt_ToDate"   runat="server"  Width="100%">
                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy" >
                                                </DateInput>
                                                <Calendar ID="Calendar3" runat="server">
                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                </Calendar>
                                            </telerik:RadDatePicker> 
                                                </div>
                                             </div>
                                        

                                                <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                       <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                         <telerik:RadButton ID="btnClear" Skin="Simple"  CssClass ="btn btn-default"  OnClick="btnClearFilter_Click"
                                                            runat="server" Text="Clear Filter" TabIndex ="6" />
                                                    </div>
                                                    </div>
                                                </div>
                                        <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                         <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" /> 
                         <telerik:RadButton ID="btn_import" Skin ="Simple"    runat="server" Text="Import" CssClass ="btn btn-warning" Visible="false" />

                     </div>
                        </div>
                                            </div>

                                </ContentTemplate>
                                
                            </asp:UpdatePanel>
                  <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvHoliday" runat="server" EmptyDataText="No Hoilday to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="True"  PageSize="25" CellPadding="0" CssClass="tablecellalign">
                                                 
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                 OnClientClick="return ConfirmDelete('Would you like to delete the selected hoilday ?',event);"
                                                                    OnClick="btnDeleteAll_Click" CssClass="checkboximgvalign" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                <asp:ImageButton ToolTip="Delete Hoilday" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected hoilday?',event);" CssClass="checkboximgvalign" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Hoilday" runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Holiday_Date" HeaderText="Holiday Date" SortExpression="Holiday_Date" DataFormatString="{0:dd-MMM-yyyy}"  >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHoliday" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                            
                                       <telerik:RadWindow ID="MPEDetails" Title= "Holiday Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                            <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                    
                                            <div class="row">
                                                <div class="col-sm-5">
                                                    <label>Date</label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <telerik:RadDatePicker ID="HolidayDate" Width="100%" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                            </DateInput>
                                                            <Calendar ID="Calendar1" runat="server">
                                                                <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5">
                                                    <label>Description</label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtDescription" TabIndex="2" Width ="100%" MaxLength="50"  runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5"></div>
                                                <div class="col-sm-5">
                                                    <div class="form-group">
                                                        <telerik:RadButton ID="btnSave" Skin="Simple" CssClass ="btn btn-primary" 
                                                           OnClick="btnSave_Click"   
                                                            runat="server" Text="Save" TabIndex ="6" >
                                                          </telerik:RadButton>

                                                                         <telerik:RadButton ID="btnUpdate" Skin="Simple" CssClass ="btn btn-primary" 
                                                                               OnClick="btnUpdate_Click"   
                                                                                runat="server" Text="Update" TabIndex ="6" >
                                                          </telerik:RadButton>

                                                                         <telerik:RadButton ID="btnCancel" Skin="Simple"  CssClass ="btn btn-danger" 
                                                                             OnClick="btnCancel_Click" 
                                                                                runat="server" Text="Cancel" TabIndex ="7" >
                                                          </telerik:RadButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                <img alt="Processing..." src="../assets/img/ajax-loader.gif" style="z-index: 10010; vertical-align: middle;" />
                                                <span>Processing... </span>
                                            </asp:Panel>
                                        </div>
                                       </ContentTemplate>
                            </asp:UpdatePanel>
                                              </ContentTemplate>
                                                    </telerik:RadWindow> 

                                     <div>
                                             
                                                    <telerik:RadWindow ID="MPEImport" Title= "Import Holidays" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
                                              <ContentTemplate>
                                                  <div class="popupcontentblk">
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                    <table   border="0" cellspacing="2" cellpadding="2"  width="100%">
                  
                  <tr>
                  <td colspan ="2">
                  <asp:Label runat ="server" ID="Label6" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Blue"  Text =""></asp:Label>
                  
                  </td>
                  </tr>      
                      
		 <tr>
    <td class ="txtSMBold" style="width:45%"> Select a File :</td>
    <td  style ="color:Black;"> <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                    </asp:UpdatePanel>    
          
         </td>
       
          </tr>
          <tr><td colspan ="2"><br /></td></tr>
          <tr>
         
          <td colspan="2">
           <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-primary" /> 
              <asp:Button ID="btnCancelImport"  CssClass ="btn btn-danger"  TabIndex="5" runat="server"
                                    CausesValidation="false" Text="Close" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                 CssClass ="btn btn-primary" />
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
            <span style ="text-decoration: underline !important;Color:#337AB7;"> <asp:LinkButton ID="lbLog" Font-Bold ="true" Font-Size ="13px"   ForeColor  ="#337AB7"
              ToolTip ="Click here to download the uploaded log" runat ="server"
               Text ="View Log" Visible="false" ></asp:LinkButton></span>
           </td>
          </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr><td colspan ="2">
                        <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" Font-Size ="12px" ForeColor ="Green" Font-Bold ="true" > </asp:Label></td></tr>
                        <tr>
                        <td colspan="2">
                       
                         <asp:GridView Width="100%" ID="dgvErros"   runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" Visible="false" 
                                                        AllowPaging="true" AllowSorting="false"  PageSize="15" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                        
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                       
                                                          
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="LogInfo"
                                                                HeaderText="Log Info">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                             
                                                        
                                                          
                                                        </Columns>
                                                        <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                        
                         
                        </td>
                        </tr>
                    </table>
                                                      </div>
                                                </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                             </div>
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
            
</asp:Content>
