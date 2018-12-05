<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_BankList.aspx.vb" Inherits="SalesWorx_BO.Rep_BankList" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
<style>

div[id*="ReportDiv"] {  
    overflow: hidden  !important;
    WIDTH: 100%;
    direction: ltr;
} 

#MainContent_gvRep{
    margin:15px 0;
}
</style>
    <script type="text/javascript">
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

       
        function NumericOnly(e) {
          
            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            alert(keycode);
            return true;
        }
       
    </script>
    <style type="text/css">      
   div.RadGrid .rgPager .rgAdvPart     
   {     
    display:none;        
   }      
</style>
 </asp:Content>
   <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
       <h4>Bank Listing</h4>
	 <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
                          <ContentTemplate >
                            
                               <telerik:RadPanelBar runat="server" ID="rpbFilter" Skin="Simple" Width="100%"
                            ExpandMode="MultipleExpandedItems">
                            <Items>
                                <telerik:RadPanelItem Expanded="True" Text=" ">

                                    <ContentTemplate>
                                         <div>
                                        <div class="row">
                                            <div class="col-sm-10 col-md-10 col-lg-10">
                                                <div class="row">
                                         
                                           <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Code</label>
                                                
                                                <telerik:RadTextBox ID="txt_BankCode" Width ="100%" CssClass="inputSM" runat="server" Enabled="true"></telerik:RadTextBox>
                                            </div>
                                          </div>

                                         <div class="col-sm-6">
                                            <div class="form-group">
                                                <label>Name</label>
                                                <asp:TextBox ID="txt_Name" Width ="100%" CssClass="inputSM" runat="server"></asp:TextBox>
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
                                       
                                    
                        </div>
                                        
                                         </ContentTemplate>
                                        </telerik:RadPanelItem> 
                                     </Items> 
                    </telerik:RadPanelBar> 
                            
                               <div id="Args" runat="server" visible="false">
        <div id="popoverblkouter">
            Hover on icon to view search criteria <i class="fa fa-info-circle">
            <span class="popoverblk">
              <p><strong>Code: </strong><asp:Label ID="lbl_Code" runat="server" Text=""></asp:Label></p>
              <p><strong>Name: </strong> <asp:Label ID="lbl_Name" runat="server" Text=""></asp:Label></p>
              
            </span>
            </i>      
        </div>
    </div>

                     <div class="table-responsive">         
                              <telerik:RadGrid id="gvRep" AllowSorting="True" AutoGenerateColumns="False" Skin="Simple" Width="100%" BorderColor="transparent"
                                PageSize="10" AllowPaging="True" runat="server" 
                                GridLines="None" >
                                                       
                                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                <ClientSettings EnableRowHoverStyle="true">
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" TableLayout="Auto" Width="100%" GridLines="None" BorderColor="transparent"
                    PageSize="10">


                  <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                        <Columns>
                                                                             
                                                             <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="code_value" HeaderText="Code" SortExpression ="code_value"
                                                               >
                                                                 
                                                            </telerik:GridBoundColumn>
                                                      
                                                          
                                                              <telerik:GridBoundColumn HeaderStyle-HorizontalAlign="Left" DataField="Code_Description" HeaderText="Description"
                                                                  SortExpression ="Code_Description" >
                                                               
                                                            </telerik:GridBoundColumn>
                                                             
                                                          
                                                        </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                           

                               </div>
                              
                       

                           </ContentTemplate>
        
        </asp:UpdatePanel> 
                           
    <div style="display:none">
     <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportExcel" runat="server" Text="Export" />
    <asp:Button  CssClass ="btn btn-primary"  ID="BtnExportPDF" runat="server" Text="Export" />
        
   </div>
    
           

   <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10"
                                runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="Panel14" CssClass="overlay" runat="server">
                                        <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
                                        <span style="font-size: 12px; color: #3399ff;">Processing... </span>
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress> 

	

<%--	   <div style="overflow:scroll; height:100%; border:groove" id="RepSec" runat="server" visible="false">
  <rsweb:ReportViewer ID="RVMain" runat="server"  CssClass="abc"   ShowBackButton ="true" 
                  ProcessingMode="Remote" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>    
	 </div> --%>
</asp:Content>
