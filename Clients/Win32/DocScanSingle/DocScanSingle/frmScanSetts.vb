Public Class frmScanSetts

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim pt As Integer = 0
        Dim mp As Integer = 0
        Dim int As Integer = 0
        Dim res As Integer = 0
        Select Case ComboBox1.Text
            Case ""
                Exit Sub
            Case "Black and White"
                pt = 0
            Case "Greyscale"
                pt = 1
            Case "RGB Colour"
                pt = 2
            Case "Palette Colour"
                pt = 3
            Case Else
                Exit Sub
        End Select
        mp = NumericUpDown1.Value
        Select Case ComboBox2.Text
            Case ""
                Exit Sub
            Case "Show Interface"
                int = 1
            Case "Hide Interface"
                int = 0
            Case "Modal Interface"
                int = 2
            Case Else
                Exit Sub
        End Select
        res = NumericUpDown2.Value
        Dim wr As Boolean = ScanControl.ScanSettings.writesettings(pt, mp, int, res)
        Select Case wr
            Case True
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Case Else
                MsgBox("Settings failed to write please try again", MsgBoxStyle.Critical, "Error!")
                'Me.DialogResult = Windows.Forms.DialogResult.Abort
        End Select
    End Sub

    Private Sub frmScanSetts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class