<%@ Page Title="Approve Route Plan" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="ApproveRoutePlan.aspx.vb" Inherits="SalesWorx_BO.ApproveRoutePlan" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <input type="hidden" name="Action_Mode" id="Action_Mode" value="" runat="server">
   
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
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'block';
                 postBackElement.disabled = true;
             }

         }

         function EndRequest(sender, args) {
             if (postBackElement.id == 'ctl00_ContentPlaceHolder1_Route_FSR_ID') {
                 $get('ctl00_ContentPlaceHolder1_UpdateProgress1').style.display = 'none';
                 postBackElement.disabled = false;
             }
         } 

</script>
	<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
  <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">

	<tr>
		<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Approve Route Plan</span></div>
	<div id="pagenote" >This screen may be used to approve prepared route plans.</div>	
		
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
	  <tr>
  <td>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
   <ContentTemplate>
      <table>   
       <tr><th></th></tr>   
       	 <asp:Panel ID="VanPanel" runat="server">
  <tr>
    <td><label>Van: </label> </td>
     <td><asp:DropDownList  ID="Route_FSR_ID" runat="server" AutoPostBack="true"
                    DataTextField="SalesRep_Name" CssClass="input" DataValueField="SalesRep_ID" AppendDataBoundItems="true">
                    <asp:ListItem>-- Select a Van --</asp:ListItem>
              </asp:DropDownList> </td>
 
    </tr>
    </asp:Panel>
    <tr> 
            <td ><label>Route Plans: </label></td>
            <td><asp:DropDownList ID="RoutePlan" runat="server" DataTextField="Details" CssClass="input" DataValueField="FSR_Plan_ID" AppendDataBoundItems="false">
                        </asp:DropDownList>  
        </td>
          </tr>
           <tr>
           <td></td> 
            <td align="left" style="padding-left:20px;"> <asp:Button ID="ViewAppBtn" CssClass="btnInput" runat="server" Text="View/Approve" /></td>
            </tr>
             <tr><th></th></tr>
            </table>
            
            
           </ContentTemplate>
            </asp:UpdatePanel>
            </td>
              
              </tr>
      <asp:RequiredFieldValidator runat="server" ID="SalesRepReq"
            ControlToValidate="Route_FSR_ID"
            Display="None" InitialValue="-- Select a Van --"
            ErrorMessage="<b>Required Field Missing</b><br />Select a SalesRep." />
    <ajaxToolkit:ValidatorCalloutExtender  runat="Server" ID="SalesRepReqCall" Width="30%" 
            TargetControlID="SalesRepReq"  />
    <asp:RequiredFieldValidator runat="server" ID="RouteReq"
            ControlToValidate="RoutePlan"
            Display="None" InitialValue="  -- Select a route plan -- "
            ErrorMessage="<b>Required Field Missing</b><br />Select a FSR Plan." />
    <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="StartDateReqE" Width="30%"
            TargetControlID="RouteReq"/>
    </table>
	<br/>
	<br/>
	</td> 
		</tr>
</table>
	</ContentTemplate>
	</asp:UpdatePanel>	
		  <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
         <ProgressTemplate>
          <div style="z-index:9999; position:absolute; top:48%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>
</asp:Content>
