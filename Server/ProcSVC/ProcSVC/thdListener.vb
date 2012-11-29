Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.VisualBasic
Imports System.Threading

Friend Class thdListener

#Region "Declarations"

    ' Sockets
    Public Client As System.Net.Sockets.Socket
    Dim myConnect As System.Net.Sockets.Socket

    ' Buffers
    Dim buffer(1025) As Byte
    Dim bteAccept(1025) As Byte

    Dim quit As Boolean = False
    Dim _proc As String = ""

#End Region

#Region "Main Thread Loop"

    Public Sub Connection()

        ClientReceiveStart()

        While Not quit

            Try
                If Client.Connected Then
                    Thread.Sleep(10)
                Else
                    Exit While
                End If
            Catch e As Exception
                Exit While
            End Try

            Thread.Sleep(1)

        End While

        Client.Disconnect(True)

        Dim sOutput As String = ""
        Dim sErrs As String = ""
        Dim myProcess As Process = New Process()

        With myProcess
            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                .RedirectStandardError = True
            End With
            .Start()

            Dim sIn As StreamWriter = myProcess.StandardInput
            Dim sOut As StreamReader = myProcess.StandardOutput
            Dim sErr As StreamReader = myProcess.StandardError

            With sIn
                Dim cmd As String = _Bin95 & "\WINRUN.exe " & _
                    Chr(34) & Chr(34) & _
                    " " & _PriorityUser & _
                    " " & _PriorityPwd & _
                    " " & _prepUNC & "\system\prep" & _
                    " " & _PriorityCompany & _
                    " WINACTIV -P " & Split(_proc, ":")(0)

                .AutoFlush = True
                .Write("whoami" & _
                    System.Environment.NewLine)
                .Write("dir " & _prepMapDrive & ":" & _
                    System.Environment.NewLine)
                .Write(cmd & _
                    System.Environment.NewLine)
                .Write("exit" & _
                    System.Environment.NewLine)
                .Close()

            End With

            Dim l As Integer = 0
            Do Until l = 10000
                If sOut.Peek <> 0 Then
                    sOutput = sOutput + sOut.ReadLine
                End If
                l = l + 1
                Thread.Sleep(1)
            Loop

            If Len(sOutput) > 0 Then
                sOutput = sOut.ReadToEnd
                sOut.Close()
                sErrs = sErr.ReadToEnd()
                sErr.Close()
            End If

            If Not myProcess.HasExited Then
                myProcess.Kill()
            End If

            .Close()

        End With

        If Len(sErrs) > 0 Then
            WriteToEventLog(sErrs, EventLogEntryType.FailureAudit)
        End If

    End Sub

#End Region

#Region "Receive data from Sender"

    Private Sub ClientReceiveStart()

        Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientReceiveData)

        ReDim buffer(1025)
        Try

            Client.BeginReceive _
              (buffer, 0, buffer.Length, 0, _
              myAsyncCallBack, Client)
        Catch
            quit = True
        End Try
    End Sub

    Private Sub ClientReceiveData(ByVal pIAsyncResult As IAsyncResult)

        Dim intByte As Integer
        Try
            intByte = Client.EndReceive(pIAsyncResult)
            If intByte > 0 Then

                Dim b() As Byte = CleanBuffer(buffer)
                _proc = _proc & Encoding.ASCII.GetString(b)
                If InStr(_proc, ":") Then
                    quit = True
                Else
                    ClientReceiveStart()
                End If

                'sIn.Write(Encoding.ASCII.GetString(b))

            End If
        Catch
            quit = True
        End Try

    End Sub

#End Region

#Region "Send data to Sender"

    Private Sub ClientSendStart(ByVal send As String)
        Dim bteSend() As Byte
        Dim myAsyncCallBack As New AsyncCallback(AddressOf ClientSendData)

        Try
            bteSend = Encoding.ASCII.GetBytes(send)
            Client.BeginSend _
              (bteSend, 0, bteSend.Length, _
              SocketFlags.DontRoute, myAsyncCallBack, Client)
        Catch
            quit = True
        End Try
    End Sub

    Private Sub ClientSendData(ByVal pIAsyncResult As IAsyncResult)
        Dim intSend As Integer
        intSend = Client.EndSend(pIAsyncResult)
    End Sub

#End Region

#Region "Private Function"

    Private Function CleanBuffer(ByVal bteAccept() As Byte) As Byte()

        Dim b() As Byte = Nothing

        If Not bteAccept(bteAccept.Length - 1) = 0 Then

            Return bteAccept

        Else

            Dim lastzero As Integer = bteAccept.Length - 1

            For m As Integer = lastzero To 0 Step -10000
                If bteAccept(m) = 0 Then
                    lastzero = m
                Else
                    Exit For
                End If
            Next

            For m As Integer = lastzero To 0 Step -1000
                If bteAccept(m) = 0 Then
                    lastzero = m
                Else
                    Exit For
                End If
            Next

            For m As Integer = lastzero To 0 Step -100
                If bteAccept(m) = 0 Then
                    lastzero = m
                Else
                    Exit For
                End If
            Next

            For m As Integer = lastzero To 0 Step -10
                If bteAccept(m) = 0 Then
                    lastzero = m
                Else
                    Exit For
                End If
            Next

            For i As Integer = lastzero To 0 Step -1
                If bteAccept(i) = 0 Then
                    ReDim Preserve bteAccept(i - 1)
                Else
                    Exit For
                End If
            Next
            Return bteAccept
        End If

    End Function

    Private Function ReadPassword(ByVal pw As String) As System.Security.SecureString
        Dim ret As New System.Security.SecureString
        For Each c As Char In pw
            ret.AppendChar(c)
        Next
        Return ret
    End Function


#End Region

End Class