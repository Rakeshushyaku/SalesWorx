<%@ Page Title="Customer Survey" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepCustomerSurvey.aspx.vb" Inherits="SalesWorx_BO.RepCustomerSurvey" %>
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


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Customer Survey</h4>
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
                                                <label> Survey<em><span>&nbsp;</span>*</em> </label>
              
              <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSurvey"  Width ="100%"
                    runat="server" DataTextField="Survey_title" DataValueField="Survey_Id" AutoPostBack="true"> </telerik:RadComboBox>
                  </div>
                                             </div>
                                                    </div>
                      
                    <div class="row">
                                                     <div class="col-sm-6">
                                                       <div class="form-group">
                                                           <label>
            Customer</label>
                
                 <telerik:RadComboBox Skin="Simple" Width ="100%" ID="ddlCustomer" runat="server" AutoPostBack="false" DataTextField="Name" DataValueField="ID">                        
                     </telerik:RadComboBox>
              
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
      

       <div id="RepDiv" runat="server" visible="false" >
                        <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Survey: </strong> <asp:Label ID="lbl_Survey" runat="server" Text=""></asp:Label></p>
              <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>
              
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
            <div class="col-sm-3"><asp:Label ID="lbl_SurveyedTxt" runat="server" Text="Customers Surveyed"></asp:Label> <strong><asp:Label ID="lbl_CustSurveyed" runat="server" Text="" ></asp:Label></strong></div> 
        </div>
                  </div>
                              <div class="table-responsive">
                                   
                                     
                                 <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
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
            </div>
                            
           <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
               <asp:Label ID="lbl_currency" runat="server" Text="N2"></asp:Label>
                                   </div>
        </div>
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
                        </telerik:RadAjaxPanel>
  
   <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
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


</asp:Content>

