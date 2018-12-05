<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepSales_WeeklyMonthly.aspx.vb" Inherits="SalesWorx_BO.RepSales_WeeklyMonthly" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Weekly/Monthly/Quarterly/Yearly Sales Reports</h4>
	
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
  <asp:UpdatePanel ID="Panel" runat="server" >
        <ContentTemplate>
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
               
                  
                  <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlCustomer"  Width ="100%"
                    runat="server" DataTextField="Outlet" DataValueField="CustomerID"> </telerik:RadComboBox>
                                             </div>
                                          </div>
     </tr> 
     <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Type</label>
          
                <telerik:RadComboBox Skin="Simple"  runat="server"   Width="100%" 
                    AutoPostBack="True" ID="ddlType">
                    <Items>
                        <telerik:RadComboBoxItem Value="W" Text="Weekly" />
                        <telerik:RadComboBoxItem Value="M" Text="Monthly" />
                        <telerik:RadComboBoxItem Value="Q" Text="Quarterly" />
                        <telerik:RadComboBoxItem Value="Y" Text="Yearly" />

                    </Items>
                     
                </telerik:RadComboBox>
                                                </div>
         </div>

           
         
          <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>From Date </label>
             
                
                <telerik:RadComboBox Skin="Simple"  ID="ddlFromYear" runat="server"    Width="100%" 
                    AutoPostBack="True">
                </telerik:RadComboBox>
                                                </div>
              </div>
                <div class="col-sm-2">
                                            <div class="form-group">
                                                 <label>&nbsp;</label>
               <telerik:RadComboBox Skin="Simple" ID="ddlFromMonth" runat="server"   Width="100%" 
                    >
               </telerik:RadComboBox>
             </div>
              </div>
            <div class="col-sm-2">
                                            <div class="form-group">
                                                <label>To Date </label>
            
                <telerik:RadComboBox Skin="Simple" runat="server"  AutoPostBack="True" Width="100%" ID="ddltoYear"
                    >
                </telerik:RadComboBox>
                   </div>
              </div>
                <div class="col-sm-2">
                                            <div class="form-group">
                                                 <label>&nbsp;</label>
                <telerik:RadComboBox Skin="Simple" ID="ddltoMonth" runat="server"  Width="100%"  
                  >
                </telerik:RadComboBox>
                                                </div>
                </div>
                                                    </div>

                    </div>
                                                    
                                                    <div class="col-sm-2">
                                                 <div class="form-group">
                                                    <label>&nbsp;</label>
                                                <asp:Button  CssClass ="btn btn-sm btn-block btn-primary"  ID="btnSearch" runat="server" Text="Search" />
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
              <p><strong>From Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
              <p><strong>To Date: </strong><asp:Label ID="lbl_To" runat="server" Text=""></asp:Label></p>  
              <p><strong>Outlet: </strong><asp:Label ID="lbl_Cust" runat="server" Text=""></asp:Label></p>            
            </span>
            </i>      
        </div>
    </div>

              
            <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>
                                <div style="width:100%; overflow-x:auto; border:#ccc solid 1px;" id="reportblocker" runat="server" visible="false" >
                                           <rsweb:ReportViewer ID="RVMain" runat="server"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>   
                             </div>
                                 </ContentTemplate>
                                </asp:UpdatePanel>
                           
 </ContentTemplate> </asp:UpdatePanel> 

     
                                

   <div style="display: none">
        <asp:Button CssClass="btn btn-primary" ID="BtnExportExcel" runat="server" Text="Export" />
        <asp:Button CssClass="btn btn-primary" ID="BtnExportPDF" runat="server" Text="Export" />
    </div>
    
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="Panel" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
          
                
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
	 
</asp:Content>
