<%@ Page Title="Van Activity" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
 CodeBehind="RepVanTransactions.aspx.vb" Inherits="SalesWorx_BO.RepVanTransactions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
         input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
         

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

     
    
    </style>
    <script>
        function pageLoad(sender, args) {
            $('.rgMasterTable').find('th > a').attr("data-container", "body");
            $('.rgMasterTable').find('th > a').attr("data-toggle", "tooltip");
            $('[data-toggle="tooltip"]').tooltip();
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
        function alertCallBackFn(arg) {

        }

        function OpenViewWindow(cid) {
            var URL
            //URL = 'RepDetails.aspx?Type=Col&ReportName=CollectionDetails&ID=' + cid;
            URL = 'Rep_CollectionDetails.aspx?ID=' + cid;
            var oWnd = radopen(URL, null);
            oWnd.SetSize(900, 600);
            oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close); //Close:4
            oWnd.SetModal(true);
            oWnd.Center;
            oWnd.set_visibleStatusbar(false)

            return false

        }

        document.onkeydown = function (evt) {

            evt = evt || window.event;
            if (evt.keyCode == 27) {

                HideRadWindow();
            }
        };

        function HideRadWindow() {

            var elem = $('a[class=rwCloseButton');

            if (elem != null && elem != undefined) {
                $('a[class=rwCloseButton')[0].click();
            }


        }

    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <h4>Van/FSR Activity</h4>
	   <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px" >
  <asp:UpdatePanel ID="ClassUpdatePnl" runat="server" >
        <ContentTemplate>
      
	 
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                                    <div class="col-sm-2" runat="server" id="dvCountry">
                                            <div class="form-group">
                                                <label>Country</label>
                                                <telerik:RadComboBox Skin="Simple" EmptyMessage="Select Country" ID="ddlCountry" Width ="100%" runat="server" DataTextField="Country" DataValueField="MAS_ORG_ID"  AutoPostBack="true" >
                                            </telerik:RadComboBox>
                                               
                                            </div>
                                        </div>
                                         <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>Organization <em><span>&nbsp;</span>*</em></label>
            
             <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization"   width="100%" 
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                </telerik:RadComboBox>
                </div>
                                          </div>
                                           <div class="col-sm-5">
                                            <div class="form-group">
                                                <label>
                   Van/FSR <em><span>&nbsp;</span>*</em></label>
              
                  <telerik:RadComboBox ID="ddl_Van" Skin="Simple" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"  width="100%"  
                CssClass="inputSM"   EmptyMessage="Select Van/FSR" Filter="Contains" DatavalueField="SalesRep_ID"  DataTextField="SalesRep_Name" >
                       </telerik:RadComboBox>
              </div>
                                          </div>
                                           <div class="col-sm-6">
                                               <div class="row">
                                                   <div class="col-sm-6">
                                            
                                            <div class="form-group">
                                                <label>
                  From Date </label>
              <telerik:RadDatePicker ID="txt_FromDate"  Width="100%"  runat="server">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>
              </div>
                                                 </div>

                                                   <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label>To Date </label>
              <telerik:RadDatePicker ID="txtToDate"   Width="100%" runat="server">
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
                                            <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"   ID="SearchBtn" runat="server" Text="Search"  />
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
              <p><strong>Van: </strong><asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              <p><strong>From Date: </strong> <asp:Label ID="lbl_Month" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong> <asp:Label ID="lbl_ToMonth" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
             
       <div id="summary" runat="server" class="row"></div>
            <p><br /></p>
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="SalesRep_Name" HeaderText="Van/FSR" SortExpression ="SalesRep_Name"
                                                               >
                                                               <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Code" HeaderText="Emp Code"
                                                                  SortExpression ="Emp_Code" >
                                                                <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Name" HeaderText="Emp Name"
                                                                  SortExpression ="Emp_Name" >
                                                               <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Emp_Phone" HeaderText="Emp Phone"
                                                                  SortExpression ="Emp_Phone" >
                                                                <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                           <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Supervisor" HeaderText="Supervisor"
                                                                  SortExpression ="Supervisor" >
                                                                <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrderCnt" HeaderText="Invoice<br />Count"
                                                                  SortExpression ="OrderCnt" DataType="System.Double" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="OrderAmt" HeaderText="Invoice<br />Value"
                                                                  SortExpression ="OrderAmt" DataType="System.Double" DataFormatString="{0:N2}">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="rmaCnt" HeaderText="Return<br />Count"
                                                                  SortExpression ="rmaCnt" DataType="System.Double">
                                                                 <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="rma" HeaderText="Return<br />Value"
                                                                  SortExpression ="rma" DataType="System.Double" DataFormatString="{0:N2}">
                                                               <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                           </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="PaymentCnt" HeaderText="Collection<br />Count"
                                                                  SortExpression ="PaymentCnt" DataType="System.Double" >
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                            </telerik:GridBoundColumn>
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Payment" HeaderText="Collection<br />Value"
                                                                  SortExpression ="Payment" DataType="System.Double" DataFormatString="{0:N2}">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Right"  />
                                                                <HeaderStyle Wrap ="true" HorizontalAlign ="Center" />
                                                            </telerik:GridBoundColumn>
                                                          <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="LastFullSync" HeaderText="Last<br />Full Sync<i class='fa fa-info-circle'></i>" SortExpression ="LastFullSync"
                                                             DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" HeaderTooltip="Date and time of the last full synchronization"   >
                                                                <ItemStyle Wrap="true" HorizontalAlign ="Left" />
                                                                 <HeaderStyle Wrap="true" HorizontalAlign ="Left"  />
                                                            </telerik:GridBoundColumn>
                                                      
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                              
                                   
                        <div style="display:none">
                                   <asp:Label ID="lblDecimal" runat="server" Text="N2"></asp:Label>
                                   </div>

                           </ContentTemplate>
        
        </asp:UpdatePanel> 
    
                           
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div>
           

   <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="ClassUpdatePnl" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress> 
</td>
</tr>
 
           
        
  
    </table>
    </asp:Content>

