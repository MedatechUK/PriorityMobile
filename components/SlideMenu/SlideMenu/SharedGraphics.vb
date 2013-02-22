Module SharedGraphics

    Private p As New PictureBox
    Private g As Graphics

    Public Function MeasureString(ByVal text As String, ByVal fontface As Font) As SizeF
        Try
            Return g.MeasureString(text, fontface)
        Catch ex As Exception
            g = p.CreateGraphics
            Return g.MeasureString(text, fontface)
        End Try

    End Function

    Public DAImages As New Dictionary(Of String, DAImage)
    Public SlideImages As New Dictionary(Of String, SlideMenuImage)

End Module

Public Class DAImage
    Public Sub New(ByVal ButtonGraphic As Image, ByVal GreyButtonGraphic As Image)
        _ButtonGraphic = ButtonGraphic
        _GreyButtonGraphic = GreyButtonGraphic
    End Sub
    Private _ButtonGraphic As Image
    Public Property ButtonGraphic() As Image
        Get
            Return _ButtonGraphic
        End Get
        Set(ByVal value As Image)
            _ButtonGraphic = value
        End Set
    End Property
    Private _GreyButtonGraphic As Image
    Public Property GreyButtonGraphic() As Image
        Get
            Return _GreyButtonGraphic
        End Get
        Set(ByVal value As Image)
            _GreyButtonGraphic = value
        End Set
    End Property
End Class

Public Class SlideMenuImage
    Private _SelectedImage As Image
    Public Property SelectedImage() As Image
        Get
            Return _SelectedImage
        End Get
        Set(ByVal value As Image)
            _SelectedImage = value
        End Set
    End Property
    Private _UnselectedImage As Image
    Public Property UnselectedImage() As Image
        Get
            Return _UnselectedImage
        End Get
        Set(ByVal value As Image)
            _UnselectedImage = value
        End Set
    End Property
End Class