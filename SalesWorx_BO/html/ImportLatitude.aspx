<%@ Page Title="Import Geolocation Data" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/Site.Master" CodeBehind="ImportLatitude.aspx.vb" Inherits="SalesWorx_BO.ImportLatitude" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
             //            if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
             $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
             postBackElement.disabled = true;
             //           }

         }

         function EndRequest(sender, args) {
             //        if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
             $get('<%= UpdateProgress1.ClientID %>').style.display = 'none';
             postBackElement.disabled = false;
             //       }
         } 

</script>
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
         <input type="hidden" name="Action_Mode" value="" />
       <h4>Import Geolocation Data</h4>
	 
		
	   <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
          <ProgressTemplate>
          <div  class ="overlay">
         <img src="../assets/img/ajax-loader.gif" alt="Processing..."  />            
           <span>Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>
         <div class="form-group">
	    <label>Select a File </label>
   <asp:FileUpload ID="ExcelFileUpload" runat="server" />
            </div>
         <div class="form-group">
            <asp:Button ID="BtnImport" runat="server" Text="Import"  CssClass ="btn btn-warning" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                  <asp:Button ID="btnBack" runat="server" Text="Back" CssClass ="btn btn-default" />
            <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
         </div>
           
           
         


<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
        <div id="divResult" runat="server">
         <asp:Label ID="lblResult" runat="server"></asp:Label>       
        </div>
    	</ContentTemplate>
	<Triggers>
	<asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	</Triggers>
	</asp:UpdatePanel>	

	
</asp:Content>
