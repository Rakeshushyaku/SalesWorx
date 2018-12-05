<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="Rep_WareHousePurchase.aspx.vb" Inherits="SalesWorx_BO.Rep_WareHousePurchase" %>
 <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function alertCallBackFn(arg) {

        }

    </script>
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
     <h4>Download Purchase Report</h4>
	
 	<telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager> 
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             
                                                <div class="row">
                                                      
                                         <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Organization<em><span>&nbsp;</span>*</em></label>
                                                <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlOrganization" Width ="100%" runat="server" DataTextField="Description" DataValueField="MAS_Org_ID" AutoPostBack="True" >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Customer</label>
                                                   <telerik:RadComboBox Skin="Simple"  Filter="Contains" ID="ddlAgency" Width ="100%" runat="server"  >
                                        </telerik:RadComboBox>
                                            </div>
                                          </div>
                                           
                                        
                                         
                                                 <div class="col-sm-2">
                                                   
                                                      
                                                    <div class="form-group">
                                                        <label>Date</label>

                                                       <telerik:RadDatePicker ID="txtFromDate"  Width ="100%"  runat="server">
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
                                                        <label>&nbsp;</label>
                                                       <asp:Button CssClass ="btn btn-sm btn-block btn-primary"  ID="SearchBtn" runat="server" Text="Download" />
                                                         </div>
                                                        
                                                     </div>
                                                     

                                                 </div>
                                                
                                             
                                           
                                            
                                          
                                     
                                                
 
 </ContentTemplate>
  <Triggers>
                                 <asp:PostBackTrigger ControlID="SearchBtn"  />
                                </Triggers>
  </asp:UpdatePanel> 
  
    
   <asp:UpdateProgress ID="UpdateProgress1"   AssociatedUpdatePanelID="UpdatePanel1" runat="server" DisplayAfter="10">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:bold; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress>       
 
</asp:Content>
