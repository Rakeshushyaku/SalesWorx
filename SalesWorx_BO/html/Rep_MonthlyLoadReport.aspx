<%@ Page Title="Monthly Van Load" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_MonthlyLoadReport.aspx.vb" Inherits="SalesWorx_BO.Rep_MonthlyLoadReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
            height:53px !important;
        }
    </style>

        <script type="text/javascript">
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
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h4>Monthly Van/FSR Load</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
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
                                                   <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Van/FSR"  ID="ddl_Van" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
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
              <p><strong>Month: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>               
            </span>
            </i>      
        </div>
    </div>
             <div >
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
<div id="Rptdiv" runat="server" visible="false" >
            
     </div>
               <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true" 
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="true" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings  />
                                                                                                        
                                                    <Fields>
                                                         
                                                          <telerik:PivotGridRowField  ShowGroupsWhenNoData="false"  DataField="Item" ZoneIndex="0" SortOrder="None"   Caption="Item" UniqueName="Item"   CellStyle-CssClass="nowhitespace">
                                                                </telerik:PivotGridRowField>
                                                       
                                                        <telerik:PivotGridColumnField   ShowGroupsWhenNoData="false" DataField="Day" SortOrder="None"  CellStyle-CssClass="nowhitespace" >
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField  IgnoreNullValues="true" DataField="LoadQty"  SortOrder="None" DataFormatString="{0:#,##0.00}">                 
                                                        </telerik:PivotGridAggregateField>
                                                
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
               </telerik:RadPivotGrid>

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

