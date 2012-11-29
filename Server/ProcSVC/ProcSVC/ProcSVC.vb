Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Threading

Public Class ProcSVC

    Friend server As TcpListener
    Dim stopping As Boolean = False

    Protected Overrides Sub OnStart(ByVal args() As String)
        If loadconfig() Then

            ' Are we mapping a drive ?
            If Len(_prepMapDrive) > 0 Then
                Try ' to map the drive
                    Dim map As New cNetworkDrive
                    With map
                        .LocalDrive = _prepMapDrive & ":"
                        .ShareName = _prepUNC
                        .MapDrive(_Domain & "\" & _UserName, _Password)
                    End With
                Catch ex As Exception
                    If ex.Message <> "The local device name is already in use" Then
                        WriteToEventLog(ex.Message)
                        Me.ExitCode = 2
                        Me.Stop()
                    End If
                End Try
            End If

            Try
                Dim localAddr As IPAddress = IPAddress.Parse("127.0.0.1")
                server = New TcpListener(localAddr, 8022)
                server.Start()
            Catch ex As Exception
                WriteToEventLog(ex.Message)
                Me.ExitCode = 3
                Me.Stop()
            End Try

            Me.MainProcess.RunWorkerAsync()

        Else
            Me.ExitCode = 1
            Me.Stop()
        End If
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        stopping = True
        server.Stop()
        MyBase.OnStop()
    End Sub

    Protected Overrides Sub OnContinue()
        MyBase.OnContinue()
    End Sub
    Private Sub DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles MainProcess.DoWork
        ' Enter the listening loop.            
        While Not stopping
            If server.Pending Then
                Do Until Not server.Pending
                    Dim myThread As Thread
                    myThread = New Thread(New ThreadStart(AddressOf Connect))
                    myThread.Start()
                    Thread.Sleep(10)
                Loop
            End If
            Thread.Sleep(1)
        End While
    End Sub

    Private Sub Connect()

        Dim myConnectionobject As New thdListener()
        With myConnectionobject
            .Client = server.AcceptSocket()
            .Connection()
        End With
        myConnectionobject = Nothing

    End Sub

End Class
