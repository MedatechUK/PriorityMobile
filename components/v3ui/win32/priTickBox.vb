Public Class PriTickBox

    Overrides Sub NewFont(ByVal FormFont As System.Drawing.Font)
        CheckBox.Font = FormFont
    End Sub

    Public Overrides Sub SetReadOnly()
        CheckBox.Enabled = Not (MyBase.IsReadOnly)
    End Sub

    Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Select Case Me.CheckBox.Checked
                    Case True
                        Return "Y"
                    Case Else
                        Return ""
                End Select
            Else
                With DG
                    If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                        Return ""
                    Else
                        Select Case .Item(.CurrentRowIndex, ColNo)
                            Case True
                                Return "Y"
                            Case Else
                                Return ""
                        End Select
                    End If
                End With
            End If

        End Get
        Set(ByVal value As String)
            Me.CheckBox.Checked = CBool(value = "Y")
        End Set
    End Property

    Public Overrides Sub hResize(ByVal sender As Object, ByVal e As System.EventArgs)
        meResize(sender, e)
    End Sub
    Private Sub meResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With CheckBox
            .Left = Label.Left + Label.Width + 5
            .Width = Me.Width - .Left
            .Top = 1
            .Height = Me.Height - 2
        End With
    End Sub

End Class
