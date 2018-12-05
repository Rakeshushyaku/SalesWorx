<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepDailyReport.aspx.vb" Inherits="SalesWorx_BO.RepDailyReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    div.RadGrid .rgHeader {
       white-space:nowrap;
    }
    .k-chart svg{
	margin:0 -7px;
}
</style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Daily Report</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>



                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
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
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_org" Width="100%"
                                                                runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Van</label>
                                                            <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" ID="ddl_Van" Width="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-6">
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>From Date </label>
                                                            <telerik:RadDatePicker ID="txt_fromDate" Width="100%" runat="server">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar2" runat="server">
                                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>To Date </label>
                                                            <telerik:RadDatePicker ID="txt_ToDate" Width="100%" runat="server">
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
                                                </div>


                                            </div>



                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />  
                                                </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

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
                                        <p><strong>Organisation: </strong>
                                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
                                        <p><strong>Van: </strong>
                                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                                        <p><strong>Form Date: </strong>
                                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
                                        <p><strong>To Date: </strong>
                                            <asp:Label ID="lbl_to" runat="server" Text=""></asp:Label></p>
                                    </span>
                                </i>
                            </div>
                        </div>


                          <div id="divCurrency" runat="server" visible="false"  >
                                                <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                           </div>


                      
                            
                <telerik:RadTabStrip ID="DailyReptab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Summary" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="Productivity" runat="server">
                        </telerik:RadTab>
                        
                        <telerik:RadTab Text="Sales" runat="server">
                        </telerik:RadTab>

                        <%--<telerik:RadTab Text="Collection" runat="server">
                        </telerik:RadTab>--%>

                    </Tabs>
                </telerik:RadTabStrip>

               <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                     <telerik:RadPageView ID="RadPageView1" runat="server">
                     
                         <%--Summary Chart--%>
                         <div class="overflowx">

                                <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="false" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" PlotArea-XAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"   PlotArea-XAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="summaryChart" Height="500" Transitions="true" Skin="Silk">
                           <PlotArea>
                                    <Series >
                                        <telerik:ColumnSeries DataFieldY="TotalCallVisited" Name="No of Calls Visited" >
                                            <LabelsAppearance Visible="false"></LabelsAppearance>
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                <ClientTemplate >
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    No of Calls Visited :   #=dataItem.TotalCallVisited#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                        <telerik:ColumnSeries DataFieldY="TotalProductiveCall" Name="No of Productive Calls">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    No of Productive Calls :   #=dataItem.TotalProductiveCall#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                 
                                    </Series>
                                    <Series>
                                      <telerik:LineSeries DataFieldY="Sales" Name="Net Sales" AxisName="Sales" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Crimson" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                   Sales :   #=dataItem.Sales#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                    </Series>
                                    <XAxis DataLabelsField="FSR" Color="LightGray"  >
                                        <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                                        <MinorGridLines Visible="false"></MinorGridLines>
                                        <MajorGridLines Visible="false"></MajorGridLines>
                                    </XAxis>       
                                    <YAxis Color="LightGray" >
                                         <TitleAppearance Position="Center" RotationAngle="0" Text="Total">
                                             <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" />
                                         </TitleAppearance>
                                    </YAxis>    
                                    <AdditionalYAxes  >
                                        
                                       <telerik:AxisY  Name="Sales" Color="Crimson"  >
                                          
                                           <TitleAppearance Text="Net Sales"   >
                                                <TextStyle Color="#14b4fc" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>                     
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>
                         </div>


                         <%--Summary Chart Ends here--%>

                         <p><br /><br /></p>
                    <div class="overflowx">
                        <asp:updatePanel runat="server" ID="innerPanel" UpdateMode="Conditional" >
                            <ContentTemplate>
                                      <telerik:RadGrid id="gvRep" AutoGenerateColumns="True" Skin="Simple" Width="100%" BorderColor="LightGray" Visible="false" 
                                PageSize="14" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                                            <ClientSettings EnableRowHoverStyle="true">
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="True" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="14">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                       

                                                 <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Van" FieldName="FSR"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="FSR" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                        
                                                    </GroupByExpressions>
                                    

                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:updatePanel> 
                              
                     </div> 

                                                  

                    </telerik:RadPageView>

                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        <p><br /></p>
                            <%--Productivity Chart--%>
                         <div class="overflowx" >

                           <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"  Visible="false"  
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" ChartTitle-Text="Productivity"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="prodChart" Height="500" Transitions="true" Skin="Silk"  >
                           <PlotArea >
                                   <%-- <Series >
                                      <telerik:LineSeries DataFieldY="Productivity" Name="Productivity">
                                            <LabelsAppearance Visible="false"></LabelsAppearance>
                                            <TooltipsAppearance Visible="true">
                                                <ClientTemplate>
                                                    Van : #=dataItem.Van#
                                                    <br />
                                                   Productivity  :   #=dataItem.Productivity#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                    </Series>--%>
                               <XAxis DataLabelsField="VisitDate" Color="LightGray">
                                        <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                                        <MinorGridLines Visible="false"></MinorGridLines>
                                        <MajorGridLines Visible="false"></MajorGridLines>
                                    </XAxis>       
                                    <YAxis Color="LightGray"  MaxValue="100.9" >
                                         <TitleAppearance Position="Center" RotationAngle="0" Text="Productivity %">
                                             <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" />
                                         </TitleAppearance>
                                    </YAxis>                         
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="True" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>
                         </div>


                         <%--Productivity Chart Ends here--%>
                    </telerik:RadPageView>

                   <telerik:RadPageView ID="RadPageView3" runat="server">
                         <p><br /></p>
                            <%--Sales Chart--%>
                         <div class="overflowx">

                           <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"     Visible="false" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" ChartTitle-Text="Sales"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="saleChart" Height="500" Transitions="true" Skin="Silk">
                           <PlotArea>
                                
                                    <XAxis DataLabelsField="VisitDate" Color="LightGray">
                                        <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                                        <MinorGridLines Visible="false"></MinorGridLines>
                                        <MajorGridLines Visible="false"></MajorGridLines>
                                    </XAxis>       
                                    <YAxis Color="LightGray">
                                         <TitleAppearance Position="Center" RotationAngle="0" Text="Sales">
                                             <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="12" />
                                         </TitleAppearance>
                                    </YAxis>                         
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="True" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>
                         </div>


                         <%--Productivity Chart Ends here--%>
                    </telerik:RadPageView>

                   <telerik:RadPageView ID="RadPageView4" runat="server">

                    </telerik:RadPageView>

                </telerik:RadMultiPage>

                                
                       
                        
                             
                     <asp:HiddenField ID="hfCurrency" runat="server" Value='' ></asp:HiddenField>                                 
                            <asp:HiddenField ID="hfDecimal" runat="server" Value='0' ></asp:HiddenField>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <div style="display: none">
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



                    </ProgressTemplate>
                </asp:UpdateProgress>


                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="innerPanel" runat="server" DisplayAfter="10">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel1" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                            <span>Processing... </span>
                        </asp:Panel>



                    </ProgressTemplate>
                </asp:UpdateProgress>

                <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove" ShowBackButton="true"
                    ProcessingMode="Remote" Width="58%"
                    SizeToReportContent="true" AsyncRendering="false"
                    DocumentMapWidth="100%" ShowParameterPrompts="False" Visible="False">
                </rsweb:ReportViewer>
</asp:Content>
