<%@ Page Title="Assortment Plan Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
    CodeBehind="AssortmentDefinitionMinQty.aspx.vb" Inherits="SalesWorx_BO.AssortmentDefinitionMinQty" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        .rcTimePopup
        {
            display: none !important;
        }

        .RadListBox_Default .rlbText, .RadListBox_Default .rlbItem, .RadListBox_Default .rlbButtonText, .RadListBox_Default .rlbEmptyMessage
        {
            font: 13px "Calibri";
            color: black;
        }

        .RadComboBox_Default .rcbInput, .RadComboBoxDropDown_Default
        {
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
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <div class="">
    </div>
    <asp:UpdatePanel ID="TopPnl" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                                  
                                  
                                                  
                                          
                           
                                
                                        <asp:Panel ID="Panel1" runat="server" GroupingText ="">

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
                                                        <div class="col-sm-6">
                                                            <div class="">
                                                                  <label>Min.Qty</label>
                                                                    <telerik:RadNumericTextBox Skin="Simple" CssClass ="inputSM"  runat="server" ID="txtFromQty"  Width="40%" TabIndex ="9" IncrementSettings-InterceptMouseWheel="false"
                                                                                        IncrementSettings-InterceptArrowKeys="false" NumberFormat-GroupSeparator=""
                                                                      MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                                                    </telerik:RadNumericTextBox>
                                                                    <asp:RequiredFieldValidator runat ="server" ID="rfv1" ControlToValidate ="txtFromQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                                
                                                            <asp:Button ID="btnOrdAdd" CssClass="btn btn-success" runat ="server" Text ="Add" />
                                                                <asp:Button ID="btnOrdRemove"  CssClass ="btn btn-danger"  runat ="server" Text ="Remove" /> </div>
                                                        </div>
                                                        <div class="col-sm-3" style ="display:none;">
                                                            <div class="">
                                                                  <label>Max.Qty</label>
                                                                  <telerik:RadNumericTextBox Skin="Simple" CssClass ="inputSM"  runat="server" ID="txtToQty" Width="100%"
                                                                                                         TabIndex ="10" IncrementSettings-InterceptMouseWheel="false"
                                                                                                        NumberFormat-GroupSeparator=""
                                                                                                                IncrementSettings-InterceptArrowKeys="false"
                                                                                                MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                                                                            </telerik:RadNumericTextBox>
                                                                                         <%--    <asp:RequiredFieldValidator runat ="server" ID="rfv2" ControlToValidate ="txtToQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                                                              <asp:CompareValidator ID="QtyCompareValidator" runat="server" ControlToValidate="txtToQty"
                                                                        ControlToCompare="txtFromQty" Operator="GreaterThan"    Type="Double"
                                                                        ErrorMessage="To qty > From qty">
                                                                    </asp:CompareValidator>--%>  
                                                            </div>
                                                        </div>
                                                        </div> 
                                                    
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                             <%--   <telerik:RadListBox ID="lstOrderd"  CssClass ="inputSM"    
                                                                        tooltip="Press CTRL key for multi select item"
                                                                        SelectionMode ="Multiple" Width="100%" AutoPostBack ="true" 
                                                                        runat="server" CheckBoxes="true" ShowCheckAll="true" Height="250px">
                                                                </telerik:RadListBox>--%>
                                                                  <telerik:RadGrid ID="rgOrdered" Skin="Simple"
                        AllowSorting="true" autogeneratedcolumns="false" AutoGenerateColumns="False"
                        PageSize="8" AllowPaging="false" runat="server" AllowFilteringByColumn="false"
                        GridLines="None">
                        <GroupingSettings CaseSensitive="false" />
                         <ClientSettings EnableRowHoverStyle="false"  EnableAlternatingItems="false"   >
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                        </ClientSettings>
                        <MasterTableView Summary="RadGrid table"  AllowFilteringByColumn="false" AllowPaging="false" PageSize="8" TableLayout ="Auto"  >
                           
                            <NoRecordsTemplate>
                                <div>
                                    There are no records to display
                                </div>
                            </NoRecordsTemplate>

                            <Columns>
                                <telerik:GridTemplateColumn AllowFiltering="false" Visible ="false"  HeaderText ="Is Mandatory" AutoPostBackOnFilter="false" >
                                    <HeaderStyle  HorizontalAlign="Justify" />
                                    <ItemStyle  HorizontalAlign="Justify" />
                                  
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Bind("IsMandatory")%>' AutoPostBack ="true" OnCheckedChanged="chkIgnore_CheckedChanged" ID="checkbox2" />

                                        <asp:Label ID="lblOItemCode" Visible="false"
                                            runat="server" Text='<%# Bind("ItemCode")%>'></asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn UniqueName="Description" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Description" DataField="Description">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" Width="150px" />
                                </telerik:GridBoundColumn>
                               
                                   <telerik:GridBoundColumn UniqueName="Qty" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Min.Order Qty" DataField="Qty" DataFormatString ="{0:N0}">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" Width="150px" />
                                </telerik:GridBoundColumn>

                                 <telerik:GridBoundColumn UniqueName="MaxQty" Visible ="false"  DataFormatString ="{0:N0}" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Max.Order Qty" DataField="MaxQty">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" Width="150px" />
                                </telerik:GridBoundColumn>


                            

                            </Columns>




                        </MasterTableView>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true"></PagerStyle>
                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                    </telerik:RadGrid>
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
                                                                
                                                               <asp:Button ID="btnCopy" Visible ="false"   CssClass ="btn btn-primary" runat ="server" Text ="Copy Ordered &rArr; Bonus" />
                                                                   <asp:Button ID="btnGoBack"  CssClass ="btn btn-default"  runat ="server" Text ="Go Back" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                              <%--  <telerik:RadListBox ID="lstGet"   CssClass ="inputSM"   SelectionMode ="Multiple"  
                                                                        runat="server"  Width="100%"   tooltip="Press CTRL key for multi select item" Height="250px">
                                                                </telerik:RadListBox>--%>
                                                                    <telerik:RadGrid ID="rgGet" Skin="Simple"
                        AllowSorting="true" autogeneratedcolumns="false" AutoGenerateColumns="False"
                        PageSize="8" AllowPaging="false" runat="server" AllowFilteringByColumn="false"
                        GridLines="None">
                        <GroupingSettings CaseSensitive="false" />
                         <ClientSettings EnableRowHoverStyle="false"  EnableAlternatingItems="false"   >
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" />
                                        </ClientSettings>
                        <MasterTableView Summary="RadGrid table"  AllowFilteringByColumn="false" AllowPaging="false" PageSize="8" >
                           
                            <NoRecordsTemplate>
                                <div>
                                    There are no records to display
                                </div>
                            </NoRecordsTemplate>

                            <Columns>
                                <telerik:GridTemplateColumn AllowFiltering="false" Visible ="false"  HeaderText ="" AutoPostBackOnFilter="false" HeaderStyle-Width="20px">
                                    <HeaderStyle Width="20px" HorizontalAlign="Justify" />
                                    <ItemStyle Width="20px" HorizontalAlign="Justify" />
                                  
                                    <ItemTemplate>
                                       

                                        <asp:Label ID="lblBItemCode" Visible="false"
                                            runat="server" Text='<%# Bind("ItemCode")%>'></asp:Label>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn UniqueName="Description" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Description" DataField="Description">
                                    <ItemStyle Wrap="false" />
                                    <HeaderStyle Wrap="false" Width="150px" />
                                </telerik:GridBoundColumn>
                              

                            

                            </Columns>




                        </MasterTableView>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true"></PagerStyle>
                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                    </telerik:RadGrid>
                                                      
                                                               
                                                            </div>
                                                                   
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                            
                                            

                                          <br />
                                                                 <div class="row">
                                                       <div class="col-sm-3">
                                                            <div class="form-group">
                                                                  <label>Type</label>
                                                                  <telerik:RadComboBox Skin="Simple"  CssClass ="inputSM" ID="ddlType"  Width="100%" runat="server" TabIndex ="11" ></telerik:RadComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="">
                                                                  <label>Get Qty</label>
                                                                <telerik:RadNumericTextBox Skin="Simple" runat="server" ID="txtGetQty"   Width="40%" TabIndex ="12"
                                                         IncrementSettings-InterceptMouseWheel="false" NumberFormat-GroupSeparator=""
                                                                IncrementSettings-InterceptArrowKeys="false"
                                              MinValue="0" autocomplete="off" NumberFormat-DecimalDigits="0">
                                            </telerik:RadNumericTextBox>
                                             <asp:RequiredFieldValidator runat ="server" ID="rfv3" ControlToValidate ="txtGetQty" ErrorMessage ="*" ValidationGroup ="valslab"></asp:RequiredFieldValidator>
                                                                   <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-primary" OnClientClick="javascript:return confirm('Would you like to confirm this?');" Text="Confirm" />     
                                                            </div>
                                                        </div>
                                                       </div> 
                                        </asp:Panel> 
                                          
                                                      
                                                    
                                                    
                                                    
                                             
                                              
                                </contenttemplate>
    </asp:UpdatePanel>



    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="TopPnl" runat="server">
        <progresstemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img  src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>
                    </progresstemplate>
    </asp:UpdateProgress>


</asp:Content>
