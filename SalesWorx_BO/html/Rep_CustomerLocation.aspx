<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_CustomerLocation.aspx.vb" Inherits="SalesWorx_BO.Rep_CustomerLocation" %>


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
             
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 500);
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
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Customer Location Report</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	
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
                                                <label>Name</label>
                                               <asp:TextBox ID="txtCustomerName" Width ="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                                </div>
                                             </div>
                                            <div class="col-sm-4">
                                                <div class="row">
                                                    <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Customer No</label>
                                                 <asp:TextBox ID="txtCustomerNo" CssClass="inputSM" Width ="100%" runat="server"></asp:TextBox>
                                                 </div>
                                            </div>

                                                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Van"  Width ="100%"
                    runat="server" >
                </telerik:RadComboBox>
                                                 </div>
                                             </div>
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
                                                <label>Geo Codes</label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddl_Gps"  Width ="100%"
                    runat="server" >
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="All" Value="0" />
                                                        <telerik:RadComboBoxItem Text="With Geo Codes" Value="2" />
                                                       <telerik:RadComboBoxItem Text="Without Geo Codes" Value="1" />
                                                    </Items>
                </telerik:RadComboBox>
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
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"   ID="SearchBtn" runat="server" Text="Search" />
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
              <p><strong>Geo Codes: </strong> <asp:Label ID="lbl_Geocodes" runat="server" Text=""></asp:Label></p> 
            </span>
            </i>      
        </div>
    </div>

                              <div id="summary" runat="server" class="row">&nbsp;</div> 
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
                                                                <asp:LinkButton ID="Site_Use_ID" runat="server" Text='<%# Bind("Customer")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"");", Eval("ID"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                              <ItemStyle Wrap="True"  />
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Cash_Cust" HeaderText="Cash Customer" SortExpression ="Cash_Cust"
                                                               >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CustStatus" HeaderText="Customer Status"
                                                                  SortExpression ="CustStatus" >
                                                                <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                             </telerik:GridBoundColumn>
                                                             
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_District " HeaderText="Sales District"
                                                                  SortExpression ="Sales_District" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_Segment" HeaderText="Customer Segment"
                                                                  SortExpression ="Customer_Segment" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Location" HeaderText="Location"
                                                                  SortExpression ="Location" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>

                                                              
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
                              
                                   <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>     
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
