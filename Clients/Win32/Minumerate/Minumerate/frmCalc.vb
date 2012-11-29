Public Class frmCalc

    Private ReadOnly Property FormDate() As DateTime
        Get
            Dim DT As New DateTime(0)
            With Me.DatePick.Value
                DT = DT.AddYears(.Year - 1)
                DT = DT.AddMonths(.Month - 1)
                DT = DT.AddDays(.Day - 1)
            End With
            DT = DT.AddHours(CInt(Me.txtHr.Text))
            DT = DT.AddMinutes(CInt(Me.txtMin.Text))
            Return DT
        End Get
    End Property

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetDateTime(Now)
        txtPriMin.Text = DateToMin(FormDate)
        Me.Location = My.Settings("CalcLocation")
        If Me.Location.X = 0 And Me.Location.Y = 0 Then
            With My.Settings("formLocation")
                Dim P As New System.Drawing.Point(.X + 50, .Y + 50)
                Me.Location = P
                My.Settings.CalcLocation = P
                My.Settings.Save()
            End With
        End If
    End Sub

    Private Sub hVScroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles VScrollHour.Scroll, VScrollMin.Scroll

        Dim scroll As VScrollBar = sender
        Dim text As TextBox

        Select Case scroll.Name
            Case "VScrollHour"
                text = txtHr
            Case "VScrollMin"
                text = txtMin
            Case Else
                Exit Sub
        End Select

        text.Text = Strings.Right("00" & scroll.Maximum - scroll.Value.ToString, 2)
        txtPriMin.Text = DateToMin(FormDate)

    End Sub

    Private Sub hkeydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHr.KeyDown, txtMin.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            htxt_LostFocus(sender, New System.EventArgs)
        End If
    End Sub

    Private Sub htxt_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHr.LostFocus, txtMin.LostFocus

        Dim scroll As VScrollBar
        Dim text As TextBox = sender

        Select Case text.Name
            Case "txtHr"
                scroll = Me.VScrollHour
            Case "txtMin"
                scroll = Me.VScrollMin
            Case Else
                Exit Sub
        End Select

        If Not IsNumeric(text.Text) Then
            text.Text = "00"
            txtPriMin.Text = DateToMin(FormDate)
            Exit Sub
        End If

        If CInt(text.Text) > scroll.Maximum Then text.Text = scroll.Maximum
        If CInt(text.Text) < scroll.Minimum Then text.Text = scroll.Minimum

        text.Text = Strings.Right("00" & text.Text, 2)
        scroll.Value = scroll.Maximum - CInt(text.Text)
        txtPriMin.Text = DateToMin(FormDate)

    End Sub

    Private Sub DatePick_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatePick.ValueChanged
        txtPriMin.Text = DateToMin(FormDate)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Clipboard.SetText(txtPriMin.Text)
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        PasteToolStripMenuItem.Enabled = False
        If Not IsNothing(Clipboard.GetText) Then
            If Clipboard.GetText.Length > 0 Then
                If IsNumeric(Clipboard.GetText) Then
                    PasteToolStripMenuItem.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        SetDateTime(DateFromInt(Clipboard.GetText))
    End Sub

    Private Sub SetDateTime(ByVal newdt As DateTime)

        Dim DT As New DateTime(0)

        DT = DT.AddYears(newdt.Year - 1)
        DT = DT.AddMonths(newdt.Month - 1)
        DT = DT.AddDays(newdt.Day - 1)
        Me.DatePick.Value = DT

        txtHr.Text = Strings.Right("00" & newdt.Hour.ToString, 2)
        txtMin.Text = Strings.Right("00" & newdt.Minute.ToString, 2)

        Me.txtPriMin.Text = Clipboard.GetText

        Me.VScrollHour.Value = Me.VScrollHour.Maximum - CInt(txtHr.Text)
        Me.VScrollMin.Value = Me.VScrollMin.Maximum - CInt(txtMin.Text)

    End Sub

    Private Sub frmCalc_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        My.Settings("CalcLocation") = Me.Location
        My.Settings.Save()
    End Sub

End Class
