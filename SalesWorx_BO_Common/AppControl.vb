Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Xml


Public Class AppControl

    Private _objAppControl As DAL.DAL_AppControl

    Public _sPathToXMLFile As String

    Public _iRouteSyncDays As Integer
    Public _iRoutePlanDueDate As Integer
    Public _iOrdPending As String
    Public _iCLOD As String
    Public _iFC As String
    Public _iRMADays As String
    Public _iRMALimit As Decimal
    Public _iOD As String
    Public _iAutoBlock As String
    Public _iOverDueLimit As Decimal

    'Private Shared _BO_DB As String = DatabaseConnection.BODatabase()
    'Private Shared _BO_SYNC_DB As String = DatabaseConnection.BOSyncDatabase()

    Public Sub New()
        _objAppControl = New DAL.DAL_AppControl()
    End Sub

    Public WriteOnly Property PathToXMLFile() As String
        Set(ByVal Value As String)
            _sPathToXMLFile = Trim(Value)
        End Set
    End Property


    Private Function GetSectionName(ByVal node As System.Xml.XmlNode, ByVal NodeID As String) As String
        Try
            Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
            Dim _iIndex As Integer
            Dim _xmlNode As XmlNode
            Dim _Item As XmlNode

            _menuList = node.SelectNodes("*/Section")
            For _iIndex = 0 To _menuList.Count - 1
                _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
                If _xmlNode.Attributes(0).Value = NodeID Then
                    _pageList = _xmlNode.ChildNodes
                    For Each _Item In _pageList(0)
                        _strTmp = _Item.Value
                        Exit For
                    Next
                End If
            Next
            Return _strTmp
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetPointName(ByVal node As System.Xml.XmlNode, ByVal NodeID As String) As String

        On Error Resume Next

        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/*/*/Point")

        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = NodeID Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(0)
                    _strTmp = _Item.Value
                    Exit For
                Next
            End If
        Next

        Return _strTmp
    End Function


    Private Function GetSubPointName(ByVal node As System.Xml.XmlNode, ByVal NodeID As String) As String

        On Error Resume Next

        Dim _menuList As XmlNodeList, _pageList As XmlNodeList, _strTmp As String
        Dim _iIndex As Integer
        Dim _xmlNode As XmlNode
        Dim _Item As XmlNode

        _menuList = node.SelectNodes("*/*/*/*/*/SubPoint")

        For _iIndex = 0 To _menuList.Count - 1
            _xmlNode = _menuList.Item(_iIndex).CloneNode(True)
            If _xmlNode.Attributes(0).Value = NodeID Then
                _pageList = _xmlNode.ChildNodes
                For Each _Item In _pageList(0)
                    _strTmp = _Item.Value
                    Exit For
                Next
            End If
        Next

        Return _strTmp
      
    End Function
    Public Function DeleteAppcode(ByRef Err_No As Long, ByRef Err_Desc As String, Type As String, Key As String) As Boolean
        Try
            Return _objAppControl.DeleteAppcode(Err_No, Err_Desc, Type, Key)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function LoadReturnPurpose(ByRef Error_No As Long, ByRef Error_Desc As String, Type As String) As DataTable
        Try
            Return _objAppControl.LoadReturnPurpose(Error_No, Error_Desc, Type)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadAppCodeTypes(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Try
            Return _objAppControl.LoadAppCodeTypes(Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SaveAppCodes(ByRef Error_No As Long, ByRef Error_Desc As String, CodeType As String, Code As String, Description As String, CreatedBy As Integer) As Boolean
        Try
            Return _objAppControl.SaveAppCodes(Error_No, Error_Desc, CodeType, Code, Description, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function AppCodeExists(CodeType As String, Code As String) As Boolean
        Try
            Return _objAppControl.AppCodeExists(CodeType, Code)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadParamDropdown(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal controlKey As String) As DataTable
        Return _objAppControl.LoadParamDropdown(Error_No, Error_Desc, controlKey)
    End Function
    Public Function CopyLoadParentNode(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Return _objAppControl.CopyLoadParentNode(Error_No, Error_Desc, ParentNode)
    End Function
    Public Function LoadParentNode(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Return _objAppControl.LoadParentNode(Error_No, Error_Desc)
    End Function
    Public Function CopyLoadAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Return _objAppControl.CopyLoadAppControlParams(Error_No, Error_Desc, ParentNode)
    End Function
    Public Function LoadAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ParentNode As String) As DataTable
        Return _objAppControl.LoadAppControlParams(Error_No, Error_Desc, ParentNode)
    End Function
    Public Function LoadOtherAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal VersionNo As String) As DataTable
        Return _objAppControl.LoadOtherAppControlParams(Error_No, Error_Desc, VersionNo)
    End Function
    Public Function LoadAppParamType(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Return _objAppControl.LoadAppParamType(Error_No, Error_Desc)
    End Function
    Public Function GetControlParamsTree() As String
        Try

      
            Dim xdControlParams As New XmlDocument
            Dim xnlSections As XmlNodeList
            Dim xnlPoints As XmlNodeList
            Dim xnlSubPoints As XmlNodeList
            Dim xnRoot As XmlNode
            Dim xnSection As XmlNode
            Dim xnPoint As XmlNode
            Dim xnSubPoint As XmlNode
            Dim sSectionLine As String
            Dim sPointLine As String
            Dim sSubPointLine As String
            Dim sRetTree As String = ""
            Dim sSpacer As String = ""
            Try
                xdControlParams.Load(Me._sPathToXMLFile)
                xnRoot = xdControlParams
                xnlSections = xnRoot.SelectNodes("*/Section")

                sSectionLine = "{3}<BR>{4}<input type='checkbox' class='txtSM' name='SECTION' Section_Id='{0}' {2} onClick=""sectionClicked(this);"" style='vertical-align: middle;'>&nbsp;<B>{1}</B>"
                sPointLine = "{4}<BR>{5}<input type='checkbox' class='txtSM' name='POINT' Section_Id='{0}' Point_Id='{1}' {3} onClick=""pointClicked(this);"" value='{1}' style='vertical-align: middle;' sublink='{6}'>&nbsp;{2}"
                sSubPointLine = "{5}<BR>{6}{3}:&nbsp;<input class='txtSM' type='radio' name='SUB_POINT_{2}' Section_Id='{0}' Point_Id='{1}' SubPoint_Id='{2}' state='1' {4} onClick=""subPointClicked(this);""  value='{1}' style='vertical-align: middle;' {10}>&nbsp;{7}&nbsp;/&nbsp;<input type='radio' class='txtSM' name='SUB_POINT_{2}' Section_Id='{0}' Point_Id='{1}' SubPoint_Id='{2}' state='0' {9} onClick=""subPointClicked(this);"" style='vertical-align: middle;' {10}>&nbsp;{8}"

                Dim sSection As String, sSectionID As String
                Dim sPoint As String, sPointID As String, sSubLink As String
                Dim sSubPoint As String, sSubPointID As String
                Dim bSelectSection As Boolean
                Dim sSubPointStyle As String

                Dim sState1 As String, sState0 As String

                'retrieve current control params from db....
                Dim lControlParams As Long = Me.GetControlParams()

                'parsing main menu items..........
                For iSecIndex As Integer = 0 To xnlSections.Count - 1
                    sSection = ""
                    sPoint = ""
                    sSectionID = "0"
                    sPointID = "0"
                    sSubPointID = "0"
                    bSelectSection = False

                    xnSection = xnlSections.Item(iSecIndex).CloneNode(True)
                    sSectionID = xnSection.Attributes("id").Value

                    'parsing points.....
                    xnlPoints = xnSection.SelectNodes("//Point")
                    For iPtIndex As Integer = 0 To xnlPoints.Count - 1
                        xnPoint = xnlPoints.Item(iPtIndex).CloneNode(True)

                        sSubPointStyle = "disabled"

                        sPointID = xnPoint.Attributes("id").Value
                        sSubPoint = ""
                        sSubLink = ""

                        If Not IsNothing(xnPoint.Attributes("sub-link")) Then
                            sSubLink = xnPoint.Attributes("sub-link").Value
                        End If

                        sSpacer = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&bull;&nbsp;"

                        If Me.IsCPSelected(lControlParams, sPointID) Then
                            sPoint = String.Format(sPointLine, sSectionID, sPointID, GetPointName(xdControlParams, sPointID), "checked", sPoint, sSpacer, sSubLink)

                            'set section selection var...
                            bSelectSection = True

                            'for enabling sub-point....
                            sSubPointStyle = ""
                        Else
                            sPoint = String.Format(sPointLine, sSectionID, sPointID, GetPointName(xdControlParams, sPointID), "", sPoint, sSpacer, sSubLink)

                            'enable sub-point, in case sub-link is false....
                            If sSubLink.ToUpper() = "FALSE" Then
                                sSubPointStyle = ""
                            End If
                        End If

                        'parsing sub-points..........
                        xnlSubPoints = xnPoint.SelectNodes("//SubPoint")
                        For iSubPtIndex As Integer = 0 To xnlSubPoints.Count - 1
                            xnSubPoint = xnlSubPoints.Item(iSubPtIndex).CloneNode(True)

                            sSubPointID = xnSubPoint.Attributes("id").Value

                            sSpacer = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"

                            sState1 = xnSubPoint.SelectSingleNode("State1").InnerText()
                            sState0 = xnSubPoint.SelectSingleNode("State0").InnerText()

                            If IsCPSelected(lControlParams, sSubPointID) Then
                                sSubPoint = String.Format(sSubPointLine, sSectionID, sPointID, sSubPointID, GetSubPointName(xdControlParams, sSubPointID), "checked", sSubPoint, sSpacer, sState1, sState0, "", sSubPointStyle)
                            Else
                                sSubPoint = String.Format(sSubPointLine, sSectionID, sPointID, sSubPointID, GetSubPointName(xdControlParams, sSubPointID), "", sSubPoint, sSpacer, sState1, sState0, "checked", sSubPointStyle)
                            End If

                        Next

                        'add sub-points under main point...
                        sPoint = String.Format("{0}{1}", sPoint, sSubPoint)

                    Next

                    If sRetTree = "" Then
                        sSpacer = "&rarr;&nbsp;"
                    Else
                        sSpacer = "<BR>&rarr;&nbsp;"
                    End If

                    sSection = String.Format(sSectionLine, sSectionID, GetSectionName(xdControlParams, sSectionID), IIf(bSelectSection, "checked", ""), sSection, sSpacer)

                    sRetTree = String.Format("{0}{1}{2}", sRetTree, sSection, sPoint)
                Next

            Catch ex As Exception
                sRetTree = sRetTree & "$" & ex.Message & "$"
            Finally
                xdControlParams = Nothing
                xnRoot = Nothing
                xnPoint = Nothing
                xnSubPoint = Nothing
                xnlSections = Nothing
                xnlPoints = Nothing
                xnlSubPoints = Nothing
            End Try
            Return sRetTree
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function IsCPSelected(ByVal ControlParams As Long, ByVal ControlPoint As String) As Boolean
        Try
            Dim bRetVal As Boolean = False
            If IsNumeric(ControlPoint.Substring(2)) Then
                If (ControlParams And Math.Pow(2, CType(ControlPoint.Substring(2), Long) - 1)) > 0 Then
                    bRetVal = True
                End If
            End If
            Return bRetVal
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetControlParams() As Long
        Try
            Return _objAppControl.GetControlParams()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GeLoginMode(ByRef Error_No As Long, ByRef Error_Desc As String) As String
        Try
            Return _objAppControl.GeLoginMode(Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateAppParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ModifiedBy As Integer, ByVal ControlKey As String, ByVal ControlValue As String) As Boolean

        Try
            Return _objAppControl.UpdateAppParams(Error_No, Error_Desc, ModifiedBy, ControlKey, ControlValue)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateAppControlParams(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal ModifiedBy As Integer, ByVal ALLOW_EXCESS_CASH_COLLECTION As String, ByVal ALLOW_LOAD_QTY_CHANGE As String, ByVal ALLOW_ORDER_DISCOUNT As String, ByVal ALLOW_PARTIAL_UNLOAD As String, ByVal ALLOW_UNLOAD_QTY_CHANGE As String, ByVal CN_LIMIT_MODE As String, ByVal COLLECTION_MODE As String, ByVal DISCOUNT_MODE As String, ByVal ENABLE_COLLECTIONS As String, ByVal ENABLE_CUSTOMER_SIGNATURE As String, ByVal ENABLE_DISTRIB_CHECK As String, ByVal ENABLE_LOT_SELECTION As String, ByVal ENABLE_MARKET_SURVEY As String, ByVal ENABLE_ORDER_HISTORY As String, ByVal ENABLE_SHORT_DOC_REF As String, ByVal EOD_ON_UNLOAD As String, ByVal MERGE_RETURN_STOCK As String, ByVal OPTIONAL_RETURN_HDR_REASON As String, ByVal OPTIONAL_RETURN_LOT As String, ByVal RETURN_STOCK_MERGE_MODE As String, ByVal UNLOAD_QTY_CHANGE_MODE As String, ByVal VAN_LOAD_TYPE As String, ByVal VAN_UNLOAD_TYPE As String, ByVal DISC_LIMIT_MIN As String, ByVal DISC_LIMIT_MAX As String) As Boolean

        Try
            Return _objAppControl.UpdateAppControlParams(Error_No, Error_Desc, ModifiedBy, ALLOW_EXCESS_CASH_COLLECTION, ALLOW_LOAD_QTY_CHANGE, ALLOW_ORDER_DISCOUNT, ALLOW_PARTIAL_UNLOAD, ALLOW_UNLOAD_QTY_CHANGE, CN_LIMIT_MODE, COLLECTION_MODE, DISCOUNT_MODE, ENABLE_COLLECTIONS, ENABLE_CUSTOMER_SIGNATURE, ENABLE_DISTRIB_CHECK, ENABLE_LOT_SELECTION, ENABLE_MARKET_SURVEY, ENABLE_ORDER_HISTORY, ENABLE_SHORT_DOC_REF, EOD_ON_UNLOAD, MERGE_RETURN_STOCK, OPTIONAL_RETURN_HDR_REASON, OPTIONAL_RETURN_LOT, RETURN_STOCK_MERGE_MODE, UNLOAD_QTY_CHANGE_MODE, VAN_LOAD_TYPE, VAN_UNLOAD_TYPE, DISC_LIMIT_MIN, DISC_LIMIT_MAX)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateControlParams(ByVal PointData As String, ByVal SubPointData As String, ByRef Error_No As Long, ByRef Error_Desc As String) As Boolean
        Try
            Return _objAppControl.UpdateControlParams(PointData, SubPointData, Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function UpdateOtherParams(Optional ByRef SQLConn As SqlConnection = Nothing) As Boolean
        Try
            Return _objAppControl.UpdateOtherParams(_iRouteSyncDays, _iRoutePlanDueDate, _iFC, _iCLOD, _iOrdPending, _iRMADays, _iRMALimit, _iOD, _iAutoBlock, _iOverDueLimit)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadOtherParams() As Boolean
        Try
            Return _objAppControl.LoadOtherParams(_iRouteSyncDays, _iRoutePlanDueDate, _iFC, _iCLOD, _iOrdPending, _iRMADays, _iRMALimit, _iOD, _iAutoBlock, _iOverDueLimit)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadRecipientType(ByRef Error_No As Long, ByRef Error_Desc As String) As DataTable
        Try
            Return _objAppControl.LoadRecipientType(Error_No, Error_Desc)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SavePushNotification(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal Title As String, ByVal Description As String, RecipeintType As String, RecipientList As String, CreatedBy As Integer) As Boolean

        Try
            Return _objAppControl.SavePushNotification(Error_No, Error_Desc, Title, Description, RecipeintType, RecipientList, CreatedBy)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadRecipients(ByRef Error_No As Long, ByRef Error_Desc As String, UID As Integer) As DataTable
        Try
            Return _objAppControl.LoadRecipients(Error_No, Error_Desc, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadRecipientsforReview(ByRef Error_No As Long, ByRef Error_Desc As String, UID As Integer) As DataTable
        Try
            Return _objAppControl.LoadRecipientsforReview(Error_No, Error_Desc, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadRecipientUsers(ByRef Error_No As Long, ByRef Error_Desc As String, UID As Integer) As DataTable
        Try
            Return _objAppControl.LoadRecipientUsers(Error_No, Error_Desc, UID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
