Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Collections
Imports System.Web.Services
Imports System.Web.Script
Imports System.Web.Services.Protocols
Imports System.Configuration
Imports SalesWorx.BO.Common


<WebService([Namespace]:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<System.Web.Script.Services.ScriptService()> _
Public Class wsProduct
    Inherits System.Web.Services.WebService
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    'Private AutoCategory As String = ConfigurationSettings.AppSettings("AutoCategory")
    ' Private AutouserId As String = ConfigurationSettings.AppSettings("AutoUserId")


    <WebMethod()> _
   Public Function GetBItemCode(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String
        objSQLConn = New SqlConnection(_strSQLConn)
        objSQLConn.Open()

        '        Dim sql As String = "SET ROWCOUNT @Count SELECT      B.Item_Code FROM     TBL_Product AS B  WHERE Item_Code LIKE @Desc  ORDER By Item_Code SET ROWCOUNT 0"
        Dim sql As String = "SET ROWCOUNT @Count SELECT      B.Item_Code FROM     TBL_Product AS B  WHERE B.Organization_ID=@OrgID AND Item_Code LIKE @Desc  ORDER By Item_Code SET ROWCOUNT 0"
        objSQLDa = New SqlDataAdapter(sql, objSQLConn)
        objSQLDa.SelectCommand.Parameters.Add("@Count", SqlDbType.Int, 4).Value = count
        objSQLDa.SelectCommand.Parameters.Add("@Desc", SqlDbType.NVarChar, 150).Value = "%" & prefixText & "%"
        objSQLDa.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 50).Value = contextKey

        Dim ItemCode As String() = Nothing
        Try
            objSQLDa.Fill(dt)
            ItemCode = New String(dt.Rows.Count - 1) {}
            Dim i As Integer = 0
            For Each dr As DataRow In dt.Rows
                ItemCode.SetValue(dr("Item_Code").ToString(), i)
                i += 1
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn.Dispose()
            dt.Dispose()
        End Try
        Return ItemCode
    End Function

    <WebMethod()> _
    Public Function GetBItemDescription(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String
        objSQLConn = New SqlConnection(_strSQLConn)
        objSQLConn.Open()

        Dim sql As String = "SET ROWCOUNT @Count  SELECT      B.Description FROM     TBL_Product AS B WHERE B.Organization_ID=@OrgID AND B.Description LIKE @Desc  ORDER By B.Description SET ROWCOUNT 0"

        objSQLDa = New SqlDataAdapter(sql, objSQLConn)
        objSQLDa.SelectCommand.Parameters.Add("@Count", SqlDbType.Int, 4).Value = count
        objSQLDa.SelectCommand.Parameters.Add("@Desc", SqlDbType.NVarChar, 150).Value = "%" & prefixText & "%"
        objSQLDa.SelectCommand.Parameters.Add("@OrgID", SqlDbType.VarChar, 50).Value = contextKey

        Dim Desc As String() = Nothing
        Try
            objSQLDa.Fill(dt)
            Desc = New String(dt.Rows.Count - 1) {}
            Dim i As Integer = 0
            For Each dr As DataRow In dt.Rows
                Desc.SetValue(dr("Description").ToString(), i)
                i += 1
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn.Dispose()
            dt.Dispose()
        End Try
        Return Desc
    End Function
End Class

