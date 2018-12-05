Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class SyncCutOffDate
    Dim ObjSyncCutOffDate As New DAL_SyncCutOffDate
    Private _sYear As String
    Private _sMonth As String
    Private _sCountry As String
    Private _dCutOffDate As String
    
    Public Property Year() As String
        Set(ByVal value As String)
            _sYear = value
        End Set
        Get
            Return _sYear
        End Get
    End Property

    Public Property Month() As String
        Set(ByVal value As String)
            _sMonth = value
        End Set
        Get
            Return _sMonth
        End Get
    End Property
    Public Property Country() As String
        Set(ByVal value As String)
            _sCountry = value
        End Set
        Get
            Return _sCountry
        End Get
    End Property
    Public Property CutOffDate() As String
        Set(ByVal value As String)
            _dCutOffDate = value
        End Set
        Get
            Return _dCutOffDate
        End Get
    End Property

    Public Function GetYears(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjSyncCutOffDate.GetYears(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSyncCutOffDates(ByRef Err_No As Long, ByRef Err_Desc As String, year As String, month As String, Country As String) As DataTable
        Try
            Return ObjSyncCutOffDate.GetSyncCutOffDates(Err_No, Err_Desc, year, month, Country)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCountries(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjSyncCutOffDate.GetCountries(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveSyncCutOffDate(ByRef Err_No As Long, ByRef Err_Desc As String, UserID As String) As Boolean
        Try
            Return ObjSyncCutOffDate.SaveSyncCutOffDate(Err_No, Err_Desc, Me.Year, Me._sMonth, Me._dCutOffDate, UserID, _sCountry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteSyncCutOffDate(ByRef Err_No As Long, ByRef Err_Desc As String, Country As String, Yr As String, Mnth As String) As Boolean
        Try
            Return ObjSyncCutOffDate.DeleteSyncCutOffDate(Err_No, Err_Desc, Country, Yr, Mnth)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
