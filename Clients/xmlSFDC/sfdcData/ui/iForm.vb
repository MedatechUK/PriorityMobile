Imports System.Windows.Forms

Public Class iForm

    Public Sub New(ByRef intf As sfdc3.cInterface)
        InitializeComponent()

        With Me
            With .FormView
                .Load(Me, intf.Form)
            End With
            With .TableView
                .Load(Me, intf.Table)
            End With

            Width = Screen.PrimaryScreen.WorkingArea.Width
            Height = Screen.PrimaryScreen.WorkingArea.Height - 50

            Dim p As System.Drawing.Point
            p.X = (Screen.PrimaryScreen.WorkingArea.Width - .Width) / 2
            p.Y = (Screen.PrimaryScreen.WorkingArea.Height - .Height) / 2
            .Location = p

        End With

    End Sub

End Class