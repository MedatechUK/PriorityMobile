Friend Class cp_Line : Inherits CPCommand

    Private _TopLeft As Point
    Private _BottomRight As Point
    Private _thickness As Integer

    Public Sub New(ByVal sender As Label, ByVal TopLeft As Point, ByVal BottomRight As Point, ByVal Thickness As Integer)
        _TopLeft = TopLeft
        _BottomRight = BottomRight
        _thickness = Thickness
        changeHeight(sender, BottomRight.Y)
    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Return String.Format( _
                "LINE {0} {1} {2} {3} {4}{5}", _
                _TopLeft.X, _
                _TopLeft.Y, _
                _BottomRight.X, _
                _BottomRight.Y, _
                _thickness.ToString, _
                vbCrLf _
                )
        End Get
    End Property

End Class
