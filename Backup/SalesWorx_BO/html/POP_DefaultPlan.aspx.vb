Imports System.Web.UI.WebControls
Partial Public Class POP_DefaultPlan
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsNothing(Session("USER_ACCESS")) Then
        '    Response.Redirect("Login.aspx")
        'End If

        If (Not IsPostBack) Then
            ' MakeButtonVisibility(True, True, False, False)
            If Request.QueryString("DType") = "X" Or Request.QueryString("DType") = "" Then
                MakeButtonVisibility(True, True, False, False)
                TimeSelection.Enabled = True
                UserComments.Enabled = True
            ElseIf Request.QueryString("DType") = "W" Then
                MakeButtonVisibility(True, False, False, False)
                TimeSelection.Enabled = True
                UserComments.Enabled = True

            ElseIf Request.QueryString("DType") = "M" Then
                MakeButtonVisibility(True, False, True, False)
                TimeSelection.Enabled = True
                UserComments.Enabled = True
            End If
            LoadTimer()
            If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) Then
                Dim HValu As String = Session("HDay" & Request.QueryString("DayRef") & "")
                
                Dim Arr As Array = HValu.Split("|")
                If (Arr.Length > 1) Then
                    UserComments.Text = Arr(1).ToString()
                Else
                    UserComments.Text = ""
                End If

                If (Arr.Length > 2) Then
                    TimePanel.Visible = True
                    TimeSelection.Checked = True
                    Dim SVal As String = Arr(2).ToString()
                    StartHH.SelectedIndex = StartHH.Items.IndexOf(StartHH.Items.FindByText(SVal.Substring(0, 2)))
                    StartMM.SelectedIndex = StartMM.Items.IndexOf(StartMM.Items.FindByText(SVal.Substring(3, 2)))
                End If

                If (Arr.Length > 3) Then
                    Dim EVal As String = Arr(3).ToString()
                    EndHH.SelectedIndex = EndHH.Items.IndexOf(EndHH.Items.FindByText(EVal.Substring(0, 2)))
                    EndMM.SelectedIndex = EndMM.Items.IndexOf(EndMM.Items.FindByText(EVal.Substring(3, 2)))
                End If
            End If
            End If

    End Sub
    Private Sub LoadTimer()
        Dim HH As Int32
        ' Dim MM As Int32
        Dim li As ListItem
        If StartHH.Items.Count = 0 Then
            For HH = 0 To 23
                li = New ListItem
                li.Text = IIf(HH <= 9, "0" + HH.ToString(), HH.ToString())

                li.Value = IIf(HH <= 9, "0" + HH.ToString(), HH.ToString())

                StartHH.Items.Add(li)
                li = Nothing
            Next HH

            StartMM.Items.Add("00")
            StartMM.Items.Add("15")
            StartMM.Items.Add("30")
            StartMM.Items.Add("45")
            StartMM.Items.Add("59")
        End If

        If EndHH.Items.Count = 0 Then
            For HH = 0 To 23
                li = New ListItem
                li.Text = IIf(HH <= 9, "0" + HH.ToString(), HH.ToString())

                li.Value = IIf(HH <= 9, "0" + HH.ToString(), HH.ToString())

                EndHH.Items.Add(li)
                li = Nothing
            Next HH

            EndMM.Items.Add("00")
            EndMM.Items.Add("15")
            EndMM.Items.Add("30")
            EndMM.Items.Add("45")
            EndMM.Items.Add("59")
        End If
        EndHH.SelectedIndex = EndHH.Items.IndexOf(EndHH.Items.FindByText("23"))
        EndMM.SelectedIndex = EndMM.Items.IndexOf(EndMM.Items.FindByText("59"))
    End Sub
    Private Sub MakeButtonVisibility(ByVal daysetbool As Boolean, ByVal dayoffbool As Boolean, ByVal Rswebool As Boolean, ByVal MADWbool As Boolean)
        ResetBtn.Visible = daysetbool
        DayOffBtn.Visible = dayoffbool
        RSWEBtn.Visible = Rswebool
        MAWDBtn.Visible = MADWbool

    End Sub

    Protected Sub DayOffBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DayOffBtn.Click
        If (TimeSelection.Checked And UserComments.Text <> "") Then
            If (CInt(StartHH.SelectedItem.Text) > CInt(EndHH.SelectedItem.Text) _
                             Or (StartHH.SelectedItem.Text & ":" & StartMM.SelectedItem.Text = _
                                       EndHH.SelectedItem.Text & ":" & EndMM.SelectedItem.Text) _
                            Or (StartHH.SelectedItem.Text = EndHH.SelectedItem.Text And CInt(StartMM.SelectedItem.Text) > _
                 CInt(EndMM.SelectedItem.Text))) Then
                ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'> Start Time should be less than End Time</span>"
                Exit Sub
            Else
                CallCloseEvent("O", "")
            End If
        Else
            CallCloseEvent("O", "")
        End If
    End Sub

    Protected Sub ResetBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ResetBtn.Click
        If Request.QueryString("DType") = "W" Or Request.QueryString("DType") = "M" Then
            If (TimeSelection.Checked And UserComments.Text <> "") Then
                If (CInt(StartHH.SelectedItem.Text) > CInt(EndHH.SelectedItem.Text) _
                                 Or (StartHH.SelectedItem.Text & ":" & StartMM.SelectedItem.Text = _
                                           EndHH.SelectedItem.Text & ":" & EndMM.SelectedItem.Text) _
                                Or (StartHH.SelectedItem.Text = EndHH.SelectedItem.Text And CInt(StartMM.SelectedItem.Text) > _
                     CInt(EndMM.SelectedItem.Text))) Then
                    ErrMsg.Text = "<span class='error'>Error:</SPAN><span class='errormsg' style='font-size:.82em'> Start Time should be less than End Time</span>"
                    Exit Sub
                Else
                    CallCloseEvent("X", "")
                End If
            Else
                CallCloseEvent("X", "Reset")
            End If
        Else

            CallCloseEvent("X", "Reset")
        End If
    End Sub

    Protected Sub RSWEBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RSWEBtn.Click
        CallCloseEvent("W", "Reset")
    End Sub

    Protected Sub MAWDBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MAWDBtn.Click
        CallCloseEvent("M", "")
    End Sub
    Protected Sub CallCloseEvent(ByVal DayValue As String, ByVal ButtonType As String)
        Dim strScript As String
        Dim Time As String = ""
        Dim Ucomments As String = ""
        strScript = ""
        strScript = "<script  language='javascript' type='text/javascript'>"

        If (Request.QueryString("Mode") = "MODIFY") Then
            strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_Action_Mode.value ='MODIFY';"
        Else
            strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_Action_Mode.value ='ADD';"
        End If
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_CheckRefresh.value ='Y';"
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_FromPopUp.value ='Y';"
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_ISRefreskClick.value ='N';"
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_Cell" & Request.QueryString("DayRef") & ".value ='Y';"
        If (Not ButtonType = "Reset") Then
            If (UserComments.Text <> "") Then
                Ucomments = "|" & UserComments.Text.Trim()
            Else
                Ucomments = ""
            End If
            If (TimeSelection.Checked And UserComments.Text <> "") Then

                If (StartHH.SelectedItem.Text <> "00") Then
                    Time = "|" & StartHH.SelectedItem.Text & ":" & StartMM.SelectedItem.Text
                    'End If

                    'If (EndHH.SelectedItem.Text <> "23") Then
                    Time += "|" & EndHH.SelectedItem.Text & ":" & EndMM.SelectedItem.Text
                End If
            End If
        End If
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_HDay" & Request.QueryString("DayRef") & ".value='" & DayValue & Ucomments & Time & "';"
        strScript += "window.opener.document.aspnetForm.ctl00_ContentPlaceHolder1_DayRef.value='" & Request.QueryString("DayRef") & "';"

        '  strScript += "self.opener.window.document.aspnetForm('ctl00_ContentPlaceHolder1_Button1').click();"
        strScript += "self.opener.document.aspnetForm['ctl00_ContentPlaceHolder1_Button1'].click();"
        If (Request.QueryString("Mode") = "MODIFY") Then
            strScript += "window.opener.document.aspnetForm.submit();"
        End If
        strScript += "window.close();"

        strScript += "</script>"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
     strScript, False)
        '       strScript += "this.close();"

        '       Page.ClientScript.RegisterStartupScript(Me.GetType(), "StrScript", _
        'strScript, True)
    End Sub

    Private Sub TimeSelection_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimeSelection.CheckedChanged
        If (TimeSelection.Checked) Then
            TimePanel.Visible = True
        Else
            TimePanel.Visible = False
        End If

    End Sub

End Class