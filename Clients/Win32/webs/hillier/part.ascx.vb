
Partial Class part
    Inherits System.Web.UI.UserControl

    Private _seeme As Boolean = False

    Public Property seeme() As Boolean
        Get
            Return _seeme
        End Get
        Set(ByVal value As Boolean)
            _seeme = value
        End Set
    End Property

    Sub New(ByVal ws As priwebsvc.Service)
        txtPartName.Text = Me.ID
    End Sub
    Sub New()
        txtPartName.Text = Me.ID
    End Sub

End Class
