Imports System
Imports System.IO
Imports System.Threading
Imports System.Diagnostics
Imports cfLoadData
Imports cfMyCls

Public MustInherit Class PDAData

#Region "Local Property Variables"

    Private myBaseForm As BaseForm

    Private loadLock As Queue

    Private _SigIndex As String
    Private _NewData As Boolean = False
    Private _Name As String = "New Dataset"
    Private _ConQuery As String = ""
    Private _appExit As Boolean = False
    Private _SavePath As String = "c:\"
    Private _PostPath As String = "c:\"
    Private _Seeking As Boolean = False
    Private _RedrawForm As Boolean = False
    Private _currentIndex As String = ""
    Shared _EventIndex As Integer = 0

#End Region

#Region "Local Variables"

    Private Caller As Object = Nothing

    Public ar As New cfMyCls.MyArray
    Public p As New cfLoadData.Load
    Public sd As New cfLoadData.SerialData

    Public thisArray(-1, -1) As String ' The live data
    Public tempArray(-1, -1) As String ' Used to hold downloaded data
    Public deletedArray(-1, -1) As String ' Records that have been deleted

    Public Column() As String

    Public CallerApp As cfOnBoardData.BaseForm

#End Region

#Region "Initialisation"

    Public Sub New()
        ReDim Column(999)
        loadLock = New Queue()
    End Sub

#End Region

#Region "Public Events"

    Public Shared Event NewData(ByVal EventIndex As Integer, ByVal o As PDAData)
    Public Shared Event FormRedraw(ByVal EventIndex As Integer, ByVal o As PDAData)
    Public Shared Event Warning(ByVal EventIndex As Integer, ByVal o As PDAData, ByVal SubName As String, ByVal Message As String, ByVal infoOnly As Boolean)
    Public Shared Event CancelWarning(ByVal EventIndex As Integer)
    Public Shared Event SigSaved(ByVal EventIndex As Integer, ByVal o As PDAData, ByVal Ordinal As Integer)
    Public Shared Event EnQueueLoading(ByVal EventIndex As Integer, ByVal Loadfile As String)

#End Region

#Region "Overridable Subs"

    Public MustOverride Function ConWebService(ByRef data) As Boolean
    Public MustOverride Sub LoadData(ByVal Ordinal As Integer)
    Public MustOverride Sub ConFail(ByRef Cancel As Boolean)
    Public MustOverride Sub SyncNewData()
    Public MustOverride Function Validate() As Boolean

    Public Overridable Sub ChangeSettings()

    End Sub

    Public Overridable Function Selection() As Integer()
        Dim con(1, 0) As String
        con(0, 0) = "0"
        con(1, 0) = currentIndex
        Return ar.Matching(thisArray, con)
    End Function

    Public Overridable Function SaveSig(ByVal Data As String, ByRef Response As String) As Boolean
        Return ""
    End Function

#End Region

#Region "Public Property Declarations"

    Public Property HasNewData() As Boolean
        Get
            HasNewData = _NewData
        End Get
        Set(ByVal value As Boolean)
            _NewData = value
        End Set
    End Property

    Public Property SigIndex() As String
        Get
            SigIndex = _SigIndex
        End Get
        Set(ByVal value As String)
            _SigIndex = value
        End Set
    End Property

    Public Property Name() As Object
        Get
            Name = _Name
        End Get
        Set(ByVal value As Object)
            _Name = value
        End Set
    End Property

    Public ReadOnly Property EventIndex() As Integer
        Get
            _EventIndex = _EventIndex + 1
            Return _EventIndex
        End Get
    End Property

    Public ReadOnly Property ColumnCount() As Integer
        Get
            For n As Integer = UBound(Column) To 0 Step -1
                If Not IsNothing(Column(n)) Then
                    Return n
                End If
            Next
            Return -1
        End Get
    End Property

    Public Property ConQuery() As String
        Get
            Return _ConQuery
        End Get
        Set(ByVal value As String)
            _ConQuery = value
        End Set
    End Property

    Public Property SavePath() As String
        Get
            Return _SavePath
        End Get
        Set(ByVal value As String)
            If Right(value, 1) <> "\" Then value = value & "\"
            _SavePath = value
        End Set
    End Property

    Public Property PostPath() As String
        Get
            Return _PostPath
        End Get
        Set(ByVal value As String)
            If Right(value, 1) <> "\" Then value = value & "\"
            _PostPath = value
        End Set
    End Property

    Public Property appExit() As Boolean
        Get
            Return _appExit
        End Get
        Set(ByVal value As Boolean)
            _appExit = value
        End Set
    End Property

    Public ReadOnly Property Seeking() As Boolean
        Get
            Return _Seeking
        End Get
    End Property

    Public Property RedrawForm() As Boolean
        Get
            Return _RedrawForm
        End Get
        Set(ByVal value As Boolean)
            _RedrawForm = value
        End Set
    End Property

    Public Property currentIndex() As String
        Get
            Return _currentIndex
        End Get
        Set(ByVal value As String)
            _currentIndex = value
        End Set
    End Property

    Public Property currentOrdinal() As Integer
        Get
            Return ar.InArray(thisArray, 0, _currentIndex)
        End Get
        Set(ByVal value As Integer)
            Try
                currentIndex = thisArray(0, value)
            Catch
                currentIndex = ""
            End Try
        End Set
    End Property

#End Region

#Region "Public Procedures"

    Public Sub SetBaseForm(ByRef Basef As BaseForm)
        myBaseForm = Basef
    End Sub

    Public Function doInfo(ByVal subName As String, ByVal Message As String) As Integer
        Dim e As Integer = EventIndex
        RaiseEvent Warning(e, Me, subName, Message, True)
        Return e
    End Function

    Public Sub doWarning(ByVal subName As String, ByVal Message As String)
        RaiseEvent Warning(EventIndex, Me, subName, Message, False)
    End Sub

#Region "Get / Set Data"

    Public Function GetField(ByVal ColumnName As String) As String

        Dim subName As String = "GetField" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            If currentOrdinal > -1 Then
                For x As Integer = 0 To UBound(Column)
                    If LCase(Trim(ColumnName)) = LCase(Trim(Column(x))) Then
                        Return thisArray(x, currentOrdinal)
                        Exit Function
                    End If
                Next
            Else
                doWarning(subName, "Get Fails. No record selected in " & Me.Name & ".")
            End If
            doWarning(subName, "Column [" & ColumnName & "] does not exist in " & Me.Name & ".")
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

        Return Nothing

    End Function

    Public Sub SetField(ByVal ColumnName As String, ByVal Value As String)

        Dim subName As String = "SetField" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            If currentOrdinal > -1 Then
                For x As Integer = 0 To UBound(Column)
                    If LCase(Trim(ColumnName)) = LCase(Trim(Column(x))) Then
                        thisArray(x, currentOrdinal) = Value
                        Exit Sub
                    End If
                Next
                doWarning(subName, "Column [" & ColumnName & "] does not exist in " & Me.Name & ".")
            Else
                doWarning(subName, "Set Fails. No record selected in " & Me.Name & ".")
            End If
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

#End Region

    Public Function NewRecord() As Integer

        Try
            ReDim Preserve thisArray(ColumnCount, UBound(thisArray, 2) + 1)
        Catch
            ReDim thisArray(ColumnCount, 0)
        End Try

        Return UBound(thisArray, 2)

    End Function

    Public Sub BeginSeek(ByVal o As Object)

        Dim subName As String = "BeginSeek" 'StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            If Not Seeking Then
                _Seeking = True

                ' Create new Seeking thread 
                Dim myThread As Thread
                Caller = o
                myThread = New Thread(New ThreadStart(AddressOf seekThread))
                myThread.Start()

            End If
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Sub BeginSigSave(ByVal o As Object)

        Dim subName As String = "BeginSigSave" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            If Not Seeking Then
                _Seeking = True

                ' Create new Seeking thread 
                Dim myThread As Thread
                Caller = o
                myThread = New Thread(New ThreadStart(AddressOf seekSig))
                myThread.Start()

            End If
        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

    Public Function doLoading(ByVal o As PDAData) As Boolean

        Monitor.Enter(loadLock)

        Dim ret As Boolean = False
        Dim subName As String = "doLoading" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Dim evID As Integer = doInfo(subName, "Do Loading")
        Try
            Dim i As Integer = currentOrdinal
            If Validate() Then
                LoadDeleted()
                o.LoadData(i)

                If p.Validate Then
                    Dim fn As String = MakeLDFN()
                    If ar.ArrayToFile(fn, p.Data) Then
                        p.Clear()
                        RaiseEvent EnQueueLoading(EventIndex, fn)
                        ret = True
                    Else
                        doWarning(subName, "Error saving " & fn & ".")
                        p.Clear()
                        ret = False
                    End If
                Else
                    p.Clear()
                    ret = True
                End If
            End If

            RaiseEvent CancelWarning(evID)
            Monitor.Exit(loadLock)
            Return ret

        Catch e As Exception
            RaiseEvent CancelWarning(evID)
            doWarning(subName, e.Message)
            Monitor.Exit(loadLock)
        End Try

    End Function

    Public Function Load() As Boolean

        Dim ret As Boolean = False
        Dim subName As String = "Load" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Dim evID As Integer = doInfo(subName, "Loading")
        Try
            Dim fn As String = MakeFN()
            If File.Exists(fn) Then
                ret = ar.ArrayFromFile(thisArray, MakeFN)
            Else
                thisArray = Nothing
                ret = True
            End If

            RaiseEvent CancelWarning(evID)
            Return ret

        Catch e As Exception
            RaiseEvent CancelWarning(evID)
            doWarning(subName, e.Message)
            Return False
        End Try

    End Function

    Public Function Save() As Boolean

        Dim ret As Boolean = False
        Dim subName As String = "Save" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Dim evID As Integer = doInfo(subName, "Saving")
        Try
            If ar.ArrayToFile(MakeFN, thisArray) Then
                ret = True
            Else
                ret = False
            End If

            RaiseEvent CancelWarning(evID)
            Return ret

        Catch e As Exception
            RaiseEvent CancelWarning(evID)
            doWarning(subName, e.Message)
        End Try

    End Function

    Public Function DeleteIndex() As Boolean

        Dim firstIns As Integer = 0
        Dim subName As String = "DeleteIndex" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            Dim d(0) As String
            Dim i() As Integer = Selection()

            If UBound(i) > -1 Then

                LoadDeleted()

                If IsNothing(deletedArray) Then
                    ReDim deletedArray(UBound(thisArray, 1), UBound(i))
                Else
                    If deletedArray.Length = 0 Then
                        ReDim deletedArray(UBound(thisArray, 1), UBound(i))
                    Else
                        Try
                            firstIns = UBound(deletedArray, 2) + 1
                            ReDim Preserve deletedArray(UBound(deletedArray, 1), UBound(deletedArray, 2) + UBound(i) + 1)
                        Catch ex As Exception
                            firstIns = 0
                            ReDim deletedArray(UBound(thisArray, 1), UBound(i))
                        End Try
                    End If
                End If

                For ub As Integer = 0 To UBound(i)
                    For x As Integer = 0 To UBound(thisArray, 1)
                        deletedArray(x, firstIns + ub) = thisArray(x, i(ub))
                    Next
                Next

                ' Save the updated deletion file
                If ar.ArrayToFile(MakeDelFN(), deletedArray) Then
                    ar.DeleteByIndex(thisArray, ar.IntToStr1d(i))
                    If Not Save() Then
                        doWarning(subName, "Error saving " & MakeFN() & ".")
                        Return False
                    End If
                Else
                    doWarning(subName, "Error saving " & MakeDelFN() & ".")
                    Return False
                End If

                currentIndex = ""
                RaiseEvent FormRedraw(EventIndex, Me)
                Return True

            Else
                Return False
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

    Public Function UnDelete(ByVal Index As String) As Boolean

        Dim subName As String = "UnDelete" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Try
            Dim d(0) As String
            Dim ub As Integer = 0
            Dim i As Integer = 0

            LoadDeleted()
            Do While i > -1
                i = ar.InArray(deletedArray, 0, Index)
                If i > -1 Then
                    d(0) = CStr(ar.InArray(deletedArray, 0, Index))
                    ar.DeleteByIndex(deletedArray, d)
                End If
            Loop

            ' Save the updated deletion file
            If ar.ArrayToFile(MakeDelFN(), deletedArray) Then
                Return True
            Else
                doWarning(subName, "Error saving " & MakeDelFN() & ".")
                Return False
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Function

#End Region

#Region "Private Functions"

    Private Function SigData(ByVal Ordinal As Integer) As String

        Dim _sigdata As String = thisArray(1, Ordinal)

        Do Until Not Right(_sigdata, 1) = "."
            _sigdata = Left(_sigdata, Len(_sigdata) - 1)
        Loop

        Dim l() As String = Split(_sigdata, ".")
        Dim tarray(1, UBound(l)) As Integer

        For y As Integer = 0 To UBound(l)
            tarray(0, y) = CInt(Split(l(y), ",")(0))
            tarray(1, y) = CInt(Split(l(y), ",")(1))
        Next

        Return sd.SerialiseDataArray(ar.IntToStr(tarray))

    End Function

    Private Function LoadDeleted() As Boolean

        Dim fn As String = MakeDelFN()
        If File.Exists(fn) Then
            Return ar.ArrayFromFile(deletedArray, MakeDelFN)
        Else
            deletedArray = Nothing
            Return True
        End If

    End Function

    Private Function MakeFN() As String

        Return _SavePath & _Name & ".txt"

    End Function

    Private Function MakeDelFN() As String

        Return _SavePath & _Name & "_del.txt"

    End Function

    Private Function MakeLDFN() As String

        Dim fn As String
        fn = myBaseForm.thisRandom.RandomFileName(".TXT")
        While File.Exists(_PostPath & fn)
            fn = myBaseForm.thisRandom.RandomFileName(".TXT")
        End While
        Return _PostPath & fn

    End Function

#End Region

#Region "Threads"

    Private Sub seekThread()

        Dim subName As String = "seekThread" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Dim evID As Integer = doInfo(subName, "Getting Data")

        Try

            Dim Cancel As Boolean = False
            Dim Sucsess As Boolean = False
            Dim Data As String = ""

            Sucsess = ConWebService(Data)
            Do While Not Sucsess
                ConFail(Cancel)
                If Cancel Or appExit Then
                    Exit Do
                Else
                    For i As Integer = 1 To 10000
                        Thread.Sleep(1)
                    Next
                    Sucsess = ConWebService(Data)
                End If
            Loop

            _Seeking = False

            If Sucsess Then
                If Left(Data, 1) = "!" Then
                    doWarning(subName, Right(Data, Len(Data) - 1))
                Else
                    tempArray = sd.DeSerialiseData(Data)
                    ' Load the deleted Calls
                    LoadDeleted()
                    _NewData = False
                    SyncNewData()
                    If _RedrawForm Then
                        RaiseEvent FormRedraw(EventIndex, Me)
                        _RedrawForm = False
                    End If
                    If _NewData Then
                        RaiseEvent NewData(EventIndex, Me)
                    End If
                End If
            End If

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

        RaiseEvent CancelWarning(evID)

    End Sub

    Private Sub seekSig()

        Dim subName As String = "seekSig" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        Dim evID As Integer = doInfo(subName, "Save signature")
        Try

            Dim Ordinal As Integer = -1
            Dim sData As String = Nothing
            Dim Cancel As Boolean = False
            Dim Sucsess As Boolean = False
            Dim cond(1, 1) As String
            Dim ret As String = ""

            cond(0, 0) = "2"
            cond(1, 0) = "L"
            cond(0, 1) = "3"
            cond(1, 1) = ""

            Dim toLoad() As Integer = ar.Matching(thisArray, cond)

            While UBound(toLoad) > -1

                Ordinal = toLoad(0)
                sData = SigData(Ordinal)

                Sucsess = SaveSig(sData, ret)
                Do While Not Sucsess
                    ConFail(Cancel)
                    If Cancel Or appExit Then
                        Exit Do
                    Else
                        For i As Integer = 1 To 1000
                            Thread.Sleep(1)
                        Next
                        Sucsess = SaveSig(sData, ret)
                    End If
                Loop

                If Sucsess Then
                    If Left(ret, 1) = "!" Then
                        doWarning(subName, Right(ret, Len(ret) - 1))
                    Else
                        thisArray(2, Ordinal) = "L"
                        thisArray(3, Ordinal) = ret
                        Save()
                    End If
                End If

                ' Refresh Load array
                toLoad = ar.Matching(thisArray, cond)

            End While

            RaiseEvent CancelWarning(evID)
            For i As Integer = 0 To UBound(thisArray, 2)
                If thisArray(2, i) = "L" And thisArray(3, i) <> "" Then
                    RaiseEvent SigSaved(EventIndex, Me, i)
                End If
            Next
            _Seeking = False

        Catch e As Exception
            doWarning(subName, e.Message)
        End Try

    End Sub

#End Region

End Class
