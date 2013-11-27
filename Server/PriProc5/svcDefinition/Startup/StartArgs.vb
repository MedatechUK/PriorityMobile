Imports System.Runtime.InteropServices
Imports System.Reflection

Public Enum eRunMode
    Interactive
    Service
End Enum

Public Class StartArgs

    <DllImport("kernel32.dll")> _
    Public Shared Function GetConsoleWindow() As IntPtr
    End Function

    Private _Caller As Assembly
    Public ReadOnly Property CallingAssembly() As Assembly
        Get
            Return _Caller
        End Get
    End Property

    Private _RunMode As eRunMode
    Public ReadOnly Property RunMode() As eRunMode
        Get
            Return _RunMode
        End Get
    End Property

    Private _ServiceType As String
    Public ReadOnly Property ServiceType() As String
        Get
            Return _ServiceType
        End Get
    End Property

    Private _StartLog As msgLogRequest
    Public Property StartLog() As msgLogRequest
        Get
            Return _StartLog
        End Get
        Set(ByVal value As msgLogRequest)
            _StartLog = value
        End Set
    End Property

    Public Sub New(ByVal ServiceType As String, ByRef ThisAssambly As Assembly, ByVal Args() As String)

        _Caller = ThisAssambly
        _ServiceType = ServiceType
        _StartLog = New msgLogRequest(_ServiceType, EvtLogSource.SYSTEM)

        Select Case GetConsoleWindow() <> IntPtr.Zero
            Case True
                _RunMode = eRunMode.Interactive
            Case Else
                _RunMode = eRunMode.Service
        End Select

    End Sub

End Class
