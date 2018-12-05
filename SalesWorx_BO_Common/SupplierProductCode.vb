Imports System.Data.OleDb
Imports System.Configuration
Public Class SupplierProductCode
#Region "Private Fields"

    Private _sSupplierCode As String = ""
    Private _sItemcode As String = ""
    Private _sProductCode As String = ""

    Dim objSupplier As New DAL.DAL_Product

#End Region
#Region "Accessors"
    Public Property SupplierCode() As String
        Get
            Return Me._sSupplierCode
        End Get

        Set(ByVal value As String)
            Me._sSupplierCode = value
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

    Public Function SaveSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Userid As String, ByVal OrgId As String) As Boolean
        Try
            Return objSupplier.SaveSupplierProdCode(Err_No, Err_Desc, _sSupplierCode, _sItemcode, _sProductCode, Userid, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable
        Try
            Return objSupplier.GetSupplierProdCode(Err_No, Err_Desc, Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteSupplierProdCode(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SupplierCode As String, ByVal Itemcode As String, ByVal OrgId As String) As Boolean
        Try
            Return objSupplier.DeleteSupplierProdCode(Err_No, Err_Desc, SupplierCode, Itemcode, OrgId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteSupplierProdCodes(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal idCollection As String, ByVal orgid As String) As Boolean
        Try
            Dim Suppliercode As String
            Dim itemcode As String
            Dim SupplierProdcode() As String
            SupplierProdcode = idCollection.Split("|")
            Dim bRetvl As Boolean = False
            For i As Integer = 0 To SupplierProdcode.Length - 1
                If SupplierProdcode(i).Trim <> "" Then
                    Suppliercode = SupplierProdcode(i).Split("~")(0)
                    itemcode = SupplierProdcode(i).Split("~")(1)
                    bRetvl = objSupplier.DeleteSupplierProdCode(Err_No, Err_Desc, Suppliercode, itemcode, orgid)
                End If
            Next
            Return bRetvl
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ImportFile(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Filename As String, ByVal Userid As String, ByVal OrgID As String, ByRef ErrorTbl As DataTable) As Boolean
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


            ErrorTbl.Columns.Add("Reason")
            ErrorTbl.Columns.Add("RowNo")

            Dim bRetvl As Boolean = True
            Dim i As Integer = 1

            For Each dr As DataRow In dt.Rows
                If dr(0).ToString.Trim <> "" And dr(1).ToString.Trim <> "" And dr(2).ToString.Trim <> "" Then
                If objSupplier.ValidItemandAgency(OrgID, dr(0).ToString.Trim, dr(1).ToString.Trim) Then
                   Dim bsaved As Boolean = False
                    bsaved = objSupplier.SaveSupplierProdCode(Err_No, Err_Desc, dr(0).ToString, dr(1).ToString, dr(2).ToString, Userid, OrgID)
                    If bRetvl = True Then
                        bRetvl = bsaved
                    End If
                Else
                    bRetvl = False
                    Dim errorRow As DataRow
                    errorRow = ErrorTbl.NewRow
                    errorRow("RowNo") = i.ToString
                    errorRow("Reason") = "Invalid Agency/Item Code "
                    ErrorTbl.Rows.Add(errorRow)
                End If
                Else
                bRetvl = False
                    Dim errorRow As DataRow
                    errorRow = ErrorTbl.NewRow
                    errorRow("RowNo") = i.ToString
                    errorRow("Reason") = "Blank Input value"
                    ErrorTbl.Rows.Add(errorRow)
                End If
                i = i + 1
            Next
            Return bRetvl
        Catch ex As Exception
            Err_Desc = ex.Message
        End Try
    End Function
End Class
