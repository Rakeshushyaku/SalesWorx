<%@ Page Title="Information" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DefaultLayout.Master" CodeBehind="Information.aspx.vb" Inherits="SalesWorx_BO.Information" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#f6f6f6">
		<tr>
	<td height="12%">  
	
   </td>
	</tr>

	<tr>
	
	<td id="contentofpage" width="100%" height="76%" class="topshadow">

		<div class="pgtitileposition">
	<span class="pgtitile3">Information</span></div>
	<div id="pagenote" >  </div>
<table width="400" height="200" border="0" cellpadding="0" cellspacing="0" id="oldpagetable">
        
        <tr>
         
          
    <td align="center" valign="top" class="txtBold"> <br>
      <%
          Dim Mode As String
          Dim Error_No As String
          Dim Error_Desc As String
          Dim Info_Msg As String
          Dim Next_URL As String
         
         Mode = Request("mode")
         
          If Mode = "0" Then
              Info_Msg = Request("msg")
              Next_URL = Request("next")
          ElseIf Mode = "1" Then
              Error_No = Request("errno")
              Error_Desc = Request("msg")
              Next_URL = Request("next")
          End If
          If Mode = "0" Then    'information message....
              '	Response.Write("<strong>Information</strong><BR><BR>")
              Response.Write(Info_Msg)
              Response.Write("<BR><BR>Click here to <a href='" & Next_URL & "'>continue</a>")
          ElseIf Mode = "1" Then
              Response.Write("<strong>Error</strong><BR><BR>")
              Response.Write(Error_No & "-" & Error_Desc)
              Response.Write("<BR><BR>Click here to <a href='" & Next_URL & "'>continue</a>")
          End If
%>
    </td>
          
        </tr>
       
      </table>
<br/>
	<br/>


	
	</td> 
	
	</tr>


	<tr>
	
	</tr>

</table>
</asp:Content>
