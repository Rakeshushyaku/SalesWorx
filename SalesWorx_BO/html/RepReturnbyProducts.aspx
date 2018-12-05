<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepReturnbyProducts.aspx.vb" Inherits="SalesWorx_BO.RepReturnbyProducts" %>


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
         /*.rpSlide .col-sm-2 .form-group input[type="submit"].btn-block
        {
            width:49%;
            margin-right: 1%;
            float:left;
        }
    .rpSlide .col-sm-2 .form-group input[type="submit"].btn-block + .btn-block {
    margin-top: 0px;
}
    .rpSlide .col-sm-2 .form-group input[type="submit"].btn-block:last-child {
    background: #EA4C3A;
    margin-right: 0%;
    margin-left: 1%;
}*/
    
    </style>
    <script>
        function clickExportBiffExcel() {
            $("#MainContent_BtnExportBiffExcel").click()
            return false

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

        function OpenViewWindow(cid, Refno) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
           // URL = 'RepDetails.aspx?Type=Order&ReportName=OrderDetailsNew&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
            URL = 'Rep_OrderDetails.aspx?ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value() + '&Type=O'
            var oWnd = radopen(URL, null);
            oWnd.SetSize(800, 580);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        function OpenDeliveryNoteWindow(cid, Refno) {
            var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            URL = 'Rep_DelivNotesOfConsoliDateOrder.aspx?Type=I&ID=' + Refno + '&OrgID=' + combo.get_selectedItem().get_value()
            
            var oWnd = radopen(URL, null);
            oWnd.SetSize(980, 580);
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
       <h4>Return By Product</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                ExpandMode="MultipleExpandedItems">
                <Items>
                    <telerik:RadPanelItem Expanded="True" Text=" ">

                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-10 col-md-10 col-lg-10">
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>

                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Van</label><telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van" Filter="Contains" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width="100%"
                                                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                                </telerik:RadComboBox>

                                            </div>
                                        </div>

                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>From Date</label>
                                                <telerik:RadDatePicker ID="txtFromDate" runat="server">
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
                                                <telerik:RadDatePicker ID="txtToDate" runat="server">
                                                    <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                    </DateInput>
                                                    <Calendar ID="Calendar1" runat="server">
                                                        <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                    </Calendar>
                                                </telerik:RadDatePicker>

                                            </div>
                                        </div>
                                        </div>
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label>Ref. No</label>
                                                <asp:TextBox ID="txtRefNo" Width="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                            </div>
                                            
                                        </div>

                                        <div class="col-sm-3">
                                                <div class="form-group">

                                                <label>
                                                    Customer Type
                                                </label>

                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlPaymentType" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                                <%--<telerik:RadComboBox ID="ddlPaymentType" Skin="Simple" Width="100%" runat="server" CssClass="inputSM">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="1" Text="Both"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="2" Text="Credit"></telerik:RadComboBoxItem>
                                                        <telerik:RadComboBoxItem Value="3" Text="Cash"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>--%>

                                            </div>   
                                        </div>

                                        <div class="col-sm-3">
                                         <div class="form-group">

                                                <label>Customer</label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlCustomer" Width="100%"
                                                    runat="server" DataTextField="Customer" DataValueField="CustomerID" EnableLoadOnDemand="True" AutoPostBack="true" EmptyMessage="Please type Customer No./Name">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>

                                        

                                   
                                    </div>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                        <asp:Button CssClass="btn btn-sm btn-block btn-default" ID="ClearBtn" runat="server" Text="Clear" />

                                    </div>
                                    <div class="form-group fontbig text-center">
                                        <label>&nbsp;</label>
                                        <asp:HyperLink href="" CssClass="" ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                        <asp:HyperLink href="" CssClass="" ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>

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
                            <p><strong>Organisation: </strong>
                                <asp:Label ID="lbl_org" runat="server" Text=""></asp:Label></p>
                            <p><strong>Van: </strong>
                                <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                            <p><strong>From Date: </strong>
                                <asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
                            <p><strong>To Date: </strong>
                                <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>
                            <p><strong>Customer: </strong>
                                <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label></p>
                            <p><strong>Order Ref No: </strong>
                                <asp:Label ID="lbl_OrderRefno" runat="server" Text=""></asp:Label></p>
                            <p><strong>Order Type: </strong>
                                <asp:Label ID="lbl_type" runat="server" Text=""></asp:Label></p>
                        </span>
                    </i>
                </div>
            </div>

            <div id="summary" runat="server" class="row"></div>
            <p>
                <br />
            </p>
            <div class="overflowx">
            <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                 
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10" CommandItemDisplay="Top">
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>

                   
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                        <%--<telerik:GridTemplateColumn UniqueName="Orig_Sys_Document_Ref" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression="Orig_Sys_Document_Ref"
                            HeaderText="Ref No">
                            <ItemTemplate>
                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>'></asp:HiddenField>
                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"");", Eval("Row_ID"), Eval("Orig_Sys_Document_Ref"))%>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>--%>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Start_Time" HeaderText="Date" SortExpression="Start_Time"
                            DataFormatString="{0:dd-MMM-yyyy hh:mm tt}">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Route" HeaderText="Route"
                            SortExpression="Route">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep" HeaderText="Sales Rep"
                            SortExpression="SalesRep">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                                      

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber" HeaderText="Document Number"
                            SortExpression="OrderNumber">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TransactionType" HeaderText="Customer Type"
                            SortExpression="TransactionType">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customercode" HeaderText="Customer Code"
                            SortExpression="Customercode">
                            <ItemStyle Wrap="False" />
                             <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                      

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OutletCode" HeaderText="Outlet Code"
                            SortExpression="OutletCode">
                            <ItemStyle Wrap="False" />
                             <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>

                        
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Outletname" HeaderText="Outlet Name"
                            SortExpression="Outletname">
                            <ItemStyle Wrap="False" />
                             <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code"
                            SortExpression="Item_Code">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                            SortExpression="Description">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_Order_Quantity" HeaderText="Quantity" UniqueName="Ordered_Quantity"
                            SortExpression="Display_Order_Quantity" DataType="System.Double" DataFormatString="{0:#,###.0000}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_UOM" HeaderText="UOM"
                            SortExpression="Display_UOM">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                                               

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Unit_Selling_Price" HeaderText="Item Price"
                            SortExpression="Unit_Selling_Price"  DataType="System.Double" DataFormatString="{0:N2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Net_Total_Price" HeaderText="Line Total" UniqueName="Net_Total_Price"
                            SortExpression="Net_Total_Price" DataType="System.Double" DataFormatString="{0:N2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CustomerGroup" HeaderText="Customer Group"
                            SortExpression="CustomerGroup">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                                              

                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            </div>


            <div style="display: none">
                <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
            </div>

             

        </ContentTemplate>

    </asp:UpdatePanel> 
                           
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export"  />
   </div>
  
    
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />            
           <span>Processing... </span>
       </asp:Panel>
          
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       

	 
</asp:Content>
