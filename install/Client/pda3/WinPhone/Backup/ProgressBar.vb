Public Class ProgressBar

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With Me
            .Menu = MainMenu1
            With Progress
                .Maximum = 100
                .Minimum = 0
                .Value = 0
            End With
        End With

    End Sub

End Class