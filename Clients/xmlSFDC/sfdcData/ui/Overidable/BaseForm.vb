Imports System.Windows.Forms
Imports Microsoft.WindowsCE.Forms

Public Class BaseForm
    Public Sub New()
        InitializeComponent()
        With Me
            .BackColor = Drawing.Color.White
            Select Case SystemSettings.Platform
                Case WinCEPlatform.WinCEGeneric
                    .WindowState = FormWindowState.Maximized
                    .MaximizeBox = True
                    .MinimizeBox = True

                Case Else
                    .Width = Screen.PrimaryScreen.WorkingArea.Width
                    .Height = Screen.PrimaryScreen.WorkingArea.Height - 50

                    Dim p As System.Drawing.Point
                    p.X = (Screen.PrimaryScreen.WorkingArea.Width - .Width) / 2
                    p.Y = (Screen.PrimaryScreen.WorkingArea.Height - .Height) / 2
                    .Location = p

            End Select
        End With
    End Sub
End Class