Public Class LogicEditor

    Private _Conditions As Dictionary(Of String, Condition)

    Private _keywords() As String = {"AND", "OR", "NOT"}

    Private _result As String
    Public ReadOnly Property ResultLogic() As String
        Get
            Return _result
        End Get
    End Property

    Public Sub New(ByRef Conditions As Dictionary(Of String, Condition))
        InitializeComponent()
        _Conditions = Conditions

        With ConditionMenu.Items
            .Clear()
            For Each Keyword As String In _keywords
                .Add(Keyword)
                AddHandler .Item(.Count - 1).Click, AddressOf hAddCondition
            Next
            .Add("-")
            For Each Cond As cargo3.Condition In _Conditions.Values
                .Add(Cond.Name)
                AddHandler .Item(.Count - 1).Click, AddressOf hAddCondition
            Next
        End With

    End Sub

    Private Sub hAddCondition(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mi As Windows.Forms.ToolStripMenuItem = sender
        If txt_Logic.Text.Length > 0 Then
            If Not (txt_Logic.Text.Last = " ") Then txt_Logic.Text += " "
        End If
        txt_Logic.Text += mi.Text & " "
        txt_Logic.SelectionStart = txt_Logic.Text.Length
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim openParenth As Integer = 0
        Dim closeParenth As Integer = 0
        Dim teststr As String = txt_Logic.Text

        For i As Integer = 0 To teststr.Length - 1
            Select Case teststr.Substring(i, 1)
                Case "("
                    openParenth += 1
                Case ")"
                    closeParenth += 1
            End Select
        Next

        If Not (openParenth = closeParenth) Then
            MsgBox("Parenthesis mismatch.")
            Exit Sub
        Else
            teststr = teststr.Replace("(", "").Replace(")", "")
        End If

        For Each Keyword As String In _keywords
            teststr = teststr.Replace(Keyword, "")
        Next
        For Each Cond As cargo3.Condition In _Conditions.Values
            teststr = teststr.Replace(Cond.Name, "")
        Next

        If Not (teststr.Trim.Length = 0) Then
            MsgBox(String.Format("invalid syntax: {0}.", teststr.Trim))
            Exit Sub
        End If

        _result = txt_Logic.Text
        Me.Close()

    End Sub

End Class