Imports System
Imports System.IO
Imports System.Threading
Imports System.Diagnostics

Public MustInherit Class BaseForm
    Inherits System.Windows.Forms.Form

    Private qLock As Queue

    Public rss(9999) As PDAOnBoardData.PDAData
    Private q As PDAOnBoardData.PDAData
    Private ar As New Priority.MyArray
    Private sd As New Priority.SerialData
    Private evts() As Integer

#Region "private variables"

    Private _SavePath As String = ""
    Private _PostPath As String = ""
    Private _SentPath As String = ""
    Private LastEvent As Integer = -1
    Private qing As Boolean = False

#End Region

#Region "public Properties"

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
    Public MustOverride Sub hNewData(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData)
    Public MustOverride Sub hWarning(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData, ByVal SubName As String, ByVal Message As String, ByVal infoOnly As Boolean)
    Public MustOverride Sub hCancelWarning(ByVal EventIndex As Integer)
    Public MustOverride Sub hFormRedraw(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData)
    Public MustOverride Sub hSigSaved(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData, ByVal Ordinal As Integer)

#End Region

#Region "Initialisation and Finalisation"

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'BaseForm
        '
        Me.ClientSize = New System.Drawing.Size(292, 273)
        Me.Name = "BaseForm"
        Me.ResumeLayout(False)

    End Sub

    Private Sub BaseForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        qLock = New Queue()

        q = New q
        With q
            .SavePath = System.AppDomain.CurrentDomain.BaseDirectory()
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

    Public Sub AddRSS(ByVal i As Integer, ByVal otype As PDAOnBoardData.PDAData)

        rss(i) = otype
        With rss(i)
            .SavePath = _SavePath
            .PostPath = _PostPath
            .Load()
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
