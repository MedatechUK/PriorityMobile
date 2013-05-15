Public Class TablePanel
    Inherits Panel

    Private Function NewListView() As ListView
        Dim ret As New ListView
        With ret
            .View = View.Details
            .FullRowSelect = True
            .Dock = DockStyle.Fill
        End With
        Return ret
    End Function

    Public Sub New(ByRef Columns As cColumns)
        With Me.Controls
            Dim lv As ListView = newListView()
            .Add(lv)
            For Each col As cColumn In Columns.Values
                If col.Visible Then
                    Dim ch = New ColumnHeader
                    With ch
                        .Text = col.Name
                    End With
                    lv.Columns.Add(ch)
                End If
            Next
        End With
    End Sub

End Class
