Imports System.Data.SqlClient
Imports System.Configuration
Imports log4net
Public Class DAL_AdminSurvey
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    Private dtsurvey As New DataTable
    Private dtSalRep As New DataTable
    Public dtSalesRepSelected As New DataTable
    Private dtSurveyFSR As New DataTable
    Private dtQuestions As New DataTable
    Private dtResponses As New DataTable
    Private dtResponseType As New DataTable
    Private dtFSRAvailCustomers As New DataTable
    Private dtFSRAssignCustomers As New DataTable
    Private mExpiryDate As Integer = CInt(ConfigurationSettings.AppSettings("MESSAGE_EXPIRY_DAYS"))

    Public Function GetConnection() As SqlConnection
        Dim objSQLConn As SqlConnection
        objSQLConn = _objDB.GetSQLConnection
        Return objSQLConn
    End Function
    Public Sub CloseConnection(ByRef ObjCloseConn As SqlConnection)
        ' Dim objSQLConn As SqlConnection
        If ObjCloseConn IsNot Nothing Then
            _objDB.CloseSQLConnection(ObjCloseConn)
        End If
    End Sub
    Public Function GetSurveyData(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * from TBL_Survey  order by Survey_Title ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "750166"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyData = dtsurvey
    End Function

    Public Function SearchSurveyGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            If FilterBy = "Type" Then
                Query = "SELECT * from TBL_Survey WHERE Survey_Type_Code='" + FilterValue.ToUpper() + "' ORDER BY Survey_Title"
            ElseIf FilterBy = "Survey" Then
                Query = "SELECT * FROM TBL_Survey WHERE Survey_Title LIKE '%" + FilterValue + "%' ORDER BY Survey_Title"
            Else
                Query = "SELECT * FROM TBL_Survey ORDER BY Survey_Title"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtsurvey)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtsurvey
    End Function

    Public Function GetSurveys(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Survey_ID, Survey_Title from TBL_Survey  order by Survey_Title ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)
            Dim dr As DataRow = dtsurvey.NewRow
            dr("Survey_Title") = "--Select--"
            dtsurvey.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveys = dtsurvey
    End Function

    Public Function GetMarketSurveys(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Survey_ID, Survey_Title from TBL_Survey WHERE Survey_Type_Code='M'  order by Survey_Title ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtsurvey)
            Dim dr As DataRow = dtsurvey.NewRow
            dr("Survey_Title") = "--Select--"
            dtsurvey.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetMarketSurveys = dtsurvey
    End Function
    Public Function GetSalesRep(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR order by SalesRep_Name ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtSalRep)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSalesRep = dtSalRep
    End Function
    Public Function DeleteVanAuditPlan(AuditorID As String, AuditYear As String, AuditMonth As String, SalesRep_ID As String, UserId As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_DeleteVanAuditPlan"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@AuditorID", SqlDbType.Int).Value = Val(AuditorID)
            objSQLCmd.Parameters.Add("@Audit_Year", SqlDbType.Int).Value = Val(AuditYear)
            objSQLCmd.Parameters.Add("@Audit_Month", SqlDbType.Int).Value = Val(AuditMonth)
            objSQLCmd.Parameters.Add("@SalesRep_ID", SqlDbType.Int).Value = Val(SalesRep_ID)
            objSQLCmd.Parameters.Add("@userID", SqlDbType.Int).Value = Val(SalesRep_ID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

        Catch ex As Exception

            Err_No = 79001
            Err_Desc = String.Format("Error while saving Van Audit Plan: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function InsertVanAuditPlan(AuditorID As String, AuditYear As String, AuditMonth As String, SalesRep_ID As String, UserId As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String

        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "app_SaveVanAuditPlan"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@AuditorID", SqlDbType.Int).Value = Val(AuditorID)
            objSQLCmd.Parameters.Add("@Audit_Year", SqlDbType.Int).Value = Val(AuditYear)
            objSQLCmd.Parameters.Add("@Audit_Month", SqlDbType.Int).Value = Val(AuditMonth)
            objSQLCmd.Parameters.Add("@SalesRep_ID", SqlDbType.Int).Value = Val(SalesRep_ID)
            objSQLCmd.Parameters.Add("@userID", SqlDbType.Int).Value = Val(SalesRep_ID)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

        Catch ex As Exception

            Err_No = 79001
            Err_Desc = String.Format("Error while saving Van Audit Plan: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function SaveSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Title As String, ByVal TypeCode As String, ByVal SDate As DateTime, ByVal ExpDate As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim tran As SqlTransaction = Nothing
        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction("RowIns")
            sQry = "insert into TBL_Survey(Survey_Title,Survey_type_Code, Start_time, End_Time) values(@Title,@TypeCode,@SDate,@EDate)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = Title
            objSQLCmd.Parameters.Add("@TypeCode", SqlDbType.Char, 1).Value = TypeCode
            objSQLCmd.Parameters.Add("@SDate", SqlDbType.DateTime).Value = SDate
            objSQLCmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = ExpDate
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = New SqlCommand("select @@IDENTITY as Survey_ID", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Transaction = tran
            Dim SurveyID As Integer = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()
            Dim bulk As SqlBulkCopy = New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.TableLock, tran)

            If dtSalesRepSelected.Rows.Count > 0 Then
                bulk.DestinationTableName = "TBL_Survey_FSR"
                For Each dr As DataRow In dtSalesRepSelected.Rows
                    dr(0) = SurveyID
                Next
                bulk.WriteToServer(dtSalesRepSelected)
            End If

            tran.Commit()
            retVal = True

        Catch ex As Exception
            tran.Rollback("RowIns")
            Error_No = 79001
            Error_Desc = String.Format("Error while saving survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            tran.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function




    Public Function SearchSurveys(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyId As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * from TBL_Survey WHERE Survey_id=@SID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@SID", SqlDbType.BigInt).Value = SurveyId
            objSQLDA.Fill(dtsurvey)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        SearchSurveys = dtsurvey
    End Function


    Public Function UpdateSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SurvId As Integer, ByVal Title As String, ByVal TypeCode As String, ByVal SDate As DateTime, ByVal ExpDate As DateTime) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Dim tran As SqlTransaction = Nothing

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction("RowUpd")
            sQry = "DELETE FROM TBL_Survey_FSR WHERE Survey_ID=@SurveyID; UPDATE TBL_Survey SET Survey_Title=@Title,Survey_type_Code=@TypeCode, Start_time=@SDate, End_Time=@EDate WHERE Survey_Id=@SurveyID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.Add("@SurveyID", SqlDbType.BigInt).Value = SurvId
            objSQLCmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = Title
            objSQLCmd.Parameters.Add("@TypeCode", SqlDbType.Char, 1).Value = TypeCode
            objSQLCmd.Parameters.Add("@SDate", SqlDbType.DateTime).Value = SDate
            objSQLCmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = ExpDate
            objSQLCmd.ExecuteNonQuery()
            Dim bulk As SqlBulkCopy = New SqlBulkCopy(objSQLConn, SqlBulkCopyOptions.TableLock, tran)

            If dtSalesRepSelected.Rows.Count > 0 Then
                bulk.DestinationTableName = "TBL_Survey_FSR"
                bulk.WriteToServer(dtSalesRepSelected)
            End If
            objSQLCmd.Dispose()
            tran.Commit()
            retVal = True
        Catch ex As Exception
            tran.Rollback("RowUpd")
            Error_No = 79001
            Error_Desc = String.Format("Error while Updating survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            tran.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function DeleteSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SurvId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "DELETE FROM TBL_Survey WHERE Survey_Id=@SurveyID;DELETE FROM TBL_Survey_FSR WHERE Survey_ID=@SurveyID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SurveyID", SqlDbType.BigInt).Value = SurvId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True

        Catch ex As Exception
            Error_No = 79001
            Error_Desc = String.Format("Error while Delete survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function GetSurveysFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyId As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Survey_Id,SalesRep_ID from TBL_Survey_FSR WHERE Survey_id=@SID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@SID", SqlDbType.BigInt).Value = SurveyId
            objSQLDA.Fill(dtSurveyFSR)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75019"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveysFSR = dtSurveyFSR
    End Function



    Public Function SaveQuestions(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Question As String, ByVal SurveyId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_Survey_Questions(Question_Text,Survey_Id) VALUES(@Question,@SurveyId)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@Question", SqlDbType.VarChar, 255).Value = Question
            objSQLCmd.Parameters.Add("@SurveyId", SqlDbType.BigInt).Value = SurveyId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True
        Catch ex As Exception

            Error_No = 79001
            Error_Desc = String.Format("Error while Inserting survey Questions: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function



    Public Function SearchQuestion(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer, ByVal surveyId As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * from TBL_Survey_Questions WHERE Question_id=@QID AND Survey_Id=@SID ORDER BY Question_id", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@QID", SqlDbType.BigInt).Value = QuestId
            objSQLDA.SelectCommand.Parameters.Add("@SId", SqlDbType.BigInt).Value = surveyId
            If dtQuestions.Rows.Count > 0 Then
                dtQuestions.Rows.Clear()
            End If
            objSQLDA.Fill(dtQuestions)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75046"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        SearchQuestion = dtQuestions
    End Function


    Public Function UpdateQuestions(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal QuestId As Integer, ByVal Question As String, ByVal SurveyId As Integer, ByVal ResponseId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "UPDATE TBL_Survey_Questions SET Question_Text=@Question,Survey_Id=@SurveyId, Default_Response_id=@ResponseID WHERE Question_Id=@QuestionID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            objSQLCmd.Parameters.Add("@QuestionID", SqlDbType.BigInt).Value = QuestId
            objSQLCmd.Parameters.Add("@Question", SqlDbType.VarChar, 255).Value = Question
            objSQLCmd.Parameters.Add("@SurveyId", SqlDbType.BigInt).Value = SurveyId
            objSQLCmd.Parameters.Add("@ResponseId", SqlDbType.BigInt).Value = ResponseId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True
        Catch ex As Exception

            Error_No = 79001
            Error_Desc = String.Format("Error while Updating survey Questions: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function DeleteQuestion(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal QuestId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "DELETE FROM TBL_Survey_Questions WHERE Question_Id=@QuestID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@QuestID", SqlDbType.BigInt).Value = QuestId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True

        Catch ex As Exception
            Error_No = 79001
            Error_Desc = String.Format("Error while Delete survey question: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function GetQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal surveyid As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Question_ID, Question_Text from TBL_Survey_Questions WHERE survey_Id=@SID   order by Question_Id ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@SID", SqlDbType.BigInt).Value = surveyid
            If dtQuestions.Rows.Count > 0 Then
                dtQuestions.Rows.Clear()
            End If
            objSQLDA.Fill(dtQuestions)
            Dim dr As DataRow = dtQuestions.NewRow
            dr("Question_Text") = "--Select a Question--"
            dtQuestions.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "73016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetQuestions = dtQuestions
    End Function

    Public Function GetResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Questionid As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Response_ID, Response_Text from TBL_Survey_Responses WHERE Question_Id=@QID   order by Response_Id ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@QID", SqlDbType.BigInt).Value = Questionid
            If dtResponses.Rows.Count > 0 Then
                dtResponses.Rows.Clear()
            End If
            objSQLDA.Fill(dtResponses)
            Dim dr As DataRow = dtResponses.NewRow
            dr("Response_Text") = "--Select a Response--"
            dtResponses.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "73017"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetResponses = dtResponses
    End Function


    Public Function GetResponsesType(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim objSQLCMD As SqlCommand
        Dim query As String = ""
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLCMD = New SqlCommand("SELECT Survey_Type_Code FROm TBL_Survey WHERE Survey_ID=@SurveyID", objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.AddWithValue("@SurveyID", SurveyID)

            Dim Surveytype As String = Nothing

            Surveytype = objSQLCMD.ExecuteScalar()

            objSQLCMD.Dispose()

            If Surveytype = "M" Then

                query = "SELECT Response_Type_ID, Response_Type from TBL_Survey_Response_Types   order by Response_Type_ID ASC"
            Else
                query = "SELECT Response_Type_ID, Response_Type from TBL_Survey_Response_Types WHERE Response_Type_ID<>4  order by Response_Type_ID ASC"
            End If
            objSQLDA = New SqlDataAdapter(query, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtResponseType)
            Dim dr As DataRow = dtResponseType.NewRow
            dr("Response_Type") = "--Select Response Type--"
            dtResponseType.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "73017"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetResponsesType = dtResponseType
    End Function


    Public Function SaveResponses(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ResponseText As String, ByVal QuestionId As Integer, ByVal ResponseTypeId As Integer, ByVal defaultRespId As Boolean) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "INSERT INTO TBL_Survey_Responses(Response_Text,Question_Id,Response_Type_Id) VALUES(@ResponseText,@QuestionId,@RespTypeId)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@ResponseText", SqlDbType.VarChar, 255).Value = IIf(ResponseTypeId = 4, DBNull.Value, ResponseText)
            objSQLCmd.Parameters.Add("@QuestionId", SqlDbType.BigInt).Value = QuestionId
            objSQLCmd.Parameters.Add("@RespTypeId", SqlDbType.BigInt).Value = ResponseTypeId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = New SqlCommand("select @@IDENTITY as Response_ID", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim RespID As Integer = Convert.ToInt32(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()
            If defaultRespId = True Then
                objSQLCmd = New SqlCommand("UPDATE TBL_Survey_Questions SET Default_Response_Id=@RespID WHERE Question_ID=@QuestID", objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Parameters.Add("@QuestId", SqlDbType.BigInt).Value = QuestionId
                objSQLCmd.Parameters.Add("@RespId", SqlDbType.BigInt).Value = RespID
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            End If
            retVal = True
        Catch ex As Exception

            Error_No = 79001
            Error_Desc = String.Format("Error while Inserting survey Responses: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing

            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function SearchResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT a.Response_Text,a.Question_Id,a.Response_Type_ID,(SELECT COUNT(*) FROM  TBL_Survey_Responses WHERE Question_id=a.Question_Id)AS OptCnt,(SELECT ISNULL(Default_Response_id,0) FROM  TBL_Survey_Questions WHERE Question_id=a.Question_Id AND Default_Response_Id=a.Response_Id)AS DefValue  from TBL_Survey_Responses a WHERE a.Question_id=@QID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@QID", SqlDbType.BigInt).Value = QuestId
            If dtResponses.Rows.Count > 0 Then
                dtResponses.Rows.Clear()
            End If
            objSQLDA.Fill(dtResponses)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75076"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        SearchResponses = dtResponses
    End Function

    Public Function DeleteResponses(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal QuestId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "DELETE FROM TBL_Survey_Responses WHERE Question_Id=@QuestID"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@QuestID", SqlDbType.BigInt).Value = QuestId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            objSQLCmd = New SqlCommand("UPDATE TBL_Survey_Questions SET Default_Response_Id='0' WHERE Question_ID=@QID", objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@QID", SqlDbType.BigInt).Value = QuestId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True

        Catch ex As Exception
            Error_No = 79001
            Error_Desc = String.Format("Error while Delete survey Responses: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function



    Public Function GetAvailableCustomers(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SaleRepId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'sQry = " SELECT Customer_Name,CAST(Customer_ID AS VARCHAR)+'$'+CAST(Site_Use_ID AS VARCHAR) As CustSiteID, ISNULL(Customer_No, 'N/A')+'-'+ ISNULL(Customer_Name, 'N/A')+' - '+ISNULL(Location,'N/A') AS CustName    FROM  V_FSR_CustomerShipAddress  WHERE SalesRep_Id=@SaleRepId AND  Customer_Id NOT IN (SELECT Customer_ID FROM TBL_Survey_FSR_Customers) ORDER BY Customer_Name "
            'sQry = " SELECT Customer_Name,CAST(Customer_ID AS VARCHAR)+'$'+CAST(Site_Use_ID AS VARCHAR) As CustSiteID, ISNULL(Customer_No, 'N/A')+'-'+ ISNULL(Customer_Name, 'N/A')+' - '+ISNULL(Location,'N/A') AS CustName    FROM  V_FSR_CustomerShipAddress  WHERE SalesRep_Id=@SaleRepId AND  Customer_Id NOT IN (SELECT Customer_ID FROM TBL_Survey_FSR_Customers WHERE SalesRep_Id=@SaleRepId) ORDER BY Customer_Name "
            sQry = " SELECT Customer_Name,CAST(Customer_ID AS VARCHAR)+'$'+CAST(Site_Use_ID AS VARCHAR) As CustSiteID, ISNULL(Customer_No, 'N/A')+'-'+ ISNULL(Customer_Name, 'N/A')+' - '+ISNULL(Location,'N/A') AS CustName ,Customer_No   FROM  V_FSR_CustomerShipAddress  WHERE SalesRep_Id=@SaleRepId AND  Customer_Id NOT IN (SELECT Customer_ID FROM TBL_Survey_FSR_Customers WHERE SalesRep_Id=@SaleRepId) ORDER BY Customer_Name "
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@SaleRepID", SqlDbType.BigInt).Value = SaleRepId
            objSQLDA.Fill(dtFSRAvailCustomers)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtFSRAvailCustomers
    End Function


    Public Function GetAssignedCustomers(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SaleRepId As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim sRetVal As String = ""
        Try

            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            'sQry = "SELECT DISTINCT (CAST(A.Customer_ID AS VARCHAR)+'$'+CAST(A.Site_Use_ID AS VARCHAR)) As CustSiteID,Customer_Name,  ISNULL(Customer_No, 'N/A')+'-'+ ISNULL(A.Customer_Name, 'N/A')+' - '+ISNULL(A.Location,'N/A') AS CustName FROM TBL_Customer_Ship_Address As A INNER JOIN TBL_Survey_FSR_Customers As B ON A.Customer_Id=B.Customer_Id AND A.Site_Use_ID=B.Site_Use_ID WHERE B.SalesRep_ID=@SaleRepId  ORDER BY Customer_Name"
            sQry = "SELECT DISTINCT (CAST(A.Customer_ID AS VARCHAR)+'$'+CAST(A.Site_Use_ID AS VARCHAR)) As CustSiteID,Customer_Name,  ISNULL(Customer_No, 'N/A')+'-'+ ISNULL(A.Customer_Name, 'N/A')+' - '+ISNULL(A.Location,'N/A') AS CustName,Customer_No FROM TBL_Customer_Ship_Address As A INNER JOIN TBL_Survey_FSR_Customers As B ON A.Customer_Id=B.Customer_Id AND A.Site_Use_ID=B.Site_Use_ID WHERE B.SalesRep_ID=@SaleRepId  ORDER BY Customer_Name"
            objSQLDA = New SqlDataAdapter(sQry, objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.Add("@SaleRepID", SqlDbType.BigInt).Value = SaleRepId
            objSQLDA.Fill(dtFSRAssignCustomers)
            objSQLDA.Dispose()
        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtFSRAssignCustomers
    End Function


    Public Function InsertCustomersToFSR(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SalRepId As Integer, ByVal CustId As Integer, ByVal SiteUsedId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "INSERT INTO TBL_Survey_FSR_Customers(SalesRep_ID, Customer_Id, Site_Use_Id) VALUES(@SaleRepId,@CustID,@SIteId)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SaleRepID", SqlDbType.BigInt).Value = SalRepId
            objSQLCmd.Parameters.Add("@CustID", SqlDbType.BigInt).Value = CustId
            objSQLCmd.Parameters.Add("@SiteID", SqlDbType.BigInt).Value = SiteUsedId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            retVal = True

        Catch ex As Exception
            Error_No = 79014
            Error_Desc = String.Format("Error while assigning customers: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function

    Public Function RemoveCustomersFromFSR(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SalRepId As Integer, ByVal CustId As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "DELETE FROM TBL_Survey_FSR_Customers WHERE SalesRep_ID=@SaleRepId AND Customer_ID=@CustomerId"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SaleRepID", SqlDbType.BigInt).Value = SalRepId
            objSQLCmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = CustId
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True
        Catch ex As Exception
            Error_No = 79014
            Error_Desc = String.Format("Error while Removing customers: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function


    Public Function CheckSurveyDate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyId As Integer) As DateTime

        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim StartDate As DateTime
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT Start_time FROM TBL_Survey WHERE Survey_Id=@SID", objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@SID", SqlDbType.BigInt).Value = SurveyId
            StartDate = Convert.ToDateTime(objSQLCMD.ExecuteScalar())
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return StartDate
    End Function


    Public Function CheckSurveyTitle(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyTitle As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim RetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Survey WHERE UPPER(Survey_Title)=@STitle", objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@STitle", SqlDbType.VarChar, 50).Value = SurveyTitle.ToUpper()
            Dim Cnt As Integer = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            If Cnt > 0 Then
                RetVal = True
            End If
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return RetVal
    End Function
    Public Function GetAllSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("SELECT Survey_ID,Survey_Title,Start_Time,End_Time,Survey_Type_Code from TBL_Survey  WHERE 1=1 AND Survey_Type_Code IN('N,A')  {0} order by Survey_Title ASC", QueryStr)
            Dim QueryString As String = String.Format("SELECT Survey_ID,Survey_Title,Start_Time,End_Time,Survey_Type_Code from TBL_Survey  WHERE 1=1  {0} order by Survey_Title ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblSurvey")

            GetAllSurvey = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetCustomerSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT (LTRIM(STR(A.Customer_ID)) + '$' + LTRIM(STR(A.Site_Use_ID))) as ID, (A.Customer_Name+'-'+ISNULL(A.Location,'N/A')) As Name from TBL_Customer_Ship_Address A,TBL_Survey_Cust_Responses B where A.Customer_Id=B.Customer_Id AND A.Site_Use_ID=B.Site_Use_ID {0} order by Name ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblCustSurvey")
            GetCustomerSurvey = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
   
    Public Function GetAssignedFsrForVanAudit(OrgID As String, Auditor_ID As String, AuditYear As String, AuditMonth As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.SalesRep_ID as ID,A.SalesRep_Name as Name from TBL_FSR A inner join TBL_Org_CTL_DTL B on A.SalesRep_ID=B.SalesRep_ID where A.SalesRep_ID  in(select SalesREp_ID from  TBL_Van_Audit_Plan A inner join TBL_Van_Audit_Plan_Details B on A.Van_Audit_Plan_ID=B.Van_Audit_Plan_ID where a.Audit_year=@Audit_year and A.Audit_Month=@Audit_Month and Auditor_User_ID=@Auditor_ID) and B.MAS_Org_ID=@ORG_ID order by A.SalesRep_Name")

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@Audit_year", SqlDbType.Int)
            objSQLCmd.Parameters("@Audit_year").Value = AuditYear
            objSQLCmd.Parameters.Add("@Audit_Month", SqlDbType.Int)
            objSQLCmd.Parameters("@Audit_Month").Value = AuditMonth
            objSQLCmd.Parameters.Add("@ORG_ID", SqlDbType.VarChar)
            objSQLCmd.Parameters("@ORG_ID").Value = OrgID
            objSQLCmd.Parameters.Add("@Auditor_ID", SqlDbType.Int)
            objSQLCmd.Parameters("@Auditor_ID").Value = Auditor_ID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblAuditSurvey")
            GetAssignedFsrForVanAudit = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
   
    Public Function GetFsrForVanAudit(OrgID As String, AuditYear As String, AuditMonth As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.SalesRep_ID as ID,A.SalesRep_Name as Name from TBL_FSR A  inner join TBL_Org_CTL_DTL B on a.SalesRep_ID =B.SalesRep_ID    where A.SalesRep_ID not in(select SalesREp_ID from TBL_Van_Audit_Plan A inner join TBL_Van_Audit_Plan_Details B on A.Van_Audit_Plan_ID=B.Van_Audit_Plan_ID where a.Audit_year=@Audit_year and A.Audit_Month=@Audit_Month and Auditor_User_ID in(Select User_ID from TBL_User X inner join TBL_User_Types Y on x.User_Type_ID=y.User_Type_ID where  y.Designation='Y' and x.Org_HE_ID=@ORG_ID) )and b.MAS_Org_ID=@ORG_ID")

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@Audit_year", SqlDbType.Int)
            objSQLCmd.Parameters("@Audit_year").Value = AuditYear
            objSQLCmd.Parameters.Add("@Audit_Month", SqlDbType.Int)
            objSQLCmd.Parameters("@Audit_Month").Value = AuditMonth
            objSQLCmd.Parameters.Add("@ORG_ID", SqlDbType.VarChar)
            objSQLCmd.Parameters("@ORG_ID").Value = OrgID
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblAuditSurvey")
            GetFsrForVanAudit = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSurveyAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.SalesRep_ID as ID,A.SalesRep_Name as Name from TBL_FSR A,TBL_Survey_Audit_Responses B where A.SalesRep_ID=B.SalesRep_ID {0} order by Name ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "tblAuditSurvey")
            GetSurveyAudit = MsgDs.Tables(0)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74091"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerSurveyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Customer_ID, A.Site_Use_ID,(B.Customer_Name+'-'+ISNULL(B.Location,'N/A')) As Name,A.Survey_Timestamp,A.Survey_ID  from TBL_Survey_Cust_Responses A, TBL_Customer_Ship_Address As B  WHERE A.Customer_Id=B.Customer_ID AND A.Site_Use_Id=B.Site_Use_ID {0} ORDER By Name ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustSurveyListTbl")
            GetCustomerSurveyList = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAuditSurveyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT B.SalesRep_ID as Customer_ID,B.SalesRep_Name as Name,A.Survey_Timestamp,-1 as Site_Use_ID,A.Survey_ID from TBL_Survey_Audit_Responses A,TBL_FSR As B WHERE A.SalesRep_ID=B.SalesRep_ID {0} ORDER By Name ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustSurveyListTbl")
            GetAuditSurveyList = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetCustomerSurveyListDetail(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText from TBL_Survey_Cust_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Response=LTRIM(STR(E.Response_ID)) WHERE 1=1 {0} ORDER BY A.Question_ID", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustSurveyListDtlTbl")
            GetCustomerSurveyListDetail = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetAuditSurveyListDetail(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select B.Survey_Title,D.Question_Text,A.Response,E.Response_Type_ID,A.Question_ID,CASE ISNULL (E.Response_Type_ID,'') when '' Then A.Response Else E.Response_Text End as ResponseText from TBL_Survey_Audit_Responses as A INNER JOIN TBL_Survey as B ON A.Survey_ID=B.Survey_ID INNER JOIN TBL_Survey_Questions as D on A.Question_ID=D.Question_ID  LEFT OUTER JOIN TBL_Survey_Responses as E ON A.Response=LTRIM(STR(E.Response_ID)) WHERE 1=1 {0} ORDER BY A.Question_ID", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CustSurveyListDtlTbl")
            GetAuditSurveyListDetail = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function
    Public Function GetSurveyStats(ByRef iSurveyedCount As Integer, ByVal SurveyId As String, ByVal QuestionTemplate As String, ByVal ResponseHeaderTemplate As String, ByVal TextResponseTemplate As String, ByVal NonTextResponseTemplate As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting surveyed customer count......
            sQry = String.Format("SELECT COUNT(*) FROM (SELECT DISTINCT Customer_Id, Site_Use_Id from TBL_Survey_Cust_Responses WHERE Survey_Id={0}) DERIVEDTBL", SurveyId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iSurveyedCount = CInt(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()

            sQry = String.Format("SELECT DISTINCT A.Question_ID, A.Question_Text, B.Response_Type_ID from TBL_Survey_Questions as A, TBL_Survey_Responses As B where A.Question_Id=B.Question_Id AND A.Survey_ID='{0}' ORDER BY A.Question_ID ASC", SurveyId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader()

            Dim tempDBVal As Object
            Dim sQuestionID As String
            Dim sQuestionText As String

            Dim sTemp As String

            While objSQLDR.Read
                sQuestionID = CStr(objSQLDR.GetValue(0))
                tempDBVal = objSQLDR.GetValue(1)
                sQuestionText = CStr(IIf(IsDBNull(tempDBVal), "N/A", tempDBVal))
                tempDBVal = CStr(objSQLDR.GetValue(2))

                If tempDBVal <> "1" Then 'not a text type response...
                    sTemp = Replace(ResponseHeaderTemplate, "$QUES_TEXT", sQuestionText)

                    sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                    sRetVal = sRetVal & GetResponses(sQuestionID, NonTextResponseTemplate, iSurveyedCount)
                Else
                    sTemp = Replace(TextResponseTemplate, "$QUES_TEXT", sQuestionText)
                    sTemp = Replace(sTemp, "$QUES_ID", sQuestionID)

                    sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                End If
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetSurveyStats = sRetVal
    End Function
    Public Function GetResponses(ByVal QuestionId As String, ByVal NonTextResponseTemplate As String, ByVal iSurveyedCount As Integer) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting responses for a question...
            sQry = String.Format("SELECT Response_Text, ROUND(Cast((ResponseType * ResponseCount * 100) AS float) / {1}, 2) AS Response_Percentage, (ResponseType * ResponseCount) AS Response_Count FROM (SELECT TOP 20 A.Response_ID, A.Response_Text, CASE ISNULL(B.Customer_ID, 0) WHEN 0 THEN 0 ELSE 1 END AS ResponseType, COUNT(A.Response_ID) AS ResponseCount FROM TBL_Survey_Responses A LEFT OUTER JOIN TBL_Survey_Cust_Responses B ON CAST(A.Response_ID AS varchar) = B.Response  WHERE (A.Question_ID = {0}) GROUP BY A.Response_ID, A.Response_Text, CASE ISNULL(B.Customer_ID, 0) WHEN 0 THEN 0 ELSE 1 END ORDER BY A.Response_ID) DERIVEDTBL", QuestionId, iSurveyedCount)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sRespText As String
            Dim sPercentage As String
            Dim sCount As String

            Dim sTemp As String

            While objSQLDR.Read
                tempDBVal = objSQLDR.GetValue(0)
                sRespText = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(1)
                sPercentage = IIf(IsDBNull(tempDBVal), "0", tempDBVal)
                tempDBVal = objSQLDR.GetValue(2)
                sCount = IIf(IsDBNull(tempDBVal), "0", tempDBVal)

                sTemp = Replace(NonTextResponseTemplate, "$RESP_TEXT", sRespText)
                sTemp = Replace(sTemp, "$RESP_PERCENTAGE", sPercentage)
                sTemp = Replace(sTemp, "$RESP_COUNT", sCount)

                If sCount = 0 Then
                    sTemp = Replace(sTemp, "$STAT_BAR", "0")
                Else
                    sTemp = Replace(sTemp, "$STAT_BAR", "7")
                End If

                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetResponses = sRetVal
    End Function
    Public Function GetCustomerResponses(ByVal QuestionId As String, ByVal HeaderTemplate As String, ByVal ResponseStartTemplate As String, ByVal ResponseEndTemplate As String, ByVal ResponseTemplate As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting responses for a question...
            sQry = String.Format("SELECT B.Survey_Title,C.Question_Text, (D.Customer_Name+'-'+ISNULL(D.Location,'N/A')) As Customer_Name, A.Response FROM  TBL_Survey_Cust_Responses As A, TBL_Survey As B, TBL_Survey_Questions As C, TBL_Customer_Ship_Address As D WHERE A.Survey_Id=B.Survey_Id AND A.Question_Id=C.Question_Id AND A.Customer_Id=D.Customer_Id AND A.Site_Use_Id=D.Site_Use_Id AND A.Question_ID={0} ORDER By Customer_Name ASC", QuestionId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sSurveyTitle As String
            Dim sQuesText As String
            Dim sCustomerName As String
            Dim sResponse As String

            Dim sTemp As String

            While objSQLDR.Read
                tempDBVal = objSQLDR.GetValue(0)
                sSurveyTitle = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(1)
                sQuesText = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(2)
                sCustomerName = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(3)
                sResponse = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)

                If IsNothing(sTemp) Then
                    sTemp = Replace(HeaderTemplate, "$SURVEY_NAME", sSurveyTitle)
                    sTemp = Replace(sTemp, "$QUES_TEXT", sQuesText)
                    sTemp = String.Format("{0}{1}{2}", sTemp, vbCrLf, ResponseStartTemplate)
                End If

                sTemp = String.Format("{0}{1}{2}", sTemp, vbCrLf, Replace(ResponseTemplate, "$CUST_NAME", sCustomerName))
                sTemp = Replace(sTemp, "$RESP_TEXT", sResponse)

                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                sTemp = ""
            End While

            If Not IsNothing(sRetVal) Then
                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, ResponseEndTemplate)
            End If

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetCustomerResponses = sRetVal
    End Function
    Public Function GetSurveyAuditStats(ByRef iSurveyedCount As Integer, ByVal SurveyId As String, ByVal QuestionTemplate As String, ByVal ResponseHeaderTemplate As String, ByVal TextResponseTemplate As String, ByVal NonTextResponseTemplate As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting surveyed customer count......
            sQry = String.Format("SELECT COUNT(*) FROM (SELECT DISTINCT SalesRep_ID from TBL_Survey_Audit_Responses WHERE Survey_Id={0}) DERIVEDTBL", SurveyId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iSurveyedCount = CInt(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()

            sQry = String.Format("SELECT DISTINCT A.Question_ID, A.Question_Text, B.Response_Type_ID from TBL_Survey_Questions as A, TBL_Survey_Responses As B where A.Question_Id=B.Question_Id AND A.Survey_ID='{0}' ORDER BY A.Question_ID ASC", SurveyId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader()

            Dim tempDBVal As Object
            Dim sQuestionID As String
            Dim sQuestionText As String

            Dim sTemp As String

            While objSQLDR.Read
                sQuestionID = CStr(objSQLDR.GetValue(0))
                tempDBVal = objSQLDR.GetValue(1)
                sQuestionText = CStr(IIf(IsDBNull(tempDBVal), "N/A", tempDBVal))
                tempDBVal = CStr(objSQLDR.GetValue(2))

                If tempDBVal <> "1" Then 'not a text type response...
                    sTemp = Replace(ResponseHeaderTemplate, "$QUES_TEXT", sQuestionText)

                    sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                    sRetVal = sRetVal & GetAuditResponses(sQuestionID, NonTextResponseTemplate, iSurveyedCount)
                Else
                    sTemp = Replace(TextResponseTemplate, "$QUES_TEXT", sQuestionText)
                    sTemp = Replace(sTemp, "$QUES_ID", sQuestionID)

                    sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                End If
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetSurveyAuditStats = sRetVal
    End Function
    Public Function GetAuditResponses(ByVal QuestionId As String, ByVal NonTextResponseTemplate As String, ByVal iSurveyedCount As Integer) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting responses for a question...
            sQry = String.Format("SELECT Response_Text, ROUND(Cast((ResponseType * ResponseCount * 100) AS float) / {1}, 2) AS Response_Percentage, (ResponseType * ResponseCount) AS Response_Count FROM (SELECT TOP 20 A.Response_ID, A.Response_Text, CASE ISNULL(B.SalesRep_ID, 0) WHEN 0 THEN 0 ELSE 1 END AS ResponseType, COUNT(A.Response_ID) AS ResponseCount FROM TBL_Survey_Responses A LEFT OUTER JOIN TBL_Survey_Audit_Responses B ON CAST(A.Response_ID AS varchar) = B.Response  WHERE (A.Question_ID = {0}) GROUP BY A.Response_ID, A.Response_Text, CASE ISNULL(B.SalesRep_ID, 0) WHEN 0 THEN 0 ELSE 1 END ORDER BY A.Response_ID) DERIVEDTBL", QuestionId, iSurveyedCount)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sRespText As String
            Dim sPercentage As String
            Dim sCount As String

            Dim sTemp As String

            While objSQLDR.Read
                tempDBVal = objSQLDR.GetValue(0)
                sRespText = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(1)
                sPercentage = IIf(IsDBNull(tempDBVal), "0", tempDBVal)
                tempDBVal = objSQLDR.GetValue(2)
                sCount = IIf(IsDBNull(tempDBVal), "0", tempDBVal)

                sTemp = Replace(NonTextResponseTemplate, "$RESP_TEXT", sRespText)
                sTemp = Replace(sTemp, "$RESP_PERCENTAGE", sPercentage)
                sTemp = Replace(sTemp, "$RESP_COUNT", sCount)

                If sCount = 0 Then
                    sTemp = Replace(sTemp, "$STAT_BAR", "0")
                Else
                    sTemp = Replace(sTemp, "$STAT_BAR", "7")
                End If

                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetAuditResponses = sRetVal
    End Function
    Public Function GetVanAuditResponses(ByVal QuestionId As String, ByVal HeaderTemplate As String, ByVal ResponseStartTemplate As String, ByVal ResponseEndTemplate As String, ByVal ResponseTemplate As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'getting responses for a question...
            sQry = String.Format("SELECT B.Survey_Title,C.Question_Text, D.SalesRep_Name As Customer_Name, A.Response FROM  TBL_Survey_Audit_Responses As A, TBL_Survey As B, TBL_Survey_Questions As C, TBL_FSR As D WHERE A.Survey_Id=B.Survey_Id AND A.Question_Id=C.Question_Id AND A.SalesRep_ID=D.SalesRep_ID AND A.Question_ID={0} ORDER By Customer_Name ASC", QuestionId)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sSurveyTitle As String
            Dim sQuesText As String
            Dim sCustomerName As String
            Dim sResponse As String

            Dim sTemp As String

            While objSQLDR.Read
                tempDBVal = objSQLDR.GetValue(0)
                sSurveyTitle = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(1)
                sQuesText = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(2)
                sCustomerName = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                tempDBVal = objSQLDR.GetValue(3)
                sResponse = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)

                If IsNothing(sTemp) Then
                    sTemp = Replace(HeaderTemplate, "$SURVEY_NAME", sSurveyTitle)
                    sTemp = Replace(sTemp, "$QUES_TEXT", sQuesText)
                    sTemp = String.Format("{0}{1}{2}", sTemp, vbCrLf, ResponseStartTemplate)
                End If

                sTemp = String.Format("{0}{1}{2}", sTemp, vbCrLf, Replace(ResponseTemplate, "$CUST_NAME", sCustomerName))
                sTemp = Replace(sTemp, "$RESP_TEXT", sResponse)

                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, sTemp)
                sTemp = ""
            End While

            If Not IsNothing(sRetVal) Then
                sRetVal = String.Format("{0}{1}{2}", sRetVal, vbCrLf, ResponseEndTemplate)
            End If

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetVanAuditResponses = sRetVal
    End Function








    Public Function UpdateVanAudit(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal EmpCode As String, ByVal SalesRepId As String, ByVal SurveyId As String, ByVal UserId As String, ByRef dtItem As DataTable, ByVal SurveyTimeStamp As DateTime) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim sQry As String
        Dim sucess As Boolean = False
        Dim tran As SqlTransaction = Nothing
        Dim SurveyTime As DateTime = Now.ToString("MM-dd-yyyy HH:mm")

        Try
            'getting MSSQL DB connection.....
            'DELETE TBL_Survey_Audit_Response Table
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction()
            sQry = "DELETE FROM  TBL_Survey_Audit_responses WHERE Status='N' AND Survey_Id=@SurveyId AND SalesRep_Id=@SalesRepId AND Convert(Varchar(19),Survey_Timestamp,120)=Convert(Varchar(19),@SurveyTimestamp,120)"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@SurveyId", SurveyId)
            objSQLCmd.Parameters.AddWithValue("@SalesRepId", SalesRepId)
            objSQLCmd.Parameters.AddWithValue("@SurveyTimeStamp", SurveyTimeStamp)
            objSQLCmd.Transaction = tran
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            'Insert TBL_Survey_Audit_responses
            objSQLCmd = New SqlCommand("UpDateVanAudit", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add("@SurveyId", SqlDbType.VarChar, 100)
            objSQLCmd.Parameters.Add("@QuestionId", SqlDbType.VarChar, 50)
            objSQLCmd.Parameters.Add("@Response", SqlDbType.VarChar, 50)
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.VarChar, 50)
            objSQLCmd.Parameters.Add("@EmpCode", SqlDbType.VarChar, 100)
            objSQLCmd.Parameters.Add("@UserId", SqlDbType.VarChar, 100)
            objSQLCmd.Parameters.Add("@SurveyTime", SqlDbType.DateTime)
            objSQLCmd.Parameters.Add("@ResponseType", SqlDbType.VarChar, 50)
            objSQLCmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200)
            objSQLCmd.Transaction = tran


            For Each dr As DataRow In dtItem.Rows
                If dr("QuestId").ToString() <> "" Then
                    If dr("RespTypeId").ToString() <> "2" And dr("RespTypeId").ToString() <> "3" Then
                        objSQLCmd.Parameters("@SurveyId").Value = SurveyId
                        objSQLCmd.Parameters("@QuestionId").Value = dr("QuestId").ToString()
                        objSQLCmd.Parameters("@Response").Value = dr("Response").ToString()
                        objSQLCmd.Parameters("@SalesRepId").Value = SalesRepId
                        objSQLCmd.Parameters("@EmpCode").Value = EmpCode
                        objSQLCmd.Parameters("@UserId").Value = UserId
                        objSQLCmd.Parameters("@SurveyTime").Value = SurveyTime
                        objSQLCmd.Parameters("@ResponseType").Value = dr("RespTypeId").ToString()
                        objSQLCmd.Parameters("@Remarks").Value = dr("Remarks").ToString()
                        objSQLCmd.ExecuteNonQuery()
                    ElseIf dr("RespTypeId").ToString() = "2" Or dr("RespTypeId").ToString() = "3" Then

                        objSQLCmd.Parameters("@SurveyId").Value = SurveyId
                        objSQLCmd.Parameters("@QuestionId").Value = dr("QuestId").ToString()
                        objSQLCmd.Parameters("@Response").Value = dr("Response").ToString()
                        objSQLCmd.Parameters("@SalesRepId").Value = SalesRepId
                        objSQLCmd.Parameters("@EmpCode").Value = EmpCode
                        objSQLCmd.Parameters("@UserId").Value = UserId
                        objSQLCmd.Parameters("@SurveyTime").Value = SurveyTime
                        objSQLCmd.Parameters("@ResponseType").Value = dr("RespTypeId").ToString()
                        objSQLCmd.Parameters("@Remarks").Value = dr("Remarks").ToString()
                        objSQLCmd.ExecuteNonQuery()

                    End If
                End If
            Next

            objSQLCmd.Dispose()

            ''Update SurveyTimeStamp
            'sQry = "UPDATE  TBL_Survey_Audit_responses SET Survey_timestamp=GETDATE() WHERE Survey_Id=@SurveyId AND SalesRep_Id=@SalesRepId"
            'objSQLCmd = New SqlCommand(sQry, objSQLConn)
            'objSQLCmd.Parameters.AddWithValue("@SurveyId", SurveyId)
            'objSQLCmd.Parameters.AddWithValue("@SalesRepId", SalesRepId)
            'objSQLCmd.Transaction = tran
            'objSQLCmd.ExecuteNonQuery()
            'objSQLCmd.Dispose()

            sucess = True
            tran.Commit()
        Catch ex As Exception
            tran.Rollback()
            Error_No = 75014
            Error_Desc = String.Format("Error while updating van audit", ex.Message)

        Finally
            tran.Dispose()
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return sucess

    End Function






    Public Function ConfirmAuditSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SurvId As String, ByVal SalesRepId As String, ByVal userId As String, ByVal SurveyTimeStamp As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False
        Dim ConfirmAt As DateTime = Now.ToString("MM-dd-yyyy HH:mm")

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "UPDATE TBL_Survey_Audit_Responses  SET Status='C',Survey_TimeStamp=@ConfirmAt,Surveyed_by=@userId WHERE Survey_ID=@SurveyID and SalesRep_Id=@SalesRepID and Status='N' AND Survey_Timestamp=@SurveyTimestamp"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SurveyID", SqlDbType.BigInt).Value = SurvId
            objSQLCmd.Parameters.Add("@SalesRepId", SqlDbType.VarChar, 50).Value = SalesRepId
            objSQLCmd.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = userId
            objSQLCmd.Parameters.AddWithValue("@SurveyTimeStamp", SurveyTimeStamp)
            objSQLCmd.Parameters.AddWithValue("@ConfirmAt", ConfirmAt)

            objSQLCmd.ExecuteNonQuery()

            objSQLCmd.Dispose()

            retVal = True
        Catch ex As Exception
            Error_No = 79001
            Error_Desc = String.Format("Error while confirm survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function





    Public Function GetSurveyType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Code_value As Value,Code_Description AS [Key] from TBL_App_Codes WHERE Code_Type='SURVEY_TYPE_CODE' order by Code_value DESC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyType = dt
    End Function


    Public Function GetAvailFSRBySurveyID(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtAvail As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            ' objSQLDA = New SqlDataAdapter("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR WHERE SalesRep_ID NOT IN(SELECT SalesRep_ID FROM TBL_Survey_FSR WHERE Survey_ID=@SurveyID) order by SalesRep_Name ASC", objSQLConn) 'COMMENTED ON MAY 16'TH 2018 VOID ERROR IN CASE REP_ID NOT IN TBL_USER
            objSQLDA = New SqlDataAdapter("SELECT F.SalesRep_ID, SalesRep_Name from TBL_FSR F INNER JOIN TBL_User U ON U.SalesRep_ID =F.SalesRep_ID  WHERE F.SalesRep_ID NOT IN(SELECT SalesRep_ID FROM TBL_Survey_FSR WHERE Survey_ID=@SurveyID) order by SalesRep_Name ASC", objSQLConn)

            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.Fill(dtAvail)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetAvailFSRBySurveyID = dtAvail
    End Function
    Public Function GetAssignedFSRBySurveyID(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT A.SalesRep_ID, SalesRep_Name from TBL_FSR AS A INNER JOIN TBL_Survey_FSR AS B ON A.Salesrep_id=B.Salesrep_id WHERE Survey_ID=@SurveyID order by SalesRep_Name ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.Fill(dt)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetAssignedFSRBySurveyID = dt
    End Function
    Public Function GetResponsesType(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Response_Type_ID, Response_Type from TBL_Survey_Response_Types   order by Response_Type_ID ASC", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.Fill(dtResponseType)
            Dim dr As DataRow = dtResponseType.NewRow
            dr("Response_Type") = "--Select Response Type--"
            dtResponseType.Rows.InsertAt(dr, 0)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "73017"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetResponsesType = dtResponseType
    End Function
    Public Function GetSurveyDetailsByID(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dts As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT * from TBL_Survey  WHERE Survey_ID=@SurveyID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.Fill(dts)

            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyDetailsByID = dts
    End Function
    Public Function GetSurveyResponsesBySurveyID(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtR As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Question_ID ,(SELECT Question_Text FROM TBL_Survey_Questions WHERE Question_ID =A.Question_ID AND Survey_ID =@SurveyID)AS Question_Text ,Response_ID ,Response_Text,Show_Comment_Box ,CASE WHEN (SELECT COUNT(*) FROM TBL_Survey_Questions WHERE Question_ID =A.Question_ID AND Survey_ID =@SurveyID AND Default_Response_ID =A.Response_ID)>0 THEN 'Y' ELSE 'N' END AS DefValue ,'N' As IsDeleted,(SELECT Top 1 Response_Type_ID FROm TBL_Survey_Responses AS B WHERE B.Question_ID =A.Question_ID)AS Response_Type_ID,(SELECT Top 1 C.Response_Type   FROm TBL_Survey_Responses AS B INNER JOIN TBL_Survey_Response_Types   AS C On C.Response_Type_ID =B.Response_Type_ID  WHERE B.Question_ID =A.Question_ID)AS    Response_Type FROM TBL_Survey_Responses As A     WHERE Question_ID IN(SELECT Question_ID FROM TBL_Survey_Questions WHERE Survey_ID  =@SurveyID)", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.Fill(dtR)

            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyResponsesBySurveyID = dtR
    End Function
    Public Function GetSurveyQuestionsBySurveyID(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As Integer) As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter
        Dim dtQ As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter("SELECT Question_ID ,Is_Mandatory,Group_Text,Has_Image,Image_File,Question_Text ,Default_Response_ID ,'N' As IsDeleted,(SELECT Top 1 Response_Type_ID FROm TBL_Survey_Responses AS B WHERE B.Question_ID =A.Question_ID)AS Response_Type_ID,(SELECT Top 1 C.Response_Type  FROm TBL_Survey_Responses AS B INNER JOIN TBL_Survey_Response_Types AS C On C.Response_Type_ID =B.Response_Type_ID  WHERE B.Question_ID =A.Question_ID)AS Response_Type,Stuff((SELECT N'',  ',' + Response_Text  FROM TBL_Survey_Responses  AS X   WHERE X.Question_ID   =A.Question_ID  FOR XML PATH(''),TYPE).value('text()[1]','nvarchar(max)'),1,1,N'')AS ResponsesList,[Mandatory_On_Confirmation],[Remarks_Required],[No_Of_Lines_For_Text],[No_Of_Lines_For_Remarks],[Default_Star_Rating],[Sequence] FROM TBL_Survey_Questions As A WHERE Survey_ID =@SurveyID", objSQLConn)
            objSQLDA.SelectCommand.CommandType = CommandType.Text
            objSQLDA.SelectCommand.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLDA.Fill(dtQ)

            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        GetSurveyQuestionsBySurveyID = dtQ
    End Function
    Public Function UpdateSurveyStatus(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal SurvId As Integer, OldStatus As String, NewStatus As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim retVal As Boolean = False

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            If NewStatus = "A" Then
                sQry = "UPDATE TBL_Survey Set Status=@NewStatus,Start_Time =(CASE WHEN dbo.getdatepart('d',Start_Time)< dbo.getdatepart('d',GETDATE()) THEN  dbo.getdatepart('d',GETDATE()) ELSE  Start_Time END)  WHERE Survey_ID=@SurveyID AND Status=@OldStatus"
            Else
                sQry = "UPDATE TBL_Survey Set Status=@NewStatus WHERE Survey_ID=@SurveyID AND Status=@OldStatus"
            End If
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.Add("@SurveyID", SqlDbType.BigInt).Value = SurvId
            objSQLCmd.Parameters.Add("@OldStatus", SqlDbType.Char, 1).Value = OldStatus
            objSQLCmd.Parameters.Add("@NewStatus", SqlDbType.Char, 1).Value = NewStatus
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True

        Catch ex As Exception
            Error_No = 79001
            Error_Desc = String.Format("Error while Delete survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function CheckSurveyStatus(ByRef Err_No As Long, ByRef Err_Desc As String, SurveyID As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim RetVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection

            objSQLCMD = New SqlCommand("SELECT COUNT(*) FROM TBL_Survey WHERE Survey_ID=@SurveyID AND Status='D'", objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@SurveyID", SqlDbType.Int).Value = SurveyID
            Dim Cnt As Integer = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            If Cnt > 0 Then
                RetVal = True
            End If
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return RetVal
    End Function
    Public Function InsertSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, SSurveyID As Integer, ByVal Title As String, ByVal SDate As DateTime, ByVal ExpDate As DateTime, TypeCode As String, dtFSR As DataTable, dtQuestions As DataTable, dtResponses As DataTable, MessageMode As String, CreatedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim tran As SqlTransaction = Nothing
        Dim retVal As Boolean = False
        Dim SurveyID As Integer
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            tran = objSQLConn.BeginTransaction("RowIns")
            log.Error("started")
            If SSurveyID = "0" Then
                sQry = "insert into TBL_Survey(Survey_Title,Survey_type_Code, Start_time, End_Time,Status) values(@Title,@TypeCode,@SDate,@EDate,'N')"
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Transaction = tran
                objSQLCmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = Title
                objSQLCmd.Parameters.Add("@TypeCode", SqlDbType.Char, 1).Value = TypeCode
                objSQLCmd.Parameters.Add("@SDate", SqlDbType.DateTime).Value = SDate
                objSQLCmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = ExpDate
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                objSQLCmd = New SqlCommand("select @@IDENTITY as Survey_ID", objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Transaction = tran
                SurveyID = Convert.ToInt32(objSQLCmd.ExecuteScalar())
                objSQLCmd.Dispose()
            Else
                SurveyID = SSurveyID
                sQry = "UPDATE TBL_Survey SET Survey_Title =@Title,Survey_type_Code=@TypeCode, Start_time=@SDate, End_Time=@EndDate WHERE Survey_ID=@SurveyID"
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Transaction = tran
                objSQLCmd.Parameters.Add("@SurveyID", SqlDbType.Int).Value = SurveyID
                objSQLCmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = Title
                objSQLCmd.Parameters.Add("@TypeCode", SqlDbType.Char, 1).Value = TypeCode
                objSQLCmd.Parameters.Add("@SDate", SqlDbType.DateTime).Value = SDate
                objSQLCmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = ExpDate
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
            End If
            log.Error(SurveyID)
            'Delete all the existing FSR and Reinsert

            Dim SalesRepList As String = ""
            For Each dr As DataRow In dtFSR.Rows
                SalesRepList = SalesRepList + dr("SalesRep_ID").ToString() + ","
            Next

            log.Error("Step1")
            sQry = "DELETE FROM TBL_Survey_FSR WHERE Survey_ID=@SurveyID AND Salesrep_ID NOT IN (SELECT CAST( [Value] as int) FROM dbo.Split1(0,@SalesRepList  ,',') WHERE Value <> '' AND VALUE IS NOT NULL );DELETE FROM TBL_Survey_Responses WHERE Question_ID IN(SELECT Question_ID FROM TBl_Survey_Questions WHERE Survey_ID=@SurveyID AND Survey_ID IN(SELECT Survey_ID FROM TBL_Survey WHERE Status='N'));DELETE FROM TBl_Survey_Questions WHERE Survey_ID=@SurveyID AND Survey_ID IN(SELECT Survey_ID FROM TBL_Survey WHERE Status='N');"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Transaction = tran
            objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
            objSQLCmd.Parameters.AddWithValue("@SalesRepList", SalesRepList)
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            log.Error("Step2")

            If dtFSR.Rows.Count > 0 Then
                log.Error("Step3")
                For Each dr As DataRow In dtFSR.Rows
                    sQry = "INSERT INTO TBL_Survey_FSR(Survey_ID,SalesRep_ID) SELECT @SurveyID,@SalesRepID WHERE NOT EXISTS (SELECT 1 from TBL_Survey_FSR WHERE Survey_ID =@SurveyID AND SalesRep_ID =@SalesRepID)"
                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
                    objSQLCmd.Parameters.AddWithValue("@SalesRepID", dr("SalesRep_ID").ToString())
                    objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()
                Next

            End If

            If SDate > Now.Date Then
                log.Error("Step4")
                sQry = "Delete from TBL_Survey_Questions where Survey_ID=@SurveyID "
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.CommandType = CommandType.Text
                objSQLCmd.Transaction = tran
                objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
                objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

                For Each dr As DataRow In dtQuestions.Rows




                    sQry = "INSERT INTO TBL_Survey_Questions(Question_Text,Survey_ID,Is_Mandatory,Group_Text,Has_Image,Image_File,Mandatory_On_Confirmation,Remarks_Required,No_Of_Lines_For_Text,No_Of_Lines_For_Remarks,Default_Star_Rating,Sequence)VALUES(@QuestionText, @SurveyID,@Is_Mandatory,@Group_Text,@Has_Image,@Image_File,@Mandatory_On_Confirmation,@Remarks_Required,@No_Of_Lines_For_Text,@No_Of_Lines_For_Remarks,@Default_Star_Rating,@Sequence)"
                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
                    '  objSQLCmd.Parameters.AddWithValue("@QuestionID", QuestionID)
                    objSQLCmd.Parameters.AddWithValue("@QuestionText", dr("Question_Text").ToString())
                    objSQLCmd.Parameters.AddWithValue("@Is_Mandatory", IIf(dr("IsMandatory").ToString = "1", "Y", "N"))
                    objSQLCmd.Parameters.AddWithValue("@Group_Text", dr("Group_Text").ToString)
                    objSQLCmd.Parameters.AddWithValue("@Has_Image", IIf(dr("HasImage").ToString = "1", "Y", "N"))
                    objSQLCmd.Parameters.AddWithValue("@Image_File", dr("ImageName").ToString)
                    objSQLCmd.Parameters.AddWithValue("@Mandatory_On_Confirmation", dr("Mandatory_On_Confirmation").ToString)
                    objSQLCmd.Parameters.AddWithValue("@Remarks_Required", dr("Remarks_Required").ToString)
                    objSQLCmd.Parameters.AddWithValue("@No_Of_Lines_For_Text", dr("No_Of_Lines_For_Text").ToString)
                    objSQLCmd.Parameters.AddWithValue("@No_Of_Lines_For_Remarks", dr("No_Of_Lines_For_Remarks").ToString)
                    objSQLCmd.Parameters.AddWithValue("@Default_Star_Rating", dr("Default_Star_Rating").ToString)
                    objSQLCmd.Parameters.AddWithValue("@Sequence", dr("Sequence").ToString)
                    objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()



                    Dim QuestionID As Integer = 0
                    objSQLCmd = New SqlCommand("select @@IDENTITY as Question_ID", objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    QuestionID = Convert.ToInt32(objSQLCmd.ExecuteScalar())
                    objSQLCmd.Dispose()


                    sQry = "Delete from TBL_Survey_Responses where Question_ID=@Question_ID"
                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    objSQLCmd.Parameters.AddWithValue("@Question_ID", QuestionID)
                    objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()

                    For Each r As DataRow In dtResponses.Rows
                        If r("Question_ID").ToString() = dr("Question_ID").ToString() Then




                            'insert survey responses for the question
                            sQry = "INSERT INTO TBL_Survey_Responses( Response_Text,Question_ID,Response_Type_ID,Show_Comment_Box)VALUES(@ResponseText,@QuestionID, @RespTypeID,@Show_Comment_Box)  "
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Transaction = tran
                            ' objSQLCmd.Parameters.AddWithValue("@ResponseID", ResponseID)
                            objSQLCmd.Parameters.AddWithValue("@QuestionID", QuestionID)
                            objSQLCmd.Parameters.AddWithValue("@ResponseText", r("Response_Text").ToString())
                            objSQLCmd.Parameters.AddWithValue("@RespTypeID", dr("Response_Type_ID").ToString())
                            objSQLCmd.Parameters.AddWithValue("@Show_Comment_Box", IIf(r("ShowCommentBox").ToString() = "1", "Y", "N"))
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()


                            Dim ResponseID As Integer = 0
                            objSQLCmd = New SqlCommand("select @@IDENTITY as ResponseID ", objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Transaction = tran
                            ResponseID = Convert.ToInt32(objSQLCmd.ExecuteScalar())
                            objSQLCmd.Dispose()


                            'Update default response id
                            If r("Defvalue").ToString() = "1" Then
                                sQry = "UPDATE TBL_Survey_Questions SET Default_Response_ID=@ResponseID WHERE Survey_ID=@SurveyID AND Question_ID=@QuestionID"
                                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                                objSQLCmd.CommandType = CommandType.Text
                                objSQLCmd.Transaction = tran
                                objSQLCmd.Parameters.AddWithValue("@ResponseID", ResponseID)
                                objSQLCmd.Parameters.AddWithValue("@QuestionID", QuestionID)
                                objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
                                objSQLCmd.ExecuteNonQuery()
                                objSQLCmd.Dispose()
                            End If
                        End If

                    Next


                Next

            End If

            'Send Message to all FSR

            If MessageMode = "1" Then
                For Each dr As DataRow In dtFSR.Rows

                    sQry = "SELECT COUNT(*) FROM TBL_Message WHERE Sender_ID=@SID AND FSR_Plan_ID=@SurveyID"
                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    objSQLCmd.Parameters.AddWithValue("@SID", dr("SalesRep_ID").ToString())
                    objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)

                    Dim i As Integer = 0
                    i = objSQLCmd.ExecuteScalar()
                    objSQLCmd.Dispose()

                    Dim bExist As Boolean = False

                    If i > 0 Then
                        bExist = True
                    End If

                    If bExist = False Then
                        objSQLCmd = New SqlCommand("app_InsertApprovalComments", objSQLConn)
                        objSQLCmd.Transaction = tran
                        objSQLCmd.CommandType = CommandType.StoredProcedure
                        objSQLCmd.Parameters.Add(New SqlParameter("@Rp_ID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@Rp_ID").Value = SurveyID
                        objSQLCmd.Parameters.Add(New SqlParameter("@Message", SqlDbType.Text))
                        objSQLCmd.Parameters("@Message").Value = Title & " has been assigned to you"
                        objSQLCmd.Parameters.Add(New SqlParameter("@Sender_ID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@Sender_ID").Value = dr("SalesRep_ID").ToString()
                        objSQLCmd.Parameters.Add(New SqlParameter("@Msg_Title", SqlDbType.VarChar, 25))
                        objSQLCmd.Parameters("@Msg_Title").Value = "Survey"
                        objSQLCmd.Parameters.Add(New SqlParameter("@Expiry_Date", SqlDbType.DateTime))
                        objSQLCmd.Parameters("@Expiry_Date").Value = IIf(mExpiryDate <> 0, Today.AddDays(mExpiryDate - 1), 0)
                        Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@MessageId", SqlDbType.Int))
                        parameter.Direction = ParameterDirection.Output

                        Dim MessageID As Integer
                        objSQLCmd.ExecuteNonQuery()
                        MessageID = parameter.Value
                        objSQLCmd.Dispose()

                        objSQLCmd = New SqlCommand("app_AssignMessage", objSQLConn)
                        objSQLCmd.Transaction = tran
                        objSQLCmd.CommandType = CommandType.StoredProcedure
                        objSQLCmd.Parameters.Add(New SqlParameter("@MessageID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@MessageID").Value = MessageID
                        objSQLCmd.Parameters.Add(New SqlParameter("@SalesRep_ID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@SalesRep_ID").Value = dr("SalesRep_ID").ToString()
                        objSQLCmd.Parameters.Add(New SqlParameter("@Read_Stat", SqlDbType.Char))
                        objSQLCmd.Parameters("@Read_Stat").Value = "N"
                        Dim parameter1 As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@RowsAffected", SqlDbType.Int))
                        parameter1.Direction = ParameterDirection.Output

                        objSQLCmd.ExecuteNonQuery()
                        objSQLCmd.Dispose()
                    End If
                    bExist = False
                Next

            Else


                For Each dr As DataRow In dtFSR.Rows

                    sQry = "SELECT COUNT(*) FROM TBL_Msg_Recipients AS A INNER JOIN TBL_MSG AS B ON A.Msg_ID=B.Msg_ID WHERE A.Recipient_ID IN (SELECT User_ID FROM TBl_User WHERE SalesRep_ID=@SID) AND A.Custom_Attribute_1=@SurveyID AND B.Msg_Title =@Title AND B.Msg_Body =@Message_Content AND B.Sender_ID=@SenderID "
                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    objSQLCmd.CommandType = CommandType.Text
                    objSQLCmd.Transaction = tran
                    objSQLCmd.Parameters.AddWithValue("@SID", dr("SalesRep_ID").ToString())
                    objSQLCmd.Parameters.AddWithValue("@SurveyID", SurveyID)
                    objSQLCmd.Parameters.AddWithValue("@SenderID", CreatedBy)
                    objSQLCmd.Parameters.AddWithValue("@Title", "Survey")
                    objSQLCmd.Parameters.AddWithValue("@Message_Content", Title & " has been assigned to you")
                    Dim i As Integer = 0
                    i = objSQLCmd.ExecuteScalar()
                    objSQLCmd.Dispose()

                    Dim bExist As Boolean = False

                    If i > 0 Then
                        bExist = True
                    End If

                    If bExist = False Then
                        objSQLCmd = New SqlCommand("app_InsertSurveyandRouteMsg", objSQLConn)
                        objSQLCmd.Transaction = tran
                        objSQLCmd.CommandType = CommandType.StoredProcedure
                        objSQLCmd.Parameters.Add(New SqlParameter("@Title", SqlDbType.VarChar, 50))
                        objSQLCmd.Parameters("@Title").Value = "Survey"
                        objSQLCmd.Parameters.Add(New SqlParameter("@Message_Content", SqlDbType.VarChar, 255))
                        objSQLCmd.Parameters("@Message_Content").Value = Title & " has been assigned to you"
                        objSQLCmd.Parameters.Add(New SqlParameter("@SenderID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@SenderID").Value = CreatedBy

                        objSQLCmd.Parameters.Add(New SqlParameter("@MessageDate", SqlDbType.DateTime))
                        objSQLCmd.Parameters("@MessageDate").Value = SDate
                        objSQLCmd.Parameters.Add(New SqlParameter("@SalesRepID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@SalesRepID").Value = dr("SalesRep_ID").ToString()

                        objSQLCmd.Parameters.Add(New SqlParameter("@SourceID", SqlDbType.BigInt))
                        objSQLCmd.Parameters("@SourceID").Value = SurveyID
                        objSQLCmd.ExecuteNonQuery()
                        objSQLCmd.Dispose()
                    End If
                    bExist = False
                Next
            End If


            tran.Commit()
            retVal = True

        Catch ex As Exception
            log.Error(ex.ToString)
            tran.Rollback("RowIns")
            Error_No = 79001
            Error_Desc = String.Format("Error while saving survey: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            tran.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return retVal
    End Function
    Public Function CheckSurveyTitle(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyTitle As String, SurveyID As String) As Boolean

        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim RetVal As Boolean = False
        Try
            Dim qry As String = ""
            objSQLConn = _objDB.GetSQLConnection
            If SurveyID = "0" Then
                qry = "SELECT COUNT(*) FROM TBL_Survey WHERE UPPER(Survey_Title)=@STitle"
            Else
                qry = "SELECT COUNT(*) FROM TBL_Survey WHERE UPPER(Survey_Title)=@STitle AND Survey_ID<>@SurveyID"
            End If
            objSQLCMD = New SqlCommand(qry, objSQLConn)
            objSQLCMD.CommandType = CommandType.Text
            objSQLCMD.Parameters.Add("@STitle", SqlDbType.VarChar, 50).Value = SurveyTitle.ToUpper()
            objSQLCMD.Parameters.Add("@SurveyID", SqlDbType.Int).Value = SurveyID
            Dim Cnt As Integer = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            If Cnt > 0 Then
                RetVal = True
            End If
            objSQLCMD.Dispose()
        Catch ex As Exception
            Err_No = "75016"
            Err_Desc = ex.Message
            Throw ex
        Finally
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return RetVal
    End Function
End Class
