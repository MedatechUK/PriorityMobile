Imports Microsoft.VisualBasic.Strings
Public Class priSign
    ' Signature variables
    Public coord(1, 0) As Integer

    Public Sub Reset()
        ' Reset the signature co-ordinates
        ReDim coord(1, 0)
        coord(0, 0) = 0
        coord(1, 0) = 0
    End Sub

    Public Overloads Sub dispose()
        Me.Sign.Dispose()
    End Sub

    Public Function CompressSignature() As String

        Dim bstr As String = ""
        Dim tarray(UBound(coord, 1), -1) As String
        Dim y As Integer
        Dim add As Boolean

        For y = 0 To UBound(coord, 2)
            If y > 0 Then
                If coord(0, y - 1) = coord(0, y) And coord(1, y - 1) = coord(1, y) Then
                    'same as last
                    add = False
                Else
                    add = True
                End If
            Else
                add = True
            End If
            If add Then
                bstr = bstr & CStr(coord(0, y)) & "," & CStr(coord(1, y)) & "."
            End If
        Next

        Return bstr

    End Function

    Public Function UnpackSignature(ByRef SigData As String)

        Do Until Not Strings.Right(SigData, 1) = "."
            SigData = Strings.Left(SigData, Len(SigData) - 1)
        Loop

        Dim l() As String = Split(SigData, ".")
        Dim tarray(1, UBound(l)) As Integer

        If UBound(l) > 0 Then
            For y As Integer = 0 To UBound(l)
                tarray(0, y) = CInt(Split(l(y), ",")(0))
                tarray(1, y) = CInt(Split(l(y), ",")(1))
            Next
        End If

        Return tarray

    End Function

    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Sign.MouseDown

        ReDim Preserve coord(1, UBound(coord, 2) + 1)
        coord(0, UBound(coord, 2)) = e.X
        coord(1, UBound(coord, 2)) = e.Y

    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Sign.MouseUp

        ReDim Preserve coord(1, UBound(coord, 2) + 1)
        coord(0, UBound(coord, 2)) = e.X
        coord(1, UBound(coord, 2)) = e.Y

        ReDim Preserve coord(1, UBound(coord, 2) + 1)
        coord(0, UBound(coord, 2)) = 0
        coord(1, UBound(coord, 2)) = 0

        Sign.Invalidate()

    End Sub

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Sign.MouseMove

        Static refr As Integer

        Try
            ReDim Preserve coord(1, UBound(coord, 2) + 1)
            coord(0, UBound(coord, 2)) = e.X
            coord(1, UBound(coord, 2)) = e.Y
        Catch

        End Try

        If refr >= 2 Then
            refr = 0
            Sign.Invalidate()
        Else
            refr = refr + 1
        End If

    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Sign.Paint

        Try ' error trapping

            Dim myPen As System.Drawing.Pen = New System.Drawing.Pen(System.Drawing.Color.Black)
            Dim i As Integer
            Dim x1, y1, x2, y2 As Integer

            For i = 1 To UBound(coord, 2)

                If UBound(coord, 2) > 1 Then

                    x1 = coord(0, i - 1)
                    y1 = coord(1, i - 1)
                    x2 = coord(0, i)
                    y2 = coord(1, i)

                    If Not (x1 = 0 And y1 = 0) And Not (x2 = 0 And y2 = 0) Then
                        e.Graphics.DrawLine(myPen, _
                            x1, _
                            y1, _
                            x2, _
                            y2)
                    End If
                End If
            Next

        Catch
            MsgBox("Not happening!")
        End Try

    End Sub

    Public Sub New()
        InitializeComponent()
        Sign.BackColor = Color.White
    End Sub

End Class
