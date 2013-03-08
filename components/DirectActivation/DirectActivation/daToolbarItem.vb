﻿
Public Enum daToolbarItemStyle
    Normal = 1
    Space = 2
End Enum

Public Class daToolbarItem

    Private _ItemImage As Graphics
    Private _ButtonGraphic As Image
    Private _GreyButtonGraphic As Image
    Private _spacer As Boolean = False

#Region "Public Properties"

    Private _Handler As System.EventHandler
    Public Property Handler() As System.EventHandler
        Get
            Return _Handler
        End Get
        Set(ByVal value As System.EventHandler)
            _Handler = value
        End Set
    End Property

    Private _ButtonStyle As daToolbarItemStyle
    Public Property ButtonStyle() As daToolbarItemStyle
        Get
            Return _ButtonStyle
        End Get
        Set(ByVal value As daToolbarItemStyle)
            _ButtonStyle = value
        End Set
    End Property

    Private _ForeColour As Color = Color.FromArgb(186, 219, 251)
    Public Property ForeColour() As Color
        Get
            Return _ForeColour
        End Get
        Set(ByVal value As Color)
            _ForeColour = value
        End Set
    End Property

    Private _BackColour As Color = Color.FromArgb(240, 240, 240)
    Public Property BackColour() As Color
        Get
            Return _BackColour
        End Get
        Set(ByVal value As Color)
            _BackColour = value
        End Set
    End Property

    Private _Image As Image
    Public ReadOnly Property ButtonImage() As Image
        Get
            Return _Image
        End Get
    End Property

    Private _Enabled As Boolean = True
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            drawImage()
        End Set
    End Property

#End Region

#Region "initialisation and Finalisation"

    Public Sub New(ByVal Handler As System.EventHandler, _
                   ByVal FileName As String, _
                   ByVal ForeColour As Color, _
                   ByVal BackColour As Color, _
                   ByVal Enabled As Boolean, _
                   ByVal Style As daToolbarItemStyle)

        _ButtonStyle = Style
        _Handler = Handler
        _ForeColour = ForeColour
        _BackColour = BackColour
        _spacer = CBool(Style = daToolbarItemStyle.Space)

        If _spacer Then
            _ButtonGraphic = New Bitmap(8, 22)
            Me.Enabled = True
        Else
            Dim ptrBitmap As Bitmap = New Bitmap(FileName)
            Dim hbmp As IntPtr = ptrBitmap.GetHbitmap

            Dim tmp As Bitmap = Image.FromHbitmap(hbmp)
            With tmp
                For x = 0 To .Width - 1
                    For y = 0 To .Height - 1
                        If .GetPixel(x, y) = Color.Magenta Then
                            .SetPixel(x, y, _ForeColour)
                        End If
                    Next
                Next
            End With
            _ButtonGraphic = tmp

            Dim tmp2 As Bitmap = Image.FromHbitmap(hbmp)
            With tmp2
                For x = 0 To .Width - 1
                    For y = 0 To .Height - 1
                        Dim thisColour As Color = .GetPixel(x, y)
                        Select Case thisColour
                            Case Color.Magenta
                                .SetPixel(x, y, _ForeColour)
                            Case Color.Black
                                .SetPixel(x, y, Color.FromArgb(128, 128, 128))
                            Case Else
                                .SetPixel(x, y, Color.FromArgb(192, 192, 192))
                        End Select
                    Next
                Next
            End With
            _GreyButtonGraphic = tmp2

            hbmp = Nothing
            Me.Enabled = Enabled
        End If

    End Sub

#End Region

#Region "Private Methods"

    Private Sub drawImage()

        Select Case _spacer
            Case True
                _Image = New Bitmap(8, 22)
                _ItemImage = Graphics.FromImage(_Image)
                _ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _Image.Width, _Image.Height)
                _ItemImage.DrawLine(New Drawing.Pen(Color.Gray), 3, 4, 3, _Image.Height - 6)
            Case Else
                _Image = New Bitmap(24, 22)
                _ItemImage = Graphics.FromImage(_Image)
                _ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _Image.Width, _Image.Height)
                _ItemImage.DrawRectangle(New Drawing.Pen(Color.Black), 2, 1, _Image.Width - 5, _Image.Height - 3)
                _ItemImage.FillRectangle(New Drawing.SolidBrush(_ForeColour), 3, 2, _Image.Width - 6, _Image.Height - 4)
                Select Case Me.Enabled
                    Case True
                        _ItemImage.DrawImage(_ButtonGraphic, 4, 3)
                    Case False
                        _ItemImage.DrawImage(_GreyButtonGraphic, 4, 3)
                End Select
        End Select
        _ItemImage.DrawImage(_Image, 0, 0)

        '_Image.Save(Get_app_path() & "\button.bmp", System.Drawing.Imaging.ImageFormat.Bmp)

    End Sub

    Private Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

#End Region

End Class