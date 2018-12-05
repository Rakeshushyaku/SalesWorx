<%@ Page Title="Request Stock" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RequestStock.aspx.vb" Inherits="SalesWorx_BO.RequestStock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">

<style type="text/css">

.rcbSlide
{
	z-index: 100002 !important;
}

 div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   } 
</style>
    <script type="text/javascript">

        function pageLoad(sender, eventArgs) {

            if (!eventArgs.get_isPartialLoad()) {

                $find("<%= RadAjaxManager2.ClientID%>").ajaxRequest("InitialPageLoad");

        }

    }
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {

            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
        postBackElement.disabled = true;
    }

    function EndRequest(sender, args) {
        $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
        postBackElement.disabled = false;
    }
        function alertCallBackFn(arg) {

        }
        </script>
  
     </asp:Content>
     <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   



    <h4>Request Stock </h4>
         <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                        
                       </UpdatedControls>
                   </telerik:AjaxSetting>
               </AjaxSettings>
           </telerik:RadAjaxManager>
        <%--<asp:Timer runat="server" ID="Timer1" OnTick="Unnamed1_Tick"></asp:Timer>--%>
            
           <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>

         <asp:Panel ID="Panel2" runat="server"></asp:Panel> 
                   
          
                
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                    
                                       <asp:Timer runat="server" ID="Timer1" OnTick="Unnamed1_Tick"></asp:Timer>
                                   
                                     
                                     <telerik:RadTabStrip ID="RequestStocktab" runat="server" MultiPageID="RadMultiPage21" SelectedIndex="0" Skin="Windows7">
                                         <tabs>
                                             <telerik:RadTab runat="server" Text="Request">
                                             </telerik:RadTab>
                                             <telerik:RadTab runat="server" Text="Search">
                                             </telerik:RadTab>
                                             <telerik:RadTab runat="server" Text="" Visible="false">
                                             </telerik:RadTab>
                                         </tabs>
                                     </telerik:RadTabStrip>
                                     <br />
                                     <telerik:RadMultiPage ID="RadMultiPage21" runat="server" SelectedIndex="0">
                                         <telerik:RadPageView ID="RadPageView1" runat="server">
                                           <div class="row">
                                        <div class="col-sm-4">
                                            <label>Organization</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox ID="ddl_org" runat="server" AutoPostBack="true" Filter="Contains" Skin="Simple" width="100%"></telerik:RadComboBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Van</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox ID="ddlVan" runat="server" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" Filter="Contains" Skin="Simple" width="100%"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="hidden-xs"><br /></label>
                                            <div class="form-group">
                                                
                                               <asp:Button ID="BTn_Request" runat="server" CssClass="btn btn-warning" TabIndex="4" Text="Request"></asp:Button>
                                                
                                             
                                            </div>
                                        </div>
                                    </div>
                                             </telerik:RadPageView>
                                         <telerik:RadPageView ID="RadPageView2" runat="server">
                      
                      <div class="row">
                                        <div class="col-sm-4">
                                            <label>Organization</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox ID="ddl_org1" runat="server" AutoPostBack="true" Filter="Contains" Skin="Simple" width="100%"></telerik:RadComboBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Van</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox ID="ddlVan1" runat="server" CheckBoxes="true" EmptyMessage="Select Van" EnableCheckAllItemsCheckBox="true" Filter="Contains" Skin="Simple" width="100%"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                       
                                    </div>
                      <div class="row">

                                                       <div class="col-sm-2">
                                                           <div class="form-group">
                                                               <label>From Date</label>

                                                               <telerik:RadDatePicker ID="txtFromDate" runat="server">
                                                                   <dateinput displaydateformat="dd-MMM-yyyy" readonly="true">
                                                                   </dateinput>
                                                                   <calendar id="Calendar2" runat="server">
                                                                       <fastnavigationsettings okbuttoncaption="     OK    " todaybuttoncaption="Current Month" />
                                                                   </calendar>
                                                               </telerik:RadDatePicker>
                                                           </div>
                                                       </div>
                                                       <div class="col-sm-2">
                                                           <div class="form-group">
                                                               <label>To Date</label>
                                                               <telerik:RadDatePicker ID="txtToDate" runat="server">
                                                                   <dateinput displaydateformat="dd-MMM-yyyy" readonly="true">
                                                                   </dateinput>
                                                                   <calendar id="Calendar1" runat="server">
                                                                       <fastnavigationsettings okbuttoncaption="     OK    " todaybuttoncaption="Current Month" />
                                                                   </calendar>
                                                               </telerik:RadDatePicker>
                                                           </div>
                                                       </div>
                                                       <div class="col-sm-4">
                                                           <div class="form-group">
                                                             <label>
                                                    Status
                                                </label>


                                                <telerik:RadComboBox ID="ddl_Status" runat="server" CheckBoxes="true" EmptyMessage="Select Status" EnableCheckAllItemsCheckBox="true" Filter="Contains" Skin="Simple" Width="100%">
                 </telerik:RadComboBox>
                                                           </div>
                                                       </div>
                                                <div class="col-sm-4">
                                            <label class="hidden-xs"><br /></label>
                                            <div class="form-group">
                                                
                                               <asp:Button ID="SearchBtn" runat="server" CssClass="btn btn-primary" TabIndex="4" Text="Search"></asp:Button>
                                                
                                            <asp:Button  CssClass ="btn btn-sm btn-default"  ID="ClearBtn" runat="server" Text="Clear"  />
                                            </div>
                                        </div>
                                                   </div>
                 </telerik:RadPageView>
                                     </telerik:RadMultiPage>
                                     <%-- <div class="row">
                                        <div class="col-sm-4">
                                            <label>Organization</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple"  ID="ddl_org" Filter="Contains"   runat="server" width="100%" AutoPostBack="true" ></telerik:RadComboBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label>Van</label>
                                            <div class="form-group">
                                                <telerik:RadComboBox Skin="Simple" ID="ddlVan" EmptyMessage="Select Van" Filter="Contains" runat="server" width="100%" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="hidden-xs"><br /></label>
                                            <div class="form-group">
                                                
                                               <asp:Button id="BTn_Request" runat="server" CssClass="btn btn-warning" Text="Request" TabIndex="4" ></asp:Button>
                                                
                                             
                                            </div>
                                        </div>
                                    </div>--%>
                                     <%--  <div class="row">

                                                       <div class="col-sm-2">
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
                                                       <div class="col-sm-2">
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
                                                       <div class="col-sm-4">
                                                           <div class="form-group">
                                                             <label>
                                                    Status
                                                </label>


                                                <telerik:RadComboBox Skin="Simple"  EmptyMessage="Select Status" Filter="Contains"   CheckBoxes="true" EnableCheckAllItemsCheckBox="true" ID="ddl_Status" Width ="100%" 
                    runat="server" >
                 </telerik:RadComboBox>
                                                           </div>
                                                       </div>
                                                <div class="col-sm-4">
                                            <label class="hidden-xs"><br /></label>
                                            <div class="form-group">
                                                
                                               <asp:Button id="SearchBtn" runat="server" CssClass="btn btn-primary" Text="Search" TabIndex="4" ></asp:Button>
                                                
                                           
                                            </div>
                                        </div>
                                                   </div>--%>
                                     <div class="table-responsive">
                                         <telerik:RadGrid ID="rgStockRequests" runat="server" AllowFilteringByColumn="false" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="False" autogeneratedcolumns="false" GridLines="None" PageSize="10" Skin="Simple">
                                             <GroupingSettings CaseSensitive="false" />
                                             <clientsettings>
                                             </clientsettings>
                                             <mastertableview allowfilteringbycolumn="false" pagesize="10" summary="RadGrid table" tablelayout="fixed">

                                                 <norecordstemplate>
                                                     <div>
                                                         There are no records to display
                                                     </div>
                                                 </norecordstemplate>
                                                 <sortexpressions>
                                                     <telerik:GridSortExpression FieldName="Logged_At" SortOrder="Descending" />
                                                 </sortexpressions>
                                                 <Columns>
                                                     <telerik:GridBoundColumn AllowFiltering="false" DataField="SalesRep_Name" HeaderText="VAN" ShowFilterIcon="false" UniqueName="VAN">
                                                         <ItemStyle Wrap="false" />
                                                         <HeaderStyle Width="40px" Wrap="false" />
                                                     </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Logged_At" DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" HeaderText="Date Request Created" ShowFilterIcon="false" UniqueName="DateCreated">
                                                         <ItemStyle Wrap="false" />
                                                         <HeaderStyle Width="80px" Wrap="false" />
                                                     </telerik:GridBoundColumn>
                                                     <telerik:GridBoundColumn AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Status" HeaderText="Status" ShowFilterIcon="false" UniqueName="status">
                                                         <ItemStyle Wrap="false" />
                                                         <HeaderStyle Width="90px" Wrap="false" />
                                                     </telerik:GridBoundColumn>
                                                 </Columns>
                                             </mastertableview>
                                             <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                             <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                         </telerik:RadGrid>
                                     </div>
                                     <%-- <%-- <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:SWXConnectionString %>"
                                                                    SelectCommand="Rep_StockRequestList" SelectCommandType="StoredProcedure">--%><%-- </asp:SqlDataSource>--%>
                                     <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                     <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground" Drag="true" PopupControlID="pnlConfirm" TargetControlID="btnHidden" />
                                     <asp:Panel ID="pnlConfirm" runat="server" CssClass="modalPopup" height="500px" Style="display: none" Width="700">
                                         <div class="panelouterblk">
                                             <asp:Panel ID="Panel1" runat="server" CssClass="popupbartitle">
                                                 Information</asp:Panel>
                                             <asp:ImageButton ID="ImageButton1" runat="server" CssClass="Closebtnimg" ImageUrl="~/assets/img/close.jpg" Visible="false" />
                                             <table id="tableinPopupErr" cellpadding="10" style="background-color: White;" width="690">
                                                 <tr align="center">
                                                     <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                         <asp:Label ID="lblinfo" runat="server" Text="Stock Requests Status"></asp:Label>
                                                    <%-- Stock Request can't be created for following VAN's as &lt;br/&gt; there is already a pending request--%>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td align="center" style="text-align: left">
                                                         <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                         <div style="height:370px;overflow-y: auto;">
                                                             <asp:GridView ID="gv_InvalidStock" runat="server" CssClass="tablecellalign">
                                                             </asp:GridView>
                                                         </div>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td align="center" style="text-align: center;">
                                                         <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Cancel" />
                                                     </td>
                                                 </tr>
                                             </table>
                                         </div>
                                     </asp:Panel>
                                     <%--<br>
                                     <br>
                                     <br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     <br></br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>
                                     </br>--%>



                                </ContentTemplate>
                                <Triggers>
                                
                                <%-- <asp:PostBackTrigger ControlID="BTn_Export" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        
                           <asp:UpdateProgress ID="UpdateProgress1" 
                    runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                            <span style="font-size: 12px; color:  #337AB7  ;">Processing... </span>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
</asp:Content>
