Imports System.Drawing
Imports System.Drawing.Imaging

Public Class Form1
    Dim Material As Shape
    Dim Template As New Shape(50, 100)
    Dim b As Bitmap
    Dim objGraphics As Graphics
    Dim cursorPoint As Point
    Dim MD As Boolean = False
    Dim rotate As Shape
    Dim myShapes As New Dictionary(Of String, Shape)

#Region "Initialisation and finalisation"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        With myShapes
            .Add("Sheet1", New Shape(300, 300, 250, 150))
            .Add("Sheet2", New Shape(200, 300, 250, 150))
            .Add("Sheet3", New Shape(300, 300, 50, 150))
        End With

        For Each k As String In myShapes.Keys
            Me.lstShapes.Items.Add(k)
        Next

        With Template
            .Top = 100
            .Left = 100
            .colour = Brushes.Gray
        End With

    End Sub

    Private Sub Form1_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        b.Dispose()
        objGraphics.Dispose()
    End Sub

#End Region

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        b = New System.Drawing.Bitmap(IMG.Width, IMG.Height, PixelFormat.Format24bppRgb)
        objGraphics = Graphics.FromImage(b)
        objGraphics.Clear(Color.Blue)

        If Not IsNothing(Material) Then
            With Material
                .draw(objGraphics)
            End With
        End If

        If Not IsNothing(Template) Then
            Dim inside As Boolean = True
            With Template
                If Not IsNothing(.p) Then
                    If Not IsNothing(Material) Then
                        For Each pf As PointF In .p
                            If Not Material.PointInPolygon(pf.X, pf.Y) Then
                                inside = False
                                Exit For
                            End If
                        Next
                    End If
                End If
                If inside Then
                    .colour = Brushes.Green
                Else
                    .colour = Brushes.Gray
                End If
                .draw(objGraphics)
            End With
        End If

        IMG.Image = b
    End Sub

#Region "Mouse Handlers"

    Private Sub IMG_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles IMG.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                Dim p As New System.Drawing.Point(e.Location.X, e.Location.Y)
                If Template.PointInPolygon(e.Location.X, e.Location.Y) Then
                    rotate = Template
                    RotateMenu.Show(IMG, p)
                Else
                    If Material.PointInPolygon(e.Location.X, e.Location.Y) Then
                        rotate = Material
                        RotateMenu.Show(IMG, p)
                    End If
                End If

            Case Windows.Forms.MouseButtons.Left
                If Template.PointInPolygon(e.Location.X, e.Location.Y) Then
                    MD = True
                    Template.colour = Brushes.AntiqueWhite
                    With cursorPoint
                        .X = e.Location.X
                        .Y = e.Location.Y
                    End With
                    Me.Invalidate()
                End If
        End Select
    End Sub

    Private Sub IMG_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles IMG.MouseMove
        If MD Then
            With Template
                .Left += e.Location.X - cursorPoint.X
                .Top += e.Location.Y - cursorPoint.Y
            End With
            With cursorPoint
                .X = e.Location.X
                .Y = e.Location.Y
            End With
            Me.Invalidate()
        End If
    End Sub

    Private Sub IMG_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles IMG.MouseUp
        MD = False
        Template.colour = Brushes.Gray
        Me.Invalidate()
    End Sub

#End Region

#Region "keyboard Handlers"
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown, lstShapes.KeyDown
        With Template
            Select Case e.KeyData
                Case Keys.Up
                    .Top -= 1
                Case Keys.Down
                    .Top += 1
                Case Keys.Left
                    .Left -= 1
                Case Keys.Right
                    .Left += 1
            End Select
        End With
        e.SuppressKeyPress = True
        Me.Invalidate()
    End Sub
#End Region

#Region "Rotate Context Menus"
    Private Sub RotateLeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateLeftToolStripMenuItem.Click
        With rotate
            Select Case .Rotate
                Case tRotate.rot0
                    .Rotate = tRotate.rot270
                Case tRotate.rot90
                    .Rotate = tRotate.rot0
                Case tRotate.rot180
                    .Rotate = tRotate.rot90
                Case tRotate.rot270
                    .Rotate = tRotate.rot180
            End Select
        End With
        Me.Invalidate()
    End Sub

    Private Sub RotateRightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RotateRightToolStripMenuItem.Click
        With rotate
            Select Case .Rotate
                Case tRotate.rot0
                    .Rotate = tRotate.rot90
                Case tRotate.rot90
                    .Rotate = tRotate.rot180
                Case tRotate.rot180
                    .Rotate = tRotate.rot270
                Case tRotate.rot270
                    .Rotate = tRotate.rot0
            End Select
        End With
        Me.Invalidate()
    End Sub
#End Region

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        Me.Invalidate()
    End Sub

    Private Sub lstShapes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstShapes.SelectedIndexChanged
        Material = myShapes(lstShapes.Items(lstShapes.SelectedIndex))
        With Material
            .colour = Brushes.Red
            .Top = 20
            .Left = 20
            .Rotate = tRotate.rot0
        End With
        With Me
            .Invalidate()
            .Focus()
        End With
    End Sub

End Class
