Public Class Output
    Private _ID As String
    Private mcaller As frmMain
    Public Sub New(ByVal Caller As frmMain)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        mcaller = Caller
        ' Add any initialization after the InitializeComponent() call.
        _ID = System.Guid.NewGuid.ToString
    End Sub
    Public ReadOnly Property OutputID() As String
        Get
            Return _ID
        End Get
    End Property
    Public Function Editable() As Boolean
        Return True
    End Function
    Public Function editControl() As Windows.Forms.TextBox
        Return strOutput
    End Function

    Private Sub strOutput_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles strOutput.GotFocus
        With mcaller
            .setEditToolbar(True)
            .EditControl = Me.strOutput
        End With
    End Sub

    Private Sub strOutput_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles strOutput.LostFocus
        With mcaller
            .setEditToolbar(False)
            .EditControl = Nothing
        End With
    End Sub
End Class