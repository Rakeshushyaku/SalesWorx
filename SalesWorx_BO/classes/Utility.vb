Public Class Utility
    Public Shared Function ProcessSqlParamString(ByVal Str As String) As String
        Dim ProcessStr As String
        ProcessStr = Str
        If Not (Str Is DBNull.Value) And Str <> "" Then
            ProcessStr = Replace(ProcessStr, "'", "''")
            ProcessStr = Replace(ProcessStr, ";", "")
            ProcessStr = Replace(ProcessStr, "--", "")
            ProcessStr = Replace(ProcessStr, "/*", "")
            ProcessStr = Replace(ProcessStr, "exec ", "")
        End If
        Return ProcessStr
    End Function
End Class
