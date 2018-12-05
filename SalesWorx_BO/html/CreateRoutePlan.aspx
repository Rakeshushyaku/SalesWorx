<%@ Page Title="Create Route Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master"
     CodeBehind="CreateRoutePlan.aspx.vb" Inherits="SalesWorx_BO.CreateRoutePlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script type="text/javascript" src="../js/RoutePlanner.js"></script>
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
                 if (postBackElement.id == 'ctl00_MainContent_Route_FSR_ID') {
                     $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                     postBackElement.disabled = true;
                 }
            
             }

             function EndRequest(sender, args) {
                 if (postBackElement.id == 'ctl00_MainContent_Route_FSR_ID') {
                     $get('<%= UpdateProgress1.ClientID %>').style.display = 'none';
                     postBackElement.disabled = false;
                 }
             }

             function alertCallBackFn(arg) {
              
             }

</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <h4>Create Route Plan</h4>
    <telerik:RadAjaxLoadingPanel ID="AjaxLoadingPanel1" runat="server">
        <div id="ProgressPanel" class="overlay">

            <img src="../assets/img/ajax-loader.gif" alt="Processing..." style="z-index: 10010; vertical-align: middle;" />
        </div>
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Simple" EnableShadow="true">
    </telerik:RadWindowManager>
    
         <input type="hidden" name="Action_Mode" value="" />

	

    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpPanel" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
                    <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />
                   <span>Processing...</span>
            </asp:Panel>

        </ProgressTemplate>
    </asp:UpdateProgress>
	<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
	  <div class="row">
	<asp:Panel ID="VanPanel" runat="server">
        <div class="col-sm-4">
           <div class="form-group">
               <label>Van/FSR </label>
               <telerik:RadComboBox  ID="Route_FSR_ID" runat="server" AutoPostBack="true" Skin="Simple" Filter="Contains" EmptyMessage="Please type a Van/FSR"
                    DataTextField="SalesRep_Name"  TabIndex ="1"   Width="100%" Height="250px"  DataValueField="SalesRep_ID" AppendDataBoundItems="true">
                 
                    
              </telerik:RadComboBox>

            </div>
        </div>
</asp:Panel>
	<asp:Panel runat="server" ID="BtnPanel" Visible="false">
        <div class="col-sm-4">
	 <div class="form-group">
         <label>Default Plans</label>

         <telerik:RadComboBox 
                    ID="Default_Plan_DD" runat="server" DataTextField="Details"  Skin="Simple" 
                    DataValueField="Default_Plan_ID" TabIndex ="2" Width ="100%" >
                </telerik:RadComboBox>
     </div>
            </div>
        <div class="col-sm-4">
        
         <div class="form-group">
             <label class="hidden-xs"><br /></label>
             <telerik:RadButton ID="CreateBtn"  TabIndex ="3" CssClass="btn btn-primary" Skin="Simple" runat="server" Text="Create" />
        </div>
          </div>           

	</asp:Panel>
       
  
</div>
    
	</ContentTemplate>
	</asp:UpdatePanel>	
	 
	


</asp:Content>
