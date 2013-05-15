Friend Class BoolQuestion

    Private CH As New CheckBox()
    Public Sub New(ByVal QuestionNumber As Integer, ByVal QuestionText As String, ByVal Options As Dictionary(Of Integer, String), Optional ByVal ResponseValue As Integer = -1, Optional ByVal QuestionMandatory As Boolean = False)
        InitializeComponent()
        With Me
            NP = Options
            .QuestionText.Text = QuestionText
            .QuestionNumber = QuestionNumber

            Select Case QuestionMandatory
                Case True
                    Me.QuestionText.ForeColor = Color.Red
                Case Else
                    Me.QuestionText.ForeColor = Color.Black
            End Select

            With CH
                .Dock = DockStyle.Left
                .Width = 100
                .Text = ""
                If Not ResponseValue = -1 Then
                    Select Case Options(ResponseValue).ToLower
                        Case "y", "yes", "true"
                            .Checked = True
                        Case Else
                            .Checked = False
                    End Select
                Else
                    .Checked = False
                End If
            End With

            With .ResponsePanel.Controls
                .Add(CH)
            End With

            AddHandler CH.CheckStateChanged, AddressOf hValueChange

        End With
    End Sub

    Private Sub hValueChange(ByVal sender As Object, ByVal args As System.EventArgs)
        ValueChange(Me)
    End Sub

    Public Overrides Sub getSelected(ByRef Value As String, ByRef Text As String)
        For Each key As Integer In NP.Keys
            Select Case NP(key).ToLower
                Case "y", "yes", "true"
                    If CH.Checked Then
                        Value = key
                        Text = NP(key)
                        Exit For
                    End If
                Case Else
                    If Not CH.Checked Then
                        Value = key
                        Text = NP(key)
                    End If
            End Select
        Next
    End Sub

End Class
