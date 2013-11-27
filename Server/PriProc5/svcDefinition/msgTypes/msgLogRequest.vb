Imports System.Xml

#Region "Enumerations"

Public Enum LogEntryType As Integer
    Err = 1
    Information = 4
    FailureAudit = 16
    SuccessAudit = 8
    Warning = 2
End Enum

Public Enum EvtLogVerbosity As Integer
    Normal = 1
    Verbose = 10
    VeryVerbose = 50
    Arcane = 99
End Enum

Public Enum EvtLogSource As Integer
    APPLICATION
    SYSTEM
End Enum

#End Region

Public Class msgLogRequest : Inherits svcRequest

#Region "Message Properies"

    Private _svcType As String
    Public Property svcType() As String
        Get
            Return _svcType
        End Get
        Set(ByVal value As String)
            _svcType = value
        End Set
    End Property

    Private _LogSource As EvtLogSource
    Public Property LogSource() As EvtLogSource
        Get
            Return _LogSource
        End Get
        Set(ByVal value As EvtLogSource)
            _LogSource = value
        End Set
    End Property

    Private _Verbosity As EvtLogVerbosity
    Public Property Verbosity() As EvtLogVerbosity
        Get
            Return _Verbosity
        End Get
        Set(ByVal value As EvtLogVerbosity)
            _Verbosity = value
        End Set
    End Property

    Private _EntryType As LogEntryType
    Public Property EntryType() As LogEntryType
        Get
            Return _EntryType
        End Get
        Set(ByVal value As LogEntryType)
            _EntryType = value
        End Set
    End Property

    Private _LogData As New System.Text.StringBuilder
    Public Property LogData() As System.Text.StringBuilder
        Get
            Return _LogData
        End Get
        Set(ByVal value As System.Text.StringBuilder)
            _LogData = value
        End Set
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New( _
        ByVal svcType As String, _
           Optional ByVal LogSource As EvtLogSource = EvtLogSource.Application, _
           Optional ByVal Verbosity As EvtLogVerbosity = EvtLogVerbosity.VeryVerbose, _
           Optional ByVal EntryType As LogEntryType = LogEntryType.Information _
        )

        TimeStamp = Now.ToString("dd/MM/yyyy hh:mm:ss")
        Source = Environment.MachineName        
        _svcType = svcType
        _LogSource = LogSource
        _Verbosity = Verbosity
        _EntryType = EntryType

    End Sub

    Public Sub New(ByRef Request As XmlNode)
        TimeStamp = Request.SelectSingleNode("timestamp").InnerText
        Source = Request.SelectSingleNode("source").InnerText
        _svcType = Request.SelectSingleNode("svctype").InnerText
        Select Case Request.SelectSingleNode("logSource").InnerText
            Case "application"
                _LogSource = EvtLogSource.Application
            Case Else
                _LogSource = EvtLogSource.System
        End Select
        _Verbosity = Request.SelectSingleNode("Verbosity").InnerText
        _EntryType = Request.SelectSingleNode("EntryType").InnerText
        _LogData.Append(Request.SelectSingleNode("LogData").InnerText)

    End Sub

#End Region

#Region "Overriden properties"

    Public Overrides ReadOnly Property msgType() As String
        Get
            Return "log"
        End Get
    End Property

    Private _Source As String
    Public Overrides Property Source() As String
        Get
            Return _Source
        End Get
        Set(ByVal value As String)
            _Source = value
        End Set
    End Property

    Private _TimeStamp As String
    Public Overrides Property TimeStamp() As String
        Get
            Return _TimeStamp
        End Get
        Set(ByVal value As String)
            _TimeStamp = value
        End Set
    End Property

#End Region

#Region "Overridden Methods"

    Public Overrides Sub writeXML(ByRef outputStream As System.Xml.XmlWriter)
        With outputStream
            .WriteElementString("svctype", _svcType)
            Select Case LogSource
                Case EvtLogSource.Application
                    .WriteElementString("logSource", "application")
                Case Else
                    .WriteElementString("logSource", "system")
            End Select
            .WriteElementString("Verbosity", _Verbosity)
            .WriteElementString("EntryType", _EntryType)
            .WriteElementString("LogData", _LogData.ToString)
        End With
    End Sub

#End Region

End Class
