Public Class frmNumber

    Public Property number() As Integer
        Get
            Return Me.Ct_number1.Value
        End Get
        Set(ByVal value As Integer)
            Me.Ct_number1.Value = value
        End Set
    End Property

    Public ReadOnly Property Manual() As Boolean
        Get
            Return Me.Ct_number1.manual
        End Get
    End Property

    Private Sub frmNumber_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
        Ct_number1.Max = 999999
    End Sub

    Private Sub Ct_number1_SetNumber(ByVal MyValue As Integer) Handles Ct_number1.SetNumber
        number = Me.Ct_number1.Value
        Me.Close()
    End Sub
End Class