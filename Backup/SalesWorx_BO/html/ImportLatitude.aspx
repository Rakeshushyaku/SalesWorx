<%@ Page Title="Import Geolocation Data" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ImportLatitude.aspx.vb" Inherits="SalesWorx_BO.ImportLatitude" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
         <input type="hidden" name="Action_Mode" value="" />
      <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Import Geolocation Data</span></div>
		
	   <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
          <ProgressTemplate>
          <div style="z-index:9999; position:absolute; top:48%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color: #337AB7  ;">Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>

	    <table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">

		 <tr>
    <td><label>Select a File </label> </td>
    <td><asp:FileUpload ID="ExcelFileUpload" runat="server" /></td>
        <td>     <asp:Button ID="BtnImport" runat="server" Text="Import" CssClass="btnInputGreen" />
                  <asp:Button ID="DummyImBtn" style="display:none"  runat="server" Text="Reimport" />
                  &nbsp;&nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" 
                 CssClass="btnInputOrange" />
        </td>  
         <td>    
           <asp:Button ID="DummyReimBtn" style="display:none"  runat="server" Text="Reimport" />
         </td>
         <td>     
           
         
<tr>
<td colspan="4">

<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
        <div id="divResult" runat="server" style="width:95%; height:365px; overflow:scroll; text-align:left; padding:10px; ">
         <asp:Label ID="lblResult" runat="server"></asp:Label>       
        </div>
    	</ContentTemplate>
	<Triggers>
	<asp:AsyncPostBackTrigger ControlID="DummyImBtn" EventName="Click" />
	<asp:AsyncPostBackTrigger ControlID="DummyReimBtn" EventName="Click" />
	</Triggers>
	</asp:UpdatePanel>	
</td>

</tr>
  	
	  </table>
	<br/>
	<br/>
	</tr>
</table>
</asp:Content>
