<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepPrincipal-SalesManWiseSales.aspx.vb" Inherits="SalesWorx_BO.RepPrincipal_SalesManWiseSales" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
     <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }  
   .k-chart svg{
	margin:0 -14px;
}    
</style>
    <script>
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
        function OpenSalesViewWindow(cid, Agency) {

            var URL


            URL = 'RepDetails.aspx?Type=PrincipalWiseSalesReturn&ReportName=PrincipalWiseSalesReturn_Dtl&RType=S&SID=' + cid + '&Org=' + document.getElementById('<%= HORGID.ClientID%>').value + '&From=' + document.getElementById('<%= HDate.ClientID%>').value + '&To=' + document.getElementById('<%= HToDate.ClientID%>').value + '&Agency=' + Agency

            var oWnd = radopen(URL, null);
            oWnd.SetSize(970, 580);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        function OpenReturnsViewWindow(cid, Agency) {

            var URL


            URL = 'RepDetails.aspx?Type=PrincipalWiseSalesReturn&ReportName=PrincipalWiseSalesReturn_Dtl&RType=R&SID=' + cid + '&Org=' + document.getElementById('<%= HORGID.ClientID%>').value + '&From=' + document.getElementById('<%= HDate.ClientID%>').value + '&To=' + document.getElementById('<%= HToDate.ClientID%>').value + '&Agency=' + Agency

            var oWnd = radopen(URL, null);
            oWnd.SetSize(970, 580);
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

    </script>
    </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Sales By Principal/Sales Man </h4>
	
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

   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                         <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization <em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Principal</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Principal"  ID="ddlAgency" Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div> 
             
              <div class="col-sm-4">
                                            <div class="form-group"><label>Van/fsr </label>
                              
             
                 <telerik:RadComboBox ID="ddlVan" Width="100%" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
               Skin="Simple"   EmptyMessage="Select Van/FSR" Filter="Contains"  >
                       </telerik:RadComboBox>
               </div>
               </div>
                                                    </div>

           <div class="row">
                    <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
</div>

                                                        <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label>To Date</label>
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
              <p><strong>Principal: </strong> <asp:Label ID="lbl_Principle" runat="server" Text=""></asp:Label></p>
             <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>     
              <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>            
              <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>     
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
</div>

             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                PageSize="12" AllowPaging="True" runat="server" width="100%"
                                GridLines="None" >
                                                       
                                                        <GroupingSettings IgnorePagingForGroupAggregates="true" CaseSensitive="false"  GroupContinuesFormatString=""></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="12">
                    <Columns>

                         
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Agency" HeaderText="Agency"
                                                                  SortExpression ="Agency" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                          
                         <telerik:GridTemplateColumn uniqueName="Sqty"  HeaderStyle-HorizontalAlign="Center"    HeaderStyle-Wrap="false" DataField="Sqty" SortExpression ="Sqty"
                                                                HeaderText="Sales Qty" ItemStyle-HorizontalAlign="Right" >
                                                            <ItemTemplate >
                                                                <asp:LinkButton ID="Lnk_RefIDS" runat="server" Text='<%# Bind("Sqty", "{0:##,0.00}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenSalesViewWindow(""{0}"",""{1}"");", Eval("salesRep_ID"), Eval("Agency"))%>' ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="SValue" HeaderText="Sales Value"
                                                                  SortExpression ="SValue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>

                         <telerik:GridTemplateColumn uniqueName="Rqty"  HeaderStyle-HorizontalAlign="Center"    HeaderStyle-Wrap="false" DataField="Rqty" SortExpression ="Rqty"
                                                                HeaderText="Returns Qty" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Lnk_RefIDR" runat="server" Text='<%# Bind("Rqty", "{0:##,0.00}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenReturnsViewWindow(""{0}"",""{1}"");", Eval("salesRep_ID"), Eval("Agency"))%>'   ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                       

                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="RValue" HeaderText="Returns Value"
                                                                  SortExpression ="RValue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="RetPerc" HeaderText="Return %"
                                                                  SortExpression ="RetPerc" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                 <HeaderStyle HorizontalAlign="Center" />

                                                           </telerik:GridBoundColumn>
                    </Columns>

                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                 <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField  HeaderText="Van/FSR" FieldName="Salesrep_name"   
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
               <asp:HiddenField runat="server" id="Hcurrency"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HORGID"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HDate"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HToDate"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HCID"></asp:HiddenField>
            <asp:HiddenField runat="server" id="HFSRID"></asp:HiddenField>
            <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

 </ContentTemplate> </asp:UpdatePanel> 
  
    
       <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>

   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
</asp:Content>
