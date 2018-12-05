<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AssignMSLGroupToCustomer.aspx.vb" Inherits="SalesWorx_BO.AssignMSLGroupToCustomer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .rcbSlide
        {
            z-index: 100002 !important;
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

    <h4>Product MSL Group Assignment</h4>
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
                                            <label>Group Name </label>
                                           <asp:DropDownList ID="ddlGroup" Skin="Simple"  runat="server" width="250" AutoPostBack="true">
                                                    
                                                    </asp:DropDownList>
                                           </div>
                                                </div>

                                                 
                                                     
                                    <br />
                                    <br />
                                     <div class="row">
                                                        <div class="col-sm-12">
                                                    <label>Filter</label>
                                                     
                                                    <telerik:RadComboBox Skin="Simple"   ID="ddlFilter" Width="150px"
                                                    runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="No Filter" Value="ALL"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer No" Value="Customer_No"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer Name" Value="Customer_Name"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer Type" Value="Customer_Type"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer Chain" Value="Customer_Chain"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Customer Class" Value="Customer_Class"></telerik:RadComboBoxItem>
                                                                                                            </Items>
                                                </telerik:RadComboBox>  
                                                             <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="Filter value" Skin ="Simple"  Width ="250"  ></telerik:RadTextBox>
                                                       
                                                      <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                                          
                                                       
                                                    TabIndex="4" Text="Search" />
                                                             <asp:Button ID="Btn_Reset" runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                                    TabIndex="4" Text="Reset" /> 
                                                              <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-primary" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport"  runat="server" CssClass ="btn btn-warning" Text="Export" TabIndex ="11" />
                                                             <div class="text-primary" style="font-weight:700;padding-bottom:15px;">
                        <asp:Label  ID="lblnote" runat ="server" Text ="Note: Assigning a MSL Group to the customer removes any existing MSL group assigned to the same customer."></asp:Label> 
                                      </div>
                                                            </div>
                                                      </div>
                                                 <table width="100%">
                                        <tr>
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
                                            <td width="49%">
                                              
                                              

                                                  <telerik:RadListBox ID="lstDefault"  ToolTip ="Press CTRl key for multiple selection"   runat ="server" 
                                                        Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>

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

                                       
                                            <td width="49%">
                                                

                                                  <telerik:RadListBox ID="lstSelected"    ToolTip ="Press CTRl key for multiple selection"      runat ="server"  
                                                       Width="100%" Height ="290px"   SelectionMode ="Multiple"   >
          
            

        </telerik:RadListBox>
                                            </td>
                                        </tr>
                                    </table>
                                     <telerik:RadWindow ID="MPEImport" Title= "Import Customer MSL Group" runat="server" Skin="Windows7" Behaviors="Move,Close"
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
        <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional" >
                             <Triggers>
           
        <asp:AsyncPostBackTrigger ControlID="btnImportWindow" EventName="Click" />
            
	
        </Triggers>
                                <ContentTemplate>
  
                                    </ContentTemplate>
         </asp:UpdatePanel>
    <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
        <Triggers>
                                 
                                       <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                       
                                </Triggers>
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
