Imports System.Drawing
Imports System.Drawing.Graphics

Public Class SlideMenu

    Private g As System.Drawing.Graphics = Me.CreateGraphics
    Private _Font As New Font("Tahoma", 10, FontStyle.Regular)

    Public Event MenuClick(ByRef MenuItem As String)

    Public Property MenuFont() As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value            
        End Set
    End Property

    Public Overrides Property BackColor() As System.Drawing.Color
        Get
            Return Me.Panel1.BackColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            Panel1.BackColor = value
            'Me.BackColor = value
        End Set
    End Property

    Sub Add(ByVal StrVal As String)

        Dim L As New LinkLabel

        With L
            .Text = StrVal
            .Font = _Font
            .Width = g.MeasureString(StrVal, .Font).Width + 10
            .BackColor = Me.BackColor
            AddHandler .Click, AddressOf hLinkClick
        End With

        With Me
            .HScrollBar1.Minimum = 1
            With .Panel1
                With .Controls
                    .Add(L)
                    If .Count = 1 Then
                        .Item(.Count - 1).Left = 0
                    Else
                        .Item(.Count - 1).Left = .Item(.Count - 2).Left + .Item(.Count - 2).Width + 10
                    End If
                    Me.Panel1.Width = .Item(.Count - 1).Left + .Item(.Count - 1).Width + 10
                    Me.HScrollBar1.Enabled = (.Item(.Count - 1).Left + .Item(.Count - 1).Width) > Me.Width - Me.HScrollBar1.Width
                    Me.HScrollBar1.Maximum = .Count
                End With
            End With
        End With

    End Sub

    Function Count() As Integer
        Return Panel1.Controls.Count
    End Function

    Sub Clear()
        Panel1.Controls.Clear()
    End Sub

    Function Item(ByVal i As Integer) As String
        Return Panel1.Controls.Item(i - 1).Text
    End Function

    Public Sub form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        With Me.Panel1
            With .Controls
                If .Count > 0 Then
                    Me.HScrollBar1.Enabled = (.Item(.Count - 1).Left + .Item(.Count - 1).Width) > Me.Width - Me.HScrollBar1.Width
                End If
            End With
            If Not Me.HScrollBar1.Enabled Then
                .Left = 0
            End If
        End With
    End Sub

    Private Sub hLinkClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim l As LinkLabel = sender
        RaiseEvent MenuClick(l.Text)
    End Sub

    Private Sub HScrollBar1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HScrollBar1.ValueChanged        
        With Me
            If .HScrollBar1.Value = 1 Then
                .Panel1.Left = 0
            Else
                With .Panel1
                    .Left = 0 - .Controls.Item(Me.HScrollBar1.Value - 1).Left
                End With
            End If
        End With
    End Sub

End Class
