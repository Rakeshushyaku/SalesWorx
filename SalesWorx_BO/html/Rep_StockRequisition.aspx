<%@ Page Title="Stock Requisition" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_StockRequisition.aspx.vb" Inherits="SalesWorx_BO.Rep_StockRequisition" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style2 {
            width: 240px;
        }
        div[id*="ReportDiv"] {
            overflow: hidden !important;
            WIDTH: 100%;
            direction: ltr;
        }
    </style>

        <script type="text/javascript">
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


            function OpenViewWindow(cid) {
                var combo = $find('<%=ddlOrganization.ClientID%>');
            var URL
            URL = 'RepDetails.aspx?OrgID=' + combo.get_selectedItem().get_value() + '&Type=stock&ReportName=StockRequisitionItems&ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(700, 480);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <h4>Stock Requisition</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            
                   <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Van/FSR</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Van/FSR"    ID="ddlVan" Width ="100%" runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                   
                                              
                                                   
                                                     <div class="col-sm-6">
                                                         <div class="row">
                                                             <div class="col-sm-6">
                                                               <div class="form-group">
                                                                   <label>From Date(Req. Dt.) </label>
                                                                     <telerik:RadDatePicker ID="txtfromDate" Width ="100%"  runat="server">
                                                                <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                </DateInput>
                                                                <Calendar ID="Calendar2" runat="server" >
                                                                    <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                                </Calendar>
                                                            </telerik:RadDatePicker>    

                                                                </div>
                                                              </div>
                                                               <div class="col-sm-6">
                                                               <div class="form-group">
                                                                   <label>To Date(Req. Dt.) </label>
                                                                 <telerik:RadDatePicker ID="txtToDate" Width ="100%"  runat="server">
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
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()" Visible="false"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()" Visible="false"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
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
            
            </span>
            </i>      
        </div>
    </div>


             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings  CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridTemplateColumn uniqueName="Request_Date"  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" DataField="Request_Date" SortExpression ="Request_Date"
                                                               HeaderText="Request Date"  ItemStyle-Width="25%" >
                                                            <ItemTemplate>                                                                
                                                                <asp:LinkButton  DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  ID="StockRequisition_ID" runat="server" Text='<%# Bind("Request_Date", "{0:dd-MMM-yyyy hh:mm tt}")%>' ForeColor="SteelBlue" Font-Underline="true" OnClientClick='<%# String.Format("OpenViewWindow(""{0}"");", Eval("StockRequisition_ID"))%>'    ></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                                                                                                                  
                                                             
                                                      <telerik:GridBoundColumn   HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR"
                                                                ItemStyle-Width="25%"   SortExpression ="SalesRep_Name" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn   HeaderStyle-HorizontalAlign="Left" DataField="Comments" HeaderText="Comments"
                                                                 ItemStyle-Width="50%"  SortExpression ="Comments" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                               
                                                        </Columns>
                                                                  
                                    

                                                        </MasterTableView>
                                                    </telerik:RadGrid>


      </contenttemplate>
    </asp:UpdatePanel>
        <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>

</asp:Content>
