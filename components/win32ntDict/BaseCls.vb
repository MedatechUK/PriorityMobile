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
    Inherits DictionaryBase

    Private _myParent As dCls

#Region "Public Property Values"

    Public Sub SetMyParent(ByRef Sender As dCls)
        _myParent = Sender
    End Sub

    Default Public Property Item(ByVal key As [String]) As [Object]
        Get
            Return CType(Dictionary(key), [Object])
        End Get
        Set(ByVal value As [Object])
            Dictionary(key) = value
        End Set
    End Property

    Public ReadOnly Property Keys() As ICollection
        Get
            Return Dictionary.Keys
        End Get
    End Property

    Public ReadOnly Property Values() As ICollection
        Get
            Return Dictionary.Values
        End Get
    End Property

    Public ReadOnly Property FirstKey() As String
        Get
            If Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In Keys
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
            If Count > 0 Then
                Dim ordinal As Integer = 1
                For Each k As String In Keys
                    If ordinal = Count Then
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
        Dictionary.Add(key, value)
    End Sub 'Add

    Public Function ContainsKey(ByVal key As [String]) As Boolean
        Return Dictionary.Contains(key)
    End Function 'Contains

    Public Sub Remove(ByVal key As [String])
        If ContainsKey(key) Then
            Dictionary.Remove(key)
        End If
    End Sub 'Remove

#End Region

#Region "Save to file on dataset changes"

    Protected Overrides Sub OnRemove(ByVal key As Object, ByVal value As Object)
        If _myParent.Undelete Then
            With _myParent.Deleted
                If .Containskey(key) Then
                    .Remove(key)
                End If
                .Add(key, value)
            End With
        End If
        MyBase.OnRemove(key, value)
        _myParent.OnRemove(key, value)
    End Sub

    Protected Overrides Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnInsertComplete(key, value)
        _myParent.OnInsertComplete(key, value)
    End Sub

    Protected Overrides Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnRemoveComplete(key, value)
        _myParent.OnRemoveComplete(key, value)
    End Sub

    Protected Overrides Sub OnSetComplete(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)
        MyBase.OnSetComplete(key, oldValue, newValue)
        _myParent.OnSetComplete(key, oldValue, newValue)
    End Sub

#End Region

End Class

