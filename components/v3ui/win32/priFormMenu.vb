Public Class priFormMenu

    Public Event OpenForm(ByVal FormName As String)

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Property MenuFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            Me.Font = MenuFont
        End Set
    End Property

    Public Sub New(ByRef config As xmlConfiguration)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        txtVersion.Text = String.Format("{0}.{1}", _
            config.xmlMajorVersion, _
            config.xmlMinorVersion)

        ' Add any initialization after the InitializeComponent() call.
        For Each f As xmlForm In config.Forms.Values
            lstModules.Items.Add(f.Name)
        Next
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        With lstModules
            RaiseEvent OpenForm(.SelectedItem.ToString)
        End With
    End Sub

    Private Sub lstModules_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstModules.SelectedValueChanged
        With lstModules            
            btnStart.Enabled = Not (IsNothing(.SelectedItem))
        End With
    End Sub

    Private Sub lstModules_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstModules.SelectedIndexChanged

    End Sub
End Class
