<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="AccessApprovalCode.aspx.vb" Inherits="SalesWorx_BO.AccessApprovalCode" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

      <script language="javascript" type="text/javascript">

          var prm = Sys.WebForms.PageRequestManager.getInstance();

          prm.add_initializeRequest(InitializeRequest);
          prm.add_endRequest(EndRequest);
          var postBackElement;
          function InitializeRequest(sender, args) {

              if (prm.get_isInAsyncPostBack())
                  args.set_cancel(true);
              postBackElement = args.get_postBackElement();
              //  $get('UpdateProgress1').style.display = 'block'
              $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
              postBackElement.disabled = true;
          }


          function EndRequest(sender, args) {
              $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
              postBackElement.disabled = false;
          } 


     function alertCallBackFn(arg) {
            
        }
        
    </script>
   </asp:Content>

   <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
   
  
  <h4>Access Approval Code</h4>
    <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true"></telerik:RadWindowManager>
	<div id="pagenote" >This screen may be used for getting approval code for a van/FSR.<p></p></div>	
	<div class="row">
        <div class="col-sm-4">
            <div class="form-group">
                <label>Van/FSR</label>
                <telerik:RadComboBox Skin="Simple"  ID="SalesRep_ID" runat="server" CssClass="inputSM" TabIndex ="2" Width="100%" DataTextField="SalesRep_Name" DataValueField="SalesRep_ID" > </telerik:RadComboBox>
           </div>
        </div>
        <div class="col-sm-4" id="rowCode" runat="server"  visible = "false">
            <div class="form-group">  
                <label>Approval Code</label> 
                <asp:Label Font-Bold="true" Font-Size="X-Large" ForeColor="Green"  ID="lblApprovalCode" runat="server"></asp:Label> 
            </div>
        </div>
        <div class="col-sm-4">
            <div class="form-group"> 
                <label class="hidden-xs"><br /></label> 
                <asp:Button CssClass ="btn btn-primary" ID="BtnAccess"  TabIndex ="2" runat="server" Text="Get Approval Code" />
            </div>
        </div>
	</div>
	
 

  
        
  
 



    
	     </ContentTemplate>
 </asp:UpdatePanel>	
      <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpdatePanel1" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress> 
</asp:Content>
