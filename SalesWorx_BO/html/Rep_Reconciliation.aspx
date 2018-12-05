<%@ Page Title="Reconciliation Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_Reconciliation.aspx.vb" Inherits="SalesWorx_BO.Rep_Reconciliation" %>
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
 .RadGrid .rgMasterTable thead tr th {
    position: relative;
}
 .RadGrid .rgMasterTable thead tr th.BO a {
    color: #32b18e !important;
}
  .RadGrid .rgMasterTable thead tr th.ERP a {
    color: #dc9f51 !important;
}
 .RadGrid .rgMasterTable thead tr th.BO:before {
    content: "";
    height: 10px;
    background: #7bd8be;
    position: absolute;
    top: -12px;
    width: 104%;
    left: -2px;
}
 .RadGrid .rgMasterTable thead tr th.ERP:before {
    content: "";
    height: 10px;
    background: #ffbd6b;
    position: absolute;
    top: -12px;
    width: 104%;
    left: -2px;
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

            $("#MainContent_BtnExportExcel").click()
            return false

        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Reconciliation</h4>
	
 

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
                                                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>

           <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                  </div>
                                          </div>
          
                                                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label> Van<em><span>&nbsp;</span>*</em></label>
             <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                </div>
                                                         </div>
          <div class="col-sm-6">
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
                                                 <div class="col-sm-6">
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
                                                    <div style="display:none">
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
  
                                                </div>
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
            
            </span>
            </i>      
        </div>
    </div>
    <div class="overflowx" style="padding-top:10px;">
                                   
                                     
                                 <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                   <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Ref_No"  HeaderText="Document No"
                                                                  SortExpression ="Doc_Ref_No" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Type" HeaderText="Document Type"
                                                                  SortExpression ="Doc_Type" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Date" HeaderText="Doc Date"
                                                                  SortExpression ="Doc_Date"  HeaderStyle-CssClass="BO" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer No."
                                                                  SortExpression ="Customer_No"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Number"  HeaderText="Van Id"
                                                                  SortExpression ="SalesRep_Number"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Salesperson Name"
                                                                  SortExpression ="Emp_Name"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_Org" HeaderText="Sales Org"
                                                                  SortExpression ="Sales_Org" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_date" HeaderText="Creation Date"
                                                                  SortExpression ="Creation_date" HeaderStyle-CssClass="BO" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code"  HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Line_No" HeaderText="Line No"
                                                                  SortExpression ="Line_No" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Qty" HeaderText="Qty"
                                                                  SortExpression ="Qty" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Unit_Price" HeaderText="Unit Price"
                                                                  SortExpression ="Unit_Price" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="List_Price" HeaderText="List Price"
                                                                  SortExpression ="List_Price" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Lot_Number"  HeaderText="Lot Number"
                                                                  SortExpression ="Lot_Number" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExpiryDate"  HeaderText="Expiry Date"
                                                                  SortExpression ="ExpiryDate" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Gross" HeaderText="Gross Amt"
                                                                  SortExpression ="Gross" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Discount_Value" HeaderText="Disc %"
                                                                  SortExpression ="Discount_Value" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Discount_Amount" HeaderText="Discount Amt"
                                                                  SortExpression ="Discount_Amount" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Net" HeaderText="Net Amt"
                                                                  SortExpression ="Net" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="VAT_Amount" HeaderText="VAT Amt"
                                                                  SortExpression ="VAT_Amount" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Total" HeaderText="Total Amt"
                                                                  SortExpression ="Total" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>



                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Ref_No_ERP"  HeaderText="Document No"
                                                                  SortExpression ="Doc_Ref_No_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Type_ERP" HeaderText="Document Type"
                                                                  SortExpression ="Doc_Type_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Date_ERP" HeaderText="Doc Date"
                                                                  SortExpression ="Doc_Date_ERP" HeaderStyle-CssClass="ERP" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No_ERP" HeaderText="Customer No."
                                                                  SortExpression ="Customer_No_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Number_ERP"  HeaderText="Van Id"
                                                                  SortExpression ="SalesRep_Number_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name_ERP" HeaderText="Salesperson Name"
                                                                  SortExpression ="Emp_Name_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_Org_ERP" HeaderText="Sales Org"
                                                                  SortExpression ="Sales_Org_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_Date_ERP" HeaderText="Creation Date"
                                                                  SortExpression ="Creation_Date_ERP" HeaderStyle-CssClass="ERP" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code_ERP"  HeaderText="Item Code"
                                                                  SortExpression ="Item_Code_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Line_No_ERP" HeaderText="Line No"
                                                                  SortExpression ="Line_No_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Qty_ERP" HeaderText="Qty"
                                                                  SortExpression ="Qty_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Unit_Price_ERP" HeaderText="Unit Price"
                                                                  SortExpression ="Unit_Price_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="List_Price_ERP" HeaderText="List Price"
                                                                  SortExpression ="List_Price_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Lot_Number_ERP"  HeaderText="Lot Number"
                                                                  SortExpression ="Lot_Number_ERP" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExpiryDate_Erp"  HeaderText="Expiry Date"
                                                                  SortExpression ="ExpiryDate_Erp" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False" CssClass="ERP" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Gross_ERP" HeaderText="Gross Amt"
                                                                  SortExpression ="Gross_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Discount_Value_ERP" HeaderText="Disc %"
                                                                  SortExpression ="Discount_Value_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Discount_Amount_ERP" HeaderText="Discount Amt"
                                                                  SortExpression ="Discount_Amount_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Net_ERP" HeaderText="Net Amt"
                                                                  SortExpression ="Net_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Vat_Amount_ERP" HeaderText="VAT Amt"
                                                                  SortExpression ="Vat_Amount_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Total_ERP" HeaderText="Total Amt"
                                                                  SortExpression ="Total_ERP" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="ERP">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="ERP"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Diff" HeaderText="Difference"
                                                                  SortExpression ="Diff" DataType="System.Double" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <%--<GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Product" FieldName="Product"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="Product" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>--%>
                                                          
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
        </div>
         <br />
       <div class="overflowx" style="padding-top:10px;">
      
             <telerik:RadGrid id="gvRep_Stock" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                   <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="StockTransfer_ID"  HeaderText="Doc ID"
                                                                  SortExpression ="StockTransfer_ID" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Type" HeaderText="Document Type"
                                                                  SortExpression ="Doc_Type" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Doc_Date" HeaderText="Doc Date"
                                                                  SortExpression ="Doc_Date"  HeaderStyle-CssClass="BO" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="IMSRef"  HeaderText="IMS Reference"
                                                                  SortExpression ="IMSRef"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Number"  HeaderText="Van Id"
                                                                  SortExpression ="SalesRep_Number"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Salesperson Name"
                                                                  SortExpression ="Emp_Name"  HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_Org" HeaderText="Sales Org"
                                                                  SortExpression ="Sales_Org" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_date" HeaderText="Creation Date"
                                                                  SortExpression ="Creation_date" HeaderStyle-CssClass="BO" DataFormatString="{0:dd-MMM-yyyy HH:mm}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Center" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code"  HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Line_No" HeaderText="Line No"
                                                                  SortExpression ="Line_No" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Qty" HeaderText="Qty"
                                                                  SortExpression ="Qty" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Unit_Price" HeaderText="Unit Price"
                                                                  SortExpression ="Unit_Price" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="List_Price" HeaderText="List Price"
                                                                  SortExpression ="List_Price" DataType="System.Double" DataFormatString="{0:N2}" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" CssClass="BO"/>
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Lot_Number"  HeaderText="Lot Number"
                                                                  SortExpression ="Lot_Number" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExpiryDate"  HeaderText="Expiry Date"
                                                                  SortExpression ="ExpiryDate" HeaderStyle-CssClass="BO">
                                                                <ItemStyle Wrap="False" CssClass="BO" />
                                                            </telerik:GridBoundColumn>
                                                              

                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                </MasterTableView>
                 </telerik:RadGrid>
        </div>
        </telerik:RadAjaxPanel>
  <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
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