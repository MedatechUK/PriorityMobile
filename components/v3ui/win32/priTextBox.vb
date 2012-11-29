Imports System.Text.RegularExpressions

Public Class priTextBox

    Overrides Sub NewFont(ByVal FormFont As System.Drawing.Font)
        TextBox.Font = FormFont
    End Sub
    Public Overrides Sub SetReadOnly()
        TextBox.Enabled = Not (MyBase.IsReadOnly)
    End Sub
    Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Return TextBox.Text
            Else
                With DG
                    If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                        Return ""
                    Else
                        Return .Item(.CurrentRowIndex, ColNo)
                    End If
                End With
            End If
        End Get
        Set(ByVal value As String)
            TextBox.Text = value
        End Set
    End Property
    Public Overrides Property ControlText() As String
        Get
            Return Me.TextBox.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox.Text = value
        End Set
    End Property

    Private Sub TextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox.LostFocus
        If Me.regex.Length > 0 And CStr(Value()).Length > 0 Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(Value, Me.regex) Then
                'MsgBox(String.Format("Invalid entry for {0}: '{1}'", LabelText, Value))
                Beep()
                Me.Focus()
            End If
        End If
    End Sub

    Public Overrides Sub hResize(ByVal sender As Object, ByVal e As System.EventArgs)
        meResize(sender, e)
    End Sub

    Private Sub meResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With TextBox
            .Left = Label.Left + Label.Width + 5
            .Width = Me.Width - .Left
            .Top = 1
            .Height = Me.Height - 2
        End With
    End Sub

End Class
