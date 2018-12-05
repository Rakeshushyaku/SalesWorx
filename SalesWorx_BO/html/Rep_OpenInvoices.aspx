<%@ Page Title="Open Invoices" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_OpenInvoices.aspx.vb" Inherits="SalesWorx_BO.Rep_OpenInvoices" %>
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
         .label-set input + label {
             display:inline-block;
         }


          .Dueclass
  {
    color:#DC143C;
   
  }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
    <script>

        function alertCallBackFn(arg) {

        }
        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

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

    <h4>Open Invoices</h4>
	
 

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

                                                    

          <div class="col-sm-3">
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
                                                 <div class="col-sm-3">
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

                                                    <div class="col-sm-6">
                                                      <div class="form-group label-set">
                                                        <label>&nbsp;</label>
                                                          <asp:CheckBox ID="chkActive" runat ="server" Text ="Show Over Dues"  />
                                                       </div>
                                                    </div>
                                                    
                
                                                   
                                                   
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <label>Customer</label>
                                                 
                                                
                                                            <telerik:RadAutoCompleteBox RenderMode="Lightweight" runat="server" ID="ddl_Product"  EmptyMessage="Please type Customer No./Name"
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
                                                    <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                </div>
                                            </div>
                                       
                                    
                        
                                        
                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar><div id="Args" runat="server" visible="false">
           <div id="popoverblkouter">
               Hover on icon to view search criteria <i class="fa fa-info-circle">
               <span class="popoverblk">
               <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
               <p><strong>Van: </strong>&nbsp;<asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
               <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
               <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p> 
               <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p> 
               <p><strong><asp:Label ID="lbl_Overdue" runat="server" Text=""></asp:Label></strong></p>
 </span>
               </i>
           </div>
 </div>
       <div class="table-responsive">
           <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12"  CommandItemDisplay="Top">
                                                         <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>

                                                        <Columns>
                                                                                                                        
                                                             
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Invoice_Ref_No" HeaderText="Invoice Ref No"
                                                                  SortExpression ="Invoice_Ref_No" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer"
                                                                  SortExpression ="Customer_No" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="left" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Pending_Amount" HeaderText="Pending Amount" uniqueName="Pending_Amount"
                                                                  SortExpression ="Pending_Amount" DataType="System.Decimal" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Invoice_Date"  HeaderText="Invoice Date"
                                                                  SortExpression ="Invoice_Date"  DataFormatString="{0:dd-MMM-yyyy}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Due_Date"  HeaderText="Due Date"
                                                                  SortExpression ="Due_Date"  DataFormatString="{0:dd-MMM-yyyy}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Invoice_Amount" HeaderText="Invoice Amount" uniqueName="Invoice_Amount"
                                                                  SortExpression ="Invoice_Amount" DataType="System.Decimal" DataFormatString="{0:N2}" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"/>
                                                            </telerik:GridBoundColumn>



                                                            

                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                              <%--  <GroupByExpressions>

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
                                                         
                                                    </GroupByExpressions>
                                                          --%>
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
       </div>
 </telerik:RadAjaxPanel>
  <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
       <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
          <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
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