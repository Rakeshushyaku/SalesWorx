Imports log4net
Imports NHibernate.Cfg.ConfigurationSchema
Imports System.Configuration
Imports System.Data.SqlClient


Public Class DatabaseHandler

    Private Shared ReadOnly log As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Shared _connString As String = Nothing

    Private Shared Sub Init()
        If String.IsNullOrEmpty(_connString) Then
            Dim oNHCfg As HibernateConfiguration
            Try

                oNHCfg = CType(System.Configuration.ConfigurationManager.GetSection("hibernate-configuration"), HibernateConfiguration)
                _connString = oNHCfg.SessionFactory.Properties.Item("connection.connection_string")
                oNHCfg = Nothing
            Catch ex As Exception
                log.Error(ex.Message)
            End Try
        End If
    End Sub

    Public Shared Sub OpenDBConnection(ByRef sqlConn As SqlConnection)
        Init()

        CloseDBConnection(sqlConn)

        sqlConn = New SqlConnection(_connString)
        sqlConn.Open()
    End Sub

    Public Shared Sub GetDBCommand(ByRef sqlConn As SqlConnection, ByRef sqlCmd As SqlCommand)
        If Not sqlConn Is Nothing Then
            sqlCmd = New SqlCommand()
            sqlCmd.Connection = sqlConn
        End If
    End Sub

    Public Shared Sub CloseDBConnection(ByRef sqlConn As SqlConnection)
        If Not IsNothing(sqlConn) Then
            sqlConn.Close()
            sqlConn = Nothing
        End If
    End Sub

    Public Shared Sub CloseDBResources(Optional ByRef sqlConn As SqlConnection = Nothing, Optional ByRef sqlCmd As SqlCommand = Nothing, Optional ByRef sqlDR As SqlDataReader = Nothing, Optional ByRef sqlDA As SqlDataAdapter = Nothing, Optional ByRef ds As DataSet = Nothing)
        If Not IsNothing(sqlDR) Then
            sqlDR.Close()
            sqlDR = Nothing
        End If

        If Not IsNothing(sqlCmd) Then
            sqlCmd.Dispose()
            sqlCmd = Nothing
        End If
        If Not sqlDA Is Nothing Then
            sqlDA.Dispose()
            sqlDA = Nothing
        End If
        If Not ds Is Nothing Then
            ds.Dispose()
            ds = Nothing
        End If
        CloseDBConnection(sqlConn)
    End Sub

End Class
