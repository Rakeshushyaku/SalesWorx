<%@ Page Title="Assortment Plan Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AssortmentDefinition.aspx.vb" Inherits="SalesWorx_BO.AssortmentDefinition" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
 <style>
     .rcTimePopup
 {
   display:none ! important;
 }
  .RadListBox_Default .rlbText, .RadListBox_Default .rlbItem, .RadListBox_Default .rlbButtonText, .RadListBox_Default .rlbEmptyMessage {
font: 13px "Calibri" ;
color: black;
}
.RadComboBox_Default .rcbInput, .RadComboBoxDropDown_Default {
font: 13px "Calibri";
color: black;
}
 </style> 
    <script>
        function alertCallBackFn(arg) {

        }
    </script>
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Assortment Plan Details</h4>
         <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

         <div class="">
               <asp:Button ID="btnGoBack"  CssClass ="btn btn-default"  runat ="server" Text ="Go Back" />
                   </div>                           
             <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                  
                                  
                                                  
                                          
                           
                                
                                        <asp:Panel ID="Panel1" runat="server" GroupingText ="Assortment Bonus Details">

                                            <!--div class="row">
                                                <div class="col-sm-12">
                                                    <p>Checkbox indicates  mandatory item</p>
                                                </div>
                                            </div-->

                                            <div class="row">
                                                <div class="col-sm-3">
                                                    <label>Organization</label>
                                                    <div class="form-group">
                                                        <asp:Label ID="lblOrgID" runat ="server" Visible ="false" ></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat ="server" cssClass="text-primary"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <label>Plan Name</label>
                                                    <div class="form-group">
                                                        <asp:Label ID="lblPlanId" Visible ="false" runat ="server" ></asp:Label>   
                                                        <asp:Label ID="lblPlanName"  runat ="server" cssClass="text-primary"></asp:Label> 
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <strong>Ordered Items</strong>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <label>Item Code</label>
                                                            <div class="form-group">
                                                                <telerik:RadComboBox Skin="Simple"  ID="ddlOrdCode" Filter="Contains"  EmptyMessage ="Please type product code"
                                                                   EnableLoadOnDemand="True" TabIndex="1"  Sort ="Ascending"   AutoPostBack ="true" 
                                                                    MinimumFilterLength="1"  runat="server"  
                                                                    Height="200px" Width="100%">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label>Description</label>
                                                            <div class="form-group">
                                                                <telerik:RadComboBox Skin="Simple" ID="ddlOrdDesc" Filter="Contains" EmptyMessage ="Please type product decription"
                                                                   EnableLoadOnDemand="True" TabIndex="2"  Sort ="Ascending" 
                                                                    MinimumFilterLength="1"  runat="server" CssClass ="inputSM" AutoPostBack ="true" 
                                                                    Height="200px" Width="100%">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group"> 
                                                                <asp:Button ID="btnOrdAdd" CssClass="btn btn-success" runat ="server" Text ="Add" />
                                                                <asp:Button ID="btnOrdRemove"  CssClass ="btn btn-danger"  runat ="server" Text ="Remove" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                                <telerik:RadListBox ID="lstOrderd"  CssClass ="inputSM"    
                                                                        tooltip="Press CTRL key for multi select item"
                                                                        SelectionMode ="Multiple" Width="100%" AutoPostBack ="true" 
                                                                        runat="server" CheckBoxes="true" ShowCheckAll="true" Height="250px">
                                                                </telerik:RadListBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <strong>Bonus Items</strong>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6">
                                                            <label>Item Code</label>
                                                            <div class="form-group">  
                                                                <telerik:RadComboBox Skin="Simple"  ID="ddlGetCode" Filter="Contains"  EmptyMessage ="Please type product code"
                                                                   EnableLoadOnDemand="True" TabIndex="3"  Sort ="Ascending"   AutoPostBack ="true" 
                                                                    MinimumFilterLength="1"  runat="server"  
                                                                    Height="200px" Width="100%">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <label>Description</label>
                                                            <div class="form-group"> 
                                                                <telerik:RadComboBox Skin="Simple"  ID="ddlgetDesc" Filter="Contains"  EmptyMessage ="Please type product decription"
                                                                   EnableLoadOnDemand="True" TabIndex="4" Sort ="Ascending"   AutoPostBack ="true" 
                                                                    MinimumFilterLength="1"  runat="server" 
                                                                    Height="200px" Width="100%">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                    </div> 
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group"> 
                                                                <asp:Button ID="btnGetAdd"   CssClass="btn btn-success" runat ="server" Text ="Add" />
                                                                <asp:Button ID="btnGetRemove"  CssClass ="btn btn-danger"  runat ="server" Text ="Remove" />
                                                               <asp:Button ID="btnCopy"  CssClass ="btn btn-primary" runat ="server" Text ="Copy Ordered &rArr; Bonus" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                                <telerik:RadListBox ID="lstGet"   CssClass ="inputSM"   SelectionMode ="Multiple"  
                                                                        runat="server"  Width="100%"   tooltip="Press CTRL key for multi select item" Height="250px">
                                                                </telerik:RadListBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                                <asp:Panel ID="ZoneReg"  runat="server" GroupingText ="Bonus Rules">
                                                    <div class="row">
                                                        <div class="col-sm-3">
                                                            <div class="">
                                                                  <label>From Qty</label>
                                                                    <telerik:RadNumericTextBox Skin="Simple" CssClass ="inputSM"  runat="server" ID="txtFromQty"  Width="100%" TabIndex ="9" IncrementSettings-InterceptMouseWheel="false"
                                                                                        IncrementSettings-InterceptArrowKeys="false" NumberFormat-GroupSeparator=""
                                                                      MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                                                    </telerik:RadNumericTextBox>
                                                                    <asp:RequiredFieldValidator runat ="server" ID="rfv1" ControlToValidate ="txtFromQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <div class="">
                                                                  <label>To Qty</label>
                                                                  <telerik:RadNumericTextBox Skin="Simple" CssClass ="inputSM"  runat="server" ID="txtToQty" Width="100%"
                                                                                                         TabIndex ="10" IncrementSettings-InterceptMouseWheel="false"
                                                                                                        NumberFormat-GroupSeparator=""
                                                                                                                IncrementSettings-InterceptArrowKeys="false"
                                                                                                MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                                                                            </telerik:RadNumericTextBox>
                                                                                             <asp:RequiredFieldValidator runat ="server" ID="rfv2" ControlToValidate ="txtToQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                                                              <asp:CompareValidator ID="QtyCompareValidator" runat="server" ControlToValidate="txtToQty"
                                                                        ControlToCompare="txtFromQty" Operator="GreaterThan"    Type="Double"
                                                                        ErrorMessage="To qty > From qty">
                                                                    </asp:CompareValidator>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <div class="form-group">
                                                                  <label>Type</label>
                                                                  <telerik:RadComboBox Skin="Simple"  CssClass ="inputSM" ID="ddlType"  Width="100%" runat="server" TabIndex ="11" ></telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <div class="">
                                                                  <label>Get Qty</label>
                                                                <telerik:RadNumericTextBox Skin="Simple" runat="server" ID="txtGetQty"   Width="100%" TabIndex ="12"
                                                         IncrementSettings-InterceptMouseWheel="false" NumberFormat-GroupSeparator=""
                                                                IncrementSettings-InterceptArrowKeys="false"
                                              MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>
                                             <asp:RequiredFieldValidator runat ="server" ID="rfv3" ControlToValidate ="txtGetQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-3">
                                                            <div class="form-group">
                                                               <asp:HiddenField ID="hfRow" runat="server" />
                                                               <asp:HiddenField ID="hfDeletedSlabs" runat="server" />
                                                               <asp:Button ID="btnAddSlab" runat="server" TabIndex ="13" Text="Add" CssClass="btn btn-success" ValidationGroup ="valslab" />
                                                               <asp:HiddenField ID="hfSlabID" runat ="server" />
                                                               <asp:HiddenField ID="hfOldFrom" runat ="server" />
                                                               <asp:HiddenField ID="hfOldTo" runat ="server" />
                                                                <asp:Button ID="btnCanSlab" runat="server" TabIndex ="15" Text="Cancel" CssClass ="btn btn-default"  />
                                                            </div>
                                                        </div>
                                                    </div>
                                            
                                             <div class="table-responsive">
                                                 <asp:GridView Width="100%" ID="dgvSlabs" runat="server" EmptyDataText="No items to display"
                                                        EmptyDataRowStyle-Font-Bold="true"  AutoGenerateColumns="False" DataKeyNames ="SlabId"
                                                        AllowPaging="false" AllowSorting="false"  PageSize="25" CellPadding="0" CellSpacing="0" CssClass="tablecellalign">
                                                 
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                                        <EmptyDataRowStyle Font-Bold="True" />
                                                        <Columns>
                                                     
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left"  Visible ="false"  HeaderStyle-Wrap="false" DataField="SlabId"
                                                                HeaderText="SlabId">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                           
                                                         
                                                            <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="FromQty"
                                                                HeaderText="From Qty"  DataFormatString="{0:F0}">
                                                                   <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="ToQty"
                                                                HeaderText="To Qty"  DataFormatString="{0:F0}">
                                                                 <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                               <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="TypeCode"
                                                                HeaderText="Type">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                              <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="GetQty"
                                                                HeaderText="Bonus Qty"  DataFormatString="{0:F0}">
                                                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                         
                                                             
                                                         
                                                             
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Edit" ID="btnEdit"  Visible ="false"  runat="server"  CommandName="EditSlab"
                                                                        CausesValidation="false"   ImageUrl="~/images/edit-13.png"   />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ToolTip="Delete" ID="btnCan" runat="server" CommandName="DeleteSlab"
                                                                        CausesValidation="false"  ImageUrl="~/images/delete-13.png" OnClientClick="javascript:return confirm('Would you like to delete the selected rule?');" />
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
                                            

                                         <div>
                                             <br />
                                            <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-primary" OnClientClick="javascript:return confirm('Would you like to confirm this?');" Text="Confirm" />     
                                        </div>
                                        </asp:Panel> 
                                          
                                                      
                                                    
                                                   <telerik:RadWindow ID="MPInfo" Title= "Information" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                          Width="500" Height="450"    AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="false">
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
                
                                
                              
               <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPnl" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img  src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
              
           
</asp:Content>
