<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_TradeEffecSimpleDetail.aspx.vb" Inherits="SalesWorx_BO.Rep_TradeEffecSimpleDetail" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function alertCallBackFn(arg) {

        }

        
        function clickExportExcel() {
            //$("#MainContent_BtnExportExcel").click()
            //return false
            debugger;
            let file = new Blob([$('.divclass').html()], { type: "application/vnd.ms-excel" });
            let url = URL.createObjectURL(file);
            let a = $("<a />", {
                href: url,
                download: "filename.xls"
            }).appendTo("body").get(0).click();
            e.preventDefault();


          
        }
        
       
        function clickExportBiffExcel() {
            debugger;
            $("#MainContent_BtnExportBiffExcel").click()
            return false

        }


    </script>

    <style type="text/css">
        div.RadGrid .rgHeader {
            white-space: nowrap;
        }

        .k-chart svg {
            margin: 0 -7px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Report</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>



    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
                        

                     <div id="divPrint" class="divclass">

                            

                            <div class="row" id="MainContent_summary">
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                        Unique Customers Billed<div class="text-primary">
                            <asp:Label ID="lblUcb" runat="server"></asp:Label></div>

                    </div>
                </div>
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Number of Unique Invoices <asp:Label ID="lblSalesCurr" runat="server" Visible ="false" ></asp:Label><div class="text-primary">
                            <asp:Label ID="lblNui" runat="server"></asp:Label></div>

                    </div>
                </div>
                 <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Unique SKUs ordered<asp:Label ID="Label1" runat="server" Visible ="false" ></asp:Label><div class="text-primary">
                            <asp:Label ID="LblSkuO" runat="server"></asp:Label></div>

                    </div>
                </div>
                <div class="col-sm-4 col-md-3 col-lg-2-1">
                    <div class="widgetblk widgetblkinsmall">
                       Unique SKUs given as FOC <asp:Label ID="Label3" runat="server" Visible ="false" ></asp:Label><div class="text-primary">
                            <asp:Label ID="LblSkuF" runat="server"></asp:Label></div>

                    </div>
                </div>


               

               
            </div>

                          <div id="divCurrency" runat="server" visible="false"  >
                                                <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                           </div>


                         <div class="overflowx">
                        <asp:updatePanel runat="server" ID="innerPanel" UpdateMode="Conditional" >
                            <ContentTemplate>
                                      <telerik:RadGrid id="gvRep" AutoGenerateColumns="false" Skin="Simple" Width="100%" BorderColor="LightGray" Visible="true" 
                                PageSize="14" AllowPaging="True" runat="server"   export
                                GridLines="None" >
                                         
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                                            <ClientSettings EnableRowHoverStyle="true">
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10"  CommandItemDisplay="Top" >
                    <CommandItemTemplate>
                        <div style="text-align:right;padding:4px 10px 4px 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl ="../assets/img/export-excel.png" OnClientClick="clickExportBiffExcel()" ></asp:ImageButton>
                            </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ShowExportToExcelButton="false" ShowRefreshButton="false" ShowAddNewRecordButton="false"/>

                                                       

                                                
                                    
                                                  <Columns>
                                     <telerik:GridBoundColumn UniqueName="OrderItemCode" 
                                        
                                        SortExpression="OrderItemCode" HeaderText="Order Item Code" DataField="OrderItemCode"
                                        ShowFilterIcon="false">
                                        <HeaderStyle Width="110px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                       <telerik:GridBoundColumn UniqueName="OrderDesc" 
                                        SortExpression="OrderDesc" HeaderText="Order Description" DataField="OrderDesc"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                                       <telerik:GridBoundColumn UniqueName="OrderUOM" 
                                        SortExpression="OrderUOM" HeaderText="Order UOM" DataField="OrderUOM"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>


                                                      <telerik:GridBoundColumn UniqueName="BonusItemCode" 
                                        SortExpression="BonusItemCode" HeaderText="Bonus Item Code" DataField="BonusItemCode"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="BonusItemDec" 
                                        SortExpression="BonusItemDec" HeaderText="Bonus Item Dec" DataField="BonusItemDec"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="BonusUOM" 
                                        SortExpression="BonusUOM" HeaderText="Bonus UOM" DataField="BonusUOM"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>


                                                      <telerik:GridBoundColumn UniqueName="PointType" 
                                        SortExpression="PointType" HeaderText="Point Type" DataField="PointType"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      

                                                      <telerik:GridBoundColumn UniqueName="Invoices" 
                                        SortExpression="Invoices" HeaderText="Invoices" DataField="Invoices"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                       <telerik:GridBoundColumn UniqueName="SKUUnitsOrdered" 
                                        SortExpression="SKUUnitsOrdered" HeaderText="SKU Units Ordered" DataField="SKUUnitsOrdered"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="SKUUnitUOM" 
                                        SortExpression="SKUUnitUOM" HeaderText="SKU Unit UOM" DataField="SKUUnitUOM"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="FOC_ordered" 
                                        SortExpression="FOC_ordered" HeaderText="FOC ordered" DataField="FOC_ordered"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="FOC_Unit_UOM" 
                                        SortExpression="FOC_Unit_UOM" HeaderText="FOC Unit UOM" DataField="FOC_Unit_UOM"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn UniqueName="Customer" 
                                        SortExpression="Customer" HeaderText="Customer" DataField="Customer"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>

                                                      <telerik:GridBoundColumn UniqueName="FromQTy" 
                                        SortExpression="FromQTy" HeaderText="From Quantity" DataField="FromQTy"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn UniqueName="ToQty" 
                                        SortExpression="ToQty" HeaderText="To Quantity" DataField="ToQty"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>
                                                      <telerik:GridBoundColumn UniqueName="BonusQty" 
                                        SortExpression="BonusQty" HeaderText="Bonus Quantity" DataField="BonusQty"
                                        ShowFilterIcon="false" 
                                          >
                                        <HeaderStyle Width="270px" Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                    </telerik:GridBoundColumn>





                                                      </Columns>
                                                 <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                              <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="OrderItemCode" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="OrderItemCode" FieldName="OrderItemCode" />
                                                                <telerik:GridGroupByField FieldAlias="Total_From_Quantity" Aggregate="Sum" FieldName="FromQTy" HeaderText="Total From Quantity" HeaderValueSeparator="FromQTy"  FormatString="sd"/>
                                                                
                                                                <telerik:GridGroupByField FieldAlias="Total_To_Quantity" Aggregate="Sum" FieldName="ToQty" />
                                                                <telerik:GridGroupByField FieldAlias="Total_Bonus_Quantity" Aggregate="Sum" FieldName="BonusQty" />

                                                            </SelectFields>
                                                                
                                                        </telerik:GridGroupByExpression>
                                                        
                                                    </GroupByExpressions>


                                               
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:updatePanel> 
                              
                     </div> 
                            
           

                      </div>          
                       
                        
                             
                    <%-- <asp:HiddenField ID="hfCurrency" runat="server" Value='' ></asp:HiddenField>                                 
                            <asp:HiddenField ID="hfDecimal" runat="server" Value='0' ></asp:HiddenField>--%>

                    </contenttemplate>
    </asp:UpdatePanel>

    <div >
        <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
         <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportBiffExcel" runat="server" Text="Export" />
   </div>
         
    </div>
   


  


</asp:Content>
