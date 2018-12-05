Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Collections.Specialized
Imports System.Xml

Public Class Login

    Private objDALLogin As New SalesWorx.BO.DAL.DAL_Login
    Private _sUsername As String
    Private _sPassword As String
    Private _objUserAccess As UserAccess
    Private _sPathToXMLFile As String
    Private _objParams As ControlParams
    'Private Shared _BO_DB As String = DatabaseConnection.BODatabase()
    'Private Shared _BO_SYNC_DB As String = DatabaseConnection.BOSyncDatabase()

    Private objControl As New SalesWorx.BO.DAL.DAL_AppControl

    Public WriteOnly Property UserName() As String
        Set(ByVal Value As String)
            _sUserName = Trim(Value)
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal Value As String)
            _sPassword = Trim(Value)
        End Set
    End Property

    Public WriteOnly Property PathToXMLFile() As String
        Set(ByVal Value As String)
            _sPathToXMLFile = Trim(Value)
        End Set
    End Property

    Public Property UserAccess() As UserAccess
        Get
            UserAccess = _objUserAccess
        End Get
        Set(ByVal Value As UserAccess)
            _objUserAccess = Value
        End Set
    End Property

  
    Public Function ValidateUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal mode As String) As Boolean

        Dim retVal As Boolean = False
        Try
            Dim dtUser As DataTable
            dtUser = objDALLogin.ValidateUser(Error_No, Error_Desc, _sUsername, _sPassword, mode)
            If Not dtUser Is Nothing Then
                If dtUser.Rows.Count > 0 Then
                    Dim tempDBVal As Object
                    Dim sUserID As String
                    Dim sSalesRepID As String
                    _objUserAccess = New UserAccess
                    _objUserAccess.MenuID = New ArrayList
                    _objUserAccess.PageID = New ArrayList
                    _objUserAccess.FieldRights = New ArrayList
                    _objUserAccess.AssignedSalesReps = New ArrayList
                    _objUserAccess.VanName = New ArrayList
                    _objUserAccess.Site = New ArrayList
                    _objUserAccess.OrgID = New ArrayList

                    For Each dr As DataRow In dtUser.Rows
                        If IsNothing(sUserID) Then
                            tempDBVal = dr(0)
                            sUserID = IIf(IsDBNull(tempDBVal), "NULL", tempDBVal)
                            tempDBVal = dr(1)
                            sSalesRepID = IIf(IsDBNull(tempDBVal), "NULL", tempDBVal)
                            tempDBVal = dr(5)
                            _objUserAccess.IsSS = tempDBVal ' IIf(tempDBVal = "Y", True, False)
                            _objUserAccess.Designation = tempDBVal
                            'tempDBVal = dr(6)
                            '_objUserAccess.OrgID = IIf(IsDBNull(tempDBVal), System.Configuration.ConfigurationSettings.AppSettings("DEFAULT_ORG_ID"), tempDBVal)
                            'tempDBVal = dr(7)
                            '_objUserAccess.Site = IIf(IsDBNull(tempDBVal), System.Configuration.ConfigurationSettings.AppSettings("DEFAULT_SITE"), tempDBVal)
                            _objUserAccess.PDARights = dr(8)
                            _objUserAccess.UserType = dr(9)

                            _objUserAccess.CurrencyCode = IIf(dr(10) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gCurrency"), dr(10))
                            _objUserAccess.DecimalDigits = "{0:f" & CStr(IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) & "}"

                            If (IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) = 0 Then
                                _objUserAccess.LabelDecimalDigits = "0"
                            ElseIf (IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) = 1 Then
                                _objUserAccess.LabelDecimalDigits = "0.0"
                            ElseIf (IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) = 2 Then
                                _objUserAccess.LabelDecimalDigits = "0.00"
                            ElseIf (IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) = 3 Then
                                _objUserAccess.LabelDecimalDigits = "0.000"
                            ElseIf (IIf(dr(11) Is DBNull.Value, System.Configuration.ConfigurationSettings.AppSettings("gDecimalDigits"), dr(11))) >= 4 Then
                                _objUserAccess.LabelDecimalDigits = "0.0000"
                            End If

                        End If
                        tempDBVal = dr(2)
                        _objUserAccess.MenuID.Add(IIf(IsDBNull(tempDBVal), "NULL", tempDBVal))
                        tempDBVal = dr(3)
                        _objUserAccess.PageID.Add(IIf(IsDBNull(tempDBVal), "NULL", tempDBVal))
                        tempDBVal = dr(4)
                        _objUserAccess.FieldRights.Add(IIf(IsDBNull(tempDBVal), "NULL", tempDBVal))
                    Next
                    _objUserAccess.UserID = sUserID
                    _objUserAccess.SalesRepID = sSalesRepID

                    Dim dtAssignedSalesRep As New DataTable
                    dtAssignedSalesRep = objDALLogin.AssignedSalesRep(sUserID)
                    For Each drRep As DataRow In dtAssignedSalesRep.Rows
                        _objUserAccess.AssignedSalesReps.Add(drRep("SalesRep_ID"))
                        _objUserAccess.VanName.Add(drRep("Org_ID"))
                        _objUserAccess.Site.Add(drRep("Site"))
                        _objUserAccess.OrgID.Add(drRep("MAS_Org_ID"))
                    Next

                    sUserID = Nothing
                    sSalesRepID = Nothing
                    tempDBVal = Nothing
                    retVal = True






                    Dim dtParams As New DataTable
                    dtParams = objControl.InitialiazeAppControlParams(Error_No, Error_Desc)

                    If dtParams.Rows.Count > 0 Then
                        _objParams = New ControlParams
                        For Each dr As DataRow In dtParams.Rows

                            If dr("Control_key").ToString() = "ALLOW_EXCESS_CASH_COLLECTION" Then
                                _objParams.ALLOW_EXCESS_CASH_COLLECTION = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ALLOW_LOAD_QTY_CHANGE" Then
                                _objParams.ALLOW_LOAD_QTY_CHANGE = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ALLOW_ORDER_DISCOUNT" Then
                                _objParams.ALLOW_ORDER_DISCOUNT = dr("Control_Value").ToString()
                            End If


                            If dr("Control_key").ToString() = "ALLOW_PARTIAL_UNLOAD" Then
                                _objParams.ALLOW_PARTIAL_UNLOAD = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "ALLOW_UNLOAD_QTY_CHANGE" Then
                                _objParams.ALLOW_UNLOAD_QTY_CHANGE = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "CN_LIMIT_MODE" Then
                                _objParams.CN_LIMIT_MODE = dr("Control_Value").ToString()
                            End If


                            If dr("Control_key").ToString() = "COLLECTION_MODE" Then
                                _objParams.COLLECTION_MODE = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "DISCOUNT_MODE" Then
                                _objParams.DISCOUNT_MODE = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_COLLECTIONS" Then
                                _objParams.ENABLE_COLLECTIONS = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_CUSTOMER_SIGNATURE" Then
                                _objParams.ENABLE_CUSTOMER_SIGNATURE = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_DISTRIB_CHECK" Then
                                _objParams.ENABLE_DISTRIB_CHECK = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_LOT_SELECTION" Then
                                _objParams.ENABLE_LOT_SELECTION = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "ENABLE_MARKET_SURVEY" Then
                                _objParams.ENABLE_MARKET_SURVEY = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_ORDER_HISTORY" Then
                                _objParams.ENABLE_ORDER_HISTORY = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "ENABLE_SHORT_DOC_REF" Then
                                _objParams.ENABLE_SHORT_DOC_REF = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "EOD_ON_UNLOAD" Then
                                _objParams.EOD_ON_UNLOAD = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "MERGE_RETURN_STOCK" Then
                                _objParams.MERGE_RETURN_STOCK = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "OPTIONAL_RETURN_HDR_REASON" Then
                                _objParams.OPTIONAL_RETURN_HDR_REASON = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "OPTIONAL_RETURN_LOT" Then
                                _objParams.OPTIONAL_RETURN_LOT = dr("Control_Value").ToString()
                            End If

                            If dr("Control_key").ToString() = "RETURN_STOCK_MERGE_MODE" Then
                                _objParams.RETURN_STOCK_MERGE_MODE = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "UNLOAD_QTY_CHANGE_MODE" Then
                                _objParams.UNLOAD_QTY_CHANGE_MODE = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "VAN_LOAD_TYPE" Then
                                _objParams.VAN_LOAD_TYPE = dr("Control_Value").ToString()
                            End If
                            If dr("Control_key").ToString() = "VAN_UNLOAD_TYPE" Then
                                _objParams.VAN_UNLOAD_TYPE = dr("Control_Value").ToString()
                            End If
                        Next
                    Else
                        _objParams.ALLOW_EXCESS_CASH_COLLECTION = "Y"
                        _objParams.ALLOW_LOAD_QTY_CHANGE = "Y"
                        _objParams.ALLOW_ORDER_DISCOUNT = "Y"
                        _objParams.ALLOW_PARTIAL_UNLOAD = "Y"
                        _objParams.ALLOW_UNLOAD_QTY_CHANGE = "Y"
                        _objParams.CN_LIMIT_MODE = "T"
                        _objParams.COLLECTION_MODE = "ALL"
                        _objParams.DISCOUNT_MODE = "P"
                        _objParams.ENABLE_COLLECTIONS = "Y"
                        _objParams.ENABLE_CUSTOMER_SIGNATURE = "Y"
                        _objParams.ENABLE_DISTRIB_CHECK = "Y"
                        _objParams.ENABLE_LOT_SELECTION = "N"
                        _objParams.ENABLE_MARKET_SURVEY = "Y"
                        _objParams.ENABLE_ORDER_HISTORY = "Y"
                        _objParams.ENABLE_SHORT_DOC_REF = "N"
                        _objParams.EOD_ON_UNLOAD = "Y"
                        _objParams.MERGE_RETURN_STOCK = "Y"
                        _objParams.OPTIONAL_RETURN_HDR_REASON = "Y"
                        _objParams.OPTIONAL_RETURN_LOT = "Y"
                        _objParams.RETURN_STOCK_MERGE_MODE = "R"
                        _objParams.UNLOAD_QTY_CHANGE_MODE = "U,E,D,R"
                        _objParams.VAN_LOAD_TYPE = "AGENCY"
                        _objParams.VAN_UNLOAD_TYPE = "AGENCY"
                    End If
























                Else
                    Error_No = 75001
                    Error_Desc = "Invalid user credentials."
                End If
            End If
        Catch ex As Exception
            Error_No = 75001
            Error_Desc = String.Format("Error while validating: {0}", ex.Message)
            Throw ex
        End Try
        Return retVal

    End Function

    Private Function GetMenuName(ByVal node As System.Xml.XmlNode, ByVal constr As String) As String
        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/Menu")
        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = constr Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(0)
                    _strTmp = _Item.Value
                Next
            End If
        Next
        GetMenuName = _strTmp
    End Function

    Private Overloads Function GetPageName(ByVal node As System.Xml.XmlNode, ByVal constr As String) As String

        On Error Resume Next

        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/*/*/Page")

        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = constr Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(0)
                    _strTmp = _Item.Value & """,url:""" & _xmlNode.Attributes(1).Value
                Next
            End If
        Next

        GetPageName = _strTmp
    End Function

    Private Overloads Function GetPageName(ByVal node As System.Xml.XmlNode, ByVal constr As String, ByRef PageURL As String) As String

        On Error Resume Next

        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/*/*/Page")

        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = constr Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(0)
                    _strTmp = _Item.Value
                    PageURL = _xmlNode.Attributes(1).Value
                Next
            End If
        Next

        GetPageName = _strTmp
    End Function

    Public Function GetMenu(ByRef MainMenu As String, ByRef SubMenu As String) As Boolean
        Dim MenuXML As New XmlDocument
        Dim RootNode As XmlNode
        Dim _menuList As XmlNodeList
        Dim _pageList As XmlNodeList, _pageList1 As XmlNodeList
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode
        Dim _Item1 As XmlNode
        Dim sRetMenu As Boolean = False
        Dim sMainFormat As String
        Dim sSubFormat As String
        Dim objControl As AppControl
        Dim lControlParams As Long
        Dim bControlOK As Boolean
        Try
            'retrieving app control params...
            objControl = New AppControl
            lControlParams = objControl.GetControlParams()

            MainMenu = ""
            SubMenu = ""
            MenuXML.Load(_sPathToXMLFile)
            RootNode = MenuXML
            _menuList = RootNode.SelectNodes("*/Menu")

            'main menu formatting.........
            sMainFormat = "overflow=""scroll"";" & vbCrLf
            sMainFormat = sMainFormat & "alwaysvisible=1;" & vbCrLf
            sMainFormat = sMainFormat & "orientation=""horizontal"";" & vbCrLf
            sMainFormat = sMainFormat & "position=""relative"";" & vbCrLf
            sMainFormat = sMainFormat & "style=menuStyle;" & vbCrLf

            'sub menu formatting.........
            sSubFormat = "overflow=""scroll"";" & vbCrLf
            sSubFormat = sSubFormat & "style=submenuStyle;" & vbCrLf

            'applying main menu formatting.....
            MainMenu = "with(milonic=new menuname(""Main Menu"")){" & vbCrLf
            MainMenu = MainMenu & sMainFormat

            Dim sMMenuName As String, sSMenuName As String, sItemURL As String

            'added for grouping of reports based on gid....
            Dim L2SubMenu As String, sGroupID As String, sPrevGroupID As String

            'complete set of L2Menus.....
            Dim L2Menus As String

            For _iIndex = 0 To _menuList.Count - 1
                _xmlNode = _menuList.Item(_iIndex).CloneNode(True)

                bControlOK = False

                'check if control is enabled....
                If Not IsNothing(_xmlNode.Attributes("cpoint")) Then
                    bControlOK = objControl.IsCPSelected(lControlParams, _xmlNode.Attributes("cpoint").Value)
                Else
                    bControlOK = True
                End If

                If bControlOK And _objUserAccess.MenuID.Contains(_xmlNode.Attributes(0).Value) Then
                    sMMenuName = GetMenuName(MenuXML, _xmlNode.Attributes(0).Value)

                    'creating main menu item.....
                    MainMenu = MainMenu & "aI(""status=" & sMMenuName & ";title=" & sMMenuName & ";text=" & sMMenuName & ";showmenu=MID_" & _xmlNode.Attributes(0).Value & ";"");" & vbCrLf
                    'starting corresponding sub menu section.....
                    SubMenu = SubMenu & "with(milonic=new menuname(""MID_" & _xmlNode.Attributes(0).Value & """)){" & vbCrLf

                    'applying sub menu formatting.....
                    SubMenu = SubMenu & sSubFormat

                    _pageList = _xmlNode.ChildNodes
                    For Each _Item In _pageList
                        If _Item.Name = "Pages" Then
                            'resetting L2 menu set....
                            L2Menus = ""

                            _pageList1 = _Item.SelectNodes("Page")
                            For Each _Item1 In _pageList1
                                bControlOK = False

                                'check if control is enabled....
                                If Not IsNothing(_Item1.Attributes("cpoint")) Then
                                    bControlOK = objControl.IsCPSelected(lControlParams, _Item1.Attributes("cpoint").Value)
                                Else
                                    bControlOK = True
                                End If

                                If bControlOK And _objUserAccess.PageID.Contains(_Item1.Attributes(0).Value) Then
                                    sGroupID = _Item1.Attributes(2).Value

                                    sSMenuName = GetPageName(MenuXML, _Item1.Attributes(0).Value, sItemURL)
                                    If sGroupID = "" Then
                                        'resetting any previous L2 menu...
                                        If L2SubMenu <> "" Then
                                            L2Menus = L2Menus & L2SubMenu & "}" & vbCrLf
                                            L2SubMenu = ""
                                        End If

                                        'creating corresponding sub menu items.....
                                        SubMenu = SubMenu & "aI(""status=" & sSMenuName & ";title=" & sSMenuName & ";text=" & sSMenuName & ";url=" & sItemURL & ";"");" & vbCrLf
                                    Else
                                        If L2SubMenu = "" Or sGroupID <> sPrevGroupID Then
                                            If L2SubMenu <> "" Then
                                                L2Menus = L2Menus & L2SubMenu & "}" & vbCrLf
                                                L2SubMenu = ""
                                            End If

                                            L2SubMenu = "with(milonic=new menuname(""MID_L2_" & sGroupID & """)){" & vbCrLf
                                            'applying sub menu formatting.....
                                            L2SubMenu = L2SubMenu & sSubFormat

                                            SubMenu = SubMenu & "aI(""status=" & sGroupID & ";title=" & sGroupID & ";text=" & sGroupID & ";showmenu=MID_L2_" & sGroupID & ";"");" & vbCrLf
                                            L2SubMenu = L2SubMenu & "aI(""status=" & sSMenuName & ";title=" & sSMenuName & ";text=" & sSMenuName & ";url=" & sItemURL & ";"");" & vbCrLf
                                        Else
                                            L2SubMenu = L2SubMenu & "aI(""status=" & sSMenuName & ";title=" & sSMenuName & ";text=" & sSMenuName & ";url=" & sItemURL & ";"");" & vbCrLf
                                        End If
                                    End If

                                    sPrevGroupID = sGroupID
                                End If
                            Next
                        End If
                    Next

                    If L2SubMenu <> "" Then
                        L2Menus = L2Menus & L2SubMenu & "}" & vbCrLf
                        L2SubMenu = ""
                    End If

                    'closing sub menu section..........
                    SubMenu = L2Menus & SubMenu & "}" & vbCrLf

                End If
            Next

            'creating "Logout" main menu item.....
            MainMenu = MainMenu & "aI(""status=Logout;title=Logout;text=Logout;url=Logout.aspx;"");" & vbCrLf

            'finalizing main menu and sub menus...........
            SubMenu = SubMenu & "drawMenus();"
            MainMenu = MainMenu & "}" & vbCrLf & "drawMenus();"

            sRetMenu = True

        Catch ex As Exception
            MainMenu = ex.ToString
        Finally
            MenuXML = Nothing
            RootNode = Nothing
            objControl = Nothing
        End Try
        GetMenu = sRetMenu
    End Function

    Function SaveUserLog(ByRef Err_No As Long, ByRef Err_Desc As String, ByVal TranType As Char, ByVal ModuleName As String, ByVal SubModule As String, ByVal KeyValue As String, ByVal Description As String, ByVal LoggedBy As String, ByVal Van As String, ByVal OrgId As String) As Boolean
        Return objDALLogin.InsertUserLog(Err_No, Err_Desc, TranType, ModuleName, SubModule, KeyValue, Description, LoggedBy, Van, OrgId)
    End Function
    Public Function GetModule() As DataTable
        Dim dt As New DataTable
        dt = objDALLogin.GetModule
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "--Select--"
        dt.Rows.InsertAt(dr, 0)
        Return dt

    End Function


    Public Function GetSubModule() As DataTable
        Dim dt As New DataTable
        dt = objDALLogin.GetSubModule
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "--Select--"
        dt.Rows.InsertAt(dr, 0)
        Return dt

    End Function
End Class
