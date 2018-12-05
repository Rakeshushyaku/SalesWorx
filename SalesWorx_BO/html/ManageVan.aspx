<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageVan.aspx.vb" Inherits="SalesWorx_BO.ManageVan" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

        <style>
            .RadUpload .ruCancel {
                background: transparent !important;
                color: red !important;
                border: none !important;
                font-size: 11px !important;
            }
            .RadUpload .ruBrowse {
                width: auto;
                padding: 0 10px;
            }
            .ruFileInput {
                cursor:pointer;
            }
            .RadUpload .ruInputs li {
                padding: 0;
            }
            .RadUpload_Simple .ruFakeInput {
                border: 0 !important;
            }
        </style>
        <script type="text/javascript">

            function CloseWindow(sender, args) {
                var window;
                window = $find('<%= DocWindow.ClientID%>');
                
               
                window.close();
            }
            function OpenWindow() {
                var window;
                window = $find('<%= DocWindow.ClientID%>');

             
                document.getElementById("<%= btnSave.ClientID%>").value = "Save";
               


                document.getElementById("<%= txtEmpCode.ClientID%>").value = "";
                document.getElementById("<%= txtEmpName.ClientID%>").value = "";
                document.getElementById("<%= txtEmpphone.ClientID%>").value = "";
                document.getElementById("<%= txtPrefix.ClientID%>").value = "";
                document.getElementById("<%= txtVanID.ClientID%>").value = "";
                document.getElementById("<%= txtvanname.ClientID%>").value = "";
              
                


                document.getElementById("<%= lblMsg.ClientID%>").innerHTML = ""; 
                document.getElementById("<%= lblVMsg.ClientID%>").innerHTML = ""; 


                $find("<%= ddlOrg.ClientID%>").clearSelection();
                $find("<%= ddlOrg.ClientID%>").enable();
                $find("<%= txtVanID.ClientID%>").enable();
                document.getElementById("<%= btnSave.ClientID%>").innerHTML = "Save";
               
         window.show();
     }


     function CloseWindowExpUom(sender, args) {
         var window;
         window = $find('<%= DocExpUomWindow.ClientID%>');
                window.close();
            }
            function OpenWindowExpUom() {
                var window;
                window = $find('<%= DocExpUomWindow.ClientID%>');
                window.show();
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
         HideRadWindow()
     }
     function HideRadWindow() {

         var elem = $('a[class=rwCloseButton');

         if (elem != null && elem != undefined) {
             $('a[class=rwCloseButton')[0].click();
         }

         $("#frm").find("iframe").hide();
     }
     function RestrictSpaceSpecial(e) {
         var k;
         document.all ? k = e.keyCode : k = e.which;
         return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
     }
     function OnKeyPress(sender, eventArgs) {
         var char = eventArgs.get_keyCharacter();
         if (char == '$' || char == '#' || char == '^' || char == '[' || char == ']'  || char == '!'  || char == '@' || char == '%'|| char == '&'|| char == '*'|| char == '('|| char == ')' || char == '~'|| char == '_'|| char == '+'|| char == '='|| char == '{'|| char == '}') {
             eventArgs.set_cancel(true);
         }
     }
     function ValidatePhoneNo() {
         if ((event.keyCode > 47 && event.keyCode < 58) || event.keyCode == 43 || event.keyCode == 32)
             return event.returnValue;
         return event.returnValue = '';
     }
     function onlyAlphabets(e, t) {

         try {

             if (window.event) {

                 var charCode = window.event.keyCode;

             }

             else if (e) {

                 var charCode = e.which;

             }

             else { return true; }

             if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))

                 return true;

             else

                 return false;

         }

         catch (err) {

             alert(err.Description);

         }

     }
        </script>
    </telerik:RadScriptBlock>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
        <script type="text/javascript">
            function OnClientFilesUploaded(sender, args) {

             
                $find('<%= RadAjaxManager2.ClientID%>').ajaxRequest();
            }
        </script>
    </telerik:RadScriptBlock>
    <h4>Manage Van</h4>
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
               
                <telerik:RadButton ID="btnExport" Skin="Simple" Text="Export Van" runat="server" CssClass="btn btn-danger"></telerik:RadButton>
                <telerik:RadButton ID="btndownloadTemplate" Skin="Simple" Text="Download Template" runat="server" CssClass="btn btn-warning"></telerik:RadButton>
                
            </div>
            <div class="form-group">
                <telerik:RadAsyncUpload ID="ExcelFileUpload" runat="server" 
                    Skin="Simple" OnFileUploaded="ExcelFileUpload_FileUploaded"
                    Localization-Select="Import Van"  OnClientFilesUploaded="OnClientFilesUploaded"
                    MultipleFileSelection="Disabled" InitialFileInputsCount="1" 
                    MaxFileInputsCount="1" />
            </div>

            
            <div class="pull-right">
                <label>
                    <a id="link1" href="#">
                        <asp:Image alt="Upload Info" ToolTip="Upload Info" ImageUrl="~/images/info.png" ID="upl" runat="server" Width="18px" Height="18px" /></a>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" RelativeTo="Element" Width="300px" AutoCloseDelay="30000" BackColor="WhiteSmoke"
                        Height="320px" TargetControlID="link1" IsClientID="true" Animation="None" Position="TopCenter">
                        <h5>Upload Information</h5>
                        <p>New Van will be uploaded. Existing Van and invalid rows are ignored.</p>
                        <hr/>
                            <h5>Validations</h5>
                            <ul style="padding:0 0 0 15px;margin:0;list-style-type:disc;">
                                <li>Van_Code,Van_Name,Emp_Code,Emp_Name and Sales_Org_ID  columns are mandatory.</li>
                                <li> VanCode sholud not contains space and special characters.</li>
                                <li>Existing Van are updated.New Van are inserted.</li>
                                <li>Van Sheet name should be Van_info.</li>
                                
                            </ul>

                    </telerik:RadToolTip>

                    <asp:LinkButton ID="lbLog" runat="server" Text="View Uploaded Log" Font-Underline="true" CssClass="btn btn-link" ToolTip="Click here to view the uploaded log" ForeColor="Blue"
                         OnClick="lbLog_Click" ></asp:LinkButton>
                    <telerik:RadButton ID="btnClear" Skin="Simple" Visible="false" runat="server" CssClass="btn btn-default" Text="Reset">
                    </telerik:RadButton>
                </label>
            </div>
        </div>


    </div>

      <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <label>Filter By</label>
                                    <div class="row">
                                           <div class="col-sm-4">
                                    <div class="form-group">  
                                              <telerik:RadComboBox ID="ddFilterBy" Skin="Simple"  
                    Width="100%" Height="250px" TabIndex="2"
                    runat="server">
                    <Items>

                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Organization" Text="Organization"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Van Code" Text="Van Code"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Van Name" Text="Van Name"></telerik:RadComboBoxItem>
                        <telerik:RadComboBoxItem Value="Emp Code" Text="Emp Code"></telerik:RadComboBoxItem>
                    </Items>
                </telerik:RadComboBox>
                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group"> 
                                                 <telerik:RadTextBox runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%"></telerik:RadTextBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-4">
                                                 <div class="form-group">   
                                              
                                                      <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" />
                                                        <telerik:RadButton ID="btnReset" Skin ="Simple"    runat="server" Text="Reset" CssClass ="btn btn-default" />
                                                      <telerik:RadButton ID="btnAdd"  Skin="Simple" Text="Add New Van" runat="server" CssClass="btn btn-success"  OnClick="btnAdd_Click" ></telerik:RadButton>
                                                   
                                           
                                                     </div>
                                                </div>
                                        </div> 

                                    <style type="text/css">     
   div.RadGrid .rgPager .rgAdvPart    
   {    
    display:none;       
   }     
</style> 


                                       <div class="table-responsive">         
                              <telerik:RadGrid id="gvVan" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                                  <telerik:GridTemplateColumn UniqueName="EditColumn" AllowFiltering="false" 
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                             <asp:Label ID="lblVanID_e" runat="server" Text='<%# Bind("Van_Org_ID")%>' Visible="false"></asp:Label>
                              <asp:Label ID="lblOrgID_e" runat="server" Text='<%# Bind("Sales_Org_ID")%>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="btnEdit" ToolTip="Edit Van" runat="server" CausesValidation="false"
                          CommandName="EditSelected"        ImageUrl="~/Images/edit-13.png" />

                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>    
                                                            
                                                            
                                                            
                                                             <telerik:GridTemplateColumn UniqueName="DeleteColumn" AllowFiltering="false"
                        InitializeTemplatesFirst="false">


                        <ItemTemplate>
                            <asp:Label ID="lblVanID" runat="server" Text='<%# Bind("Van_Org_ID")%>' Visible="false"></asp:Label>
                            
                            <asp:Label ID="lblOrgID" runat="server" Text='<%# Bind("Sales_Org_ID")%>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="btnDelete" ToolTip="Delete Van" runat="server" CausesValidation="false"
                                CommandName="DeleteSelected"
                                ImageUrl="~/Images/delete-13.png"
                             
                                OnClientClick="return ConfirmDelete('Are you sure to delete this item?',event);" />

                            
                           <%-- <asp:ImageButton ID="ImageButton1" ToolTip="Activate Product" runat="server" CausesValidation="false"
                                CommandName="ActivateSelected"
                                ImageUrl="~/Images/close.jpg"
                               Visible='<%# Bind("ActivateVisible")%>'
                                OnClientClick="return ConfirmDelete('Are you sure to active this item?',event);" />--%>

                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </telerik:GridTemplateColumn>        
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_Org_ID" Visible ="false"  HeaderText="Organization ID" SortExpression ="Sales_Org_ID"
                                                               >
                                                                 
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrgName" HeaderText="Organization"
                                                                  SortExpression ="OrgName" >  </telerik:GridBoundColumn>
                                                                  <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Van_Org_ID" HeaderText="Van Code"
                                                                  SortExpression ="Van_Org_ID" >  </telerik:GridBoundColumn>
                                                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VanName" HeaderText="Van Name"
                                                                  SortExpression ="VanName" >  </telerik:GridBoundColumn>
                                                                          

                                                               
                                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Code" HeaderText="Emp Code"
                                                                  SortExpression ="Emp_Code" >  </telerik:GridBoundColumn>
                                                                                    <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Trx_Currency_Code" HeaderText="Currency"
                                                                  SortExpression ="Trx_Currency_Code" >  </telerik:GridBoundColumn>
                                                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Prefix_No" HeaderText="Doc Prefix"
                                                                  SortExpression ="Prefix_No" >
                                                            </telerik:GridBoundColumn>
                                                             
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                               </div>

                                      <telerik:RadWindow ID="DocWindow" Title="Add/Edit Van" runat="server" Skin="Windows7" Behaviors="Close"
             AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
            <ContentTemplate>


                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
            <div class="popupcontentblk">
                
                <p>
                    <asp:Label ID="lblMsg" ForeColor ="Red" runat="server"></asp:Label>
                    <asp:Label ID="lblVMsg" ForeColor ="Red" runat="server"></asp:Label>
                </p>
                
                <div><small>* Indicates the mandatory fields</small></div>
                            <div class="row">
                              <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Organization *
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlOrg" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                        <telerik:RadComboBox ID="ddlOrg" Skin="Simple" Filter="Contains" Width="100%" TabIndex="1" EmptyMessage="Select Organization"
                                            runat="server">
                                        </telerik:RadComboBox>

                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Van Code *
                                         <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProdNo"
                                                ErrorMessage="" ForeColor="Red" Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator>--%></label>
                                        <telerik:RadTextBox ID="txtVanID" MaxLength="10" runat="server" Skin="Simple"
                                            TabIndex="2">
                                        </telerik:RadTextBox>
                                   
                                          <asp:Label ID="lblOrgID" Visible="false" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Van Name * 
                                          </label>
                                        <telerik:RadTextBox ID="txtvanname" MaxLength="25" runat="server" Skin="Simple" ClientEvents-OnKeyPress="OnKeyPress"
                                            TabIndex="3">
                                        </telerik:RadTextBox>
                                        <asp:Label ID="Label3" Visible="false" runat="server"></asp:Label>

                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Emp Code * 
                                          </label>
                                        <telerik:RadTextBox ID="txtEmpCode" MaxLength="10" runat="server" Skin="Simple" ClientEvents-OnKeyPress="OnKeyPress"
                                            TabIndex="3">
                                        </telerik:RadTextBox>

                                        
                                        <asp:Label ID="lblProdID" Visible="false" runat="server"></asp:Label>

                                    </div>
                                </div>
                                </div>


                          
                             
                                   <div class="row">
                                 <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Emp Phone 
                                          </label>
                                          <telerik:RadTextBox ID="txtEmpphone" runat="server" MaxLength="20" Skin="Simple" onkeypress="return ValidatePhoneNo()"
                                            TabIndex="4">
                                        </telerik:RadTextBox>
                                    </div>
                                </div>

                              

  <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                             Emp Name *    
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtEmpName"
                                                ErrorMessage="" ForeColor="Red" Font-Bold="true" ValidationGroup="valsum"></asp:RequiredFieldValidator>
                                        </label>
                                        <telerik:RadTextBox ID="txtEmpName" runat="server" MaxLength="240" Skin="Simple" 
                                            TabIndex="4">
                                        </telerik:RadTextBox>

                                    </div>
                                </div>

                                </div>


                  <div class="row">
                                <div class="col-sm-6">
                                      <div class="form-group">
                                        <label>
                                           Doc.Prefix 
                                          </label>
                                          <telerik:RadTextBox ID="txtPrefix" runat="server" MaxLength="10" Skin="Simple"
                                            TabIndex="4">
                                        </telerik:RadTextBox>
                                    </div>
                                </div>
                                </div>
                         
                          
                        
                            <div class="row">                    
                                <div class="col-sm-6"></div>
                                  <div class="col-xs-6">
                                         <div class="form-group">
                                    <telerik:RadButton ID="btnSave" ValidationGroup="valsum" TabIndex="17" Skin="Simple" runat="server" Text="Save" CssClass="btn btn-success" />
                                    <telerik:RadButton ID="btnCancel" Skin="Simple" runat="server" Text="Cancel" TabIndex="18"
                                        OnClientClicked="CloseWindow"
                                        AutoPostBack="false"
                                        CssClass="btn btn-default" />
                                    </div>
                                    </div>
                            </div>    
                       
               

              <%--  <telerik:RadTabStrip runat="server" ID="RadTabStrip1" MultiPageID="RadMultiPage1" SelectedIndex="0" Skin="Simple">
                    <Tabs>
                        <telerik:RadTab Text="Basic Info *">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Additional Info">
                        </telerik:RadTab>

                    </Tabs>
                </telerik:RadTabStrip>

                <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="outerMultiPage">
                    <telerik:RadPageView runat="server" ID="RadPageView1" Height="350px">


                     





                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView4" Height="350px">
                        <div class="form-inline">
                            <div class="popupform">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>Item Meta Data</label>
                                        <telerik:RadTextBox ID="txtMetaData" runat="server" Skin="Simple" MaxLength="1000" TextMode="MultiLine" Rows="2"
                                            TabIndex="7">
                                        </telerik:RadTextBox>
                                    </div>

                                    <div class="form-group">
                                        <label>Detailed Info</label>
                                        <telerik:RadTextBox ID="txtDetailInfo" runat="server" Skin="Simple" TextMode="MultiLine" Rows="3"
                                            TabIndex="8">
                                        </telerik:RadTextBox>
                                    </div>



                                </div>
                             

                            </div>
                        </div>






                    </telerik:RadPageView>



                </telerik:RadMultiPage>--%>

               </div>
                                      </ContentTemplate>
                                
                            </asp:UpdatePanel>

            </ContentTemplate>
        </telerik:RadWindow>
                                </ContentTemplate>


           
                            </asp:UpdatePanel>




    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>


      




               




            <telerik:RadWindow ID="DocExpUomWindow" Title="Select Organization" runat="server" Skin="Windows7" Behaviors="Move,Close"
            Width="450px" Height="200px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
            <ContentTemplate>

                <span style="font-size: 12px; padding-left: 10px; color: darkmagenta; padding-top: 5px;">* Indicates the mandatory fields</span>
                <div style="float: right; margin-right: 5px;">
                    <asp:Label ID="Label1" ForeColor="#d9534f" Font-Bold="true" runat="server"></asp:Label>
                    <asp:Label ID="Label2" ForeColor="#d9534f" Font-Bold="true" runat="server"></asp:Label>
                </div>


                   <div class="form-inline">
                            <div class="popupform">

                              <div class="col-sm-6">
                                    <div class="form-group">
                                        <label>
                                            Organization
                               *
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlOrg_UOM" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                        <telerik:RadComboBox ID="ddlOrg_UOM" Skin="Simple" Filter="Contains" Width="100%" TabIndex="1" EmptyMessage="Select Organization"
                                            runat="server">
                                        </telerik:RadComboBox>

                                    </div>
                                </div>


                                  <div class="text-right col-xs-6">
                                         <div class="form-group"><label>  &nbsp; </label>
                                    <telerik:RadButton ID="btnUOMExp" TabIndex="17" Skin="Simple" runat="server" Text="Export UOMs" CssClass="btn btn-success" />
                                    <telerik:RadButton ID="btnUOMCancel" Skin="Simple" runat="server" Text="Cancel" TabIndex="18"
                                        OnClientClicked="CloseWindowExpUom"
                                        AutoPostBack="false"
                                        CssClass="btn btn-danger" />
                                    </div>
                                    </div>
                                </div>
                            </div>
                       
               

            

               


            </ContentTemplate>
        </telerik:RadWindow>
        <asp:SqlDataSource ID="sqlVan" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
            SelectCommand="usp_GetVanList" SelectCommandType="StoredProcedure"></asp:SqlDataSource>




    </telerik:RadAjaxPanel>
</asp:Content>
