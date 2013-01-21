Public Class Form1

    Public ReadOnly Property ServerURL() As String
        Get
            Return "http://redknave:8080/"
        End Get
    End Property

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click

        Dim args As New Dictionary(Of String, String)
        With args
            .Add("user", "tabula")
            .Add("whs", "Main")
            .Add("startarg", "*")
        End With

        Dim frm As New sfdc3ui.iForm(ServerURL, "module", New myTestHandler, args)
        With frm
            .ControlHeight = 24
            .ControlFont = New Font("Verdana", 10, FontStyle.Regular)
            .Show()
        End With


    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim mnu As New sfdc3ui.ctrlMenu
        'Me.Menu = mnu
    End Sub

End Class
