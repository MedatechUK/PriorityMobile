Imports System.Reflection
Imports System.Xml
Imports System.IO
Imports System.Net

Public Class Update : Implements IDisposable

#Region "Private Properties"

    Private _svcType As String    

#End Region

#Region "Public Properties"

    Private _AssemblyFile As String
    Public ReadOnly Property Assemblyfile() As String
        Get
            Return _AssemblyFile
        End Get
    End Property

    Private ReadOnly Property updService() As String
        Get
            Return "http://redknave:8080/"
        End Get
    End Property

    Private Function ServerVersion(ByRef StartLog As msgLogRequest) As System.Version

        Static _ServerVersion As System.Version = Nothing
        If Not IsNothing(_ServerVersion) Then
            Return _ServerVersion
        Else
            StartLog.LogData.AppendFormat("Contacting update service...")
            Try
                Dim updSvc As New XmlDocument
                updSvc.Load( _
                    String.Format( _
                        "{0}update.ashx?dll={1}", _
                         updService, _
                         Assemblyfile _
                     ) _
                )
                Dim response As XmlNode = updSvc.SelectSingleNode("response")
                With response
                    If .Attributes("status").Value = 200 Then
                        StartLog.LogData.AppendFormat("[Ok]").AppendLine()
                        _ServerVersion = New System.Version( _
                            .Attributes("Major").Value, _
                            .Attributes("Minor").Value, _
                            .Attributes("Build").Value, _
                            .Attributes("Revision").Value _
                        )
                        With _ServerVersion
                            StartLog.LogData.AppendFormat("Server version is {0}.{1}.{2}.{3}.", _
                                .Major, _
                                .Minor, _
                                .Build, _
                                .Revision _
                            ).AppendLine()
                        End With
                    Else
                        Throw New Exception(.Attributes("message").Value)
                    End If
                End With

            Catch ex As Exception
                StartLog.LogData.Append("[Fail]").AppendLine()
                StartLog.LogData.AppendFormat("Error: {0}", ex.Message).AppendLine()
                _ServerVersion = New System.Version(0, 0, 0, 0)

            End Try

            Return _ServerVersion

        End If

    End Function

    Private Function CurrentVersion(ByRef StartLog As msgLogRequest) As System.Version

        Static _CurrentVersion As System.Version = Nothing
        If Not IsNothing(_CurrentVersion) Then
            Return _CurrentVersion
        Else
            If File.Exists(Path.Combine(BinFolder, Assemblyfile)) Then
                Dim ass As AssemblyName = AssemblyName.GetAssemblyName(Path.Combine(BinFolder, Assemblyfile))
                StartLog.LogData.AppendFormat("Current version is {0}.{1}.{2}.{3}.", _
                    ass.Version.Major, _
                    ass.Version.Minor, _
                    ass.Version.Build, _
                    ass.Version.Revision _
                ).AppendLine()
                _CurrentVersion = ass.Version

            Else
                _CurrentVersion = New System.Version(0, 0, 0, 0)
            End If
        End If
        Return _CurrentVersion

    End Function

    Private ReadOnly Property BinFolder() As String
        Get
            Static _BinFolder As String = Nothing
            If IsNothing(_BinFolder) Then
                Dim loc As String = Reflection.Assembly.GetExecutingAssembly.Location
                _BinFolder = loc.Substring(0, loc.LastIndexOf("\"))
            End If
            Return _BinFolder
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef sArg As StartArgs)

        _svcType = sArg.ServiceType        
        _AssemblyFile = String.Format( _
            "h{0}.dll", _
            sArg.ServiceType _
        )

        With sArg.StartLog.LogData

            .AppendFormat("Running {0}.", sArg.CallingAssembly.Location).AppendLine()

            CurrentVersion(sArg.StartLog)
            ServerVersion(sArg.StartLog)

            .AppendFormat("{0}: Checking for updates...", Assemblyfile)

            If Not File.Exists(Path.Combine(BinFolder, Assemblyfile)) Then
                .AppendFormat( _
                    "{0} not found.", _
                    Assemblyfile, _
                    updService _
                ).AppendLine()
                DownloadUpdated(sArg.StartLog)

            ElseIf Not ( _
                ServerVersion( _
                    sArg.StartLog _
                ).CompareTo( _
                    CurrentVersion( _
                        sArg.StartLog _
                    ) _
                ) <= 0) Then

                .AppendFormat( _
                    "Found newer version of {0}.", _
                    Assemblyfile, _
                    updService _
                ).AppendLine()
                DownloadUpdated(sArg.StartLog)

            Else
                With ServerVersion(sArg.StartLog)
                    If Not (.Major = 0 And .Minor = 0 And .Build = 0 And .Revision = 0) Then
                        sArg.StartLog.LogData.AppendFormat( _
                            "Assembly {0} is up to date.", _
                            Assemblyfile _
                        ).AppendLine()
                    End If
                End With
            End If

            If Not File.Exists(Path.Combine(BinFolder, Assemblyfile)) Then
                Throw New SystemFail( _
                    eSysFailCode.FAIL_NO_HANDLER, _
                    String.Format( _
                        "{0} not found.", _
                        Assemblyfile, _
                        updService _
                    ) _
                )
            End If

        End With

    End Sub

#End Region

#Region "Create Object"

    Public Function LoadHandler(ByRef sArg As StartArgs) As Object

        Dim args(0) As Object
        Dim hdlr As System.Reflection.Assembly

        Try
            hdlr = Assembly.LoadFrom(Path.Combine(BinFolder, Assemblyfile))

        Catch ex As Exception
            Throw New SystemFail( _
                eSysFailCode.FAIL_ASSBY_LOAD, _
                String.Format( _
                    "Could not load assembly {0}. {1}", _
                    Assemblyfile, _
                    ex.Message _
                ) _
            )

        End Try

        Try
            Return Activator.CreateInstance( _
                hdlr.GetType( _
                    String.Format( _
                        "PriPROC.{0}", _svcType _
                    ) _
                ), _
                sArg _
            )

        Catch ex As Exception
            Throw New SystemFail( _
                eSysFailCode.FAIL_ASSBY_CTOR, _
                String.Format( _
                    "Could not create instance of [PriPROC.{0}] from assembly {1}. {2}", _
                    sArg.ServiceType, _
                    Assemblyfile, _
                    ex.Message _
                ) _
            )

        End Try

    End Function

#End Region

#Region "Perform Update"

    Public Sub DownloadUpdated(ByRef StartLog As msgLogRequest)

        StartLog.LogData.AppendFormat( _
            "Downloading {0} from {1}...", _
            Assemblyfile, _
            updService _
        )

        Dim getFileResult As Exception = _
        GetFile( _
            New System.Uri(updService), _
            Assemblyfile, _
            BinFolder _
        )

        If IsNothing(getFileResult) Then
            ' Download ok
            StartLog.LogData.AppendFormat( _
                "[Ok]" _
            ).AppendLine()

            Dim tries As Integer = 0
            While File.Exists(Path.Combine(BinFolder, Assemblyfile)) And tries < 5
                Try
                    File.Delete(Path.Combine(BinFolder, Assemblyfile))
                Catch
                    tries += 1
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            If Not File.Exists(Path.Combine(BinFolder, Assemblyfile)) Then
                File.Move(String.Format("{0}.tmp", Path.Combine(BinFolder, Assemblyfile)), Path.Combine(BinFolder, Assemblyfile))

                StartLog.LogData.AppendFormat( _
                    "Updated {4} to version {0}.{1}.{2}.{3}.", _
                    ServerVersion(StartLog).Major, _
                    ServerVersion(StartLog).Minor, _
                    ServerVersion(StartLog).Build, _
                    ServerVersion(StartLog).Revision, _
                    Assemblyfile _
                ).AppendLine()
            Else
                ' Overwrite Fail
                StartLog.LogData.AppendFormat( _
                    "[Fail]" _
                ).AppendLine()
                StartLog.LogData.AppendFormat("Error: {0}", "File in use.").AppendLine()
            End If

        Else
            ' Download fail
            StartLog.LogData.AppendFormat( _
                "[Fail]" _
            ).AppendLine()
            StartLog.LogData.AppendFormat("Error: {0}", getFileResult.Message).AppendLine()
        End If

    End Sub

    Private Function GetFile(ByVal Server As Uri, ByVal filename As String, ByVal toFolder As String) As Exception

        Dim ret As New Exception
        ret = Nothing

        Dim DownloadTo As String = String.Format( _
            "{0}\{1}.tmp", _
            toFolder, _
            filename.Replace("/", "\") _
        )

        Try
            While File.Exists(DownloadTo)
                Try
                    File.Delete(DownloadTo)
                Catch
                End Try
            End While

            Dim dlfile As String = _
                    String.Format("{0}{1}", _
                        Server.AbsoluteUri, _
                        filename.Replace("\", "/") _
                    )

            Dim httpRequest As HttpWebRequest = DirectCast( _
                WebRequest.Create(dlfile _
                ),  _
                HttpWebRequest _
            )

            httpRequest.Credentials = CredentialCache.DefaultCredentials

            Dim httpResponse As HttpWebResponse = DirectCast( _
                httpRequest.GetResponse(),  _
                HttpWebResponse _
            )

            Dim dataStream As System.IO.Stream = httpResponse.GetResponseStream()
            Dim inBuf As Byte() = New Byte(10000000) {}
            Dim bytesToRead As Integer = Convert.ToInt32(inBuf.Length)
            Dim bytesRead As Integer = 0
            While bytesToRead > 0
                Dim n As Integer = dataStream.Read(inBuf, bytesRead, bytesToRead)
                If n = 0 Then
                    Exit While
                End If
                bytesRead += n
                bytesToRead -= n
            End While

            Dim fstr As New FileStream( _
                DownloadTo, _
                FileMode.OpenOrCreate, _
                FileAccess.Write _
            )

            fstr.Write(inBuf, 0, bytesRead)
            dataStream.Close()
            fstr.Close()

        Catch ex As Exception
            ret = ex
        End Try

        Return ret

    End Function

#End Region

#Region " IDisposable Support "

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
