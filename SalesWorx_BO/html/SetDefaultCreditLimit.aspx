<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="SetDefaultCreditLimit.aspx.vb" Inherits="SalesWorx_BO.SetDefaultCreditLimit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script>
        function NumericOnly(e) {

            var keycode;

            if (window.event) {
                keycode = window.event.keyCode;
            } else if (e) {
                keycode = e.which;
            }
            if ((parseInt(keycode) >= 48 && parseInt(keycode) <= 57) || parseInt(keycode) == 8 || parseInt(keycode) == 46 || parseInt(keycode) == 0)
                return true;

            return false;
        }
        function alertCallBackFn(arg) {
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Set Credit Limit</h4>
                <telerik:RadAjaxLoadingPanel  ID="AjaxLoadingPanel1" runat="server" >
             <div ID="ProgressPanel" class ="overlay"  >
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />      
       </div>
           </telerik:RadAjaxLoadingPanel>  

        <asp:Panel ID="Panel1" runat="server">      </asp:Panel> 
             <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true" Skin="Simple">   </telerik:RadWindowManager>
    
                            <asp:UpdatePanel ID="TopPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row">
                                    <div class="col-sm-4">
                                                <div class="form-group"> 
                                                    <label>Default Credit Limit</label>
                                                 <asp:TextBox ID="txt_CreditLimit" runat="server" TabIndex="10" CssClass="inputSM" MaxLength="7" Enabled="false" Width ="100%" onKeypress='return NumericOnly(event)'></asp:TextBox>
                                                </div>
                                             </div>
                                        <div class="col-sm-4">
                                                <div class="form-group"> 
                                                    <label>&nbsp;</label>  <asp:Button   ID="btnUpdate" ValidationGroup="valsum" runat="server" causesValidation="false"
                                   Text="Update"  CssClass="btn btn-success" />
                                                    </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>            
</asp:Content>
