Imports priority
Imports System.IO
Imports System.Data.SqlClient
Imports System.Security.AccessControl

Module BubbleHandler

    Friend Class tCountLoad
        Public Sub New(ByVal loadLines As Integer, ByVal LoadedLines As Integer, ByVal ErrLines As Integer)
            With Me
                .LoadedLines = LoadedLines
                .loadLines = loadLines
                .ErrLines = ErrLines
            End With
        End Sub
        Public loadLines As Integer
        Public LoadedLines As Integer
        Public ErrLines As Integer
    End Class

    Friend Enum tCountState
        UnLoaded = 0
        LoadedWithErrors = 1
        LoadedNoErrors = 2
        MaxRetry = 3
    End Enum

    Public lEv As PriLoadEvents
    Private logBuilder As Builder
    Private Reloads As New Dictionary(Of String, Integer)

    Public Sub hNewBubble(ByVal BubbleFile As String)

        Dim MoveToFolder As tBubbleFolder = tBubbleFolder.LogFolder
        Dim et As ntEvtlog.LogEntryType = ntEvtlog.LogEntryType.SuccessAudit
        Dim verb As ntEvtlog.EvtLogVerbosity = ntEvtlog.EvtLogVerbosity.Verbose
        Dim dest As New SaveDestination(BubbleFile)

        ' Check if we have seen this bubble before
        If Reloads.ContainsKey(BubbleFile) Then
            Reloads(BubbleFile) += 1
        Else
            Reloads.Add(BubbleFile, 0)
        End If

        logBuilder = New Builder
        Try
            ev.LogVerbosity = ApplicationSetting("LogVerbosity")
            logBuilder.AppendFormat( _
                "New bubble data at [{0}]: Attempt {1}.", _
                BubbleFile, Reloads(BubbleFile) + 1 _
            ).AppendLine()

            Using xl As New Loading(ApplicationSetting("Environment"), Connection.Provider)
                With xl
                    .FromFile(BubbleFile)

                    ' Loaded the Bubble file
                    ' Set the properties of the SaveDestination object
                    With dest
                        .Environment = xl.Environment
                        .procedure = xl.Procedure
                    End With

                    logBuilder.AppendFormat( _
                        "Loading Bubble ID [{0}] in environment [{1}].", _
                        .BubbleID, _
                        .Environment _
                    ).AppendLine().AppendLine()

                    ' Check to see if loading is procedure only
                    ' i.e. no loadinging data to insert.
                    ' Si - 04/06/13
                    If .HasSQL Then

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
                        Dim command As New GenericCommand(.BubbleSQL.Replace("%USERNAME%", My.Settings.PRIORITYUSER), Connection)

                        Try
                            command.ExecuteNonQuery()

                        Catch FirstChanceException As Exception
                            logBuilder.AppendFormat( _
                                "Datasource [{0}] returned an first chance error while inserting bubble data: {1}", _
                                My.Settings.DATASOURCE, _
                                FirstChanceException.Message _
                            ).AppendLine().AppendLine()
                            Try
                                logBuilder.AppendFormat( _
                                    "Attempting to re-open datasource [{0}]...", _
                                    My.Settings.DATASOURCE _
                                ).AppendLine().AppendLine()
                                Connection.Open()
                                command.ExecuteNonQuery()
                            Catch SecondChanceException As Exception
                                Throw New Exception( _
                                    String.Format( _
                                        "Datasource [{0}] returned an second chance error while inserting bubble data: {1}", _
                                        My.Settings.DATASOURCE, _
                                        SecondChanceException.Message _
                                    ) _
                                )
                            End Try
                        End Try

                    End If ' .hasSQL

                    ' Data was inserted ok
                    logBuilder.Append("Shell executing command:").AppendLine.AppendFormat( _
                        "{0}\BIN.95\WINRUN.exe {5}{5} {1} {2} {0}\system\prep {3} WINACTIV -P {4}", _
                        My.Settings.PRIORITYDIR, _
                        My.Settings.PRIORITYUSER, _
                        "******", _
                        .Environment, _
                        .Procedure, _
                        "'" _
                    ).AppendLine()

                    Try
                        ' Run Loading
                        RunCmd( _
                                .Environment, _
                                .Procedure, _
                                logBuilder _
                            )
                        logBuilder.AppendLine()

                    Catch LoadingException As Exception
                        ' Catch the error and proceed to countloaded 
                        ' as it may mean the WMI is fuxored
                        ' Set a warning just in case
                        logBuilder.Append(LoadingException.Message).AppendLine()
                        et = ntEvtlog.LogEntryType.Warning
                        verb = ntEvtlog.EvtLogVerbosity.Normal
                    End Try

                    If .HasSQL Then

                        ' check the load table for unloaded records
                        ' Si 11/4/13: Set MoveToQ on invalid loading: 
                        '                    MoveToFolder = tBubbleFolder.QueueFolder

                        ' Get the results of the loading
                        '   loadLines = number of lines in load table
                        '   LoadedLines = line where LOADED ='Y'
                        '   ErrLines = number of errors in ERRMSG

                        Dim CL As tCountLoad
                        Select Case Connection.Provider
                            Case eProviderType.ORACLE
                                CL = plCountLoaded(.Environment, .Table, logBuilder)
                            Case Else
                                CL = CountLoaded(.Environment, .Table, logBuilder)
                        End Select

                        ' Check the resuls of the loading
                        Dim CountState As tCountState
                        With CL
                            If (.ErrLines > 0) Or (.LoadedLines > 0 And .loadLines > .LoadedLines) Then
                                CountState = tCountState.LoadedWithErrors
                            ElseIf .loadLines = .LoadedLines Then
                                CountState = tCountState.LoadedNoErrors
                            Else
                                CountState = tCountState.UnLoaded
                            End If
                        End With

                        ' Quit re-qing after 3 attempts
                        If CountState = tCountState.UnLoaded And Reloads(BubbleFile) > 1 Then CountState = tCountState.MaxRetry

                        Select Case CountState
                            Case tCountState.UnLoaded
                                logBuilder.AppendFormat( _
                                    "Bubble [{0}] failed to load on attempt {1}.", _
                                    BubbleFile, _
                                    Reloads(BubbleFile) + 1 _
                                    ).AppendLine().Append( _
                                    "Sending it to the back of the q.").AppendLine()
                                MoveToFolder = tBubbleFolder.QueueFolder
                                et = ntEvtlog.LogEntryType.FailureAudit
                                verb = ntEvtlog.EvtLogVerbosity.Verbose

                            Case tCountState.LoadedWithErrors
                                et = ntEvtlog.LogEntryType.Warning
                                verb = ntEvtlog.EvtLogVerbosity.Normal
                                Reloads.Remove(BubbleFile)

                            Case tCountState.MaxRetry
                                logBuilder.AppendFormat( _
                                    "Bubble [{0}] still didn't load after attempt {1}.", _
                                    BubbleFile, Reloads(BubbleFile) + 1 _
                                    ).AppendLine().Append("I'm giving up, and sending this to badmail." _
                                    ).AppendLine.Append("Sorry it didn't work out.").AppendLine()
                                MoveToFolder = tBubbleFolder.BadMailFolder
                                et = ntEvtlog.LogEntryType.Err
                                verb = ntEvtlog.EvtLogVerbosity.Normal
                                Reloads.Remove(BubbleFile)

                            Case tCountState.LoadedNoErrors
                                Reloads.Remove(BubbleFile)

                        End Select

                    End If

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

            Try

                ' Get the detination path, creating folders as required
                dest.MoveToFolder = MoveToFolder

                Select Case dest.MoveToFolder
                    Case tBubbleFolder.QueueFolder

                        ' Move the failed bubble to the back of the queue
                        File.SetCreationTime(BubbleFile, Now)

                    Case Else

                        Dim SaveTo As String = dest.DestinationPath(logBuilder)

                        logBuilder.AppendFormat( _
                            "Moving Bubble File to file://{0}.", _
                            SaveTo _
                        ).AppendLine()


                        ' Make sure the file does not already exist
                        Do
                            File.Delete(SaveTo)
                            Threading.Thread.Sleep(1)
                        Loop Until Not File.Exists(SaveTo)

                        ' Remove from queue
                        Dim excep As Exception = Nothing
                        If File.Exists(BubbleFile) Then
                            Do
                                Try
                                    excep = Nothing
                                    System.IO.File.Move(BubbleFile, SaveTo)
                                Catch ex As UnauthorizedAccessException
                                    logBuilder.AppendFormat( _
                                        "The caller does not have the required permission. {0}", _
                                        ex.Message _
                                    ).AppendLine()
                                Catch ex As DirectoryNotFoundException
                                    logBuilder.AppendFormat( _
                                        "The path specified in sourceFileName or destFileName is invalid, (for example, it is on an unmapped drive). {0}", _
                                        ex.Message _
                                    ).AppendLine()
                                Catch ex As NotSupportedException
                                    logBuilder.AppendFormat( _
                                        "sourceFileName or destFileName is in an invalid format. {0}", _
                                        ex.Message _
                                    ).AppendLine()
                                Catch ex As Exception
                                    excep = ex
                                    Threading.Thread.Sleep(1)
                                End Try
                            Loop Until IsNothing(excep) Or Not (File.Exists(BubbleFile))
                        End If

                End Select

            Catch ex As Exception
                et = ntEvtlog.LogEntryType.Err
                verb = ntEvtlog.EvtLogVerbosity.Normal
                logBuilder.AppendFormat( _
                    "Enexpected exception during file cleanup: {0}.", _
                    ex.Message _
                ).AppendLine()

            Finally
                ' Write logbuilder to the event log
                ev.Log(logBuilder.ToString, et, verb)

                ' Resume the queue
                lEv.RestartQ()

            End Try

        End Try

    End Sub

    Private Sub RunCmd(ByVal Environment As String, ByVal Procedure As String, ByRef Log As Builder)

        Dim tio As Integer = CInt(ApplicationSetting("LoadingTimeout", "60"))

        Dim myProcess As Process = New Process()
        With myProcess

            With .StartInfo
                .FileName = String.Format("{0}\BIN.95\WINRUN.exe", My.Settings.PRIORITYDIR)
                .Arguments = String.Format( _
                                "{5}{5} {1} {2} {0}\system\prep {3} WINACTIV -P {4}", _
                                    My.Settings.PRIORITYDIR, _
                                    My.Settings.PRIORITYUSER, _
                                    My.Settings.PRIORITYPWD, _
                                    Environment, _
                                    Procedure, _
                                    "'" _
                            )
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
            End With

            ' Watch Process
            lEv.WatchProcess(Log, tio)
            .Start()

            Log.AppendFormat("Started WINRUN with process ID {0} and a loading timeout of {1} seconds.", _
                .Id, _
                tio _
            ).AppendLine().AppendLine()

        End With

        With lEv
            Log.Append("Waiting for WINACTIV to complete...").AppendLine()
            Do Until .HasEnded
                Threading.Thread.Sleep(100)
            Loop

            ' Ensure command line process is closed
            If Not myProcess.HasExited Then
                Log.Append("WINRUN process was not closed. Killing process...").AppendLine()
                Do
                    myProcess.Kill()
                    Threading.Thread.Sleep(100)
                Loop Until myProcess.HasExited
            End If

            If .HasFailed Then Throw New Exception("The WINACTIV process failed to start.")
            If .HasElapsed Then Throw New Exception( _
                String.Format( _
                    "The loading timed out after {0} seconds.", _
                    tio) _
                )

        End With

    End Sub

#Region "Count Loaded"

    Private Function CountLoaded(ByVal Environment As String, ByVal Table As String, ByRef Log As Builder) As tCountLoad

        Dim ld As System.Text.StringBuilder
        Dim cmd As GenericCommand

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("SELECT T$USER FROM system.dbo.USERS where USERLOGIN='{0}'", My.Settings.PRIORITYUSER).AppendLine()
        Dim tUser As Integer = ExecuteAndLog(ld, Log, "Service User")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("select count(*) from {0} where LINE > 0", Table).AppendLine()
        Dim loadLines As Integer = ExecuteAndLog(ld, Log, "#Lines to be loaded")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("select count(*) from {0} where LINE > 0 and LOADED='Y'", Table).AppendLine()
        Dim LoadedLines As Integer = ExecuteAndLog(ld, Log, "#Lines where LOADED='Y'")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        ld.AppendFormat("select count(*) from ERRMSGS ", Table).AppendLine()
        ld.AppendFormat("where T$USER={0} ", tUser.ToString).AppendLine()
        ld.AppendFormat("and ERRMSGS.TYPE = 'i'", tUser.ToString).AppendLine()
        Dim ErrLines As Integer = ExecuteAndLog(ld, Log, "#Lines in ERRMSG table")

        Log.AppendFormat("Loaded {0} of {1} records.", LoadedLines.ToString, loadLines.ToString).AppendLine()

        If ErrLines > 0 Then
            Log.AppendFormat("Priority reported [{0}] loading errors: ", ErrLines.ToString).AppendLine()
            ld = New System.Text.StringBuilder
            ld.AppendFormat("Use [{0}];", Environment).AppendLine()
            ld.AppendFormat("select MESSAGE from ERRMSGS ", Table).AppendLine()
            ld.AppendFormat("where T$USER={0} ", tUser.ToString).AppendLine()
            ld.AppendFormat("and ERRMSGS.TYPE = 'i'", tUser.ToString).AppendLine()
            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
            cmd = New GenericCommand(ld.ToString, Connection)
            Dim RD As GenericDataReader = cmd.ExecuteReader()
            While RD.Read
                Log.AppendFormat("   {0}", RD.Item(0)).AppendLine()
            End While
            RD.Close()
        End If

        Return New tCountLoad(loadLines, LoadedLines, ErrLines)

        'If loadLines > LoadedLines Then ' Unloaded lines

        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        '    ld.AppendFormat("select count(*) from sys.tables where name ='{0}_UL'", Table).AppendLine()
        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)
        '    If cmd.ExecuteScalar() = 1 Then
        '        Log.AppendFormat("Found {0}_UL (unloaded) table.", Table).AppendLine()
        '        Log.AppendFormat("Copying data from {0} to {0}_UL table.", Table).AppendLine()
        '        ld = New System.Text.StringBuilder
        '        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        '        ld.AppendFormat("declare @LN int;", Table).AppendLine()
        '        ld.AppendFormat("set @LN = (select MAX(LINE) from {0}_UL);", Table).AppendLine()
        '        ld.AppendFormat("update {0} set LINE = (LINE + @LN) where LINE > 0;", Table).AppendLine()
        '        ld.AppendFormat("update ERRMSGS set LINE = (LINE + @LN) where T$USER = {0};", tUser.ToString).AppendLine()
        '        ld.AppendFormat("insert into {0}_UL select * from {0} where LINE > 0 ;", Table).AppendLine()

        '        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '        cmd = New GenericCommand(ld.ToString, Connection)
        '        cmd.ExecuteNonQuery()
        '    Else
        '        Log.AppendFormat("Unloaded table not created: {0}_UL.", Table).AppendLine()
        '    End If

        '    ' Get the 
        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        '    ld.AppendFormat("select {0}.LINE, isnull(ERRMSGS.MESSAGE,'was not loaded.')", Table).AppendLine()
        '    ld.AppendFormat("from {0} left outer join ERRMSGS on {0}.LINE = ERRMSGS.LINE", Table).AppendLine()
        '    ld.AppendFormat("where {0}.LINE > 0 and LOADED <> 'Y'", Table).AppendLine()
        '    ld.AppendFormat("and (isnull(ERRMSGS.T$USER,{0}) = {0})", tUser.ToString).AppendLine()
        '    ld.AppendFormat("and (isnull(ERRMSGS.TYPE,'i') = 'i')", tUser.ToString).AppendLine()

        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)
        '    Dim RD As GenericDataReader = cmd.ExecuteReader()
        '    While RD.Read
        '        Log.AppendFormat("Line {0} {1}", RD.Item(0), RD.Item(1)).AppendLine()
        '    End While
        '    RD.Close()

        'Else ' All Lines Loaded
        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        '    ld.AppendFormat("select count(*) from sys.tables where name ='{0}_LD'", Table).AppendLine()
        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)

        '    If cmd.ExecuteScalar() = 1 Then
        '        Log.AppendFormat("Found {0}_LD (loaded) table.", Table).AppendLine()
        '        Log.AppendFormat("Copying data from {0} to {0}_LD table.", Table).AppendLine()
        '        ld = New System.Text.StringBuilder
        '        ld.AppendFormat("Use [{0}];", Environment).AppendLine()
        '        ld.AppendFormat("declare @LN int;", Table).AppendLine()
        '        ld.AppendFormat("set @LN = (select MAX(LINE) from {0}_LD);", Table).AppendLine()
        '        ld.AppendFormat("update {0} set LINE = (LINE + @LN) where LINE > 0;", Table).AppendLine()
        '        ld.AppendFormat("update ERRMSGS set LINE = (LINE + @LN) where T$USER = {0};", tUser.ToString).AppendLine()
        '        ld.AppendFormat("insert into {0}_LD select * from {0};", Table).AppendLine()
        '        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '        cmd = New GenericCommand(ld.ToString, Connection)
        '        cmd.ExecuteNonQuery()
        '    Else
        '        Log.AppendFormat("Loaded table not created: {0}_LD.", Table).AppendLine()
        '    End If

        'End If

        'Return Not (loadLines > LoadedLines)

    End Function

    Private Function plCountLoaded(ByVal Environment As String, ByVal Table As String, ByRef Log As Builder) As tCountLoad

        Dim ld As System.Text.StringBuilder
        Dim cmd As GenericCommand

        ld = New System.Text.StringBuilder
        ld.AppendFormat("SELECT T$USER FROM USERS where USERLOGIN='{0}' or reverse(USERLOGIN)='{0}'", My.Settings.PRIORITYUSER).AppendLine()
        Dim tUser As Integer = ExecuteAndLog(ld, Log, "Service User")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("select count(LOADED) from {0}${1} where LINE > 0", Environment, Table).AppendLine()
        Dim loadLines As Integer = ExecuteAndLog(ld, Log, "#Lines to be loaded")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("select count(*) from {0}${1} where LINE > 0 and LOADED='Y'", Environment, Table).AppendLine()
        Dim LoadedLines As Integer = ExecuteAndLog(ld, Log, "#Lines where LOADED='Y'")

        ld = New System.Text.StringBuilder
        ld.AppendFormat("select count(*) from {0}$ERRMSGS ", Environment, Table).AppendLine()
        ld.AppendFormat("where T$USER={0} ", tUser.ToString).AppendLine()
        ld.AppendFormat("and TYPE = 'i'", tUser.ToString).AppendLine()
        Dim ErrLines As Integer = ExecuteAndLog(ld, Log, "#Lines in ERRMSG table")

        Log.AppendFormat("Loaded {0} of {1} records.", LoadedLines.ToString, loadLines.ToString).AppendLine()

        If ErrLines > 0 Then
            Log.AppendFormat("Priority reported [{0}] loading errors: ", ErrLines.ToString).AppendLine()
            ld = New System.Text.StringBuilder
            ld.AppendFormat("select MESSAGE from {0}$ERRMSGS ", Environment, Table).AppendLine()
            ld.AppendFormat("where T$USER={0} ", tUser.ToString).AppendLine()
            ld.AppendFormat("and TYPE = 'i'", tUser.ToString).AppendLine()
            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
            cmd = New GenericCommand(ld.ToString, Connection)
            Dim RD As GenericDataReader = cmd.ExecuteReader()
            While RD.Read
                Log.AppendFormat("   {0}", RD.Item(0)).AppendLine()
            End While
            RD.Close()
            Log.AppendLine()
        End If

        Return New tCountLoad(loadLines, LoadedLines, ErrLines)

        'Log.AppendFormat("Loaded {0} of {1} records.", LoadedLines.ToString, loadLines.ToString).AppendLine()

        'If loadLines > LoadedLines Then ' Unloaded lines

        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("SELECT count(*) FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'TABLE' AND upper(OBJECT_NAME) = upper('{0}${1}_UL')", Environment, Table).AppendLine()
        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)
        '    If cmd.ExecuteScalar() = 1 Then
        '        Log.AppendFormat("Found {0}_UL (unloaded) table.", Table).AppendLine()
        '        Log.AppendFormat("Copying data from {0} to {0}_UL table.", Table).AppendLine()
        '        ld = New System.Text.StringBuilder
        '        ld.AppendFormat("DECLARE LN INTEGER ;", "").AppendLine()
        '        ld.AppendFormat("BEGIN", "").AppendLine()
        '        ld.AppendFormat("select MAX(LINE) into LN from {0}${1}_UL;", Environment, Table).AppendLine()
        '        ld.AppendFormat("update {0}${1} set LINE = (LINE + LN) where LINE > 0;", Environment, Table).AppendLine()
        '        ld.AppendFormat("update {0}$ERRMSGS set LINE = (LINE + LN) where T$USER = {1};", Environment, tUser.ToString).AppendLine()
        '        ld.AppendFormat("insert into {0}${1}_UL select * from {0}${1} where LINE > 0;", Environment, Table).AppendLine()
        '        ld.AppendFormat("END;", "").AppendLine()

        '        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '        cmd = New GenericCommand(ld.ToString, Connection)
        '        cmd.ExecuteNonQuery()
        '    Else
        '        Log.AppendFormat("Unloaded table not created: {0}_UL .", Table).AppendLine()
        '    End If

        '    ' Get the 
        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("select {0}${1}.LINE, NVL({0}$ERRMSGS.MESSAGE,'was not loaded.')", Environment, Table).AppendLine()
        '    ld.AppendFormat("from {0}${1} left outer join {0}$ERRMSGS on {0}${1}.LINE = {0}$ERRMSGS.LINE", Environment, Table).AppendLine()
        '    ld.AppendFormat("where {0}${1}.LINE > 0 and LOADED <> 'Y'", Environment, Table).AppendLine()
        '    ld.AppendFormat("and (NVL({0}$ERRMSGS.T$USER,{1}) = {1})", Environment, tUser.ToString).AppendLine()
        '    ld.AppendFormat("and (NVL({0}$ERRMSGS.TYPE,'i') = 'i')", Environment, tUser.ToString).AppendLine()

        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)
        '    Dim RD As GenericDataReader = cmd.ExecuteReader()
        '    While RD.Read
        '        Log.AppendFormat("Line {0} {1}", RD(0), RD(1)).AppendLine()
        '    End While
        '    RD.Close()

        'Else ' All Lines Loaded
        '    ld = New System.Text.StringBuilder
        '    ld.AppendFormat("SELECT count(*) FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'TABLE' AND upper(OBJECT_NAME) = upper('{0}${1}_LD')", Environment, Table).AppendLine()
        '    If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '        Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '    cmd = New GenericCommand(ld.ToString, Connection)

        '    If cmd.ExecuteScalar() = 1 Then
        '        Log.AppendFormat("Found {0}_LD (loaded) table.", Table).AppendLine()
        '        Log.AppendFormat("Copying data from {0} to {0}_LD table.", Table).AppendLine()

        '        ld = New System.Text.StringBuilder
        '        ld.AppendFormat("DECLARE LN INTEGER ;", "").AppendLine()
        '        ld.AppendFormat("BEGIN", "").AppendLine()
        '        ld.AppendFormat("select MAX(LINE) into LN from {0}${1}_LD;", Environment, Table).AppendLine()
        '        ld.AppendFormat("update {0}${1} set LINE = (LINE + LN) where LINE > 0;", Environment, Table).AppendLine()
        '        ld.AppendFormat("update {0}$ERRMSGS set LINE = (LINE + LN) where T$USER = {1};", Environment, tUser.ToString).AppendLine()
        '        ld.AppendFormat("insert into {0}${1}_LD select * from {0}${1};", Environment, Table).AppendLine()
        '        ld.AppendFormat("END;", "").AppendLine()

        '        If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
        '            Log.Append("Running SQL:").AppendLine.Append(ld.ToString).AppendLine()
        '        cmd = New GenericCommand(ld.ToString, Connection)
        '        cmd.ExecuteNonQuery()
        '    Else
        '        Log.AppendFormat("Loaded table not created: {0}_LD.", Table).AppendLine()
        '    End If

        'End If

        'Return Not (loadLines > LoadedLines)

    End Function

    Private Function ExecuteAndLog(ByRef ld As System.Text.StringBuilder, ByRef Log As Builder, ByVal resultName As String) As Object

        Dim ret As Object = Nothing
        Try
            If ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane Then _
                Log.AppendLine.Append("Running SQL:").AppendLine.Append(ld.ToString)
            Dim cmd As New GenericCommand(ld.ToString, Connection)
            ret = cmd.ExecuteScalar()

        Catch ex As Exception
            Log.AppendLine.AppendFormat("Unexpected error getting load results: {0}", ex.Message).AppendLine()
            If Not (ev.LogVerbosity = ntEvtlog.EvtLogVerbosity.Arcane) Then _
                Log.Append("Running SQL:").AppendLine.Append(ld.ToString)

        Finally
            Select Case ev.LogVerbosity
                Case ntEvtlog.EvtLogVerbosity.Normal
                Case Else
                    Log.AppendFormat("{0} = {1}", resultName, ret.ToString).AppendLine()
            End Select

        End Try

        Return ret.ToString

    End Function

#End Region

End Module

Friend Class SaveDestination

    Public Sub New(ByVal BubbleFile As String)
        _BubbleFile = BubbleFile
    End Sub

    Private _MoveToFolder As tBubbleFolder
    Public Property MoveToFolder() As tBubbleFolder
        Get
            Return _MoveToFolder
        End Get
        Set(ByVal value As tBubbleFolder)
            _MoveToFolder = value
        End Set
    End Property

    Private _Environment As String = Nothing
    Public Property Environment() As String
        Get
            Return _Environment
        End Get
        Set(ByVal value As String)
            _Environment = value
        End Set
    End Property

    Private _procedure As String = Nothing
    Public Property procedure() As String
        Get
            Return _procedure
        End Get
        Set(ByVal value As String)
            _procedure = value
        End Set
    End Property

    Private _BubbleFile As String
    Private Property BubbleFile() As String
        Get
            Return _BubbleFile
        End Get
        Set(ByVal value As String)
            _BubbleFile = value
        End Set
    End Property

    Private Function Security() As System.Security.AccessControl.DirectorySecurity
        Dim dSecurity As New DirectorySecurity
        With dSecurity
            .AddAccessRule( _
                New FileSystemAccessRule( _
                    "everyone", _
                    FileSystemRights.Modify, _
                    (InheritanceFlags.ContainerInherit + InheritanceFlags.ObjectInherit), _
                    PropagationFlags.InheritOnly, _
                    AccessControlType.Allow _
                ) _
            )
            .SetAccessRuleProtection(True, True)
        End With
        Return dSecurity
    End Function

    Public Function DestinationPath(ByRef logBuilder As Builder) As String
        Try
            With Me
                ' Build the destination path for this bubble
                Dim pth As String = BubbleFolder(.MoveToFolder)

                ' Add environment and table as folders if they have been specified
                If Not (IsNothing(.Environment) Or IsNothing(.procedure)) Then
                    pth += String.Format("\{0}\{1}\{2}{3}{4}", _
                         .Environment, _
                         .procedure, _
                         Date.Now.Year.ToString.Substring(2), _
                         Date.Now.Month.ToString, _
                         Date.Now.Day.ToString _
                     )
                End If

                ' Get the top level path
                Dim rootfolder As String = iisFolder

                ' Check each folder in path exists
                For Each d As String In pth.Split("\")
                    ' Add the current path element to the rootfolder param
                    rootfolder = IO.Path.Combine(rootfolder, d)
                    ' And attempt to oppen it
                    Dim di As New DirectoryInfo(rootfolder)
                    With di
                        ' Does it exist?
                        If Not .Exists Then
                            Try
                                ' Folder does not exist so create
                                .Create()
                            Catch ex As Exception
                                ' Cannot create the log/folder
                                ' return default movetofolder param (either log or badmail)
                                logBuilder.AppendFormat("Unable to create folder [{0}]. {1}", rootfolder, ex.Message).AppendLine()
                                logBuilder.AppendFormat("Using {0} instead.", BubbleFolder(MoveToFolder)).AppendLine()
                                Return BubbleFolder(MoveToFolder)
                            End Try
                        End If
                    End With
                Next

                ' The path is valid 
                ' So return the full new destination filename
                Return Replace( _
                    BubbleFile, _
                    BubbleFolder(tBubbleFolder.QueueFolder), _
                    pth _
                    , , , CompareMethod.Text _
                )

            End With

        Catch ex As Exception
            ' MUST always return a valid path 
            ' even if this function fails
            logBuilder.AppendFormat("Unexpected error in BubbleHandler.DestinationPath() function. ", "").AppendLine()
            logBuilder.AppendFormat("{0}", ex.Message).AppendLine()
            logBuilder.AppendFormat("Using {0} instead.", BubbleFolder(MoveToFolder)).AppendLine()

            ' return default movetofolder param (either log or badmail)
            Return BubbleFolder(MoveToFolder)

        End Try

    End Function

End Class