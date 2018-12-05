<%@ Page Title="Off Take Summary Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_monthlyOffTakeSummary.aspx.vb" Inherits="SalesWorx_BO.Rep_monthlyOffTakeSummary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
         input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }

        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

     
    
    </style>
    <script>
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
        function alertCallBackFn(arg) {

        }

        
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Off Take Summary Report</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
                              
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4 col-md-5  col-lg-5">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4 col-md-4  col-lg-5">
                                            <div class="form-group">
                                                <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                                   <telerik:RadComboBox Skin="Simple"    ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                                                                
                                                   
                                                     <div class="col-sm-4 col-md-3  col-lg-2">
                                                       <div class="form-group">
                                                           <label>Month </label>
                                                         <telerik:RadMonthYearPicker RenderMode="Lightweight" Width ="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>     

                                                        </div>
                                                      </div>
                                              
                       

                                                    </div>
                                                <div class="row"> 
                                                    <div class="col-sm-4 col-md-5  col-lg-5">

                                            <div class="form-group">
                                                <label>Item</label>
                                                
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
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
              <p><strong>Month: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p> 
               <p><strong>Item: </strong> <asp:Label ID="lbl_item" runat="server" Text=""></asp:Label></p>              
            </span>
            </i>      
        </div>
    </div>

            

             
                              <div class="table-responsive">
                                   <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                       <Columns>
                                                          
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item" HeaderText="Item Description" SortExpression ="Item"
                                                               >
                                                                <ItemStyle Wrap="true" />
                                                            </telerik:GridBoundColumn>                                       
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Requ_Quantity" HeaderText="Requisition Qty" SortExpression ="Requ_Quantity"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SO_Quantity" HeaderText="Bulk Order Qty" SortExpression ="SO_Quantity"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                                 <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SoldQty" HeaderText="Sales Quantity" SortExpression ="SoldQty"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesValue" HeaderText="Sales Value" SortExpression ="SalesValue"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ResellQuantity" HeaderText="Resellable Qty" SortExpression ="ResellQuantity"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>


                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ResellValue" HeaderText="Resellable Value" SortExpression ="ResellValue"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="DamgedQuantity" HeaderText="Damage Quantity" SortExpression ="DamgedQuantity"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="DamgedValue" HeaderText="Damage Value" SortExpression ="DamgedValue"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExpiredQty" HeaderText="Expired Quantity" SortExpression ="ExpiredQty"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExpiredValue" HeaderText="Expired Value" SortExpression ="ExpiredValue"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="StandardDVal" HeaderText="Standard <br/>Damage Value" SortExpression ="StandardDVal"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Variance" HeaderText="Variance" SortExpression ="Variance"
                                                              DataFormatString="{0:#,##0.00}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                                  </div>
                              <asp:HiddenField ID="HCurrency" runat="server" />          
 <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                        
                                 
          
 </ContentTemplate> </asp:UpdatePanel> 
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
   <asp:UpdateProgress ID="UpdateProgress2"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       

	
</asp:Content>

