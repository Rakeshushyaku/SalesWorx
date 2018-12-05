Imports SalesWorx.BO.Common
Imports System.Web.UI.WebControls
Imports System.Resources
Imports System.Configuration
Imports log4net
Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices.Marshal

Partial Public Class CustomerLocation
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim ErrorResource As ResourceManager
    Dim oGeocodeList As List(Of String)
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Page.Response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, post-check=0, pre-check=0")
        If Not IsPostBack Then


            oGeocodeList = New List(Of [String])()
            ' Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            'ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)

            'Page.ClientScript.RegisterStartupScript(GetType(String), "Intialization", "initialize();", True)
            Dim visitid As String = Request.QueryString("VisitID").ToString()
            
            BindMapData()

            '   MSG_Date.Text = DateTime.Now.ToShortDateString
        End If
        'lblmsg.Text = ""

    End Sub
    'Sub MessageBoxValidation(ByVal str As String)
    '    lblMessage.ForeColor = Drawing.Color.Red
    '    lblinfo.Text = "Validation"
    '    lblMessage.Text = str
    '    MpInfoError.Show()
    '    MpInfoError.Show()
    '    Exit Sub
    'End Sub




 








 

 


    Private Sub BindMapData()

        Dim cmd As SqlCommand
        Dim conn As SqlConnection = New SqlConnection
        Dim objDA As New SqlDataAdapter
        Dim dt As New DataTable
        Dim oMessageList As New List(Of String)()
        Dim temp_geocode As String = ""
        Dim temp_mapinfo
        Try
            oGeocodeList = New List(Of String)()

            Dim visitid As String = Request.QueryString("VisitID").ToString()
            conn.ConnectionString = ConfigurationSettings.AppSettings("SQLConnString")
            cmd = New SqlCommand("app_CustomerVisitLocation", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conn
            cmd.Parameters.Add(New SqlParameter("@VisitID", SqlDbType.VarChar, 500))
            cmd.Parameters("@VisitId").Value = visitid
            objDA.SelectCommand = cmd
            conn.Open()
            objDA.Fill(dt)



            Session.Remove("VisitDate")
            Session("VisitDate") = dt



            For Each t As DataRow In dt.Rows

                If Not t("Lat") Is DBNull.Value And Not t("Lng") Is DBNull.Value Then


                    If t("Category").ToString() = "CUSTOMER" And CDec(t("Lat").ToString()) <= 0 Then
                        customercol.Visible = False
                    End If
                    If t("Category").ToString() = "START" And CDec(t("Lat").ToString()) <= 0 Then
                        startcol.Visible = False
                    End If
                    If t("Category").ToString() = "END" And CDec(t("Lat").ToString()) <= 0 Then
                        endcol.Visible = False
                    End If

                    If CDec(t("Lat").ToString()) > 0 Then
                        temp_geocode = " '" & t("Lat").ToString() & "," & t("Lng").ToString() & "," & t("Category").ToString() & "'"


                        oGeocodeList.Add(temp_geocode)

                        temp_mapinfo = " '<span class=formatText>" & Replace(t("LocationType"), "'", "") & "</span>' "
                        oMessageList.Add(temp_mapinfo)
                    End If
                End If
            Next



            Dim message As [String] = String.Join(",", oMessageList.ToArray())
            Dim geocodevalues = String.Join(",", oGeocodeList.ToArray())
            ClientScript.RegisterArrayDeclaration("locationList", geocodevalues)
            ClientScript.RegisterArrayDeclaration("message", message)
            Page.ClientScript.RegisterStartupScript(GetType(String), "Intialization", "initialize();", True)
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        Finally
            conn.Close()
            cmd.Dispose()
        End Try

    End Sub

  
End Class