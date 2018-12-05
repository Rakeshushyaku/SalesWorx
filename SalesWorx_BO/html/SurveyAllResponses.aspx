<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="SurveyAllResponses.aspx.vb" Inherits="SalesWorx_BO.SurveyAllResponses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
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
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4><a href="RepSurveyStatistics.aspx?b=1" title="Back"><i class="fa fa-arrow-circle-o-left"></i></a>All Responses</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <asp:HiddenField ID="hfSurveyID" runat="server" />
    <asp:HiddenField ID="hfOrg" runat="server" />
    <asp:HiddenField ID="hfType" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                   <div class="col-sm-12 text-right">
                        <h5><a href="javascript:clickExportExcel()" class="text-blue">Export Excel <i class="fa fa-file-excel-o text-success"></i></a>
                        <a href="javascript:clickExportPDF()" class="text-blue">Export PDF <i class="fa fa-file-pdf-o text-danger"></i></a></h5>
                    </div>


                <div id="Details" runat="server" visible="false" class="empdetailsblk">
             <div class="row">
            <div class="col-sm-3">Survey <strong><asp:Label ID="lbl_Survey1" runat="server" Text=""></asp:Label></strong></div>
            <div class="col-sm-3">Start Date <strong><asp:Label ID="lbl_startDate" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">End Date <strong><asp:Label ID="lbl_EndDate" runat="server" Text=""></asp:Label></strong></div> 
        </div>
                  </div>
                              <div class="table-responsive">
                                   
                                     
                                 <telerik:RadGrid id="gvRep"  AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       <PagerStyle PageSizeControlType="None" />
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Survey_Timestamp"  
                                                                  SortExpression ="Survey_Timestamp" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Question_Text" HeaderText="Question"
                                                                  SortExpression ="Question_Text" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Response" HeaderText="Response"
                                                                  SortExpression ="Response" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Customer" FieldName="Customer"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="Customer" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>
                                                          
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>


                                     <telerik:RadGrid id="gvAudit" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Survey_Timestamp"  
                                                                  SortExpression ="Survey_Timestamp" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Question_Text" HeaderText="Question"
                                                                  SortExpression ="Question_Text" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Response" HeaderText="Response"
                                                                  SortExpression ="Response" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                             
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <GroupByExpressions>

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
                                                          
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
            </div>

            </contenttemplate>
    </asp:UpdatePanel>

     <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportPDF" runat="server" Text="Export" />

</asp:Content>
