<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AssignProductToFSR.aspx.vb" Inherits="SalesWorx_BO.AssignProductToFSR" %>
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
                    $get('<%= Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'block';
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
                    $get('<%=Me.DetailPnl.FindControl("Panel12").ClientID%>').style.display = 'none';
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

        window.onload = function() {
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
   
     <h4>Product Assignment</h4>
                      <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>     
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           
             
    
               
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
                                           <asp:DropDownList ID="ddlSalesRep" runat="server" width="250" AutoPostBack="true">
                                                    
                                                    </asp:DropDownList>
                                           </div>
                                            <div class="col-sm-4">
                                                <label>&nbsp; </label>
                                                   <asp:Button ID="BTn_Import" runat="server" CausesValidation="false" CssClass="btn btn-success"
                                                    OnClick="btnImport_Click" TabIndex="1" Text="Import" />
                                                    
                                                     <asp:Button ID="BTn_Export" runat="server" CausesValidation="false" CssClass="btn btn-default"
                                                     TabIndex="1" Text="Export" />
                                                    
                                           </div>
                                           </div>
                                                <br />
                                    <br />
                                                  <div class="row">
                                                        <div class="col-sm-6">
                                                    <label>Filter</label>
                                                     
                                                       <telerik:RadTextBox runat ="server" ID="txtFilter" EmptyMessage ="Filter By Item Code" Skin ="Simple"  Width ="250"  ></telerik:RadTextBox>
                                                       
                                                      <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btn btn-success"
                                                    TabIndex="4" Text="Search" /> <asp:Button ID="Btn_Reset" runat="server" CausesValidation="False" CssClass="btn btn-warning"
                                                    TabIndex="4" Text="Reset" /> 
                                                            </div>
                                                      </div>
                                 

                                    <table>
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
                                   
                                </ContentTemplate>
                                <Triggers>
                                
                                 <asp:PostBackTrigger ControlID="BTn_Export" />
                                </Triggers>
                            </asp:UpdatePanel>
                        
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="600" CssClass="modalPopup" Style="display: none" >
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                         <asp:Panel ID="DragPnl" runat="server" CssClass="popupbartitle">
                                            Import </asp:Panel><asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                         <asp:ImageButton ID="btn_close" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton> 
                                          <div class="popupcontentblk">
                                               <p><asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Red"></asp:Label></p>

                                         <div class="row">
                                                <div class="col-sm-5">
                                                    <label>
                                                Organization</label>
                                                    </div>
                                                    <div class="col-sm-7">
                                                    <div class="form-group">
                                              
                                                        <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Organization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" EmptyMessage="Select Organisation" >
                                        </telerik:RadComboBox>
                                                    </div>
                                             </div>
                                             </div>
                                               <div class="row">
                                                <div class="col-sm-5">
                                                    <label>
                                                Van</label>
                                            </div>
                                                    <div class="col-sm-7">
                                                    <div class="form-group">
                                                <asp:DropDownList ID="ddlSalesRepImp" runat="server" width="250"  >
                                                
                                                </asp:DropDownList></div>
                                             </div>
                                                   </div>
                                               <div class="row">
                                                <div class="col-sm-5">
                                                    <label>File</label></div>
                                                    <div class="col-sm-7">
                                                    <div class="form-group">
                                                 
                                                <asp:UpdatePanel ID="UpdatePanelF" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <asp:FileUpload ID="ExcelFileUpload" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="ExcelFileUpload" Display="Dynamic"  ErrorMessage="*Please upload only .xls and .xlsx Files" ValidationExpression="^(.)+(.xls|.XLS|.xlsx|.XLSX)$"></asp:RegularExpressionValidator>
                            
                                                 
                                                   </ContentTemplate>
                                                    </asp:UpdatePanel>    
                                                </div>
                                                        </div>
                                                   </div>
                                                   <div class="row">
                                                    <div class="col-sm-5">
                                                        </div>
                                                       <div class="col-sm-7">
                                                    <div class="form-group">
                                                           <asp:Button ID="btnSave" CssClass="btn btn-default" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Import" />
                                                    
                                                    <asp:Button ID="btnCancel" CssClass="btn btn-warning" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                     </div>
                                                       </div>
                                               <asp:LinkButton id="BtnDownLoad" runat="server" Text="Download" Visible="false" ></asp:LinkButton>
                                               <asp:GridView Width="100%" ID="dgvErros" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true" Font-Size ="12px" CssClass="txtSM" AutoGenerateColumns="False" 
                                                        AllowPaging="true" AllowSorting="false" PageSize="10" CellPadding="6" >
                                                     <RowStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" CssClass="tdstyle"
                                                        Height="12px" Wrap="True" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                             <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="RowNo"
                                                                HeaderText="RowNo">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                                                                                      
                                                    
                                                             <asp:BoundField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="item_Code"
                                                                HeaderText="Item_Code">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                
                                                            </asp:BoundField>
                                                         
                                                           
                                                         
                                                             <asp:BoundField  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Reason"
                                                                HeaderText="Error Text">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                
                                                            </asp:BoundField>    
                                                        
                                                        </Columns>
                                                          <PagerStyle CssClass="pagernumberlink" />
                                                    <HeaderStyle BackColor="Silver" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                                        CssClass="headerstyle" />
                                                    </asp:GridView>
                                              
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../images/Progress.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                 
                                                </div>    
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSave" />
                                            
                                            </Triggers>
                                                    </asp:UpdatePanel> 
                                    </asp:Panel>
                                    
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                     <asp:PostBackTrigger ControlID="BtnDownLoad" />
     
                                </Triggers>
                            </asp:UpdatePanel>
                        
                
                 </ContentTemplate>
                 </asp:UpdatePanel>
                
                
                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                
</asp:Content>