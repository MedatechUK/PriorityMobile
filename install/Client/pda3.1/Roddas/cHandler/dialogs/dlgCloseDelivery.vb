Public Class dlgCloseDelivery

    Inherits PriorityMobile.UserDialog

    Private _PartialDelivery As Boolean = False
    Public Property PartialDelivery() As Boolean
        Get
            Return _PartialDelivery
        End Get
        Set(ByVal value As Boolean)
            _PartialDelivery = value
        End Set
    End Property

    Private _CompleteDelivery As Boolean = False
    Public Property CompleteDelivery() As Boolean
        Get
            Return _CompleteDelivery
        End Get
        Set(ByVal value As Boolean)
            _CompleteDelivery = value
        End Set
    End Property

    Private _NoDelivery As Boolean = False
    Public Property NoDelivery() As Boolean
        Get
            Return _NoDelivery
        End Get
        Set(ByVal value As Boolean)
            _NoDelivery = value
        End Set
    End Property

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Private Sub btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnOK.Click
        Dim btn As Button = sender
        Select Case btn.Name.ToLower
            Case "btnok"
                Result = DialogResult.OK
            Case "btncancel"
                Result = DialogResult.Cancel
        End Select
        EndDialog()
    End Sub

    Private Sub Reason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reason.SelectedIndexChanged
        If Reason.SelectedIndex > 0 Then
            btnOK.Enabled = True
        Else
            btnOK.Enabled = False
        End If
    End Sub
End Class
