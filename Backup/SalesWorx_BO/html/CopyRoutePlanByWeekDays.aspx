<%@ Page Title="Copy Route Plan Week Days" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="CopyRoutePlanByWeekDays.aspx.vb" Inherits="SalesWorx_BO.CopyRoutePlanByWeekDays" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="../js/RoutePlanner.js"></script>
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
         <input type="hidden" name="Action_Mode" value="" />
      <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">

    	<div class="pgtitileposition">
	<span class="pgtitile3">Copy Route Plan By Week Days</span></div>
	<div id="pagenote" >This screen may be used to create a new route plan for a van using an existing route plan. Copy route plan will copy the customers by matching the days of the week.</div>	
	
     <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
         <ProgressTemplate>
          <div style="z-index:9999; position:absolute; top:48%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>
	<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
	    <table width="auto" border="0" cellspacing="0" cellpadding="4" id="tableform">
	 <tr><th></th></tr>
	 <asp:Panel ID="SRepPanel" runat="server" Visible="false">
	 <tr>
    <td><label>Van: </label> </td>
     <td><asp:DropDownList  ID="Route_FSR_ID" runat="server" AutoPostBack="true"
                    DataTextField="SalesRep_Name" Width ="250px" TabIndex ="1"  CssClass="input" DataValueField="SalesRep_ID" AppendDataBoundItems="true">
                    <asp:ListItem>-- Select a Van --</asp:ListItem>
              </asp:DropDownList> </td>
 
    </tr>
	 </asp:Panel>
	<asp:Panel runat="server" ID="BtnPanel" Visible="false">
	            <td class="txtBold"><label>From :</label></td>
            <td>
                <asp:DropDownList ID="Route_ID" Width ="250px" TabIndex ="2"  CssClass="input" runat="server" 
                    DataTextField="Details" DataValueField="FSR_Plan_ID">
                </asp:DropDownList>
            &nbsp;</td>
            <td class="txtBold">
              <asp:Button ID="CopyBtn" CssClass="btnInputBlue" TabIndex ="4" runat="server" Text="Copy" OnClientClick="return ValidateForm()" />
            <label>To:</label></td>
            <td>
             <asp:DropDownList ID="RP_ID" Width ="250px" TabIndex ="3" CssClass="input" runat="server" 
                    DataTextField="Details" DataValueField="FSR_Plan_ID">
                </asp:DropDownList>
           </td>
          </tr>
          
	
	</asp:Panel>
  <tr><th></th></tr>
    </table>
	</ContentTemplate>
	</asp:UpdatePanel>	
<br/>
	<br/>
	</td> 
	</tr>
	

</table>
</asp:Content>
