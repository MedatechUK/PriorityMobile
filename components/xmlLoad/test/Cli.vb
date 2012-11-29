Imports System.IO

Friend Class AsyncState

    Public Sub New(ByVal stream As StreamReader, ByVal buffer As Byte())
        _stream = stream
        _buffer = buffer
    End Sub

    Public ReadOnly Property Stream() As StreamReader
        Get
            Return _stream
        End Get
    End Property

    Public ReadOnly Property Buffer() As Byte()
        Get
            Return _buffer
        End Get
    End Property

    Protected _stream As StreamReader
    Protected _buffer As Byte()

End Class

Public Class Cli

    Private proc As Process
    Private CurrentCommand As String = ""
    Private SI As StreamWriter
    Private SO As StreamReader
    Private SE As StreamReader

    Private _outputReady As AsyncCallback
    Private _outputState As AsyncState
    Private _errorReady As AsyncCallback
    Private _errorState As AsyncState

    Private _errorBuffer As Byte() = New Byte(511) {}
    Private _outputBuffer As Byte() = New Byte(511) {}

    Private _Started As Boolean = False
    Private _closing As Boolean = False

    Private starttmr As System.Timers.Timer

    Public Event Response(ByVal ConnectionID As String, ByVal Data As String)
    Public Event Exited(ByVal ConnectionID As String)

    Public Sub New(ByVal ConnectionID As String)

        _ConnectionID = ConnectionID

        proc = New Process       
        With proc
            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                .RedirectStandardError = True
                .WorkingDirectory = iisFolder
            End With

            AddHandler .Exited, AddressOf hExit
            .EnableRaisingEvents = True
            .Start()

            SI = .StandardInput
            SI.AutoFlush = True

            SO = .StandardOutput
            _outputReady = New AsyncCallback(AddressOf OutputCallback)
            _outputState = New AsyncState(SO, _outputBuffer)
            SO.BaseStream.BeginRead(_outputBuffer, 0, _outputBuffer.Length, _outputReady, _outputState)

            SE = .StandardError
            _errorReady = New AsyncCallback(AddressOf OutputCallback)
            _errorState = New AsyncState(SE, _errorBuffer)
            SE.BaseStream.BeginRead(_errorBuffer, 0, _errorBuffer.Length, _errorReady, _errorState)

        End With

        starttmr = New System.Timers.Timer
        With starttmr
            .Interval = 500
            AddHandler .Elapsed, AddressOf hStart
            .Enabled = True
        End With

    End Sub

    Private Sub hStart(ByVal sender As Object, ByVal e As System.EventArgs)

        With starttmr
            .Enabled = False
            .Dispose()
        End With

        _Started = True

        CurrentCommand = "rem" & _
            System.Environment.NewLine
        With SI
            .Write("rem" & _
               System.Environment.NewLine)
        End With
    End Sub

    Private _ConnectionID
    Public ReadOnly Property ConnectionID() As String
        Get
            Return _ConnectionID
        End Get
    End Property

    Private Sub hExit(ByVal sender As Object, ByVal e As System.EventArgs)

        SI.Dispose()
        SO.Dispose()
        SE.Dispose()

        RaiseEvent Exited(Me.ConnectionID)
    End Sub

    Private Sub OutputCallback(ByVal ar As IAsyncResult)
        If Not _closing Then
            Dim state As AsyncState = DirectCast(ar.AsyncState, AsyncState)
            Dim count As Integer = state.Stream.BaseStream.EndRead(ar)
            Try
                If _Started And count > 0 Then
                    Dim bstr = System.Text.Encoding.ASCII.GetString(state.Buffer, 0, count)
                    If CurrentCommand.Length > 0 Then
                        If bstr.IndexOf(CurrentCommand) > -1 Then
                            bstr = bstr.Replace(CurrentCommand, "")
                            CurrentCommand = ""
                        End If
                    End If
                    If bstr.Length > 0 Then RaiseEvent Response(Me.ConnectionID, bstr)
                End If
            Catch ex As Exception
                RaiseEvent Response(Me.ConnectionID, String.Format("-{0}", ex.Message))
            Finally
                If Not _closing Then state.Stream.BaseStream.BeginRead(state.Buffer, 0, state.Buffer.Length, _outputReady, state)
            End Try
        End If
    End Sub

    Public Sub Send(ByVal Command As String)
        If Not _closing Then
            If String.Compare(Command, "exit", True) = 0 Then _closing = True
            CurrentCommand = Command & _
                System.Environment.NewLine
            With SI
                .Write(Command & _
                   System.Environment.NewLine)
            End With
        End If
    End Sub

    Public Sub Quit()        
        _closing = True
        CurrentCommand = "exit" & _
               System.Environment.NewLine
        With SI
            .Write("exit" & _
               System.Environment.NewLine)
        End With
    End Sub

End Class

