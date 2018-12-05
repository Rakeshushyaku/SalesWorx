<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanPerformance.aspx.vb" Inherits="SalesWorx_BO.Rep_VanPerformance" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
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
    <style type="text/css">
        div.RadPivotGrid .rgPager .rgAdvPart {
            display: none;
        }

        .RadPivotGrid .rpgRowsZone span.rpgFieldItem {
            background: none;
            border: none;
        }

        .RadPivotGrid span.rpgFieldItem {
            background: none;
            border: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Van Performance</h4>
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
                                                     <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                                                                     runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                                            </div>
                                                      </div>
          
                                                     <div class="col-sm-6">
                                                          <div class="form-group">
                                                            <label>Van</label>
                                                           <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
                                                         </div>
                                                     </div>
                                    
                                               <div class="col-sm-6">     
                                                     <div class="row">
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                               <label>From Month </label>
                                                                 <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtFromDate" runat="server" Width ="100%">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                    </DateInput>
                                                         
                                                            </telerik:RadMonthYearPicker>    

                                                            </div>
                                                      </div>
                                                      <div class="col-sm-6">
                                                           <div class="form-group">
                                                               <label>End Month </label>
                                                               <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="txtToDate" runat="server" Width ="100%">
                                                                 <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                            </DateInput>
                                                            </telerik:RadMonthYearPicker> 

                                                            </div>
                                                      </div>
                                                     </div>
                                              </div>     
                                                     

             
                                                      <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Sales District</label>
                                                             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSalesDist"  Width ="100%"
                                                              runat="server" DataTextField="Description" DataValueField="Sales_District_ID">
                                                             </telerik:RadComboBox>
                                                        </div>
                                                     </div>
                                                     <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Show By</label>
                                                            <telerik:RadComboBox Skin="Simple"   ID="ddlDisplayMode" Width="50%"
                                                    runat="server">
                                                    <Items>
                                                         <telerik:RadComboBoxItem Text="Van" Value="Van"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Text="Area" Value="Area"></telerik:RadComboBoxItem>
                                                                                                               
                                                       
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
             <p><strong>Sales District: </strong><asp:Label ID="lbl_district" runat="server" Text=""></asp:Label></p>  
            </span>
            </i>      
        </div>
    </div>
           
                            
            <div id="summary" runat="server" class="row"></div> 
                              <div class="table-responsive">
                             
                           

                              </div>

       

          

             <telerik:RadPivotGrid  RenderMode="Lightweight" AllowPaging="true"  PageSize="10" EnableViewState ="true"    
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="true" ShowDataHeaderZone="false"  
                                                     >
                                                    <TotalsSettings GrandTotalsVisibility="None"  />
                                                                                                        
                                                    <Fields>
                                                       
                                                          <telerik:PivotGridRowField DataField="Description" CellStyle-Width ="100px" ZoneIndex="0"  Caption="Name" UniqueName="Description" >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Year" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>
                                                        
                                                         <telerik:PivotGridAggregateField DataField="Calls" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="No of Outlets" SortOrder="None">                 
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField  DataField="Outlets Productivity %" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                         <telerik:PivotGridAggregateField  DataField="Adherance %" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                         <telerik:PivotGridAggregateField  DataField="Revenue" SortOrder="None">  
                                                                    
                                                        </telerik:PivotGridAggregateField>
                                                    </Fields>
                                                    <PagerStyle PageSizeControlType="None" Mode="NumericPages" />
                                                </telerik:RadPivotGrid>


        

             <asp:HiddenField id="hfCurrency" runat="server"></asp:HiddenField>
             <asp:HiddenField id="hfCurDecimal" runat="server"></asp:HiddenField>
            
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
