<%@ Page Title="Incentive Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepIncentive.aspx.vb" Inherits="SalesWorx_BO.RepIncentive" %>

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
        .hide
  {
    display: none !important;
  }
    </style>
    <script type="text/javascript">
        function OpenLocWindow(Org_ID, Incentive_Month, Incentive_Year,  Emp_Code) {


            var URL
            URL = 'RepIncentiveMonthDetails.aspx?Org_ID=' + Org_ID + '&Incentive_Month=' + Incentive_Month + '&Incentive_Year=' + Incentive_Year + '&Emp_Code=' + Emp_Code;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(1000, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            return false
        }
        function clickExportBiffExcel() {
            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
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
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="tooltip"]').on('click', function () { $(this).tooltip('hide'); });
        }

    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
        .k-chart svg{
	        margin:0 -6px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Incentive</h4>
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional"  >
        <contenttemplate>
      

                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                        <div class="col-sm-10 col-md-10 col-lg-10">
            <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization <em><span></span>*</em></label>
            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"   Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </telerik:RadComboBox>
             </div>
                                          </div>
                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Employee <em><span></span>*</em></label>
                                                     <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Employee" ID="ddl_empcode" Width ="100%" runat="server"  >
                                                            </telerik:RadComboBox >
            
                     
                   </div>
                                          </div>

                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Select Year </label>
                                                     <%-- <telerik:RadDatePicker ID="dtp_incentiveyear"   runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar4" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                          
                                                    </telerik:RadDatePicker>--%>
                                              
                   <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Year" ID="ddl_year" Width ="100%" runat="server"  >
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
             <div id="repdiv" runat="server" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
             
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div> 
                              <div class="table-responsive">
                             
                           

                              </div>

             
                
                 
                   <div class="table-responsive" id="Detailed"  runat="server">
                      
 
      <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
          <ContentTemplate>
      
                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="15" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="15" CommandItemDisplay="Top">
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                       <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>
                            <PagerStyle PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>

                                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Incentive_Year" HeaderText="Year"
                                                                  SortExpression ="Incentive_Year"  >
                                                                <ItemStyle Wrap="false" />
                                                             </telerik:GridBoundColumn>  

                                                           <%--  <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Tmonth" HeaderText="Month"
                                                                  SortExpression ="Incentive_Month" >
                                                                <ItemStyle Wrap="false" />
                                                             </telerik:GridBoundColumn> --%>
                                                             
                                                            <telerik:GridTemplateColumn UniqueName="Tmonth" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Tmonth" 
                                                                SortExpression="Incentive_Month" HeaderText="Month">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Lnk_Month"  runat="server" Text='<%# Bind("Tmonth")%>' CommandName="MonthDetails" ForeColor="SteelBlue" Font-Underline="true"    ></asp:LinkButton>
                                                             <asp:Label ID="lblROW_Month" runat ="server" Visible ="false"  Text='<%# Bind("Incentive_Month")%>'></asp:Label>
                                                            
                                                             
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="false" />
                                                        </telerik:GridTemplateColumn>
                                                                                                                                            
                                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="NET_SALES_VALUE" HeaderText="Target Sales Value"
                                                                  SortExpression ="NET_SALES_VALUE" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   
                                                             

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="NET_SALES_VOLUME" HeaderText="Target Sales Volume"
                                                                  SortExpression ="NET_SALES_VOLUME" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="SUCCESSFUL_VISITS" HeaderText="Target Visits"
                                                                  SortExpression ="SUCCESSFUL_VISITS" DataFormatString="{0:#,###.00}" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                             </telerik:GridBoundColumn>    
                                                       

                                                             <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Sales_Value_Acheived" HeaderText="Acheived Sales Value"
                                                                  SortExpression ="Sales_Value_Acheived" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   
                                                             

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Sales_Volume_Acheived" HeaderText="Acheived Sales Volume"
                                                                  SortExpression ="Sales_Volume_Acheived" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Success_Visits_Acheived" HeaderText="Acheived Visits"
                                                                  SortExpression ="Success_Visits_Acheived" DataFormatString="{0:#,###.00}" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>    

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="TotalCommission" HeaderText="Total Commission"
                                                                  SortExpression ="TotalCommission" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="CommissionTarget" HeaderText="Commission As Per Target"
                                                                  SortExpression ="CommissionTarget" DataFormatString="{0:#,###.00}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                             </telerik:GridBoundColumn>   
                                                        
                                                          


                                                      
                                                        </Columns>
                                                        
                                                        
                            

                                                     <%--<GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Visit" FieldName="VisitDate"    HeaderValueSeparator =" Date : "
                                                                FormatString="{0:dd-MMM-yyyy}"    ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="VisitDate"  >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                     
                                                                <telerik:GridGroupByField FieldAlias="Van" FieldName="Salesrep_name"   
                                                                   ></telerik:GridGroupByField>
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                             
                                                                     <telerik:GridGroupByField FieldName="Salesrep_name" >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                    </GroupByExpressions>
                                    --%>

                                                        </MasterTableView>
                                                    </telerik:RadGrid> </ContentTemplate>
          </asp:UpdatePanel> 
                           
                  
</div>

<p><br /><br /></p>

                 <div class="overflowx" >

                <div class="chart-wrapper padding0" style="" id="Chartwrapper" runat="server" >

                


 <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Total  vs Target Commission"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server" Visible="false" ID="Chart" Height="500" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="TotalCommission" Name="Total Commission" >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Total Commission :   #=dataItem.TotalCommission#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:ColumnSeries DataFieldY="CommissionTarget" Name="Commission As Per Target">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                Commission As Per Target  :   #=dataItem.CommissionTarget#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                </Series>
                <XAxis DataLabelsField="Tmonth" >
                    <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="Commissions" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>
            </PlotArea>
            <Legend>
                <Appearance Visible="true" Position="Top"></Appearance>
            </Legend>
           <%-- <ChartTitle Text="Outlets Covered vs Billed">
            </ChartTitle>--%>
        </telerik:RadHtmlChart>



                    </div>


                 </div>
           
</div>
       
     

    </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
                             <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export"  />
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
