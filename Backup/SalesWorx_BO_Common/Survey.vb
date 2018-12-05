Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL

Public Class Survey
    Private _sTitle As String
    Private _sSTypeCode As Char
    Private _dSTime As DateTime
    Private _dETime As DateTime
    Public _dtSR As New DataTable

    Private _sSurveyID As Integer
    Private _SQuestion As String
    Private _sResponseID As Integer

    Private _sRespQuestID As Integer
    Private _SResponse As String
    Private _sResponseTypeID As Integer


    Private _sSalRepID As Integer
    Private _SCustID As Integer
    Private _sSiteID As Integer

    Private _iSurveyedCount As Integer
    Dim ObjDAlSurvey As New DAL_AdminSurvey

    Public Property Title() As String
        Set(ByVal value As String)
            _sTitle = value
        End Set
        Get
            Return _sTitle
        End Get
    End Property

    Public Property TypeCode() As Char
        Set(ByVal value As Char)
            _sSTypeCode = value
        End Set
        Get
            Return _sSTypeCode
        End Get
    End Property

    Public Property StartDate() As DateTime
        Set(ByVal value As DateTime)
            _dSTime = value
        End Set
        Get
            Return _dSTime
        End Get
    End Property


    Public Property ExpiryDate() As DateTime
        Set(ByVal value As DateTime)
            _dETime = value
        End Set
        Get
            Return _dETime
        End Get
    End Property

    Public Property SurveyID() As Integer
        Set(ByVal value As Integer)
            _sSurveyID = value
        End Set
        Get
            Return _sSurveyID
        End Get
    End Property

    Public Property ResponseID() As Integer
        Set(ByVal value As Integer)
            _sResponseID = value
        End Set
        Get
            Return _sResponseID
        End Get
    End Property

    Public Property Question() As String
        Set(ByVal value As String)
            _SQuestion = value
        End Set
        Get
            Return _SQuestion
        End Get
    End Property



    Public Property ResponseQuestID() As Integer
        Set(ByVal value As Integer)
            _sRespQuestID = value
        End Set
        Get
            Return _sRespQuestID
        End Get
    End Property

    Public Property ResponseTypeID() As Integer
        Set(ByVal value As Integer)
            _sResponseTypeID = value
        End Set
        Get
            Return _sResponseTypeID
        End Get
    End Property

    Public Property ResponseText() As String
        Set(ByVal value As String)
            _SResponse = value
        End Set
        Get
            Return _SResponse
        End Get
    End Property

    Public Property SalesRepID() As Integer
        Set(ByVal value As Integer)
            _sSalRepID = value
        End Set
        Get
            Return _sSalRepID
        End Get
    End Property

    Public Property CustomerID() As Integer
        Set(ByVal value As Integer)
            _SCustID = value
        End Set
        Get
            Return _SCustID
        End Get
    End Property

    Public Property SiteUsedID() As Integer
        Set(ByVal value As Integer)
            _sSiteID = value
        End Set
        Get
            Return _sSiteID
        End Get
    End Property

    Public Property SurveyedCount() As Integer
        Get
            SurveyedCount = _iSurveyedCount
        End Get
        Set(ByVal Value As Integer)
            _iSurveyedCount = Value
        End Set
    End Property


    Public Function LoadSurveys(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlSurvey.GetSurveys(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadSurveysFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.GetSurveysFSR(Err_No, Err_Desc, SurveyId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function LoadSalesRep(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlSurvey.GetSalesRep(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function AddSurvey(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            ObjDAlSurvey.dtSalesRepSelected = _dtSR
            Return ObjDAlSurvey.SaveSurvey(Err_No, Err_Desc, _sTitle, _sSTypeCode, _dSTime, _dETime)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EditSurveys(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.SearchSurveys(Err_No, Err_Desc, SurveyId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ModifySurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurvId As Integer) As Boolean
        Try
            ObjDAlSurvey.dtSalesRepSelected = _dtSR
            Return ObjDAlSurvey.UpdateSurvey(Err_No, Err_Desc, SurvId, _sTitle, _sSTypeCode, _dSTime, _dETime)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RemoveSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurvId As Integer) As Boolean
        Try
            Return ObjDAlSurvey.DeleteSurvey(Err_No, Err_Desc, SurvId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ModifyQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer) As Boolean
        Try

            Return ObjDAlSurvey.UpdateQuestions(Err_No, Err_Desc, QuestId, _SQuestion, _sSurveyID, _sResponseID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RemoveSurveyQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer) As Boolean
        Try
            Return ObjDAlSurvey.DeleteQuestion(Err_No, Err_Desc, QuestId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function AddSurveyQuestions(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlSurvey.SaveQuestions(Err_No, Err_Desc, _SQuestion, _sSurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EditSurveysQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer, ByVal SurveyId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.SearchQuestion(Err_No, Err_Desc, QuestId, _sSurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadQuestions(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlSurvey.GetQuestions(Err_No, Err_Desc, _sSurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestID As Integer) As DataTable
        Try
            Return ObjDAlSurvey.GetResponses(Err_No, Err_Desc, QuestID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function LoadResponseTypes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SurveyID As String) As DataTable
        Try
            Return ObjDAlSurvey.GetResponsesType(Err_No, Err_Desc, SurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function




    Public Function RemoveSurveyResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer) As Boolean
        Try
            Return ObjDAlSurvey.DeleteResponses(Err_No, Err_Desc, QuestId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function AddSurveyResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal defresp As Boolean) As Boolean
        Try
            Return ObjDAlSurvey.SaveResponses(Err_No, Err_Desc, _SResponse, _sRespQuestID, _sResponseTypeID, defresp)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function EditSurveysResponses(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QuestId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.SearchResponses(Err_No, Err_Desc, _sRespQuestID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function LoadDefaultFSRCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalRepId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.GetAvailableCustomers(Err_No, Err_Desc, SalRepId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadAssignedFSRCustomers(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalRepId As Integer) As DataTable
        Try
            Return ObjDAlSurvey.GetAssignedCustomers(Err_No, Err_Desc, SalRepId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AddFSRCustomers(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlSurvey.InsertCustomersToFSR(Err_No, Err_Desc, _sSalRepID, _SCustID, _sSiteID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RemoveFSRCustomers(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlSurvey.RemoveCustomersFromFSR(Err_No, Err_Desc, _sSalRepID, _SCustID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetSurveyDate(ByRef Err_No As Long, ByRef Err_Desc As String) As DateTime
        Try
            Return ObjDAlSurvey.CheckSurveyDate(Err_No, Err_Desc, _sSurveyID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetMarketSurveys(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlSurvey.GetMarketSurveys(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckduplicateSurvey(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlSurvey.CheckSurveyTitle(Err_No, Err_Desc, _sTitle)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAllSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDAlSurvey.GetAllSurvey(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDAlSurvey.GetCustomerSurvey(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSurveyAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDAlSurvey.GetSurveyAudit(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSurveyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDAlSurvey.GetCustomerSurveyList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAuditSurveyList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDAlSurvey.GetAuditSurveyList(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerSurveyListDetail(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDAlSurvey.GetCustomerSurveyListDetail(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAuditSurveyListDetail(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDAlSurvey.GetAuditSurveyListDetail(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSurveyStats(ByVal SurveyId As String, ByVal QuestionTemplate As String, ByVal ResponseHeaderTemplate As String, ByVal TextResponseTemplate As String, ByVal NonTextResponseTemplate As String) As String
        Try
            Return ObjDAlSurvey.GetSurveyStats(_iSurveyedCount, SurveyId, QuestionTemplate, ResponseHeaderTemplate, TextResponseTemplate, NonTextResponseTemplate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerResponses(ByVal QuestionId As String, ByVal HeaderTemplate As String, ByVal ResponseStartTemplate As String, ByVal ResponseEndTemplate As String, ByVal ResponseTemplate As String) As String
        Try
            Return ObjDAlSurvey.GetCustomerResponses(QuestionId, HeaderTemplate, ResponseStartTemplate, ResponseEndTemplate, ResponseTemplate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSurveyAuditStats(ByVal SurveyId As String, ByVal QuestionTemplate As String, ByVal ResponseHeaderTemplate As String, ByVal TextResponseTemplate As String, ByVal NonTextResponseTemplate As String) As String
        Try
            Return ObjDAlSurvey.GetSurveyAuditStats(_iSurveyedCount, SurveyId, QuestionTemplate, ResponseHeaderTemplate, TextResponseTemplate, NonTextResponseTemplate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetVanAuditResponses(ByVal QuestionId As String, ByVal HeaderTemplate As String, ByVal ResponseStartTemplate As String, ByVal ResponseEndTemplate As String, ByVal ResponseTemplate As String) As String
        Try
            Return ObjDAlSurvey.GetVanAuditResponses(QuestionId, HeaderTemplate, ResponseStartTemplate, ResponseEndTemplate, ResponseTemplate)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function SaveVanAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal EmpCode As String, ByVal SalesRepId As String, ByVal SurveyId As String, ByVal UserId As String, ByRef dtItem As DataTable, ByVal SurveyTimeStamp As String) As Boolean
        Try
            Return ObjDAlSurvey.UpdateVanAudit(Err_No, Err_Desc, EmpCode, SalesRepId, SurveyId, UserId, dtItem, SurveyTimeStamp)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConfirmVanAudit(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SalesRepId As String, ByVal SurveyId As String, ByVal UserId As String, ByVal SurveyTimeStamp As String) As Boolean
        Try
            Return ObjDAlSurvey.ConfirmAuditSurvey(Err_No, Err_Desc, SurveyId, SalesRepId, UserId, SurveyTimeStamp)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
