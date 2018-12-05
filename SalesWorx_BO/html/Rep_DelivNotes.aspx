<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_DelivNotes.aspx.vb" Inherits="SalesWorx_BO.Rep_DelivNotes" %>
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
            URL = 'RepDetails.aspx?Type=DelivNotes&ReportName=DelivNotesDetails&ID=' + cid + '&OrgID=' + combo.get_selectedItem().get_value()
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
       <h4>Delivery Notes </h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td  style="padding:0px 0px 20px" >
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
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Van</label><telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van" Filter="Contains" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width="100%"
                                                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                                </telerik:RadComboBox>

                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Order Ref. No</label>
                                                <asp:TextBox ID="txtRefNo" Width="100%" CssClass="inputSM" runat="server"></asp:TextBox>
                                            </div>
                                            
                                        </div>
                                        </div>
                                    <div class="row">
                                      

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
                                          <div class="col-sm-6">
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
                           
                        </span>
                    </i>
                </div>
            </div>

            <div id="summary" runat="server" class="row"></div>
             <telerik:RadGrid ID="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="Orig_Sys_Document_Ref" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Orig_Sys_Document_Ref" SortExpression="Orig_Sys_Document_Ref"
                            HeaderText="Ref No">
                            <ItemTemplate>
                                <asp:HiddenField ID="HID" runat="server" Value='<%# Bind("Row_ID")%>'></asp:HiddenField>
                                <asp:LinkButton ID="Lnk_RefID" runat="server" Text='<%# Bind("Orig_Sys_Document_Ref")%>' ForeColor="SteelBlue" Font-Underline="true" CommandName="ConfirmReturnNote" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"", ""{1}"");", Eval("Row_ID"), Eval("Orig_Sys_Document_Ref"))%>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_Date" HeaderText="Order Date" SortExpression="Creation_Date"
                            DataFormatString="{0:dd-MMM-yyyy}">
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>


                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Sales Rep"
                            SortExpression="SalesRep_Name">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer" HeaderText="Customer"
                            SortExpression="Customer">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
 
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Amount" HeaderText="Amount"
                            SortExpression="Transaction_Amt" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAT_Amount" HeaderText="Vat Amount"
                            SortExpression="VAT_Amount" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>



            <div style="display: none">
                <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
            </div>

             

        </ContentTemplate>

    </asp:UpdatePanel> 
                           
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
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
	 
  
    </table>
	 
</asp:Content>

