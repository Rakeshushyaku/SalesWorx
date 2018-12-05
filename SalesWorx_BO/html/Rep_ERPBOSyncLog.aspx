<%@ Page Title="ERP to Back Office Sync Log" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ERPBOSyncLog.aspx.vb" Inherits="SalesWorx_BO.Rep_ERPBOSyncLog" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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

      function OpenViewWindow(cid) {
          var URL
          URL = 'RepDetails.aspx?Type=ERPSYNCLOG&ReportName=ERPBOSyncLogDetails&ID=' + cid;
          var oWnd = radopen(URL, null);
          oWnd.SetSize(930, 600);
          oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
          oWnd.SetModal(true);
          oWnd.Center;
          oWnd.set_visibleStatusbar(false)

          return false

      }
    </script>
    <Style>
        input[type="text"].rdfd_
{
    margin:0 !important;
    padding:0 !important;
    height:0 !important;
    width:0 !important;
}

        div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }   
    </Style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
 <h4> ERP to BO Sync Exception Log</h4>
	
  <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
  <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        
               <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2"/>
                           <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                       </UpdatedControls>
                    
                   </telerik:AjaxSetting>
                 
               </AjaxSettings>
           </telerik:RadAjaxManager>

  <asp:UpdatePanel ID="Panel" runat="server"  >
        <ContentTemplate>
	
<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-3">
                                                     
                                            <div class="form-group">
                                                <label>From Date</label>
           <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
               
        </div>
                                                     </div>
                                                    
            <div class="col-sm-6">
                                                     
                                            <div class="form-group">
                                                <label>ERP Table</label>
            <telerik:RadComboBox ID="ddl_ERPTable" runat="server"  Skin="Simple"
                  Width="290px"    >
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
                <p><strong>From Date</strong> <asp:Label ID="lbl_Fromdt" runat="server" Text=""></asp:Label></p>
                <p><strong>To Date</strong> <asp:Label ID="lbl_Todt" runat="server" Text=""></asp:Label></p>
               <p><strong>ERP Table: </strong> <asp:Label ID="lbl_tbl" runat="server" Text=""></asp:Label></p>
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
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Log_ID" HeaderText="Log ID"
                                                                  SortExpression ="Log_ID" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Source_Table" HeaderText="ERP Table"
                                                                  SortExpression ="Source_Table" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Destination_Table" HeaderText="Back Office Table"
                                                                  SortExpression ="Destination_Table" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Logged_At" HeaderText="Logged At" SortExpression ="Logged_At"
                                                               DataFormatString="{0:dd-MMM-yyyy hh:mm:ss tt}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                         <telerik:GridTemplateColumn uniqueName="TotErrors"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="TotErrors" SortExpression ="TotErrors"
                                                                HeaderText="Error Count" ItemStyle-HorizontalAlign="Right" >
                                                            <ItemTemplate>
                                                               
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("TotErrors")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenViewWindow(""{0}"");" , Eval("Log_ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                           
                                                    </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

 </ContentTemplate> </asp:UpdatePanel> 
  
   <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
       
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
