Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class Message
    Private MsgDate As Date
    Private _sMessageTitle As String
    Private _sMessageContent As String
    Private _dMessageDate As Date
    Private _dMessageExpiryDate As Date
    Private _sSalesRepID As String
    Private _MessageID As Integer
    Private SenderID As Integer
    Private _sSender_ID As Integer
    Dim ObjDALMsg As New DAL_Message
    Private _sMessageStatus As String
    Public Property Sender_ID() As String
        Get
            Sender_ID = _sSender_ID
        End Get
        Set(ByVal Value As String)
            _sSender_ID = Value
        End Set
    End Property
    Public Property MessageStatus() As String
        Get
            MessageStatus = _sMessageStatus
        End Get
        Set(ByVal Value As String)
            _sMessageStatus = Value
        End Set
    End Property
    Public Property MsgDateProp() As Date
        Set(ByVal value As Date)
            MsgDate = value
        End Set
        Get
            Return MsgDate
        End Get
    End Property
    Public Property MessageTitle() As String
        Get
            MessageTitle = _sMessageTitle
        End Get
        Set(ByVal Value As String)
            _sMessageTitle = Value
        End Set
    End Property

    Public Property MessageContent() As String
        Get
            MessageContent = _sMessageContent
        End Get
        Set(ByVal Value As String)
            _sMessageContent = Value
        End Set
    End Property

    Public Property MessageDate() As Date
        Get
            MessageDate = _dMessageDate
        End Get
        Set(ByVal Value As Date)
            _dMessageDate = Value
        End Set
    End Property

    Public Property MessageExpiryDate() As Date
        Get
            MessageExpiryDate = _dMessageExpiryDate
        End Get
        Set(ByVal Value As Date)
            _dMessageExpiryDate = Value
        End Set
    End Property

    Public Property SalesRepID() As String
        Get
            SalesRepID = _sSalesRepID
        End Get
        Set(ByVal Value As String)
            _sSalesRepID = Value
        End Set
    End Property
    Public Property MessageID() As Integer
        Set(ByVal Value As Integer)
            _MessageID = Value
        End Set
        Get
            MessageID = _MessageID
        End Get
    End Property
    Public WriteOnly Property SenderIDProp() As Integer
        Set(ByVal value As Integer)
            SenderID = value
        End Set
    End Property
    Function GetAllMessagesByUD(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String, Mode As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDALMsg.GetAllMessagesByUD(Err_No, Err_Desc, UD_SUB_QRY, Me.MsgDate, Mode)

            Dim dr As DataRow
            dr = TempDataTable.NewRow()
            dr(0) = "0"
            dr(1) = "Select Title"
            TempDataTable.Rows.InsertAt(dr, 0)

            'Dim tempDBVal As Object
            'Dim mTitle As String
            'Dim msgID As Integer
            '' Dim RepID As Int64
            'Dim Description As String
            'Dim MsgDate As Date
            '' Dim eDate As Date

            'Dim MyRow As DataRow
            'MyDT.Columns.Add(New DataColumn("Msg_ID", _
            '    GetType(Int32)))
            'MyDT.Columns.Add(New DataColumn("Msg_Title", _
            '    GetType(String)))


            'For i As Integer = 0 To TempDataTable.Rows.Count() - 1
            '    msgID = CStr(TempDataTable.Rows(i).Item(0))

            '    tempDBVal = TempDataTable.Rows(i).Item(3)
            '    If (IsDBNull(tempDBVal)) Then
            '        tempDBVal = TempDataTable.Rows(i).Item(1)
            '        mTitle = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
            '    Else
            '        tempDBVal = TempDataTable.Rows(i).Item(1)
            '        Description = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
            '        MsgDate = TempDataTable.Rows(i).Item(2)
            '        tempDBVal = TempDataTable.Rows(i).Item(4)
            '        mTitle = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
            '        ' mTitle = Description & " ( " & mTitle & " ) " & " @ " & MsgDate.ToString("H:mm")
            '        mTitle = Description & " - " & IIf(IsDBNull(TempDataTable.Rows(i).Item(5)), "NA", TempDataTable.Rows(i).Item(5)) & " - " & mTitle & " - " & MsgDate.ToString("H:mm")
            '    End If

            '    MyRow = MyDT.NewRow()
            '    MyRow(0) = msgID
            '    MyRow(1) = mTitle
            '    MyDT.Rows.Add(MyRow)
            'Next
            Return TempDataTable
        Catch ex As Exception
            Err_Desc = ex.Message
            Err_No = "74059"
        Finally
            TempDataTable = Nothing
        End Try

    End Function
    Function GetSalesRepList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Query As String) As DataTable
        Return ObjDALMsg.GetSalesRepList(Err_No, Err_Desc, Query)
        ''Dim dt As New DataTable
        ''dt = ObjDALMsg.GetSalesRepList(Err_No, Err_Desc, Query)
        ''Dim dr As DataRow
        ''dr = dt.NewRow()
        ''dr(0) = "0"
        ''dr(1) = ""
        ''dt.Rows.InsertAt(dr, 0)
        ''Return dt
    End Function
    Function SendMessage(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDALMsg.SendMessage(Err_No, Err_Desc, Me._sMessageTitle, Me._sMessageContent, Me._dMessageDate, Me._dMessageExpiryDate, Me._sSalesRepID.Substring(0, _sSalesRepID.Length - 1), _sSender_ID)
    End Function
    Function SendMsgByFSR(ByRef Err_No As Long, ByRef Err_Desc As String, SenderID As Integer, SenderName As String) As Boolean
        Return ObjDALMsg.SendMsgByFSR(Err_No, Err_Desc, Me._sMessageTitle, Me._sMessageContent, Me._dMessageDate, Me._dMessageExpiryDate, Me._sSalesRepID.Substring(0, _sSalesRepID.Length - 1), SenderID, SenderName)
    End Function
    Function RecallMsg(ByRef Err_No As Long, ByRef Err_Desc As String, SenderID As Integer, SenderName As String) As Boolean
        Return ObjDALMsg.RecallMsg(Err_No, Err_Desc, Me._sMessageTitle, Me._sMessageContent, Me._dMessageDate, Me._dMessageExpiryDate, Me._sSalesRepID.Substring(0, _sSalesRepID.Length - 1), SenderID, SenderName)
    End Function
    Function SendMsgByGroup(ByRef Err_No As Long, ByRef Err_Desc As String, SenderID As Integer, SenderName As String) As Boolean
        Return ObjDALMsg.SendMsgByGroup(Err_No, Err_Desc, Me._sMessageTitle, Me._sMessageContent, Me._dMessageDate, Me._dMessageExpiryDate, Me._sSalesRepID.Substring(0, _sSalesRepID.Length - 1), SenderID, SenderName)
    End Function
    Function GetMessage(ByRef Err_No As Long, ByRef Err_Desc As String)
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDALMsg.GetMessage(Err_No, Err_Desc, Me.MessageID)
            Dim tempDBVal As Object
            'Dim mTitle As String
            'Dim msgID As Integer
            'Dim RepID As Int64
            ' Dim Description As String
            ' Dim sDate As Date
            ' Dim eDate As Date


            If TempDataTable IsNot Nothing Then
                For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                    If (i = 0) Then
                        tempDBVal = TempDataTable.Rows(i).Item(0)
                        _sMessageTitle = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(1)
                        _sMessageContent = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(2)
                        _dMessageDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(4)
                        _dMessageExpiryDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                    End If
                    _sSalesRepID += TempDataTable.Rows(i).Item(3) & ","
                Next
            End If

        Catch ex As Exception
            Err_Desc = ex.Message
            Err_No = "74060"
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Function GetSearchMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALMsg.GetSearchMessage(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    
    Function GetSearchIncomingMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String, MessageMode As String) As DataTable
        Try
            Return ObjDALMsg.GetIncomingSearchMessage(Err_No, Err_Desc, UD_SUB_QRY, SearchQuery, MessageMode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetMessagebyFSR(ByRef Err_No As Long, ByRef Err_Desc As String, MessageID As String)
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDALMsg.GetMessageByFSR(Err_No, Err_Desc, MessageID)
            Dim tempDBVal As Object
            'Dim mTitle As String
            'Dim msgID As Integer
            'Dim RepID As Int64
            ' Dim Description As String
            ' Dim sDate As Date
            ' Dim eDate As Date
            _sSalesRepID = Nothing

            If TempDataTable IsNot Nothing Then
                For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                    If (i = 0) Then
                        tempDBVal = TempDataTable.Rows(i).Item(0)
                        _sMessageTitle = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(1)
                        _sMessageContent = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(2)
                        _dMessageDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(4)
                        _dMessageExpiryDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(5)
                        _sMessageStatus = IIf(IsDBNull(tempDBVal), "Y", tempDBVal)
                    End If
                    _sSalesRepID += TempDataTable.Rows(i).Item(3) & ","
                Next
            End If
        Catch ex As Exception
            Err_Desc = ex.Message
            Err_No = "74060"
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Function GetMessageForReview(ByRef Err_No As Long, ByRef Err_Desc As String, MessageID As String)
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDALMsg.GetMessageByFSR(Err_No, Err_Desc, MessageID)
            Dim tempDBVal As Object
            'Dim mTitle As String
            'Dim msgID As Integer
            'Dim RepID As Int64
            ' Dim Description As String
            ' Dim sDate As Date
            ' Dim eDate As Date
            _sSalesRepID = Nothing

            If TempDataTable IsNot Nothing Then
                For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                    If (i = 0) Then
                        tempDBVal = TempDataTable.Rows(i).Item(0)
                        _sMessageTitle = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(1)
                        _sMessageContent = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(2)
                        _dMessageDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(4)
                        _dMessageExpiryDate = IIf(IsDBNull(tempDBVal), "1/1/1900", tempDBVal)
                        tempDBVal = TempDataTable.Rows(i).Item(5)
                        _sMessageStatus = IIf(IsDBNull(tempDBVal), "Y", tempDBVal)
                    End If
                    _sSalesRepID += TempDataTable.Rows(i).Item("Recipient_ID") & ","
                Next
            End If
        Catch ex As Exception
            Err_Desc = ex.Message
            Err_No = "74060"
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Function GetSearchMessageFromFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, UserID As Integer) As DataSet
        Try
            Return ObjDALMsg.GetSearchMessageFromFSR(Err_No, Err_Desc, SearchQuery, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSearchMessageByFSR(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALMsg.GetSearchMessageByFSR(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSearchMessageFromFSR_v2(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, UserID As Integer) As DataSet
        Try
            Return ObjDALMsg.GetSearchMessageFromFSR_v2(Err_No, Err_Desc, SearchQuery, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSearchMessageByFSR_v2(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchQuery As String, ByVal UD_SUB_QRY As String) As DataSet
        Try
            Return ObjDALMsg.GetSearchMessageByFSR_v2(Err_No, Err_Desc, SearchQuery, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function LoadMSgRecipients(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SearchParams As String) As DataTable
        Try
            Return ObjDALMsg.LoadMSgRecipients(Err_No, Err_Desc, SearchParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
