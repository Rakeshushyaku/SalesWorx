Imports System.Data.SqlClient
Imports SalesWorx.BO.Common

Partial Public Class DummyLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _objUserAccess As UserAccess
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim retVal As Boolean = False
        Try

            'getting MSSQL DB connection.....
            objSQLConn = New SqlConnection(ConfigurationSettings.AppSettings("SQLConnString"))
            objSQLConn.Open()
            sQry = "Select A.User_ID,A.User_Type_ID,A.SalesRep_ID,A.Is_SS,B.SalesRep_ID As VanId,C.SalesRep_Name,D.SalesRep_Name As Van_Name from TBL_User A left join TBL_User_FSR_Map B on A.User_ID= B.User_ID inner join TBL_FSR C on A.SalesRep_ID = C.SalesRep_ID left outer join TBL_FSR D on B.SalesRep_ID = D.SalesRep_ID  where A.UserName='" & "barham" & "' and A.Password='" & "barham" & "'"
            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            If objSQLDR.HasRows Then
                Dim tempDBVal As Object
                Dim sUserID As String
                Dim sSalesRepID As String
                _objUserAccess = New UserAccess
                _objUserAccess.AssignedSalesReps = New ArrayList
                _objUserAccess.VanName = New ArrayList
                While objSQLDR.Read
                    If IsNothing(sUserID) Then
                        tempDBVal = objSQLDR.GetValue(0)
                        _objUserAccess.UserID = IIf(IsDBNull(tempDBVal), "NULL", tempDBVal)
                        _objUserAccess.UserType = objSQLDR.GetValue(1)
                        _objUserAccess.IsSS = objSQLDR.GetValue(3)
                        _objUserAccess.SalesRepID = objSQLDR.GetValue(2)

                        If Not objSQLDR.GetValue(4) Is DBNull.Value Then
                            _objUserAccess.AssignedSalesReps.Add(objSQLDR.GetValue(4))
                        End If

                        _objUserAccess.SalesRep_Name = objSQLDR.GetValue(5)


                        If Not objSQLDR.GetValue(6) Is DBNull.Value Then
                            _objUserAccess.VanName.Add(objSQLDR.GetValue(6))
                        End If


                    End If
                End While
                sUserID = Nothing
                sSalesRepID = Nothing
                tempDBVal = Nothing
                retVal = True
                Session("USER_ACCESS") = Nothing
                Session("USER_ACCESS") = _objUserAccess
            Else
            End If


            objSQLDR.Close()
            objSQLCmd.Dispose()


        Catch ex As Exception
        Finally
        End Try

      
    End Sub

End Class