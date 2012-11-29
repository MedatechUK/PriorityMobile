Public Class PriDatePick

    Private basedt As DateTime = New DateTime("1988", "1", "1")

    Overrides Sub NewFont(ByVal FormFont As System.Drawing.Font)
        dt.Font = FormFont
    End Sub

    Public Overrides Sub SetReadOnly()
        dt.Enabled = Not (MyBase.IsReadOnly)
    End Sub

    Public Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Return DateDiff(DateInterval.Minute, #1/1/1988#, dt.Value).ToString
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
            dt.Value = basedt.AddMinutes(CInt(value))
        End Set
    End Property

    Public Property Format() As DateTimePickerFormat
        Get
            Return dt.Format
        End Get
        Set(ByVal value As DateTimePickerFormat)
            dt.Format = value
        End Set
    End Property

    Public Overrides Sub hResize(ByVal sender As Object, ByVal e As System.EventArgs)
        meResize(sender, e)
    End Sub
    Private Sub meResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With dt
            .Left = Label.Left + Label.Width + 5
            .Width = Me.Width - .Left
            .Top = 1
            .Height = Me.Height - 2
        End With
    End Sub

End Class
