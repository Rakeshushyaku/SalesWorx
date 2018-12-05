Imports System.Data
Imports System.Data.SqlClient

Public Class DAL_PriceList
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetProductsPrices(ByRef ds As DataSet, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim objSQLConn As New SqlConnection
        Dim objSQLCmd As New SqlCommand
        Dim SqlAd As SqlDataAdapter


        Try

            objSQLConn = _objDB.GetSQLConnection
            With objSQLCmd
                .Connection = objSQLConn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "app_GetProductsPrices"

            End With

            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(ds)

            Return True

        Catch ex As Exception
            Err_No = "1001"
            Err_Desc = ex.Message
            Return False
        Finally

            If Not objSQLCmd Is Nothing Then
                objSQLCmd.Dispose()
                objSQLCmd = Nothing
                SqlAd = Nothing

            End If

            _objDB.CloseSQLConnection(objSQLConn)

        End Try
    End Function
End Class

