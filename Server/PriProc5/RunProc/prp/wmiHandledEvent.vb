Imports PriPROC.wmiQuery

Public Class HandledWMIEvent

    Public Sub New(ByVal Name As tWMIProcess, ByVal EventState As tWMIEventState, ByVal ParentProcessID As Integer, ByVal ProcessID As Integer)
        _Name = Name
        _EventState = EventState
        _ParentProcessID = ParentProcessID
        _ProcessID = ProcessID
    End Sub
    Private _Name As tWMIProcess
    Public Property Name() As tWMIProcess
        Get
            Return _Name
        End Get
        Set(ByVal value As tWMIProcess)
            _Name = value
        End Set
    End Property
    Private _EventState As tWMIEventState
    Public Property EventState() As tWMIEventState
        Get
            Return _EventState
        End Get
        Set(ByVal value As tWMIEventState)
            _EventState = value
        End Set
    End Property
    Private _ParentProcessID As Integer
    Public Property ParentProcess() As Integer
        Get
            Return _ParentProcessID
        End Get
        Set(ByVal value As Integer)
            _ParentProcessID = value
        End Set
    End Property
    Private _ProcessID
    Public Property Process() As Integer
        Get
            Return _ProcessID
        End Get
        Set(ByVal value As Integer)
            _ProcessID = value
        End Set
    End Property

End Class