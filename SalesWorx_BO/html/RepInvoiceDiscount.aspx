<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepInvoiceDiscount.aspx.vb" Inherits="SalesWorx_BO.RepInvoiceDiscount" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

     <script>

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
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server"/> 


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
     <h4>Invoice Discount Details</h4>
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
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization" Width="100%"
                                                                runat="server" DataTextField="Description" DataValueField="MAS_Org_ID"
                                                                AutoPostBack="True">
                                                            </telerik:RadComboBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>Van</label>
                                                            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan" Width="100%"
                                                                runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>
                                                                Customer :</label>

                                                            <telerik:RadComboBox EmptyMessage="Please type Customer No/ Name" Skin="Simple" AutoPostBack="true" EnableLoadOnDemand="true" Filter="Contains" ID="ddlCustomer" Width="100%"
                                                                runat="server" DataTextField="Customer" DataValueField="CustomerID">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label>From Date </label>
                                                            <telerik:RadDatePicker ID="txtFromDate" runat="server">
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
                                                            <label>To Date </label>
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
                                            </div>


                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                    <asp:Button CssClass="btn btn-sm btn-block btn-primary" ID="SearchBtn" runat="server" Text="Search" />
                                                     <asp:Button  CssClass ="btn btn-sm btn-block btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />  
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

                        
                                             <div id="divCurrency" runat="server" visible="false"  >
                                                <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                                    </div>
                           <div class="table-responsive">
                                <telerik:RadGrid id="gvRep" FooterStyle-HorizontalAlign="Right"   ShowFooter="true" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings IgnorePagingForGroupAggregates="true" GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true" 
                    PageSize="11">


                  <PagerStyle  PageSizeControlType="None"  Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                        <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>
                                                           
                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Ordered_Quantity" HeaderText="Qty"
                                                                  SortExpression ="Ordered_Quantity" DataFormatString="{0:#,###.##}"  ItemStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                  <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                        

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Display_UOM" HeaderText="UOM"
                                                                  SortExpression ="Display_UOM"  >
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="UPrice" HeaderText="Price" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="UPrice" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right" />
                                                                 <HeaderStyle HorizontalAlign="Center" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Left" DataField="Discount" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="Discount">
                                                                <ItemTemplate>
                                                                   <asp:HiddenField ID="HType" runat="server" Value='<%# Bind("DisType")%>' ></asp:HiddenField>
                                                                    <asp:HiddenField ID="HDisVal" runat="server" Value='<%# Bind("Discount")%>' ></asp:HiddenField>
                                                                     <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("DisType")%>' ></asp:HiddenField>
                                                                    <asp:HiddenField ID="hDC" runat="server" Value='<%# Bind("DC")%>' ></asp:HiddenField>
                                                                    <asp:Label ID="lbl_DisCount" Text="" runat="server"  ></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        
                                                    <%--           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="Discount" HeaderText="Discount" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="Discount"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>--%>

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right" DataField="ItemCost" HeaderText="Original Value" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="ItemCost"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                               <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Right"  UniqueName="ItemPrice" DataField="ItemPrice" HeaderText="Discounted Value" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="ItemPrice" FooterText="Net Amount : "   Aggregate="Sum"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridCalculatedColumn HeaderText="Total Price" UniqueName="TotalPrice" DataType="System.Double" Visible="false" ItemStyle-HorizontalAlign="Right"
                                                            DataFields="ItemPrice, Discount" Expression="{0}" FooterText="Total : "
                                                            Aggregate="Sum" />

                                                             <telerik:GridCalculatedColumn HeaderText="Total Price" UniqueName="CalDiscount" DataType="System.Double" Visible="false"
                                                            DataFields="ItemPrice" Expression="{0}*1.00000" 
                                                            Aggregate="Sum" />

                                                            <telerik:GridBoundColumn Visible="false"  HeaderStyle-HorizontalAlign="Right" DataField="DisType" HeaderText="DisType" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="DisType" FooterText="Net Amount : "   Aggregate="First"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn  Visible="false"   HeaderStyle-HorizontalAlign="Right" DataField="DC" HeaderText="DC" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="DC" FooterText="Net Amount : "   Aggregate="First"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn  Visible="false"   HeaderStyle-HorizontalAlign="Right" DataField="OrdDisValue" HeaderText="OrdDisValue" ItemStyle-HorizontalAlign="Right"
                                                                  SortExpression ="OrdDisValue" FooterText="Net Amount : "   Aggregate="First"  >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="SalesRep_Name"
                                                                  SortExpression ="SalesRep_Name" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Creation_Date" HeaderText="Creation_Date"
                                                                  SortExpression ="Creation_Date" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_No" HeaderText="Customer_No"
                                                                  SortExpression ="Customer_No" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Customer_Name" HeaderText="Customer_Name"
                                                                  SortExpression ="Customer_Name" Visible="false" Aggregate="First"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn UniqueName="Order_Amt" HeaderStyle-HorizontalAlign="Left" DataField="Order_Amt" HeaderText="Order_Amt"
                                                                  SortExpression ="Order_Amt" Visible="false" Aggregate="Sum"   >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                        </Columns>

                                                     <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField FieldAlias="RefNo" FieldName="Orig_Sys_Document_Ref"   
                                                                      ></telerik:GridGroupByField>
                                                              
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField FieldName="Orig_Sys_Document_Ref" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
<%--                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                     
                                                                <telerik:GridGroupByField FieldAlias="Item" FieldName="SalesRep_Name"  
                                                                   ></telerik:GridGroupByField>
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                             
                                                                     <telerik:GridGroupByField FieldName="SalesRep_Name" >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>--%>
                                                    </GroupByExpressions>
                                    
                                                    <GroupFooterTemplate>
                                                      
                                                        <td>
                                                            <asp:HiddenField ID="hfDisType" runat="server" Value='<%# Eval("DisType")%>' ></asp:HiddenField>
                                                            <asp:HiddenField ID="hfDC" runat="server" Value='<%# Eval("DC")%>' ></asp:HiddenField>  
                                                            <asp:HiddenField ID="hfDisValue" runat="server" Value='<%# Eval("OrdDisValue")%>' ></asp:HiddenField>   
                                                            <asp:HiddenField ID="hfOrderAmount" runat="server" Value='<%# Eval("Order_Amt")%>' ></asp:HiddenField>       
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                       <td align="right">                                                         
                                                              Net Amount1 : 
                                                            <asp:Label ID="lbl_ItemPrice" runat="server" Text='<%# Eval("ItemPrice")%>'>
                                                            </asp:Label>
                                                         </td>
                                                       <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                            <td></td>
                                                            <td></td>
                                                           <td align="right" style="background-color:papayawhip">                                                             
                                                         <b> Order  Discount : </b>
                                                            <asp:Label Font-Bold="true"  ID="lbl_Discount" runat="server" Text='0'>
                                                            </asp:Label>
                                                         </td>
                                                       </tr>
                                                      <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                            <td></td>
                                                            <td></td>
                                                          <td align="right" style="background-color:honeydew">                                                            
                                                            <b> Total :</b>
                                                            <asp:Label Font-Bold="true"  ID="lbl_Total" runat="server" Text='0'>
                                                            </asp:Label>
                                                         </td>
                                                       </tr>
                                                    </GroupFooterTemplate>
                                                    
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                               
                           <asp:HiddenField ID="hfCurrency" runat="server" Value='0' ></asp:HiddenField>                                 
                            <asp:HiddenField ID="hfDecimal" runat="server" Value='0' ></asp:HiddenField>

                              </div>
                      

                    </ContentTemplate>
                </asp:UpdatePanel>


                <div style="display: none">
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
                    <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">

                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align: middle;" />
                            <span style="font-size: 12px; font-weight: bold; color: #3399ff;">Processing... </span>
                        </asp:Panel>



                    </ProgressTemplate>
                </asp:UpdateProgress>

    <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove"  ShowBackButton ="true"  Visible="false" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>      
</asp:Content>
