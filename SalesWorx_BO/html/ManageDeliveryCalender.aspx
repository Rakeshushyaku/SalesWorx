<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageDeliveryCalender.aspx.vb" Inherits="SalesWorx_BO.ManageDeliveryCalender" %>

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
    <h4>Manage DeliveryCalender</h4>
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



   

    <div class="mrgbtm">
        <div class="form-inline">
            <div class="form-group">
              
                <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Exception" runat="server" CssClass="btn btn-danger"></telerik:RadButton>
                <telerik:RadButton ID="btndownloadTemplate" Skin="Simple" Text="Download Template" runat="server" CssClass="btn btn-primary"></telerik:RadButton>

            </div>
            <div class="form-group">
                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server"
                    Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"
                    Localization-Select="Import Exception" OnClientFilesUploaded="OnClientFilesUploaded"
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
                        <p>Exception dates will be uploaded. </p>
                        <hr />
                        <h5>Validations</h5>
                        <ul style="padding: 0 0 0 15px; margin: 0; list-style-type: disc;">
                            <li>Organization,date and Is_working columns are mandatory in Exception dates.</li>
                            <li>Is_working value should be either Y or N.</li>
                            <li>Existing dates are updated.New dates are inserted.</li>
                            <li>Sheet name should be ExceptionDates.</li>
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
                                <ContentTemplate>
                                  
                                    <div class="row">
                                             <div class="col-sm-6 col-md-4 col-lg-6">
                                    <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
                                            </div>
                                            </div>
                                            <div class="col-sm-6 col-md-4 col-lg-3">
                                                <div class="form-group"> 
                                           

                <label>Normal Holidays<em><span>&nbsp;</span></em></label>                                     

 <telerik:RadComboBox Skin="Simple" ID="ddlday" EmptyMessage="Select holidays"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Width ="100%" runat="server" Filter="Contains">
                                       
      <Items>   
        <telerik:RadComboBoxItem runat="server" Text="Sunday" Value="1" />   
        <telerik:RadComboBoxItem runat="server" Text="Monday" Value="2" />   
        <telerik:RadComboBoxItem runat="server" Text="Tuesday"  Value="3" /> 
        <telerik:RadComboBoxItem runat="server"  Text="Wednesday" Value="4"/>
        <telerik:RadComboBoxItem runat="server"  Text="Thursday" Value="5"/>
        <telerik:RadComboBoxItem runat="server"  Text="Friday" Value="6"/>
        <telerik:RadComboBoxItem runat="server" Text="Saturday" Value="7"/>

    </Items>
      </telerik:RadComboBox >




                                                </div>
                                             </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                                 <div class="form-group">   
                                                <label class="hidden-xs hidden-sm">&nbsp;</label>
                                        <telerik:RadButton ID="btnSave_holiday" Skin ="Simple"    runat="server" Text="Save" CssClass ="btn btn-success" OnClick ="btnSave_holiday_Click"  />
                                                          <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add Exception" CssClass="btn btn-success" />
                                            <asp:Button ID="btnImport" runat="server" Visible ="false"  CausesValidation="false" CssClass="btnInputGreen"
                                                    TabIndex="2" Text="Import"  />
                                        </div>
                                    </div>
                                </div>
                                    <div class="row">


                                                 <div class="col-sm-6 col-md-4 col-lg-3">
                                            <div class="form-group">
                                                <label>From Date</label>
                                                <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
              </div>
                                          </div>
              <div class="col-sm-6 col-md-4 col-lg-3">
                                                    <div class="form-group">
                                                        <label>To Date</label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar3" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

              </div>
                                          </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                    <div class="form-group">
                                                <label>Working<em><span>&nbsp;</span></em></label>
                                                <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Y" Text="YES"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="N" Text="NO"></telerik:RadComboBoxItem>
                       
                    </Items>
                </telerik:RadComboBox>
                                            </div>
                                            </div>

                                            <div class="col-sm-6 col-md-4 col-lg-3">
                                                 <div class="form-group">   
                                                <label class="hidden-xs hidden-sm">&nbsp;</label>
                                                      
                                                     <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                            
                                                      <telerik:RadButton ID="btnReset" Skin="Simple" OnClick ="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-default" />
                                                      
                                                     </div>
                                                </div>
                                        </div> 
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>




    <div class="table-responsive">
        <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
            <ContentTemplate>
             <asp:GridView Width="100%" ID="gvDlvClndr" runat="server" EmptyDataText="No Delivery Calender details found."
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"   PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <HeaderStyle width="75" />
                                                              
                                                                
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                              
                                                                
                                                                <asp:ImageButton ToolTip="Delete Configuration " ID="btnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Row_ID")%>'
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" 
                                                                     ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected expection date?');"/>

                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Configuration" runat="server" CausesValidation="false"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Row_ID")%>'
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Delivery_Date" HeaderText="Date" SortExpression="Delivery_Date" DataFormatString="{0:dd/MMM/yyyy}">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                      
                                                        <asp:BoundField DataField="Working" HeaderText="Is Working " SortExpression="Working">
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
                        var win = $find('<%= MPEDetails.ClientID%>');
                        if (win) {
                            if (!win.isClosed()) {
                                win.center();
                            }
                        }

                    });
                </script>
                  <telerik:RadWindow ID="MPEDetails" Title= "Delivery Calendar Exception Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   
                                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="conditional">
                                                  <ContentTemplate>
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label ID="lblPop" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                                    
                                                   


                                                       <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Organization</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                     <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization_add"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Date</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                        <telerik:RadDatePicker ID="txtExDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label>Is Working</label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div class="form-group">
                                                            <asp:RadioButtonList ID="rdo_Working" runat="server" TabIndex="18" RepeatDirection="Horizontal"  >
                                                                  <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                                  <asp:ListItem Value="N" Selected="True" >No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                   
                                                  
                                                    <div class="row">
                                                        <div class="col-sm-5">
                                                            <label></label>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="7" OnClick="btnSave_Click" runat="server" Text="Save" />
                                                            <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update" OnClick="btnUpdate_Click" runat="server" />
                                                            <asp:Button ID="btnCancel" CssClass ="btn btn-default" Text="Cancel" OnClick="btnCancel_Click"  runat="server" />

                                                        <%--   <telerik:RadButton ID="" Skin="Simple" runat="server" Text="Cancel" on TabIndex="18"  CssClass="btn btn-default" />    --%>    
                                        
                                        
                                                                                       </div>
                                                        
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
