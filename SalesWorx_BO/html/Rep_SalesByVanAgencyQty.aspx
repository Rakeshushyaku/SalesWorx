<%@ Page Title="Sales By Van/Agency(Qty)" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_SalesByVanAgencyQty.aspx.vb" Inherits="SalesWorx_BO.Rep_SalesByVanAgencyQty" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
  <style type="text/css">
        .style2 {
            width: 240px;
        }
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }

       .RadPivotGrid .rpgRowsZone span.rpgFieldItem {
            background: none;
            border: none;
        }

        .RadPivotGrid span.rpgFieldItem {
            background: none;
            border: none;
        }

        #ctl00_MainContent_gvRep_OT > tbody > tr:first-child {
            height:88px !important;
        }
      #ctl00_MainContent_gvRep_OT thead tr:last-child th.rpgColumnHeader      {
          height: 53px !important;
      }
    </style>
    <script>
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

        function UpdateHeader() {

            var HMV = document.getElementById('<%= HM.ClientID%>').value;
            var HMV1 = document.getElementById('<%= HM1.ClientID%>').value;
            var HMV2 = document.getElementById('<%= HM2.ClientID%>').value;

            $("th:contains('Sum of QtyM2')").html(HMV2)
            $("th:contains('Sum of QtyM1')").html(HMV1)
            $("th:contains('Sum of QtyM')").html(HMV)


        }
        
    </script>
    </asp:Content><asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Sales By Van/Agency(Qty)</h4> 
  <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	 <asp:HiddenField runat="server" id="HM" ></asp:HiddenField>
            <asp:HiddenField runat="server" id="HM1"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HM2"></asp:HiddenField>

 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"  >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">
                                      <ContentTemplate>
                                         <div class="row">
                                              <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                      
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"  >
                                        </telerik:RadComboBox>
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
              
            </span>
            </i>      
        </div>
      
    </div>
            <div>
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
 <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="true" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings  />
                                                                                                        
                                                    <Fields>
                                                         
                                                          <telerik:PivotGridRowField  ShowGroupsWhenNoData="false"  DataField="Van" ZoneIndex="0"   Caption="Van" UniqueName="Van"   CellStyle-CssClass="nowhitespace">
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField   ShowGroupsWhenNoData="false" DataField="Agency" SortOrder="None"  CellStyle-CssClass="nowhitespace" >
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField  IgnoreNullValues="true" DataField="QtyM2"  SortOrder="None" DataFormatString="{0:#,##0.00}" Caption="Month1">                 
                                                        </telerik:PivotGridAggregateField>
                                                         <telerik:PivotGridAggregateField  IgnoreNullValues="true" DataField="QtyM1"  SortOrder="None" DataFormatString="{0:#,##0.00}" Caption="Month2">                 
                                                        </telerik:PivotGridAggregateField>
                                                  <telerik:PivotGridAggregateField  IgnoreNullValues="true" DataField="QtyM"  SortOrder="None" DataFormatString="{0:#,##0.00}" Caption="Month">                 
                                                        </telerik:PivotGridAggregateField>
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
               </telerik:RadPivotGrid>
 
 </ContentTemplate> </asp:UpdatePanel> 
  
   <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>

   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
</asp:Content>
