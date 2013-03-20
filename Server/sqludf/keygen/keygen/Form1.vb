
Public Class Form1

    Private Class ch

        Private Numbers As String = "1234567890ABCDEFGHIJKLMNOPQRTSUVWXYZ"
        Private _loc As Integer = 0

        Public ReadOnly Property Value() As String
            Get
                Return Numbers.Substring(_loc, 1)
            End Get
        End Property

        Public Function Increment() As Boolean
            If _loc < Numbers.Length - 1 Then
                _loc += 1
                Return True
            Else
                Return False
            End If
        End Function

    End Class

    Private Sub hTextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Licence.TextChanged, txt_Module.TextChanged, txt_key.TextChanged
        Dim c As Integer = CheckVal(Me.txt_Licence.Text, Me.txt_Module.Text)
        Me.txt_key.Text = MakeKey(c)
    End Sub

    Private Function MakeKey(ByVal CheckVal As Integer) As String

        Dim objRandom As New System.Random(CType(System.DateTime.Now.Ticks Mod System.Int32.MaxValue, Integer))

        Dim t As New List(Of ch)
        For i As Integer = 0 To 9
            t.Add(New ch)
        Next

        For i As Integer = 0 To CheckVal
            Dim r As Integer
            Do
                r = objRandom.Next(t.Count)
            Loop Until (t(r).Increment())
        Next

        Dim str As New System.Text.StringBuilder
        For Each Val As ch In t
            str.Append(Val.Value)
        Next
        Return str.ToString

    End Function

    Private Function CheckVal(ByVal PriorityLicence As String, ByVal ModuleName As String) As Integer

        Dim c As Integer = 0
        For i As Integer = 0 To PriorityLicence.Length - 1
            If IsNumeric(PriorityLicence.Substring(i, 1)) Then
                c += CInt(PriorityLicence.Substring(i, 1))
            End If
        Next
        For i As Integer = 0 To ModuleName.Length - 1
            c += CInt(Microsoft.VisualBasic.Right(Asc(ModuleName.Substring(i, 1)).ToString, 1))
        Next

        Return c

    End Function

    Private Sub txt_key_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_key.KeyDown
        e.Handled = True
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Clipboard.SetText(Me.txt_key.Text)
    End Sub

End Class
