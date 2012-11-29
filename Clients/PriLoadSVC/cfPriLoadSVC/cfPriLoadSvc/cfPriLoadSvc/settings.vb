Public Class settings

    Private _key As Microsoft.Win32.RegistryKey
    Private _host As Microsoft.Win32.RegistryKey

    Public Sub New(ByVal Key As String)
        _key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(Key)
        _host = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("Comm\Tcpip\Hosts\soapsvc")
    End Sub

    Protected Overrides Sub Finalize()
        _key.Close()
        MyBase.Finalize()
    End Sub

    Public Property LogVerbosity() As Integer
        Get
            Dim ret As Integer = _key.GetValue("LogVerbosity", -1)
            If ret = -1 Then
                _key.SetValue("LogVerbosity", 99)
                Return 99
            Else
                Return ret
            End If
        End Get
        Set(ByVal value As Integer)
            _key.SetValue("LogVerbosity", value)
        End Set
    End Property

    Public Property SoapSVC() As String

        Get
            With _host

                If IsNothing(.GetValue("ipaddr", Nothing)) Then
                    Dim str() As String = Split(InputBox("Please enter the IP address of your SOAP Server."), ".")
                    Dim b() As Byte = {CInt(str(0)), CInt(str(1)), CInt(str(2)), CInt(str(3))}
                    .SetValue("ipaddr", _
                        b, Microsoft.Win32.RegistryValueKind.Binary _
                    )
                    Dim c() As Byte = {153, 153, 153, 153, 153, 153, 153}
                    .SetValue("ExpireTime", _
                        c, Microsoft.Win32.RegistryValueKind.Binary _
                    )
                End If

                Dim a() As Byte = .GetValue("ipaddr", Nothing)
                Return String.Format("{0}.{1}.{2}.{3}", _
                    a(0).ToString, _
                    a(1).ToString, _
                    a(2).ToString, _
                    a(3).ToString _
                )

            End With
        End Get

        Set(ByVal value As String)

            With _host
                Dim str() As String = Split(value, ".")
                Dim b() As Byte = {CInt(str(0)), CInt(str(1)), CInt(str(2)), CInt(str(3))}
                .SetValue("ipaddr", _
                    b, Microsoft.Win32.RegistryValueKind.Binary _
                )
                Dim c() As Byte = {153, 153, 153, 153, 153, 153, 153}
                .SetValue("ExpireTime", _
                    c, Microsoft.Win32.RegistryValueKind.Binary _
                )
            End With

        End Set

    End Property

End Class
