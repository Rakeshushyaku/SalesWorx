<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DelayedTrendByVan.aspx.vb" Inherits="SalesWorx_BO.Rep_DelayedTrendByVan" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
<style>
         input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
         

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

     
    
    </style>
    <script type="text/javascript">
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
        function alertCallBackFn(arg) {

        }

        function OnClientDropDownClosedHandler(sender, eventArgs) {
           $("#MainContent_rpbFilter_i0_dummyOrgBtn").click()
        }

        function OpenViewWindow(cid) {
            var URL
           // URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        document.onkeydown = function (evt) {

            evt = evt || window.event;
            if (evt.keyCode == 27) {

                HideRadWindow();
            }
        };

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }


        }

    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
        <h4>Delayed Trend By Van/FSR</h4>
<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
</telerik:RadWindowManager>



    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                          <ContentTemplate >
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                        <div class="col-sm-4" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID" OnClientDropDownClosed="OnClientDropDownClosedHandler" AutoPostBack="false" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Sales Org"  EnableCheckAllItemsCheckBox="true"  CheckBoxes="true"   ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" OnClientDropDownClosed="OnClientDropDownClosedHandler" AutoPostBack="false" >
                                        </telerik:RadComboBox>
                                                <asp:Button ID="dummyOrgBtn" runat="server" Style="display:none" />
                                            </div>
                                          </div>
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Manager</label>
                                                  <telerik:RadComboBox  Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"  CheckBoxes="true" EmptyMessage="Select Manager" ID="ddManager" Width ="100%" runat="server" DataTextField="Username" DataValueField="User_ID" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Location</label>
                                                <telerik:RadComboBox  Filter="Contains" Skin="Simple" EnableCheckAllItemsCheckBox="true"  CheckBoxes="true" EmptyMessage="Select Location" ID="ddlLocation" Width ="100%" runat="server" DataTextField="Description" DataValueField="Sales_District_ID" >
                                                 </telerik:RadComboBox>
                                            </div>
                                          </div>
                                        
                                         
                                                 <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>Month</label>
                                                        

                                                        <telerik:RadMonthYearPicker RenderMode="Lightweight" Width="100%" ID="txtFromDate" runat="server" Skin="Simple">
                                                            <DateInput ReadOnly="true" DisplayDateFormat="MMM-yyyy">
                                                        </DateInput>
                                                         
                                                        </telerik:RadMonthYearPicker>
                                            </div>
                                              <%--         <telerik:RadMonthYearPicker ID="txtFromDate"   runat="server" >
                                                        <DateInput runat="server" ReadOnly="true" DisplayDateFormat="MMM yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadMonthYearPicker>--%>
                                                    </div>
                                                 </div>
                                  
                                               
                                          </div>

                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
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
              <p><strong>Sales Manger: </strong> <asp:Label ID="lbl_Manager" runat="server" Text=""></asp:Label></p>
              <p><strong>Location: </strong><asp:Label ID="lbl_Loc" runat="server" Text=""></asp:Label></p>  
              <p><strong>Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              
            </span>
            </i>      
        </div>
    </div>

                              <div id="summary" runat="server" class="row"></div>
                              <br />
                              
                            

<div class="row">
    <div class="col-sm-8 col-md-8 col-lg-6">
        <div class="table-responsive">
                 <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server"  
                                GridLines="None" >
                                       
                                                        <GroupingSettings CaseSensitive="false"  >
                                                            

                                                        </GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                   

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR" ItemStyle-Width="65%"
                                                                  SortExpression ="SalesRep_Name" DataType="System.String" >
                                                                <ItemStyle Wrap="True"    />
                                                            </telerik:GridBoundColumn>


                                                 <%--              <telerik:GridBoundColumn Aggregate="Sum" DataField="Amount"  ItemStyle-Width="30%" SortExpression ="Amount"  DataFormatString="{0:G17}" 
                                                                    FooterAggregateFormatString ="Total : {0:G17}" 
                                                                         FooterText="Total : "  HeaderText="Delayed Amount">
                                                                     <ItemStyle Wrap="False"  HorizontalAlign="Right"   />
                                                                     <FooterStyle  Wrap="False"  HorizontalAlign="Right" />
                                                                </telerik:GridBoundColumn>--%>

                                                            <telerik:GridTemplateColumn Aggregate="Sum" DataField="Amount"  ItemStyle-Width="30%" SortExpression ="Amount"  
                                                                   
                                                                         FooterText="Total : "  HeaderText="Delayed Amount">
                                                                     <ItemStyle Wrap="False"  HorizontalAlign="Right"   />
                                                                     <FooterStyle  Wrap="False"  HorizontalAlign="Right" />
                                                                 <ItemTemplate>
                                                                      <asp:Label Text='<%# Bind("Amount")%>' runat="server" ID="lblAmount"    />  
                                                                      <asp:Label Text='<%# Bind("Currency_Code")%>' Visible ="false"  runat="server" ID="Label1"  />   
                                                                      <asp:Label Text='<%# Bind("Decimal_Digits")%>' Visible ="false" runat="server" ID="lblDigits" />                                                              
                                                                </ItemTemplate>

                                                                </telerik:GridTemplateColumn>

                                                         <%--   <telerik:GridTemplateColumn SortExpression="Address" UniqueName="TemplateColumn" HeaderText="" ItemStyle-Width="0%" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("Currency_Code")%>' runat="server" ID="lblCCode" />
                                                                    <asp:Label Text='<%# Bind("Decimal_Digits")%>' runat="server" ID="lblDigits" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>
                                                        
                                                        </Columns>
                                <GroupByExpressions>

                                     <telerik:GridGroupByExpression>
                                        <SelectFields>
                                            <telerik:GridGroupByField FieldAlias="Manager" FieldName="USERNAME" 
                                               ></telerik:GridGroupByField>
                                        </SelectFields>
                                             <GroupByFields>
                                                 <telerik:GridGroupByField FieldName="USERNAME"></telerik:GridGroupByField>
                                            </GroupByFields>
                                    </telerik:GridGroupByExpression>
                                       
                                  </GroupByExpressions>

                                                        </MasterTableView>
                                                    </telerik:RadGrid>
          </div>                 
    </div>
                        </div>   

                  
                                       <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>             
  


                          </ContentTemplate>
            
        </asp:UpdatePanel> 
                           
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
           

   <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress> 


</asp:Content>
