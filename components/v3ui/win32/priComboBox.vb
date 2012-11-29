Public Class priComboBox

    Overrides Sub NewFont(ByVal FormFont As System.Drawing.Font)
        ComboBox.Font = FormFont
    End Sub
    Public Overrides Sub SetReadOnly()
        ComboBox.Enabled = Not (MyBase.IsReadOnly)
    End Sub
    Overrides Property Value() As String
        Get
            With ComboBox
                Try
                    If Active Or (DG.CurrentRowIndex = -1) Then
                        Dim pi As System.Reflection.PropertyInfo = .SelectedItem.GetType.GetProperty(Me.ListValueCol)
                        Return CStr(pi.GetValue(.SelectedItem, Nothing))
                    Else
                        With DG
                            If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                                Return ""
                            Else
                                Return .Item(.CurrentRowIndex, ColNo)
                            End If                            
                        End With
                    End If
                Catch
                    Return ""
                End Try
            End With
        End Get
        Set(ByVal value As String)
            ComboBox.SelectedText = value
        End Set
    End Property
    Public Overrides Property ControlText() As String
        Get
            Return Me.ComboBox.SelectedItem
        End Get
        Set(ByVal value As String)
            Dim pi As System.Reflection.PropertyInfo = Me.ComboBox.Items(0).GetType.GetProperty(Me.ListValueCol)
            For i As Integer = 0 To Me.ComboBox.Items.Count - 1
                If pi.GetValue(Me.ComboBox.Items(i), Nothing) = value Then
                    Me.ComboBox.SelectedIndex = i
                End If
            Next
        End Set
    End Property

    Public Overrides Sub hResize(ByVal sender As Object, ByVal e As System.EventArgs)
        meResize(sender, e)
    End Sub

    Private Sub meResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With ComboBox
            .Left = Label.Left + Label.Width + 5
            .Width = Me.Width - .Left
            .Top = 1
            Me.Height = .Height + 5
        End With
    End Sub

End Class
