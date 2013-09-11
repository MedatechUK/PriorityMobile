Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Collections.Generic
Imports System.Xml

<WebService(Namespace:="http://priwebsvc.ntsa.org.uk/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService

#Region "Private Properties"

    Private ReadOnly Property StarredPassword(ByVal ConnectionString As String) As String
        Get
            Dim dict As New Dictionary(Of String, String)
            Dim bStr As New System.Text.StringBuilder
            For Each pair As String In ConnectionString.Split(";")
                If pair.Length > 0 Then dict.Add(pair.Split("=")(0), pair.Split("=")(1))
            Next
            For Each key As String In dict.Keys
                If String.Compare(key, "password", True) = 0 Then
                    bStr.AppendFormat("{0}=******;", key)
                ElseIf String.Compare(key, "pwd", True) = 0 Then
                    bStr.AppendFormat("{0}=******;", key)
                Else
                    bStr.AppendFormat("{0}={1};", key, dict(key))
                End If
            Next
            Return bStr.ToString
        End Get
    End Property

    Private ReadOnly Property AppName() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("appName")
        End Get
    End Property

    Private ReadOnly Property ConnectionString(Optional ByVal UseEnvironment As String = Nothing) As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("DSN")
        End Get
    End Property

    Private ReadOnly Property Provider() As Integer
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("PROVIDER")
        End Get
    End Property

    Private ReadOnly Property Environment() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings.Get("Environment")
        End Get
    End Property

    Private ReadOnly Property LogVerbosity() As Integer
        Get
            Try
                Return CInt(System.Configuration.ConfigurationManager.AppSettings.Get("LogVerbosity"))
            Catch
                Return 99
            End Try
        End Get
    End Property

    Private ReadOnly Property RemoteIP() As String
        Get
            Return HttpContext.Current.Request.UserHostAddress.ToString
        End Get
    End Property

#End Region

    <WebMethod()> _
    Public Function LoadData(ByVal SerialData As String) As Boolean

        Dim ret As Boolean = True
        Dim ev As New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, LogVerbosity, AppName)
        Dim BubbleID As String = System.Guid.NewGuid.ToString
        Try
            Using sw As New StreamWriter(String.Format("{0}\queue\{1}.txt", Server.MapPath("/"), BubbleID))
                sw.Write(SerialData.Replace("\n", vbCrLf).Replace("\t", Chr(9)))
                ev.Log( _
                    String.Format( _
                        "Saved Bubble Data from [{1}] to [{0}].", _
                        String.Format("{0}queue\{1}.txt", Server.MapPath("/"), BubbleID), _
                        RemoteIP _
                    ), _
                    EventLogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End Using
        Catch ex As Exception
            ev.Log( _
                String.Format( _
                    "An error occured saving Bubble Data from [{2}] to [{0}]. {1}", _
                    String.Format("{0}\queue\{1}.txt", Server.MapPath("/"), BubbleID), _
                    ex.Message, _
                    RemoteIP _
                ), _
                EventLogEntryType.FailureAudit, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            ret = False
        End Try
        Return ret

    End Function

    <WebMethod()> _
    Public Function GetData(ByVal SQL As String, ByVal Env As String) As String

        Dim command As GenericCommand = Nothing
        Dim ret As System.Text.StringBuilder
        Dim ev As New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, LogVerbosity, AppName)

        ret = New System.Text.StringBuilder
        Try
            Using Connection As New GenericConnection(Provider)
                Connection.ConnectionString = ConnectionString

                If Env.Length = 0 Then Env = Environment

                Connection.Open()
                command = Connection.CreateCommand()

                Select Case Provider
                    Case 1
                        command.CommandText = String.Format("use [{0}]; {1}", Env, SQL)
                    Case 2
                        command.CommandText = String.Format("{1}", Env, SQL)
                End Select

                Dim dataReader As GenericDataReader = _
                             command.ExecuteReader()

                If dataReader.HasRows Then

                    Dim args() As String = Nothing
                    Dim Format As New System.Text.StringBuilder

                    dataReader.Read()
                    Do
                        If IsNothing(args) Then
                            ReDim args(dataReader.FieldCount - 1)
                            For i As Integer = 0 To UBound(args) - 1
                                Format.Append("{").Append(i.ToString).Append("}").Append("\t")
                            Next
                            Format.Append("{").Append(UBound(args).ToString).Append("}")
                        Else
                            ret.Append("\n")
                        End If

                        For i As Integer = 0 To UBound(args)
                            args(i) = dataReader.Item(i).ToString
                        Next
                        ret.AppendFormat(Format.ToString, args)

                    Loop While dataReader.Read()

                End If

                ev.Log( _
                    String.Format( _
                        "Ran SQL Statement for client [{4}].{0}" & _
                        "The sql query was [{2}].", _
                        vbCrLf, _
                        "", _
                        command.CommandText, _
                        ConnectionString, _
                        RemoteIP _
                    ), _
                    EventLogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End Using

        Catch ex As Exception
            ret = New System.Text.StringBuilder
            ret.AppendFormat("!{0}", ex.Message)
            If IsNothing(command) Then
                ev.Log( _
                    String.Format( _
                        "An error occured while processing an SQL Statement for client [{3}].{0}" & _
                        "The connection string used was: [{2}].{0}" & _
                        "The error message was [{1}].", _
                        vbCrLf, _
                        ex.Message, _
                        StarredPassword(Me.ConnectionString), _
                        RemoteIP _
                    ), _
                    EventLogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
            Else
                ev.Log( _
                    String.Format( _
                        "An error occured while processing an SQL Statement for client [{4}].{0}" & _
                        "The sql query was [{2}].{0}The connection string used was: [{3}].{0}" & _
                        "The error message was [{1}].", _
                        vbCrLf, _
                        ex.Message, _
                        command.CommandText, _
                        StarredPassword(Me.ConnectionString), _
                        RemoteIP _
                    ), _
                    EventLogEntryType.FailureAudit, _
                    ntEvtlog.EvtLogVerbosity.Normal _
                )
            End If

        End Try

        Return ret.ToString

    End Function

    <WebMethod()> _
    Public Function SaveSignature(ByVal SerialData As String) As String

        Dim ev As New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, LogVerbosity, AppName)
        Dim fn As String = System.Guid.NewGuid.ToString & ".JPG"

        Dim objGraphics As Graphics
        Dim X1 As Integer = 0
        Dim X2 As Integer = 0
        Dim Y1 As Integer = 0
        Dim Y2 As Integer = 0
        Dim maxX As Integer = 0
        Dim maxY As Integer = 0

        Dim sd As New priority.SerialData
        With sd
            .FromStr(SerialData)
            For i As Integer = 0 To UBound(.Data, 2)
                If .Data(0, i) > maxX Then
                    maxX = .Data(0, i)
                End If
                If .Data(1, i) > maxY Then
                    maxY = .Data(1, i)
                End If
            Next

            Dim b As Bitmap = New System.Drawing.Bitmap(maxX, maxY, PixelFormat.Format24bppRgb)
            objGraphics = Graphics.FromImage(b)
            objGraphics.Clear(Color.White)

            For i As Integer = 1 To UBound(.Data, 2)
                Try
                    X1 = CInt(.Data(0, i - 1))
                    Y1 = CInt(.Data(1, i - 1))
                    X2 = CInt(.Data(0, i))
                    Y2 = CInt(.Data(1, i))

                    If Not (X1 = 0 And Y1 = 0) And Not (X2 = 0 And Y2 = 0) Then
                        objGraphics.DrawLine(Pens.Black, X1, Y1, X2, Y2)
                    End If
                Catch

                End Try
            Next

            b.Save(String.Format("{0}signatures/{1}", Server.MapPath("/"), fn), ImageFormat.Jpeg)
            b.Dispose()
            objGraphics.Dispose()

            ev.Log( _
                String.Format( _
                    "Saved new signature as [{0}] for client at [{1}].", _
                    fn, _
                    RemoteIP _
                ), _
                EventLogEntryType.SuccessAudit, _
                ntEvtlog.EvtLogVerbosity.VeryVerbose _
            )

            Return fn

        End With

    End Function

    <WebMethod()> _
    Public Function RunProc(ByVal ProcName As String) As Boolean
        Dim ret As Boolean = True
        Dim ev As New ntEvtlog.evt(ntEvtlog.EvtLogMode.EventLog, LogVerbosity, AppName)
        Dim BubbleID As String = System.Guid.NewGuid.ToString
        Try
            ' Create XmlWriterSettings.
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = True

            ' Create XmlWriter.
            Using writer As XmlWriter = XmlWriter.Create(String.Format("{0}\queue\{1}.xml", Server.MapPath("/"), BubbleID), settings)
                With writer
                    ' Begin writing.
                    .WriteStartDocument()
                    .WriteStartElement("PriorityLoading") ' Root.
                    .WriteAttributeString("PROCEDURE", ProcName)
                    .WriteEndElement()
                    .WriteEndDocument()
                End With
                ev.Log( _
                    String.Format( _
                        "Saved Bubble Data from [{1}] to [{0}].", _
                        String.Format("{0}queue\{1}.txt", Server.MapPath("/"), BubbleID), _
                        RemoteIP _
                    ), _
                    EventLogEntryType.SuccessAudit, _
                    ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )
            End Using

        Catch ex As Exception
            ev.Log( _
                String.Format( _
                    "An error occured saving Bubble Data from [{2}] to [{0}]. {1}", _
                    String.Format("{0}\queue\{1}.txt", Server.MapPath("/"), BubbleID), _
                    ex.Message, _
                    RemoteIP _
                ), _
                EventLogEntryType.FailureAudit, _
                ntEvtlog.EvtLogVerbosity.Normal _
            )
            ret = False
        End Try
        Return ret
    End Function

End Class
