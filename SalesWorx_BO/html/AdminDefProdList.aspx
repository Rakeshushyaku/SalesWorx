<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="AdminDefProdList.aspx.vb" Inherits="SalesWorx_BO.AdminDefProdList" %>
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
                          $get('<%=UpdateProgress1.ClientID %>').style.display = 'block';
                 postBackElement.disabled = true;
           }

         function EndRequest(sender, args) {
                $get('<%=UpdateProgress1.ClientID %>').style.display = 'none';
                 postBackElement.disabled = false;
           } 

</script>
       <asp:UpdateProgress ID="UpdateProgress1"  AssociatedUpdatePanelID="UpPanel" runat="server">
         <ProgressTemplate>
          <div style="z-index:9999; position:absolute; top:48%; left:48%;">
         <img src="../images/Progress.gif" alt="Processing..." style="vertical-align:middle;" />            
           <span style="font-size:12px; font-weight:700px; color:#3399ff;">Processing... </span></div>
           </ProgressTemplate>
           </asp:UpdateProgress>
	<asp:UpdatePanel runat="server" ID="UpPanel">
	<ContentTemplate>
  <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">
	
		<div class="pgtitileposition">
	<span class="pgtitile3">Setup Default Product List</span></div>
	<div id="pagenote" >This screen may be used to create a product list for all sales representatives</div>	
	
	<table width="auto" border="0" cellspacing="0" cellpadding="0" id="tableform">
	<tr><th></th></tr>
	
  <tr>
  <td>
      <asp:Label ID="lblLable" runat="server" Text="Please Select a Organisation"></asp:Label>&nbsp;&nbsp;
        <asp:DropDownList ID="ddOraganisation" AutoPostBack="true" runat="server">
          </asp:DropDownList>
      </td>
      
  </tr>
  
  <tr>
  <td>
  
  
      <table>
    <tr>
    <td><asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Products Available:"></asp:Label></td>
    <td></td>
         <td><asp:Label Font-Bold="true" ID="lblProdAssign" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
    <td>
     
        <asp:ListBox Rows=20 SelectionMode="Multiple"  Width="300" ID="lstDefault" runat="server">
        </asp:ListBox> </td>
        <td>
        <table>
            <tr><td>
                <asp:Button align="center" Width="100" ID="btnAdd" runat="server" Text="Add -> " /></td></tr>
                   <tr><td>
                <asp:Button  Width="100" ID="btnAddAll" runat="server" Text="Add All -> " /></td></tr>
                   <tr><td>
                <asp:Button Width="100" ID="btnRemove" runat="server" Text=" <- Remove" /></td></tr>
                   <tr><td>
                <asp:Button Width="100" ID="btnRemoveAll" runat="server" Text=" <- Remove All" /></td></tr>
        </table>
        </td>
         <td> <asp:ListBox Rows=20 SelectionMode="Multiple"  Width="300" ID="lstSelected" runat="server">
        </asp:ListBox> </td> 
    </tr>
    </table>
  
  
  </td>
  </tr>
   
    <tr><th></th></tr>
    </table>

    </ContentTemplate>
	</asp:UpdatePanel>

   

</asp:Content>
