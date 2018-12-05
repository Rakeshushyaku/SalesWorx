<%@ Page Title="Bonus Details Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepOfferDetails.aspx.vb" Inherits="SalesWorx_BO.RepOfferDetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }

        function clickSearch() {
            $("#MainContent_SearchBtn").click()
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
    </script>
    <style>
    
    .divtablebelow > table{
        border:#ccc solid 1px;
    }
    .divtablebelow table th{
        background:#eee;
    }
    .divtablebelow table th.text-center{
        background:#f9f5e4;
    }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>FOC Promotions</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>

           <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                                                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Type</label>
                                                   <telerik:RadComboBox  Skin="Simple"   Width ="100%"
                                                     ID="ddlMode" runat="server" AutoPostBack="true" >
                                                     <Items >
                                                     <telerik:RadComboBoxItem Text ="Simple" Value ="Simple" />
                                                     <telerik:RadComboBoxItem Text ="Assortment" Value ="Assortment" />
                                                     </Items>
                                                </telerik:RadComboBox>
                                                 </div>
                                             </div>
                                              
                                               <div class="col-sm-4"> 
                                                    <div class="form-group">
                                                        <label>Brand</label>
                                                            
                                                            <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"  Width ="100%"
                                                                 CheckBoxes="true" EmptyMessage="Select Brand" ID="ddlBrand"  runat="server">
                                                              </telerik:RadComboBox>
                                                         </div>
                                                     </div>
                                                    
                                                    
                                               </div>
                                              
                                                  <div class="row">
                                                     <div class="col-sm-4">
                                                       <div class="form-group">
                                                           <label>From Date  </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"    runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>
                                                       <div class="col-sm-4">
                                                       <div class="form-group">
                                                           <label>To Date  </label>
                                                             <telerik:RadDatePicker ID="txtToDate"    runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>

                                                      <div class="col-sm-4">
                                                           <div class="form-group">
                                                                <label><asp:Label ID="lbl_Status" Text="Include Deactivated Items" runat="server" ></asp:Label></label>
                                                                       <telerik:RadComboBox  Skin="Simple" 
                                                                             ID="ddlStatus" Width="100%"  runat="server">
                                                                             <Items >
                                                                             <telerik:RadComboBoxItem Text ="No" Value ="N" />
                                                                             <telerik:RadComboBoxItem Text ="Yes" Value ="Y" />
                                                     
                                                                             </Items>
                                                                        </telerik:RadComboBox>
                                                           </div>
                                                      </div>

                                                  </div>


                                            </div>
                                                    
                                                    <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search" />
                                                         <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
                                            </div>
                                            </div>
                                        </div>

                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar>

     <div id="Args" runat="server" visible="false">
            <div id="popoverblkouter">
                Hover on icon to view search criteria <i class="fa fa-info-circle">
                <span class="popoverblk">
                  <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
                  <p><strong>Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
                  <p><strong>Brand: </strong> <asp:Label ID="lbl_Brand" runat="server" Text=""></asp:Label></p>
                  <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
                  <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
                  <p><strong>Include Deactivated Items: </strong><asp:Label ID="lbl_Include" runat="server" Text=""></asp:Label></p>  
                </span>
                </i>      
            </div>
    </div>

    

       <div class="overflowx">
               <telerik:RadGrid id="gvSimpleRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server"  Visible="false" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" 
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None"></PagerStyle>
                                                        <Columns>
                                                        <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Left" DataField="OBrand" HeaderText="Order Item Brand"
                                                                  SortExpression ="OBrand" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>                                                                                                    
                                                             
                                                      <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Left" DataField="OrderItemCode" HeaderText="Order Item Code"
                                                                  SortExpression ="OrderItemCode" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Left" DataField="OrderItemDesc" HeaderText="Order Item Desc"
                                                                  SortExpression ="OrderItemDesc" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn  DataFormatString="{0:N0}"  ItemStyle-HorizontalAlign="Center"   HeaderStyle-HorizontalAlign="Left" DataField="FromQty" HeaderText="From Qty"
                                                                  SortExpression ="FromQty" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                              <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                                                                             
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ToQty" HeaderText="To Qty" DataFormatString="{0:N0}"
                                                               ItemStyle-HorizontalAlign="Center"     SortExpression ="ToQty" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                               <HeaderStyle HorizontalAlign="Center" />
                                                             </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BonusType" HeaderText="Bonus Type"
                                                                  SortExpression ="BonusType" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BonusQty" HeaderText="Bonus Qty" DataFormatString="{0:N0}"
                                                                ItemStyle-HorizontalAlign="Center"   SortExpression ="BonusQty" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BBrand" HeaderText="Bonus Item Brand"
                                                                  SortExpression ="BBrand" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BonusItemCode" HeaderText="Bonus Item Code"
                                                                  SortExpression ="BBrand" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="BonuseDesc" HeaderText="Bonus Item Desc"
                                                                  SortExpression ="BonuseDesc" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>


                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                  SortExpression ="StartDate"  >
                                                                <ItemStyle Wrap="False"  />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                                                  SortExpression ="EndDate" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status"
                                                                  SortExpression ="Status" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="PlanName" HeaderText="Plan Name"
                                                                  SortExpression ="PlanName" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>

                                                                                 

                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           


                 <telerik:RadGrid id="gvAssort" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server"  Visible="false" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="false">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowHeader="false"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None"></PagerStyle>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                <ItemTemplate>
                                                                   <asp:HiddenField ID="hfID" runat="server"  Value='<%# Eval("PlanID")%>' />
                                                                    
                                                                      
                                                                                    <div id="divOrder" class="divtablebelow" runat="server" >
                                                                                         
                                                                                    </div>
                                                                               
                                                                                    <div id="divRules" class="divtablebelow" runat="server" >

                                                                                    </div>
                                                                                
                                                                                    <div id="divBonus" class="divtablebelow" runat="server" >

                                                                                    </div>
                                                                                
                                                                    
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>                                                                                          
                                                  
                                                           <telerik:GridBoundColumn UniqueName="StartDate" HeaderStyle-HorizontalAlign="Left" DataField="StartDate" HeaderText="StartDate"
                                                                  SortExpression ="StartDate" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                           <telerik:GridBoundColumn UniqueName="EndDate" HeaderStyle-HorizontalAlign="Left" DataField="EndDate" HeaderText="EndDate"
                                                                  SortExpression ="EndDate" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn UniqueName="Status" HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status"
                                                                  SortExpression ="Status" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center"  />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>

                                                        <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Plan" FieldName="PlanName"   SortOrder="Ascending"
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="PlanName"  >

                                                                     </telerik:GridGroupByField>
                                                                     <telerik:GridGroupByField FieldName="PlanID"  >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                     </GroupByExpressions>                       

                                                        </MasterTableView>
                                                    </telerik:RadGrid>
           

       </div>

    <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove" ShowBackButton="true"  Visible="false"
        ProcessingMode="Remote" Width="100%"
        SizeToReportContent="true" AsyncRendering="false" DocumentMapWidth="100%">
    </rsweb:ReportViewer>

            
      </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>




</asp:Content>
