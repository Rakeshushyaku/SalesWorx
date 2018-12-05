<%@ Page Title="Manage Assets" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="ManageAssets.aspx.vb" Inherits="SalesWorx_BO.ManageAssets" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
<style>
    #ctl00_ContentPlaceHolder1_MapWindow_C
    {
    	overflow: hidden !important;
    }
    .RadWindow_Default a.rwIcon {
   background-image: none !important;
}
</style>
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
                    $get('<%= Me.detPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
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
                    $get('<%=Me.detPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
                    //<a href="javascript:void(0);" class="rwCloseButton" title="Close"><span>Close</span></a>
                   }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;

                // Hiding the radwindow
                if (cancelString != -1)
                {
                    HideRadWindow();
                }
            }
        }


        function TestCheckBox(event) {
            if (TargetBaseControl == null) return false;
            var TargetChildControl = "chkDelete";
            var TimeCon = document.getElementById('<%= Me.ClassUpdatePnl.ClientID %>');

            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                    return ConfirmDelete('Would you like to delete the selected assets and history?',event);
                return true;
            }
            //alert('Select at least one assets!');
            radalert('Select at least one asset history!', 330, 180, 'Validation', alertCallBackFn);
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

    
        function alertCallBackFn(arg) {
            HideRadWindow()
                
        }

        function HideRadWindow()
        {
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
            var win = $find('<%= MapWindow.ClientID %>');
            if (win) {
                if (!win.isClosed()) {
                    win.center();
                }
            }

        });
    </script>
 </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
   
    <h4>Manage Assets</h4>
         <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        </telerik:RadAjaxManager>
     
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
     </telerik:RadWindowManager>
     

                
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                             <Triggers>
                           <asp:PostBackTrigger  ControlID="btnExport"   />
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                            </Triggers>
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-4">
                                      <div class="form-group">  
                                          <label>Organization</label>
                                             <telerik:RadComboBox CssClass="inputSM" ID="ddOraganisation"  Width ="100%"  Skin="Simple" 
                                                runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                                                    AutoPostBack="True"> </telerik:RadComboBox>
                                        </div>
                                          </div>
                                        <div class="col-sm-8">
                                      <div class="form-group"> 
                                          <label class="hidden-xs"><br /></label>
                                           <telerik:RadButton ID="btnAdd" runat="server" CausesValidation="false"  CssClass ="btn btn-success" Skin="Simple"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                 <telerik:RadButton ID="btnExport" runat="server" CssClass="btn btn-warning" Skin="Simple" Text="Export"/>
                                                      <telerik:RadButton ID="btnImportWindow" runat="server" CssClass="btn btn-default " Skin="Simple" Text="Import" />
                                       </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-4">
                                     <div class="form-group">  
                                         <label>
                                                 Filter By
                                            </label>

                                                <telerik:RadComboBox ID="ddlFilterBy" Skin="Simple"  
                                                    Width="100%"  TabIndex="3"
                                                    runat="server">
                                                <Items>

                                                    <telerik:RadComboBoxItem Selected="True" Text="All"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Customer No" Text="Customer No"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Customer Name" Text="Customer Name"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Asset Type" Text="Asset Type"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="AssetType ID" Text="AssetType ID"></telerik:RadComboBoxItem>
                                                     <telerik:RadComboBoxItem Value="Description" Text="Description"></telerik:RadComboBoxItem>
                                                    <telerik:RadComboBoxItem Value="Is Active" Text="Is Active"></telerik:RadComboBoxItem>
                                                </Items>
                                            </telerik:RadComboBox>
                                         </div>
                                          </div>
                                        <div class="col-sm-4">
                                      <div class="form-group"> 
                                          <label class="hidden-xs"><br /></label>
                                          <telerik:RadTextBox autocomplete="off" runat="server" ID="txtFilterVal" EmptyMessage="Enter Filter Value" Skin="Simple" Width="100%">

                                             </telerik:RadTextBox>
                           </div>
                                          </div>
                                        <div class="col-sm-4">
                                      <div class="form-group"> 
                                          <label class="hidden-xs"><br /></label>
                                                                                        
                                                <telerik:RadButton  ID="btnFilter" runat="server" CausesValidation="False" Skin="Simple" CssClass="btn btn-primary"
                                                    OnClick="btnFilter_Click" TabIndex="5" Text="Filter" />
                                                          <telerik:RadButton  ID="Clear" runat="server" CausesValidation="False" Skin="Simple" CssClass="btn btn-default"
                                                     TabIndex="6" Text="Reset" />


                                    </div>

                                 </div>
                                          </div>
                                           
                                </ContentTemplate>
                            </asp:UpdatePanel>
                           <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="Conditional">
                           
                                <ContentTemplate>
                                 <asp:GridView Width="100%" ID="dgvErros" Visible ="false"  runat="server" 
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" >
                   
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="Row No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <%-- <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColNo"
                                                                HeaderText="Col No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ColName"
                                                                HeaderText="Colume Name">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>--%>
                                                          
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
                                   <telerik:RadWindow ID="MapWindow" Title ="Asset Details" runat="server"  Behaviors="Move,Close" Skin="Windows7"
                                          AutoSize="true"  ReloadOnShow="false"  VisibleStatusbar="false" Modal ="true"   Overlay="true"  >
                                         <ContentTemplate>
                                             <div class="popupcontentblk">
                                               <telerik:RadAjaxPanel ID="UpdatePanel1" runat="server" >
                                                <%-- <ContentTemplate>--%>
                                                     <asp:Panel ID="detPnl" runat="server">
                                                         <p class="popupcontenterror"><asp:Label runat ="server" ID="lblValMsg" ForeColor ="Red"></asp:Label></p>
                                                         <asp:TextBox ID="txtAssetTypeId" Visible ="false" Enabled ="false" runat="server" TabIndex="1" CssClass="inputSM"></asp:TextBox>

                                                         <div class="row">
		                                                    <div class="col-sm-5">
			                                                    <label>Customer</label>
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <telerik:RadComboBox EmptyMessage="Please type Customer No/ Name" Skin="Simple" AutoPostBack="true" EnableLoadOnDemand="true"  Filter="Contains" ID="ddlCustomer"   Width ="100%"
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </telerik:RadComboBox>  
                                                                </div>
                                                            </div>
                                                         </div>
                                                         <div class="row">
		                                                    <div class="col-sm-5">
			                                                    <label>Asset Type</label>
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <telerik:RadComboBox  ID="ddlAssetType"  Width ="100%" EmptyMessage ="Please select" Skin ="Simple"
                                                                         runat="server" DataTextField="Description" DataValueField="AssetTypeID" >
                                                                     </telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                         </div>
                                                         <div class="row">
		                                                    <div class="col-sm-5">
			                                                    <label>Asset Code</label>
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <asp:TextBox ID="txtAssetCode"  Width ="100%" TabIndex="4" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                         </div>
                                                         <div class="row">
		                                                    <div class="col-sm-5">
			                                                    <label>Description</label>
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <asp:TextBox ID="txtDescription" TextMode ="MultiLine"   Width="100%"  TabIndex="5" CssClass="inputSM" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                         </div>

                                                           <div class="row">
		                                                    <div class="col-sm-5">
			                                                    <label>Value</label>
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                     <telerik:RadNumericTextBox runat="server" ID="txtValue" CssClass="inputSM" Skin ="Simple"   Width="150px"
                                                                    TabIndex="6"  MinValue="0"
                                                                    autocomplete="off" NumberFormat-DecimalDigits="2" IncrementSettings-InterceptMouseWheel="false"
                                                                    IncrementSettings-InterceptArrowKeys="false">
                                                                </telerik:RadNumericTextBox>
                                                                </div>
                                                            </div>
                                                         </div>
                                                         <div class="row">
		                                                    <div class="col-sm-5">
			                                                    Is Active
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <label><asp:CheckBox runat ="server" ID="ChkActive"  Text ="" /></label>
                                                                </div>
                                                            </div>
                                                         </div>
                                                         <div class="row">
		                                                    <div class="col-sm-5">
		                                                    </div>
		                                                    <div class="col-sm-7">
			                                                    <div class="form-group">
                                                                    <telerik:RadButton ID="btnSave" CssClass="btn btn-success" TabIndex="8" Skin="Simple"
                                                        runat="server" Text="Save" OnClick="btnSave_Click" />
                                                    <telerik:RadButton ID="btnUpdate" CssClass="btn btn-success" OnClick="btnUpdate_Click" Skin="Simple"
                                                        runat="server" Text="Update"  />
                                                    <telerik:RadButton ID="btnCancel" CssClass="btn btn-warning" TabIndex="9" OnClientClick="return DisableValidation()" Skin="Simple"
                                                        runat="server" CausesValidation="false" Text="Cancel" />
                                                                </div>
                                                            </div>
                                                         </div>

                                                             
                                                      </asp:Panel>      
                                                   
                                                   <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span>Processing... </span>
                                                    </asp:Panel>                             
                                                 <%--</ContentTemplate>--%>
                                            </telerik:RadAjaxPanel>
                                                 </div>
                                         </ContentTemplate>
                                  </telerik:RadWindow>
                              
                                       
                                       <asp:HiddenField id="HfDecimal" runat="server"></asp:HiddenField>
            <asp:HiddenField id="hfCurrency" runat="server"></asp:HiddenField>
                                    
                                   <asp:GridView Width="100%" ID="gvAssets" runat="server" EmptyDataText="No assets to Display"
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False" 
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign" DataKeyNames ="Asset_ID" >
                                                  
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                  
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    CausesValidation="false"  ImageUrl="~/images/delete-13.png" 
                                                                    OnClientClick="return TestCheckBox(event)"
                                                                    OnClick="btnDeleteAll_Click" />
                                                            </HeaderTemplate>
                                                           <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete assets" ID="btnDelete" OnClick="btnDelete_Click"
                                                                    runat="server" CausesValidation="false"  ImageUrl="~/images/delete-13.png"  OnClientClick="return ConfirmDelete('Would you like to delete the selected assets and history?',event);"   />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit assets" runat="server" CausesValidation="false"
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                                                                                                  </ItemTemplate>
                                                                                                                                   <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Asset_Code" HeaderText="Asset Code" SortExpression="Asset_Code">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                                                            <ItemStyle Wrap="true" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                           <asp:BoundField DataField="Customer_No" HeaderText="Cust.No" SortExpression="Customer_No">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CustomerName" HeaderText="Cust.Name" SortExpression="CustomerName">
                                                          <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="AssetValue" HeaderText="Value" SortExpression="AssetValue">
                                                          <ItemStyle Wrap="false" HorizontalAlign ="Right"  />
                                                               <HeaderStyle HorizontalAlign="Center" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Is_Active" HeaderText="Is Active" SortExpression="Is_Active">
                                                            <ItemStyle Wrap="false" />
                                                               <HeaderStyle HorizontalAlign="Left" Wrap ="false"  />
                                                        </asp:BoundField>
                                                         <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Asset_id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                      <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                    
                               
                                
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                 
                                                </td>
                                            </tr>
                                        </table>
                                </ContentTemplate>
                            
                            </asp:UpdatePanel>
                       
                </div>
                   
                <asp:Button ID="btnImportHidden" CssClass="btnInput" runat="Server" Style="display: none" />
<%--                <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEImport"
                    runat="server" PopupControlID="DetailPnl" TargetControlID="btnImportHidden" CancelControlID="btnCancelImport">
                </ajaxToolkit:ModalPopupExtender>--%>

                <telerik:RadWindow ID="MapImport" Title ="Import Assets" runat="server"  Behaviors="Move,Close" Skin="Windows7"
                                          AutoSize="true"  ReloadOnShow="false"  VisibleStatusbar="false" Modal ="true"   Overlay="true"  >
                        <ContentTemplate>
                            <div class="popupcontentblk">
                              <asp:Panel ID="DetailPnl" runat="server" CssClass="modalPopup2">
                              
                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                      
                       <div class="row">
                             <div class="col-sm-5"> 
                                     <label style="transform: translateY(-4px)">Select File</label>
                                </div>
                            <div class="col-sm-7">
                                <div class="form-group">
                                     <asp:FileUpload ID="ExcelFileUpload" runat="server" />
                               </div>
                            </div>
                       </div>
                        <div class="row">
                            <div class="col-sm-5"></div>
                             <div class="col-sm-7">
                                <div class="form-group form-inline-blk"> 
                                      <asp:CheckBox ID="chkUpdate" CssClass ="inputSM" runat ="server" Text ="" /><label>Update Existing Assets</label> 
                               </div>
                            </div>
                       </div>
                          <div class="row">
                             <div class="col-sm-5"></div>
                             <div class="col-sm-7">
                                <div class="form-group"> 
                                     <telerik:RadButton ID="btnImportSave" CssClass="btn btn-warning" TabIndex="1" CausesValidation="false" Skin="Simple"
                                    OnClientClick="return DisableValidation()" runat="server" Text="Import" />
                                <telerik:RadButton ID="DummyImBtn" Style="display: none" runat="server" Text="Import" CausesValidation="false" Skin="Simple"
                                    OnClientClick="return DisableValidation()" />
                                <telerik:RadButton ID="btnCancelImport" CssClass="btn btn-default" TabIndex="2" OnClientClick="return DisableValidation()" Skin="Simple"
                                    runat="server" CausesValidation="false" Text="Cancel" />
                                     <asp:LinkButton ID="lbLog" ToolTip ="Click here to see the uploaded log" runat ="server" Text ="View Log" OnClick ="lbLog_Click"></asp:LinkButton>
                               </div>
                            </div>
                       </div>                      
                       <div class="row">
                             <div class="col-sm-12">
                                <div class="form-group"> 
                                   <asp:Label runat ="server" ID="lblUpMsg" CssClass ="txtSM" ForeColor ="Green"></asp:Label>
                               </div>
                            </div>
                       </div>
                         <div class="row">
                             <div class="col-md-12">
                                <div class="form-group"> 
                                    <asp:UpdatePanel runat="server" ID="UpPanel">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                               </div>
                            </div>
                       </div>          
                </asp:Panel>
                                </div>
                        </ContentTemplate>
                </telerik:RadWindow>

                  
            
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #e10000;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>

</asp:Content>
