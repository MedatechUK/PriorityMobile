Imports System.Windows.Forms

Public Class FormView

    Private _ViewForm As ColumnPanel
    Public Property ViewForm() As ColumnPanel
        Get
            Return _ViewForm
        End Get
        Set(ByVal value As ColumnPanel)
            _ViewForm = value
        End Set
    End Property

    Public Sub Load(ByRef Parent As iForm, ByRef thisForm As cForm)

        ViewForm = New ColumnPanel(Me, thisForm.Columns)
        With Me.Controls
            .Add(ViewForm)
            ViewForm.Dock = DockStyle.Fill
        End With

    End Sub

End Class
