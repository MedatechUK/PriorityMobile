Module AutoConfig

    Sub Configure(ByRef LogBuilder As Builder)

        With My.Settings

            LogBuilder.Append("Beginning Auto Configuration ...").AppendLine()
            Dim Instance As ServerInstance
            If Connection.Provider = eProviderType.MSSQL Then
                ' Locate the priority database
                Dim FindInstanceException As New Exception
                instance = PriorityInstances(FindInstanceException, LogBuilder)
                If Not IsNothing(FindInstanceException) Then Throw FindInstanceException

                .DATASOURCE = Instance.ServerInstance
                .PRIUNC = Instance.PriorityUNC
            Else
                LogBuilder.Append("Provider is not MSSQL.").AppendLine()
                LogBuilder.Append("Continuing configuration without SQL server search ...").AppendLine()
            End If

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

            .Save()

            ' Attempt to open the connection
            LogBuilder.AppendFormat("Connection to database [{0}]...", .DATASOURCE)
            Select Case Connection.Provider
                Case eProviderType.MSSQL
                    Connection = New GenericConnection(.PROVIDER, Instance.ServerInstance, .PRIORITYUSER, .PRIORITYPWD)
                Case eProviderType.ORACLE
                    Connection = New GenericConnection(.PROVIDER, .DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
            End Select            

            Connection.Open()

            LogBuilder.Append("Connected. Saving connection string...").AppendLine()
            ApplicationSetting("DSN") = Connection.ConnectionString

            LogBuilder.Append("Auto configuration sucsessful.").AppendLine()

            ' Save settings
            .Save()

            If Not WatchingConfig Then
                ' Watch the sites web.config for changes            
                LogBuilder.AppendFormat( _
                        "Starting filewatcher on [{0}web.config]...", _
                        iisFolder).AppendLine()
                BeginWatchWebConfig()
            End If

            If IsNothing(lEv) Then
                ' Initialise the Bubble Queue
                LogBuilder.AppendFormat( _
                        "Starting Bubble queue at [{0}{1}\]...", _
                        iisFolder, _
                        BubbleFolder(tBubbleFolder.QueueFolder) _
                ).AppendLine()

                lEv = New PriLoadEvents( _
                    New System.IO.DirectoryInfo( _
                        String.Format( _
                                "{0}{1}\", _
                                iisFolder, _
                                BubbleFolder(tBubbleFolder.QueueFolder) _
                            ) _
                        ) _
                    )
                AddHandler lEv.NewBubble, AddressOf hNewBubble
            End If

        End With

    End Sub

End Module
