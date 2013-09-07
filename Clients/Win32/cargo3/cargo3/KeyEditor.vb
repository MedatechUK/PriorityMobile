Imports System.Windows.Forms

Public Class KeyEditor

    Private kd As New KeyDef

    'Private Sub txt_Plain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    Select Case e.KeyValue
    '        Case Keys.ShiftKey, Keys.ControlKey, Keys.Alt
    '            'e.Handled = True
    '        Case Else
    '            Dim tmp As String

    '            If e.Shift Then
    '                tmp = String.Format("KD(162),KD({0}),KU({0}),KU(162)", e.KeyValue)
    '            ElseIf e.Control Then
    '                Select Case e.KeyData
    '                    Case Keys.A, Keys.C, Keys.V
    '                        e.Handled = True
    '                        txt_Plain.Text += e.KeyData
    '                End Select
    '                tmp = String.Format("KD(162),KD({0}),KU({0}),KU(162)", e.KeyValue)
    '            ElseIf e.Alt Then
    '                tmp = String.Format("KD(162),KD({0}),KU({0}),KU(162)", e.KeyValue)
    '            Else
    '                tmp = String.Format("KB_CLK({0})", e.KeyValue)
    '            End If
    '            If Not (txt_Encoded.Text.Split(",").Last = tmp) Then
    '                If txt_Encoded.Text.Length > 0 Then txt_Encoded.Text += ","
    '                txt_Encoded.Text += tmp
    '            End If
    '    End Select

    'End Sub

    'Private Sub txt_Plain_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Plain.KeyUp
    '    Select Case e.KeyValue
    '        Case Keys.ShiftKey, Keys.ControlKey
    '            e.Handled = True
    '        Case Else
    '            'If Not Microsoft.VisualBasic.Right(txt_Encoded.Text, String.Format("KD({0})", e.KeyValue).Length) = String.Format("KD({0})", e.KeyValue) Then
    '            If txt_Encoded.Text.Length > 0 Then txt_Encoded.Text += ","

    '            'End If
    '            Select Case e.Modifiers
    '                Case Keys.Shift
    '                    txt_Encoded.Text += String.Format("KD(162),KU({0}),KU({0}),KD(162)", e.KeyValue)
    '                Case Keys.Alt
    '                    txt_Encoded.Text += String.Format("KD(162),KU({0}),KU({0}),KD(162)", e.KeyValue)
    '                Case Keys.Control
    '                    txt_Encoded.Text += String.Format("KD(162),KU({0}),KU({0}),KD(162)", e.KeyValue)
    '                Case Else
    '                    txt_Encoded.Text += String.Format("KU({0}),KU({0})", e.KeyValue)
    '            End Select
    '    End Select
    'End Sub

    Private Sub KeyEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each k As String In kd.Values
            Me.ComboBox1.Items.Add(k)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim tx As New System.Text.StringBuilder

        For Each i As Integer In kd.Keys
            If Me.ComboBox1.SelectedItem = kd(i) Then
                If Chk_Ctrl.Checked Then
                    If tx.ToString.Length > 0 Then tx.Append(",")
                    tx.AppendFormat("{0}(ctrl)", "KD")
                End If

                If Chk_Shift.Checked Then
                    If tx.ToString.Length > 0 Then tx.Append(",")
                    tx.AppendFormat("{0}(shift)", "KD")
                End If

                If chk_Alt.Checked Then
                    If tx.ToString.Length > 0 Then tx.Append(",")
                    tx.AppendFormat("{0}(alt)", "KD")
                End If

                If tx.ToString.Length > 0 Then tx.Append(",")
                tx.AppendFormat("KD({0}),KU({0})", i)

                If chk_Alt.Checked Then
                    tx.AppendFormat(",{0}(alt)", "KU")
                End If

                If Chk_Shift.Checked Then
                    tx.AppendFormat(",{0}(shift)", "KU")
                End If

                If Chk_Ctrl.Checked Then
                    tx.AppendFormat(",{0}(ctrl)", "KU")
                End If

                With txt_Encoded
                    If .Text.Length > 0 Then .Text += ","
                    .Text += tx.ToString
                End With

                Exit For

            End If
        Next
    End Sub

End Class

Public Class KeyDef
    Inherits Dictionary(Of Integer, String)

    Public Sub New()
        With Me
            .Add(48, "0")
            .Add(49, "1")
            .Add(50, "2")
            .Add(51, "3")
            .Add(52, "4")
            .Add(53, "5")
            .Add(54, "6")
            .Add(55, "7")
            .Add(56, "8")
            .Add(57, "9")
            .Add(65, "A")
            .Add(66, "B")
            .Add(67, "C")
            .Add(68, "D")
            .Add(69, "E")
            .Add(70, "F")
            .Add(71, "G")
            .Add(72, "H")
            .Add(73, "I")
            .Add(74, "J")
            .Add(75, "K")
            .Add(76, "L")
            .Add(77, "M")
            .Add(78, "N")
            .Add(79, "O")
            .Add(80, "P")
            .Add(81, "Q")
            .Add(82, "R")
            .Add(83, "S")
            .Add(84, "T")
            .Add(85, "U")
            .Add(86, "V")
            .Add(87, "W")
            .Add(88, "X")
            .Add(89, "Y")
            .Add(90, "Z")
            .Add(45, "Insert")
            .Add(36, "Home")
            .Add(33, "Page Up")
            .Add(46, "Delete")
            .Add(35, "End")
            .Add(34, "Page Down")
            .Add(9, "Tab")
            .Add(8, "Backspace")
            .Add(13, "Enter")
            .Add(27, "Escape")
            .Add(32, "Space")
            .Add(37, "%")
            .Add(38, "&")
            .Add(39, "'")
            .Add(40, "(")
            .Add(41, ")")
            .Add(42, "*")
            .Add(43, "+")
            .Add(44, ",")
            .Add(47, "/")
            .Add(112, "F1")
            .Add(113, "F2")
            .Add(114, "F3")
            .Add(115, "F4")
            .Add(116, "F5")
            .Add(117, "F6")
            .Add(118, "F7")
            .Add(119, "F8")
            .Add(120, "F9")
            .Add(121, "F10")
            .Add(122, "F11")
            .Add(123, "F12")
            .Add(91, "[")
            .Add(92, "\")
            .Add(93, "]")
            .Add(94, "^")
            .Add(95, "_")
            .Add(96, "`")

            .Add(126, "~")
            .Add(58, ":")
            .Add(59, ";")
            .Add(60, "<")
            .Add(61, "=")
            .Add(62, ">")
            .Add(63, "?")
            .Add(64, "@")
        End With
    End Sub
End Class