Imports System.Data.SqlClient
Imports SalesWorx.BO.DAL
Public Class CustomerSegment
Private _sDescription As String
    Private _sID As String
    Private _sCode As String
    Dim ObjCustomerSegment As New DAL_CustomerSegment
    Public Property Description() As String
        Set(ByVal value As String)
            _sDescription = value
        End Set
        Get
            Return _sDescription
        End Get
    End Property
    Public Property ID() As String
        Set(ByVal value As String)
            _sID = value
        End Set
        Get
            Return _sID
        End Get
    End Property
    Public Property Code() As String
        Set(ByVal value As String)
            _sCode = value
        End Set
        Get
            Return _sCode
        End Get
    End Property
    Public Function ManageCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal opt As String) As Boolean
        Try
            Return ObjCustomerSegment.ManageCustomerSegment(Err_No, Err_Desc, _sID, _sDescription, opt, _sCode)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
   Public Function GetCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Dim success As Boolean = False
        Try

            Dim dt As New DataTable
            dt = ObjCustomerSegment.GetCustomerSegment(Err_No, Err_Desc, _sID)
            If dt.Rows.Count > 0 Then
                _sDescription = dt.Rows(0)("Description").ToString()
                success = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function SearchCustomerSegmentGrid(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal FilterBy As String, ByVal FilterValue As String) As DataTable
        Try
            Return ObjCustomerSegment.SearchCustomerSegmentGrid(Err_No, Err_Desc, FilterBy, FilterValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Validatecode(ByVal Customer_Segment_Code As String) As Boolean
        Dim success As Boolean = False
        Try


            success = ObjCustomerSegment.Validatecode(Customer_Segment_Code)

        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function

    Public Function LoadExportCustomerSegmentTemplate() As DataTable
        Try
            Return ObjCustomerSegment.LoadExportCustomerSegmentTemplate()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SaveCustomerSegment(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String, ByVal Code As String) As Boolean
        Try
            Return ObjCustomerSegment.ManageCustomerSegment(Err_No, Err_Desc, "", Description, "1", Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ValidateDescription(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Description As String) As Boolean
        Dim success As Boolean = False
        Try
            success = ObjCustomerSegment.CheckDuplicate(Err_No, Err_Desc, Description)

        Catch ex As Exception
            Throw ex
        End Try
        Return success
    End Function
End Class
