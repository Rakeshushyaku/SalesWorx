<%@ Page Title="Outletwise - SKU wise Sales / Returns" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepOutletSKUwiseSalesReturn.aspx.vb" Inherits="SalesWorx_BO.RepOutletSKUwiseSalesReturn" %>

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

        function OpenSalViewWindow(CID, qty, SID, INID) {
            var FSR = '-1'
            if (qty > 0) {
                var param1 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param2 = document.getElementById('<%= HTo.ClientID%>').value;
                var param3 = document.getElementById('<%= HorgID.ClientID%>').value;
                var param4= document.getElementById('<%= HUID.ClientID%>').value;


                var URL
                URL = 'RepDetails.aspx?Type=OWSWSR&rtype=S&ReportName=OutletSKUwiseSales&SID=' + SID + '&From=' + param1 + '&To=' + param2 + '&Org=' + param3 + '&CID=' + CID + "&INID=" + INID + '&FID=' + FSR + '&UID=' + param4;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(850, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }

        function OpenRetViewWindow(CID, qty, SID, INID) {
           var FSR = '-1'
            if (qty > 0) {
                var param1 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param2 = document.getElementById('<%= HTo.ClientID%>').value;
                var param3 = document.getElementById('<%= HorgID.ClientID%>').value;
                var param4 = document.getElementById('<%= HUID.ClientID%>').value;
                var URL
                URL = 'RepDetails.aspx?Type=OWSWSR&rtype=R&ReportName=OutletSKUWiseReturn&SID=' + SID + '&From=' + param1 + '&To=' + param2 + '&Org=' + param3 + '&CID=' + CID + "&INID=" + INID + '&FID=' + FSR + '&UID=' + param4;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(850, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }
    </script>
    <style>
        #clsAgency.rgFooter td {
            background:#fffdef !important;
        }
        #clsOutlet.rgFooter td {
            background:#ffefef !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Outletwise - SKU wise Sales / Returns</h4>
    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Simple" runat="server" EnableShadow="true">
    </telerik:RadWindowManager>

    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">

        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxManager2" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
      
         <asp:HiddenField ID="HTo" runat="server" />
         <asp:HiddenField ID="Hfrom" runat="server" />         
         <asp:HiddenField ID="HorgID" runat="server" />
         <asp:HiddenField ID="HFSR" runat="server" />   
             <asp:HiddenField ID="HUID" runat="server" />   
                 <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>

                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="row">
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"  Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="true"> </telerik:RadComboBox>
                                            </div>
                                          </div>
          
                                                     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Outlet</label>
                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true" EmptyMessage="Select Outlet" EnableCheckAllItemsCheckBox="true" ID="ddlCustomer" Width ="100%" runat="server"  Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>

                                                         <div class="col-sm-2">
                                                       <div class="form-group">
                                                           <label>From Date </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>

                                                           <div class="col-sm-2">
                                                       <div class="form-group">
                                                           <label>To Date </label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>

                                                        </div>
                                                      </div>

                                                    </div>
                                              
                                                  <div class="row">
                                                    <div class="col-sm-4">
                                                           <div class="form-group">
                                                                <label>Agency </label>
                                                                   <telerik:RadComboBox AutoPostBack="true" Skin="Simple" EmptyMessage="Select Agency" ID="ddlAgency" Width ="100%" runat="server" DataTextField="Agency" DataValueField="Agency" Filter="Contains">
                                        </telerik:RadComboBox>
                                                           </div>
                                                      </div>
                                                

                                                      <div class="col-sm-4">
                                                           <div class="form-group">
                                                                <label>SKU</label>
                                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true"   EmptyMessage="Select SKU" EnableCheckAllItemsCheckBox="true" ID="ddSKU" Width ="100%" runat="server" Filter="Contains">
                                        </telerik:RadComboBox >
                                                           </div>
                                                      </div>
                                                       <div class="col-sm-4">
                                                           <div class="form-group">
                                                                <label>Van/FSR</label>
                                                                   <telerik:RadComboBox Skin="Simple" CheckBoxes="true"  EmptyMessage="Select Van/FSR" EnableCheckAllItemsCheckBox="true" ID="ddlVan" Width ="100%" runat="server" Filter="Contains" >
                                                         </telerik:RadComboBox >
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
                                                <asp:HyperLink href="" CssClass=""  ID="BtnExportDummyExcel" runat="server" OnClick="return clickExportExcel()"><i class="fa fa-file-excel-o text-success"></i></asp:HyperLink>
                                                <asp:HyperLink href=""  CssClass =""  ID="BtnExportDummyPDF" runat="server" OnClick="return clickExportPDF()"><i class="fa fa-file-pdf-o text-danger"></i></asp:HyperLink>
                                                
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
              <p><strong>Outlet: </strong><asp:Label ID="lbl_Outlet" runat="server" Text=""></asp:Label></p>
              <p><strong>Agency: </strong><asp:Label ID="lbl_Agency" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>SKU: </strong><asp:Label ID="lbl_SKU" runat="server" Text=""></asp:Label></p>  
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
             <div id="summary" runat="server" class="row"></div> 
            <div class="row">
             <div class="col-sm-8">
                 <div style="margin: 15px 0 10px;">
                                     <asp:Label ID="lblmsgUOM" runat="server"   Text=""></asp:Label>   
                     </div>
                                </div>
              <div id="divCurrency" runat="server" visible="false"  >
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                <asp:HiddenField runat="server" ID="hfDigit" Value="N2" />
</div></div>
                            
           
                              <div class="table-responsive">
                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server"  ShowFooter="false"
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false" ></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowGroupFooter="true"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None"></PagerStyle>
                                                        <Columns>
                                                        <%--<telerik:GridBoundColumn Visible="false" HeaderStyle-HorizontalAlign="Left" DataField="salesrep_name" HeaderText="Van"
                                                                  SortExpression ="salesrep_name" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>                                                                                                    
                                                             
                                                      <telerik:GridBoundColumn Visible="false"  HeaderStyle-HorizontalAlign="Left" DataField="Item_Code" HeaderText="Item Code"
                                                                  SortExpression ="Item_Code" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn Visible="false"  HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>--%>

                                                         <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="SKU" HeaderText="SKU (Item)"
                                                                  SortExpression ="SKU" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="UOM" HeaderText="UOM"
                                                                  SortExpression ="UOM" >
                                                                <ItemStyle Wrap="False" />
                                                             </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn UniqueName="SALES" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="SALES" SortExpression="SALES" 
                                                                HeaderText="Sales Qty" ItemStyle-HorizontalAlign="Right"   Aggregate="Sum" FooterText=" "  FooterStyle-HorizontalAlign="Right"  FooterAggregateFormatString="{0:#,##0.0000}" >
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("SaleValue").ToString()) > 0, True, False)%>' DataFormatString="{0:#,##0.0000}" OnClientClick='<%# String.Format("OpenSalViewWindow(""{0}"", ""{1}"",""{2}"",""{3}"");", Eval("Ship_To_Customer_ID"), Eval("SALES"), Eval("Ship_To_Site_ID"), Eval("Inventory_Item_ID"))%>' ID="lnkSalQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SALES", "{0:#,##0.0000}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                        </telerik:GridTemplateColumn>

                                                           <%--<telerik:GridBoundColumn DataFormatString="{0:#,##0.00}" ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="SALES" HeaderText="Sales Qty"
                                                                  SortExpression ="SALES" >
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>--%>
                                                              <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  DataField="SaleValue" HeaderText="Sales Value"
                                                                  SortExpression ="SaleValue" Aggregate="Sum" UniqueName="SaleValue" DataType="System.Decimal" DataFormatString="{0:N2}"   FooterText =" ">  
                                                                <ItemStyle Wrap="False" />
                                                            </telerik:GridBoundColumn>
                                                        
                                                         <%--  <telerik:GridBoundColumn DataFormatString="{0:#,##0.00}"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  DataField="Returns" HeaderText="Returns Qty"
                                                                  SortExpression ="Returns" >
                                                                <ItemStyle Wrap="True" />
                                                             </telerik:GridBoundColumn>--%>

                                                             <telerik:GridTemplateColumn UniqueName="Returns" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="Returns" SortExpression="Returns" 
                                                                HeaderText="Returns Qty" ItemStyle-HorizontalAlign="Right"  Aggregate="Sum" FooterText =" " FooterStyle-HorizontalAlign="Right"  FooterAggregateFormatString="{0:#,##0.0000}" >
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("ReturnValue").ToString()) > 0, True, False)%>'  DataFormatString="{0:#,##0.0000}" OnClientClick='<%# String.Format("OpenRetViewWindow(""{0}"", ""{1}"",""{2}"",""{3}"");", Eval("Ship_To_Customer_ID"), Eval("Returns"), Eval("Ship_To_Site_ID"), Eval("Inventory_Item_ID"))%>' ID="lnkRetQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Returns", "{0:#,##0.0000}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                           </telerik:GridTemplateColumn>

                                                              <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  DataField="ReturnValue" HeaderText="Returns Value"
                                                                  SortExpression ="ReturnValue" UniqueName="ReturnValue" DataType="System.Decimal" DataFormatString="{0:N2}"  Aggregate="Sum" FooterText =" ">
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  DataField="NetSalesQty" HeaderText="Net Sales Qty"
                                                                  SortExpression ="NetSalesQty" DataFormatString="{0:#,##0.0000}" Aggregate="Sum" FooterText =" ">
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"  DataField="NetSales" HeaderText="Net Sales"
                                                                  SortExpression ="NetSales" UniqueName="NetSales" DataType="System.Decimal" DataFormatString="{0:N2}" Aggregate="Sum" FooterText =" ">
                                                                <ItemStyle Wrap="True" />
                                                            </telerik:GridBoundColumn>


                                                             <telerik:GridCalculatedColumn UniqueName="RetValPers" ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center"  HeaderText="Returns %"
                                                              DataFields="ReturnValue, SaleValue"  Expression="  iif({1} = 0, 0, ({0}/{1})* 100 )   "    DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridCalculatedColumn>

                                                          
                                                        </Columns>

                                                     <GroupByExpressions>

                                                         <telerik:GridGroupByExpression>
                                                            <SelectFields>
                                                                <telerik:GridGroupByField  FieldAlias="Outlet" FieldName="Outlet"   
                                                                   ></telerik:GridGroupByField>
                                                   
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                                     <telerik:GridGroupByField  FieldName="Outlet" >

                                                                     </telerik:GridGroupByField>
                                                         
                                                                </GroupByFields>
                                                             
                                                        </telerik:GridGroupByExpression>
                                                         <telerik:GridGroupByExpression >
                                                            <SelectFields >
                                                     
                                                                <telerik:GridGroupByField FieldAlias="Agency" FieldName="Agency"   
                                                                   ></telerik:GridGroupByField>
                                                            </SelectFields>
                                                                 <GroupByFields>
                                                             
                                                                     <telerik:GridGroupByField FieldName="Agency"  >

                                                                     </telerik:GridGroupByField>
                                                                </GroupByFields>
                                                        </telerik:GridGroupByExpression>
                                                    </GroupByExpressions>
                                    

                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                              </div>
        
       <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>
    </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
        <progresstemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
            
       
         
    </progresstemplate>
    </asp:UpdateProgress>

</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
		<div class="pgtitileposition">
	<span class="pgtitile3">Outletwise - SKU wise Sales / Returns</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="table1">
	<tr>
<td style="padding:6px 12px">
<table  border="0" cellspacing="2" cellpadding="2" >
     <tr> 
          <td class="txtSMBold">
             Organization :
               
            </td>  
            <td> <asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="200px" AutoPostBack=true
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList></td>
            <td  class="txtSMBold">Outlet :</td>
            <td>
                     <telerik:RadComboBox ID="ddlCustomer" Width="250px" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                CssClass="inputSM"      >
                       </telerik:RadComboBox>
               &nbsp;
                        </td>            
              
          <td>
              &nbsp;</td>
          <td>
              <asp:Button ID="btnSearch" runat="server" CssClass="btnInput" Text="Search" />
          </td>
              
          </tr>  
          <tr> 
            <td  class="txtSMBold">From Date :</td>
            <td>
                <asp:TextBox  ID="txtFromDate"  Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="calendarButtonExtender" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtFromDate" PopupButtonID="txtFromDate"  />                
            </td>
            <td  class="txtSMBold">To Date :</td>
            <td>
                <asp:TextBox  ID="txtToDate"  Width ="150px" CssClass="inputSM" runat="server"></asp:TextBox>&nbsp;
                <ajaxToolkit:CalendarExtender  CssClass="cal_Theme1" ID="CalendarExtender1" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtToDate" PopupButtonID="txtToDate"  />                
            &nbsp;
                  
            </td>       
              <td>
                  &nbsp;</td>
              <td>
                  &nbsp;</td>
          </tr>   
          <tr> 
               <td class="txtSMBold">
                 
               
                   Agency:</td>  
            <td>  
                <asp:DropDownList CssClass="inputSM" ID="ddlAgency"   Width ="150px"
                runat="server" DataTextField="Agency" DataValueField="Agency" AutoPostBack="true">
                </asp:DropDownList></td>  
            <td  class="txtSMBold">SKU :</td>
            <td>
           <telerik:RadComboBox ID="ddSKU" Width="250px" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                CssClass="inputSM"      >
                       </telerik:RadComboBox>
               
            </td>            
          
               <td class="txtSMBold">
                   Van:</td>
               <td>
                   <telerik:RadComboBox ID="ddlVan" Width="250px" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" 
                CssClass="inputSM"      >
                       </telerik:RadComboBox>
               </td>
          
          </tr>         
        </table>
 
 </ContentTemplate> </asp:UpdatePanel> 
  
   <asp:UpdatePanel ID="UPModal" runat="server">
                                <ContentTemplate>
                                    <table width="auto" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
                                                <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                                                    TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                                                    Drag="true" />
                                                <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="cursor: move;
                                                    background-color: #3399ff; text-align: center; border: solid 1px #3399ff; color: White;
                                                    padding: 3px; display: none;">
                                                    <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                                        <tr align="center">
                                                            <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                                                border: solid 1px #3399ff; color: White; padding: 3px">
                                                                <asp:Label ID="lblinfo" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center">
                                                                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                                                <asp:Label ID="lblMessage" runat="server" Font-Size ="13px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                            <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
	<tr>
       <td style="background:#ffffff">
           <rsweb:ReportViewer ID="RVMain" runat="server" BorderStyle="Groove"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>      
       </td>              
	</tr>
  
    </table>
	<br/>
	<br/>
	</td> 
	</tr>
	</table>
</asp:Content>--%>
