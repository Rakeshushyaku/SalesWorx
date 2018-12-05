Imports System.Data.SqlClient
Imports System.Configuration

Public Class DAL_POSM
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Public Function GetQuestionGroups(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_App_Codes where Code_Type='POSM_QUES_GRP'", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)
            
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetQuestionGroups = dtsurvey

    End Function

    Public Function GetSurveyQuestionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_Survey_POSM where Question_ID=" & ID, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyQuestionDetails = dtsurvey

    End Function
    Public Function GetQuestionCodes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code_Type As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("select * from TBL_App_Codes where Code_Type='" & Code_Type & "' order by Code_Description", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetQuestionCodes = dtsurvey

    End Function
    Public Function GetSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Group As String = "") As DataTable

        Dim objSQLConn As SqlConnection
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            Dim objSQLCmd As SqlCommand
            objSQLCmd = New SqlCommand("app_GetPOSMSurvey", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@QGRP", Group)
         

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurvey = dtsurvey

    End Function
    Public Function GetQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Group As String = "", Optional ByVal Question As String = "", Optional ByVal Filter As String = "") As DataTable

        Dim objSQLConn As SqlConnection
        Dim dtsurvey As New DataTable
        Try

            objSQLConn = _objDB.GetSQLConnection
            Dim objSQLCmd As SqlCommand
            objSQLCmd = New SqlCommand("app_GetPOSMQuestions", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@QGRP", Group)
            objSQLCmd.Parameters.AddWithValue("@QUES", Question)
            objSQLCmd.Parameters.AddWithValue("@Filter", Filter)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dtsurvey)

        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetQuestions = dtsurvey

    End Function
    Public Function ManagePOSMAppcodes(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Opt As String, ByVal Code_Type As String, ByVal Code_Value As String, ByVal Code_Description As String, ByVal Addedby As Integer, ByRef msg As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            Dim dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_InsertPOSMAppcode"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Code_Type", SqlDbType.VarChar).Value = Code_Type
            objSQLCmd.Parameters.Add("@Code_Value", SqlDbType.VarChar).Value = Code_Value
            objSQLCmd.Parameters.Add("@Code_Description", SqlDbType.VarChar).Value = Code_Description
            objSQLCmd.Parameters.Add("@Addedby", SqlDbType.BigInt).Value = Addedby
            objSQLCmd.Parameters.Add("@Opt", SqlDbType.BigInt).Value = Opt
            dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dr.Read Then
                If dr(0) = "1" Then
                    retVal = True
                Else
                    msg = "The code already exists"
                End If
            End If
            objSQLCmd.Dispose()


        Catch ex As Exception
            msg = ex.Message
            Error_No = 79014
            Error_Desc = String.Format("Error while assigning customers: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function ManagePOSMSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Opt As String, ByVal Question_Group_1 As String, ByVal Question_1 As String, ByVal Question_2 As String, ByVal Addedby As Integer, ByVal Response_Type As String, ByVal QID As String, ByRef msg As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            Dim dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_ManagePOSMSurvey"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@Question_Group_1", SqlDbType.VarChar).Value = Question_Group_1
            objSQLCmd.Parameters.Add("@Question_1", SqlDbType.VarChar).Value = Question_1
            objSQLCmd.Parameters.Add("@Question_2", SqlDbType.VarChar).Value = IIf(Question_2.Trim = "0", DBNull.Value, Question_2)
            objSQLCmd.Parameters.Add("@Response_Type", SqlDbType.VarChar).Value = Response_Type
            objSQLCmd.Parameters.Add("@UserID", SqlDbType.BigInt).Value = Addedby
            objSQLCmd.Parameters.Add("@Opt", SqlDbType.BigInt).Value = Opt
            objSQLCmd.Parameters.Add("@QID", SqlDbType.BigInt).Value = QID

            dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dr.Read Then
                If dr(1) = "1" Then
                    retVal = True
                Else
                    msg = dr(0)
                End If
            End If
            objSQLCmd.Dispose()


        Catch ex As Exception
            msg = ex.Message
            Error_No = 79014
            Error_Desc = String.Format("Error while assigning customers: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
End Class
