Public Class ColumnPanel
    Inherits Panel

    Public Sub New(ByRef Columns As cColumns)
        With Me.Controls
            For Each col As cColumn In Columns.Values
                .Add(New Column(col))
                With .Item(.Count - 1)
                    .Dock = DockStyle.Top
                End With
            Next
        End With
    End Sub

End Class