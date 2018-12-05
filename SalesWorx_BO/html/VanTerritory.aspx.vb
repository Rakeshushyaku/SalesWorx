Imports SalesWorx.BO.DAL

Partial Public Class VanTerritory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadSalesReps()
            LoadCustomersSegments()
            LoadSalesDistricts()
        End If
    End Sub
    Private Sub LoadSalesReps()
        Dim objCustMgmt As New SalesWorx.BO.DAL.DAL_CustomerManagement
        Dim ds As New DataSet
        Dim Err_No As String = ""
        Dim Err_Desc As String = ""
        Try
            If objCustMgmt.ReturnAllSalesRep(ds, Err_No, Err_Desc) Then
                ddlSalesRep.DataSource = ds
                ddlSalesRep.DataTextField = "SalesRep_Name"
                ddlSalesRep.DataValueField = "SalesRep_ID"
                ddlSalesRep.DataBind()
            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
            objCustMgmt = Nothing
        End Try

    End Sub

    Private Sub LoadCustomersSegments()
        Dim objVanTerritory As New DAL_VanTerritory
        Dim ds As New DataSet
        Dim Err_No As String = ""
        Dim Err_Desc As String = ""
        Try
            If objVanTerritory.ReturnAllCustomerSegments(ds, Err_No, Err_Desc) Then
                lstCustomerSegment.DataSource = ds
                lstCustomerSegment.DataTextField = "Description"
                lstCustomerSegment.DataValueField = "Customer_Segment_ID"
                lstCustomerSegment.DataBind()
            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
            objVanTerritory = Nothing
        End Try

    End Sub
    Private Sub LoadSalesDistricts()
        Dim objVanTerritory As New DAL_VanTerritory
        Dim ds As New DataSet
        Dim Err_No As String = ""
        Dim Err_Desc As String = ""
        Try
            If objVanTerritory.ReturnAllSalesDistricts(ds, Err_No, Err_Desc) Then
                lstSalesDistrict.DataSource = ds
                lstSalesDistrict.DataTextField = "Description"
                lstSalesDistrict.DataValueField = "Sales_District_ID"
                lstSalesDistrict.DataBind()
            End If
        Catch ex As Exception
            ' Response.Write(Err_No & "-" & Err_Desc)
        Finally
            ds.Dispose()
            ds = Nothing
            objVanTerritory = Nothing
        End Try

    End Sub
End Class