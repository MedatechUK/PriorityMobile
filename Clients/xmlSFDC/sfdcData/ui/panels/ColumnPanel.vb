Imports System.Windows.Forms

Public Class ColumnPanel
    Inherits Panel
    Private _Columns As cColumns

    Private _Parent As System.Windows.Forms.UserControl
    Public Overloads ReadOnly Property Parent() As System.Windows.Forms.UserControl
        Get
            Return _Parent
        End Get
    End Property

    Public Sub New(ByRef Parent As System.Windows.Forms.UserControl, ByRef Columns As cColumns)
        _Columns = Columns
        With Me.Controls
            For Each col As cColumn In _Columns.Values
                If col.Visible Then
                    .Add(New uiColumn(Me, col))
                End If                
            Next
            For i As Integer = .Count - 1 To 0 Step -1
                With .Item(i)
                    .Dock = DockStyle.Top
                End With
            Next
        End With
    End Sub

End Class