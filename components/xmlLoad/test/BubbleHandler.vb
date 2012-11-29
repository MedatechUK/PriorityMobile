Imports xmlLoad
Imports System.IO
Imports System.Data.SqlClient

Module BubbleHandler

    Public lEv As PriLoadEvents

    Private logBuilder As Builder

    Public Sub hNewBubble(ByVal BubbleFile As String)

        Dim MoveToFolder As tBubbleFolder = tBubbleFolder.LogFolder
        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose

        logBuilder = New Builder
        Try
            ev.LogVerbosity = ApplicationSetting("LogVerbosity")
            logBuilder.AppendFormat( _
                "New bubble data at [{0}].", _
                BubbleFile _
            ).AppendLine()

            Using xl As New Loading(ApplicationSetting("Environment"))
                With xl
                    .FromFile(BubbleFile)

                    logBuilder.AppendFormat( _
                        "Loading Bubble ID [{0}] in environment [{1}].", _
                        .BubbleID, _
                        .Environment _
                    ).AppendLine().AppendLine()

                    ' Test database connection
                    If Not Connection.State = ConnectionState.Open Then
                        logBuilder.AppendFormat( _
                            "Connection to datasource [{0}] is not open. Attempting to open...", _
                            My.Settings.DATASOURCE _
                        ).AppendLine().AppendLine()
                        Connection.Open()
                    End If

                    ' Run SQL
                    logBuilder.Append("Inserting Bubble Data:").AppendLine.AppendFormat( _
                        "{0}", _
                        .BubbleSQL.Replace("%USERNAME%", My.Settings.PRIORITYUSER) _
                    ).AppendLine()
                    Dim command As New SqlCommand(.BubbleSQL.Replace("%USERNAME%", My.Settings.PRIORITYUSER), Connection)
                    command.ExecuteNonQuery()

                    logBuilder.Append("Shell executing command:").AppendLine.AppendFormat( _
                        "{0}\BIN.95\WINRUN.exe {5}{5} {1} {2} {0}\system\prep {3} WINACTIV -P {4}", _
                        My.Settings.PRIORITYDIR, _
                        My.Settings.PRIORITYUSER, _
                        "******", _
                        .Environment, _
                        .Procedure, _
                        Chr(34) _
                    ).AppendLine()

                    ' Run Loading
                    RunCmd( _
                        String.Format( _
                            "{0}\BIN.95\WINRUN.exe {5}{5} {1} {2} {0}\system\prep {3} WINACTIV -P {4}", _
                                My.Settings.PRIORITYDIR, _
                                My.Settings.PRIORITYUSER, _
                                My.Settings.PRIORITYPWD, _
                                .Environment, _
                                .Procedure, _
                                Chr(34) _
                        ) _
                    , logBuilder)
                    logBuilder.AppendLine()

                    ' check the load table for unloaded records
                    If Not CountLoaded(.Environment, .Table, logBuilder) Then
                        et = ntEvtlog.LogEntryType.Warning
                        verb = ntEvtlog.EvtLogVerbosity.Normal
                    End If

                    logBuilder.AppendFormat( _
                            "Loaded bubble ID [{0}].", _
                            .BubbleID _
                    ).AppendLine()

                End With
            End Using

        Catch ex As Exception
            et = ntEvtlog.LogEntryType.Err
            verb = ntEvtlog.EvtLogVerbosity.Normal
            MoveToFolder = tBubbleFolder.BadMailFolder
            logBuilder.AppendFormat( _
                "Bubble load failed: {0}.", _
                ex.Message _
            ).AppendLine()

        Finally
            logBuilder.AppendFormat( _
                "Moving Bubble File to [{0}].", _
                Replace( _
                    BubbleFile, _
                    BubbleFolder(tBubbleFolder.QueueFolder), _
                    BubbleFolder(MoveToFolder) _
                    , , , CompareMethod.Text _
                ) _
            ).AppendLine()

            ' Make sure the file does not already exist
            While File.Exists( _
                Replace( _
                    BubbleFile, _
                    BubbleFolder(tBubbleFolder.QueueFolder), _
                    BubbleFolder(MoveToFolder) _
                    , , , CompareMethod.Text _
                ) _
            )

                File.Delete( _
                    Replace( _
                        BubbleFile, _
                        BubbleFolder(tBubbleFolder.QueueFolder), _
                        BubbleFolder(MoveToFolder) _
                        , , , CompareMethod.Text _
                    ) _
                )
            End While

            ' Remove from queue
            System.IO.File.Move(BubbleFile, _
                Replace( _
                    BubbleFile, _
                    BubbleFolder(tBubbleFolder.QueueFolder), _
                    BubbleFolder(MoveToFolder) _
                    , , , CompareMethod.Text _
                ) _
            )

            ev.Log(logBuilder.ToString, et, verb)
        End Try

    End Sub

    Private Sub RunCmd(ByVal cmd As String, ByRef Log As Builder)

        Dim myProcess As Process = New Process()
        With myProcess

            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
            End With

            .Start()

            ' Watch Process
            Dim tio As Integer = CInt(ApplicationSetting("LoadingTimeout", "60"))
            lEv.WatchProcess(.Id, Log, tio)
            Log.AppendFormat("Started command line with process ID {0} and a loading timeout of {1} seconds.", _
                .Id, _
                tio _
            ).AppendLine().AppendLine()

            Dim sIn As StreamWriter = .StandardInput
            With sIn                
                .AutoFlush = True
                .Write(cmd & _
                   System.Environment.NewLine)
                .Write("exit" & System.Environment.NewLine)
            End With

        End With

        With lEv
            Log.Append("Waiting for WINACTIV to complete...").AppendLine()
            Do Until .HasEnded
                Threading.Thread.Sleep(100)
            Loop

            ' Ensure command line process is closed
            If Not myProcess.HasExited Then
                Log.Append("Command line process was not closed. Killing process...").AppendLine()
                Do
                    myProcess.Kill()
                    Threading.Thread.Sleep(100)
                Loop Until myProcess.HasExited
            End If

            If .HasFailed Then Throw New Exception("The WINACTIV process failed to start.")
            If .HasElapsed Then Throw New Exception( _
                String.Format( _
                    "The loading timed out after {0} seconds.", _
                    My.Settings.LOADTIMEOUT.ToString _
                ) _
            )

        End With

    End Sub

    Private Function CountLoaded(ByVal Environment As String, ByVal Table As String, ByRef Log As Builder) As Boolean

        Dim ret As Boolean = True
        Dim ld As System.Text.StringBuilder
        Dim cmd As SqlCommand

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("SELECT T$USER FROM system.dbo.USERS where USERLOGIN='{0}'", My.Settings.PRIORITYUSER).AppendLine()
        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        cmd = New SqlCommand(ld.ToString, Connection)
        Dim tUser As Integer = cmd.ExecuteScalar()

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("select count(LOADED) from {0} where LINE > 0", Table).AppendLine()
        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        cmd = New SqlCommand(ld.ToString, Connection)
        Dim loadLines As Integer = cmd.ExecuteScalar()

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("select count(LOADED) from {0} where LINE > 0 and LOADED='Y'", Table).AppendLine()
        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        cmd = New SqlCommand(ld.ToString, Connection)
        Dim LoadedLines As Integer = cmd.ExecuteScalar()

        Log.AppendFormat("Loaded {0} of {1} records.", LoadedLines.ToString, loadLines.ToString).AppendLine()

        If loadLines > LoadedLines Then ' Unloaded lines

            ld = New System.Text.StringBuilder
            ld.AppendFormat("Use [{0}];", Environment).AppendLine()
            ld.AppendFormat("select count(*) from sys.tables where name ='{0}_UL'", Table).AppendLine()
            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
            cmd = New SqlCommand(ld.ToString, Connection)
            If cmd.ExecuteScalar() = 1 Then
                Log.AppendFormat("Found {0}_UL (unloaded) table.", Table).AppendLine()
                Log.AppendFormat("Copying data from {0} to {0}_UL table.", Table).AppendLine()
                ld = New System.Text.StringBuilder
                ld.AppendFormat("Use [{0}];", Environment).AppendLine()
                ld.AppendFormat("declare @LN int;", Table).AppendLine()
                ld.AppendFormat("set @LN = (select MAX(LINE) from {0}_UL);", Table).AppendLine()
                ld.AppendFormat("update {0} set LINE = (LINE + @LN) where LINE > 0;", Table).AppendLine()
                ld.AppendFormat("update ERRMSGS set LINE = (LINE + @LN) where T$USER = {0};", tUser.ToString).AppendLine()
                ld.AppendFormat("insert into {0}_UL select * from {0};", Table).AppendLine()

                If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                    Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
                cmd = New SqlCommand(ld.ToString, Connection)
                cmd.ExecuteNonQuery()
            End If

            ' Get the 
            ld = New System.Text.StringBuilder
            ld.AppendFormat("Use [{0}];", Environment).AppendLine()
            ld.AppendFormat("select {0}.LINE, isnull(ERRMSGS.MESSAGE,'was not loaded.')", Table).AppendLine()
            ld.AppendFormat("from {0} left outer join ERRMSGS on {0}.LINE = ERRMSGS.LINE", Table).AppendLine()
            ld.AppendFormat("where {0}.LINE > 0 and LOADED <> 'Y'", Table).AppendLine()
            ld.AppendFormat("and (isnull(ERRMSGS.T$USER,{0}) = {0})", tUser.ToString).AppendLine()
            ld.AppendFormat("and (isnull(ERRMSGS.TYPE,'i') = 'i')", tUser.ToString).AppendLine()

            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
            cmd = New SqlCommand(ld.ToString, Connection)
            Dim RD As SqlDataReader = cmd.ExecuteReader()
            While RD.Read
                Log.AppendFormat("Line {0} {1}", RD(0), RD(1)).AppendLine()
            End While
            RD.Close()

        Else ' All Lines Loaded
            ld = New System.Text.StringBuilder
            ld.AppendFormat("Use [{0}];", Environment).AppendLine()
            ld.AppendFormat("select count(*) from sys.tables where name ='{0}_LD'", Table).AppendLine()
            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
            cmd = New SqlCommand(ld.ToString, Connection)

            If cmd.ExecuteScalar() = 1 Then
                Log.AppendFormat("Found {0}_LD (loaded) table.", Table).AppendLine()
                Log.AppendFormat("Copying data from {0} to {0}_LD table.", Table).AppendLine()
                ld = New System.Text.StringBuilder
                ld.AppendFormat("Use [{0}];", Environment).AppendLine()
                ld.AppendFormat("declare @LN int;", Table).AppendLine()
                ld.AppendFormat("set @LN = (select MAX(LINE) from {0}_LD);", Table).AppendLine()
                ld.AppendFormat("update {0} set LINE = (LINE + @LN) where LINE > 0;", Table).AppendLine()
                ld.AppendFormat("update ERRMSGS set LINE = (LINE + @LN) where T$USER = {0};", tUser.ToString).AppendLine()
                ld.AppendFormat("insert into {0}_LD select * from {0};", Table).AppendLine()
                If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                    Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
                cmd = New SqlCommand(ld.ToString, Connection)
                cmd.ExecuteNonQuery()

            End If

        End If

        Return Not (loadLines > LoadedLines)

    End Function

End Module
