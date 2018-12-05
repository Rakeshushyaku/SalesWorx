<%@ Page Title="Change Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="SalesWorx_BO.ChangePassword" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
    function alertCallBackFn(arg) {

    }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Change Password</h4>
       

     <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
               <AjaxSettings>
                   <telerik:AjaxSetting AjaxControlID="RadAjaxManager2">
                       <UpdatedControls>
                           <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="AjaxLoadingPanel1" />
                       </UpdatedControls>
                   </telerik:AjaxSetting>
               </AjaxSettings>
           </telerik:RadAjaxManager>
        
            
           <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index:10010;vertical-align:middle;" />      
       </div>
           </telerik:RadAjaxLoadingPanel>

        <asp:Panel ID="Panel1" runat="server">    </asp:Panel> 
      <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
                             
          </telerik:RadWindowManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
    
        <ContentTemplate>
           <p><asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></p>
           <div class="row">

           <div class="col-sm-6 col-md-5 col-lg-4">
               <div class="form-group">
                <label>Old Password</label>
                      <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                </div>
            </div>
        </div>
            <div class="row">
               <div class="col-sm-6 col-md-5 col-lg-4">
            <div class="form-group">
                               
                               <label>New Password</label>
                             
                                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                              </div>
                   </div>
                </div>
            <div class="row">
               <div class="col-sm-6 col-md-5 col-lg-4">
                           <div class="form-group">
                                     <label>Confirm Password</label>
                                
                                    <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                                </div>
                       </div>
                </div>
            <div class="row">
               <div class="col-sm-6 col-md-5 col-lg-4">
                            
                             <div class="form-group">
                                   

                                 <telerik:RadButton ID="Button1" Skin="Simple" CssClass ="btn btn-success" 

                                                            runat="server" Text="Save" TabIndex ="6" >
                                     </telerik:RadButton>
                                </div>
                   </div>
           </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
