Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Net.Http
Imports System.Collections.Specialized
Public Class PushNotifcation
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Public Function GetUTCDateTime() As DateTime
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As DateTime
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select GETUTCDATE()"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)

            iRowsAffected = DateTime.Parse(objSQLCmd.ExecuteScalar())

        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetUTCDateTime = iRowsAffected
    End Function
    Public Class Result
        Public status As String, ProcessResponse As String, RequestID As String, RequestTimestamp As String, NotificationIDList() As String

    End Class
    Public Class StatusResult
        Public NotificationID As String, StatusList() As PostStatus, status As String, ProcessResponse As String, RequestID As String, RequestTimestamp As String


    End Class
    Public Class PostStatus
        Public RecipientID As String, Status As String, ProcessResponse As String, PostedAt As String

    End Class
    Public Function HashString(ByVal StringToHash As String, ByVal HachKey As String) As String
        Dim myEncoder As New System.Text.UTF8Encoding
        Dim Key() As Byte = myEncoder.GetBytes(HachKey)
        Dim Text() As Byte = myEncoder.GetBytes(StringToHash)
        Dim myHMACSHA1 As New System.Security.Cryptography.HMACSHA1(Key)
        Dim HashCode As Byte() = myHMACSHA1.ComputeHash(Text)
        Dim hash As String = Replace(BitConverter.ToString(HashCode), "-", "")
        Return hash.ToLower
    End Function

    Public Shared Function GenerateHMAC(ByVal inputData As String, ByVal key As String) As String
        Using oHMAC As System.Security.Cryptography.HMAC = System.Security.Cryptography.HMAC.Create("HMACSHA256")
            oHMAC.Key = UTF8Encoding.UTF8.GetBytes(key)
            Return Convert.ToBase64String(oHMAC.ComputeHash(Encoding.UTF8.GetBytes(inputData))).ToUpper()
        End Using
    End Function

    Public Function SendNotifications() As List(Of String)

        Dim _strSQLConn As String = ConfigurationSettings.AppSettings("BOConnectionString")
        Dim ServerTimeStamp As DateTime = GetUTCDateTime()
        Dim PrivateKey As String = CStr(ConfigurationSettings.AppSettings("PUSHPRIVATEKEY"))
        Dim _strReceipientDevice As String = CStr(ConfigurationSettings.AppSettings("PUSHRECEIPIENTDEVICE"))
        Dim _strOtherInfo As String = ""
        Dim _strUserKey As String = CStr(ConfigurationSettings.AppSettings("PUSHUSERKEY"))
        Dim _StrURL As String = CStr(ConfigurationSettings.AppSettings("PUSHNOTIFYURL"))

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim success As Boolean = False
        Dim dtNotification As New DataTable
        Dim retVal As New List(Of String)


        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection
        Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
        Try

            retVal.Add("Process started for retrieving pending push notifications")

            sQry = "app_GetPendingPushNotifications"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Transaction = myTrans
            objSQLDA = New SqlDataAdapter()
            objSQLDA.SelectCommand = objSQLCmd
            If dtNotification.Rows.Count > 0 Then
                dtNotification.Rows.Clear()
            End If
            objSQLDA.Fill(dtNotification)

            objSQLCmd.Dispose()
            objSQLDA.Dispose()

            Dim PostStatus As HttpResponseMessage
            Dim sOtherData As String
            If dtNotification.Rows.Count > 0 Then
                For Each l As DataRow In dtNotification.Rows
                    retVal.Add("Process started to send a notification to Notification ID :  " & l("Notification_ID").ToString() & "  Recipient : " & l("Recipient").ToString())

                    Try
                        Dim jss = New JavaScriptSerializer()

                        Using client As New System.Net.Http.HttpClient
                            client.BaseAddress = New Uri(_StrURL)
                            ' client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded")

                            'Dim reqparm As New Specialized.NameValueCollection
                            'reqparm.Add("ResponseFormat", "JSON")
                            'reqparm.Add("Recipient", l("Recipient").ToString())
                            'reqparm.Add("RecipientType", l("RecipientType").ToString())
                            'reqparm.Add("Message", l("Message").ToString())
                            'reqparm.Add("ContentType", l("ContentType").ToString())
                            'reqparm.Add("OtherInfo", _strOtherInfo)
                            'reqparm.Add("LoggedAt", ServerTimeStamp.ToString("yyyyMMddHHmmss"))
                            'reqparm.Add("UKey", _strUserKey)



                            sOtherData = String.Format("{0}", l("Message"))


                            Dim StringToHash As String = "Recipient=" & l("Recipient").ToString() & "&RecipientType=" & l("RecipientType").ToString() & "&Message=" & l("Title").ToString() & "&ContentType=" & l("ContentType").ToString() & "&OtherInfo=" & sOtherData & "&LoggedAt=" & ServerTimeStamp.ToString("yyyyMMddHHmmss") & "&UKey=" & _strUserKey


                            Dim _strCData As String = GenerateHMAC(StringToHash, PrivateKey)
                            '  reqparm.Add("CData", _strCData)

                            '  Dim responsebytes = client.UploadValues(_StrURL, "POST", reqparm)


                            Dim keyValues = New List(Of KeyValuePair(Of String, String))()
                            keyValues.Add(New KeyValuePair(Of String, String)("ResponseFormat", "JSON"))
                            keyValues.Add(New KeyValuePair(Of String, String)("Recipient", l("Recipient").ToString()))
                            keyValues.Add(New KeyValuePair(Of String, String)("RecipientType", l("RecipientType").ToString()))
                            keyValues.Add(New KeyValuePair(Of String, String)("Message", l("Title").ToString()))
                            keyValues.Add(New KeyValuePair(Of String, String)("ContentType", l("ContentType").ToString()))
                            keyValues.Add(New KeyValuePair(Of String, String)("OtherInfo", sOtherData))
                            keyValues.Add(New KeyValuePair(Of String, String)("LoggedAt", ServerTimeStamp.ToString("yyyyMMddHHmmss")))
                            keyValues.Add(New KeyValuePair(Of String, String)("UKey", _strUserKey))
                            keyValues.Add(New KeyValuePair(Of String, String)("CData", _strCData))





                            'sOtherData = String.Format(" {0}", l("Message"))


                            'Dim StringToHash As String = "Recipient=" & l("Recipient").ToString() & "&RecipientType=" & l("RecipientType").ToString() & "&Message=" & l("Title").ToString() & "&ContentType=" & l("ContentType").ToString() & "&OtherInfo=" & sOtherData & "&LoggedAt=" & ServerTimeStamp.ToString("yyyyMMddHHmmss") & "&UKey=" & _strUserKey


                            'Dim _strCData As String = GenerateHMAC(StringToHash, PrivateKey)
                            ''  reqparm.Add("CData", _strCData)

                            ''  Dim responsebytes = client.UploadValues(_StrURL, "POST", reqparm)


                            'Dim keyValues = New List(Of KeyValuePair(Of String, String))()
                            'keyValues.Add(New KeyValuePair(Of String, String)("ResponseFormat", "JSON"))
                            'keyValues.Add(New KeyValuePair(Of String, String)("Recipient", l("Recipient").ToString()))
                            'keyValues.Add(New KeyValuePair(Of String, String)("RecipientType", l("RecipientType").ToString()))
                            'keyValues.Add(New KeyValuePair(Of String, String)("Message", l("Title").ToString()))
                            'keyValues.Add(New KeyValuePair(Of String, String)("ContentType", l("ContentType").ToString()))
                            'keyValues.Add(New KeyValuePair(Of String, String)("OtherInfo", sOtherData))
                            'keyValues.Add(New KeyValuePair(Of String, String)("LoggedAt", ServerTimeStamp.ToString("yyyyMMddHHmmss")))
                            'keyValues.Add(New KeyValuePair(Of String, String)("UKey", _strUserKey))
                            'keyValues.Add(New KeyValuePair(Of String, String)("CData", _strCData))


                            Dim content As HttpContent = New FormUrlEncodedContent(keyValues)
                            'content.Headers.Add("Content-Type", "application/x-www-form-urlencoded")


                            PostStatus = client.PostAsync(_StrURL, content).Result

                            PostStatus.EnsureSuccessStatusCode()


                            If PostStatus.IsSuccessStatusCode = True Then

                                Dim responsebytes As Byte() = PostStatus.Content.ReadAsByteArrayAsync().Result
                                Dim response = (New System.Text.UTF8Encoding).GetString(responsebytes)


                                Dim Output = jss.Deserialize(Of Result)(response)

                                Dim statuscode As String = PostStatus.StatusCode
                                Dim ProcessResponse As String = Output.ProcessResponse
                                Dim PostRefID As String = Output.NotificationIDList(0).ToString()

                                If Output.status = "True" Then
                                    sQry = "UPDATE TBL_Push_Recipients SET Status='Y',Processed_At=GETDATE(),Last_Updated_At=GETDATE(),Proc_Response=@ProcessResponse,Posting_Ref_ID=@PostRefID WHERE Notification_ID=@NotificationID AND Recipient_Ref=@Recipient"
                                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                                    objSQLCmd.CommandType = CommandType.Text
                                    objSQLCmd.Parameters.AddWithValue("@NotificationID", l("Notification_ID").ToString())
                                    objSQLCmd.Parameters.AddWithValue("@ProcessResponse", IIf(ProcessResponse Is Nothing, DBNull.Value, ProcessResponse))
                                    objSQLCmd.Parameters.AddWithValue("@PostRefID", PostRefID)
                                    objSQLCmd.Parameters.AddWithValue("@Recipient", l("Recipient").ToString())
                                    objSQLCmd.Transaction = myTrans
                                    objSQLCmd.ExecuteNonQuery()
                                    objSQLCmd.Dispose()
                                End If

                            End If

                        End Using

                        retVal.Add("Process ended to send a notification  to Notification ID :  " & l("Notification_ID").ToString() & " Recipient : " & l("Recipient").ToString())
                    Catch hre As HttpRequestException
                        retVal.Add(hre.Message & " Notification ID :  " & l("Notification_ID").ToString() & "  Recipient:" & l("Recipient").ToString())

                        If PostStatus.StatusCode = 500 Then 'Internal server error
                            retVal.Add("Internal server error occured to send a notification " & l("Recipient").ToString())
                            sQry = "UPDATE TBL_Push_Recipients SET Status='F',Last_Updated_At=GETDATE(),Proc_Response=@ProcessResponse WHERE Notification_ID=@NotificationID AND Recipient_Ref=@Recipient"
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Parameters.AddWithValue("@NotificationID", l("Notification_ID").ToString())
                            objSQLCmd.Parameters.AddWithValue("@ProcessResponse", PostStatus.ReasonPhrase.ToString())
                            objSQLCmd.Parameters.AddWithValue("@Recipient", l("Recipient").ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            Continue For
                        ElseIf PostStatus.StatusCode = 400 Then 'bad request error
                            retVal.Add("Bad request error occured to send a notification" & l("Recipient").ToString())
                            sQry = "UPDATE TBL_Push_Recipients SET Status='F',Last_Updated_At=GETDATE(),Proc_Response=@ProcessResponse WHERE Notification_ID=@NotificationID AND Recipient_Ref=@Recipient"
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Parameters.AddWithValue("@NotificationID", l("Notification_ID").ToString())
                            objSQLCmd.Parameters.AddWithValue("@ProcessResponse", PostStatus.ReasonPhrase.ToString())
                            objSQLCmd.Parameters.AddWithValue("@Recipient", l("Recipient").ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            Continue For
                        Else
                            Continue For
                        End If

                    End Try
                Next
            End If


            myTrans.Commit()

        Catch hre As HttpRequestException
            myTrans.Rollback()
            retVal.Add(hre.ToString())


        Finally
            objSQLCmd = Nothing
            myTrans.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return retVal
    End Function


    Public Function NotificationStatus() As List(Of String)
        Dim _strSQLConn As String = ConfigurationSettings.AppSettings("BOConnectionString")

        Dim ServerTimeStamp As DateTime = GetUTCDateTime()
        Dim PrivateKey As String = CStr(ConfigurationSettings.AppSettings("PUSHPRIVATEKEY"))
        Dim _strReceipientDevice As String = CStr(ConfigurationSettings.AppSettings("PUSHRECEIPIENTDEVICE"))
        Dim _strOtherInfo As String = ""
        Dim _strUserKey As String = CStr(ConfigurationSettings.AppSettings("PUSHUSERKEY"))
        Dim _StrURL As String = CStr(ConfigurationSettings.AppSettings("PUSHNOTIFYURL"))

        _StrURL = _StrURL.Replace("SendNotification", "NotificationStatus")

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim success As Boolean = False
        Dim dtNotification As New DataTable
        Dim retVal As New List(Of String)


        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetSQLConnection
        Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
        Try

            retVal.Add("Process started for retrieving pending push notifications")

            sQry = "app_GetPendingPostedNotifications"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Transaction = myTrans
            objSQLDA = New SqlDataAdapter()
            objSQLDA.SelectCommand = objSQLCmd
            If dtNotification.Rows.Count > 0 Then
                dtNotification.Rows.Clear()
            End If
            objSQLDA.Fill(dtNotification)

            objSQLCmd.Dispose()
            objSQLDA.Dispose()
            Dim GetStatus As HttpResponseMessage
            Dim jss = New JavaScriptSerializer()
            If dtNotification.Rows.Count > 0 Then
                For Each l As DataRow In dtNotification.Rows
                    retVal.Add("Process started to check the status for Post Ref ID: " & l("PostRefID").ToString())

                    Try
                        Using client As New System.Net.Http.HttpClient
                            _StrURL = _StrURL.Replace("SendNotification", "NotificationStatus")
                            _StrURL = _StrURL & "/" & l("PostRefID").ToString()
                            client.BaseAddress = New Uri(_StrURL)

                            ' client.Encoding = Encoding.UTF8
                            '  Dim responsebytes = client.DownloadString(_StrURL)
                            '   Dim response = (New System.Text.UTF8Encoding).GetString(responsebytes)


                            '   Dim Output = jss.Deserialize(Of StatusResult)(responsebytes)






                            GetStatus = client.GetAsync(_StrURL).Result

                            GetStatus.EnsureSuccessStatusCode()


                            If GetStatus.IsSuccessStatusCode = True Then

                                Dim responsebytes As Byte() = GetStatus.Content.ReadAsByteArrayAsync().Result
                                Dim response = (New System.Text.UTF8Encoding).GetString(responsebytes)


                                Dim Output = jss.Deserialize(Of StatusResult)(response)

                                If Output.status = "True" Then

                                    Dim OpenStatusCnt As Integer = 0

                                    For Each i As PostStatus In Output.StatusList

                                        If i.Status = "N" Then
                                            OpenStatusCnt = OpenStatusCnt + 1
                                        End If
                                        sQry = "app_UpdatePushNotificationStatus"
                                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
                                        objSQLCmd.CommandType = CommandType.StoredProcedure
                                        objSQLCmd.Parameters.AddWithValue("@PostRefID", l("PostRefID").ToString())
                                        objSQLCmd.Parameters.AddWithValue("@RecipientID", i.RecipientID)
                                        objSQLCmd.Parameters.AddWithValue("@Status", i.Status)
                                        objSQLCmd.Parameters.AddWithValue("@ProcessResponse", i.ProcessResponse)
                                        objSQLCmd.Parameters.AddWithValue("@PostedAt", i.PostedAt)
                                        objSQLCmd.Transaction = myTrans
                                        objSQLCmd.ExecuteNonQuery()
                                        objSQLCmd.Dispose()
                                    Next

                                    If OpenStatusCnt = 0 Then
                                        sQry = "UPDATE TBL_Push_Recipients SET Status='S',Last_Updated_At=GETDATE() WHERE Posting_Ref_ID=@PostRefID"
                                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
                                        objSQLCmd.CommandType = CommandType.Text
                                        objSQLCmd.Parameters.AddWithValue("@PostRefID", l("PostRefID").ToString())
                                        objSQLCmd.Transaction = myTrans
                                        objSQLCmd.ExecuteNonQuery()
                                        objSQLCmd.Dispose()
                                    End If

                                End If


                            End If

                        End Using

                        retVal.Add("Process ended to check the status for Post Ref ID " & l("PostRefID").ToString())
                    Catch hre As HttpRequestException
                        retVal.Add(hre.Message & l("PostRefID").ToString())

                        If GetStatus.StatusCode = 500 Then 'Internal server error
                            retVal.Add("Internal server error occured to  check the status for Post Ref ID " & l("PostRefID").ToString())
                            retVal.Add("Page not found error occured to  check the status for Post Ref ID " & l("PostRefID").ToString())
                            sQry = "UPDATE TBL_Push_Recipients SET Status='F',Last_Updated_At=GETDATE(),Proc_Response=@ProcessResponse WHERE Posting_Ref_ID=@PostRefID"
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Parameters.AddWithValue("@PostRefID", l("PostRefID").ToString())
                            objSQLCmd.Parameters.AddWithValue("@ProcessResponse", GetStatus.ReasonPhrase.ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            Continue For

                        ElseIf GetStatus.StatusCode = 400 Then 'bad request error
                            retVal.Add("Bad request error occured to  check the status for Post Ref ID " & l("PostRefID").ToString())
                            sQry = "UPDATE TBL_Push_Recipients SET Status='F',Last_Updated_At=GETDATE(),Proc_Response=@ProcessResponse WHERE Posting_Ref_ID=@PostRefID"
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.Text
                            objSQLCmd.Parameters.AddWithValue("@PostRefID", l("PostRefID").ToString())
                            objSQLCmd.Parameters.AddWithValue("@ProcessResponse", GetStatus.ReasonPhrase.ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            Continue For
                        Else
                            Continue For
                        End If
                    End Try

                Next
            End If


            myTrans.Commit()

        Catch hre As HttpRequestException

            myTrans.Rollback()
            retVal.Add(hre.ToString())


        Finally
            objSQLCmd = Nothing
            myTrans.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return retVal
    End Function

End Class
