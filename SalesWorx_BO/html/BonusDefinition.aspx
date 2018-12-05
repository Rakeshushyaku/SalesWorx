<%@ Page Title="Bonus Definition" Language="vb" AutoEventWireup="false" EnableEventValidation="false"    
 MasterPageFile="~/html/Site.Master" CodeBehind="BonusDefinition.aspx.vb"  Inherits="SalesWorx_BO.BonusDefinition" %>

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

         <h4>Bonus Definition</h4>

         <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />
                 <span>Processing... </span>      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
                         
 
    
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    
               <div class="row">
                   <div class="col-sm-4">
                       <div class="form-group">
                            <asp:Button  ID="btnBack" runat="server" Text="Go Back" CssClass ="btn btn-default"  />
                        </div>
                   </div>
                                         <div class="col-sm-8">
                                            <div class="form-group text-right">
                         
                       
                        <asp:Button ID="btnExport" runat="server"  CssClass="btn btn-warning" Text="Export" TabIndex ="11" />
                                                        <asp:Button ID="btnImportWindow" runat="server" CssClass ="btn btn-warning" Text="Import" TabIndex ="12" /> 
                                                <asp:Button ID="btndownloadTemp" runat="server" CssClass ="btn btn-primary" Text="Download Template" TabIndex ="11" />                                  
       </div>

              </div>
                   </div>                                                   
      
                
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode ="Conditional" >
                             <Triggers>
                             <asp:PostBackTrigger  ControlID="btnExport"   />
                            
                              
                          <asp:PostBackTrigger  ControlID="btnImportWindow"  />
                                  <asp:PostBackTrigger  ControlID="btndownloadTemp"  />
                        
                            </Triggers>          
                                  
                                <ContentTemplate>
                                
                                      
                                
                                
                                
                                
                        
                                   
                                    <asp:Panel ID="PnlOrderDetails"  GroupingText="" runat="server"  >
                                       
                                 <div class="row">
                                 <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Organization</label> 

                                  
                                  
                                        <telerik:RadComboBox Skin="Simple"  ID="ddl_org" Visible ="false" runat="server" Width ="200px"   AutoPostBack="true">
        </telerik:RadComboBox>
                                  <asp:Label runat ="server" Font-Bold ="true" ForeColor ="#248aaf" ID="lblOrg"></asp:Label>
                                    
                                </div>
                                     </div>
                               
                            <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>Plan Name
                                  </label>
                                    
                                    <asp:Label ID="lblPlanName"  runat ="server" Font-Bold ="true"  ForeColor="#248aaf"  ></asp:Label>
                                    
                                </div>
                                </div>
                            </div>
                            <div class="row">
                                          <div class="col-sm-3">
                                            <div class="form-group">
                                   <label> Order Item </label>
                                               
    <telerik:RadComboBox Skin="Simple" ID="ddlOrdCode" Filter="Contains"  EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="1"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server"
                                                   Width="100%" Height="175px">
                                                </telerik:RadComboBox>
                                               </div>
                                              </div>
                                 <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                 
                                                    UOM </label>
                                                
                                                     <telerik:RadComboBox Skin="Simple" CssClass ="inputSM" ID="ddlPUOM" Width="100%" runat="server" TabIndex ="5" AutoPostBack ="false">
                                                   
                                                    </telerik:RadComboBox>
                                                  
                                                </div>
                                         </div>
                                                   <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                    Description 
                                                </label>
                                                 
                                                 <telerik:RadComboBox Skin="Simple" ID="ddlOrdDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending" 
                                                    MinimumFilterLength="1"  runat="server"  AutoPostBack ="true" 
                                                     Width="100%" Height="175px">
                                                </telerik:RadComboBox>
                                                </div>
                                                       </div>
                                 </div>
                            <div class="row">            
                                            <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                   Bonus Item</label>
                                                  
  <telerik:RadComboBox Skin="Simple" ID="ddlGetCode" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="3"  Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Width="100%" Height="175px">
                                                </telerik:RadComboBox>
                                                    </div>
                                                </div> 
                                 <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                 
                                                    UOM </label>
                                                
                                                     <telerik:RadComboBox Skin="Simple" CssClass ="inputSM" ID="ddlBUOM" Width="100%" runat="server" TabIndex ="5" AutoPostBack ="false">
                                                   
                                                    </telerik:RadComboBox>
                                                  
                                                </div>
                                         </div>
                                               <div class="col-sm-6">
                                            <div class="form-group">
                                   <label>
                                                    Description 
                                                 </label>
                                                   <telerik:RadComboBox Skin="Simple" ID="ddlgetDesc" Filter="Contains" EmptyMessage ="Please select"
                                                   EnableLoadOnDemand="True" TabIndex="4" Sort ="Ascending"   AutoPostBack ="true" 
                                                    MinimumFilterLength="1"  runat="server" 
                                                    Width="100%" Height="175px">
                                                </telerik:RadComboBox>
                                               </div>
                                                   </div>
                                </div>
                            <div class="row">
                                             <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                 
                                                    Bonus Type </label>
                                                
                                                     <telerik:RadComboBox Skin="Simple" CssClass ="inputSM" ID="ddlType" Width="100%" runat="server" TabIndex ="5" AutoPostBack ="false">
                                                   
                                                    </telerik:RadComboBox>
                                                       <asp:HiddenField  ID="hfOrgID" runat="server" />
                                    <asp:HiddenField  ID="hfPlanId" runat="server" />
                                                </div>
                                         </div>
                                           <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                   From Qty </label>
                                                 
                                                    <asp:TextBox ID="txtFromQty" runat="server"  TabIndex ="6" 
                                                            CssClass="inputSM" Width="100%" MaxLength="8"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FTEOrdQty" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtFromQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                               </div>
                                     
                                                
                                             
                                                 <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                   To Qty </label>
                                                 
                                                    <asp:TextBox ID="txtToQty" runat="server"  TabIndex ="7"
                                                            CssClass="inputSM" Width="100%" MaxLength="8"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtToQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                                     </div>
                                                <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                                                  Bonus Qty </label>
                                              
                                                    <asp:TextBox ID="txtGetQty" runat="server" TabIndex ="8"
                                                            CssClass="inputSM" Width="100%" MaxLength="8"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtGetQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                
                                                
                                                
                                               
                                                    <asp:TextBox ID="txtAddPercent" Visible ="false"  runat="server" TabIndex ="8" MaxLength ="3" 
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtAddPercent" >
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                               </div>
                                                    </div>


                                </div>
                            <div class="row">
                                     
                <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                  Valid From</label>
                 
                  <telerik:RadDateTimePicker ID="StartTime"  MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"    Width="100%" TabIndex ="9"    runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                  <asp:RequiredFieldValidator runat="server" Visible ="false" Width ="3px" ID="RequiredFieldValidator1" ControlToValidate="StartTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator></div>
                    </div>
                                     <div class="col-sm-3">
                                            <div class="form-group">
                                   <label>
                      
                     Valid To</label>
                 <telerik:RadDateTimePicker ID="EndTime" MinDate ="1900-01-01 00:00:00.000"  MaxDate ="9999-12-31 00:00:00.000"  Width="100%" TabIndex ="10"   runat="server">
                                    <DateInput DateFormat ="dd-MM-yyyy" readonly="true" ></DateInput>
                                   
                                </telerik:RadDateTimePicker>
                                 <asp:RequiredFieldValidator runat="server" Visible ="false" ID="Requiredfieldvalidator2" Width ="3px"  ControlToValidate="EndTime"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="EndTime"
                        ControlToCompare="StartTime" Operator="GreaterThan"    Type="String"
                        ErrorMessage="To date > From date">
                    </asp:CompareValidator>
                       
                     
                        </div>
                                         </div>
                                 
                                                 <div class="col-sm-2">
                                            <div class="form-group">
                                   <label>
                                                   Maximum FOC Qty </label>
                                                 
                                                    <asp:TextBox ID="txtMaxQty" runat="server" MaxLength ="5"  TabIndex ="8"  
                                                            CssClass="inputSM" Width="100%"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                                            FilterType="Numbers,Custom" TargetControlID="txtMaxQty">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                                     </div>
                                           
                                              <div class="col-sm-4">
                                            <div class="form-group">
                                     <label class="hidden-xs empty-label"><br /></label>
                                               
                                                
                                                         <asp:Button ID="btnAddItems" runat="server" CssClass ="btn btn-success" Text="Add" TabIndex ="9" />
                                                        <asp:Button ID="btnClear" runat="server" CssClass ="btn btn-default"  Text="Clear" TabIndex ="10" />
                                                   
                                                   <asp:Button ID="btnGo" Visible ="false"  runat="server" CssClass="btn btn-default" Text="Go Back" TabIndex ="10" />
                                                     <span class="checkbox-dispin">
                                                   <asp:CheckBox ID="chShow" runat ="server" Text ="Show deactivated items" AutoPostBack ="true" Width="200"  />
                                                         </span>
                                                    
                                             
                                        
                                          
                                            
                                                        <asp:Label ID="lblDItemCode" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                    
                                                        <asp:Label ID="LblDItemId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                            <asp:Label ID="lblEditRow" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                   
                                                        <asp:Label ID="lblDDescription" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                 
                                                        <asp:Label ID="lblBItemCode" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                     
                                                        <asp:Label ID="LblBItemId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblLineId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                     
                                                        <asp:Label ID="lblBDescription" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                           <asp:Label ID="lblOrgId" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                               <asp:Label ID="lblVF" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                             <asp:Label ID="lblVT" Visible="false" Width="45" CssClass="inputSM" runat="server"></asp:Label>
                                                               <asp:Label ID="lblDUOM" Visible ="false"  runat="server"></asp:Label>
                                                         <asp:Label ID="lblBUOM" Visible ="false"  runat="server"></asp:Label>
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
                                
                                     
                                             
                                                    <telerik:RadWindow ID="MPEImport" Title= "Import Bonus Rules" runat="server" Skin="Windows7" Behaviors="Move,Close"
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
                                                                <asp:Button ID="btnCancelImport"  CssClass ="btn btn-default"  TabIndex="5" runat="server" CausesValidation="false" Text="Close" />
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
                                         <div class="overflowx">
                                                    <asp:GridView Width="100%" ID="dgvItems" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="LineId"
                                                        AllowPaging="true" AllowSorting="true"  PageSize="5" CellPadding="0" CellSpacing="0" CssClass="tablecellalign"  >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                        <asp:TemplateField>
                                                           <HeaderStyle HorizontalAlign="Left" />
                                                           
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblStatus"  CssClass="txtLinkSM" ForeColor='<%# System.Drawing.Color.FromName(DataBinder.Eval(Container.DataItem,"IsColor").ToString()) %>'  Font-Bold ="true"  CommandName="DeActivate" runat="server" Text='<%# Bind("IsActive") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"  Visible ="false"  HeaderStyle-Wrap="false" DataField="LineId"
                                                                HeaderText="Line No">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                                                                                      
                                                                  <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="ItemCode" HeaderText="Order Item" SortExpression ="ItemCode"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="DUOM" HeaderText="UOM"
                                                                NullDisplayText="N/A"  SortExpression ="DUOM" >
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                  
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="BItemCode" HeaderText="Bonus Item" SortExpression ="BItemCode"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="BUOM" HeaderText="UOM"
                                                                NullDisplayText="N/A"  SortExpression ="BUOM" >
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                          
                                                            
                                                        
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="FromQty"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ToQty"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                 <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                               <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="TypeCode"
                                                                HeaderText="Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="GetQty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Get_Add_Per"
                                                                HeaderText="Addl.%"  Visible ="false"  DataFormatString="{0:F2}">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  DataFormatString="{0:dd-MM-yyyy}"   SortExpression ="Valid_From">
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"  SortExpression ="Valid_To" DataField="Valid_To"
                                                                HeaderText="Valid To"  DataFormatString="{0:dd-MM-yyyy}" >
                                                               <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="MaxQty"
                                                                HeaderText="Max.Foc Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"   runat="server" Visible ='<%# Bind("IsVisible") %>' CommandName="EditRecord"
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
                                                          
                                                              <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDItem" runat="server" Text='<%# Bind("DItemId") %>'></asp:Label>
                                                                 <asp:Label ID="lblACode" runat="server" Text='<%# Bind("ACode") %>'></asp:Label>
                                                                 <asp:Label ID="lblADesc" runat="server" Text='<%# Bind("ADesc") %>'></asp:Label>
                                                                <asp:Label ID="lblOrderUOM" runat="server" Text='<%# Bind("DUOM")%>'></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         
                                                          <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBonusUOM" runat="server" Text='<%# Bind("BUOM")%>'></asp:Label>
                                                                <asp:Label ID="lblBItem" runat="server" Text='<%# Bind("BItemId") %>'></asp:Label>
                                                                 <asp:Label ID="lblBCode" runat="server" Text='<%# Bind("BCode") %>'></asp:Label>
                                                                 <asp:Label ID="lblBDesc" runat="server" Text='<%# Bind("BDesc") %>'></asp:Label>
                                                                    <asp:Label ID="lblValidFrom" runat="server" Text='<%# Bind("Valid_From") %>'></asp:Label>
                                                                       <asp:Label ID="lblValidTo" runat="server" Text='<%# Bind("Valid_To") %>'></asp:Label>
                                                                    <asp:Label ID="lblMaxQty" runat="server" Text='<%# Bind("MaxQty")%>'></asp:Label>
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
                                   
                                       
                                      
                                               
                                                    <telerik:RadWindow ID="MPEAlloc" Title= "Confirmation" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                               AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
                                              <ContentTemplate>
                                                  
                                                    <table id="table1" width="100%" cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;display:none ;
                                                                border: solid 1px #3399ff;  padding: 3px" >
                                                                
                                                                <asp:Label ID="lblinfo1" runat="server" Font-Size ="12px"></asp:Label>
                                                                <asp:Label ID="lblAction" Visible="false" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="center" style="text-align: center">
                                                            <asp:Label ID="lblMsg2" runat="server"  ForeColor ="Green" Font-Bold ="true"  ></asp:Label><br /><br />
                                                                <div class="overflowx">
                                                              <asp:GridView Width="400px" ID="dgvActive" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="ActiveLineId"
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0"  CssClass="tablecellalign" >
                                                       
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                      
                                                          
                                                           
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" DataField="ItemName" HeaderText="Item Name"
                                                                NullDisplayText="N/A">
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                      
                                                          
                                                                                                                   
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Prom_Qty_From"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                    <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Prom_Qty_To"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Get_Qty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                                <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_From"
                                                                HeaderText="Valid From"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Valid_To"
                                                                HeaderText="Valid To"  DataFormatString="{0:dd-MM-yyyy}" >
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                          
                                                          <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="MaxQty"
                                                                HeaderText="Max.FOC Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Right" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                            </asp:BoundField>
                                                          
                                                              <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActiveLineID" runat="server" Text='<%# Bind("ActiveLineID") %>'></asp:Label>
                                                              
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         
                                                       
                                                          
                                                        </Columns>
                                                       <PagerStyle CssClass="pagerstyle" />
                                                    <HeaderStyle />
                                                    <RowStyle CssClass="tdstyle" />
                                                    <AlternatingRowStyle CssClass="alttdstyle" />
                                                    </asp:GridView>
                                                                    </div>
                                                            </td>
                                                        </tr>
                                                       
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblmessage1" runat="server" Font-Size ="12px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center" style="text-align: center;">
                                                                <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate & Apply" CssClass ="btn btn-primary"  />
                                                                <asp:Button ID="btnhide" Visible="false" CssClass="btnInput" TabIndex="7" runat="server"
                                                                    Text="Cancel" CausesValidation="false" OnClientClick="$find('MPEAlloc').Hide(); return false;" />
                                                                <asp:Button ID="btnClose1" runat="server" Text="Cancel"  CssClass ="btn btn-danger"  />
                                                             
                                                            </td>
                                                        </tr>
                                                    </table>
                                                      <div class="form-group text-center">
                                     
                                    <asp:Button ID="Button2" Visible="false" CssClass="btn btn-default" TabIndex="26" runat="server"
                                        Text="OK" CausesValidation="false" OnClientClick="$find('MPError').Hide(); return false;" />
                                 

                                </div>
                                              </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                       <telerik:RadWindow ID="MPInfo" Title= "Information" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                          Width="500" Height="420"    AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
                                              <ContentTemplate>
                                                  <br />
                                                 <b>You can not add the item as it already exists in the below plans. </b>
                                                  <div style="height:400px;width:550px;overflow-y:scroll " >
                                                     <telerik:RadGrid id="GV_Item_error" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="5" AllowPaging="false" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                   >
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                       
                                                             
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Response" HeaderText="Plan Details"
                                                                  SortExpression ="Response" >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                      </div>
                                                  <div class="form-group text-center">
                                     
                                    <asp:Button ID="Button1" Visible="false" CssClass="btn btn-default" TabIndex="26" runat="server"
                                        Text="OK" CausesValidation="false" OnClientClick="$find('MPInfo').Hide(); return false;" />
                                 

                                </div>
                                              </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 

                                

                        
                    
                            
                                
                                            
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="Panel" runat="server" 
                                DisplayAfter="10">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                                        <span style="font-size: 12px; font-weight:700px;color: #3399ff;">Processing...
                                        
                                        </span>
                                    </asp:Panel>
                                  
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                                        <span>Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                    
                <br />
                <br />
            
</asp:Content>
