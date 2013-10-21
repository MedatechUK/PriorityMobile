Public Class Folder : Inherits System.Collections.Generic.Dictionary(Of String, Folder)

    Public Sub New(ByVal Name As String, ByVal Depth As Integer)
        _Name = Name
        _Depth = Depth
    End Sub

    Private _Files As New Dictionary(Of String, CodeFile)
    Public Property Files() As Dictionary(Of String, CodeFile)
        Get
            Return _Files
        End Get
        Set(ByVal value As Dictionary(Of String, CodeFile))
            _Files = value
        End Set
    End Property

    Private _Name As String
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Depth As Integer
    Public ReadOnly Property Depth() As Integer
        Get
            Return _Depth
        End Get
    End Property

End Class
