Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Bank
    Private _sBankCode As String
    Private _sDescription As String
    Private _sCurrency As String
    Dim ObjDAlBank As New DAL_Bank

    Public Property BankCode() As String
        Set(ByVal value As String)
            _sBankCode = value
        End Set
        Get
            Return _sBankCode
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



    Public Property Currency() As String
        Set(ByVal value As String)
            _sCurrency = value
        End Set
        Get
            Return _sCurrency
        End Get
    End Property
    Public Function FillCountry(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlBank.FillCurrency(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillBankCode(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlBank.FillBankCodeGrid(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlBank.CheckDuplicate(Err_No, Err_Desc, _sBankCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertBankCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlBank.InsertBankCode(Err_No, Err_Desc, _sBankCode, _sDescription, _sCurrency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateBankCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlBank.UpdateBankCode(Err_No, Err_Desc, _sBankCode, _sDescription, _sCurrency)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteBankCode(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlBank.DeleteBankCode(Err_No, Err_Desc, _sBankCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SearchBankCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlBank.SearchBankCodeGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
