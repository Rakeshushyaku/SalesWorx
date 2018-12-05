<%@ Page Title="Copy Route Plan Week Days" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="CopyRoutePlanByWeekDays.aspx.vb" Inherits="SalesWorx_BO.CopyRoutePlanByWeekDays" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript" src="../js/RoutePlanner.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

     <script language="javascript" type="text/javascript">

         function alertCallBackFn(arg) {

         }
         var prm = Sys.WebForms.PageRequestManager.getInstance();

         prm.add_initializeRequest(InitializeRequest);
         prm.add_endRequest(EndRequest);
         var postBackElement;
         function InitializeRequest(sender, args) {

             if (prm.get_isInAsyncPostBack())
                 args.set_cancel(true);
             postBackElement = args.get_postBackElement();
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                 $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                 postBackElement.disabled = true;
             }

         }

         function EndRequest(sender, args) {
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                 $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
                 postBackElement.disabled = false;
             }
         } 

</script>
    </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
     <h4>Copy Route Plan By Week Days</h4>
   <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
   </telerik:RadWindowManager>
     
   <input type="hidden" name="Action_Mode" value="" />
     
	
  		  <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
       <ProgressTemplate>
          <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
         <img src="../assets/img/ajax-loader.gif" alt="Processing..." />            
           <span>Processing... </span></asp:Panel>
           </ProgressTemplate>
           </asp:UpdateProgress>
	<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>



	 <asp:Panel ID="SRepPanel" runat="server" Visible="false">
         <div class="row">
            <div class="col-sm-4">
            <label>Van/FSR</label>
         <div class="form-group">
                <telerik:RadComboBox  ID="Route_FSR_ID" runat="server" AutoPostBack="true" Skin="Simple" Filter="Contains" EmptyMessage="Please type a Van/FSR"
                        DataTextField="SalesRep_Name" TabIndex ="1" Width ="100%" DataValueField="SalesRep_ID" AppendDataBoundItems="true">
 
                  </telerik:RadComboBox>
           </div>
</div>
</div>
	 </asp:Panel>
	<asp:Panel runat="server" ID="BtnPanel" Visible="false">
         <div class="row">
            <div class="col-sm-4">
                   <label>From </label>
                <div class="form-group">
                  <telerik:RadComboBox ID="Route_ID"  TabIndex ="2" Width ="100%"  runat="server"  Skin="Simple"
                        DataTextField="Details" DataValueField="FSR_Plan_ID">
                    </telerik:RadComboBox>
                  </div>
            </div>
             
            <div class="col-sm-4"> 
                <label>To</label>
                <telerik:RadComboBox ID="RP_ID"  TabIndex ="3" Width ="100%" runat="server" Skin="Simple"
                    DataTextField="Details" DataValueField="FSR_Plan_ID">
                </telerik:RadComboBox>
            </div>
             <div class="col-sm-4">
                  <label class="hidden-xs"><br /></label>
                  <telerik:RadButton ID="CopyBtn" Skin="Simple" CssClass="btn btn-primary" TabIndex ="4" runat="server" Text="Copy" OnClientClick="return ValidateForm()" />
            </div>
    </div>

	</asp:Panel>

	</ContentTemplate>
	</asp:UpdatePanel>	

</asp:Content>
