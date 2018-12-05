<%@ Page Title="Price Definition" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/Site.Master" CodeBehind="PriceDefinition.aspx.vb"  Inherits="SalesWorx_BO.PriceDefinition" %>

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
 </style> 

  <script language="javascript" type="text/javascript">

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


    </script>

    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

         <h4>Price Definition</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
                 <span>Processing... </span>
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    
             
                
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                     <%--     <asp:PostBackTrigger  ControlID="btnImportWindow"  />--%>
                        
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                      
                                  <div class="row">
                   <div class="col-sm-4">
                       <div class="form-group">
                           
                        </div>
                   </div>
                                         <div class="col-sm-8">
                                            <div class="form-group text-right">
                         
                       
                        <asp:Button ID="btnExport" runat="server"  CssClass="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass="btn btn-warning"  Text="Import" TabIndex ="12" />                                   
       </div>

              </div>
                   </div>                                                   
      
                                
                                
                                
                        
                                   
                                    <asp:Panel ID="PnlOrderDetails"  GroupingText="" runat="server"  >
                                       
                                 <div class="row">
                                 <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Organization</label> 

                                  
                                  
                                        <telerik:RadComboBox Skin="Simple"  ID="ddl_org" Visible ="false" runat="server" Width ="100%"   AutoPostBack="true">
        </telerik:RadComboBox>
                                  <asp:Label runat ="server" Font-Bold ="true" ForeColor ="#248aaf" ID="lblOrg"></asp:Label>
                                    
                                </div>
                                     </div>
                               
                            <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Price List Name
                                  </label>
                                    
                                    <asp:Label ID="lblPlanName"  runat ="server" Font-Bold ="true"  ForeColor="#248aaf"  ></asp:Label>
                                    
                                </div>
                                </div>
                            </div>
                            <div class="row">
                                          <div class="col-sm-6">
                                            <div class="form-group">
                                   <label> Product Name <em><span>&nbsp;</span>*</em></label>
                                               
    <telerik:RadComboBox Skin="Simple" ID="ddlOrdCode" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="1"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                   Width="100%" Height="175px">
                                                </telerik:RadComboBox>
                                               </div>
                                              </div>

                                                
                              
                                             <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                 
                                                    UOM <em><span>&nbsp;</span>*</em></label>
                                                
                                                     <telerik:RadComboBox Skin="Simple" CssClass ="inputSM" ID="ddlUOM" Width="100%" Height="175px" runat="server" TabIndex ="5" >
                                                   
                                                    </telerik:RadComboBox>
                                                       <asp:HiddenField  ID="hfOrgID" runat="server" />
                                    <asp:HiddenField  ID="hfPlanId" runat="server" />
                                                </div>
                                         </div>
                                </div> 
                                   <div class="row">
                                           <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                   Unit Selling Price<em><span>&nbsp;</span>*</em> </label>
                                                   <telerik:RadNumericTextBox runat="server" ID="txtUnitPrice"  Width="100%"
                                              MinValue="0" Skin ="Simple" TabIndex ="4" MaxLength ="7" 
                                            autocomplete="off" NumberFormat-DecimalDigits="2"
                                            AllowOutOfRangeAutoCorrect="false" 

                                            IncrementSettings-InterceptMouseWheel="false" IncrementSettings-InterceptArrowKeys="false">
                                        </telerik:RadNumericTextBox>
                                                </div>
                                               </div>
                                     
                                                
                                          
                           
               
                                           
                                              <div class="col-sm-6">
                                            <div class="form-group">
                                     <label class="hidden-xs empty-label"><br /></label>
                                               
                                                
                                                         <asp:Button ID="btnAddItems" runat="server" CssClass ="btn btn-success" Text="Add" TabIndex ="9" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-default"  Text="Clear" TabIndex ="10" />
                                                
                                                     <asp:Button  ID="btnBack" runat="server" Text="Go Back" CssClass ="btn btn-primary"  />
                                             
                                        
                                          
                                            
                                                                                                          
                                                        <asp:Label ID="LblDItemId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                            <asp:Label ID="lblEditRow" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                   
                                                         <asp:Label ID="lblLineID" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                       
                                                           <asp:Label ID="lblOrgId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                        
                                                                                                                   
                                                               <asp:Label ID="lblDUOM" Visible ="false"  runat="server"></asp:Label>
                                                       
                                                     </div>
                                                  </div>
                                     </div>
                                    </asp:Panel>
                                    
                                
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         
                            <asp:UpdatePanel ID="Panel" runat="server" >
                             <Triggers>
           
      
            
	
        </Triggers>
                                <ContentTemplate>
                                
                                     
                                             
                                                    <telerik:RadWindow ID="MPEImport" Title= "Import Price Details" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                    <asp:HiddenField ID="HFV" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                                                      <p><asp:Label runat ="server" ID="Label6" ForeColor="Red"  Text =""></asp:Label>
                                                          <asp:Label runat ="server" ID="lblUpMsg" ForeColor="Red"> </asp:Label>
                                                      </p>
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
                                                        <div class="col-sm-5"></div>
		                                                <div class="col-sm-7">
			                                                <div class="form-group">
                                                                <asp:Button ID="btnImport" runat="server" Text="Import" CssClass ="btn btn-warning" /> 
                                                                <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default"  TabIndex="5" runat="server" Visible ="false"  CausesValidation="false" Text="Close" />
                                                                <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                                                                <asp:Button ID="BtnReimport" runat="server" Text="Reimport"  Visible ="false" CssClass ="btn btn-warning" />
                                                                <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
                                                                <asp:LinkButton ID="lbLog" ToolTip ="Click here to download the uploaded log" runat="server" Text ="View Log" Visible="false" ></asp:LinkButton>
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
                                             
                                
                               
                                    <asp:Panel ID="PnlGridData" runat="server" Visible ="false" >
                                         <div class="table-responsive">
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="LineId"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="5" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                          <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" CommandName="EditRecord"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteRecord"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected item?');" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"  Visible ="false"  HeaderStyle-Wrap="false" DataField="LineId"
                                                                HeaderText="Line No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="ItemCode" HeaderText="Product" SortExpression ="ItemCode"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                          
                                                               <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="DUOM"
                                                                HeaderText="UOM" SortExpression ="DUOM">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Center" SortExpression ="UnitSellingPrice" HeaderStyle-Wrap="false" DataField="UnitSellingPrice"
                                                                HeaderText="Unit Selling Price"  DataFormatString="{0:F2}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                           
                                                              
                                                        
                                                          
                                                              <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDItem" runat="server" Text='<%# Bind("DItemId") %>'></asp:Label>
                                                                  <asp:Label ID="lblDCode" runat="server" Text='<%# Bind("DItemCode")%>'></asp:Label>
                                                                                                                          </ItemTemplate>
                                                        </asp:TemplateField>
                                                         
                                                        
                                                        
                                                          
                                                        </Columns>
                                                       <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                                             </div>   
                                    </asp:Panel>
                                   
                                       
                                      
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" 
                                DisplayAfter="10">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" />
                                        <span>Processing... </span>
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
