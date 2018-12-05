<%@ Page Title="Salesman - Outletwise Sales/Return" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepSalesmanOutletwiseSaleReturn.aspx.vb" Inherits="SalesWorx_BO.RepSalesmanOutletwiseSaleReturn" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


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
    <h4>Salesman - Outletwise Sales/Return</h4>
	
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
 
  <asp:UpdatePanel ID="Panel" runat="server" >
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
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
               <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van </label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                     </telerik:RadComboBox >
                     
            </div>
                   </div>
                                                    <div class="col-sm-4"  >
                                                          <div class="form-group"> <label>Outlet   </label>
                 <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Customer"  ID="ddlCustomer" Width ="100%" runat="server" DataTextField="Outlet" DataValueField="CustomerID" Filter="Contains">
                                                     </telerik:RadComboBox >
                     
              </div>             
                                                  </div>
                <div class="col-sm-4"  >
                                                          <div class="form-group"> <label>From Date  </label>
             <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar> 
                                                    </telerik:RadDatePicker>
                                                         </div>
                                                      </div>
            <div class="col-sm-4"  >
                                                          <div class="form-group"> <label>To Date  </label>
                                                              <telerik:RadDatePicker ID="txtToDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar> 
                                                    </telerik:RadDatePicker></div>
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
             <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>Outlet: </strong><asp:Label ID="lbl_Cust" runat="server" Text=""></asp:Label></p>            
            </span>
            </i>      
        </div>
    </div>

              
            <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>
                                <div style="width:100%; overflow-x:auto; border:#ccc solid 1px;" id="reportblocker" runat="server" visible="false" >
                                           <rsweb:ReportViewer ID="RVMain" runat="server"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>   
                             </div>
                                 </ContentTemplate>
                                </asp:UpdatePanel>

 </ContentTemplate> </asp:UpdatePanel> 
  
   
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
</asp:Content>