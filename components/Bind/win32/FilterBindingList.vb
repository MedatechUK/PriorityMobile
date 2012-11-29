'---------------------------------------------------------------------
'  Copyright (C) Microsoft Corporation.  All rights reserved.
' 
'THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
'KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
'IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
'PARTICULAR PURPOSE.
'---------------------------------------------------------------------

Option Explicit On
'Option Strict On

Imports System.ComponentModel
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports Bind

Public Class FilteredBindingList(Of T)
    Inherits BindingList(Of T)
    Implements IBindingListView
    Private del As New List(Of Integer)

    Public Sub New()

    End Sub

    Public Sub ClearMyItems()
        Me.OriginalList.Clear()
        Me.Clear()
    End Sub

    Private _Bind As oBind
    Public Property Bind() As oBind
        Get
            Return _Bind
        End Get
        Set(ByVal value As oBind)
            _Bind = value
        End Set
    End Property

    Private _KeyProperty As PropertyInfo = Nothing
    Private ReadOnly Property KeyProperty() As PropertyInfo
        Get
            If IsNothing(_KeyProperty) Then
                _KeyProperty = GetType(T).GetProperty("Key")
            End If
            Return _KeyProperty
        End Get
    End Property

    Public Sub RemoveMyItem(ByVal i As Integer, ByVal k As String)
        For Each o As T In Me
            If String.Compare(k, KeyProperty.GetValue(o, Nothing), False) = 0 Then
                Remove(o)
                Exit For
            End If
        Next
        For Each o As T In originalListValue
            If String.Compare(k, KeyProperty.GetValue(o, Nothing), False) = 0 Then
                originalList.Remove(o)
                Exit For
            End If
        Next
    End Sub

    Public Function GetMyItem(ByVal i As Integer) As Object
        Return Me.OriginalList.Item(i)
    End Function

    Public Sub SetMyItem(ByVal i As Integer, ByVal o As Object)
        Me.OriginalList.Item(i) = o
    End Sub

    Private originalListValue As List(Of T) = New List(Of T)
    Private Property OriginalList() As List(Of T)
        Get
            Return originalListValue
        End Get
        Set(ByVal value As List(Of T))
            originalListValue = value
        End Set
    End Property

    Public Function ObjectList() As List(Of Object)
        Dim l As New List(Of Object)
        With l
            For Each o As Object In originalListValue
                .Add(o)
            Next
        End With
        Return l
    End Function

    Public Sub AddItem(ByVal NewItem As T)
        Try
            Me.Add(NewItem)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

#Region "Searching"

    Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides Function FindCore(ByVal prop As System.ComponentModel.PropertyDescriptor, ByVal key As Object) As Integer
        ' Get the property info for the specified property.
        Dim propInfo As PropertyInfo = GetType(T).GetProperty(prop.Name)
        Dim item As T

        If (key IsNot Nothing) Then
            'Loop through the items to see if the key
            'value matches the property value.
            Dim i As Integer
            For i = 0 To Count - 1
                item = CType(Items(i), T)
                If (propInfo.GetValue(item, Nothing).Equals(key)) Then
                    Return i
                End If
            Next
        End If
        Return -1
    End Function

    Public Function Find(ByVal propertyName As String, ByVal key As Object) As Integer
        ' Check the properties for a property with the specified name.
        Dim properties As PropertyDescriptorCollection = _
            TypeDescriptor.GetProperties(GetType(T))
        Dim prop As PropertyDescriptor = properties.Find(propertyName, True)

        ' If there is not a match, return -1 otherwise pass search to
        ' FindCore method.
        If (prop Is Nothing) Then
            Return -1
        Else
            Return FindCore(prop, key)
        End If
    End Function

#End Region

#Region "Sorting"
    Private sortedList As ArrayList
    Private unsortedItems As FilteredBindingList(Of T)
    Private isSortedValue As Boolean
    Private sortDirectionValue As ListSortDirection
    Private sortPropertyValue As PropertyDescriptor

    Protected Overrides ReadOnly Property SupportsSortingCore() As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property IsSortedCore() As Boolean
        Get
            Return isSortedValue
        End Get
    End Property

    Protected Overrides ReadOnly Property SortPropertyCore() As System.ComponentModel.PropertyDescriptor
        Get
            Return sortPropertyValue
        End Get
    End Property

    Protected Overrides ReadOnly Property SortDirectionCore() As System.ComponentModel.ListSortDirection
        Get
            Return sortDirectionValue
        End Get
    End Property

    Public Sub ApplySort(ByVal propertyName As String, ByVal direction As ListSortDirection)
        ' Check the properties for a property with the specified name.
        Dim prop As PropertyDescriptor = TypeDescriptor.GetProperties(GetType(T))(propertyName)

        ' If there is not a match, return -1 otherwise pass search to
        ' FindCore method.
        If (prop Is Nothing) Then
            Throw New ArgumentException(propertyName + _
                    " is not a valid property for type:" + GetType(T).Name)
        Else
            ApplySortCore(prop, direction)
        End If
    End Sub

    Protected Overrides Sub ApplySortCore(ByVal prop As System.ComponentModel.PropertyDescriptor, ByVal direction As System.ComponentModel.ListSortDirection)
        sortedList = New ArrayList()

        ' Check to see if the property type we are sorting by implements
        ' the IComparable interface.
        'Dim interfaceType As Type '= prop.PropertyType.GetInterface("IComparable")

        'If (interfaceType IsNot Nothing) Then
        ' If so, set the SortPropertyValue and SortDirectionValue.
        sortPropertyValue = prop
        sortDirectionValue = direction

        unsortedItems = New FilteredBindingList(Of T)
        unsortedItems.Bind = Me.Bind
        If (originalListValue.Count = 0) Then
            'originalListValue = New List(Of T)
            'originalListValue.AddRange(Me.Items)
        End If

        If (sortPropertyValue IsNot Nothing) Then
            ' Loop through each item, adding it the the sortedItems ArrayList.
            Dim item As Object
            For Each item In Me.Items
                unsortedItems.Add(CType(item, T))
                sortedList.Add(prop.GetValue(item))
            Next
        End If
        ' Call Sort on the ArrayList.
        sortedList.Sort()
        Dim temp As T

        ' Check the sort direction and then copy the sorted items
        ' back into the list.
        If (direction = ListSortDirection.Descending) Then
            sortedList.Reverse()
        End If

        Dim i As Integer
        For i = 0 To Me.Count - 1
            Dim position As Integer = Find(prop.Name, sortedList(i))
            If (position <> i And position > 0) Then

                Dim t() As System.Type = {GetType(Boolean)}
                Dim targs() As Object = {True}
                Dim fargs() As Object = {False}
                GetType(T).GetMethod("SetLoaded", t).Invoke(Me(i), fargs)
                GetType(T).GetMethod("SetLoaded", t).Invoke(Me(position), fargs)

                temp = Me(i)
                Me(i) = Me(position)
                Me(position) = temp

                GetType(T).GetMethod("SetLoaded", t).Invoke(Me(i), targs)
                GetType(T).GetMethod("SetLoaded", t).Invoke(Me(position), targs)

            End If
        Next

        isSortedValue = True

        ' Raise the ListChanged event so bound controls refresh their
        ' values. Pass -1 for the index since this is a Reset.

        OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))

        'Else
        '' If the property type does not implement IComparable, let the user
        '' know.
        'Throw New InvalidOperationException("Cannot sort by " _
        '    + prop.Name + ". This" + prop.PropertyType.ToString() + _
        '    " does not implement IComparable")
        'End If
    End Sub

    Protected Overrides Sub RemoveSortCore()
        Me.RaiseListChangedEvents = False
        ' Ensure the list has been sorted.
        If (unsortedItems IsNot Nothing And originalListValue.Count > 0) Then
            Me.Clear()
            If (Filter IsNot Nothing) Then
                unsortedItems.Filter = Me.Filter
                Dim item As T
                For Each item In unsortedItems
                    Me.Add(item)
                Next
            Else
                For Each item As T In OriginalList()
                    Me.Add(item)
                Next
            End If
            isSortedValue = False
            Me.RaiseListChangedEvents = True
            ' Raise the list changed event, indicating a reset, and index
            ' of -1.
            OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
        End If
    End Sub

    Public Sub RemoveSort()
        RemoveSortCore()
    End Sub

    Public Overrides Sub EndNew(ByVal itemIndex As Integer)
        ' Check to see if the item is added to the end of the list,
        ' and if so, re-sort the list.
        If (IsSortedCore And itemIndex > 0 _
            And itemIndex = Me.Count - 1) Then
            ApplySortCore(Me.sortPropertyValue, _
                Me.sortDirectionValue)
            Select Case Bind.CancelEdit(Me(itemIndex))
                Case False
                    MyBase.EndNew(itemIndex)
                Case True
                    MyBase.CancelNew(itemIndex)
            End Select
        End If
    End Sub

#End Region

#Region "AdvancedSorting"

    Public ReadOnly Property SupportsAdvancedSorting() As Boolean Implements System.ComponentModel.IBindingListView.SupportsAdvancedSorting
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SortDescriptions() As System.ComponentModel.ListSortDescriptionCollection Implements System.ComponentModel.IBindingListView.SortDescriptions
        Get
            Return Nothing
        End Get
    End Property

    Public Sub ApplySort(ByVal sorts As System.ComponentModel.ListSortDescriptionCollection) Implements System.ComponentModel.IBindingListView.ApplySort
        Throw New NotSupportedException()
    End Sub

#End Region

#Region "Filtering"

    Public ReadOnly Property SupportsFiltering() As Boolean Implements System.ComponentModel.IBindingListView.SupportsFiltering
        Get
            Return True
        End Get
    End Property

    Public Sub RemoveFilter() Implements System.ComponentModel.IBindingListView.RemoveFilter
        If (Filter IsNot Nothing) Then
            Filter = Nothing
        End If
    End Sub

    Private filterValue As String = Nothing

    Public Property Filter() As String Implements System.ComponentModel.IBindingListView.Filter
        Get
            Return filterValue
        End Get
        Set(ByVal value As String)
            If filterValue = value Then
                Return
            End If
            ' If the value is not null or empty, but doesn't
            ' match expected format, throw an exception.
            If Not String.IsNullOrEmpty(value) AndAlso Not Regex.IsMatch(value, BuildRegExForFilterFormat(), RegexOptions.Singleline) Then
                Throw New ArgumentException("Filter is not in " + "the format: propName[<>=]'value'.")
            End If

            'Turn off list-changed events.
            RaiseListChangedEvents = False

            ' If the value is null or empty, reset list.
            If String.IsNullOrEmpty(value) Then
                ResetList()
            Else
                ResetList()

                Dim count As Integer = 0
                Dim matches As String() = Split(value, " AND ", , CompareMethod.Text)

                While count < matches.Length
                    Dim filterPart As String = matches(count).ToString()

                    ' Check to see if the filter was set previously.
                    ' Also, check if current filter is a subset of 
                    ' the previous filter.
                    'If Not String.IsNullOrEmpty(filterValue) Then 'AndAlso Not value.Contains(filterValue)
                    '    ResetList()
                    'End If
                    ' Parse and apply the filter.
                    Dim filterInfo As SingleFilterInfo = ParseFilter(filterPart)
                    ApplyFilter(filterInfo)
                    count += 1
                End While

            End If

            ' Set the filter value and turn on list changed events.
            filterValue = value
            RaiseListChangedEvents = True
            OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
        End Set
    End Property

    ' Build a regular expression to determine if filter is in correct format.
    Friend Shared Function BuildRegExForFilterFormat() As String
        Dim regex As StringBuilder = New StringBuilder()

        ' Look for optional literal brackets, 
        ' followed by word characters or space.
        regex.Append("\[?[\w\s]+\]?\s?")

        ' Add the operators: > < or =.
        regex.Append("[><=]")

        'Add optional space followed by optional quote and
        ' any character followed by the optional quote.
        regex.Append("\s?'?.+'?")

        Return regex.ToString()
    End Function

    Private Sub ResetList()
        Me.ClearItems()
        Dim _t As T
        For Each _t In OriginalList()
            Me.Items.Add(_t)
        Next
        If (IsSortedCore) Then
            ApplySortCore(SortPropertyCore, SortDirectionCore)
        End If
    End Sub

    Protected Overrides Sub OnListChanged(ByVal e As System.ComponentModel.ListChangedEventArgs)
        Dim Cancel As Boolean = False

        ' If the list is reset, check for a filter. If a filter 
        ' is applied don't allow items to be added to the list.
        If (e.ListChangedType = ListChangedType.Reset) Then
            If (Filter = Nothing Or Filter = "") Then
                AllowNew = True
            Else
                AllowNew = False
            End If
        End If

        ' Add the new item to the original list.
        If (e.ListChangedType = ListChangedType.ItemAdded) Then ' adding
            If Not Bind.Loaded(Me(e.NewIndex)) Then ' not loaded
                If Not Bind.Loading Then  ' parent notloading
                    'Bind.Trigger(tTrigger.PREINSERT, Me(e.NewIndex), , Cancel)
                End If
                If Not Cancel Then
                    Cancel = Not Bind.ContainsKey(Bind.KeyProperty.GetValue(Me(e.NewIndex), Nothing)) = -1
                    If Cancel Then
                        MsgBox("The key of the added record already exists.")
                        Bind.CancelEdit(Me(e.NewIndex)) = True
                        'RemoveMyItem(e.NewIndex, KeyProperty.GetValue(Me(e.NewIndex), Nothing))
                        Remove(Me(e.NewIndex))
                    End If
                    If Not Cancel Then
                        Bind.PropertyChanged.AddEventHandler(Me(e.NewIndex), _
                           New EventHandler(AddressOf Bind.hPropertyChanged))
                        Bind.PropertyBeginEdit.AddEventHandler(Me(e.NewIndex), _
                            New EventHandler(AddressOf Bind.hBeginEdit))
                        Bind.Loaded(Me(e.NewIndex)) = True
                        If Not Bind.Loading Then _
                            Bind.Trigger(tTrigger.POSTINSERT, Me(e.NewIndex))
                        OriginalList.Add(Me(e.NewIndex))
                        MyBase.OnListChanged(e)
                    End If
                Else
                    Bind.CancelEdit(Me(e.NewIndex)) = True
                    Remove(Me(e.NewIndex))
                End If
            End If ' end not loadeD
            If (Not (String.IsNullOrEmpty(Filter))) Then
                Dim cachedFilter As String = Me.Filter
                Me.Filter = ""
                Me.Filter = cachedFilter
            End If
        End If

        ' Remove the new item from the original list.
        Try
            If (e.ListChangedType = ListChangedType.ItemDeleted) Then
                If Not Bind.CancelEdit(Me(e.NewIndex)) Then
                    Bind.Trigger(tTrigger.PREDELETE, , Me(e.NewIndex), Cancel)
                    If Not Cancel Then
                        OriginalList.RemoveAt(e.NewIndex)
                        Bind.Trigger(tTrigger.PREDELETE, , Me(e.NewIndex))
                        MyBase.OnListChanged(e)
                    End If
                End If
            End If            
        Catch
        End Try

    End Sub


    Friend Sub ApplyFilter(ByVal filterParts As SingleFilterInfo)
        'Dim results As List(Of T)

        ' Check to see if the property type we are filtering by implements
        ' the IComparable interface.
        'Dim interfaceType As Type '= _
        'TypeDescriptor.GetProperties(GetType(T))(filterParts.PropName) _
        '.PropertyType.GetInterface("IComparable")

        'If (interfaceType Is Nothing) Then
        '    Throw New InvalidOperationException("Filtered property" + _
        '    " must implement IComparable.")
        'End If

        Dim results As New List(Of T)

        ' Check each value and add to the results list.
        Dim item As T
        For Each item In Me
            If (filterParts.PropDesc.GetValue(item) IsNot Nothing) Then
                Dim compareValue As IComparable _
                    = TryCast(filterParts.PropDesc.GetValue(item), IComparable)
                Dim result As Integer
                Select Case filterParts.PropDesc.GetValue(item).GetType.Name
                    Case "Int32"
                        result = compareValue.CompareTo(CInt(filterParts.CompareValue))
                        If (filterParts.OperatorValue = FilterOperator.EqualTo _
                            And result = 0) Then
                            results.Add(item)
                        End If
                        If (filterParts.OperatorValue = FilterOperator.GreaterThan _
                            And result > 0) Then
                            results.Add(item)
                        End If
                        If (filterParts.OperatorValue = FilterOperator.LessThan _
                            And result < 0) Then
                            results.Add(item)
                        End If
                    Case "String"
                        result = compareValue.CompareTo(filterParts.CompareValue)
                        If (filterParts.OperatorValue = FilterOperator.EqualTo _
                            And result = 0) Then
                            results.Add(item)
                        End If
                End Select
            End If
        Next
        Me.ClearItems()
        Dim itemFound As T
        For Each itemFound In results
            Me.Add(itemFound)
        Next
    End Sub


    Friend Function ParseFilter(ByVal filterPart As String) As SingleFilterInfo
        Dim filterInfo As SingleFilterInfo = New SingleFilterInfo()
        filterInfo.OperatorValue = DetermineFilterOperator(filterPart)

        Dim op As Char
        Select Case filterInfo.OperatorValue            
            Case FilterOperator.EqualTo
                op = "="c
            Case FilterOperator.LessThan
                op = "<"c
            Case FilterOperator.GreaterThan
                op = ">"c
            Case FilterOperator.None
                op = " "c
        End Select

        Dim filterStringParts As String() = _
            filterPart.Split(New Char() {op})

        filterInfo.PropName = _
            filterStringParts(0).Replace("[", ""). _
            Replace("]", "").Replace(" AND ", "").Trim()

        ' Get the property descriptor for the filter property name.
        Dim filterPropDesc As PropertyDescriptor = _
            TypeDescriptor.GetProperties(GetType(T))(filterInfo.PropName)

        ' Convert the filter compare value to the property type.
        If (filterPropDesc Is Nothing) Then
            Throw New InvalidOperationException("Specified property to " + _
                "filter " + filterInfo.PropName + _
                " on does not exist on type: " + GetType(T).Name)
        End If

        filterInfo.PropDesc = filterPropDesc

        Dim comparePartNoQuotes As String = StripOffQuotes(filterStringParts(1))
        Try
            Dim converter As TypeConverter = Nothing '_
            'TypeDescriptor.GetConverter(filterPropDesc.PropertyType)
            filterInfo.CompareValue = _
                comparePartNoQuotes
        Catch ex As NotSupportedException
            Throw New InvalidOperationException("Specified filter" + _
                "value " + comparePartNoQuotes + " can not be converted" + _
                "from string. Implement a type converter for " + _
                filterPropDesc.PropertyType.ToString())
        End Try
        Return filterInfo
    End Function


    Friend Function DetermineFilterOperator(ByVal filterPart As String) As FilterOperator
        ' Determine the filter's operator.
        If (Regex.IsMatch(filterPart, "[^>^<]=")) Then
            Return FilterOperator.EqualTo
        ElseIf (Regex.IsMatch(filterPart, "<[^>^=]")) Then
            Return FilterOperator.LessThan
        ElseIf (Regex.IsMatch(filterPart, "[^<]>[^=]")) Then
            Return FilterOperator.GreaterThan
        Else
            Return FilterOperator.None
        End If
    End Function


    Friend Shared Function StripOffQuotes(ByVal filterPart As String) As String
        ' Strip off quotes in compare value if they are present.
        If (Regex.IsMatch(filterPart, "'.+'")) Then
            Dim quote As Integer = filterPart.IndexOf("'"c)
            filterPart = filterPart.Remove(quote, 1)
            quote = filterPart.LastIndexOf("'"c)
            filterPart = filterPart.Remove(quote, 1)
            filterPart = filterPart.Trim()
        End If
        Return filterPart
    End Function

#End Region

End Class


