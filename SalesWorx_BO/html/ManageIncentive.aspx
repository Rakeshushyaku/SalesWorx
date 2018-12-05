<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ManageIncentive.aspx.vb" Inherits="SalesWorx_BO.ManageIncentive" %>
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
        </style>
        <script type="text/javascript">

        
             
            function checkconfirm(status) {
                if (status == "Y")
                    return confirm("Are you sure to disable this parameter?")
                else
                    return confirm("Are you sure to enable this parameter?")
            }

            function checkconfirmslab(status) {
                if (status == "Y")
                    return confirm("Are you sure to delete this parameter?")
                else
                    return confirm("Are you sure to enable this parameter?")
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
         Hidewindow()
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

     function Hidewindow() {
         var elemfrm = $("#frm")
         if (elemfrm != null) {
             var frm = $("#frm").find("iframe")
             if (frm != null) {
                 $("#frm").find("iframe").hide();
             }
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
    <h4>Manage Incentive</h4>
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

    <br />


                <telerik:RadTabStrip ID="IncentiveTab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Incentive Weightage" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="Incentive Payout Slabs" runat="server">
                        </telerik:RadTab>
                        
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">

                    <telerik:RadPageView ID="RadPageView3" runat="server">

                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                <br />
                          <div class="row">

                                            <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>
                                               Select Organization
                                            </label>
                                                <telerik:RadComboBox Skin="Simple"  ID="ddlOrg_I" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                   
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                      
                                        <div class="col-sm-8">   
                                            <div class="form-group">   
                                                <label>
                                                &nbsp;
                                            </label>    
                                                 <telerik:RadButton ID="btnSearch_i" Skin ="Simple"    runat="server" Text="Search" OnClick="btnSearch_i_Click"  CssClass ="btn btn-primary" />
                                                  <telerik:RadButton ID="btnSave_i" Skin="Simple" CssClass ="btn btn-success" 
                                                           OnClick="btnSave_i_Click"  ValidationGroup ="valsum"
                                                            runat="server" Text="Save" TabIndex ="6" >
                                                      </telerik:RadButton>
                                                 <telerik:RadButton ID="btnReset_i" Skin ="Simple"    runat="server" Text="Reset"  OnClick ="btnReset_i_Click" CssClass ="btn btn-primary" />
                                            </div>
                                        </div>
                                 </div> 
                            <div class="table-responsive">
                                  <asp:Label ID="lblStatus" runat ="server" Visible ="false"></asp:Label>

                                <telerik:RadGrid ID="gvIncentivePara" AllowSorting="True" Skin="Simple" BorderColor="LightGray"
                                    PageSize="9" AllowPaging="false" runat="server" ShowFooter="true"
                                    GridLines="None">

                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ClientSettings EnableRowHoverStyle="true" AllowGroupExpandCollapse="true">
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" HierarchyDefaultExpanded="true" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                                      DataKeyNames="Code_Description,Code_Value,ROW_ID"   PageSize="9">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right" />
                                        <HeaderStyle Wrap="true" HorizontalAlign="Right" />
                                        <AlternatingItemStyle Wrap="false" HorizontalAlign="Right" />

                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>


                                        <Columns>
                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Code_Description" HeaderText="Description"
                                                SortExpression="Code_Description" AllowFiltering="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="left" />
                                                <HeaderStyle Wrap="true" HorizontalAlign="left" />
                                              
                                            </telerik:GridBoundColumn>


                                             <telerik:GridTemplateColumn HeaderText = "" UniqueName="Status"   >  
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <ItemTemplate> 
                                                    
                                             <asp:LinkButton  runat="server" CommandName="Status" ID="lbChangeStatus"  Text='<%# Bind("Status")%>'  OnClientClick=<%# "javascript:return checkconfirm('" + Eval("Is_Active") + "');"%> OnClick ="lbChangeStatus_Click"></asp:LinkButton>
                                                      <asp:Label ID="lblROW_ID" runat ="server" Visible ="false"  Text='<%# Bind("ROW_ID")%>'></asp:Label>
                                                     <asp:Label ID="lblActive" runat ="server" Visible ="false"  Text='<%# Bind("Is_Active")%>'></asp:Label>
                                                </ItemTemplate> 
                                           </telerik:GridTemplateColumn> 

                                            

                                            <telerik:GridTemplateColumn HeaderText = "Value" UniqueName="Value" DataField="Weightage">  
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                <HeaderStyle Wrap="true" HorizontalAlign="Center" />
                                                <ItemTemplate> 
                                                         <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txt_incentvalue"   Width="190px"  Text='<%# Bind("Weightage")%>'   ></telerik:RadNumericTextBox>

                                                         <%--  <asp:TextBox ID="" runat="server" Width ="100%" Text='<%# Bind("Weightage")%>' ></asp:TextBox> --%>
                                                </ItemTemplate> 
                                           </telerik:GridTemplateColumn> 

                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                 <div class="row">
                                                                                                           
                                     <div class="col-sm-12">
                                          
                                     </div>
                                          
                                      </div>
                            </div>
                        </telerik:RadAjaxPanel>

                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server">
                
                         <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                       <br />   
                                    <div class="row">

                                            <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                               Select Organization
                                            </label>
                                                <telerik:RadComboBox Skin="Simple"  ID="ddlorg_F" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                  
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                                Filter By
                                            </label>
                                                <telerik:RadComboBox Skin="Simple"  ID="ddFilterBy" Width="100%" Height="250px" TabIndex="2" runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="Parameter Code " Text="Parameter Code"></telerik:RadComboBoxItem>
                                                       
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">   
                                            <div class="form-group">  
                                                   <label>
                                                &nbsp;
                                            </label> 
                                             <telerik:RadTextBox  runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value"  Width="100%"></telerik:RadTextBox>
                                          </div>
                                        </div>
                                        <div class="col-sm-3">   
                                            <div class="form-group">   
                                                <label>
                                                &nbsp;
                                            </label>    
                                                 <telerik:RadButton ID="btnFilter" Skin ="Simple"    runat="server" Text="Search" CssClass ="btn btn-primary" OnClick="btnFilter_Click" />
                                                 <telerik:RadButton ID="btnAdd" Skin="Simple" OnClick ="btnAdd_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                                  <telerik:RadButton ID="btnReset" Skin ="Simple"    runat="server" Text="Reset"  OnClick ="btnReset_Click" CssClass ="btn btn-primary" />
                                            </div>
                                        </div>
                                 </div> 
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                 
                                                <asp:GridView Width="100%" ID="gvPercentage_Slabs" runat="server" EmptyDataText="No Incentive Pay Percentage Slabs to Display"
                                                    EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                    AllowPaging="True" AllowSorting="true"  PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <Columns>


                                                      <%--  <asp:TemplateField>
                                                            <HeaderTemplate>
                                                              
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                 <asp:LinkButton  runat="server" CommandName="Status" ID="lbChangeStatus_slab"  Text='<%# Bind("Status")%>'  OnClientClick=<%# "javascript:return checkconfirmslab('" + Eval("Is_Active") + "');"%> OnClick ="lbChangeStatus_slab_Click" ></asp:LinkButton>
                                                                                                                       </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>--%>



                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                              
                                                              
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                 <asp:Label ID="lblROW_ID_slab" runat ="server" Visible ="false"  Text='<%# Bind("ROW_ID")%>'></asp:Label>
                                                               <asp:Label ID="lblActive_slab" runat ="server" Visible ="false"  Text='<%# Bind("Is_Active")%>'></asp:Label>  
                                                                 <asp:Label ID="lblPcode" runat ="server" Visible ="false"  Text='<%# Bind("Parameter_Code")%>'></asp:Label>     
                                                               <asp:ImageButton ToolTip="Delete Payout slab " ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png" OnClientClick="return ConfirmDelete('Would you like to delete the selected slab?',event);" CssClass="checkboximgvalign" />

                                                                  <asp:ImageButton ID="btnEdit" ToolTip="Edit Payout Percentage " runat="server" CausesValidation="false"
                                                                    ImageUrl="~/images/edit-13.png" OnClick="btnEdit_Click" CssClass="checkboximgvalign" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>



                                                        <asp:BoundField DataField="Code_Description" HeaderText="Parameter" SortExpression="Code_Description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="From_Percentage" DataFormatString="{0:F2}" HeaderText="From Percentage" SortExpression="From_Percentage">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="To_Percentage" DataFormatString="{0:F2}" HeaderText="To Percentage"
                                                            SortExpression="To_Percentage">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Payout_Percentage" DataFormatString="{0:F2}" HeaderText="Payout Percentage" SortExpression="Payout_Percentage">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                                                                               
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRowID" runat="server" Text='<%# Bind("Row_ID")%>'></asp:Label>
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
                                           
                                    <telerik:RadWindow ID="MPEDetails" Title= "Incentive Pay Percentage Slab Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                               <asp:HiddenField ID="RowID_slab" runat="server" Value="-1" />
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
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
                                                        Parameter *
                                            <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="valsum"
                                                ValueToCompare="" Operator="NotEqual" ControlToValidate="ddlPayParam" ForeColor="Red" ErrorMessage="" Font-Bold="true" /></label>
                                                    <telerik:RadComboBox ID="ddlPayParam" Skin="Simple" Filter="Contains" Width="100%" TabIndex="1" EmptyMessage="Select Parameter"
                                                        runat="server">
                                                    </telerik:RadComboBox>

                                                </div>
                                            </div>
                                        </div>

                                           <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label>
                                                        From Percentage * 
                                                  </label>
                                                

                                                    
                                                     <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtFrom_p" Width="100%"  
                                                MinValue="0" MaxValue="999" ></telerik:RadNumericTextBox>
                                                    <asp:Label ID="lblProdID" Visible="false" runat="server"></asp:Label>
                                                    <asp:Label ID="lblOrgID" Visible="false" runat="server"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label>
                                                        To Percentage *    
                                                     </label>
                                                  

                                                     <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtTo_p" Width="100%"  
                                                MinValue="0" MaxValue="999" ></telerik:RadNumericTextBox>

                                                </div>
                                            </div>
                                        </div>

                                          <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label>
                                                        Payout Percentage *
                                            <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtPayout_p" Width="100%" 
                                                MinValue="0" MaxValue="999" ></telerik:RadNumericTextBox>

                                                </div>
                                            </div>


                                        </div>

                                             <div class="row">
                                    
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

                    </telerik:RadPageView>
                  

                   
                </telerik:RadMultiPage>

               
         



    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>


      




               




          




    </telerik:RadAjaxPanel>
</asp:Content>
