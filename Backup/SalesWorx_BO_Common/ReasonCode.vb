Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL

Public Class ReasonCode
    Private _sReasonCode As String
    Private _sDescription As String
    Private _sPurpose As String
    
    Dim ObjDAlReason As New DAL_ReasonCode

    Public Property ReasonCode() As String
        Set(ByVal value As String)
            _sReasonCode = value
        End Set
        Get
            Return _sReasonCode
        End Get
    End Property

    Public Property Description() As String
        Set(ByVal value As String)
            _sDescription = value
        End Set
        Get
            Return _sDescription
        End Get
    End Property

    

    Public Property Purpose() As String
        Set(ByVal value As String)
            _sPurpose = value
        End Set
        Get
            Return _sPurpose
        End Get
    End Property



    Public Function InsertReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlReason.InsertReasonCode(Err_No, Err_Desc, _sReasonCode, _sDescription, _sPurpose)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlReason.UpdateReasonCode(Err_No, Err_Desc, _sReasonCode, _sDescription, _sPurpose)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlReason.DeleteReasonCode(Err_No, Err_Desc, _sReasonCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FillReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlReason.FillReasonCodeGrid(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Try

            Dim dt As New DataTable
            dt = ObjDAlReason.GetReasonCode(Err_No, Err_Desc, _sReasonCode)
            If dt.Rows.Count > 0 Then
                _sReasonCode = dt.Rows(0)("Reason_Code").ToString()
                _sDescription = dt.Rows(0)("Description").ToString()
                _sPurpose = dt.Rows(0)("Purpose").ToString()
                success = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function SearchReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlReason.SearchReasonCodeGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlReason.CheckDuplicate(Err_No, Err_Desc, _sReasonCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetConnection() As SqlConnection
        Return (ObjDAlReason.GetConnection())
    End Function


    Public Function RebuildAllReasonCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlReason.DeleteAll(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
