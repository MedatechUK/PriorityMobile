Imports System.IO

Public Class daToolbar

#Region "private Variables"

    Private _image As Image
    Private _ItemImage As Graphics

#End Region

#Region "Initialisation and Finalisation"

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

#End Region

#Region "Public Properties"

    Private _ForeColour As Color = Color.FromArgb(186, 219, 251)
    Public Overrides Property ForeColor() As Color
        Get
            Return _ForeColour
        End Get
        Set(ByVal value As Color)
            _ForeColour = value
        End Set
    End Property

    Private _BackColour As Color = Color.FromArgb(240, 240, 240)
    Public Overrides Property BackColor() As Color
        Get
            Return _BackColour
        End Get
        Set(ByVal value As Color)
            _BackColour = value
        End Set
    End Property

    Private _IconFolder As String = ""
    Public Property IconFolder() As String
        Get
            Return _IconFolder
        End Get
        Set(ByVal value As String)
            _IconFolder = value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Public Sub MakeImage()
        With _Items
            _image = New Bitmap(Me.Width, Me.Height)

            _ItemImage = Graphics.FromImage(_image)
            _ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _image.Width, _image.Height)

            Dim l As Integer = 5
            For Each da As daToolbarItem In _Items
                _ItemImage.DrawImage(da.ButtonImage, l, 0)
                l += da.ButtonImage.Width
            Next
            _ItemImage.DrawImage(_image, 0, 0)
        End With

        With Me.PictureBox
            .Top = 0
            .Image = _image
        End With
    End Sub

#End Region

#Region "Public Methods"

    Private _Items As New List(Of daToolbarItem)

    Public Overloads Sub Add(ByVal Handler As System.EventHandler, ByVal bmpFileName As String, Optional ByVal Enabled As Boolean = True, Optional ByVal Style As daToolbarItemStyle = daToolbarItemStyle.Normal)
        _Items.Add(New daToolbarItem(Handler, _IconFolder & "\" & bmpFileName, _ForeColour, _BackColour, Enabled, Style, Me.Height))
    End Sub

    Public Overloads Sub Add(Optional ByVal Style As daToolbarItemStyle = daToolbarItemStyle.Space)
        _Items.Add(New daToolbarItem(Nothing, "", _ForeColour, _BackColour, False, Style, Me.Height))
    End Sub

    Public Sub Clear()
        _Items.Clear()        
    End Sub

#End Region

#Region "Mouse Events"

    Private Sub PictureBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox.MouseUp

        Dim l As Integer = 5
        For Each da As daToolbarItem In _Items

            If e.X > l And e.X < l + da.ButtonImage.Width Then
                If da.ButtonStyle = daToolbarItemStyle.Normal And da.Enabled Then
                    da.Handler.Invoke(Me, New System.EventArgs)
                    Exit For
                Else
                    Beep()
                    Exit For
                End If
            End If

            l += da.ButtonImage.Width
        Next
    End Sub

    Private Sub daToolbar_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        MakeImage()
    End Sub

#End Region

End Class
