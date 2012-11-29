Imports System.Xml
Imports System.IO
Imports System.Security.AccessControl

Module iisFolders

#Region "Priavte Properties"

    Private webServer As String = String.Format("http://{0}:8080", "localhost")
    Private webConfig As New XmlDocument
    Private WithEvents WatchWebconfig As System.IO.FileSystemWatcher

#End Region

#Region "Bubble Folder defintions"

    Public Enum tBubbleFolder
        QueueFolder = 1
        BadMailFolder = 2
        LogFolder = 3
        Signatures = 4
    End Enum

    Public ReadOnly Property BubbleFolder(ByVal FolderType As tBubbleFolder) As String
        Get
            Select Case FolderType
                Case tBubbleFolder.BadMailFolder
                    Return "badmail"
                Case tBubbleFolder.LogFolder
                    Return "log"
                Case tBubbleFolder.QueueFolder
                    Return "queue"
                Case tBubbleFolder.Signatures
                    Return "signatures"
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public ReadOnly Property enumBubbleFolder() As List(Of tBubbleFolder)
        Get
            Dim ret As New List(Of tBubbleFolder)
            ret.Add(tBubbleFolder.QueueFolder)
            ret.Add(tBubbleFolder.LogFolder)
            ret.Add(tBubbleFolder.BadMailFolder)
            ret.Add(tBubbleFolder.Signatures)
            Return ret
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _NoIIS As Boolean = False
    Public ReadOnly Property NoIIS() As Boolean
        Get
            Return _NoIIS
        End Get
    End Property

    Public ReadOnly Property iisFolder() As String
        Get
            Static MapPath As String
            If IsNothing(MapPath) Then

                Dim ConfigASHX As New XmlDocument
                With ConfigASHX
                    Try
                        .Load(String.Format("{0}/config.ashx", webServer))
                    Catch ex As Exception
                        _NoIIS = True
                        Throw New Exception(String.Format("Could not open Soap Service configuration from {0}/config.ashx.{2}{1}", webServer, ex.Message, vbCrLf))
                    End Try

                    Try
                        MapPath = .SelectSingleNode("response").Attributes("SoapPath").Value.ToString
                    Catch ex As Exception
                        _NoIIS = True
                        Throw New Exception(String.Format("Invalid configuration file at {0}/config.ashx.", webServer))
                    End Try

                End With

                ' Load the web.config into memory 
                Try
                    webConfig.Load(String.Format("{0}web.config", MapPath))
                    If IsNothing(webConfig.SelectSingleNode("configuration/appSettings")) Then Throw New Exception(String.Format("Invalid Web Configuration file {0}web.config", MapPath))
                Catch ex As Exception                    
                    Throw New Exception(String.Format("Could not open {0}web.config", MapPath))
                End Try

                ' Create the bubble folders
                Dim dir As DirectoryInfo
                For Each folder As tBubbleFolder In enumBubbleFolder
                    dir = New DirectoryInfo(String.Format("{0}{1}", MapPath, BubbleFolder(folder)))
                    With dir
                        If Not .Exists Then
                            Try
                                .Create()
                            Catch ex As Exception
                                Throw New Exception(String.Format("Cannot create bubble folder at {0}{1}.", MapPath, BubbleFolder(folder)))
                            End Try
                        End If

                        ' Set permissions on the Bubble folders
                        Try
                            Dim dSecurity As DirectorySecurity = .GetAccessControl()
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
                            .SetAccessControl(dSecurity)
                        Catch ex As Exception
                            Throw New Exception(String.Format("Cannot set MODIFY permission on folder {0}{1}.", MapPath, BubbleFolder(folder)))
                        End Try
                    End With
                Next

                Try
                    Dim webConfigFile As New FileInfo(String.Format("{0}/web.config", MapPath))
                    With webConfigFile
                        Dim fSecurity As FileSecurity = webConfigFile.GetAccessControl
                        With fSecurity
                            .AddAccessRule( _
                                New FileSystemAccessRule( _
                                "everyone", _
                                FileSystemRights.Modify, _
                                Nothing, _
                                PropagationFlags.InheritOnly, _
                                AccessControlType.Allow _
                                ) _
                            )
                            .SetAccessRuleProtection(True, True)
                        End With
                        .SetAccessControl(fSecurity)
                    End With
                Catch ex As Exception
                    Throw New Exception(String.Format("Cannot set MODIFY permission on folder {0}/web.config. {1}", MapPath, ex.Message))
                End Try

            End If
            Return MapPath
        End Get
    End Property

    Public Property ApplicationSetting(ByVal Name As String, Optional ByVal defaultValue As String = "") As String
        Get
            With webConfig
                Try
                    Return .SelectSingleNode( _
                        String.Format( _
                            "configuration/appSettings/add[@key='{0}']", _
                            Name _
                        ) _
                    ).Attributes("value").Value
                Catch
                    Try
                        .SelectSingleNode("configuration/appSettings").AppendChild(NewApplicationSetting(Name, defaultValue))
                        .Save(String.Format("{0}\web.config", iisFolder))
                        Return defaultValue
                    Catch ex As Exception
                        Throw New Exception(String.Format("Failed writing DSN string to {0}/web.config.", iisFolder))
                    End Try
                End Try
            End With
        End Get
        Set(ByVal Value As String)
            Try
                With webConfig
                    If IsNothing( _
                        .SelectSingleNode( _
                            String.Format( _
                                "configuration/appSettings/add[@key='{0}']", _
                                Name _
                            ) _
                        ) _
                    ) Then
                        .SelectSingleNode("configuration/appSettings").AppendChild(NewApplicationSetting(Name, Value))
                        .Save(String.Format("{0}\web.config", iisFolder))
                    Else
                        If Not .SelectSingleNode( _
                            String.Format( _
                                "configuration/appSettings/add[@key='{0}']", _
                                Name _
                            ) _
                        ).Attributes("value").Value = Value Then
                            .SelectSingleNode( _
                                String.Format( _
                                    "configuration/appSettings/add[@key='{0}']", _
                                    Name _
                                ) _
                            ).Attributes("value").Value = Value
                            .Save(String.Format("{0}\web.config", iisFolder))
                        End If
                    End If                    
                End With
            Catch ex As Exception
                Throw New Exception( _
                    String.Format( _
                        "Failed writing key [{0}] value [{1}] to application settings in [{2}\web.config]. " & _
                        "Thrown error was [{3}]", _
                        Name, _
                        Value, _
                        iisFolder, _
                        ex.Message _
                    ) _
                )
            End Try
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Function NewApplicationSetting(ByVal Key As String, Optional ByVal Value As String = "") As XmlNode
        Dim NewNode As XmlNode = webConfig.CreateElement("add")
        With NewNode
            .Attributes.Append(webConfig.CreateAttribute("key"))
            .Attributes.Append(webConfig.CreateAttribute("value"))
            .Attributes("key").Value = Key
            .Attributes("value").Value = Value
        End With
        Return NewNode
    End Function

#End Region

#Region "Web.Config Watcher"

    Private _WatchingConfig As Boolean = False
    Public ReadOnly Property WatchingConfig() As Boolean
        Get
            Return _WatchingConfig
        End Get
    End Property

    Sub BeginWatchWebConfig()
        Try
            WatchWebconfig = New System.IO.FileSystemWatcher(iisFolder, "web.config")
            WatchWebconfig.EnableRaisingEvents = True
            _WatchingConfig = True
        Catch ex As Exception
            Throw New Exception(String.Format("Could begin watching {0}web.config. Thrown error was {1}", iisFolder, ex.Message))
        End Try
    End Sub

    Private Sub webconfig_Updated(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs) Handles WatchWebconfig.Changed
        Try
            webConfig.Load(String.Format("{0}web.config", iisFolder))
        Catch
        End Try
    End Sub

#End Region

End Module

