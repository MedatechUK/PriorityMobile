Imports ntSettings

Public Class Settings
    Private _xS As xmlSettings
    Public Sub New(ByRef xS As xmlSettings)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _xS = xS
    End Sub

    Private Sub Settings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Props.SelectedObject = New SimpleProperties(_xS)
    End Sub
End Class