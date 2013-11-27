Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.prisql)
        End Get
    End Property

End Module
