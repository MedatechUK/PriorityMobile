Imports System.IO

Friend Class NetworkDrive

    Public Sub New(ByVal DriveLetter As String, ByVal UNCPath As String, ByVal info As DriveInfo)
        _DriveLetter = DriveLetter.ToString.Trim
        _UNCPath = UNCPath.ToString.Trim
        _info = info
    End Sub

    Private _DriveLetter As String
    Public Property DriveLetter() As String
        Get
            Return _DriveLetter.Trim.ToString
        End Get
        Set(ByVal value As String)
            _DriveLetter = value.ToString.Trim
        End Set
    End Property

    Private _UNCPath As String
    Public Property UNCPath() As String
        Get
            Dim retstr As New System.Text.StringBuilder
            For i As Integer = 0 To _UNCPath.Length - 1
                Select Case _UNCPath.Substring(i, 1).ToLower
                    Case "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "x", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "\", "-", "_", ".", "+"
                        retstr.Append(_UNCPath.Substring(i, 1))
                    Case Chr(32)
                        If i + 1 < _UNCPath.Length - 1 Then
                            Select Case _UNCPath.Substring(i + 1, 1).ToLower
                                Case "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "x", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "\", "-", "_", ".", "+"
                                    retstr.Append(_UNCPath.Substring(i, 1))
                            End Select
                        End If
                End Select
            Next
            Return retstr.ToString
        End Get
        Set(ByVal value As String)
            _UNCPath = value.ToString.Trim
        End Set
    End Property

    Private _info As DriveInfo
    Public Property info() As DriveInfo
        Get
            Return _info
        End Get
        Set(ByVal value As DriveInfo)
            _info = value
        End Set
    End Property

End Class

Module MapDrive

    Declare Function WNetGetConnection Lib "mpr.dll" Alias "WNetGetConnectionA" (ByVal lpszLocalName As String, _
             ByVal lpszRemoteName As String, ByRef cbRemoteName As Integer) As Integer

    Private ReadOnly Property NetworkDrives() As List(Of NetworkDrive)
        Get
            Dim ret As New List(Of NetworkDrive)
            For Each d As DriveInfo In DriveInfo.GetDrives
                If d.DriveType = IO.DriveType.Network Then
                    Dim UNCName As String = Space(160)
                    If WNetGetConnection(d.Name.Substring(0, 2), UNCName, UNCName.Length) = 0 Then
                        ret.Add(New NetworkDrive(d.Name.Substring(0, 2), UNCName.ToString.Trim, d))
                    End If
                End If
            Next
            Return ret
        End Get
    End Property

    Public ReadOnly Property UnusedDriveLetter() As List(Of String)
        Get
            Dim ret As New List(Of String)
            With ret
                .Add("P:")
                .Add("Z:")
                .Add("U:")
                .Add("W:")
                .Add("V:")
                .Add("K:")
                .Add("S:")
                .Add("H:")
                .Add("I:")
                .Add("O:")
                .Add("X:")
                For Each nDrive As NetworkDrive In NetworkDrives()
                    .Remove(nDrive.DriveLetter)
                Next
            End With
            Return ret
        End Get
    End Property

    Public Function GetNetworkDrive(ByVal UNCPath As String) As NetworkDrive
        If UNCPath.Length = 0 Or Not UNCPath.Substring(0, 2) = "\\" Then Return Nothing
        For Each d As DriveInfo In DriveInfo.GetDrives
            If d.DriveType = IO.DriveType.Network Then
                Dim UNCName As String = Space(160)
                If WNetGetConnection(d.Name.Substring(0, 2), UNCName, UNCName.Length) = 0 Then
                    If String.Compare(UNCName.ToString.Trim, UNCPath.ToString.Trim, True) = 0 Then
                        Return New NetworkDrive(d.Name.Substring(0, 2), UNCName, d)
                        Exit For
                    End If
                End If
            End If
        Next
        Return Nothing
    End Function

    Public Sub MapDriveLetter(ByVal DriveLetter As String, ByVal UNCPath As String, ByRef LogBuilder As Builder)

        Dim myProcess As New Process()
        With myProcess
            With .StartInfo
                .FileName = "cmd.exe"
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardInput = True
                .RedirectStandardError = True
            End With

            .Start()

            Dim sErr As StreamReader = .StandardError
            Dim sIn As StreamWriter = .StandardInput

            With sIn
                .AutoFlush = True
                LogBuilder.Append("Scanning locally mapped drives.").AppendLine()
                For Each nDrive As NetworkDrive In NetworkDrives()
                    LogBuilder.AppendFormat("Found drive [{0}] mapped to [{1}].", nDrive.DriveLetter, nDrive.UNCPath).AppendLine() '
                    If String.Compare(nDrive.UNCPath, UNCPath, True) = 0 Or _
                        String.Compare(nDrive.DriveLetter, DriveLetter, True) = 0 Then
                        LogBuilder.AppendFormat("Deleting drive [{0}] mapped to [{1}].", nDrive.DriveLetter, nDrive.UNCPath.ToString).AppendLine()
                        .Write( _
                            String.Format("net use {0} /DELETE /y{1}", _
                                    nDrive.DriveLetter, _
                                    System.Environment.NewLine _
                            ) _
                        )
                    End If
                Next

                LogBuilder.AppendFormat("Mapping drive [{0}] to fileshare [{1}]...", DriveLetter, UNCPath).AppendLine()
                .Write( _
                    String.Format("net use {0} {1}{2}exit{2}", _
                            DriveLetter, _
                            UNCPath, _
                            System.Environment.NewLine _
                    ) _
                )
                .Close()
            End With

            While Not .HasExited
                Threading.Thread.Sleep(100)
            End While

            If Not sErr.EndOfStream Then
                Throw New Exception( _
                    String.Format( _
                        "Error Mapping drive {0} to {1}. {2}.", _
                        DriveLetter, _
                        UNCPath, _
                        sErr.ReadToEnd _
                    ) _
                )
            Else
                LogBuilder.AppendFormat("Mapped drive [{0}] to [{1}].", DriveLetter, UNCPath).AppendLine()
            End If

            sErr.Close()

        End With

    End Sub

End Module
