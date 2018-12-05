<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_Summary.aspx.vb" Inherits="SalesWorx_BO.Rep_Summary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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
    <style type="text/css">
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Jan,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Feb,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Mar,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Apr,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_May,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Jun,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Jul,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Aug,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Sep,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Oct,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Nov,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Dec,
        #ctl00_MainContent_rpbFilter_i0_RadYear_MonthYearTableViewID #rcMView_Today{
            display:none;
        }
        #ctl00_MainContent_rpbFilter_i0_txtFromDate_calendar,
        #ctl00_MainContent_rpbFilter_i0_txtToDate_calendar {
            width: 100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Purchase Vs Sales Vs Returns Report</h4>
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


    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
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
                                                <label>Agency</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Agency"  ID="ddlAgency" Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                            
                                      
                                                    </div>

                                              <div class="row">
                                              <div class="col-sm-6">
                                            <div class="form-group">
                                                    <label>Type</label>
                                                   <telerik:RadComboBox Skin="Simple" AutoPostBack="true" EmptyMessage="Select Type"  ID="ddlType" Width ="100%" runat="server"
                                                        DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                       <Items>                                                          
                                                              <telerik:RadComboBoxItem  Value="D" Text="Daily" Selected="true" />
                                                              <telerik:RadComboBoxItem  Value="W" Text="Weekly" />
                                                              <telerik:RadComboBoxItem  Value="M" Text="Monthly" />
                                                              <telerik:RadComboBoxItem  Value="Y" Text="Yearly" />                 
                                                       </Items>
                                                     </telerik:RadComboBox >
                                                 </div>
                                             </div>

                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                    <label>Van/FSR</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van/FSR"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                     </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                              </div>

                                              
                                                  <div class="row">
                                                     <div class="col-sm-6"  >
                                                       <div class="form-group" runat="server" id="divDaily">
                                                           <label>Period   </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"   Width ="45%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar> 
                                                    </telerik:RadDatePicker> &nbsp;&nbsp; To &nbsp;&nbsp;
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="45%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                        </div>

                                                         <div class="form-group" runat="server" id="divWeekly" visible="false">
                                                           <label>Period   </label>
                                                               <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" AutoPostBack="true" Width="45%" ID="ddWeekMonthYear" runat="server">
                                                                    <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                    </DateInput>

                                                             </telerik:RadMonthYearPicker>

                                                             &nbsp;&nbsp; <telerik:RadComboBox Skin="Simple"  ID="ddlWeek" Width ="45%" runat="server">                                                      
                                                          </telerik:RadComboBox >
                                                         </div>
                                                          <div class="form-group" runat="server" id="divMonthly" visible="false">
                                                           <label>Period   </label>
                                                               <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple" AutoPostBack="true" Width="45%" ID="RadMonth" runat="server">
                                                                    <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                    </DateInput>

                                                             </telerik:RadMonthYearPicker>

                                                             
                                                         </div>
                                                         <div class="form-group" runat="server" id="divYear" visible="false">
                                                           <label>Period   </label>
                                                               <telerik:RadMonthYearPicker RenderMode="Lightweight" Skin="Simple"  ID="RadYear" runat="server" Width="45%" >
                                                                        <DateInput ReadOnly="true" runat="server" DateFormat="yyyy" DisplayDateFormat="yyyy">
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
              <p><strong>Agency: </strong> <asp:Label ID="lbl_Principle" runat="server" Text=""></asp:Label></p>
              <p><strong>Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
              <p><strong>Period : </strong><asp:Label ID="lbl_Period" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>            
            </span>
            </i>      
        </div>
    </div>
            <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
               <div id="summary" runat="server" class="row"></div> 
                              
                
                                <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="true"  PageSize="10"
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering ="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" 
                                                     >
          <TotalsSettings  ColumnsSubTotalsPosition="None"   />
                                                    <Fields>
                                                          <telerik:PivotGridRowField DataField="salesRepName" ZoneIndex="0" Caption="Salesman " >
                                                                </telerik:PivotGridRowField>
                                                        <telerik:PivotGridColumnField DataField="Agency" SortOrder="None"  >
                 
                                                        </telerik:PivotGridColumnField>
                                                         <telerik:PivotGridColumnField DataField="Type" SortOrder="None">
                 
                                                        </telerik:PivotGridColumnField>

                                                        <telerik:PivotGridAggregateField DataField="Qty"     >
                                                        </telerik:PivotGridAggregateField>
                                                        <telerik:PivotGridAggregateField DataField="Value" >
                                                        </telerik:PivotGridAggregateField>
                                                        

                                                       
                                                    </Fields>
         
                                                </telerik:RadPivotGrid>               
                           

                             
            <asp:HiddenField id="Hcurrency" runat="server"></asp:HiddenField>

        </contenttemplate>
    </asp:UpdatePanel>
       <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
        <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>

 