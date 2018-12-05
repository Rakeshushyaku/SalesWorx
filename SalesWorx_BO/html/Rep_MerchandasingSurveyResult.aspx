<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"  CodeBehind="Rep_MerchandasingSurveyResult.aspx.vb" Inherits="SalesWorx_BO.Rep_MerchandasingSurveyResult" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .FirstGroupHead td {
    background-color: #8cb0c5 !Important;
    color: #fff !Important;
}
        .FirstGroupHead td p {
    display: inline;
    margin: 0;
    padding: 0 10px;
    color: #000 !Important;
    font-weight: bold !Important;
}
    </style>
    <script>
        function clickExportBiffExcel() {
            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#MainContent_BtnExportPDF").click()
            return false
        }
        function clickExportExcel() {
            $("#MainContent_BtnExportExcel").click()
            return false

        }
        
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4><asp:HyperLink NavigateUrl="Rep_MerchandasingSurveyResp.aspx" runat="server" ID="hyp_back"  text="Back" ><i class="fa fa-arrow-circle-o-left"></i></asp:HyperLink>Survey Response</h4>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
        
 </telerik:RadWindowManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>

      <div id="summary" runat="server">
              
              
                     <div class="text-right">
                                
                             <a href="javascript:clickExportBiffExcel()" class="btn btn-sm btn-success"><i class="fa fa-file-excel-o"></i> Export Excel</a>
                             <a href="javascript:clickExportPDF()" class="btn btn-sm btn-danger"><i class="fa fa-file-pdf-o"></i> Export PDF</a>
                    </div>
             

           <div class="row">  
                 <div class="col-sm-4">
                    <label>Survey</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_Survey" runat="server"></asp:Label></p>
                </div>             
                 <div class="col-sm-4">
                    <label>Van</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_Van" runat="server"></asp:Label></p>
                </div>
                 
                 <div class="col-sm-4">
                    <label>Surveyed At</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_Date" runat="server"></asp:Label></p>
                </div>
              </div>

              <div class="row">
                 <div class="col-sm-4">
                    <label>Customer Name</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_CusName" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Customer No</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_CusNo" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label> </label>
                    
                </div>
              </div>

            </div> 
      <div class="table-responsive">
                                   
                                     
                                 <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="25" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="25">
                                                        <Columns>
                                                             
                                                            <telerik:GridTemplateColumn uniqueName="Question_txt"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Question_txt" SortExpression ="Question_txt"
                                                                        HeaderText="Question" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuestion_Text"  ForeColor="Brown"  runat="server" Text='<%# Bind("Question_txt")%>' ></asp:Label><br />
                                                                        
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            
                                                             <telerik:GridTemplateColumn uniqueName="Response"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Response" SortExpression ="Response"
                                                                        HeaderText="Response" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblResp" runat="server" Text='<%# Bind("Response")%>' Visible='<%# Bind("ShowLable")%>'></asp:Label>
                                                                         <div id="imgdiv1" runat="server" Visible='<%# Bind("ShowImage")%>'>
                                                                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("fval1")%>' Visible='<%# Bind("ShowImage")%>' Font-Bold="true"></asp:Label>
                                                                      </div>
                                                                         <div id="imgdiv2" runat="server" Visible='<%# Bind("ShowImage")%>'>
                                                                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("fval2")%>' Visible='<%# Bind("ShowImage")%>' Font-Bold="true" ></asp:Label>
                                                                        
                                                                        </div>
                                                                        <asp:HyperLink ID="hyp_Img" runat="server" Target="_blank" NavigateUrl='<%# Bind("Response1")%>' ImageUrl='<%# Bind("Response1")%>' Visible='<%# Bind("ShowImage")%>' ImageWidth="50px" ImageHeight="50px" >
                                                                            </asp:HyperLink>
                                                                       
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Comment" HeaderText="Comments"
                                                                  SortExpression ="Comment" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <GroupByExpressions>
                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields >
                                                                <telerik:GridGroupByField FieldAlias="Batch" FieldName="Batch"   
                                                                  HeaderText=" " HeaderValueSeparator=" "    ></telerik:GridGroupByField>

                                                                 
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="Batch_ID"  >

                                                                     </telerik:GridGroupByField>
                                                                     
                                                                      
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                

                                                                <telerik:GridGroupByField FieldAlias="group_text" FieldName="group_text"   
                                                                  HeaderText=" " HeaderValueSeparator=" " SortOrder="None"  ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     
                                                                    <telerik:GridGroupByField FieldName="GroupID"    >

                                                                     </telerik:GridGroupByField>
                                                                      
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>
                                                          
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>
            </div>
             </contenttemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
          
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
     <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportPDF" runat="server" Text="Export" />
     <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportBiffExcel" runat="server" Text="Export"  />
</asp:Content>
