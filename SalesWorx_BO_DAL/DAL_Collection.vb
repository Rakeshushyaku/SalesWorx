Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Public Class DAL_Collection
    Private _objDB As DatabaseConnection
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Public Function GetHeldPDC(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String, ByVal OrgID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT A.Collection_ID,A.Collection_Ref_No,A.Collected_On,B.SalesRep_Name As Collected_By,A.Amount,A.Cheque_No,A.Cheque_Date,A.Bank_Name,A.Emp_Code,C.Customer_Name,A.Bank_Branch,A.Collection_Type,C.Customer_No  FROM (select * from   TBL_Collection Where   [Status]='W' And Collection_Type='PDC')  AS A inner  JOIN TBL_FSR As B ON B.SalesRep_ID=A.Collected_By inner join TBL_Org_CTL_DTL D on D.SalesRep_ID=B.SalesRep_ID LEFT JOIN [dbo].[app_GetOrgCustomersFromMultiOrgs](@OrgID) As C ON C.Customer_ID=A.Customer_ID And C.Site_Use_ID=A.Site_Use_ID Where D.MAS_Org_ID in(Select item from dbo.SplitQuotedString('" & OrgID & "')) {0} order by C.Customer_Name Asc", _sSearchParams, QueryStr)

            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CollLstTbl")

            GetHeldPDC = MsgDs
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

    Public Function GetCollectionList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String, ByVal OrgID As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            '  Dim QueryString As String = String.Format("SELECT A.Collection_ID,A.Collection_Ref_No,A.Collected_On,B.SalesRep_Name As Collected_By,A.Amount,A.Cheque_No,A.Cheque_Date,A.Bank_Name,A.Emp_Code,C.Customer_Name,A.Bank_Branch,A.Collection_Type,C.Customer_No  FROM TBL_Collection AS A LEFT JOIN TBL_FSR As B ON B.SalesRep_ID=A.Collected_By LEFT JOIN TBL_Customer As C ON C.Customer_ID=A.Customer_ID And C.Site_Use_ID=A.Site_Use_ID Where 1=1 and [Status]='N' And Collection_Type='PDC' {0} order by C.Customer_Name Asc", _sSearchParams, QueryStr)
            Dim QueryString As String = String.Format("SELECT A.Collection_ID,A.Collection_Ref_No,A.Collected_On,B.SalesRep_Name As Collected_By,A.Amount,A.Cheque_No,A.Cheque_Date,A.Bank_Name,A.Emp_Code,C.Customer_Name,A.Bank_Branch,A.Collection_Type,C.Customer_No  FROM TBL_Collection AS A LEFT JOIN TBL_FSR As B ON B.SalesRep_ID=A.Collected_By LEFT JOIN TBL_Customer_Ship_Address As C ON C.Customer_ID=A.Customer_ID And C.Site_Use_ID=A.Site_Use_ID Where 1=1  and (C.Custom_Attribute_1 IS NULL OR C.Custom_Attribute_1 IN(SELECT CAST([Value] AS VARCHAR)     FROM dbo.Split1(0,@OrgID,','))) AND [Status]='W' And Collection_Type='PDC' {0} order by C.Customer_Name Asc", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.Parameters.AddWithValue("@OrgID", OrgID)
            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CollLstTbl")

            GetCollectionList = MsgDs
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
    Public Function GetCollectionDetailList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sSearchParams As String, ByVal QueryStr As String) As DataSet
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("SELECT Collection_Inv_ID,Collection_ID,Collection_Line_Ref,Invoice_No,Amount,dbo.GetStatusDescription('I',ERP_Status) as ERP_Status from TBL_Collection_Invoices where Collection_ID={0}", _sSearchParams, QueryStr)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text

            Dim MsgDs As New DataSet
            Dim SqlAd As SqlDataAdapter
            SqlAd = New SqlDataAdapter(objSQLCmd)
            SqlAd.Fill(MsgDs, "CollDtlLstTbl")

            GetCollectionDetailList = MsgDs
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

    Public Function ReleaseCollectionList(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal _sCollectionID As String, ByVal _iReleasedBy As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim QueryString As String = String.Format("UPDATE TBL_Collection SET Status ='Y' , Released_By={1} Where Collection_ID = {0} AND Status='W'", _sCollectionID, _iReleasedBy)
            objSQLCmd = New SqlCommand(QueryString, objSQLConn)
            objSQLCmd.CommandType = CommandType.Text
            objSQLCmd.ExecuteNonQuery()
            objSQLCmd.Dispose()
            retVal = True
        Catch ex As Exception
            Err_No = "74061"
            Err_Desc = ex.Message
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
            ReleaseCollectionList = retVal
        End Try

    End Function
End Class
