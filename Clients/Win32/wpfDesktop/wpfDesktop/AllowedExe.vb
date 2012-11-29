Public Class AllowedExe

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim d As Boolean = False
        Using br As New BrowseExe
            With br
                Do
                    If .ShowDialog = Windows.Forms.DialogResult.OK Then
                        For Each i As ListViewItem In exelist.Items
                            If Strings.StrComp(i.Text, .exeName.Text, CompareMethod.Text) = 0 Then
                                d = MsgBox("Executable exists. Update?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel
                                If Not d Then
                                    i.SubItems(1).Text = .exePath.Text
                                    Exit Do
                                Else
                                    Exit For
                                End If
                            End If
                        Next
                        If Not d Then
                            exelist.Items.Add(UCase(.exeName.Text))
                            exelist.Items(exelist.Items.Count - 1).SubItems.Add(.exePath.Text)
                        End If
                    End If
                Loop Until Not d
            End With
        End Using
    End Sub

    Private Sub exelist_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles exelist.DoubleClick

        For Each i As ListViewItem In Me.exelist.Items
            If i.Selected Then
                edit(i)
            End If
        Next

    End Sub

    Private Sub edit(ByRef item As ListViewItem)

        Dim d As Boolean = False
        Using br As New BrowseExe
            With br
                .exeName.Text = item.Text
                .exePath.Text = item.SubItems(1).Text
                Do
                    If .ShowDialog = Windows.Forms.DialogResult.OK Then
                        item.Text = UCase(.exeName.Text)
                        item.SubItems(1).Text = .exePath.Text
                    End If
                Loop Until Not d
            End With
        End Using

    End Sub

    Private Sub exelist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exelist.SelectedIndexChanged
        Me.btndel.Enabled = exelist.SelectedItems.Count > 0
    End Sub

    Private Sub btndel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndel.Click
        With exelist
            For i As Integer = .Items.Count - 1 To 0 Step -1
                If .Items(i).Selected Then
                    If MsgBox("This will delete the currently selected item." & vbCrLf & "Are you sure you wish to do this?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        .Items(i).Remove()
                    End If
                End If
            Next
        End With
    End Sub

    Private Sub AllowedExe_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        With Me.exelist
            .Columns(1).Width = .Width - .Columns(0).Width - 5
        End With
    End Sub

    Protected Overrides Sub Finalize()

        MyBase.Finalize()
    End Sub
End Class