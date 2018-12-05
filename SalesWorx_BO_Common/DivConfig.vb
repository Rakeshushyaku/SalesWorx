
Public Class DivConfig
#Region "Private Fields"

    Private _sDiv_Config_ID As Integer = 0
    Private _sOrg_ID As String = ""
    Private _sAllow_Manual_FOC As String = ""
    Private _sOdo_Reading_At_Visit As String = ""
    Private _sPrintFormat As String = ""
    Private _sAdvance_PDC_Posting As Short = 0
    Private _sSync_Timestamp As DateTime = DateTime.Parse("1900-1-1 12:00")
    Private _sDC_Optional As New ArrayList
    Private _sCollection_Output_Folder As String
    Private _sCRLimit As Decimal = 0
    Dim objDivConfig As New DAL.DAL_DivConfig
    Private _sCustomerSequence As Decimal = 0
    Private _sDeliveryCustoffTime As String = ""
    Private _sTRN As String = ""
#End Region
#Region "Accessors"

    Public Property CustomerSequence() As Decimal
        Get
            Return Me._sCustomerSequence
        End Get

        Set(ByVal value As Decimal)
            Me._sCustomerSequence = value
        End Set
    End Property
    Public Property CreditNoteLimit() As Decimal
        Get
            Return Me._sCRLimit
        End Get

        Set(ByVal value As Decimal)
            Me._sCRLimit = value
        End Set
    End Property

    Public Property Div_Config_ID() As Integer
        Get
            Return Me._sDiv_Config_ID
        End Get

        Set(ByVal value As Integer)
            Me._sDiv_Config_ID = value
        End Set
    End Property


    Public Property Org_ID() As String
        Get
            Return Me._sOrg_ID
        End Get

        Set(ByVal value As String)
            Me._sOrg_ID = value
        End Set
    End Property


    Public Property Allow_Manual_FOC() As String
        Get
            Return Me._sAllow_Manual_FOC
        End Get

        Set(ByVal value As String)
            Me._sAllow_Manual_FOC = value
        End Set
    End Property


    Public Property Odo_Reading_At_Visit() As String
        Get
            Return Me._sOdo_Reading_At_Visit
        End Get

        Set(ByVal value As String)
            Me._sOdo_Reading_At_Visit = value
        End Set
    End Property


    Public Property Advance_PDC_Posting() As Short
        Get
            Return Me._sAdvance_PDC_Posting
        End Get

        Set(ByVal value As Short)
            Me._sAdvance_PDC_Posting = value
        End Set
    End Property

    Public Property Collection_Output_Folder() As String
        Get
            Return Me._sCollection_Output_Folder
        End Get

        Set(ByVal value As String)
            Me._sCollection_Output_Folder = value
        End Set
    End Property

    Public Property Sync_Timestamp() As DateTime
        Get
            Return Me._sSync_Timestamp
        End Get

        Set(ByVal value As DateTime)
            Me._sSync_Timestamp = value
        End Set
    End Property

    Public Property DC_Optional() As ArrayList
        Get
            Return _sDC_Optional
        End Get
        Set(ByVal value As ArrayList)
            _sDC_Optional = value
        End Set
    End Property

    Public Property PrintFormat() As String
        Get
            Return Me._sPrintFormat
        End Get

        Set(ByVal value As String)
            Me._sPrintFormat = value
        End Set
    End Property

    Public Property TRN() As String
        Get
            Return Me._sTRN
        End Get

        Set(ByVal value As String)
            Me._sTRN = value
        End Set
    End Property

    Public Property DeliveryCustoffTime() As String
        Get
            Return Me._sDeliveryCustoffTime
        End Get

        Set(ByVal value As String)
            Me._sDeliveryCustoffTime = value
        End Set
    End Property

#End Region

    Public Function InsertDivConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CreatedBy As Integer) As Boolean
        Try
            Return objDivConfig.InsertDivConfig(Err_No, Err_Desc, _sOrg_ID, _sAllow_Manual_FOC, _sOdo_Reading_At_Visit, _sAdvance_PDC_Posting, _sDC_Optional, _sPrintFormat, _sCollection_Output_Folder, _sCRLimit, CreatedBy, _sCustomerSequence, _sTRN, _sDeliveryCustoffTime)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateDivConfig(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UpdatedBy As Integer) As Boolean
        Try
            Return objDivConfig.UpdateDivConfig(Err_No, Err_Desc, _sOrg_ID, _sAllow_Manual_FOC, _sOdo_Reading_At_Visit, _sAdvance_PDC_Posting, _sDiv_Config_ID, _sDC_Optional, _sPrintFormat, _sCollection_Output_Folder, _sCRLimit, UpdatedBy, _sCustomerSequence, _sTRN, _sDeliveryCustoffTime)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDivisionalConfiguration(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable
        Try
            Return objDivConfig.GetDivConfig(Err_No, Err_Desc, Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function CheckDivControl(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As Boolean
        Try
            Return objDivConfig.CheckDivControlExist(Err_No, Err_Desc, OrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllDivisions(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Dim dt As New DataTable
            dt = objDivConfig.GetAllDivisions(Err_No, Err_Desc)
            Dim dr As DataRow
            dr = dt.NewRow()
            dr(0) = "0"
            dr(1) = "-- Select a Organization --"
            dr(2) = "-- Select a Organization --"
            dt.Rows.InsertAt(dr, 0)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteDivisionalConfiguration(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Div_Config_ID_collection As String) As Boolean
        Try
            Return objDivConfig.DeleteDivConfig(Err_No, Err_Desc, Div_Config_ID_collection)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
