Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports System.Configuration

Public Class DAL_VanManage

    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")

    Private dtvan As New DataTable


    Public Function SaveVan(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String, Sales_Org_ID As String, Van_Name As String, Emp_Code As String, Emp_Name As String, Emp_Phone As String, Prefix_No As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String


        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection

        Try
            sQry = "app_SaveVan"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Van_Org_ID", Van_Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Sales_Org_ID", Sales_Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Van_Name", Van_Name)
            objSQLCmd.Parameters.AddWithValue("@Emp_Code", Emp_Code)
            objSQLCmd.Parameters.AddWithValue("@Emp_Name", Emp_Name)
            objSQLCmd.Parameters.AddWithValue("@Emp_Phone", Emp_Phone)
            objSQLCmd.Parameters.AddWithValue("@Prefix_No", Prefix_No)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            retVal = True

        Catch ex As Exception

            Error_No = 13351
            Error_Desc = String.Format("Error while saving record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SaveVan = retVal
    End Function

    Public Function UpdateVan(ByRef Error_No As Long, ByRef Error_Desc As String, Van_Org_ID As String, Sales_Org_ID As String, Van_Name As String, Emp_Code As String, Emp_Name As String, Emp_Phone As String, Prefix_No As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String


        Dim retVal As Boolean = False
        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection

        Try
            sQry = "app_UpdateVan"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@Van_Org_ID", Van_Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Sales_Org_ID", Sales_Org_ID)
            objSQLCmd.Parameters.AddWithValue("@Van_Name", Van_Name)
            objSQLCmd.Parameters.AddWithValue("@Emp_Code", Emp_Code)
            objSQLCmd.Parameters.AddWithValue("@Emp_Name", Emp_Name)
            objSQLCmd.Parameters.AddWithValue("@Emp_Phone", Emp_Phone)
            objSQLCmd.Parameters.AddWithValue("@Prefix_No", Prefix_No)

            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()

            retVal = True

        Catch ex As Exception

            Error_No = 23155
            Error_Desc = String.Format("Error while updating record: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        UpdateVan = retVal
    End Function



    Public Function CheckVancodeDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van_org_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            Dim qry As String


            qry = "SELECT COUNT(*) FROM TBL_FSR WHERE SalesRep_Number= '" + Van_org_ID + "'"

            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(qry, objSQLConn)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function

    Public Function CheckEmpcodeDuplicate(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal empcode As String, ByVal Van_org_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            Dim qry As String

            qry = "SELECT COUNT(*) FROM TBL_Van_Info WHERE Emp_Code= '" + empcode + "'"
            If Van_org_ID <> "" Then
                qry = qry & " AND Van_Org_ID<>'" & Van_org_ID & "'"
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(qry, objSQLConn)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
    Public Function DeleteVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van_Org_ID As String, ByVal Sales_Org_ID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sRetVal As Boolean = False
        Try

            Dim Dr As SqlDataReader
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_DeleteVan", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Van_Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Van_Org_ID").Value = Van_Org_ID
            objSQLCmd.Parameters.Add(New SqlParameter("@Sales_Org_ID", SqlDbType.VarChar))
            objSQLCmd.Parameters("@Sales_Org_ID").Value = Sales_Org_ID

            Dr = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Dr.Read Then
                If Val(Dr(0).ToString) = 0 Then
                    sRetVal = True
                Else
                    Err_Desc = "The Van cannot be deleted as associated user is active ."
                    sRetVal = False

                End If
            End If
            Dr.Close()

            objSQLCmd.Dispose()
        Catch ex As Exception
            Err_No = "74015"
            Err_Desc = ex.Message
            sRetVal = False
            Throw ex
        Finally
            objSQLCmd = Nothing
            If objSQLConn IsNot Nothing Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        DeleteVan = sRetVal
    End Function

    Public Function LoadExportVanTemplate() As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Dim Ds As New DataSet
        Dim sQry As String

        Try

            objSQLConn = _objDB.GetSQLConnection

            sQry = "app_ExportVanTemplate"



            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLDa = New SqlDataAdapter(objSQLCmd)

            objSQLDa.Fill(Ds)
            objSQLCmd.Dispose()





            Return Ds
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return Ds
    End Function

    Public Function SearchVanGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try
            dtvan.Clear()
            Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID"

            If FilterBy = "Organization" Then
                If FilterValue = "0" Then
                    FilterValue = ""
                End If
                Query = "SELECT H.Description as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID INNER JOIN  TBL_Org_CTL_H H on I.Sales_Org_ID =H.ORG_HE_ID  WHERE H.Description Like '%" + FilterValue.ToUpper() + "%' ORDER BY F.Sync_Timestamp DESC"
                ' Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID WHERE  Sales_Org_ID LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY F.Sync_Timestamp DESC"
            ElseIf FilterBy = "Van Code" Then
                Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID WHERE  Van_Org_ID LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY F.Sync_Timestamp DESC"
            ElseIf FilterBy = "Van Name" Then
                Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID WHERE  SalesRep_Name LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY F.Sync_Timestamp DESC"
            ElseIf FilterBy = "Emp Code" Then
                Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName,* FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID WHERE  Emp_Code LIKE '%" + FilterValue.ToUpper() + "%' ORDER BY F.Sync_Timestamp DESC"
            Else
                Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName,SalesRep_Name as VanName, * FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID ORDER BY F.Sync_Timestamp DESC "
            End If
            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtvan)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtvan
    End Function
    Public Function GetVanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Van_ID As String, ByVal Org_ID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDA As SqlDataAdapter

        Dim Query As String = ""
        Try


            Query = "SELECT (SELECT Description  from TBL_Org_CTL_H where ORG_HE_ID=I.Sales_Org_ID) as OrgName, SalesRep_Name as VanName,* FROM  TBL_FSR F INNER JOIN TBL_Van_Info I ON F.SalesRep_Number =I.Van_Org_ID WHERE  Van_Org_ID = '" + Van_ID + "' ORDER BY F.Sync_Timestamp DESC"

            objSQLConn = _objDB.GetSQLConnection
            objSQLDA = New SqlDataAdapter(Query, objSQLConn)
            objSQLDA.Fill(dtvan)
            objSQLDA.Dispose()
        Catch ex As Exception
            Err_No = "740022"
            Err_Desc = ex.Message
            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtvan
    End Function
    Public Function ValidOrgnization(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal org_ID As String) As Boolean
        Dim success As Boolean = False
        Dim objSQLConn As SqlConnection
        Dim objSQLCMD As SqlCommand
        Dim Rcnt As Integer
        Try
            Dim qry As String

            qry = "SELECT COUNT(*) FROM TBL_Org_CTL_H WHERE ORG_HE_ID= '" + org_ID + "'"

            objSQLConn = _objDB.GetSQLConnection
            objSQLCMD = New SqlCommand(qry, objSQLConn)
            Rcnt = Convert.ToInt32(objSQLCMD.ExecuteScalar())
            objSQLCMD.Dispose()
            If Rcnt > 0 Then
                success = True
            End If
        Catch ex As Exception

            Throw ex
        Finally
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return success
    End Function
End Class
