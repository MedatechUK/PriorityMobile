Public Class frmProvisionDialog

    Public Sub New(Optional ByVal ProvisionString As String = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.        
        With Me            
            .Menu = MainMenu1
        End With
    End Sub

    Public ReadOnly Property ProvisionString() As String
        Get
            Return Me.txtProvisionString.Text
        End Get
    End Property

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub HideSIPButton()
        Capture = True
        Dim hWnd As IntPtr = GetCapture()
        Capture = False
        SetFullScreen(hWnd, SHFS.HIDESIPBUTTON)
    End Sub

    Private Sub frmProvisionDialog_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        With Me            
            HideSIPButton()
        End With
    End Sub

    Private Sub frmMain_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        HideSIPButton()
    End Sub

    Private Sub LinkLabel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReprovision.Click
        With Me
            .panel_reprovision.Visible = False
            .Panel_Text.Visible = True
            With .txtProvisionString
                .Focus()
                .SelectAll()
            End With
        End With
    End Sub

    Private Sub txtProvisionString_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtProvisionString.KeyDown
        If e.KeyValue = 13 Then
            btnConnect_Click(Me, New System.EventArgs)
        End If
    End Sub

End Class