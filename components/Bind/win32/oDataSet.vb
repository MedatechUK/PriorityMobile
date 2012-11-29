Imports System.Threading
Imports System.Reflection
Imports System.IO
Imports loading
Imports ntEvtlog

Public Class oDataSet
    Public ws As New PriWebSVC.Service
    Public evt As ntEvtlog.evt = Nothing
    Public WithEvents svc As Loading.plc
    Private closing As Boolean = False

    Public Event Refresh()

    Public Event BeginLoadType(ByVal LoadType As String, ByVal Max As Integer)
    Public Event LoadType(ByVal TypeName As String)

    Public Event GetInterfaceValue(ByVal Name As String, ByRef value As String)
    Public Sub hGetInterfaceValue(ByVal Name As String, ByRef value As String)
        RaiseEvent GetInterfaceValue(Name, value)
    End Sub

    Public Event SetInterfaceValue(ByVal Name As String, ByVal value As String)
    Public Sub hSetInterfaceValue(ByVal Name As String, ByVal value As String)
        RaiseEvent SetInterfaceValue(Name, value)
    End Sub

#Region "Public properties"
    Private _UseEnvironment As String = ""
    Public Property UseEnvironment() As String
        Get
            Return _UseEnvironment
        End Get
        Set(ByVal value As String)
            _UseEnvironment = value
        End Set
    End Property
    Private _DataSet As New Dictionary(Of String, Object)
    Public Property DataSet() As Dictionary(Of String, Object)
        Get
            Return _DataSet
        End Get
        Set(ByVal value As Dictionary(Of String, Object))
            _DataSet = value
        End Set
    End Property
    Private _Triggers As New List(Of Object)
    Public Property Triggers() As List(Of Object)
        Get
            Return _Triggers
        End Get
        Set(ByVal value As List(Of Object))
            _Triggers = value
        End Set
    End Property
    Private _Sync As New List(Of oSync)
    Public Property Sync() As List(Of oSync)
        Get
            Return _Sync
        End Get
        Set(ByVal value As List(Of oSync))
            _Sync = value
        End Set
    End Property
    Private _BasePath As String = Nothing
    Public ReadOnly Property BasePath() As String
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
    Private _QueryConsts As New Dictionary(Of String, String)
    Public Property QueryConsts() As Dictionary(Of String, String)
        Get
            Return _QueryConsts
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _QueryConsts = value
        End Set
    End Property
#End Region
#Region "Subs"
#Region "Private Subs"
    Private Sub mkNEDir(ByVal dir As String)
        Dim f As New IO.DirectoryInfo(BasePath & dir & "\")
        If Not f.Exists Then
            IO.Directory.CreateDirectory(BasePath & dir)
        End If
    End Sub
#End Region
#Region "initialisation and finalistion"
    Public Sub init(ByVal dllName As String, Optional ByVal AppName As String = "undefined", Optional ByRef Evt As ntEvtLog.evt = Nothing)
        Dim maxVal As New Dictionary(Of String, Integer)
        If File.Exists(BasePath & dllName) Then
            ' Check Paths Exist
            mkNEDir("MAIL")
            mkNEDir("MAIL/SENT")
            mkNEDir("MAIL/OUTBOX")
            mkNEDir("DATA")
            svc = New Loading.plc(AppName, Evt)
            Me.evt = Evt
            Dim TargetAssembly As Assembly
            TargetAssembly = Assembly.LoadFrom(BasePath & dllName)

            For Each mi As System.Type In TargetAssembly.GetTypes
                If Not maxVal.ContainsKey(mi.BaseType.Name) Then maxVal.Add(mi.BaseType.Name, 0)
                maxVal(mi.BaseType.Name) += 1
            Next

            RaiseEvent BeginLoadType("DatasetObjectBase", maxVal("DatasetObjectBase"))
            For Each mi As System.Type In TargetAssembly.GetTypes
                Select Case mi.BaseType.Name
                    Case "DatasetObjectBase"
                        DataSet.Add(mi.Name, New oBind(Me, BasePath & dllName, mi.FullName))
                        Dim b As oBind = DataSet(mi.Name)
                        AddHandler b.Refresh, AddressOf hRefresh
                        RaiseEvent LoadType(mi.Name)
                End Select
            Next

            RaiseEvent BeginLoadType("TriggerBase", maxVal("TriggerBase"))
            For Each mi As System.Type In TargetAssembly.GetTypes
                Select Case mi.BaseType.Name
                    Case "TriggerBase"
                        Triggers.Add(New oTrigger(Me, BasePath & dllName, mi.FullName))
                        RaiseEvent LoadType(mi.Name)
                End Select
            Next

            RaiseEvent BeginLoadType("SyncBase", maxVal("SyncBase"))
            For Each mi As System.Type In TargetAssembly.GetTypes
                Select Case mi.BaseType.Name
                    Case "SyncBase"
                        Sync.Add(New oSync(Me, BasePath & dllName, mi.FullName))
                        RaiseEvent LoadType(mi.Name)
                End Select
            Next

            For Each tr As oTrigger In Triggers
                For Each ob As Object In DataSet.Values
                    'String.Compare(ob.ToString, tr.ToString, True) = 0
                    If ob.Equals(tr) Then
                        Dim t() As System.Type = {GetType(oTrigger)}
                        Dim args() As Object = {tr}
                        GetType(oBind).GetMethod("AddTrigger", t).Invoke(ob, args)
                    End If
                Next
            Next
            ' Create new event thread 
            Dim myThread As Thread
            myThread = New Thread(New ThreadStart(AddressOf DoQEvents))
            'myThread.Start()
        End If
    End Sub
    Private Sub hRefresh()
        RaiseEvent Refresh()
    End Sub
    Protected Overrides Sub Finalize()
        closing = True
        MyBase.Finalize()
    End Sub
#End Region
#Region "q events"
    Public Sub DoQEvents()
        Do
            Dim snt As Boolean = False
            Do
                Try
                    Dim s As String = _
                        svc.SendBubble(Nothing, RunMode.ReadQ)
                    snt = True
                Catch ex As Exception
                    MsgBox(ex.Message)
                    If Not IsNothing(evt) Then
                        SafeLog(ex.Message, LogEntryType.Err, EvtLogVerbosity.Normal)
                    End If
                End Try
            Loop Until snt
            For i As Integer = 0 To 1200
                Thread.Sleep(100)
            Next
        Loop While Not closing
    End Sub
    Private Sub hEvent(ByVal BUBBLEID As String, _
        ByVal SOAPPROC As String, _
        ByVal RESULT As Boolean, _
        ByVal DATA As String) Handles svc.hEvent

    End Sub
#End Region
#Region "Logging"
    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As ntEvtlog.LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            If Not IsNothing(evt) Then
                evt.Log( _
                    Entry, _
                    EventType, _
                    Verbosity _
                  )
            End If
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                evt.LogName, evt.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub
#End Region
#End Region

End Class
