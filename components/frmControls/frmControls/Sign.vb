Public Class Sign

#Region "Private Properties"

    Private myPen As System.Drawing.Pen = New System.Drawing.Pen(System.Drawing.Color.Black)
    Private coord As New List(Of Point)

#End Region

#Region "Public Properties"

    Private _Enabled As Boolean = True
    Public Overloads Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property

#End Region

#Region "Local Control Handlers"

    Private Sub Signature_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseDown
        If _Enabled Then coord.Add(New Point(e.X, e.Y))
    End Sub

    Private Sub Signature_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseMove
        If _Enabled Then
            coord.Add(New Point(e.X, e.Y))
            If coord.Count Mod 3 = 1 Then Signature.Invalidate()
        End If
    End Sub

    Private Sub Signature_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseUp
        coord.Add(New Point(0, 0))
        If coord.Count Mod 2 = 1 Then Signature.Invalidate()
    End Sub

    Private Sub Signature_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Signature.Paint
        For i As Integer = 1 To coord.Count - 1
            If Not (coord(i - 1).X = 0 And coord(i - 1).Y = 0) And Not (coord(i).X = 0 And coord(i).Y = 0) Then
                e.Graphics.DrawLine(myPen, _
                    coord(i - 1).X, _
                    coord(i - 1).Y, _
                    coord(i).X, _
                    coord(i).Y)
            End If
        Next
    End Sub

#End Region

#Region "Public Methods"

    Public Sub DrawSignature(ByVal Serialdata As String)
        With Me
            .coord.Clear()
            Dim points() As String = Split(SerialData, "\n")
            For Each p As String In points
                If p.Length > 0 Then
                    coord.Add(New Point(CInt(Split(p, "\t")(0)), CInt(Split(p, "\t")(1))))
                End If
            Next
            .Signature.Invalidate()
        End With
    End Sub

    Public Sub Clear()
        coord.Clear()
        Signature.Invalidate()
    End Sub

    Public Function toSerial() As String
        Dim ret As String = ""
        For Each p As Point In coord
            ret += String.Format("{0}\t{1}\n", p.X, p.Y)
        Next
        Return ret
    End Function

#End Region

End Class
