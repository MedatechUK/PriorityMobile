Public Class Label : Implements IDisposable

    Private Commands As New List(Of CPCommand)

#Region "Public Properties"

    Private _PRN As CPCL.LabelPrinter
    Public ReadOnly Property Printer() As CPCL.LabelPrinter
        Get
            Return _PRN
        End Get
    End Property

    Private _Style As eLabelStyle = eLabelStyle.receipt
    Public Property Style() As eLabelStyle
        Get
            Return _Style
        End Get
        Set(ByVal value As eLabelStyle)
            _Style = value
        End Set
    End Property

    Private _offset As Integer
    Public Property offset() As Integer
        Get
            Return _offset
        End Get
        Set(ByVal value As Integer)
            _offset = value
        End Set
    End Property

    Private _LabelQty As Integer
    Public Property LabelQty() As Integer
        Get
            Return _LabelQty
        End Get
        Set(ByVal value As Integer)
            _LabelQty = value
        End Set
    End Property

#End Region

#Region "initialisation and Finalisation"

    Public Sub New(ByRef Printer As CPCL.LabelPrinter, ByVal LabelStyle As eLabelStyle, Optional ByVal LabelQty As Integer = 1, Optional ByVal offset As Integer = 0)
        _PRN = Printer
        _Style = LabelStyle
        _offset = offset
        _LabelQty = LabelQty
        If LabelStyle = eLabelStyle.receipt Then
            _PRN.Dimensions = New Size(_PRN.Dimensions.Width, 0)
        End If
    End Sub

    Public Sub dispose() Implements IDisposable.Dispose
        Commands.Clear()
        Commands = Nothing
    End Sub

#End Region

#Region "CP Object Instantiation Methods"

    Public Sub AddLine(ByVal TopLeft As Point, ByVal BottomRight As Point, Optional ByVal Thickness As Integer = 1)
        Commands.Add(New cp_Box(Me, TopLeft, BottomRight, Thickness))
    End Sub

    Public Sub AddBox(ByVal TopLeft As Point, ByVal BottomRight As Point, Optional ByVal Thickness As Integer = 1)
        Commands.Add(New cp_Box(Me, TopLeft, BottomRight, Thickness))
    End Sub

    Public Sub AddText(ByVal strVal As String, ByVal Location As Point, ByVal thisFont As PrinterFont, Optional ByVal Orientaion As TextOrientation = TextOrientation.normal)
        Commands.Add(New cp_Text(Me, strVal, Location, thisfont, Orientaion))
    End Sub

    Public Sub AddMultiLine(ByVal strVal As String, ByVal Location As Point, ByVal thisFont As PrinterFont, ByVal lineHeight As Integer, Optional ByVal Orientaion As TextOrientation = TextOrientation.normal)
        Commands.Add(New cp_Multiline(Me, strVal, Location, thisFont, Orientaion, lineHeight))
    End Sub

    Public Sub AddBarcode(ByVal Barcode As String, ByVal Location As Point, ByVal Height As Integer, Optional ByVal Symbol As Symbology = Symbology.CODE39, Optional ByVal Orientaion As TextOrientation = TextOrientation.normal)
        Commands.Add(New cp_Barcode(Me, Symbol, Barcode, Location, Height, Orientaion))
    End Sub

    Public Sub AddImage(ByVal Filename As String, ByVal location As Point, Optional ByVal Height As Integer = 100)
        Commands.Add(New cp_image(Me, Filename, location, Height))
    End Sub

    Public Sub AddTearArea(ByVal location As Point, Optional ByVal Height As Integer = 100)
        Commands.Add(New cp_TearArea(Me, location, Height))
    End Sub

#End Region

#Region "Make CPCL Statements"

    Public Shadows Function toString() As String

        Dim sb As New System.Text.StringBuilder
        sb.AppendFormat( _
            "! {0} {1} {2} {3} {4}{5}", _
            offset, _
            _PRN.dpi.X, _
            _PRN.dpi.Y, _
            _PRN.Dimensions.Height, _
            LabelQty, _
            vbCrLf _
        )

        sb.AppendFormat( _
            "PAGE-WIDTH {0}{1}", _
            _PRN.Dimensions.Width, _
            vbCrLf _
        )

        For Each cmd As CPCommand In Commands
            sb.Append(cmd.tostring)
        Next

        sb.AppendFormat("FORM{0}", vbCrLf)
        sb.AppendFormat("PRINT{0}", vbCrLf)

        Return sb.ToString

    End Function

    Public Function toByte() As Byte()

        Dim encoding As New System.Text.UTF8Encoding()
        Return encoding.GetBytes(Me.toString)

    End Function

#End Region

End Class