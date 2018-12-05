<%@ Page Title="Cash Van Audit Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepCashVanAudit_Asr.aspx.vb" Inherits="SalesWorx_BO.RepCashVanAudit_Asr" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <style>
         

div[id*="ReportDiv"] {  overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

input[type="text"].rdfd_
        {
            margin:0 !important;
            padding:0 !important;
            height:0 !important;
            width:0 !important;
        }
    </style>
     <script>


         function OnClientItemExpand(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "350px"

             }
         }

         function OnClientItemCollapse(sender, args) {
             if (document.getElementById('MainContent_RepSec')) {

                 document.getElementById('MainContent_RepSec').style.height = "450px"
             }

         }

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
    <h4>Cash Van/FSR Audit Report</h4>
     <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>
	<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
<td style="padding:0px 0px 20px;width:100%" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate >
                <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems" onclientitemexpand="OnClientItemExpand"  onclientitemcollapse="OnClientItemCollapse"   >
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                        
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
            <div class="row">
                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Organization <em><span>&nbsp;</span>*</em></label>
            <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlOrganization"   Width ="100%"
                    runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" 
                    AutoPostBack="True">
                </telerik:RadComboBox>
             </div>
                                          </div>
                 <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>
             <asp:Label ID="lblCustVan" runat="server" Text="Van/FSR"></asp:Label> <em><span>&nbsp;</span>*</em></label>
                 
                <telerik:RadComboBox Skin="Simple" Filter="Contains" ID="ddlVan" runat="server"  Width ="100%"
                         DataTextField="SalesRep_Name" DataValueField="SalesRep_ID">  </telerik:RadComboBox>                       
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
              <p><strong>Van: </strong> <asp:Label ID="lbl_van" runat="server" Text=""></asp:Label></p>
              
               
            </span>
            </i>      
        </div>
    </div>

   

     <asp:Label ID="lbl_noAudit" runat="server" Text="No audit information found." Visible="false" ></asp:Label>
    
    <div id="Details" runat="server" visible="false" class="empdetailsblk">
        <div class="row">
            <div class="col-sm-3">Employee Name <strong><asp:Label ID="lbl_EmpName" runat="server" Text=""></asp:Label></strong></div>
            <div class="col-sm-3">Employee Code <strong><asp:Label ID="lbl_Code" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">Survey <strong><asp:Label ID="lbl_survey" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">Date of Audit <strong><asp:Label ID="lbl_DateofAudit" runat="server" Text=""></asp:Label></strong></div>
            <div class="col-sm-3">Date of Previous Audit <strong><asp:Label ID="lbl_prev" runat="server" Text=""></asp:Label></strong></div> 
            <div class="col-sm-3">Status <strong><asp:Label ID="lbl_status" runat="server" Text=""></asp:Label></strong></div>
        </div>
    </div>
       <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="LightGray"
                                 runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="LightGray" ShowHeader="false" 
                     >

                    <ItemStyle Font-Bold="true" />
                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                         
                                                                                                              
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Description" HeaderText="" SortExpression ="Description"
                                                               >
                                                                 <HeaderStyle  />
                                                                <ItemStyle Wrap="False" />
                                                                 
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                               
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
    </ContentTemplate> </asp:UpdatePanel> 

    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
   </div> 
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
</td>
</tr>
 
  
    </table>
	 
</asp:Content>
