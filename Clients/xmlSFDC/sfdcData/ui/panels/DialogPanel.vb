Imports System.Windows.Forms

Public Class DialogPanel
    Inherits iFormPanel

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _Parent
        End Get
    End Property

    Private _Parent As iForm
    Public Overloads ReadOnly Property Parent() As iForm
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Initialisation and finalisation"

    Public Sub Load(ByRef Parent As iForm)
        _Parent = Parent
    End Sub

#End Region

End Class
