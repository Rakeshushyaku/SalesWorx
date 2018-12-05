Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Message
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetAllMessagesByUD(ByRef Err_No As Long, ByRef Err_desc As String, ByVal SubQuery As String, ByVal MsgDate As Date, Mode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If Mode = "1" Then
                'QueryString = String.Format("SELECT DISTINCT A.Message_ID, A.Message_Title,A.Message_Date,A.FSR_Plan_ID,E.Description,(select SalesRep_Name from TBL_FSR where SalesRep_ID In (B.SalesRep_ID)) As SalesRep_Name FROM TBL_Message As A INNER JOIN TBL_Message_Assignment As B ON A.Message_ID=B.Message_ID LEFT OUTER JOIN (SELECT C.FSR_Plan_ID,D.Description FROM TBL_Route_Plan_Default as D INNER JOIN TBL_Route_Plan_FSR As C on C.Default_Plan_ID = D.Default_Plan_ID) AS E ON E.FSR_Plan_ID= A.FSR_Plan_ID WHERE A.Message_Date>='{0} 00:00:00' AND A.Message_Date<='{0} 23:59:59' AND B.SalesRep_ID IN ({1})", CStr(MsgDate), SubQuery)
                QueryString = String.Format("SELECT DISTINCT A.Message_ID as Msg_ID, A.Message_Title as Msg_Title  FROM TBL_Message As A INNER JOIN TBL_Message_Assignment As B ON A.Message_ID=B.Message_ID LEFT OUTER JOIN (SELECT C.FSR_Plan_ID,D.Description FROM TBL_Route_Plan_Default as D INNER JOIN TBL_Route_Plan_FSR As C on C.Default_Plan_ID = D.Default_Plan_ID) AS E ON E.FSR_Plan_ID= A.FSR_Plan_ID WHERE A.Message_Date>='{0} 00:00:00' AND A.Message_Date<='{0} 23:59:59' AND B.SalesRep_ID IN ({1})", CStr(MsgDate), SubQuery)
            Else
                QueryString = String.Format("SELECT DISTINCT CAST(A.Msg_ID AS Nvarchar(100)) AS Msg_ID,Msg_Title FROM TBL_Msg As A INNER JOIN TBL_Msg_Recipients As B ON A.Msg_ID=B.Msg_ID WHERE A.Sent_At>='{0} 00:00:00' AND A.Sent_At<='{0} 23:59:59' AND B.Recipient_ID IN ({1})", CStr(MsgDate), SubQuery)
            End If

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

    Public Function GetIncomingSearchMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String, MessageMode As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dtSearchMsg As New DataTable

        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String
            If MessageMode = "1" Then
                QueryString = String.Format("select A.Rcpt_User_ID,A.Message_ID AS Message_ID, A.Message_Title AS Message_Title, A.Message_Content AS Message_Content,A.Message_Date AS Message_Date, B.SalesRep_Name AS SalesRep_Name from TBL_Incoming_Messages As A, TBL_FSR as B where  A.SalesRep_ID = B.SalesRep_ID AND A.Rcpt_User_ID IN ({1}) {0} order by A.Message_Date Desc, B.SalesRep_Name ASC", QueryStr, _sSearchParams)
            Else
                QueryString = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name AS SalesRep_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date,B.Recipient_ID AS Rcpt_User_ID   from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID    And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL And C.User_ID IN ({1}) {0} order by A.Logged_At Desc, A.Sender_Name ASC", QueryStr, _sSearchParams)
            End If

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

    Public Function GetMessageByFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal MessageID As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            objSQLCmd = New SqlCommand("app_GetFSRMessageByID", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.Add(New SqlParameter("@Message_ID", SqlDbType.NVarChar, 100))
            objSQLCmd.Parameters("@Message_ID").Value = MessageID

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgTbl")
            GetMessageByFSR = MsgDs.Tables("MsgTbl")
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
    Public Function SendMsgByGroup(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sMessageTitle As String, ByVal _sMessageContent As String, ByVal _dMessageDate As Date, ByVal _dMessageExpiryDate As Date, ByVal _sSalesRepID As String, ByVal SenderID As Integer, SenderName As String) As Boolean
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
                objSQLCmd = New SqlCommand("app_InsertMsgbyGroup", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@Title", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@Title").Value = _sMessageTitle
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageContent", SqlDbType.VarChar, 255))
                objSQLCmd.Parameters("@MessageContent").Value = _sMessageContent
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@MessageDate").Value = _dMessageDate
                objSQLCmd.Parameters.Add(New SqlParameter("@ExpiryDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@ExpiryDate").Value = _dMessageExpiryDate
                objSQLCmd.Parameters.Add(New SqlParameter("@SenderID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SenderID").Value = SenderID

                objSQLCmd.Parameters.Add(New SqlParameter("@SenderName", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@SenderName").Value = SenderName

                objSQLCmd.Parameters.Add(New SqlParameter("@RecipientList", SqlDbType.VarChar, 8000))
                objSQLCmd.Parameters("@RecipientList").Value = _sSalesRepID

                objSQLCmd.ExecuteNonQuery()

                objSQLCmd.Dispose()
                sRetBool = True
            Else
                Error_No = 76001
                Error_Desc = "No recipient selected."
            End If
        Catch ex As Exception
            Error_No = 76001
            Error_Desc = ex.Message & sQry
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetBool
    End Function
    Public Function SendMsgByFSR(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sMessageTitle As String, ByVal _sMessageContent As String, ByVal _dMessageDate As Date, ByVal _dMessageExpiryDate As Date, ByVal _sSalesRepID As String, ByVal SenderID As Integer, SenderName As String) As Boolean
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
                objSQLCmd = New SqlCommand("app_InsertMsg", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@Title", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@Title").Value = _sMessageTitle
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageContent", SqlDbType.VarChar, 255))
                objSQLCmd.Parameters("@MessageContent").Value = _sMessageContent
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@MessageDate").Value = _dMessageDate
                objSQLCmd.Parameters.Add(New SqlParameter("@ExpiryDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@ExpiryDate").Value = _dMessageExpiryDate
                objSQLCmd.Parameters.Add(New SqlParameter("@SenderID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SenderID").Value = SenderID

                objSQLCmd.Parameters.Add(New SqlParameter("@SenderName", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@SenderName").Value = SenderName

                objSQLCmd.Parameters.Add(New SqlParameter("@RecipientList", SqlDbType.VarChar, 8000))
                objSQLCmd.Parameters("@RecipientList").Value = _sSalesRepID

                objSQLCmd.ExecuteNonQuery()

                objSQLCmd.Dispose()
                sRetBool = True
            Else
                Error_No = 76001
                Error_Desc = "No recipient selected."
            End If
        Catch ex As Exception
            Error_No = 76001
            Error_Desc = ex.Message & sQry
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetBool
    End Function
    Public Function RecallMsg(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sMessageTitle As String, ByVal _sMessageContent As String, ByVal _dMessageDate As Date, ByVal _dMessageExpiryDate As Date, ByVal _sSalesRepID As String, ByVal SenderID As Integer, SenderName As String) As Boolean
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
                objSQLCmd = New SqlCommand("app_RecallMsg", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.Parameters.Add(New SqlParameter("@Title", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@Title").Value = _sMessageTitle
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageContent", SqlDbType.VarChar, 255))
                objSQLCmd.Parameters("@MessageContent").Value = _sMessageContent
                objSQLCmd.Parameters.Add(New SqlParameter("@MessageDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@MessageDate").Value = _dMessageDate
                objSQLCmd.Parameters.Add(New SqlParameter("@ExpiryDate", SqlDbType.DateTime))
                objSQLCmd.Parameters("@ExpiryDate").Value = _dMessageExpiryDate
                objSQLCmd.Parameters.Add(New SqlParameter("@SenderID", SqlDbType.BigInt))
                objSQLCmd.Parameters("@SenderID").Value = SenderID

                objSQLCmd.Parameters.Add(New SqlParameter("@SenderName", SqlDbType.VarChar, 100))
                objSQLCmd.Parameters("@SenderName").Value = SenderName

                objSQLCmd.Parameters.Add(New SqlParameter("@RecipientList", SqlDbType.VarChar, 8000))
                objSQLCmd.Parameters("@RecipientList").Value = _sSalesRepID

                objSQLCmd.ExecuteNonQuery()

                objSQLCmd.Dispose()
                sRetBool = True
            Else
                Error_No = 76001
                Error_Desc = "No recipient selected."
            End If
        Catch ex As Exception
            Error_No = 76001
            Error_Desc = ex.Message & sQry
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetBool
    End Function
    Public Function GetSearchMessageByFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            ''Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL And A.Sender_ID IN ({1}) {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL  {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgOutbox")

            GetSearchMessageByFSR = MsgDs
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
    Public Function GetSearchMessageByFSR_v2(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL  {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgOutbox")

            GetSearchMessageByFSR_v2 = MsgDs
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
    Public Function GetSearchMessageFromFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal UserID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            'Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL And C.User_ID IN ({1}) {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, UserID)
            Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL  {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, UserID)

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgInbox")

            GetSearchMessageFromFSR = MsgDs
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
    Public Function GetSearchMessageFromFSR_v2(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal UserID As Integer) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand


        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            Dim QueryString As String = String.Format("select CAST(A.Msg_ID as nvarchar(100)) AS Message_ID, A.Msg_Title AS  Message_Title,A.Sender_Name, A.Msg_Body AS Message_Content,A.Logged_At AS Message_Date, C.UserName AS SalesRep_Name,CASE WHEN Read_At IS NULL THEN 'N' ELSE 'Y' END AS  Message_Read,ISNULL((SELECT Msg_Body  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID),'N/A')AS  Message_Reply, (SELECT Logged_At  FROM TBl_Msg WHERE Msg_ID= B.Reply_Msg_ID) AS Reply_Date from TBL_Msg As A, TBL_Msg_Recipients as B, TBL_User as C where A.Msg_ID = B.Msg_ID And B.Recipient_ID = C.User_ID AND A.Parent_Msg_ID IS NULL  {0} order by A.Logged_At  Asc, C.UserName ASC", _sSearchParams, UserID)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "MsgInbox")

            GetSearchMessageFromFSR_v2 = MsgDs
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
    Public Function LoadMSgRecipients(ByRef Error_No As Long, ByRef Error_Desc As String, UID As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim dt As New DataTable
        Try
            objSQLConn = _objDB.GetSQLConnection
            objSQLCmd = New SqlCommand("SELECT User_ID  as UserID ,(SELECT TOP 1 SalesRep_Name FROM TBL_FSR WHERE SalesRep_ID =A.SalesRep_ID)AS UserName FROM TBL_User AS A WHERE Is_SS='N' AND SalesRep_ID  IN(SELECT SalesRep_ID FROM dbo.app_GetControlInfo(@UID))  order by Username ASC ", objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@UID", UID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception
            Error_No = "92674"
            Error_Desc = ex.Message

            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return dt
    End Function
End Class
