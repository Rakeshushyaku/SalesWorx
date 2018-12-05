<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepCustomerListing.aspx.vb" Inherits="SalesWorx_BO.RepCustomerListing" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
}

          
    
    </style>

    <script>

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;

        function InitializeRequest(sender, args) {
            $(".rpTemplate .form-group").addClass("form-disabled");
        }

        function EndRequest(sender, args) {
           // $("#ctl00_MainContent_rpbFilter_i0_SearchBtn").removeAttr("disabled");
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
        function alertCallBackFn(arg) {

        }

        function OpenViewWindow(cid) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            URL = 'RepDetails.aspx?OrgID=' + combo.get_selectedItem().get_value() + '&Type=Cust&ReportName=CustomerDetails&ID=' + cid;
            var oWnd = radopen(URL, "winDetail");
            oWnd.SetSize(880, 500);
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
        function OpenLocWindow(Lat, Long) {

            var URL
            URL = 'ShowMap.aspx?Lat=' + Lat + '&Long=' + Long;

            var oWnd = radopen(URL, null);
            oWnd.SetSize(940, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }


    </script>
     <script type="text/javascript">
         $(window).resize(function () {
             var win = $find('winDetail');
                if (win) {
                    if (!win.isClosed()) {
                        win.center();
                    }
                }

            });
     </script> 
<style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Customer Listing</h4>

       <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	 <telerik:RadWindow >

	 </telerik:RadWindow>
	  <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />

                </UpdatedControls>

            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>


 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
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
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true">
                </telerik:RadComboBox>
                                            </div>
                                          </div>
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer No</label>
                                                <asp:TextBox ID="txtCustomerNo" CssClass="inputSM" Width ="100%" runat="server"></asp:TextBox>
                                                </div>
                                             </div>
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Van"  Width ="100%"
                    runat="server" >
                </telerik:RadComboBox>
                                                 </div>
                                             </div>
                                                    </div>
                                                 <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Segment</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSegment" Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="Customer_Segment_ID">
                </telerik:RadComboBox>
                                                      </div>
                                             </div>

                                                      <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Sales District</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlSalesDist"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="Sales_District_ID">
                </telerik:RadComboBox>
                                                  </div>
                                             </div>

                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Channel</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Channel"  Width ="100%"
                    runat="server" >
                </telerik:RadComboBox>
                                                </div>
                                            </div>

                                                     </div>
                                                 <div class="row">
                                                      <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Name</label>
                                                <asp:TextBox ID="txtCustomerName" Width ="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                                 </div>
                                            </div>


                                                       <div class="col-sm-4">
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <div class="form-group">
                                                                    <label>Customer Status</label>
                                                                    <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlCust_Stat" CssClass="inputSM" Width ="100%" runat="server">
                            <Items>
                                        <telerik:RadComboBoxItem Value="0" text="All"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Active" ></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="2" Text="Blocked"></telerik:RadComboBoxItem>
                    
                                </Items>
                                    </telerik:RadComboBox>
                                                                    </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <div class="form-group">
                                                                <label>Customer Type</label>
                                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_CustType" CssClass="inputSM" Width ="100%" runat="server">
                            <Items>
                                        <telerik:RadComboBoxItem Value="0" text="All"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="Cash" ></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="2" Text="Credit"></telerik:RadComboBoxItem>
                    
                                </Items>
                                    </telerik:RadComboBox>
                                                                    </div>
                                                            </div>
                                                        </div>
                                                           </div>


                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Sub-Channel</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_SubChannel"  Width ="100%"
                    runat="server" >
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
                            
                              <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Organisation: </strong><asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
              <p><strong>Customer No.: </strong> <asp:Label ID="lbl_CustNo" runat="server" Text=""></asp:Label></p>
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>Segment: </strong><asp:Label ID="lbl_Segment" runat="server" Text=""></asp:Label></p>  
              <p><strong>Sales District: </strong><asp:Label ID="lbl_SalesDistrict" runat="server" Text=""></asp:Label></p>  
              <p><strong>Channel: </strong><asp:Label ID="lbl_Channel" runat="server" Text=""></asp:Label></p>  
              <p><strong>Customer Name: </strong> <asp:Label ID="lbl_CustName" runat="server" Text=""></asp:Label></p>    
              <p><strong>Customer Status: </strong> <asp:Label ID="lbl_Status" runat="server" Text=""></asp:Label></p>    
              <p><strong>Customer Type: </strong> <asp:Label ID="lbl_CustType" runat="server" Text=""></asp:Label></p>    
              <p><strong>Sub-Channel: </strong> <asp:Label ID="lbl_SubChannel" runat="server" Text=""></asp:Label></p> 
            </span>
            </i>      
        </div>
    </div>
                                
                              <div id="summary" runat="server" class="row"></div> 
                              <p><br /></p>
                              <div class="table-responsive">
                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="customer" SortExpression ="customer"
                                                                HeaderText="Customer" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Customer_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Site_Use_ID" runat="server" Text='<%# Bind("Customer")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format( "OpenViewWindow(""{0}"");" , Eval("ID") )%>'    ></asp:LinkButton>
                                                            </ItemTemplate >
                                                              <ItemStyle Wrap="True"  />
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             
                                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CustStatus" HeaderText="Customer </br> Status"
                                                                  SortExpression ="CustStatus" >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CustType" HeaderText="Customer </br> Type"
                                                                  SortExpression ="CustType" >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                             </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_District " HeaderText="Sales District"
                                                                  SortExpression ="Sales_District" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_Segment" HeaderText="Customer Segment"
                                                                  SortExpression ="Customer_Segment" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Location" HeaderText="Location"
                                                                  SortExpression ="Location" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Credit_Limit" HeaderText="Credit Limit"
                                                                  SortExpression ="Credit_Limit" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>


                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Bill_Credit_Period" HeaderText="Credit Period (in days)"
                                                                  SortExpression ="Bill_Credit_Period" >
                                                                <ItemStyle Wrap="False" Width="70px" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                                            </telerik:GridBoundColumn>

                                                          
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TrnNo" HeaderText="TRN Number"
                                                                  SortExpression ="TrnNo" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Payment_Term" HeaderText="Payment Term"
                                                                  SortExpression ="Payment_Term" UniqueName="Payment_Term" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn uniqueName="Van"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" 
                                                                HeaderText="Assigned Vans" HeaderStyle-ForeColor="SteelBlue"  >
                                                            <ItemTemplate>
                                                               
                                                                <asp:LinkButton ID="Van" runat="server"  CommandName="Van" Text="Van " ForeColor="SteelBlue" Font-Underline="true"   ></asp:LinkButton>
                                                                  <asp:Label ID="lblSite_Use_ID" runat="server" Visible ="false"  Text='<%# Bind("Site_Use_ID")%>'></asp:Label>
                                                                 <asp:Label ID="lblCustomer_ID" runat="server"  Visible ="false" Text='<%# Bind("Customer_ID")%>'></asp:Label>
                                                            </ItemTemplate >
                                                              <ItemStyle Wrap="True" Width="80"  />
                                                        </telerik:GridTemplateColumn>
                                                             <telerik:GridTemplateColumn  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" HeaderStyle-ForeColor="#0090d9" 
                                                                HeaderText="View" >
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="Lnk_RefID" runat="server" Text='View Location' ForeColor="SteelBlue" Font-Underline="true"  Visible ='<%# Bind("ViewLocIsVisible")%>'  OnClientClick='<%# String.Format("OpenLocWindow(""{0}"",""{1}"");", Eval("CustLat"), Eval("CustLng"))%>'    ></asp:LinkButton>--%>
                                                                <asp:Image style="cursor:pointer" ID="Lnk_RefID" ImageUrl="../images/location.png" runat="server" Visible ='<%# Bind("ViewLocIsVisible")%>'   OnClick='<%# String.Format("OpenLocWindow(""{0}"",""{1}"");", Eval("CustLat"), Eval("CustLng"))%>' ></asp:Image>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                  
                           

                              </div>

                               <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                    <asp:Label ID="lbl_Currency" runat="server" Text="N2"></asp:Label>
                                   </div>

                               <script type="text/javascript">
                                   $(window).resize(function () {
                                       var win = $find('<%=VanList.ClientID%>');
                                            if (win) {
                                                if (!win.isClosed()) {
                                                    win.center();
                                                }
                                            }

                                        });
                                    </script> 

                                  <telerik:RadWindow ID="VanList" Title= "Vans Assigned to Customer" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true"  ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>

                           <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
                                <ContentTemplate>

                             
                                    <div class="popupcontentblk">
                                        <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                   
                                          
                                <telerik:RadGrid id="gvVan" AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="260" BorderColor="LightGray"
                                PageSize="11" AllowPaging="false" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="360" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                                                                                                                                      
                                                             
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="left"  DataField="SalesRep_Name" HeaderText="Van"
                                                                   >
                                                                <ItemStyle Wrap="True" HorizontalAlign="left" Width="200" />
                                                             </telerik:GridBoundColumn>
                                                         
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                  
                           

                              


                                           
                                        </div>
                                        <asp:Panel ID="Panel12" runat="server" Style="display: none" CssClass="overlay">
                                                        <img alt="Processing..." src="../assets/img/ajax-loader.gif"  />
                                                        <span>Processing... </span>
                                                    </asp:Panel>
                                    </div>
                                      
                                      </ContentTemplate>
                                
                            </asp:UpdatePanel>
                                       </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                                  
 </ContentTemplate> </asp:UpdatePanel> 
       
  <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
                             
            
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
        
	
</asp:Content>
