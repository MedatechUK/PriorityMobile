Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing

Namespace PL.TabControl
    Public Delegate Function OnBeforeCloseTab(ByVal indx As Integer) As Boolean

    Class ClosableTab
        Inherits System.Windows.Forms.TabControl
        'Public Event BeforeCloseATabPage As OnBeforeCloseTab

        Private _img As Image
        Private _imageLocation As New Point(15, 5)
        Private _imgHitArea As New Point(13, 2)

        Private _imgExist As Boolean = False
        Private _tabWidth As Integer = 0

        Public Sub New()
            MyBase.New()
            'BeforeCloseATabPage = Nothing
            Me.DrawMode = TabDrawMode.OwnerDrawFixed
            Me.Padding = New Point(12, 3)
        End Sub

        Public Property SetImage() As Image
            Get
                Return _img
            End Get
            Set(ByVal value As Image)
                _img = value
                If _img IsNot Nothing Then
                    _imgExist = True
                End If
            End Set
        End Property

        Public Property ImageLocation() As Point
            Get
                Return _imageLocation
            End Get
            Set(ByVal value As Point)
                _imageLocation = value
            End Set
        End Property

        Public Property ImageHitArea() As Point
            Get
                Return _imgHitArea
            End Get
            Set(ByVal value As Point)
                _imgHitArea = value
            End Set
        End Property

        Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
            Dim r As Rectangle = e.Bounds
            r = GetTabRect(e.Index)
            r.Offset(2, 2)

            Dim TitleBrush As Brush = New SolidBrush(Color.Black)
            Dim f As Font = Me.Font

            Dim title As String = Me.TabPages(e.Index).Text

            e.Graphics.DrawString(title, f, TitleBrush, New PointF(r.X, r.Y))

            Try
                e.Graphics.DrawImage(_img, New Point(r.X + (GetTabRect(e.Index).Width - _imageLocation.X), _imageLocation.Y))
            Catch generatedExceptionName As Exception
            End Try
        End Sub

        Protected Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)
            If _imgExist = True Then
                Dim p As Point = e.Location
                For i As Integer = 0 To TabCount - 1
                    _tabWidth = GetTabRect(i).Width - (_imgHitArea.X)

                    Dim r As Rectangle = GetTabRect(i)
                    r.Offset(_tabWidth, _imgHitArea.Y)
                    r.Width = _img.Width
                    r.Height = _img.Height
                    If r.Contains(p) Then
                        CloseTab(i)
                    End If
                Next
            End If
        End Sub

        Private Sub CloseTab(ByVal i As Integer)
            'If BeforeCloseATabPage IsNot Nothing Then
            '    Dim CanClose As Boolean = BeforeCloseATabPage(i)
            '    If Not CanClose Then
            '        Return
            '    End If
            'End If
            TabPages.Remove(TabPages(i))
        End Sub
    End Class
End Namespace
