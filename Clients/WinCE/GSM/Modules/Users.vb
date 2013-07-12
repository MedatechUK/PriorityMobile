Public Class Users
    Private Name As String
    Private start As String
    Private endt As String
    Private operation As String
    Public Property nam() As String
        Get
            Return Name
        End Get
        Set(ByVal value As String)
            Name = value
        End Set
    End Property
    Public Property stime() As String
        Get
            Return start
        End Get
        Set(ByVal value As String)
            start = value
        End Set
    End Property
    Public Property etime() As String
        Get
            Return endt
        End Get
        Set(ByVal value As String)
            endt = value
        End Set
    End Property
    Public Property op() As String
        Get
            Return operation
        End Get
        Set(ByVal value As String)
            operation = value
        End Set
    End Property
    Public Sub New(ByVal n As String, ByVal s As String, ByVal e As String, ByVal o As String)
        nam = n
        stime = s
        etime = e
        op = o
    End Sub
End Class
