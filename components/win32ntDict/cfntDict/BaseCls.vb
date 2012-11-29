Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.ComponentModel
Imports System.Reflection
Imports System.Threading

Public Class DatasetObjectBase
    Implements INotifyPropertyChanged
    Private _id As String

    Public Sub New()
        MyBase.New()
        Me._id = System.Guid.NewGuid().ToString()
    End Sub

    Public Property ID() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

#Region "INotifyPropertyChanged Members"

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Public Event BasePropertyChanged(ByVal ID As String, ByVal NAME_Const As String, ByVal value As String)

    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        OnPropertyChanged(New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        Dim pi As System.Reflection.PropertyInfo = Me.GetType().GetProperty(e.PropertyName)
        RaiseEvent BasePropertyChanged(ID, e.PropertyName, pi.GetValue(Me, Nothing))
    End Sub

#End Region

End Class


Public Class oDictionary(Of t, DatasetObjectBase)
    Private WithEvents _Dictionary As New Dictionary(Of String, DatasetObjectBase)
    Private _myParent As dCls

#Region "Public Property Values"

    Public Sub SetMyParent(ByRef Sender As dCls)
        _myParent = Sender
    End Sub

    Default Public Property Item(ByVal key As [String]) As DatasetObjectBase
        Get
            Return _Dictionary.Item(key) 'CType(Dictionary(key), [Object])
        End Get
        Set(ByVal value As DatasetObjectBase)
            Dim oldValue As DatasetObjectBase
            oldValue = Item(key)
            _Dictionary.Item(key) = value 'Dictionary(key) = value
            _myParent.OnSetComplete(key, oldValue, value)
        End Set
    End Property

    Public ReadOnly Property Keys() As ICollection
        Get
            Return _Dictionary.Keys 'Return Dictionary.Keys
        End Get
    End Property

    Public ReadOnly Property Values() As ICollection
        Get
            Return _Dictionary.Values 'Return Dictionary.Values
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return _Dictionary.Count
        End Get
    End Property

    Public ReadOnly Property FirstKey() As String
        Get
            If _Dictionary.Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In _Dictionary.Keys
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
            If _Dictionary.Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In _Dictionary.Keys
                    If ordinal = _Dictionary.Count Then
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
        _Dictionary.Add(key, value) 'Dictionary.Add(key, value)
        _myParent.OnInsertComplete(key, value)
    End Sub 'Add

    Public Function ContainsKey(ByVal key As [String]) As Boolean
        Return _Dictionary.ContainsKey(key) ' Return Dictionary.Contains(key)
    End Function 'Contains

    Public Sub Remove(ByVal key As [String])
        Dim value As DatasetObjectBase
        value = Item(key)
        If _myParent.Undelete Then
            With _myParent.Deleted
                If .ContainsKey(key) Then
                    .Remove(key)
                End If
                Add(key, value)
            End With
        End If
        _Dictionary.Remove(key)
        _myParent.OnRemoveComplete(key, value)
    End Sub 'Remove

#End Region

#Region "Save to file on dataset changes"

    Protected Overloads Sub OnRemove(ByVal key As Object, ByVal value As Object)
        If _myParent.Undelete Then
            With _myParent.Deleted
                If ContainsKey(key) Then
                    Remove(key)
                End If
                Add(key, value)
            End With
        End If
        'MyBase.OnRemove(key, value)
        _myParent.OnRemoveComplete(key, value)
    End Sub

    Protected Overloads Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        'MyBase.OnInsertComplete(key, value)
        _myParent.OnInsertComplete(key, value)
    End Sub

    Protected Overloads Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        'MyBase.OnRemoveComplete(key, value)
        _myParent.OnRemoveComplete(key, value)
    End Sub

    Protected Overloads Sub OnSetComplete(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)
        'MyBase.OnSetComplete(key, oldValue, newValue)
        _myParent.OnSetComplete(key, oldValue, newValue)
    End Sub

#End Region

End Class

