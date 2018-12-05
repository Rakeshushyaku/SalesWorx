<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepCustomerVisits.aspx.vb" Inherits="SalesWorx_BO.RepCustomerVisits" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

     <script>
         
        
         function OpenLocWindow(Lat, Long, CustLat, CustLong) {

             var URL
             URL = 'ShowMap.aspx?Lat=' + Lat + '&Long=' + Long+'&Type=Visits&CustLat=' + CustLat + '&CustLong=' + CustLong;

             var oWnd = radopen(URL, null);
             oWnd.SetSize(900, 600);
             oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)
             oWnd.moveTo(175, 10);
             return false

         }

         function OpenBeaconWindow(ID) {

             var URL
             URL = 'ShowBeaconDetails.aspx?ID=' + ID 

             var oWnd = radopen(URL, null);
             oWnd.SetSize(900, 400);
             oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
             oWnd.SetModal(true);
             oWnd.Center;
             oWnd.set_visibleStatusbar(false)
             oWnd.moveTo(175, 10);
             return false

         }

         
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

         function pageLoad(sender, args) {
             $('.rgMasterTable').find('th > a').attr("data-container", "body");
             $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
             $('[data-toggle="tooltip"]').tooltip();
         }

</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
 
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Customer Visits</h4>	
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
	
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>

                   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                         <div class="row">
                                        <div class="col-sm-10 col-md-10 col-lg-10">
            <div class="row">
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization <em><span>&nbsp;</span>*</em></label>
            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"   Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </telerik:RadComboBox>
             </div>
                                          </div>
                 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van/FSR </label>
                                                     <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
            
                     
                   </div>
                                          </div>

                <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer Type </label>
            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddl_CustType"   Width ="100%"
                    runat="server" AutoPostBack="false" >
                <Items>
                    <telerik:RadComboBoxItem Value="0" Text="All" Selected="true"  />
                    <telerik:RadComboBoxItem Value="Y" Text="Cash"/>
                    <telerik:RadComboBoxItem Value="N" Text="Credit"/>
                </Items>
                </telerik:RadComboBox>
                     
                   </div>
                                          </div>

 <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer </label>
                                                <telerik:RadComboBox EmptyMessage="Please type Customer No/ Name" Skin="Simple" AutoPostBack="true" EnableLoadOnDemand="true"  Filter="Contains" ID="ddlCustomer"   Width ="100%"
                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                </telerik:RadComboBox>  
              </div>
                                          </div>
          
           
          <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>From Date</label>
                                                <telerik:RadDatePicker ID="txtFromDate"   runat="server">
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
                                                             <telerik:RadDatePicker ID="txtToDate"   runat="server">
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>Customer: </strong><asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>  
              <p><strong>Customer Type: </strong><asp:Label ID="lbl_CustomerType" runat="server" Text=""></asp:Label></p>  
            </span>
            </i>      
        </div>
    </div>


            <div id="summary" runat="server" class="row"></div> 

            <p><br /></p>
              <div class="overflowx">
                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" Visible="false" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                            <PagerStyle PageSizeControlType="None" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>

                                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="VisitDate" HeaderText="Visit Date"
                                                                  SortExpression ="VisitDate" DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="false" />
                                                             </telerik:GridBoundColumn>  

                                                             <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Salesrep_name" HeaderText="Van/FSR"
                                                                  SortExpression ="Salesrep_name" >
                                                                <ItemStyle Wrap="false" />
                                                             </telerik:GridBoundColumn>  

                                                          <telerik:GridTemplateColumn uniqueName="Customer_Name"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer_Name" SortExpression ="Customer_Name"
                                                                HeaderText="Customer Name" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="HVisitDate" Value='<%# Bind("VisitDate")%>' />
                                                                <asp:HiddenField runat="server" ID="HVisitID" Value='<%# Bind("Actual_Visit_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HSID" Value='<%# Bind("SalesRep_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HSName" Value='<%# Bind("SalesRep_Name")%>' />
                                                                <asp:HiddenField runat="server" ID="HCusID" Value='<%# Bind("Customer_No")%>' />
                                                                <asp:HiddenField runat="server" ID="HSiteID" Value='<%# Bind("Site_Use_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HCusName" Value='<%# Bind("Customer_Name")%>' />
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Customer_Name")%>' ForeColor="SteelBlue" Font-Underline="true"  OnClick="ViewDetails_Click" Width="100%"  ></asp:LinkButton>
                                                            </ItemTemplate>
                                                              <ItemStyle Wrap="False" />
                                                        </telerik:GridTemplateColumn>

                                                       <%-- <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Customer_Name" HeaderText="Customer Name"
                                                                  SortExpression ="Customer_Name" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>   --%>                                                                                                 
                                                            
                                                             

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="City" HeaderText="City"
                                                                  SortExpression ="City" >
                                                                <ItemStyle Wrap="true" />
                                                             </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Visit_Start_Date" HeaderText="Start Time"
                                                                  SortExpression ="Visit_Start_Date" DataFormatString="{0:HH:mm}" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>    
                                                       
                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Visit_End_Date" HeaderText="End Time"
                                                                  SortExpression ="Visit_End_Date" DataFormatString="{0:HH:mm}">
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                                
                                       
                                                       <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top" Visible="true" HeaderTooltip="Location captured during the visit"  uniqueName="Location"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ShowLocation" SortExpression ="ShowLocation"
                                                                HeaderText="Location<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                               <a  href="#" onclick='<%# String.Format("javascript: OpenLocWindow(""{0}"",""{1}"",""{2}"",""{3}"");", Eval("Latitude"), Eval("Longitude"), Eval("Cust_Lat"), Eval("Cust_Long"))%>'><img src="../images/location.png" /> </a> 
                                                         
                                                            </ItemTemplate >
                                                           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>

                                                            
                                                        <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top"  uniqueName="DC" HeaderTooltip="Was a Distribution check is done?"   HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="DC" SortExpression ="DC"
                                                                HeaderText="DC<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="DCYes" runat="server" ImageUrl="~/images/yes_icon.gif"   />
                                                                <asp:Image ID="DCNo" runat="server" ImageUrl="~/images/no_icon.gif" />
                                                                <asp:HiddenField runat="server" ID="HfDC" Value='<%# Bind("DC")%>' />                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>
                                                        
                                                        <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top"  uniqueName="BD" HeaderTooltip="Was a beacon detected?"   HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="BD" SortExpression ="BD"
                                                                HeaderText="Beacon</br>Detected<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="BDYes" runat="server" ImageUrl="~/images/yes_icon.gif"   />
                                                             <a  href="#" onclick='<%# String.Format("javascript: OpenBeaconWindow(""{0}"");", Eval("Actual_Visit_ID"))%>'>   <asp:Image ID="BDMaybe" runat="server" ImageUrl="~/images/not_icon.gif"   /></a>
                                                                <asp:Image ID="BDNo" runat="server" ImageUrl="~/images/no_icon.gif" />
                                                                <asp:HiddenField runat="server" ID="HfCBD" Value='<%# Bind("CBD")%>' />  
                                                                <asp:HiddenField runat="server" ID="HfBD" Value='<%# Bind("BD")%>' />                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left"  DataField="InvCnt" HeaderText="Invoice Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="InvCnt" HeaderTooltip="Number of invoices created during visit" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Left" DataField="OrderAmt" HeaderText="Invoice Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="OrderAmt"  UniqueName="InvoiceValue" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all invoices created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   
                                                        
                                                        
                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="SoCnt" HeaderText="Sal. Ord. Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="SoCnt"   HeaderTooltip="Number of Presales orders created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="SoAmt" HeaderText="Sal. Ord. Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="SoAmt" UniqueName="SalesOrderValue" DataType="System.Decimal" DataFormatString="{0:N2}"  HeaderTooltip="Value of all presales orders created during visit" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   


                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" DataField="ProformaCnt" HeaderText="Proforma Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="ProformaCnt"  HeaderTooltip="Number of Proforma orders created" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"     ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="UnConfirmedOrderAmt" HeaderText="Proforma Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="UnConfirmedOrderAmt" UniqueName="ProformaValue" DataType="System.Decimal" DataFormatString="{0:N2}"  HeaderTooltip="Value of all proforma orders created">
                                                                <ItemStyle Wrap="False" />
                                                           </telerik:GridBoundColumn>  
                                                            
                                                            
                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="RMACnt" HeaderText="Return Count<i class='fa fa-info-circle'></i>"
                                                                 SortExpression ="RMACnt" HeaderTooltip="Number of Returns created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Left" DataField="RMA" HeaderText="Return Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="RMA" UniqueName="ReturnValue" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all returns created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>  
                                                             

                                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="CollectionCnt" HeaderText="Payment Count<i class='fa fa-info-circle'></i>"
                                                                 SortExpression ="CollectionCnt" HeaderTooltip="Number of Collection documents created" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn  HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="Payment" HeaderText="Payment Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Payment"  UniqueName="PaymentValue" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all the collection documents created">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>  
                                                             <telerik:GridTemplateColumn UniqueName="TradeLic" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"  
                            HeaderText="Trade License">
                            <ItemTemplate>
                                 <asp:HiddenField runat="server" ID="HTrdLic" Value='<%# Bind("TrdLicense")%>' />
                                <asp:LinkButton ID="Lnk_TradeID" runat="server" Text='View Trade License' ForeColor="SteelBlue" Font-Underline="true"  Visible='<%# Bind("TrdLicenseSh")%>' CommandName="TradeLic" ></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                                                        </Columns>
                                                        
                                                        
                            

                                                
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                                <telerik:RadGrid id="gvRep_grp" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" Visible="false"  >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                            <PagerStyle PageSizeControlType="None" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>

                                                             

                                                          <telerik:GridTemplateColumn uniqueName="Customer_Name"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer_Name" SortExpression ="Customer_Name"
                                                                HeaderText="Customer Name" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="HVisitDate" Value='<%# Bind("VisitDate")%>' />
                                                                <asp:HiddenField runat="server" ID="HVisitID" Value='<%# Bind("Actual_Visit_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HSID" Value='<%# Bind("SalesRep_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HSName" Value='<%# Bind("SalesRep_Name")%>' />
                                                                <asp:HiddenField runat="server" ID="HCusID" Value='<%# Bind("Customer_No")%>' />
                                                                <asp:HiddenField runat="server" ID="HSiteID" Value='<%# Bind("Site_Use_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HCusName" Value='<%# Bind("Customer_Name")%>' />
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Customer_Name")%>' ForeColor="SteelBlue" Font-Underline="true"  OnClick="ViewDetails_Click" Width="100%"  ></asp:LinkButton>
                                                            </ItemTemplate>
                                                              <ItemStyle Wrap="False" />
                                                        </telerik:GridTemplateColumn>

                                                       <%-- <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Customer_Name" HeaderText="Customer Name"
                                                                  SortExpression ="Customer_Name" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>   --%>                                                                                                 
                                                            
                                                             

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="City" HeaderText="City"
                                                                  SortExpression ="City" >
                                                                <ItemStyle Wrap="true" />
                                                             </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Visit_Start_Date" HeaderText="Start Time"
                                                                  SortExpression ="Visit_Start_Date" DataFormatString="{0:HH:mm}" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>    
                                                       
                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   HeaderStyle-HorizontalAlign="Left" DataField="Visit_End_Date" HeaderText="End Time"
                                                                  SortExpression ="Visit_End_Date" DataFormatString="{0:HH:mm}">
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                                
                                       
                                                       <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top" Visible="true" HeaderTooltip="Location captured during the visit"  uniqueName="Location"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="ShowLocation" SortExpression ="ShowLocation"
                                                                HeaderText="Location<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                               <a  href="#" onclick='<%# String.Format("javascript: OpenLocWindow(""{0}"",""{1}"",""{2}"",""{3}"");", Eval("Latitude"), Eval("Longitude"), Eval("Cust_Lat"), Eval("Cust_Long"))%>'><img src="../images/location.png" /> </a> 
                                                         
                                                            </ItemTemplate >
                                                           <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>

                                                            
                                                        <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top"  uniqueName="DC" HeaderTooltip="Was a Distribution check is done?"   HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="DC" SortExpression ="DC"
                                                                HeaderText="DC<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="DCYes" runat="server" ImageUrl="~/images/yes_icon.gif"   />
                                                                <asp:Image ID="DCNo" runat="server" ImageUrl="~/images/no_icon.gif" />
                                                                <asp:HiddenField runat="server" ID="HfDC" Value='<%# Bind("DC")%>' />                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>
                                                        
                                                        <telerik:GridTemplateColumn HeaderStyle-VerticalAlign="Top"  uniqueName="BD" HeaderTooltip="Was a beacon detected?"   HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="BD" SortExpression ="BD"
                                                                HeaderText="Beacon</br>Detected<i class='fa fa-info-circle'></i>" >
                                                            <ItemTemplate>
                                                                <asp:Image ID="BDYes" runat="server" ImageUrl="~/images/yes_icon.gif"   />
                                                             <a  href="#" onclick='<%# String.Format("javascript: OpenBeaconWindow(""{0}"");", Eval("Actual_Visit_ID"))%>'>   <asp:Image ID="BDMaybe" runat="server" ImageUrl="~/images/not_icon.gif"   /></a>
                                                                <asp:Image ID="BDNo" runat="server" ImageUrl="~/images/no_icon.gif" />
                                                                <asp:HiddenField runat="server" ID="HfCBD" Value='<%# Bind("CBD")%>' />  
                                                                <asp:HiddenField runat="server" ID="HfBD" Value='<%# Bind("BD")%>' />                                                               
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" cssClass="td-middle"  />
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" DataField="InvCnt" HeaderText="Invoice Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="InvCnt" HeaderTooltip="Number of invoices created during visit" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Left" DataField="OrderAmt" HeaderText="Invoice Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="OrderAmt" UniqueName="InvoiceValue_grp" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all invoices created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   
                                                        
                                                        
                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="SoCnt" HeaderText="Sal. Ord. Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="SoCnt"   HeaderTooltip="Number of Presales orders created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="SoAmt" HeaderText="Sal. Ord. Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="SoAmt" UniqueName="SalesOrderValue_grp" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all presales orders created during visit" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   


                                                        <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" DataField="ProformaCnt" HeaderText="Proforma Count<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="ProformaCnt"  HeaderTooltip="Number of Proforma orders created" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"     ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="UnConfirmedOrderAmt" HeaderText="Proforma Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="UnConfirmedOrderAmt"  UniqueName="ProformaValue_grp" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all proforma orders created">
                                                                <ItemStyle Wrap="False" />
                                                           </telerik:GridBoundColumn>  
                                                            
                                                            
                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="RMACnt" HeaderText="Return Count<i class='fa fa-info-circle'></i>"
                                                                 SortExpression ="RMACnt" HeaderTooltip="Number of Returns created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Left" DataField="RMA" HeaderText="Return Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="RMA"  UniqueName="ReturnValue_grp" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all returns created during visit">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>  
                                                             

                                                            <telerik:GridBoundColumn HeaderStyle-VerticalAlign="Top"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Left" DataField="CollectionCnt" HeaderText="Payment Count<i class='fa fa-info-circle'></i>"
                                                                 SortExpression ="CollectionCnt" HeaderTooltip="Number of Collection documents created" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>   

                                                         <telerik:GridBoundColumn  HeaderStyle-VerticalAlign="Top"   ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" DataField="Payment" HeaderText="Payment Value<i class='fa fa-info-circle'></i>"
                                                                  SortExpression ="Payment"  UniqueName="PaymentValue_grp" DataType="System.Decimal" DataFormatString="{0:N2}" HeaderTooltip="Value of all the collection documents created">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                           </telerik:GridBoundColumn>  
                                                             <telerik:GridTemplateColumn UniqueName="TradeLic" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"  
                            HeaderText="Trade License">
                            <ItemTemplate>
                                 <asp:HiddenField runat="server" ID="HTrdLic" Value='<%# Bind("TrdLicense")%>' />
                                <asp:LinkButton ID="Lnk_TradeID" runat="server" Text='View Trade License' ForeColor="SteelBlue" Font-Underline="true"  Visible='<%# Bind("TrdLicenseSh")%>' CommandName="TradeLic" ></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                                                        </Columns>
                                                        
                                                        
                            

                                                     <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="Visit" FieldName="VisitDate"    HeaderValueSeparator =" Date : "
                                                                FormatString="{0:dd-MMM-yyyy}"    ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="VisitDate" SortOrder="Descending"   >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
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

                              </div>   

            <asp:HiddenField id="hfCurrency"  runat="server"></asp:HiddenField>
            <asp:HiddenField id="hfDecimal"  runat="server"></asp:HiddenField>
              <div style="display: none">
                               <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                           </div>
            <telerik:RadWindow ID="Viewimage_Window" Title= "View Image" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                             width="650px" height="600px"   AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                                                   <asp:HiddenField ID="HidVal" runat="server" Value="-1" />
                                                  <div class="popupcontentblk">
                                                    <p><asp:Label ID="lblPop"  runat ="server" ForeColor ="Red" ></asp:Label></p>
                                                    <div class="row">
                                                     <asp:Image id="vimg" runat="server" AlternateText="" ImageAlign="left" width="630px" height="509px"/>
                                                    </div>
                                                   

                                        
                                                      </div>
                                              </ContentTemplate>
                                                    </telerik:RadWindow> 
        </contenttemplate>
     </asp:UpdatePanel>

    
                            
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />            
           <span>Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>
