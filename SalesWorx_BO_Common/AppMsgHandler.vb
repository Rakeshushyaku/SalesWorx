Public Module AppMsgHandler

    ''' <summary>
    ''' method for retrieving an error description from a resource file
    ''' </summary>
    ''' <param name="errorCode">errorCode to get the description for</param>
    ''' <returns>a string value</returns>
    Public Function GetErrorMessage(ByVal errorCode As String) As String
        Dim resourceValue As String = String.Empty
        If Not String.IsNullOrEmpty(errorCode) Then
            Try
                resourceValue = My.Resources.ErrorMessages.ResourceManager.GetString(errorCode)
            Catch ex As Exception
                resourceValue = String.Format("Unable to retrieve a valid error description for the error code: {0}", errorCode)
            End Try
        End If
        Return resourceValue
    End Function

    ''' <summary>
    ''' method for retrieving an error description from a resource file
    ''' </summary>
    ''' <param name="infoCode">infoCode to get the description for</param>
    ''' <returns>a string value</returns>
    Public Function GetInfoMessage(ByVal infoCode As String) As String
        Dim resourceValue As String = String.Empty
        If Not String.IsNullOrEmpty(infoCode) Then
            Try
                resourceValue = My.Resources.InfoMessages.ResourceManager.GetString(infoCode)
            Catch ex As Exception
                resourceValue = String.Format("Unable to retrieve a valid description for the info code: {0}", infoCode)
            End Try
        End If
        Return resourceValue
    End Function

    ''' <summary>
    ''' method for retrieving exception information in a specific formatted manner
    ''' </summary>
    ''' <param name="oEx">exception object to get information for</param>
    ''' <returns>a string value</returns>
    Public Function GetExceptionInfo(ByRef oEx As Exception) As String
        Dim oST As StackTrace = Nothing
        Dim oSF As StackFrame = Nothing
        Dim retVal As String = oEx.Message
        Try
            If Not IsNothing(oEx) Then
                oST = New StackTrace(oEx, True)
                oSF = oST.GetFrame(oST.FrameCount - 1)
                If Not IsNothing(oSF) Then
                    retVal = String.Format("{0}[{1}]", oEx.Message, oSF.GetFileLineNumber())
                End If
            End If
        Catch ex As Exception
        Finally
            oST = Nothing
            oSF = Nothing
        End Try
        Return retVal
    End Function

End Module
