Imports System
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data
Imports System.Data.OleDb
Public Class Crypto
    Private Const Key As String = "#ifEdi$Y7iNE8A=i"
    Private Const IV As String = "6&L~~v7="

    'Public Function Encrypting(ByVal Source As String, ByVal Key As String) As String

    '    Try

    '        Dim _textConverter As New ASCIIEncoding
    '        Dim _rc2CSP As New RC2CryptoServiceProvider
    '        Dim _encrypted() As Byte
    '        Dim _toEncrypt() As Byte

    '        'Get an encryptor.
    '        Dim _encryptor As ICryptoTransform = _rc2CSP.CreateEncryptor(ConvertStringToByteArray(Key), ConvertStringToByteArray(Key))

    '        'Encrypt the data.
    '        Dim _msEncrypt As New MemoryStream
    '        Dim _csEncrypt As New CryptoStream(_msEncrypt, _encryptor, CryptoStreamMode.Write)

    '        'Convert the data to a byte array.
    '        _toEncrypt = _textConverter.GetBytes(Source)

    '        'Write all data to the crypto stream and flush it.
    '        _csEncrypt.Write(_toEncrypt, 0, _toEncrypt.Length)
    '        _csEncrypt.FlushFinalBlock()

    '        'Get encrypted array of bytes.
    '        _encrypted = _msEncrypt.ToArray()

    '        ' convert into Base64 so that the result can be used in xml
    '        Return System.Convert.ToBase64String(_encrypted) ', 0, encrypted.Length - 1)
    '    Catch ex As Exception

    '        Return ""
    '    Finally

    '    End Try
    'End Function

    'Public Function Decrypting(ByVal Source As String, ByVal Key As String) As String


    '    Try

    '        Dim _textConverter As New ASCIIEncoding
    '        Dim _rc2CSP As New RC2CryptoServiceProvider
    '        Dim _toDecrypt() As Byte = System.Convert.FromBase64String(Source)
    '        Dim _msDecrypt As New System.IO.MemoryStream(_toDecrypt, 0, _toDecrypt.Length)


    '        'This is where the message would be transmitted to a recipient
    '        ' who already knows your secret key. Optionally, you can
    '        ' also encrypt your secret key using a public key algorithm
    '        ' and pass it to the mesage recipient along with the RC2
    '        ' encrypted message.            
    '        'Get a decryptor that uses the same key and IV as the encryptor.
    '        Dim decryptor As ICryptoTransform = _rc2CSP.CreateDecryptor(ConvertStringToByteArray(Key), ConvertStringToByteArray(Key))

    '        'Now decrypt the previously encrypted message using the decryptor
    '        ' obtained in the above step.

    '        Dim csDecrypt As New CryptoStream(_msDecrypt, decryptor, CryptoStreamMode.Read)

    '        Dim sr As New System.IO.StreamReader(csDecrypt)
    '        Return sr.ReadToEnd()
    '    Catch ex As Exception

    '        Return ""
    '    Finally

    '    End Try
    'End Function

    Public Function ConvertStringToByteArray(ByVal s As String) As Byte() ' [Byte]()


        Try
            Return (New UnicodeEncoding).GetBytes(s)
        Catch ex As Exception

            Return System.Convert.FromBase64String("00")
        Finally

        End Try
    End Function 'ConvertStringToByteArray
    Public Function ConvertStringToByteArray16(ByVal s As String) As Byte() ' [Byte]()


        Try
            Return (New UTF8Encoding).GetBytes(s)
        Catch ex As Exception

            Return System.Convert.FromBase64String("00")
        Finally

        End Try
    End Function
    'for user password encryption
    Protected Friend Shared Function PasswordEncrypt(ByVal plaintext As String, ByVal keytext As String) As String
        Dim data As Byte()
        Dim key As Byte()
        Dim encoding As System.Text.UTF8Encoding
        Const BLOCK_SIZE As Integer = 6
        Try
            Dim data_length, key_length As Integer

            encoding = New UTF8Encoding
            data = encoding.GetBytes(plaintext)
            key = encoding.GetBytes(keytext)

            data_length = data.Length()
            key_length = key.Length()

            Dim y As Integer = 0
            Dim i As Integer = 0
            While i <= data_length
                For o As Integer = 0 To BLOCK_SIZE
                    If i < data_length Then
                        data(i) = data(i) Xor key(y)
                    End If
                    i += 1
                Next

                y += 1

                If y = key_length Then y = 0
            End While

            Return System.Convert.ToBase64String(data)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'for user password decryption
    Protected Friend Shared Function PasswordDecrypt(ByVal encryptedtext As String, ByVal keytext As String) As String
        Dim data As Byte()
        Dim key As Byte()
        Dim encoding As System.Text.UTF8Encoding
        Const BLOCK_SIZE As Integer = 6
        Try
            Dim data_length, key_length As Integer

            encoding = New UTF8Encoding
            data = System.Convert.FromBase64String(encryptedtext)
            key = encoding.GetBytes(keytext)

            data_length = data.Length()
            key_length = key.Length()

            Dim y As Integer = 0
            Dim i As Integer = 0
            While i <= data_length
                For o As Integer = 0 To BLOCK_SIZE
                    If i < data_length Then
                        data(i) = data(i) Xor key(y)
                    End If
                    i += 1
                Next

                y += 1

                If y = key_length Then y = 0
            End While

            Return encoding.GetString(data)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function encrypt(ByVal plaintext As String, ByVal keytext As String) As String
        Dim data As Byte()
        Dim key As Byte()
        Dim encoding As System.Text.UTF8Encoding
        Const BLOCK_SIZE As Integer = 6
        Try
            Dim data_length, key_length As Integer

            encoding = New UTF8Encoding
            data = encoding.GetBytes(plaintext)
            key = encoding.GetBytes(keytext)

            data_length = data.Length()
            key_length = key.Length()

            Dim y As Integer = 0
            Dim i As Integer = 0
            While i <= data_length
                For o As Integer = 0 To BLOCK_SIZE
                    If i < data_length Then
                        data(i) = data(i) Xor key(y)
                    End If
                    i += 1
                Next

                y += 1

                If y = key_length Then y = 0
            End While

            Return System.Convert.ToBase64String(data, 0, data.Length)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function decrypt(ByVal encryptedtext As String, ByVal keytext As String) As String
        Dim data As Byte()
        Dim key As Byte()
        Dim encoding As System.Text.UTF8Encoding
        Const BLOCK_SIZE As Integer = 6
        Try
            Dim data_length, key_length As Integer

            encoding = New UTF8Encoding
            data = System.Convert.FromBase64String(encryptedtext)
            key = encoding.GetBytes(keytext)

            data_length = data.Length()
            key_length = key.Length()

            Dim y As Integer = 0
            Dim i As Integer = 0
            While i <= data_length
                For o As Integer = 0 To BLOCK_SIZE
                    If i < data_length Then
                        data(i) = data(i) Xor key(y)
                    End If
                    i += 1
                Next

                y += 1

                If y = key_length Then y = 0
            End While

            Return encoding.GetString(data, 0, data.Length)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function DecryptReportName(ByVal Source As String) As String


        Try

            Dim _textConverter As New ASCIIEncoding
            Dim _rc2CSP As New RC2CryptoServiceProvider
            Dim _toDecrypt() As Byte = System.Convert.FromBase64String(Source)
            Dim _msDecrypt As New System.IO.MemoryStream(_toDecrypt, 0, _toDecrypt.Length)


            'This is where the message would be transmitted to a recipient
            ' who already knows your secret key. Optionally, you can
            ' also encrypt your secret key using a public key algorithm
            ' and pass it to the mesage recipient along with the RC2
            ' encrypted message.            
            'Get a decryptor that uses the same key and IV as the encryptor.
            Dim decryptor As ICryptoTransform = _rc2CSP.CreateDecryptor(ConvertStringToByteArray16(Key), ConvertStringToByteArray16(IV))

            'Now decrypt the previously encrypted message using the decryptor
            ' obtained in the above step.

            Dim csDecrypt As New CryptoStream(_msDecrypt, decryptor, CryptoStreamMode.Read)

            Dim sr As New System.IO.StreamReader(csDecrypt)
            Return sr.ReadToEnd()
        Catch ex As Exception

            Return ""
        Finally
        End Try
    End Function
    Public Function EncryptReportName(ByVal Source As String) As String
        Try

            Dim _textConverter As New ASCIIEncoding
            Dim _rc2CSP As New RC2CryptoServiceProvider
            Dim _encrypted() As Byte
            Dim _toEncrypt() As Byte
           
            'Get an encryptor.
            Dim _encryptor As ICryptoTransform = _rc2CSP.CreateEncryptor(ConvertStringToByteArray16(Key), ConvertStringToByteArray16(IV))

            'Encrypt the data.
            Dim _msEncrypt As New MemoryStream
            Dim _csEncrypt As New CryptoStream(_msEncrypt, _encryptor, CryptoStreamMode.Write)

            'Convert the data to a byte array.
            _toEncrypt = _textConverter.GetBytes(Source)

            'Write all data to the crypto stream and flush it.
            _csEncrypt.Write(_toEncrypt, 0, _toEncrypt.Length)
            _csEncrypt.FlushFinalBlock()

            'Get encrypted array of bytes.
            _encrypted = _msEncrypt.ToArray()

            ' convert into Base64 so that the result can be used in xml
            Return System.Convert.ToBase64String(_encrypted) ', 0, encrypted.Length - 1)
        Catch ex As Exception

            Return ""
        Finally

        End Try
    End Function
End Class
