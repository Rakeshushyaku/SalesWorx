Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.IO.Compression

Partial Public Class ViewImage
    Inherits System.Web.UI.Page
    Const IMAGE_WIDTH As Integer = 208
    Const IMAGE_HEIGHT As Integer = 80
    Dim Err_Desc As String
    Dim Err_No As Long
    Private _tempsignature As Byte()
    Public Property tempsignature() As Byte()
        Get
            Return _tempsignature
        End Get
        Set(ByVal value As Byte())
            _tempsignature = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim arrImageData() As Byte
        Dim ReqID As String = Request.QueryString("id")
        If Not IsNothing(Request.QueryString("type")) Then
            Dim type As String = ""
            type = Request.QueryString("type").ToString()
            If type = "order" Then
                Dim ObjCustomer As New SalesWorx.BO.Common.Customer
                Dim dt As New DataTable
                dt = ObjCustomer.GetOrderDetails(Err_No, Err_Desc, "And A.Row_ID='" + ReqID + "'", "")
                Dim dr As DataRow = Nothing
                If dt.Rows.Count > 0 Then
                    dr = dt.Rows(0)
                    If Not IsDBNull(dr("Customer_Signature")) Then
                        _tempsignature = dr("Customer_Signature")
                    End If
                    arrImageData = tempsignature
                End If
                dr = Nothing
                dt = Nothing
                ObjCustomer = Nothing
            ElseIf type = "orderret" Then
                Dim ObjCustomer As New SalesWorx.BO.Common.Customer
                Dim dt As New DataTable
                dt = ObjCustomer.GetOrderReturnDetails(Err_No, Err_Desc, "And A.Row_ID='" + ReqID + "'", "")
                Dim dr As DataRow = Nothing
                If dt.Rows.Count > 0 Then
                    dr = dt.Rows(0)
                    If Not IsDBNull(dr("Customer_Signature")) Then
                        _tempsignature = dr("Customer_Signature")
                    End If
                    arrImageData = tempsignature
                End If
                dr = Nothing
                dt = Nothing
                ObjCustomer = Nothing
            End If
        Else
            Dim objVanReq As New SalesWorx.BO.Common.VanRequisition
            objVanReq.StockRequisition_ID = ReqID
            objVanReq.Load()
        End If
        If Not IsNothing(arrImageData) Then
            arrImageData = DecompressData(arrImageData)
            If Not arrImageData Is Nothing Then
                Response.ContentType = "image/bmp"
                Response.BinaryWrite(arrImageData)
            Else
                Response.Write("Invalid signature data")
            End If
        End If
        Response.Flush()
        Response.End()
    End Sub
    Private Function DecompressData(ByVal data() As Byte) As Byte()
        Dim retVal() As Byte = Nothing
        Dim oInputStream As MemoryStream = Nothing
        Dim oOutputStream As MemoryStream = Nothing
        Dim oGZipStream As GZipStream = Nothing
        Try
            oInputStream = New MemoryStream(data)
            oInputStream.Seek(0, SeekOrigin.Begin)

            oOutputStream = New MemoryStream()
            oGZipStream = New Compression.GZipStream(oInputStream, CompressionMode.Decompress)

            Dim buffer As Byte() = New Byte(2047) {}
            Dim size As Integer = 2048

            While True
                size = oGZipStream.Read(buffer, 0, buffer.Length)
                If size > 0 Then
                    oOutputStream.Write(buffer, 0, size)
                Else
                    Exit While
                End If
            End While

            oGZipStream.Flush()
            oGZipStream.Close()

            oOutputStream.Flush()

            retVal = oOutputStream.ToArray()
        Finally
            If Not oInputStream Is Nothing Then
                oInputStream.Dispose()
                oInputStream = Nothing
            End If
            If Not oGZipStream Is Nothing Then
                oGZipStream.Dispose()
                oGZipStream = Nothing
            End If
            If Not oOutputStream Is Nothing Then
                oOutputStream.Dispose()
                oOutputStream = Nothing
            End If
        End Try
        Return retVal
    End Function
End Class