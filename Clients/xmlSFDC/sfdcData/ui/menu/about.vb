Imports System.Windows.Forms

Public Class about

    Private _ue As UserEnv

    Public Sub New(ByRef ue As UserEnv)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ue = ue

        With Me

            Width = Screen.PrimaryScreen.WorkingArea.Width
            Height = Screen.PrimaryScreen.WorkingArea.Height - 50

            Dim p As System.Drawing.Point
            p.X = (Screen.PrimaryScreen.WorkingArea.Width - .Width) / 2
            p.Y = (Screen.PrimaryScreen.WorkingArea.Height - .Height) / 2
            .Location = p

            .lbl_sfdc_vers.Text = FrameworkVersion
            .lbl_Handler_vers.Text = HandlerVersion
            _Result = Windows.Forms.DialogResult.Cancel

        End With

    End Sub

    Private ReadOnly Property FrameworkVersion() As String
        Get
            With System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
                Return String.Format("{0}.{1}.{2}.{3}", _
                    .Major, _
                    .Minor, _
                    .Build, _
                    .Revision _
                )
            End With
        End Get
    End Property

    Private ReadOnly Property HandlerVersion() As String
        Get
            Dim excep As New Exception
            Return _ue.HandlerVersion(excep)
        End Get
    End Property

    Private _Result As DialogResult = Nothing
    Public Property Result() As DialogResult
        Get
            Return _Result
        End Get
        Set(ByVal value As DialogResult)
            _Result = value
        End Set
    End Property

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        _Result = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        _Result = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

End Class