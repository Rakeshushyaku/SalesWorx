Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class POSM
    Dim ObjPOSM As New DAL_POSM
    Public Function GetQuestionGroups(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjPOSM.GetQuestionGroups(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetQuestionCodes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Code_Type As String) As DataTable
        Try
            Return ObjPOSM.GetQuestionCodes(Err_No, Err_Desc, Code_Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSurveyQuestionDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal ID As String) As DataTable
        Try
            Return ObjPOSM.GetSurveyQuestionDetails(Err_No, Err_Desc, ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetQuestions(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Group As String = "", Optional ByVal Question As String = "", Optional ByVal Filter As String = "") As DataTable
        Try
            Return ObjPOSM.GetQuestions(Err_No, Err_Desc, Group, Question, Filter)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ManagePOSMAppcodes(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Opt As String, ByVal Code_Type As String, ByVal Code_Value As String, ByVal Code_Description As String, ByVal Addedby As Integer, ByRef msg As String) As Boolean
        Try
            Return ObjPOSM.ManagePOSMAppcodes(Error_No, Error_Desc, Opt, Code_Type, Code_Value, Code_Description, Addedby, msg)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ManagePOSMSurvey(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Opt As String, ByVal Question_Group_1 As String, ByVal Question_1 As String, ByVal Question_2 As String, ByVal Addedby As Integer, ByVal Response_Type As String, ByVal QID As String, ByRef msg As String) As Boolean
        Try
            Return ObjPOSM.ManagePOSMSurvey(Error_No, Error_Desc, Opt, Question_Group_1, Question_1, Question_2, Addedby, Response_Type, QID, msg)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetSurvey(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal Group As String = "") As DataTable
        Try
            Return ObjPOSM.GetSurvey(Err_No, Err_Desc, Group)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
