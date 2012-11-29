Public Class LogHour

    Private _events As New Dictionary(Of Integer, List(Of LogEvent))
    Public Property Events() As Dictionary(Of Integer, List(Of LogEvent))
        Get
            Return _events
        End Get
        Set(ByVal value As Dictionary(Of Integer, List(Of LogEvent)))
            _events = value
        End Set
    End Property

End Class

Public Class LogEvent

    Private _edate As Date
    Public Property edate() As Date
        Get
            Return _edate
        End Get
        Set(ByVal value As Date)
            _edate = value
        End Set
    End Property

    Private _time As Date
    Public Property time() As Date
        Get
            Return _time
        End Get
        Set(ByVal value As Date)
            _time = value
        End Set
    End Property

    Private _sip As String
    Public Property sip() As String
        Get
            Return _sip
        End Get
        Set(ByVal value As String)
            _sip = value
        End Set
    End Property

    Private _csmethod As String
    Public Property csmethod() As String
        Get
            Return _csmethod
        End Get
        Set(ByVal value As String)
            _csmethod = value
        End Set
    End Property

    Private _csuristem As String
    Public Property csuristem() As String
        Get
            Return _csuristem
        End Get
        Set(ByVal value As String)
            _csuristem = value
        End Set
    End Property

    Private _csuriquery As String
    Public Property csuriquery() As String
        Get
            Return _csuriquery
        End Get
        Set(ByVal value As String)
            _csuriquery = value
        End Set
    End Property

    Private _sport As String
    Public Property sport() As String
        Get
            Return _sport
        End Get
        Set(ByVal value As String)
            _sport = value
        End Set
    End Property

    Private _csusername As String
    Public Property csusername() As String
        Get
            Return _csusername
        End Get
        Set(ByVal value As String)
            _csusername = value
        End Set
    End Property

    Private _cip As String
    Public Property cip() As String
        Get
            Return _cip
        End Get
        Set(ByVal value As String)
            _cip = value
        End Set
    End Property

    Private _csUserAgent As String
    Public Property csUserAgent() As String
        Get
            Return _csUserAgent
        End Get
        Set(ByVal value As String)
            _csUserAgent = value
        End Set
    End Property

    Private _scstatus As String
    Public Property scstatus() As String
        Get
            Return _scstatus
        End Get
        Set(ByVal value As String)
            _scstatus = value
        End Set
    End Property

    Private _scsubstatus As String
    Public Property scsubstatus() As String
        Get
            Return _scsubstatus
        End Get
        Set(ByVal value As String)
            value = _scsubstatus
        End Set
    End Property

    Private _scwin32status As String
    Public Property scwin32status() As String
        Get
            Return _scwin32status
        End Get
        Set(ByVal value As String)
            _scwin32status = value
        End Set
    End Property

    Private _timetaken As Integer
    Public Property timetaken() As Integer
        Get
            Return _timetaken
        End Get
        Set(ByVal value As Integer)
            _timetaken = value
        End Set
    End Property

End Class
