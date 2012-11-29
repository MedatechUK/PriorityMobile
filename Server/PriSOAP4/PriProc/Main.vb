Imports xmlLoad
Imports System.IO
Imports System.Threading

Module Main


    Sub Main()

        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        Dim StartMessage As String = Sysinfo        

        ' System Start-up
        LogBuilder = New Builder()
        Try

            ' Configure logging options
            ev = New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, ntEvtlog.EvtLogVerbosity.Arcane, AppName)
            ev.RegisterLog(AppName)                                  

            ' Set default user info
            With My.Settings
                If .PRIORITYUSER.Length = 0 Then .PRIORITYUSER = "tabula"
                If .PRIORITYPWD.Length = 0 Then .PRIORITYPWD = "Tabula!"
                .Save()
            End With

            ' Connect to website
            Dim testConnection As String = iisFolder
            LogBuilder.AppendFormat("Connected to IIS folder at [{0}]", iisFolder).AppendLine()

            ' Set web.config values
            LogBuilder.AppendFormat("Writing system variables to [{0}web.config]", iisFolder).AppendLine()
            ApplicationSetting("start") = Now().ToString
            ApplicationSetting("sysinfo") = Sysinfo
            ApplicationSetting("appName") = AppName

            Try
                ' Database/UNC Connection tests
                With My.Settings
                    Connection = NewConnection(.DATASOURCE, .PRIORITYUSER, .PRIORITYPWD)
                    If .DATASOURCE.Length = 0 Then Throw New Exception("No datasource specified.")
                    Dim dr As NetworkDrive = GetNetworkDrive(.PRIUNC)
                    If .PRIUNC.Length = 0 Then Throw New Exception("No UNC Fileshare specified.")
                    If IsNothing(dr) Then Throw New Exception(String.Format("Priority Fileshare [{0}] is unmapped.", .PRIUNC))
                    If Not String.Compare(dr.DriveLetter, .PRIORITYDIR, True) = 0 Then Throw New Exception(String.Format("Drive [{0}] is not mapped to [{1}].", .PRIORITYDIR, .PRIUNC))
                    If Not dr.info.IsReady Then Throw New Exception(String.Format("Priority mapped drive [{0}] is not ready.", dr.DriveLetter))
                    ' Attempt connection to database
                    LogBuilder.Append("Initialisation checks OK.").AppendLine()
                    LogBuilder.AppendFormat("Connecting to database [{0}]...", .DATASOURCE).AppendLine()
                    Connection.Open()
                    LogBuilder.AppendFormat("Connection to database [{0}]...OK!", .DATASOURCE).AppendLine()
                End With
            Catch ConnectionException As Exception
                LogBuilder.Append("Initialisation exception encountered:").AppendLine()
                LogBuilder.AppendFormat("{0}", ConnectionException.Message).AppendLine()
            End Try

            ' Verify Connection            
            If Not Connection.State = ConnectionState.Open Then Configure(LogBuilder)

            ' Watch the sites web.config for changes            
            LogBuilder.AppendFormat( _
                    "Starting filewatcher on [{0}web.config]...", _
                    iisFolder).AppendLine()
            BeginWatchWebConfig()

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

        Catch EX As Exception
            et = ntEvtlog.LogEntryType.Err
            LogBuilder.AppendFormat( _
                    "Fatal Exception during initialisation. {0}", _
                    EX.Message _
            ).AppendLine()

        Finally

            Try
                ' Start the Admin Console
                LogBuilder.AppendFormat("Starting [{0}]...", StartMessage).AppendLine()
                svr.StartSvc()
            Catch SvcStartException As Exception
                LogBuilder.Append("Admin console failed to initialise.").AppendLine()
                LogBuilder.AppendFormat("{0}", SvcStartException.Message).AppendLine()
            End Try

            ev.Log( _
                LogBuilder.ToString(), _
                et, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )

        End Try

        If et = ntEvtlog.LogEntryType.SuccessAudit Then
            TestLoading()
            Do Until Console.Read
                Threading.Thread.Sleep(1)
            Loop
        End If

    End Sub

    Private Sub TestLoading()

        ' ------ Test Loading ------
        Using xl As New Loading
            With xl
                Try
                    .Table = "ZSFDC_TABLE"
                    .Procedure = "ZSFDC_TEST"
                    .Environment = "wl"

                    .AddColumn(1) = New LoadColumn("USERNAME", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("WARHS", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("BIN", tColumnType.typeCHAR)
                    .AddColumn(1) = New LoadColumn("CURDATE", tColumnType.typeDATE)
                    .AddColumn(2) = New LoadColumn("PART", tColumnType.typeCHAR)
                    .AddColumn(2) = New LoadColumn("STATUS", tColumnType.typeCHAR)
                    .AddColumn(2) = New LoadColumn("CQUANT", tColumnType.typeINT)

                    .AddRecordType(1) = New LoadRow("user", "Main", "0", Now.ToString)
                    .AddRecordType(2) = New LoadRow("PART123", "Goods", "1")
                    .AddRecordType(2) = New LoadRow("PART321", "Goods", "1")

                    Dim exp As New Exception
                    If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp
                    'If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp
                    'If Not .Post("http://redknave:8080/loadHandler.ashx", exp) Then Throw exp

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End With
        End Using
    End Sub

End Module
