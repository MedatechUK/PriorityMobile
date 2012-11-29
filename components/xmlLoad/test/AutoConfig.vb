Module AutoConfig

    Sub Configure(ByRef LogBuilder As Builder)

        With My.Settings

            LogBuilder.Append("Beginning Auto Configuration ...").AppendLine()

            ' Locate the priority database
            Dim FindInstanceException As New Exception
            Dim Instance As ServerInstance = PriorityInstances(FindInstanceException, LogBuilder)
            If Not IsNothing(FindInstanceException) Then Throw FindInstanceException

            .DATASOURCE = Instance.ServerInstance
            .PRIUNC = Instance.PriorityUNC

            If .PRIORITYDIR.Length = 0 Then
                LogBuilder.AppendFormat("No Priority mapped drive exists. Mapping using [{0}].", _
                                        UnusedDriveLetter.First()).AppendLine()
                MapDriveLetter(UnusedDriveLetter.First(), .PRIUNC, LogBuilder)
                .PRIORITYDIR = UnusedDriveLetter.First()
            Else
                Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                If IsNothing(dr) Then
                    LogBuilder.AppendFormat("[{0}] share is unmapped. Mapping to [{1}].", _
                        .PRIUNC, .PRIORITYDIR).AppendLine()
                    MapDriveLetter(.PRIORITYDIR, .PRIUNC, LogBuilder)
                    dr = GetNetworkDrive(.PRIUNC)
                End If
                If Not String.Compare(dr.DriveLetter, .PRIORITYDIR) = 0 Then
                    .PRIORITYDIR = dr.DriveLetter
                End If
                If Not dr.info.IsReady Then
                    LogBuilder.AppendFormat("[{0}] mapped drive not ready. Re-mapping.", _
                        .PRIORITYDIR).AppendLine()
                    MapDriveLetter(.PRIORITYDIR, .PRIUNC, LogBuilder)
                End If
            End If

            ' Attempt to open the connection
            Connection = NewConnection(Instance.ServerInstance, .PRIORITYUSER, .PRIORITYPWD)
            LogBuilder.AppendFormat("Connection to database [{0}]...OK!", .DATASOURCE).AppendLine()
            Connection.Open()

            LogBuilder.Append("Connected. Saving connection string...").AppendLine()
            ApplicationSetting("DSN") = NewConnectionString(.DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)

            LogBuilder.Append("Auto configuration sucsessful.").AppendLine()

            ' Save settings
            .Save()

        End With

    End Sub

End Module
