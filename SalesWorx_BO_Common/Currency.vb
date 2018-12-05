Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL

Public Class Currency
    Private _sCurrencyCode As String
    Private _sDescription As String
    Private _dRate As Decimal
    Private _ddigits As Integer
    Dim ObjDAlCurrency As New DAL_Currency

    Public Property CurrencyCode() As String
        Set(ByVal value As String)
            _sCurrencyCode = value
        End Set
        Get
            Return _sCurrencyCode
        End Get
    End Property

    Public Property CurrencyName() As String
        Set(ByVal value As String)
            _sDescription = value
        End Set
        Get
            Return _sDescription
        End Get
    End Property

    Public Property ConversionRate() As Decimal
        Set(ByVal value As Decimal)
            _dRate = value
        End Set
        Get
            Return _dRate
        End Get
    End Property


    Public Property DecimalDigits() As Integer
        Set(ByVal value As Integer)
            _ddigits = value
        End Set
        Get
            Return _ddigits
        End Get
    End Property
    Dim _SCountry As String
    Public Property Country() As String
        Set(ByVal value As String)
            _SCountry = value
        End Set
        Get
            Return _SCountry
        End Get
    End Property


    Public Function InsertCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlCurrency.InsertCurrency(Err_No, Err_Desc, _sCurrencyCode, _sDescription, _dRate, _ddigits, _SCountry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlCurrency.UpdateCurrency(Err_No, Err_Desc, _sCurrencyCode, _sDescription, _dRate, _ddigits, _SCountry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlCurrency.DeleteCurrency(Err_No, Err_Desc, _sCurrencyCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FillCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlCurrency.FillCurrencyGrid(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FillCountry(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlCurrency.FillCountry(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Try

            Dim dt As New DataTable
            dt = ObjDAlCurrency.GetCurrency(Err_No, Err_Desc, _sCurrencyCode)
            If dt.Rows.Count > 0 Then
                _sCurrencyCode = dt.Rows(0)("Currency_Code").ToString()
                _sDescription = dt.Rows(0)("Description").ToString()
                _dRate = Double.Parse(dt.Rows(0)("Conversion_Rate").ToString())
                _ddigits = Integer.Parse(dt.Rows(0)("Decimal_Digits").ToString())
                success = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function SearchCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjDAlCurrency.SearchCurrencyGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CheckDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlCurrency.CheckDuplicate(Err_No, Err_Desc, _sCurrencyCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
