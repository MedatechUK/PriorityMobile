Imports System
Imports System.IO
Imports System.Threading
Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows.Forms

Public Class dCls
    Implements IListSource

    Public Data
    Private ble
    Public Deleted
    Public Downloaded
    Private _ItemType As String = Nothing
    Private DataGrid As DataGridView

#Region "Initialisation and finalisation"

    Public Sub New(ByVal ItemType As String)

        ' Check Paths Exist
        mkNEDir("MAIL")
        mkNEDir("MAIL/SENT")
        mkNEDir("MAIL/OUTBOX")
        mkNEDir("DATA")

        Dim t As Type = Type.GetType(ItemType, True, True)
        Dim generic As Type = GetType(oDictionary(Of ,)) 'Dictionary(Of ,)
        Dim vanilla As Type = GetType(Dictionary(Of ,))
        Dim BindList As Type = GetType(BindingList(Of ))
        Dim typeArgs() As Type = {GetType(String), t}
        Dim constructed As Type = generic.MakeGenericType(typeArgs)
        Dim vcon As Type = vanilla.MakeGenericType(typeArgs)
        Dim bindtype As Type = BindList.MakeGenericType(t)
        Data = Activator.CreateInstance(constructed)
        ble = Activator.CreateInstance(bindtype)
        Deleted = Activator.CreateInstance(vcon)
        Downloaded = Activator.CreateInstance(vcon)

        Data.SetMyParent(Me)
        _ItemType = ItemType

    End Sub

#End Region

#Region "Private Variables"

    Private _Name As String
    Private _myBasePath As String
    Private _Undelete As Boolean = False
    Private _Seeking As Boolean = False
    Private _appExit As Boolean = False
    Private _RedrawForm As Boolean = False
    Private _HasNewData As Boolean = False
    Private _oID As Integer = -1
    Private loadLock As New Queue
    Private _StackMode As tStackMode = tStackMode.fifo
    Private LocalCols() As String ' Used to populate columns when not overrides Columns

#End Region

#Region "Public Properties"

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
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

    Private Sub mkNEDir(ByVal dir As String)
        Dim f As New IO.DirectoryInfo(_myBasePath & dir & "\")
        If Not f.Exists Then
            FileSystem.MkDir(_myBasePath & dir)
        End If
    End Sub

    Public ReadOnly Property FileName(ByVal Style As tFilePath) As String
        Get
            Select Case Style
                Case tFilePath.data
                    Return BasePath & "DATA\" & Name & ".txt"
                Case tFilePath.del
                    Return BasePath & "DATA\" & Name & "_del.txt"
                Case tFilePath.post
                    Dim PostPath As String = BasePath & "MAIL\OUTBOX\"
                    Dim fn As String
                    fn = System.Guid.NewGuid().ToString() & ".TXT"
                    While File.Exists(PostPath & fn)
                        fn = System.Guid.NewGuid().ToString() & ".TXT"
                    End While
                    Return PostPath & fn
                Case Else
                    Return Nothing
            End Select
        End Get
    End Property

    Public Property Undelete() As Boolean
        Get
            Return _Undelete
        End Get
        Set(ByVal value As Boolean)
            _Undelete = value
        End Set
    End Property

    Public ReadOnly Property ColumnOrdinal(ByVal ColumnName As String) As Integer
        Get
            Dim ret As Integer = -1
            For i As Integer = 0 To UBound(Columns)
                If Strings.StrComp(Columns(i), ColumnName, CompareMethod.Text) = 0 Then
                    ret = i
                    Exit For
                End If
            Next
            Return ret
        End Get
    End Property

    Public Property Seeking() As Boolean
        Get
            Return _Seeking
        End Get
        Set(ByVal value As Boolean)
            _Seeking = value
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

    Public Property oID() As Integer
        Get
            Return _oID
        End Get
        Set(ByVal value As Integer)
            _oID = value
        End Set
    End Property

    Public Property StackMode() As tStackMode
        Get
            Return _StackMode
        End Get
        Set(ByVal value As tStackMode)
            _StackMode = value
        End Set
    End Property

    Public ReadOnly Property ContainsListCollection() As Boolean Implements System.ComponentModel.IListSource.ContainsListCollection
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Overrideable stubs"

    Public Overridable Function ConWebService(ByRef data) As Boolean
        Throw New Exception("Function cannot be called directly. It must be overriden in a derived class.")
    End Function

    Public Overridable Sub LoadData(ByVal Index As String, ByRef p As Priority.Loading)
        Throw New Exception("Function cannot be called directly. It must be overriden in a derived class.")
    End Sub

    Public Overridable Sub ConFail(ByRef Cancel As Boolean)
        Throw New Exception("Function cannot be called directly. It must be overriden in a derived class.")
    End Sub

    Public Overridable Sub SyncNewData(ByRef SyncResult As hSyncResult)
        Throw New Exception("Function cannot be called directly. It must be overriden in a derived class.")
    End Sub

    Public Overridable Function Validate(ByVal Index As String) As Boolean
        Return Data.ContainsKey(Index)
    End Function

    Public Overridable Function ConQuery() As String
        Throw New Exception("Function cannot be called directly. It must be overriden in a derived class.")
    End Function

    Public Overridable Function Columns() As String()
        If IsNothing(LocalCols) Then
            Throw New Exception("Function cannot be called directly unless the object is instantiated with column data.")
        End If
        Return LocalCols
    End Function

    Private Function NewItem(Optional ByVal bWithEvents As Boolean = True) As Object
        Dim t As Type = Type.GetType(_ItemType)
        Dim ret As New DatasetObjectBase
        ret = Activator.CreateInstance(t)
        If bWithEvents Then _
            AddHandler ret.BasePropertyChanged, AddressOf hPropertyChanged
        Return ret
    End Function

    Private Sub hPropertyChanged(ByVal ID As String, ByVal NAME_Const As String, ByVal value As String)
        OnPropertyChanged(ID, NAME_Const, value)
    End Sub

    Public Overridable Function Index(ByVal RowData() As String) As String
        If IsNothing(RowData) Then
            Throw New Exception("Row data contains nothing.")
            Exit Function
        End If
        Return RowData(0)
    End Function

    Public Overridable Sub OnRemove(ByVal key As Object, ByVal value As Object)

    End Sub

    Public Overridable Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)

    End Sub

    Public Overridable Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)

    End Sub

    Public Overridable Sub OnSetComplete(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)

    End Sub

    Public Overridable Sub OnPropertyChanged(ByVal ID As String, ByVal NAME_Const As String, ByVal value As String)

    End Sub

    Public Overridable Sub OnRowLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

#End Region

#Region "File I/O"

    Public Sub Load(ByVal source As tSource)

        Dim tmp As String = ""

        Deleted.Clear()
        Downloaded.Clear()

        Select Case source
            Case tSource.File

                Seeking = True
                If File.Exists(FileName(tFilePath.data)) Then
                    Using sr As New StreamReader(FileName(tFilePath.data))
                        tmp = sr.ReadToEnd
                    End Using
                    If tmp.Length > 0 Then
                        Dim l = Split(Replace(tmp, Chr(10), ""), Chr(13))
                        For i As Integer = 0 To UBound(l)
                            If l(i).length > 0 Then
                                Dim c() As String = Split(l(i), Chr(9))
                                Dim o As Object = NewItem(True)
                                For col As Integer = 0 To UBound(c)
                                    Dim pi As System.Reflection.PropertyInfo = o.GetType().GetProperty(Columns(col))
                                    pi.SetValue(o, c(col), Nothing)
                                Next
                                Data.Add(Index(c), o)
                            End If
                        Next
                    End If
                End If


                If Undelete Then
                    If File.Exists(FileName(tFilePath.del)) Then
                        Using sr As New StreamReader(FileName(tFilePath.del))
                            tmp = sr.ReadToEnd
                        End Using
                        If tmp.Length > 0 Then
                            Dim l = Split(Replace(tmp, Chr(10), ""), Chr(13))
                            For i As Integer = 0 To UBound(l)
                                If l(i).length > 0 Then
                                    Dim c() As String = Split(l(i), Chr(9))
                                    Dim o As Object = NewItem(False)
                                    For col As Integer = 0 To UBound(c)
                                        Dim pi As System.Reflection.PropertyInfo = o.GetType().GetProperty(Columns(col))
                                        pi.SetValue(o, c(col), Nothing)
                                    Next
                                    Deleted.Add(Index(c), o)
                                End If
                            Next
                        End If
                    End If
                End If
                Seeking = False

            Case tSource.SOAP
                Dim subName As String = "BeginSeek" 'StackTrace().GetFrame(0).GetMethod.ToString()
                Try
                    If Not Seeking Then
                        Seeking = True

                        ' Create new Seeking thread 
                        Dim myThread As Thread
                        'Caller = o
                        myThread = New Thread(New ThreadStart(AddressOf seekThread))
                        myThread.Start()

                    End If
                Catch e As Exception
                    'doWarning(subName, e.Message)
                End Try

        End Select

    End Sub

    Public Sub Save(ByVal Destination As tSource, Optional ByVal Index As String = Nothing)

        If Seeking Then Exit Sub

        Select Case Destination
            '*** To File
            Case tSource.File
                If Data.Count = 0 Then
                    If File.Exists(FileName(tFilePath.data)) Then
                        File.Delete(FileName(tFilePath.data))
                    End If
                Else
                    Using sw As StreamWriter = New StreamWriter(FileName(tFilePath.data))
                        For Each y As Object In Data.Values
                            For x As Integer = 0 To UBound(Columns)
                                Dim pi As System.Reflection.PropertyInfo = y.GetType().GetProperty(Columns(x))
                                sw.Write(pi.GetValue(y, Nothing))
                                If x < UBound(Columns) Then sw.Write(Chr(9))
                            Next
                            sw.Write(vbCrLf)
                        Next
                    End Using
                End If

                If Undelete Then
                    If Deleted.Count = 0 Then
                        If File.Exists(FileName(tFilePath.del)) Then
                            File.Delete(FileName(tFilePath.del))
                        End If

                    Else
                        Using sw As StreamWriter = New StreamWriter(FileName(tFilePath.del))
                            For Each y As Object In Deleted.Values
                                For x As Integer = 0 To UBound(Columns)
                                    Dim pi As System.Reflection.PropertyInfo = y.GetType().GetProperty(Columns(x))
                                    sw.Write(pi.GetValue(y, Nothing))
                                    If x < UBound(Columns) Then sw.Write(Chr(9))
                                Next
                                sw.Write(vbCrLf)
                            Next
                        End Using
                    End If

                End If

            Case tSource.SOAP
                ' From SOAP

                Monitor.Enter(loadLock)
                Try
                    If IsNothing(Index) Then
                        Throw New Exception("Invalid record data.")
                    End If

                    If Not Validate(Index) Then
                        Throw New Exception("Invalid SOAP bubble.")
                    End If

                    Dim p As New Priority.Loading
                    LoadData(Index, p)
                    If Not p.Validate Then
                        Throw New Exception("Invalid SOAP bubble.")
                    End If

                    Dim fn As String = FileName(tFilePath.post)
                    If p.ToFile(fn) Then
                        p = Nothing
                        RaiseEvent EnQueueLoading(oID, fn)
                        Data.Remove(Index)
                    Else
                        Throw New Exception("Could not save SOAP bubble to " & fn)
                    End If

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    Monitor.Exit(loadLock)
                End Try

        End Select

    End Sub

    Public Sub Recover(ByVal Key As String)

        If Not Undelete Then
            Throw New Exception("This dataset has no undelete function.")
        End If

        If Data.ContainsKey(Key) Then
            Throw New Exception("The record is not deleted.")
        End If

        With Deleted
            If Not .ContainsKey(Key) Then
                Throw New Exception("Key not found in undelete dataset.")
            End If

            Dim it As Object = .Item(Key)
            .Remove(Key)
            Data.Add(Key, it)

        End With

    End Sub

#Region "Threads"

    Private Sub seekThread()

        Dim subName As String = "seekThread" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        'Dim evID As Integer = doInfo(subName, "Getting Data")

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
                    For i As Integer = 1 To 2000
                        Thread.Sleep(1)
                    Next
                    Sucsess = ConWebService(Data)
                End If
            Loop

            _Seeking = False

            If Sucsess Then
                If Left(Data, 1) = "!" Then
                    'doWarning(subName, Right(Data, Len(Data) - 1))
                Else
                    DeSerialise(Data)
                    Dim hresult As New hSyncResult
                    Seeking = True
                    SyncNewData(hresult)
                    Seeking = False
                    With hresult
                        If .Redraw Then
                            RaiseEvent FormRedraw(oID)
                        End If
                        If .NewData Then
                            Save(tSource.File)
                            RaiseEvent NewData(oID)
                        End If
                    End With
                End If
            End If

        Catch e As Exception
            'doWarning(subName, e.Message)
        End Try

        'RaiseEvent CancelWarning(evID)

    End Sub

    Private Sub seekSig()

        '    Dim subName As String = "seekSig" 'New StackTrace().GetFrame(0).GetMethod.ToString()
        '    Dim evID As Integer = doInfo(subName, "Save signature")
        '    Try

        '        Dim Ordinal As Integer = -1
        '        Dim sData As String = Nothing
        '        Dim Cancel As Boolean = False
        '        Dim Sucsess As Boolean = False
        '        Dim cond(1, 1) As String
        '        Dim ret As String = ""

        '        cond(0, 0) = "2"
        '        cond(1, 0) = "L"
        '        cond(0, 1) = "3"
        '        cond(1, 1) = ""

        '        Dim toLoad() As Integer = ar.Matching(thisArray, cond)

        '        While UBound(toLoad) > -1

        '            Ordinal = toLoad(0)
        '            sData = SigData(Ordinal)

        '            Sucsess = SaveSig(sData, ret)
        '            Do While Not Sucsess
        '                ConFail(Cancel)
        '                If Cancel Or appExit Then
        '                    Exit Do
        '                Else
        '                    For i As Integer = 1 To 1000
        '                        Thread.Sleep(1)
        '                    Next
        '                    Sucsess = SaveSig(sData, ret)
        '                End If
        '            Loop

        '            If Sucsess Then
        '                If Left(ret, 1) = "!" Then
        '                    doWarning(subName, Right(ret, Len(ret) - 1))
        '                Else
        '                    thisArray(2, Ordinal) = "L"
        '                    thisArray(3, Ordinal) = ret
        '                    Save()
        '                End If
        '            End If

        '            ' Refresh Load array
        '            toLoad = ar.Matching(thisArray, cond)

        '        End While

        '        RaiseEvent CancelWarning(evID)
        '        For i As Integer = 0 To UBound(thisArray, 2)
        '            If thisArray(2, i) = "L" And thisArray(3, i) <> "" Then
        '                RaiseEvent SigSaved(EventIndex, Me, i)
        '            End If
        '        Next
        '        _Seeking = False

        '    Catch e As Exception
        '        doWarning(subName, e.Message)
        '    End Try

    End Sub

    Private Sub DeSerialise(ByVal Data As String)
        With Downloaded
            .clear()
            Dim l As String() = Split(Data, "\n")
            For y As Integer = 0 To UBound(l)
                Dim it As Object = NewItem(False)
                Dim v As String() = Split(l(y), "\t")
                For i As Integer = 0 To UBound(Columns)
                    Dim pi As System.Reflection.PropertyInfo = it.GetType().GetProperty(Columns(i))
                    pi.SetValue(it, v(i), Nothing)
                Next
                .Add(Index(v), it)
            Next
        End With
    End Sub

#End Region

#End Region

#Region "Public Events"

    Event NewData(ByVal oID As o)
    Event FormRedraw(ByVal oID As o)
    Event EnQueueLoading(ByVal oID As o, ByVal Loadfile As String)
    'Public Shared Event Warning(ByVal EventIndex As Integer, ByVal o As PDAData, ByVal SubName As String, ByVal Message As String, ByVal infoOnly As Boolean)
    'Public Shared Event CancelWarning(ByVal EventIndex As Integer)
    'Public Shared Event SigSaved(ByVal EventIndex As Integer, ByVal o As PDAData, ByVal Ordinal As Integer)    

#End Region

#Region "Make Data recordset default"

#Region "Public Property Values"

    Default Public Property Item(ByVal key As [String]) As [Object]
        Get
            Return CType(Data(key), [Object])
        End Get
        Set(ByVal value As [Object])
            Data(key) = value
        End Set
    End Property

    Public ReadOnly Property Keys() As ICollection
        Get
            Return Data.Keys
        End Get
    End Property

    Public ReadOnly Property Values() As ICollection
        Get
            Return Data.Values
        End Get
    End Property

    Public ReadOnly Property FirstKey() As String
        Get
            If Data.Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In Data.Keys
                    If ordinal = 1 Then
                        Return k
                    End If
                Next
                Return Nothing
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property LastKey() As String
        Get
            If Data.Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In Data.Keys
                    If ordinal = Data.Count Then
                        Return k
                    End If
                    ordinal += 1
                Next
                Return Nothing
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

#Region "Public Subs"

    Public Sub Add(ByVal key As [String], ByVal value As [Object])
        Data.Add(key, value)
    End Sub 'Add

    Public Function ContainsKey(ByVal key As [String]) As Boolean
        Return Data.Containskey(key)
    End Function 'Contains

    Public Sub Remove(ByVal key As [String])
        If ContainsKey(key) Then
            Data.Remove(key)
        End If
    End Sub 'Remove

#End Region

#End Region

#Region "Public functions"

    Public Function PeekKey() As String

        Select Case StackMode
            Case tStackMode.fifo
                Return Data.FirstKey
            Case Else
                Return Data.LastKey
        End Select

    End Function

    Public Function GetList() As System.Collections.IList Implements System.ComponentModel.IListSource.GetList

        If Not (IsNothing(Data)) Then
            With ble
                .clear()
                For Each key As String In Data.keys
                    ble.add(Data.item(key))
                Next
            End With
            Return ble
        Else
            Return Nothing
        End If

    End Function

    Public Function ObjectIndex(ByVal o As Object) As String
        Dim rowdata(UBound(Columns)) As String
        For x As Integer = 0 To UBound(Columns)
            Dim pi As System.Reflection.PropertyInfo = o.GetType().GetProperty(Columns(x))
            rowdata(x) = pi.GetValue(o, Nothing)
        Next
        Return Index(rowdata)
    End Function

    Public Sub NewItemFromObject(ByRef ob As Object)
        Dim oba = NewItem(True)
        For i As Integer = 0 To UBound(Columns)
            Dim pi As System.Reflection.PropertyInfo = oba.GetType().GetProperty(Columns(i))
            Dim pi2 As System.Reflection.PropertyInfo = oba.GetType().GetProperty(Columns(i))
            pi.SetValue(oba, pi2.GetValue(ob, Nothing), Nothing)
        Next
        oba.id = ob.id
        Add(ObjectIndex(oba), oba)
    End Sub

    Public Function ItemChanged(ByRef item As Object, ByVal newitem As Object) As Boolean
        Dim ret As Boolean = False
        For i As Integer = 0 To UBound(Columns)
            Dim pi As System.Reflection.PropertyInfo = item.GetType().GetProperty(Columns(i))
            Dim pi2 As System.Reflection.PropertyInfo = item.GetType().GetProperty(Columns(i))
            If Not pi.GetValue(item, Nothing) = pi2.GetValue(newitem, Nothing) Then
                ret = True
                pi.SetValue(item, pi2.GetValue(newitem, Nothing), Nothing)
            End If
        Next
        Return ret
    End Function

#End Region

#Region "Bound Data Grid Handlers"

    Public Sub BindDataGrid(ByRef dv As DataGridView)
        DataGrid = dv
        With DataGrid
            .Columns.Clear()
            For Each c As String In Columns()
                .Columns.Add(c, c)
                .Columns(.Columns.Count - 1).DataPropertyName = c
            Next
        End With
        AddHandler dv.RowValidating, AddressOf hRowValidating
        AddHandler dv.UserDeletingRow, AddressOf hUserDeletingRow
        AddHandler dv.RowEnter, AddressOf hRowEnter
        AddHandler dv.CellValidating, AddressOf hCellValidating
        AddHandler dv.CellBeginEdit, AddressOf hCellBeginEdit
        AddHandler dv.RowLeave, AddressOf hRowLeave

    End Sub

    Private Sub hRowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If sender.Rows(e.RowIndex).IsNewRow Then
            sender.Rows(e.RowIndex).Tag = "New"
        End If
    End Sub

    Private Sub hCellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs)
        If Not sender.Rows(e.RowIndex).Tag = "New" Then _
            sender.Rows(e.RowIndex).Tag = "Edit"
    End Sub

    Private Sub hUserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs)
        If MsgBox("Are you sure you wish to delete the selected records?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            Remove(ObjectIndex(e.Row.DataBoundItem))
        End If
    End Sub

    Private Sub hRowValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs)

        Dim ex As Object
        With sender

            '.endedit()
            Try
                ex = .Rows(e.RowIndex).DataBoundItem
            Catch
                ex = Nothing
            End Try
            If IsNothing(ex) Then Exit Sub

            Try
                If .Rows(e.RowIndex).Tag = "New" Then
                    If Not ContainsKey(ObjectIndex(ex)) Then
                        NewItemFromObject(ex)
                        Save(tSource.File)
                        e.Cancel = False
                        .Rows(e.RowIndex).Tag = ""
                    Else
                        e.Cancel = True
                        Throw New Exception("A record with the index: " & ObjectIndex(ex) & " already exists.")
                    End If

                ElseIf .Rows(e.RowIndex).Tag = "Edit" Then
                    'For Each k As String In Data.keys
                    '    If ex.id <> Data.item(k).id Then
                    '        If Strings.StrComp(ObjectIndex(ex), ObjectIndex(Data.item(k)), CompareMethod.Text) = 0 Then
                    '            e.Cancel = True
                    '            .BeginEdit(False)
                    '            Throw New Exception("A record with the index: " & ObjectIndex(Data.item(k)) & " already exists.")
                    '        End If
                    '    End If
                    'Next
                    .Rows(e.RowIndex).Tag = ""
                    Save(tSource.File)
                End If

            Catch exc As Exception
                MsgBox(exc.Message)
            End Try
        End With

    End Sub

    Private Sub hCellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)

        Dim dv As DataGridView = sender
        Try
            With dv.Rows(e.RowIndex)
                If .Tag = "Edit" Then
                    If Strings.StrComp(e.FormattedValue, .Cells(e.ColumnIndex).Value, CompareMethod.Text) = 0 Then
                        'Column is unchanged
                        Exit Sub
                    End If

                    Dim testob As New Object
                    testob = NewItem()

                    Dim chcol As Integer = Nothing
                    For i As Integer = 0 To UBound(Columns)
                        Dim pi As System.Reflection.PropertyInfo = testob.GetType().GetProperty(Columns(i))
                        If Strings.StrComp(dv.Columns(e.ColumnIndex).Name, Columns(i), CompareMethod.Text) = 0 Then
                            chcol = i
                            pi.SetValue(testob, e.FormattedValue, Nothing)
                        Else
                            Dim pi2 As System.Reflection.PropertyInfo = testob.GetType().GetProperty(Columns(i))
                            pi.SetValue(testob, pi2.GetValue(.DataBoundItem, Nothing), Nothing)
                        End If
                    Next

                    Dim found As Object = Nothing
                    For Each k As String In Data.keys
                        If .DataBoundItem.id <> Data.item(k).id Then
                            If Strings.StrComp(ObjectIndex(testob), ObjectIndex(Data.item(k)), CompareMethod.Text) = 0 Then
                                e.Cancel = True
                                dv.BeginEdit(False)
                                Throw New Exception("A record with the index: " & ObjectIndex(Data.item(k)) & " already exists.")
                            End If
                        Else
                            found = Data.item(k)
                        End If
                    Next

                    If Not IsNothing(found) Then
                        Dim pi As System.Reflection.PropertyInfo = found.GetType().GetProperty(Columns(chcol))
                        pi.SetValue(found, e.FormattedValue, Nothing)
                    End If

                End If
            End With

        Catch exc As Exception
            MsgBox(exc.Message)
        End Try
    End Sub

    Private Sub hRowLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        onRowLeave(sender, e)
    End Sub
#End Region

End Class