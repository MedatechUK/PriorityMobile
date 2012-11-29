Imports System.Windows.Forms

Public Class Splash

    Private Sub Splash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cursor.Current = Cursors.Default
        Application.DoEvents()
        With Me
            .Visible = False
            .Width = 0
            .Height = 0
            .Top = Windows.Forms.Screen.PrimaryScreen.Bounds.Height
            .Left = Windows.Forms.Screen.PrimaryScreen.Bounds.Width
        End With
        Cursor.Current = Nothing        
        Application.DoEvents()
    End Sub

End Class