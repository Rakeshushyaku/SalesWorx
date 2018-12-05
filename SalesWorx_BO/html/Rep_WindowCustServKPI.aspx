<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Rep_WindowCustServKPI.aspx.vb" Inherits="SalesWorx_BO.Rep_WindowCustServKPI" MasterPageFile="~/html/Site.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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
    </script>
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
     <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }  
   .k-chart svg{
	margin:0 -14px;
}    
</style>
    </asp:Content>
 

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Window of Customer Service - KPI </h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                       </UpdatedControls>
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>
     

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server" >
     <asp:HiddenField ID="UId" runat="server" />
    <asp:HiddenField ID="hfOrgID" runat ="server" />
    <asp:HiddenField ID="HFrom" runat ="server" />
        <asp:HiddenField ID="hfCC" runat ="server" />
                <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"  >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">
                                      <ContentTemplate>
                                         <div class="row">
                                              <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                      <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>

                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true"   Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"  AutoPostBack="true" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van</label>
                                                  <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Van" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                 <div class="col-sm-2">
                                                  
                                                    <div class="form-group">
                                                        <label>Month</label>

                                                       <telerik:RadMonthYearPicker RenderMode="Lightweight" Width ="100%" ID="txtFromDate" runat="server">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                                </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>     
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
              <p><strong>Month: </strong> <asp:Label ID="lbl_FromDate" runat="server" Text=""></asp:Label></p>
               
            </span>
            </i>      
        </div>
    </div>
         
                               
    
          
 
                   <div class="table-responsive" id="Detailed"  runat="server">

      <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server" >

          <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0" Visible="false" >
                    <Tabs>
                        <telerik:RadTab Text="Daily" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Average" runat="server">
                        </telerik:RadTab>

                         
                        

                    </Tabs>
                </telerik:RadTabStrip>
           <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       <telerik:RadPageView ID="RadPageView1" runat="server">
           
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" OnItemCommand="gvRep_ItemCommand" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12" CommandItemDisplay="Top" >
                                                        <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                        <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VanCode" HeaderText="Van Code"
                                                                  SortExpression ="VanCode"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                          
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="EmpName" HeaderText="EmpName"
                                                                  SortExpression ="EmpName"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="EmpNo" HeaderText="EmpNo"
                                                                  SortExpression ="EmpNo"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="FromDate" HeaderText="Date"
                                                                  SortExpression ="FromDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TDay" HeaderText="Day"
                                                                  SortExpression ="TDay"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="FirstCall" HeaderText="FirstCall"
                                                                  SortExpression ="FirstCall"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastCall" HeaderText="LastCall"
                                                                  SortExpression ="LastCall"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Diff" HeaderText="Diff"
                                                                  SortExpression ="Diff"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                    </Columns>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
              </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                      <telerik:RadGrid id="gv_Summary" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%" OnItemCommand="gvRepSummary_ItemCommand" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12" CommandItemDisplay="Top" >
                                                        <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VanCode" HeaderText="Van Code"
                                                                  SortExpression ="VanCode"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                          
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="EmpName" HeaderText="EmpName"
                                                                  SortExpression ="EmpName"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="EmpNo" HeaderText="EmpNo"
                                                                  SortExpression ="EmpNo"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Mnth" HeaderText="Month"
                                                                  SortExpression ="Mnth"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NoofWorkingdays" HeaderText="No of Working days"
                                                                  SortExpression ="NoofWorkingdays"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Right"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="FirstCall" HeaderText="FirstCall"
                                                                  SortExpression ="FirstCall"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastCall" HeaderText="LastCall"
                                                                  SortExpression ="LastCall"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Diff" HeaderText="Diff"
                                                                  SortExpression ="Diff"  >
                                                                <ItemStyle Wrap="True"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                    </telerik:RadPageView>
               </telerik:RadMultiPage>
          </telerik:RadAjaxPanel> 
                           
                
</div>
                   
           

            
           
   
  
     
    
     <div style="display:none">
          <asp:Button  CssClass ="btn btn-primary"  ID="btn_LoadVan" runat="server" Text="Export"  /> </div>
        <asp:HiddenField ID="HToDate" runat="server" />
        <asp:HiddenField ID="HVan" runat="server" />
        <asp:HiddenField ID="HOrgID" runat="server" />

       </telerik:RadAjaxPanel>  


    

	       <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
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
