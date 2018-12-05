Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Services
Imports System.Runtime.Serialization
Imports System.IO
Imports System.Web.Script.Serialization
Imports SalesWorx.BO.Common
Partial Public Class Vans
    Inherits System.Web.UI.Page
 Dim ObjCommon As Common
Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim filter As String = Me.Request.QueryString("q")
        Dim orgid As String = Me.Request.QueryString("orgid")
        Dim list As String = Me.GetVans(filter, orgid)
        Me.Response.Clear()
        Me.Response.ContentType = "text/plain"
        Me.Response.Write(list)
        Me.Response.End()
    End Sub
    Public Function GetVans(ByVal filter As String, ByVal OrgId As String) As String
        Dim objUserAccess As UserAccess
        objUserAccess = CType(Session.Item("USER_ACCESS"), UserAccess)
        ObjCommon = New Common()
        Dim VanDt As New DataTable
        VanDt = ObjCommon.GetVanByOrg(Err_No, Err_Desc, OrgId, objUserAccess.UserID.ToString(), filter)
        Dim OVan As New List(Of Orgs)()
        For Each dr As DataRow In VanDt.Rows
        Dim Objvan As New Orgs
        Objvan.name = dr("SalesRep_Name").ToString
        Objvan.id = dr("SalesRep_ID").ToString
        OVan.Add(Objvan)
        Objvan = Nothing
        Next
        Return New JavaScriptSerializer().Serialize(OVan)
    End Function
End Class
