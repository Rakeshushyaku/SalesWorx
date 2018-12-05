<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DailyProductSales.aspx.vb" Inherits="SalesWorx_BO.Rep_DailyProductSales" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
     <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

 div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   } 
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
    <script>

        function alertCallBackFn(arg) {

        }

        function clickSearch() {
            $("#MainContent_SearchBtn").click()
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportBiffExcel").click()
            
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Daily Product Sales</h4>
	
 

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

   <telerik:RadAjaxPanel runat="server" ID="g">
	
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
                                                <label> Van<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                </div>
                                                         </div>

                                                    <div class="col-sm-4">
                                            <div class="form-group">
                                                <label> Agency<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple" ID="ddl_Agency" EmptyMessage="Select Agency"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" AutoPostBack="true"    Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox >
                                                </div>
                                                         </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label> Category<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple" ID="ddl_Category" EmptyMessage="Select Category"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  AutoPostBack="true" Width ="100%" runat="server" DataTextField="Category" DataValueField="Category" Filter="Contains">
                                        </telerik:RadComboBox >
                                                </div>
                                                         </div>


          <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                        <label>To Date</label>
                                                          <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                  </div> 
           
                                                    </div>
                <div class="row"> 
                                                   
                                                   
                                                    <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Product</label>
                                                 
                                                
                                                <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="ddl_Product"  EmptyMessage="Please type Item No./Description"
                 InputType="Token" Width="100%"  MinFilterLength="2" DropDownWidth="200px"   >
            </telerik:RadAutoCompleteBox>
                                            </div>
                                          </div>        
                                           
                                             </div>
                                                 <div class="row"> 
                                                   
                                                   
                                                    <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                 
                                                
                                                <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="ddl_Customer"  EmptyMessage="Please type Customer No./Name"
                 InputType="Token" Width="100%"  MinFilterLength="2" DropDownWidth="200px"   >
            </telerik:RadAutoCompleteBox>
                                            </div>
                                          </div>        
                                           
                                             </div>
                                                                                </div>

                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                                    </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                 
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
              <p><strong>Agency: </strong><asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label></p>  
              <p><strong>Category: </strong><asp:Label ID="lbl_Category" runat="server" Text=""></asp:Label></p>  
              <p><strong>Product: </strong><asp:Label ID="lbl_Product" runat="server" Text=""></asp:Label></p>  
              <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>  

            </span>
            </i>      
        </div>
    </div>
    <div class="table-responsive">
                                   
                                     
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                                        <asp:HiddenField ID="hfDecimal" runat="server" />
                              <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true"    
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="ColumnsOnly"  />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="SDate" CellStyle-Width ="100px" ZoneIndex="0"  Caption="Date" UniqueName="Date" DataFormatString="{0:dd-MMM-yyyy}" >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Van" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField DataField="Qty" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Amount" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Invoices" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Customers" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>
                                    </telerik:RadAjaxPanel>
            </div>
        </telerik:RadAjaxPanel>
  <div style="display: none">
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
    </div>
   <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>

  
   
</asp:Content>