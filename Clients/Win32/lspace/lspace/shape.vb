Public Enum tRotate
    rot0
    rot90
    rot180
    rot270
End Enum

Public Class Shape
    Public p() As PointF = Nothing
    Private _scale As Double = 0.0
    Private _top As Integer = 0
    Private _left As Integer = 0
    Private _x As Integer = 0
    Private _y As Integer = 0
    Private _a As Integer = 0
    Private _b As Integer = 0
    Private _colour As System.Drawing.Brush
    Private _rotate As tRotate = tRotate.rot0

#Region "Public Properties"
    Public Property x() As Integer
        Get
            Return _x
        End Get
        Set(ByVal value As Integer)
            _x = value
        End Set
    End Property
    Public Property y() As Integer
        Get
            Return _y
        End Get
        Set(ByVal value As Integer)
            _y = value
        End Set
    End Property
    Public Property a() As Integer
        Get
            Return _a
        End Get
        Set(ByVal value As Integer)
            _a = value
        End Set
    End Property
    Public Property b() As Integer
        Get
            Return _b
        End Get
        Set(ByVal value As Integer)
            _b = value
        End Set
    End Property
    Public Property colour() As System.Drawing.Brush
        Get
            Return _colour
        End Get
        Set(ByVal value As System.Drawing.Brush)
            _colour = value
        End Set
    End Property
    Public Property Rotate() As tRotate
        Get
            Return _rotate
        End Get
        Set(ByVal value As tRotate)
            _rotate = value
        End Set
    End Property
    Public Property Top() As Integer
        Get
            Return _top
        End Get
        Set(ByVal value As Integer)
            _top = value
        End Set
    End Property
    Public Property Left() As Integer
        Get
            Return _left
        End Get
        Set(ByVal value As Integer)
            _left = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal a As Integer, ByVal b As Integer)
        _x = x
        _y = y
        _a = a
        _b = b
    End Sub

    Public Sub New(ByVal x As Integer, ByVal y As Integer)
        _x = x
        _y = y
        _a = 0
        _b = 0
    End Sub

    Public Sub draw(ByRef o As Graphics)
        p = Nothing
        With o
            If a = 0 Or b = 0 Then
                Select Case Me.Rotate
                    Case tRotate.rot0, tRotate.rot180
                        AddPoint(p, Me.Left, Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top + Me.y)
                        AddPoint(p, Me.Left, Me.Top + Me.y)
                    Case tRotate.rot270, tRotate.rot90
                        AddPoint(p, Me.Left, Me.Top)
                        AddPoint(p, Me.Left + Me.y, Me.Top)
                        AddPoint(p, Me.Left + Me.y, Me.Top + Me.x)
                        AddPoint(p, Me.Left, Me.Top + Me.x)
                End Select
            Else
                Select Case Me.Rotate
                    Case tRotate.rot0
                        AddPoint(p, Me.Left, Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top + Me.b)
                        AddPoint(p, Me.Left + Me.a, Me.Top + Me.b)
                        AddPoint(p, Me.Left + Me.a, Me.Top + Me.y)
                        AddPoint(p, Me.Left, Me.Top + Me.y)
                    Case tRotate.rot90
                        AddPoint(p, Me.Left, Me.Top)
                        AddPoint(p, Me.Left + Me.y, Me.Top)
                        AddPoint(p, Me.Left + Me.y, Me.Top + Me.x)
                        AddPoint(p, Me.Left + (Me.y - Me.b), Me.Top + Me.x)
                        AddPoint(p, Me.Left + (Me.y - Me.b), Me.Top + Me.a)
                        AddPoint(p, Me.Left, Me.Top + Me.a)
                    Case tRotate.rot180
                        AddPoint(p, Me.Left + (Me.x - Me.a), Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top)
                        AddPoint(p, Me.Left + Me.x, Me.Top + Me.y)
                        AddPoint(p, Me.Left, Me.Top + Me.y)
                        AddPoint(p, Me.Left, Me.Top + (Me.y - Me.b))
                        AddPoint(p, Me.Left + (Me.x - Me.a), Me.Top + (Me.y - Me.b))
                    Case tRotate.rot270
                        AddPoint(p, Me.Left, Me.Top)
                        AddPoint(p, Me.Left + (Me.y - Me.b), Me.Top)
                        AddPoint(p, Me.Left + (Me.y - Me.b), Me.Top + (Me.y - Me.a))
                        AddPoint(p, Me.Left + Me.y, Me.Top + (Me.x - Me.a))
                        AddPoint(p, Me.Left + Me.y, Me.Top + Me.x)
                        AddPoint(p, Me.Left, Me.Top + Me.x)
                End Select

            End If
            .FillPolygon(colour, p)
        End With
    End Sub

    Private Sub AddPoint(ByRef p() As PointF, ByVal x As Integer, ByVal y As Integer)
        Try
            ReDim Preserve p(UBound(p) + 1)
        Catch
            ReDim p(0)
        Finally
            With p(UBound(p))
                .X = x
                .Y = y
            End With
        End Try
    End Sub

#Region "Point in Polygon?"
    ' Return True if the point is in the polygon.
    Public Function PointInPolygon(ByVal X As Single, ByVal Y As Single) As Boolean
        ' Get the angle between the point and the
        ' first and last vertices.
        Dim max_point As Integer = p.Length - 1
        Dim total_angle As Single = GetAngle( _
            p(max_point).X, p(max_point).Y, _
            X, Y, _
            p(0).X, p(0).Y)

        ' Add the angles from the point
        ' to each other pair of vertices.
        For i As Integer = 0 To max_point - 1
            total_angle += GetAngle( _
                p(i).X, p(i).Y, _
                X, Y, _
                p(i + 1).X, p(i + 1).Y)
        Next i

        ' The total angle should be 2 * PI or -2 * PI if
        ' the point is in the polygon and close to zero
        ' if the point is outside the polygon.
        Return Math.Abs(total_angle) > 0.000001
    End Function

    Private Function GetAngle(ByVal Ax As Single, ByVal Ay As Single, ByVal Bx As Single, ByVal By As Single, ByVal Cx As Single, ByVal Cy As Single) As Single
        Dim dot_product As Single
        Dim cross_product As Single

        ' Get the dot product and cross product.
        dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy)
        cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy)

        ' Calculate the angle.
        Return Math.Atan2(cross_product, dot_product)
    End Function

    ' Return the cross product AB x BC.
    ' The cross product is a vector perpendicular to AB
    ' and BC having length |AB| * |BC| * Sin(theta) and
    ' with direction given by the right-hand rule.
    ' For two vectors in the X-Y plane, the result is a
    ' vector with X and Y components 0 so the Z component
    ' gives the vector's length and direction.
    Private Function CrossProductLength( _
        ByVal Ax As Single, ByVal Ay As Single, _
        ByVal Bx As Single, ByVal By As Single, _
        ByVal Cx As Single, ByVal Cy As Single _
      ) As Single
        ' Get the vectors' coordinates.
        Dim BAx As Single = Ax - Bx
        Dim BAy As Single = Ay - By
        Dim BCx As Single = Cx - Bx
        Dim BCy As Single = Cy - By

        ' Calculate the Z coordinate of the cross product.
        Return BAx * BCy - BAy * BCx
    End Function

    ' Return the dot product AB · BC.
    ' Note that AB · BC = |AB| * |BC| * Cos(theta).
    Private Function DotProduct( _
        ByVal Ax As Single, ByVal Ay As Single, _
        ByVal Bx As Single, ByVal By As Single, _
        ByVal Cx As Single, ByVal Cy As Single _
      ) As Single
        ' Get the vectors' coordinates.
        Dim BAx As Single = Ax - Bx
        Dim BAy As Single = Ay - By
        Dim BCx As Single = Cx - Bx
        Dim BCy As Single = Cy - By

        ' Calculate the dot product.
        Return BAx * BCx + BAy * BCy
    End Function
#End Region

End Class