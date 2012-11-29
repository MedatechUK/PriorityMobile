Imports System.Reflection
Imports system.io
Imports System.Threading
Imports System.ComponentModel

Public Class oBind
    Public FilterObject As Object
    Private _Parent As oDataSet
    Private pi As System.Reflection.PropertyInfo
    Public Event Refresh()
#Region "FilterObject Methods"
#Region "Initialisation / finalisation"
    Public Sub New(ByVal Parent As oDataSet, ByVal dllName As String, ByVal className As String)
        _Parent = Parent
        Dim TargetAssembly As Assembly = Assembly.LoadFrom(dllName)
        _TagetType = TargetAssembly.GetType(className, True)
        FilterObject = Activator.CreateInstance(BindType)
        BindProperty.SetValue(FilterObject, Me, Nothing)
        If File.Exists(FileName(tFilePath.data)) Then
            Loading = True
            Using sr As New StreamReader(FileName(tFilePath.data))
                Do Until sr.EndOfStream
                    Add(sr.ReadLine)
                Loop
            End Using
            Loading = False
        End If
    End Sub
#End Region
#Region "Add Item"
    Public Sub Add(ByRef NewObject As Object)        
        Dim t() As System.Type = {TagetType}
        Dim args() As Object = {NewObject}
        BindType.GetMethod("AddItem", t).Invoke(FilterObject, args)
        If Not Loading Then
            _Changes = True
        End If
    End Sub
    Public Sub Add(ByVal str As String)
        If str.Length > 0 Then
            Dim c() As String = Split(str, Chr(9))
            If UBound(c) = UBound(Columns) Then
                Dim t As Object = Activator.CreateInstance(TagetType)
                Dim i As Integer = 0
                For Each col As String In Columns()
                    pi = TagetType.GetProperty(col)
                    Select Case pi.PropertyType.Name
                        Case "Int32"
                            pi.SetValue(t, CInt(c(i)), Nothing)
                        Case "String"
                            pi.SetValue(t, Replace(c(i), "\n", vbCrLf), Nothing)
                    End Select
                    i += 1
                Next
                Me.Add(t)
            End If
        End If
    End Sub
#End Region
#Region "Get / Set / Remove / clear Item / value"
    Public Sub Clear()
        Dim t() As System.Type = {}
        Dim args() As Object = {}
        BindType.GetMethod("ClearMyItems", t).Invoke(FilterObject, args)
        _Changes = True
    End Sub
    Public Sub Remove(ByVal i As Integer)
        Dim cancel As Boolean = False
        Trigger(tTrigger.PREDELETE, Nothing, Item(i), cancel)
        If Not cancel Then
            Dim del As Object = Item(i)
            Dim t() As System.Type = {GetType(Integer), GetType(String)}
            Dim args() As Object = {i, KeyProperty.GetValue(Item(i), Nothing)}
            BindType.GetMethod("RemoveMyItem", t).Invoke(FilterObject, args)
            Trigger(tTrigger.POSTDELETE, Nothing, del)
            _Changes = True
        End If
    End Sub
    Public Sub Remove(ByVal k As String)
        Dim i As Integer = ContainsKey(k)
        If Not i = -1 Then
            Remove(i)
        End If
    End Sub
    Public ReadOnly Property OriginalList() As List(Of Object)
        Get
            Dim t() As System.Type = {}
            Dim args() As Object = {}
            Return BindType.GetMethod("ObjectList", t).Invoke(FilterObject, args)
        End Get
    End Property
    Public Property Item(ByVal i As Integer) As Object
        Get
            Dim t() As System.Type = {GetType(Integer)}
            Dim args() As Object = {i}
            Return BindType.GetMethod("GetMyItem", t).Invoke(FilterObject, args)
        End Get
        Set(ByVal value As Object)
            Dim k As Integer = ContainsKey(KeyProperty.GetValue(value, Nothing))
            If k = -1 Or KeyProperty.GetValue(value, Nothing) = KeyProperty.GetValue(OriginalList(i), Nothing) Then
                Dim t() As System.Type = {GetType(Integer), TagetType}
                Dim args() As Object = {i, value}
                BindType.GetMethod("SetMyItem", t).Invoke(FilterObject, args)
            Else
                MsgBox("The record already exists.")
            End If
        End Set
    End Property
    Public Property Item(ByVal k As String) As Object
        Get
            Dim it As Object = Nothing
            For i As Integer = 0 To Me.OriginalList.Count - 1
                it = Item(i)
                If String.Compare(KeyProperty.GetValue(it, Nothing), k, True) = 0 Then
                    Return it
                    Exit For
                End If
            Next
            Return Nothing
        End Get
        Set(ByVal value As Object)
            For i As Integer = 0 To Me.OriginalList.Count - 1
                If String.Compare(KeyProperty.GetValue(Item(i), Nothing), k, True) = 0 Then
                    Dim c As Object = Clone(Item(i))
                    Item(i) = value
                    Exit For
                End If
            Next
        End Set
    End Property
    Public Property Value(ByVal o As Object, ByVal ColumnName As String) As Object
        Get
            pi = TagetType.GetProperty(ColumnName)
            Return pi.GetValue(o, Nothing)
        End Get
        Set(ByVal value As Object)
            'Dim test As Object = Clone(o)
            'Dim c As Object = Clone(o)
            pi = TagetType.GetProperty(ColumnName)
            Select Case pi.PropertyType.Name
                Case "Int32"
                    pi.SetValue(o, CInt(value), Nothing)
                Case "String"
                    pi.SetValue(o, value, Nothing)
            End Select
            'Dim k As Integer = ContainsKey(KeyProperty.GetValue(o, Nothing))
            'Dim nk As Integer = ContainsKey(KeyProperty.GetValue(test, Nothing))
            'If nk = -1 Or nk = k Then
            '    Select Case pi.PropertyType.Name
            '        Case "Int32"
            '            pi.SetValue(o, CInt(value), Nothing)
            '        Case "String"
            '            pi.SetValue(o, value, Nothing)
            '    End Select
            '    Me.TriggerEdit(o, c)
            'Else
            '    MsgBox("The record already exists.")
            'End If
        End Set
    End Property
#End Region
#Region "Misc Properties"
    Private _Loading As Boolean = False
    Public Property Loading() As Boolean
        Get
            Return _Loading
        End Get
        Set(ByVal value As Boolean)
            _Loading = value
        End Set
    End Property
    Private _Changes As Boolean = False
    Public ReadOnly Property Changes() As Boolean
        Get
            Return _Changes
        End Get
    End Property
    Public ReadOnly Property Count() As Integer
        Get
            Return CountProperty.GetValue(FilterObject, Nothing)
        End Get
    End Property
    Public ReadOnly Property ContainsKey(ByVal Key As String) As Integer
        Get
            With OriginalList
                For i As Integer = 0 To .Count - 1
                    If String.Compare(KeyProperty.GetValue(.Item(i), Nothing), Key, True) = 0 Then
                        Return i
                        Exit For
                    End If
                Next
            End With

            Return -1
        End Get
    End Property
    Public Overrides Function ToString() As String
        Dim ret As String = ""
        With OriginalList
            For i As Integer = 0 To .Count - 1
                ret += .Item(i).ToString
                If i < .Count - 1 Then ret += vbCrLf
            Next
        End With
        Return ret
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return CBool(String.Compare(obj.ToString, TagetType.Name, True) = 0)
    End Function
    Private Property EditClone(ByVal o As Object) As Object
        Get
            Dim t() As System.Type = {}
            Dim args() As Object = {}
            Return TagetType.GetMethod("GetEditClone", t).Invoke(o, args)
        End Get
        Set(ByVal value As Object)
            Dim t() As System.Type = {TagetType}
            Dim args() As Object = {value}
            TagetType.GetMethod("SetEditClone", t).Invoke(o, args)
        End Set
    End Property
    Public Property Loaded(ByVal o As Object) As Boolean
        Get
            Dim t() As System.Type = {}
            Dim args() As Object = {}
            Return TagetType.GetMethod("GetLoaded", t).Invoke(o, args)
        End Get
        Set(ByVal value As Boolean)
            Dim t() As System.Type = {GetType(Boolean)}
            Dim args() As Object = {value}
            TagetType.GetMethod("SetLoaded", t).Invoke(o, args)
        End Set
    End Property
    Public Property CancelEdit(ByVal o As Object) As Boolean
        Get
            Dim t() As System.Type = {}
            Dim args() As Object = {}
            Return TagetType.GetMethod("GetCancelEdit", t).Invoke(o, args)
        End Get
        Set(ByVal value As Boolean)
            Dim t() As System.Type = {GetType(Boolean)}
            Dim args() As Object = {value}
            TagetType.GetMethod("SetCancelEdit", t).Invoke(o, args)
        End Set
    End Property
#End Region
#End Region
#Region "Base type Property info"
    Private _BindProperty As System.Reflection.PropertyInfo = Nothing
    Private ReadOnly Property BindProperty() As System.Reflection.PropertyInfo
        Get
            If IsNothing(_BindProperty) Then
                _BindProperty = BindType.GetProperty("Bind")
            End If
            Return _BindProperty
        End Get
    End Property
    Private _ChangesProperty As System.Reflection.PropertyInfo = Nothing
    Private ReadOnly Property ChangesProperty() As System.Reflection.PropertyInfo
        Get
            If IsNothing(_ChangesProperty) Then
                _ChangesProperty = BindType.GetProperty("Changes")
            End If
            Return _ChangesProperty
        End Get
    End Property
    Private _CancelEditProperty As System.Reflection.PropertyInfo = Nothing
    'Private ReadOnly Property CancelEditProperty() As System.Reflection.PropertyInfo
    '    Get
    '        If IsNothing(_CancelEditProperty) Then
    '            _CancelEditProperty = TagetType.GetProperty("CancelEdit")
    '        End If
    '        Return _CancelEditProperty
    '    End Get
    'End Property
    'Private _EditCloneProperty As System.Reflection.PropertyInfo = Nothing
    'Private ReadOnly Property EditCloneProperty() As System.Reflection.PropertyInfo
    '    Get
    '        If IsNothing(_EditCloneProperty) Then
    '            _EditCloneProperty = TagetType.GetProperty("EditClone")
    '        End If
    '        Return _EditCloneProperty
    '    End Get
    'End Property
    Private _KeyProperty As System.Reflection.PropertyInfo = Nothing
    Public ReadOnly Property KeyProperty() As System.Reflection.PropertyInfo
        Get
            If IsNothing(_KeyProperty) Then
                _KeyProperty = TagetType.GetProperty("Key")
            End If
            Return _KeyProperty
        End Get
    End Property
    Private _CountProperty As System.Reflection.PropertyInfo = Nothing
    Private ReadOnly Property CountProperty() As System.Reflection.PropertyInfo
        Get
            If IsNothing(_CountProperty) Then
                _CountProperty = BindType.GetProperty("Count")
            End If
            Return _CountProperty
        End Get
    End Property
    'Private _LoadedProperty As System.Reflection.PropertyInfo = Nothing
    'Public ReadOnly Property LoadedProperty() As System.Reflection.PropertyInfo
    '    Get
    '        If IsNothing(_LoadedProperty) Then
    '            _LoadedProperty = TagetType.GetProperty("Loaded")
    '        End If
    '        Return _LoadedProperty
    '    End Get
    'End Property
#End Region
#Region "Built Types"
    Public ReadOnly Property Name() As String
        Get
            Return TagetType.FullName
        End Get
    End Property
    Private _TagetType As Type
    Private ReadOnly Property TagetType() As Type
        Get
            Return _TagetType
        End Get
    End Property
    Private _BindList As Type = Nothing
    Private ReadOnly Property BindType() As Type
        Get
            If IsNothing(_BindList) Then
                Dim BindList As Type = GetType(FilteredBindingList(Of ))
                _BindList = BindList.MakeGenericType(TagetType)
            End If
            Return _BindList
        End Get
    End Property
    Private _Prototype As Object = Nothing
    Public ReadOnly Property Prototype() As Object
        Get
            If IsNothing(_Prototype) Then _
                _Prototype = Activator.CreateInstance(TagetType)
            Return _Prototype
        End Get
    End Property
    Private ReadOnly Property TriggerType()
        Get
            Return GetType(oTrigger)
        End Get
    End Property
    Public ReadOnly Property FileName(ByVal Style As tFilePath) As String
        Get
            Select Case Style
                Case tFilePath.data
                    Return _Parent.BasePath & "DATA\" & Name & ".txt"
                Case tFilePath.del
                    Return _Parent.BasePath & "DATA\" & Name & "_del.txt"
                Case tFilePath.post
                    Dim PostPath As String = _Parent.BasePath & "MAIL\OUTBOX\"
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
#End Region
#Region "Prototype Functions"
    Private _Columns() As String = Nothing
    Private ReadOnly Property Columns() As String()
        Get
            If IsNothing(_Columns) Then
                Dim t() As System.Type = {}
                Dim args() As Object = {}
                _Columns = TagetType.GetMethod("Columns", t).Invoke(Prototype, args)
            End If
            Return _Columns
        End Get
    End Property
    Private _ConQuery As String = Nothing
    Private ReadOnly Property ConQuery() As String
        Get
            Dim ret As String = Nothing
            If IsNothing(_ConQuery) Then
                Dim t() As System.Type = {}
                Dim args() As Object = {}
                _ConQuery = TagetType.GetMethod("ConQuery", t).Invoke(Prototype, args)
            End If
            If Not IsNothing(_ConQuery) Then
                ret = _ConQuery
                For Each k As String In _Parent.QueryConsts.Keys
                    Dim rep As String = String.Format("%{0}%", k).ToUpper
                    Dim repw As String = _Parent.QueryConsts(k)
                    ret = Replace(ret, rep, repw, , , CompareMethod.Text)
                Next
            End If
            Return ret
        End Get
    End Property
    Private Function Clone(ByVal o As Object) As Object
        Dim r As Object = Activator.CreateInstance(TagetType)
        For Each col As String In Columns()
            pi = TagetType.GetProperty(col)
            pi.SetValue(r, pi.GetValue(o, Nothing), Nothing)
        Next
        Return r
    End Function
    Private Function Compare(ByVal k As Object, ByVal o As Object) As Integer
        Dim objectProps As PropertyDescriptorCollection = TypeDescriptor.GetProperties(Prototype)
        For Each prop As PropertyDescriptor In objectProps
            pi = TagetType.GetProperty(prop.Name)
            If Not (pi.GetValue(k, Nothing) = pi.GetValue(o, Nothing)) Then
                Return -1
            End If
        Next
        Return 0
    End Function
#End Region
#Region "Handlers"
    Private _PropertyChanged As System.Reflection.EventInfo = Nothing
    Public ReadOnly Property PropertyChanged() As Reflection.EventInfo
        Get
            If IsNothing(_PropertyChanged) Then
                _PropertyChanged = TagetType.GetEvent("Changes")
            End If
            Return _PropertyChanged
        End Get
    End Property
    Public Sub hPropertyChanged(ByVal sender As Object, ByVal e As EventArgs)
        _Changes = True
        Trigger(tTrigger.POSTUPDATE, sender, EditClone(sender), Nothing)
        Save()
        RaiseEvent Refresh()
    End Sub
    Private _PropertyBeginEdit As System.Reflection.EventInfo = Nothing
    Public ReadOnly Property PropertyBeginEdit() As Reflection.EventInfo
        Get
            If IsNothing(_PropertyBeginEdit) Then
                _PropertyBeginEdit = TagetType.GetEvent("BeginEdit")
            End If
            Return _PropertyBeginEdit
        End Get
    End Property
    Public Sub hBeginEdit(ByVal sender As Object, ByVal e As EventArgs)
        Dim Cancel As Boolean = False
        Dim oldkey As String = KeyProperty.GetValue(sender, Nothing)
        Dim newkey As String = KeyProperty.GetValue(EditClone(sender), Nothing)
        If Not String.Compare(newkey, oldkey) = 0 Then
            If Not ContainsKey(newkey) = -1 Then
                Cancel = True
                CancelEdit(sender) = True
                MsgBox("The key of the updated record already exists.")
            Else
                Trigger(tTrigger.PREUPDATE, EditClone(sender), sender, Cancel)
                If Cancel Then
                    CancelEdit(sender) = True
                End If
            End If
        Else
            Trigger(tTrigger.PREUPDATE, EditClone(sender), sender, Cancel)
            If Cancel Then
                CancelEdit(sender) = True
            End If
        End If
        If Cancel Then RaiseEvent Refresh()
    End Sub
    Private Sub hSyncGetData()
        Dim ex As Exception = Nothing
        Dim Data As String = ""
        Do
            ex = Nothing
            Try
                Data = _Parent.ws.GetData(ConQuery, _Parent.UseEnvironment)
            Catch e As Exception
                ex = e
                Thread.Sleep(2000)
            End Try
        Loop Until IsNothing(ex)
        If Left(Data, 1) = "!" Then
            MsgBox(Right(Data, Len(Data) - 1))
        Else
            hSyncReceiveData(SOAPObjects(Data))
        End If
        Loading = False
    End Sub
    Private Function SOAPObjects(ByVal Data As String) As List(Of Object)
        If Data.Length = 0 Then Return Nothing
        Dim Downloaded As New List(Of Object)
        With Downloaded
            For Each l As String In Split(Data, "\n")
                Dim t As Object = Activator.CreateInstance(TagetType)
                Dim i As Integer = 0
                For Each col As String In Columns()
                    Dim c() As String = Split(l, "\t")
                    pi = TagetType.GetProperty(col)
                    Select Case pi.PropertyType.Name
                        Case "Int32"
                            pi.SetValue(t, CInt(c(i)), Nothing)
                        Case "String"
                            pi.SetValue(t, c(i), Nothing)
                    End Select
                    i += 1
                Next
                .Add(t)
            Next
        End With
        Return Downloaded
    End Function
    Private Sub hSyncReceiveData(ByVal Downloaded As List(Of Object))
        Dim ins As New List(Of String)
        Dim upd As New List(Of String)
        Dim del As New List(Of String)
        If IsNothing(Downloaded) Then
            Clear()
        Else
            For Each o As Object In Downloaded
                Dim key As String = KeyProperty.GetValue(o, Nothing)
                Dim k As Integer = ContainsKey(key)
                Select Case k
                    Case -1
                        Add(o)
                        ins.Add(key)
                        _Changes = True
                    Case Else
                        upd.Add(key)
                        If Not String.Compare(Item(k).ToString, o.ToString, True) = 0 Then
                            Dim t() As System.Type = {TagetType}
                            Dim args() As Object = {o}
                            TagetType.GetMethod("Update", t).Invoke(Item(k), args)
                            _Changes = True
                        End If
                End Select
            Next
            If upd.Count < OriginalList.Count Then
                For Each o As Object In OriginalList
                    Dim key As String = KeyProperty.GetValue(o, Nothing)
                    If Not upd.Contains(key) And Not ins.Contains(key) Then
                        del.Add(key)
                    End If
                Next
                For Each delKey As String In del
                    Remove(delKey)
                    _Changes = True
                Next
            End If
        End If
        If _Changes Then RaiseEvent Refresh()
        Save()
    End Sub
#End Region
#Region "Public Subs"
    Public Sub Save()
        If Changes Then
            With OriginalList
                If .Count = 0 Then
                    If File.Exists(FileName(tFilePath.data)) Then
                        File.Delete(FileName(tFilePath.data))
                    End If
                Else
                    Using sw As StreamWriter = New StreamWriter(FileName(tFilePath.data))
                        For i As Integer = 0 To .Count - 1
                            sw.WriteLine(.Item(i).ToString)
                        Next
                    End Using
                End If
            End With
            _Changes = False
        End If
    End Sub
    Public Sub Sync()
        If Not IsNothing(ConQuery) Then
            ' Create new Seeking thread 
            With _Parent.ws
                If Not Loading Then
                    Loading = True
                    ' Create new Seeking thread 
                    Dim myThread As Thread
                    myThread = New Thread(New ThreadStart(AddressOf hSyncGetData))
                    myThread.Start()
                End If
            End With
        End If
    End Sub
#End Region
#Region "Triggers"
    Public Sub AddTrigger(ByVal tr As Object)
        Triggers.Add(tr)
    End Sub
    Private _Triggers As New List(Of Object)
    Public Property Triggers() As List(Of Object)
        Get
            Return _Triggers
        End Get
        Set(ByVal value As List(Of Object))
            _Triggers = value
        End Set
    End Property
    Public Sub Trigger(ByVal TT As tTrigger, Optional ByVal NewObject As Object = Nothing, Optional ByVal OldObject As Object = Nothing, Optional ByRef Cancel As Boolean = False)
        For Each tr As oTrigger In Triggers
            Try
                Select Case TT
                    Case tTrigger.PREINSERT
                        tr.Trigger(TT, NewObject, Nothing, Cancel)
                    Case tTrigger.POSTINSERT
                        If Not IsNothing(NewObject) Then
                            tr.Trigger(TT, NewObject, Nothing, Nothing)
                        Else
                            Throw New Exception("Invalid parameter for POSTINSERT trigger.")
                        End If
                    Case tTrigger.PREUPDATE
                        If Not IsNothing(NewObject) And Not IsNothing(OldObject) Then
                            tr.Trigger(TT, NewObject, OldObject, Cancel)
                        Else
                            Throw New Exception("Invalid parameter for PREUPDATE trigger.")
                        End If
                    Case tTrigger.POSTUPDATE
                        If Not IsNothing(NewObject) And Not IsNothing(OldObject) Then
                            tr.Trigger(TT, NewObject, OldObject, Nothing)
                        Else
                            Throw New Exception("Invalid parameter for POSTUPDATE trigger.")
                        End If
                    Case tTrigger.PREDELETE
                        If Not IsNothing(OldObject) Then
                            tr.Trigger(TT, Nothing, OldObject, Cancel)
                        Else
                            Throw New Exception("Invalid parameter for PREDELETE trigger.")
                        End If
                    Case tTrigger.POSTDELETE
                        If Not IsNothing(OldObject) Then
                            tr.Trigger(TT, Nothing, OldObject, Nothing)
                        Else
                            Throw New Exception("Invalid parameter for POSTDELETE trigger.")
                        End If
                End Select
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Next
    End Sub
#End Region

End Class
