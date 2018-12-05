Imports System.Xml
Imports SalesWorx.BO.Common


Partial Public Class TopMenu
    Inherits System.Web.UI.UserControl

    Dim Str As String
    Dim StartMenu As Integer = 0
    Dim SubMenu As Integer = 0
    Dim PrevSub As String = ""
    Dim HasSub As Boolean = False
    Dim Check As Integer = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("USER_ACCESS")) Then
            'lbl_User.Text = (New User).GetUserName(Session("USER_ACCESS").UserID).ToString
            Dim MainMenu As String
            Dim SubMenu As String
            Str = "<ul class='sf-menu'>"
            GetMenu(MainMenu, SubMenu)
            Str += "<li class='current'> <a href='Logout.aspx' title='Logout'>Logout</a> </li></ul>"
            MenuLbl.Text = Str
        End If
    End Sub

    Public Function GetMenu(ByRef MainMenu As String, ByRef SubMenu As String)
        Dim MenuXML As New XmlDocument

        Dim RootNode As XmlNode

        Dim _menuList As XmlNodeList

        Dim _pageList As XmlNodeList, _pageList1 As XmlNodeList

        Dim _iIndex As Integer

        Dim _xmlNode As XmlNode

        Dim _Item As XmlNode

        Dim _Item1 As XmlNode

        Dim sRetMenu As Boolean = False

        Dim objControl As AppControl

        Dim lControlParams As Long

        Dim bControlOK As Boolean

        Try
            'retrieving app control params...
            objControl = New AppControl

            lControlParams = objControl.GetControlParams()

            MenuXML.Load(String.Format("{0}", Server.MapPath("..\xml\UserRights.xml")))

            RootNode = MenuXML

            _menuList = RootNode.SelectNodes("*/Menu")

            For _iIndex = 0 To _menuList.Count - 1
                _xmlNode = _menuList.Item(_iIndex).CloneNode(True)

                bControlOK = False

                'check if control is enabled....
                If Not IsNothing(_xmlNode.Attributes("cpoint")) Then
                    bControlOK = objControl.IsCPSelected(lControlParams, _xmlNode.Attributes("cpoint").Value)
                Else
                    bControlOK = True
                End If

                If bControlOK And Session("USER_ACCESS").MenuID.Contains(_xmlNode.Attributes(0).Value) Then
                    GetMenuName(MenuXML, _xmlNode.Attributes(0).Value)

                    _pageList = _xmlNode.ChildNodes

                    For Each _Item In _pageList
                        If _Item.Name = "Pages" Then

                            _pageList1 = _Item.SelectNodes("Page")

                            For Each _Item1 In _pageList1
                                If _Item1.Attributes(0).Value = "P87" Then
                                    Continue For
                                End If
                                bControlOK = False

                                'check if control is enabled....
                                If Not IsNothing(_Item1.Attributes("cpoint")) Then
                                    bControlOK = objControl.IsCPSelected(lControlParams, _Item1.Attributes("cpoint").Value)
                                Else
                                    bControlOK = True
                                End If

                                If bControlOK And Session("USER_ACCESS").PageID.Contains(_Item1.Attributes(0).Value) Then

                                    If StartMenu = 1 Then Str += "<ul>"

                                    GetPageName(MenuXML, _Item1.Attributes(0).Value)

                                    StartMenu = 2
                                End If
                            Next

                            If StartMenu = 2 Then Str += "</ul>"

                        End If

                    Next



                End If

            Next




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

                    If (PrevSub <> _xmlNode.Attributes(2).Value And PrevSub <> "") Then

                        If SubMenu = 1 And Check = 0 Then

                            Str += "</ul></li>"

                            SubMenu = 0

                            HasSub = False
                        End If

                    End If



                    If (_xmlNode.Attributes(2).Value = "") Then

                        Str += "<li class='current'> <a href='" & _xmlNode.Attributes(1).Value & "' title='" & _Item.Value & "' >" & _Item.Value & " </a></li>"

                    Else

                        If SubMenu = 0 Then Str += "<li class='current'> <a href='#' title='" & _xmlNode.Attributes(2).Value & "'>" & _xmlNode.Attributes(2).Value & "</a><ul>"

                        Str += "<li class='current'> <a href='" & _xmlNode.Attributes(1).Value & "' title='" & _Item.Value & "' >" & _Item.Value & " </a></li>"

                        PrevSub = _xmlNode.Attributes(2).Value

                        SubMenu = 1

                        HasSub = True

                    End If

                Next

            End If

        Next



        GetPageName = _strTmp



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

                    If HasSub Then

                        Str += "</li></ul>"

                        Check = 1

                        SubMenu = 0

                        HasSub = False
                    Else

                        Check = 0

                    End If



                    Str += "<li class='current'> <a href='#' title='" & _Item.Value & "'>" & _Item.Value & "</a>"

                    StartMenu = 1

                Next

            End If

        Next

        GetMenuName = _strTmp

    End Function




End Class