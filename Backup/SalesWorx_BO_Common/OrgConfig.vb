Public Class OrgConfig
#Region "Private Fields"

    Private _sOrg_ID As String = ""
    Private _sMas_Org_ID As String = ""
    Private _sSales_org_ID As String = ""
    Private _sEmp_Code As String = ""
    Dim objDivConfig As New DAL.DAL_DivConfig

#End Region
#Region "Accessors"
    Public Property Org_ID() As String
        Get
            Return Me._sOrg_ID
        End Get

        Set(ByVal value As String)
            Me._sOrg_ID = value
        End Set
    End Property


    Public Property Stock_Org_ID() As String
        Get
            Return Me._sMas_Org_ID
        End Get

        Set(ByVal value As String)
            Me._sMas_Org_ID = value
        End Set
    End Property


    Public Property Sales_org_ID() As String
        Get
            Return Me._sSales_org_ID
        End Get

        Set(ByVal value As String)
            Me._sSales_org_ID = value
        End Set
    End Property


    Public Property Emp_code() As String
        Get
            Return Me._sEmp_Code
        End Get

        Set(ByVal value As String)
            Me._sEmp_Code = value
        End Set
    End Property
#End Region
    Dim objOrgConfig As New DAL.DAL_OrgConfig

    Function GetDistinctVanOrgs(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return objOrgConfig.GetDistinctVanOrgs(Err_No, Err_Desc)
    End Function
    Function GetDistinctWareHouses(ByRef Err_No As Long, ByRef Err_Desc As String, Optional ByVal SubQuery As String = "") As DataTable
        Return objOrgConfig.GetDistinctWareHouses(Err_No, Err_Desc, SubQuery)
    End Function
    Function GetOrganisation(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal SubQuery As String) As DataTable
        Return objOrgConfig.GetOrganisation(Err_No, Err_Desc, SubQuery)
    End Function
    Public Function GetOrgConfiguration(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal Criteria As String) As DataTable
        Try
            Return objOrgConfig.GetOrgConfig(Err_No, Err_Desc, Criteria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Function GetSalesMan(ByRef Err_No As Long, ByRef Err_Desc As String) As DataTable
        Return objOrgConfig.GetSalesMan(Err_No, Err_Desc)
    End Function
    
    Function GetCurrency(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal OrgID As String) As DataTable
        Return objOrgConfig.GetCurrency(Err_No, Err_Desc, OrgID)
    End Function
    Public Function UpdateOrgConfig(ByRef Err_No As Long, ByRef Err_Desc As String) As Boolean
        Try
            Return objOrgConfig.UpdateOrgConfig(Err_No, Err_Desc, _sOrg_ID, _sMas_Org_ID, _sSales_org_ID, _sEmp_Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
