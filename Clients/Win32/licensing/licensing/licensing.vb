Imports System.Windows.Forms

Friend Class EncChar
    Private _current As Integer = 0
    Private _Password As Char()

    Public Sub New(ByVal Password As String)
        If Password.Length < 3 Then Throw New Exception("Password must be more that two characters.")
        _Password = Password.ToCharArray()
    End Sub

    Public Function NextChar() As Int16
        Dim ret As Integer = 0
        Select Case Convert.ToInt16(_Password(_current)) Mod 2
            Case 0
                ret += -4
            Case Else
                ret += 3
        End Select
        Select Case Convert.ToInt16(_Password(_current)) Mod 3
            Case 0
                ret += -2
            Case Else
                ret += 1
        End Select
        _current += 1
        If Not (_current < _Password.Count) Then _current = 0
        Return ret
    End Function

    Public Sub Reset()
        _current = 0
    End Sub

End Class


Friend Class KeyEncryption
    Private _SharedKey As EncChar

    Public Sub New(ByVal SharedKey As String)
        _SharedKey = New EncChar(SharedKey)
    End Sub

    Public Sub Encrypt(ByRef StrVal As String)
        _SharedKey.Reset()
        Dim b As Integer = 11 - StrVal.Length
        StrVal = Chr(b + 110) + StrVal
        Dim rnd As New Random()
        For i As Integer = 1 To 12 - StrVal.Length
            Dim x As Char = Chr(rnd.Next(33, 126))
            StrVal = StrVal.PadRight(StrVal.Length + 1, x)
        Next
        Dim ch() As Char = StrVal.ToCharArray
        For i As Int32 = 0 To ch.Length - 1
            ch(i) = Convert.ToChar(Convert.ToInt16(ch(i)) + _SharedKey.NextChar)
        Next
        StrVal = ch
        StrVal = Left(StrVal, 12)
    End Sub

    Public Sub Decrypt(ByRef StrVal As String)
        _SharedKey.Reset()
        Dim ch() As Char = StrVal.ToCharArray()
        For i As Int32 = 0 To ch.Length - 1
            ch(i) = Convert.ToChar(Convert.ToInt16(ch(i)) - _SharedKey.NextChar)
        Next
        StrVal = ch
        Dim b As Integer = Convert.ToInt16(ch(0)) - 110
        StrVal = Left(StrVal, 12 - b)
    End Sub

End Class


Public Class License
    Private _isValid = False
    Public Property isValid()
        Get
            isValid = _isValid
        End Get
        Set(ByVal value)
            _isValid = value
        End Set
    End Property


    Public Sub New(ByVal source As String, ByVal licenseKey As String)
        If licenseKey.Length < 12 Then
            isValid = False
            MsgBox("License key is invalid. Please contact support@emerge-it.co.uk")
            Exit Sub
        End If
        Dim lkey As New KeyEncryption(System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()(0).GetPhysicalAddress.ToString)
        Dim check As String = source.Split(",")(0)
        lkey.Decrypt(licenseKey)
        Dim offset As Integer = Convert.ToInt16(licenseKey(0)) - 110
        Dim comparison As String = Right(licenseKey, licenseKey.Length - 1)
        If offset < 0 Then
            check = Left(check, check.Length + offset)
        End If
        If check = comparison Then
            isValid = True
        Else
            isValid = False
            MsgBox("License key is invalid. Please contact support@emerge-it.co.uk")
        End If
    End Sub

End Class

Public Class generator
    Private _license
    Public Property license()
        Get
            license = _license
        End Get
        Set(ByVal value)
            _license = value
        End Set
    End Property

    Public Sub New(ByVal assembly As String, ByVal macAddress As String)
        Dim lkey As New KeyEncryption(macAddress)
        lkey.Encrypt(assembly)
        license = assembly
    End Sub
End Class

