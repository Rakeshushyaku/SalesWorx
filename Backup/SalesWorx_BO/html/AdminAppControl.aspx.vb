Imports SalesWorx.BO.Common
Imports log4net
Imports Telerik.Web.UI

Partial Public Class AdminAppControl
    Inherits System.Web.UI.Page

    Dim objControl As New SalesWorx.BO.Common.AppControl
    Dim Err_No As Long
    Dim Err_Desc As String
    Private Const ModuleName As String = "AdminAppCOntrol.aspx"
    Private Const PageID As String = "P56"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session.Item("USER_ACCESS") Is Nothing Then
                    Session.Add("BringmeBackHere", ModuleName)
                    Response.Redirect("Login.aspx", False)
                    Exit Sub
                End If
                If Not IsPostBack Then
                    Dim HasPermission As Boolean = False
                    ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
                    If Not HasPermission Then
                        Err_No = 500
                        Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
                    End If
                    LoadAppControlParams()
                    BindData()
                End If
            End If
        Catch ex As Exception
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub

    Private Sub BindData()

        TreeView1.Nodes.Clear()
        Dim z As New DataTable


        z = objControl.LoadParentNode(Err_No, Err_Desc)

        Dim success As Boolean = False
        For Each dr As DataRow In z.Rows
            success = True
            Dim tnMainSubject As TreeNode = New TreeNode(dr("Value"), dr("ParentID"))
            tnMainSubject.SelectAction = TreeNodeSelectAction.None
            TreeView1.Nodes.Add(tnMainSubject)
            tnMainSubject.ShowCheckBox = False
            addSubSubject(tnMainSubject, dr("ParentID").ToString())
        Next
        TreeView1.ExpandAll()
    End Sub

    Private Sub addSubSubject(ByVal ParentNode As TreeNode, ByVal ParentId As String)

        Dim y As New DataTable


        y = objControl.LoadAppControlParams(Err_No, Err_Desc, ParentId)
        Dim sucess As Boolean = False
        For Each f As DataRow In y.Rows
            sucess = True
            Dim tnSubSubject As TreeNode = New TreeNode(f("ChildText"), f("ChildName"))
            tnSubSubject.SelectAction = TreeNodeSelectAction.None
            tnSubSubject.Checked = IIf(f("ChildId").ToString() = "Y", True, False)
            ParentNode.ChildNodes.Add(tnSubSubject)
        Next

    End Sub

    Private Sub LoadAppControlParams()
        Dim versionno As String = AppInfo.GetVersion()
        Dim y As New DataTable
        y = objControl.LoadAppControlParams(Err_No, Err_Desc, "ALL")
        If y.Rows.Count > 0 Then





            'chkParams.DataTextField = "Control_Key"
            'chkParams.DataValueField = "Control_Value"
            'chkParams.DataSource = y
            'chkParams.DataBind()

            'For Each li As ListItem In chkParams.Items
            '    If li.Value = "Y" Then
            '        li.Checked = True
            '    End If

            'Next

        End If

        Dim x As New DataTable
        x = objControl.LoadOtherAppControlParams(Err_No, Err_Desc, versionno)
        If x.Rows.Count > 0 Then
            For Each dr As DataRow In x.Rows



                If dr("Control_key").ToString() = "COLLECTION_MODE" Then
                    Me.ddlCollectMode.SelectedValue = dr("Control_Value").ToString()
                End If




                If dr("Control_key").ToString() = "RETURN_STOCK_MERGE_MODE" Then
                    Me.ddlCRMergeMode.SelectedValue = dr("Control_Value").ToString()
                End If
                If dr("Control_key").ToString() = "VAN_LOAD_TYPE" Then
                    Me.ddlVanLoad.SelectedValue = dr("Control_Value").ToString()
                End If
                If dr("Control_key").ToString() = "VAN_UNLOAD_TYPE" Then
                    Me.ddlUnLoad.SelectedValue = dr("Control_Value").ToString()
                End If

                If dr("Control_key").ToString() = "DISCOUNT_MODE" Then
                    Me.ddlDiscountMode.SelectedValue = dr("Control_Value").ToString()
                End If



                If dr("Control_key").ToString() = "CN_LIMIT_MODE" Then
                    Me.ddlCNLimitMode.SelectedValue = dr("Control_Value").ToString()
                End If


                If dr("Control_key").ToString() = "DISC_LIMIT_MIN" Then
                    Me.txtDisMinLimit.Text = dr("Control_Value").ToString()
                End If

                If dr("Control_key").ToString() = "DISC_LIMIT_MAX" Then
                    Me.txtDisMaxLimit.Text = dr("Control_Value").ToString()
                End If


                If dr("Control_key").ToString() = "UNLOAD_QTY_CHANGE_MODE" Then
                    Dim s() As String = dr("Control_Value").ToString().Split(",")
                    If Not s Is Nothing Then
                        If s.Length > 0 Then
                            For i As Integer = 0 To s.Length - 1


                                For Each itm As RadComboBoxItem In chkUnloadChange.Items
                                    If itm.Value = s(i).ToString() Then
                                        itm.Checked = True
                                    End If
                                Next
                                'For Each li As ListItem In chkUnloadChange.Items
                                '    If li.Value = s(i).ToString() Then
                                '        li.Selected = True
                                '        Exit For
                                '    End If
                                'Next
                            Next
                        End If

                    End If
                End If




            Next


        End If









    End Sub
    'Sub Loaddetails()
    '    Try


    '        objControl.PathToXMLFile = Server.MapPath("..\xml\ControlParams.xml")
    '        ControlParamsTree.Text = objControl.GetControlParamsTree()
    '        objControl.LoadOtherParams()
    '        Route_Sync_Days.Text = objControl._iRouteSyncDays.ToString()
    '        txtOrdPending.Text = objControl._iOrdPending.ToString()
    '        txtRMADays.Text = objControl._iRMADays.ToString()
    '        txtRMAlimit.Text = objControl._iRMALimit.ToString()
    '        txtOverDue.Text = objControl._iOverDueLimit.ToString()

    '        If objControl._iCLOD.ToString() = "Y" Then
    '            chkCL.Checked = True
    '        Else
    '            chkCL.Checked = False
    '        End If

    '        If objControl._iFC.ToString() = "Y" Then
    '            chkFC.Checked = True
    '        Else
    '            chkFC.Checked = False
    '        End If
    '        If objControl._iOD.ToString() = "Y" Then
    '            chkOD.Checked = True
    '        Else
    '            chkOD.Checked = False
    '        End If
    '        If objControl._iAutoBlock.ToString() = "Y" Then
    '            chkBlock.Checked = True
    '        Else
    '            chkBlock.Checked = False
    '        End If

    '        'RoutePlan_Due_Date.Text = objControl._iRoutePlanDueDate.ToString()
    '    Catch ex As Exception
    '        log.Error(GetExceptionInfo(ex))
    '    End Try
    'End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Try



            Dim ALLOW_EXCESS_CASH_COLLECTION As String = "0"
            Dim ALLOW_LOAD_QTY_CHANGE As String = "0"
            Dim ALLOW_ORDER_DISCOUNT As String = "0"
            Dim ALLOW_PARTIAL_UNLOAD As String = "0"
            Dim ALLOW_UNLOAD_QTY_CHANGE As String = "0"
            Dim CN_LIMIT_MODE As String = "0"
            Dim COLLECTION_MODE As String = "0"
            Dim DISCOUNT_MODE As String = "0"
            Dim ENABLE_COLLECTIONS As String = "0"
            Dim ENABLE_CUSTOMER_SIGNATURE As String = "0"
            Dim ENABLE_DISTRIB_CHECK As String = "0"
            Dim ENABLE_LOT_SELECTION As String = "0"
            Dim ENABLE_MARKET_SURVEY As String = "0"
            Dim ENABLE_ORDER_HISTORY As String = "0"
            Dim ENABLE_SHORT_DOC_REF As String = "0"
            Dim EOD_ON_UNLOAD As String = "0"
            Dim MERGE_RETURN_STOCK As String = "0"
            Dim OPTIONAL_RETURN_HDR_REASON As String = "0"
            Dim OPTIONAL_RETURN_LOT As String = "0"
            Dim RETURN_STOCK_MERGE_MODE As String
            Dim UNLOAD_QTY_CHANGE_MODE As String
            Dim VAN_LOAD_TYPE As String = "0"
            Dim VAN_UNLOAD_TYPE As String = "0"
            Dim DISC_LIMIT_MIN As String = "0"
            Dim DISC_LIMIT_MAX As String = "0"

            COLLECTION_MODE = Me.ddlCollectMode.SelectedValue
            RETURN_STOCK_MERGE_MODE = Me.ddlCRMergeMode.SelectedValue
            VAN_LOAD_TYPE = Me.ddlVanLoad.SelectedValue
            VAN_UNLOAD_TYPE = Me.ddlUnLoad.SelectedValue
            DISCOUNT_MODE = Me.ddlDiscountMode.SelectedValue
            CN_LIMIT_MODE = Me.ddlCNLimitMode.SelectedValue

            DISC_LIMIT_MIN = IIf(Me.txtDisMinLimit.Text = "", "0", Me.txtDisMinLimit.Text)
            DISC_LIMIT_MAX = IIf(Me.txtDisMaxLimit.Text = "", "0", Me.txtDisMaxLimit.Text)

            Dim s As String = Nothing
            'For Each li As ListItem In chkUnloadChange.Items
            '    If li.Selected = True Then
            '        s = s + li.Value.ToString() + ","
            '    End If
            'Next

            For Each itm As RadComboBoxItem In chkUnloadChange.Items
                If itm.Checked = True Then
                    s = s + itm.Value.ToString() + ","
                End If
            Next

            If Not s Is Nothing Then
                If s.Length > 0 Then
                    s = s.Remove(s.Length - 1, 1).Trim()
                End If
            End If

            UNLOAD_QTY_CHANGE_MODE = IIf(s Is Nothing, "0", s)

            For Each p As TreeNode In TreeView1.Nodes
                For Each li As TreeNode In p.ChildNodes
                    If li.Checked = True And li.Value = "ALLOW_EXCESS_CASH_COLLECTION" Then
                        ALLOW_EXCESS_CASH_COLLECTION = "Y"
                    ElseIf li.Checked = False And li.Value = "ALLOW_EXCESS_CASH_COLLECTION" Then
                        ALLOW_EXCESS_CASH_COLLECTION = "N"
                    End If


                    If li.Checked = True And li.Value = "ALLOW_LOAD_QTY_CHANGE" Then
                        ALLOW_LOAD_QTY_CHANGE = "Y"
                    ElseIf li.Checked = False And li.Value = "ALLOW_LOAD_QTY_CHANGE" Then
                        ALLOW_LOAD_QTY_CHANGE = "N"
                    End If


                    If li.Checked = True And li.Value = "ALLOW_ORDER_DISCOUNT" Then
                        ALLOW_ORDER_DISCOUNT = "Y"
                    ElseIf li.Checked = False And li.Value = "ALLOW_ORDER_DISCOUNT" Then
                        ALLOW_ORDER_DISCOUNT = "N"
                    End If


                    If li.Checked = True And li.Value = "ALLOW_PARTIAL_UNLOAD" Then
                        ALLOW_PARTIAL_UNLOAD = "Y"
                    ElseIf li.Checked = False And li.Value = "ALLOW_PARTIAL_UNLOAD" Then
                        ALLOW_PARTIAL_UNLOAD = "N"
                    End If


                    If li.Checked = True And li.Value = "ALLOW_UNLOAD_QTY_CHANGE" Then
                        ALLOW_UNLOAD_QTY_CHANGE = "Y"
                    ElseIf li.Checked = False And li.Value = "ALLOW_UNLOAD_QTY_CHANGE" Then
                        ALLOW_UNLOAD_QTY_CHANGE = "N"
                    End If


                    If li.Checked = True And li.Value = "CN_LIMIT_MODE" Then
                        CN_LIMIT_MODE = "Y"
                    ElseIf li.Checked = False And li.Value = "CN_LIMIT_MODE" Then
                        CN_LIMIT_MODE = "N"
                    End If

                    If li.Checked = True And li.Value = "ENABLE_COLLECTIONS" Then
                        ENABLE_COLLECTIONS = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_COLLECTIONS" Then
                        ENABLE_COLLECTIONS = "N"
                    End If

                    If li.Checked = True And li.Value = "ENABLE_CUSTOMER_SIGNATURE" Then
                        ENABLE_CUSTOMER_SIGNATURE = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_CUSTOMER_SIGNATURE" Then
                        ENABLE_CUSTOMER_SIGNATURE = "N"
                    End If

                    If li.Checked = True And li.Value = "ENABLE_DISTRIB_CHECK" Then
                        ENABLE_DISTRIB_CHECK = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_DISTRIB_CHECK" Then
                        ENABLE_DISTRIB_CHECK = "N"
                    End If


                    If li.Checked = True And li.Value = "ENABLE_LOT_SELECTION" Then
                        ENABLE_LOT_SELECTION = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_LOT_SELECTION" Then
                        ENABLE_LOT_SELECTION = "N"
                    End If

                    If li.Checked = True And li.Value = "ENABLE_MARKET_SURVEY" Then
                        ENABLE_MARKET_SURVEY = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_MARKET_SURVEY" Then
                        ENABLE_MARKET_SURVEY = "N"
                    End If


                    If li.Checked = True And li.Value = "ENABLE_ORDER_HISTORY" Then
                        ENABLE_ORDER_HISTORY = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_ORDER_HISTORY" Then
                        ENABLE_ORDER_HISTORY = "N"
                    End If

                    If li.Checked = True And li.Value = "ENABLE_SHORT_DOC_REF" Then
                        ENABLE_SHORT_DOC_REF = "Y"
                    ElseIf li.Checked = False And li.Value = "ENABLE_SHORT_DOC_REF" Then
                        ENABLE_SHORT_DOC_REF = "N"
                    End If

                    If li.Checked = True And li.Value = "EOD_ON_UNLOAD" Then
                        EOD_ON_UNLOAD = "Y"
                    ElseIf li.Checked = False And li.Value = "EOD_ON_UNLOAD" Then
                        EOD_ON_UNLOAD = "N"
                    End If

                    If li.Checked = True And li.Value = "MERGE_RETURN_STOCK" Then
                        MERGE_RETURN_STOCK = "Y"
                    ElseIf li.Checked = False And li.Value = "MERGE_RETURN_STOCK" Then
                        MERGE_RETURN_STOCK = "N"
                    End If


                    If li.Checked = True And li.Value = "OPTIONAL_RETURN_HDR_REASON" Then
                        OPTIONAL_RETURN_HDR_REASON = "Y"
                    ElseIf li.Checked = False And li.Value = "OPTIONAL_RETURN_HDR_REASON" Then
                        OPTIONAL_RETURN_HDR_REASON = "N"
                    End If



                    If li.Checked = True And li.Value = "OPTIONAL_RETURN_LOT" Then
                        OPTIONAL_RETURN_LOT = "Y"
                    ElseIf li.Checked = False And li.Value = "OPTIONAL_RETURN_LOT" Then
                        OPTIONAL_RETURN_LOT = "N"
                    End If



                    If li.Checked = True And li.Value = "RETURN_STOCK_MERGE_MODE" Then
                        RETURN_STOCK_MERGE_MODE = "Y"
                    ElseIf li.Checked = False And li.Value = "RETURN_STOCK_MERGE_MODE" Then
                        RETURN_STOCK_MERGE_MODE = "N"
                    End If


                Next

            Next





            If objControl.UpdateAppControlParams(Err_No, Err_Desc, CType(Session.Item("USER_ACCESS"), UserAccess).UserID, ALLOW_EXCESS_CASH_COLLECTION, ALLOW_LOAD_QTY_CHANGE, ALLOW_ORDER_DISCOUNT, ALLOW_PARTIAL_UNLOAD, ALLOW_UNLOAD_QTY_CHANGE, CN_LIMIT_MODE, COLLECTION_MODE, DISCOUNT_MODE, ENABLE_COLLECTIONS, ENABLE_CUSTOMER_SIGNATURE, ENABLE_DISTRIB_CHECK, ENABLE_LOT_SELECTION, ENABLE_MARKET_SURVEY, ENABLE_ORDER_HISTORY, ENABLE_SHORT_DOC_REF, EOD_ON_UNLOAD, MERGE_RETURN_STOCK, OPTIONAL_RETURN_HDR_REASON, OPTIONAL_RETURN_LOT, RETURN_STOCK_MERGE_MODE, UNLOAD_QTY_CHANGE_MODE, VAN_LOAD_TYPE, VAN_UNLOAD_TYPE, DISC_LIMIT_MIN, DISC_LIMIT_MAX) Then
                MessageBoxInfo("Control parameters updated successfully")
                BindData()
                LoadAppControlParams()
            Else
                MessageBoxValidation("Error occured while updating control parameter.")
            End If












            'If IsNumeric(_Route_Sync_Days) Then
            '    If IsNumeric(_RoutePlan_Due_Date) Then ' AndAlso _RoutePlan_Due_Date > 0 AndAlso _RoutePlan_Due_Date <= 15 Then
            '        objControl._iRouteSyncDays = _Route_Sync_Days
            '        objControl._iRoutePlanDueDate = 0 ' _RoutePlan_Due_Date
            '        objControl._iOrdPending = CStr(IIf(Me.txtOrdPending.Text = "", "10", Me.txtOrdPending.Text))
            '        objControl._iFC = IIf(Me.chkFC.Checked = True, "Y", "N")
            '        objControl._iCLOD = IIf(Me.chkCL.Checked = True, "Y", "N")
            '        objControl._iOD = IIf(Me.chkOD.Checked = True, "Y", "N")
            '        objControl._iRMADays = CStr(IIf(Me.txtRMADays.Text = "", "10", Me.txtRMADays.Text))
            '        objControl._iRMALimit = CStr(IIf(Me.txtRMAlimit.Text = "", "10000", Me.txtRMAlimit.Text))
            '        objControl._iAutoBlock = IIf(Me.chkBlock.Checked = True, "Y", "N")
            '        objControl._iOverDueLimit = CStr(IIf(Me.txtOverDue.Text = "", "1000", Me.txtOverDue.Text))
            '        objControl.UpdateOtherParams()
            '        If objControl.UpdateControlParams(Request.Form("POINT"), Request.Form("SUB_POINT"), Err_No, Err_Desc) Then
            '            MessageBoxInfo("Control parameters updated successfully")
            '            Loaddetails()
            '            'Dim lRetVal As Long = 0
            '            'lRetVal = objControl.GetControlParams()
            '            'If objControl.IsCPSelected(lRetVal, "CP30") = True Then
            '            '    ConfigurationSettings.AppSettings("OverLimit") = "True"
            '            'Else
            '            '    ConfigurationSettings.AppSettings("OverLimit") = "False"
            '            'End If
            '            'If objControl.IsCPSelected(lRetVal, "CP31") = True Then
            '            '    ConfigurationSettings.AppSettings("OverStock") = "True"
            '            'Else
            '            '    ConfigurationSettings.AppSettings("OverStock") = "False"
            '            'End If
            '        Else
            '            MessageBoxValidation("Error occured while updating control parameter.")
            '        End If
            '    Else
            '        MessageBoxValidation("Invalid plan due date")
            '    End If
            'Else
            '    MessageBoxValidation("Invalid route sync days")
            'End If
        Catch ex As Exception
            MessageBoxValidation("Error occured while updating application control parameters.")
            log.Error(GetExceptionInfo(ex))
        End Try
    End Sub


    Sub MessageBoxValidation(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Red
        lblinfo.Text = "Validation"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
    Sub MessageBoxInfo(ByVal str As String)
        lblMessage.ForeColor = Drawing.Color.Green
        lblinfo.Text = "Information"
        lblMessage.Text = str
        MpInfoError.Show()
        Exit Sub
    End Sub
End Class