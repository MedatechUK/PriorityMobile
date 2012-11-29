Imports System
Imports System.IO
Imports System.Threading

Public MustInherit Class BaseForm
    Inherits System.Windows.Forms.Form

    Private qLock As Queue

    Public mySettings As cfSettings
    Public rss(9999) As cfOnBoardData.PDAData
    Private q As cfOnBoardData.PDAData
    Private ar As New cfMyCls.MyArray
    Private sd As New cfLoadData.SerialData
    Public thisRandom As New cfMyCls.MyRandom

    Private evts() As Integer

#Region "private variables"

    Private _myBasePath As String = ""
    Private _SavePath As String = ""
    Private _PostPath As String = ""
    Private _SentPath As String = ""
    Private LastEvent As Integer = -1
    Private qing As Boolean = False

#End Region

#Region "public Properties"

    Public Property myBasePath() As String
        Get
            Return _myBasePath
        End Get
        Set(ByVal value As String)
            _myBasePath = value
        End Set
    End Property

    Public Property SavePath() As String
        Get
            Return _SavePath
        End Get
        Set(ByVal value As String)
            If Not Strings.Right(value, 1) = "\" Then value = value & "\"
            _SavePath = value
        End Set
    End Property

    Public Property PostPath() As String
        Get
            Return _PostPath
        End Get
        Set(ByVal value As String)
            If Not Strings.Right(value, 1) = "\" Then value = value & "\"
            _PostPath = value
        End Set
    End Property

    Public Property SentPath() As String
        Get
            Return _SentPath
        End Get
        Set(ByVal value As String)
            If Not Strings.Right(value, 1) = "\" Then value = value & "\"
            _SentPath = value
        End Set
    End Property

#End Region

#Region "Must Override Subs"

    Public MustOverride Function hSend(ByVal LoadData As String) As Boolean
    Public MustOverride Sub hNewData(ByVal EventIndex As Integer, ByVal o As cfOnBoardData.PDAData)
    Public MustOverride Sub hWarning(ByVal EventIndex As Integer, ByVal o As cfOnBoardData.PDAData, ByVal SubName As String, ByVal Message As String, ByVal infoOnly As Boolean)
    Public MustOverride Sub hCancelWarning(ByVal EventIndex As Integer)
    Public MustOverride Sub hFormRedraw(ByVal EventIndex As Integer, ByVal o As cfOnBoardData.PDAData)
    Public MustOverride Sub hSigSaved(ByVal EventIndex As Integer, ByVal o As cfOnBoardData.PDAData, ByVal Ordinal As Integer)

#End Region

#Region "Initialisation and Finalisation"

    Private Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

    Private Sub BaseForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Set paths
        _myBasePath = Get_app_path()
        SavePath = _myBasePath & "\DATA"
        PostPath = _myBasePath & "\MAIL\OUTBOX"
        SentPath = _myBasePath & "\MAIL\SENT"

        qLock = New Queue()

        q = New q
        With q
            .SavePath = _myBasePath
            .Load()
        End With

        ' Create new q thread 
        Dim myThread As Thread
        myThread = New Thread(New ThreadStart(AddressOf doq))
        qing = True
        myThread.Start()

    End Sub

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        ' Close running threads
        q.appExit = True
        For i As Integer = 0 To UBound(rss)
            With rss(i)
                If Not IsNothing(rss(i)) Then
                    .Save()
                    .appExit = True
                End If
            End With
        Next
    End Sub

#End Region

#Region "Public functions"

    Public Function NewEvent(ByVal EventIndex As Integer) As Boolean

        If Not IsNothing(evts) Then
            For i As Integer = 0 To UBound(evts)
                If evts(i) = EventIndex Then
                    Return False
                End If
            Next
        End If

        Try
            ReDim Preserve evts(UBound(evts) + 1)
        Catch
            ReDim evts(0)
        End Try
        evts(UBound(evts)) = EventIndex

        Return True

    End Function

    Public Sub AddRSS(ByVal i As Integer, ByVal otype As cfOnBoardData.PDAData)

        rss(i) = otype
        With rss(i)
            .SavePath = _SavePath
            .PostPath = _PostPath
            .Load()
            .SetBaseForm(Me)
            AddHandler .NewData, AddressOf hNewData
            AddHandler .Warning, AddressOf hWarning
            AddHandler .CancelWarning, AddressOf hCancelWarning
            AddHandler .FormRedraw, AddressOf hFormRedraw
            AddHandler .SigSaved, AddressOf hSigSaved
            AddHandler .EnQueueLoading, AddressOf hEnQueueLoading
        End With

    End Sub

#End Region

#Region "Send Loading"

    Private Sub hEnQueueLoading(ByVal EventIndex As Integer, ByVal Loadfile As String)
        If NewEvent(EventIndex) Then
            Monitor.Enter(qLock)
            With q
                Dim i As Integer = q.NewRecord
                .thisArray(0, i) = Loadfile
                .Save()
                If Not qing Then
                    qing = True
                    ' Create new q thread 
                    Dim myThread As Thread
                    myThread = New Thread(New ThreadStart(AddressOf doq))
                    myThread.Start()
                End If
            End With
            Monitor.Exit(qLock)
        End If
    End Sub

    Private Sub doq()

        Dim data(,) As String

        q.currentOrdinal = 0

        While q.currentOrdinal > -1 And Not (q.appExit)
            data = Nothing

            If ar.ArrayFromFile(data, q.GetField("filename")) Then
                If hSend(sd.SerialiseDataArray(data)) Then
                    Dim f() As String = Split(q.GetField("filename"), "\")
                    Try
                        File.Move(q.GetField("filename"), _
                        _SentPath & f(UBound(f)))
                    Catch
                    End Try
                    q.DeleteIndex()
                Else
                    For i As Integer = 0 To 100
                        Thread.Sleep(10)
                        If q.appExit Then Exit For
                    Next
                End If
            Else
                q.DeleteIndex()
            End If

            q.currentOrdinal = 0

        End While
        qing = False

    End Sub

#End Region

End Class
