Public Class Purpose
    Public Enum Purpose_desc_for_ddl
        Returns
    End Enum
    Public Enum Purpose_Val_for_ddl
        R
    End Enum

    Public Enum SurveyTypeCode_desc_for_ddl
        Normal
        Audit
        Market
    End Enum
    Public Enum SurveyTypeCode_Val_for_ddl
        N
        A
        M
    End Enum
    
    Public Function BindToEnum() As Hashtable
        ' get the Reason Code Purpose from the enumeration
        Dim Purpose As String() = System.Enum.GetNames(GetType(Purpose_desc_for_ddl))
        '' get the values from the enumeration
        Dim values As String() = System.Enum.GetNames(GetType(Purpose_Val_for_ddl))
        '' turn it into a hash table
        Dim ht As Hashtable = New Hashtable
        Dim i As Integer = 0
        For i = 0 To Purpose.Length - 1
            ht.Add(Purpose(i), values(i))
        Next
        Return ht
    End Function

    Public Function BindSurveyTypeCode() As Hashtable

        Dim SType As String() = System.Enum.GetNames(GetType(SurveyTypeCode_desc_for_ddl))

        Dim values As String() = System.Enum.GetNames(GetType(SurveyTypeCode_Val_for_ddl))

        Dim ht As Hashtable = New Hashtable
        Dim i As Integer = 0
        For i = 0 To SType.Length - 1
            ht.Add(SType(i), values(i))
        Next
        Return ht
    End Function
End Class
