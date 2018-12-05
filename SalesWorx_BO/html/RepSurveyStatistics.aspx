<%@ Page Title="Survey Statistics" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepSurveyStatistics.aspx.vb" Inherits="SalesWorx_BO.RepSurveyStatistics" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
     <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

 div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   } 

 .rgGroupCol
{
    padding-left: 0 !important;
    padding-right: 0 !important;
}
 .RadGrid_Simple .rgMasterTable .rgHoveredRow {
        background: none !important;
    }
     .RadGrid_Simple .rgMasterTable .rgHoveredRow td {
        border-top: 0px solid transparent !important;
        border-left: 0px solid transparent !important;
        border-right: 0px solid #ddd !important;
        border-bottom: 0px solid #ddd !important;
    }
    .RadGrid_Simple .rgMasterTable .rgHoveredRow > td {
        border-top: 1px solid transparent !important;
        border-right: 1px solid #ddd !important;
        border-left: 1px solid transparent !important;
        border-bottom: 1px solid transparent !important;
    }
    .RadGrid_Simple .rgMasterTable .rgHoveredRow > td table td table td{
        border-top: 1px solid #ddd !important;
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    
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


        function ShowProgress() {
            document.getElementById('<% Response.Write(UpdateProgress2.ClientID)%>').style.display = "inline";
         }

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Survey Statistics</h4>
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

   <telerik:RadAjaxPanel runat="server" ID="g">
           
        <asp:HiddenField ID="HSID" runat="server" />
        <asp:HiddenField ID="hfSMonth" runat="server" />
        <asp:HiddenField ID="hfEMonth" runat="server" />
        <asp:HiddenField ID="hfOrg" runat="server" />
       <asp:HiddenField ID="HUID" runat="server" />
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
                                                <label> Survey <em><span>&nbsp;</span>*</em></label>
              
              <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSurvey"  Width ="100%"
                    runat="server" DataTextField="Survey_title" DataValueField="Survey_Id"  > </telerik:RadComboBox>
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
       <table>
          
            <tr>
                <td class="txtSMBold">
                <asp:Label ID="lblStartDatetxt" runat="server" Text="Start Date :" Visible="False"></asp:Label>
                    </td>
                <td class="txtSMBold">
                <asp:Label ID="lblStartDateval" runat="server" CssClass="inputSM" Visible="False"></asp:Label>
                </td>
                <td  align="left" class="txtSMBold"><asp:Label ID="lblEndDatetxt" runat="server" 
                        Text="End Date :" Visible="False"></asp:Label></td>
                <td  class="txtSMBold">
                    <asp:Label ID="lblEndDateval" runat="server" 
                        CssClass="inputSM" Visible="False"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>             
          
        </table>

       <div id="RepDiv" runat="server" visible="false" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Survey: </strong> <asp:Label ID="lbl_Survey" runat="server" Text=""></asp:Label></p>
                          
            </span>
            </i>      
        </div>
    </div>

                            
            <div id="summary" runat="server" class="row"></div>
              <div id="Details" runat="server" visible="false" class="empdetailsblk">
                  <div class="row">
            <div class="col-sm-3">Survey <strong><asp:Label ID="lbl_Survey1" runat="server" Text=""></asp:Label></strong></div>
            <div class="col-sm-3">Start Date <strong><asp:Label ID="lbl_startDate" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">End Date <strong><asp:Label ID="lbl_EndDate" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">Survey Count <strong><asp:Label ID="lbl_Count" runat="server" Text=""></asp:Label></strong></div>

             </div>

                  <div class="row">
                        <div class="col-sm-12 text-right" style="font-weight:500;"><asp:LinkButton ID="lnkView" runat="server" Text="View All Responses" ></asp:LinkButton></div>
                  </div>


            </div>
                              <div class="table-responsive">
                                   
                                     
                                 <telerik:RadGrid id="gvRep" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="1" AllowPaging="True" runat="server"  ExpandCollapseColumn-Display="false"
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings Resizing-AllowColumnResize="true" >
                                                    </ClientSettings>
                                                    <MasterTableView  ExpandCollapseColumn-Display="false" AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowHeader="false" 
                                                        PageSize="1">
                                                        <Columns>

                                                        <telerik:GridTemplateColumn>
                                                                <ItemTemplate>
                                                                   <asp:HiddenField ID="hfID" runat="server"  Value='<%# Eval("QuestionId")%>' />
                                                                    <div>
                                                                     <table width="100%" >
                                                                            <tr>
                                                                                <td style="width:50%" valign="top" align="left"  >
                                                                                    <div id="divTbl" runat="server" >
                                                                                         
                                                                                    </div>
                                                                                </td>   
                                                                                <td style="width:50%" valign="top" align="left"  >
                                                                                       <telerik:RadHtmlChart runat="server" Width="600px" Height="250px"  ID="surveyChart" Skin="Silk" PlotArea-XAxis-MajorGridLines-Visible ="false"
                                                                                      PlotArea-XAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MinorGridLines-Visible="false" PlotArea-YAxis-MajorGridLines-Visible="false"  >
                                                                                        <PlotArea>
                                                                                            <Series>
                                                                                              
                                                                                                <telerik:BarSeries   DataFieldY="ResponsePercentage" Name=" " >
                                                                                                    <TooltipsAppearance Color="White" />
                                                                                                    <Appearance Overlay-Gradient="None"></Appearance>
                                                                                                        <TooltipsAppearance Visible="true" BackgroundColor="Black" Color="White">
                                                                                                            <ClientTemplate>
                                                                                                                Response : #=dataItem.Response#
                                                                                                                <br />
                                                                                                                Percentage :   #=dataItem.ResponsePercentage#
                                                                                                            </ClientTemplate>
                                                                                                        </TooltipsAppearance>
                                                                                                </telerik:BarSeries>
                                                                                            </Series>
                                                                                          <XAxis DataLabelsField="Response">
                                                                                              
                                                                                                <TitleAppearance  Text=" ">
                                                                                                </TitleAppearance>
                                                                                                 <LabelsAppearance Visible="true"></LabelsAppearance>
                                                                                            </XAxis>

      
                                                                                          
                                                                                        </PlotArea>
                                                                                        <Legend>
                                                                                            <Appearance Visible="false">
                                                                                            </Appearance>
                                                                                        </Legend>
                                                                                        <ChartTitle Text="">
                                                                                        </ChartTitle>
                                                                                    </telerik:RadHtmlChart>
                                                                                </td>                                                                          
                                                                            </tr>

                                                                            <tr>
                                                                                <td style="width:100%" valign="top" align="left"  >
                                                                      
                                                                                 




                                                                                </td>                                                                             
                                                                            </tr>

                                                                        </table>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>


                                                           
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Q" FieldName="QuestionText"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="QuestionID" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>
                                                          
                                                                                            </MasterTableView>
                                                    </telerik:RadGrid>
            </div>
                            
           <div style="display:none">
                <asp:HiddenField ID="hfSurveyType" runat="server" />             
           </div>
        </div>
    
   <p><br /></p>
       <asp:UpdatePanel ID="pnlOtherRes" runat="server"  UpdateMode="Conditional"  >
           <ContentTemplate>
                      <div id="divOtherResponses" runat="server" visible="false" >
              <h3>Other Responses</h3>

            <telerik:RadGrid id="gvOtherResponse" AllowSorting="false" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="1" AllowPaging="True" runat="server"  ExpandCollapseColumn-Display="false"
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings>
                                                    </ClientSettings>
                                                    <MasterTableView  AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"  
                                                        PageSize="12">
                                                        <Columns>
                                                            <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Left" DataField="Respondent" HeaderText="Respondent"
                                                                  SortExpression ="Respondent" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>  
                                                     
                                                            <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Left" DataField="Response" HeaderText="Response"
                                                                  SortExpression ="Response" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>  
                                                           
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Question" FieldName="Question_Text"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="Question_ID" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                         
                                                    </GroupByExpressions>
                                                          
                                                                                            </MasterTableView>
                                                    </telerik:RadGrid>


       </div>

           </ContentTemplate>
       </asp:UpdatePanel>



   


       
      </telerik:RadAjaxPanel>
  
   <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export"  />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
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

    <script type="text/javascript" >
        $(document).ready(function () {
            //alert('Called');
            //$("#MainContent_UpdateProgress2").hide()
        });
    </script>

</asp:Content>

