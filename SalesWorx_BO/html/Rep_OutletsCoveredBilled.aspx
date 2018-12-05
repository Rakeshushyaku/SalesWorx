<%@ Page Title="Outlets Covered vs Billed" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_OutletsCoveredBilled.aspx.vb" Inherits="SalesWorx_BO.OutletsCoveredBilled" %>

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
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
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

    <h4>Outlets Covered vs Billed</h4>
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
                                            <div class="col-sm-10">
                                                <div class="row">
                                                    <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                                     <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                                     
          
                                                     <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Van</label>
                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                    </div>
                                              
                                                  <div class="row">
                                                     <div class="col-sm-6">
                                                         <div class="row">
                                                             <div class="col-sm-6">
                                                               <div class="form-group">
                                                                   <label>From Date </label>
                                                                    <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                                     <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
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

             <div class="overflowx" >

                <div class="chart-wrapper padding0" style="" id="Chartwrapper" runat="server" >

                


 <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Outlets Covered vs Billed"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server" Visible="false" ID="Chart" Height="500" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="Covered" Name="No of Outlets Covered" >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                No of Outlets Covered :   #=dataItem.Covered#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:ColumnSeries DataFieldY="Billed" Name="No of Outlets Billed">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                                No of Outlets Billed :   #=dataItem.Billed#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                </Series>
                <XAxis DataLabelsField="SalesRep_Name" >
                    <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
                <YAxis>
                    <TitleAppearance Text="No of outlets" >
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
                <p><br /><br /></p>
                 
                   <div class="table-responsive" id="Detailed"  runat="server">
                      
 
      <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
          <ContentTemplate>
      
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="70%"
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van"
                                                                  SortExpression ="SalesRep_Name"  >
                                                                <ItemStyle Wrap="true"  HorizontalAlign="Left"  />
                             
                                                            </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Covered" HeaderText="No of Outlets Covered<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Covered"  HeaderTooltip="Unique Outlets for which Sales invoice was generated" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                             <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
            
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="Billed" HeaderText="No of Outlets Billed<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Billed" HeaderTooltip="Unique Outlets for which Sales invoice was generated" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                            </telerik:GridBoundColumn>
                        
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Percentage" HeaderText="Billing Productivity (%)<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Percentage" DataType="System.Decimal" DataFormatString="{0:0.00}" HeaderTooltip="Outlet level billing productivity" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                            <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 
                                                        </MasterTableView>
                                                    </telerik:RadGrid> </ContentTemplate>
          </asp:UpdatePanel> 
                           
                  
</div>
           
</div>
       
     

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
