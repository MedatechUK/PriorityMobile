Imports System.Windows.Forms

Public Class frmCopies : Inherits BaseForm

    Private ftimer As Timer

    Private Sub PrinterSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        With Me
            .Text = "Printing ..."
            .btnOK.Enabled = Copies.Value > 0
        End With

        ftimer = New Timer
        With ftimer
            .Interval = 100
            AddHandler .Tick, AddressOf hFtimer
            .Enabled = True
        End With
    End Sub

    Private Sub hFtimer(ByVal sender As Object, ByVal e As EventArgs)
        With ftimer
            .Enabled = False
            .Dispose()
        End With
        Copies.Focus()
    End Sub

    Private _result As DialogResult
    Public ReadOnly Property Result() As DialogResult
        Get
            Return _result
        End Get
    End Property

    Public Property NumCopies()
        Get
            Return Me.Copies.Value
        End Get
        Set(ByVal value)
            Me.Copies.Value = value
        End Set
    End Property

    Private Sub Copies_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Copies.KeyDown
        If e.KeyCode = 13 Then
            If Copies.Value > 0 Then
                btnOK_Click(Me, New System.EventArgs)
            Else
                btnCancel_Click(Me, New System.EventArgs)
            End If
        End If
    End Sub

    Private Sub Copies_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Copies.ValueChanged
        Me.btnOK.Enabled = Copies.Value > 0
    End Sub

    Private Sub PrinterSetting_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me
            .btnCancel.Width = Screen.PrimaryScreen.WorkingArea.Width / 2
            .btnOK.Width = Screen.PrimaryScreen.WorkingArea.Width / 2
        End With
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        _result = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        _result = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
