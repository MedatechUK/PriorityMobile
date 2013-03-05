Public Class frmView

    Private thisImg As Bitmap
    Private AspectRatio As Double

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Sub New(ByVal Filename As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            thisImg = New Bitmap(Filename)
            AspectRatio = thisImg.Width / thisImg.Height
            With Me
                'Dim BorderWidth As Integer = (.Width - .ClientSize.Width) / 2
                Dim TitlebarHeight As Integer = .Height - .ClientSize.Height - 2 * ((.Width - .ClientSize.Width) / 2)
                .Height = thisImg.Height + TitlebarHeight
                .Width = thisImg.Width
                .img.Image = thisImg
                .Text = String.Format("Viewing {0}.", Filename)
            End With
        Catch ex As Exception
            MsgBox(String.Format("An exception occured opening [{0}]. {1}.", Filename, ex.Message))
            End
        End Try
    End Sub

    Private Sub frmView_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

    End Sub

    Private Sub frmView_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        With Me
            .SuspendLayout()
            .Width = .Height * AspectRatio
            .ResumeLayout()
        End With
    End Sub

End Class
