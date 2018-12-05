<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POP_DefaultPlan.aspx.vb" Inherits="SalesWorx_BO.POP_DefaultPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <base target="_self">
    <title>Route Planner</title>
         <script type="text/javascript" src="../js/jquery-1.3.2.min.js"></script>
<link href="../facebox/facebox.css" media="screen" rel="stylesheet" type="text/css"/>
<script src="../facebox/facebox.js" type="text/javascript"></script> 
<link href="../styles/UpdateProgress.css" rel="stylesheet" type="text/css">
<link href="../assets/css/bootstrap.css" rel="stylesheet" type="text/css">
<link href="../assets/css/style.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">
</head>
<script>
    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

   
</script>

<body>
    
    <form id="frmPopupCustList" runat="server" class="outerform">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <input id="DayText" name="DayText" type="hidden"  runat="server"/>

    
	
	<div id="contentofpage" class="popupcontentblk" style="width:auto;">
    <p><asp:Label ID="ErrMsg" runat="server" ForeColor="Red"></asp:Label></p>
	<div class="pgtitile3">Setup Default Plan</div>
	    <div class="row">
            <div class="col-sm-6">
                <label>Comments</label>
                <div class="form-group">
                    <asp:TextBox ID="UserComments" CssClass ="calinput" Width="100%" runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline-blk">
                    <asp:CheckBox ID="TimeSelection" runat="server" Text="Enable Time Selection" AutoPostBack="True" CssClass="calinput" />
                </div>
                <div class="form-group">
                    <asp:UpdatePanel ID="TimerPanel" runat="server" UpdateMode="Conditional">
				    <ContentTemplate>
				     <asp:Panel ID="TimePanel" runat="server" Visible="false">
                         <div class="row">
                             <div class="col-xs-3">
                                 <label><asp:Label ID="Label1"  runat="server" Text="Start Time"></asp:Label></label>
                                 <div class="form-group">
                                     <asp:DropDownList ID="StartHH" Width="100%" runat="server"></asp:DropDownList>
                                 </div>
                             </div>
                             <div class="col-xs-3">
                                 <label><br /></label>
                                 <div class="form-group">
                                     <asp:DropDownList ID="StartMM" Width="100%" runat="server"></asp:DropDownList>
                                 </div>
                             </div>
                             <div class="col-xs-3">
                                 <label><asp:Label ID="Label2"  runat="server" Text="End Time"/></label>
                                 <div class="form-group">
                                     <asp:DropDownList ID="EndHH" Width="100%" runat="server"></asp:DropDownList>
                                 </div>
                             </div>
                             <div class="col-xs-3">
                                 <label><br /></label>
                                 <div class="form-group">
                                     <asp:DropDownList ID="EndMM" Width="100%" runat="server"></asp:DropDownList>
                                 </div>
                             </div>
                         </div>
				     
					
						       </asp:Panel>
						       </ContentTemplate>
						       <Triggers>
						       <asp:AsyncPostBackTrigger ControlID="TimeSelection" EventName="CheckedChanged" />
						       </Triggers>
						       </asp:UpdatePanel>
                </div>
            </div>
       </div>	

			<div align="center">
				<asp:Button ID="ResetBtn" runat="server" Text="Reset Day" CssClass="btn btn-default" />
                <asp:Button ID="DayOffBtn" runat="server" Text="Day Off" CssClass="btn btn-primary2" />
                <asp:Button ID="RSWEBtn" runat="server" Text="Mark as  Weekend"  CssClass="btn btn-success" />
                <asp:Button ID="MAWDBtn" runat="server" Text="Reset Working Day"  CssClass="btn btn-default" />
			</div>
							
	</div> 
	
    </form>

</body>
</html>
