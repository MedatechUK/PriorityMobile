Imports System.Windows.Forms

Public Class genOptions

    Dim mCaller As MDIParent

    Public Sub New(ByVal Caller As MDIParent)
        InitializeComponent()
        mcaller = caller
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        mCaller.ConsoleWidth = Me.int_CWidth.Value
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Me.txt_Opacity.Text = CStr(Me.TrackBar1.Value) & " %"
        mCaller.Opacity = CDbl(Me.TrackBar1.Value / 100)
    End Sub

    Private Sub genOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TrackBar1.Value = mCaller.Opacity * 100
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub
End Class
