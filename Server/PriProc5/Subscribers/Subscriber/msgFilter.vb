Public Class msgFilter

    Private _Verbosity As EvtLogVerbosity
    Private _SystemMSG As Boolean
    Private _AppMSG As Boolean

    Public Sub New( _
        Optional ByVal Verbosity As EvtLogVerbosity = EvtLogVerbosity.Arcane, _
        Optional ByVal SystemMSG As Boolean = True, _
        Optional ByVal AppMSG As Boolean = True _
    )

        _Verbosity = Verbosity
        _SystemMSG = SystemMSG
        _AppMSG = AppMSG

    End Sub

    Public Function Match(ByRef msg As msgLogRequest) As Boolean
        With msg
            Select Case .LogSource
                Case EvtLogSource.APPLICATION
                    If Not _AppMSG Then Return False
                Case Else
                    If Not _SystemMSG Then Return False
            End Select
            If msg.Verbosity > _Verbosity Then Return False
            Return True
        End With
    End Function

End Class
