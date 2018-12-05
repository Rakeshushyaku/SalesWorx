<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Customer-Product-Code.aspx.vb" Inherits="SalesWorx_BO.Customer_Product_Code" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
 

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
  
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
                    return confirm('Would you like to delete the selected codes?');
                    return true;
                }
            alert('Select at least one codes!');
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
   
    <h4>
    Customer Product Codes </h4>
     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

                    
                
                           
             
 
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                     <div class="row">
                                            
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                 <telerik:RadComboBox  Skin="Simple"  ID="ddl_FilterCustomer" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </div>
                                             </div>
                                             
                                            <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Product</label>
                                                 
                                                    <telerik:RadComboBox  Skin="Simple"  ID="ddl_FilterProduct"  Filter="Contains"  EmptyMessage="Please type product code/ description"
                                                         EnableLoadOnDemand="True"   AutoPostBack="true"  runat="server" AllowCustomText="false" MarkFirstMatch="true" width="100%">
                                                    <Items></Items>
                                                    </telerik:RadComboBox>
                                                 </div>
                                                </div>

                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>&nbsp; </label>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False"  CssClass ="btn btn-primary"
                                                    TabIndex="4" Text="Search" />
                                                <asp:Button ID="BTn_Import" runat="server" CausesValidation="false" CssClass ="btn btn-warning" 
                                                      TabIndex="1" Text="Import" />
                                                 <asp:Button ID="btnAdd" runat="server" CausesValidation="false"  CssClass="btn btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                     
                                                    </div>
                                                </div>
                    
                                              </div>
                                         

                                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                      <div class="table-responsive">
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" Width="100%" >
                                        <tr>
                                            <td>
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No customer product code."
                                                    EmptyDataRowStyle-Font-Bold="true" AutoGenerateColumns="False"
                                                    AllowPaging="True" AllowSorting="true"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                   
                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                    <EmptyDataRowStyle Font-Bold="True" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HCustomer" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Customer_ID_1") %>'/>
                                                                <asp:HiddenField ID="HItemCode" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Item_ID_1") %>'/>
                                                                                                                              
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete code" ID="btnDelete" 
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected code?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Code" runat="server" CausesValidation="false"
                                                                   
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Customer_ID_1" HeaderText="Customer" SortExpression="Customer_ID_1">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="description" HeaderText="Product" SortExpression="description">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Cust_Item_Code" HeaderText="Customer Product   Code"
                                                            SortExpression="Cust_Item_Code" HtmlEncode="false">
                                                            <ItemStyle Wrap="False" />
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
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        CancelControlID="btnCancel">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="DetailPnl" runat="server" Width="486" CssClass="modalPopup" Style="display: none" >
                                    
                                                             <div class="panelouterblk">
                                        <asp:Panel ID="DragPnl" runat="server" CssClass="popupbartitle">
                                            Customer Product Code</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="btn_close" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>    
                                                         
                                                            <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        
                                        <div class="popupcontentblk">
                                            <p><asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Red"></asp:Label></p>

                                            <div class="row">
                                                <div class="col-sm-5">
                                                    <label>Customer</label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <telerik:RadComboBox Skin ="Simple" ID="ddl_Customer" width="100%" runat="server" AllowCustomText="false" MarkFirstMatch="true">
                                                            <Items></Items>
                                                            </telerik:RadComboBox> 
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5">
                                                    <label>Products</label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <telerik:RadComboBox Skin ="Simple" ID="ddl_Product"  Filter="Contains"  EmptyMessage="Please type product code/ name"
                                                         EnableLoadOnDemand="True"   AutoPostBack="true" runat="server" AllowCustomText="false" MarkFirstMatch="true" width="100%">
                                                        <Items></Items>
                                                        </telerik:RadComboBox> 
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5">
                                                    <label>Code</label>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txt_code" width="100%" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5"></div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <asp:Button ID="btnSave" CssClass ="btn btn-success" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                        <asp:Button ID="btnUpdate" CssClass ="btn btn-success" Text="Update" OnClick="btnUpdate_Click"
                                                            runat="server" />
                                                    
                                                        <asp:Button ID="btnCancel" CssClass ="btn btn-default"  TabIndex="6" runat="server" CausesValidation="false"
                                                            Text="Cancel" />
                                                    </div>
                                                </div>
                                            </div>

                                        
                                                    <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                                
                                          </div>           
                                                                 </div>
                                                             
                                         
                                    </asp:Panel>
                                    
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    
       <asp:PostBackTrigger ControlID="btn_import" />
     
                                </Triggers>
                            </asp:UpdatePanel>
                         
                </div>
                 <asp:Button ID="btnModelImport" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPImport"
                                        runat="server" PopupControlID="Panel1" TargetControlID="btnModelImport"
                                        CancelControlID="btnCancelImport">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel ID="Panel1" runat="server" Width="486" CssClass="modalPopup" Style="display: none" >
                                    
                                                             <div class="panelouterblk">
                                        <asp:Panel ID="Panel2" runat="server" CssClass="popupbartitle">
                                            Customer Product Code</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="Closebtnimg" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>    
                                                         
                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                      <div class="popupcontentblk">
                                        <asp:Label ID="lblImp" runat="server" Text="" ForeColor="Maroon"></asp:Label>

                                        <div class="row">
                                            <div class="col-sm-5">
                                                <label>Customer</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <telerik:RadComboBox Skin ="Simple" ID="ddl_custimport" width="100%" runat="server" AllowCustomText="false" MarkFirstMatch="true">
                                                    <Items></Items>
                                                    </telerik:RadComboBox> 
                                                </div>
                                            </div>
                                        </div>
                                          <asp:Panel runat="server" ID="pnl_File" Visible="true">
                                          <div class="row">
                                            <div class="col-sm-5">
                                                <label>File</label>
                                            </div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:FileUpload ID="file_import" runat="server"  /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="file_import" Display="Dynamic"  ErrorMessage="*Please upload only .xls Files" ValidationExpression="^(.)+(.xls|.XLS)$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                              </asp:Panel>    
                                          <div class="row">
                                            <div class="col-sm-5"></div>
                                            <div class="col-sm-7">
                                                <div class="form-group">
                                                    <asp:Button ID="btnimport" CssClass ="btn btn-warning" TabIndex="5"  
                                                        runat="server" Text="Import" />
                                                    <asp:Button ID="btnCancelImport" CssClass ="btn btn-default"  TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel" />
                                                </div>
                                            </div>
                                        </div>
                                        
                                       
                                                    <asp:Panel ID="Panel4" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif" style="z-index: 10010; vertical-align: middle;" />
                                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                                    </asp:Panel>
                                                
                                                                 </div>
                                             </div>                
                                         
                                    </asp:Panel>

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
