Imports System.Windows.Forms
Public Class PrinterSetting

    Private ftimer As Timer

    Private Sub PrinterSetting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.btnOK.Enabled = rxIsPattern(rxMAC, txt_MACAddress.Text)
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
        txt_MACAddress.Focus()
    End Sub

    Private _result As DialogResult
    Public ReadOnly Property Result() As DialogResult
        Get
            Return _result
        End Get
    End Property

    Public Property MACAddress()
        Get
            Return Me.txt_MACAddress.Text
        End Get
        Set(ByVal value)
            Me.txt_MACAddress.Text = value
        End Set
    End Property

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

    Private Sub txt_MACAddress_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_MACAddress.TextChanged
        Me.btnOK.Enabled = rxIsPattern(rxMAC, txt_MACAddress.Text)
    End Sub

End Class
