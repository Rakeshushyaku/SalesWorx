<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_CustomerVisitDetail.aspx.vb" Inherits="SalesWorx_BO.Rep_CustomerVisitDetail" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function alertCallBackFn(arg) {

        }
        function OpenDialog(item) {


            //  alert(item.href);
            var img = $get("<%= rbImage.ClientID%>");
              img.src = item.href;
              var $ = $telerik.$;
              var mapWindow = $find("<%=MapWindow.ClientID%>");
            mapWindow.show();
        }
        function OpenViewWindow(cid, Refno, Ord_Type) {
                  
                var URL
              //  URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + $('#MainContent_hfOrg').val()
                URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + $('#MainContent_hfOrg').val() + '&Type=' + Ord_Type;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(900, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)

                return false

            }

        function OpenRetViewWindow(cid) {
         
            var URL
            //URL = 'RepDetails.aspx?Type=Return&ReportName=ReturnDetailsNew&ID=' + cid + '&OrgID=' + $('#MainContent_hfOrg').val()
            URL = 'Rep_ReturnDetails.aspx?ID=' + cid + '&OrgID=' + $('#MainContent_hfOrg').val() + '&Type=R'
            
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }


        function OpenDisViewWindow(cid) {

            var URL
            URL = 'RepDetails.aspx?Type=DisCheck&ReportName=DistributionCheckDetails&ID=' + cid
            var oWnd = radopen(URL, null);
            oWnd.SetSize(1000, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

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
    <style>
        label { color: #444; }


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4><a href="RepCustomerVisits.aspx?b=1"  title="Back"><i class="fa fa-arrow-circle-o-left"></i></a>Visit Details</h4>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <asp:HiddenField ID="hfVisitID" runat="server" />
    <asp:HiddenField ID="hfOrg" runat="server" />
    <asp:HiddenField ID="hfDecimal" runat="server" />
      <asp:HiddenField ID="hFromDate" runat="server" />
      <asp:HiddenField ID="hToDate" runat="server" />
    <div style="display: none">
                               <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                           </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            
            <div id="summary" runat="server">
                <telerik:RadWindow ID="MapWindow" Title="Trade License & Passport" runat="server" Behaviors="Move,Close"
            Width="850px" Height="550px" Skin="Windows7" ReloadOnShow="false" VisibleStatusbar="false" Modal="true" Overlay="true">
            <ContentTemplate>
            <div>
             <div style="float:left; width:17%;">

                <telerik:RadListView ID="TradeList"  runat="server" skin="Simple"
               
                    ItemPlaceholderID="TradeContainer" AllowPaging="true"
                    PageSize="5" Width ="98%"
                  >
       
            
         
            <ItemTemplate>
             
                <div style=" margin-top: 5px;margin-left:15px; padding: 2px; position: relative; background: #eeeeee;"
                   >
               
                             <a href='<%#Eval("TradePath")%>' style="font-weight:bold;"  onclick="OpenDialog(this);  return false">
                  
                     <telerik:radbinaryimage id="RadBinaryImage14"  runat="server" ToolTip ="Click here to full view" alternatetext="BinaryImage" ImageUrl='<%#Eval("TradePath")%>' 
                      width="120" height="90" resizemode="fit"     >
                      
                      </telerik:radbinaryimage>
                  </a> 
                 
           
                   
                </div>
             
            </ItemTemplate>
           
        </telerik:RadListView>
                 </div>
               
                  <div style ="float:right;height:500px;width:80%;" >
            <telerik:RadBinaryImage  runat="server" ID="rbImage"  Height="500px" 
                        AutoAdjustImageControlSize="true" ResizeMode="Fit" 
                        />
                  </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
              
                     <div class="text-right">
                        <p><a href="javascript:clickExportExcel()" class="btn btn-sm btn-success"><i class="fa fa-file-excel-o"></i> Export Excel</a>
                        <a href="javascript:clickExportPDF()" class="btn btn-sm btn-danger"><i class="fa fa-file-pdf-o"></i> Export PDF</a></p>
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
                    <label>City</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_City" runat="server"></asp:Label></p>
                </div>
              </div>

              <div class="row">  
                 <div class="col-sm-4">
                    <label>Credit Customer</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_CrCus" runat="server"></asp:Label></p>
                </div>             
                 <div class="col-sm-4">
                    <label>Cash Customer</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_CashCus" runat="server"></asp:Label></p>
                </div>
                 
                 <div class="col-sm-4">
                    <label>Visit At</label>
                     <p class="font-weight-600"><asp:Label ID="lbl_Date" runat="server"></asp:Label></p>
                </div>
              </div>

             <div class="row">
                <div class="col-sm-4">
                    <label>Started At</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_Start" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Ended At</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_End" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Visited By</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_Van" runat="server"></asp:Label></p>
                </div>
            </div>

            <div class="row">
               <div class="col-sm-4">
                   <label>Scanned</label>
                   <p class="font-weight-600"><asp:Label ID="lbl_Scanned" runat="server"></asp:Label></p>
               </div>
               <div class="col-sm-4">
                   <label>Odometer Reading</label>
                   <p class="font-weight-600"><asp:Label ID="lbl_Odo" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                   <label>Employee Name</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_Emp" runat="server"></asp:Label></p>
                </div>
            </div>

                <div class="row">
                   <div class="col-sm-4">
                    <label>Cash Customer Tel. No.</label>
                       <p class="font-weight-600"><asp:Label ID="lbl_CCTelNo" runat="server"></asp:Label></p>
                </div>   
                     <div class="col-sm-4">
                    <label>Reason for non productive visit</label>
                       <p class="font-weight-600"><asp:Label ID="lbl_ReasonNPV" runat="server"></asp:Label></p>
                </div>   
                    <div class="col-sm-4">
                    <label><asp:LinkButton ID="lbTradeImages" 
                            OnClick ="lbTradeImages_Click"  Font-Underline ="true" Font-Bold ="true"  ToolTip ="View Trade License & Passport images"  runat ="server" Text ="View Trade License & Passport" ></asp:LinkButton></label>
                       <p class="font-weight-600"></p>
                </div>                
              </div>
            </div>

                       <hr />
            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Sales Invoices</p>
              <div style="max-height:230px;overflow-y: auto;" class="table-responsive">
                            <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                                         <PagerStyle PageSizeControlType="None" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression ="Orig_Sys_Document_Ref"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"", ""{2}"");", Eval("Row_ID"), Eval("Orig_Sys_Document_Ref"), Eval("Ord_Type"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="System_Order_Date" HeaderText="Order Date" SortExpression ="System_Order_Date"
                                                              DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <%--<telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>--%>
                                                          

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrderType" HeaderText="Order Type"
                                                                  SortExpression ="OrderType" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="OrderAmount" HeaderText="Amount"
                                                                  SortExpression ="OrderAmount" UniqueName="SalesAmount" DataType="System.Decimal" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Order_Status" HeaderText="Status"
                                                                  SortExpression ="Order_Status" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                           
              </div> 
            <p>&nbsp;</p>

            <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Returns</p>
              <div style="max-height:230px;overflow-y: auto;" class="table-responsive">
              
               <telerik:RadGrid id="gvReturns" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                          <telerik:GridTemplateColumn uniqueName="Orig_Sys_Document_Ref"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression ="Orig_Sys_Document_Ref"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HRowID" runat="server" Value='<%# Bind("RowID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenRetViewWindow(""{0}"", ""{1}"");", Eval("RowID"), Eval("Orig_Sys_Document_Ref"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_Date" HeaderText="Creation Date" SortExpression ="Creation_Date"
                                                              DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                                                                  SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridTemplateColumn uniqueName="Customer"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Customer" SortExpression ="Customer"
                                                                        HeaderText="Customer" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcust" runat="server" Text='<%# Bind("Customer")%>' ></asp:Label>
                                                               
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                          
 
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="OrderAmount" HeaderText="Amount"
                                                                  SortExpression ="OrderAmount" UniqueName="ReturnsAmount" DataType="System.Decimal" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Order_Status" HeaderText="Status"
                                                                  SortExpression ="Order_Status" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

              </div>
              <p>&nbsp;</p>
             <p style="font-size:16px; color:#222; margin:10px 0; font-weight:600;">Distribution Check</p>
              <div style="max-height:230px;overflow-y: auto;" class="table-responsive">

                           <telerik:RadGrid id="gvDisCheck" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                            
                                                      <telerik:GridTemplateColumn uniqueName="Checked_On"   HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Checked_On" SortExpression ="Checked_On"
                                                                HeaderText="Checked On" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HDISID" runat="server" Value='<%# Bind("DistributionCheck_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Checked_On", "{0:dd-MMM-yyyy hh:mm tt}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenDisViewWindow(""{0}"");", Eval("DistributionCheck_ID"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                <%--                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Checked_On" HeaderText="Checked On" SortExpression ="Checked_On"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>--%>
                                                      
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Status" HeaderText="Status" SortExpression ="Status"
                                                               >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                                                                                        
                                                          
                                                        </Columns>
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
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>  
     <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary" Style="display:none"  ID="BtnExportPDF" runat="server" Text="Export" />
</asp:Content>
