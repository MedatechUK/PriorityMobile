Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.ComponentModel
Imports System.Reflection
Imports System.Threading

Public MustInherit Class DatasetObjectBase
    Implements INotifyPropertyChanged

    Private pi As System.Reflection.PropertyInfo
    MustOverride Function ConQuery() As String
    MustOverride Function Columns() As String()    
    MustOverride Function KeyColumns() As String()
    MustOverride Function UpdateColumns() As String()

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Public Event Changes As EventHandler
    Public Event BeginEdit As EventHandler

    Private _guid As String = Nothing

    Public Sub New()
        MyBase.New()
        _guid = System.Guid.NewGuid.ToString
    End Sub

    Public ReadOnly Property GUID() As String
        Get
            Return _guid
        End Get
    End Property

    Private _key As String = Nothing
    Public ReadOnly Property Key() As String
        Get
            If IsNothing(_key) Then
                Dim ret As String = ""
                For Each k As String In KeyColumns()
                    pi = Me.GetType().GetProperty(k)
                    Select Case pi.PropertyType.Name
                        Case "Int32"
                            ret += CStr(pi.GetValue(Me, Nothing))
                        Case "String"
                            ret += pi.GetValue(Me, Nothing)
                    End Select
                Next
                _key = ret
            End If
            Return _key
        End Get
    End Property

    Private _Loaded As Boolean = False
    Public Sub SetLoaded(ByVal b As Boolean)
        _Loaded = b
    End Sub
    Public Function GetLoaded() As Boolean
        Return _Loaded
    End Function
    Private _CancelEdit As Boolean = False
    Public Sub SetCancelEdit(ByVal b As Boolean)
        _CancelEdit = b
    End Sub
    Public Function GetCancelEdit() As Object
        Return _CancelEdit
    End Function
    Private _EditClone As Object = Nothing
    Public Sub SetEditClone(ByRef o As Object)
        _EditClone = o
    End Sub
    Public Function GetEditClone() As Object
        Return _EditClone
    End Function

    Public Sub Update(ByVal NewObject As Object)
        For Each k As String In UpdateColumns()
            pi = Me.GetType().GetProperty(k)
            pi.SetValue(Me, pi.GetValue(NewObject, Nothing), Nothing)
        Next
    End Sub

#Region "INotifyPropertyChanged Members"

    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        For Each s As String In KeyColumns
            If String.compare(propertyName, s, True) = 0 Then
                Dim ret As String = ""
                For Each k As String In KeyColumns()
                    pi = Me.GetType().GetProperty(k)
                    Select Case pi.PropertyType.Name
                        Case "Int32"
                            ret += CStr(pi.GetValue(Me, Nothing))
                        Case "String"
                            ret += pi.GetValue(Me, Nothing)
                    End Select
                Next
                _KEY = RET
            End If
        Next
        OnPropertyChanged(New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        If GetLoaded() Then
            RaiseEvent PropertyChanged(Me, e)
            RaiseEvent Changes(Me, New System.EventArgs())
        End If
    End Sub

    Public Sub OnPropertyChanging(ByVal propertyName As String, ByVal value As Object)
        If GetLoaded() Then
            SetCancelEdit(False)
            If IsNothing(_EditClone) Then
                SetEditClone(Activator.CreateInstance(Me.GetType))
            End If
            For Each col As String In Columns()
                If String.Compare(col, propertyName, True) = 0 Then
                    pi = Me.GetType.GetProperty(col)
                    pi.SetValue(GetEditClone, value, Nothing)
                Else
                    pi = Me.GetType.GetProperty(col)
                    pi.SetValue(GetEditClone, pi.GetValue(Me, Nothing), Nothing)
                End If
            Next
            RaiseEvent BeginEdit(Me, New System.EventArgs())
            If Not GetCancelEdit() Then
                SetLoaded(False)
                For Each col As String In Columns()
                    pi = Me.GetType.GetProperty(col)
                    pi.SetValue(Me, pi.GetValue(GetEditClone, Nothing), Nothing)
                Next
                SetLoaded(True)
            End If
        End If
    End Sub

#End Region

    Public Function Clone() As Object
        Dim r As Object = Activator.CreateInstance(Me.GetType)
        For Each col As String In Columns()
            pi = Me.GetType.GetProperty(col)
            pi.SetValue(r, pi.GetValue(Me, Nothing), Nothing)
        Next
        Return r
    End Function

    Public Overrides Function toString() As String
        Dim str As String = ""
        For Each col As String In Columns()
            pi = Me.GetType().GetProperty(col)
            Select Case pi.PropertyType.Name
                Case "Int32"
                    str += CStr(pi.GetValue(Me, Nothing))
                Case "String"
                    str += Replace(pi.GetValue(Me, Nothing), vbCrLf, "\n")
            End Select
            str += Chr(9)
        Next
        Return Left(str, str.Length - 1)
    End Function

End Class

