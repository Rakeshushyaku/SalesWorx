<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepPrincipalWiseSales.aspx.vb" Inherits="SalesWorx_BO.RepPrincipalWiseSales" %>
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

        function OpenSalViewWindow(agency, qty) {
            
            if (qty > 0) {
                var param3 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param4 = document.getElementById('<%= HTo.ClientID%>').value;
                var param1 = document.getElementById('<%= HorgID.ClientID%>').value;



                var URL
                URL = 'RepDetails.aspx?Type=PWS&rtype=S&ReportName=PrincipalWiseSalesReturn_Dtl&SID=0&From=' + param3 + '&To=' + param4 + '&age=' + agency + '&Org=' + param1;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(1000, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }

        function OpenRetViewWindow(agency, qty) {
            if (qty > 0) {
                var param3 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param4 = document.getElementById('<%= HTo.ClientID%>').value;
                var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
                var URL
                URL = 'RepDetails.aspx?Type=PWS&rtype=R&ReportName=PrincipalWiseSalesReturn_Dtl&SID=0&From=' + param3 + '&To=' + param4 + '&age=' + agency + '&Org=' + param1;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(1000, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Sales By Principal</h4>
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
         <asp:HiddenField ID="HTo" runat="server" />
         <asp:HiddenField ID="Hfrom" runat="server" />         
         <asp:HiddenField ID="HorgID" runat="server" />

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
                                                <label>Principal</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Principal" EnableCheckAllItemsCheckBox="true" ID="ddlAgency" Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                    </div>
                                              
                                                  <div class="row">
                                                     <div class="col-sm-3">
                                                       <div class="form-group">
                                                           <label>From Date  </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
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
              <p><strong>Agency: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>            
            </span>
            </i>      
        </div>
    </div>

            <div >
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>

            <div id="summary" runat="server" class="row"></div> 

                        <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                <asp:HiddenField runat="server" ID="hfDigit" Value="N2" />
</div>
                 <div class="overflowx" >

                <div class="chart-wrapper padding0" style="" id="Chartwrapper" runat="server" >

                


 <telerik:RadHtmlChart ChartTitle-Appearance-TextStyle-Color="black" ChartTitle-Appearance-TextStyle-Bold="true"      ChartTitle-Text="Principal Wise Sales & Returns"
        PlotArea-XAxis-LabelsAppearance-Color ="black"  PlotArea-YAxis-LabelsAppearance-Color="black"  
         ChartTitle-Appearance-TextStyle-FontFamily="Segoe UI,Trebuchet MS, Arial !important" PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false" runat="server" Visible="false" ID="Chart" Height="500" Transitions="true" Skin="Silk">
       <PlotArea>
                <Series >
                    <telerik:ColumnSeries DataFieldY="SValue" Name="Sales" >
                        <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None"  ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                               Sales :   #=dataItem.SValue#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                    <telerik:ColumnSeries DataFieldY="RValue" Name="Returns">
                         <LabelsAppearance Visible="false"></LabelsAppearance>
                        <Appearance Overlay-Gradient="None" ></Appearance>
                        <TooltipsAppearance Visible="true">
                            <ClientTemplate>
                               Returns :   #=dataItem.RValue#
                            </ClientTemplate>
                        </TooltipsAppearance>
                    </telerik:ColumnSeries>
                </Series>
                <XAxis DataLabelsField="Agency" >
                    <LabelsAppearance RotationAngle="-90"></LabelsAppearance>
                    <MinorGridLines Visible="false"></MinorGridLines>
                    <MajorGridLines Visible="false"></MajorGridLines>
                </XAxis>
              <%--  <YAxis>
                    <TitleAppearance Text="No of outlets" >
                        <TextStyle Color="black" FontFamily="Segoe UI,Trebuchet MS, Arial !important" FontSize="14" Bold="true" />
                    </TitleAppearance>
                    
                </YAxis>--%>
            </PlotArea>
            <Legend>
                <Appearance Visible="true" Position="Top"></Appearance>
            </Legend>
           <%-- <ChartTitle Text="Outlets Covered vs Billed">
            </ChartTitle>--%>
        </telerik:RadHtmlChart>



                    </div>


                 </div>


                            
            
                              <div class="table-responsive">
                
                                              <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>

                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                                                                  SortExpression ="Agency" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>

                                                        <%--<telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="Sqty" HeaderText="Sales Qty"
                                                                  SortExpression ="Sqty" DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>--%>

                                                         <telerik:GridTemplateColumn UniqueName="Sqty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="Sqty" SortExpression="Sqty"
                                                                HeaderText="Sales Qty" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("SValue").ToString()) > 0, True, False)%>' DataFormatString="{0:#,##0.00}" OnClientClick='<%# String.Format("OpenSalViewWindow(""{0}"", ""{1}"");", Eval("Agency"), Eval("Sqty"))%>' ID="lnkSalQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Sqty", "{0:#,##0.00}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                        </telerik:GridTemplateColumn>

                                                         <telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="SValue" HeaderText="Sales Value"
                                                                  SortExpression ="SValue" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridBoundColumn>


                                                     <%--   <telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="Rqty" HeaderText="Returns Qty"
                                                                  SortExpression ="Rqty" DataFormatString="{0:#,##0.00}">
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>--%>

                                                                 <telerik:GridTemplateColumn UniqueName="Rqty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="Rqty" SortExpression="Rqty"
                                                                HeaderText="Returns Qty" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("RValue").ToString()) > 0, True, False)%>' DataFormatString="{0:#,##0.00}" OnClientClick='<%# String.Format("OpenRetViewWindow(""{0}"", ""{1}"");", Eval("Agency"), Eval("Rqty"))%>' ID="lnkRetQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rqty", "{0:#,##0.00}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                        </telerik:GridTemplateColumn>

                                                         <telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="RValue" HeaderText="Returns Value"
                                                                  SortExpression ="RValue" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridBoundColumn>
                                                   
                                                         

                                                            <telerik:GridCalculatedColumn UniqueName="RetValPers" ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center"  HeaderText="Returns %"
                                                              DataFields="RValue, SValue"  Expression="  iif({1} = 0, 0, ({0}/{1})* 100 )   "    DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridCalculatedColumn>
                                                   
                                                          
                                                        </Columns>

                                                  
                                    

                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                

                                 </ContentTemplate>
                                </asp:UpdatePanel>
                           

                              </div>
        
       <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

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



