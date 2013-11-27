Public Module svcConsts

    Public Enum eServicePorts
        discovery = 8090
        log = 8091
        q = 8092
        loader = 8093
        prisql = 8094
        console = 8095
        subscriber = 8096
    End Enum

    Public Function ServiceName(ByVal svc As eServicePorts) As String
        Select Case svc
            Case eServicePorts.discovery
                Return "discovery"
            Case eServicePorts.log
                Return "log"
            Case eServicePorts.q
                Return "q"
            Case eServicePorts.loader
                Return "loader"
            Case eServicePorts.prisql
                Return "prisql"
            Case eServicePorts.console
                Return "console"
            Case eServicePorts.subscriber
                Return "subscriber"
            Case Else
                Return String.Empty
        End Select
    End Function

    Public Function ServiceFromDescriptor(ByVal Descriptor As String) As eServicePorts
        Select Case Descriptor.ToLower
            Case "discovery"
                Return eServicePorts.discovery
            Case "log"
                Return eServicePorts.log
            Case "q"
                Return eServicePorts.q
            Case "loader"
                Return eServicePorts.loader
            Case "prisql"
                Return eServicePorts.prisql
            Case "console"
                Return eServicePorts.console
            Case "subscriber"
                Return eServicePorts.subscriber

            Case Else
                Return Nothing
        End Select
    End Function

End Module
