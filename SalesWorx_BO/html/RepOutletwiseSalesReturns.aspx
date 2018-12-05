<%@ Page Title="Outletwise Sales / Returns" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepOutletwiseSalesReturns.aspx.vb" Inherits="SalesWorx_BO.RepOutletwiseSalesReturns" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
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

        function OpenSalViewWindow(CID, qty,SID,Outlet) {

            if (qty > 0) {
                var param1 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param2 = document.getElementById('<%= HTo.ClientID%>').value;
                var param3 = document.getElementById('<%= HorgID.ClientID%>').value;
                var param4 = document.getElementById('<%= HUID.ClientID%>').value;


                var URL
                URL = 'RepDetails.aspx?Type=OWSR&rtype=S&ReportName=OutletwiseSalesAmt&From=' + param1 + '&To=' + param2 + '&SID=' + SID +  '&CID=' + CID + "&Outlet=" + Outlet + '&Org=' + param3 + "&UID=" + param4;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(800, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }

        function OpenRetViewWindow(CID, qty, SID, Outlet) {
            if (qty > 0) {
                var param1 = document.getElementById('<%= Hfrom.ClientID%>').value;
                var param2 = document.getElementById('<%= HTo.ClientID%>').value;
                var param3 = document.getElementById('<%= HorgID.ClientID%>').value;
                var param4 = document.getElementById('<%= HUID.ClientID%>').value;
                var URL
                URL = 'RepDetails.aspx?Type=OWSR&rtype=R&ReportName=OutletwiseReturnsAmt&SID=' + SID + '&From=' + param1 + '&To=' + param2 + '&CID=' + CID + "&Outlet=" + Outlet + '&Org=' + param3 + "&UID=" + param4;
                var oWnd = radopen(URL, null);
                oWnd.SetSize(800, 600);
                oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
                oWnd.SetModal(true);
                oWnd.Center;
                oWnd.set_visibleStatusbar(false)
            }
            return false

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h4>Outletwise Sales / Returns</h4>
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

      <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
        <contenttemplate>
         <asp:HiddenField ID="HTo" runat="server" />
         <asp:HiddenField ID="Hfrom" runat="server" />         
         <asp:HiddenField ID="HorgID" runat="server" />
      
              <asp:HiddenField ID="HUID" runat="server" />
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
                                                <label>Outlet</label>
                                                   <telerik:RadComboBox Skin="Simple" EmptyMessage="Please type outlet No./Name" EnableCheckAllItemsCheckBox="true"
                                                        EnableLoadOnDemand="True" AutoPostBack="true"  
                                                       ID="ddlCustomer" Width ="100%" runat="server" DataTextField="Outlet" DataValueField="CustomerID" Filter="Contains">
                                        </telerik:RadComboBox >
                                                 </div>
                                             </div>
                                                    </div>
                                              
                                                  <div class="row">
                                                     <div class="col-sm-3">
                                                       <div class="form-group">
                                                           <label>From Date  </label>
                                                            <telerik:RadDatePicker ID="txtFromDate"   Width ="100%" runat="server">
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
                                                           <label>To Date </label>
                                                             <telerik:RadDatePicker ID="txtToDate"  Width ="100%"  runat="server">
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
              <p><strong>Outlet: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>            
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
                        <div id="divCurrency" runat="server" visible="false"   class="col-sm-4">
    <h5 class='text-right'>Currency <span class='text-blue'><strong><asp:Label ID="lbl_Currency" runat="server" Text=""></asp:Label></strong></span></h5>
                <asp:HiddenField runat="server" ID="hfDigit" Value="N2" />
</div>
            </div>
         

                              
                                 
                                    
                               
              
                              <div class="table-responsive">
           
                                              <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>
                               
                                <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="11" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings GroupContinuesFormatString="" CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="11">


                  <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None" ></PagerStyle>
                                                        <Columns>
                                                         <telerik:GridBoundColumn Visible="true"  HeaderStyle-HorizontalAlign="Left" DataField="Outlet" HeaderText="Outlet"
                                                                  SortExpression ="Outlet" >
                                                                <ItemStyle Wrap="False" />
                                                         </telerik:GridBoundColumn>


                                                         <telerik:GridTemplateColumn UniqueName="SaleQty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="SaleQty" SortExpression="SaleQty"
                                                                HeaderText="Sales Qty" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("Sales").ToString()) > 0, True, False)%>' DataFormatString="{0:#,##0.0000}" OnClientClick='<%# String.Format("OpenSalViewWindow(""{0}"", ""{1}"",""{2}"",""{3}"");", Eval("Ship_To_Customer_ID"), Eval("SaleQty"), Eval("Ship_To_Site_ID"), Eval("Outlet"))%>' ID="lnkSalQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SaleQty", "{0:#,##0.0000}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                        </telerik:GridTemplateColumn>

                                                         <telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="Sales" HeaderText="Sales Value"
                                                                  SortExpression ="Sales" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridBoundColumn>
                                           

                                                                 <telerik:GridTemplateColumn UniqueName="ReturnQty" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" DataField="ReturnQty" SortExpression="ReturnQty"
                                                                HeaderText="Returns Qty" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Enabled='<%# IIf(CInt(Eval("Returns").ToString()) > 0, True, False)%>'  DataFormatString="{0:#,##0.0000}" OnClientClick='<%# String.Format("OpenRetViewWindow(""{0}"", ""{1}"",""{2}"",""{3}"");", Eval("Ship_To_Customer_ID"), Eval("ReturnQty"), Eval("Ship_To_Site_ID"), Eval("Outlet"))%>' ID="lnkRetQty"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReturnQty", "{0:#,##0.0000}")%>' ForeColor="SteelBlue" Font-Underline="true" Width="100%"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="True" />
                                                        </telerik:GridTemplateColumn>

                                                         <telerik:GridBoundColumn ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center" DataField="Returns" HeaderText="Returns Value"
                                                                  SortExpression ="Returns" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridBoundColumn>
                                                   
                                                         

                                                            <telerik:GridCalculatedColumn UniqueName="RetValPers" ItemStyle-HorizontalAlign="Right"   HeaderStyle-HorizontalAlign="Center"  HeaderText="Returns %"
                                                              DataFields="Returns, Sales"  Expression="  iif({1} = 0, 0, ({0}/{1})* 100 )   "    DataFormatString="{0:#,##0.00}" >
                                                                <ItemStyle Wrap="False" />
                                                          </telerik:GridCalculatedColumn>
                                                   
                                                          
                                                        </Columns>

                                                  
                                    

                                                        </MasterTableView>
                                                    </telerik:RadGrid>

                                  <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

                                 </ContentTemplate>
                                </asp:UpdatePanel>
                           

                              </div>
        
      

    </contenttemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" runat="server" DisplayAfter="10">
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
	<span class="pgtitile3">Outletwise Sales / Returns</span></div>
	
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:6px 12px" >
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="table1">
	<tr>
<td style="padding:6px 12px">
<table  border="0" cellspacing="0" cellpadding="0" >
       <tr> 
          <td class="txtSMBold">
             Organization :
               
            </td>  
            <td> <asp:DropDownList CssClass="inputSM" ID="ddlOrganization"  Width ="150px" AutoPostBack="true"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID">
                </asp:DropDownList></td>
            <td  class="txtSMBold">Outlet :</td>
            <td>
                <asp:DropDownList CssClass="inputSM" ID="ddlCustomer"  Width ="200px"
                runat="server" DataTextField="Outlet" DataValueField="CustomerID">
                </asp:DropDownList>  
               &nbsp;
                     <asp:Button CssClass="btnInput" ID="btnSearch" runat="server" Text="Search" />
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
