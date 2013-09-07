Public Class frm_Debug

    Private loaded As Boolean = False

    Private _Running As Boolean = False
    Public Property Running() As Boolean
        Get
            Return _Running
        End Get
        Set(ByVal value As Boolean)
            _Running = value
        End Set
    End Property

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.Size = My.Settings.fDebugSize
        Me.Location = My.Settings.fDebugLocation

        ' Add any initialization after the InitializeComponent() call.
        loaded = True
    End Sub

    Public Sub BringToFore(ByVal e As String)
        If Me.InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf BringToFore), "")
        Else
            Me.BringToFront()
        End If
    End Sub

    Public Sub Clear(ByVal e As String)
        If Me.InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf Clear), "")
        Else
            rtb.Text = ""
        End If
    End Sub

    Public Sub Log(ByVal str As String)
        If Me.InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf Log), str)
        Else
            rtb.Text += str
            rtb.Text += vbCrLf
            rtb.Select(rtb.TextLength, 0)
            Me.Refresh()
        End If
    End Sub

    Public Sub Log(ByVal Pattern As String, ByVal ParamArray args() As String)
        If Me.InvokeRequired Then
            Invoke(New Action(Of String, String())(AddressOf Log), Pattern, args)
        Else
            rtb.Text += String.Format(Pattern, args)
            rtb.Text += vbCrLf
            rtb.Select(rtb.TextLength, 0)
            Me.Refresh()
        End If

    End Sub

    Private Sub frm_Debug_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        If loaded Then
            My.Settings.fDebugLocation = Me.Location
            My.Settings.Save()
        End If
    End Sub

    Private Sub frm_Debug_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        If loaded Then
            My.Settings.fDebugSize = Me.Size
            My.Settings.Save()
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Me.Visible And Running Then
            Me.BringToFront()
        End If
    End Sub
End Class