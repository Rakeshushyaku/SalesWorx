<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepDistributionCheckDetails.aspx.vb" Inherits="SalesWorx_BO.RepDistributionCheckDetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }

        input[type="text"].rdfd_ {
            margin: 0 !important;
            padding: 0 !important;
            height: 0 !important;
            width: 0 !important;
        }
    </style>

     <style>
    div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
    <script>

        function clickExportBiffExcel() {
                      $("#MainContent_BtnExportBiffExcel").click()
            return false

        }
        function OnClientItemExpand(sender, args) {
            if (document.getElementById('MainContent_RepSec')) {

                document.getElementById('MainContent_RepSec').style.height = "350px"

            }
        }

        function OnClientItemCollapse(sender, args) {
            if (document.getElementById('MainContent_RepSec')) {

                document.getElementById('MainContent_RepSec').style.height = "450px"
            }

        }

        function alertCallBackFn(arg) {

        }

        function clickSearch() {
            $("#MainContent_SearchBtn").click()
            return false;
        }

        function OpenDisViewWindow(cid) {

            var URL
            URL = 'RepDetails.aspx?Type=DisCheck&ReportName=DistributionCheckDetails&ID=' + cid
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Distribution Check Details</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
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
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%"
                                                                runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"
                                                                AutoPostBack="True">
                                                            </telerik:RadComboBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Van</label>
                                                            <%--<telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan" Width="100%"
                                                                runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                                            </telerik:RadComboBox>--%>
                                                             <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" 
                                                             ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                                            </telerik:RadComboBox >
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                    <div class="form-group">
                                                                        <label>From Date </label>
                                                                        <telerik:RadDatePicker ID="txtFromDate" Width="100%" runat="server">
                                                                            <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                            </DateInput>
                                                                            <Calendar ID="Calendar2" runat="server">
                                                                                <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                            </Calendar>
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="form-group">
                                                                        <label>To Date </label>
                                                                        <telerik:RadDatePicker ID="txtToDate" Width="100%" runat="server">
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



                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>
                                                                Customer </label>

                                                            <telerik:RadComboBox EmptyMessage="Please type Customer No/ Name" Skin="Simple" AutoPostBack="true" EnableLoadOnDemand="true" Filter="Contains" ID="ddlCustomer" Width="100%"
                                                                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>




                                                </div>
                                            </div>


                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                                      <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />  
                                                </div>
                                                <div class="form-group fontbig text-center">
                                                    <label>&nbsp;</label>
                                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" Visible="false"   runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                    <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" Visible="false"  runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                  
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
                                        <p>
                                            <strong>Organisation: </strong>
                                            <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Van: </strong>
                                            <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>From Date: </strong>
                                            <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>To Date: </strong>
                                            <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label>
                                        </p>
                                        <p>
                                            <strong>Customer: </strong>
                                            <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label>
                                        </p>
                                    </span>
                                </i>
                            </div>
                        </div>

                        <div>
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
                        <div class="table-responsive">
                             
                            <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent" 
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None"   >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                        <%-- <telerik:GridTemplateColumn uniqueName="Collection_Ref_No"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Collection_Ref_No" SortExpression ="Collection_Ref_No"
                                                                HeaderText="Ref No" >
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("DistributionCheck_ID")%>' ></asp:HiddenField>
                                                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("DistributionCheck_ID")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"");", Eval("DistributionCheck_ID"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
                                                                                                              
                                                                                                              
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customercode" HeaderText="Customer Code"
                                                                  SortExpression ="Customercode" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                        
                                                          

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_Name" HeaderText="Customer Name"
                                                                  SortExpression ="Customer_Name" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Checked_On" HeaderText="Checked On" SortExpression ="Checked_On"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CheckedBy" HeaderText="Checked By"
                                                                  SortExpression ="CheckedBy" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product_Name" HeaderText="Product Name "
                                                                  SortExpression ="Product_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                        
                                                          

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Product Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn uniqueName="Entry"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false"  HeaderText="Entry" >
                                                               <ItemTemplate>
                                                                  <asp:HiddenField ID="HEntry_Is_Available" runat="server" Value='<%# Bind("Is_Available")%>' ></asp:HiddenField>
                                                                    
                                                                  <asp:ImageButton id="btnimg_entry" runat="server" AlternateText="ImageButton1" ImageAlign="left"  ImageUrl="images/pict.jpg"/>
                                                             </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                      
                                                              <telerik:GridTemplateColumn uniqueName="Exit"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" 
                                                                HeaderText="Exit" >
                                                            <ItemTemplate>
                                                                  <asp:HiddenField ID="HExit_Is_Available" runat="server" Value='<%# Bind("Is_Available")%>' ></asp:HiddenField>
                                                                  <asp:HiddenField ID="HEntry_ExitInfo" runat="server" Value='<%# Bind("ExitInfo")%>' ></asp:HiddenField>
                                                                      <asp:ImageButton id="btnimg_Exit" runat="server" AlternateText="ImageButton2" ImageAlign="left"  ImageUrl="images/pict.jpg"/>
                                                             </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                                              
                                                          
                                                            
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qty" HeaderText="Qty in Shelf"
                                                                  SortExpression ="Qty"  DataFormatString="{0:0.#}" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qty_2" HeaderText="Qty in Store" 
                                                                  SortExpression ="Qty_2"  DataFormatString="{0:0.#}" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="InvoiceQty" HeaderText="Invoiced Qty"
                                                                  SortExpression ="InvoiceQty"  DataFormatString="{0:0.#}" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                        

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Expiry_Dt" HeaderText="Expiry Date" SortExpression ="Expiry_Dt"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                             <telerik:RadGrid id="gvRep_ASR" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent" 
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None"   >
                                                       
                 <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10" CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                        
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Checked_On" HeaderText="Checked On" SortExpression ="Checked_On"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>                                        
                                                                      
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Checked_On" HeaderText="Checked Time" SortExpression ="Checked_On"
                                                               DataFormatString="{0:hh:mm tt}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>       

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CheckedBy" HeaderText="VAN"
                                                                  SortExpression ="CheckedBy" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                                                                     
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customercode" HeaderText="Customer Code"
                                                                  SortExpression ="Customercode" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                        
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_Name" HeaderText="Customer Name"
                                                                  SortExpression ="Customer_Name" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                           

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product_Name" HeaderText="Item Description"
                                                                  SortExpression ="Product_Name" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                        
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Is_Available" HeaderText="Entry"
                                                                  SortExpression ="Is_Available" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ExitVal" HeaderText="Exit"
                                                                  SortExpression ="ExitVal" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                                                                                    
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qty" HeaderText="Qty in Shelf"
                                                                  SortExpression ="Qty"  DataFormatString="{0:0.#}" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Qty_2" HeaderText="Qty in Store" 
                                                                  SortExpression ="Qty_2"  DataFormatString="{0:0.#}" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_UOM" HeaderText="UOM"
                                                                  SortExpression ="Display_UOM" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Expiry_Dt" HeaderText="Expiry Date" SortExpression ="Expiry_Dt"
                                                               DataFormatString="{0:dd-MMM-yyyy}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>


                        </div>
                    </ContentTemplate>

                    
                                
                </asp:UpdatePanel>


                <div style="display: none">
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
                     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export"  />
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



                    </ProgressTemplate>
                </asp:UpdateProgress>

    <div style="overflow: scroll; height: 445px; border: groove; background-color: white; border-color: LightGrey; border-width: 1px" id="RepSec" runat="server" visible="false">
        <rsweb:ReportViewer ID="RVMain" runat="server" ShowBackButton="true"
            ProcessingMode="Remote"
            DocumentMapWidth="100%" AsyncRendering="false" SizeToReportContent="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
