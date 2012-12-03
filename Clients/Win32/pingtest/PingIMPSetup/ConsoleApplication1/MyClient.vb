Imports ntSoxLib

Public Class MyClient
    Inherits ntClient

    Sub New(ByVal IP As String, ByVal Port As Integer)
        MyBase.New(IP, Port)
    End Sub
    Public Overrides Sub Command(ByVal Args() As String)
        MyBase.Command(Args)
    End Sub
    Public Overrides Sub Connection()
        MyBase.Connection()
    End Sub
    Public Overrides Sub ConnectionFail(ByVal ex As String)
        MyBase.ConnectionFail(ex)
    End Sub

End Class