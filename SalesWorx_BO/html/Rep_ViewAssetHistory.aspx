<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_ViewAssetHistory.aspx.vb" Inherits="SalesWorx_BO.Rep_ViewAssetHistory" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        function alertCallBackFn(arg) {

        }
        
       
        function OpenWindow(Asset_ID, Row_ID,LoggedAt) {
                  
       
               

  
                var URL
                URL = 'ShowAssetHistoryImages.aspx?Asset_ID=' + Asset_ID + '&Row_ID=' + Row_ID + '&CustName=' + $("#MainContent_lbl_CusName").text() + '&AssetType=' + $("#MainContent_lbl_AssetType").text() + '&LoggAt=' + LoggedAt;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(950, 600);
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
    <h4><a href="RepAssets.aspx?b=1"  title="Back"><i class="fa fa-arrow-circle-o-left"></i></a>View Asset History</h4>

    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

   <asp:HiddenField ID="hfAssetID" runat="server" />
    <asp:HiddenField ID="hfOrgID" runat="server" />
    <asp:HiddenField ID="hfDecimal" runat="server" />
      <asp:HiddenField ID="hFromDate" runat="server" />
      <asp:HiddenField ID="hToDate" runat="server" />

    

    <div style="display: none">
                               <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                           </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
            
            <div id="summary" runat="server">
         

                 <div class="row">
                                           

                  
             
                                  <div class="col-sm-6 col-md-4 col-lg-3">
                                                    <div class="form-group">
                                                        <label>From Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                          <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
                                                    </div>
                                                  </div> 
                      <div class="col-sm-6 col-md-4 col-lg-3">
                                <div class="form-group">
                                    <label class="hidden-xs">&nbsp;</label>
                                    <asp:Button  CssClass ="btn btn-sm btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
                                                   <%-- <asp:Button  CssClass ="btn btn-sm btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />--%>
                                    </div>
                          </div>
              
        

                                               


                       <div class="col-sm-6 col-md-12 col-lg-3">
                             
                                                   
                           <div class="form-group text-right">
                                                    <label class="hidden-md">&nbsp;</label>
                                               <a href="javascript:clickExportExcel()" class="btn btn-sm btn-success"><i class="fa fa-file-excel-o"></i> Excel</a>
                                                   <a href="javascript:clickExportPDF()" class="btn btn-sm btn-danger"><i class="fa fa-file-pdf-o"></i> PDF</a>                                                
                                            </div>
                                                        
                                 

                             </div>
                     </div>
           
                <hr style="margin: 0px 0 15px;" />
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
                    <label>Asset Type</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_AssetType" runat="server"></asp:Label></p>
                </div>
              </div>
         <div class="row">
                <div class="col-sm-4">
                    <label>Asset Code</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_AssetCode" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Description</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_Description" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>Change Type</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_ChangeType" runat="server"></asp:Label></p>
                </div>
            </div>

                  <div class="row">
                <div class="col-sm-4">
                    <label>Data Period</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_dataPeriod" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>From Date</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_FromDate" runat="server"></asp:Label></p>
                </div>
                <div class="col-sm-4">
                    <label>To Date</label>
                    <p class="font-weight-600"><asp:Label ID="lbl_ToDate" runat="server"></asp:Label></p>
                </div>
            </div>
            </div>

                            <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="3" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="3">


                                         <PagerStyle PageSizeControlType="None" Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                        
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ChangeType" HeaderText="Change Type"
                                                                  SortExpression ="ChangeType" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Condition" HeaderText="Condition"
                                                                  SortExpression ="Condition" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Presence" HeaderText="Presence"
                                                                  SortExpression ="Presence" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OtherNotes" HeaderText="OtherNotes" SortExpression ="OtherNotes">
                                                                <ItemStyle Wrap="False" />

                                                            </telerik:GridBoundColumn>


                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Center" DataField="AssetValue" HeaderText="Asset Value"
                                                                  SortExpression ="AssetValue" UniqueName="AssetValue" DataType="System.Decimal" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False"  HorizontalAlign="Right"  />
                                                           </telerik:GridBoundColumn>

                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="ChangedIn" HeaderText="Modified In"
                                                                  SortExpression ="ChangedIn" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Logged_At" HeaderText="Logged On"
                                                                  SortExpression ="Logged_At" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Logged_By" HeaderText="Logged By"
                                                                  SortExpression ="Logged_By" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn  HeaderStyle-HorizontalAlign="Left"    HeaderStyle-Wrap="false" HeaderStyle-ForeColor="#0090d9"  
                                                                HeaderText="View Image" >
                                                            <ItemTemplate>
                                                                                                                    
                                                                <asp:HiddenField runat="server" ID="HRow_ID" Value='<%# Bind("Row_ID")%>' />
                                                                <asp:HiddenField runat="server" ID="HAsset_ID" Value='<%# Bind("Asset_ID")%>' />
                                                                 <asp:HiddenField runat="server" ID="HLoggedAt" Value='<%# Bind("Logged_At")%>' />
                                                                <asp:LinkButton ID="Lnk_RowID" runat="server"  Text='View' Visible='<%# Bind("VisibleView")%>' ForeColor="SteelBlue" Font-Underline="true"  OnClientClick='<%# String.Format("OpenWindow(""{0}"",""{1}"",""{2}"");", Eval("Asset_ID"), Eval("Row_ID"), Eval("Logged_At"))%>'  ></asp:LinkButton>

                                                                 </ItemTemplate>
                                                        </telerik:GridTemplateColumn>






                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           
                           
           
     
     
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
