Imports ntEvtlog

Public Class LoadCallBack

    Private _ev As ntEvtlog.evt
    Sub New(Optional ByRef ev As ntEvtlog.evt = Nothing)
        If Not IsNothing(ev) Then
            _ev = ev
        End If
    End Sub

    Private _Result As String
    Public ReadOnly Property Result() As Boolean
        Get
            Return _Result = "Y"
        End Get
    End Property

    Private _Data As String = ""
    Public ReadOnly Property Data() As String
        Get
            Return _Data
        End Get
    End Property

    Private _Connection As SqlClient.SqlConnection = Nothing
    Public Property Connection() As SqlClient.SqlConnection
        Get
            Return _Connection
        End Get
        Set(ByVal value As SqlClient.SqlConnection)
            _Connection = value
        End Set
    End Property

    Private _Table As String = Nothing
    Public Property Table() As String
        Get
            Return _Table
        End Get
        Set(ByVal value As String)
            _Table = value
        End Set
    End Property

    Private ReadOnly Property OpenConnection() As SqlClient.SqlCommand
        Get
            Try
                If IsNothing(Connection) Then Throw New Exception("The connection has not been initialised.")
                If IsNothing(Table) Then Throw New Exception("No table has been specified.")
                If Not (Connection.State = ConnectionState.Open) Then
                    Connection.Open()
                End If
                Return Connection.CreateCommand()
            Catch ex As Exception
                Throw New Exception(String.Format("Unable to open connection [{0}]. The error was: {1}", Connection.ConnectionString, ex.Message))
            End Try
        End Get
    End Property

    Private ReadOnly Property TriggerName() As String
        Get
            Return String.Format("{0}_SFDCLdTrig", Table)
        End Get
    End Property

    Public ReadOnly Property IsBubbleTable() As Boolean
        Get
            Dim cmd As SqlClient.SqlCommand = OpenConnection
            cmd.CommandText = String.Format( _
                "SELECT COUNT(*) FROM sys.all_columns where object_id = " & _
                "(select object_id from sys.tables where name = '{0}') " & _
                "and name = 'BUBBLEID'" _
                , Table _
            )
            Try
                If cmd.ExecuteScalar > 0 Then
                    cmd.CommandText = "SELECT count(*) " & _
                                      "FROM sys.tables " & _
                                      "where name = 'ZSFDC_EVENT'"
                    If cmd.ExecuteScalar = 0 Then
                        cmd.CommandText = "CREATE TABLE [dbo].[ZSFDC_EVENT]( " & _
                                          "	[BUBBLEID] [nvarchar](50) NOT NULL DEFAULT (''), " & _
                                          "	[RESULT] [nchar](1) NOT NULL DEFAULT (''), " & _
                                          "	[RESULTDATA] [nvarchar](255) NOT NULL DEFAULT ('') " & _
                                          ") ON [PRIMARY]"
                        cmd.ExecuteNonQuery()
                    End If
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw New Exception(String.Format("Unable to open verify if the table [{0}] supports Bubble events. " & _
                    "The error was: {1}", Table, ex.Message))
            End Try
        End Get
    End Property

    Public ReadOnly Property TriggerExists() As Boolean
        Get
            Dim cmd As SqlClient.SqlCommand = OpenConnection
            cmd.CommandText = String.Format("select count(*) from sys.triggers where name = '{0}'", TriggerName)
            Try
                Return cmd.ExecuteScalar > 0
            Catch ex As Exception
                Throw New Exception(String.Format("Unable to open verify trigger [{0}] exists. " & _
                    "The error was: {1}", TriggerName, ex.Message))
            End Try
        End Get
    End Property

    Public Function MakeTrigger() As Boolean
        Dim ret As Boolean = False
        Try
            Dim cmd As SqlClient.SqlCommand = OpenConnection

            If TriggerExists() Then ' Try to drop existing trigger
                Try
                    cmd.CommandText = String.Format( _
                        "DROP TRIGGER [dbo].[{0}]", _
                        TriggerName _
                    )
                Catch ex As Exception
                    Throw New Exception(String.Format("The trigger [{0}] already exists. " & _
                        "DROP failed becuase: (1)", TriggerName, ex.Message))
                End Try
            End If

            cmd.CommandText = _
                String.Format( _
                    "CREATE TRIGGER [dbo].[{0}] {2}" & _
                    "   ON  [dbo].[{1}] {2}" & _
                    "   AFTER UPDATE {2}" & _
                    "AS  {2}" & _
                    "begin  {2}" & _
                    "	SET NOCOUNT ON;  {2}" & _
                    "	declare @b int {2}" & _
                    "	set @b = (select COUNT(*) FROM sys.all_columns where object_id =  {2}" & _
                    "	(select object_id from sys.tables where name = '{1}') {2}" & _
                    "	and name = 'BUBBLEID') {2}" & _
                    "	if @b = 1 {2}" & _
                    "	begin {2}" & _
                    "		declare @c int  {2}" & _
                    "		declare @bstr varchar(50)  {2}" & _
                    "		declare @k int  {2}" & _
                    "		declare @t int {2}" & _
                    "		set @bstr = (select top 1 BUBBLEID from inserted)  {2}" & _
                    "		set @c = (select count(LOADED) from {1}  {2}" & _
                    "		where BUBBLEID = @bstr and LOADED <> 'Y')  {2}" & _
                    "		if @c = 0  {2}" & _
                    "		begin  {2}" & _
                    "			set @k = (select max(KEY1) from {1} where BUBBLEID = @bstr and KEY1 <> '')  {2}" & _
                    "			update dbo.ZSFDC_EVENT set RESULT='Y', RESULTDATA = @k  {2}" & _
                    "			where BUBBLEID = @bstr  {2}" & _
                    "			set @t = (select count(*) from sys.tables where name ='{1}_LD') {2}" & _
                    "			if @t = 1 {2}" & _
                    "			begin {2}" & _
                    "				insert into {1}_LD {2}" & _
                    "				select * from {1}  {2}" & _
                    "				where BUBBLEID = @bstr {2}" & _
                    "			end {2}" & _
                    "			delete from {1} where BUBBLEID = @bstr {2}" & _
                    "		end  {2}" & _
                    "	end {2}" & _
                    "end {2}", _
                    TriggerName, Table, vbCrLf _
                )
            Try
                cmd.ExecuteNonQuery()
                ret = True
            Catch ex As Exception
                Throw New Exception(String.Format("CREATE Trigger [{0}] failed. The error was: {1}", TriggerName, ex.Message))
            End Try

        Catch ex As Exception
            ' Log the exception
        End Try

        Return ret
    End Function

    Public Function RegisterBubble(ByVal BubbleID As String) As Boolean
        Dim ret As Boolean = False
        Try
            Dim cmd As SqlClient.SqlCommand = OpenConnection
            cmd.CommandText = _
                String.Format( _
                    "insert into ZSFDC_EVENT (BUBBLEID,RESULT) values ('{0}','N')", _
                    BubbleID _
                )
            Try
                cmd.ExecuteNonQuery()
                ret = True
            Catch ex As Exception
                Throw New Exception(String.Format("Register bubble [{0}] failed. The error was: {1}", BubbleID, ex.Message))
            End Try

        Catch ex As Exception
            ' Log the exception
            Me.SafeLog( _
                String.Format( _
                    "{0}", _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
        Return ret
    End Function

    Public Function UnRegisterBubble(ByVal BubbleID As String) As Boolean
        Dim ret As Boolean = False
        Try
            Dim cmd As SqlClient.SqlCommand = OpenConnection
            cmd.CommandText = _
                String.Format( _
                    "delete from ZSFDC_EVENT where BUBBLEID= '{0}'", _
                    BubbleID _
                )
            Try
                cmd.ExecuteNonQuery()
                ret = True
            Catch ex As Exception
                Throw New Exception(String.Format("UnRegister bubble [{0}] failed. " & _
                    "The error was: {1}", BubbleID, ex.Message))
            End Try

        Catch ex As Exception
            ' Log the exception
            Me.SafeLog( _
                String.Format( _
                    "{0}", _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
        Return ret
    End Function

    Public Function GetBubbleStatus(ByVal BubbleID As String) As Boolean
        Dim ret As Boolean = False
        Try
            Dim cmd As SqlClient.SqlCommand = OpenConnection
            cmd.CommandText = _
                String.Format( _
                    "SELECT RESULT, RESULTDATA from dbo.ZSFDC_EVENT WHERE BUBBLEID = '{0}'", _
                    BubbleID _
                )
            Try
                Dim dataReader As SqlClient.SqlDataReader = _
                             cmd.ExecuteReader()
                With dataReader
                    If .HasRows Then
                        .Read()
                        _Result = .Item(0)
                        _Data = .Item(1)
                        ret = True
                    Else
                        ret = False
                    End If
                    .Close()
                End With
            Catch ex As Exception
                Throw New Exception(String.Format("Could not read status for bubble [{0}]. " & _
                    "The error was: {1}", BubbleID, ex.Message))
            End Try

        Catch ex As Exception
            ' Log the exception
            Me.SafeLog( _
                String.Format( _
                    "{0}", _
                    ex.Message _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
        Return ret
    End Function

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)

        Try
            If Not IsNothing(_ev) Then
                _ev.Log( _
                    Entry, _
                    EventType, _
                    Verbosity _
                  )
            End If
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _ev.LogName, _ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

End Class
