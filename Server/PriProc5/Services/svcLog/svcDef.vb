Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.log)
        End Get
    End Property

End Module
