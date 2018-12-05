Imports SalesWorx.BO.DAL
Imports System.Data.SqlClient

Public Class RoutePlan
    Private _defaultPlanId As Integer
    Private _Description As String
    Private _SDate As Date
    Private _EDate As Date
    Private _StartDate As Date
    Private _EndDate As Date
    Private _Site As String
    Private _NoOfWorkingDays As Integer
    Private _day As Integer
    Private _Start_Time As Date
    Private _End_Time As Date
    Private _DayType As Char
    Private _UserComments As String
    Private _CanPlanVisits As Char
    Private _DefPlanDetailID As Integer
    Private _SalesRepID As Integer
    Private _FSRPlanID As Integer
    Private _CustomerID As Integer
    Private _UserSiteID As Integer
    Private _FSRPlanDetailID As Integer
    Private _DayRef As Integer
    Private _Message As String
    Private _MessageID As Integer
    Private _SenderID As Integer
    Private _ApprovedBy As Integer
    Private _Sequence As Integer
    Private _AllowOptimization As String

    Private _HolidayDate As String
    Private _HolidayDescription As String

    Dim ObjDAlRoutePlanner As New DAL_RoutePlan

    Public Property HolidayDescription() As String
        Set(ByVal value As String)
            _HolidayDescription = value
        End Set
        Get
            Return _HolidayDescription
        End Get
    End Property

    Public Property HolidayDate() As String
        Set(ByVal value As String)
            _HolidayDate = value
        End Set
        Get
            Return _HolidayDate
        End Get
    End Property

    Public Property Description() As String
        Set(ByVal value As String)
            _Description = value
        End Set
        Get
            Return _Description
        End Get
    End Property

    Public Property SDate() As String
        Set(ByVal value As String)
            _SDate = value
        End Set
        Get
            Return _SDate
        End Get
    End Property

    Public Property EDate() As String
        Set(ByVal value As String)
            _EDate = value
        End Set
        Get
            Return _EDate
        End Get
    End Property
    Public Property Site() As String
        Set(ByVal value As String)
            _Site = value
        End Set
        Get
            Return _Site
        End Get
    End Property
    Public WriteOnly Property NoOfWDays() As Integer
        Set(ByVal value As Integer)
            _NoOfWorkingDays = value
        End Set
    End Property
    Public WriteOnly Property DefPlanId() As Integer
        Set(ByVal value As Integer)
            _defaultPlanId = value
        End Set
    End Property
    Public WriteOnly Property FSRPlanId() As Integer
        Set(ByVal value As Integer)
            _FSRPlanID = value
        End Set
    End Property
    Public WriteOnly Property day() As Integer
        Set(ByVal value As Integer)
            _day = value
        End Set
    End Property
    Public WriteOnly Property SenderID() As Integer
        Set(ByVal value As Integer)
            _SenderID = value
        End Set
    End Property
    Public WriteOnly Property StartTime() As Date
        Set(ByVal value As Date)
            _Start_Time = value
        End Set
    End Property
    Public WriteOnly Property End_Time() As Date
        Set(ByVal value As Date)
            _End_Time = value
        End Set
    End Property
    Public WriteOnly Property DayType() As Char
        Set(ByVal value As Char)
            _DayType = value
        End Set
    End Property
    Public WriteOnly Property UserComments() As String
        Set(ByVal value As String)
            _UserComments = value
        End Set
    End Property
    Public WriteOnly Property CanPlanVisits() As Char
        Set(ByVal value As Char)
            _CanPlanVisits = value
        End Set
    End Property
    Public WriteOnly Property StartDate() As Date
        Set(ByVal value As Date)
            _StartDate = value
        End Set
    End Property
    Public WriteOnly Property EndDate() As Date
        Set(ByVal value As Date)
            _EndDate = value
        End Set
    End Property
    Public WriteOnly Property DefPlanDetailID() As Integer
        Set(ByVal value As Integer)
            _DefPlanDetailID = value
        End Set
    End Property
    Public WriteOnly Property SalesRepID() As Integer
        Set(ByVal value As Integer)
            _SalesRepID = value
        End Set
    End Property
    Public Property CustomerID() As Integer
        Set(ByVal value As Integer)
            _CustomerID = value
        End Set
        Get
            Return _CustomerID
        End Get
    End Property
    Public WriteOnly Property UserSiteID() As Integer
        Set(ByVal value As Integer)
            _UserSiteID = value
        End Set
    End Property
    Public WriteOnly Property Sequence() As Integer
        Set(ByVal value As Integer)
            _Sequence = value
        End Set
    End Property
    Public WriteOnly Property AllowOptimization() As String
        Set(ByVal value As String)
            _AllowOptimization = value
        End Set
    End Property
    Public WriteOnly Property FSRPlanDetailIDProp() As Integer
        Set(ByVal value As Integer)
            _FSRPlanDetailID = value
        End Set
    End Property
    Public WriteOnly Property DayRef() As Integer
        Set(ByVal value As Integer)
            _DayRef = value
        End Set
    End Property
    Public WriteOnly Property Message() As String
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property
    Public WriteOnly Property MessageID() As Integer
        Set(ByVal value As Integer)
            _MessageID = value
        End Set
    End Property

    Public WriteOnly Property ApprovedBy() As Integer
        Set(ByVal value As Integer)
            _ApprovedBy = value
        End Set
    End Property

    Public Function SaveHoliday(ByRef Err_No As Long, ByRef Err_Desc As String, UserID As String) As Boolean
        Try
            Return ObjDAlRoutePlanner.SaveHoliday(Err_No, Err_Desc, Me._HolidayDate, _HolidayDescription, UserID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteHoliday(ByRef Err_No As Long, ByRef Err_Desc As String, ID As String) As Boolean
        Try
            Return ObjDAlRoutePlanner.DeleteHoliday(Err_No, Err_Desc, ID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function DeleteDefaultPlanID(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlRoutePlanner.DeleteDefaultPlan(Err_No, Err_Desc, Me._defaultPlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetRoutePlanID(ByRef Err_No As Long, ByRef Err_Desc As String) As ArrayList
        Try
            Return ObjDAlRoutePlanner.GetRoutePlanID(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ShowExistingPlan(ByVal Site As String, ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.ShowExistingPlan(Site, Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDefaultPlanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, PlanID As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetDefaultPlanDetails(Err_No, Err_Desc, PlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsPlanUsed(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return ObjDAlRoutePlanner.IsPlanUsed(Err_No, Err_Desc, _defaultPlanId)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function GetDefaultPlan(ByRef Err_No As Long, ByRef Err_Desc As String)
        Dim ObjDAlRoutePlan As New DAL_RoutePlan

        Try
            Dim PlanRow As DataRow
            PlanRow = ObjDAlRoutePlan.GetDefaultPlan(Err_No, Err_Desc, Me._defaultPlanId)
            Me.Description = PlanRow.Item(0)
            Me.SDate = PlanRow.Item(1)
            Me.EDate = PlanRow.Item(2)
        Catch ex As Exception
            Err_No = "74006"
            Err_Desc = ex.Message
            Throw ex
        Finally
            ObjDAlRoutePlan = Nothing
        End Try
    End Function
    Public Function GetDefaultCalendarDetails(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjDAlRoutePlanner.GetDefaultCalendarDetails(Err_No, Err_Desc, Me._defaultPlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDefaultCalendarDetails_new(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjDAlRoutePlanner.GetDefaultCalendarDetails_New(Err_No, Err_Desc, Me._defaultPlanId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsValidDateRange() As Boolean
        Try
            Return ObjDAlRoutePlanner.IsValidDateRange(_StartDate, _EndDate, _defaultPlanId, _Site)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetConnection() As SqlConnection
        Return (ObjDAlRoutePlanner.GetConnection())
    End Function
    Public Function InsertDefaultPlan(ByRef objRecords As ArrayList, ByRef Err_No As Long, ByRef Err_Desc As String) As Integer
        Try
            Return ObjDAlRoutePlanner.InsertDefaultPlan(objRecords, Err_No, Err_Desc, Description, SDate, EDate, _NoOfWorkingDays, _Site)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub CloseConnection(ByRef ObjCloseConn As SqlConnection)
        ObjDAlRoutePlanner.CloseConnection(ObjCloseConn)
    End Sub
    Public Function InsertDefaultPlanDetails(ByRef Process_Results As ArrayList, ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.InsertDefaultPlanDetails(Process_Results, Err_No, Err_Desc, _defaultPlanId, _Start_Time, _End_Time, _day, _DayType, _UserComments, _CanPlanVisits, SqlConn, sqlcomm, transaction)
    End Function
    Public Function UpdateDefaultPlanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.UpdateDefaultPlanDetails(Err_No, Err_Desc, _Start_Time, _End_Time, _day, _DayType, _UserComments, _CanPlanVisits, _DefPlanDetailID, SqlConn, sqlcomm, transaction)
    End Function
    Function ShowSalesRepsByUD(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UD_SUB_QRY As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.ShowSalesRepsByUD(Err_No, Err_Desc, UD_SUB_QRY)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetAllDefaultPlans(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetAllDefaultPlans(Err_No, Err_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function ShowDefPlans(ByRef Err_No As Long, ByRef Err_desc As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowDefPlans(Err_No, Err_desc, Me._Site, Me._SalesRepID)
            Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim isAssigned As String
            Dim SiteId As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("Default_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("Details", _
                GetType(String)))


            MyRow = MyDT.NewRow()
            MyRow(0) = 0
            MyRow(1) = "  -- Select an existing plan -- "
            MyDT.Rows.Add(MyRow)

            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                tempDBVal = TempDataTable.Rows(i).Item(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sStartDate = TempDataTable.Rows(i).Item(2)
                sEndDate = TempDataTable.Rows(i).Item(3)
                SiteId = TempDataTable.Rows(i).Item(4)

                'isAssigned = tempDBVal & " (" & sStartDate.Day & "/" & sStartDate.Month & "/" & sStartDate.Year & " - " & sEndDate.Day & "/" & sEndDate.Month & "/" & sEndDate.Year & ") "
                isAssigned = tempDBVal & " [ " & SiteId & " ] "

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = isAssigned
                MyDT.Rows.Add(MyRow)
            Next
        Catch ex As Exception
            Err_No = "74017"
            Err_desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Public Function SetCommentPanelVisibility() As Boolean
        Try
            Return ObjDAlRoutePlanner.SetCommentPanelVisibility(Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetFSRDetails(ByRef Err_No As Long, ByRef Err_Desc As String) As DataSet
        Try
            Return ObjDAlRoutePlanner.GetFSRDetails(Err_No, Err_Desc, Me._FSRPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetCustomerList(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCustomerList(Error_No, Error_Desc, Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Function GetCustomerListForRoutePlan(ByRef Error_No As Long, ByRef Error_Desc As String, Optional ByVal SearchQry As String = "", Optional ByVal SearchValue As String = "") As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCustomerListForRoutePlan(Error_No, Error_Desc, Me._SalesRepID, SearchQry, SearchValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCommentsByFSR(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCommentsByFSR(Err_No, Err_Desc, Me._FSRPlanID)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Function GetMoveCopyDays(ByRef Err_No As Long, ByRef Err_desc As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.GetMoveCopyDays(Err_No, Err_desc, Me._defaultPlanId, Me._SDate, Me._EDate)
            Dim tempDBVal As Object
            Dim DayID As Integer
            Dim sStartDate As Date
            'Dim sEndDate As Date
            Dim DateStr As String
            Dim LDate As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("Date_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("DateStr", _
                GetType(String)))

            sStartDate = Me.SDate

            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                DayID = CStr(TempDataTable.Rows(i).Item(0))
                tempDBVal = CStr(TempDataTable.Rows(i).Item(0))

                DateStr = DayID & "-" & sStartDate.Month & "-" & sStartDate.Year
                LDate = sStartDate.Month & "-" & DayID & "-" & sStartDate.Year

                MyRow = MyDT.NewRow()
                MyRow(0) = DayID
                MyRow(1) = DateStr
                If Not (_DayRef = DayID) And DateTime.Parse(LDate) > Today Then
                    MyDT.Rows.Add(MyRow)
                End If
            Next
        Catch ex As Exception
            Err_No = "74027"
            Err_desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function

    Public Function GetHolidayList(ByRef Err_No As Long, ByRef Err_Desc As String, SearchDateFrom As String, SearchDateTo As String, Description As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetHolidayList(Err_No, Err_Desc, SearchDateFrom, SearchDateTo, Description)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetClassList(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetClassList(Err_No, Err_Desc, Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function



    Public Function GetCustomerAddress2(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCustomerAddress2(Err_No, Err_Desc, Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerCity(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCustomerCity(Err_No, Err_Desc, Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerNo(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetCustomerNo(Err_No, Err_Desc, Me._SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function InsertApprovalComments(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef SqlCmd As SqlCommand, ByRef transaction As SqlTransaction) As Integer
        Return ObjDAlRoutePlanner.InsertApprovalComments(Err_No, Err_Desc, Me._FSRPlanID, Me._Message, Me._SenderID, SqlConn, SqlCmd, transaction)
    End Function
    Public Function AssignMessage(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef SqlCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.AssignMessage(Err_No, Err_Desc, Me._MessageID, Me._SalesRepID, "N", SqlConn, SqlCmd, transaction)
    End Function
    Function ApproveRoutePlan(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.ApproveRoutePlan(Err_No, Err_Desc, "Y", Me._ApprovedBy, Me._FSRPlanID)
    End Function
    Public Function InsertFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String) As Integer
        Try
            Return ObjDAlRoutePlanner.InsertFSRPlan(Err_No, Err_Desc, Me._SalesRepID, Me._defaultPlanId, "N")
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetOffDays() As String
        Return ObjDAlRoutePlanner.GetOffDays()
    End Function
    Public Function GetSequence(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction, ByVal SalesRep_ID As String) As String
        Return ObjDAlRoutePlanner.GetSequence(Err_No, Err_Desc, Me._FSRPlanID, Me._day, SalesRep_ID, SqlConn, sqlcomm, transaction)
    End Function
    Public Function InsertFSRPlanDetails(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.InsertFSRPlanDetails(Err_No, Err_Desc, Me._FSRPlanID, Me._day, Me._CustomerID, Me._UserSiteID, Me._Start_Time, Me._End_Time, SqlConn, sqlcomm, transaction, Me._DayType, Me._UserComments, Me._Sequence, Me._AllowOptimization)
    End Function
    Public Function InsertFSRPlanDetailsbyDay(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef sqlcomm As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.InsertFSRPlanDetailsbyDay(Err_No, Err_Desc, Me._FSRPlanID, Me._day, Me._CustomerID, Me._UserSiteID, Me._Start_Time, Me._End_Time, SqlConn, sqlcomm, transaction, Me._DayType, Me._UserComments, Me._Sequence, Me._AllowOptimization)
    End Function
    Public Function UpdateFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByRef SqlConn As SqlConnection, ByRef SqlCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.UpdateFSRPLan(Err_No, Err_Desc, Me._FSRPlanID, "U", SqlConn, SqlCmd, transaction)
    End Function
    Function DeleteRoutePlanByFSRID(ByRef err_no As Long, ByRef Err_desc As String, ByRef SqlConn As SqlConnection, ByRef SqlCmd As SqlCommand, ByRef transaction As SqlTransaction) As Boolean
        Return ObjDAlRoutePlanner.DeleteRoutePlanByFSRID(err_no, Err_desc, Me._FSRPlanID, Me._day, SqlConn, SqlCmd, transaction)
    End Function
    Public Function DeleteFSRPlanID(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.DeleteFSRPlan(Err_No, Err_Desc, Me._FSRPlanID)
    End Function
    Function ShowFSRPlans(ByRef Err_No As Long, ByRef Err_desc As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowFSRPlans(Err_No, Err_desc, Me._SalesRepID)
            Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim isAssigned As String
            Dim AppStat As Char
            Dim DefPlanID As Integer
            'Dim ApprovedBy As Integer
            Dim SiteName As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("Details", _
                GetType(String)))
            MyDT.Columns.Add(New DataColumn("Approved", _
                GetType(Char)))
            MyDT.Columns.Add(New DataColumn("DefPlanID", _
               GetType(Integer)))
            'MyDT.Columns.Add(New DataColumn("Approved_By", _
            '   GetType(Integer)))

            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                tempDBVal = TempDataTable.Rows(i).Item(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sStartDate = TempDataTable.Rows(i).Item(2)
                sEndDate = TempDataTable.Rows(i).Item(3)
                AppStat = TempDataTable.Rows(i).Item(4)
                DefPlanID = TempDataTable.Rows(i).Item(5)
                'ApprovedBy = TempDataTable.Rows(i).Item(6)
                SiteName = CStr(TempDataTable.Rows(i).Item(6))

                'isAssigned = tempDBVal & " (" & sStartDate.Day & "/" & sStartDate.Month & "/" & sStartDate.Year & " - " & sEndDate.Day & "/" & sEndDate.Month & "/" & sEndDate.Year & ") "
                isAssigned = tempDBVal & " [" & SiteName & "]"

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = isAssigned
                MyRow(2) = AppStat
                MyRow(3) = DefPlanID
                ' MyRow(4) = ApprovedBy
                MyDT.Rows.Add(MyRow)

            Next
        Catch ex As Exception
            Err_No = "74055"
            Err_desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Public Function ShowPlanListForApprovalByUD(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SubQuery As String)
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowPlanListForApprovalByUD(Err_No, Err_Desc, SubQuery)
            'Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim SalesRepName As String
            Dim WorkingDays As Integer
            Dim Desc As String
            Dim IsVal As String
            Dim Visits As Integer
            Dim DefaultID As Integer
            Dim RepID As Integer
            Dim SiteName As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("SalesRep_Name", _
                GetType(String)))
            MyDT.Columns.Add(New DataColumn("Route_Plan", _
               GetType(String)))
            MyDT.Columns.Add(New DataColumn("No_Of_Working_Days", _
               GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("No_Of_Visits", _
             GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Default_Plan_ID", _
           GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Sales_Rep_ID", _
          GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Site", _
               GetType(String)))


            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                SalesRepName = CStr(TempDataTable.Rows(i).Item(1))
                Desc = TempDataTable.Rows(i).Item(2)
                sStartDate = TempDataTable.Rows(i).Item(3)
                sEndDate = TempDataTable.Rows(i).Item(4)
                WorkingDays = TempDataTable.Rows(i).Item(5)
                DefaultID = TempDataTable.Rows(i).Item(6)
                RepID = TempDataTable.Rows(i).Item(7)
                SiteName = CStr(TempDataTable.Rows(i).Item(8))

                IsVal = Desc & " (" & sStartDate.Day & "-" & sStartDate.Month & "-" & sStartDate.Year & " -> " & sEndDate.Day & "-" & sEndDate.Month & "-" & sEndDate.Year & ") "

                Visits = ObjDAlRoutePlanner.GetNoOfVisits(sRPID, sStartDate, sEndDate)

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = SalesRepName
                MyRow(2) = IsVal
                MyRow(3) = WorkingDays
                MyRow(4) = Visits
                MyRow(5) = DefaultID
                MyRow(6) = RepID
                MyRow(7) = SiteName
                MyDT.Rows.Add(MyRow)

            Next
        Catch ex As Exception
            Err_No = "74062"
            Err_Desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function

    Public Function ShowPlanListForReveiwByUD(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SubQuery As String)
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowPlanListForReveiwByUD(Err_No, Err_Desc, SubQuery)
            'Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim SalesRepName As String
            Dim WorkingDays As Integer
            Dim Desc As String
            Dim IsVal As String
            Dim Visits As Integer
            Dim DefaultID As Integer
            Dim RepID As Integer
            Dim SiteName As String
            Dim Approved As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("SalesRep_Name", _
                GetType(String)))
            MyDT.Columns.Add(New DataColumn("Route_Plan", _
               GetType(String)))
            MyDT.Columns.Add(New DataColumn("No_Of_Working_Days", _
               GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("No_Of_Visits", _
             GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Default_Plan_ID", _
           GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Sales_Rep_ID", _
          GetType(Integer)))
            MyDT.Columns.Add(New DataColumn("Site", _
               GetType(String)))

            MyDT.Columns.Add(New DataColumn("Status", _
            GetType(String)))

            MyDT.Columns.Add(New DataColumn("Start_Date", _
            GetType(DateTime)))


            MyDT.Columns.Add(New DataColumn("Start_Date_Month", _
            GetType(Integer)))

            MyDT.Columns.Add(New DataColumn("Start_Date_Year", _
            GetType(Integer)))


            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                SalesRepName = CStr(TempDataTable.Rows(i).Item(1))
                Desc = TempDataTable.Rows(i).Item(2)
                sStartDate = TempDataTable.Rows(i).Item(3)
                sEndDate = TempDataTable.Rows(i).Item(4)
                WorkingDays = TempDataTable.Rows(i).Item(5)
                DefaultID = TempDataTable.Rows(i).Item(6)
                RepID = TempDataTable.Rows(i).Item(7)
                SiteName = CStr(TempDataTable.Rows(i).Item(8))
                Approved = CStr(TempDataTable.Rows(i).Item(9))
                IsVal = Desc & " (" & sStartDate.Day & "-" & sStartDate.Month & "-" & sStartDate.Year & " -> " & sEndDate.Day & "-" & sEndDate.Month & "-" & sEndDate.Year & ") "

                Visits = ObjDAlRoutePlanner.GetNoOfVisits(sRPID, sStartDate, sEndDate)

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = SalesRepName
                MyRow(2) = IsVal
                MyRow(3) = WorkingDays
                MyRow(4) = Visits
                MyRow(5) = DefaultID
                MyRow(6) = RepID
                MyRow(7) = SiteName
                MyRow(8) = Approved
                MyRow(9) = sStartDate
                MyRow(10) = sStartDate.Month
                MyRow(11) = sStartDate.Year
                MyDT.Rows.Add(MyRow)

            Next
        Catch ex As Exception
            Err_No = "74062"
            Err_Desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function

    Function ShowCopyFromPlans(ByRef Err_No As Long, ByRef Err_desc As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowCopyFromPlans(Err_No, Err_desc, Me._SalesRepID, Me._Site)
            Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim isAssigned As String
            Dim AppStat As Char
            Dim DefPlanID As Integer
            Dim SiteName As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("Details", _
                GetType(String)))
            MyDT.Columns.Add(New DataColumn("Approved", _
                GetType(Char)))
            MyDT.Columns.Add(New DataColumn("DefPlanID", _
               GetType(Integer)))

            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                tempDBVal = TempDataTable.Rows(i).Item(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sStartDate = TempDataTable.Rows(i).Item(2)
                sEndDate = TempDataTable.Rows(i).Item(3)
                AppStat = TempDataTable.Rows(i).Item(4)
                DefPlanID = TempDataTable.Rows(i).Item(5)
                SiteName = CStr(TempDataTable.Rows(i).Item(6))

                'isAssigned = tempDBVal & " (" & sStartDate.Day & "/" & sStartDate.Month & "/" & sStartDate.Year & " - " & sEndDate.Day & "/" & sEndDate.Month & "/" & sEndDate.Year & ") "
                isAssigned = tempDBVal & " [" & SiteName & "]"

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = isAssigned
                MyRow(2) = AppStat
                MyRow(3) = DefPlanID
                MyDT.Rows.Add(MyRow)

            Next
        Catch ex As Exception
            Err_No = "74068"
            Err_desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Function ShowCopyToPlans(ByRef Err_No As Long, ByRef Err_desc As String) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            TempDataTable = ObjDAlRoutePlanner.ShowCopyToPlans(Err_No, Err_desc, Me._SalesRepID, Me._Site)
            Dim tempDBVal As Object
            Dim sRPID As String
            Dim sStartDate As Date
            Dim sEndDate As Date
            Dim isAssigned As String
            Dim AppStat As Integer
            ' Dim DefPlanID As Integer
            Dim SiteName As String

            Dim MyRow As DataRow
            MyDT.Columns.Add(New DataColumn("FSR_Plan_ID", _
                GetType(Int32)))
            MyDT.Columns.Add(New DataColumn("Details", _
                GetType(String)))
            MyDT.Columns.Add(New DataColumn("No_Of_Working_Days", _
                GetType(Int32)))

            For i As Integer = 0 To TempDataTable.Rows.Count() - 1
                sRPID = CStr(TempDataTable.Rows(i).Item(0))
                tempDBVal = TempDataTable.Rows(i).Item(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sStartDate = TempDataTable.Rows(i).Item(2)
                sEndDate = TempDataTable.Rows(i).Item(3)
                AppStat = TempDataTable.Rows(i).Item(4)
                SiteName = CStr(TempDataTable.Rows(i).Item(5))
                ' isAssigned = tempDBVal & " (" & sStartDate.Day & "/" & sStartDate.Month & "/" & sStartDate.Year & " - " & sEndDate.Day & "/" & sEndDate.Month & "/" & sEndDate.Year & ") "
                isAssigned = tempDBVal & " [" & SiteName & "]"

                MyRow = MyDT.NewRow()
                MyRow(0) = sRPID
                MyRow(1) = isAssigned
                MyRow(2) = AppStat
                MyDT.Rows.Add(MyRow)

            Next
        Catch ex As Exception
            Err_No = "74071"
            Err_desc = ex.Message
            Throw ex
        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT
    End Function
    Function GetCustomerId(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal CustNo As String, ByVal SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDAlRoutePlanner.GetCustomerId(transaction, SqlConn, objSQLCmd, CustNo, SiteID, Err_No, Err_Desc)
    End Function
    Function CopyFSRPlan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CopyToDefPlan As Int64) As Boolean
        Return ObjDAlRoutePlanner.CopyFSRPlan(Err_No, Err_Desc, Me._defaultPlanId, Me._FSRPlanID, CopyToDefPlan, Me._SalesRepID)
    End Function
    Function CopyFSRPlanByWeekDays(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CopyToDefPlan As Int64) As Boolean
        Return ObjDAlRoutePlanner.CopyFSRPlanByWeekDays(Err_No, Err_Desc, Me._defaultPlanId, Me._FSRPlanID, CopyToDefPlan, Me._SalesRepID)
    End Function
    Function DefaultPlanExist(ByVal Dat As DateTime, ByVal vanname As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.DefaultPlanExist(Dat, vanname, Err_No, Err_Desc)
    End Function
    Function CheckVanNCustomer(ByVal VanID As String, ByVal CustomerID As String, ByVal SiteID As String, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDAlRoutePlanner.CheckVanNCustomer(VanID, CustomerID, SiteID, Err_No, Err_Desc)
    End Function
    Function CheckValidFSR(ByVal VanID As String, ByVal UserID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDAlRoutePlanner.CheckValidFSR(VanID, UserID, Err_No, Err_Desc)
    End Function
    Function RoutePlanExist(ByVal Dat As DateTime, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDAlRoutePlanner.RoutePlanExist(Dat, SalesRep, Err_No, Err_Desc)
    End Function
    Function GetFSRPlanID(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal DefPlanID As Integer, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String, ByVal userId As String) As String
        Return ObjDAlRoutePlanner.GetFSRPlanID(transaction, SqlConn, objSQLCmd, DefPlanID, SalesRep, Err_No, Err_Desc, userId)
    End Function

    Function GetDayType(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByVal DefPlanID As Integer, ByVal VisitDat As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As String
        Return ObjDAlRoutePlanner.CanVisitCustomer(transaction, SqlConn, objSQLCmd, DefPlanID, VisitDat, Err_No, Err_Desc)
    End Function

    Public Function CheckVisitExist(ByRef transaction As SqlTransaction, ByRef SqlConn As SqlConnection, ByRef objSQLCmd As SqlCommand, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.CheckVisitExist(transaction, SqlConn, objSQLCmd, _FSRPlanID, _CustomerID, _UserSiteID, _day, Err_No, Err_Desc)
    End Function


    Function CheckFSRPlanID(ByVal DefPlanID As Integer, ByVal SalesRep As String, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.CheckPlanID(DefPlanID, SalesRep, Err_No, Err_Desc)
    End Function
    Public Function DefaultPlanUsed(ByVal DefPlanID As Integer, ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Return ObjDAlRoutePlanner.DefaultPlanUsed(DefPlanID, Err_No, Err_Desc)
    End Function
    Function GetHolidays(Month As Integer, Year As Integer) As DataTable
        Dim TempDataTable As New DataTable
        Dim MyDT As New DataTable
        Try
            MyDT = ObjDAlRoutePlanner.GetHolidays(Month, Year)

        Catch ex As Exception

        Finally
            TempDataTable = Nothing
        End Try
        Return MyDT

    End Function
    Function RouteplanForExport(ByRef Err_No As Long, ByRef Err_desc As String, FSR_Plan_ID As String) As DataSet
        Return ObjDAlRoutePlanner.RouteplanForExport(Err_No, Err_desc, FSR_Plan_ID)
    End Function

    Public Function GetSiteNew(ByRef Err_No As Long, ByRef Err_Desc As String, SalesRepID As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetSiteNew(Err_No, Err_Desc, SalesRepID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetFSRPlanStartEnd_Map(ByRef Err_No As Long, ByRef Err_Desc As String, FSRPlanID As String) As DataTable
        Try
            Return ObjDAlRoutePlanner.GetFSRPlanStartEnd_Map(Err_No, Err_Desc, FSRPlanID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
