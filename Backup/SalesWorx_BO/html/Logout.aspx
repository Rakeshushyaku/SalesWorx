<%@ Page language="VB" debug="true" %>
<%@ Import  Namespace ="SalesWorx.BO.Common"%>
<%
    Dim objLogin As New SalesWorx.BO.Common.Login
    Dim objUserAccess As UserAccess
    If Not Session.Item("USER_ACCESS") IS Nothing 
    objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
    objLogin.SaveUserLog("100", "Log Out", "O", "USER MANAGEMENT", "LOGOUT", objUserAccess.UserID, objUserAccess.UserID, objUserAccess.UserID, "0", "0")
    End If   
Session("ROUTE_FSR_ID")=Nothing
Session("USER_ACCESS")=Nothing
Session("MAIN_MENU")=Nothing
Session("SUB_MENU")=Nothing
Session("ORDER_LOTS")=Nothing
    Response.Redirect("Login.aspx")

%>

