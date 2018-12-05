Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class Process
    Private Shared ProcessLog As List(Of String) = Nothing
    Private Shared NoOfCodesCount As String
    Private Shared UnAssignedCodeCount As String
    Private Shared CodeLength As String
    Private Shared GeneratedCodesCount As Integer

    Public Function GenerateApprovalCodes() As List(Of String)
        NoOfCodesCount = Nothing
        UnAssignedCodeCount = Nothing
        CodeLength = Nothing
        GeneratedCodesCount = 0
        Dim ActUnAssignedCodesCount As Integer = 0
        ProcessLog = New List(Of String)
        Try

            ProcessLog.Add("Approval Codes Generation Process Started.")

            ProcessLog.Add("Getting No Of Unassigned Codes and Other Info from DB")

            GetUnAssignedCodeCount()

            If String.IsNullOrEmpty(NoOfCodesCount) Or String.IsNullOrEmpty(CodeLength) Then
                ProcessLog.Add("Length/No. of Approval Code Configuration not available")
                Exit Try
            End If

            ActUnAssignedCodesCount = IIf(String.IsNullOrEmpty(UnAssignedCodeCount), 0, CInt(UnAssignedCodeCount))

            ProcessLog.Add(String.Format("Available Count Of Unassigned Approval Code is {0}", ActUnAssignedCodesCount))

            GenerateCodes(NoOfCodesCount - ActUnAssignedCodesCount)

            ProcessLog.Add(String.Format("Process Generated Approval Code Count is {0}", GeneratedCodesCount))

        Catch ex As Exception
            ProcessLog.Add(String.Format("Error in Generate Approval Codes and Error Details : {0}", ex.Message))
        End Try
        GenerateApprovalCodes = ProcessLog
    End Function

    Private Sub GetUnAssignedCodeCount()
        Dim _objDB As DB_Handler = Nothing
        Dim objSQLConn As SqlConnection
        Dim objSQLDt As New DataTable
        Dim objSQLCmd As SqlCommand = Nothing
        Dim objSQLDa As New SqlDataAdapter
        Try
            _objDB = New DB_Handler()
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand()
            objSQLCmd.Connection = objSQLConn
            objSQLCmd.CommandText = "Usp_GetApprovalCodeInfo"
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(objSQLDt)
            If objSQLDt IsNot Nothing AndAlso objSQLDt.Rows.Count > 0 Then
                NoOfCodesCount = IIf(objSQLDt.Rows(0).Item("NoOfApprovalCodes") Is DBNull.Value, Nothing, objSQLDt.Rows(0).Item("NoOfApprovalCodes"))
                UnAssignedCodeCount = IIf(objSQLDt.Rows(0).Item("UnAssignedCount") Is DBNull.Value, Nothing, objSQLDt.Rows(0).Item("UnAssignedCount"))
                CodeLength = IIf(objSQLDt.Rows(0).Item("CodeLength") Is DBNull.Value, Nothing, objSQLDt.Rows(0).Item("CodeLength"))
            End If
        Catch ex As Exception
            ProcessLog.Add(String.Format("Error in Getting Unassigned Code Count and Error Details : {0}", ex.Message))
        Finally
            If Not IsNothing(objSQLConn) Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
    End Sub

    Private Sub GenerateCodes(ByVal CodeCount As Integer)
        Dim LstCode As New List(Of String)
        Try
            ProcessLog.Add(String.Format("No of Approval Codes going to be generated : {0}", CodeCount))

            If CodeCount > 0 Then
                Dim StrCount As Integer = 0
                Dim StrCodes As String = Nothing
                For i As Integer = 0 To CodeCount - 1
                    ' Dim g As Guid = Guid.NewGuid()
                    Dim GuidString As String = BuildApprovalCode(CodeLength)
                    ' GuidString = GuidString.Replace("=", "")
                    ' GuidString = GuidString.Replace("+", "")
                    ' GuidString = GuidString.Replace("/", "")

                    If GuidString.Length > CodeLength Then
                        GuidString = GuidString.Substring(0, CodeLength)
                    End If

                    If StrCodes Is Nothing Then
                        StrCodes = GuidString
                    Else
                        StrCodes += "," & GuidString
                    End If

                    StrCount += 1

                    If StrCount = 1000 Or i = CodeCount - 1 Then '' Splitting the String into List
                        LstCode.Add(StrCodes)
                        StrCodes = Nothing
                        StrCount = 0
                    End If
                Next

                If LstCode.Count > 0 Then
                    ProcessLog.Add(String.Format("{0} Approval Codes Generated Successfully", CodeCount))

                    '' Insert Into DB 
                    ProcessLog.Add(String.Format("{0} Approval Codes Insertion Into DB Started", CodeCount))
                    InsertCodesintoDB(LstCode)
                Else
                    ProcessLog.Add("Approval Code List Count is 0")
                End If
              

            End If
        Catch ex As Exception
            ProcessLog.Add(String.Format("Error In Code Generation and Error Details : {0}", ex.Message))
        End Try
    End Sub
    Private Function BuildApprovalCode(ByVal maxSize As Integer) As String
        Dim chars As Char() = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray()
        Dim data As Byte() = New Byte(0) {}
        Dim crypto As New RNGCryptoServiceProvider()
        crypto.GetNonZeroBytes(data)
        data = New Byte(maxSize - 1) {}
        crypto.GetNonZeroBytes(data)
        Dim result As New StringBuilder(maxSize)
        For Each b As Byte In data
            result.Append(chars(b Mod (chars.Length - 1)))
        Next
        Return result.ToString()
    End Function

    Private Sub InsertCodesintoDB(ByVal CodesList As List(Of String))
        Dim DuplicateCount As Integer = 0
        Try
            For i As Integer = 0 To CodesList.Count - 1
                Dim _objDB As DB_Handler = Nothing
                Dim objSQLConn As SqlConnection
                Dim objSQLDt As New DataTable
                Dim objSQLCmd As SqlCommand = Nothing
                Dim objSQLDa As New SqlDataAdapter
                Try
                    _objDB = New DB_Handler()
                    objSQLConn = _objDB.GetSQLConnection
                    objSQLCmd = New SqlCommand()
                    objSQLCmd.Connection = objSQLConn
                    objSQLCmd.CommandText = "Usp_InsertApprovalCodes"
                    objSQLCmd.CommandType = CommandType.StoredProcedure
                    objSQLCmd.Parameters.Add("@ApprovalCodes", SqlDbType.VarChar).Value = CodesList(i).ToUpper()
                    objSQLDa = New SqlDataAdapter(objSQLCmd)
                    objSQLDa.Fill(objSQLDt)
                    If objSQLDt IsNot Nothing AndAlso objSQLDt.Rows.Count > 0 Then
                        DuplicateCount += CInt(objSQLDt.Rows(0)(0))
                        GeneratedCodesCount += CInt(objSQLDt.Rows(0)(1))
                    End If
                Catch ex As Exception
                    ProcessLog.Add(String.Format("Error in InsertIntoDB Execution and Error Details : {0}", ex.Message))
                Finally
                    If Not IsNothing(objSQLConn) Then
                        _objDB.CloseSQLConnection(objSQLConn)
                    End If
                    objSQLDa = Nothing
                End Try
            Next

            '' If there are Approval Codes already exists, Generate Again
            If DuplicateCount > 0 Then
                ProcessLog.Add(String.Format("Duplicate Count is {0}", DuplicateCount))
                GenerateCodes(DuplicateCount)
            End If

        Catch ex As Exception
            ProcessLog.Add(String.Format("Error In Insert Code Into DB and Error Details : {0}", ex.Message))
        End Try
    End Sub

End Class
