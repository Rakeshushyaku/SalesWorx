<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Supplier-Product-Code.aspx.vb" Inherits="SalesWorx_BO.Supplier_Product_Code" %>

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

        function alertCallBackFn(arg) {

        }

    </script>

    <script type="text/javascript">



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
            alert('Select at least one configurations!');
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



    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Supplier Product Codes </h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                            
                
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                            
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>
                                               Organisation</label>
                                            
                                                <telerik:RadComboBox ID="ddl_orgFilter" runat="server"  AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true" width="100%" Skin="Simple"  Filter="Contains">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                           </div>
                                             </div>
                                    
                                     <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Supplier</label>
                                             
                                                <telerik:RadComboBox ID="ddl_FilterSupplier" runat="server" width="100%" Skin="Simple"  Filter="Contains" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                             </div>
                                         </div>
                                               <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Product</label>
                                               
                                                    <telerik:RadComboBox ID="ddl_FilterProduct" EmptyMessage="-- Select a Product --" runat="server" AllowCustomText="false" MarkFirstMatch="true" width="100%" Skin="Simple"  Filter="Contains" AutoPostBack="true">
                                                    <Items></Items>
                                                    </telerik:RadComboBox>
                                                 </div>
                                         </div>

                                                <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>&nbsp;</label>
                                                <asp:Button ID="btnFilter" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-primary"
                                                    TabIndex="4" Text="Search" />
                                                     <asp:Button ID="btnAdd" runat="server" CausesValidation="false" CssClass="btn btn-sm btn-success"
                                                    OnClick="btnAdd_Click" TabIndex="1" Text="Add" />
                                                       <asp:Button ID="BTnImport" runat="server" CausesValidation="false" CssClass="btn btn-sm  btn-warning"
                                                      TabIndex="1" Text="Import" />
                                                    </div>
                                         </div>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                       
                            <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" UpdateMode="conditional">
                                <ContentTemplate>
                                     
                                                <asp:GridView Width="100%" ID="gvDivConfig" runat="server" EmptyDataText="No Supplier product code."
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
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HSupplier" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Custom_Attribute_1") %>'/>
                                                                <asp:HiddenField ID="HItemCode" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"item_code") %>'/>
                                                                <asp:HiddenField ID="HOrg" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Organization_ID") %>'/>
                                                                                                                             
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Code" ID="btnDelete" 
                                                                    OnClick="btnDelete_Click" runat="server" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="javascript:return confirm('Would you like to delete the selected Code?');" />
                                                                <asp:ImageButton ID="btnEdit" ToolTip="Edit Code" runat="server" CausesValidation="false"
                                                                   
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Custom_Attribute_1" HeaderText="Supplier" SortExpression="Custom_Attribute_1">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="product" HeaderText="Product" SortExpression="product">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Attrib_Value" HeaderText="Supplier Product Code"
                                                            SortExpression="Attrib_Value" HtmlEncode="false">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        
                                                    </Columns>
                                                   <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle CssClass="tdstyle" />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                </asp:GridView>
                                             
                                    <asp:Button ID="btnHiddenCurrency" CssClass="btnInput" runat="Server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender BackgroundCssClass="modalBackground" ID="MPEDivConfig"
                                        runat="server" PopupControlID="DetailPnl" TargetControlID="btnHiddenCurrency"
                                        >
                                    </ajaxToolkit:ModalPopupExtender>

                                    <asp:Panel ID="DetailPnl" runat="server" Width="500" CssClass="modalPopup" Style="display: none" >
                                        <div class="panelouterblk">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="DragPnl" runat="server" CssClass="popupbartitle">
                                            Supplier Product Code</asp:Panel>
                                                           
                                                     <asp:ImageButton ID="btn_close" runat="server"  ImageUrl="~/assets/img/close.jpg" CssClass="Closebtnimg"></asp:ImageButton>   
                                        <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                              <div class="popupcontentblk">
                                                                  <p>
                                        <asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Maroon"></asp:Label>
                                        </p>
                                                                  <div runat="server" id="adddiv" visible="false" >
                                          <div class="row">
                                                <div class="col-sm-12">
                                                    <label>
                                                    Organisation </label>
                                              
                                                 <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <telerik:RadComboBox ID="ddl_Org"  width="100%" Skin="Simple"  Filter="Contains" runat="server" AllowCustomText="false" MarkFirstMatch="true"  AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                 </div>
                                              </div>
                                            <div class="row">
                                             <div class="col-sm-12">
                                                    <label> Supplier </label>
                                                
                                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                                    <telerik:RadComboBox ID="ddl_Supplier"  width="100%" Skin="Simple"  Filter="Contains" runat="server" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel> 
                                                  </div>
                                             </div>
                                                   <div class="row">
                                             <div class="col-sm-12">
                                                    <label>   Products </label>
                                               
                                                   <telerik:RadComboBox ID="ddl_Product"  runat="server" AllowCustomText="false" MarkFirstMatch="true"  width="100%" Skin="Simple"  Filter="Contains">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </div>
                                                       </div>
                                                   <div class="row">
                                             <div class="col-sm-4">
                                                    <label> 
                                                    Code </label>
                                               
                                                    <asp:TextBox ID="txt_code" width="150" runat="server"></asp:TextBox>

                                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional">
                                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnl_File" Visible="false">
                                                  
                                            </asp:Panel>       
                                                     </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                                                                
                                                                
                                          
                                             
                                                 
                                                 
                                                <div class="col-sm-6"  >
                                                    <div class="form-group">
                                          <label>&nbsp;</label>
                                                    <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" TabIndex="5" OnClick="btnSave_Click"
                                                        runat="server" Text="Save" />
                                                    <asp:Button ID="btnUpdate" CssClass="btn btn-sm btn-success" Text="Update" OnClick="btnUpdate_Click"
                                                        runat="server" />
                                                    <asp:Button ID="btnCancel" CssClass="btn btn-sm btn-warning" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel"  OnClick="btnMPCancel_Click"/>
                                                        </div>
                                                 </div>
                                                       </div>

                                                                      </div>
                                                                  
<div   runat="server" id="uploadDiv" visible="false" >
                                                                  <table width="100%" cellpadding ="6" cellspacing ="6" border="0">
                                        <tr>
                                        <td colspan="2"> <asp:Label ID="Label1" runat="server" Text="" ForeColor="Maroon"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td  class="txtSMBold">
                                                Organization:</td>
                                            <td class="txtSMBold">
                                                <asp:DropDownList ID="ddl_Organization" runat="server" width="250" >
                                                
                                                </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="txtSMBold">
                                                    File:
                                                </td>
                                                <td>
                                                
                                                    
                                                    <asp:FileUpload ID="file_import" runat="server"  />
                                                    <asp:Button ID="btn_importCode" CssClass="btn btn-sm btn-primary" TabIndex="5" OnClick="btnimportcode_Click"
                                                        runat="server" Text="Import" />
                                                    
                                                    <asp:Button ID="btnCancelimport" CssClass="btn btn-sm btn-warning" TabIndex="6" runat="server" CausesValidation="false"
                                                        Text="Cancel"  OnClick="btnMPCancel_Click"/>
                                                      
                                                       
                                                </td>
                                            </tr>     
                                            
                                             <tr>
                                             <td colspan="2">
                                               <asp:LinkButton id="BtnDownLoad" runat="server" Text="Download Error Log" Visible="false" ></asp:LinkButton>
                                               
                                             </td>
                                             </tr>                      
                                           
                                            
                                        </table>
    </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            
                                           
                                            </Triggers>
                                                    </asp:UpdatePanel>
                                            </div> 
                                    </asp:Panel>
                                 
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="btnSave" />
       
     <asp:PostBackTrigger ControlID="btnSave" />
                                            <asp:PostBackTrigger ControlID="btnUpdate" />
                                            <asp:PostBackTrigger ControlID="btn_importCode" />
                                              <asp:PostBackTrigger ControlID="BtnDownLoad" />
                                </Triggers>
                            </asp:UpdatePanel>
                      
                
               
                 </contenttemplate>
    </asp:UpdatePanel>


    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl"
        runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../images/Progress.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>
    <br />
    <br />

</asp:Content>
