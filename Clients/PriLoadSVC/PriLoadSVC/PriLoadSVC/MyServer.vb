Public Class MyServer
    Inherits ntSox.ntServer

    Public Event OnConnect(ByVal ConnectionID As String)
    Public Event OnCommand(ByVal ConnectionID As String, ByVal CmdStr As String, ByVal Args() As String)

    Sub New(ByVal IP As String, ByVal Port As Integer)
        MyBase.New(IP, Port)
    End Sub

    Public Overrides Sub Connect(ByVal ConnectionID As String)
        RaiseEvent OnConnect(ConnectionID)
    End Sub

    Public Overrides Sub Command(ByVal ConnectionID As String, ByVal CmdStr As String, ByVal Args() As String)
        RaiseEvent OnCommand(ConnectionID, CmdStr, Args)
    End Sub

End Class
