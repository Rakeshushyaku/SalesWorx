<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_StockMovement.aspx.vb" Inherits="SalesWorx_BO.Rep_StockMovement" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }
        function clickExportBiffExcel() {

            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
        function OpenViewWindowOpening_S(SPID, InvID) {
        }
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }
        .rgMasterTable > thead > tr > th, .rgMasterTable > tbody > tr > td
        {
            vertical-align: middle !important;
        }
        .label-width
        {
            width:150px;
            display:block;
        }
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
 
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Stock Movement Report</h4>	
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
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
	
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            	 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
              ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                    <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
            <td> <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                  </div>
                                          </div>
                                                    <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van<em><span>&nbsp;</span>*</em></label>
                                                  <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Van"  EmptyMessage="Select Van" 
                    runat="server" DataTextField="SalesRep_Name"  Width ="100%" DataValueField="SalesRep_ID">
                </telerik:RadComboBox>
                                            </div>
                                          </div>

                                                  <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Item</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
                                            </div>
                                          </div>  
                                                    
                                                    <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate" Width="100%"  runat="server">
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

                                                       <telerik:RadDatePicker ID="txtToDate" Width="100%"  runat="server">
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
              <p><strong>Van: </strong><asp:Label ID="lbl_Van" runat="server" Text=""></asp:Label></p>
              <p><strong>Product: </strong> <asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>  
              <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
             
            </span>
            </i>      
        </div>
    </div>
           
              <div class="overflowx">
            <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="6" AllowPaging="True" runat="server"
                GridLines="None" GroupingSettings-GroupContinuesFormatString="">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                 
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="6">

                   
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Product" SortExpression="Description" >
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                   <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TDate" HeaderText="Date" SortExpression="TDate"
                            DataFormatString="{0:dd-MMM-yyyy}">
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                  
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Type" HeaderText="Type" SortExpression="Type" DataFormatString="{0:#,###.0000}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qtyin" HeaderText="Quantity In" SortExpression="Qtyin"  DataFormatString="{0:#,###.0000}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qtyout" HeaderText="Quantity Out" SortExpression="Qtyout"  DataFormatString="{0:#,###.0000}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                    
                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TotalStk" HeaderText="Stock" SortExpression="TotalStk"  DataFormatString="{0:#,###.0000}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                    
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="UP" HeaderText="Unit Price" SortExpression="UP"  DataFormatString="{0:#,###.00}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Stockvalue" HeaderText="Total" SortExpression="Stockvalue"  DataFormatString="{0:#,###.0000}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="totvalue" HeaderText="Stock value" SortExpression="totvalue"  DataFormatString="{0:#,###.00}">
                           
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                    </Columns>
                    
                </MasterTableView>
            </telerik:RadGrid>
            </div>

            </contenttemplate>
         </asp:UpdatePanel>
     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
        
   </div>
  
     <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
                 
         
    </ProgressTemplate>
            </asp:UpdateProgress>    
    </asp:Content>