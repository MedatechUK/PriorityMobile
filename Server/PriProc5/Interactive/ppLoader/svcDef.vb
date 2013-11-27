Imports System.Reflection

Module svcDef

    Public ReadOnly Property svcType() As String
        Get
            Return ServiceName(eServicePorts.loader)
        End Get
    End Property

End Module