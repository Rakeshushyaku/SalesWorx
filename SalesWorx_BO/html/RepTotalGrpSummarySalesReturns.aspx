<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepTotalGrpSummarySalesReturns.aspx.vb" Inherits="SalesWorx_BO.RepTotalGrpSummarySalesReturns" %>
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
    <h4>Total Group Summary Company Wise  Sales/Returns</h4>
	
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
                                                <label>Principal </label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Principal"  ID="ddlAgency" Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                            
                                      
                                                 
               <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR </label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van/FSR"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                     </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                              </div>
            
           

                                              
                                                  <div class="row">
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
            
               <div class="col-sm-4"  >
                                                          <div class="form-group"> <label>Customer   </label>
                 <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Customer"  ID="ddlCustomer" Width ="100%" runat="server" DataTextField="Location" DataValueField="LocCode" Filter="Contains">
                                                     </telerik:RadComboBox >
                     
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
              <p><strong>Principal: </strong> <asp:Label ID="lbl_Principle" runat="server" Text=""></asp:Label></p>
              <p><strong>Customer: </strong> <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date : </strong><asp:Label ID="lbl_Fromdt" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date : </strong><asp:Label ID="lbl_ToDate" runat="server" Text=""></asp:Label></p>
                         
            </span>
            </i>      
        </div>
    </div>
            <div class="row">
            <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>

            <div id="divCurrency" runat="server" visible="false"  >
             
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>
                </div>
               <div id="summary" runat="server" class="row"></div> 
                              
                
                                <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="true"  PageSize="10"
                                                    ID="gvRep" runat="server"  
                                                    ShowFilterHeaderZone="false" AllowFiltering ="false" ShowColumnHeaderZone="false" ShowRowHeaderZone="false" ShowDataHeaderZone="false" 
                                                     >
          <TotalsSettings  ColumnsSubTotalsPosition="None"   />
                                                    <Fields>
                                                          <telerik:PivotGridRowField DataField="Customer" ZoneIndex="0" Caption="Customer " >
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

            <asp:HiddenField id="HCurrency" runat="server"></asp:HiddenField>
 </ContentTemplate>
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
