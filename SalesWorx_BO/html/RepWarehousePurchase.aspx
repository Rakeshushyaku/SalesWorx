﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepWarehousePurchase.aspx.vb" Inherits="SalesWorx_BO.RepWarehousePurchase" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
      input[type="text"].rdfd_
{
    margin:0 !important;
    padding:0 !important;
    height:0 !important;
    width:0 !important;
}
div[id*="ReportDiv"] {  
    overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

#MainContent_gvRep{
    margin:15px 0;
}

    </style>
     <script>

 

         function alertCallBackFn(arg) {

         }

         function clickSearch() {
             $("#MainContent_SearchBtn").click()
             return false;
         }
         function clickExportExcel() {
             $("#MainContent_BtnExportExcel").click()
             return false

         }
         function clickExportPDF() {
             $("#MainContent_BtnExportPDF").click()
             return false
         }


         function OpenViewWindow(cid,dt) {
            
             var URL
             URL = 'RepDetails.aspx?Type=WareHousePurchase&ReportName=WareHousePurchase_Dtl&ID=' + cid + '&Date=' + dt
             var oWnd = radopen(URL, null);
             oWnd.SetSize(800, 600);
             oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)

             return false

         }

         document.onkeydown = function (evt) {

             evt = evt || window.event;
             if (evt.keyCode == 27) {

                 HideRadWindow();
             }
         };

         function HideRadWindow() {

             var elem = $('a[class=rwCloseButton');

             if (elem != null && elem != undefined) {
                 $('a[class=rwCloseButton')[0].click();
             }


         }
</script>
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Warehouse Purchase</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px" >
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                  <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"   >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
  <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                            
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                             
                                            </div>
                                          </div>
                                           <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Agency</label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddlAgency"     Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                 <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"   runat="server">
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
                                                          <telerik:RadDatePicker ID="txtToDate"   runat="server">
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
              <p><strong>Agency: </strong> <asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              
            </span>
            </i>      
        </div>
    </div>


            <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Transfer_date" HeaderText="Date" SortExpression ="Transfer_date"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                                                                  SortExpression ="Agency" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="PORefNo"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="PORefNo" SortExpression ="PORefNo"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("PORefNo")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"",""{1}"");", Eval("PORefNo"), Eval("Transfer_date"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

 </ContentTemplate>
 
  </asp:UpdatePanel> 
  
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
   </div>
    
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img  src="../assets/img/ajax-loader.gif"  alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
	
  
    </table>
	<%--<div style="overflow:scroll; height:445px; border:groove;background-color:white;border-color:LightGrey;border-width:1px" id="RepSec" runat="server" visible="false">
  <rsweb:ReportViewer ID="RVMain" runat="server"     ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                    DocumentMapWidth="100%"  AsyncRendering="false" SizeToReportContent="true" > 
              </rsweb:ReportViewer>    
	 </div> --%>
	
</asp:Content>
