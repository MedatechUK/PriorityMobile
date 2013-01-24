Friend Class cp_Text : Inherits CPCommand

    Private _strVal As String = ""
    Private _Font As Integer
    Private _FontSize As Integer
    Private _height As Integer
    Private _Orientation As TextOrientation = TextOrientation.normal

    Public Sub New(ByVal sender As Label, ByVal strVal As String, ByVal Location As Point, ByVal thisFont As PrinterFont, ByVal Orientaion As TextOrientation)
        Me.Location = Location
        _strVal = strVal
        _Font = thisFont.Fontface
        _FontSize = thisFont.Fontsize        
        _Orientation = Orientaion
        changeHeight(sender, Location.Y + thisFont.VerticalOffset)
    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Dim cmd As String = "TEXT"
            Select Case _Orientation
                Case TextOrientation.Text90
                    cmd = "TEXT90"
                Case TextOrientation.Text180
                    cmd = "TEXT180"
                Case TextOrientation.text270
                    cmd = "TEXT270"
            End Select

            Return String.Format( _
                "{0} {1} {2} {3} {4} {5}{6}", _
                cmd, _
                _Font, _
                _FontSize, _
                Location.X, _
                Location.Y, _
                _strVal, _
                vbCrLf _
                )
        End Get
    End Property

End Class
