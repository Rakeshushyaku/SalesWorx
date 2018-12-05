Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL

Public Class ApprovalCode
    Private _iFSR As Integer
    Private _iUserID As Integer
    Private _sApprovalCode As String
    Dim ObjDAlAppCode As New DAL_ApprovalCode

    Public Property FSR() As Integer
        Set(ByVal value As Integer)
            _iFSR = value
        End Set
        Get
            Return _iFSR
        End Get
    End Property

    Public Property UserID() As Integer
        Set(ByVal value As Integer)
            _iUserID = value
        End Set
        Get
            Return _iUserID
        End Get
    End Property


    Public Property ApprovalCode() As String
        Set(ByVal value As String)
            _sApprovalCode = value
        End Set
        Get
            Return _sApprovalCode
        End Get
    End Property

    Public Function GetAppCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Try

            Dim dt As New DataTable
            dt = ObjDAlAppCode.GetApprovalCode(Err_No, Err_Desc, _iFSR, _iUserID)
            If dt.Rows.Count > 0 Then
                _sApprovalCode = dt.Rows(0)(0).ToString()
                success = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function
End Class
