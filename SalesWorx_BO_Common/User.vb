Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Xml

Public Class User


    Private _sUserID As String
    Private _sUsername As String
    Private _sPassword As String
    Private _sOldPassword As String
    Private _sNewPassword As String
    Private _sUserTypeID As String
    Private _sUserType As String
    Private _sUserDesignation As String
    Private _sSalesRepID As String
    Private _sPDARights As String
    Private _alMenuID As ArrayList
    Private _alPageID As ArrayList
    Private _alFieldRights As ArrayList
    Private _alAssignedSalesRep As ArrayList
    Private _sPathToXMLFile As String
    Private _sUserRightsData As String
    Private _sIsSS As String
    Private objDALUser As DAL.DAL_User
    'Private Shared _BO_DB As String = DatabaseConnection.BODatabase()
    'Private Shared _BO_SYNC_DB As String = DatabaseConnection.BOSyncDatabase()
    Private _sOrgID As String
    Public Sub New()
        objDALUser = New DAL.DAL_User
    End Sub

    Public Property OrgID() As String
        Get
            OrgID = _sOrgID
        End Get
        Set(ByVal Value As String)
            _sOrgID = Trim(Value)
        End Set
    End Property
    Public Property UserID() As String
        Get
            UserID = _sUserID
        End Get
        Set(ByVal Value As String)
            _sUserID = Trim(Value)
        End Set
    End Property
    Public Property UserTypeDesignation() As String
        Get
            UserTypeDesignation = _sUserDesignation
        End Get
        Set(ByVal Value As String)
            _sUserDesignation = Trim(Value)
        End Set
    End Property
    Public Property UserName() As String
        Get
            UserName = _sUsername
        End Get
        Set(ByVal Value As String)
            _sUsername = Trim(Value)
        End Set
    End Property

    Public Property Password() As String
        Get
            Password = _sPassword
        End Get
        Set(ByVal Value As String)
            _sPassword = Trim(Value)
        End Set
    End Property

    Public Property UserTypeID() As String
        Get
            UserTypeID = _sUserTypeID
        End Get
        Set(ByVal Value As String)
            _sUserTypeID = Trim(Value)
        End Set
    End Property

    Public Property SalesRepID() As String
        Get
            SalesRepID = _sSalesRepID
        End Get
        Set(ByVal Value As String)
            _sSalesRepID = Trim(Value)
        End Set
    End Property

    Public Property UserType() As String
        Get
            UserType = _sUserType
        End Get
        Set(ByVal Value As String)
            _sUserType = Trim(Value)
        End Set
    End Property

    Public Property PDARights() As String
        Get
            PDARights = _sPDARights
        End Get
        Set(ByVal Value As String)
            _sPDARights = Trim(Value)
        End Set
    End Property
    Public Property AssignedSalesRep() As ArrayList
        Get
            AssignedSalesRep = _alAssignedSalesRep
        End Get
        Set(ByVal Value As ArrayList)
            _alAssignedSalesRep = Value
        End Set
    End Property

    Public ReadOnly Property MenuID() As ArrayList
        Get
            MenuID = _alMenuID
        End Get
    End Property

    Public ReadOnly Property PageID() As ArrayList
        Get
            PageID = _alPageID
        End Get
    End Property

    Public ReadOnly Property FieldRights() As ArrayList
        Get
            FieldRights = _alFieldRights
        End Get
    End Property

    Public WriteOnly Property OldPassword() As String
        Set(ByVal Value As String)
            _sOldPassword = Trim(Value)
        End Set
    End Property

    Public WriteOnly Property NewPassword() As String
        Set(ByVal Value As String)
            _sNewPassword = Trim(Value)
        End Set
    End Property

    Public WriteOnly Property PathToXMLFile() As String
        Set(ByVal Value As String)
            _sPathToXMLFile = Trim(Value)
        End Set
    End Property

    Public WriteOnly Property UserRightsData() As String
        Set(ByVal Value As String)
            _sUserRightsData = Trim(Value)
        End Set
    End Property

    Public Property IsSS() As String
        Get
            IsSS = _sIsSS
        End Get
        Set(ByVal Value As String)
            _sIsSS = Trim(Value)
        End Set
    End Property




    Public Function GetIsSS() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("id")
        dt.Columns.Add("val")

        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "--Select--"
        dt.Rows.Add(dr)
        dr = dt.NewRow()
        dr(0) = "N"
        dr(1) = "FSR"
        dt.Rows.Add(dr)
        dr = dt.NewRow()
        dr(0) = "Y"
        dr(1) = "Supervisor"
        dt.Rows.Add(dr)
        dr = dt.NewRow()
        dr(0) = "M"
        dr(1) = "Sales Manager"
        dt.Rows.Add(dr)
        dr = dt.NewRow()
        dr(0) = "U"
        dr(1) = "User"
        dt.Rows.Add(dr)
        dr = dt.NewRow()
        dr(0) = "A"
        dr(1) = "Administrator"
        dt.Rows.Add(dr)
        Return dt



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

    Private Function GetPageName(ByVal node As System.Xml.XmlNode, ByVal constr As String) As String

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
                Next
            End If
        Next

        GetPageName = _strTmp
    End Function


    Private Function GetFieldName(ByVal node As System.Xml.XmlNode, ByVal constr As String) As String

        On Error Resume Next

        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/*/*/*/*/Field")

        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = constr Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(1)
                    _strTmp = _Item.Value
                Next
            End If
        Next

        GetFieldName = _strTmp
    End Function


    Private Function IsSelected(ByVal NodeType As String, ByRef Items As ArrayList, ByVal Value As String) As Boolean
        IsSelected = False
        If Not IsNothing(Items) Then
            Select Case NodeType
                Case "MENU_ITEM", "PAGE_ITEM"
                    IsSelected = Items.Contains(Value)
                Case "FIELD_ITEM"
                    Dim item As String
                    For Each item In Items
                        If item.IndexOf(String.Format("{0}:1|", Value)) >= 0 Then
                            IsSelected = True
                            Exit Function
                        End If
                    Next
            End Select
        End If
    End Function


    Public Function GetUserRightsTree() As String
        Dim MenuXML As New XmlDocument
        Dim RootNode As XmlNode
        Dim _menuList As XmlNodeList
        Dim _pageList As XmlNodeList, _pageList1 As XmlNodeList
        Dim _fieldList As XmlNodeList, _fieldList1 As XmlNodeList
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item, _Item1 As XmlNode
        Dim _Item2, _Item3 As XmlNode
        Dim sMenuCheckBox As String
        Dim sPageCheckBox As String
        Dim sFieldCheckBox As String
        Dim sMenuID As String
        Dim sPageID As String
        Dim sFieldID As String
        Dim sRetTree As String = ""
        Dim sSpacer As String = ""
        Dim sCurrURType As String = "", sPrevURType As String = ""
        Try
            MenuXML.Load(_sPathToXMLFile)
            RootNode = MenuXML
            _menuList = RootNode.SelectNodes("*/Menu")

            sMenuCheckBox = "{3}<BR>{4}<input type='checkbox' name='MENU_ITEM' Menu_Id='{0}' Page_Id='0' Field_Id='0' {2} onClick=""menuItemClicked(this);"">&nbsp;{1}"
            sPageCheckBox = "{4}<BR>{5}<input type='checkbox' name='PAGE_ITEM' Menu_Id='{0}' Page_Id='{1}' Field_Id='0' {3} onClick=""pageItemClicked(this);"">&nbsp;{2}"
            sFieldCheckBox = "{5}<BR>{6}<input type='checkbox' name='FIELD_ITEM' Menu_Id='{0}' Page_Id='{1}' Field_Id='{2}' {4} onClick=""fieldItemClicked(this);"">&nbsp;{3}"

            'parsing main menu items..........
            For _iIndex = 0 To _menuList.Count - 1
                sMenuID = "0"
                sPageID = "0"
                sFieldID = "0"
                _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
                sMenuID = _xmlNode.Attributes(0).Value
                sCurrURType = _xmlNode.Attributes(1).Value

                If sCurrURType <> sPrevURType Then
                    If sRetTree = "" Then
                        sSpacer = String.Format("<b><u>{0}</u></b><BR>&rarr;&nbsp;", sCurrURType)
                    Else
                        sSpacer = String.Format("<BR><b><u>{0}</u></b><BR>&rarr;&nbsp;", sCurrURType)
                    End If
                Else
                        sSpacer = "&rarr;&nbsp;"
                    End If

                    sRetTree = String.Format(sMenuCheckBox, sMenuID, GetMenuName(MenuXML, sMenuID), IIf(IsSelected("MENU_ITEM", _alMenuID, sMenuID), "checked", ""), sRetTree, sSpacer)
                    'parsing pages.....
                    _pageList = _xmlNode.ChildNodes
                    For Each _Item In _pageList
                        If _Item.Name = "Pages" Then
                            _pageList1 = _Item.SelectNodes("Page")
                            For Each _Item1 In _pageList1
                                sPageID = _Item1.Attributes(0).Value
                                sSpacer = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&bull;&nbsp;"
                                sRetTree = String.Format(sPageCheckBox, sMenuID, sPageID, GetPageName(MenuXML, sPageID), IIf(IsSelected("PAGE_ITEM", _alPageID, sPageID), "checked", ""), sRetTree, sSpacer)

                                'parsing fields..........
                                _fieldList = _Item1.ChildNodes
                                For Each _Item2 In _fieldList
                                    If _Item2.Name = "Fields" Then
                                        _fieldList1 = _Item2.SelectNodes("Field")
                                        For Each _Item3 In _fieldList1
                                            sFieldID = _Item3.Attributes(0).Value
                                            sSpacer = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                            sRetTree = String.Format(sFieldCheckBox, sMenuID, sPageID, sFieldID, GetFieldName(MenuXML, sFieldID), IIf(IsSelected("FIELD_ITEM", _alFieldRights, sFieldID), "checked", ""), sRetTree, sSpacer)
                                        Next
                                    End If
                                Next

                            Next
                        End If
                    Next
                    sPrevURType = sCurrURType
            Next

        Catch ex As Exception
            sRetTree = sRetTree & "$" & ex.Message & "$"
        Finally
            MenuXML = Nothing
            RootNode = Nothing
        End Try
        GetUserRightsTree = sRetTree
    End Function
    Public Function GetUsers() As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetUsers()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "Select User"
        dt.Rows.InsertAt(dr, 0)
        Return dt

    End Function
    Public Function GetAllUsers() As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetAllUsers()
        Return dt
   End Function
    Public Function GetUserName(ByVal UserID As String) As String
        Return objDALUser.GetUserName(UserID)
    End Function
    Public Function GetUser(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Return objDALUser.GetUser(Error_No, Error_Desc, _sUsername, _sPassword, _sSalesRepID, _sUserTypeID, _sUserID, _sIsSS, _alAssignedSalesRep,_sOrgID)
    End Function
    Public Function ChangePassword(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Return objDALUser.ChangePassword(Error_No, Error_Desc, _sOldPassword, _sNewPassword, _sUserID)
    End Function
    Public Function GetUserTypes() As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetUserTypes()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "--Select User Type--"
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetUserTypesByDesignation(ByVal Designation As String) As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetUserTypesByDesignation(Designation)
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "--Select User Type--"
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetPDARights(ByVal Designation As String) As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetPDARights(Designation)
        Return dt
    End Function
    Public Function GetSalesReps() As DataTable
        Dim dt As New DataTable
        dt = objDALUser.GetSalesReps()
        Dim dr As DataRow
        dr = dt.NewRow()
        dr(0) = "0"
        dr(1) = "--Select a Van/FSR --"
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetSalesReps_chkbox() As DataTable
        Return objDALUser.GetSalesReps()
    End Function
    Public Function IsSalesRepAssigned() As Boolean
        Return objDALUser.IsAssignedSalesRep(Convert.ToInt32(_sSalesRepID))
    End Function
    Public Function IsSalesRepAssigned_ForUpdate() As Boolean
        Return objDALUser.IsAssignedSalesRep_ForUpdate(Convert.ToInt32(_sSalesRepID), Convert.ToInt32(_sUserID))
    End Function
    'Public Function SaveUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal dtRights As DataTable) As Boolean
    '    Try
    '        Return objDALUser.SaveUserType(Error_No, Error_Desc, _sUserType, dtRights)
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Public Function SaveUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal dtRights As DataTable, ByVal VanRights As String) As Boolean
        Try
            Dim lHHUserRights As Long

            'filter out HH rights and prepare binary data....
            For Each dr As DataRow In dtRights.Rows
                If dr(0).StartsWith("HM") And dr(1).StartsWith("HP") Then
                    lHHUserRights = lHHUserRights + Math.Pow(2, CType(dr(1).Substring(2), Long) - 1)
                End If
            Next
            Return objDALUser.SaveUserType(Error_No, Error_Desc, _sUserType, dtRights, lHHUserRights, VanRights, _sUserDesignation)
        Catch ex As Exception
            Error_Desc = GetExceptionInfo(ex)
            Throw ex
        End Try
    End Function

    Public Function GetUserRights() As DataTable
        Return objDALUser.GetUserRights(_sUserTypeID)
    End Function
    Public Function DeleteUserType(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Return objDALUser.DeleteUserType(Error_No, Error_Desc, _sUserTypeID)
    End Function
    Public Function UpdateUserRights(ByRef Error_No As Long, ByRef Error_Desc As String, ByRef dlRights As DataTable, ByVal VanRights As String) As Boolean
        Dim lHHUserRights As Long

        'filter out HH rights and prepare binary data....
        For Each dr As DataRow In dlRights.Rows
            If dr(0).StartsWith("HM") And dr(1).StartsWith("HP") Then
                lHHUserRights = lHHUserRights + Math.Pow(2, CType(dr(1).Substring(2), Long) - 1)
            End If
        Next
        Return objDALUser.UpdateUserRights(Error_No, Error_Desc, _sUserType, _sUserTypeID, dlRights, lHHUserRights, VanRights, _sUserDesignation)
    End Function
    Public Function SaveUser(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Return objDALUser.SaveUser(Error_No, Error_Desc, _sUsername, _sPassword, _sSalesRepID, _sUserTypeID, _sIsSS, _alAssignedSalesRep, _sOrgID)
    End Function
    Public Function UpdateUser(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Try
            Return objDALUser.UpdateUser(Error_No, Error_Desc, _sUsername, _sPassword, _sSalesRepID, _sUserTypeID, Convert.ToInt32(_sUserID), _sIsSS, _alAssignedSalesRep, _sOrgID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function DeleteUser(ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Return objDALUser.DeleteUser(Error_No, Error_Desc, Convert.ToInt32(_sUserID))
    End Function
    Public Function UserExists() As Boolean
        Return objDALUser.UserExists(_sUsername)
    End Function
    Public Function GetVanListByOrgID(ByVal Org_HE_ID As Long) As DataTable
        Return objDALUser.GetVanlistByOrgID(Org_HE_ID)
    End Function
End Class
