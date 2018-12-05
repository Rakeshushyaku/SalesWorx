Imports System.Resources

Public Class Utils

    Public Shared Function SqlBless(ByVal QryString As Object) As Object
        If Not (IsNothing(QryString) Or IsDBNull(QryString)) Then
            SqlBless = Replace(CStr(QryString), "'", "''")
        Else
            SqlBless = QryString
        End If
    End Function

    Public Shared Function FixQuotes(ByVal QryVar As Object) As String
        QryVar = Trim(CStr(QryVar))
        If QryVar.Length > 3 Then
            If QryVar.StartsWith("''") And QryVar.EndsWith("''") Then
                QryVar = QryVar.Remove(0, 1)
                QryVar = Left(QryVar, QryVar.Length - 1)
            End If
        End If
        Return QryVar
    End Function

    Public Shared Function PrepSQL(ByVal QryVar As String) As String
        If IsNothing(QryVar) Then
            Return "NULL"
        Else
            If QryVar = "NULL" Then
                Return QryVar
            Else
                Return String.Format("'{0}'", SqlBless(QryVar))
            End If
        End If
    End Function

    Public Shared Function FormatDate(ByVal DateValue As Object) As String
        If IsDate(DateValue) Then
            Dim TempStr As String
            TempStr = String.Format("{0}/{1}/{2}", Format(Month(DateValue), "00"), Format(Day(DateValue), "00"), Year(DateValue))
            Return TempStr
        Else
            Return CStr(DateValue)
        End If
    End Function

    Public Shared Function FormatDate(ByVal DateValue As Object, ByVal DateFormat As String) As String
        If IsDate(DateValue) Then
            Dim TempStr As String
            TempStr = Replace(DateFormat, "DD", "{0}")
            TempStr = Replace(TempStr, "MM", "{1}")
            TempStr = Replace(TempStr, "YYYY", "{2}")
            TempStr = Replace(TempStr, "HH", "{3}")
            TempStr = Replace(TempStr, "MI", "{4}")
            TempStr = Replace(TempStr, "SS", "{5}")
            TempStr = String.Format(TempStr, Format(Day(DateValue), "00"), Format(Month(DateValue), "00"), Year(DateValue), Format(Hour(DateValue), "00"), Format(Minute(DateValue), "00"), Format(Second(DateValue), "00"))
            Return TempStr
        Else
            Return CStr(DateValue)
        End If
    End Function


    Public Shared Function GetArabicDay(ByVal DateValue As Date) As System.DayOfWeek
        If DateValue.DayOfWeek < DayOfWeek.Friday Then
            Return DateValue.DayOfWeek + (7 - DayOfWeek.Friday)
        Else
            Return DateValue.DayOfWeek - DayOfWeek.Friday
        End If
    End Function

    Public Shared Function WrapText(ByVal TextStr As Object, ByVal WrapInterval As Integer) As String
        Dim iInsPos As Integer = 0
        Dim iCounter As Integer
        Dim sTempStr As String

        If Not IsNothing(TextStr) Then

            sTempStr = CStr(TextStr)

            If sTempStr.Length <> WrapInterval Then

                For iCounter = 1 To Math.Floor(sTempStr.Length / WrapInterval)
                    iInsPos = iInsPos + WrapInterval
                    sTempStr = sTempStr.Insert(iInsPos, "<BR>")
                    iInsPos = iInsPos + 4
                Next

            End If

            Return sTempStr
        Else
            Return ""
        End If

    End Function

    Public Shared Function DisplayAsTime(ByVal TimeValueInSecs As Double) As String
        Dim hh As Double, mi As Double, ss As Double
        hh = Math.Floor(TimeValueInSecs / 3600)
        mi = Math.Floor(TimeValueInSecs / 60)
        ss = Math.Round(TimeValueInSecs Mod 60, 0)

        Return String.Format("{0}:{1}:{2}", Format(hh, "00"), Format(mi, "00"), Format(ss, "00"))

    End Function

    Public Shared Function JSBless(ByVal Param As Object) As Object
        If Not IsNothing(Param) Then
            Return Replace(CStr(Param), "'", "’")
        Else
            Return Param
        End If
    End Function

    Public Shared Function JSBless(ByVal Param As String) As String
        If Not IsNothing(Param) Then
            Return Replace(CStr(Param), "'", "’")
        Else
            Return Param
        End If
    End Function

    Public Shared Function PrepSQLDT(ByVal QryVar As String) As String
        If IsNothing(QryVar) Then
            Return "NULL"
        Else
            If QryVar = "NULL" Then
                Return QryVar
            Else
                If IsNumeric(QryVar) Then
                    Return QryVar
                Else
                    Return String.Format("'{0}'", SqlBless(QryVar))
                End If
            End If
        End If
    End Function

    Public Shared Function PrepBrandCode(ByVal BrandCode As String) As String
        If BrandCode.Length < 5 Then
            For i As Integer = 0 To 13 - BrandCode.Length Step 1
                BrandCode = BrandCode & "&nbsp;"
            Next
        End If
        Return BrandCode
    End Function

    Public Shared Function GetUDSubQuery(ByVal UserDesignation As String, ByVal SiteName As String, ByVal OrgId As String, ByVal FSRID As String) As String
        Dim sSubQry As String = "SELECT AB.SalesRep_ID FROM TBL_Org_CTL_DTL As AB"

        Select Case UserDesignation
            Case UD.FSR
                sSubQry = FSRID
                Exit Select
            Case UD.SS
                sSubQry = String.Format("{0} WHERE {1}={2}", sSubQry, "AB.Org_Id", OrgId)
                Exit Select
            Case UD.SM
                sSubQry = String.Format("{0} WHERE {1}='{2}'", sSubQry, "AB.Site", SiteName)
                Exit Select
            Case Else
                If FSRID <> "0" Then
                    sSubQry = FSRID
                End If
        End Select

        Return sSubQry
    End Function

    Public Shared Function GetDeptQuery(Optional ByVal SalesRepID As String = Nothing) As String
        If IsNothing(SalesRepID) Then
            Return "(SELECT DISTINCT AB.Site FROM TBL_Org_CTL_DTL As AB) UNION (SELECT 'General')"
        Else
            Return String.Format("(SELECT AB.Site FROM TBL_Org_CTL_DTL As AB WHERE AB.SalesRep_ID={0}) UNION (SELECT 'General')", SalesRepID)
        End If
    End Function
    Public Shared Function GetDashboardSubQuery(ByVal User_Type_ID As String) As String
        Dim sSubQry As String = "select AB.SalesRep_ID from TBL_User As AB"
        sSubQry = String.Format("{0} WHERE {1}={2}", sSubQry, "AB.User_Type_ID", User_Type_ID)
        Return sSubQry
    End Function
    Public Shared Function FormatErrorMessage(ByVal ErrorMsg As String, ByVal ClassName As String)
        Dim sRetValue As String = ErrorMsg
        Try
            sRetValue = Replace(sRetValue, "$CLASS", ClassName, 1, , CompareMethod.Text)
        Catch ex As Exception

        End Try
        Return sRetValue
    End Function
    Public Shared Function FormatDuplicateMessage(ByVal ErrorMsg As String, Optional ByVal ClassName As String = "", Optional ByVal PropertyName As String = "", Optional ByVal Value As String = "") As String
        Dim sRetValue As String = ErrorMsg
        Try

            sRetValue = Replace(sRetValue, "$CLASS", ClassName, 1, , CompareMethod.Text)
            sRetValue = Replace(sRetValue, "$PROPERTY", PropertyName, 1, , CompareMethod.Text)
            sRetValue = Replace(sRetValue, "$VALUE", Value, 1, , CompareMethod.Text)
        Catch ex As Exception

        End Try
        Return sRetValue
    End Function
    ''' <summary>
    ''' Get Validation Message from the Resource File
    ''' </summary>
    ''' <param name="MessageCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetValidationMessage(ByVal MessageCode As String) As String
        Dim sRetValue As String = ""
        Try
            Dim oRM As ResourceManager = Nothing

            If IsNothing(oRM) Then
                oRM = My.Resources.InfoMessages.ResourceManager
                sRetValue = oRM.GetString(MessageCode)
            End If
        Catch ex As Exception

        End Try
        Return sRetValue
    End Function
End Class
