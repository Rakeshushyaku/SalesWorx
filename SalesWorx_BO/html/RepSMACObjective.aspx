<%@ Page Title="Daily Objective Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="RepSMACObjective.aspx.vb" Inherits="SalesWorx_BO.RepSMACObjective" %>

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
       

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Daily Objective Report</h4>
	
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
                                                <label>Van <em><span>&nbsp;</span>*</em></label>
                                                 <telerik:RadComboBox Filter="Contains" Skin="Simple" 
                                               ID="ddlVan" Width="100%" runat="server" Height ="200px">
                                            </telerik:RadComboBox>
                     
            </div>
                   </div>
                 <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Date</label>
                                            

                                         <telerik:RadDatePicker ID="txtFromDate" Width="100%"  runat="server">
                                                                    <DateInput ReadOnly="true" DisplayDateFormat="dd-MMM-yyyy">
                                                                    </DateInput>
                                                                    <Calendar ID="Calendar2" runat="server">
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
              <p><strong>Date: </strong><asp:Label ID="lbl_from" runat="server" Text=""></asp:Label></p>
          
                         
            </span>
            </i>      
        </div>
    </div>

              
            <asp:UpdatePanel ID="RadAjaxPanel2" runat ="server"   >
                            <ContentTemplate>
                                        <asp:HiddenField ID="hfOrgID" runat="server" />
        <asp:HiddenField ID="hfVans" runat="server" />
      
        <asp:HiddenField ID="hfSMonth" runat="server" />
                                   <asp:HiddenField ID="hfTop" runat="server" />
                            
                                
                               <div style="width:100%; overflow-x:auto; border:#ccc solid 1px;" id="reportblocker" runat="server" visible="false" >
                                           <rsweb:ReportViewer ID="RVMain" Visible ="false"  runat="server"  ShowBackButton ="true" 
                  ProcessingMode="Remote" Width="100%" 
                 SizeToReportContent="true" AsyncRendering="false"  DocumentMapWidth="100%" > 
              </rsweb:ReportViewer>    
                             </div>
                                 </ContentTemplate>
                                </asp:UpdatePanel>

 </ContentTemplate> </asp:UpdatePanel> 
  
   
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
       
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
</asp:Content>