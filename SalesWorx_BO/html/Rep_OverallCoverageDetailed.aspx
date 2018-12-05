<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Rep_OverallCoverageDetailed.aspx.vb" Inherits="SalesWorx_BO.Rep_OverallCoverageDetailed" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/responsive.css" rel="stylesheet" type="text/css" />
    <style type="text/css">  
        .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            font-size: 13px;
            font-weight: bold;
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #eff9ff !Important;
            font-weight: bold !Important;
        }

    
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }  
        </style>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <asp:HiddenField ID="HSID" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />

    <div class="table-responsive" id="Detailed"  runat="server" style="padding:15px">
        <div class="row">
            <div class="col-sm-3">
                <label>Sales Rep Name</label><p><strong class="text-blue"><asp:Label ID="lbl_Sp" runat="server"></asp:Label></strong></p>
            </div>
            <div class="col-sm-3">
                <label>From Date</label><p><strong class="text-blue"><asp:Label ID="lbl_FromDate" runat="server"></asp:Label></strong></p>
            </div>
            <div class="col-sm-3">
                <label>To Date</label><p><strong class="text-blue"><asp:Label ID="lbl_ToDate" runat="server"></asp:Label></strong></p>
            </div>
        </div>

        <p></p>
        <div id="repDiv" runat="server" visible="false">
                 <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Planned Customers" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Visited Customers" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="0 Billed Customers" runat="server" >
                        </telerik:RadTab>
                       <%--   <telerik:RadTab Text="Missed Visits" runat="server" Visible ="false"  >
                        </telerik:RadTab>--%>
                        

                    </Tabs>
                </telerik:RadTabStrip>

         <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       <telerik:RadPageView ID="RadPageView1" runat="server">
                  <p></p>
                           <div class="row">
         <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Customer No/Name</label>
                                                <asp:TextBox ID="txt_CustNo" runat="server"></asp:TextBox> <asp:Button  CssClass ="btn btn-sm btn-primary"  ID="SearchBtn" runat="server" Text="Search" /> <asp:Button  CssClass ="btn btn-sm btn-default"  ID="Btn_Clear" runat="server" Text="Clear" />
                                                 </div>
     </div>
                </div>                                          
 <div class="overflowx">
   
     
                        <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="8" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="8"  >
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="Customer">
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn uniqueName="VistedYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="VistedYES"
                                                                HeaderText="Visited" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgVisitedyes" runat="server"  Visible='<%# Bind("VistedYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgVisitedNo" runat="server"  Visible='<%# Bind("Vistedno")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridTemplateColumn uniqueName="BilledYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="BilledYES"
                                                                HeaderText="Billed" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgBilledyes" runat="server"  Visible='<%# Bind("BilledYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgBilledNo" runat="server"  Visible='<%# Bind("Billedno")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                 <ItemStyle HorizontalAlign="Center" />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visited" HeaderText="Visits"
                                                                  SortExpression ="Visited" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="billed" HeaderText="Success Visits"
                                                                  SortExpression ="billed" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                            </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
                                         </div>
                                             </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView2" runat="server">
                                                <p></p>
                                            <div class="row">
         <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Customer No/Name</label>
                                                <asp:TextBox ID="txt_CustNo_V" runat="server"></asp:TextBox> <asp:Button  CssClass ="btn btn-sm btn-primary"  ID="Btn_Seacrh_V" runat="server" Text="Search" /> <asp:Button  CssClass ="btn btn-sm btn-default"  ID="Btn_Cancel_V" runat="server" Text="Clear" />
                                                 </div>
     </div>
                </div>    
                                             <div class="overflowx">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRepVisited" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="8" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="8">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn uniqueName="PlannedYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="PlannedYES"
                                                                HeaderText="Planned" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgPlannedyes" runat="server"  Visible='<%# Bind("PlannedYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgPlannedNo" runat="server"  Visible='<%# Bind("PlannedNo")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridTemplateColumn uniqueName="BilledYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="BilledYES"
                                                                HeaderText="Billed" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgBilledyes" runat="server"  Visible='<%# Bind("BilledYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgBilledNo" runat="server"  Visible='<%# Bind("Billedno")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                 <ItemStyle HorizontalAlign="Center" />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Visited" HeaderText="Visits"
                                                                  SortExpression ="Visited" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right" />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="billed" HeaderText="Success Visits"
                                                                  SortExpression ="billed" >
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
                                </div>
                                             </telerik:RadPageView>
                                        <telerik:RadPageView ID="RadPageView3" runat="server">
                                            <p></p>
                                           
     <div class="row">
         <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Customer No/Name</label>
                                                <asp:TextBox ID="txt_CustNo_B" runat="server"></asp:TextBox> <asp:Button  CssClass ="btn btn-sm btn-primary"  ID="Btn_search_B" runat="server" Text="Search" /> <asp:Button  CssClass ="btn btn-sm btn-default"  ID="Btn_Clear_B" runat="server" Text="Clear" />
                                                 </div>
     </div>
                </div>    
      
                       <div class="overflowx">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat ="server"  >
                                                                  <telerik:RadGrid id="gvRepZeroBilled" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="8" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="8">
                                                        <Columns>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                                                                  SortExpression ="Customer" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                               <telerik:GridTemplateColumn uniqueName="PlannedYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="PlannedYES"
                                                                HeaderText="Planned" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgPlannedyes" runat="server"  Visible='<%# Bind("PlannedYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgPlannedNo" runat="server"  Visible='<%# Bind("PlannedNo")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                   <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridTemplateColumn uniqueName="VistedYES"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"   SortExpression ="VistedYES"
                                                                HeaderText="Visited" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImgVistedyes" runat="server"  Visible='<%# Bind("VistedYES")%>' ImageUrl="~/images/yes_icon.gif" ></asp:Image>
                                                                 <asp:Image ID="ImgVistedNo" runat="server"  Visible='<%# Bind("VistedNo")%>' ImageUrl="~/images/no_icon.gif" ></asp:Image>
                                                            </ItemTemplate>
                                                                 <ItemStyle HorizontalAlign="Center" />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                        </telerik:GridTemplateColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
                                                               </telerik:RadAjaxPanel>
                           </div>
                                                             
                                            </telerik:RadPageView>

         <%--      <telerik:RadPageView ID="RadPageView4" runat="server">
                                            <p></p>
                                           
     
      
                       <div class="overflowx">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat ="server"  >
                                                               <telerik:RadGrid ID="rgVisits" DataSourceID="sqlVisits" 
                            AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                             PageSize="10"  AllowPaging="false" runat="server" AllowFilteringByColumn="false" ShowFooter ="true"
                            GridLines="None">

                   
                           <ClientSettings EnableRowHoverStyle="true">
           
</ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" ShowGroupFooter ="true"  TableLayout="Fixed" Width="100%" GridLines="None" BorderColor="LightGray"
                                DataSourceID="sqlVisits"
                                 PageSize="10" >
                                
                                        <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField  FieldAlias="Date"   HeaderValueSeparator=" : "    FieldName="PDate"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField  FieldAlias="Visits"   HeaderValueSeparator=" : "   Aggregate ="Count"  FieldName="PDate"></telerik:GridGroupByField>

                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="PlannedDate"       SortOrder="Descending"></telerik:GridGroupByField>
                                                        
                                                    </GroupByFields>
                                                    
                                                </telerik:GridGroupByExpression>
                                            
                                            </GroupByExpressions>

                                <PagerStyle Mode="NextPrevAndNumeric"  AlwaysVisible="true"></PagerStyle>

                                <Columns>
                                     <telerik:GridBoundColumn UniqueName="Customer"  Aggregate ="Count"  FooterText ="Total Visits :"
                                        
                                        SortExpression="Customer" HeaderText="Customer" DataField="Customer"
                                        ShowFilterIcon="false">
                                        <HeaderStyle  Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                   

                             

                                   
                                   
                                
                                </Columns>

















                            </MasterTableView>


                        </telerik:RadGrid>
                        <asp:SqlDataSource ID="sqlVisits" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                            SelectCommand="Rep_GetMissedVisitsByVan" SelectCommandType="StoredProcedure">

                            <SelectParameters>


                                <asp:ControlParameter Name="OID" ControlID="hfOrg" Type="String" DefaultValue ="0"  />
                                <asp:ControlParameter Name="SID" ControlID="HSID" Type="String" DefaultValue ="0"  />
                                   <asp:ControlParameter Name="Dat" ControlID="hfSMonth" Type="String" DefaultValue ="01-01-1900"  />
                                <asp:ControlParameter Name="Dat1" ControlID="hfEMonth" Type="String" DefaultValue ="01-01-1900"  />
                            </SelectParameters>
                            </asp:SqlDataSource> 
                                                               </telerik:RadAjaxPanel>
                           </div>
                                                             
                                            </telerik:RadPageView>--%>
                      </telerik:RadMultiPage>
        </div>
        </div>
    </div>
    </form>
</body>
</html>
