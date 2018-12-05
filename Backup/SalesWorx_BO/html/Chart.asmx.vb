Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Services
Imports System.Runtime.Serialization
Imports System.IO
Imports System.Web.Script.Serialization
Imports SalesWorx.BO.Common
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Chart
    Inherits System.Web.Services.WebService
    Private _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
Public Function GetSalesbyAgency(ByVal param As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByAgency_DashBoard]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Uid", param)
            objSQLCmd.Parameters.AddWithValue("@FromDate", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", Now.ToString("dd-MMM-yyyy"))

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetSalesbyAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception
           
            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Agency").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
Public Function GetSalesbyVan(ByVal param As String) As Object
        Dim DashBoardSalesbyVan As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyVan As DashBoardSales


        'Dim ds As New DataSet
        'ds = GetSalesbyVan("", "", param, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"), Now.ToString("dd-MMM-yyyy"))
        Dim MsgDs As New DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByVan_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@Uid", param)
                objSQLCmd.Parameters.AddWithValue("@FromDate", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))
                objSQLCmd.Parameters.AddWithValue("@ToDate", Now.ToString("dd-MMM-yyyy"))

                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                objSQLCmd.Dispose()
            Catch ex As Exception
              
                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyVan = New DashBoardSales
            oDashBoardSalesbyVan.Description = dr("Vanno").ToString
            oDashBoardSalesbyVan.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyVan.Add(oDashBoardSalesbyVan)
            oDashBoardSalesbyVan = Nothing
        Next
        Return DashBoardSalesbyVan
    End Function

    Private Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByAgency_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyAgency = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetSalesbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByVan_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyVan = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
  
End Class