<%@ Page Title="Ageing Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_Ageing.aspx.vb" Inherits="SalesWorx_BO.Rep_Ageing" %>
 
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
     <script>
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

         function OpenViewWindow(cid) {
             var URL
             //URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
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

         function RefreshChart() {
             createChart3();
         }
    </script>
    <style>
    div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
     </asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
   <h4>Ageing Report</h4>
	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
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

     <telerik:RadAjaxPanel ID="l" runat="server">
 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
            <td> <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                  </div>
                                          </div>
                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_Customer" Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type Customer No./Name"
  EnableLoadOnDemand="True" 
                                                                 Width="100%"  AutoPostBack="true" />
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
                 <p><strong>Customer: </strong> <asp:Label ID="lbl_customer" runat="server" Text=""></asp:Label></p>
                
            </span>
            </i>      
        </div>
    </div>

                              <div id="summary" runat="server" class="row"></div>

           <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
</div>

                           <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer" SortExpression ="Customer"
                                                               >
                                                                <ItemStyle HorizontalAlign ="Left" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Bill_Credit_Period" HeaderText="Credit Period"
                                                                  SortExpression ="Bill_Credit_Period" >
                                                              <ItemStyle HorizontalAlign ="right" Wrap="False" />
                                                                  <HeaderStyle HorizontalAlign ="Left" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CRL" HeaderText="Credit Limit"
                                                                  SortExpression ="CRL" >
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                           
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="totOutAmt" HeaderText="Total Outstanding"
                                                                  SortExpression ="totOutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="PriorOutAmt" HeaderText="Prior(if any)"
                                                                  SortExpression ="PriorOutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                     <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Last3OutAmt" HeaderText=""
                                                                  SortExpression ="Last3OutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                     <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Last2OutAmt" HeaderText=""
                                                                  SortExpression ="Last2OutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>

                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastOutAmt" HeaderText=""
                                                                  SortExpression ="LastOutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                    <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CurOutAmt" HeaderText=""
                                                                  SortExpression ="CurOutAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                     <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign ="Center" Wrap="true" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>   
         
         <asp:HiddenField ID="HCurrency" runat="server" />          
 <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
                              
                        

                         </telerik:RadAjaxPanel>
    
         <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
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

