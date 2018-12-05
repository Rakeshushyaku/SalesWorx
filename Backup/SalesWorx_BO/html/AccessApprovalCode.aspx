<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AccessApprovalCode.aspx.vb" Inherits="SalesWorx_BO.AccessApprovalCode" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">

      .overlay {
        position: fixed;
        z-index: 1000;
        top: 49%;
        left: 49%;
        width: 100%;
        height: 100%;
        
        
            }
   
  
</style>
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
              //  $get('UpdateProgress1').style.display = 'block'
              $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'block';
              postBackElement.disabled = true;
          }


          function EndRequest(sender, args) {
              $get('<%= Me.UpdateProgress1.ClientID %>').style.display = 'none';
              postBackElement.disabled = false;
          } 

</script>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>
   
  
  <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Access Approval Code</span></div>
	<div id="pagenote" >This screen may be used for getting approval code for a van.</div>	
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
	<tr><th></th></tr>

   
 <tr runat="server" id="rowCode" visible = "false">
   <td><label>Approval Code:</label> </td>
    <td> <asp:Label Font-Bold="true" Font-Size="X-Large" ForeColor="Green"  ID="lblApprovalCode" runat="server"></asp:Label> </td>
  </tr>
 

  <tr>
    <td><label>Van:</label> </td>
    <td> <asp:DropDownList ID="SalesRep_ID" runat="server" CssClass="inputSM" TabIndex ="2" Width="200px" DataTextField="SalesRep_Name"
    DataValueField="SalesRep_ID" > 
    
              </asp:DropDownList></td>
    </tr>
 
  <tr>
    <td colspan="2" align="center" > <asp:Button CssClass="btnInputBlue" ID="BtnAccess"  TabIndex ="2" runat="server" Text="Get Approval Code" />&nbsp;
               </td>
  </tr>
  
 



    <tr><th></th></tr>
    </table>
	<br/>
	<br/>
	</td> <!-- "contentofpage" ends in this td -->
	</tr>
	</table>
	     <asp:Button ID="btnHidden" runat="Server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="MpInfoError" runat="server" BackgroundCssClass="modalBackground"
                            TargetControlID="btnHidden" PopupControlID="pnlConfirm" CancelControlID="btnClose"
                            Drag="true" />
                        <asp:Panel ID="pnlConfirm" Width="400" runat="server" CssClass="modalPopup" Style="display: none">
                            <table id="tableinPopupErr" width="400" cellpadding="10" style="background-color: White;">
                                <tr align="center">
                                    <td align="center" style="cursor: move; background-color: #3399ff; text-align: center;
                                        border: solid 1px #3399ff; color: White; padding: 3px">
                                        <asp:Label ID="lblinfo" runat="server" Font-Size="14px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center">
                                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" />
                                        <asp:Label ID="lblMessage" runat="server" Font-Size="13px" Font-Names="Calibri"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: center;">
                                        <asp:Button ID="btnClose" runat="server" Text="Ok" CssClass="btnInputBlue" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
	     </ContentTemplate>
 </asp:UpdatePanel>	
      <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpdatePanel1" runat="server">
     <ProgressTemplate>
     <asp:Panel ID="Panel11" CssClass="overlay" runat="server">
     
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span>
       </asp:Panel>
           </div>
       
         
    </ProgressTemplate>
            </asp:UpdateProgress> 
</asp:Content>
