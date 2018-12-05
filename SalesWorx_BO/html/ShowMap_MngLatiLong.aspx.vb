Imports SalesWorx.BO.Common
Imports log4net

Public Class ShowMap_MngLatiLong

    Inherits System.Web.UI.Page
    Dim objLatitude As New LatiLongitude
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Dt As New DataTable
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Shared _strDefLat As String = "0.00000"
    Private Shared _strDefLong As String = "0.0000"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CustID.Value = Request.QueryString("CustID")
            SiteID.Value = Request.QueryString("SiteID")
            CustLng.Value = Request.QueryString("CustLong")
            CustLat.Value = Request.QueryString("CustLat")
            hgeo_mod.Value = Request.QueryString("geo_mod")
            If hgeo_mod.Value.Trim() = "E" Then
                span2.InnerHtml = "Explicit Capture Location"
                btnUpdateLastVisit.Text = "Update From Explicit Capture "
            End If

            _strDefLat = ConfigurationSettings.AppSettings("DefaultLat")
            _strDefLong = ConfigurationSettings.AppSettings("DefaultLong")

            txtLocLatitude.Text = IIf(Request.QueryString("Lat") = "0", _strDefLat, Request.QueryString("Lat"))
            txtLocLong.Text = IIf(Request.QueryString("Long") = "0", _strDefLong, Request.QueryString("Long"))




            If Val(Request.QueryString("Lat")) = 0 And Val(Request.QueryString("Long")) = 0 Then
                ' lblNoMap.Visible = True 
                'mapwrap.Visible = False
            Else
                ' lblNoMap.Visible = False
                hfLng.Value = Request.QueryString("Long")
                hflat.Value = Request.QueryString("Lat")
                txtLocLatitude.Text = Request.QueryString("Lat")
                txtLocLong.Text = Request.QueryString("Long")

            End If
            If Request.QueryString("CustLat").ToString() = "-1" Then
                CustLat.Value = "0.0000"
            End If

            If Request.QueryString("CustLong").ToString() = "-1" Then
                CustLng.Value = "0.0000"
            End If
            If Not Request.QueryString("Type") Is Nothing And Request.QueryString("Type") = "Visits" Then
                tips.Visible = True
               
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)
            Else
                tips.Visible = False
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "RefreshChart();", True)
            End If
        End If
    End Sub
    Protected Sub btnSet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateLoc.Click
        Me.lbl_msg.Text = ""
        If IsValidLatitude(Me.txtLocLatitude.Text.Trim()) = False Then

            Me.lbl_msg.Text = "Please enter valid Latitude Value."
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)

            'MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsValidLatitude(Me.txtLocLong.Text.Trim()) = False Then
            Me.lbl_msg.Text = "Please enter valid Longitude Value."
            'MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        Dim obj As New LatiLongitude

        Dim success As Boolean = False
        Try
            obj.Latitude = Convert.ToDouble(txtLocLatitude.Text.Trim())
            obj.Longitude = Convert.ToDouble(txtLocLong.Text.Trim())
            obj.CustomerId = CustID.Value.Trim()
            obj.SiteUserId = SiteID.Value.Trim()

            If obj.UpdateLatiLongitude(Err_No, Err_Desc) = True Then
                success = True
                Me.lbl_msg.Text = "Successfully updated."
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)

                ' MessageBoxValidation("Successfully updated.", "Information")
                ' MPEDetails.VisibleOnPageLoad = False
                ' Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ' BindCustomerData()
                ' Resetfields()

                ' ClassUpdatePnl.Update()
            Else
                success = False
                Me.lbl_msg.Text = "Could not update."
                ' MessageBoxValidation("Could not update.", "Information")
                ' MPEDetails.VisibleOnPageLoad = False
                '  Resetfields()
                ' Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                'BindCustomerData()
                'ClassUpdatePnl.Update()
            End If



        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Public Function IsValidLatitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function
    Public Function IsValidLongitude(ByVal LatStr As String) As Boolean
        Dim _isDouble As System.Text.RegularExpressions.Regex = New  _
                Regex("^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}")
        Return _isDouble.Match(LatStr).Success
    End Function
    Sub MessageBoxValidation(ByVal str As String, ByVal Title As String)
        ' RadWindowManager1.RadAlert(str, 330, 180, Title, "alertCallBackFn")
        Exit Sub
    End Sub
    Protected Sub btnUpdateLastVisit_Click(sender As Object, e As EventArgs)

        ' MessageBoxConfirm("Your amount exceeded the default Limit. Do you want to send mail to designated company user ?", "Information")
        Me.lbl_msg.Text = ""
        If Me.CustLng.Value.Trim() = "-1" Or Me.CustLat.Value.Trim() = "-1" Then

            If hgeo_mod.Value.Trim() = "E" Then
                Me.lbl_msg.Text = "Explicit capture  location does not exist."
            Else
                Me.lbl_msg.Text = "Last visited location does not exist."
            End If

            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)
            Exit Sub
        End If
        If IsValidLatitude(Me.CustLng.Value.Trim()) = False Then

            If hgeo_mod.Value.Trim() = "E" Then
                Me.lbl_msg.Text = "Invalid explicit capture  location Value."
            Else
                Me.lbl_msg.Text = "Invalid last visited Latitude Value."
            End If



            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)
            ' MapWindow.VisibleOnPageLoad = True
            Exit Sub
        End If

        If IsValidLatitude(Me.CustLat.Value.Trim()) = False Then
            If hgeo_mod.Value.Trim() = "E" Then
                Me.lbl_msg.Text = "Invalid explicit capture  location Value."
            Else
                Me.lbl_msg.Text = "Invalid last visited Latitude Value."
            End If
            '  MapWindow.VisibleOnPageLoad = True
            ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)

            Exit Sub
        End If

        Dim obj As New LatiLongitude

        Dim success As Boolean = False
        Try
            obj.Latitude = Convert.ToDouble(CustLat.Value.Trim())
            obj.Longitude = Convert.ToDouble(CustLng.Value.Trim())
            obj.CustomerId = CustID.Value.Trim()
            obj.SiteUserId = SiteID.Value.Trim()

            If obj.UpdateLatiLongitude(Err_No, Err_Desc) = True Then
                success = True
                Me.lbl_msg.Text = "Successfully updated."
                txtLocLatitude.Text = CustLat.Value.Trim()
                txtLocLong.Text = CustLng.Value.Trim()
                ScriptManager.RegisterStartupScript(Me, GetType(String), "closePopup", "closePopup();", True)
                ScriptManager.RegisterStartupScript(Me, GetType(String), "Intialization", "ShowVisit(" & txtLocLatitude.Text & "," & txtLocLong.Text & "," & CustLat.Value & " ," & CustLng.Value & ");", True)


                'MessageBoxValidation("Successfully updated.", "Information")
                ' MPEDetails.VisibleOnPageLoad = False
                ' Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ' BindCustomerData()
                'Resetfields()

                ' ClassUpdatePnl.Update()
            Else
                success = False
                Me.lbl_msg.Text = "Could not update."
                ' MessageBoxValidation("Could not update.", "Information")
                ' MPEDetails.VisibleOnPageLoad = False
                ' Resetfields()
                'Dt = objLatitude.FillCusShipAddress(Err_No, Err_Desc, ddlOrganization.SelectedItem.Value)
                ' BindCustomerData()
                '  ClassUpdatePnl.Update()
            End If



        Catch ex As Exception
            Err_No = "74062"
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
        End Try
    End Sub
    Protected Sub btnCancelLoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "openPopUp", "javascript:closePopup();", True)

        Catch
        End Try
    End Sub
    'Sub MessageBoxConfirm(ByVal str As String, ByVal Title As String)
    '    RadWindowManager1.RadConfirm(str, "confirmCallbackFn", 300, 200, Nothing, "Confirm")
    '    Exit Sub
    'End Sub
End Class