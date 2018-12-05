<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="test.aspx.vb" Inherits="SalesWorx_BO.test" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <div id="chart2">
                                                <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="true" 
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
                                        <telerik:LineSeries DataFieldY="Return" Name="Return" AxisName="Return" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Green" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                   Return :   #=dataItem.Return#
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

                                        <telerik:AxisY  Name="Return" Color="Green"  >
                                          
                                           <TitleAppearance Text="Return"   >
                                                <TextStyle Color="Green" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>                     
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>





            <%--  <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="true" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" PlotArea-XAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"   PlotArea-XAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="summaryChart" Height="500" Transitions="true" Skin="Silk">
                           <PlotArea>
                                    <Series >
                                        <telerik:LineSeries  DataFieldY="TotalCallVisited" Name="No of Calls Visited" >
                                            <LabelsAppearance Visible="false"></LabelsAppearance>
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                <ClientTemplate >
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    No of Calls Visited :   #=dataItem.TotalCallVisited#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="TotalProductiveCall" Name="No of Productive Calls">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    No of Productive Calls :   #=dataItem.TotalProductiveCall#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                 
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
                                        <telerik:LineSeries DataFieldY="Return" Name="Return" AxisName="Return" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Green" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                   Return :   #=dataItem.Return#
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

                                        <telerik:AxisY  Name="Return" Color="Green"  >
                                          
                                           <TitleAppearance Text="Return"   >
                                                <TextStyle Color="Green" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>                     
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>--%>

             <%--<telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"    Visible="true" 
                            PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black" PlotArea-XAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"   PlotArea-XAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-TitleAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"  PlotArea-YAxis-LabelsAppearance-TextStyle-FontFamily ="Segoe UI,Trebuchet MS, Arial !important"
                             ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server"  ID="summaryChart" Height="500" Transitions="true" Skin="Silk">
                           <PlotArea>
                                    <Series >
                                        <telerik:ColumnSeries   DataFieldY="Sales" Name="Sales" >
                                            <LabelsAppearance Visible="false"></LabelsAppearance>
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White"  >
                                                <ClientTemplate >
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    Sales :   #=dataItem.Sales#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                        <telerik:ColumnSeries DataFieldY="Return" Name="Return">
                                            <Appearance Overlay-Gradient="None"></Appearance>
                                             <LabelsAppearance Visible="false" ></LabelsAppearance>
                                            <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                    Return :   #=dataItem.Return#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:ColumnSeries>
                                 
                                    </Series>
                                    <Series>
                                      <telerik:LineSeries DataFieldY="TotalCallVisited" Name="TotalCallVisited" AxisName="TotalCallVisited" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Crimson" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                   Total Calls :   #=dataItem.TotalCallVisited#
                                                </ClientTemplate>
                                            </TooltipsAppearance>
                                        </telerik:LineSeries>
                                        <telerik:LineSeries DataFieldY="TotalProductiveCall" Name="TotalProductiveCall" AxisName="TotalProductiveCall" >
                                          <Appearance>
                                                 <FillStyle BackgroundColor="Green" />
                                            </Appearance>
                                          <LabelsAppearance Visible="false"></LabelsAppearance>
                                          <Appearance Overlay-Gradient="None"></Appearance>
                                            <TooltipsAppearance Visible="true" Color="White" BackgroundColor="Black"  >
                                                <ClientTemplate>
                                                    Van : #=dataItem.FSR#
                                                    <br />
                                                   Total Productive Call :   #=dataItem.TotalProductiveCall#
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
                                        
                                       <telerik:AxisY  Name="TotalCallVisited" Color="Crimson"  >
                                          
                                           <TitleAppearance Text="Total Calls"   >
                                                <TextStyle Color="Crimson" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>

                                        <telerik:AxisY  Name="TotalProductiveCall" Color="Green"  >
                                          
                                           <TitleAppearance Text="Total Productive Calls"   >
                                                <TextStyle Color="Green" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14"  />
                                           </TitleAppearance>
                                        
                                       </telerik:AxisY>
                                       
                                    </AdditionalYAxes>                     
                                </PlotArea>
                                <Legend>
                                    <Appearance Visible="true" Position="Top"></Appearance>
                                </Legend>                               
                            </telerik:RadHtmlChart>--%>
                                            </div>
</asp:Content>
