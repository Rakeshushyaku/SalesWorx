Imports System.IO
Imports SalesWorx.BO.Common
Imports System.Reflection
Imports System.Xml.Serialization

Partial Public Class Dashboard
    Inherits System.Web.UI.Page
    Private Const PageID As String = "P87"
    Dim Err_No As Long
    Dim Err_Desc As String
    Private oDashboardLayout As New DashboardLayout()
    Private Sub Dashboard_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitDashboardLayout()

        If Not IsNothing(oDashboardLayout.Lines) AndAlso oDashboardLayout.Lines.Count > 0 Then
            If Not ClientScript.IsClientScriptBlockRegistered("LoaderScript") Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "LoaderScript", GetJSFunctions())
            End If
        End If
    End Sub
    Private Sub InitDashboardLayout()
        Try
            If LoadFromXml(oDashboardLayout, Path.Combine(Server.MapPath(".."), "xml\DashboardLayout.xml")) Then
                Dim row As HtmlTableRow
                Dim cell As HtmlTableCell

                Dim sCellID As String
                Dim iRowCounter As Integer = 1
                Dim iColCounter As Integer = 1

                For Each oLine As Line In oDashboardLayout.Lines
                    row = New HtmlTableRow()

                    iColCounter = 1
                    For Each oSection As Section In oLine.Sections
                        sCellID = String.Format("DS_{0}_{1}", iRowCounter, iColCounter)

                        cell = New HtmlTableCell()

                        cell.Attributes.Add("id", String.Format("TD_{0}", sCellID))
                        cell.Attributes.Add("width", oSection.Width)
                        cell.Attributes.Add("height", oDashboardLayout.SectionContainerHeight)
                        cell.Attributes.Add("align", oSection.Align)
                        cell.Attributes.Add("style", oSection.Style)
                        cell.Attributes.Add("valign", "top")

                        If IsNumeric(oSection.Span) Then
                            cell.ColSpan = CInt(Val(oSection.Span))
                        End If

                        cell.InnerHtml = String.Format("{0}{1}", oDashboardLayout.SectionHeaderTemplate, oDashboardLayout.SectionContentTemplate)

                        cell.InnerHtml = cell.InnerHtml.Replace("$TITLE$", oSection.Title)
                        cell.InnerHtml = cell.InnerHtml.Replace("$ID$", sCellID)
                        cell.InnerHtml = cell.InnerHtml.Replace("$NAME$", sCellID)
                        cell.InnerHtml = cell.InnerHtml.Replace("$SRC$", oSection.InitialSource)
                        cell.InnerHtml = cell.InnerHtml.Replace("$STYLE$", oSection.FrameStyle)
                        cell.InnerHtml = cell.InnerHtml.Replace("$HEIGHT$", oSection.Height)

                        row.Cells.Add(cell)
                        cell = Nothing
                        iColCounter += 1
                    Next

                    iRowCounter += 1

                    Me.TblDashboard.Rows.Add(row)
                Next
            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Function GetJSFunctions() As String
        Dim tempSB As New StringBuilder("")

        tempSB.Append(GetTimerInitJS())
        tempSB.Append(GetLoaderJS())

        Return EncloseJS(tempSB.ToString())
    End Function

    Private Function GetLoaderJS() As String
        Dim retVal As String = ""
        Try
            Dim sCellID As String
            Dim iRowCounter As Integer = 1
            Dim iColCounter As Integer = 1

            Dim tempSB As New StringBuilder("")
            tempSB.Append(SuffixCRLF("function loadSections() {"))

            For Each oLine As Line In oDashboardLayout.Lines
                iColCounter = 1
                For Each oSection As Section In oLine.Sections
                    sCellID = String.Format("DS_{0}_{1}", iRowCounter, iColCounter)
                    tempSB.Append(SuffixCRLF(String.Format("$(""#{0}"").attr(""src"",""{1}"");", sCellID, oSection.ContentSource)))
                    iColCounter += 1
                Next

                iRowCounter += 1
            Next

            tempSB.Append(SuffixCRLF("initRefreshTimers();"))
            tempSB.Append(SuffixCRLF("}"))

            retVal = tempSB.ToString()
        Catch ex As Exception
            retVal = GetAsJSAlert(ex.Message)
        End Try
        Return retVal
    End Function

    Private Function GetTimerInitJS() As String
        Dim retVal As String = ""
        Try
            Dim sCellID As String
            Dim iRowCounter As Integer = 1
            Dim iColCounter As Integer = 1

            Dim tempSB As New StringBuilder("")
            tempSB.Append(SuffixCRLF("function initRefreshTimers() {"))

            For Each oLine As Line In oDashboardLayout.Lines
                iColCounter = 1
                For Each oSection As Section In oLine.Sections
                    sCellID = String.Format("DS_{0}_{1}", iRowCounter, iColCounter)
                    If oSection.RefreshInterval > 0 Then
                        tempSB.Append(SuffixCRLF(GetTimerJS(sCellID, (oSection.RefreshInterval * 1000))))
                    End If
                    iColCounter += 1
                Next

                iRowCounter += 1
            Next

            tempSB.Append(SuffixCRLF("}"))

            retVal = tempSB.ToString()
        Catch ex As Exception
            retVal = GetAsJSAlert(ex.Message)
        End Try
        Return retVal
    End Function

    Private Function GetTimerJS(ByVal sectionID As String, ByVal refreshInterval As Integer) As String
        Dim tempSB As New StringBuilder("$(document).everyTime(")
        tempSB.Append(refreshInterval)
        tempSB.Append(String.Format(", ""TMR_{0}"", ", sectionID))
        tempSB.Append("function(){ ")
        tempSB.Append(String.Format("refreshSection('{0}')", sectionID))
        tempSB.Append(" }, 0);")
        Return tempSB.ToString()
    End Function

    Private Function GetAsJSAlert(ByVal message As String)
        Return SuffixCRLF(String.Format("alert(""{0}"");", message.Replace("""", "\""")))
    End Function

    Private Function EncloseJS(ByVal script As String)
        Return SuffixCRLF(String.Format("<script type=""text/javascript"">{0}{1}{0}</script>", vbCrLf, script))
    End Function

    Private Function SuffixCRLF(ByVal text As String)
        Return String.Format("{0}{1}", text, vbCrLf)
    End Function

    Protected Friend Shared Function LoadFromXml(Of T)(ByRef instance As T, ByVal filePath As String) As Boolean
        Dim objSerialize As XmlSerializer
        Dim fs As System.IO.FileStream = Nothing
        Dim bRetVal As Boolean = False
        Try
            If instance Is Nothing Then
                Throw New ArgumentNullException("instance")
            End If

            If Not System.IO.File.Exists(filePath) Then
                Throw New IO.FileNotFoundException("File not found", filePath)
            End If

            objSerialize = New XmlSerializer(instance.GetType)
            fs = New FileStream(filePath, IO.FileMode.Open, FileAccess.Read)
            instance = CType(objSerialize.Deserialize(fs), T)

            bRetVal = True
        Catch ex As Exception
            Throw
        Finally
            If Not fs Is Nothing Then
                fs.Close()
                fs = Nothing
            End If
            objSerialize = Nothing
        End Try
        Return bRetVal
    End Function

    <Serializable()> <XmlRoot("Dashboard")> _
    Public Class DashboardLayout
        Public Lines As List(Of Line)

        Public SectionContainerHeight As String

        Public SectionHeaderTemplate As String

        Public SectionContentTemplate As String
    End Class

    <Serializable()> _
    Public Class Line
        Public Sections As List(Of Section)
    End Class

    <Serializable()> _
    Public Class Section

        Private _Title As String
        <XmlAttribute("title")> _
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Private _Span As String
        <XmlAttribute("span")> _
        Public Property Span() As String
            Get
                Return _Span
            End Get
            Set(ByVal value As String)
                _Span = value
            End Set
        End Property

        Private _Width As String
        <XmlAttribute("width")> _
        Public Property Width() As String
            Get
                Return _Width
            End Get
            Set(ByVal value As String)
                _Width = value
            End Set
        End Property

        Private _Height As String
        <XmlAttribute("height")> _
        Public Property Height() As String
            Get
                Return _Height
            End Get
            Set(ByVal value As String)
                _Height = value
            End Set
        End Property

        Private _Align As String
        <XmlAttribute("align")> _
        Public Property Align() As String
            Get
                Return _Align
            End Get
            Set(ByVal value As String)
                _Align = value
            End Set
        End Property

        Private _Style As String
        <XmlAttribute("style")> _
        Public Property Style() As String
            Get
                Return _Style
            End Get
            Set(ByVal value As String)
                _Style = value
            End Set
        End Property

        Private _InitialSource As String
        <XmlAttribute("initsrc")> _
        Public Property InitialSource() As String
            Get
                Return _InitialSource
            End Get
            Set(ByVal value As String)
                _InitialSource = value
            End Set
        End Property

        Private _ContentSource As String
        <XmlAttribute("src")> _
        Public Property ContentSource() As String
            Get
                Return _ContentSource
            End Get
            Set(ByVal value As String)
                _ContentSource = value
            End Set
        End Property

        Private _RefreshInterval As String
        <XmlAttribute("refresh")> _
        Public Property RefreshInterval() As String
            Get
                Return _RefreshInterval
            End Get
            Set(ByVal value As String)
                _RefreshInterval = value
            End Set
        End Property


        Private _FrameStyle As String
        <XmlAttribute("framestyle")> _
        Public Property FrameStyle() As String
            Get
                Return _FrameStyle
            End Get
            Set(ByVal value As String)
                _FrameStyle = value
            End Set
        End Property


    End Class
    

    Private Sub Dashboard_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Session("USER_ACCESS")) Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            Dim HasPermission As Boolean = False
            ManageAuthentication.HasPermission(CType(Session.Item("USER_ACCESS"), UserAccess), PageID, HasPermission)
            If Not HasPermission Then
                Err_No = 500
                Response.Redirect("information.aspx?mode=1&errno=" & Err_No & "&msg=" & AppMsgHandler.GetErrorMessage("E_BO_Unauthorized") & "&next=Welcome.aspx&Title=Message", False)
            End If
          
        End If
    End Sub
End Class