Public Class AppInfo

    Private Shared VERSION As String = "1.1.1000"
    Private Shared NAME As String = "Salesworx"
    Private Shared EDITION As String = "Back Office Server"
    Private Shared AUTHOR As String = "Unique Computer Systems FZE"
    Private Shared COPYRIGHT_YEAR As String = "2015"

    Public Shared Function GetVersion() As String
        Return VERSION
    End Function

    Public Shared Function GetName() As String
        Return NAME
    End Function

    Public Shared Function GetEdition() As String
        Return EDITION
    End Function

    Public Shared Function GetAuthor() As String
        Return AUTHOR
    End Function

    Public Shared Function GetCopyrightYear() As String
        Return COPYRIGHT_YEAR
    End Function

End Class
