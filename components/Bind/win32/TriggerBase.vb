Imports Loading
Imports ntEvtlog

Public MustInherit Class TriggerBase

    Private _Parent As oDataSet
    Public p As New Loading.DataLoad
    Public svc As Loading.plc
    Public Tables As Dictionary(Of String, Object)
    Public ws As PriWebSVC.Service
    Public sd As SerialData
    Private _Ev As ntEvtlog.evt

    Public Event GetInterfaceValue(ByVal Name As String, ByRef Value As String)
    Public Event SetInterfaceValue(ByVal Name As String, ByVal Value As String)

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
        AddHandler GetInterfaceValue, AddressOf Parent.hGetInterfaceValue
        AddHandler SetInterfaceValue, AddressOf Parent.hSetInterfaceValue
    End Sub

    Public ReadOnly Property UseEnvironment() As String
        Get
            Return _Parent.UseEnvironment
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return TriggerType.Name
    End Function

    Public Property InterfaceValue(ByVal Name As String) As String
        Get
            Dim value As String = Nothing
            RaiseEvent GetInterfaceValue(Name, value)
            Return value
        End Get
        Set(ByVal value As String)
            RaiseEvent SetInterfaceValue(Name, value)
        End Set
    End Property

    Public MustOverride Function TriggerType() As System.Type

    Public Overridable Sub PREINSERT(ByRef NewObject As Object, ByRef Cancel As Boolean)

    End Sub

    Public Overridable Sub POSTINSERT(ByRef NewObject As Object)

    End Sub

    Public Overridable Sub PREUPDATE(ByRef NewObject As Object, ByRef OldObject As Object, ByRef Cancel As Boolean)

    End Sub

    Public Overridable Sub POSTUPDATE(ByRef NewObject As Object, ByRef OldObject As Object)

    End Sub

    Public Overridable Sub PREDELETE(ByRef OldObject As Object, ByRef Cancel As Boolean)

    End Sub

    Public Overridable Sub POSTDELETE(ByRef OldObject As Object)

    End Sub

    Public Sub Log(ByVal Entry As String, ByVal EventType As ntEvtlog.LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            If Not IsNothing(_Ev) Then
                _Ev.Log( _
                    Entry, _
                    EventType, _
                    Verbosity _
                  )
            End If
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _Ev.LogName, _Ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

End Class
