Imports System.Threading

Public Class trm : Inherits consoleMustInherit

    Private thdSearchPriSQL As Thread

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef sArg As StartArgs)
        Instantiate(sArg)
    End Sub

    Public Overrides Sub onTerminate()
        Using term As New msgLogRequest(ServiceType, EvtLogSource.SYSTEM, EvtLogVerbosity.Normal, LogEntryType.Warning)
            With term.LogData
                .AppendFormat("Stopping [{0}] service on {1}.", ServiceType, NetBiosName).AppendLine()
            End With
            LogEvent(term)
        End Using

    End Sub

#End Region

    Public Overrides Sub Start(ByRef sArg As StartArgs)

        sArg.StartLog.LogData.AppendFormat("Service [{0}] started on {1}.", ServiceType, NetBiosName).AppendLine()

    End Sub

    Public Overrides Sub NewTCPMessage(ByRef Request As svcMsgXML, ByRef Response() As Byte)

        With Request
            Select Case .Verb
                Case eVerb.Request
                    Select Case .msgType
                        Case "config"                            

                    End Select

            End Select
        End With

    End Sub

    Public Overrides Function BeginConfig(ByRef Request As svcMsgXML) As ServiceMessage

    End Function

End Class
