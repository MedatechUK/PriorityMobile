Imports System.Reflection
Imports system.io
Imports System.Threading
Imports System.ComponentModel

Public Class oSync

#Region "Initialisation / finalisation"

    Private _sync As SyncBase
    Private _TagetType As Type
    Private ReadOnly Property TagetType() As Type
        Get
            Return _TagetType
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _sync.ToString
    End Function

    Public ReadOnly Property SyncDes() As String
        Get
            Return _sync.SyncDes
        End Get        
    End Property

    Public ReadOnly Property FormName() As String
        Get
            Return _sync.FormName
        End Get
    End Property

    Public ReadOnly Property FormInherit() As Boolean
        Get
            Return _sync.FormInherit
        End Get
    End Property

    Public Sub New(ByRef Parent As oDataSet, ByVal dllName As String, ByVal className As String)

        Dim TargetAssembly As Assembly = Assembly.LoadFrom(dllName)
        _TagetType = TargetAssembly.GetType(className, True)
        _sync = Activator.CreateInstance(TagetType)
        _sync.SetParent(Parent)

    End Sub

    Public Sub DoSync()
        _sync.Sync()
    End Sub

#End Region

End Class
