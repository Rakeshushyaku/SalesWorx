Imports SalesWorx.BO.Common
Imports System.Configuration.ConfigurationManager
Public Class SiteMaster
    Inherits MasterPage

    Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Dim _antiXsrfTokenValue As String
    Dim HasPermission As Boolean = False
    Dim HasPermission1 As Boolean = False
    Dim objLogin As New SalesWorx.BO.Common.Login
    Protected Sub Page_Init(sender As Object, e As System.EventArgs)

        ' The code below helps to protect against XSRF attacks
        Dim requestCookie As HttpCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If ((Not requestCookie Is Nothing) AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue)) Then
            ' Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie As HttpCookie = New HttpCookie(AntiXsrfTokenKey) With {.HttpOnly = True, .Value = _antiXsrfTokenValue}
            If (FormsAuthentication.RequireSSL And Request.IsSecureConnection) Then
                responseCookie.Secure = True
            End If
            Response.Cookies.Set(responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Private Sub master_Page_PreLoad(sender As Object, e As System.EventArgs)


        If (Not IsPostBack) Then
            ' Set Anti-XSRF token
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)
        Else
            ' Validate the Anti-XSRF token
            If (Not DirectCast(ViewState(AntiXsrfTokenKey), String) = _antiXsrfTokenValue _
                Or Not DirectCast(ViewState(AntiXsrfUserNameKey), String) = If(Context.User.Identity.Name, String.Empty)) Then
                Throw New InvalidOperationException("Validation of Anti-XSRF token failed.")
            End If
        End If
    End Sub

    Private Sub Page_Init1(sender As Object, e As EventArgs) Handles Me.Init
        Dim t As String
        t = Page.Title
        Page.Title = t & " " & AppSettings("PageTitle")

    End Sub

   

    'Protected Sub logo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles logo1.Click
    '    If Not IsNothing(Session("USER_ACCESS")) Then
    '        ManageAuthentication.HasPermission(Session("USER_ACCESS"), "P87", HasPermission)
    '        If HasPermission = True Then

    '            Response.Redirect("ASRDashboard.aspx", False)
    '        Else
    '            Response.Redirect("Welcome.aspx", False)
    '        End If
    '    End If
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim frm = Me.FindControl("frm")
        'If Session("NewOrder") Is Nothing Then
        '    Session("NewOrder") = "Y"
        'Else
        '    Session("NewOrder") = Nothing
        'End If

        If Not IsNothing(Session("USER_ACCESS")) Then
            Me.lblLoginUser.Text = (New SalesWorx.BO.Common.User).GetUserName(Session("USER_ACCESS").UserID).ToString

        Else
            Me.lblLoginUser.Text = ""
            Response.Redirect("Login.aspx")

        End If

        If Not IsNothing(Session("dtuser")) Then
            For Each dr As DataRow In CType(Session("dtuser"), DataTable).Rows
                ilogodash.Src = AppSettings("LogoPath")
                ilogoWelcome.Src = AppSettings("LogoPath")
                If dr("Page_Id").ToString() <> "P87" Then
                    If dr("Page_Id") = "P118" Then
                        Dim k = 1
                    End If
                    If Not dr("Page_Id") Is DBNull.Value Then

                        Dim pageid As HtmlAnchor = DirectCast(frm.FindControl(dr("Page_Id").ToString()), HtmlAnchor)
                        If Not pageid Is Nothing Then
                            pageid.Visible = True
                        End If
                    End If
                    If Not dr("Parent_Id") Is DBNull.Value Then

                        Dim menuid As HtmlAnchor = DirectCast(frm.FindControl(dr("Parent_Id").ToString()), HtmlAnchor)
                        If Not menuid Is Nothing Then
                            menuid.Visible = True
                        End If
                    End If

                    'If dr("Page_ID").ToString() = "P45" Then
                    '    If Session("NewOrder") Is Nothing Then
                    '        Session("NewOrder") = "Y"
                    '    Else
                    '        Session("NewOrder") = Nothing
                    '    End If
                    'End If

                End If
            Next

        End If
        If Not Session("PageID") Is Nothing Then
            If Not Me.FindControl(Session("PageID")) Is Nothing Then
                Dim activepage As HtmlAnchor = CType(Me.FindControl(Session("PageID")), HtmlAnchor)
                activepage.Attributes.Add("class", "menu-selected")
            End If
        End If
        If Not IsNothing(Session("USER_ACCESS")) Then
            ManageAuthentication.HasPermission(Session("USER_ACCESS"), "P87", HasPermission)
            If HasPermission = True Then
                dash.Visible = True
                welcome.Visible = False
            Else
                dash.Visible = False
                welcome.Visible = True
            End If
        End If
    End Sub

    Protected Sub logout_ServerClick(sender As Object, e As EventArgs) Handles logout.ServerClick
        If Not Session.Item("USER_ACCESS") Is Nothing Then
            objLogin.SaveUserLog("100", "Log Out", "O", "USER MANAGEMENT", "LOGOUT", CType(Session("User_Access"), UserAccess).UserID.ToString(), "Logout By: " & CType(Session("User_Access"), UserAccess).UserID.ToString(), CType(Session("User_Access"), UserAccess).UserID.ToString(), "0", "0")
            Session.Abandon()
            Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
        End If
        Session("ROUTE_FSR_ID") = Nothing
        Session.Remove("USER_ACCESS")
        Session("USER_ACCESS") = Nothing
        Session("MAIN_MENU") = Nothing
        Session("SUB_MENU") = Nothing
        Session("ORDER_LOTS") = Nothing
        Session("NewOrder") = Nothing
        Response.Redirect("Login.aspx")
    End Sub


End Class