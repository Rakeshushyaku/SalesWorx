<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ApproveReturnNotes.aspx.vb" Inherits="SalesWorx_BO.ApproveReturnNotes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
    input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
    .rgMasterTable .rgSelectedRow,
    .rgMasterTable .rgSelectedRow.rgHoveredRow {
        background: #ffa517 !important;
        color: #ffffff !important;
    }
    .rgMasterTable > tbody > tr > td{vertical-align:middle !important;}
</style>
<script type="text/javascript">


    function alertCallBackFn(arg) {
        HideRadWindow()
    }
    function HideRadWindow() {

        var elem = $('a[class=rwCloseButton');

        if (elem != null && elem != undefined) {
            $('a[class=rwCloseButton')[0].click();
        }

        $("#frm").find("iframe").hide();
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
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Approve Return Note</h4>
 
                                         
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
     <telerik:RadAjaxPanel ID="l" runat="server">
          <div class="row">   
               <div class="col-sm-3">
                             <div class="form-group">  <label><strong>Ref No.</strong></label><asp:HiddenField ID="HRowID" runat="server" />
                                 <asp:HiddenField ID="HOrgID" runat="server" />
                                  <asp:HiddenField ID="HCustID" runat="server" />
                                  <asp:HiddenField ID="HSite" runat="server" />
                                   <asp:HiddenField ID="HSalesRep" runat="server" />
                                  <asp:HiddenField ID="HInvID" runat="server" />
                                  <asp:HiddenField ID="HItemCode" runat="server" />
                                  <asp:HiddenField ID="HLineID" runat="server" />
                                 <asp:HiddenField ID="HUOM" runat="server" />
                                 <asp:Label ID="lbl_refno" runat="server" Text="Label"></asp:Label>
                                 <asp:HiddenField ID="HPrice_List" runat="server" />
             </div>
              </div>
              <div class="col-sm-3">
                             <div class="form-group">  <label> <strong>Date</strong>  </label>
                              <asp:Label ID="lbl_Date" runat="server" Text="Label"></asp:Label>
             </div>
              </div>
               <div class="col-sm-3">
                             <div class="form-group">   <label><strong>Transaction Amount</strong>  </label> 
                                 <asp:Label ID="lbl_amount" runat="server" Text="Label"></asp:Label>
             </div>
              </div>
              <div class="col-sm-3">
                             <div class="form-group">    <label><strong>Created by</strong>  </label> 
                                 <asp:Label ID="lbl_Salesep" runat="server" Text="Label"></asp:Label>
             </div>
              </div>
              </div>
       <div class="row">
            <div class="col-sm-6">
                             <div class="form-group">    <label><strong>Customer</strong>  </label>
                                 <asp:Label ID="lbl_Customer" runat="server" Text="Label"></asp:Label>
             </div>
              </div>
           <div class="col-sm-3">
                      <div class="form-group">&nbsp; </div>
             </div>
              
               <div class="col-sm-3">  
                  <div class="form-group"> 
                       <label>&nbsp;</label>
                <telerik:RadButton ID="btnConfirm" Skin ="Simple"    runat="server" Text="Confirm" CssClass ="btn btn-success" />
                <telerik:RadButton ID="btnBack" Skin ="Simple"    runat="server" Text="Go Back" CssClass ="btn btn-primary" />
                                  </div>
            </div>  
              </div>

        

         <telerik:RadWindow ID="MPEDetails" Title= "Attach Invoice" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                AutoSize="true" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true" Width="800px"  MinHeight="160px" MinWidth="800px">
                                              <ContentTemplate>
                                                  <div class="container-fluid">

                                      <asp:Label ID="lbl_msg" runat="server" Text="Label" CssClass="text-danger h4 center-block"></asp:Label>

                                                      <div class="row">
                                                      <div class="col-sm-4">
                             <div class="form-group"> <b style="display:block; padding-top:8px;">
            <asp:Label ID="lbl_UOM" runat="server" Text="Please Enter Returning Quantity"></asp:Label> </b>
                                 </div>
                                                          </div>
                                                      <div class="col-sm-3">
                             <div class="form-group"> 
            <asp:TextBox ID="Txt_Quantity" runat="server" onKeypress='return NumericOnly(event)' Text='' MaxLength="10" ForeColor="SteelBlue" Font-Underline="true"  ></asp:TextBox>
                                 </div>
                                                          </div>
                                                       <div class="col-sm-5">
                             <div class="form-group"> 
  <telerik:RadButton ID="Btn_attach" Skin ="Simple"    runat="server" Text="OK" CssClass ="btn btn-success" />
                                 <telerik:RadButton ID="Btn_Cancel" Skin ="Simple"    runat="server" Text="Cancel" CssClass ="btn btn-warning" />
                                 <telerik:RadButton ID="Btn_release" Skin ="Simple"  Visible="false"   runat="server" Text="Reset Attached Invoice" CssClass ="btn btn-primary" />
                                                 </div>
                                                           </div>
                                                        
                                                      </div>

      <div class="row">    
              <div class="col-sm-12"><div class="form-group">                             
                               <telerik:RadGrid ID="gv_Invoice" AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="false" runat="server" AllowMultiRowSelection="false"   
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="True"></Selecting>
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10" DataKeyNames="Invoice_No">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Invoice_No" HeaderText="Invoice Ref. No."
                            SortExpression="Invoice_No">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Invoice_Date" HeaderText="Invoice Date"
                            SortExpression="Invoice_Date"  DataFormatString="{0:dd-MMM-yyyy hh:mm tt}">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CUOM" HeaderText="UOM"
                            SortExpression="CUOM">
                            <ItemStyle Wrap="True" />
                        </telerik:GridBoundColumn>
                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CNet_Unit_Price" HeaderText="Net Unit Price"
                            SortExpression="CNet_Unit_Price" DataType="System.Decimal" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" Width="120" />
                        </telerik:GridBoundColumn>
                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CInvoice_Qty" HeaderText="Invoiced Qty"
                            SortExpression="CInvoice_Qty" DataFormatString="{0:f2}">
                            <ItemStyle Wrap="false" HorizontalAlign="Right" Width="100" />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="CAvlQty" HeaderText="Available Qty"
                            SortExpression="CAvlQty" DataFormatString="{0:f2}">
                             <ItemStyle Wrap="false" HorizontalAlign="Right" Width="100" />
                        </telerik:GridBoundColumn>
                           
                        <telerik:GridTemplateColumn  UniqueName="Max_Unit_Price" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" Visible="false" DataField="Max_Unit_Price" SortExpression="Unit_Selling_Price"
                            >
                            <ItemTemplate>
                                <asp:hiddenfield ID="HMax_Unit_Price" runat="server" value='<%# Bind("Max_Unit_Price")%>'/>
                                 <asp:hiddenfield ID="HReturn_Qty" runat="server" value='<%# Bind("AvlQty")%>'/>
                                <asp:hiddenfield ID="HInvoice_No" runat="server" value='<%# Bind("Invoice_No")%>'/>
                                <asp:hiddenfield ID="HNet_Unit_Price" runat="server" value='<%# Bind("CNet_Unit_Price")%>'/>
                              
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
                  </div>
                  </div>
          </div>
                                                  
                                                    </div>
                             </ContentTemplate>
                                                    </telerik:RadWindow> 
    <div class="row">

     <telerik:RadGrid ID="gvItems" AllowSorting="false" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                PageSize="10" AllowPaging="false" runat="server"
                GridLines="None">

                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <Columns>

                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Product" HeaderText="Item Code & Description"
                            SortExpression="Product">
                            <ItemStyle Wrap="True" VerticalAlign="Middle"  />
                        </telerik:GridBoundColumn>
                         <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Line_Number" HeaderText="Line Number"
                            SortExpression="Line_Number">
                            <ItemStyle Wrap="True" VerticalAlign="Middle" Width="100" />
                        </telerik:GridBoundColumn>
                          
                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Order_Quantity_UOM" HeaderText="UOM"
                            SortExpression="Order_Quantity_UOM">
                            <ItemStyle Wrap="True"  VerticalAlign="Middle"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn UniqueName="Unit_Selling_Price" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Unit_Selling_Price" SortExpression="Unit_Selling_Price"
                            HeaderText="Unit Selling Price">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_Price" runat="server" Text='<%# Eval("Unit_Selling_Price", "{0:N2}")%>'/>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="100" VerticalAlign="Middle" />
                        </telerik:GridTemplateColumn>
                         
                        <telerik:GridTemplateColumn UniqueName="Display_Order_Quantity" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Display_Order_Quantity" SortExpression="Display_Order_Quantity"
                            HeaderText="Quantity">
                            <ItemTemplate>
                                 <asp:HiddenField ID="HActUP" runat="server" Value='<%# Bind("Unit_Selling_Price")%>'/>
                                <asp:HiddenField ID="HLine_Number" runat="server" Value='<%# Bind("Line_Number")%>'/>
                                <asp:HiddenField ID="HInventory_Item_ID" runat="server" Value='<%# Bind("Inventory_Item_ID")%>' />
                                <asp:HiddenField ID="HItem_Code" runat="server" Value='<%# Bind("Item_Code")%>' />
                                <asp:HiddenField ID="HPrimaryUOM" runat="server" Value='<%# Bind("Primary_UOM_Code")%>' />
                                <asp:TextBox ID="Txt_Display_Order_Quantity" Width="80" runat="server" Enabled="false"  onKeypress='return NumericOnly(event)' Text='<%# Eval("Display_Order_Quantity", "{0:N2}")%>' MaxLength="10" ForeColor="SteelBlue" Font-Underline="true"  ></asp:TextBox>
                            </ItemTemplate>
                              <ItemStyle HorizontalAlign="Right" Width="80" VerticalAlign="Middle" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn   HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false"  
                            HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_Invoice_No" runat="server" Text=''/>
                            </ItemTemplate>
                             <ItemStyle   VerticalAlign="Middle" />
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn UniqueName="Display_Order_Quantity" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" DataField="Display_Order_Quantity" SortExpression="Display_Order_Quantity"
                            HeaderText="Attach Invoice">
                            <ItemTemplate>
                                <asp:HiddenField ID="HInvNo" runat="server"  />
                                <asp:HiddenField ID="MaxPrice" runat="server"  />
                                <asp:HiddenField ID="UnitPrice" runat="server"  />
                                <asp:HiddenField ID="HUOM" runat="server" Value='<%# Bind("Order_Quantity_UOM")%>'/>
                                <asp:ImageButton ID="Img_Attach" runat="server" AlternateText="Attach Invoice" ImageUrl="~/images/attachment.png" OnClick="ImageButton_Click" />   
                               
                            </ItemTemplate>
                               <ItemStyle HorizontalAlign="Right" Width="50" VerticalAlign="Middle"/>
                        </telerik:GridTemplateColumn>
                        
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
</div>

	 
  
	 </telerik:RadAjaxPanel>
     
     <asp:UpdateProgress ID="UpdateProgress2" DisplayAfter="10"
        runat="server">
        <progresstemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </progresstemplate>
    </asp:UpdateProgress>
</asp:Content>
