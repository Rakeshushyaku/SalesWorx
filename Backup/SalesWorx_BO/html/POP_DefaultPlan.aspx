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
<link href="../styles/swxstyle.css" rel="stylesheet" type="text/css">
<link href="../styles/salesworx.css" rel="stylesheet" type="text/css">
<link rel="stylesheet" type="text/css" href="../styles/superfish.css" media="screen">
<link href="../styles/stylesheet.css" rel="stylesheet" type="text/css">
</head>
<body>
    
    <form id="frmPopupCustList" runat="server" class="outerform">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <input id="DayText" name="DayText" type="hidden"  runat="server"/>

    <table width="100%"  height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	

	<tr>
	
	<td id="contentofpage" width="100%" height="100%" class="topshadow">
	<span class="pgtitile3">Setup Default Plan</span>
		
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableformNrml">
	<tr>
	<td align="center" >  <asp:Label ID="ErrMsg" runat="server" ForeColor="Red"></asp:Label></td>
	</tr>
  	 <tr>
			<td style="padding-left:12px;"> <label>Comments:&nbsp;&nbsp;</label>
                       <asp:TextBox ID="UserComments" CssClass ="calinput" runat="server"></asp:TextBox>
			</td>
			
	</tr>
	<tr>
		<td style="padding-left:12px;">
         <asp:CheckBox ID="TimeSelection" runat="server" Text="Enable Time Selection" 
                               AutoPostBack="True" CssClass="calinput" /></td>
    </tr> 
      
      <tr>
	        <td style="padding-left:12px;">
			    <asp:UpdatePanel ID="TimerPanel" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				 <asp:Panel ID="TimePanel" runat="server" Visible="false">
				 <table>
				 <tr>
				 <td>
				 <label><asp:Label ID="Label1"  runat="server" Text="Start Time"></asp:Label></label>
           &nbsp;
            <asp:DropDownList ID="StartHH" runat="server">
            </asp:DropDownList>&nbsp;<asp:DropDownList ID="StartMM" runat="server">
            </asp:DropDownList>
             &nbsp;&nbsp;
            <label><asp:Label ID="Label2"  runat="server" Text="End Time"/></label>
          &nbsp;<asp:DropDownList ID="EndHH" runat="server">
            </asp:DropDownList>&nbsp;<asp:DropDownList ID="EndMM" runat="server">
            </asp:DropDownList>
            <br />

				 </td>
				 </tr>
				 </table>
					
						   </asp:Panel>
						   </ContentTemplate>
						   <Triggers>
						   <asp:AsyncPostBackTrigger ControlID="TimeSelection" EventName="CheckedChanged" />
						   </Triggers>
						   </asp:UpdatePanel>
						      </td>
						    </tr>
   
 
    		<tr>
								<td style="padding-left:12px;" class="txtSM" align="center">
									
									<asp:Button ID="ResetBtn" runat="server" Text="Reset Day" CssClass="btnInputGreen" />
                                    <asp:Button ID="DayOffBtn" runat="server" Text="Day Off" CssClass="btnInputRed" />
                                    <asp:Button ID="RSWEBtn" runat="server" Text="Mark as  Weekend" 
                                        CssClass="btnInputBlue" />
                                    <asp:Button ID="MAWDBtn" runat="server" Text="Reset Working Day" 
                                        CssClass="btnInputGrey" />
								
			<br />
			<br />
							
								
			
								</td>
							</tr>
    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>

</table>
    </form>

</body>
</html>
