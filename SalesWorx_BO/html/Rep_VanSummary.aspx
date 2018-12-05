<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_VanSummary.aspx.vb" Inherits="SalesWorx_BO.Rep_VanSummary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FeaturedContent" runat="server">
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

         function OpenSoldViewWindow(cid) {

             var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
             var param2 = document.getElementById('<%= HVan.ClientID%>').value;
             var param3 = document.getElementById('<%= Hfrom.ClientID%>').value;
             var param4 = document.getElementById('<%= HTo.ClientID%>').value;
             var param5 = document.getElementById('<%= HUID.ClientID%>').value;
              
             var URL
             URL = 'RepDetails.aspx?Type=SoldbySKU&ReportName=SKUwiseSalesQtyValue&ID=' + cid + '&From=' + param3 + '&To=' + param4 + '&Org=' + param1 + '&SPID=' + param2 + '&UID=' + param5;
           
             var oWnd = radopen(URL, null);
             oWnd.SetSize(950, 600);
             oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)

             return false

         }

         function OpenReturnedViewWindow(cid) {

             var param1 = document.getElementById('<%= HorgID.ClientID%>').value;
             var param2 = document.getElementById('<%= HVan.ClientID%>').value;
             var param3 = document.getElementById('<%= Hfrom.ClientID%>').value;
             var param4 = document.getElementById('<%= HTo.ClientID%>').value;
             var param5 = document.getElementById('<%= HUID.ClientID%>').value;

             var URL
             URL = 'RepDetails.aspx?Type=ReturnbySKU&ReportName=SKUwiseReturnsQtyValue&ID=' + cid + '&From=' + param3 + '&To=' + param4 + '&Org=' + param1 + '&SPID=' + param2 + '&UID=' + param5;

             var oWnd = radopen(URL, null);
             oWnd.SetSize(950, 600);
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

         function NumericOnly(e) {

             var keycode;

             if (window.event) {
                 keycode = window.event.keyCode;
             } else if (e) {
                 keycode = e.which;
             }
             if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
                 return true;

             return false;
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
   <h4>Van/FSR Summary by Product</h4>
	
 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
            <asp:HiddenField runat="server" id="HUID"></asp:HiddenField>
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
                                                    <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van/FSR<em><span>&nbsp;</span>*</em></label>
                                                  <telerik:RadComboBox Skin="Simple" ID="ddl_Van"   Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                            </div>
                                          </div>

                                                    <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Unload Percentage (%)</label>
                                                 
                                                 <asp:TextBox ID="txt_Per" runat="server" CssClass="inputSM" Width="100%" onKeypress='return NumericOnly(event)'></asp:TextBox> 
                                            </div>
                                          </div>     

                                                    
                                                    <div class="col-sm-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate" Width="100%"  runat="server">
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
                                                        <label>To Date</label>
                                                          <telerik:RadDatePicker ID="txtToDate"  Width="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                  </div> 
                                                   <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Item</label>
                                                 
                                                <telerik:RadComboBox ID="ddl_item"   Skin="Simple"   runat="server"
                                                                Filter="Contains"  EmptyMessage="Please type product code/ name"
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
              <p><strong>Van: </strong><asp:Label ID="lbl_Van" runat="server" Text=""></asp:Label></p>
              <p><strong>Product: </strong> <asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>  
              <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>Unload Percentage: </strong> <asp:Label ID="lbl_Unload" runat="server" Text=""></asp:Label></p>  
            </span>
            </i>      
        </div>
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
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Product" SortExpression ="Product"
                                                               >
                                                                   <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                      
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="openingstk" HeaderText="Opening Stock"
                                                                  SortExpression ="openingstk" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="openingstkvalue" HeaderText="Opening Stock Value"
                                                                  SortExpression ="openingstkvalue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                              
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vanload" HeaderText="Loaded Qty"
                                                                  SortExpression ="vanload" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vanloadvalue" HeaderText="Loaded Value"
                                                                  SortExpression ="vanloadvalue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn uniqueName="sold"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="sold" SortExpression ="sold"
                                                                HeaderText="Sold Qty" >
                                                            <ItemTemplate >
                                                               <asp:LinkButton ID="Lnk_Sold" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "sold", "{0:##,##0.00}")%>'    ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenSoldViewWindow(""{0}"");" , Eval("Inventory_Item_ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                               <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                        </telerik:GridTemplateColumn>

                                                            

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="soldvalue" HeaderText="Sold Value"
                                                                  SortExpression ="soldvalue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridTemplateColumn uniqueName="returned"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="returned" SortExpression ="returned"
                                                                HeaderText="Returned Qty" >
                                                            <ItemTemplate >
                                                               <asp:LinkButton ID="Lnk_Sold" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "returned", "{0:##,##0.00}")%>'  ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenReturnedViewWindow(""{0}"");" , Eval("Inventory_Item_ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                        </telerik:GridTemplateColumn>

                                                         
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="returnedvalue" HeaderText="Returned Value"
                                                                  SortExpression ="returnedvalue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vanUnload" HeaderText="Unloaded Qty"
                                                                  SortExpression ="vanUnload" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="vanUnloadvalue" HeaderText="Unloaded Value"
                                                                  SortExpression ="vanUnloadvalue" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>   

              <asp:HiddenField ID="HTo" runat="server" />
         <asp:HiddenField ID="Hfrom" runat="server" />
         <asp:HiddenField ID="HVan" runat="server" />
         <asp:HiddenField ID="HorgID" runat="server" />

 </ContentTemplate> </asp:UpdatePanel> 
  
     <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
   </div>
   
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       

</asp:Content>
