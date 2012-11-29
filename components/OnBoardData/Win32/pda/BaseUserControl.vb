Partial Public MustInherit Class BaseUserControl
    Inherits System.Windows.Forms.UserControl

    Dim ar As Priority.MyArray

    Dim _ServiceCall As String = ""
    Dim _App As PDAOnBoardData.BaseForm

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            NewData()
        End Set
    End Property

    Public Sub New(Optional ByRef App As BaseForm = Nothing) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            _App = App
        End If

    End Sub

    Public MustOverride Sub NewData()

End Class
