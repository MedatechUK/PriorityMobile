Imports System.Windows.Forms

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

    Private _Parent As System.Windows.Forms.UserControl
    Public Overloads ReadOnly Property Parent() As System.Windows.Forms.UserControl
        Get
            Return _Parent
        End Get
    End Property

    Public Sub New(ByRef Parent As System.Windows.Forms.UserControl, ByRef Columns As cColumns)
        _Parent = Parent
        With Me.Controls
            Dim lv As ListView = NewListView()
            .Add(lv)
            For Each col As cColumn In Columns.Values
                If col.Visible Then
                    Dim ch = New ColumnHeader
                    With ch
                        .Text = col.Title
                    End With
                    lv.Columns.Add(ch)
                End If
            Next
        End With
    End Sub

End Class
