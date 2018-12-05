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
Public Class StockRequistionEmail
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection


    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Function FormatMultipleEmailAddresses(ByVal emailAddresses As String) As String
        Dim delimiters = {","c, ";"c}
        Dim addresses = emailAddresses.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
        Return String.Join(",", addresses)
    End Function

    'Public Function SendStockRequistionEmail() As List(Of String)

    '    Dim _strSQLConn As String = ConfigurationSettings.AppSettings("BOConnectionString")
    '    Dim _strHost As String = ConfigurationSettings.AppSettings("SMTPHost")
    '    Dim _strEnableSsl As String = ConfigurationSettings.AppSettings("EnableSsl")
    '    Dim _strnSubject As String = ConfigurationSettings.AppSettings("Subject")
    '    Dim _strnUserName As String = ConfigurationSettings.AppSettings("SMTPUserName")
    '    Dim _strPassword As String = ConfigurationSettings.AppSettings("SMTPPassword")
    '    Dim _strPort As String = ConfigurationSettings.AppSettings("SMTPPort")
    '    Dim _strStockReqEggEmails As String = ConfigurationSettings.AppSettings("STOCKREQ_EMAIL_EGG")
    '    Dim _strStockReqChickenEmails As String = ConfigurationSettings.AppSettings("STOCKREQ_EMAIL_CHICKEN")

    '    Dim objSQLConn As SqlConnection
    '    Dim objSQLCmd As SqlCommand
    '    Dim objSQLDA As SqlDataAdapter
    '    Dim sQry As String
    '    Dim success As Boolean = False
    '    Dim dtStockRequest As New DataTable
    '    Dim retVal As New List(Of String)


    '    'getting MSSQL DB connection.....
    '    objSQLConn = _objDB.GetSQLConnection
    '    Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
    '    Try

    '        retVal.Add("Process started for retriving stock requisition to send a email")

    '        sQry = "app_GetPendingStockRequestEmail"

    '        objSQLCmd = New SqlCommand(sQry, objSQLConn)
    '        objSQLCmd.CommandType = CommandType.StoredProcedure

    '        objSQLCmd.Transaction = myTrans
    '        objSQLDA = New SqlDataAdapter()
    '        objSQLDA.SelectCommand = objSQLCmd
    '        If dtStockRequest.Rows.Count > 0 Then
    '            dtStockRequest.Rows.Clear()
    '        End If
    '        objSQLDA.Fill(dtStockRequest)

    '        objSQLCmd.Dispose()
    '        objSQLDA.Dispose()






    '        If dtStockRequest.Rows.Count > 0 Then
    '            For Each l As DataRow In dtStockRequest.Rows
    '                retVal.Add("Process started to send a Email")
    '                If l("Email_Sent").ToString() = "N" Then


    '                    retVal.Add("SMTpUserName:" & _strnUserName & "SmtpPassword:" & _strPassword & "SMTPHost" & _strHost)

    '                    Dim mailMessage As MailMessage = New MailMessage
    '                    mailMessage.From = New MailAddress(_strnUserName)
    '                    mailMessage.Subject = "Stock Requisition Doc.Ref.No - " + l("Doc_Ref_No").ToString() + " ( " + l("StockRequisition_ID").ToString + " )"
    '                    mailMessage.Body = PopulateBody(l("Salesrep_Name").ToString(), DateTime.Parse(l("Request_Date").ToString()).ToString("dd-MMM-yyyy HH:mm"), l("Doc_Ref_No").ToString, l("StockRequisition_ID").ToString)
    '                    mailMessage.IsBodyHtml = True

    '                    Dim arEmails As New ArrayList
    '                    Dim s() As String = IIf(l("SalesOrg").ToString() = "EGG", _strStockReqEggEmails.Split(";"), _strStockReqChickenEmails.Split(";"))
    '                    For i As Integer = 0 To s.Length - 1
    '                        ' arEmails.Add(s(i).ToString())
    '                        mailMessage.To.Add(New MailAddress(s(i).ToString()))
    '                    Next
    '                    '  Dim EmailsList As String = String.Join(", ", arEmails)

    '                    ' mailMessage.To.Add(New MailAddress(EmailsList))
    '                    Dim smtp As SmtpClient = New SmtpClient
    '                    ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

    '                    smtp.Host = _strHost
    '                    smtp.EnableSsl = Convert.ToBoolean(_strEnableSsl)
    '                    Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
    '                    NetworkCred.UserName = _strnUserName
    '                    NetworkCred.Password = _strPassword
    '                    smtp.UseDefaultCredentials = True
    '                    smtp.Credentials = NetworkCred
    '                    smtp.Port = Integer.Parse(_strPort)
    '                    Try

    '                        smtp.Send(mailMessage)
    '                        retVal.Add("Process ended to send a email successfully")
    '                        sQry = "UPDATE TBL_Stock_Requisition SET Email_Send='Y' WHERE StockRequisition_ID=@RequestID"

    '                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
    '                        objSQLCmd.CommandType = CommandType.Text
    '                        objSQLCmd.Parameters.AddWithValue("@RequestID", l("StockRequisition_ID").ToString())
    '                        objSQLCmd.Transaction = myTrans
    '                        objSQLCmd.ExecuteNonQuery()
    '                        objSQLCmd.Dispose()
    '                        smtp.Dispose()
    '                    Catch ex As SmtpFailedRecipientException
    '                        retVal.Add("Error occured while sending Email to " & ex.ToString())
    '                        Continue For
    '                    End Try
    '                End If



    '            Next
    '        End If
    '        retVal.Add("Process ended for to send a email")
    '        myTrans.Commit()

    '    Catch ex As Exception
    '        myTrans.Rollback()
    '        retVal.Add(ex.ToString())


    '    Finally
    '        objSQLCmd = Nothing
    '        myTrans.Dispose()
    '        _objDB.CloseSQLConnection(objSQLConn)
    '    End Try

    '    Return retVal
    'End Function
    Public Function SendStockRequistionEmail() As List(Of String)

        Dim _strSQLConn As String = ConfigurationSettings.AppSettings("INT_ConnectionString")
        Dim _strHost As String = ConfigurationSettings.AppSettings("SMTPHost")
        Dim _strEnableSsl As String = ConfigurationSettings.AppSettings("EnableSsl")
        Dim _strnSubject As String = ConfigurationSettings.AppSettings("Subject")
        Dim _strnUserName As String = ConfigurationSettings.AppSettings("SMTPUserName")
        Dim _strPassword As String = ConfigurationSettings.AppSettings("SMTPPassword")
        Dim _strPort As String = ConfigurationSettings.AppSettings("SMTPPort")
        Dim _strStockReqEggEmails As String = ConfigurationSettings.AppSettings("STOCKREQ_EMAIL_EGG")
        Dim _strStockReqChickenEmails As String = ConfigurationSettings.AppSettings("STOCKREQ_EMAIL_CHICKEN")

        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim success As Boolean = False
        Dim dtStockRequest As New DataTable
        Dim retVal As New List(Of String)


        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetINTSQLConnection
        Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
        Try

            retVal.Add("Process started for retriving stock requisition to send a email")

            sQry = "app_GetPendingStockRequestEmail"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Transaction = myTrans
            objSQLDA = New SqlDataAdapter()
            objSQLDA.SelectCommand = objSQLCmd
            If dtStockRequest.Rows.Count > 0 Then
                dtStockRequest.Rows.Clear()
            End If
            objSQLDA.Fill(dtStockRequest)

            objSQLCmd.Dispose()
            objSQLDA.Dispose()


            If dtStockRequest.Rows.Count > 0 Then
                For Each l As DataRow In dtStockRequest.Rows
                    retVal.Add("Process started to send a Email")
                    If l("Email_Sent").ToString() = "N" Then


                        retVal.Add("SMTpUserName:" & _strnUserName & "SmtpPassword:" & _strPassword & "SMTPHost" & _strHost)

                        Dim mailMessage As MailMessage = New MailMessage
                        mailMessage.From = New MailAddress(_strnUserName)
                        mailMessage.Subject = "Stock Requisition Doc.Ref.No - " + l("Doc_Ref_No").ToString() + " ( " + l("StockRequisition_ID").ToString + " )"
                        mailMessage.Body = PopulateBody(l("Salesrep_Name").ToString(), DateTime.Parse(l("Request_Date").ToString()).ToString("dd-MMM-yyyy HH:mm"), l("Doc_Ref_No").ToString, l("StockRequisition_ID").ToString)
                        mailMessage.IsBodyHtml = True

                        Dim arEmails As New ArrayList
                        Dim s() As String = IIf(l("SalesOrg").ToString() = "EGG", _strStockReqEggEmails.Split(";"), _strStockReqChickenEmails.Split(";"))
                        For i As Integer = 0 To s.Length - 1
                            ' arEmails.Add(s(i).ToString())
                            mailMessage.To.Add(New MailAddress(s(i).ToString()))
                        Next
                        '  Dim EmailsList As String = String.Join(", ", arEmails)

                        ' mailMessage.To.Add(New MailAddress(EmailsList))
                        Dim smtp As SmtpClient = New SmtpClient
                        ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

                        smtp.Host = _strHost
                        smtp.EnableSsl = Convert.ToBoolean(_strEnableSsl)
                        Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
                        NetworkCred.UserName = _strnUserName
                        NetworkCred.Password = _strPassword
                        smtp.UseDefaultCredentials = True
                        smtp.Credentials = NetworkCred
                        smtp.Port = Integer.Parse(_strPort)
                        Try

                            smtp.Send(mailMessage)
                            retVal.Add("Process ended to send a email successfully")
                            sQry = "sync_BO_UpdateStockRequisition"

                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.StoredProcedure
                            objSQLCmd.Parameters.AddWithValue("@RequestID", l("StockRequisition_ID").ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            smtp.Dispose()
                        Catch ex As SmtpFailedRecipientException
                            retVal.Add("Error occured while sending Email to " & ex.ToString())
                            Continue For
                        End Try
                    End If



                Next
            End If
            retVal.Add("Process ended for to send a email")
            myTrans.Commit()

        Catch ex As Exception
            myTrans.Rollback()
            retVal.Add(ex.ToString())


        Finally
            objSQLCmd = Nothing
            myTrans.Dispose()
            _objDB.CloseINTSQLConnection(objSQLConn)
        End Try

        Return retVal
    End Function


    Public Shared Function IsValidEmail(ByVal strToCheck As [String]) As Boolean
        Const matchEmailPattern As String = "^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
        Return Regex.IsMatch(strToCheck, matchEmailPattern)
    End Function
    Public Function SendVanunloadEmail() As List(Of String)

        Dim _strSQLConn As String = ConfigurationSettings.AppSettings("BOConnectionString")
        Dim _strHost As String = ConfigurationSettings.AppSettings("SMTPHost")
        Dim _strEnableSsl As String = ConfigurationSettings.AppSettings("EnableSsl")
        Dim _strnSubject As String = ConfigurationSettings.AppSettings("Subject")
        Dim _strnUserName As String = ConfigurationSettings.AppSettings("SMTPUserName")
        Dim _strPassword As String = ConfigurationSettings.AppSettings("SMTPPassword")
        Dim _strPort As String = ConfigurationSettings.AppSettings("SMTPPort")
        Dim _strVanunloadEggEmails As String = ConfigurationSettings.AppSettings("VAN_UNLOAD_EGG")
        Dim _stVanunloadChickenEmails As String = ConfigurationSettings.AppSettings("VAN_UNLOAD_CHICKEN")


        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDA As SqlDataAdapter
        Dim sQry As String
        Dim success As Boolean = False
        Dim dtStockRequest As New DataTable
        Dim retVal As New List(Of String)


        'getting MSSQL DB connection.....
        objSQLConn = _objDB.GetINTSQLConnection
        Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
        Try

            retVal.Add("Process started for retriving van unload to send a email")

            sQry = "app_GetPendingVanUnloadEmail"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure

            objSQLCmd.Transaction = myTrans
            objSQLDA = New SqlDataAdapter()
            objSQLDA.SelectCommand = objSQLCmd
            If dtStockRequest.Rows.Count > 0 Then
                dtStockRequest.Rows.Clear()
            End If
            objSQLDA.Fill(dtStockRequest)

            objSQLCmd.Dispose()
            objSQLDA.Dispose()






            If dtStockRequest.Rows.Count > 0 Then
                For Each l As DataRow In dtStockRequest.Rows
                    retVal.Add("Process started to send a  vanunload Email")
                    If l("Email_Sent").ToString() = "N" Then


                        retVal.Add("SMTpUserName:" & _strnUserName & "SmtpPassword:" & _strPassword & "SMTPHost" & _strHost)

                        Dim mailMessage As MailMessage = New MailMessage
                        mailMessage.From = New MailAddress(_strnUserName)
                        mailMessage.Subject = "Van Unload Doc.Ref.No - " + l("Doc_Ref_No").ToString() + " ( " + l("Transfer_Ref_No").ToString() + " )"

                        mailMessage.Body = PopulateBody1(l("Salesrep_Name").ToString(), DateTime.Parse(l("Transfer_Date").ToString()).ToString("dd-MMM-yyyy HH:mm"), l("Doc_Ref_No").ToString(), l("Transfer_Ref_No").ToString())
                        mailMessage.IsBodyHtml = True

                        Dim arEmails As New ArrayList
                        Dim s() As String = IIf(l("SalesOrg").ToString() = "EGG", _strVanunloadEggEmails.Split(";"), _stVanunloadChickenEmails.Split(";"))
                        For i As Integer = 0 To s.Length - 1
                            ' arEmails.Add(s(i).ToString())
                            mailMessage.To.Add(New MailAddress(s(i).ToString()))
                        Next
                        '  Dim EmailsList As String = String.Join(", ", arEmails)

                        ' mailMessage.To.Add(New MailAddress(EmailsList))
                        Dim smtp As SmtpClient = New SmtpClient
                        ServicePointManager.ServerCertificateValidationCallback = AddressOf AcceptAllCertifications

                        smtp.Host = _strHost
                        smtp.EnableSsl = Convert.ToBoolean(_strEnableSsl)
                        Dim NetworkCred As System.Net.NetworkCredential = New System.Net.NetworkCredential
                        NetworkCred.UserName = _strnUserName
                        NetworkCred.Password = _strPassword
                        smtp.UseDefaultCredentials = True
                        smtp.Credentials = NetworkCred
                        smtp.Port = Integer.Parse(_strPort)
                        Try

                            smtp.Send(mailMessage)
                            retVal.Add("Process ended to send a email successfully")
                            sQry = "sync_BO_UpdateVanLoad"

                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.CommandType = CommandType.StoredProcedure
                            objSQLCmd.Parameters.AddWithValue("@TransferID", l("StockTransfer_ID").ToString())
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                            objSQLCmd.Dispose()
                            smtp.Dispose()
                        Catch ex As SmtpFailedRecipientException
                            retVal.Add("Error occured while sending Email to " & ex.ToString())
                            Continue For
                        End Try
                    End If



                Next
            End If
            retVal.Add("Process ended for to send a van unload email")
            myTrans.Commit()

        Catch ex As Exception
            myTrans.Rollback()
            retVal.Add(ex.ToString())


        Finally
            objSQLCmd = Nothing
            myTrans.Dispose()
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return retVal
    End Function
    Private Function PopulateBody(ByVal Van As String, ByVal RequestDate As String, ERPRefNo As String, SWXRefNo As String) As String
        Dim body As String = String.Empty
        Dim reader As StreamReader = New StreamReader(ConfigurationSettings.AppSettings("Templates"))
        ' Dim reader As StreamReader = New StreamReader(HttpContext.Current.Server.MapPath("~\Templates\EmailTemplate.html"))
        body = reader.ReadToEnd
        body = body.Replace("[VAN]", Van)

        body = body.Replace("[REQUESTDATE]", RequestDate)

        body = body.Replace("[ERPRefNo]", ERPRefNo)

        body = body.Replace("[SWXRefNo]", SWXRefNo)


        Return body
    End Function
    Private Function PopulateBody1(ByVal Van As String, ByVal RequestDate As String, ERPRefNo As String, SWXRefNo As String) As String
        Dim body As String = String.Empty
        Dim reader As StreamReader = New StreamReader(ConfigurationSettings.AppSettings("VanUnloadTemplate"))
        ' Dim reader As StreamReader = New StreamReader(HttpContext.Current.Server.MapPath("~\Templates\EmailTemplate.html"))
        body = reader.ReadToEnd
        body = body.Replace("[VAN]", Van)

        body = body.Replace("[TRANSFERDATE]", RequestDate)

        body = body.Replace("[ERPRefNo]", ERPRefNo)

        body = body.Replace("[SWXRefNo]", SWXRefNo)

        Return body
    End Function
End Class
