Friend Class cp_Multiline : Inherits CPCommand

    Private _strVal As String = ""
    Private _Font As Integer
    Private _FontSize As Integer
    Private _height As Integer
    Private _Orientation As TextOrientation = TextOrientation.normal
    Private _lineHeight As Integer

    Public Sub New(ByVal sender As Label, ByVal strVal As String, ByVal Location As Point, ByVal thisFont As PrinterFont, ByVal lineHeight As Integer, ByVal Orientaion As TextOrientation)
        Me.Location = Location
        _strVal = strVal
        _Font = thisFont.Fontface
        _FontSize = thisFont.Fontsize
        _lineHeight = lineHeight
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
                "ML {0}{1} {2} {3} {4} {5} {6}{7}ENDML{8}", _
                _lineHeight & vbCrLf, _
                cmd, _
                _Font, _
                _FontSize, _
                Location.X, _
                Location.Y, _
                _strVal, _
                vbCrLf, _
                vbCrLf _
                )
        End Get
    End Property

End Class

