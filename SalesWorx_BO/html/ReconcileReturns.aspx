<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ReconcileReturns.aspx.vb" Inherits="SalesWorx_BO.ReconcileReturns" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
   <style>
       input.rdfd_[type="text"] { height:0 !important; padding:0 !important; }
       .rgSelectedRow:hover td {
           background: #828282 !important;
       }
       .rgSelectedRow td {
            background: #828282 !important;
        }
       #ctl00_MainContent_MPSettle_C {
           height: auto !important;
           padding: 20px 0;
       }
       .rwWindowContent {
           vertical-align: middle !important;
       }
       .rwTable {
           height: auto !important;
       }
       .RadGrid .rgFilterRow{
           display:none;
       }
   </style>
    <script>
        function RowSelectedReturn(sender, eventArgs) {
            
            var grid = sender;
            var MasterTable = grid.get_masterTableView(); var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "Pending_Amount");
            findSettlementAmount(cell.innerHTML)
        }

        function RowSelectedOrders(sender, eventArgs) {
            var grid = sender;
            var MasterTable = grid.get_masterTableView(); var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cell = MasterTable.getCellByColumnUniqueName(row, "Pending_Amount");
            findSettlementAmount(cell.innerHTML)
        }
        function findSettlementAmount(v) {
            var oldval = $("#ctl00_ContentPlaceHolder1_txtAmt").val()
            if(oldval=="")
                $("#ctl00_ContentPlaceHolder1_txtAmt").val(parseFloat(v))
            else
            $("#ctl00_ContentPlaceHolder1_txtAmt").val(Math.min(parseFloat(oldval), parseFloat(v)))
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
    function alertCallBackFn(arg) {
         
    }
    </script>

    </asp:Content>
 
     <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Reconciliation of Returns</h4>
                                             
                     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
                <asp:UpdatePanel ID="ClassUpdatePnl1" runat="server"  UpdateMode="conditional">
                    <ContentTemplate>
                        
                        
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Maroon"></asp:Label>
                                  <div class="row">
          <div class="col-sm-6 col-md-4 col-lg-3">
                             <div class="form-group">  <label>Organization</label>
                                                        
                                                        
                                                            <telerik:RadComboBox Skin="Simple" ID="ddlOrganization"  Width ="100%" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true" Filter="Contains" ></telerik:RadComboBox>
                               </div>
              </div>
               <div class="col-sm-6 col-md-4">
                             <div class="form-group">                        
                                <label>Customer</label>
                                                        
                                                        
                                                            <telerik:RadComboBox Skin="Simple" ID="ddl_customer"  Width ="100%"  EnableLoadOnDemand="true"  EmptyMessage ="Please enter Customer code/Name"
                    runat="server" AutoPostBack="true" Filter="Contains"  ></telerik:RadComboBox>
                                                        </div>
              </div>
                                      
          <div class="col-sm-6 col-md-4">
              <div class="row">
                  <div class="col-sm-6">
                             <div class="form-group">  <label>From Date</label>
                                    
                                   <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%">
                                                    <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                    </DateInput>
                                                    <Calendar ID="Calendar2" runat="server">
                                                        <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                    </Calendar>
                                                </telerik:RadDatePicker>                          
                    </div>             
                  </div>
                      <div class="col-sm-6">
                             <div class="form-group"> <label>To Date</label>
                                                         
                                                           <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%">
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
        
                  <div class="col-sm-6 col-lg-1">
                      <div class="form-group">
                          <label class="hidden-xs hidden-md">&nbsp;</label>
                          <asp:Button ID="Btn_Search" runat="server" CssClass="btn btn-primary" Text="Search" /></div>
                      </div>
                            </div>
                                    
                                         <asp:Panel ID="Pnl_details" runat="server" Visible="false">
                                        <div class="row">
                                             <div class="col-sm-6">
                                            <p><asp:Label ID="Ret" Text="List of Returns and Collections" runat="server"></asp:Label>
                                                <span style="text-align:right">
                                                <asp:TextBox ID="txt_returnfilter" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btn_returnFilter" runat="server" Text="Filter"  CssClass="btn btn-warning"></asp:Button>&nbsp;<asp:Button ID="btn_ClearReturn" runat="server" Text="Clear"  CssClass="btn btn-warning"></asp:Button>
                                                    </span>
                                            </p>
                                            
                                                <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" BorderStyle="Solid"
                                        BorderWidth="1px" Width="100%" BorderColor="#cccccc">
                                        <telerik:RadGrid ID="Grd_Returns" runat="server" CssClass="tablecellalign" AllowRowSelect="true" allowmultirowselection="false" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="false" PageSize="10" 
                                            DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                            <GroupingSettings CaseSensitive="false" />
                                            <mastertableview width="100%" summary="RadGridtable" EditMode="InPlace" AllowFilteringByColumn="true" 
                                            DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                            <NoRecordsTemplate><div>There are no returns to display</div></NoRecordsTemplate>
                                            <Columns>
                                            <telerik:GridBoundColumn  UniqueName="Orig_Sys_Document_Ref" DataField="Orig_Sys_Document_Ref" HeaderText ="RefNo." 
                                             AllowFiltering="false" ShowFilterIcon="false" AutoPostBackOnFilter="true" ><HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Invoice_Date"  DataField="Invoice_Date" HeaderText ="Date" DataFormatString="{0:dd-MMM-yyyy}"
                                              AllowFiltering="false"  ShowFilterIcon="false" ><HeaderStyle Width="200px" />
                                              </telerik:GridBoundColumn><telerik:GridBoundColumn UniqueName="Pending_Amount"  DataFormatString="{0:###0.00}"
                                              DataField="Pending_Amount" HeaderText ="Amount"  AllowFiltering="false" ShowFilterIcon="false" >
                                              <HeaderStyle Width="150px" /></telerik:GridBoundColumn></Columns></mastertableview><ClientSettings>
                                              <Selecting AllowRowSelect="True"></Selecting>
                                               <ClientEvents OnRowSelected="RowSelectedReturn" />
                                              </ClientSettings>
                                              <pagerstyle mode="NextPrevAndNumeric">
                                              </pagerstyle>
                                              </telerik:RadGrid>
                                    </asp:Panel>
                                            </div>
                                            <div class="col-sm-6">
                                           <p><asp:Label ID="Label1" Text="List of Invoices" runat="server"></asp:Label>
                                               <span style="text-align:right">
                                                <asp:TextBox ID="txt_SalesFilter" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btn_SalesFilter" runat="server" Text="Filter"  CssClass="btn btn-warning"></asp:Button>&nbsp;<asp:Button ID="Btn_clearSales" runat="server" Text="Clear"  CssClass="btn btn-warning"></asp:Button>
                                                    </span>
                                           </p>
                                                <asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Auto" BorderStyle="Solid"
                                        BorderWidth="1px"  Width="100%" BorderColor="#cccccc">
                                        <telerik:RadGrid ID="Grd_Orders" runat="server"  CssClass="tablecellalign" AllowRowSelect="true" allowmultirowselection="false" 
                                            AutoGenerateColumns="False"  AllowPaging="true" AllowFilteringByColumn="false" PageSize="10"
                                             DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                             <GroupingSettings CaseSensitive="false" />
                                             <mastertableview width="100%" summary="RadGrid table" EditMode="InPlace" 
                                             AllowFilteringByColumn="true" DataKeyNames="Orig_Sys_Document_Ref"  ClientDataKeyNames="Orig_Sys_Document_Ref">
                                             <NoRecordsTemplate><div>There are no Invoices to display</div></NoRecordsTemplate>
                                             <Columns>
                                             <telerik:GridBoundColumn  UniqueName="Orig_Sys_Document_Ref" DataField="Orig_Sys_Document_Ref" 
                                             HeaderText ="RefNo."   AllowFiltering="false" ShowFilterIcon="false" AutoPostBackOnFilter="true" >
                                             <HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Invoice_Date"  DataField="Invoice_Date" HeaderText ="Date"  DataFormatString="{0:dd-MMM-yyyy}"
                                             AllowFiltering="false" ShowFilterIcon="false" ><HeaderStyle Width="200px" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn UniqueName="Pending_Amount"  DataField="Pending_Amount" HeaderText ="Amount"  DataFormatString="{0:###0.00}"
                                             AllowFiltering="false" ShowFilterIcon="false" >
                                             <HeaderStyle Width="150px" />
                                             </telerik:GridBoundColumn>
                                             </Columns></mastertableview>
                                             <ClientSettings><Selecting AllowRowSelect="True"></Selecting>
                                             <ClientEvents OnRowSelected="RowSelectedOrders" />
                                             </ClientSettings>
                                             <pagerstyle mode="NextPrevAndNumeric"></pagerstyle></telerik:RadGrid>
                                    </asp:Panel>
                                                <div>
                                                    <p>&nbsp;</p>
                                                    <div class="form-inline">
                                                        <asp:Label ID="lblMsg1" runat="server" Text="Settlement Amount " ></asp:Label>
                                                        <asp:TextBox ID="txtAmt" runat="server" CssClass="inputSM" onKeypress='return NumericOnly(event)'></asp:TextBox>
                                                        <asp:Button ID="Btn_Save" runat="server" CssClass ="btn btn-primary" Text="Save" />
                                                    </div>
                                                </div>
                                                </div>
                                        </div>
                                        </asp:Panel>
                                    
                                
                                                
                              <telerik:RadWindow ID="MPSettle" Title= "Confirmation" runat="server" Skin="Windows7" Behaviors="Move,Close"
                                                Width="450px" Height="210px" ReloadOnShow="false" VisibleStatusbar="false" Overlay="true" Modal="true">
                                              <ContentTemplate>
                            <table id="table1" width="400" cellpadding="10" style="background-color: White;">
                                
                                <tr>
                                    <td align="center" style="text-align: center">
                                       <asp:Label ID="Label3" runat="server"  Font-Size ="13px"  Text="Are you sure to settle the amount?" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btn_Yes" runat="server" Text="Yes"  CssClass="btn btn-success"  />
                                        <asp:Button ID="btnCloseconfirm" runat="server" Text="No"  CssClass ="btn btn-danger"  />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                               
                            
                                                    </telerik:RadWindow> 
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
             
    
     <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="ClassUpdatePnl1"
        runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                <img src="../images/Progress.gif" alt="Processing..." style="padding-left: 400px" />
                <span style="font-size: 12px; color: #666;">Processing... </span>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>

