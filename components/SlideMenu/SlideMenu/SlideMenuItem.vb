Public Class SlideMenuItem

#Region "Private Variables"

    Private _ItemImage As Graphics

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Text As String, ByVal Fontface As Font, ByVal ForeColour As Color, ByVal BackColour As Color)

        _text = Text
        _Fontface = Fontface
        _ForeColour = ForeColour
        _BackColour = BackColour

        If Not SlideImages.Keys.Contains(_text) Then

            SlideImages.Add(_text, New SlideMenuImage)
            _iSize = MeasureString(_text, _Fontface)

            _image = New Bitmap(_iSize.Width + 20, _iSize.Height + 2)
            _ItemImage = Graphics.FromImage(_image)
            _ItemImage.FillRectangle(New Drawing.SolidBrush(_ForeColour), 0, 0, _iSize.Width + 20, _iSize.Height + 2)
            _ItemImage.DrawLine(New Pen(Color.Black), 0, 0, _iSize.Width + 20, 0)
            _ItemImage.DrawLine(New Pen(Color.Black), 0, _iSize.Height + 1, _iSize.Width + 20, _iSize.Height + 1)
            _ItemImage.DrawString(Text, _Fontface, New Drawing.SolidBrush(_BackColour), 10, 0)
            _ItemImage.DrawImage(_image, 0, 0)

            SlideImages(_text).SelectedImage = _image
            'SlideImages(_text).SelectedImage.Save(Get_app_path() & "\TEST.bmp", System.Drawing.Imaging.ImageFormat.Bmp)

            _image = New Bitmap(_iSize.Width + 20, _iSize.Height + 2)
            _ItemImage = Graphics.FromImage(_image)
            _ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _iSize.Width + 20, _iSize.Height + 2)
            _ItemImage.DrawLine(New Pen(Color.Black), 0, 0, _iSize.Width + 20, 0)
            _ItemImage.DrawLine(New Pen(Color.Black), 0, _iSize.Height + 1, _iSize.Width + 20, _iSize.Height + 1)
            _ItemImage.DrawString(Text, _Fontface, New Drawing.SolidBrush(_ForeColour), 10, 0)
            _ItemImage.DrawImage(_image, 0, 0)

            SlideImages(_text).UnselectedImage = _image

        End If
        _image = SlideImages(_text).UnselectedImage
        _iSize = New SizeF(_image.Width, _image.Height)

    End Sub

#End Region

    Private Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

#Region "Public Properties"

    Private _Fontface As Font
    Public Property Fontface() As Font
        Get
            Return _Fontface
        End Get
        Set(ByVal value As Font)
            _Fontface = value
        End Set
    End Property

    Private _ForeColour As Color = Color.Black
    Public Property ForeColour() As Color
        Get
            Return _ForeColour
        End Get
        Set(ByVal value As Color)
            _ForeColour = value
        End Set
    End Property

    Private _BackColour As Color = Color.White
    Public Property BackColour() As Color
        Get
            Return _BackColour
        End Get
        Set(ByVal value As Color)
            _BackColour = value
        End Set
    End Property

    Private _text As String
    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Private _iSize As SizeF
    Public ReadOnly Property iSize() As SizeF
        Get
            Dim ret As SizeF
            ret.Width = _iSize.Width
            ret.Height = _iSize.Height
            Return ret
        End Get
    End Property

    Private _image As Image
    Public ReadOnly Property Image() As Image
        Get
            Return _image
        End Get
    End Property

    Private _Selected As Boolean = False
    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            If Not _Selected = value Then
                _Selected = value
                Select Case _Selected
                    Case True
                        _image = SlideImages(_text).SelectedImage                        
                        '_image = New Bitmap(_iSize.Width + 20, _iSize.Height + 2)
                        '_ItemImage = Graphics.FromImage(_image)
                        '_ItemImage.FillRectangle(New Drawing.SolidBrush(_ForeColour), 0, 0, _iSize.Width + 20, _iSize.Height + 2)
                        '_ItemImage.DrawLine(New Pen(Color.Black), 0, 0, _iSize.Width + 20, 0)
                        '_ItemImage.DrawLine(New Pen(Color.Black), 0, _iSize.Height + 1, _iSize.Width + 20, _iSize.Height + 1)
                        '_ItemImage.DrawString(Text, _Fontface, New Drawing.SolidBrush(_BackColour), 10, 0)
                        '_ItemImage.DrawImage(_image, 0, 0)
                    Case False
                        _image = SlideImages(_text).UnselectedImage                        
                        '_image = New Bitmap(_iSize.Width + 20, _iSize.Height + 2)
                        '_ItemImage = Graphics.FromImage(_image)
                        '_ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _iSize.Width + 20, _iSize.Height + 2)
                        '_ItemImage.DrawLine(New Pen(Color.Black), 0, 0, _iSize.Width + 20, 0)
                        '_ItemImage.DrawLine(New Pen(Color.Black), 0, _iSize.Height + 1, _iSize.Width + 20, _iSize.Height + 1)
                        '_ItemImage.DrawString(Text, _Fontface, New Drawing.SolidBrush(_ForeColour), 10, 0)
                        '_ItemImage.DrawImage(_image, 0, 0)
                End Select
                _iSize = New SizeF(_image.Width, _image.Height)
            End If
        End Set
    End Property

#End Region

End Class