Public Class SlideMenu

#Region "initialisation and Finalisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SlideMenu_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

    End Sub

#End Region

#Region "Private Variables"

    Private MenuWidth As Integer = 0
    Private _image As Image
    Private _ItemImage As Graphics
    Private xoffset As Integer = 0
    Private startDrag As Point = Nothing
    Private dragging As Boolean = False
    Private ListItems As New List(Of SlideMenuItem)

#End Region

#Region "Public Events"

    Public Event ItemClick(ByVal Button As Integer)

#End Region

#Region "Public Properties"

    Private _FontFace As Font = New Font("Microsoft Sans Serif", 12, FontStyle.Regular)
    Public Overrides Property Font() As Font
        Get
            Return _FontFace
        End Get
        Set(ByVal value As Font)
            _FontFace = value
        End Set
    End Property

    Private _ForeColour As Color = Color.Black
    Public Overrides Property ForeColor() As Color
        Get
            Return _ForeColour
        End Get
        Set(ByVal value As Color)
            _ForeColour = value
        End Set
    End Property

    Private _BackColour As Color = Color.White
    Public Overrides Property BackColor() As Color
        Get
            Return _BackColour
        End Get
        Set(ByVal value As Color)
            _BackColour = value
        End Set
    End Property

    Public ReadOnly Property Internalheight() As Integer
        Get
            Return _image.Height + 4
        End Get
    End Property

    Public Property Selected(ByVal n As Integer) As Boolean
        Get
            Return ListItems(n).Selected
        End Get
        Set(ByVal value As Boolean)
            For i As Integer = 0 To ListItems.Count - 1
                ListItems(i).Selected = CBool(i = n)
            Next
            MakeImage()
        End Set
    End Property

#End Region

#Region "public Methods"

    Public Sub Add(ByVal Text As String)
        Dim r As Integer = MenuWidth
        With ListItems
            .Add(New SlideMenuItem(Text, _FontFace, _ForeColour, _BackColour))
            MenuWidth += .Item(.Count - 1).iSize.Width
            MakeImage()
        End With
    End Sub

    Public Sub Clear()
        ListItems.Clear()
        MenuWidth = 0
    End Sub

#End Region

#Region "Private Methods"

    Private Sub MakeImage()
        With ListItems
            If MenuWidth < Me.Width Then
                _image = New Bitmap(Me.Width, .Item(.Count - 1).iSize.Height)
            Else
                _image = New Bitmap(MenuWidth, .Item(.Count - 1).iSize.Height)
            End If

            _ItemImage = Graphics.FromImage(_image)
            _ItemImage.FillRectangle(New Drawing.SolidBrush(_BackColour), 0, 0, _image.Width, _image.Height)

            Dim l As Integer = 0
            For Each m As SlideMenuItem In ListItems
                _ItemImage.DrawImage(m.Image, l, 0)
                l += m.iSize.Width
            Next
            _ItemImage.DrawImage(_image, 0, 0)
        End With

        '_image.Save(Get_app_path() & "\MENU.bmp", Drawing.Imaging.ImageFormat.Bmp)

        With Me.PictureBox
            .Top = 0
            .Image = _image
            .Width = _image.Width
            .Height = _image.Height
        End With

    End Sub

    Private Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

#End Region

#Region "Mouse Drag Events"

    Private Sub PictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox.MouseDown
        startDrag.X = e.X
    End Sub

    Private Sub PictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox.MouseMove
        If Not IsNothing(startDrag) Then
            If e.X <> startDrag.X Then
                xoffset = xoffset + (e.X - startDrag.X)
                If xoffset < -1 * (_image.Width - Me.Width) Then xoffset = -1 * (_image.Width - Me.Width)
                If xoffset > 0 Then xoffset = 0
                Me.PictureBox.Left = xoffset
                dragging = True
            End If
        End If
    End Sub

    Private Sub PictureBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox.MouseUp
        If dragging Then
            xoffset = xoffset + (e.X - startDrag.X)
            If xoffset < -1 * (_image.Width - Me.Width) Then xoffset = -1 * (_image.Width - Me.Width)
            If xoffset > 0 Then xoffset = 0
            Me.PictureBox.Left = xoffset
        Else
            Dim l As Integer = 0
            Dim btn As Integer = 0

            For Each m As SlideMenuItem In ListItems
                Dim r As Integer = (l + m.iSize.Width)
                If e.X > l And e.X < r Then
                    RaiseEvent ItemClick(btn)
                    Exit For
                End If
                l += m.iSize.Width
                btn += 1
            Next
        End If
        startDrag = Nothing
        dragging = False
    End Sub

#End Region

#Region "Paint and resize"

    Private Sub SlideMenu_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Me.PictureBox.Image = _image

    End Sub

    Private Sub SlideMenu_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me.PictureBox
            .Left = 0
            .Top = 0
            .Height = Me.Height
            If Not IsNothing(_image) Then
                .Width = _image.Width
            Else
                .Width = Me.Width
            End If
        End With
    End Sub

#End Region

End Class
