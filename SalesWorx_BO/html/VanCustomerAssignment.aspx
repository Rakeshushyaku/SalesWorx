<%@ Page Title="Van Customer Assignment" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="VanCustomerAssignment.aspx.vb" Inherits="SalesWorx_BO.VanCustomerAssignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .rcbSlide
        {
            z-index: 100002 !important;
        }
        .RadGrid .rgPager .RadComboBox {
    margin: 0 4px 0 0;
    vertical-align: top;
    visibility: hidden !important;
}
        .RadGrid .rgPagerLabel {
    margin: 0 4px 0 0;
    vertical-align: top;
    visibility: hidden !important;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <script language="javascript" type="text/javascript">
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
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);
                if (AddString != -1 || EditString != -1) {

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
                var myRegExp1 = /btnSaveAcc/
                var AddString = postBackElement.id.search(myRegExp);
                var EditString = postBackElement.id.search(myRegExp1);

                if (AddString != -1 || EditString != -1) {

                }
                else {
                    $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
                }
                postBackElement.disabled = false;
            }
        }



    </script>

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
                    return confirm('Would you like to confirm the selected Stock Requisitions?');
                    return true;
                }
            alert('Select at least one Stock Requisition!');
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


        function alertCallBackFn(arg) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Van/FSR Customer Assignment</h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="Panel1" runat="server"></asp:Panel>




    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                           
             
     
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:Label ID="lblSelectedID" runat ="server" Visible ="false" ></asp:Label>
                                              <asp:Label ID="lblRemovedID" runat ="server" Visible ="false" ></asp:Label>
                                     
                                           
                                            <div class="row">
                                           
                                            <div class="col-sm-4">
                                               <label>
                                               Organization</label>
                                             
                                                        <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_org" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" EmptyMessage="Select Organisation" >
                                        </telerik:RadComboBox>
                                                
                                                </div>
                                                 <div class="col-sm-4">
                                            <label>Van/FSR </label>
                                        
                                                      <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSalesRep1" 
                                                          Width ="100%" runat="server"  AutoPostBack="True" EmptyMessage="Select van" >
                                        </telerik:RadComboBox>
                                                  
                                                </div>
                                                      <div class="col-sm-4">
                                            <label><span style ="visibility:hidden;">-1</span> </label>
                                         <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-primary" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport"  runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                                          </div> 

                                           </div>
                                              

                                                 
                                                     
                                 
                                     <div class="row">
                                                        <div class="col-sm-12">
                                                    <label></label>
                                                     
                                                    <telerik:RadComboBox Skin="Simple" Visible ="false"    ID="ddlFilter" Width="150px"
                                                    runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="No Filter" Value="ALL"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer No" Value="Customer_No"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer Name" Value="Customer_Name"></telerik:RadComboBoxItem>
                                                       
                                                                                                            </Items>
                                                </telerik:RadComboBox>  
                                                             <telerik:RadTextBox  Visible ="false"  runat ="server" ID="txtFilter" EmptyMessage ="Filter value" Skin ="Simple"  Width ="250"  ></telerik:RadTextBox>
                                                       
                                                      <asp:Button ID="btnFilter"  Visible ="false"  runat="server" CausesValidation="False" CssClass="btn btn-success"
                                                          
                                                       
                                                    TabIndex="4" Text="Search" />
                                                             <asp:Button ID="Btn_Reset"  Visible ="false"  runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                                    TabIndex="4" Text="Reset" /> 
                                                           
                                                             <div class="text-primary" style="font-weight:700;padding-bottom:15px;">
                        <asp:Label  ID="lblnote" runat ="server" Visible="false" ></asp:Label> 
                                      </div>
                                                            </div>
                                                      </div>
                                                 <table width="100%">
                                        <tr style ="visibility:hidden;">
                                            <td width="49%">
                                                <asp:Label ID="lblProdAvailed" Font-Bold="true" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="49%">
                                                <asp:Label Font-Bold="true" ID="lblProdAssign" ForeColor ="#337AB7" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="49%" valign ="top" >
                                              
                                              

                                                 <%-- <telerik:RadListBox ID="lstDefault"  ToolTip ="Press CTRl key for multiple selection"   runat ="server" 
                                                        Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>--%>

                                                <telerik:RadGrid ID="gvAvail" DataSourceID="sqlAvail"  AllowMultiRowSelection="True"
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None" ToolTip ="Press CTL key or drag for select multirow"
                                        PageSize="15" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="true" ShowFooter="true"
                                        GridLines="None">

                                        <GroupingSettings CaseSensitive="false"    ></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="false"  EnableAlternatingItems="false"   >
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataSourceID="sqlAvail" ShowFooter="true"  AllowFilteringByColumn ="true" 
                                            TableLayout="Auto" CommandItemDisplay="Top" Width="100%" GridLines="None" BorderColor="LightGray"
                                            PageSize="15">
                                               <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                            <Columns>

                                                <telerik:GridBoundColumn DataField="CustName" HeaderText="Available Customers" SortExpression="CustName"
                                                     ShowFilterIcon="false" AllowFiltering="true"  FilterControlToolTip="Type customer no/Name and press enter"  
                                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" Aggregate="Count" FooterText="Total: " >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>


                                                <telerik:GridTemplateColumn UniqueName="ProdColumn" Visible ="false" >


                                                    <ItemTemplate>

                                                        <asp:label runat="server" Visible="false"  ID="lblCustSite" Text='<%# Bind("CustSiteID")%>'
                                                            ></asp:label>

                                                    </ItemTemplate>

                                                  
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold ="true"  Font-Size ="13px"   />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>


                                               
                                            </Columns>
                                        </MasterTableView>


                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="sqlAvail" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                        SelectCommand="app_GetAvailCustomersByVan" SelectCommandType ="StoredProcedure"   >

                                        <SelectParameters >
                                            <asp:ControlParameter ControlID="ddl_org" Name="ORGID" DefaultValue="0" />
                                            <asp:ControlParameter ControlID="ddlSalesRep1" Name="SID" DefaultValue="0" />
                                        </SelectParameters>

                                    </asp:SqlDataSource>

                                            </td>


                                             <td  width="2%">
                <table border ="0" width="2%">
                   
                    <tr><td><asp:ImageButton runat ="server" ID="imgRemoveSelected" BorderStyle ="None" 
                         ToolTip ="Move selected item to right"   ValidationGroup="valsum"  OnClick ="imgAddSlected_Click"   ImageUrl="~/Images/arrowSingleRight.png" /></td></tr>
                   
                    <tr><td>  
                      <asp:ImageButton runat ="server" ID="imgAddSlected"   ValidationGroup="valsum"  BorderStyle ="None"  ToolTip ="Move selected item to left" OnClick ="imgRemoveSlected_Click"
                              ImageUrl="~/Images/arrowSingleLeft.png" /></td></tr>
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllLeft" BorderStyle ="None"    ValidationGroup="valsum"
                       ToolTip ="Move all item to left" OnClick ="imgMoveAllRight_Click" ImageUrl="~/Images/doubleRight.png" /></td>
                        </tr> 
                    <tr>
                        <td >
                  <asp:ImageButton runat ="server" ID="imgMoveAllRight"    ValidationGroup="valsum"  BorderStyle ="None"  OnClick ="imgMoveAllLeft_Click"  ToolTip ="Move all Item to right" ImageUrl="~/Images/doubleLeft.png" /></td>
                        </tr> 
                    </table> 
                  </td> 

                                       
                                            <td width="49%" valign ="top" >
                                                

                        <%--                          <telerik:RadListBox ID="lstSelected"    ToolTip ="Press CTRl key for multiple selection"      runat ="server"  
                                                       Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox--%>


                                                      <telerik:RadGrid ID="rgAssigned" DataSourceID="sqlAssigned" AllowMultiRowSelection="True"
                                        AllowSorting="True" AutoGenerateColumns="False" Width="100%" BorderStyle="None" ToolTip ="Press CTL key or drag for select multirow"
                                        PageSize="15" AllowPaging="True" runat="server" Skin="Simple" AllowFilteringByColumn="true" ShowFooter="true"
                                        GridLines="None">

                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ClientSettings EnableRowHoverStyle="false"   EnableAlternatingItems="false" >
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" DataSourceID="sqlAssigned" ShowFooter="true"  AllowFilteringByColumn ="true" 
                                            TableLayout="Auto" CommandItemDisplay="Top" Width="100%" GridLines="None" BorderColor="LightGray"
                                            PageSize="15">
                                               <CommandItemTemplate>
                                           <div style="float: right;">
                                       <asp:ImageButton ID="Button1" runat="server" AlternateText="Clear filter" ToolTip="Clear filter" OnClick="Button1_Click" ImageUrl="~/images/Clearfilter.png" />
                                   </div>
                                    </CommandItemTemplate>
                                            <Columns>

                                                <telerik:GridBoundColumn DataField="CustName" HeaderText="Assigned Customers" SortExpression="CustName"
                                                     ShowFilterIcon="false" AllowFiltering="true"  FilterControlToolTip="Type customer no/Name and press enter"  
                                                    CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" Aggregate="Count" FooterText="Total: " >
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridBoundColumn>


                                                <telerik:GridTemplateColumn UniqueName="ProdColumn" Visible ="false" >


                                                    <ItemTemplate>

                                                        <asp:label runat="server" Visible="false"  ID="lblCustSite" Text='<%# Bind("CustSiteID")%>'
                                                            ></asp:label>

                                                    </ItemTemplate>

                                                  
                                                    <HeaderStyle HorizontalAlign="Center" Font-Bold ="true"  Font-Size ="13px"   />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" BackColor="#eff9ff" ForeColor="#000" Font-Size="15px" />
                                                </telerik:GridTemplateColumn>


                                               
                                            </Columns>
                                        </MasterTableView>


                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="sqlAssigned" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                        SelectCommand="app_GetAssignedCustomerByVan" SelectCommandType ="StoredProcedure"   >

                                        <SelectParameters >
                                              <asp:ControlParameter ControlID="ddl_org" Name="ORGID" DefaultValue="0" />
                                            <asp:ControlParameter ControlID="ddlSalesRep1" Name="SID" DefaultValue="0" />
                                        </SelectParameters>

                                    </asp:SqlDataSource>


                                            </td>
                                        </tr>
                                    </table>
                                     <telerik:RadWindow ID="MPEImport" Title= "Import Customer Van Map" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                              Height="170px" Width ="490px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
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
                                                                                    CausesValidation="false" Text="Cancel" Visible ="false"  />
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
                                </ContentTemplate>
                                <Triggers>
                                
                                 
                                </Triggers>

                            </asp:UpdatePanel>
                      
                
                 </contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional">
        <triggers>
           
        <asp:AsyncPostBackTrigger ControlID="btnImportWindow" EventName="Click" />
            
	
        </triggers>
        <contenttemplate>
  
                                    </contenttemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
        <triggers>
                                 
                                       <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                       
                                </triggers>
        <contenttemplate>
                 <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                                                    <table id="tableinPopupErr" cellpadding="10" style="background-color: White;width:100%">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label></td></tr><tr>
                                                            <td align="center" style="text-align: center;">
                                                                <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    </contenttemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
        runat="server">
        <progresstemplate>
                            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>
    <br />
    <br />
    </td>
        </tr>
    </table>
</asp:Content>
