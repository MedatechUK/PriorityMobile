Imports Microsoft.VisualBasic
Imports system.threading
Imports System.Collections.Generic
Imports System.Xml

Partial Public MustInherit Class iSettings
    Inherits System.Web.UI.Page

#Region "Public Properties"

    Public ReadOnly Property AppName() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("appName")
        End Get
    End Property

    Public ReadOnly Property LoadingTimeout() As Integer
        Get
            Try
                Return System.Configuration.ConfigurationManager.AppSettings.Get("LoadingTimeout")
            Catch
                Return 60
            End Try
        End Get
    End Property

    Public ReadOnly Property Environment() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("Environment")
        End Get
    End Property

    Public ReadOnly Property LogVerbosity() As Integer
        Get
            Try
                Return CInt(System.Configuration.ConfigurationManager.AppSettings.Get("LogVerbosity"))
            Catch
                Return 99
            End Try
        End Get
    End Property

    Public ReadOnly Property RemoteIP() As String
        Get
            Return HttpContext.Current.Request.UserHostAddress.ToString
        End Get
    End Property

    Private _webConfig As XmlDocument
    Public ReadOnly Property webConfig() As XmlDocument
        Get
            If IsNothing(_webConfig) Then
                _webConfig = New XmlDocument
                _webConfig.Load(webConfigPath)
            End If
            Return _webConfig
        End Get
    End Property

    Public ReadOnly Property webConfigPath() As String
        Get
            Return String.Format("{0}web.config", Server.MapPath("/"))
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
                        .SelectSingleNode("configuration/appSettings").AppendChild(NewApplicationSetting(webConfig, Name, defaultValue))
                        .Save(String.Format("{0}\web.config", webConfigPath))
                        Return defaultValue
                    Catch ex As Exception
                        Throw New Exception(String.Format("Failed to update {0}. {1}", webConfigPath, ex.Message))
                    End Try
                End Try
            End With
        End Get

        Set(ByVal Value As String)

            With webConfig
                Try
                    If IsNothing( _
                        .SelectSingleNode( _
                            String.Format( _
                                "configuration/appSettings/add[@key='{0}']", _
                                Name _
                            ) _
                        ) _
                    ) Then
                        .SelectSingleNode("configuration/appSettings").AppendChild(NewApplicationSetting(webConfig, Name, Value))
                        .Save(webConfigPath)
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
                            .Save(webConfigPath)
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception( _
                        String.Format( _
                            "Failed writing key [{0}] value [{1}] to application settings in [{2}]. " & _
                            "Thrown error was [{3}]", _
                            Name, _
                            Value, _
                            webConfigPath, _
                            ex.Message _
                        ) _
                    )
                End Try
            End With
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Function NewApplicationSetting(ByVal webConfig As XmlDocument, ByVal Key As String, Optional ByVal Value As String = "") As XmlNode
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

End Class
