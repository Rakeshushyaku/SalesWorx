Public Class ManageAuthentication
    Public Shared Sub HasPermission(ByVal objUserAccess As UserAccess, ByVal rightID As String, ByRef _hasPermission As Boolean)

        Dim objCurrentUserRights As New UserAccess
        objCurrentUserRights = objUserAccess

        Try
            _hasPermission = False

            If rightID <> "" Then

                If rightID.StartsWith("M") Then
                    If objCurrentUserRights.MenuID.Contains(rightID) Then
                        _hasPermission = True
                        Exit Sub
                    End If
                ElseIf rightID.Contains("F") Then
                    If objCurrentUserRights.FieldRights.Contains(rightID) Then
                        _hasPermission = True
                        Exit Sub
                    End If

                ElseIf rightID.StartsWith("P") Then
                    If objCurrentUserRights.PageID.Contains(rightID) Then
                        _hasPermission = True
                        Exit Sub
                    End If
                End If

            End If

        Catch ex As Exception

        Finally


        End Try
    End Sub
End Class
