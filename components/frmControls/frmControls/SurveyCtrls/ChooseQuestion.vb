Friend Class ChooseQuestion
    Private CH As New ComboBox

    Public Sub New(ByVal QuestionNumber As Integer, ByVal QuestionText As String, ByVal Options As Dictionary(Of Integer, String), Optional ByVal ResponseValue As Integer = -1, Optional ByVal QuestionMandatory As Boolean = False)

        InitializeComponent()
        With Me
            .QuestionText.Text = QuestionText
            .QuestionNumber = QuestionNumber
            NP = Options

            Select Case QuestionMandatory
                Case True
                    Me.QuestionText.ForeColor = Color.Red
                Case Else
                    Me.QuestionText.ForeColor = Color.Black
            End Select

            Dim ci As Integer = 0
            For Each key As Integer In Options.Keys
                CH.Items.Add(Options(key))
                If Not ResponseValue = -1 Then
                    If ResponseValue = key Then
                        CH.SelectedIndex = ci
                    End If
                End If
                ci += 1
            Next

            CH.Dock = DockStyle.Fill
            With .ResponsePanel.Controls
                .Add(CH)
            End With

            AddHandler CH.SelectedIndexChanged, AddressOf hValueChange

        End With

    End Sub

    Private Sub hValueChange(ByVal sender As Object, ByVal args As System.EventArgs)
        ValueChange(Me)
    End Sub

    Public Overrides Sub getSelected(ByRef Value As String, ByRef Text As String)
        For Each key As Integer In NP.Keys
            If String.Compare(NP(key), CH.SelectedItem) = 0 Then
                Value = key
                Text = NP(key)
                Exit For
            End If
        Next
    End Sub

End Class
