Imports System.Configuration
Imports System.Data.SqlClient
Public Class DAL_User
    Public Sub New()
        _objDB = New DatabaseConnection
    End Sub
    Private _objDB As DatabaseConnection
    Private Shared _strSQLConn As String = ConfigurationSettings.AppSettings("SQLConnString")

    Public Function SaveUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUsername As String, ByVal _sPassword As String, ByVal _sSalesRepID As Integer, ByVal _sUserTypeID As Integer, ByVal _sIsSS As String, ByVal _sAssignedSalesRep As ArrayList, ByVal _sOrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim _sUserID As Integer
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Try
                sQry = "insert into TBL_User(Username, Password, SalesRep_ID, User_Type_ID,Is_SS,Org_HE_ID) values(@Username, @Password, @SalesRep_ID, @User_Type_ID,@Is_SS,@Org_HE_ID)"

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Parameters.AddWithValue("@Username", _sUsername)
                objSQLCmd.Parameters.AddWithValue("@Password", _sPassword)
                objSQLCmd.Parameters.AddWithValue("@SalesRep_ID", _sSalesRepID)
                objSQLCmd.Parameters.AddWithValue("@User_Type_ID", _sUserTypeID)
                objSQLCmd.Parameters.AddWithValue("@Is_SS", _sIsSS)
                If Val(_sOrgID) > 0 Then
                 objSQLCmd.Parameters.AddWithValue("@Org_HE_ID", _sOrgID)
                Else
                 objSQLCmd.Parameters.AddWithValue("@Org_HE_ID", DBNull.Value)
                End If
                objSQLCmd.Transaction = myTrans
                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

                objSQLCmd = New SqlCommand("select @@IDENTITY as User_ID", objSQLConn)
                objSQLCmd.Transaction = myTrans
                _sUserID = CStr(objSQLCmd.ExecuteScalar())

                If _sIsSS <> "N" Then
                    Try
                        For Each RepID As Integer In _sAssignedSalesRep
                            sQry = String.Format("insert into TBL_User_FSR_Map(User_ID, SalesRep_ID) values('{0}','{1}')", _sUserID, RepID)
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        myTrans.Rollback()
                        Throw ex
                    End Try

                End If
                objSQLCmd.Dispose()
                myTrans.Commit()
                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 77001
                    Error_Desc = "Unable to save user."
                End If
            Catch ex As Exception
                myTrans.Rollback()
                Throw ex
            End Try
        Catch ex As Exception
            Error_No = 77001
            Error_Desc = String.Format("Error while saving user: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        SaveUser = retVal
    End Function
    Public Function UserExists(ByVal _sUserName As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            sQry = String.Format("Select count(UserName) from  TBL_User where UserName='{0}'", _sUserName)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            If iRowsAffected > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UpdateUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUsername As String, ByVal _sPassword As String, ByVal _sSalesRepID As Integer, ByVal _sUserTypeID As Integer, ByVal _sUserID As Integer, ByVal _sIsSS As String, ByVal _sAssignedSalesRep As ArrayList, ByVal _sOrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Try
                sQry = String.Format("update TBL_User set Username=@Username, Password=@Password, SalesRep_ID=@SalesRep_ID, User_Type_ID=@User_Type_ID, Is_SS=@Is_SS,Sync_Timestamp=getdate(),Org_HE_ID=@Org_HE_ID where User_ID=@User_ID", _sUsername, _sPassword, _sSalesRepID, _sUserTypeID, _sUserID, _sIsSS)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Parameters.AddWithValue("@Username", _sUsername)
                objSQLCmd.Parameters.AddWithValue("@Password", _sPassword)
                objSQLCmd.Parameters.AddWithValue("@SalesRep_ID", _sSalesRepID)
                objSQLCmd.Parameters.AddWithValue("@User_Type_ID", _sUserTypeID)
                objSQLCmd.Parameters.AddWithValue("@Is_SS", _sIsSS)
                objSQLCmd.Parameters.AddWithValue("@User_ID", _sUserID)
                If Val(_sOrgID) > 0 Then
                 objSQLCmd.Parameters.AddWithValue("@Org_HE_ID", _sOrgID)
                Else
                 objSQLCmd.Parameters.AddWithValue("@Org_HE_ID", DBNull.Value)
                End If
                objSQLCmd.Transaction = myTrans
                iRowsAffected = objSQLCmd.ExecuteNonQuery()

                ' if <>N to N / or modify <> N
                sQry = String.Format("delete from TBL_User_FSR_Map where User_ID={0}", _sUserID)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Transaction = myTrans
                objSQLCmd.ExecuteNonQuery()

                If _sIsSS <> "N" Then
                    Try
                        For Each RepID As Integer In _sAssignedSalesRep
                            sQry = String.Format("insert into TBL_User_FSR_Map(User_ID, SalesRep_ID) values('{0}','{1}')", _sUserID, RepID)
                            objSQLCmd = New SqlCommand(sQry, objSQLConn)
                            objSQLCmd.Transaction = myTrans
                            objSQLCmd.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        myTrans.Rollback()
                        Throw ex
                    End Try

                End If
                objSQLCmd.Dispose()
                myTrans.Commit()
            Catch ex As Exception
                myTrans.Rollback()
                Throw ex
            End Try
            If iRowsAffected > 0 Then
                retVal = True
            Else
                Error_No = 77002
                Error_Desc = "Unable to update user."
            End If

        Catch ex As Exception
            Error_No = 77002
            Error_Desc = String.Format("Error while updating user: {0}", ex.Message)
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
            UpdateUser = retVal
    End Function


    Public Function DeleteUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Try

                'getting MSSQL DB connection.....

                sQry = String.Format("delete from TBL_User_FSR_Map where User_ID='{0}'", _sUserID)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Transaction = myTrans
                iRowsAffected = objSQLCmd.ExecuteNonQuery()

                sQry = String.Format("delete from TBL_User where User_ID='{0}'", _sUserID)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Transaction = myTrans
                iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()
                myTrans.Commit()
                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 77003
                    Error_Desc = "Unable to delete user."
                End If

            Catch ex As Exception
                myTrans.Rollback()
                Throw ex
            End Try
        Catch ex As Exception

            Error_No = 77003
            Error_Desc = String.Format("Error while deleting user: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteUser = retVal
    End Function


    Public Function ChangePassword(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sOldPassword As String, ByVal _sNewPassword As String, ByVal _sUserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            If _sOldPassword <> "" And _sNewPassword <> "" Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                sQry = String.Format("update TBL_User set Password='{0}' where Password='{1}' AND User_Id={2}", _sNewPassword, _sOldPassword, _sUserID)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 77004
                    Error_Desc = "Invalid old password."
                End If
            Else
                Error_No = 77004
                Error_Desc = "Invalid passwords specified."
            End If

        Catch ex As Exception
            Error_No = 77004
            Error_Desc = String.Format("Error while changing password: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        ChangePassword = retVal
    End Function
  Public Function GetAllUsers() As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select A.*,B.User_Type from TBL_User A inner join TBL_User_Types B on A.User_Type_ID=B.User_Type_ID order by Username ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt

    End Function
    Public Function GetUsers() As DataTable

        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select User_ID, Username from TBL_User order by Username ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt

    End Function


    Public Function GetUser(ByRef Error_No As Long, ByRef Error_Desc As String, ByRef _sUsername As String, ByRef _sPassword As String, ByRef _sSalesRepID As Integer, ByRef _sUserTypeID As Integer, ByRef _sUserID As String, ByRef _sIsSS As String, ByRef _sAssignedSalesRep As ArrayList, ByRef _sOrgID As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader

        Dim sQry As String
        Dim bRetBool As Boolean = False

        Try
            If _sUserID <> "" Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                sQry = String.Format("select Username, Password, SalesRep_ID, User_Type_ID, Is_SS,Org_HE_ID from TBL_User where User_ID='{0}'", _sUserID)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

                Dim tempDBVal As Object

                If objSQLDR.Read Then
                    tempDBVal = objSQLDR.GetValue(0)
                    _sUsername = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                    tempDBVal = objSQLDR.GetValue(1)
                    _sPassword = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                    tempDBVal = objSQLDR.GetValue(2)
                    _sSalesRepID = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                    tempDBVal = objSQLDR.GetValue(3)
                    _sUserTypeID = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                    tempDBVal = objSQLDR.GetValue(4)
                    _sIsSS = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)

                    tempDBVal = objSQLDR.GetValue(5)
                    _sOrgID = IIf(IsDBNull(tempDBVal), "0", tempDBVal)
                    bRetBool = True
                End If

                objSQLDR.Close()


                sQry = String.Format("select User_ID, SalesRep_ID from TBL_User_FSR_Map where User_ID='{0}'", _sUserID)
                objSQLConn = _objDB.GetSQLConnection
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)
                _sAssignedSalesRep = New ArrayList
                While objSQLDR.Read()
                    _sAssignedSalesRep.Add(objSQLDR.GetValue(1))
                End While
                objSQLDR.Close()
                objSQLCmd.Dispose()
            Else
                _sUsername = ""
                _sPassword = ""
                _sSalesRepID = ""
                _sUserTypeID = ""
                _sIsSS = ""
                bRetBool = True
            End If

        Catch ex As Exception
            Error_No = 77005
            Error_Desc = ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetUser = bRetBool
    End Function


    Public Function GetUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserTypeID As Integer, ByVal _sUserType As String, ByVal _sPDARights As String, ByRef _sUserDesignation As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim bRetBool As Boolean = False

        Try
            Dim _alMenuID As ArrayList
            Dim _alPageID As ArrayList
            Dim _alFieldRights As ArrayList
            If _sUserTypeID <> "" Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                sQry = String.Format("select A.User_Type, A.PDA_Rights , B.Menu_ID, B.Page_ID, B.Field_Rights,A.Designation from TBL_User_Types as A, TBL_User_Rights as B where A.User_Type_ID=B.User_Type_ID AND A.User_Type_ID='{0}'", _sUserTypeID)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

                Dim tempDBVal As Object

                While objSQLDR.Read
                    If IsNothing(_sUserType) Then
                        tempDBVal = objSQLDR.GetValue(0)
                        _sUserType = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)
                        tempDBVal = objSQLDR.GetValue(1)
                        _sPDARights = IIf(IsDBNull(tempDBVal), "N/A", tempDBVal)

                        tempDBVal = objSQLDR.GetValue(5)

                        _sUserDesignation = IIf(IsDBNull(tempDBVal), "U", tempDBVal)
                        _alMenuID = New ArrayList
                        _alPageID = New ArrayList
                        _alFieldRights = New ArrayList
                    End If
                    tempDBVal = objSQLDR.GetValue(2)
                    _alMenuID.Add(IIf(IsDBNull(tempDBVal), "NA", tempDBVal))
                    tempDBVal = objSQLDR.GetValue(3)
                    _alPageID.Add(IIf(IsDBNull(tempDBVal), "NA", tempDBVal))
                    tempDBVal = objSQLDR.GetValue(4)
                    _alFieldRights.Add(IIf(IsDBNull(tempDBVal), "NA", tempDBVal))
                    bRetBool = True
                End While

                objSQLDR.Close()
                objSQLCmd.Dispose()
            Else
                _sUserType = ""
                _alMenuID = New ArrayList(0)
                _alPageID = New ArrayList(0)
                _alFieldRights = New ArrayList(0)
                bRetBool = True
            End If

        Catch ex As Exception
            Error_No = 77006
            Error_Desc = ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetUserType = bRetBool
    End Function


    Public Function SaveUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserType As String, ByVal dtRights As DataTable, ByVal PDA_Rights As Long, ByVal VanRights As String, ByVal _sUserDesignation As String) As Boolean
        Try
            Dim objSQLConn As SqlConnection
            Dim iRowsAffected As Integer = 0
            Dim UserTypeID As Integer = 0
            Dim Result As Boolean = False
            Dim sQry As String
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Dim objSQLCmd As New SqlCommand("app_InsertUserType", objSQLConn)
            objSQLCmd.CommandType = CommandType.StoredProcedure
            objSQLCmd.Parameters.AddWithValue("@UserType", _sUserType)
            objSQLCmd.Parameters.AddWithValue("@PDA_Rights", VanRights)
            objSQLCmd.Parameters.AddWithValue("@Designation", _sUserDesignation)

            Dim objOutputParameter As New SqlParameter("@Result", SqlDbType.Int)
            objSQLCmd.Parameters.Add(objOutputParameter)
            objOutputParameter.Direction = ParameterDirection.Output

            Try
                objSQLCmd.Transaction = myTrans
                objSQLCmd.ExecuteNonQuery()
                UserTypeID = Convert.ToInt32(objOutputParameter.Value)
                If UserTypeID = -1 Then
                    Error_Desc = "User already exists."
                    Throw New Exception("User already exists.")
                Else
                    iRowsAffected = 0
                    For Each dr As DataRow In dtRights.Rows
                        sQry = String.Format("insert into TBL_User_Rights(User_Type_Id, Menu_Id, Page_Id, Field_Rights) values({0},'{1}','{2}','{3}')", UserTypeID, dr(0), dr(1), dr(2))
                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
                        objSQLCmd.Transaction = myTrans
                        iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
                        objSQLCmd.Dispose()
                    Next
                    If iRowsAffected = 0 Then
                        Error_No = 77007
                        Throw New Exception("Unable to save user rights.")
                    End If
                    myTrans.Commit()
                    Result = True
                End If

                Return Result
            Catch ex As Exception
                myTrans.Rollback()
                Throw ex
            End Try
        Catch ex As Exception
            Throw ex
        End Try
        'Dim param As 

        'Dim sQry As String

        'objSQLCmd.
        'Dim iRowsAffected As Integer = 0
        'Dim retVal As Boolean = False
        'Try
        '    If _sUserRightsData <> "" Then

        '        'parse user rights data....
        '        Dim arrUserRights() As String
        '        arrUserRights = Split(_sUserRightsData, "^")
        '        Dim itemRights As String
        '        Dim arrItem() As String

        '        Dim lHHUserRights As Long

        '        'filter out HH rights and prepare binary data....
        '        For Each itemRights In arrUserRights
        '            If itemRights <> "" Then
        '                arrItem = Split(itemRights, "#")
        '                If arrItem(0).StartsWith("HM") And arrItem(1).StartsWith("HP") Then
        '                    lHHUserRights = lHHUserRights + Math.Pow(2, CType(arrItem(1).Substring(2), Long) - 1)
        '                End If
        '            End If
        '        Next

        '        'getting MSSQL DB connection.....
        '        objSQLConn = _objDB.GetSQLConnection

        '        sQry = String.Format("insert into TBL_User_Types(User_Type, PDA_Rights) values('{0}',{1})", StringHelper.SqlBless(_sUserType), lHHUserRights)

        '        objSQLCmd = New SqlCommand(sQry, objSQLConn)
        '        iRowsAffected = objSQLCmd.ExecuteNonQuery()
        '        objSQLCmd.Dispose()

        '        objSQLCmd = New SqlCommand("select @@IDENTITY as User_Type_ID", objSQLConn)
        '        _sUserTypeID = CStr(objSQLCmd.ExecuteScalar())
        '        objSQLCmd.Dispose()

        '        If iRowsAffected > 0 Then
        '            iRowsAffected = 0

        '            For Each itemRights In arrUserRights
        '                If itemRights <> "" Then
        '                    arrItem = Split(itemRights, "#")
        '                    sQry = String.Format("insert into TBL_User_Rights(User_Type_Id, Menu_Id, Page_Id, Field_Rights) values({0},'{1}','{2}','{3}')", _sUserTypeID, arrItem(0), arrItem(1), arrItem(2))
        '                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
        '                    iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
        '                    objSQLCmd.Dispose()
        '                End If
        '            Next

        '            If iRowsAffected > 0 Then
        '                retVal = True
        '            Else
        '                Error_No = 77007
        '                Error_Desc = "Unable to save user rights."
        '            End If
        '        Else
        '            Error_No = 77007
        '            Error_Desc = "Unable to save user type."
        '        End If

        '    Else
        '        Error_No = 77007
        '        Error_Desc = "No user rights specified."
        '    End If

        'Catch ex As Exception
        '    Error_No = 77007
        '    Error_Desc = String.Format("Error while saving user type: {0}", ex.Message)
        'Finally
        '    objSQLCmd = Nothing
        '    _objDB.CloseSQLConnection(objSQLConn)
        'End Try
        ' SaveUserType = retVal
    End Function

    Public Function GetUserRights(ByVal User_Type_Id As Integer) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDt As New DataTable
        Dim objSQLDa As New SqlDataAdapter
        Dim sQry As String
        Dim bRetBool As Boolean = False

        Try
            If User_Type_Id <> 0 Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                sQry = String.Format("select *,ISNULL((SELECT PDA_Rights FROM TBL_User_Types WHERE User_Type_Id=A.User_Type_ID),'0')AS PDARights ,ISNULL((SELECT Designation FROM TBL_User_Types WHERE User_Type_Id=A.User_Type_ID),'U')AS Designation from  TBL_User_Rights AS A  where User_Type_Id={0}", User_Type_Id)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDa = New SqlDataAdapter(objSQLCmd)
                objSQLDa.Fill(objSQLDt)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(objSQLConn) Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return objSQLDt
    End Function


    Public Shared Function SqlBless(ByVal QryString As Object) As Object
        If Not (IsNothing(QryString) Or IsDBNull(QryString)) Then
            SqlBless = Replace(CStr(QryString), "'", "''")
        Else
            SqlBless = QryString
        End If
    End Function
    Public Function UpdateUserRights(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserType As String, ByVal _sUserTypeID As Integer, ByVal dtRights As DataTable, ByVal PDA_Rights As Long, ByVal VanRights As String, ByVal _SUserDesignation As String) As Boolean
        Try
            Dim objSQLConn As SqlConnection
            Dim objSQLCmd As New SqlCommand
            Dim iRowsAffected As Integer = 0

            Dim Result As Boolean = False
            Dim sQry As String
            objSQLConn = _objDB.GetSQLConnection
            Dim myTrans As SqlTransaction = objSQLConn.BeginTransaction()
            Try
                sQry = String.Format("delete from TBL_User_Rights where User_Type_ID='{0}'", _sUserTypeID)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Transaction = myTrans
                objSQLCmd.CommandTimeout = 120
                iRowsAffected = objSQLCmd.ExecuteNonQuery()

                sQry = String.Format("update TBL_User_Types set User_Type='{0}', Designation='{3}',PDA_Rights='{1}' where User_Type_ID='{2}'", StringHelper.SqlBless(_sUserType), VanRights, _sUserTypeID, _sUserDesignation)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLCmd.Transaction = myTrans
                objSQLCmd.ExecuteNonQuery()
                iRowsAffected = objSQLCmd.ExecuteNonQuery()


                If iRowsAffected > 0 Then
                    iRowsAffected = 0
                    For Each dr As DataRow In dtRights.Rows
                        sQry = String.Format("insert into TBL_User_Rights(User_Type_Id, Menu_Id, Page_Id, Field_Rights) values({0},'{1}','{2}','{3}')", _sUserTypeID, dr(0), dr(1), dr(2))
                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
                        objSQLCmd.Transaction = myTrans
                        iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
                        objSQLCmd.Dispose()
                    Next
                    If iRowsAffected = 0 Then
                        Error_No = 77007
                        Throw New Exception("Unable to update user rights.")
                    End If
                    myTrans.Commit()
                    Result = True
                Else
                    Result = False
                    Error_No = 77201
                    Throw New Exception("Unable to update user type.")
                End If
                Return Result
            Catch ex As Exception
                Error_No = 77007
                Throw New Exception("Unable to update user rights.")
                myTrans.Rollback()
            End Try
        Catch ex As Exception
            Error_No = 77007
            Throw New Exception("Unable to update user rights.")
            Throw ex
        End Try

    End Function
    'Public Function UpdateUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserRightsData As String, ByVal _sUserType As String, ByVal _sUserTypeID As Integer) As Boolean
    '    Dim objSQLConn As SqlConnection
    '    Dim objSQLCmd As SqlCommand
    '    Dim sQry As String
    '    Dim iRowsAffected As Integer = 0
    '    Dim retVal As Boolean = False
    '    Try
    '        If _sUserRightsData <> "" Then

    '            'parse user rights data....
    '            Dim arrUserRights() As String
    '            arrUserRights = Split(_sUserRightsData, "^")
    '            Dim itemRights As String
    '            Dim arrItem() As String

    '            Dim lHHUserRights As Long

    '            'filter out HH rights and prepare binary data....
    '            For Each itemRights In arrUserRights
    '                If itemRights <> "" Then
    '                    arrItem = Split(itemRights, "#")
    '                    If arrItem(0).StartsWith("HM") And arrItem(1).StartsWith("HP") Then
    '                        lHHUserRights = lHHUserRights + Math.Pow(2, CType(arrItem(1).Substring(2), Long) - 1)
    '                    End If
    '                End If
    '            Next

    '            'getting MSSQL DB connection.....
    '            objSQLConn = _objDB.GetSQLConnection

    '            sQry = String.Format("update TBL_User_Types set User_Type='{0}', PDA_Rights={1} where User_Type_ID='{2}'", StringHelper.SqlBless(_sUserType), lHHUserRights, _sUserTypeID)

    '            objSQLCmd = New SqlCommand(sQry, objSQLConn)
    '            iRowsAffected = objSQLCmd.ExecuteNonQuery()
    '            objSQLCmd.Dispose()

    '            If iRowsAffected > 0 Then

    '                sQry = String.Format("DELETE from TBL_User_Rights where User_Type_Id={0}", _sUserTypeID)

    '                objSQLCmd = New SqlCommand(sQry, objSQLConn)
    '                objSQLCmd.ExecuteNonQuery()
    '                objSQLCmd.Dispose()

    '                iRowsAffected = 0

    '                For Each itemRights In arrUserRights
    '                    If itemRights <> "" Then
    '                        arrItem = Split(itemRights, "#")
    '                        sQry = String.Format("insert into TBL_User_Rights(User_Type_Id, Menu_Id, Page_Id, Field_Rights) values({0},'{1}','{2}','{3}')", _sUserTypeID, arrItem(0), arrItem(1), arrItem(2))
    '                        objSQLCmd = New SqlCommand(sQry, objSQLConn)
    '                        iRowsAffected = iRowsAffected + objSQLCmd.ExecuteNonQuery()
    '                        objSQLCmd.Dispose()
    '                    End If
    '                Next

    '                If iRowsAffected > 0 Then
    '                    retVal = True
    '                Else
    '                    Error_No = 77008
    '                    Error_Desc = "Unable to update user rights."
    '                End If
    '            Else
    '                Error_No = 77008
    '                Error_Desc = "Unable to update user type."
    '            End If
    '        Else
    '            Error_No = 77008
    '            Error_Desc = "No user rights specified."
    '        End If

    '    Catch ex As Exception
    '        Error_No = 77007
    '        Error_Desc = String.Format("Error while updating user type: {0}", ex.Message)
    '    Finally
    '        objSQLCmd = Nothing
    '        _objDB.CloseSQLConnection(objSQLConn)
    '    End Try
    '    UpdateUserType = retVal
    'End Function

    Public Function DeleteUserType(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserTypeID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            If Not HasUsersAssigned(Error_No, Error_Desc, _sUserTypeID) Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                sQry = String.Format("delete from TBL_User_Types where User_Type_ID='{0}'", _sUserTypeID)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

                If iRowsAffected > 0 Then
                    iRowsAffected = 0
                    sQry = String.Format("delete from TBL_User_Rights where User_Type_ID='{0}'", _sUserTypeID)

                    objSQLCmd = New SqlCommand(sQry, objSQLConn)
                    iRowsAffected = objSQLCmd.ExecuteNonQuery()
                    objSQLCmd.Dispose()

                    If iRowsAffected > 0 Then
                        retVal = True
                    Else
                        Error_No = 77009
                        Error_Desc = "Unable to delete user rights."
                    End If
                Else
                    Error_No = 77009
                    Error_Desc = "Unable to delete user type."
                End If
            Else
                retVal = False
                Error_Desc = "There are some users are associated with this role.Before deleting the user type,please delete all the users associated with this type."
            End If

        Catch ex As Exception
            Error_No = 77009
            Error_Desc = String.Format("Error while deleting user type: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        DeleteUserType = retVal
    End Function
    Public Function IsAssignedSalesRep(ByVal _sSalesRepID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("select count(*) from TBL_User where SalesRep_ID='{0}'", _sSalesRepID)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            If iRowsAffected > 0 Then
                retVal = True
            Else
                retVal = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        IsAssignedSalesRep = retVal
    End Function
    Public Function IsAssignedSalesRep_ForUpdate(ByVal _sSalesRepID As Integer, ByVal _sUserID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("select count(*) from TBL_User where SalesRep_ID='{0}' and User_ID<>'{1}'", _sSalesRepID, _sUserID)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            If iRowsAffected > 0 Then
                retVal = True
            Else
                retVal = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        IsAssignedSalesRep_ForUpdate = retVal
    End Function

    Private Function HasUsersAssigned(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal _sUserTypeID As Integer) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = True
        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("select count(*) from TBL_User where User_Type_ID='{0}'", _sUserTypeID)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            iRowsAffected = CInt(objSQLCmd.ExecuteScalar())
            objSQLCmd.Dispose()

            If iRowsAffected > 0 Then
                retVal = True
                Error_No = 77010
                Error_Desc = "Please first delete all users under this user type."
            Else
                retVal = False
            End If

        Catch ex As Exception
            Error_No = 77010
            Error_Desc = String.Format("Error while deleting user type: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        HasUsersAssigned = retVal
    End Function

    Public Function GetSalesReps() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select SalesRep_ID, SalesRep_Name from TBL_FSR order by SalesRep_Name ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt


     
    End Function

    Public Function GetPDARights(ByVal Designation As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select Code_Value AS Code,Code_Description AS Description from TBL_App_Codes WHERE Code_Type='ENABLE_MODULES' AND Custom_Attribute_1 LIKE '%' + @Designation +'%' order by  Description ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLCmd.Parameters.AddWithValue("@Designation", designation)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function

    Public Function GetUserTypesByDesignation(ByVal Designation As String) As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select User_Type_ID, User_Type,PDA_Rights from TBL_User_Types WHERE Designation=@Designation order by User_Type ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLCmd.Parameters.AddWithValue("@Designation", Designation)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
    Public Function GetUserTypes() As DataTable
        Dim objSQLConn As SqlConnection
        Dim objSQLDa As New SqlDataAdapter
        Dim objSQLCmd As New SqlCommand
        Dim dt As New DataTable

        Dim sQry As String

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection
            sQry = "select User_Type_ID, User_Type,PDA_Rights from TBL_User_Types order by User_Type ASC"

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDa = New SqlDataAdapter(objSQLCmd)
            objSQLDa.Fill(dt)
            objSQLCmd.Dispose()
            Return dt
        Catch ex As Exception
            Throw ex
        Finally

            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try

        Return dt
    End Function
    'retrieves salesreps under a particular supervisor
    'by matching TBL_Org_CTL_DTL.ORG_ID
    Public Function GetSalesRepsORG(ByVal SupervisorID As String, ByVal _sSalesRepID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String = ""

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = String.Format("SELECT SalesRep_ID, SalesRep_Name from TBL_FSR where SalesRep_ID In (SELECT DISTINCT A.SalesRep_ID FROM TBL_Org_CTL_DTL As A INNER JOIN TBL_Org_CTL_DTL As B ON A.Org_ID=B.Org_ID WHERE B.SalesRep_ID={0}) ORDER BY SalesRep_Name ASC", SupervisorID)

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim tempDBVal As Object
            Dim sSalesRepID As String
            Dim sSalesRepName As String

            While objSQLDR.Read
                sSalesRepID = CStr(objSQLDR.GetValue(0))
                tempDBVal = objSQLDR.GetValue(1)
                tempDBVal = IIf(IsDBNull(tempDBVal), "NA", tempDBVal)
                sSalesRepName = CStr(tempDBVal)
                If Not IsNothing(_sSalesRepID) Then
                    sRetVal = String.Format("{0}{1}<option value='{2}' {4}>{3}</option>", sRetVal, vbCrLf, sSalesRepID, sSalesRepName, IIf(_sSalesRepID.IndexOf(sSalesRepID) >= 0, "selected", ""))
                Else
                    sRetVal = String.Format("{0}{1}<option value='{2}'>{3}</option>", sRetVal, vbCrLf, sSalesRepID, sSalesRepName)
                End If
            End While

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
            sRetVal = sRetVal & ex.Message
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        Return sRetVal
    End Function
    Public Function GetUserName(ByVal UserID As String) As String
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDR As SqlDataReader
        Dim sQry As String
        Dim sRetVal As String = ""

        Try
            'getting MSSQL DB connection.....
            objSQLConn = _objDB.GetSQLConnection

            sQry = "select Username from TBL_User where User_ID=" & UserID

            objSQLCmd = New SqlCommand(sQry, objSQLConn)
            objSQLDR = objSQLCmd.ExecuteReader(CommandBehavior.CloseConnection)


            If objSQLDR.Read Then
                sRetVal = CStr(objSQLDR.GetValue(0).ToString)
            End If

            objSQLDR.Close()
            objSQLCmd.Dispose()

        Catch ex As Exception
        Finally
            objSQLDR = Nothing
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        GetUserName = sRetVal
    End Function
    Public Function ChangePassword(ByRef Error_No As Long, ByRef Error_Desc As String, ByVal OldPassword As String, ByVal NewPassword As String, ByVal UserId As String) As Boolean
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim sQry As String
        Dim iRowsAffected As Integer = 0
        Dim retVal As Boolean = False
        Try
            If OldPassword <> "" And NewPassword <> "" Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection

                sQry = String.Format("update TBL_User set Password='{0}' where Password='{1}' AND User_Id={2}", NewPassword, OldPassword, UserId)

                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                iRowsAffected = objSQLCmd.ExecuteNonQuery()
                objSQLCmd.Dispose()

                If iRowsAffected > 0 Then
                    retVal = True
                Else
                    Error_No = 74212
                    Error_Desc = "You have entered invalid old password."
                End If
            Else
                Error_No = 74212
                Error_Desc = "Invalid passwords entered."
            End If

        Catch ex As Exception
            Error_No = 74213
            Error_Desc = String.Format("Error while changing password: {0}", ex.Message)
        Finally
            objSQLCmd = Nothing
            _objDB.CloseSQLConnection(objSQLConn)
        End Try
        ChangePassword = retVal
    End Function

    'Get Van list based on MASOrgID
    Public Function GetVanlistByOrgID(ByRef Org_HE_ID As Long)
        Dim objSQLConn As SqlConnection
        Dim objSQLCmd As SqlCommand
        Dim objSQLDt As New DataTable
        Dim objSQLDa As New SqlDataAdapter
        Dim sQry As String
        Dim bRetBool As Boolean = False


        Try
            If Org_HE_ID <> 0 Then
                'getting MSSQL DB connection.....
                objSQLConn = _objDB.GetSQLConnection
                sQry = String.Format("SELECT TBL_FSR.SalesRep_ID, SalesRep_Name,isnull(Is_DC_Optional,'N') as Is_DC_Optional FROM TBL_FSR LEFT OUTER JOIN TBL_FSR_Config ON TBL_FSR.SalesRep_ID=TBL_FSR_Config.SalesRep_ID WHERE TBL_FSR.SalesRep_ID IN (SELECT SalesRep_ID FROM TBL_Org_CTL_DTL WHERE Org_HE_ID={0})", Org_HE_ID)
                objSQLCmd = New SqlCommand(sQry, objSQLConn)
                objSQLDa = New SqlDataAdapter(objSQLCmd)
                objSQLDa.Fill(objSQLDt)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(objSQLConn) Then
                _objDB.CloseSQLConnection(objSQLConn)
            End If
        End Try
        Return objSQLDt
    End Function



End Class
