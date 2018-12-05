Imports System.Data.OleDb
Imports System.Configuration
Public Class CustomerProductCode
#Region "Private Fields"

    Private _sCustomerCode As String = ""
    Private _sItemcode As String = ""
    Private _sProductCode As String = ""

    Dim objDivConfig As New DAL.DAL_Customer

#End Region
#Region "Accessors"
    Public Property CustomerCode() As String
        Get
            Return Me._sCustomerCode
        End Get

        Set(ByVal value As String)
            Me._sCustomerCode = value
        End Set
    End Property


    Public Property Itemcode() As String
        Get
            Return Me._sItemcode
        End Get

        Set(ByVal value As String)
            Me._sItemcode = value
        End Set
    End Property


    Public Property ProductCode() As String
        Get
            Return Me._sProductCode
        End Get

        Set(ByVal value As String)
            Me._sProductCode = value
        End Set
    End Property

#End Region
    Dim objCustomer As New DAL.DAL_Customer
    Public Function SaveCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Userid As String) As Boolean
        Try
            Return objCustomer.SaveCustomerProdCode(Err_No, Err_Desc, _sCustomerCode, _sItemcode, _sProductCode, Userid)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable
        Try
            Return objCustomer.GetCustomerProdCode(Err_No, Err_Desc, Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteCustomerProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal CustomerCode As String, ByVal Itemcode As String) As Boolean
        Try
            Return objCustomer.DeleteCustomerProdCode(Err_No, Err_Desc, CustomerCode, Itemcode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteCustomerProdCodes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal idCollection As String) As Boolean
        Try
            Dim customercode As String
            Dim itemcode As String
            Dim customerProdcode() As String
            customerProdcode = idCollection.Split("|")
            Dim bRetvl As Boolean = False
            For i As Integer = 0 To customerProdcode.Length - 1
                If customerProdcode(i).Trim <> "" Then
                    customercode = customerProdcode(i).Split("~")(0)
                    itemcode = customerProdcode(i).Split("~")(1)
                    bRetvl = objCustomer.DeleteCustomerProdCode(Err_No, Err_Desc, customercode, itemcode)
                End If
            Next
            Return bRetvl
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImportFile(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Filename As String, ByVal CustomerCode As String, ByVal Userid As String) As Boolean
        Try
           
            Dim _strXls As String

            Dim sfilename() As String
            sfilename = Filename.Split(".")
            Dim XlsConstr As String
            If sfilename(1).ToUpper = "XLS" Then
                XlsConstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Filename & " ;Extended Properties=""Excel 8.0;HDR=Yes"""
            ElseIf sfilename(1).ToUpper = "XLSX" Then
                XlsConstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Filename & " ;Extended Properties=""Excel 12.0;HDR=Yes"""
            End If
            Dim _XlsCon As New OleDbConnection(XlsConstr)
            _XlsCon.Open()

            Dim sheets As New DataTable
            sheets = _XlsCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
            _strXls = "SELECT * FROM [" & sheets.Rows(0)("TABLE_NAME").ToString & "]"

            Dim objExlDA As OleDbDataAdapter
            Dim dt As New DataTable
            objExlDA = New OleDbDataAdapter(_strXls, _XlsCon)
            objExlDA.Fill(dt)
            objExlDA.Dispose()

            Dim bRetvl As Boolean = False
            For Each dr As DataRow In dt.Rows
                If dr(0).ToString.Trim <> "" And dr(1).ToString.Trim <> "" Then
                    bRetvl = objCustomer.SaveCustomerProdCode(Err_No, Err_Desc, CustomerCode, dr(0).ToString, dr(1).ToString, Userid)
                End If
            Next
            Return bRetvl
        Catch ex As Exception
            Err_Desc = ex.Message
        End Try
    End Function
End Class
