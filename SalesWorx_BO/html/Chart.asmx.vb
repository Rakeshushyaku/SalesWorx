Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Services
Imports System.Runtime.SerializationCommandTimeout
Imports System.IO
Imports System.Web.Script.Serialization
Imports SalesWorx.BO.Common
Imports HttpContext.Current.Session
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Chart
    Inherits System.Web.Services.WebService
    Private _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetSalesbyAgency(ByVal param As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByAgency_DashBoard]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@Uid", param)
            objSQLCmd.Parameters.AddWithValue("@FromDate", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", Now.ToString("dd-MMM-yyyy"))

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetSalesbyAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Agency").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetSalesbyAgencyForMonth(ByVal param1 As String, ByVal param2 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_DashBoard_SalesByAgency]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@VanIDs", param1)
            objSQLCmd.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetSalesbyAgencyForMonth = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Agency").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function

    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetCollectionbyVanForMonth(ByVal param1 As String, ByVal param2 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_VanCollections_DashBoard]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OrgIds", param1)
            objSQLCmd.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetCollectionbyVanForMonth = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Van").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("CashAmount").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("PCDAmount").ToString)
            oDashBoardSalesbyAgency.Amount2 = Val(dr("ChequeAmount").ToString)
            oDashBoardSalesbyAgency.Amount2 = Val(dr("ChequeAmount").ToString)
            oDashBoardSalesbyAgency.Amount3 = Val(dr("CCAmount").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetFSRCollection(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_FSRCollection]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@Uid", param2)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param3)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param4)
            objSQLCmd.Parameters.AddWithValue("@SID", param5)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetFSRCollection = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("SalesRep_name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Cash").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("PDC").ToString)
            oDashBoardSalesbyAgency.Amount2 = Val(dr("Cheque").ToString)
            oDashBoardSalesbyAgency.Amount3 = Val(dr("CC").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function


    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetSalesbyVanForMonth(ByVal param1 As String, ByVal param2 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_DashBoard_SalesByVan]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OrgIDs", param1)
            objSQLCmd.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd-MMM-yyyy"))

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetSalesbyVanForMonth = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Vanno").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function

    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetPDCReceivables(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_PDC_Receivables]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OrgID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try



        If dt.Rows.Count > 0 Then
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = Now.AddMonths(-2).ToString("MMM-yyyy")
            oDashBoardSalesbyAgency.Amount = Val(dt.Rows(0)("ReceivableM2").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing

            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = Now.AddMonths(-1).ToString("MMM-yyyy")
            oDashBoardSalesbyAgency.Amount = Val(dt.Rows(0)("ReceivableM1").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing

            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = Now.ToString("MMM-yyyy")
            oDashBoardSalesbyAgency.Amount = Val(dt.Rows(0)("ReceivableM").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        End If
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getMonthlyDistribution(ByVal param As String, param1 As String, param2 As String, param3 As String, param4 As String, param5 As String, param6 As String, param7 As String, param8 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)
        Dim alRecords As ArrayList = New ArrayList(0)
        Const sql As String = "Rep_MonthlyDistribution"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(param2), "-1", param2))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Brand", IIf(String.IsNullOrEmpty(param3), "-1", param3))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SKUList", IIf(String.IsNullOrEmpty(param4), "-1", param4))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param5)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param6)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ViewBy", param7)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ChartMode", param8)

        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()

        'Dim query

        'query = (From UserEntry In dt _
        '            Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
        '            Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("TotValue")) Order By Total Descending).ToList


        'If dt.Rows.Count > 0 Then
        '    For Each x In query

        '        Dim oEntity As New DelayedPayment
        '        oEntity.OrgName = x.Salesrep_name

        '        Dim tfromdate As DateTime
        '        tfromdate = DateTime.Parse(param5)

        '        While tfromdate <= DateTime.Parse(param6)

        '            oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
        '            Dim Seldr() As DataRow
        '            Seldr = dt.Select("Description='" & x.Salesrep_name & "' and mnorder=" & tfromdate.ToString("MMM-yyyy"))
        '            If Seldr.Length > 0 Then
        '                oEntity.Percentage.Add(Val(Seldr(0)("TotValue").ToString()))
        '            Else
        '                oEntity.Percentage.Add(0)
        '            End If
        '            tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
        '        End While

        '        alRecords.Add(oEntity)
        '        oEntity = Nothing
        '    Next
        'End If
        'Return alRecords



        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dt.Rows

            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0"))


        Next



        'TargetvsSalesDataList = (
        '                From m In dt.Rows.Cast(Of DataRow)()
        '                Select New TargetVsSalesbyMonths With {.AgencyName = m.Field(Of String)("MnOrder"), .Count = Convert.ToDecimal(m("TotValue")), .Description = m.Field(Of String)("Description"), .DispOrder = m.Field(Of String)("DispOrder"), .MonthYear = m.Field(Of String)("MonthYear"), .Unit = 0}).ToList()



        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "1", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function
    Public Function GetTargetType() As String
        Dim myConnection As New SqlConnection(_strSQLConn)
        Dim objSQLCmd As SqlCommand
        Dim Name As String = Nothing

        Try
            myConnection.Open()
            Dim QueryString As String = String.Format("Select top 1 Target_Type from TBL_Sales_Target order by Created_At desc")
            objSQLCmd = New SqlCommand(QueryString, myConnection)
            objSQLCmd.CommandType = CommandType.Text
            Name = Convert.ToString(objSQLCmd.ExecuteScalar())

            objSQLCmd.Dispose()
        Catch ex As Exception
         
        Finally
            objSQLCmd = Nothing
            myConnection.Close()
        End Try
        Return Name
    End Function
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getTargetvsSalesByMonths(ByVal param As String, param1 As String, param2 As String, param3 As String, param4 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Dim targettype As String = "A"
        Dim Procname As String = "Rep_MonthlyBusinessReport_TVA"
        targettype = GetTargetType()

        If targettype = "P" Then
            Procname = "Rep_MonthlyBusinessReport_Item"
        ElseIf targettype = "B" Then
            Procname = "Rep_MonthlyBusinessReport_Brand"
        Else
            Procname = "Rep_MonthlyBusinessReport_TVA"
        End If

        Dim sql As String = Procname
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 900
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(param2), "-1", param2))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param3)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0"))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "0", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function


    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getTargetvsSalesMBR(ByVal param As String, param1 As String, param2 As String, param3 As String, param4 As String, param5 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyVan)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_MonthlyBusinessReportCopy"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 900
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(param2), "-1", param2))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param3)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        'myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyVan(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), IIf(param5 <> "Q", CStr(row("DispOrder")), "0"), CStr(row("MonthYear")), "0"))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyVan("No Data", 0, "No data", "0", "0", "0"))
        End If
        Return TargetvsSalesDataList
    End Function


    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getTargetvsSalesByVan(ByVal param As String, param1 As String, param2 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_TargetvsSalesByVan"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param2)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Page")


        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        ' myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CInt(IIf(row("MnOrder") Is DBNull.Value, "0", row("MnOrder").ToString())), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), "01-01-1900", CStr(IIf(row("ProdCount") Is DBNull.Value, "0", row("ProdCount").ToString()))))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "0", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function


    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getBestSellers(ByVal param As String, param1 As String, param2 As String, param3 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_BestSellers"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param2)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param3)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ChartMode", "Van")

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CInt(IIf(row("MnOrder") Is DBNull.Value, "0", row("MnOrder").ToString())), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), "01-01-1900", CInt(IIf(row("ProdCount") Is DBNull.Value, "0", row("ProdCount").ToString()))))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "0", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function


    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getBestSellersBySKU(ByVal param As String, param1 As String, param2 As String, param3 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_BestSellers"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param2)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param3)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ChartMode", "SKU")

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CInt(IIf(row("MnOrder") Is DBNull.Value, "0", row("MnOrder").ToString())), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), "01-01-1900", CStr(IIf(row("ProdCount") Is DBNull.Value, "0", row("ProdCount").ToString()))))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "0", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function



    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getMBRStatistics(ByVal param As String, param1 As String, param2 As String, param3 As String, param4 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Dim targettype As String = "A"
        Dim Procname As String = "Rep_MonthlyBusinessSummary_Agency"
        targettype = GetTargetType()

        If targettype = "P" Then
            Procname = "Rep_MonthlyBusinessSummary_itemCode"
        ElseIf targettype = "B" Then
            Procname = "Rep_MonthlyBusinessSummary_Brand"
        Else
            Procname = "Rep_MonthlyBusinessSummary_Agency"
        End If


        Dim sql As String = Procname
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()
        'Dim dt As DataTable = CType(Session("dtMBRStat"), DataTable)
        'Session.Remove("dtMBRStat")


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandTimeout = 900
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@VanList", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", IIf(String.IsNullOrEmpty(param2), "-1", param2))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FMonth", param3)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@TMonth", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Mode", "Chart")

        Dim dt As New DataTable
        objSQLDA.Fill(dt)
        '  myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dt.Rows
            '   If row("Description").ToString() = "JP Adherance %" Or row("Description").ToString() = "Call Productivity %" Or row("Description").ToString() = "Growth over last year %" Or row("Description").ToString() = "Outlet Productivity %" Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0"))
            '  End If
        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "1", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function


    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function DelayedCustomerCount(ByVal param As String, param1 As String, param2 As String, param3 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DelayedCustomerCountByOrg"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", param2)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param3)

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        Dim i As Integer = 0

        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CDate(row("m") & "/01/" & row("Yr")).ToString("MMM-yyyy"), CDec(IIf(row("CustCount") Is DBNull.Value, "0", row("CustCount").ToString())), CStr(row("Description")), i, CDate(row("m") & "/01/" & row("Yr")), 0))
            i = i + 1

        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "1", "01-01-1900", 0))
        End If
        Return TargetvsSalesDataList
    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getSMLast3MonthsSalesgrowth(ByVal param As String, param1 As String, param2 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsSalesbyMonths)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_SMLast3MonthsSalesgrowth"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SMID", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@MonthYear", param2)

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0"))


        Next
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsSalesbyMonths("No Data", 0, "No data", "0", "01-01-1900", "0"))
        End If
        Return TargetvsSalesDataList
    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getDashTotalVsProductive(ByVal param As String, ByVal param1 As String) As Object
        Dim TotVsProductiveDataList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DashTotalVsProductiveCalls"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@VanList", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYear", param1)


        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            If CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())) > 0 Then
                TotVsProductiveDataList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))
            End If

        Next
        If TotVsProductiveDataList.Count = 0 Then
            TotVsProductiveDataList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return TotVsProductiveDataList
    End Function
    <System.Web.Services.WebMethod()> _
  <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getDashTop10VanSales(ByVal param As String, ByVal param1 As String) As Object
        Dim VanSalesList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DashTop10VanSales"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@VanList", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYEAR", param1)
        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            VanSalesList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))


        Next
        If VanSalesList.Count = 0 Then
            VanSalesList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return VanSalesList
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getSMTop10CustomerSales(ByVal param As String, ByVal param1 As String, ByVal param2 As String) As Object
        Dim VanSalesList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_SMTop10CustomerSales"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@OID", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@SMID", param1)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYEAR", param2)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@Mode", "Top 10")
        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        ' myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            VanSalesList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))


        Next
        If VanSalesList.Count = 0 Then
            VanSalesList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return VanSalesList
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getDashTop10VanCollections(ByVal param As String, ByVal param1 As String) As Object
        Dim VanSalesList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DashTop10VanCollections"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@VanList", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYEAR", param1)
        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        ' myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            VanSalesList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))


        Next
        If VanSalesList.Count = 0 Then
            VanSalesList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return VanSalesList
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getDashTop10SalesByAgency(ByVal param As String, ByVal param1 As String) As Object
        Dim VanSalesList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DashTop10SalesByAgency"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@VanList", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYEAR", param1)
        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        ' myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            VanSalesList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))


        Next
        If VanSalesList.Count = 0 Then
            VanSalesList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return VanSalesList
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getSalesVsReturns(ByVal param As String, ByVal param1 As String) As Object
        Dim SalesVsReturnsDataList As New List(Of TotalVsProductive)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_DashOrdersVsReturns"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim dataAdapter As New SqlDataAdapter()
        dataAdapter.SelectCommand = myCommand
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        dataAdapter.SelectCommand.CommandTimeout = 600
        dataAdapter.SelectCommand.Parameters.AddWithValue("@VanList", param)
        dataAdapter.SelectCommand.Parameters.AddWithValue("@MonthYear", param1)


        Dim dSet As New DataSet()
        dataAdapter.Fill(dSet)
        'myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next
        For Each row As DataRow In dSet.Tables(0).Rows
            If CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())) > 0 Then
                SalesVsReturnsDataList.Add(New TotalVsProductive(CInt(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description"))))
            End If

        Next
        If SalesVsReturnsDataList.Count = 0 Then
            SalesVsReturnsDataList.Add(New TotalVsProductive(0, "No Data"))
        End If
        Return SalesVsReturnsDataList
    End Function


    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function DivisionWiseCollection(ByVal param1 As String, ByVal param2 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try
            Dim param3 As String


            param2 = "01/" & param2
            param3 = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param2))).ToString("dd/MMM/yyyy") & " 23:59:59"
            objSQLCmd = New SqlCommand("[Rep_DeleyedCollectionByOrg]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@ORGID", param1)
            objSQLCmd.Parameters.AddWithValue("@Fromdate", param2)
            objSQLCmd.Parameters.AddWithValue("@Todate", param3)
            objSQLCmd.Parameters.AddWithValue("@Uid", param4)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim TotalPaidOnTIme As Decimal = 0
        Dim TotalDelayed As Decimal = 0

        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows

                oDashBoardSalesbyAgency = New DashBoardSales
                oDashBoardSalesbyAgency.Description = dr("Description")
                If Val(dr("Delayed").ToString) + Val(dr("PaidOnTIme").ToString) <> 0 Then
                    oDashBoardSalesbyAgency.Amount = Math.Round((Val(dr("Delayed").ToString) / (Val(dr("Delayed").ToString) + Val(dr("PaidOnTIme").ToString))) * 100, 2)
                Else
                    oDashBoardSalesbyAgency.Amount = 0
                End If

                TotalPaidOnTIme = TotalPaidOnTIme + Val(dr("PaidOnTIme").ToString)
                TotalDelayed = TotalDelayed + Val(dr("Delayed").ToString)

                DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
                oDashBoardSalesbyAgency = Nothing
            Next
            'oDashBoardSalesbyAgency = New DashBoardSales
            'oDashBoardSalesbyAgency.Description = "Over All"
            'If TotalPaidOnTIme + TotalDelayed <> 0 Then
            '    oDashBoardSalesbyAgency.Amount = Math.Round((TotalDelayed / (TotalPaidOnTIme + TotalDelayed)) * 100, 2)
            'Else
            '    oDashBoardSalesbyAgency.Amount = 0
            'End If
            'DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            'oDashBoardSalesbyAgency = Nothing
        End If
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SalesVseReturnByCustType(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object
        Dim DashBoardSalesbyVan As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyVan As DashBoardSales


        'Dim ds As New DataSet
        'ds = GetSalesbyVan("", "", param, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"), Now.ToString("dd-MMM-yyyy"))
        Dim MsgDs As New DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByCustomerType]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.CommandTimeout = 600
                objSQLCmd.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
                objSQLCmd.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
                objSQLCmd.Parameters.AddWithValue("@SID", param5)
                objSQLCmd.Parameters.AddWithValue("@OID", param1)
                objSQLCmd.Parameters.AddWithValue("@UID", param4)

                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                objSQLCmd.Dispose()
            Catch ex As Exception

                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyVan = New DashBoardSales
            oDashBoardSalesbyVan.Description = dr("Customer_type").ToString
            oDashBoardSalesbyVan.Amount = Val(dr("Sales").ToString)
            oDashBoardSalesbyVan.ReturnAmount = Math.Abs(Val(dr("Returns").ToString))
            DashBoardSalesbyVan.Add(oDashBoardSalesbyVan)
            oDashBoardSalesbyVan = Nothing
        Next
        Return DashBoardSalesbyVan
    End Function

    <System.Web.Services.WebMethod(EnableSession:=False)> _
    <System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function CollectionTrendByLoc(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, param5 As String) As Object

        Dim alRecords As ArrayList = New ArrayList(0)


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim tdate As String = ""
        Dim dt As New DataTable
        Try


            param2 = "01/" & param2
            tdate = "01/" & param3
            tdate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param3))).ToString("dd/MMM/yyyy") & " 23:59:59"
            objSQLCmd = New SqlCommand("[Rep_DeleyedCollectionBySalesDistrict]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@Fromdate", param2)
            objSQLCmd.Parameters.AddWithValue("@Todate", tdate)
            objSQLCmd.Parameters.AddWithValue("@Uid", param4)
            objSQLCmd.Parameters.AddWithValue("@Loc", param5)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim TotalPaidOnTIme As Decimal = 0
        Dim TotalDelayed As Decimal = 0

        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = CDate(param2)
        todate = CDate(tdate)

        Dim query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
                         Select Description = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Delayed"))).ToList

        If dt.Rows.Count > 0 Then
            For Each x In query

                Dim oEntity As New DelayedPayment
                oEntity.OrgName = x.Description

                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate

                    oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                    Dim Seldr() As DataRow
                    Seldr = dt.Select("Description='" & x.Description & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                    If Seldr.Length > 0 Then
                        If Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString) <> 0 Then
                            Dim perc As Decimal
                            perc = (Val(Seldr(0)("Delayed").ToString) / (Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString))) * 100.0
                            oEntity.Percentage.Add(Format(perc, "##0.00"))
                        Else
                            oEntity.Percentage.Add(0)
                        End If
                    Else
                        oEntity.Percentage.Add(0)
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While

                alRecords.Add(oEntity)
                oEntity = Nothing
            Next
        End If
        Return alRecords
    End Function
    <System.Web.Services.WebMethod(EnableSession:=False)> _
   <System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function CollectionTrendBySuperVisor(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object

        Dim alRecords As ArrayList = New ArrayList(0)


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim tdate As String = ""
        Dim dt As New DataTable
        Try


            param2 = "01/" & param2
            tdate = "01/" & param3
            tdate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param3))).ToString("dd/MMM/yyyy") & " 23:59:59"
            objSQLCmd = New SqlCommand("[Rep_DeleyedCollectionBySuperVisor]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600

            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@Fromdate", CDate(param2).ToString("dd/MMM/yyyy"))
            objSQLCmd.Parameters.AddWithValue("@Todate", tdate)
            objSQLCmd.Parameters.AddWithValue("@Uid", param4)
            objSQLCmd.Parameters.AddWithValue("@Loc", param5)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim TotalPaidOnTIme As Decimal = 0
        Dim TotalDelayed As Decimal = 0

        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = CDate(param2)
        todate = CDate(tdate)

        Dim query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
                         Select Description = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Delayed"))).ToList

        If dt.Rows.Count > 0 Then
            For Each x In query

                Dim oEntity As New DelayedPayment
                oEntity.OrgName = x.Description

                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate

                    oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                    Dim Seldr() As DataRow
                    Seldr = dt.Select("Description='" & x.Description & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                    If Seldr.Length > 0 Then
                        If Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString) <> 0 Then
                            Dim perc As Decimal
                            perc = (Val(Seldr(0)("Delayed").ToString) / (Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString))) * 100.0
                            oEntity.Percentage.Add(Format(perc, "##0.00"))
                        Else
                            oEntity.Percentage.Add(0)
                        End If
                    Else
                        oEntity.Percentage.Add(0)
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While

                alRecords.Add(oEntity)
                oEntity = Nothing
            Next
        End If
        Return alRecords
    End Function

    <System.Web.Services.WebMethod(EnableSession:=False)> _
  <System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function CollectionTrendByDivision(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object

        Dim alRecords As ArrayList = New ArrayList(0)


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim tdate As String = ""
        Dim dt As New DataTable
        Try


            param2 = "01/" & param2
            tdate = "01/" & param3
            tdate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate(param3))).ToString("dd/MMM/yyyy") & " 23:59:59"
            objSQLCmd = New SqlCommand("[Rep_DeleyedCollectionByDivision]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@Fromdate", param2)
            objSQLCmd.Parameters.AddWithValue("@Todate", tdate)
            objSQLCmd.Parameters.AddWithValue("@Uid", param4)
            objSQLCmd.Parameters.AddWithValue("@Loc", param5)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim TotalPaidOnTIme As Decimal = 0
        Dim TotalDelayed As Decimal = 0

        Dim fromdate As DateTime
        Dim todate As DateTime
        fromdate = CDate(param2)
        todate = CDate(tdate)

        Dim query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Description") Into Group _
                         Select Description = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Delayed"))).ToList

        If dt.Rows.Count > 0 Then
            For Each x In query

                Dim oEntity As New DelayedPayment
                oEntity.OrgName = x.Description

                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate

                    oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                    Dim Seldr() As DataRow
                    Seldr = dt.Select("Description='" & x.Description & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                    If Seldr.Length > 0 Then
                        If Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString) <> 0 Then
                            Dim perc As Decimal
                            perc = (Val(Seldr(0)("Delayed").ToString) / (Val(Seldr(0)("PaidOnTime").ToString) + Val(Seldr(0)("Delayed").ToString))) * 100.0
                            oEntity.Percentage.Add(Format(perc, "##0.00"))
                        Else
                            oEntity.Percentage.Add(0)
                        End If
                    Else
                        oEntity.Percentage.Add(0)
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While

                alRecords.Add(oEntity)
                oEntity = Nothing
            Next
        End If
        Return alRecords
    End Function

    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetMonthyReceivables(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_GetCurrentMonth_Receivables]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OrgID", param1)
            objSQLCmd.Parameters.AddWithValue("@Month", param3)
            objSQLCmd.Parameters.AddWithValue("@Yr", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param4)
            objSQLCmd.Parameters.AddWithValue("@Van", param5)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")

            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Van").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Receivable").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetSalesbyVan(ByVal param As String) As Object
        Dim DashBoardSalesbyVan As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyVan As DashBoardSales


        'Dim ds As New DataSet
        'ds = GetSalesbyVan("", "", param, DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"), Now.ToString("dd-MMM-yyyy"))
        Dim MsgDs As New DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByVan_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.CommandTimeout = 600
                objSQLCmd.Parameters.AddWithValue("@Uid", param)
                objSQLCmd.Parameters.AddWithValue("@FromDate", DateAdd(DateInterval.Day, -1 * (Now.Day - 1), Now).ToString("dd-MMM-yyyy"))
                objSQLCmd.Parameters.AddWithValue("@ToDate", Now.ToString("dd-MMM-yyyy"))

                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                objSQLCmd.Dispose()
            Catch ex As Exception

                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyVan = New DashBoardSales
            oDashBoardSalesbyVan.Description = dr("Vanno").ToString
            oDashBoardSalesbyVan.Amount = Val(dr("Amount").ToString)
            DashBoardSalesbyVan.Add(oDashBoardSalesbyVan)
            oDashBoardSalesbyVan = Nothing
        Next
        Return DashBoardSalesbyVan
    End Function

    Private Function GetSalesbyAgency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByAgency_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.CommandTimeout = 600
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyAgency = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetSalesbyVan(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal UserID As String, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Try
            Dim objSQLConn As New SqlConnection(_strSQLConn)
            Dim objSQLDa As New SqlDataAdapter
            Dim objSQLCmd As New SqlCommand
            Dim dt As New DataTable
            Try

                objSQLCmd = New SqlCommand("[Rep_SalesByVan_DashBoard]", objSQLConn)
                objSQLCmd.CommandType = CommandType.StoredProcedure
                objSQLCmd.CommandTimeout = 600
                objSQLCmd.Parameters.AddWithValue("@Uid", UserID)
                objSQLCmd.Parameters.AddWithValue("@FromDate", StartDate)
                objSQLCmd.Parameters.AddWithValue("@ToDate", EndDate)
                Dim MsgDs As New DataSet
                Dim SqlAd As SqlDataAdapter
                SqlAd = New SqlDataAdapter(objSQLCmd)
                SqlAd.Fill(MsgDs, "VanLogTbl")
                GetSalesbyVan = MsgDs
                objSQLCmd.Dispose()
            Catch ex As Exception
                Err_No = "74061"
                Err_Desc = ex.Message
                Throw ex
            Finally
                objSQLCmd = Nothing
                objSQLConn.Close()
                objSQLConn = Nothing
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SalesbyVan(param1 As String, param2 As String, param3 As String, param4 As String, param5 As String, param6 As String) As Object
        Dim alRecords As ArrayList = New ArrayList(0)
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_MonthlySalesByVan"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", param5)

        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        'myCommand.ExecuteNonQuery()
        myConnection.Close()

        Dim fromdate As DateTime
        Dim todate As DateTime

        fromdate = CDate(param2)
        todate = CDate(param3)
        Dim query
        If param6 = "10" Then
            query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                         Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending.Take(10)).ToList
        Else
            query = (From UserEntry In dt _
                        Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                        Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Sales")) Order By Total Descending).ToList
        End If

        If dt.Rows.Count > 0 Then
            For Each x In query
                If x.Total <> 0 Then
                    Dim oEntity As New DelayedPayment
                    oEntity.OrgName = x.Salesrep_name

                    Dim tfromdate As DateTime
                    tfromdate = fromdate

                    While tfromdate <= todate

                        oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                        Dim Seldr() As DataRow
                        Seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                        If Seldr.Length > 0 Then
                            oEntity.Percentage.Add(Val(Seldr(0)("Sales").ToString()))
                        Else
                            oEntity.Percentage.Add(0)
                        End If
                        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                    End While

                    alRecords.Add(oEntity)
                    oEntity = Nothing
                End If

            Next
        End If
        Return alRecords
    End Function

    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function PDCReceivablesbyMonth(param1 As String, param2 As String, param3 As String, param4 As String, param5 As String, param6 As String) As Object
        Dim alRecords As ArrayList = New ArrayList(0)
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_PDC_Receivables_V2"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", param5)

        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        'myCommand.ExecuteNonQuery()
        myConnection.Close()

        Dim fromdate As DateTime
        Dim todate As DateTime

        fromdate = CDate(param2)
        todate = CDate(param3)
        Dim query
        If param6 = "10" Then
            query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                         Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount")) Order By Total Descending.Take(10)).ToList
        Else
            query = (From UserEntry In dt _
                        Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                        Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Amount")) Order By Total Descending).ToList
        End If

        If dt.Rows.Count > 0 Then
            For Each x In query
                If x.Total > 0 Then
                    Dim oEntity As New DelayedPayment
                    oEntity.OrgName = x.Salesrep_name

                    Dim tfromdate As DateTime
                    tfromdate = fromdate

                    While tfromdate <= todate

                        oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                        Dim Seldr() As DataRow
                        Seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                        If Seldr.Length > 0 Then
                            oEntity.Percentage.Add(Val(Seldr(0)("Amount").ToString()))
                        Else
                            oEntity.Percentage.Add(0)
                        End If
                        tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                    End While

                    alRecords.Add(oEntity)
                    oEntity = Nothing
                End If
            Next
        End If
        Return alRecords
    End Function
    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function ZerobilledOutlets(param1 As String, param2 As String, param3 As String, param4 As String, param5 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales

        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_ZerobilledOutlets"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", param5)

        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        'myCommand.ExecuteNonQuery()
        myConnection.Close()
        For Each dr As DataRow In dt.Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("SalesRep_Name").ToString
            If Val(dr("PlannedCust").ToString) <> 0 Then
                oDashBoardSalesbyAgency.Amount = (Val(dr("NotBilled").ToString) / Val(dr("PlannedCust").ToString)) * 100
            Else
                oDashBoardSalesbyAgency.Amount = 0
            End If

            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next
        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function ReceivablesbyVan(param1 As String, param2 As String, param3 As String, param4 As String, param5 As String, param6 As String) As Object
        Dim alRecords As ArrayList = New ArrayList(0)
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_Market_Receivables_V2"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", param5)

        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        ' myCommand.ExecuteNonQuery()
        myConnection.Close()

        Dim fromdate As DateTime
        Dim todate As DateTime

        fromdate = CDate(param2)
        todate = CDate(param3)
        Dim query
        If param6 = "10" Then
            query = (From UserEntry In dt _
                         Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                         Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Pending_Amount")) Order By Total Descending.Take(10)).ToList
        Else
            query = (From UserEntry In dt _
                        Group UserEntry By key = UserEntry.Field(Of String)("Salesrep_name") Into Group _
                        Select Salesrep_name = key, Total = Group.Sum(Function(p) p.Field(Of Decimal)("Pending_Amount")) Order By Total Descending).ToList
        End If

        If dt.Rows.Count > 0 Then
            For Each x In query
                ' If x.total > 0 Then
                Dim oEntity As New DelayedPayment
                oEntity.OrgName = x.Salesrep_name

                Dim tfromdate As DateTime
                tfromdate = fromdate

                While tfromdate <= todate

                    oEntity.Mdate.Add(tfromdate.ToString("MMM-yyyy"))
                    Dim Seldr() As DataRow
                    Seldr = dt.Select("Salesrep_name='" & x.Salesrep_name & "' and m=" & tfromdate.Month & " and Yr=" & tfromdate.Year)
                    If Seldr.Length > 0 Then
                        oEntity.Percentage.Add(Val(Seldr(0)("Pending_Amount").ToString()))
                    Else
                        oEntity.Percentage.Add(0)
                    End If
                    tfromdate = DateAdd(DateInterval.Month, 1, tfromdate)
                End While

                alRecords.Add(oEntity)
                oEntity = Nothing
                ' End If
            Next
        End If
        Return alRecords
    End Function
    <System.Web.Services.WebMethod(EnableSession:=False)> _
    <System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SKUDistribution(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object

        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand

        Dim dt As New DataTable
        Try



            objSQLCmd = New SqlCommand("[Rep_SKUDistribution]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@Fromdate", param4)
            objSQLCmd.Parameters.AddWithValue("@Todate", param5)
            objSQLCmd.Parameters.AddWithValue("@Type", param6)
            objSQLCmd.Parameters.AddWithValue("@By", param7)
            objSQLCmd.Parameters.AddWithValue("@ID", param8)
            objSQLCmd.Parameters.AddWithValue("@SiteID", param9)
            objSQLCmd.Parameters.AddWithValue("@DisplayType", "G")

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim i As Integer = 0
        If dt.Rows.Count > 0 Then
            Dim TotalQty As Decimal = Val(dt.Rows(0)("Totqty").ToString)
            Dim TotalValue As Decimal = Val(dt.Rows(0)("TotValue").ToString)

            For Each dr As DataRow In dt.Rows
                If i > 0 Then
                    Dim perc As Decimal
                    If param6 = "Q" Then
                        If TotalQty <> 0 Then
                            perc = (Val(dr("qty").ToString) / TotalQty) * 100.0
                        Else
                            perc = 0
                        End If
                    Else
                        If TotalValue <> 0 Then
                            perc = (Val(dr("Value").ToString) / TotalValue) * 100.0
                        Else
                            perc = 0
                        End If
                    End If
                    oDashBoardSalesbyAgency = New DashBoardSales
                    oDashBoardSalesbyAgency.Description = dr("Description").ToString
                    oDashBoardSalesbyAgency.Amount = perc
                    DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
                    oDashBoardSalesbyAgency = Nothing
                End If
                i = i + 1
            Next

        End If


        Return DashBoardSalesbyAgency

    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepNetSalesbyVan(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByVan_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepNetSalesbyVan = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("SalesRep_Name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Net").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepSalesbyVan(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByVan_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepSalesbyVan = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("SalesRep_Name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Sales").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Math.Abs(Val(dr("Returns").ToString))
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepSalesbySku(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesBySKU_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLCmd.Parameters.AddWithValue("@ChartType", "C")
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepSalesbySku = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Product").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Sales").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Math.Abs(Val(dr("Returns").ToString))
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepNetSalesbySku(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesBySKU_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLCmd.Parameters.AddWithValue("@ChartType", "C")
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepNetSalesbySku = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Product").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Net").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepNetSalesbyCustomer(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByCustomer_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600

            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLCmd.Parameters.AddWithValue("@ChartType", "C")
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepNetSalesbyCustomer = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Customer").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Net").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepSalesbyCustomer(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByCustomer_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If
            objSQLCmd.Parameters.AddWithValue("@ChartType", "C")
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepSalesbyCustomer = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Customer").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Sales").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Math.Abs(Val(dr("Returns").ToString))
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
 <System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepSalesbyClient(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_GetCustomerSalesReturns]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OrgID", param1)
            objSQLCmd.Parameters.AddWithValue("@SalesRep_ID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param4)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param5)
            objSQLCmd.Parameters.AddWithValue("@Type", param6)
            objSQLCmd.Parameters.AddWithValue("@Customer", param7)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepSalesbyClient = MsgDs
            objSQLCmd.Dispose()

        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Location").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("SValue").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Math.Abs(Val(dr("RValue").ToString))
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepSalesbyAgency(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByAgency_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepSalesbyAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Agency").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Sales").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Math.Abs(Val(dr("Returns").ToString))
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function

    <System.Web.Services.WebMethod(EnableSession:=False)> _
<System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function RevenueDispersion(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object

        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand

        Dim dt As New DataTable
        Try



            objSQLCmd = New SqlCommand("[Rep_RevenueDispersion]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@Year", CDate(param4).Year)
            objSQLCmd.Parameters.AddWithValue("@Month", CDate(param4).Month)


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim i As Integer = 0
        If dt.Rows.Count > 0 Then


            For Each dr As DataRow In dt.Rows

                oDashBoardSalesbyAgency = New DashBoardSales
                oDashBoardSalesbyAgency.Description = dr("Billing").ToString
                oDashBoardSalesbyAgency.Amount = dr("Invoice").ToString
                oDashBoardSalesbyAgency.ReturnAmount = dr("Customer").ToString
                DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
                oDashBoardSalesbyAgency = Nothing

                i = i + 1
            Next

        End If


        Return DashBoardSalesbyAgency

    End Function
    <System.Web.Services.WebMethod(EnableSession:=False)> _
<System.Web.Script.Services.ScriptMethod(responseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function SKUDispersion(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object

        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales


        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand

        Dim dt As New DataTable
        Try



            objSQLCmd = New SqlCommand("[Rep_SKUDispersion]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@Year", CDate(param4).Year)
            objSQLCmd.Parameters.AddWithValue("@Month", CDate(param4).Month)


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(dt)
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        Dim i As Integer = 0
        If dt.Rows.Count > 0 Then


            For Each dr As DataRow In dt.Rows

                oDashBoardSalesbyAgency = New DashBoardSales
                oDashBoardSalesbyAgency.Description = dr("Distinct_Line_Item").ToString
                oDashBoardSalesbyAgency.Amount = dr("Invoice").ToString
                oDashBoardSalesbyAgency.ReturnAmount = dr("Customer").ToString
                DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
                oDashBoardSalesbyAgency = Nothing

                i = i + 1
            Next

        End If


        Return DashBoardSalesbyAgency

    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetRepNetSalesbyAgency(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String, ByVal param8 As String, ByVal param9 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_SalesByAgency_V2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@FromDate", param5)
            objSQLCmd.Parameters.AddWithValue("@ToDate", param6)
            objSQLCmd.Parameters.AddWithValue("@Type", param4)
            objSQLCmd.Parameters.AddWithValue("@Agency", param7)
            objSQLCmd.Parameters.AddWithValue("@Item", param8)
            If param9 = "0" Then
                objSQLCmd.Parameters.AddWithValue("@Customer", 0)
                objSQLCmd.Parameters.AddWithValue("@SiteID", 0)
            Else
                Dim ids() As String
                ids = param9.Split("$")
                objSQLCmd.Parameters.AddWithValue("@Customer", ids(0))
                objSQLCmd.Parameters.AddWithValue("@SiteID", ids(1))
            End If

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetRepNetSalesbyAgency = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Agency").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Net").ToString)

            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetWeeklyReturns(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String, ByVal param5 As String, ByVal param6 As String, ByVal param7 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_WeeklyReturns]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@SID", param4)
            objSQLCmd.Parameters.AddWithValue("@UID", param5)
            objSQLCmd.Parameters.AddWithValue("@Customer", param6)
            objSQLCmd.Parameters.AddWithValue("@SiteID", param7)
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            GetWeeklyReturns = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Description").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("RMA").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function PlannedCoverageCustomer(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_PlannedCoverageByFSR_v2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Dat", CDate(param3).ToString("MM-dd-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@Dat1", CDate(param4).ToString("MM-dd-yyyy"))
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            PlannedCoverageCustomer = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try


        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Salesrep_name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("TotalCustomersPDA").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("PlannedCust").ToString)
            If Val(dr("TotalCustomersPDA").ToString) <> 0 Then
                oDashBoardSalesbyAgency.Count = (Convert.ToDecimal(Val(dr("PlannedCust").ToString)) / Convert.ToDecimal(Val(dr("TotalCustomersPDA").ToString))) * 100.0
            Else
                oDashBoardSalesbyAgency.Count = 0
            End If

            oDashBoardSalesbyAgency.Description1 = dr("Salesrep_name").ToString
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function PlannedCoverageVisits(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_PlannedCoverageByFSR_v2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Dat", CDate(param3).ToString("MM-dd-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@Dat1", CDate(param4).ToString("MM-dd-yyyy"))
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            PlannedCoverageVisits = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try


        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Salesrep_name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("PlannedVisited").ToString)
            If Val(dr("TotalWorkingDays").ToString) <> 0 Then
                oDashBoardSalesbyAgency.Count = (Val(dr("PlannedVisited").ToString) / Val(dr("TotalWorkingDays").ToString))
            Else
                oDashBoardSalesbyAgency.Count = 0
            End If

            oDashBoardSalesbyAgency.Description1 = dr("Salesrep_name").ToString
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function OverallCoverageCustomer(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_OverAllCoverageByFSR_Cust]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Dat", CDate(param3).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@Dat1", CDate(param4).ToString("dd-MMM-yyyy"))
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            OverallCoverageCustomer = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try


        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Salesrep_name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("PlannedCust").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("ActualVisitedCust").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("BilledCustomer").ToString)
            oDashBoardSalesbyAgency.Count = Val(dr("Productivity").ToString)
            oDashBoardSalesbyAgency.Count1 = Val(dr("Adherence").ToString)
            oDashBoardSalesbyAgency.Description1 = dr("Salesrep_name").ToString
            oDashBoardSalesbyAgency.Description2 = dr("Salesrep_name").ToString
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function OverallCoverageVisits(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_OverAllCoverageByFSR_visit]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Dat", CDate(param3).ToString("dd-MMM-yyyy"))
            objSQLCmd.Parameters.AddWithValue("@Dat1", CDate(param4).ToString("dd-MMM-yyyy"))
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            OverallCoverageVisits = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try


        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Salesrep_name").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("PlannedVisits").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("ActualVisits").ToString)
            oDashBoardSalesbyAgency.Count = Val(dr("ProductityVisit").ToString)
            oDashBoardSalesbyAgency.Description1 = dr("Salesrep_name").ToString
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function JPAdherence(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_JourneyAdheranceNew]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@UID", param3)
            objSQLCmd.Parameters.AddWithValue("@Month", CDate(param4).Month)
            objSQLCmd.Parameters.AddWithValue("@Year", CDate(param4).Year)
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            JPAdherence = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try


        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Van").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("Mpercentage").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("M1percentage").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("M2percentage").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function GetProductAvailability(param1 As String, param2 As String, param3 As String, param4 As String, param5 As String, param6 As String, param7 As String, param8 As String) As Object
        Dim alRecords As ArrayList = New ArrayList(0)
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_ProductAvailibility"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param1)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@FromDate", CDate(param2).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@ToDate", CDate(param3).ToString("dd-MMM-yyyy"))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@UID", param4)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SID", param5)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Agency", param6)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@InvID", param7)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Availability", param8)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@Type", "C")
        Dim dt As New DataTable()
        objSQLDA.Fill(dt)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()

        Dim fromdate As DateTime
        Dim todate As DateTime

        fromdate = CDate(param2)
        todate = CDate(param3)

        Dim query
        query = (From UserEntry In dt _
                       Group UserEntry By key = UserEntry.Field(Of String)("Item_Code") Into Group _
                       Select Item_Code = key, Total = Group.Sum(Function(p) p.Field(Of Integer)("cnt")) Order By Total Descending).ToList

        Dim queryDate
        queryDate = (From UserEntry In dt _
                       Group UserEntry By key = UserEntry.Field(Of String)("Checked_On") Into Group _
                       Select Checked_On = key, Total = Group.Sum(Function(p) p.Field(Of Integer)("cnt")) Order By Checked_On Ascending).ToList

        For Each x In query

            Dim oEntity As New DelayedPayment


            Dim seldr1() As DataRow
            seldr1 = dt.Select("Item_Code='" & x.Item_Code & "'")
            If seldr1.Length > 0 Then
                oEntity.OrgName = seldr1(0)("Description").ToString
            End If
            'Dim tfromdate As DateTime
            'tfromdate = fromdate

            'While tfromdate <= todate
            For Each y In queryDate
                Dim seldr() As DataRow
                seldr = dt.Select("Item_Code='" & x.Item_Code & "' and Checked_On='" & CDate(y.Checked_On).ToString("MM/dd/yyyy") & "'")
                oEntity.Mdate.Add(CDate(y.Checked_On).ToString("dd-MMM"))
                If seldr.Length > 0 Then
                    oEntity.Percentage.Add(seldr(0)("cnt"))
                Else
                    oEntity.Percentage.Add(Nothing)
                End If
                '    tfromdate = DateAdd(DateInterval.Day, 1, tfromdate)
                'End While
            Next
            alRecords.Add(oEntity)
            oEntity = Nothing
        Next
        If alRecords.Count = 0 Then
            Dim oEntity As New DelayedPayment
            oEntity.Mdate.Add("No data")
            alRecords.Add(oEntity)
        End If
        Return alRecords
    End Function

    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function ProdcutiveCallsCustomer(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_ProductiveCalls_v2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            ProdcutiveCallsCustomer = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Van").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("M2").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("M1").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("M").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function ProdcutiveCallsVisits(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_ProductiveCalls_v2]", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.CommandTimeout = 600
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)


            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            ProdcutiveCallsVisits = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Van").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("V2").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("V1").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("V").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function
    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function ProdcutivityperMSL(ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, param4 As String) As Object
        Dim DashBoardSalesbyAgency As New List(Of DashBoardSales)()
        Dim oDashBoardSalesbyAgency As DashBoardSales
        Dim MsgDs As New DataSet

        Dim objSQLConn As New SqlConnection(_strSQLConn)
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable
        Try

            objSQLCmd = New SqlCommand("[Rep_ProductivityPerMSLLine]", objSQLConn)
            objSQLCmd.CommandTimeout = 900
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@OID", param1)
            objSQLCmd.Parameters.AddWithValue("@SID", param2)
            objSQLCmd.Parameters.AddWithValue("@Uid", param3)
            objSQLCmd.Parameters.AddWithValue("@Type", "C")
            objSQLCmd.Parameters.AddWithValue("@InvID", param4)

            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "VanLogTbl")
            ProdcutivityperMSL = MsgDs
            objSQLCmd.Dispose()
        Catch ex As Exception

            Throw ex
        Finally
            objSQLCmd = Nothing
            objSQLConn.Close()
            objSQLConn = Nothing
        End Try

        For Each dr As DataRow In MsgDs.Tables(0).Rows
            oDashBoardSalesbyAgency = New DashBoardSales
            oDashBoardSalesbyAgency.Description = dr("Product").ToString
            oDashBoardSalesbyAgency.Amount = Val(dr("PM2").ToString)
            oDashBoardSalesbyAgency.ReturnAmount = Val(dr("PM1").ToString)
            oDashBoardSalesbyAgency.Amount1 = Val(dr("PM").ToString)
            DashBoardSalesbyAgency.Add(oDashBoardSalesbyAgency)
            oDashBoardSalesbyAgency = Nothing
        Next

        Return DashBoardSalesbyAgency
    End Function

    Private Class DelayedPayment
        Private t_Date As New ArrayList
        Private t_Per As New ArrayList
        Private t_OrgName As String

        Public Property OrgName() As String
            Get
                OrgName = t_OrgName
            End Get
            Set(ByVal value As String)
                t_OrgName = value
            End Set
        End Property
        Public ReadOnly Property Mdate() As ArrayList
            Get
                Mdate = t_Date
            End Get
        End Property
        Public ReadOnly Property Percentage() As ArrayList
            Get
                Percentage = t_Per
            End Get
        End Property
    End Class


    <System.Web.Services.WebMethod()> _
<System.Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=False)> _
    Public Function getSMLast3MonthsTargetVsAchiev(ByVal param As String, param1 As String, param2 As String) As Object
        Dim TargetvsSalesDataList As New List(Of TargetVsAchiv)()
        Dim myConnection As New SqlConnection(_strSQLConn)

        Const sql As String = "Rep_SMLast3MonthsTargetVsAchiev"
        Dim myCommand As New SqlCommand(sql, myConnection)
        myConnection.Open()


        Dim objSQLDA As New SqlDataAdapter()
        objSQLDA.SelectCommand = myCommand
        objSQLDA.SelectCommand.CommandType = CommandType.StoredProcedure
        objSQLDA.SelectCommand.CommandTimeout = 600
        objSQLDA.SelectCommand.Parameters.AddWithValue("@OID", param)
        objSQLDA.SelectCommand.Parameters.AddWithValue("@SMID", IIf(String.IsNullOrEmpty(param1), "-1", param1))
        objSQLDA.SelectCommand.Parameters.AddWithValue("@MonthYear", param2)

        Dim dSet As New DataSet()
        objSQLDA.Fill(dSet)
        '  myCommand.ExecuteNonQuery()
        myConnection.Close()
        'Dim tot As Decimal
        'For Each row As DataRow In dSet.Tables(0).Rows

        '    tot += CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))

        'Next




       


        Dim dt_clone As DataTable = New DataTable()
        dt_clone = dSet.Tables(0).Clone()

        Dim dt_TA As New DataTable
        dt_TA.Columns.Add("MonYear", GetType(String))
        dt_TA.Columns.Add("Description", GetType(String))
        dt_TA.Columns.Add("TargentValue", GetType(String))
        dt_TA.Columns.Add("AchValue", GetType(String))
        dt_TA.Columns.Add("TotalCalls", GetType(String))
        dt_TA.Columns.Add("ProductiveCalls", GetType(String))
        dt_TA.Columns.Add("MonthYear", GetType(DateTime))
        dt_TA.Columns.Add("Type", GetType(String))



        For Each row As DataRow In dSet.Tables(0).Rows

            Dim result() As DataRow = dt_TA.Select("MonYear = '" & CStr(row("MonthYear")) & "'")
            If result.Count = 0 Then
                Dim rw As DataRow = dt_TA.NewRow
                rw("MonYear") = CStr(row("MonthYear"))
                rw("MonthYear") = CDate(row("MonthY"))
                If CStr(row("Description")).Contains("Target") And Val(CStr(row("DispOrder"))) = 1 Then
                    rw("TargentValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                If CStr(row("Description")).Contains("Achievement") And Val(CStr(row("DispOrder"))) = 2 Then
                    rw("AchValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                If CStr(row("Description")).Contains("Total") And Val(CStr(row("DispOrder"))) = 3 Then
                    rw("TotalCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                If CStr(row("Description")).Contains("Productive") And Val(CStr(row("DispOrder"))) = 4 Then
                    rw("ProductiveCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                dt_TA.Rows.Add(rw)
            Else
                If CStr(row("Description")).Contains("Target") And Val(CStr(row("DispOrder"))) = 1 Then
                    result(0)("TargentValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                If CStr(row("Description")).Contains("Achievement") And Val(CStr(row("DispOrder"))) = 2 Then
                    result(0)("AchValue") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If

                If CStr(row("Description")).Contains("Total") And Val(CStr(row("DispOrder"))) = 3 Then
                    result(0)("TotalCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If
                If CStr(row("Description")).Contains("Productive") And Val(CStr(row("DispOrder"))) = 4 Then
                    result(0)("ProductiveCalls") = CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))
                End If


            End If




            ' ''If CStr(row("Description")).ToUpper.Contains("CALL") Then
            ' ''    TargetvsSalesDataList.Add(New TargetVsAchiv(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0", "Calls", "", CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString()))))
            ' ''Else
            ' ''    TargetvsSalesDataList.Add(New TargetVsAchiv(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0", "TA", CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), ""))

            ' ''End If
            '  TargetvsSalesDataList.Add(New TargetVsAchiv(CStr(row("MnOrder")), CDec(IIf(row("TotValue") Is DBNull.Value, "0", row("TotValue").ToString())), CStr(row("Description")), CStr(row("DispOrder")), CDate(row("MonthYear")), "0"))


        Next
        Dim n As Integer = 0

        'Data for test 
        dt_TA.Rows(0)(2) = 2500
        'Data for test 
        dt_TA.Rows(0)(3) = 3500
        'Data for test 
        dt_TA.Rows(0)(4) = 15
        'Data for test 
        dt_TA.Rows(0)(5) = 10


        'Data for test 
        dt_TA.Rows(1)(2) = 5500
        'Data for test 
        dt_TA.Rows(1)(3) = 7500
        'Data for test 
        dt_TA.Rows(1)(4) = 25
        'Data for test 
        dt_TA.Rows(1)(4) = 35

        'Data for test 
        Dim R As DataRow = dt_TA.NewRow

        'Data for test 
        R(0) = "Jul-2018"
        'Data for test 
        R(2) = 3000
        'Data for test 
        R(3) = 4000
        'Data for test 
        R(4) = 35
        'Data for test 
        R(5) = 30
        'Data for test 
        R("MonthYear") = "07-01-2018"
        'Data for test 
        dt_TA.Rows.Add(R)

        For Each row As DataRow In dt_TA.Rows
            n = n + 1
            TargetvsSalesDataList.Add(New TargetVsAchiv(CStr(row("MonYear")), "", CDec(IIf(row("TargentValue") Is DBNull.Value, "0", row("TargentValue").ToString())), CDec(IIf(row("AchValue") Is DBNull.Value, "0", row("AchValue").ToString())), CDec(IIf(row("TotalCalls") Is DBNull.Value, "0", row("TotalCalls").ToString())), CDec(IIf(row("ProductiveCalls") Is DBNull.Value, "0", row("ProductiveCalls").ToString())), CDate(row("MonthYear")), CStr(n)))

        Next

     
        If TargetvsSalesDataList.Count = 0 Then
            TargetvsSalesDataList.Add(New TargetVsAchiv("No Data", 0, "No data", "0", "01-01-1900", "0", "", ""))
        End If
        Return TargetvsSalesDataList
    End Function

End Class
