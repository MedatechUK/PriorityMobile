Public Class scan_Reg

    Private Sub scan_Reg_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        With Me
            .txtReg.Focus()
            .Timer1.Enabled = False
        End With

    End Sub

    Private Sub txtReg_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtReg.KeyDown
        If e.KeyValue = 13 Then
            If txtReg.Text.Length <= 7 Then
                Me.Close()
            Else
                txtReg.Text = String.Empty
                Beep()
            End If
        End If
    End Sub

End Class