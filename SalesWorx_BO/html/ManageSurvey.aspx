<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageSurvey.aspx.vb" Inherits="SalesWorx_BO.ManageSurvey" %>
 

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FeaturedContent" runat="server">

    <style>
        .ruButton .ruRemove, .RadUpload .ruRemove,
        ruButton .ruCancel, .RadUpload .ruCancel
        {
            display: none !important;
        }

        .RadTabStrip .rtsLevel .rtsOut, .RadTabStrip .rtsLevel .rtsIn, .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            /* font-weight: bold; */
            font-size: 13px;
            font-family: 'Segoe UI','Trebuchet MS', Arial !important;
        }
        .RadListBox .rlbList {
            width: 100%;
        }
        .form-inline-list label {
            display: inline-block !important;
            vertical-align: text-bottom;
            margin:0 10px 0 0;
        }
        .form-inline-list input[type=checkbox] {
            display: inline-block !important;
        }
    </style>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

                <script type="text/javascript">
            function CloseWindow() {
                //  var masterTable = $find("<%= rgSurvey.ClientID%>").get_masterTableView();
                // masterTable.fireCommand("RebindGrid");
                var window;
                window = $find('<%= DocWindow.ClientID%>');
                window.hide();
                window.set_modal(false);

            }

            function pageLoad(sender, eventArgs) {

                if (!eventArgs.get_isPartialLoad()) {

                    $find("<%= RadAjaxManager2.ClientID%>").ajaxRequest("InitialPageLoad");

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



            function alertCallBackFn(arg) {
                var masterTable = $find("<%= rgSurvey.ClientID%>").get_masterTableView();
                masterTable.fireCommand("RebindGrid");
            }

            function OnClientCloseHandler() {
                var masterTable = $find("<%= rgSurvey.ClientID%>").get_masterTableView();
         masterTable.fireCommand("RebindGrid");
     }
 
        </script>
    </telerik:RadScriptBlock>
    <style>
    

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
    </telerik:RadScriptBlock>
    <h4>Survey Management</h4>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />
                
                </UpdatedControls>
            </telerik:AjaxSetting>

     
          <%--  <telerik:AjaxSetting AjaxControlID="rgSurvey">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSurvey" LoadingPanelID="AjaxLoadingPanel1" />

                    <telerik:AjaxUpdatedControl ControlID="DocWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
             <telerik:AjaxSetting AjaxControlID="btnAdd">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="DocWindow" LoadingPanelID="AjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="rgSurvey">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgSurvey" LoadingPanelID="AjaxLoadingPanel1" />

                    <telerik:AjaxUpdatedControl ControlID="DocWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>



    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>





    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>










        <div class="mrgbtm">
            <div class="form-inline">
                <div class="form-group">
                    <telerik:RadButton ID="btnAdd" Skin="Simple" Text="Add New Survey" runat="server" CssClass="btn btn-success"></telerik:RadButton>

                </div>
            </div>
        </div>







        <telerik:RadGrid ID="rgSurvey" DataSourceID="sqlSurvey"
            AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
            PageSize="10" AllowPaging="True" runat="server" AllowFilteringByColumn="true"
            GridLines="None">

            <GroupingSettings CaseSensitive="false"></GroupingSettings>
            <ClientSettings EnableRowHoverStyle="true">
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                DataSourceID="sqlSurvey" DataKeyNames="Survey_ID" ClientDataKeyNames="Survey_ID" AllowFilteringByColumn="true"
                PageSize="10">


                <PagerStyle Mode="NumericPages"></PagerStyle>

                <Columns>
                    <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>

                            <asp:ImageButton ID="btnEdit" ToolTip="Edit Survey" OnClick ="btnEdit_Click" runat="server" CausesValidation="false"
                                ImageUrl="~/Images/edit-13.png" />

                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                            <asp:Label ID="lblSurveyDelID" runat="server" Text='<%# Bind("Survey_ID")%>' Visible="false"></asp:Label>
                              <asp:Label ID="lblStatCode" runat="server" Text='<%# Bind("StatusCode")%>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="btnDelete" ToolTip="Delete Survey/Question/Responses" runat="server" CausesValidation="false"
                                CommandName="DeleteSelected"
                                ImageUrl="~/Images/delete-13.png"
                                OnClientClick="return ConfirmDelete('Are you sure to delete this survey?',event);" />

                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn UniqueName="Survey_ID" AllowFiltering="false"
                        SortExpression="Survey_ID" HeaderText="Survey ID" DataField="Survey_ID"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Survey_Title" AllowFiltering="true" CurrentFilterFunction="Contains"
                        AutoPostBackOnFilter="true"
                        SortExpression="Survey_Title" HeaderText="Survey Name" DataField="Survey_Title"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="Survey_Type_Code" AllowFiltering="true" CurrentFilterFunction="Contains"
                        AutoPostBackOnFilter="true"
                        SortExpression="Survey_Type_Code" HeaderText="Survey Type" DataField="Survey_Type_Code"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="Start_Time" AllowFiltering="false" 
                        SortExpression="Start_Time" HeaderText="Start Date" DataField="Start_Time" DataFormatString="{0:dd-MM-yyyy}"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="End_Time" AllowFiltering="false"
                        SortExpression="End_Time" HeaderText="End Date" DataField="End_Time" DataFormatString="{0:dd-MM-yyyy}"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" />
                    </telerik:GridBoundColumn>

                    <telerik:GridBoundColumn UniqueName="Status" AllowFiltering="false"
                        SortExpression="Status" HeaderText="Current Status" DataField="Status"
                        ShowFilterIcon="false">
                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>

                      <telerik:GridTemplateColumn UniqueName="ActiveColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                          <asp:LinkButton ID="lbActive" OnClick ="lbActive_Click" runat ="server" ForeColor ="CornflowerBlue"  Text='<%# Bind("NewStatus") %>' Visible='<%# Bind("Vactive")%>'
                               OnClientClick ="return ConfirmDelete('Would you like to confirm?',event);" ></asp:LinkButton>
                          <asp:Label runat ="server" ID="lbNewStatus"   Text='<%# Bind("NewStatus") %>' Visible='<%# Bind("Lactive")%>' ForeColor ="Red"></asp:Label>
                        </ItemTemplate>
                       
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>
                </Columns>

















            </MasterTableView>


        </telerik:RadGrid>
        <asp:SqlDataSource ID="sqlSurvey" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="app_LoadSurveyListing" SelectCommandType="StoredProcedure"></asp:SqlDataSource>




        <telerik:RadWindow ID="DocWindow" Title="Manage Survey" runat="server" Skin="Windows7" Behaviors="Move,Close"
            Width="800px" Height="590px" OnClientClose ="OnClientCloseHandler" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
            <ContentTemplate>

                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">

                    <asp:Label ID="lblMsg1" ForeColor="Red" Font-Bold="true" runat="server"></asp:Label>
                    <div class="col-sm-12" style="padding-top:10px;">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label style="margin-bottom: 0px;">Survey</label>
                                <asp:Label ID="lblSurveyName" Font-Size="15px" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group text-right">
                                <telerik:RadButton ID="btnSave" ValidationGroup="valsum" TabIndex="18" Skin="Simple" runat="server" Text="Confirm" CssClass="btn btn-success" />
                                <telerik:RadButton ID="btnCancel" Skin="Simple" AutoPostBack="true" runat="server" Text="Cancel" TabIndex="19" CssClass="btn btn-default" />
                            </div>
                        </div>
                    </div>
                    </div>

                    <div style="float: right; padding-right: 40px; padding-top: 5px;">
                        <asp:Label ID="lblMsg" ForeColor="#d9534f" Font-Bold="true" runat="server"></asp:Label>
                        <asp:Label ID="lblVMsg" Font-Bold="true" ForeColor="#5cb85c" runat="server"></asp:Label>
                    </div>
                    <br />
                    <asp:Label ID="lblSurveyID" runat="server" Visible="false"></asp:Label>
                    <asp:HiddenField ID="HSurveyType" runat="server" value="0"></asp:HiddenField>
                    <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Simple">
                        <Tabs>
                            <telerik:RadTab Text="Survey">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Questions and Responses">
                            </telerik:RadTab>

                          <%--  <telerik:RadTab Text="Questions and Responses List">
                            </telerik:RadTab>--%>
                            <telerik:RadTab Text="Assign To">
                            </telerik:RadTab>


                        </Tabs>
                    </telerik:RadTabStrip>
                   
                    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                        <telerik:RadPageView runat="server" ID="rpv1" Height="450px">

                            <div class="form-inline">
                                <div class="popupform">
                                    <div class="col-sm-12">
                                    <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">


                                            <label>
                                                Title<em>*</em> 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="" Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator>



                                            </label>



                                            <telerik:RadTextBox ID="txtTitle" AutoPostBack="true" Skin="Simple" Width="100%" runat="server" TabIndex="1" MaxLength="50">
                                            </telerik:RadTextBox>


                                        </div>

                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Survey Type<em>*</em></label>
                                            <telerik:RadComboBox ID="ddlTypeCode" Skin="Simple" Width="100%" TabIndex="2" runat="server"
                                                Height="200" AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                        </div>

                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">


                                            <label>
                                                Start Date<em>*</em> 
                <asp:RequiredFieldValidator runat="server" ID="StartTimeRequiredFieldValidator" Display="Dynamic" ValidationGroup="valsum"
                    ControlToValidate="txtStartDate" ErrorMessage="" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </label>



                                            <telerik:RadDatePicker ID="txtStartDate" Width="100%" runat="server" Skin="Simple"
                                                TabIndex="2"   >
                                                <Calendar Skin="Simple" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                    ViewSelectorText="x">
                                                </Calendar>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="0" />
                                                <DateInput ReadOnly="true" DateFormat="dd-MM-yyyy" DisplayDateFormat="dd-MM-yyyy">
                                                </DateInput>

                                            </telerik:RadDatePicker>



                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>
                                                End Date<em>*</em> 
                     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" Display="Dynamic" ValidationGroup="valsum"
                         ControlToValidate="txtExpDate" ErrorMessage="" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </label>



                                            <telerik:RadDatePicker ID="txtExpDate" Width="100%" runat="server" Skin="Simple"
                                                TabIndex="3" >
                                                <Calendar Skin="Simple" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                    ViewSelectorText="x">
                                                </Calendar>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" TabIndex="3" />
                                                <DateInput ReadOnly="true" DateFormat="dd-MM-yyyy" DisplayDateFormat="dd-MM-yyyy">
                                                </DateInput>

                                            </telerik:RadDatePicker>

                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">


                                            <label>
                                                Status 
               

                                            </label>



                                            <asp:Label runat ="server" ID="lblSurveyStatus" Font-Bold ="true" ></asp:Label>


                                        </div>

                                    </div>
                                        </div>
                                    </div>
                                </div>
                            </div>



                        </telerik:RadPageView>
                        <telerik:RadPageView runat="server" ID="rpv2" Height="450px">
                           
                            <div class="form-inline">
                                <div class="popupform">
                                    <div class="col-sm-12">
                                     <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>
                                                Question<em>*</em>
                                                
                                            </label>
                                            <telerik:RadTextBox ID="txtQuestion" MaxLength="255" Skin="Simple" Width="100%" runat="server" TabIndex="1">
                                            </telerik:RadTextBox>
                                            <div class="form-inline-list" id="divMandatory" runat="server" >
                                                <span><asp:CheckBox ID="chk_mandatory" runat="server" Text="Mandatory"/></span> 
                                                <span><asp:CheckBox ID="chk_mandatoryOnConf" runat="server" Visible="false"  Text="Mandatory on Confirmation"  /></span> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>
                                                Response Type<em>*</em>
                                            </label>

                                            <telerik:RadComboBox ID="ddlResponsType" Skin="Simple" AutoPostBack="true"
                                                Height="200px" TabIndex="2"
                                                runat="server" Width="100%" /> </div>
                                        </div>
                                        <div class="col-sm-2" id="NoofLines" runat="server" visible="false" >
                                            <div class="form-group">
                                            <label>
                                             <asp:Label ID="lbl_resptxt" runat="server" Text=""></asp:Label>   <em>*</em>
                                            </label>
                                            <telerik:RadTextBox ID="txt_noOfLines" MaxLength="2" Skin="Simple" Width="100%" runat="server" TabIndex="1" Visible="false" Text="1">
                                            </telerik:RadTextBox>
                                                <telerik:RadComboBox ID="ddlStarRating" Skin="Simple" AutoPostBack="true"
                                                Height="200px" TabIndex="2"
                                                runat="server" Width="100%" Visible="false" >
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                                    <telerik:RadComboBoxItem Text="2" Value="2" />
                                                    <telerik:RadComboBoxItem Text="3" Value="3" />
                                                    <telerik:RadComboBoxItem Text="4" Value="4" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="5" Value="5" />

                                                </Items>
                                                </telerik:RadComboBox>
                                        </div>
                                    </div>
                                        </div>
                                       <div class="row" id="VanAudit" runat="server" visible="false">
                                            <div class="col-sm-3" >
                                            <div class="form-group">
                                           <label>
                                                Remarks Required<em>*</em>
                                            </label>
                                           <asp:CheckBox ID="chk_remark" runat="server" AutoPostBack="true"   />
                                              </div>
                                           </div>
                                           <div class="col-sm-3" id="divNoOfRemars" runat="server" visible="false">
                                            <div class="form-group">
                                            <label>
                                              No of Lines for Remark <em>*</em>
                                            </label>
                                            <telerik:RadTextBox ID="txt_NoofLinesRemark" MaxLength="2" Skin="Simple" Width="100%" runat="server" TabIndex="1" Text="1">
                                            </telerik:RadTextBox>

                                        </div>
                                    </div>
                                            
                                           </div>
                                       
                                     
                                    <div  runat="server" id="divQuestionImage" visible="false" class="row"  >
                                        
                                     <div class="col-sm-6"   >
                                        <div class="form-group">
                                            <label>
                                                Group text</label>
                                            <asp:TextBox ID="txt_groupTxt" runat="server" Width="100%" Visible="true"></asp:TextBox>
                                            </div>
                                     </div>
                                        <div  class="col-sm-6"   >
                                        <div class="form-group">
                                            <label>Has Image <span style="display:inline-block;vertical-align: middle;"><asp:CheckBox ID="chk_Image" runat="server" AutoPostBack="true"  /></span></label>
                                            <label style="font-size:12px;" > [Allowed file extensions : png]</label>
                                            <div id="file_Imag" runat="server" visible="false" >
                                            <telerik:RadAsyncUpload runat="server" ID="txtImage" 
            MultipleFileSelection="Disabled" EnableInlineProgress="true" RenderMode="Lightweight"
             Localization-Select ="Upload" Skin ="Simple"    PostbackTriggers="btnAddQuest"    TemporaryFolder="C:\SalesWorx_VS\uploads\SurveyQuestionImages" 
            AllowedFileExtensions="png">
        </telerik:RadAsyncUpload> 

                                                </div>

                                            <asp:Label ID="lbl_imgName" runat="server" Text=""></asp:Label>
                                            </div>
                                     </div>
                                        
                                    </div>
                                         </div>
                                       
                                    <div class="row">
                                    <div class="col-sm-6" runat="server" id="divResponse">
                                        <div class="form-group">
                                            <label>
                                                Response<em>*</em>
                                            </label>

                                            <telerik:RadTextBox ID="txtResponse" MaxLength="50" Skin="Simple" Width="100%" runat="server" TabIndex="3">
                                            </telerik:RadTextBox>   <telerik:RadButton ID="rsDefault" runat="server" Text="Is Default Response" TabIndex="4" AutoPostBack="false"
                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple">
                                            </telerik:RadButton>
                                             <telerik:RadButton ID="chk_showComment" runat="server" Text="Show Comment Box" TabIndex="4" AutoPostBack="false"
                                                ToggleType="CheckBox" ButtonType="ToggleButton" Skin="Simple" Visible="true" >
                                            </telerik:RadButton>

                                        </div>
                                    </div>
                                  
                                    <div class="col-sm-6" runat="server" id="divIsDefault">
                                        <div  >
                                            <label>
                                                
                                              <span style ="visibility:hidden;">1</span>
                                            </label>
                                         
                                           <asp:ImageButton runat ="server"    ToolTip ="Add Response" ImageUrl ="~/images/add-button.png" ID="btnAddResp" />
                                               <asp:ImageButton runat ="server" ToolTip ="Delete Response" ImageUrl="~/images/Delete.png" ID="btnRemoveResp" Visible="false" />
                                             
                                        </div>
                                    </div>
                                       </div>
                                    <div class="row">
                                    <div class="col-sm-12" runat="server" id="divRespList">
                                        <div class="form-group">
                                          

                                           <%-- <telerik:RadButton ID="btnAddResp" Skin="Simple" CssClass="btn btn-primary" runat="server" Text="Add Response" TabIndex="6">
                                            </telerik:RadButton>--%>
                                           
                                           <%-- <telerik:RadButton ID="btnRemoveResp" Skin="Simple" CssClass="btn btn-danger" runat="server" Text="Remove Response" TabIndex="8" />--%>
                                             
                                            <label>Response List </label>
                                              
                                          <%--  <telerik:RadListBox ID="ddlResponseList" CheckBoxes="true" SelectionMode="Single" AutoPostBack ="true" 
                                                Height="90px" Width="80%" Skin="Simple" runat="server" TabIndex="6">
                                              
                                            </telerik:RadListBox>--%>
                                           
                                             <asp:GridView Width="100%" ID="gvResponseList" runat="server" EmptyDataText="No Response to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                    AllowPaging="false" AllowSorting="false"   CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                     
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Default Response">
                                                            
                                                            <HeaderStyle HorizontalAlign="Left" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDR" runat="server"  CssClass="checkboxvalign" Checked='<%# Bind("DefValue")%>'  AutoPostBack="true" OnCheckedChanged="chkDR_Click" />
                                                                <asp:HiddenField runat="server" ID="HresponseID" Value='<%# Bind("Response_ID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Response_Text" HeaderText="Response Text" SortExpression="Response_Text">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                          <asp:TemplateField HeaderText="Show Comment Box">
                                                            
                                                            <HeaderStyle HorizontalAlign="Left"  CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkShowCommentBox" runat="server"  CssClass="checkboxvalign"  Checked='<%# Bind("ShowCommentBox")%>'/>
                                                                 
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            
                                                            <HeaderStyle HorizontalAlign="Left" Width="40px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                
                                                                <asp:ImageButton ToolTip="Delete" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" CssClass="checkboximgvalign" />
                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>

                                           
                                        </div>
                                    </div></div>
                                   
                                  <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group text-right"> 
                                                  <telerik:RadButton ID="btnAddQuest" Skin="Simple" CssClass="btn btn-success" runat="server" Text="Save Question & Response" TabIndex="6">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnClear" Skin="Simple" CssClass="btn btn-default" runat="server" Text="Clear" TabIndex="8" />
                             


                                        </div>
                                    </div>
                                      </div>
                                    
                                         <div class="overflowx">
                                             <div>&nbsp;</div>
                                    <asp:HiddenField ID="hfQuestionID" runat="server" />
                               <%-- <telerik:RadAjaxPanel ID="pl" runat ="server" >--%>
                                        <telerik:RadGrid ID="rgQuestions"
                                            AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                            PageSize="5" AllowPaging="true" runat="server" BorderWidth ="0"
                                            GridLines="None">

                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>



                                            <ClientSettings EnableRowHoverStyle="true">
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                PageSize="5">

                                               
                                                <PagerStyle Mode="NumericPages"></PagerStyle>

                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false"
                                                        InitializeTemplatesFirst="false">


                                                        <ItemTemplate>

                                                            <asp:ImageButton ID="btnEdit" ToolTip="Edit Question/Responses" runat="server" CausesValidation="false"
                                                                CommandName="EditQuestion" ImageUrl="~/Images/edit-13.png" />

                                                        </ItemTemplate>
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                                                        InitializeTemplatesFirst="false">


                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuestionID" runat="server" Text='<%# Bind("Question_ID")%>' Visible="false"></asp:Label>
                                                            <asp:ImageButton ID="btnDelete" ToolTip="Delete Question and all Responses" runat="server" CausesValidation="false"
                                                                CommandName="DeleteQuestion"
                                                                ImageUrl="~/Images/delete-13.png" />

                                                        </ItemTemplate>
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridBoundColumn UniqueName="Question_ID" AllowFiltering="false" Visible="false"
                                                        SortExpression="Question_ID" HeaderText="Question ID" DataField="Question_ID"
                                                        ShowFilterIcon="false">
                                                        <ItemStyle Wrap="false" HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn UniqueName="Question_Text" HeaderStyle-Font-Bold ="true" 
                                                        SortExpression="Question_Text" HeaderText="Question" DataField="Question_Text"
                                                        ShowFilterIcon="false">
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>

                                                    <telerik:GridBoundColumn UniqueName="Response_Type" HeaderStyle-Font-Bold ="true" 
                                                        SortExpression="Response_Type" HeaderText="Response Type" DataField="Response_Type"
                                                        ShowFilterIcon="false">
                                                        <ItemStyle Wrap="false" />
                                                    </telerik:GridBoundColumn>

                                               <telerik:GridTemplateColumn UniqueName="ResponseList" AllowFiltering="false" 
                            InitializeTemplatesFirst="false" HeaderStyle-Font-Bold ="true" HeaderText ="Responses List" >
                                                                              
                                                                 <ItemTemplate>
                                                                        
                                                                       
                                                                               <a id="link1" href="#" runat ="server" >
                                                                            <asp:Image runat="server" ID="ImgResponseList" ToolTip ="View Responses list"  ImageUrl ="~/assets/img/notes.png" /></a> 
                                                                           
                                            <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="link1" Text='<%# DataBinder.Eval(Container.DataItem, "ResponsesList") %>'
                                                RelativeTo="Mouse"  Position="TopLeft" Width="150px" AutoCloseDelay ="30000"  ></telerik:RadToolTip>
                                        
                                            
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" Wrap="false" />
                                                                        <ItemStyle HorizontalAlign="left" Wrap="false" />
                                                   </telerik:GridTemplateColumn> 

                                                </Columns>

















                                            </MasterTableView>


                                        </telerik:RadGrid>
                               <%-- </telerik:RadAjaxPanel>--%>
                                </div>

                                </div>
                                </div>
                          
                            
                        </telerik:RadPageView>
                   
                              
                        <telerik:RadPageView runat="server" ID="rpv" Height="450px">
                            <telerik:RadAjaxPanel runat="server" ID="raf">

                                <div style="padding: 10px 15px;">


                                    <div class="form-group">
                                    <telerik:RadTextBox runat="server" ID="txtFilter" Skin="Simple" Width="30%" EmptyMessage="Type FSR name here"
                                        EmptyMessageStyle-ForeColor="LightGray">
                                    </telerik:RadTextBox>

                                    <telerik:RadButton ID="btnSearch" Skin="Simple" runat="server" Text="Search" CssClass="btn btn-primary" />
                                    <telerik:RadButton ID="btnReset" Skin="Simple" runat="server" Text="Reset" CssClass="btn btn-default" />
                                        </div>
                                    
                                    <table width="100%">
                                        <tr>
                                            <td width="49%">
                                                <asp:Label ID="lblAvailed" Text="Available FSR"  runat="server"></asp:Label></td>
                                            <td width="2%">
                                                <br />
                                            </td>
                                            <td width="49%">
                                                <asp:Label ID="lblAssigned" Text="Assigned FSR"  runat="server"></asp:Label></td>
                                        </tr>


                                        <tr>
                                            <td width="49%" valign="top">
                                                <telerik:RadListBox ID="rlvAvaile" AutoPostBack="false" runat="server" Width="100%" Height="360px" SelectionMode="Multiple">
                                                    <ItemTemplate>

                                                        <a href="#">

                                                            <asp:ImageButton runat="server" ID="imgMoveLeft" ImageAlign="TextTop" BorderStyle="None" OnClick="imgMoveLeft_Click" ToolTip="Move item to left" ImageUrl="~/Images/arrowLeft.png" />
                                                        </a>&nbsp;&nbsp;
		<asp:Label runat="server" ID="lblSalesRepID" Visible="false" Text='<%#Bind("SalesRep_ID")%>'></asp:Label>
                                                        <%# DataBinder.Eval(Container, "Text") %>
                                                    </ItemTemplate>


                                                </telerik:RadListBox>




                                            </td>
                                            <td width="2%"></td>
                                            <td width="49%" valign="top">
                                                <telerik:RadListBox ID="rlvAssigne" AutoPostBack="false" runat="server" Width="100%" Height="360px" SelectionMode="Multiple">
                                                    <ItemTemplate>
                                                        <a href="#">
                                                            <asp:ImageButton runat="server" ID="imgMoveRight" ImageAlign="TextTop" BorderStyle="None" OnClick="imgMoveRight_Click" ToolTip="Move item to right" ImageUrl="~/Images/arrowRight.png" />
                                                        </a>&nbsp;&nbsp;
		<asp:Label runat="server" ID="lblSalesRepID" Visible="false" Text='<%#Bind("SalesRep_ID")%>'></asp:Label>
                                                        <%# DataBinder.Eval(Container, "Text") %>
                                                    </ItemTemplate>


                                                </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </telerik:RadAjaxPanel>



                        </telerik:RadPageView>

             
                    </telerik:RadMultiPage>

                    <br />
                    <div class="text-right col-xs-12 form-group">
                       
                    </div>

                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>

        

    </telerik:RadAjaxPanel>







</asp:Content>


