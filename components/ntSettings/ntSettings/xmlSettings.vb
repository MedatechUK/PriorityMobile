Imports System
Imports System.xml
Imports System.io

Public Class xmlSettings

    Public Sub New()

        Dim xd As New XmlDocument

        Using sr As New StreamReader(BasePath & "settings.xml")
            xd.LoadXml(sr.ReadToEnd)
            sr.Close()
        End Using

        For Each n As XmlNode In xd.ChildNodes
            Select Case LCase(n.Name)
                Case "settings"
                    For Each no As XmlNode In n.ChildNodes
                        Select Case LCase(no.Name)
                            Case "setting"
                                Dim na As String = Nothing
                                Dim va As String = ""
                                For Each at As XmlAttribute In no.Attributes
                                    Select Case LCase(at.Name)
                                        Case "name"
                                            na = at.Value
                                        Case "value"
                                            va = at.Value
                                    End Select
                                Next
                                If Not IsNothing(na) Then
                                    Setting.Add(na, va)
                                End If
                        End Select
                    Next
            End Select
        Next
    End Sub

    Private _Setting As New Dictionary(Of String, String)
    Public Property Setting() As Dictionary(Of String, String)
        Get
            Return _Setting
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _Setting = value
        End Set
    End Property

    Private _BasePath As String = Nothing
    Private ReadOnly Property BasePath() As String
        Get
            If IsNothing(_BasePath) Then
                Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
                If InStr(fullPath, "file:///", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "file:///", "")
                End If
                If InStr(fullPath, "/", CompareMethod.Text) > 0 Then
                    fullPath = Replace(fullPath, "/", "\")
                End If
                _BasePath = fullPath.Substring(0, fullPath.LastIndexOf("\"))
                If Strings.Right(_BasePath, 1) <> "\" Then _BasePath += "\"

            End If
            Return _BasePath
        End Get
    End Property

    Public Sub SaveSettings()

        Dim sw As New StreamWriter(BasePath & "settings.xml")
        Dim writer As New XmlTextWriter(sw)
        With writer
            .WriteStartDocument()
            .WriteStartElement("Settings", "")
            For Each k As String In Setting.Keys
                .WriteStartElement("Setting", "")
                .WriteStartAttribute("name", "")
                .WriteString(k)
                .WriteEndAttribute()
                .WriteStartAttribute("value", "")
                .WriteString(Setting(k))
                .WriteEndAttribute()
                .WriteEndElement()
            Next
            .WriteEndElement()
            .WriteEndDocument()            
        End With
        sw.Close()
    End Sub

End Class
