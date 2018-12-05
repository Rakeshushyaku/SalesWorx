Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Message
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetAllMessagesByUD(ByRef Err_No As Long, ByRef Err_desc As String, ByVal SubQuery As String, ByVal MsgDate As Date) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT DISTINCT A.Message_ID, A.Message_Title,A.Message_Date,A.FSR_Plan_ID,E.Description,(select SalesRep_Name from TBL_FSR where SalesRep_ID In (B.SalesRep_ID)) As SalesRep_Name FROM TBL_Message As A INNER JOIN TBL_Message_Assignment As B ON A.Message_ID=B.Message_ID LEFT OUTER JOIN (SELECT C.FSR_Plan_ID,D.Description FROM TBL_Route_Plan_Default as D INNER JOIN TBL_Route_Plan_FSR As C on C.Default_Plan_ID = D.Default_Plan_ID) AS E ON E.FSR_Plan_ID= A.FSR_Plan_ID WHERE A.Message_Date>='{0} 00:00:00' AND A.Message_Date<='{0} 23:59:59' AND B.SalesRep_ID IN ({1})", CStr(MsgDate), SubQuery)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MessageTbl")

            GetAllMessagesByUD = MsgDs.Tables("MessageTbl")
            objSQLCmd.Dispose()
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74056"
            Err_desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function GetSalesRepList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR where SalesRep_ID IN ({0}) ORDER BY SalesRep_Name ASC", QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "SalesRepTbl")

            GetSalesRepList = MsgDs.Tables("SalesRepTbl")
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74058"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function
    Public Function SendMessage(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sMessageTitle As String, ByVal _sMessageContent As String, ByVal _dMessageDate As Date, ByVal _dMessageExpiryDate As Date, ByVal _sSalesRepID As String, ByVal SenderID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim sRetBool As Boolean = False
        Dim iRowsAffected As Integer = 0
        '  Dim sMessageID As Integer
        Try
            If _sSalesRepID <> "" Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand("app_InsertMessage", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@VarMessageTitle", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@VarMessageTitle").Value = _sMessageTitle
                objSQLCmd.Parameters.Add(New SqlParameter("@VarMessageContent", SqlDbType.VarChar, 255))
                objSQLCmd.Parameters("@VarMessageContent").Value = _sMessageContent
                objSQLCmd.Parameters.Add(New SqlParameter("@VarMessageDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@VarMessageDate").Value = _dMessageDate
                objSQLCmd.Parameters.Add(New SqlParameter("@VarExpiryDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@VarExpiryDate").Value = _dMessageExpiryDate
                objSQLCmd.Parameters.Add(New SqlParameter("@Sender_ID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@Sender_ID").Value = SenderID
                Dim parameter As SqlParameter = objSQLCmd.Parameters.Add(New SqlParameter("@MessageId", SqlDbType.Int))
                parameter.Direction = ParameterDirection.Output

                Dim MsgID As Integer
                objSQLCmd.ExecuteNonQuery()
                MsgID = parameter.Value

                objSQLCmd.Dispose()

                If MsgID > 0 Then

                    Dim arrMsgList() As String
                    arrMsgList = Split(_sSalesRepID, ",")

                    Dim i As Integer = 0
                    iRowsAffected = 0

                    For i = 0 To arrMsgList.GetLength(0) - 1
                        If arrMsgList(i).Trim() <> "" Then
                            sQry = String.Format("insert into TBL_Message_Assignment(Message_ID, SalesRep_ID,Message_Read) values('{0}','{1}','N')", MsgID, arrMsgList(i).Trim())
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                        End If
                    Next
                    sRetBool = True
                Else
                    Error_No = 76001
                    Error_Desc = "Unable to post message."
                End If
            Else
                Error_No = 76001
                Error_Desc = "No SalesReps specified."
            End If
        Catch ex As Exception
            Error_No = 76001
            Error_Desc = ex.Message & sQry
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SendMessage = sRetBool
    End Function
    Public Function GetMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal MessageID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetMessageByID", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Message_ID", SqlDbType.Int))
            objSQLCmd.Parameters("@Message_ID").Value = MessageID

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgTbl")
            GetMessage = MsgDs.Tables("MsgTbl")
            objSQLCmd.Dispose()
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74059"
            Err_Desc = ex.Message
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
    End Function

    Public Function GetSearchMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select A.Message_ID AS Message_ID, A.Message_Title AS Message_Title, A.Message_Content AS Message_Content,A.Message_Date AS Message_Date, C.SalesRep_Name AS SalesRep_Name, B.Message_Read AS Message_Read,B.Message_Reply AS Message_Reply, b.Reply_Date AS Reply_Date from TBL_Message As A, TBL_Message_Assignment as B, TBL_FSR as C where A.Message_ID = B.Message_ID And B.SalesRep_ID = C.SalesRep_ID And C.SalesRep_ID IN ({1}) {0} order by A.Message_Date Asc, SalesRep_Name ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgTbl")

            GetSearchMessage = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

    End Function

    Public Function GetIncomingSearchMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dtSearchMsg As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("select A.Rcpt_User_ID,A.Message_ID AS Message_ID, A.Message_Title AS Message_Title, A.Message_Content AS Message_Content,A.Message_Date AS Message_Date, B.SalesRep_Name AS SalesRep_Name from TBL_Incoming_Messages As A, TBL_FSR as B where  A.SalesRep_ID = B.SalesRep_ID AND A.Rcpt_User_ID IN ({1}) {0} order by A.Message_Date Asc, B.SalesRep_Name ASC", QueryStr, _sSearchParams)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dtSearchMsg)
            objSQLCmd.Dispose()
            SqlAd.Dispose()
        Catch ex As Exception
            Err_No = "77061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dtSearchMsg
    End Function


End Class
