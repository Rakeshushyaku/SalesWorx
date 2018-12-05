Imports SalesWorx.BO.Common
Imports log4net

Public Class _POP_CustomerListing
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Dim Arr As Array
    Dim IDArr As New ArrayList
    Dim RetValue As Boolean
    Dim Ucomments As String = ""
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            Cache.Remove("VisitPlannedList")
            If Not (Session("HDay" & Request.QueryString("DayRef") & "") Is Nothing) Then


                Dim HValu As String = Session("HDay" & Request.QueryString("DayRef") & "")
                Arr = HValu.Split("|")

                If (Arr.Length >= 2) Then
                    SetValueForDDs(HValu)
                End If
                bindVisitsPlanned()
            End If
            Ucomments = ComString.Value
        End If
    End Sub
    Sub bindVisitsPlanned()
        Dim objRoute As New RoutePlan
        Try
            Dim CustomerDt As New DataTable

            If Cache("VisitPlannedList") Is Nothing Then
                CustomerDt.Columns.Add("Customer_ID")
                CustomerDt.Columns.Add("Site_Use_ID")
                CustomerDt.Columns.Add("Customer_No")
                CustomerDt.Columns.Add("Customer_Name")
                CustomerDt.Columns.Add("Sequence", System.Type.GetType("System.Int32"))
                Cache("VisitPlannedList") = CustomerDt
            Else
                CustomerDt = CType(Cache("VisitPlannedList"), DataTable).Copy()
            End If



            Dim _dv As DataView
            _dv = CustomerDt.DefaultView()
            _dv.Sort = "Sequence Asc"

            CustomerGrid.DataSource = CustomerDt

            CustomerGrid.DataBind()
            CustomerDt = Nothing
            _dv = Nothing
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        Finally
            objRoute = Nothing
        End Try
    End Sub

    Private Sub SetValueForDDs(ByVal HValu As String)
        Try
            Dim CusIDArr As New ArrayList
            Dim HValArr As Array = HValu.Split("|")
            Dim str As String = Session("None")
            Dim CustomerDt As New DataTable

            If Cache("VisitPlannedList") Is Nothing Then
                CustomerDt.Columns.Add("Customer_ID")
                CustomerDt.Columns.Add("Site_Use_ID")
                CustomerDt.Columns.Add("Customer_No")
                CustomerDt.Columns.Add("Customer_Name")
                CustomerDt.Columns.Add("Sequence", System.Type.GetType("System.Int32"))
                For i As Integer = 2 To HValArr.Length - 1
                    If HValArr(i).ToString.Trim <> "" Then
                        Dim SubArray As Array = HValArr(i).ToString.Split("^")
                     

                        Dim dr As DataRow
                        dr = CustomerDt.NewRow
                        dr(0) = SubArray(0)
                        dr(1) = SubArray(2)
                        dr(2) = SubArray(5)
                        dr(3) = SubArray(1)
                        dr(4) = Val(SubArray(3))
                        CustomerDt.Rows.Add(dr)
                    End If
                Next
                Cache("VisitPlannedList") = CustomerDt
            Else
                CustomerDt = CType(Cache("VisitPlannedList"), DataTable).Copy()
            End If
           
            bindVisitsPlanned()
        Catch ex As Exception
            If Err_Desc Is Nothing Then
                log.Error(GetExceptionInfo(ex))
            Else
                log.Error(Err_Desc)
            End If
           
        End Try
    End Sub
End Class