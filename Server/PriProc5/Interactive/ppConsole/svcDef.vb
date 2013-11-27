Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.discovery)
        End Get
    End Property

End Module
