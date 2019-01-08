<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ProductMinimumStock.aspx.vb" Inherits="SalesWorx_BO.ProductMinimumStock" %>
 

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
     .rcTimePopup    
 {
   display:none ! important;
 }
 </style><script language="javascript" type="text/javascript">

      function HideRadWindow() {

          var elem = $('a[class=rwCloseButton');

          if (elem != null && elem != undefined) {
              $('a[class=rwCloseButton')[0].click();
          }

          $("#frm").find("iframe").hide();
      }

      function alertCallBackFn(arg) {
          HideRadWindow()
      }

      var TargetBaseControl = null;

      window.onload = function () {
          try {
              TargetBaseControl =
         document.getElementById('<%= Me.Panel.ClientID%>');
            }
            catch (err) {
                TargetBaseControl = null;
            }
        }

      function TestCheckBox() {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkDelete";
         
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[n].checked) {
                    return confirm('Would you like to delete the selected codes?');
                    return true;
                }
            alert('Select at least one codes!');
            return false;

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

      function CheckAll(cbSelectAll) {
          if (TargetBaseControl == null) return false;
          var TargetChildControl = "chkDelete";
          

          var Inputs = TargetBaseControl.getElementsByTagName("input");

          for (var n = 0; n < Inputs.length; ++n)
              if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0) {
                  Inputs[n].checked = cbSelectAll.checked;
              }

      }



    </script><script type="text/javascript">
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

         <h4>Product Minimum Stock</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
          
                                                                 
       <asp:UpdatePanel ID="TopPanle" runat="server" >
            <ContentTemplate>

                
                                        <div class="row">
                                            
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization</label>
                                            
                                                <telerik:RadComboBox  Skin="Simple"  ID="ddl_FilterOrg" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox>
                             
                                            </div>
                                          </div>
                                                     <%--<div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Product</label>
                                           <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Product"   
                                               DataTextField="Description" DataValueField="item_code"
                                              ID="ddl_FilterVan"  runat="server"  Width ="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                              
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>--%>


                                           <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Product</label>
                                                   <telerik:RadComboBox  Skin="Simple" EnableLoadOnDemand="true"  EmptyMessage ="Please enter Item code/Name"
                                                          ID="ddl_FilterCustomer" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox>
                                            </div>
                                          </div>
                                  
                                        
                                         


                                             
                                             
                                          
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button ID="btn_Search" runat="server" Visible="true" CausesValidation="false"   CssClass="btn btn-default"
                                                    TabIndex="2" Text="Search" /> 
                                                <asp:Button ID="btn_clearFilter" runat="server" CausesValidation="false"   CssClass="btn btn-default"
                                                    TabIndex="1" Text="Clear Filter" />
                                                        
                                                     
                                                 
                                                
                                                      
                                            </div>
                                           
                                            </div>
                                            <div class="col-sm-3">
                                                
                                           
                                            </div>
                                        </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                        <asp:Button ID="btnAdd" runat="server" CausesValidation="false"  CssClass="btn btn-success"
                                                    TabIndex="1" Text="Add" />
                                                    <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning" Text="Import" TabIndex ="12" />
                                                    <asp:Button ID="btnExport" runat="server" CssClass ="btn btn-danger" Text="Export" TabIndex ="11" />
                                                    <asp:Button ID="btndownloadTemp" runat="server" CssClass ="btn btn-primary" Text="Download Template" TabIndex ="11" />
                            </div>
                    </div>
                </div>
                
                </ContentTemplate>
                </asp:UpdatePanel>

         <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                                         <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                        
                                   <asp:PostBackTrigger  ControlID="btndownloadTemp"  />
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                      
                                
                                
                                
                                
                        
                        
                                
                                </ContentTemplate>
          </asp:UpdatePanel>
                            <asp:UpdatePanel ID="Panel" runat="server" UpdateMode="Conditional" >
                             <Triggers>
           
      
            
	
        </Triggers>
                                <ContentTemplate>
                                
                                      
                                             
                                                    <telerik:RadWindow ID="MPEImport" Title= "Import Product Minimum Stock" runat="server" Skin="Windows7" Behaviors="Move,Close"
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
                                                                                    CausesValidation="false" Text="Cancel" />
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
                                             
                                
                                

                                     
                                             
                                                    <telerik:RadWindow ID="MPEAdd" Title= "Add Minimum Stock Quantity" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                         <ContentTemplate>
                               <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                        <div class="popupcontentblk">
                                        <p><asp:Label ID="lblmsgPopUp" runat="server" Text="" ForeColor="Red"></asp:Label></p>
                                       
                                            
                                              <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Organization</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                   
                                                 <telerik:RadComboBox  Skin="Simple"  ID="ddl_OrgAdd" runat="server" width="100%" AllowCustomText="false" MarkFirstMatch="true" AutoPostBack="true">
                                                <Items></Items>
                                                </telerik:RadComboBox>



                                                </div>
                                            </div>
                                        </div>

                                            <%--  <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Van</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                     <telerik:RadComboBox Skin="Simple" ID="ddl_VanAdd" EmptyMessage="Select Van" height="200px"    Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" AutoPostBack="true">
                                        </telerik:RadComboBox >
                                                </div>
                                            </div>
                                        </div>--%>

                                            
                                             <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Products</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <telerik:RadComboBox  Skin ="Simple" ID="ddl_Customer" EnableLoadOnDemand="true"  EmptyMessage ="Please enter Item code/Name"
                                                      AutoPostBack="true" height="200px"   width="100%" runat="server" AllowCustomText="false" MarkFirstMatch="true">
                                                <Items></Items>
                                                </telerik:RadComboBox> 
                                                </div>
                                            </div>
                                        </div>


                                        <div class="row">
		                                    <div class="col-sm-5">
			                                    <label>Minimum Stock Qty</label>
		                                    </div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <asp:TextBox ID="txtQty" width="100%" runat="server" onKeypress='return NumericOnly(event)'  ></asp:TextBox>
                                                    <asp:RegularExpressionValidator id="rglExpression" runat="server" ControlToValidate ="txtQty" ValidationExpression ="^0$|^[1-9][0-9]*$" ErrorMessage ="Please enter Valid Quantity Number and decimal only" ValidationGroup ="v1" ></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rqd" runat="server" ErrorMessage ="Please Enter Quanity" ControlToValidate ="txtQty" ValidationGroup="v1" Display ="Dynamic" ></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
		                                    <div class="col-sm-5"></div>
		                                    <div class="col-sm-7">
			                                    <div class="form-group">
                                                    <asp:Button ID="btnSave" CssClass ="btn btn-success" ValidationGroup ="v1" TabIndex="5" OnClick="btnSave_Click"
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
                                    <asp:Panel ID="PnlGridData" runat="server" Visible ="false" >
                                        <table border="0"  cellspacing="0" cellpadding="0"  Width="100%">
                                            <tr>
                                                <td  Width="100%">
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" 
                                                        AllowPaging="true" AllowSorting="true"  PageSize="10" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                         <asp:TemplateField HeaderText="">
                                                               <HeaderTemplate>
                                                                <asp:CheckBox ID="cbSelectAll" onclick="CheckAll(this)" ToolTip="Select/Unselect All"
                                                                    runat="server" />
                                                                <asp:ImageButton ToolTip="Delete Selected Items " ID="btnDeleteAll" runat="server"
                                                                    OnClick="btnDeleteAll_Click" CausesValidation="false" ImageUrl="~/images/delete-13.png"
                                                                    OnClientClick="return TestCheckBox()" />
                                                            </HeaderTemplate>
                                                             <HeaderStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server"  CssClass="checkboxvalign" />
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" 
                                                                        OnClick="btnDelete_Click" CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
                                                                    <asp:ImageButton ID="btnEdit" ToolTip="Edit Code" runat="server" CausesValidation="false"
                                                                   
                                                                      ImageUrl="~/images/edit-13.png"   OnClick="btnEdit_Click" />
                                                                </ItemTemplate>
                                                             <ItemStyle HorizontalAlign="Left" Width="100px" CssClass="display-block"  />
                                                            </asp:TemplateField>

                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="item_code" SortExpression ="Site_No"
                                                                HeaderText="Item Code">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>

                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Description" SortExpression ="SalesRep_Number"
                                                                HeaderText="Item Description">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>

                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Qty" SortExpression ="SalesRep_Name"
                                                                HeaderText="Minimum Stock Qty">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>

                                                            
                                                                                                                                                                           
                                                         
                                                        
                                                        
 
                                                           <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItem" runat="server" Text='<%# Bind("item_code")%>'></asp:Label>
                                                                <asp:Label ID="LblDescription" runat="server" Text='<%# Bind("Description")%>'></asp:Label>
                                                                  <asp:Label ID="LblQty" runat="server" Text='<%# Bind("Qty")%>'></asp:Label>
                                                                  <asp:Label ID="lblMAS_Org_ID" runat="server" Text='<%# Bind("MAS_Org_ID")%>'></asp:Label>
                                                                  <asp:Label ID="lblInventoryId" runat="server" Text='<%# Bind("Inventory_Item_ID")%>'></asp:Label>

                                                                 
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         
                                                           
                                                        
                                                          
                                                        </Columns>
                                                       <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                   </div>
                                        
                         
                               
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                                        <span>Processing...</span>
                                    </asp:Panel>
                                  
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                    
               
            
</asp:Content>
