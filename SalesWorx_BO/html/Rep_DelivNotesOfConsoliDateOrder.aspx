<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Rep_DelivNotesOfConsoliDateOrder.aspx.vb" Inherits="SalesWorx_BO.Rep_DelivNotesOfConsoliDateOrder" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <asp:HiddenField ID="HOrgID" runat="server" />
        <asp:HiddenField ID="HDocID" runat="server" />
        <asp:HiddenField ID="HType" runat="server" />
         <div class="row">   
               <div class="col-sm-2">
                             <div class="form-group">  <label><strong> Ref No.</strong> </label><asp:HiddenField ID="HRowID" runat="server" />
                                  
                                 <asp:Label ID="lbl_refno" runat="server" Text=""></asp:Label>
             </div>
              </div>
              <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Date</strong> </label>
                                 <asp:Label ID="lbl_Date" runat="server" Text=""></asp:Label>
             </div>
              </div>
               <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Amount</strong> </label>
                                 <asp:Label ID="lbl_amount" runat="server" Text=""></asp:Label>
             </div>
              </div>
              <div class="col-sm-2">
                             <div class="form-group">  <label><strong>Created by</strong> </label>
                                 <asp:Label ID="lbl_Salesep" runat="server" Text=""></asp:Label>
             </div>
              </div>
             <div class="col-sm-4">
                             <div class="form-group">  <label><strong>Customer</strong> </label>
                                 <asp:Label ID="lbl_Customer" runat="server" Text=""></asp:Label>
             </div>
              </div>
              </div>
         
     <telerik:RadGrid ID="gvItems" AllowSorting="False" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="True" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>
                         
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Orig_Sys_Document_Ref" HeaderText="Ref No."
                            SortExpression="Orig_Sys_Document_Ref">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>

                       

                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Product"
                            SortExpression="Product">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                      
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_Order_Quantity" HeaderText="Quantity"
                            SortExpression="Display_Order_Quantity" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Order_Quantity_UOM" HeaderText="UOM"
                            SortExpression="Order_Quantity_UOM">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Unit_Selling_Price" HeaderText="Unit Selling Price"
                            SortExpression="Unit_Selling_Price" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="FNet_Total_Price" HeaderText="Net Total Price"
                            SortExpression="FNet_Total_Price" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="VAT_Amount" HeaderText="VAT Amount"
                            SortExpression="VAT_Amount" DataType="System.Double" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
