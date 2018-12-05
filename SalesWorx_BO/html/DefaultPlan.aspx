<%@ Page Title="Default Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" 
    CodeBehind="DefaultPlan.aspx.vb" Inherits="SalesWorx_BO.DefaultPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
	<script type="text/javascript" src="../js/RoutePlanner.js"></script>
    <script>
        function alertCallBackFn(arg) {
            HideRadWindow()
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Setup Default Plan</h4>
   <telerik:RadWindowManager ID="RadWindowManager1"  Skin="Simple" runat="server" EnableShadow="true">
   </telerik:RadWindowManager>

    <div class="row">
	
	<asp:Panel ID="sitePanel" runat="server" Visible="false" >
        
            <div class="col-sm-4">
	   <div class="form-group">  
           <label> Organization</label>

                       <telerik:RadComboBox ID="ddSite" Skin="Simple"   AutoPostBack="true" DataTextField="Site" 
                 DataValueField="Site"  Width="100%" Height="100px" TabIndex="1"
                    runat="server">
            </telerik:RadComboBox>
       </div> 
                </div>       

	</asp:Panel>
    <asp:Panel runat="server" ID="defplan" Visible="false" >
 <div class="col-sm-4">
     <div class="form-group">  
           <label> Default Plans</label>

         <telerik:RadComboBox ID="RP_ID" runat="server"  TabIndex ="2" Width="100%" DataTextField="Details" Skin="Simple"
                DataValueField="Default_Plan_ID" >  </telerik:RadComboBox>

    </div>
     </div>
        <div class="col-sm-4">
      <div class="form-group">  
          <label class="hidden-xs"><br /></label>
          <telerik:RadButton CssClass="btn btn-success" ID="AddBtn"  TabIndex ="2" runat="server" Text="Add" Skin="Simple" /> 
                <telerik:RadButton CssClass="btn btn-primary2" ID="ModBtn" runat="server" TabIndex ="4"  Text="View/Modify"  Skin="Simple"
                    CausesValidation="False"  /> 
                <telerik:RadButton CssClass="btn btn-default" ID="DelBtn" runat="server" TabIndex ="5" Text="Delete" Skin="Simple"  /> 
      </div>

</div>
    
        
     </asp:Panel>
    </div>
</asp:Content>
