Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.q)
        End Get
    End Property

End Module
