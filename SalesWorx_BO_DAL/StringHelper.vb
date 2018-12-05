Public Class StringHelper
    Public Shared Function SqlBless(ByVal QryString As Object) As Object
        If Not (IsNothing(QryString) Or IsDBNull(QryString)) Then
            SqlBless = Replace(CStr(QryString), "'", "''")
        Else
            SqlBless = QryString
        End If
    End Function
End Class
