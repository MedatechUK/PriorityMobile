Friend Class cp_image : Inherits CPCommand

    Private _filename As String

    Public Sub New(ByVal sender As Label, ByVal Filename As String, ByVal location As Point, ByVal height As Integer)
        Me.Location = location
        _filename = Filename
        changeHeight(sender, location.Y + height)
    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Return String.Format( _
                "PCX {0} {1} !<{3}{2}", _
                Me.Location.X, _
                Me.Location.Y, _
                vbCrLf, _
                _filename _
            )
        End Get
    End Property

End Class
