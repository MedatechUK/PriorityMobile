Public Class btnStart

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With Me
            With .ProgressPanel
                .Visible = True
                .Dock = DockStyle.Fill
            End With
            With .ButtonPanel
                .Visible = False
                .Dock = DockStyle.None
            End With
        End With
    End Sub

    Public Property Title() As String
        Get
            Return Me.lbl_Title.Text
        End Get
        Set(ByVal value As String)
            Me.lbl_Title.Text = value
        End Set
    End Property

    Public Property Detail() As String
        Get
            Return Me.lbl_Detail.Text
        End Get
        Set(ByVal value As String)
            Me.lbl_Detail.Text = value
        End Set
    End Property

    Public Property ProgressMax() As Integer
        Get
            Return Me.ProgressBar.Maximum
        End Get
        Set(ByVal value As Integer)
            With Me.ProgressBar
                .Value = 0
                .Maximum = value
            End With
        End Set
    End Property

    Public Sub IncrementProgress()
        With Me.ProgressBar            
            .Value += 1
        End With
    End Sub

    Public Sub EndLoad()
        With Me
            With .ProgressPanel
                .Visible = False
                .Dock = DockStyle.None
            End With
            With .ButtonPanel
                .Visible = True
                .Dock = DockStyle.Fill
            End With
        End With
    End Sub

End Class
