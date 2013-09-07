Imports System.Windows.Forms

Public MustInherit Class iFormPanel
    Inherits panel

    Public MustOverride ReadOnly Property ParentForm() As iForm

End Class

Public MustInherit Class iFormChild    
    Inherits System.Windows.Forms.UserControl

    Public MustOverride ReadOnly Property ParentForm() As iForm

    Friend _Container As cContainer
    Public ReadOnly Property Container() As cContainer
        Get
            Return _Container
        End Get
    End Property

End Class