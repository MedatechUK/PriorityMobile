Module StartArgs

    Public ReadOnly Property Arguments() As Dictionary(Of String, String)
        Get
            Dim ret As New Dictionary(Of String, String)
            With ret
                .Add("PRIORITYDIR", "The Priority mapped drive e.g. [p:].")
                .Add("PRIORITYUSER", "The Priority user which performs loadings.")
                .Add("PRIORITYPWD", "The password for the Priority user.")
                .Add("PRIUNC", "The UNC share of the Priority folder e.g \\{server}\{share}.")
                .Add("DATASOURCE", "The hostname of the Priority MSSQL Server.")
                .Add("SERVICEHOST", "The service ip of the admin console.")
                .Add("SERVICEPORT", "The service port of the admin console.")
            End With
            Return ret
        End Get
    End Property

    Public Sub doStartArgs(ByVal args() As String)

        With My.Settings
            Dim argName As String = ""
            For Each arg As String In args
                arg.Trim()
                Select Case arg.Substring(0, 1)
                    Case "/", "-"
                        argName = arg.Substring(1, arg.Length - 1)
                        If Not Arguments.Keys.Contains(argName) Then
                            argName = ""
                        End If
                    Case Else
                        Select Case argName.ToUpper
                            Case "IP"
                                If Not (UBound(arg.Split(".")) = 4) Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                For Each ipPart As String In arg.Split(".")
                                    If Not IsNumeric(ipPart) Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                    If Not CInt(arg) > -1 And CInt(arg) < 256 Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                Next
                                .ip = arg
                            Case "PORT"
                                If Not IsNumeric(arg) Then Throw New Exception("Invalid syntax. SERVICEPORT must be a port number.")
                                .port = arg
                        End Select
                End Select
            Next
            .Save()
        End With

    End Sub

End Module
