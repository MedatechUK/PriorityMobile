Imports System.Drawing
Imports System.Drawing.Graphics

Public Class SlideMenu

    Public Event MenuClick(ByRef MenuItem As LinkLabel)

    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)
    Public Property MenuFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
            Panel1.Font = MenuFont
        End Set
    End Property

    Dim _MenuItems As New Dictionary(Of Integer, LinkLabel)
    Public Property sMenuItems() As Dictionary(Of Integer, LinkLabel)
        Get
            If IsNothing(_MenuItems) Then
                _MenuItems = New Dictionary(Of Integer, LinkLabel)
            End If
            Return _MenuItems
        End Get
        Set(ByVal value As Dictionary(Of Integer, LinkLabel))
            _MenuItems = value
        End Set
    End Property

    Private Sub SlideMenu_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        With Me
            If .HScrollBar1.Enabled Then
                .Panel1.Left = 0 - .HScrollBar1.Value
            End If

            'Dim x As Integer = 0
            With .Panel1
                For i As Integer = 0 To .Controls.Count - 1
                    With .Controls(i)
                        If String.Compare("System.Windows.Forms.LinkLabel", .GetType.ToString, True) = 0 Then
                            If Not .Width = e.Graphics.MeasureString(.Text, Panel1.Font).Width + 10 Then
                                .Width = e.Graphics.MeasureString(.Text, Panel1.Font).Width + 10
                                '.Left = x
                                'x += .Width + 5
                                'If x > Me.Panel1.Width Then Me.Panel1.Width = x
                            End If
                        End If
                    End With
                Next                
            End With
        End With

    End Sub

    Public Sub form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        With Me
            .Invalidate()
            Application.DoEvents()

            Dim pwidth As Integer = 0
            With Me.Panel1
                For i As Integer = 0 To .Controls.Count - 1
                    With .Controls(i)
                        .Left = pwidth
                        pwidth += .Width + 5
                    End With
                Next
            End With

            Dim vwidth As Integer = .Width - .HScrollBar1.Width

            If pwidth < vwidth Then
                .Panel1.Width = vwidth
                .HScrollBar1.Enabled = False
            Else
                .Panel1.Width = pwidth
                .HScrollBar1.Enabled = True
            End If

            .Panel1.Height = Me.Height
            .HScrollBar1.Height = Me.Height
            .Panel1.Top = 0

            If Not .HScrollBar1.Enabled Then
                .Panel1.Left = 0
                .HScrollBar1.Maximum = 0 '(pwidth - vwidth) + .HScrollBar1.Width
                .HScrollBar1.Value = 0
            Else
                '.Panel1.Left = .HScrollBar1.Value
                .HScrollBar1.Maximum = (pwidth - vwidth) + .HScrollBar1.Width
            End If

        End With
    End Sub

    Public Sub DrawMenu()
        With Me
            .HScrollBar1.Minimum = 0
            .HScrollBar1.SmallChange = 20
            .HScrollBar1.LargeChange = 40
            With .Panel1
                With .Controls
                    .Clear()
                    For i As Integer = 1 To sMenuItems.Count
                        .Add(sMenuItems.Item(i))
                        AddHandler sMenuItems.Item(i).Click, AddressOf hLinkClick
                    Next
                End With
            End With
        End With
    End Sub

    Private Sub hLinkClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RaiseEvent MenuClick(sender)
    End Sub

    Private Sub HScrollBar1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar1.ValueChanged
        With Me
            .Panel1.Left = 0 - .HScrollBar1.Value
        End With
    End Sub

End Class
