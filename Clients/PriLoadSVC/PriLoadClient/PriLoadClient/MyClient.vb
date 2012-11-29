Imports ntSox

Public Class MyClient
    Inherits ntSox.ntClient

    Public Event onClientCommand(ByVal CmdStr As String, ByVal Args() As String)
    Public Event onClientConnection()
    Public Event onClientDisconnection()
    Public Event onClientConnectionFail(ByVal ex As String)

    Sub New(ByVal IP As String, ByVal Port As Integer)
        MyBase.New(IP, Port)
    End Sub
    Public Overrides Sub Command(ByVal CmdStr As String, ByVal Args() As String)
        RaiseEvent onClientCommand(CmdStr, Args)
    End Sub
    Public Overrides Sub Connection()
        RaiseEvent onClientConnection()
    End Sub
    Public Overrides Sub Disconnection()
        RaiseEvent onClientDisconnection()
    End Sub
    Public Overrides Sub ConnectionFail(ByVal ex As String)
        RaiseEvent onClientConnectionFail(ex)
    End Sub

End Class