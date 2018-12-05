<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepUserChangLog.aspx.vb" Inherits="SalesWorx_BO.RepUserChangLog" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
 <style>
        

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

 div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   } 
    </style>
     <script>




         function alertCallBackFn(arg) {

         }

         function clickSearch() {
             $("#MainContent_SearchBtn").click()
             return false;
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


<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
 <h4>User Log Report</h4>
	
 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	 
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
<telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems"  >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
<div class="row">
                                             <div class="col-sm-10">
                                                <div class="row">
    
                                                 <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Organization </label>
              <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"  Width="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
               </telerik:RadComboBox>
              </div>
                                                     </div>

                 <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Van/FSR</label>
              <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan"  Width="100%"
                    runat="server" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" 
                    AutoPostBack="True">
               </telerik:RadComboBox>
                     
                     
           </div>
                     </div>
           
                                                    <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>User</label>
                                                 <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlUser"  width="100%"
                runat="server" DataTextField="UserName" DataValueField="User_ID">
                 </telerik:RadComboBox>
                                                </div>
                                                        </div>
</div>
                                                 <div class="row">
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>From Date </label>
             
                <telerik:RadDatePicker ID="txtFromDate"   runat="server" Width ="100%">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar1" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker>            </div>
              </div>
            <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>To Date </label> 
             <telerik:RadDatePicker ID="txtToDate"   runat="server" Width ="100%">
                                                        <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                        </DateInput>
                                                        <Calendar ID="Calendar2" runat="server">
                                                            <FastNavigationSettings TodayButtonCaption="Current Month" OkButtonCaption="     OK    " />
                                                        </Calendar>
                                                    </telerik:RadDatePicker> 
                </div>
                </div>       
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Trans Type <//label>
             
                             
                 <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrderType" width="100%" runat="server" >
                     <Items>
                    <telerik:RadComboBoxItem Value="0" Text="ALL"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Inserted" Text="Inserted"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Deleted" Text="Deleted"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Updated" Text="Updated"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Approved" Text="Approved"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Login" Text="Login"></telerik:RadComboBoxItem>
                         <telerik:RadComboBoxItem Value="Logout" Text="Logout"></telerik:RadComboBoxItem>
                         </Items>
               </telerik:RadComboBox>
                             
                        
            </div>
                </div> 
                                                     </div>
                                                 <div class="row">      
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Module </label>
             
               <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlModule"  width="100%"
                runat="server" >
             </telerik:RadComboBox>          
            </div>
                </div>       
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Sub Module</label>
          
                             
                  <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlSubModule"  width="100%"
                runat="server" >
                  </telerik:RadComboBox>   
                             
                        
              </div>
                </div>       
          <div class="col-sm-4">
                                                     
                                            <div class="form-group"><label>Key Value</label>
            
                <asp:TextBox  ID="txtKeyValue" width="100%" CssClass="inputSM" runat="server"></asp:TextBox> 
                
             </div>
              </div>
                                                    </div>
                                                 </div>

  <div class="col-sm-2 col-md-2 col-lg-2">
                                                <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Search"  />
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
                <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
                <p><strong>User: </strong> <asp:Label ID="lbl_user" runat="server" Text=""></asp:Label></p>
                <p><strong>From Date: </strong> <asp:Label ID="lbl_From" runat="server" Text=""></asp:Label></p>
               <p><strong>To Date: </strong> <asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>
                 <p><strong>Trans Type: </strong> <asp:Label ID="lbl_Type" runat="server" Text=""></asp:Label></p>
                <p><strong>Module: </strong> <asp:Label ID="lbl_Module" runat="server" Text=""></asp:Label></p>
                <p><strong>Sub Module: </strong> <asp:Label ID="lbl_Submodule" runat="server" Text=""></asp:Label></p>
                <p><strong>Key Value: </strong> <asp:Label ID="lbl_key" runat="server" Text=""></asp:Label></p>
            </span>
            </i>      
        </div>
    </div>
         <div class="overflowx">
             <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="false" Skin="Simple" BorderColor="LightGray"
                                                                    PageSize="12" AllowPaging="True" runat="server" 
                                                                    GridLines="None" >
                                                       
                                                                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <ClientSettings EnableRowHoverStyle="true">
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray"
                                                        PageSize="12">
                                                        <Columns>

                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sub_Module" HeaderText="Sub Module"
                                                                  SortExpression ="Sub_Module" >
                                                                <ItemStyle Wrap="true" Width="15%" />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="Description"
                                                                  SortExpression ="Description" >
                                                                <ItemStyle Wrap="True" Width="30%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Logged_At" HeaderText="Modified At"
                                                                DataFormatString="{0:dd-MMM-yyyy hh:mm tt}"  SortExpression ="Logged_At" >
                                                                <ItemStyle Wrap="False" Width="15%"  />
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Username" HeaderText="Modified  By"
                                                                  SortExpression ="Username" >
                                                                <ItemStyle Wrap="true" Width="10%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Van" HeaderText="Van/FSR"
                                                                  SortExpression ="Van" >
                                                                <ItemStyle Wrap="true" Width="10%" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Sales_Org" HeaderText="Sales Org"
                                                                  SortExpression ="Sales_Org" >
                                                                <ItemStyle Wrap="true"  Width="10%"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="TranType" HeaderText="Log Type"
                                                                  SortExpression ="TranType" >
                                                                <ItemStyle Wrap="true" Width="10%" />
                                                            </telerik:GridBoundColumn>
                                                            
                                                        </Columns>

                                                      <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                                                
                                                                                            </MasterTableView>
                                                                                        </telerik:RadGrid>


            </div>
        </ContentTemplate>
 
  </asp:UpdatePanel> 
        <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
   </div>
</asp:Content>