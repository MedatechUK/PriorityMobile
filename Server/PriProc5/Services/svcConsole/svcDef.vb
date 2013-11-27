Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.console)
        End Get
    End Property

End Module
