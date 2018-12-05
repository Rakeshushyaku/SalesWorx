<%@ Page Title="Mutli Transaction Bonus Rules" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AdminMultiTransRule.aspx.vb" Inherits="SalesWorx_BO.AdminMultiTransRule" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .rcTimePopup
        {
            display: none !important;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function HideRadWindowClose() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }

            $("#frm").find("iframe").hide();
        }

        function HideRadWindow() {

             
            $("#frm").find("iframe").hide();
        }

        function alertCallBackFn(arg) {
            HideRadWindowClose()
        }


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
            var win = $find('<%= MPEImport.ClientID%>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }
            var win2 = $find('<%= MPEAdd.ClientID%>');
            if (win2) {
                if (!win2.isClosed()) {
                    win2.center();
                }
            }
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Multi Transaction Bonus Rules</h4>

    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>




    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                         <%-- <asp:PostBackTrigger  ControlID="btnImportWindow"  />--%>
                          <asp:PostBackTrigger  ControlID="btndownloadTemp"  /> 
                            </triggers>

        <contenttemplate>
                                
                                      
                                
                                   <div class="row">
                                            
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em> </label>
                                            
                                                <telerik:RadComboBox  Skin="Simple"  ID="ddl_Org" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox>
                             
                                            </div>
                                          </div>
                                          
                                         <div class="col-sm-7">
                        <div class="form-group">
                              <label><span style="visibility:hidden;">1</span> </label>
                        <asp:Button ID="btnAdd" runat="server" CausesValidation="false"  CssClass="btn btn-success"
                                                    TabIndex="1" Text="Add" />
                                                    <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport" 
                                                                                                              runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                  <asp:Button ID="btndownloadTemp" runat="server" CssClass ="btn btn-primary" Text="Download Template" TabIndex ="11" />
                            </div>
                    </div>
                                             
                                          
                                        </div>
                                
                                          <telerik:RadWindow ID="MPEImport" Title= "Import Multi Transaction Bonus Rules" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" MinHeight="160px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                        <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                    
                                                <p><asp:Label runat ="server" ID="Label6" ForeColor ="Red"  Text =""></asp:Label></p>
                                                <p><asp:Label runat ="server" ID="lblUpMsg" ForeColor ="Red"></asp:Label></p>
                  
                                                <div class="row">
		                                        <div class="col-sm-5">
			                                        <label>Select a File</label>
		                                        </div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                            <ContentTemplate><asp:FileUpload ID="ExcelFileUpload" runat="server" /></ContentTemplate>
                                                        </asp:UpdatePanel> 
                                                    </div>
                                                </div>
                                                </div>  
                                                <div class="row">
		                                        <div class="col-sm-5"></div>
		                                        <div class="col-sm-7">
			                                        <div class="form-group">
                                                        <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-warning" /> 
                                                              <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default"  TabIndex="5" runat="server"
                                                                                    CausesValidation="false" Text="Cancel" Visible ="false" />
                                                                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                                                           <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" 
                                                                 CssClass ="btn btn-primary" />
                                                           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
                                                            <span> <asp:LinkButton ID="lbLog" 
                                                              ToolTip ="Click here to download the uploaded log" runat ="server"
                                                               Text ="View Log" Visible="false" ></asp:LinkButton></span>
                                                    </div>
                                                </div>
                                                </div>   
                      
		
         
                                                <div>
                                                        <asp:UpdatePanel runat="server" ID="UpPanel">
                                                            <Triggers>
                                                              <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	                        <asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                            
                                                </div>

                        
                        <div class="table-responsive">
                       
                       
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
                        
                         
                    
                        </div>
                    
                                                      </div>
                                                </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                        
                        
                                
                                </contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <triggers>
           
      
            
	
        </triggers>
       
        <contenttemplate>
                                
                               <telerik:RadWindow ID="MPEAdd" Title= "Add/Modify Rules" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                         <ContentTemplate>
                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                              <asp:HiddenField ID="lblRule" runat="server" Value="-1" />
                                        <div class="popupcontentblk">
                                        <p><asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                        <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Sales Item<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox  Skin ="Simple" ID="ddlRedItem" EnableLoadOnDemand="true"  EmptyMessage ="Please enter Sales Product code/Name"
                                                        width="370px" Height ="100px" runat="server" AllowCustomText="false" MarkFirstMatch="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </div>
                                            </div>
                                        </div>
                                            <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Sales Qty<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                <%--    <asp:TextBox ID="txtRedQty" width="100%" runat="server" MaxLength="2"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers" TargetControlID="txtRedQty" >
                                                        </ajaxToolkit:FilteredTextBoxExtender>--%>
                                                       <telerik:RadNumericTextBox runat="server" ID="txtRedQty"  Width="40%"
                                              MinValue="0" Skin ="Simple" TabIndex ="4" MaxLength ="7" 
                                            autocomplete="off" NumberFormat-DecimalDigits="0"
                                            AllowOutOfRangeAutoCorrect="false" 

                                            IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Promo Item<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox Skin ="Simple" ID="ddlGivenItem"   Height ="100px"
                                                        EnableLoadOnDemand="true"  EmptyMessage ="Please enter Promo Product code/Name"
                                                        runat="server" AllowCustomText="false" MarkFirstMatch="true" width="370px">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </div>
                                            </div>
                                        </div>
                                      <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Promo Qty<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                 <%--   <asp:TextBox ID="txtGivenQty" width="100%" runat="server" MaxLength="2"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            FilterType="Numbers" TargetControlID="txtGivenQty" >
                                                        </ajaxToolkit:FilteredTextBoxExtender>--%>
                                                      <telerik:RadNumericTextBox runat="server" ID="txtGivenQty"  Width="40%"
                                              MinValue="0" Skin ="Simple" TabIndex ="4" MaxLength ="7" 
                                            autocomplete="off" NumberFormat-DecimalDigits="0"
                                            AllowOutOfRangeAutoCorrect="false" 

                                            IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                                </div>
                                            </div>
                                        </div>


                                            <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Valid From<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100%" TabIndex ="9"    runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                                </div>
                                            </div>
                                        </div>

                                              <div class="row">
		                                    <div class="col-sm-4">
			                                    <label>Valid To<em><span>&nbsp;</span>*</em> </label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"  Width="100%" TabIndex ="10"   runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker> <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"  ForeColor ="red"   Type="String"
                        ErrorMessage="To date > From date">
                    </asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                             <div class="row" id="divtransactiontype" runat="server" visible="false">
                                            
                                                 <div class="col-sm-4">
                                                <label>Transaction Type </label>
                                                     </div>
                                                 <div class="col-sm-7">
                                            <div class="form-group">
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple"   ID="ddl_transactionType"  TabIndex ="3"  runat="server" Width ="100%" CssClass="inputSM">
                                        <Items>
                                            <telerik:RadComboBoxItem Value ="0"  Text ="All" Selected="true"/>
                                            <telerik:RadComboBoxItem Value ="CASH"  Text ="CASH"/>
                                         
                                            <telerik:RadComboBoxItem Value ="CREDIT"  Text ="CREDIT"/>
                                         
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:HiddenField runat="server" ID="H_TransType" Value="0" />
                                 </div>
                                           </div>
                                               </div>
                                                 
                                       <div class="row" id="divCategorytype" runat="server" visible="false">
                                           <div class="col-sm-4">
                                                <label>Category</label>
                                                     </div>
                                            <div class="col-sm-7">
                                            <div class="form-group">
                                                <label></label>
                                  
                                   
                                    <telerik:RadComboBox Skin="Simple" ID="ddl_Category" EmptyMessage="Select Category" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                 </div>
                                           </div>
                                                 </div>
                                        <div class="row">
		                                    <div class="col-sm-4">
                                                 
                             
                          
                       

		                                    </div>
		                                     <div class="col-sm-7">
			                                    <div class="form-group">
                                                       <telerik:RadButton ID="rsActive" runat="server" Text="Is Active"  TabIndex="4" AutoPostBack ="false" ToggleType="CheckBox"  
                                ButtonType="ToggleButton" Skin ="Simple"></telerik:RadButton>&nbsp;&nbsp;
                                                    <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                    <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update"  OnClick="btnUpdate_Click"
                                                        runat="server" />
                                                    
                                                    <asp:Button ID="btnCancel" CssClass ="btn btn-default"  TabIndex="6" runat="server" CausesValidation="false"
                                                       OnClick="btnCancel_Click"  Text="Cancel" /> 
                                                </div>
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
                                             
                                          
                                             
                                
                                

                                     
                                             
                                                 
                                             
                                <div class="table-responsive">
                                          <telerik:RadGrid ID="gvRules" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="5" AllowPaging="true" runat="server" DataSourceID="SqlDataSource1" ShowFooter="true" AllowFilteringByColumn="true"
                                GroupingSettings-RetainGroupFootersVisibility="true" GroupingSettings-GroupContinuesFormatString="" GroupingSettings-GroupContinuedFormatString=""
                                GridLines="None">

                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ClientSettings EnableRowHoverStyle="true">
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" ShowFooter="false" AllowFilteringByColumn="true"
                                    Width="100%" GridLines="None" BorderColor="LightGray" DataSourceID="SqlDataSource1" CommandItemDisplay="Top"
                                    PageSize="5">
                                    <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>

                                    <Columns>

                                        <telerik:GridTemplateColumn UniqueName="EditColumn"  AllowFiltering ="false"   
                    InitializeTemplatesFirst="false">
                   
                  
                    <ItemTemplate>
                        
                               <asp:ImageButton ID="btnEdit" ToolTip="Edit Item" runat="server" CausesValidation="false"  
                                             OnClick ="btnEdit_Click"                ImageUrl="~/Images/edit-13.png" />
                                           <asp:Label ID="lblRuleID" runat="server" Text='<%# Bind("Rule_ID")%>' Visible ="false"></asp:Label>  
                         <asp:Label ID="lblRedID" runat="server" Text='<%# Bind("RedID")%>' Visible ="false"></asp:Label>  
                        <asp:Label ID="lblRedQty" runat="server" Text='<%# Bind("RedQty")%>' Visible ="false"></asp:Label>  
                        <asp:Label ID="lblGivenID" runat="server" Text='<%# Bind("GivenID")%>' Visible ="false"></asp:Label>  
                        <asp:Label ID="lblGivenQty" runat="server" Text='<%# Bind("GivenQty")%>' Visible ="false"></asp:Label>  
                           <asp:Label ID="lblActive" runat="server" Text='<%# Bind("IsActive")%>' Visible ="false"></asp:Label> 
                          <asp:Label ID="lblRedItem" runat="server" Text='<%# Bind("RedItem")%>' Visible ="false"></asp:Label> 
                        <asp:Label ID="lblGivenItem" runat="server" Text='<%# Bind("GivenItem")%>' Visible ="false"></asp:Label> 
                         <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("ValidFrom")%>' Visible ="false"></asp:Label> 
                        <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("ValidTo")%>' Visible ="false"></asp:Label> 
                        <asp:Label ID="lblTransaction_Type" runat="server" Text='<%# Bind("Transaction_Type")%>' Visible ="false"></asp:Label> 
                        &nbsp;&nbsp;&nbsp;
                          <asp:ImageButton ID="btnDelete" ToolTip="Delete Item" Visible ="false"  runat="server" CausesValidation="false"  
                                               CommandName="DeleteSelected"  
                               ImageUrl="~/Images/delete-13.png"
                              OnClientClick="return ConfirmDelete('Are you sure to delete this item?',event);"
                             
                            />                                
                    </ItemTemplate>
       <HeaderStyle  />
                    <ItemStyle HorizontalAlign="Center" Wrap ="false" ></ItemStyle>
                </telerik:GridTemplateColumn>
                
       <telerik:GridTemplateColumn UniqueName="DeleteColumn" Visible ="false"   AllowFiltering ="false"   
                    InitializeTemplatesFirst="false">
                   
                     
                    <ItemTemplate>
                        
                     
                    </ItemTemplate>
           <HeaderStyle  />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </telerik:GridTemplateColumn>

                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="RedItem" HeaderText="Sales Item" AutoPostBackOnFilter ="true" 
                            SortExpression="RedItem"    AllowFiltering ="true" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the item code/name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>

                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="RedQty" HeaderText="Sales Qty"
                                            SortExpression="RedQty"  AllowFiltering="false" DataFormatString ="{0:N0}">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                                 <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="GivenItem" HeaderText="Promo Item" AutoPostBackOnFilter ="true" 
                            SortExpression="GivenItem"    AllowFiltering ="false" ShowFilterIcon ="false"
                             CurrentFilterFunction ="Contains" FilterControlToolTip ="Type the item code/name and press enter" >
                            <ItemStyle Wrap="False" HorizontalAlign ="Left"  />
                                             <HeaderStyle Wrap ="false" HorizontalAlign ="Left" />
                                 <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                       
                        </telerik:GridBoundColumn>


                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="GivenQty" HeaderText="Promo Qty"
                                            SortExpression="GivenQty"  AllowFiltering="false"  DataFormatString ="{0:N0}">
                                         <ItemStyle Wrap="False" HorizontalAlign ="Right"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ValidFrom" HeaderText="Valid From"
                                            SortExpression="ValidFrom"  AllowFiltering="false"  DataFormatString="{0:dd-MMM-yyyy}">
                                         <ItemStyle Wrap="False" HorizontalAlign ="left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ValidTo" HeaderText="Valid To"
                                            SortExpression="ValidTo"  AllowFiltering="false"  DataFormatString="{0:dd-MMM-yyyy}">
                                         <ItemStyle Wrap="False" HorizontalAlign ="left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="IsActive" HeaderText="Is Active"
                                            SortExpression="IsActive"  AllowFiltering="false">
                                         <ItemStyle Wrap="False" HorizontalAlign ="left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TransType" HeaderText="Transaction Type"
                                            SortExpression="TransType"  AllowFiltering="false">
                                         <ItemStyle Wrap="False" HorizontalAlign ="left"  />
                                             <HeaderStyle Wrap ="true" HorizontalAlign ="left" />
                                            <FooterStyle HorizontalAlign="right" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                        </telerik:GridBoundColumn>
                                     
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        <%--</telerik:RadAjaxPanel>--%>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="app_LoadMultiTransRules" SelectCommandType="StoredProcedure">

                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddl_Org" Name="OID" DefaultValue="0" />
                          
                               

                            </SelectParameters>

                        </asp:SqlDataSource>
                                   </div>
                                        
                         
                               
                                </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
        <progresstemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                                        <span>Processing...</span>
                                    </asp:Panel>
                                  
                                </progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>



</asp:Content>
