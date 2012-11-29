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
                .Add("PROVIDER", "The database type. Can be either MSSQL (the default) or ORACLE.")
            End With
            Return ret
        End Get
    End Property

    Public Sub doStartArgs(ByVal args() As String, ByVal LogBuilder As Builder)

        LogBuilder.Append("Processing start arguments...").AppendLine()
        With My.Settings
            Dim argName As String = ""
            For Each arg As String In args
                arg.Trim()
                Select Case arg.Substring(0, 1)
                    Case "/", "-"
                        argName = arg.Substring(1, arg.Length - 1)
                        If Not Arguments.Keys.Contains(argName) Then
                            argName = ""
                            LogBuilder.AppendFormat("Invalid argument {0}[{1}].", arg.Substring(0, 1), argName)
                        End If
                    Case Else
                        Select Case argName.ToUpper
                            Case "PRIORITYDIR"
                                If Not (arg.Length = 2 And arg.Substring(1, 1) = ":") Then Throw New Exception("Invalid syntax. Not a valid Drive Letter.")
                                .PRIORITYDIR = arg
                            Case "PRIORITYUSER"
                                If Not (arg.Length > 0) Then Throw New Exception("Invalid syntax. Username not specified.")
                                .PRIORITYUSER = arg
                            Case "PRIORITYPWD"
                                If Not (arg.Length > 0) Then Throw New Exception("Invalid syntax. Password not specified.")
                                .PRIORITYPWD = arg
                            Case "PRIUNC"
                                If Not (arg.Substring(0, 2) = "\\" And UBound(arg.Split("\")) = 3) Then Throw New Exception("Invalid syntax. Not a valid UNC Share name.")
                                .PRIUNC = arg
                            Case "DATASOURCE"
                                If Not (arg.Length > 0) Then Throw New Exception("Invalid syntax. Datasource not specified.")
                                .DATASOURCE = arg
                            Case "SERVICEHOST"
                                If Not (UBound(arg.Split(".")) = 4) Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                For Each ipPart As String In arg.Split(".")
                                    If Not IsNumeric(ipPart) Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                    If Not CInt(arg) > -1 And CInt(arg) < 256 Then Throw New Exception("Invalid syntax. SERVICEHOST must be an IP address.")
                                Next
                                .SERVICEHOST = arg
                            Case "SERVICEPORT"
                                If Not IsNumeric(arg) Then Throw New Exception("Invalid syntax. SERVICEPORT must be a port number.")
                                .SERVICEPORT = arg
                            Case "PROVIDER"
                                Select Case arg.ToUpper
                                    Case "MSSQL"
                                        .PROVIDER = eProviderType.MSSQL
                                    Case "ORACLE"
                                        .PROVIDER = eProviderType.ORACLE
                                    Case Else
                                        Throw New Exception(String.Format("Invalid syntax. Unsupported provider [{0}].", arg))
                                End Select
                        End Select
                End Select
            Next
            .Save()
        End With

    End Sub

End Module
