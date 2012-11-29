Imports System.Windows.Forms

Public Class frmTable

    Private mCaller As MDIParent

    Public Sub New(ByRef Caller As MDIParent)
        InitializeComponent()
        mCaller = Caller
        Reset()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles filter.TextChanged

        Reset()

    End Sub

    Private Sub Reset()
        With Me.lst_Tables.Items
            .Clear()
            Dim li() As String = Split(My.Resources.TableDictionary, "CREATE TABLE")
            For Each n As String In li
                If Len(Trim(n)) > 0 Then
                    If Trim(LCase(filter.Text)) = "" Or Strings.Left(LCase(Split(Trim(n), " ")(0)), Len(filter.Text)) = LCase(filter.Text) Then
                        If Not Me.lst_selected.Items.IndexOf(Split(Trim(n), " ")(0)) > -1 Then
                            .Add(Split(Trim(n), " ")(0))
                        End If
                    End If
                End If
            Next
        End With
    End Sub

    Private Sub frmTable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        filter.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        filter.Text = ""
        Reset()
        filter.Focus()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        For Each i As Integer In Me.lst_Tables.SelectedIndices
            Me.lst_selected.Items.Add(Me.lst_Tables.Items(i))
        Next
        Reset()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        For i As Integer = Me.lst_selected.SelectedIndices.Count - 1 To 0 Step -1
            Me.lst_selected.Items.RemoveAt(Me.lst_selected.SelectedIndices(i))
        Next
        Reset()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.lst_selected.Items.Clear()
        Reset()
    End Sub
End Class
