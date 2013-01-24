Friend Class cp_Barcode : Inherits CPCommand

    Private _Barcode As String = ""
    Private _ratio As Integer
    Private _BarWidth As Integer
    Private _height As Integer
    Private _Orientation As TextOrientation = TextOrientation.normal
    Private _Symbology As Symbology = Symbology.CODE39

    Public Sub New(ByVal sender As Label, ByVal Symbol As Symbology, ByVal Barcode As String, ByVal Location As Point, ByVal Height As Integer, ByVal Orientaion As TextOrientation)
        Me.Location = Location
        _Barcode = Barcode
        _BarWidth = 2
        _height = Height
        _Orientation = Orientaion
        _Symbology = Symbol
        Select Case _Symbology
            Case Symbology.CODE39, Symbology.EAN13
                changeHeight(sender, Location.Y + Height + 20)
            Case Symbology.QRCODE
                changeHeight(sender, Location.Y + Height)
        End Select

    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Dim cmd As String = "BARCODE"
            Select Case _Orientation
                Case TextOrientation.Text90, TextOrientation.text270
                    cmd = "VBARCODE"
                Case Else
                    cmd = "BARCODE"
            End Select

            Dim sym As String = "39"
            Select Case _Symbology
                Case Symbology.EAN13
                    sym = "EAN13"
                    _ratio = 20
                Case Symbology.CODE39
                    sym = "39"
                    _ratio = 25
                Case Symbology.QRCODE
                    sym = "QR"
            End Select

            Select Case _Symbology
                Case Symbology.CODE39, Symbology.EAN13
                    Return String.Format("{0} {1} {2} {3} {4} {5} {6} {7}{8}", _
                        cmd, _
                        sym, _
                        _BarWidth, _
                        _ratio, _
                        _height, _
                        Me.Location.X, _
                        Me.Location.Y, _
                        _Barcode, _
                        vbCrLf _
                    )

                Case Symbology.QRCODE
                    Dim sb As New System.Text.StringBuilder
                    sb.AppendFormat("{0} {1} {2} {3} M 2 U 6{4}", _
                        cmd, _
                        sym, _
                        Me.Location.X, _
                        Me.Location.Y, _
                        vbCrLf _
                    )
                    sb.AppendFormat( _
                        "MA,{0}{1}", _
                        _Barcode, _
                        vbCrLf _
                    )
                    sb.AppendFormat("ENDQR{0}", vbCrLf)

                    Return sb.ToString

                Case Else
                    Return Nothing

            End Select
        End Get
    End Property

End Class
