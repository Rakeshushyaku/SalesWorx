<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Rep_OrderDetails.aspx.vb" Inherits="SalesWorx_BO.Rep_OrderDetails"  Title="Order Details"%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
      <link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/responsive.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function clickExportExcel() {
           
            $("#BtnExportExcel").click()
            return false

        }
        function clickExportPDF() {
            $("#BtnExportPDF").click()
            return false
        }
        function OpenDialog(item) {



            var URL
            URL =   item

            window.open(URL, '_blank');





        }

        function containerMouseover(sender) {
            sender.getElementsByTagName("div")[0].style.display = "";
        }
        function containerMouseout(sender) {
            sender.getElementsByTagName("div")[0].style.display = "none";
        }

        function alertCallBackFn(arg) {

        }


        function ConfirmDelete(msg, event) {

            var ev = event ? event : window.event;
            var callerObj = ev.srcElement ? ev.srcElement : ev.target;
            var callbackFunctionConfirmDelete = function (arg, ev) {
                if (arg) {
                    callerObj["onclick"] = "";
                    if (callerObj.click) callerObj.click();
                    else if (callerObj.tagName == "A") {
                        try {
                            eval(callerObj.href)
                        }
                        catch (e) { }
                    }
                }
            }
            radconfirm(msg, callbackFunctionConfirmDelete, 330, 100, null, 'Confirmation');
            return false;
        }
    </script>

    <style type="text/css">  
        .RadTabStrip .rtsLevel .rtsTxt
        {
            text-decoration: inherit;
            font-size: 13px;
            font-weight: bold;
        }

        .rgFooter td
        {
            border-top: 1px solid;
            border-color: #999 #c3c3c3;
            color: #000 !Important;
            background-color: #eff9ff !Important;
            font-weight: bold !Important;
        }

    
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }  
        </style>
</head>
<body>
    <form id="form1" runat="server">
       
    <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeOut="36000">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=272931&clcid=0x409 --%>
                <%--Framework scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="jquery.ui.combined" />
            <%--    <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />--%>
              <%--  <asp:ScriptReference Name="WebFormsBundle" />--%>
                <%--Site scripts--%>
            </Scripts>
        </asp:ScriptManager>
           <telerik:RadAjaxPanel ID="l" runat="server">
        <asp:HiddenField ID="HOrgID" runat="server" />
        <asp:HiddenField ID="HDocID" runat="server" />
        <asp:HiddenField ID="HType" runat="server" />
        <asp:HiddenField ID="lblDecimal" runat="server" />
        <asp:HiddenField ID="lblCurrency" runat="server" />
         <div class="table-responsive" id="Detailed"  runat="server">

                   <telerik:RadTabStrip ID="Salestab" runat="server" Skin="Windows7" MultiPageID="RadMultiPage21"
                    SelectedIndex="0">
                    <Tabs>
                        <telerik:RadTab Text="Order Details" runat="server">
                        </telerik:RadTab>

                         <telerik:RadTab Text="Additional Info" runat="server">
                        </telerik:RadTab>

                        <telerik:RadTab Text="LPO Images" runat="server">
                        </telerik:RadTab>
                       
                        

                    </Tabs>
                </telerik:RadTabStrip>
             </div>

        <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                       <telerik:RadPageView ID="RadPageView1" runat="server">
                        <div class="p-l-15 p-r-15 p-t-15">
         <div class="row">   
               <div class="col-sm-2">
                             <div class="form-group">  <label><strong> Ref No.</strong> </label><asp:HiddenField ID="HRowID" runat="server" />
                                  
                                 <asp:Label ID="lbl_refno" runat="server" Text=""></asp:Label>
             </div>
              </div>
             <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Creation Date</strong> </label>
                                 <asp:Label ID="lbl_Date" runat="server" Text=""></asp:Label>
             </div>
              </div>
             <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Created by</strong> </label>
                                 <asp:Label ID="lbl_Salesep" runat="server" Text=""></asp:Label>
             </div>
              </div>
             <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Status</strong> </label>
                                 <asp:Label ID="lbl_Status" runat="server" Text=""></asp:Label>
             </div>
              </div>

              <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Order Type</strong> </label>
                                 <asp:Label ID="lbl_OrderType" runat="server" Text=""></asp:Label>
             </div>
              </div>

             <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Transaction Type</strong> </label>
                                 <asp:Label ID="lbl_TransType" runat="server" Text=""></asp:Label>
             </div>
              </div>
              </div>
                <div class="row"> 
                     <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Currency</strong> </label>
                                 <asp:Label ID="lbl_CurrDesc" runat="server" Text=""></asp:Label>
             </div>
              </div>

               <div class="col-sm-3">
                             <div class="form-group">  <label><strong>Customer</strong> </label>
                                 <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label>
             </div>
              </div>

                     <div class="col-sm-3">
                             <div class="form-group">  <label><strong>Credit Customer</strong> </label>
                                 <asp:Label ID="lbl_creditCustomername" runat="server" Text=""></asp:Label>
             </div>
              </div>
              <div class="col-sm-2">
                             <div class="form-group">  <label><strong>CC Name & Tel No.</strong> </label>
                                 <asp:Label ID="lbl_CCName" runat="server" Text=""></asp:Label>
                                 
             </div>
              </div>
                    <div class="col-sm-2">
                        <div class="form-group">  <label><strong>Export</strong> </label>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportExcel()" ></asp:ImageButton>
                                 <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl ="../assets/img/export-pdf.png" OnClientClick="clickExportPDF()" ></asp:ImageButton>
                            </div>
                    </div>
              </div>

       
         
     <telerik:RadGrid ID="gvItems" AllowSorting="False" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="5" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="5">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                        
                       

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Item"
                            SortExpression="Product">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Lot_Number" HeaderText="Lot Number"
                            SortExpression="Lot_Number">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                      
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Ordered_Quantity" HeaderText="Quantity"
                            SortExpression="Ordered_Quantity" DataType="System.Double" DataFormatString="{0:N2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_UOM" HeaderText="UOM"
                            SortExpression="Display_UOM">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="UPrice" HeaderText="Price"
                            SortExpression="UPrice" DataType="System.Double" DataFormatString="{0:N2}" UniqueName="UPrice">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                       <%-- <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Discount" HeaderText="Discount"
                            SortExpression="Discount" DataType="System.Double" DataFormatString="{0:N2}" UniqueName="Discount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>--%>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="DiscountVal" HeaderText="Discount"
                            SortExpression="DiscountVal">
                            <ItemStyle Wrap="True" HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ItemPrice" HeaderText="Value"
                            SortExpression="ItemPrice" DataType="System.Double" DataFormatString="{0:N2}" UniqueName="ItemPrice">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="95px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                        
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAT_Amount" HeaderText="VAT Amount"
                            SortExpression="VAT_Amount" DataType="System.Double" DataFormatString="{0:N2}" UniqueName="VAT_Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="95px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAT_Percentage" HeaderText="VAT Percentage"
                            SortExpression="VAT_Percentage" DataType="System.Double" DataFormatString="{0:N2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="95px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="NetTotal" HeaderText="Gross Incl.VAT"
                            SortExpression="NetTotal" DataType="System.Double" DataFormatString="{0:N2}" UniqueName="NetTotal">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="95px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <table class="table text-right">
                <tr id="discountRow" runat="server" ><td><asp:Label ID="lbl_DiscountHead" runat="server" Text="Discount Amount" Font-Bold="true"></asp:Label></td><td width="95px"><asp:Label ID="lbl_discount" runat="server" Text="" Font-Bold="true"></asp:Label></td><td width="95px">&nbsp;</td><td width="95px">&nbsp;</td><td width="95px">&nbsp;</td></tr>
                <tr><td><asp:Label ID="lbl_NetHead" runat="server" Text="Net Amount" Font-Bold="true"></asp:Label></td><td><asp:Label ID="lbl_Net" runat="server" Text="" Font-Bold="true"></asp:Label></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
                <tr><td><asp:Label ID="lbl_VATHead" runat="server" Text="VAT Amount" Font-Bold="true"></asp:Label></td><td><asp:Label ID="lbl_vatAmount" runat="server" Text="" Font-Bold="true"></asp:Label></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
                <tr><td><asp:Label ID="lbl_TotalHead" runat="server" Text="Total" Font-Bold="true"></asp:Label></td><td><asp:Label ID="lbl_total" runat="server" Text="" Font-Bold="true"></asp:Label></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
            </table>
                           </div>
    </telerik:RadPageView>
             <telerik:RadPageView ID="RadPageView2" runat="server">
                 <div class="p-l-15 p-r-15 p-t-15">
                  <div class="row"> 
               <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Approval Code</strong> </label>
                                 <asp:Label ID="lbl_approvalCode" runat="server" Text=""></asp:Label>
             </div>
              </div>

                     <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Approved by</strong> </label>
                                 <asp:Label ID="lbl_approvedBy" runat="server" Text=""></asp:Label>
             </div>
              </div>
              <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Approval Reason</strong> </label>
                                 <asp:Label ID="lbl_reason" runat="server" Text=""></asp:Label>
             </div>
              </div>
              </div>
                 <div class="row"> 
               <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Customer PO</strong> </label>
                                 <asp:Label ID="lbl_CustomerpO" runat="server" Text=""></asp:Label>
             </div>
              </div>

                     <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Start Time</strong> </label>
                                 <asp:Label ID="lbl_StartTime" runat="server" Text=""></asp:Label>
             </div>
              </div>
              <div class="col-sm-4">
                             <div class="form-group">  <label><strong>End Time</strong> </label>
                                 <asp:Label ID="lbl_EndTime" runat="server" Text=""></asp:Label>
             </div>
              </div>
              </div>

                 <div class="row"> 
               <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Comments</strong> </label>
                                 <asp:Label ID="lbl_note" runat="server" Text=""></asp:Label>
             </div>
              </div>

                    
              <div class="col-sm-8">
                             <div class="form-group">  <label><strong>Shipping Instruction</strong> </label>
                                 <asp:Label ID="lbl_ShippingNote" runat="server" Text=""></asp:Label>
             </div>
              </div>
              </div>
                 <div class="row"> 
               <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Signed By</strong> </label>
                                 <asp:Label ID="lbl_Signed" runat="server" Text=""></asp:Label>
             </div>
              </div>

                    
              <div class="col-sm-8">
                             <div class="form-group">  <label><strong>&nbsp;</strong> </label>
                                <asp:Image ID="Img_Signature" runat="server" />
             </div>
              </div>
              </div>
                 </div>
                 </telerik:RadPageView>
                  <telerik:RadPageView ID="RadPageView3" runat="server">
                      <div class="p-l-15 p-r-15 p-t-15">
                           <asp:Label ID="lbl_msgimg" runat="server" ForeColor ="Red"  Text=""></asp:Label>
                         <asp:DataList ID="ImgList" runat="server" CellSpacing="3" RepeatLayout="Table" RepeatColumns="3" CssClass="hidebr table">
            
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table">
                   
                    <%--<tr>
                    
                        <td>
                             <br /><asp:Label ID="Label1" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                            <asp:HiddenField ID="HID" runat="server" Value='<%#Eval("Row_ID")%>'></asp:HiddenField>
                             <asp:HiddenField ID="HFile" runat="server" Value='<%#Eval("File_Name")%>'></asp:HiddenField>
                            <asp:HiddenField ID="HThumbNail" Value='<%#Eval("File_Name")%>' runat="server" />
                        </td>
                    </tr>--%>
                    <tr><td  style="vertical-align:middle;text-align:center;padding:0 !important;border:0;">
                        
                        <p><a href="javascript:OpenDialog('<%#Eval("SRC") %>')">
                        <asp:Image ID="Image1" ImageUrl='<%#Eval("SRC") %>'   runat="server" style="max-width:60%; max-height:60%;"></asp:Image>
                            </a></p>
                        
                            </td></tr>
                </table>
                
            </ItemTemplate>
                   <ItemStyle CssClass=""/>
       </asp:DataList> 
                      </div>
                      </telerik:RadPageView>
            </telerik:RadMultiPage>
               </telerik:RadAjaxPanel>
         <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         
   </div>
    </form>
</body>
</html>
