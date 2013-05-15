Friend Class TextQuestion

    Private CH As New TextBox()
    Public Sub New(ByVal QuestionNumber As Integer, ByVal QuestionText As String, Optional ByVal ResponseText As String = Nothing, Optional ByVal QuestionMandatory As Boolean = False)
        InitializeComponent()
        With Me
            .QuestionText.Text = QuestionText
            .QuestionNumber = QuestionNumber
            .Mandatory = QuestionMandatory

            CH.Dock = DockStyle.Fill
            CH.Text = ""
            If Not IsNothing(ResponseText) Then
                CH.Text = ResponseText
            End If

            Select Case QuestionMandatory
                Case True
                    Me.QuestionText.ForeColor = Color.Red
                Case Else
                    Me.QuestionText.ForeColor = Color.Black
            End Select

            With .ResponsePanel.Controls
                .Add(CH)
            End With

            AddHandler CH.LostFocus, AddressOf hValueChange

        End With
    End Sub

    Private Sub hValueChange(ByVal sender As Object, ByVal args As System.EventArgs)
        ValueChange(Me)
    End Sub

    Public Overrides Sub getSelected(ByRef Value As String, ByRef Text As String)
        Value = Nothing
        Text = CH.Text
    End Sub

End Class
