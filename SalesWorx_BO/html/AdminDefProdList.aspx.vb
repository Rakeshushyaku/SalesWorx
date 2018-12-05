Imports SalesWorx.BO.Common
Imports log4net

Partial Public Class AdminDefProdList
    Inherits System.Web.UI.Page
    Dim objProduct As New Product
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Sub AdminDefProdList_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Title = "Setup Default Product List"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        Try
            If Not IsPostBack Then
                Dim objCommon As New Common
                Dim SubQry As String = objCommon.GetSalesRepQry(CType(Session("User_Access"), UserAccess).UserID)
                ddOraganisation.DataSource = objProduct.GetOrganisation(Err_No, Err_Desc, SubQry)
                ddOraganisation.Items.Clear()
                ddOraganisation.Items.Add("-- Select --")
                ddOraganisation.AppendDataBoundItems = True
                ddOraganisation.DataValueField = "MAS_Org_ID"
                ddOraganisation.DataTextField = "Description"
                ddOraganisation.DataBind()

                lstDefault.Items.Clear()
                lstSelected.Items.Clear()

                ''Bind Default List

            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try

    End Sub

    Private Sub ddOraganisation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddOraganisation.SelectedIndexChanged
        Try
            If ddOraganisation.SelectedItem.Text <> "-- Select --" Then
                ''Bind Default List
                BindDefault()

                ''Bind Selected List
                BindSelected()

                lblProdAssign.Text = "Products Assigned: [" & ddOraganisation.SelectedItem.Value & "]"
            Else
                lstDefault.Items.Clear()
                lstSelected.Items.Clear()
                lblProdAssign.Text = ""
            End If
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=Welcome.aspx&Title=Product List Setup", False)
        End Try
    End Sub
    Private Sub BindDefault()
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objProd.GetDefault(ddOraganisation.SelectedItem.Value, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = TempTbl
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "Inventory_Item_ID"
                lstDefault.DataBind()
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Private Sub BindSelected()
        Dim objProd As New Product
        Dim TempTbl As New DataTable
        Try
            Err_Desc = Nothing
            Err_No = Nothing
            TempTbl = objProd.GetSelected(ddOraganisation.SelectedItem.Value, Err_No, Err_Desc)
            If TempTbl IsNot Nothing Then
                lstDefault.DataSource = TempTbl
                lstDefault.DataTextField = "Description"
                lstDefault.DataValueField = "Inventory_Item_ID"
                lstDefault.DataBind()
            End If

        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
            Throw ex
        Finally
            objProd = Nothing
        End Try
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            For Each Item As ListItem In lstDefault.Items
                If Item.Selected Then
                    Dim objProd As New Product
                    Try
                        Err_No = Nothing
                        Err_Desc = Nothing
                        objProd.InsertProduct(ddOraganisation.SelectedItem.Value, Item.Value, Err_No, Err_Desc)
                    Catch ex As Exception
                        If Err_Desc IsNot Nothing Then
                            log.Error(Err_Desc)
                        Else
                            log.Error(GetExceptionInfo(ex))
                        End If
                    End Try
                End If
            Next
        Catch ex As Exception
            log.Error(ex.Message)
            Response.Redirect("Information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_RoutePlanner_005") & "&next=AdminDefProdList.aspx&Title=Product List Setup", False)
        End Try
    End Sub
End Class