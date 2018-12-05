<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/html/DashboardSection.Master" CodeBehind="DashInformation.aspx.vb" Inherits="SalesWorx_BO.DashInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div style="background-color:White; padding:20px 4px; border:solid 5px #006c7b;"> 
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
          ElseIf Mode = "1" Then
              Response.Write("<strong>Error</strong><BR><BR>")
            Response.Write(Error_No & "-" & Error_Desc)
          End If
%></div>
</asp:Content>
