Imports Loading
Imports ntEvtlog

Public MustInherit Class SyncBase

#Region "Shared variables"
    Private _Parent As oDataSet
    Public p As New Loading.DataLoad
    Public svc As Loading.plc
    Public Tables As Dictionary(Of String, Object)
    Public ws As PriWebSVC.Service
    Public sd As SerialData
    Private _Ev As ntEvtLog.evt
#End Region
#Region "Initialisation"
    Public Sub New()

    End Sub
    Public Sub SetParent(ByRef Parent As oDataSet)
        _Parent = Parent
        With _Parent
            Tables = .DataSet
            ws = .ws
            svc = .svc
            _Ev = .evt
        End With
    End Sub
#End Region
#Region "Overridable Properties"
    Public Overridable ReadOnly Property SyncDes() As String
        Get
            Return "Unnamed Syncronisation"
        End Get
    End Property
    Public Overridable ReadOnly Property FormName() As String
        Get
            Return "Unnamed Form"
        End Get
    End Property
    Public Overridable ReadOnly Property FormInherit() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region
#Region "Overridable Subs"
    Public MustOverride Sub Sync()    
    Public Overrides Function ToString() As String
        Return SyncDes
    End Function
#End Region

End Class
