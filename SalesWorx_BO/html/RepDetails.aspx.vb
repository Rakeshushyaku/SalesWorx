Imports Microsoft.Reporting.WebForms
Imports System.Configuration.ConfigurationManager
Public Class RepDetails
    Inherits System.Web.UI.Page
    Dim Err_No As Long
    Dim Err_Desc As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("Type") = "Col" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("CollectionID", Request.QueryString("ID"))
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID})
                    '.ServerReport.Refresh()

                End With
            ElseIf Request.QueryString("Type") = "Cust" Then
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgId", Request.QueryString("OrgID"))
                Dim ids() As String
                ids = Request.QueryString("ID").Split("$")
                Dim CustID As New ReportParameter
                CustID = New ReportParameter("CustID", ids(0))
                Dim SiteID As New ReportParameter
                SiteID = New ReportParameter("SiteID", ids(1))
                With RVMain
                    .ShowParameterPrompts = False
                    .Reset()
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {OrgID, CustID, SiteID})
                    '.ServerReport.Refresh()

                End With
                ReportWrapper.Style.Add("height", "400px")

            ElseIf Request.QueryString("Type") = "Order" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                Dim type As New ReportParameter
                type = New ReportParameter("type", "O")
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode, type})
                End With
                '  ReportWrapper.Style.Add("width", "600px")
            ElseIf Request.QueryString("Type") = "DelivNotes" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
              
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID})
                End With
            ElseIf Request.QueryString("Type") = "Return" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode})
                End With
            ElseIf Request.QueryString("Type") = "SR" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode})
                End With
            ElseIf Request.QueryString("Type") = "Credit" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim RefNo As New ReportParameter
                RefNo = New ReportParameter("RefNo", Request.QueryString("RefNo"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                Dim mimeType As String = Nothing
                Dim encoding As String = Nothing
                Dim extension As String = Nothing
                Dim deviceInfo As String = "<DeviceInfo></DeviceInfo>"
                Dim streamids As String() = Nothing
                Dim warnings As Microsoft.Reporting.WebForms.Warning() = Nothing
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {RefNo})

                    '.ServerReport.SetParameters(New ReportParameter() {ID, RefNo, OrgID, ApprovalCode})

                End With
                Dim bytes As Byte() = RVMain.ServerReport.Render("PDF", deviceInfo, mimeType, encoding, extension, streamids, warnings)


                Response.Clear()

                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-disposition", "attachment;filename=DailySales.pdf")
                Response.AddHeader("Content-Length", bytes.Length)
                Response.OutputStream.Write(bytes, 0, bytes.Length)
                Response.OutputStream.Flush()
                Response.OutputStream.Close()
                Response.Flush()
                Response.Close()


        ElseIf Request.QueryString("Type") = "MonthReceivable" Then
            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("SID"))
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("orgId", Request.QueryString("OrgID"))
            Dim Month As New ReportParameter
            Month = New ReportParameter("Month", Request.QueryString("M"))
            Dim Yr As New ReportParameter
            Yr = New ReportParameter("Yr", Request.QueryString("Yr"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {SID, OrgID, Month, Yr})
            End With
        ElseIf Request.QueryString("Type") = "VisitException" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("VanList", Request.QueryString("ID"))
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OID", Request.QueryString("OrgID"))
            Dim Mode As New ReportParameter
            Mode = New ReportParameter("Mode", Request.QueryString("Mode"))

            Dim MonthName As New ReportParameter
            MonthName = New ReportParameter("FMonth", Request.QueryString("MonthName"))

            Dim VanName As New ReportParameter
            VanName = New ReportParameter("VanName", Request.QueryString("Van"))


            Dim OrgName As New ReportParameter
            OrgName = New ReportParameter("OrgName", Request.QueryString("OrgName"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {OrgID, ID, MonthName, OrgName, VanName, Mode})
            End With

        ElseIf Request.QueryString("Type") = "SoldbySKU" Then

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", "-1")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", Request.QueryString("Org"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("ID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("from"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))


            Dim FSRID As New ReportParameter
            FSRID = New ReportParameter("FSRID", Request.QueryString("SPID") & "|")

            Dim SKU As New ReportParameter
            SKU = New ReportParameter("SKU", (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, Request.QueryString("ID"), Request.QueryString("Org")))
            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("uid"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {SID, OrgID, FromDate, ToDate, FSRID, InID, SKU, UID})
            End With
        ElseIf Request.QueryString("Type") = "ReturnbySKU" Then

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", "-1")

            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", Request.QueryString("Org"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("ID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("from"))

            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))


            Dim FSRID As New ReportParameter
            FSRID = New ReportParameter("FSRID", Request.QueryString("SPID") & "|")

            Dim SKU As New ReportParameter
            SKU = New ReportParameter("SKU", (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, Request.QueryString("ID"), Request.QueryString("Org")))
            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("uid"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {SID, OrgID, FromDate, ToDate, FSRID, InID, SKU, UID})
            End With
        ElseIf Request.QueryString("Type") = "Zero" Then

            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OID", Request.QueryString("Org"))



            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("ID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", Request.QueryString("To"))

            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("uid"))

            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", Request.QueryString("Rtype"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {OrgId, SID, FromDate, ToDate, UID, Type})
            End With
        ElseIf Request.QueryString("Type") = "stock" Then
            If Not IsPostBack Then
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim CustID As New ReportParameter
                CustID = New ReportParameter("StockRequisition_ID", Request.QueryString("ID"))

                With RVMain
                    .ShowParameterPrompts = False
                    .Reset()
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {OrgID, CustID})
                    '.ServerReport.Refresh()

                End With
                ReportWrapper.Style.Add("height", "400px")
            End If
        ElseIf Request.QueryString("Type") = "DisCheck" Then
            If Not IsPostBack Then
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("SearchParams", Request.QueryString("ID"))


                With RVMain
                    .ShowParameterPrompts = False
                    .Reset()
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {OrgID})
                    '.ServerReport.Refresh()

                End With
            End If
        ElseIf Request.QueryString("Type") = "Approval" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("RowID", Request.QueryString("ID"))
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
            Dim ApprovalCode As New ReportParameter
            ApprovalCode = New ReportParameter("ApprovalCode", Request.QueryString("ApprovalCode"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode})
            End With
        ElseIf Request.QueryString("Type") = "MLT_TRX_FOC" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("Row_ID", Request.QueryString("ID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {ID})
            End With
        ElseIf Request.QueryString("Type") = "Asset" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("AssetID", Request.QueryString("ID"))
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {ID, OrgID})
            End With
        ElseIf Request.QueryString("Type") = "FOC" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("Feedback_ID", Request.QueryString("ID"))
            Dim OrgID As New ReportParameter
            OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {ID, OrgID})
            End With
        ElseIf Request.QueryString("Type") = "PWS" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))



            Dim SID As New ReportParameter
            SID = New ReportParameter("Salesrep_ID", Request.QueryString("SID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", Request.QueryString("To"))

            Dim UID As New ReportParameter
            UID = New ReportParameter("Agency", Request.QueryString("age"))

            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", Request.QueryString("rtype"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {Type, FromDate, ToDate, OrgId, UID, SID})
            End With

        ElseIf Request.QueryString("Type") = "WareHousePurchase" Then
            Dim PDate As New ReportParameter
            PDate = New ReportParameter("Date", CDate(Request.QueryString("Date")).ToString("dd-MMM-yyyy"))
            Dim ID As New ReportParameter
            ID = New ReportParameter("PORefNo", Request.QueryString("ID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {PDate, ID})
            End With
        ElseIf Request.QueryString("Type") = "OWSR" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))



            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("SID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))

            Dim CID As New ReportParameter
            CID = New ReportParameter("CID", Request.QueryString("CID"))

            Dim Outlet As New ReportParameter
            Outlet = New ReportParameter("Outlet", Request.QueryString("Outlet"))

            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("UID"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, SID, CID, Outlet, OrgId, UID})
            End With
        ElseIf Request.QueryString("Type") = "OWSWSR" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))



            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("SID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))

            Dim CID As New ReportParameter
            CID = New ReportParameter("CID", Request.QueryString("CID"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("INID"))

            Dim FID As New ReportParameter
            FID = New ReportParameter("FSRID", Request.QueryString("FID"))
            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("UID"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, SID, CID, InID, OrgId, FID, UID})
            End With
        ElseIf Request.QueryString("Type") = "OWSWR" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))



            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("SID"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))

            Dim CID As New ReportParameter
            CID = New ReportParameter("CID", Request.QueryString("CID"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("INID"))

            Dim SKU As New ReportParameter
            SKU = New ReportParameter("SKU", (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, Request.QueryString("INID"), Request.QueryString("Org")))

            Dim Outlet As New ReportParameter
            Outlet = New ReportParameter("Outlet", Request.QueryString("Out"))
            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("UID"))

            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, SID, CID, InID, OrgId, SKU, Outlet, UID})
            End With
        ElseIf Request.QueryString("Type") = "SKUReturns" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("INID"))


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, InID, OrgId})
            End With

        ElseIf Request.QueryString("Type") = "SKUSalesandReturns" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("FromDate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("ToDate", Request.QueryString("To"))

            Dim InID As New ReportParameter
            InID = New ReportParameter("InID", Request.QueryString("InID"))

            Dim SKU As New ReportParameter
            SKU = New ReportParameter("SKU", (New SalesWorx.BO.Common.Product).GetItemName(Err_No, Err_Desc, Request.QueryString("InID"), Request.QueryString("Org")))

            Dim SID As New ReportParameter
            SID = New ReportParameter("SID", Request.QueryString("CID"))
            Dim UID As New ReportParameter
            UID = New ReportParameter("UID", Request.QueryString("UID"))

            Dim FSRID As New ReportParameter
            FSRID = New ReportParameter("FSRID", Request.QueryString("FSRID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, InID, OrgId, SKU, SID, FSRID, UID})
            End With
        ElseIf Request.QueryString("Type") = "PrincipalWiseSalesReturn" Then
            Dim OrgId As New ReportParameter
            OrgId = New ReportParameter("OrgID", Request.QueryString("Org"))

            Dim FromDate As New ReportParameter
            FromDate = New ReportParameter("Fromdate", Request.QueryString("From"))


            Dim ToDate As New ReportParameter
            ToDate = New ReportParameter("Todate", Request.QueryString("To"))

            Dim Type As New ReportParameter
            Type = New ReportParameter("Type", Request.QueryString("RType"))

            Dim Agency As New ReportParameter
            Agency = New ReportParameter("Agency", Request.QueryString("Agency"))

            Dim SID As New ReportParameter
            SID = New ReportParameter("Salesrep_ID", Request.QueryString("SID"))


            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {FromDate, ToDate, Type, OrgId, SID, Agency})
            End With
        ElseIf Request.QueryString("Type") = "ERPSYNCLOG" Then
            Dim ID As New ReportParameter
            ID = New ReportParameter("LogID", Request.QueryString("ID"))
            With RVMain
                .Reset()
                .ShowParameterPrompts = False
                .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                .ServerReport.SetParameters(New ReportParameter() {ID})
                '.ServerReport.Refresh()

                End With
            ElseIf Request.QueryString("Type") = "Order" Then
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Request.QueryString("ID"))
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                Dim type As New ReportParameter
                type = New ReportParameter("type", "O")
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode, type})
                End With

            ElseIf Request.QueryString("Type") = "LinkedOrder" Then

                Dim Row_ID As String = (New SalesWorx.BO.Common.Reports).GetRowIDFromOrigSysDocNo(Request.QueryString("Refno"), "O")
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Row_ID)
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
                Dim type As New ReportParameter
                type = New ReportParameter("type", "O")
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode, type})
                End With

            ElseIf Request.QueryString("Type") = "LinkedReturn" Then

                Dim Row_ID As String = (New SalesWorx.BO.Common.Reports).GetRowIDFromOrigSysDocNo(Request.QueryString("Refno"), "R")
                Dim ID As New ReportParameter
                ID = New ReportParameter("RowID", Row_ID)
                Dim OrgID As New ReportParameter
                OrgID = New ReportParameter("OrgID", Request.QueryString("OrgID"))
                Dim ApprovalCode As New ReportParameter
                ApprovalCode = New ReportParameter("ApprovalCode", "0")
               
                With RVMain
                    .Reset()
                    .ShowParameterPrompts = False
                    .ServerReport.ReportServerCredentials = New CustomReportServerCredentials()
                    .ServerReport.ReportServerUrl = New System.Uri(AppSettings("ReportServer"))
                    .ServerReport.ReportPath = String.Format("{0}{1}", AppSettings("ReportPath"), Request.QueryString("ReportName"))
                    .ServerReport.SetParameters(New ReportParameter() {ID, OrgID, ApprovalCode})
                End With

            End If

        End If

    End Sub

End Class