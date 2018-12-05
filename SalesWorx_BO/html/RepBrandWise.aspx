<%@ Page Title="Brand Wise Sales Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepBrandWise.aspx.vb" Inherits="SalesWorx_BO.RepBrandWise" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Brand Wise Sales Report</h4>
	
  <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	
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
                                                <label>Organization</label>
            <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR  </label>
            
          
                 <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                    CheckBoxes="true" EmptyMessage="Select a Van/FSR" ID="ddlVan" Width="100%" runat="server">
                                                </telerik:RadComboBox>
                                                    </div>
                                                         </div>
                
                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Brand  </label>

                                                 <telerik:RadComboBox Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"
                                                    CheckBoxes="true" EmptyMessage="Select a Brand" ID="ddlBrand" Width="100%" runat="server">
                                                </telerik:RadComboBox>
                                                </div>
                                                        </div>


                    <div class="col-sm-3">
                                                       <div class="form-group">
                                                           <label>From Date  </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>
                                                    <div class="col-sm-3">
                                                       <div class="form-group">
                                                           <label>To Date </label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
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
                                                <label>Show By</label>
          
                 <telerik:RadComboBox  Skin="Simple" 
                                                     ID="ddlMode" Width="180px" runat="server">
                                                     <Items >
                                                     <telerik:RadComboBoxItem Text ="Summary" Value ="Summary" />
                                                     <telerik:RadComboBoxItem Text ="Details" Value ="Details" />
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>Brand: </strong> <asp:Label ID="lbl_Brand" runat="server" Text=""></asp:Label></p>          
            </span>
            </i>      
        </div>
    </div>

             <div id="summary" runat="server" class="row"></div> 
             <div class="row">
             <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
                        <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                <asp:HiddenField runat="server" ID="hfDigit" Value="N2" />
</div>

                 </div>
         

                            
           
                              <div class="table-responsive">
                
                                              <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>

                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" Visible="false" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CreationDate" HeaderText="Date" SortExpression ="CreationDate"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                         <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Area" HeaderText="Area"
                                                                  SortExpression ="Area" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="SalesRepName" HeaderText="SalesRep"
                                                                  SortExpression ="SalesRepName" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                               <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="StockUOM" HeaderText="UOM"
                                                                  SortExpression ="StockUOM" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="CreditCustomer" HeaderText="Credit Customer"
                                                                  SortExpression ="CreditCustomer" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="CCCustomer" HeaderText="Cash Customer"
                                                                  SortExpression ="CCCustomer" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Brand" HeaderText="Brand"
                                                                  SortExpression ="Brand" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                        <telerik:GridBoundColumn Visible="true"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataField="Qty" HeaderText="Qty"
                                                                  SortExpression ="Qty"  DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                         
                                           
                                                          <telerik:GridBoundColumn Visible="true"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataField="InvValue" HeaderText="Value"
                                                                  SortExpression ="InvValue"  DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                              
                                                          
                                                        </Columns>

                                                  
                                                        
                                                        </MasterTableView>
                                                    </telerik:RadGrid>




                                  <telerik:RadGrid id="gvRep_Detailed" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" Visible="false" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CreationDate" HeaderText="Date" SortExpression ="CreationDate"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                         <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Area" HeaderText="Area"
                                                                  SortExpression ="Area" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="SalesRepName" HeaderText="SalesRep"
                                                                  SortExpression ="SalesRepName" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                               <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="True" />
                                                         </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="StockUOM" HeaderText="UOM"
                                                                  SortExpression ="StockUOM" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="CreditCustomer" HeaderText="Credit Customer"
                                                                  SortExpression ="CreditCustomer" >
                                                                <ItemStyle Wrap="True" />
                                                         </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="CCCustomer" HeaderText="Cash Customer"
                                                                  SortExpression ="CCCustomer" >
                                                                <ItemStyle Wrap="True" />
                                                         </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Brand" HeaderText="Brand"
                                                                  SortExpression ="Brand" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="ItemCode" HeaderText="Item Code"
                                                                  SortExpression ="ItemCode" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>


                                                             <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="True" />
                                                         </telerik:GridBoundColumn>


                                                        <telerik:GridBoundColumn Visible="true"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataField="Qty" HeaderText="Qty"
                                                                  SortExpression ="Qty"  DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                         
                                           
                                                          <telerik:GridBoundColumn Visible="true"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" DataField="InvValue" HeaderText="Value"
                                                                  SortExpression ="InvValue"  DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>
                                                              
                                                          
                                                        </Columns>

                                                  
                                                        
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                 </ContentTemplate>
                                </asp:UpdatePanel>
                           

                              </div>
        
 <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

 </ContentTemplate> </asp:UpdatePanel> 
  
     <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>
