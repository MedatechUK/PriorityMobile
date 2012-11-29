Public Class Form1

    Private Sub GoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoToolStripMenuItem.Click

        'wr(String.Format("", A), True)
        txtOutput.Text = ""
        Dim columns() As String = Split(Replace(Replace(UCase(Me.txtInput.Text), " ", ""), vbCrLf, ""), ",")

        wr(String.Format("' ", ""), False)
        For i As Integer = 0 To UBound(columns)
            wr(String.Format("{0}", columns(i)), False)
            If i < UBound(columns) Then
                wr(String.Format(",", "", False))
            End If
        Next
        wr(String.Format("", ""), True)
        wr(String.Format("Public Class {0}", Me.txtClassName.Text), True)
        wr(String.Format("Inherits DatasetObjectBase", ""), True)
        wr(String.Format("#Region {0}Private Variables{0}", Chr(34)), True)
        For Each Column As String In Columns
            wr(String.Format("Private _{0} As String", Column), True)
        Next
        wr(String.Format("#End Region", ""), True)
        wr(String.Format("#Region {0}Initialisation{0}", Chr(34)), True)
        wr(String.Format("Public Sub New(){0}MyBase.New(){0}End Sub", vbCrLf), True)
        wr(String.Format("Public Sub New(", ""), False)
        For i As Integer = 0 To UBound(columns)
            wr(String.Format("ByVal {0} As String", columns(i)), False)
            If i < UBound(columns) Then
                wr(String.Format(", ", "", False))
            End If
        Next
        wr(String.Format(")", ""), True)
        wr(String.Format("MyBase.New()", ""), True)
        For Each Column As String In columns
            wr(String.Format("_{0} = {0}", Column), True)
        Next
        wr(String.Format("end sub{0}#End Region", vbCrLf), True)
        wr(String.Format("#Region {0}Public Properties{0}", Chr(34)), True)
        For Each column As String In Columns
            wr(String.Format("Public Property {0}() As String", column, Chr(34)), True)
            wr(String.Format("Get", column, Chr(34)), True)
            wr(String.Format("Return _{0}", column, Chr(34)), True)
            wr(String.Format("End Get", column, Chr(34)), True)
            wr(String.Format("Set(ByVal value As String)", column, Chr(34)), True)
            wr(String.Format("If _{0} <> value Then", column, Chr(34)), True)
            wr(String.Format("_{0} = value", column, Chr(34)), True)
            wr(String.Format("OnPropertyChanged({1}{0}{1})", column, Chr(34)), True)
            wr(String.Format("End If", column, Chr(34)), True)
            wr(String.Format("End Set", column, Chr(34)), True)
            wr(String.Format("End Property", column, Chr(34)), True)
        Next
        wr(String.Format("#End Region", ""), True)
        wr(String.Format("End Class", ""), True)

    End Sub

    Sub wr(ByVal str As String, Optional ByVal endline As Boolean = False)
        txtOutput.Text += str
        If endline Then txtOutput.Text += vbCrLf
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        txtOutput.SelectAll()
    End Sub
End Class
